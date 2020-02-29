using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Departments
{
  public class DepartmentRepository : RepositoryBase<Department, Guid>, IDepartmentRepository
  {
    public DepartmentRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<Department> FindAllDepartmentsPagedOf(int? pageNum, int? pageSize)
    {
      var query = Session.QueryOver<Department>();

      if (pageNum == -1 & pageSize == -1)
      {
        return new QueryResult<Department>(query?.List().AsQueryable());
      }

      return new QueryResult<Department>(query
            .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
            .Take((int) pageSize).List().AsQueryable(),
          query.ToRowCountQuery().RowCount(),
          (int) pageSize)
        ;
    }

    public Department FindOneByName(string name)
    {
      return
        (Department)
        Session.CreateCriteria(typeof(Department))
          .Add(Expression.Eq("Name", name))
          .SetCacheable(true)
          .SetCacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Never)
          .UniqueResult()
        ;
    }
  }
}
