using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Users;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Contracts
{
    public interface IUpdateUserProcessor
    {
        Task<bool> UpdateUserRefreshTokenExpiredAsync(Guid refreshToken);
        Task<UserForRetrievalUiModel> UpdateUserRefreshTokenAsync(Guid refreshToken);
        Task<UserForRetrievalUiModel> UpdateUserWithNewRefreshTokenAsync(UserForRetrievalUiModel registeredUser, Guid refreshToken);
    }
}
