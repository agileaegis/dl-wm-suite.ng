using dl.wm.suite.cms.model.Containers;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Containers.Sensors
{
  public class SensorMap : ClassMap<Sensor>
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

      //HasOne
      References(x => x.Container)
        .Class<Container>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .Columns("container_id")
        ;
    }
  }
}
