using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.cms.repository.Repositories.Base;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using NHibernate;
using NHibernate.Criterion;

namespace dl.wm.suite.cms.repository.Repositories
{
    public class DeviceModelRepository : RepositoryBase<DeviceModel, Guid>, IDeviceModelRepository
    {
        public DeviceModelRepository(ISession session)
            : base(session)
        {
        }

        public DeviceModel FindByName(string name)
        {
            return (DeviceModel)
                Session.CreateCriteria(typeof(DeviceModel))
                   .Add(Restrictions.Eq("Name", name))
                   .UniqueResult()
                   ;
        }

        public QueryResult<DeviceModel> FindAllDeviceModelsPagedOf(int? pageNum, int? pageSize)
        {
            return null;
        }

        public QueryResult<DeviceModel> FindAllDeviceModelsPagedOfByScheduledDate(DateTime scheduledDateDeviceModel, int? pageNum, int? pageSize)
        {
            return null;
        }

        public DeviceModel FindByNameSpecifiedDate(string nameDeviceModel, DateTime scheduledDateDeviceModel)
        {
            return (DeviceModel)
                Session.CreateCriteria(typeof(DeviceModel))
                   .Add(Restrictions.Eq("Name", nameDeviceModel))
                    .Add(Restrictions.Eq(
                       Projections.SqlFunction("date",
                                               NHibernateUtil.Date,
                                               Projections.Property("ScheduledDate")),
                       scheduledDateDeviceModel.Date))
                   .UniqueResult()
                   ;
        }

        public IList<DeviceModel> FindAllByScheduledDate(DateTime scheduledDate)
        {
            return 
                Session.CreateCriteria(typeof(DeviceModel))
                .Add(Restrictions.Eq(
                   Projections.SqlFunction("date",
                                           NHibernateUtil.Date,
                                           Projections.Property("ScheduledDate")),
                   scheduledDate.Date))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never)
                .List<DeviceModel>()
               ;
        }

        public IList<DeviceModel> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate)
        {
            return
                Session.CreateCriteria(typeof(DeviceModel))
                .Add(
                    Expression.Conjunction()
                        .Add(Restrictions.Ge("ScheduledDate", startedScheduledDate))
                        .Add(Restrictions.Lt("ScheduledDate", endedScheduledDate))
                    )
                    .SetCacheable(true)
                    .SetCacheMode(CacheMode.Normal)
                    .SetFlushMode(FlushMode.Never)
                    .List<DeviceModel>()
                ;
        }
    }
}
