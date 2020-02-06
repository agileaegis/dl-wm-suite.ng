using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees;

namespace dl.wm.suite.cms.contracts.Employees
{
    public interface IInquiryEmployeeProcessor
    {
        Task<EmployeeUiModel> GetEmployeeAsync(Guid id);
        Task<EmployeeUiModel> GetEmployeeByEmailAsync(string email);
        Task<bool> SearchIfAnyEmployeeByEmailOrLoginExistsAsync(string email, string login);
    }
}