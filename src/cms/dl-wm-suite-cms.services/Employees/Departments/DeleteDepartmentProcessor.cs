using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Employees.Departments
{
    public class DeleteDepartmentProcessor : IDeleteDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteDepartmentProcessor(IUnitOfWork uOf,
            IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _departmentRepository = departmentRepository;
        }

        public Task DeleteDepartmentAsync(Guid departmentToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}