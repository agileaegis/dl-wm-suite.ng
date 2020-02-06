using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Cassandra.Models;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Repositories.Contracts
{
    public interface IDeviceCassandraRepository

    {
        Task CreateKeySpaceDevice();
        Task CreateTableDevice();
        Task<Device> AddDevice(Device device);
        Task<Device> UpdateDevice(Device device);
        Task DeleteDevice(string id);
        Task<Device> GetSingleDevice(string id);
        Task<Device> GetSingleDeviceByImei(string imei);
        Task<IEnumerable<Device>> GetAllDevices();
    }
}