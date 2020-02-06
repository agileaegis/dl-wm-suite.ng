using System;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Departments
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
        Department FindOneByName(string name);
    }
}