using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Models;

namespace dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Repositories.Contracts
{
    public interface IDeviceRepository
    {
        Task<Device> WithImei(string imei);

        Task<Device> this[string imei] { get; }

        void Add(Device device);

        Task<bool> Exists(string imei);
    }
}