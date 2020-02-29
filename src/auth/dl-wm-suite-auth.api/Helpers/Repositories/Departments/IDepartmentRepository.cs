using System;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Departments
{
  public interface IDepartmentRepository : IRepository<Department, Guid>
  {
    QueryResult<Department> FindAllDepartmentsPagedOf(int? pageNum, int? pageSize);
    Department FindOneByName(string name);
  }
}