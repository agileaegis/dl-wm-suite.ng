using System;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Users
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        QueryResult<User> GetUsersPagedAsync(int? pageNum, int? pageSize);

        User FindUserByLoginAndEmail(string login, string email);
        User FindUserByLogin(string login);

        User FindUserByLoginAndPasswordAsync(string login, string password);

        User FindByUserIdAndActivationKey(Guid userIdToBeActivated, Guid activationKey);
        User FindUserByRefreshTokenAsync(Guid refreshToken);
    }
}