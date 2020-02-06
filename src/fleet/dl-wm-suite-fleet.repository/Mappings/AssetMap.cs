using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.model.Trackables;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.fleet.repository.Mappings
{
    public class AssetMap : ClassMap<Asset>
    {
        public AssetMap()
        {
            Table(@"assets");

            Id(x => x.Id)
                .Column("ID")
                .CustomType("Int32")
                .Access.Property().CustomSqlType("int(11)")
                .Not.Nullable()
                .Precision(11)                
                .GeneratedBy
                .Identity()
                ;

            Map(x => x.Name)
                .Column("name")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.NumPlate)
                .Column("num_plate")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(32)
                ;

            Map(x => x.Type)
                .Column("type")
                .CustomType<AssetType>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("int")
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

            Map(x => x.Height)
                .Column("height")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            Map(x => x.Width)
                .Column("width")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            Map(x => x.Length)
                .Column("length")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            Map(x => x.Axels)
                .Column("axels")
                .CustomType("int")
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Trailers)
                .Column("trailers")
                .CustomType("int")
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("int")
                .Nullable()
                ;

            Map(x => x.IsSemi)    
                .Column("semi")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default(@"b'1'").CustomSqlType("bit")
                .Not.Nullable()
                .Precision(1);

            Map(x => x.IsActive)    
                .Column("is_active")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default(@"b'1'").CustomSqlType("bit")
                .Not.Nullable()
                .Precision(1);

            Map(x => x.MaxGradient)
                .Column("max_gradient")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;

            Map(x => x.MinTurnRadius)
                .Column("min_turn_radius")
                .CustomType("double")
                .Access.Property()
                .Generated.Never()
                .Nullable()
                ;
            
            HasMany<TrackableAsset>(x => x.TrackableAssets)
                .Access.Property()
                .AsSet()
                .Cascade.None()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("asset_id", mapping => mapping.Name("asset_id")
                    .SqlType("int")
                    .Not.Nullable())
                ;
        }
    }
}
