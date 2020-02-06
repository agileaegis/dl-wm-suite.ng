using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Employees.EmployeeRoles
{
    public class UpdateEmployeeRoleProcessor : IUpdateEmployeeRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateEmployeeRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IEmployeeRoleRepository employeeRoleRepository)
        {
            _uOf = uOf;
            _employeeRoleRepository = employeeRoleRepository;
            _autoMapper = autoMapper;
        }

        public Task<EmployeeRoleUiModel> UpdateEmployeeRoleAsync(Guid employeeRoleIdToBeUpdated, Guid accountIdToBeUpdatedThisEmployeeRole, EmployeeRoleForModificationUiModel updatedEmployeeRole)
        {
            throw new NotImplementedException();
        }
    }
}