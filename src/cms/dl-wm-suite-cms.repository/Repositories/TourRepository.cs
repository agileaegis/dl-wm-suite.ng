using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dl.wm.suite.cms.repository.Repositories
{
    public class TourRepository : RepositoryBase<Tour, Guid>, ITourRepository
    {
        public TourRepository(ISession session)
            : base(session)
        {
        }

        public Tour FindByName(string name)
        {
            return (Tour)
                Session.CreateCriteria(typeof(Tour))
                   .Add(Restrictions.Eq("Name", name))
                   .UniqueResult()
                   ;
        }

        public QueryResult<Tour> FindAllToursPagedOf(int? pageNum, int? pageSize)
        {
            return null;
        }

        public QueryResult<Tour> FindAllToursPagedOfByScheduledDate(DateTime scheduledDateTour, int? pageNum, int? pageSize)
        {
            return null;
        }

        public Tour FindByNameSpecifiedDate(string nameTour, DateTime scheduledDateTour)
        {
            return (Tour)
                Session.CreateCriteria(typeof(Tour))
                   .Add(Restrictions.Eq("Name", nameTour))
                    .Add(Restrictions.Eq(
                       Projections.SqlFunction("date",
                                               NHibernateUtil.Date,
                                               Projections.Property("ScheduledDate")),
                       scheduledDateTour.Date))
                   .UniqueResult()
                   ;
        }

        public IList<Tour> FindAllByScheduledDate(DateTime scheduledDate)
        {
            return 
                Session.CreateCriteria(typeof(Tour))
                .Add(Restrictions.Eq(
                   Projections.SqlFunction("date",
                                           NHibernateUtil.Date,
                                           Projections.Property("ScheduledDate")),
                   scheduledDate.Date))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never)
                .List<Tour>()
               ;
        }

        public IList<Tour> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate)
        {
            return
                Session.CreateCriteria(typeof(Tour))
                .Add(
                    Expression.Conjunction()
                        .Add(Restrictions.Ge("ScheduledDate", startedScheduledDate))
                        .Add(Restrictions.Lt("ScheduledDate", endedScheduledDate))
                    )
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Tour>()
                ;
        }

        public IList<Tour> FindTodayTourByEmployee(Guid employeeId)
        {
            return
            Session.CreateCriteria<Tour>("t")
                    .CreateCriteria("t.EmployeesTours", "et", JoinType.InnerJoin)
                    .CreateCriteria("et.Employee", "e", JoinType.InnerJoin)
                    .Add(Restrictions.Eq("e.Id", employeeId))
                    .Add(Restrictions.Eq(
                        Projections.SqlFunction("date",
                            NHibernateUtil.Date,
                            Projections.Property("t.ScheduledDate")),
                        DateTime.Now.Date))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Tour>()
                ;
        }
    }
}
