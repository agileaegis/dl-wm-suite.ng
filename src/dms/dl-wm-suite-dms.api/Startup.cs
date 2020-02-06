using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNetCoreRateLimit;
using Autofac;
using FluentValidation.AspNetCore;
using GreenPipes;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;

namespace aegis.wm.suite.dms.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostEnv)
        {
            Configuration = configuration;
            HostEnv = hostEnv;
        }

        private const string CorsPolicyName = "AllowSpecificOrigins";

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostEnv { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    builderCors => { builderCors.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); });
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var name = Assembly.GetExecutingAssembly().GetName();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("aegis.wm.suite.dms.api", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{name.Name}")
                .Enrich.WithProperty("Revision", $"{name.Version}")
                .WriteTo.Debug(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{HttpContext} {NewLine}{Exception}")
                .WriteTo.RollingFile(HostEnv.WebRootPath + @"log-{Date}.txt", Serilog.Events.LogEventLevel.Information,
                    retainedFileCountLimit: 7)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder
                    .AddSerilog(dispose: true));

            services
                .AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    options.ReturnHttpNotAcceptable = true;

                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));

                    var jsonInputFormatter = options.InputFormatters
                        .OfType<JsonInputFormatter>().FirstOrDefault();

                    if (jsonInputFormatter != null)
                    {
                        jsonInputFormatter.SupportedMediaTypes
                            .Add("application/vnd.marvin.author.full+json");
                        jsonInputFormatter.SupportedMediaTypes
                            .Add("application/vnd.marvin.authorwithdateofdeath.full+json");
                    }

                    var jsonOutputFormatter = options.OutputFormatters
                        .OfType<JsonOutputFormatter>().FirstOrDefault();

                    jsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.marvin.hateoas+json");

                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddFluentValidation();

            services.AddResponseCaching();

            services.AddHttpCacheHeaders(
                (expirationModelOptions)
                    =>
                {
                    expirationModelOptions.MaxAge = 60;
                    expirationModelOptions.SharedMaxAge = 30;
                },
                (validationModelOptions)
                    =>
                {
                    validationModelOptions.MustRevalidate = true;
                    validationModelOptions.ProxyRevalidate = true;
                });

            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>((options) =>
            {
                options.GeneralRules = new System.Collections.Generic.List<RateLimitRule>()
                {
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 1000,
                        Period = "5m"
                    },
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 200,
                        Period = "10s"
                    }
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"));

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "aegis.wm.suite.dms.api - HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API for aegis.wm.suite.dms.api service.",
                    TermsOfService = ""
                });
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description =
                        "Authorization: Bearer {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }}
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builderCors => builderCors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "aegis.wm.suite.dms.api");
            });

            app.UseCors(CorsPolicyName);
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseApiVersioning();

            var bus = ApplicationContainer.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }
    }
}
