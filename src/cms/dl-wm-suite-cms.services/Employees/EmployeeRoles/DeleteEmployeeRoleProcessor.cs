using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Employees.EmployeeRoles
{
    public class DeleteEmployeeRoleProcessor : IDeleteEmployeeRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;

        public DeleteEmployeeRoleProcessor(IUnitOfWork uOf,
            IEmployeeRoleRepository employeeRoleRepository)
        {
            _uOf = uOf;
            _employeeRoleRepository = employeeRoleRepository;
        }

        public Task DeleteEmployeeRoleAsync(Guid employeeRoleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}