using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Devices
{
    public interface IDeleteDeviceProcessor
    {
        Task DeleteDeviceAsync(Guid deviceToBeDeletedId);
    }
}