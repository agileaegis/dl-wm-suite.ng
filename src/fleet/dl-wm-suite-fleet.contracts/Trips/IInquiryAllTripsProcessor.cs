using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.fleet.model.Trips;

namespace dl.wm.suite.fleet.contracts.Trips
{
    public interface IInquiryAllTripsProcessor
    {
        Task<PagedList<Trip>> GetAllTripsAsync(TripsResourceParameters tripsResourceParameters);
        Task<List<int>> GetAllTripTodaysIdsAsync();
    }
}