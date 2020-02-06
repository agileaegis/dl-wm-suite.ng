using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices.DeviceModels;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.services.Devices.DeviceModels
{
    public class InquiryAllDeviceModelsProcessor : IInquiryAllDeviceModelsProcessor
    {
        public Task<IList<DeviceModel>> GetAllDeviceModelsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<DeviceModel>> GetActiveDeviceModelsAsync(bool active)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<DeviceModel>> GetDeviceModelsAsync(DeviceModelsResourceParameters devicesResourceParameters)
        {
            throw new NotImplementedException();
        }
    }
}
