using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Devices.DeviceModels
{
    public interface IDeleteDeviceModelProcessor
    {
        Task DeleteDeviceModelAsync(Guid deviceToBeDeletedId);
    }
}