using dl.wm.suite.cms.contracts.Tours;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IToursControllerDependencyBlock
    {
        ICreateTourProcessor CreateTourProcessor { get; }
        IInquiryTourProcessor InquiryTourProcessor { get; }
        IUpdateTourProcessor UpdateTourProcessor { get; }
        IInquiryAllToursProcessor InquiryAllToursProcessor { get; }
        IDeleteTourProcessor DeleteTourProcessor { get; }
    }
}