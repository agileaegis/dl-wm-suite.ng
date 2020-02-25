using System;
using System.Reflection;
using dl.wm.suite.cms.api.Helpers;
using dl.wm.suite.cms.api.Redis.Maps.Contracts;
using dl.wm.suite.cms.api.Redis.Maps.Impls;
using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.cms.contracts.Devices.DeviceModels;
using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.contracts.Trackables;
using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.contracts.V1;
using dl.wm.suite.cms.contracts.Vehicles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Mappings.Employees;
using dl.wm.suite.cms.repository.NhUnitOfWork;
using dl.wm.suite.cms.repository.Repositories;
using dl.wm.suite.cms.services.Containers;
using dl.wm.suite.cms.services.Devices;
using dl.wm.suite.cms.services.Devices.DeviceModels;
using dl.wm.suite.cms.services.Employees;
using dl.wm.suite.cms.services.Employees.Departments;
using dl.wm.suite.cms.services.Employees.EmployeeRoles;
using dl.wm.suite.cms.services.Tours;
using dl.wm.suite.cms.services.Trackables;
using dl.wm.suite.cms.services.Users;
using dl.wm.suite.cms.services.V1;
using dl.wm.suite.cms.services.Vehicles;
using dl.wm.suite.common.infrastructure.Exceptions.Repositories;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Spatial.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;

