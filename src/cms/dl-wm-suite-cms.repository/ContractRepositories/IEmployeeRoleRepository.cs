using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IEmployeeRoleRepository : IRepository<EmployeeRole, Guid>
    {
        QueryResult<EmployeeRole> FindAllEmployeeRolesPagedOf(int? pageNum, int? pageSize);
        EmployeeRole FindOneByName(string name);
        IList<Employee> FindAllActiveEmployeeRoles(bool active);
    }
}
