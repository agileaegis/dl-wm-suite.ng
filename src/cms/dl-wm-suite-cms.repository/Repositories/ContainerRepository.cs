﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Paging;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Spatial.Criterion;
using NHibernate.Spatial.Dialect;

namespace dl.wm.suite.cms.repository.Repositories
{
    public class ContainerRepository : RepositoryBase<Container, Guid>, IContainerRepository
    {
        private readonly WKTReader _wkt;

        public ContainerRepository(ISession session)
            : base(session)
        {
            _wkt = new WKTReader();
        }

        public Container FindOneByName(string name)
        {
            return (Container)
                Session.CreateCriteria(typeof(Container))
                   .Add(Restrictions.Eq("Name", name))
                   .UniqueResult()
                   ;
        }

        public QueryResult<Container> FindAllContainersPagedOf(int? pageNum, int? pageSize)
        {
            string queryStr = 
                @"select c.Id, 
                    NHSP.AsText(c.Location) 
                    from Container as c 
                    where c.Location is not null"
                ;

            IList points = Session
                .CreateQuery(queryStr)
                .List();
            
            var query = Session.QueryOver<Container>();

            if (pageNum == -1 & pageSize == -1)
            {
                var containers = query?.List();

                if (points.Count > 0)
                {
                    foreach (Object[] point in points)
                    {
                        containers.FirstOrDefault(x => x.Id == (Guid) point[0]).Location = _wkt.Read((string) point[1]);
                    }
                }

                return new QueryResult<Container>(query?.List().AsQueryable());
            }

            return new QueryResult<Container>(query
                        .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
                        .Take((int) pageSize).List().AsQueryable(),
                    query.ToRowCountQuery().RowCount(),
                    (int) pageSize)
                ;
        }

        public QueryResult<Container> FindAllContainersPagedOfByScheduledDate(DateTime scheduledDateContainer, int? pageNum, int? pageSize)
        {
            return null;
        }

        public Container FindByNameSpecifiedDate(string nameContainer, DateTime scheduledDateContainer)
        {
            return (Container)
                Session.CreateCriteria(typeof(Container))
                   .Add(Restrictions.Eq("Name", nameContainer))
                    .Add(Restrictions.Eq(
                       Projections.SqlFunction("date",
                                               NHibernateUtil.Date,
                                               Projections.Property("ScheduledDate")),
                       scheduledDateContainer.Date))
                   .UniqueResult()
                   ;
        }

        public IList<Container> FindAllByScheduledDate(DateTime scheduledDate)
        {
            return 
                Session.CreateCriteria(typeof(Container))
                .Add(Restrictions.Eq(
                   Projections.SqlFunction("date",
                                           NHibernateUtil.Date,
                                           Projections.Property("ScheduledDate")),
                   scheduledDate.Date))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never)
                .List<Container>()
               ;
        }

        public IList<Container> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate)
        {
            return
                Session.CreateCriteria(typeof(Container))
                .Add(
                    Expression.Conjunction()
                        .Add(Restrictions.Ge("ScheduledDate", startedScheduledDate))
                        .Add(Restrictions.Lt("ScheduledDate", endedScheduledDate))
                    )
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Container>()
                ;
        }
    }
}
