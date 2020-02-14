using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.cms.model.Tours.Trips;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;

namespace dl.wm.suite.cms.repository.Mappings.Trips
{
  public class TrackingPointMap : ClassMap<TrackingPoint>
  {
    public TrackingPointMap()
    {
      Table(@"trackingpoints");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
        ;

      Map(x => x.Provider)
        .Column("provider")
        .CustomType("String")
        .Access.Property()
        .Generated.Never().CustomSqlType("varchar(256)")
        .Not.Nullable()
        .Length(256)
        ;

      Map(x => x.LocationProvider)
        .Column("location_provider")
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .Not.Nullable()
        ;

      Map(x => x.Accuracy)
        .Column("accuracy")
        .CustomType("Double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Speed)
        .Column("speed")
        .CustomType("Double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Altitude)
        .Column("altitude")
        .CustomType("Double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Bearing)
        .Column("bearing")
        .CustomType("Double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double");

      //WGS84 (SRID 4326)
      Map(m => m.GeoPoint, "geo_point")
        .CustomType<PostGisGeometryType>()
        .Not.Nullable()
        .LazyLoad();

      References(x => x.Trip)
        .Class<Trip>()
        .Access.Property()
        .Cascade.None()
        .LazyLoad()
        .Columns("trip_id")
        ;
    }
  }
}