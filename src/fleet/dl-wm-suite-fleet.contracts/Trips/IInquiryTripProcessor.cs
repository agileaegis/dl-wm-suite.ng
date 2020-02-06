using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;

namespace dl.wm.suite.fleet.contracts.Trips
{
    public interface IInquiryTripProcessor
    {
        Task<TripUiModel> GetTripAsync(int id);
        Task<TripUiModel> GetTripByTrackableVendorIdAsync(string vendorId);
    }
}