namespace dl.wm.suite.cms.api.Configurations
{
  public static class Config
  {
    public static void ConfigureRepositories(IServiceCollection services)
    {
      services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
      services.AddSingleton<ITypeHelperService, TypeHelperService>();

      services.AddScoped<IMapsRedisRepository, MapsRedisRepository>();

      services.AddScoped<IInquiryUserProcessor, InquiryUserProcessor>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IUsersControllerDependencyBlock, UsersControllerDependencyBlock>();

      services.AddScoped<IInquiryDeviceModelProcessor, InquiryDeviceModelProcessor>();
      services.AddScoped<IInquiryAllDeviceModelsProcessor, InquiryAllDeviceModelsProcessor>();
      services.AddScoped<ICreateDeviceModelProcessor, CreateDeviceModelProcessor>();
      services.AddScoped<IUpdateDeviceModelProcessor, UpdateDeviceModelProcessor>();
      services.AddScoped<IDeleteDeviceModelProcessor, DeleteDeviceModelProcessor>();
      services.AddScoped<IDeviceModelRepository, DeviceModelRepository>();
      services.AddScoped<IDeviceModelsControllerDependencyBlock, DeviceModelsControllerDependencyBlock>();

      services.AddScoped<IInquiryDeviceProcessor, InquiryDeviceProcessor>();
      services.AddScoped<IInquiryAllDevicesProcessor, InquiryAllDevicesProcessor>();
      services.AddScoped<ICreateDeviceProcessor, CreateDeviceProcessor>();
      services.AddScoped<IUpdateDeviceProcessor, UpdateDeviceProcessor>();
      services.AddScoped<IDeleteDeviceProcessor, DeleteDeviceProcessor>();
      services.AddScoped<IDeviceRepository, DeviceRepository>();
      services.AddScoped<IDevicesControllerDependencyBlock, DevicesControllerDependencyBlock>();

      services.AddScoped<IInquiryTrackableProcessor, InquiryTrackableProcessor>();
      services.AddScoped<IInquiryAllTrackablesProcessor, InquiryAllTrackablesProcessor>();
      services.AddScoped<ICreateTrackableProcessor, CreateTrackableProcessor>();
      services.AddScoped<IUpdateTrackableProcessor, UpdateTrackableProcessor>();
      services.AddScoped<IDeleteTrackableProcessor, DeleteTrackableProcessor>();
      services.AddScoped<ITrackableRepository, TrackableRepository>();
      services.AddScoped<ITrackablesControllerDependencyBlock, TrackablesControllerDependencyBlock>();

      services.AddScoped<IInquiryContainerProcessor, InquiryContainerProcessor>();
      services.AddScoped<IInquiryAllContainersProcessor, InquiryAllContainersProcessor>();
      services.AddScoped<ICreateContainerProcessor, CreateContainerProcessor>();
      services.AddScoped<IUpdateContainerProcessor, UpdateContainerProcessor>();
      services.AddScoped<IDeleteContainerProcessor, DeleteContainerProcessor>();
      services.AddScoped<IContainerRepository, ContainerRepository>();
      services.AddScoped<IContainersControllerDependencyBlock, ContainersControllerDependencyBlock>();

      services.AddScoped<IInquiryTourProcessor, InquiryTourProcessor>();
      services.AddScoped<IInquiryAllToursProcessor, InquiryAllToursProcessor>();
      services.AddScoped<ICreateTourProcessor, CreateTourProcessor>();
      services.AddScoped<IUpdateTourProcessor, UpdateTourProcessor>();
      services.AddScoped<IDeleteTourProcessor, DeleteTourProcessor>();
      services.AddScoped<ITourRepository, TourRepository>();
      services.AddScoped<IToursControllerDependencyBlock, ToursControllerDependencyBlock>();

      services.AddScoped<IInquiryDepartmentProcessor, InquiryDepartmentProcessor>();
      services.AddScoped<IInquiryAllDepartmentsProcessor, InquiryAllDepartmentsProcessor>();
      services.AddScoped<ICreateDepartmentProcessor, CreateDepartmentProcessor>();
      services.AddScoped<IUpdateDepartmentProcessor, UpdateDepartmentProcessor>();
      services.AddScoped<IDeleteDepartmentProcessor, DeleteDepartmentProcessor>();
      services.AddScoped<IDepartmentRepository, DepartmentRepository>();
      services.AddScoped<IDepartmentsControllerDependencyBlock, DepartmentsControllerDependencyBlock>();

      services.AddScoped<IInquiryEmployeeRoleProcessor, InquiryEmployeeRoleProcessor>();
      services.AddScoped<IInquiryAllEmployeeRolesProcessor, InquiryAllEmployeeRolesProcessor>();
      services.AddScoped<ICreateEmployeeRoleProcessor, CreateEmployeeRoleProcessor>();
      services.AddScoped<IUpdateEmployeeRoleProcessor, UpdateEmployeeRoleProcessor>();
      services.AddScoped<IDeleteEmployeeRoleProcessor, DeleteEmployeeRoleProcessor>();
      services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
      services.AddScoped<IEmployeeRolesControllerDependencyBlock, EmployeeRolesControllerDependencyBlock>();

      services.AddScoped<IInquiryEmployeeProcessor, InquiryEmployeeProcessor>();
      services.AddScoped<IInquiryAllEmployeesProcessor, InquiryAllEmployeesProcessor>();
      services.AddScoped<ICreateEmployeeProcessor, CreateEmployeeProcessor>();
      services.AddScoped<IUpdateEmployeeProcessor, UpdateEmployeeProcessor>();
      services.AddScoped<IDeleteEmployeeProcessor, DeleteEmployeeProcessor>();
      services.AddScoped<IEmployeeRepository, EmployeeRepository>();
      services.AddScoped<IEmployeesControllerDependencyBlock, EmployeesControllerDependencyBlock>();

      services.AddScoped<IInquiryVehicleProcessor, InquiryVehicleProcessor>();
      services.AddScoped<IInquiryAllVehiclesProcessor, InquiryAllVehiclesProcessor>();
      services.AddScoped<ICreateVehicleProcessor, CreateVehicleProcessor>();
      services.AddScoped<IUpdateVehicleProcessor, UpdateVehicleProcessor>();
      services.AddScoped<IDeleteVehicleProcessor, DeleteVehicleProcessor>();
      services.AddScoped<IVehicleRepository, VehicleRepository>();
      services.AddScoped<IVehiclesControllerDependencyBlock, VehiclesControllerDependencyBlock>();

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
          .Database(PostgreSQLConfiguration.PostgreSQL82
            .ConnectionString(connectionString)
            .Driver<NpgsqlDriver>()
            .Dialect<PostGis20Dialect>()
            .ShowSql()
            .MaxFetchDepth(5)
            .FormatSql()
            .Raw("transaction.use_connection_on_system_prepare", "true")
            .AdoNetBatchSize(100)
          )
          .Mappings(x => x.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
          .Cache(c => c.UseSecondLevelCache().UseQueryCache()
            .ProviderClass(typeof(NHibernate.Caches.RtMemoryCache.RtMemoryCacheProvider)
              .AssemblyQualifiedName)
          )
          .CurrentSessionContext("web")
          .BuildConfiguration();

        cfg.AddAssembly(Assembly.GetExecutingAssembly());
        cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
        Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
        Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);

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
        throw new NHibernateInitializationException(ex.Message, ex.InnerException?.Message);
      }
    }
  }
}
