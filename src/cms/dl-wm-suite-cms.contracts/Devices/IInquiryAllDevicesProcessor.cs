using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Devices
{
    public interface IInquiryAllDevicesProcessor
    {
        Task<IList<Device>> GetAllDevicesAsync();
        Task<IList<Device>> GetActiveDevicesAsync(bool active);
        Task<PagedList<Device>> GetDevicesAsync(DevicesResourceParameters devicesResourceParameters);
    }
}