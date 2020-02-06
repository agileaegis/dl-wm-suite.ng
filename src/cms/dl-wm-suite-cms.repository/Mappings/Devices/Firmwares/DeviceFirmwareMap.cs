using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.model.Devices.Firmware;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Devices.Firmwares
{
    public class DeviceFirmwareMap : ClassMap<DeviceFirmware>
    {
        public DeviceFirmwareMap()
        {
            Table(@"devicefirmwares");

            Id(x => x.Id)
                .Column("id")
                .CustomType("Guid")
                .Access.Property()
                .CustomSqlType("uuid")
                .Not.Nullable()
                .GeneratedBy
                .GuidComb()
                ;
        }
    }
}
