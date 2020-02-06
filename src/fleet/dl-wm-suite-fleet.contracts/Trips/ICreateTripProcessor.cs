using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;

namespace dl.wm.suite.fleet.contracts.Trips
{
    public interface ICreateTripProcessor
    {
        Task<TripUiModel> CreateTripAsync(string accountEmailToCreateThisTrip, TripForCreationUiModel newTripUiModel);
    }
}