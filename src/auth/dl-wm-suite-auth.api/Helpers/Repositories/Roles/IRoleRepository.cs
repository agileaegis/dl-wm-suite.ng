using System;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Roles
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        QueryResult<Role> FindAllActiveRolesPagedOf(int? pageNum, int? pageSize);
        int FindCountAllActiveRoles();

        Role FindRoleByName(string name);
    }
}
