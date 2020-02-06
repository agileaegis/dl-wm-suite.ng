using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;

namespace dl.wm.suite.cms.contracts.Employees.EmployeeRoles
{
    public interface IInquiryEmployeeRoleProcessor
    {
        Task<EmployeeRoleUiModel> GetEmployeeRoleByIdAsync(Guid id);
        Task<EmployeeRoleUiModel> GetEmployeeRoleByNameAsync(string name);
    }
}