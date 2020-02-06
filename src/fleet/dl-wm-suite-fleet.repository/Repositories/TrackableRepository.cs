using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.fleet.model.Trackables;
using dl.wm.suite.fleet.repository.ContractRepositories;
using dl.wm.suite.fleet.repository.Repositories.Base;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.fleet.repository.Repositories
{
    public class TrackableRepository : RepositoryBase<Trackable, int>, ITrackableRepository
    {
        public TrackableRepository(ISession session)
            : base(session)
        {
        }

        public Trackable FindOneByName(string name)
        {
            return (Trackable)
                Session.CreateCriteria(typeof(Trackable))
                    .Add(Expression.Eq("Name", name))
                    .UniqueResult()
                ;
        }

        public Trackable FindOneByVendorId(string vendorId)
        {
            return (Trackable)
                Session.CreateCriteria(typeof(Trackable))
                    .Add(Expression.Eq("VendorId", vendorId))
                    .UniqueResult()
                ;
        }

        public QueryResult<Trackable> FindAllTrackablesPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Trackable>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Trackable>(query?.List().AsQueryable());
            }

            return new QueryResult<Trackable>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
                        .Take((int) pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int) pageSize)
                ;
        }

        public QueryResult<Trackable> FindAllActiveTrackablesPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Trackable>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Trackable>(query?.List().AsQueryable());
            }

            return new QueryResult<Trackable>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
                        .Take((int) pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int) pageSize)
                ;
        }

        public IList<Trackable> FindAtLeastOneByImeiOrPhone(string imei, string phone)
        {
          return
            Session.CreateCriteria(typeof(Trackable))
              .Add(Restrictions.Or(
                Restrictions.Eq("Imei", imei),
                Restrictions.Eq("Phone", phone)))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .List<Trackable>()
            ;
        }

        public Trackable FindOneByImeiOrPhone(string imei, string phone)
        {
            return
                (Trackable)
                Session.CreateCriteria(typeof(Trackable))
                    .Add(Restrictions.Or(
                        Restrictions.Eq("Imei", imei),
                        Restrictions.Eq("Phone", phone)))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .UniqueResult();
                ;
        }
    }
}
