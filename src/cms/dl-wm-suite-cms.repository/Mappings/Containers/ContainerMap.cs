using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices;
using FluentNHibernate.Mapping;
using GeoAPI.Geometries;
using NHibernate.Spatial.Type;


namespace dl.wm.suite.cms.repository.Mappings.Containers
{
  public class ContainerMap : ClassMap<Container>
  {
    public ContainerMap()
    {
      Table(@"containers");

      ImportType<IGeometry>();
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
        .CustomType("string")
        .Access.Property()
        .Unique()
        .Generated.Never()
        .CustomSqlType("varchar(128)")
        .Not.Nullable()
        .Length(128);


      Map(x => x.Level)
        .Column("level")
        .CustomType("int")
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.FillLevel)
        .Column("fill_level")
        .CustomType("int")
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(m => m.Geo, "geo")
        .CustomType<PostGisGeometryType>()
        .LazyLoad();

      Map(x => x.Address)
        .Column("address")
        .CustomType("string")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Length(256)
        ;

      Map(x => x.ImagePath)
        .Column("image_path")
        .CustomType("string")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Not.Nullable()
        .Length(256)
        ;

      Map(x => x.TimeFull)
        .Column("time_full")
        .CustomType("double")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.LastServicedDate)
        .Column("last_serviced_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
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

      Map(x => x.CreatedBy)
        .Column("created_by")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.ModifiedBy)
        .Column("modified_by")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
        .Not.Nullable()
        ;

      Map(x => x.Type)
        .Column("type")
        .CustomType<ContainerType>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.Status)
        .Column("status")
        .CustomType<ContainerStatus>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;


      Map(x => x.MandatoryPickupDate)
        .Column("mandatory_pickup_date")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.MandatoryPickupActive)
        .Column("mandatory_pickup_active")
        .CustomType("boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
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

      Map(x => x.Capacity)
        .Column("capacity")
        .CustomType("int")
        .Access.Property()
        .Generated.Never()
        .Default(@"1100")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.UsefulLoad)
        .Column("load")
        .CustomType("int")
        .Access.Property()
        .Generated.Never()
        .Default(@"520")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.IsFixed)
        .Column("fixed")
        .CustomType("Boolean")
        .Access.Property()
        .Generated.Never()
        .Default("true")
        .CustomSqlType("boolean")
        .Not.Nullable()
        ;

      Map(x => x.Description)
        .Column("description")
        .CustomType("string")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("nvarchar")
        .Nullable()
        ;

      Map(x => x.WasteType)
        .Column("waste_type")
        .CustomType<WasteType>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;

      Map(x => x.Material)
        .Column("material")
        .CustomType<Material>()
        .Access.Property()
        .Generated.Never()
        .Default(@"1")
        .CustomSqlType("integer")
        .Not.Nullable()
        ;
      
      HasOne(x => x.Device)
        .Class<Device>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .PropertyRef(p => p.Container)
        ;

      HasMany<ContainerTour>(x => x.ContainerTours)
        .Access.Property()
        .AsSet()
        .Cascade.None()
        .LazyLoad()
        .Inverse()
        .Generic()
        .KeyColumns.Add("container_id", mapping => mapping.Name("container_id")
          .SqlType("uuid")
          .Not.Nullable())
        ;
    }
  }
}
