using dl.wm.suite.telemetry.api.Helpers.Domain.Contracts;
using dl.wm.suite.telemetry.api.Helpers.Marten.DataAccess;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Models;
using dl.wm.suite.telemetry.api.Helpers.Marten.Logging;
using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;

namespace dl.wm.suite.telemetry.api.Configurations.Installers
{
    public static class MartenInstaller
    {
        public static void AddMarten(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(CreateDocumentStore(cnnString));

            services.AddScoped<IDataStore, MartenDataStore>();
        }

        private static IDocumentStore CreateDocumentStore(string cn)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(cn);
                _.Logger(new DeviceMartenLogger());

                _.DdlRules.TableCreation = CreationStyle.CreateIfNotExists;
                _.AutoCreateSchemaObjects = AutoCreate.All;
                _.Schema.For<Device>().DocumentAlias("devices").Duplicate(t => t.Imei, pgType: "varchar(128)", configure: idx => idx.IsUnique = true);
                _.DatabaseSchemaName = Marten.StoreOptions.DefaultDatabaseSchemaName;

                _.Serializer(CustomizeJsonSerializer());
                _.Events.DatabaseSchemaName = "event_store";
            });
        }

        private static JsonNetSerializer CustomizeJsonSerializer()
        {
            var serializer = new JsonNetSerializer();

            serializer.Customize(_ =>
            {
                _.ContractResolver = new ProtectedSettersContractResolver();
            });

            return serializer;
        }
    }
}