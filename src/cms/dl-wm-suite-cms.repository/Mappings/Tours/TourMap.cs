using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.model.Tours.Trips;
using dl.wm.suite.cms.model.Vehicles;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Tours
{
    public class TourMap : ClassMap<Tour>
    {
      public TourMap()
      {
        Table(@"tours");

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
          .Column("Name")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .Unique()
          .CustomSqlType("nvarchar")
          .Not.Nullable()
          .Length(128)
          ;

        Map(x => x.Type)
          .Column("type")
          .CustomType<TourType>()
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("integer")
          .Not.Nullable()
          ;

        Map(x => x.Status)
          .Column("status")
          .CustomType<TourStatus>()
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("integer")
          .Not.Nullable()
          ;

        Map(x => x.ScheduledDate)
          .Column("scheduled_date")
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

        Map(x => x.IsActive)
          .Column("active")
          .CustomType("Boolean")
          .Access.Property()
          .Generated.Never()
          .Default("true")
          .CustomSqlType("boolean")
          .Not.Nullable()
          ;

        References(x => x.Asset)
          .Class<Vehicle>()
          .Access.Property()
          .Cascade.None()
          .LazyLoad()
          .Columns("vehicle_id")
          ;

        HasOne(x => x.Trip)
          .Class<Trip>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .Fetch.Join()
          .PropertyRef(p => p.Tour)
          ;

      HasMany<EmployeeTour>(x => x.EmployeesTours)
          .Access.Property()
          .AsSet()
          .Cascade.None()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("tour_id", mapping => mapping.Name("tour_id")
            .SqlType("uuid")
            .Not.Nullable())
          ;

        HasMany<ContainerTour>(x => x.ContainerTours)
          .Access.Property()
          .AsSet()
          .Cascade.None()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("tour_id", mapping => mapping.Name("tour_id")
            .SqlType("uuid")
            .Not.Nullable())
          ;

        HasMany<TourLeg>(x => x.TourLegs)
          .Access.Property()
          .AsSet()
          .Cascade.None()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("tour_id", mapping => mapping.Name("tour_id")
            .SqlType("uuid")
            .Not.Nullable())
          ;
      }
    }
}