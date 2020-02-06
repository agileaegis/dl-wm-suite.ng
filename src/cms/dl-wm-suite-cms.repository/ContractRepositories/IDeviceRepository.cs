using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;

namespace dl.wm.suite.cms.repository.ContractRepositories
{
    public interface IDeviceRepository : IRepository<Device, Guid>
    {
        Device FindByName(string name);
        QueryResult<Device> FindAllDevicesPagedOf(int? pageNum, int? pageSize);
        QueryResult<Device> FindAllDevicesPagedOfByScheduledDate(DateTime scheduledDateDevice, int? pageNum, int? pageSize);
        Device FindByNameSpecifiedDate(string nameDevice, DateTime scheduledDateDevice);
        IList<Device> FindAllByScheduledDate(DateTime scheduledDate);
        IList<Device> FindAllBetweenScheduledDate(DateTime startedScheduledDate, DateTime endedScheduledDate);
    }
}