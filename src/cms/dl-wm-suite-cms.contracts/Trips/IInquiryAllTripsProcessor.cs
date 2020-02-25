using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Tours.Trips;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Trips
{
    public interface IInquiryAllTripsProcessor
    {
        Task<PagedList<Trip>> GetAllTripsAsync(TripsResourceParameters tripsResourceParameters);
        Task<List<int>> GetAllTripTodaysIdsAsync();
    }
}