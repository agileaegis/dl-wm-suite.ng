using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class ContainersControllerDependencyBlock : IContainersControllerDependencyBlock
    {
        public ContainersControllerDependencyBlock(ICreateContainerProcessor createContainerProcessor,
                                                        IInquiryContainerProcessor inquiryContainerProcessor,
                                                        IUpdateContainerProcessor updateContainerProcessor,
                                                        IInquiryAllContainersProcessor allContainerProcessor,
                                                        IDeleteContainerProcessor deleteContainerProcessor)

        {
            CreateContainerProcessor = createContainerProcessor;
            InquiryContainerProcessor = inquiryContainerProcessor;
            UpdateContainerProcessor = updateContainerProcessor;
            InquiryAllContainersProcessor = allContainerProcessor;
            DeleteContainerProcessor = deleteContainerProcessor;
        }

        public ICreateContainerProcessor CreateContainerProcessor { get; private set; }
        public IInquiryContainerProcessor InquiryContainerProcessor { get; private set; }
        public IUpdateContainerProcessor UpdateContainerProcessor { get; private set; }
        public IInquiryAllContainersProcessor InquiryAllContainersProcessor { get; private set; }
        public IDeleteContainerProcessor DeleteContainerProcessor { get; private set; }
    }
}