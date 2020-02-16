using System.Collections.Generic;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.CustomTypes;
using dl.wm.suite.cms.model.Devices;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace dl.wm.suite.cms.repository.Mappings.Devices
{
  public class MeasurementMap : ClassMap<Measurement>
  {
    public MeasurementMap()
    {
      Table(@"Measurements");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
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

      Map(x => x.JsonbValue).CustomSqlType("jsonb")
        .CustomType<JsonType<JsonbType>>()
        .Column("measurement_value_json")
        .Not.Nullable();

      Map(x => x.Temperature)
        .Column("temperature")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.TempEnabled)
        .Column("temperature_enable")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.FillLevel)
        .Column("fill_level")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.FillLevelEnabled)
        .Column("fill_level_enabled")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.TiltX)
        .Column("tilt_x")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.TiltY)
        .Column("tilt_y")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.TiltZ)
        .Column("tilt_z")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.MagnetometerEnabled)
        .Column("magnetometer_enabled")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.Light)
        .Column("light")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.LightEnabled)
        .Column("light_enabled")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.Battery)
        .Column("battery")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Nullable()
        ;

      Map(x => x.BatterySaveMode)
        .Column("battery_save_mode")
        .CustomType<NBIoTMode>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.Gps)
        .Column("gps")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        ;

      Map(x => x.GpsEnabled)
        .Column("gps_enabled")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.NbIoT)
        .Column("nbiot")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        ;

      Map(x => x.NBIoTMode)
        .Column("nbiot_mode")
        .CustomType<NBIoTMode>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      References(x => x.Device)
        .Class<Device>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .LazyLoad()
        .Columns("device_id")
        ;
    }
  }
}
