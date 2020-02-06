using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.cms.repository.Repositories
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
                return new QueryResult<EmployeeRole>(query?
                    .Where(r => r.IsActive == true)
                    .List().AsQueryable());
            }

            return new QueryResult<EmployeeRole>(query
                        .Where(r => r.IsActive == true)
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
                    .Add(Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .UniqueResult()
                ;
        }

        public IList<Employee> FindAllActiveEmployeeRoles(bool active)
        {
            throw new NotImplementedException();
        }
    }
}
