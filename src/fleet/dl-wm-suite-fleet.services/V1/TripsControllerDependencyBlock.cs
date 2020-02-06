using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.contracts.V1;

namespace dl.wm.suite.fleet.services.V1
{
    public class TripsControllerDependencyBlock : ITripsControllerDependencyBlock
    {
        public TripsControllerDependencyBlock(ICreateTripProcessor createTripProcessor,
                                                        IInquiryTripProcessor inquiryTripProcessor,
                                                        IUpdateTripProcessor updateTripProcessor,
                                                        IInquiryAllTripsProcessor allTripProcessor,
                                                        IDeleteTripProcessor deleteTripProcessor)

        {
            CreateTripProcessor = createTripProcessor;
            InquiryTripProcessor = inquiryTripProcessor;
            UpdateTripProcessor = updateTripProcessor;
            InquiryAllTripsProcessor = allTripProcessor;
            DeleteTripProcessor = deleteTripProcessor;
        }

        public ICreateTripProcessor CreateTripProcessor { get; private set; }
        public IInquiryTripProcessor InquiryTripProcessor { get; private set; }
        public IUpdateTripProcessor UpdateTripProcessor { get; private set; }
        public IInquiryAllTripsProcessor InquiryAllTripsProcessor { get; private set; }
        public IDeleteTripProcessor DeleteTripProcessor { get; private set; }
    }
}