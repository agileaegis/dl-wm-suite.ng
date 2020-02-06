using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.services.Devices
{
    public class InquiryAllDevicesProcessor : IInquiryAllDevicesProcessor
    {
        public Task<IList<Device>> GetAllDevicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Device>> GetActiveDevicesAsync(bool active)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Device>> GetDevicesAsync(DevicesResourceParameters devicesResourceParameters)
        {
            throw new NotImplementedException();
        }
    }
}
