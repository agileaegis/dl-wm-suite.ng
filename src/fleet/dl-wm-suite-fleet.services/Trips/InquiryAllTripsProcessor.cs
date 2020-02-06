using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.model.Trips;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Trips
{
    public class InquiryAllTripsProcessor : IInquiryAllTripsProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ITripRepository _tripRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllTripsProcessor(IAutoMapper autoMapper,
            ITripRepository tripRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _tripRepository = tripRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<PagedList<Trip>> GetAllTripsAsync(TripsResourceParameters tripsResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_tripRepository
                        .FindAllTripsPagedOf(tripsResourceParameters.PageIndex,
                            tripsResourceParameters.PageSize),
                    tripsResourceParameters.OrderBy + " " + tripsResourceParameters.SortDirection,
                    _propertyMappingService.GetPropertyMapping<TripUiModel, Trip>());


            if (!string.IsNullOrEmpty(tripsResourceParameters.Filter) && !string.IsNullOrEmpty(tripsResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = tripsResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = tripsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Trip>)collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Trip>.Create(collectionBeforePaging,
                tripsResourceParameters.PageIndex,
                tripsResourceParameters.PageSize));
        }

        public Task<List<int>> GetAllTripTodaysIdsAsync()
        {
            return Task.Run(() => _tripRepository.FindAllTodaysTripIds().ToList());
        }
    }
}
