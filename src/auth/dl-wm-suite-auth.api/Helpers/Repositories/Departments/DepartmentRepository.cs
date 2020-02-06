using System;
using System.Collections.Generic;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Base;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Departments
{
    public class DepartmentRepository : RepositoryBase<Department, Guid>, IDepartmentRepository
    {
        public DepartmentRepository(ISession session)
            : base(session)
        {
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
