using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Devices.DeviceModels
{
    public interface IInquiryAllDeviceModelsProcessor
    {
        Task<IList<DeviceModel>> GetAllDeviceModelsAsync();
        Task<IList<DeviceModel>> GetActiveDeviceModelsAsync(bool active);
        Task<PagedList<DeviceModel>> GetDeviceModelsAsync(DeviceModelsResourceParameters devicesResourceParameters);
    }
}