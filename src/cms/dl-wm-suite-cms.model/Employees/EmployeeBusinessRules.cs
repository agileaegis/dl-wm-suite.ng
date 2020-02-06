using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Employees
{
    public class EmployeeBusinessRules
    {
        public static BusinessRule FirstName => new BusinessRule("Employee", "Employee First Name must not be null or empty!");
        public static BusinessRule LastName => new BusinessRule("Employee", "Employee Last Name must not be null or empty!");
    }
}