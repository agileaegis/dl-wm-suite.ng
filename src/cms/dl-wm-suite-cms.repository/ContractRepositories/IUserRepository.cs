using System;
using dl.wm.suite.cms.model.Users;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        User FindUserByLogin(string login);
    }
}