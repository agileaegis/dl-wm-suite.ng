using dl.wm.suite.dms.model.Devices;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.dms.repository.Mappings.Vehicles
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
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.SerialNumber)
                .Column("serial_number")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.IsActivated)
                .Column("is_activated")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default("false")
                .CustomSqlType("boolean")
                .Not.Nullable()
                ;

            Map(x => x.IsEnabled)
                .Column("is_enabled")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default("false")
                .CustomSqlType("boolean")
                .Not.Nullable()
                ;

            Map(x => x.ActivationCode)
                .Column("activation_code")
                .CustomType("Guid")
                .Unique()
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Not.Nullable()
                ;

            Map(x => x.ProvisionCode)
                .Column("provision_code")
                .CustomType("Guid")
                .Unique()
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Not.Nullable()
                ;

            Map(x => x.ResetCode)
                .Column("reset_code")
                .CustomType("Guid")
                .Unique()
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Not.Nullable()
                ;

            Map(x => x.ActivationDate)
                .Column("activated_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.ProvisionDate)
                .Column("provisioning_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.ResetDate)
                .Column("reset_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;


            Map(x => x.ActivationBy)
                .Column("activated_by")
                .CustomType("Guid")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Not.Nullable()
                ;

            Map(x => x.ActivationBy)
                .Column("provisioning_by")
                .CustomType("Guid")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Not.Nullable()
                ;

            Map(x => x.ResetBy)
                .Column("reset_by")
                .CustomType("Guid")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("UUID")
                .Not.Nullable()
                ;
        }
    }
}
