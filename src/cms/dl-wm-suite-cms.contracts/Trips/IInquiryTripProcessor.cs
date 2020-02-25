using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;

namespace dl.wm.suite.cms.contracts.Trips
{
    public interface IInquiryTripProcessor
    {
        Task<TripUiModel> GetTripAsync(Guid id);
        Task<TripUiModel> GetTripByTrackableVendorIdAsync(string vendorId);
    }
}