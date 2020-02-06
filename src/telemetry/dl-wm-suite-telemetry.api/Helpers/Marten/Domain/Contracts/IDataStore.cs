using System;
using System.Threading.Tasks;
using dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Repositories.Contracts;

namespace dl.wm.suite.telemetry.api.Helpers.Domain.Contracts
{
    public interface IDataStore : IDisposable
    {
        IDeviceRepository DeviceRepository { get; }

        Task CommitChanges();
    }
}