using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.model.TrackingPoints;
using dl.wm.suite.fleet.model.Trips;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.fleet.repository.Mappings
{
    public class TrackingpointMap : ClassMap<TrackingPoint>
    {
        public TrackingpointMap()
        {
              Table(@"trackingpoints");
              Id(x => x.Id)
                .Column("id")
                .CustomType("Int32")
                .Access.Property().CustomSqlType("int(11)")
                .Not.Nullable()
                .Precision(11)                
                .GeneratedBy.Identity()
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

              Map(x => x.Course)    
                .Column("bearing")
                .CustomType("Double")
                .Access.Property()
                .Generated.Never().CustomSqlType("double");

            References(x => x.Point)
                  .Class<Location>()
                  .Access.Property()
                  .Cascade.SaveUpdate()
                  .Fetch.Join()
                  //.LazyLoad()
                  .Columns("point");

              References(x => x.Trip)
                .Class<Trip>()
                .Access.Property()
                .Cascade.None()
                .LazyLoad()
                .Columns("trip_id");
        }
    }
}

