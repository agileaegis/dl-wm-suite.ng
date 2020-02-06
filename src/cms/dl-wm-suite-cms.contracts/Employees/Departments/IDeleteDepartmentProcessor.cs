using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Employees.Departments
{
    public interface IDeleteDepartmentProcessor
    {
        Task DeleteDepartmentAsync(Guid departmentToBeDeletedId);
    }
}