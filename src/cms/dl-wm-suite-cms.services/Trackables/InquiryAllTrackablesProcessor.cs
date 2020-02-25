using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Trackables;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Trackables
{
    public class InquiryAllTrackablesProcessor : IInquiryAllTrackablesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ITrackableRepository _trackableRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllTrackablesProcessor(IAutoMapper autoMapper,
            ITrackableRepository trackableRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _trackableRepository = trackableRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<PagedList<Trackable>> GetAllPagedTrackablesAsync(
            TrackablesResourceParameters trackablesResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_trackableRepository
                        .FindAllActiveTrackablesPagedOf(trackablesResourceParameters.PageIndex,
                            trackablesResourceParameters.PageSize),
                    trackablesResourceParameters.OrderBy + " " + trackablesResourceParameters.SortDirection,
                    _propertyMappingService.GetPropertyMapping<TrackableUiModel, Trackable>());


            if (!string.IsNullOrEmpty(trackablesResourceParameters.Filter) &&
                !string.IsNullOrEmpty(trackablesResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = trackablesResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = trackablesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Trackable>) collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields,
                        searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Trackable>.Create(collectionBeforePaging,
                trackablesResourceParameters.PageIndex,
                trackablesResourceParameters.PageSize));
        }
    }
}
