using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;

namespace dl.wm.suite.fleet.contracts.Trackables
{
    public interface IInquiryTrackableProcessor
    {
        Task<TrackableUiModel> GetTrackableAsync(int id);
        Task<TrackableUiModel> GetTrackableByImeiAsync(string imei);
    }
}