using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IEmployeeRolesControllerDependencyBlock
    {
        ICreateEmployeeRoleProcessor CreateEmployeeRoleProcessor { get; }
        IInquiryEmployeeRoleProcessor InquiryEmployeeRoleProcessor { get; }
        IUpdateEmployeeRoleProcessor UpdateEmployeeRoleProcessor { get; }
        IInquiryAllEmployeeRolesProcessor InquiryAllEmployeeRolesProcessor { get; }
        IDeleteEmployeeRoleProcessor DeleteEmployeeRoleProcessor { get; }
    }
}