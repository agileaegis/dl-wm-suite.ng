using dl.wm.suite.fleet.contracts.Locations;
using dl.wm.suite.fleet.contracts.V1;

namespace dl.wm.suite.fleet.services.V1
{
    public class LocationsControllerDependencyBlock : ILocationsControllerDependencyBlock
    {
        public LocationsControllerDependencyBlock(ICreateLocationProcessor createLocationProcessor,
                                                        IInquiryLocationProcessor inquiryLocationProcessor)

        {
            CreateLocationProcessor = createLocationProcessor;
            InquiryLocationProcessor = inquiryLocationProcessor;
        }

        public ICreateLocationProcessor CreateLocationProcessor { get; private set; }
        public IInquiryLocationProcessor InquiryLocationProcessor { get; private set; }
    }
}