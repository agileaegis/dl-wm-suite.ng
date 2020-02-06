using System;
using dl.wm.suite.auth.api.Helpers.Mappings;
using dl.wm.suite.auth.api.Helpers.Repositories;
using dl.wm.suite.auth.api.Helpers.Repositories.Departments;
using dl.wm.suite.auth.api.Helpers.Repositories.EmployeeRoles;
using dl.wm.suite.auth.api.Helpers.Repositories.Persons;
using dl.wm.suite.auth.api.Helpers.Repositories.Roles;
using dl.wm.suite.auth.api.Helpers.Repositories.Users;
using dl.wm.suite.auth.api.Helpers.Services;
using dl.wm.suite.auth.api.Helpers.Services.Persons;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts.V1;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Impls;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Impls.V1;
using dl.wm.suite.auth.api.Helpers.Services.Users.Contracts;
using dl.wm.suite.auth.api.Helpers.Services.Users.Contracts.V1;
using dl.wm.suite.auth.api.Helpers.Services.Users.Impls;
using dl.wm.suite.auth.api.Helpers.Services.Users.Impls.V1;
using dl.wm.suite.common.infrastructure.Exceptions.Repositories;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.PropertyMappings.TypeHelpers;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

namespace dl.wm.suite.auth.api.Configurations
{
    public static class Config
    {
        public static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<IPropertyMappingService, PropertyMappingService>();
            services.AddSingleton<ITypeHelperService, TypeHelperService>();

            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            services.AddScoped<IInquiryUserProcessor, InquiryUserProcessor>();
            services.AddScoped<IInquiryAllUsersProcessor, InquiryAllUsersProcessor>();
            services.AddScoped<ICreateUserProcessor, CreateUserProcessor>();
            services.AddScoped<IUpdateUserProcessor, UpdateUserProcessor>();
            services.AddScoped<IActivateUserProcessor, ActivateUserProcessor>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUsersControllerDependencyBlock, UsersControllerDependencyBlock>();

            services.AddScoped<IInquiryPersonProcessor, InquiryPersonProcessor>();
            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddScoped<IInquiryRoleProcessor, InquiryRoleProcessor>();
            services.AddScoped<IInquiryAllRolesProcessor, InquiryAllRolesProcessor>();
            services.AddScoped<ICreateRoleProcessor, CreateRoleProcessor>();
            services.AddScoped<IUpdateRoleProcessor, UpdateRoleProcessor>();
            services.AddScoped<IDeleteRoleProcessor, DeleteRoleProcessor>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRolesControllerDependencyBlock, RolesControllerDependencyBlock>();
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
                var cfg =
                    PostgreSQLConfiguration.Standard.ConnectionString(connectionString)
                        //.ShowSql()
                        .MaxFetchDepth(5)
                        .FormatSql()
                        .AdoNetBatchSize(100);

                var sessionFactory =
                    Fluently.Configure().Database(cfg)
                        .Mappings(
                            m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                        .Cache(c => c.UseSecondLevelCache().UseQueryCache()
                            .ProviderClass(typeof(NHibernate.Caches.RtMemoryCache.RtMemoryCacheProvider)
                                .AssemblyQualifiedName)
                        )
                        .CurrentSessionContext("web")
                        .BuildSessionFactory();

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
