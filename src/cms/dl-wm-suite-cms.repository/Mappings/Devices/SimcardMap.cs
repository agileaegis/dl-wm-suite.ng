using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Devices
{
  public class SimcardMap : ClassMap<Simcard>
  {
    public SimcardMap()
    {
      Table(@"simcards");

      Id(x => x.Id)
        .Column("id")
        .CustomType("Guid")
        .Access.Property()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .GeneratedBy
        .GuidComb()
        ;

      Map(x => x.Iccid)
        .Column("iccid")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
        ;

      Map(x => x.Imsi)
        .Column("imsi")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
        ;

      Map(x => x.CountryIso)
        .Column("country_iso")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Length(128)
        ;

      Map(x => x.Number)
        .Column("number")
        .CustomType("String")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Unique()
        .Length(128)
        ;

      Map(x => x.PurchaseDate)
        .Column("purchase_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.CardType)
        .Column("simcardtype")
        .CustomType<SimCardType>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.NetworkType)
        .Column("simnetworktype")
        .CustomType<SimNetworkType>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
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

      //HasOne
      References(x => x.Device)
        .Class<Device>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .Columns("device_id")
        ;
    }
  }
}
