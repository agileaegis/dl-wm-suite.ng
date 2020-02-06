using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;

namespace dl.wm.suite.cms.contracts.Employees.EmployeeRoles
{
    public interface ICreateEmployeeRoleProcessor
    {
        Task<EmployeeRoleUiModel> CreateEmployeeRoleAsync(Guid accountIdToCreateThisEmployeeRole, EmployeeRoleForCreationUiModel newEmployeeRoleUiModel);
    }
}