using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.repository.ContractRepositories;
using dl.wm.suite.fleet.repository.Repositories.Base;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.fleet.repository.Repositories
{
    public class AssetRepository : RepositoryBase<Asset, int>, IAssetRepository
    {
        public AssetRepository(ISession session)
            : base(session)
        {
        }

        public Asset FindByNumPlate(string numPlate)
        {
            return (Asset)
                Session.CreateCriteria(typeof(Asset))
                   .Add(Expression.Eq("Imei", numPlate))
                   .UniqueResult()
                   ;
        }

        public IList<Asset> FindAllActiveAssets()
        {
            return
                Session.CreateCriteria(typeof(Asset))
                    .Add(Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Asset>()
                ;
        }

        public QueryResult<Asset> FindAllAssetsPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Asset>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Asset>(query?.List().AsQueryable());
            }

            return new QueryResult<Asset>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public QueryResult<Asset> FindAllActiveAssetsPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Asset>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Asset>(query?.List().AsQueryable());
            }

            return new QueryResult<Asset>(query
                        .Where(mc => mc.IsActive == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public IList<Asset> FindAtLeastOneByNameOrNumPlate(string name, string numPlate)
        {
            return
                Session.CreateCriteria(typeof(Asset))
                    .Add(Restrictions.Or(
                        Restrictions.Eq("Name", name),
                        Restrictions.Eq("NumPlate", numPlate)))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Asset>()
                ;
        }

        public Asset FindOneByNameOrNumPlate(string name, string numPlate)
        {
            return
                (Asset)
                Session.CreateCriteria(typeof(Asset))
                    .Add(Restrictions.Or(
                        Restrictions.Eq("Name", name),
                        Restrictions.Eq("NumPlate", numPlate)))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .UniqueResult();
            ;
        }

        public Asset FindOneByNumPlate(string numPlate)
        {
            return (Asset)
                Session.CreateCriteria(typeof(Asset))
                    .Add(Expression.Eq("NumPlate", numPlate))
                    .UniqueResult()
                ;
        }
    }
}
