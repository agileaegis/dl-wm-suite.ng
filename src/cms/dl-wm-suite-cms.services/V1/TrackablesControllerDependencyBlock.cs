using dl.wm.suite.cms.contracts.Trackables;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
  public class TrackablesControllerDependencyBlock : ITrackablesControllerDependencyBlock
  {
    public TrackablesControllerDependencyBlock(ICreateTrackableProcessor createTrackableProcessor,
      IInquiryTrackableProcessor inquiryTrackableProcessor,
      IUpdateTrackableProcessor updateTrackableProcessor,
      IInquiryAllTrackablesProcessor allTrackableProcessor,
      IDeleteTrackableProcessor deleteTrackableProcessor)

    {
      CreateTrackableProcessor = createTrackableProcessor;
      InquiryTrackableProcessor = inquiryTrackableProcessor;
      UpdateTrackableProcessor = updateTrackableProcessor;
      InquiryAllTrackablesProcessor = allTrackableProcessor;
      DeleteTrackableProcessor = deleteTrackableProcessor;
    }

    public ICreateTrackableProcessor CreateTrackableProcessor { get; private set; }
    public IInquiryTrackableProcessor InquiryTrackableProcessor { get; private set; }
    public IUpdateTrackableProcessor UpdateTrackableProcessor { get; private set; }
    public IInquiryAllTrackablesProcessor InquiryAllTrackablesProcessor { get; private set; }
    public IDeleteTrackableProcessor DeleteTrackableProcessor { get; private set; }
  }
}