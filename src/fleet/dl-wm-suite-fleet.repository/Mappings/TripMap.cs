using dl.wm.suite.fleet.model.Trackables;
using dl.wm.suite.fleet.model.TrackingPoints;
using dl.wm.suite.fleet.model.TripLegs;
using dl.wm.suite.fleet.model.Trips;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.fleet.repository.Mappings
{
    public class TripMap : ClassMap<Trip>
    {
        public TripMap()
        {
              Table(@"trips");
              Id(x => x.Id)
                .Column("id")
                .CustomType("Int32")
                .Access.Property().CustomSqlType("int(11)")
                .Not.Nullable()
                .Precision(11)                
                .GeneratedBy.Identity()
                ;
              
              Map(x => x.StartTime)    
                .Column("start_time")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Default(@"CURRENT_TIMESTAMP").CustomSqlType("timestamp")
                .Not.Nullable()
                ;
              
              Map(x => x.EndTime)    
                .Column("end_time")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Default(@"CURRENT_TIMESTAMP").CustomSqlType("timestamp")
                .Not.Nullable()
                ;
              
              Map(x => x.CreatedDate)    
                .Column("created_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Default(@"CURRENT_TIMESTAMP").CustomSqlType("timestamp")
                .Not.Nullable()
                ;
              
              Map(x => x.CreatedBy)    
                .Column("created_by")
                .CustomType("String")
                .Access.Property()
                .Generated.Never().CustomSqlType("varchar(36)")
                .Not.Nullable()
                .Length(36)
                ;

              Map(x => x.Duration)    
                .Column("duration")
                .CustomType("Double")
                .Access.Property()
                .Generated.Never().CustomSqlType("double")
                ;
              
              Map(x => x.IsActive)    
                .Column("is_active")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("tinyint(1)")
                .Not.Nullable()
                .Precision(1)
                ;
				
              Map(x => x.Code)    
                .Column("code")
                .CustomType("String")
                .Access.Property()
                .Generated.Never().CustomSqlType("varchar(36)")
                .Not.Nullable()
                .Length(36)
                .Unique()
                ;
				
              Map(x => x.ModifiedDate)    
                .Column("modified_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Default(@"CURRENT_TIMESTAMP").CustomSqlType("timestamp")
                .Not.Nullable()
                ;

              Map(x => x.ModifiedBy)    
                .Column("modified_by")
                .CustomType("String")
                .Access.Property()
                .Generated.Never().CustomSqlType("varchar(36)")
                .Not.Nullable()
                .Length(36)
                ;

              HasMany<TrackingPoint>(x => x.TrackingPoints)
                .Access.Property()
                .AsSet()
                .Cascade.All()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("trip_id", mapping => mapping.Name("trip_id")
                                                                     .SqlType("int")
                                                                     .Not.Nullable())
                ;

              HasMany<TripLeg>(x => x.Legs)
                .Access.Property()
                .AsSet()
                .Cascade.All()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("trip_id", mapping => mapping.Name("trip_id")
                                                                     .SqlType("int")
                                                                     .Not.Nullable())
                ;

              References(x => x.DeviceAsset)
                .Class<TrackableAsset>()
                .Access.Property()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Columns("asset_device_id")
                ;
        }
    }
}
