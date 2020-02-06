using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees.EmployeeRoles;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Employees.EmployeeRoles
{
    public class InquiryAllEmployeeRolesProcessor : IInquiryAllEmployeeRolesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllEmployeeRolesProcessor(IAutoMapper autoMapper,
                            IEmployeeRoleRepository employeeRoleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _employeeRoleRepository = employeeRoleRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<IList<EmployeeRole>> GetAllEmployeeRolesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<EmployeeRole>> GetActiveEmployeeRolesAsync(bool active)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<EmployeeRole>> GetEmployeeRolesAsync(EmployeeRolesResourceParameters employeeRolesResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_employeeRoleRepository
                        .FindAllEmployeeRolesPagedOf(employeeRolesResourceParameters.PageIndex,
                            employeeRolesResourceParameters.PageSize), 
                    employeeRolesResourceParameters.OrderBy, 
                    _propertyMappingService.GetPropertyMapping<EmployeeRoleUiModel, EmployeeRole>());


            if (!string.IsNullOrEmpty(employeeRolesResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = employeeRolesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = collectionBeforePaging.QueriedItems
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return Task.Run(() => PagedList<EmployeeRole>.Create(collectionBeforePaging,
                employeeRolesResourceParameters.PageIndex,
                employeeRolesResourceParameters.PageSize));
        }
    }
}