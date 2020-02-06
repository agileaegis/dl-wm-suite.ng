using dl.wm.suite.cms.model.Containers;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Containers.ServiceLogs
{
    public class ContainerServiceLogMap : ClassMap<ContainerServiceLog>
    {
        public ContainerServiceLogMap()
        {
            Table(@"servicelogs");

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
