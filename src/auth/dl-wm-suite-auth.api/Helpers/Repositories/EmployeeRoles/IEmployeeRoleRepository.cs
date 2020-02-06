using System;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.auth.api.Helpers.Repositories.EmployeeRoles
{
    public interface IEmployeeRoleRepository : IRepository<EmployeeRole, Guid>
    {
        EmployeeRole FindOneByName(string name);
    }
}