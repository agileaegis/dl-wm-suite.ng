using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace dl.wm.suite.interprocess.api.Configurations
{
  public static class Config
  {
    public static void ConfigureRedis(IServiceCollection services)
    {
      services.AddSingleton<ConnectionMultiplexer>(sp =>
      {
        ConfigurationOptions options = new ConfigurationOptions
        {
          EndPoints = { { "52.178.154.16", 6379 } },
          AllowAdmin = true,
          ConnectTimeout = 60 * 1000,
          ResolveDns = true,
          AbortOnConnectFail = false,
          Password = "123456q!"
        };

        //resolving via dns before connecting
        return ConnectionMultiplexer.Connect(options);
      });
    }
  }
}
