using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.repository.Mappings.Base;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Employees
{
    public class EmployeesTourMap : ClassMap<EmployeeTour>
    {
        public EmployeesTourMap()
        {
            Table(@"employeestours");
            
            Id(x => x.Id)
                .Column("id")
                .CustomType("Guid")
                .Access.Property()
                .CustomSqlType("uuid")
                .Not.Nullable()
                .GeneratedBy
                .GuidComb()
                ;

            Map(x => x.RegisteredDate)
                .Column("registered_date")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never()
                .Not.Nullable()
                ;


            Map(x => x.StatusType)
                .Column("employee_status")
                .CustomType<EmployeeStatusType>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Role)
                .Column("employee_role")
                .CustomType<EmployeeRoleType>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Duration)
                .Column("duration")
                .CustomType("int")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Comments)
                .Column("comments")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Nullable()
                ;
            
            Map(x => x.IsActive)
              .Column("active")
              .CustomType("bool")
              .Access.Property()
              .Generated.Never()
              .Default("true")
              .CustomSqlType("boolean")
              .Not.Nullable()
              ;

            References(x => x.Employee)
              .Class<Employee>()
              .Access.Property()
              .Cascade.None()
              .LazyLoad()
              .Columns("employee_id");

            References(x => x.Tour)
              .Class<Tour>()
              .Access.Property()
              .Cascade.None()
              .LazyLoad()
              .Columns("tour_id");
        }
    }
}
