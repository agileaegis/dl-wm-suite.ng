using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;

namespace dl.wm.suite.cms.contracts.Employees.EmployeeRoles
{
    public interface IUpdateEmployeeRoleProcessor
    {
        Task<EmployeeRoleUiModel> UpdateEmployeeRoleAsync(Guid employeeRoleIdToBeUpdated, Guid accountIdToBeUpdatedThisEmployeeRole, EmployeeRoleForModificationUiModel updatedEmployeeRole);
    }
}
