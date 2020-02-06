using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Employees;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Employees;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Employees
{
    public class InquiryAllEmployeesProcessor : IInquiryAllEmployeesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllEmployeesProcessor(IAutoMapper autoMapper,
                            IEmployeeRepository employeeRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _employeeRepository = employeeRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<IList<Employee>> GetAllEmployeesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Employee>> GetActiveEmployeesAsync(bool active)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<Employee>> GetEmployeesAsync(EmployeesResourceParameters employeesResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_employeeRepository
                        .FindAllEmployeesPagedOf(employeesResourceParameters.PageIndex,
                            employeesResourceParameters.PageSize), 
                    employeesResourceParameters.OrderBy, 
                    _propertyMappingService.GetPropertyMapping<EmployeeUiModel, Employee>());


            if (!string.IsNullOrEmpty(employeesResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = employeesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = collectionBeforePaging.QueriedItems
                    .Where(a => a.Firstname.ToLowerInvariant().Contains(searchQueryForWhereClause)
                                || a.Lastname.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return Task.Run(() => PagedList<Employee>.Create(collectionBeforePaging,
                employeesResourceParameters.PageIndex,
                employeesResourceParameters.PageSize));
        }
    }
}