using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Employees
{
    public class DeleteEmployeeProcessor : IDeleteEmployeeProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEmployeeRepository _employeeRepository;

        public DeleteEmployeeProcessor(IUnitOfWork uOf,
            IEmployeeRepository employeeRepository)
        {
            _uOf = uOf;
            _employeeRepository = employeeRepository;
        }

        public Task DeleteEmployeeAsync(Guid employeeToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}