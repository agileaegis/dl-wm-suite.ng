using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.api.Configurations;
using dl.wm.suite.cms.api.Configurations.AutoMappingProfiles;
using dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Containers;
using dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Employess;
using dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Tours;
using dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Users;
using dl.wm.suite.cms.api.Configurations.AutoMappingProfiles.Vehicles;
using AspNetCoreRateLimit;
using Autofac;
using dl.wm.suite.cms.api.Mqtt;
using dl.wm.suite.cms.repository.CustomTypes;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;

namespace dl.wm.suite.cms.api
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
        .MinimumLevel.Override("dl.wm.suite.cms.api", LogEventLevel.Information)
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

      NpgsqlConnection.GlobalTypeMapper.UseJsonNet(new[] { typeof(JsonbType) });

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

      Config.ConfigureRepositories(services);
      Config.ConfigureAutoMapper(services);
      Config.ConfigureNHibernate(services, Configuration.GetConnectionString("PostgreSqlDatabase"));

      services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"));

      services.AddSwaggerGen(options =>
      {
        options.DescribeAllEnumsAsStrings();
        options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
        {
          Title = "dl.wm.suite.cms.api - CMS HTTP API",
          Version = "v1",
          Description = "The Catalog Microservice HTTP API for dl.wm.suite.cms.api service.",
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

      services.AddSingleton<ConnectionMultiplexer>(sp =>
      {
        ConfigurationOptions options = new ConfigurationOptions
        {
          EndPoints = {{"137.116.232.108", 6379}},
          AllowAdmin = true,
          ConnectTimeout = 60 * 1000,
          ResolveDns = true,
          AbortOnConnectFail = false,
          Password = "1234567890q!"
        };

        return ConnectionMultiplexer.Connect(options);
      });

      services.AddSingleton<IRabbitMqttConfiguration, RabbitMqttConfiguration>();

      services.AddHealthChecks()
        .AddNpgSql(Configuration.GetConnectionString("PostgreSqlDatabase"), failureStatus: HealthStatus.Unhealthy,
          name: "PostgreSQL database", tags: new[] {"ready"})
        .AddRedis(redisConnectionString: Configuration.GetConnectionString("RedisCache"),
          name: "Redis",
          failureStatus: HealthStatus.Unhealthy, tags: new[] {"ready"});
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

      app.UseSwagger().UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "dl.wm.suite.cms.api"); });


      app.UseHealthChecks("/health/ready",
        new HealthCheckOptions
        {

          ResultStatusCodes =
          {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
          },

          Predicate = (check) => check.Tags.Contains("ready"),
          AllowCachingResponses = false,
          ResponseWriter = WriteHealthCheckReadyResponse
        });

      app.UseHealthChecks("/health/live",
        new HealthCheckOptions
        {
          Predicate = (check) => !check.Tags.Contains("live"),
          ResponseWriter = WriteHealthCheckLiveResponse,
          AllowCachingResponses = false
        });

      app.UseHealthChecks("/health", new HealthCheckOptions
      {
        Predicate = _ => true,
        ResultStatusCodes =
        {
          [HealthStatus.Healthy] = StatusCodes.Status200OK,
          [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
          [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        },
        AllowCachingResponses = false,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      });

      app.UseCors(CorsPolicyName);
      app.UseResponseCaching();
      app.UseHttpCacheHeaders();
      app.UseCookiePolicy();
      app.UseAuthentication();
      app.UseHttpsRedirection();
      app.UseMvc();

      app.UseApiVersioning();


      var serviceProvider = app.ApplicationServices;

      var serviceMqtt = (IRabbitMqttConfiguration) serviceProvider.GetService(typeof(IRabbitMqttConfiguration));

      serviceMqtt.EstablishConnection();

      AutoMapper.Mapper.Initialize(cfg =>
      {
        cfg.AddProfile<TourEntityToTourForCreationUiAutoMapperProfile>();

        cfg.AddProfile<ContainerEntityToContainerUiAutoMapperProfile>();
        cfg.AddProfile<ContainerUiModelToContainerEntityAutoMapperProfile>();
        cfg.AddProfile<ContainerEntityToContainerDeletionUiAutoMapperProfile>();
        cfg.AddProfile<ContainerEntityToContainerPointUiAutoMapperProfile>();

        cfg.AddProfile<EmployeeRoleEntityToEmployeeRolUiAutoMapperProfile>();
        cfg.AddProfile<DepartmentEntityToDepartmentUiAutoMapperProfile>();
        cfg.AddProfile<UserEntityToUserUiAutoMapperProfile>();
        cfg.AddProfile<ContainerUiModelToContainerEntityAutoMapperProfile>();

        cfg.AddProfile<VehicleEntityToVehicleForCreationUiAutoMapperProfile>();
        cfg.AddProfile<VehicleEntityToVehicleUiAutoMapperProfile>();
        cfg.AddProfile<VehicleForCreationUiModelToVehicleEntityAutoMapperProfile>();
        cfg.AddProfile<VehicleUiModelToVehicleEntityAutoMapperProfile>();
      });
    }

    private Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
    {
      httpContext.Response.ContentType = "application/json";

      var json = new JObject(
        new JProperty("OverallStatus", result.Status.ToString()),
        new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
      );

      return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
    }

    private Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result)
    {
      httpContext.Response.ContentType = "application/json";

      var json = new JObject(
        new JProperty("OverallStatus", result.Status.ToString()),
        new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00")),
        new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(dicItem =>
          new JProperty(dicItem.Key, new JObject(
            new JProperty("Status", dicItem.Value.Status.ToString()),
            new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0:0.00")),
            new JProperty("Exception", dicItem.Value.Exception?.Message),
            new JProperty("Data", new JObject(dicItem.Value.Data.Select(dicData =>
              new JProperty(dicData.Key, dicData.Value))))
          ))
        )))
      );

      return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
    }
  }
}
