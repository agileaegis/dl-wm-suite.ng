using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;

namespace dl.wm.suite.cms.contracts.Trackables
{
    public interface IInquiryTrackableProcessor
    {
        Task<TrackableUiModel> GetTrackableAsync(Guid id);
        Task<TrackableUiModel> GetTrackableByImeiAsync(string imei);
    }
}