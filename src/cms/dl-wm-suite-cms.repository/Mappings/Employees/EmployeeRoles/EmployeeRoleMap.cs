using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Employees.EmployeeRoles
{
    public class EmployeeRoleMap : ClassMap<EmployeeRole>
    {
        public EmployeeRoleMap()
        {
            Table(@"employeeroles");
            LazyLoad();

            Id(x => x.Id)
                .Column("id")
                .CustomType("Guid")
                .Access.Property()
                .CustomSqlType("uuid")
                .Not.Nullable()
                .GeneratedBy
                .GuidComb();

            Map(x => x.Name)
                .Column("name")
                .CustomType("String")
                .Unique()
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("varchar(128)")
                .Not.Nullable()
                .Length(128);

            Map(x => x.CreatedBy)
                .Column("created_by")
                .CustomType("Guid")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Nullable()
                ;

            Map(x => x.ModifiedBy)
                .Column("modified_by")
                .CustomType("Guid")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Nullable()
                ;

            Map(x => x.CreatedDate)
                .Column("created_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.ModifiedDate)
                .Column("modified_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.IsActive)
                .Column("active")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default("true")
                .CustomSqlType("boolean")
                .Not.Nullable()
                ;


            HasMany<Employee>(x => x.Employees)
                .Access.Property()
                .AsSet()
                .Cascade.All()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("employeerole_id", mapping => mapping.Name("employeerole_id")
                    .SqlType("uuid")
                    .Not.Nullable());
        }
    }
}
