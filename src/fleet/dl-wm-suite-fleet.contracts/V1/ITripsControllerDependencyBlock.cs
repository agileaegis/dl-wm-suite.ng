using dl.wm.suite.fleet.contracts.Trips;

namespace dl.wm.suite.fleet.contracts.V1
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