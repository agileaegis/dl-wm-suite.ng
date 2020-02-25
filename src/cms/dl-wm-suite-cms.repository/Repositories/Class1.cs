using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Tours.Trips;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;

namespace dl.wm.suite.cms.repository.Repositories
{
  public class TripRepository : RepositoryBase<Trip, Guid>, ITripRepository
  {
    public TripRepository(ISession session)
        : base(session)
    {
    }

    public QueryResult<Trip> FindAllTripsPagedOf(int? pageNum, int? pageSize)
    {
      var query = Session.QueryOver<Trip>();

      if (pageNum == -1 & pageSize == -1)
      {
        return new QueryResult<Trip>(query?.List().AsQueryable());
      }

      return new QueryResult<Trip>(query
                  .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                  .Take((int)pageSize).List().AsQueryable(),
              query.ToRowCountQuery().RowCount(),
              (int)pageSize)
          ;
    }

    public IList<Trip> FindAtLeastOneByCode(string code)
    {
      return
          Session.CreateCriteria(typeof(Trip))
              .Add(Restrictions.Eq("Code", code))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .List<Trip>()
          ;
    }

    public IList<int> FindAllTodaysTripIds()
    {



      //var query = Session.QueryOver<Trip>()
      //    .Where(t => t.CreatedDate.Date == DateTime.Now.Date)
      //    .Select(tr => tr.Id)
      //    .List<int>();

      //return query;
      return
          Session.CreateCriteria(typeof(Trip))
              .Add(Restrictions.Eq(
                      //Projections.SqlFunction("date",
                      //    NHibernateUtil.Date,

                      Projections.SqlFunction(new SQLFunctionTemplate(
                              NHibernateUtil.Date,
                              "dateadd(dd, 0, datediff(dd, 0, ?1))"),
                          NHibernateUtil.Date,
                      Projections.Property<Trip>(t => t.CreatedDate)),
                  DateTime.Now.Date))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .SetProjection(Projections.Property("Id"))
              .List<int>()
          ;
    }

    public Trip FindOneByCode(string code)
    {
      return
          (Trip)
          Session.CreateCriteria(typeof(Trip))
              .Add(Restrictions.Eq("Code", code))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .UniqueResult();
      ;
    }

    public Trip FindOneEnabledByVendorId(string vendorId)
    {
      return
          (Trip)
          Session.CreateCriteria<Trip>("t")
              .CreateCriteria("t.DeviceAsset", "da", JoinType.InnerJoin)
              .CreateCriteria("da.Device", "d", JoinType.InnerJoin)
              .Add(Restrictions.Eq("da.IsEnabled", true))
              .Add(Restrictions.Eq("d.VendorId", vendorId))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .UniqueResult();
      ;
    }
  }
}
