using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.common.infrastructure.Exceptions.Repositories.Containers;
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

    public List<Container> FindAllContainersPoints()
    {
      string queryStr =
          @"select c.Id, c.Name,
                    NHSP.AsText(c.Location) 
                    from Container as c 
                    where c.Location is not null"
        ;

      IList points = Session
          .CreateQuery(queryStr)
          .List();

      List<Container> containers = new List<Container>();

      if (points.Count > 0)
      {
        containers.AddRange(from object[] point in points select new Container()
        {
          Id = (Guid) point[0], Name = (string) point[1], Location = _wkt.Read((string) point[2]),
        });
      }

      return containers;
    }

    public QueryResult<Container> FindAllContainersPagedOfByScheduledDate(DateTime scheduledDateContainer, int? pageNum,
      int? pageSize)
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

    public Container FindOneBy(Guid id)
    {
      string queryStr =
          $@"select c.Id, 
                    c.Name,
                    c.FillLevel,
                    c.IsActive,
                    c.TimeFull,
                    c.LastServicedDate,
                    c.CreatedDate,
                    c.ModifiedDate,
                    c.CreatedBy,
                    c.ModifiedBy,
                    c.ImagePath,
                    c.Type,
                    c.Status,
                    c.Address,
                    c.MandatoryPickupDate,
                    c.MandatoryPickupActive,
                    NHSP.AsText(c.Location) 
                    from Container as c 
                    where c.Location is not null and c.Id = :Id and c.IsActive = :IsActive"
        ;

       IList containersRepo = Session
        .CreateQuery(queryStr)
        .SetParameter("Id", id)
        .SetParameter("IsActive", true)
        .List();


      if (containersRepo.Count > 1)
      {
        throw new MultipleContainersForAnIdException(id);
      }

      List<Container> containers = new List<Container>();

      containers.AddRange(from object[] point in containersRepo
                          select new Container()
        {
          Id = (Guid)point[0],
          Name = (string)point[1],
          FillLevel = (int)point[2],
          IsActive = (bool)point[3],
          TimeFull = (double)point[4],
          LastServicedDate = (DateTime)point[5],
          CreatedDate = (DateTime)point[6],
          ModifiedDate = (DateTime)point[7],
          CreatedBy = (Guid)point[8],
          ModifiedBy = (Guid)point[9],
          ImagePath = (string)point[10],
          Type = (ContainerType)point[11],
          Status = (ContainerStatus)point[12],
          Address = (string)point[13],
          MandatoryPickupDate = (DateTime)point[14],
          MandatoryPickupActive = (bool)point[15],
          Location = _wkt.Read((string)point[16]),
        });

      return containers.FirstOrDefault();
    }
  }
}
