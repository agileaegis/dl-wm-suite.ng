using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Employees
{
    public interface IDeleteEmployeeProcessor
    {
        Task DeleteEmployeeAsync(Guid employeeToBeDeletedId);
    }
}