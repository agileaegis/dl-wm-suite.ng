using dl.wm.suite.auth.api.Helpers.Models;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.auth.api.Helpers.Mappings
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

            Map(x => x.IsActive)
                .Column("active")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default("true")
                .CustomSqlType("boolean")
                .Not.Nullable()
                ;


            HasMany<Person>(x => x.Persons)
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
