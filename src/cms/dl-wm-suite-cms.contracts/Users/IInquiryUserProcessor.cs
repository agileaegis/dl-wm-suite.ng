using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Users;

namespace dl.wm.suite.cms.contracts.Users
{
    public interface IInquiryUserProcessor
    {
        Task<UserUiModel> GetUserByLoginAsync(string login);
        Task<UserForRetrievalUiModel> GetAuthUserByLoginAsync(string login);
    }
}