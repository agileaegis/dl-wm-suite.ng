using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dl.wm.suite.auth.api.Helpers.Repositories.EmployeeRoles
{
  public class EmployeeRoleRepository : RepositoryBase<EmployeeRole, Guid>, IEmployeeRoleRepository
  {
    public EmployeeRoleRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<EmployeeRole> FindAllEmployeeRolesPagedOf(int? pageNum, int? pageSize)
    {
      var query = Session.QueryOver<EmployeeRole>();

      if (pageNum == -1 & pageSize == -1)
      {
        return new QueryResult<EmployeeRole>(query?.List().AsQueryable());
      }

      return new QueryResult<EmployeeRole>(query
            .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
            .Take((int) pageSize).List().AsQueryable(),
          query.ToRowCountQuery().RowCount(),
          (int) pageSize)
        ;
    }

    public EmployeeRole FindOneByName(string name)
    {
      return
        (EmployeeRole)
        Session.CreateCriteria(typeof(EmployeeRole))
          .Add(Expression.Eq("Name", name))
          .SetCacheable(true)
          .SetCacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Never)
          .UniqueResult()
        ;
    }
  }
}
