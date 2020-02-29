using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Employees
{
    public class InquiryEmployeeProcessor : IInquiryEmployeeProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IEmployeeRepository _employeeRepository;
        public InquiryEmployeeProcessor(IEmployeeRepository employeeRepository, IAutoMapper autoMapper)
        {
            _employeeRepository = employeeRepository;
            _autoMapper = autoMapper;
        }

        public Task<EmployeeUiModel> GetEmployeeByIdAsync(Guid id)
        {
            return Task.Run(() => _autoMapper.Map<EmployeeUiModel>(_employeeRepository.FindBy(id)));
        }

        public Task<EmployeeUiModel> GetEmployeeByEmailAsync(string email)
        {
            return Task.Run(() => _autoMapper.Map<EmployeeUiModel>(_employeeRepository.FindEmployeeByEmail(email)));
        }

        public Task<bool> SearchIfAnyEmployeeByEmailOrLoginExistsAsync(string email, string login)
        {
            return Task.Run(() =>  _employeeRepository.FindEmployeeByEmailOrLogin(email, login).Count > 0);
        }
    }
}