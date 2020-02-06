using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices.DeviceModels;
using dl.wm.suite.common.dtos.Vms.Devices.DeviceModels;

namespace dl.wm.suite.cms.services.Devices.DeviceModels
{
    public class InquiryDeviceModelProcessor : IInquiryDeviceModelProcessor
    {
        public Task<DeviceModelUiModel> GetDeviceModelAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
