using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;

namespace dl.wm.suite.cms.contracts.Trackables
{
    public interface ICreateTrackableProcessor
    {
        Task<TrackableUiModel> CreateTrackableAsync(string accountEmailToCreateThisTrackable, TrackableForCreationUiModel newTrackableUiModel);
    }
}