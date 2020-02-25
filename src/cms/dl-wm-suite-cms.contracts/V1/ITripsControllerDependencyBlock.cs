using dl.wm.suite.cms.contracts.Trips;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface ITripsControllerDependencyBlock
    {
        ICreateTripProcessor CreateTripProcessor { get; }
        IInquiryTripProcessor InquiryTripProcessor { get; }
        IUpdateTripProcessor UpdateTripProcessor { get; }
        IInquiryAllTripsProcessor InquiryAllTripsProcessor { get; }
        IDeleteTripProcessor DeleteTripProcessor { get; }
    }
}