using System;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Impls;
using dl.wm.suite.telemetry.api.Messaging;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dl.wm.suite.telemetry.api.Configurations.Installers
{
    public static class CassandraInitializer
    {
        // Cassandra Cluster Configs      
        private static string UserName;
        private static string Password;
        private static string CassandraContactPoint;
        private static int CassandraPort;
        public static ISession session;

        public static void AddCassandra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJSONSerializer, JSONSerializer>();
            services.AddTransient<IDeviceCassandraRepository, DeviceCassandraRepository>();
            services.AddTransient<ITelemetryRowCassandraRepository, TelemetryRowCassandraRepository>();

            UserName = configuration.GetSection("Cassandra:UserName").Value;
            Password = configuration.GetSection("Cassandra:Password").Value;
            CassandraContactPoint = configuration.GetSection("Cassandra:CassandraContactPoint").Value;
            CassandraPort = Int32.Parse(configuration.GetSection("Cassandra:CassandraPort").Value);

            Task t = CassandraInitializer.InitializeCassandraSession();
            t.Wait();   
        }

        public static async Task InitializeCassandraSession()
        {
            // Connect to cassandra cluster  (Cassandra API on Azure Cosmos DB supports only TLSv1.2)
            var options = new SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
            Cluster cluster = Cluster.Builder().WithCredentials(UserName, Password).WithPort(CassandraPort).AddContactPoint(CassandraContactPoint).WithSSL(options).Build();

            session = await cluster.ConnectAsync("aegiswmtelemetry");
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Trace.WriteLine("Certificate error: {0}", sslPolicyErrors.ToString());
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}