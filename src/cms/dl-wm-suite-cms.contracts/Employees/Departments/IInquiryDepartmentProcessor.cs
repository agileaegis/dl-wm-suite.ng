using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;

namespace dl.wm.suite.cms.contracts.Employees.Departments
{
    public interface IInquiryDepartmentProcessor
    {
        Task<DepartmentUiModel> GetDepartmentByIdAsync(Guid id);
        Task<DepartmentUiModel> GetDepartmentByNameAsync(string name);
    }
}