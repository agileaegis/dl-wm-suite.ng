using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Devices
{
  public class DeviceModelMap : ClassMap<DeviceModel>
  {
    public DeviceModelMap()
    {
      Table(@"devicemodels");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
        ;

      Map(x => x.Name)
        .Column("name")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
        ;

      Map(x => x.CodeName)
        .Column("code_name")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
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


      HasMany<Device>(x => x.Devices)
        .Access.Property()
        .AsSet()
        .Cascade.None()
        .LazyLoad()
        .Inverse()
        .Generic()
        .KeyColumns.Add("devicemodel_id", mapping => mapping.Name("devicemodel_id")
          .SqlType("uuid")
          .Not.Nullable())
        ;
    }
  }
}
