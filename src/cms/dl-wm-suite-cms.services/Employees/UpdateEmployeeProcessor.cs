using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Employees
{
    public class UpdateEmployeeProcessor : IUpdateEmployeeProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateEmployeeProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IEmployeeRepository employeeRepository)
        {
            _uOf = uOf;
            _employeeRepository = employeeRepository;
            _autoMapper = autoMapper;
        }

        public Task<EmployeeUiModel> UpdateEmployeeAsync(EmployeeForModificationUiModel updatedEmployee)
        {
            throw new NotImplementedException();
        }
    }
}