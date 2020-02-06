using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.common.dtos.Vms.Devices;

namespace dl.wm.suite.cms.services.Devices
{
    public class UpdateDeviceProcessor : IUpdateDeviceProcessor
    {
        public Task<DeviceUiModel> UpdateDeviceAsync(DeviceForModificationUiModel updatedDevice)
        {
            throw new NotImplementedException();
        }
    }
}
