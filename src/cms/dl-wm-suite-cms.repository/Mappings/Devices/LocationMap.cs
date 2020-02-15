using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;

namespace dl.wm.suite.cms.repository.Mappings.Devices
{
  public class LocationMap : ClassMap<Location>
  {
    public LocationMap()
    {
      Table(@"locations");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
        ;

      //WGS84 (SRID 4326)
      Map(m => m.Point, "point")
        .CustomType<PostGisGeometryType>()
        .Not.Nullable()
        .LazyLoad();


      Map(x => x.Speed)
        .Column("speed")
        .CustomType("double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Altitude)
        .Column("altitude")
        .CustomType("double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Angle)
        .Column("angle")
        .CustomType("double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Satellites)
        .Column("satellites")
        .CustomType("int")
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.CreatedDate)
        .Column("created_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.ModifiedDate)
        .Column("modified_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      References(x => x.Device)
        .Class<Device>()
        .Access.Property()
        .Cascade.None()
        .LazyLoad()
        .Columns("device_id")
        ;

      ////HasOne
      //References(x => x.Device)
      //  .Class<Device>()
      //  .Access.Property()
      //  .Cascade.SaveUpdate()
      //  .Fetch.Join()
      //  .Columns("device_id")
      //  ;
    }
  }
}
