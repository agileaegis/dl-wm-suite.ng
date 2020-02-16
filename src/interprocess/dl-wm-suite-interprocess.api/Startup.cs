using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using dl.wm.suite.interprocess.api.Configurations;
using dl.wm.suite.interprocess.api.Mqtt;
using dl.wm.suite.interprocess.api.UDPs;
using dl.wm.suite.interprocess.api.WSs;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;

namespace dl.wm.suite.interprocess.api
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowSpecificOrigins";

        public Startup(IConfiguration configuration, IHostingEnvironment hostEnv)
        {
            Configuration = configuration;
            HostEnv = hostEnv;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostEnv { get; }

        public IContainer ApplicationContainer { get; private set; }

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
            .MinimumLevel.Override("dl-wm-suite-interprocess.api.api", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Assembly", $"{name.Name}")
            .Enrich.WithProperty("Revision", $"{name.Version}")
            .WriteTo.Debug(
              outputTemplate:
              "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{HttpContext} {NewLine}{Exception}")
            .WriteTo.RollingFile(HostEnv.WebRootPath + @"log-{Date}.txt",
              Serilog.Events.LogEventLevel.Information, retainedFileCountLimit: 7)
            .CreateLogger();

          services.AddLogging(loggingBuilder =>
            loggingBuilder
              .AddSerilog(dispose: true));

          services.AddLogging(loggingBuilder =>
            loggingBuilder
              .AddSerilog(dispose: true));

          services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

          services.AddSignalR();

          services.AddSwaggerGen(options =>
          {
            options.DescribeAllEnumsAsStrings();
            options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
            {
              Title = "dl-wm-suite-interprocess.api - HTTP API",
              Version = "v1",
              Description = "The Catalog Microservice HTTP API for dl-wm-suite-interprocess.api service.",
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

          Config.ConfigureRedis(services);

          services.AddSingleton<IUdpConfiguration, UdpConfiguration>();
          services.AddSingleton<IWsConfiguration, WsConfiguration>();
          services.AddSingleton<IRabbitMqttConfiguration, RabbitMqttConfiguration>();
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

          var serviceProvider = app.ApplicationServices;

          var serviceUdp = (IUdpConfiguration) serviceProvider.GetService(typeof(IUdpConfiguration));
          var serviceWs = (IWsConfiguration) serviceProvider.GetService(typeof(IWsConfiguration));
          var serviceMqtt = (IRabbitMqttConfiguration) serviceProvider.GetService(typeof(IRabbitMqttConfiguration));

          serviceUdp.EstablishConnection();

          serviceWs.EstablishConnection();

          serviceMqtt.EstablishConnection();

          app.UseSwagger().UseSwaggerUI(c =>
          {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "dl-wm-suite-interprocess.api V1");
          });

          app.UseCors(CorsPolicyName);
          app.UseHttpsRedirection();
          app.UseMvc();
        }
    }
}
