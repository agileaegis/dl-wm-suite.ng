using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Tours;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Containers
{
    public class ContanerTourMap : ClassMap<ContainerTour>
    {
        public ContanerTourMap()
        {
            Table(@"containerstours");
            
            Id(x => x.Id)
                .Column("id")
                .CustomType("Guid")
                .Access.Property()
                .CustomSqlType("uuid")
                .Not.Nullable()
                .GeneratedBy
                .GuidComb()
                ;

            Map(x => x.CreatedDate)
                .Column("created_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;
            Map(x => x.ModifiedDate)
                .Column("modified_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.ServicedDate)
                .Column("serviced_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            References(x => x.Container)
              .Class<Container>()
              .Access.Property()
              .Cascade.None()
              .LazyLoad()
              .Columns("container_id");

            References(x => x.Tour)
              .Class<Tour>()
              .Access.Property()
              .Cascade.None()
              .LazyLoad()
              .Columns("tour_id");
        }
    }
}
