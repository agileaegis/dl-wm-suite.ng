using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.cms.model.Tours.Trips;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Trips
{
  public class TripMap : ClassMap<Trip>
  {
    public TripMap()
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

      Map(x => x.StartTime)
        .Column("start_time")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.EndTime)
        .Column("end_time")
        .CustomType("DateTime")
        .Access.Property()
        .Generated.Never()
        .Not.Nullable()
        ;

      Map(x => x.Duration)
        .Column("duration")
        .CustomType("Double")
        .Access.Property()
        .Generated.Never().CustomSqlType("double")
        ;

      Map(x => x.Code)
        .Column("code")
        .CustomType("Guid")
        .Access.Property()
        .Generated.Never()
        .CustomSqlType("uuid")
        .Not.Nullable()
        .Unique()
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

      References(x => x.Tour)
        .Class<Tour>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .Fetch.Join()
        .Columns("tour_id");

      References(x => x.Trackable)
        .Class<Trackable>()
        .Access.Property()
        .Cascade.None()
        .LazyLoad()
        .Columns("trackable_id")
        ;

      HasMany<TrackingPoint>(x => x.TrackingPoints)
        .Access.Property()
        .AsSet()
        .Cascade.None()
        .LazyLoad()
        .Inverse()
        .Generic()
        .KeyColumns.Add("trip_id", mapping => mapping.Name("trip_id")
          .SqlType("uuid")
          .Not.Nullable())
        ;
    }
  }
}