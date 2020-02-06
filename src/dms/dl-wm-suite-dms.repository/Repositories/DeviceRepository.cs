using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain.Queries;
using dl.wm.suite.dms.model.Devices;
using dl.wm.suite.dms.repository.ContractRepositories;
using dl.wm.suite.dms.repository.Repositories.Base;
using NHibernate;

namespace dl.wm.suite.dms.repository.Repositories
{
    public class DeviceRepository : RepositoryBase<Device, Guid>, IDeviceRepository
    {
        public DeviceRepository(ISession session)
            : base(session)
        {
        }

        public Device FindByImei(string imei)
        {
            throw new NotImplementedException();
        }

        public Device FindByImeiAndSerialNumber(string imei, string serialNum)
        {
            throw new NotImplementedException();
        }

        public IList<Device> FindAllActiveDevices()
        {
            throw new NotImplementedException();
        }

        public QueryResult<Device> FindAllDevicesPagedOf(int? pageNum, int? pageSize)
        {
            throw new NotImplementedException();
        }

        public QueryResult<Device> FindAllActiveDevicesPagedOf(int? pageNum, int? pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
