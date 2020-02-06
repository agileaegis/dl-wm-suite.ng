using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.model.Devices.Firmware;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Devices.Firmwares
{
    public class FirmwareMap : ClassMap<Firmware>
    {
        public FirmwareMap()
        {
            Table(@"firmwares");

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
