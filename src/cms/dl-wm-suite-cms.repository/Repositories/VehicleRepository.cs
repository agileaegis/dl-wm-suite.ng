using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.cms.repository.Repositories
{
    public class VehicleRepository : RepositoryBase<Vehicle, Guid>, IVehicleRepository
    {
        public VehicleRepository(ISession session)
            : base(session)
        {
        }

        public Vehicle FindByNumPlate(string numPlate)
        {
            return (Vehicle)
                Session.CreateCriteria(typeof(Vehicle))
                   .Add(Expression.Eq("NumPlate", numPlate))
                   .UniqueResult()
                   ;
        }

        public IList<Vehicle> FindAllActiveVehicles()
        {
            return
                Session.CreateCriteria(typeof(Vehicle))
                    .Add(Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Vehicle>()
                ;
        }

        public QueryResult<Vehicle> FindAllVehiclesPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Vehicle>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Vehicle>(query?.List().AsQueryable());
            }

            return new QueryResult<Vehicle>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public QueryResult<Vehicle> FindAllActiveVehiclesPagedOf(int? pageNum, int? pageSize)
        {
            var query = Session.QueryOver<Vehicle>();

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<Vehicle>(query?.List().AsQueryable());
            }

            return new QueryResult<Vehicle>(query
                        .Where(mc => mc.IsActive == true)
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                        .Take((int)pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int)pageSize)
                ;
        }

        public Vehicle FindByBrandAndNumPlate(string brand, string numPlate)
        {
            return (Vehicle)
                Session.CreateCriteria(typeof(Vehicle))
                   .Add(Expression.Eq("Brand", brand))
                   .Add(Expression.Eq("NumPlate", numPlate))
                   .UniqueResult()
                   ;
        }
    }
}
