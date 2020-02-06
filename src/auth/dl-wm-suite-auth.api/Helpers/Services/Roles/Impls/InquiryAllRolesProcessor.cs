using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Roles;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts;
using dl.wm.suite.common.dtos.Vms.Roles;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Impls
{
    public class InquiryAllRolesProcessor : IInquiryAllRolesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllRolesProcessor(IAutoMapper autoMapper,
                            IRoleRepository roleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _roleRepository = roleRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<PagedList<Role>> GetRolesAsync(RolesResourceParameters rolesResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_roleRepository.FindAllActiveRolesPagedOf(rolesResourceParameters.PageIndex,
                            rolesResourceParameters.PageSize), 
                    rolesResourceParameters.OrderBy + " " + rolesResourceParameters.SortDirection, 
                    _propertyMappingService.GetPropertyMapping<RoleUiModel, Role>());


            if (!string.IsNullOrEmpty(rolesResourceParameters.Filter) && !string.IsNullOrEmpty(rolesResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = rolesResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = rolesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Role>) collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Role>.Create(collectionBeforePaging,
                rolesResourceParameters.PageIndex,
                rolesResourceParameters.PageSize));
        }

        public Task<int> GetTotalCountRolesAsync()
        {
            return Task.Run(() => _roleRepository.FindCountAllActiveRoles());
        }
    }
}