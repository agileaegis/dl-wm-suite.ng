using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        QueryResult<Employee> FindAllEmployeesPagedOf(int? pageNum, int? pageSize);
        Employee FindByFirstNameAndLastName(string firstName, string lastName);
        IList<Employee> FindActiveEmployees(bool active);
        Employee FindEmployeeByEmail(string email);
        Employee FindEmployeeByUserId(Guid userId);
        IList<Employee> FindEmployeeByEmailOrLogin(string email, string login);
    }
}
