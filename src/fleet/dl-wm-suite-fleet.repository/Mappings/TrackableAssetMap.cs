using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.model.Trackables;
using dl.wm.suite.fleet.model.Trips;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.fleet.repository.Mappings
{
    public class TrackableAssetMap : ClassMap<TrackableAsset>
    {
        public TrackableAssetMap()
        {
            Table(@"trackablesassets");

            Id(x => x.Id)
                .Column("ID")
                .CustomType("Int32")
                .Access.Property().CustomSqlType("int(11)")
                .Not.Nullable()
                .Precision(11)                
                .GeneratedBy
                .Identity()
                ;

            Map(x => x.RegisteredDate)
                .Column("registered_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;
           
            Map(x => x.RegisteredBy)
                .Column("registered_by")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(36)
                ;

            Map(x => x.IsEnabled)    
                .Column("is_enabled")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default(@"b'1'").CustomSqlType("bit")
                .Not.Nullable()
                .Precision(1);

            References(x => x.Asset)
                .Class<Asset>()
                .Access.Property()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Columns("asset_id");

            References(x => x.Device)
                .Class<Trackable>()
                .Access.Property()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Columns("trackable_id");

            HasMany<Trip>(x => x.Trips)
                .Access.Property()
                .AsSet()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("asset_device_id", mapping => mapping.Name("asset_device_id")
                    .SqlType("int")
                    .Nullable())
                ;
        }
    }
}
