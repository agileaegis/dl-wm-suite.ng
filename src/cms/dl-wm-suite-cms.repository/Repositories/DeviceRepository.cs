using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dl.wm.suite.cms.repository.Repositories
{
    public class DeviceRepository : RepositoryBase<Device, Guid>, IDeviceRepository
    {
        public DeviceRepository(ISession session)
            : base(session)
        {
        }

        public Device FindByName(string name)
        {
            return (Device)
                Session.CreateCriteria(typeof(Device))
                   .Add(Restrictions.Eq("Name", name))
                   .UniqueResult()
                   ;
        }

        public Device FindByImei(string imei)
        {
          return (Device)
            Session.CreateCriteria(typeof(Device))
              .Add(Restrictions.Eq("Imei", imei))
              .UniqueResult()
            ;
        }

        public Device FindBySimcardNumber(string number)
        {
          return
            (Device)
            Session.CreateCriteria(typeof(Device))
              .CreateAlias("Sim", "s", JoinType.InnerJoin)
              .Add(Restrictions.Eq("s.Number", number))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .UniqueResult()
            ;
        }

        public Device FindBySimcardIccid(string iccid)
        {
          return
            (Device)
            Session.CreateCriteria(typeof(Device))
              .CreateAlias("Sim", "s", JoinType.InnerJoin)
              .Add(Restrictions.Eq("s.Iccid", iccid))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .UniqueResult()
            ;
        }

        public Device FindByImeiAndSimcardIccid(string imei, string iccid)
        {
          return
            (Device)
            Session.CreateCriteria(typeof(Device))
              .Add(Restrictions.Eq("Imei", imei))
              .CreateAlias("Sim", "s", JoinType.InnerJoin)
              .Add(Restrictions.Eq("Iccid", iccid))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .UniqueResult()
            ;
        }

        public QueryResult<Device> FindAllDevicesPagedOf(int? pageNum, int? pageSize)
        {
            return null;
        }

        public QueryResult<Device> FindAllDevicesPagedOfByScheduledDate(DateTime scheduledDateDevice, int? pageNum, int? pageSize)
        {
            return null;
        }

        public Device FindByNameSpecifiedDate(string nameDevice, DateTime scheduledDateDevice)
        {
            return (Device)
                Session.CreateCriteria(typeof(Device))
                   .Add(Restrictions.Eq("Name", nameDevice))
                    .Add(Restrictions.Eq(
                       Projections.SqlFunction("date",
                                               NHibernateUtil.Date,
                                               Projections.Property("ScheduledDate")),
                       scheduledDateDevice.Date))
                   .UniqueResult()
                   ;
        }

        public IList<Device> FindAllByScheduledDate(DateTime scheduledDate)
        {
            return 
                Session.CreateCriteria(typeof(Device))
                .Add(Restrictions.Eq(
                   Projections.SqlFunction("date",
                                           NHibernateUtil.Date,
                                           Projections.Property("ScheduledDate")),
                   scheduledDate.Date))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never)
                .List<Device>()
               ;
        }

        public IList<Device> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate)
        {
            return
                Session.CreateCriteria(typeof(Device))
                .Add(
                    Expression.Conjunction()
                        .Add(Restrictions.Ge("ScheduledDate", startedScheduledDate))
                        .Add(Restrictions.Lt("ScheduledDate", endedScheduledDate))
                    )
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<Device>()
                ;
        }
    }
}
