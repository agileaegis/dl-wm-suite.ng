using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.model.Trackables;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.fleet.repository.Mappings
{
    public class TrackableMap : ClassMap<Trackable>
    {
        public TrackableMap()
        {
            Table(@"trackables");

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
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.Model)
                .Column("model")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.VendorId)
                .Column("imei")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(64)
                ;

            Map(x => x.Phone)
                .Column("phone")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(10)
                ;

            Map(x => x.Os)
                .Column("os")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.Version)
                .Column("version")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.Notes)
                .Column("notes")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
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
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(36)
                ;

            Map(x => x.IsActive)    
                .Column("is_active")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default(@"b'1'").CustomSqlType("bit")
                .Not.Nullable()
                .Precision(1);

            
            HasMany<TrackableAsset>(x => x.TrackableAssets)
                .Access.Property()
                .AsSet()
                .Cascade.None()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("trackable_id", mapping => mapping.Name("trackable_id")
                    .SqlType("int")
                    .Not.Nullable())
                ;

        }
    }
}
