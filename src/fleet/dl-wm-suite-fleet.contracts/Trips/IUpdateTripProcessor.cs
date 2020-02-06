using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.TrackingPoints;
using dl.wm.suite.common.dtos.Vms.Trips;

namespace dl.wm.suite.fleet.contracts.Trips
{
    public interface IUpdateTripProcessor
    {
        Task<TripUiModel> UpdateTripAsync(string accountEmailToUpdateThisTrip, TripForModificationUiModel updatedTrip);
        Task<TripUiModel> UnregisterTripAsync(string accountEmailToUpdateThisTrip, string trackableImei);
        Task<TripUiModel> CreateTrtackingPoints(string accountEmailToUpdateThisTrip, int tripId, TrackingPointDto[] points);
        Task<TripUiModel> CreateTrtackingPoint(string accountEmailToUpdateThisTrip, int tripId, TrackingPointDto point);
    }
}
