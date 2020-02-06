using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Users;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Contracts
{
  public interface IInquiryUserProcessor
  {
    Task<UserForRetrievalUiModel> GetUserAuthJwtTokenByRefreshTokenAsync(Guid refreshToken);
    Task<UserForRetrievalUiModel> GetUserAuthJwtTokenByLoginAndPasswordAsync(string login, string password);
    Task<UserUiModel> GetUserByLoginAsync(string login);
    Task<UserForRetrievalUiModel> GetAuthUserByLoginAsync(string login);
    Task<UserActivationUiModel> GetUserByIdAsync(Guid userId);
  }
}