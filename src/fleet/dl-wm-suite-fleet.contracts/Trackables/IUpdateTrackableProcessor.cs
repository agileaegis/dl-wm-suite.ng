using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;

namespace dl.wm.suite.fleet.contracts.Trackables
{
    public interface IUpdateTrackableProcessor
    {
        Task<TrackableUiModel> UpdateTrackableAsync(Guid id, TrackableForModificationUiModel updatedTrackable);
    }
}
