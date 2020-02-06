using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.Departments;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.Departments;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Employees.Departments
{
    public class InquiryAllDepartmentsProcessor : IInquiryAllDepartmentsProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllDepartmentsProcessor(IAutoMapper autoMapper,
                            IDepartmentRepository departmentRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _departmentRepository = departmentRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<IList<Department>> GetAllDepartmentsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Department>> GetActiveDepartmentsAsync(bool active)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<Department>> GetDepartmentsAsync(DepartmentsResourceParameters departmentsResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_departmentRepository
                        .FindAllDepartmentsPagedOf(departmentsResourceParameters.PageIndex,
                            departmentsResourceParameters.PageSize), 
                    departmentsResourceParameters.OrderBy, 
                    _propertyMappingService.GetPropertyMapping<DepartmentUiModel, Department>());


            if (!string.IsNullOrEmpty(departmentsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = departmentsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = collectionBeforePaging.QueriedItems
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return Task.Run(() => PagedList<Department>.Create(collectionBeforePaging,
                departmentsResourceParameters.PageIndex,
                departmentsResourceParameters.PageSize));
        }
    }
}