using dl.wm.suite.cms.contracts.Users;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IUsersControllerDependencyBlock
    {
        IInquiryUserProcessor InquiryUserProcessor { get; }
    }
}