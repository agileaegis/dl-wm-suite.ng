using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;

namespace dl.wm.suite.cms.contracts.Employees.Departments
{
    public interface IUpdateDepartmentProcessor
    {
        Task<DepartmentUiModel> UpdateDepartmentAsync(Guid departmentIdToBeUpdated, Guid accountIdToBeUpdatedThisDepartment, DepartmentForModificationUiModel updatedDepartment);
    }
}
