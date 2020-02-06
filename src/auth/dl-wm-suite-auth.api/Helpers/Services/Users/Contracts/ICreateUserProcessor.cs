using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Accounts;
using dl.wm.suite.common.dtos.Vms.Users;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Contracts
{
    public interface ICreateUserProcessor
    {
        Task<UserUiModel> CreateUserAsync(Guid accountIdToCreateThisRole, UserForRegistrationUiModel newUserForRegistration);
    }
}