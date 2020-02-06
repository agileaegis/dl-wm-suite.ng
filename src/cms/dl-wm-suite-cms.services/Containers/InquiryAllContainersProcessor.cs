using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Containers
{
    public class InquiryAllContainersProcessor : IInquiryAllContainersProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IContainerRepository _containerRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllContainersProcessor(IAutoMapper autoMapper,
            IContainerRepository containerRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _containerRepository = containerRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<PagedList<Container>> GetContainersAsync(ContainersResourceParameters containersResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_containerRepository
                        .FindAllContainersPagedOf(containersResourceParameters.PageIndex,
                            containersResourceParameters.PageSize),
                    containersResourceParameters.OrderBy + " " + containersResourceParameters.SortDirection,
                    _propertyMappingService.GetPropertyMapping<ContainerUiModel, Container>());


            if (!string.IsNullOrEmpty(containersResourceParameters.Filter) && !string.IsNullOrEmpty(containersResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = containersResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = containersResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Container>)collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Container>.Create(collectionBeforePaging,
                containersResourceParameters.PageIndex,
                containersResourceParameters.PageSize));
        }
    }
}
