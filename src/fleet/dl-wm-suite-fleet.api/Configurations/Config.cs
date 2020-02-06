using System;
using dl.wm.suite.common.infrastructure.Exceptions.Repositories;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.api.Helpers;
using dl.wm.suite.fleet.api.Redis.TrackingPoints;
using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.contracts.Locations;
using dl.wm.suite.fleet.contracts.Trackables;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.contracts.V1;
using dl.wm.suite.fleet.repository.ContractRepositories;
using dl.wm.suite.fleet.repository.Mappings;
using dl.wm.suite.fleet.repository.NhUnitOfWork;
using dl.wm.suite.fleet.repository.Repositories;
using dl.wm.suite.fleet.services.Assets;
using dl.wm.suite.fleet.services.Locations;
using dl.wm.suite.fleet.services.Trackables;
using dl.wm.suite.fleet.services.Trips;
using dl.wm.suite.fleet.services.V1;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;

namespace dl.wm.suite.fleet.api.Configurations
{
    public static class Config
    {
        public static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<ITypeHelperService, TypeHelperService>();

            //Todo: Changed for Singleton Consumer
            services.AddTransient<ITrackingRedisRepository, TrackingRedisRepository>();

            services.AddScoped<IInquiryLocationProcessor, InquiryLocationProcessor>();
            services.AddScoped<ICreateLocationProcessor, CreateLocationProcessor>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationsControllerDependencyBlock, LocationsControllerDependencyBlock>();

            services.AddScoped<IInquiryAssetProcessor, InquiryAssetProcessor>();
            services.AddScoped<IInquiryAllAssetsProcessor, InquiryAllAssetsProcessor>();
            services.AddScoped<ICreateAssetProcessor, CreateAssetProcessor>();
            services.AddScoped<IUpdateAssetProcessor, UpdateAssetProcessor>();
            services.AddScoped<IDeleteAssetProcessor, DeleteAssetProcessor>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssetsControllerDependencyBlock, AssetsControllerDependencyBlock>();

            services.AddScoped<IInquiryTripProcessor, InquiryTripProcessor>();
            services.AddScoped<IInquiryAllTripsProcessor, InquiryAllTripsProcessor>();
            services.AddScoped<ICreateTripProcessor, CreateTripProcessor>();
            services.AddScoped<IUpdateTripProcessor, UpdateTripProcessor>();
            services.AddScoped<IDeleteTripProcessor, DeleteTripProcessor>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ITripsControllerDependencyBlock, TripsControllerDependencyBlock>();

            services.AddScoped<IInquiryTrackableProcessor, InquiryTrackableProcessor>();
            services.AddScoped<IInquiryAllTrackablesProcessor, InquiryAllTrackablesProcessor>();
            services.AddScoped<ICreateTrackableProcessor, CreateTrackableProcessor>();
            services.AddScoped<IUpdateTrackableProcessor, UpdateTrackableProcessor>();
            services.AddScoped<IDeleteTrackableProcessor, DeleteTrackableProcessor>();
            services.AddScoped<ITrackableRepository, TrackableRepository>();
            services.AddScoped<ITrackablesControllerDependencyBlock, TrackablesControllerDependencyBlock>();
        }

        public static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddSingleton<IAutoMapper, AutoMapperAdapter>();
        }

        public static void ConfigureNHibernate(IServiceCollection services, string connectionString)
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            try
            {
                var cfg = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard
                        .ConnectionString(connectionString)
                        .Driver<MySqlDataDriver>()
                        .Dialect<MySQL57SpatialDialect>()
                        .ShowSql()
                        .MaxFetchDepth(5)
                        .FormatSql()
                        .AdoNetBatchSize(100)
                    )
                    .Mappings(x => x.FluentMappings.AddFromAssemblyOf<AssetMap>())
                    .Cache(c => c.UseSecondLevelCache().UseQueryCache()
                        .ProviderClass(typeof(NHibernate.Caches.RtMemoryCache.RtMemoryCacheProvider)
                            .AssemblyQualifiedName)
                    )
                    .CurrentSessionContext("web")
                    .BuildConfiguration();

                cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));

                var sessionFactory = cfg.BuildSessionFactory();

                services.AddSingleton<ISessionFactory>(sessionFactory);

                services.AddScoped<ISession>((ctx) =>
                {
                    var sf = ctx.GetRequiredService<ISessionFactory>();

                    return sf.OpenSession();

                });

                services.AddScoped<IUnitOfWork, NhUnitOfWork>();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new NHibernateInitializationException(ex.Message, ex.InnerException.Message);
            }
        }
    }
}
