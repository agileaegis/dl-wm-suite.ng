using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices.DeviceModels;

namespace dl.wm.suite.cms.services.Devices.DeviceModels
{
    public class DeleteDeviceModelProcessor : IDeleteDeviceModelProcessor
    {
        public Task DeleteDeviceModelAsync(Guid deviceToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
