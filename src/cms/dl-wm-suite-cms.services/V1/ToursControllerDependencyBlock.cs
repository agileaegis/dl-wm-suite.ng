using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class ToursControllerDependencyBlock : IToursControllerDependencyBlock
    {
        public ToursControllerDependencyBlock(ICreateTourProcessor createTourProcessor,
                                                        IInquiryTourProcessor inquiryTourProcessor,
                                                        IUpdateTourProcessor updateTourProcessor,
                                                        IInquiryAllToursProcessor allTourProcessor,
                                                        IDeleteTourProcessor deleteTourProcessor)

        {
            CreateTourProcessor = createTourProcessor;
            InquiryTourProcessor = inquiryTourProcessor;
            UpdateTourProcessor = updateTourProcessor;
            InquiryAllToursProcessor = allTourProcessor;
            DeleteTourProcessor = deleteTourProcessor;
        }

        public ICreateTourProcessor CreateTourProcessor { get; private set; }
        public IInquiryTourProcessor InquiryTourProcessor { get; private set; }
        public IUpdateTourProcessor UpdateTourProcessor { get; private set; }
        public IInquiryAllToursProcessor InquiryAllToursProcessor { get; private set; }
        public IDeleteTourProcessor DeleteTourProcessor { get; private set; }
    }
}