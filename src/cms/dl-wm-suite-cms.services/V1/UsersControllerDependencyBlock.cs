using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class UsersControllerDependencyBlock : IUsersControllerDependencyBlock
    {
        public UsersControllerDependencyBlock(IInquiryUserProcessor inquiryVehicleProcessor)

        {
            InquiryUserProcessor = inquiryVehicleProcessor;
        }

        public IInquiryUserProcessor InquiryUserProcessor { get; }
    }
}