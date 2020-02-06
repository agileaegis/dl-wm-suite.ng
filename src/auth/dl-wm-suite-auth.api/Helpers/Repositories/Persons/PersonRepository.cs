using System;
using System.Collections.Generic;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.auth.api.Helpers.Repositories.Base;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dl.wm.suite.auth.api.Helpers.Repositories.Persons
{
    public class PersonRepository : RepositoryBase<Person, Guid>, IPersonRepository
    {
        public PersonRepository(ISession session)
            : base(session)
        {
        }

        public Person FindPersonByEmail(string email)
        {
            return
                (Person)
                Session.CreateCriteria(typeof(Person))
                    .Add(Expression.Eq("Email", email))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .UniqueResult()
                ;
        }

        public IList<Person> FindPersonsByEmailOrLogin(string login)
        {
            return
                Session.CreateCriteria(typeof(Person))
                    .CreateAlias("User", "u", JoinType.InnerJoin)
                    .Add(Restrictions.Or(
                        Restrictions.Eq("Email", login), 
                        Restrictions.Eq("u.Login", login)))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Person>()
                ;
        }
    }
}
