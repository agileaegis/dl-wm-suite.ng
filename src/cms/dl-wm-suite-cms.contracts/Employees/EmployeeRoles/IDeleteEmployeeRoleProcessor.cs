using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Employees.EmployeeRoles
{
    public interface IDeleteEmployeeRoleProcessor
    {
        Task DeleteEmployeeRoleAsync(Guid employeeRoleToBeDeletedId);
    }
}