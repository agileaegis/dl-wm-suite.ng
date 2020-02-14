using dl.wm.suite.cms.model.Tours;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;

namespace dl.wm.suite.cms.repository.Mappings.Tours
{
  public class TourLegMap : ClassMap<TourLeg>
  {
    public TourLegMap()
    {
      Table(@"tourlegs");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
        ;

      Map(x => x.StartTime)
        .Column("start_time")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.EndTime)
        .Column("end_time")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.AverageSpeed)
        .Column("average_speed")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.CreatedDate)
        .Column("created_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.CreatedBy)
        .Column("created_by")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
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

      //WGS84 (SRID 4326)
      Map(m => m.StartPoint, "start_location")
        .CustomType<PostGisGeometryType>()
        .Not.Nullable()
        .LazyLoad();

      //WGS84 (SRID 4326)
      Map(m => m.EndPoint, "end_location")
        .CustomType<PostGisGeometryType>()
        .Not.Nullable()
        .LazyLoad();

      //WGS84 (SRID 4326)
      Map(m => m.Route, "route")
        .CustomType<PostGisGeometryType>()
        .Not.Nullable()
        .LazyLoad();

      References(x => x.Tour)
        .Class<Tour>()
        .Access.Property()
        .Cascade.None()
        .LazyLoad()
        .Columns("tour_id")
        ;
    }
  }
}