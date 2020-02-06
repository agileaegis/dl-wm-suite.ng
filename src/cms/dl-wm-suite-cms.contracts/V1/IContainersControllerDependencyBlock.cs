using dl.wm.suite.cms.contracts.Containers;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IContainersControllerDependencyBlock
    {
        ICreateContainerProcessor CreateContainerProcessor { get; }
        IInquiryContainerProcessor InquiryContainerProcessor { get; }
        IUpdateContainerProcessor UpdateContainerProcessor { get; }
        IInquiryAllContainersProcessor InquiryAllContainersProcessor { get; }
        IDeleteContainerProcessor DeleteContainerProcessor { get; }
    }
}