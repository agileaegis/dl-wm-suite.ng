using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Employees.Departments
{
    public class InquiryDepartmentProcessor : IInquiryDepartmentProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDepartmentRepository _departmentRepository;
        public InquiryDepartmentProcessor(IDepartmentRepository departmentRepository, IAutoMapper autoMapper)
        {
            _departmentRepository = departmentRepository;
            _autoMapper = autoMapper;
        }

        public Task<DepartmentUiModel> GetDepartmentByIdAsync(Guid id)
        {
            return Task.Run(() => _autoMapper.Map<DepartmentUiModel>(_departmentRepository.FindBy(id)));
        }

        public Task<DepartmentUiModel> GetDepartmentByNameAsync(string name)
        {
            return Task.Run(() => _autoMapper.Map<DepartmentUiModel>(_departmentRepository.FindOneByName(name)));
        }
    }
}