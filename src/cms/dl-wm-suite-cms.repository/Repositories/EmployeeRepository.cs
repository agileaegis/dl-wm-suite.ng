using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dl.wm.suite.cms.repository.Repositories
{
  public class EmployeeRepository : RepositoryBase<Employee, Guid>, IEmployeeRepository
  {
    public EmployeeRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<Employee> FindAllEmployeesPagedOf(int? pageNum = -1, int? pageSize = -1)
    {
      var query = Session.QueryOver<Employee>();

      if (pageNum == -1 & pageSize == -1)
      {
        return new QueryResult<Employee>(query?.List().AsQueryable());
      }

      return new QueryResult<Employee>(query
            .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
            .Take((int) pageSize).List().AsQueryable(),
          query.ToRowCountQuery().RowCount(),
          (int) pageSize)
        ;
    }

    public Employee FindByFirstNameAndLastName(string firstName, string lastName)
    {
      return (Employee)
        Session.CreateCriteria(typeof(Employee))
          .Add(Expression.Eq("FirstName", firstName))
          .Add(Expression.Eq("Name", lastName))
          .UniqueResult()
        ;
    }

    public IList<Employee> FindActiveEmployees(bool active)
    {
      return
        Session.CreateCriteria(typeof(Employee))
          .Add(Expression.Eq("Active", active))
          .SetCacheable(true)
          .SetCacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Never)
          .List<Employee>()
        ;
    }

    public Employee FindEmployeeByEmail(string email)
    {
      return
        (Employee)
        Session.CreateCriteria(typeof(Employee))
          .Add(Expression.Eq("Name", email))
          .SetCacheable(true)
          .SetCacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Never)
          .UniqueResult()
        ;
    }

    public Employee FindEmployeeByUserId(Guid userId)
    {
      return
        (Employee)
        Session.CreateCriteria(typeof(Employee))
          .Add(Expression.Eq("UserId", userId))
          .SetCacheable(true)
          .SetCacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Never)
          .UniqueResult()
        ;
    }

    public IList<Employee> FindEmployeeByEmailOrLogin(string email, string login)
    {
      return
        Session.CreateCriteria(typeof(Employee))
          .CreateAlias("Tenant", "t", JoinType.InnerJoin)
          .Add(Restrictions.Or(
            Restrictions.Eq("Name", email),
            Restrictions.Eq("t.Login", login)))
          .SetCacheable(true)
          .SetCacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Never)
          .List<Employee>()
        ;
    }
  }
}
