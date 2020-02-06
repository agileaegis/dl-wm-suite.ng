using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.dms.model.Devices;

namespace dl.wm.suite.dms.repository.ContractRepositories
{
    public interface IDeviceRepository : IRepository<Device, Guid>
    {
        Device FindByImei(string imei);

        Device FindByImeiAndSerialNumber(string imei, string serialNum);
        IList<Device> FindAllActiveDevices();

        QueryResult<Device> FindAllDevicesPagedOf(int? pageNum = -1, int? pageSize = -1);
        QueryResult<Device> FindAllActiveDevicesPagedOf(int? pageNum = -1, int? pageSize = -1);
    }
}
