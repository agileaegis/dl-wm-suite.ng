using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Devices
{
  public class DeviceMap : ClassMap<Device>
  {
    public DeviceMap()
    {
      Table(@"devices");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
        ;

      Map(x => x.Imei)
        .Column("imei")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
        ;

      Map(x => x.SerialNumber)
        .Column("serial_number")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
        ;

      Map(x => x.ActivationCode)
        .Column("activation_code")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .Unique()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.ProvisioningCode)
        .Column("provisioning_code")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .Unique()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.ResetCode)
        .Column("reset_code")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .Unique()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.ActivatedBy)
        .Column("activated_by")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.ProvisioningBy)
        .Column("provisioning_by")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.ResetBy)
        .Column("reset_by")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.IsActivated)
        .Column("activated")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("false")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.IsEnabled)
        .Column("enabled")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
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

      References(x => x.DeviceModel)
        .Class<DeviceModel>()
        .Access.Property()
        .Cascade.None()
        .LazyLoad()
        .Columns("devicemodel_id")
        ;

      //HasOne
      References(x => x.Container)
        .Class<Container>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .Columns("container_id")
        ;

      HasOne(x => x.Location)
        .Class<Location>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .PropertyRef(p => p.Device)
        ;

      HasOne(x => x.Sim)
        .Class<Simcard>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .PropertyRef(p => p.Device)
        ;

      HasMany<Measurement>(x => x.Measurements)
        .Access.Property()
        .AsSet()
        .Cascade.None()
        .LazyLoad()
        .Inverse()
        .Generic()
        .KeyColumns.Add("device_id", mapping => mapping.Name("device_id")
          .SqlType("uuid")
          .Not.Nullable())
        ;

      HasMany<Command>(x => x.Commands)
        .Access.Property()
        .AsSet()
        .Cascade.None()
        .LazyLoad()
        .Inverse()
        .Generic()
        .KeyColumns.Add("device_id", mapping => mapping.Name("device_id")
          .SqlType("uuid")
          .Not.Nullable())
        ;
    }
  }
}
