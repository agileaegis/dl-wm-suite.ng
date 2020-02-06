using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Employees.EmployeeRoles
{
    public class InquiryEmployeeRoleProcessor : IInquiryEmployeeRoleProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        public InquiryEmployeeRoleProcessor(IEmployeeRoleRepository employeeRoleRepository, IAutoMapper autoMapper)
        {
            _employeeRoleRepository = employeeRoleRepository;
            _autoMapper = autoMapper;
        }

        public Task<EmployeeRoleUiModel> GetEmployeeRoleByIdAsync(Guid id)
        {
            return Task.Run(() => _autoMapper.Map<EmployeeRoleUiModel>(_employeeRoleRepository.FindBy(id)));
        }

        public Task<EmployeeRoleUiModel> GetEmployeeRoleByNameAsync(string name)
        {
            return Task.Run(() => _autoMapper.Map<EmployeeRoleUiModel>(_employeeRoleRepository.FindOneByName(name)));
        }
    }
}