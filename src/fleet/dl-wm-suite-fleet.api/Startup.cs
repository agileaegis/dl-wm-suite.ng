using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using dl.wm.suite.fleet.api.Configurations;
using dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles;
using dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Assets;
using dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trackables;
using dl.wm.suite.fleet.api.Configurations.AutoMappingProfiles.Trips;
using dl.wm.suite.fleet.api.Schedulers;
using dl.wm.suite.fleet.api.Schedulers.Executors;
using dl.wm.suite.fleet.api.Schedulers.PointStoring;
using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using dl.wm.suite.fleet.api.Mqtt;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;

namespace dl.wm.suite.fleet.api
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
        .MinimumLevel.Override("dl.wm.suite.fleet.api", LogEventLevel.Information)
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

      var key = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value);
      services.AddAuthentication(x =>
        {
          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
          x.RequireHttpsMetadata = false;
          x.SaveToken = true;
          x.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
            ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
          };
        });

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

      services.AddSingleton<IJobFactory, JobFactory>();
      services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
      services.AddHostedService<QuartzHostedService>();

      services.AddSingleton<RedisPointStoringInitializerJob>();
      services.AddSingleton(new JobSchedule(
        jobType: typeof(RedisPointStoringInitializerJob),
        cronExpression: "0/50 * * * * ?")); // run every 15 seconds

      Config.ConfigureAutoMapper(services);
      Config.ConfigureRepositories(services);
      Config.ConfigureNHibernate(services, Configuration.GetConnectionString("MysqlSqlDatabase"));

      services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"));

      services.AddSwaggerGen(options =>
      {
        options.DescribeAllEnumsAsStrings();
        options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
        {
          Title = "dl.wm.suite.fleet.api - HTTP API",
          Version = "v1",
          Description = "The Catalog Microservice HTTP API for dl.wm.suite.fleet.api service.",
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
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
      });

      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
          builderCors => builderCors.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
      });

      services.AddSingleton<IDomainExecutor, DomainExecutor>();
      services.AddSingleton<IRabbitMqttConfiguration, RabbitMqttConfiguration>();


      services.AddSingleton<ConnectionMultiplexer>(sp =>
      {
        ConfigurationOptions options = new ConfigurationOptions
        {
          EndPoints = {{Configuration.GetSection("Redis:Api").Value, 6379}},
          AllowAdmin = true,
          ConnectTimeout = 60 * 1000,
          ResolveDns = true,
          AbortOnConnectFail = false,
          Password = Configuration.GetSection("Redis:Password").Value
        };

        return ConnectionMultiplexer.Connect(options);
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

      app.UseSwagger().UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "dl.wm.suite.fleet.api"); });

      app.UseCors(CorsPolicyName);
      app.UseResponseCaching();
      app.UseHttpCacheHeaders();
      app.UseCookiePolicy();
      app.UseAuthentication();
      app.UseHttpsRedirection();
      app.UseMvc();

      app.UseApiVersioning();

      var serviceProvider = app.ApplicationServices;
      var serviceExecutor = (IDomainExecutor) serviceProvider.GetService(typeof(IDomainExecutor));
      var serviceMqtt = (IRabbitMqttConfiguration) serviceProvider.GetService(typeof(IRabbitMqttConfiguration));
      
      serviceExecutor.InitExecutor();
      serviceMqtt.EstablishConnection();

      AutoMapper.Mapper.Initialize(cfg =>
      {
        cfg.AddProfile<AssetEntityToAssetUiAutoMapperProfile>();
        cfg.AddProfile<AssetEntityToAssetForCreationUiAutoMapperProfile>();
        cfg.AddProfile<AssetForCreationUiModelToAssetEntityAutoMapperProfile>();
        cfg.AddProfile<AssetUiModelToAssetEntityAutoMapperProfile>();
        cfg.AddProfile<TripEntityToTripUiAutoMapperProfile>();
        cfg.AddProfile<TripEntityToTripForCreationUiAutoMapperProfile>();
        cfg.AddProfile<TripForCreationUiModelToTripEntityAutoMapperProfile>();
        cfg.AddProfile<TripUiModelToTripEntityAutoMapperProfile>();
        cfg.AddProfile<TrackableEntityToTrackableUiAutoMapperProfile>();
        cfg.AddProfile<TrackableEntityToTrackableForCreationUiAutoMapperProfile>();
        cfg.AddProfile<TrackableForCreationUiModelToTrackableEntityAutoMapperProfile>();
        cfg.AddProfile<TrackableUiModelToTrackableEntityAutoMapperProfile>();
      });
    }
  }
}
