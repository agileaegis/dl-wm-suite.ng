using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Employees.Departments
{
    public class UpdateDepartmentProcessor : IUpdateDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateDepartmentProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _departmentRepository = departmentRepository;
            _autoMapper = autoMapper;
        }

        public Task<DepartmentUiModel> UpdateDepartmentAsync(Guid departmentIdToBeUpdated, Guid accountIdToBeUpdatedThisDepartment, DepartmentForModificationUiModel updatedDepartment)
        {
            throw new NotImplementedException();
        }
    }
}