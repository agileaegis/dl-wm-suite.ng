using dl.wm.suite.fleet.contracts.Locations;

namespace dl.wm.suite.fleet.contracts.V1
{
    public interface ILocationsControllerDependencyBlock
    {
        ICreateLocationProcessor CreateLocationProcessor { get; }
        IInquiryLocationProcessor InquiryLocationProcessor { get; }
    }
}