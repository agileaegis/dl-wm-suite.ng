using System;
using System.Collections.Generic;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Base;
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
