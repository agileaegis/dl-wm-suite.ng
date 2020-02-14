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
    }
}
