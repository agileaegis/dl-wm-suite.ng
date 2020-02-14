using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IDeviceModelRepository : IRepository<DeviceModel, Guid>
    {
        DeviceModel FindByName(string name);
        QueryResult<DeviceModel> FindAllDeviceModelsPagedOf(int? pageNum, int? pageSize);
        QueryResult<DeviceModel> FindAllDeviceModelsPagedOfByScheduledDate(DateTime scheduledDateDeviceModel, int? pageNum, int? pageSize);
    }
}