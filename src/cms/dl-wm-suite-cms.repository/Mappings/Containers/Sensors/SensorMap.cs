using dl.wm.suite.cms.model.Containers;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Containers.Sensors
{
    public class SensorMap : ClassMap<ContainerServiceLog>
    {
        public SensorMap()
        {
            Table(@"sensors");

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
