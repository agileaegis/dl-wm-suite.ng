using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using FluentNHibernate.Mapping;

namespace dl.wm.suite.cms.repository.Mappings.Employees
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table(@"employees");

            Id(x => x.Id)
                .Column("id")
                .CustomType("Guid")
                .Access.Property()
                .CustomSqlType("uuid")
                .Not.Nullable()
                .GeneratedBy
                .GuidComb()
                ;

            Map(x => x.Firstname)
                .Column("Firstname")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("varchar(128)")
                .Not.Nullable()
                .Length(128);

            Map(x => x.Lastname)
                .Column("Lastname")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("varchar(128)")
                .Not.Nullable()
                .Length(128);

            Map(x => x.Email)
                .Column("Email")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("varchar(128)")
                .Not.Nullable()
                .Length(128)
                ;

            Map(x => x.UserId)
                .Column("user_id")
                .CustomType("Guid")
                .Access.Property()
                .Generated.Never()
                .Unique()
                .CustomSqlType("uuid")
                .Not.Nullable()
                ;

            Map(x => x.Gender)
                .Column("gender")
                .CustomType<GenderType>()
                .Access.Property()
                .Generated.Never()
                .Default(@"1")
                .CustomSqlType("integer")
                .Not.Nullable()
                ;

            Map(x => x.Phone)
                .Column("phone")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(10)
                ;

            Map(x => x.ExtPhone)
                .Column("extphone")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(5)
                ;

            Map(x => x.Mobile)
                .Column("mobile")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(10)
                ;

            Map(x => x.ExtMobile)
                .Column("extmobile")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Not.Nullable()
                .Length(5)
                ;

            Map(x => x.Notes)
                .Column("notes")
                .CustomType("string")
                .Access.Property()
                .Generated.Never()
                .CustomSqlType("nvarchar")
                .Nullable()
                ;

            Map(x => x.IsActive)
                .Column("active")
                .CustomType("boolean")
                .Access.Property()
                .Generated.Never()
                .Default("true")
                .CustomSqlType("boolean")
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

            Component(x => x.Address,
                employeeAddress =>
                {
                    employeeAddress.Access.Property();
                    employeeAddress.Map(x => x.StreetOne)
                        .Column("address_street_1")
                        .CustomType("string")
                        .Access.Property()
                        .Generated.Never()
                        .CustomSqlType("nvarchar")
                        .Not.Nullable()
                        .Length(128)
                        ;

                    employeeAddress.Map(x => x.StreetTwo)
                        .Column("address_street_2")
                        .CustomType("string")
                        .Access.Property()
                        .Generated.Never()
                        .CustomSqlType("nvarchar")
                        .Nullable()
                        .Length(128)
                        ;

                    employeeAddress.Map(x => x.PostCode)
                        .Column("address_postcode")
                        .CustomType("string")
                        .Access.Property()
                        .Generated.Never()
                        .CustomSqlType("nvarchar")
                        .Not.Nullable()
                        .Length(8)
                        ;

                    employeeAddress.Map(x => x.City)
                        .Column("address_city")
                        .CustomType("string")
                        .Access.Property()
                        .Generated.Never()
                        .CustomSqlType("nvarchar")
                        .Not.Nullable()
                        .Length(64)
                        ;

                    employeeAddress.Map(x => x.Region)
                        .Column("address_region")
                        .CustomType("string")
                        .Access.Property()
                        .Generated.Never()
                        .CustomSqlType("nvarchar")
                        .Not.Nullable()
                        .Length(64)
                        ;
                });

            References(x => x.EmployeeRole)
                .Class<EmployeeRole>()
                .Access.Property()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Columns("employeerole_id");

            References(x => x.Department)
                .Class<Department>()
                .Access.Property()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Columns("department_id");

            HasMany<EmployeeTour>(x => x.EmployeesTours)
                .Access.Property()
                .AsSet()
                .Cascade.None()
                .LazyLoad()
                .Inverse()
                .Generic()
                .KeyColumns.Add("employee_id", mapping => mapping.Name("employee_id")
                    .SqlType("uuid")
                    .Not.Nullable());

        }
    }
}
