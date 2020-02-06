using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.model.TripLegs;
using dl.wm.suite.fleet.model.Trips;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;

namespace dl.wm.suite.fleet.repository.Mappings
{
  public class TripLegMap : ClassMap<TripLeg>
  {
    public TripLegMap()
    {
      Table(@"triplegs");

      Id(x => x.Id)
          .Column("ID")
          .CustomType("Int32")
          .Access.Property().CustomSqlType("int(11)")
          .Not.Nullable()
          .Precision(11)                
          .GeneratedBy
          .Identity()
          ;

      Map(x => x.AverageSpeed)
        .Column("average_speed")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      //WGS84 (SRID 4326)
      Map(x => x.Route).CustomType<MySQL57GeometryType>()
        .Not.Nullable();

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

      Map(x => x.CreatedDate)
        .Column("created_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.CreatedBy)
          .Column("created_by")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("nvarchar")
          .Not.Nullable()
          .Length(36)
          ;

      References(x => x.Trip)
        .Class<Trip>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .LazyLoad()
        .Columns("trip_id");


      References(x => x.StartPoint)
        .Class<Location>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        //.LazyLoad()
        .Columns("start_location");

      References(x => x.EndPoint)
        .Class<Location>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        //.LazyLoad()
        .Columns("end_location");
    }
  }
}
