using System;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.auth.api.Helpers.Repositories.EmployeeRoles
{
  public interface IEmployeeRoleRepository : IRepository<EmployeeRole, Guid>
  {
    QueryResult<EmployeeRole> FindAllEmployeeRolesPagedOf(int? pageNum, int? pageSize);

    EmployeeRole FindOneByName(string name);
  }
}