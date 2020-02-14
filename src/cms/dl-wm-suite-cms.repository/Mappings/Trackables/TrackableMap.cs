using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.cms.model.Tours.Trips;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Trackables
{
  public class TrackableMap : ClassMap<Trackable>
  {
    public TrackableMap()
    {
      Table(@"trackables");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
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

      HasMany<TrackingPoint>(x => x.Trips)
        .Access.Property()
        .AsSet()
        .Cascade.None()
        .LazyLoad()
        .Inverse()
        .Generic()
        .KeyColumns.Add("trackable_id", mapping => mapping.Name("trackable_id")
          .SqlType("uuid")
          .Not.Nullable())
        ;
    }
  }
}