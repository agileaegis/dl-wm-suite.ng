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
        }
    }
}
