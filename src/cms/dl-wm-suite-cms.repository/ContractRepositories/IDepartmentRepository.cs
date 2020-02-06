using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
        QueryResult<Department> FindAllDepartmentsPagedOf(int? pageNum, int? pageSize);
        Department FindOneByName(string name);
        IList<Employee> FindAllActiveDepartments(bool active);
    }
}
