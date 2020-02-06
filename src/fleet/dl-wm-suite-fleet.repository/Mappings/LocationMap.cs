using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.model.TrackingPoints;
using dl.wm.suite.fleet.model.TripLegs;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;

namespace dl.wm.suite.fleet.repository.Mappings
{
    public class LocationMap : ClassMap<Location>
    {
      public LocationMap()
      {
        Table(@"locations");

        Id(x => x.Id)
            .Column("ID")
            .CustomType("Int32")
            .Access.Property().CustomSqlType("int(11)")
            .Not.Nullable()
            .Precision(11)                
            .GeneratedBy
            .Identity()
            ;

        //Map(x => x.Name)
        //  .Column("name")
        //  .CustomType("String")
        //  .Access.Property()
        //  .Generated.Never()
        //  .Unique()
        //  .CustomSqlType("nvarchar")
        //  .Not.Nullable()
        //  .Length(128)
        //  ;

        //Map(x => x.Address)
        //  .Column("address")
        //  .CustomType("String")
        //  .Access.Property()
        //  .Generated.Never()
        //  .Unique()
        //  .CustomSqlType("nvarchar")
        //  .Not.Nullable()
        //  .Length(128)
        //  ;

        Map(x => x.MinimumWaitTime)
          .Column("minimum_wait_time")
          .CustomType("int")
          .Access.Property()
          .Generated.Never()
          .Default(@"0")
          .CustomSqlType("integer")
          .Nullable()
          ;

        Map(x => x.InterestLevel)
          .Column("interest_level")
          .CustomType("int")
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("tinyint")
          .Not.Nullable()
          ;

        //WGS84 (SRID 4326)
        Map(x => x.Geo).CustomType<MySQL57GeometryType>()
            .Column("point")
            .Not.Nullable();

        HasOne(x => x.TrackingPoint)
          .Class<TrackingPoint>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .PropertyRef(p => p.Point)
          ;

        HasOne(x => x.StartPoint)
          .Class<TripLeg>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .PropertyRef(p => p.StartPoint)
          ;

        HasOne(x => x.EndPoint)
          .Class<TripLeg>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .PropertyRef(p => p.EndPoint)
          ;
    }
  }
}
