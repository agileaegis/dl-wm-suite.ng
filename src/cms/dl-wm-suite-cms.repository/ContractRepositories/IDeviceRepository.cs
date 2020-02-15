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
        Device FindByImei(string imei);
        Device FindBySimcardNumber(string number);
        Device FindBySimcardIccid(string iccid);
        Device FindByImeiAndSimcardIccid(string imei, string iccid);
        QueryResult<Device> FindAllDevicesPagedOf(int? pageNum, int? pageSize);
        Device FindBySimcardIccidOrImsi(string iccid, string imsi);
    }
}