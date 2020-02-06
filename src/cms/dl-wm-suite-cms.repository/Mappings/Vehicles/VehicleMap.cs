using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.model.Vehicles;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Vehicles
{
    public class VehicleMap : ClassMap<Vehicle>
    {
        public VehicleMap()
        {
            Table(@"vehicles");

            Id(x => x.Id)
                .Column("id")
                .CustomType("Guid")
                .Access.Property()
                .CustomSqlType("uuid")
                .Not.Nullable()
                .GeneratedBy
                .GuidComb()
                ;

            Map(x => x.NumPlate)
                .Column("num_plate")
                .CustomType("String")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(16)
                ;

            Map(x => x.Brand)
              .Column("Brand")
              .CustomType("string")
              .Access.Property()
              .Generated.Never()
              .CustomSqlType("nvarchar")
              .Not.Nullable()
              .Length(32)
              ;

            Map(x => x.RegisteredDate)
                .Column("registered_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;

            Map(x => x.Type)
              .Column("type")
              .CustomType<VehicleType>()
              .Access.Property()
              .Generated.Never()
              .Default(@"1")
              .CustomSqlType("integer")
              .Not.Nullable()
              ;

            Map(x => x.Status)
              .Column("status")
              .CustomType<VehicleStatus>()
              .Access.Property()
              .Generated.Never()
              .Default(@"1")
              .CustomSqlType("integer")
              .Not.Nullable()
              ;

            Map(x => x.Gas)
              .Column("Gas")
              .CustomType<GasType>()
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

            Map(x => x.IsActive)
                .Column("active")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never()
                .Default("true")
                .CustomSqlType("boolean")
                .Not.Nullable()
                ;

            HasMany<Tour>(x => x.Tours)
              .Access.Property()
              .AsSet()
              .Cascade.None()
              .LazyLoad()
              .Inverse()
              .Generic()
              .KeyColumns.Add("vehicle_id", mapping => mapping.Name("vehicle_id")
                                                                   .SqlType("uuid")
                                                                   .Nullable())
              ;
        }
    }
}
