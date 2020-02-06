using dl.wm.suite.fleet.contracts.Trackables;

namespace dl.wm.suite.fleet.contracts.V1
{
    public interface ITrackablesControllerDependencyBlock
    {
        ICreateTrackableProcessor CreateTrackableProcessor { get; }
        IInquiryTrackableProcessor InquiryTrackableProcessor { get; }
        IUpdateTrackableProcessor UpdateTrackableProcessor { get; }
        IInquiryAllTrackablesProcessor InquiryAllTrackablesProcessor { get; }
        IDeleteTrackableProcessor DeleteTrackableProcessor { get; }
    }
}