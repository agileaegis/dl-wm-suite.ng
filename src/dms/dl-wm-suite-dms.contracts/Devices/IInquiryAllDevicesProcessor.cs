using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.dms.model.Devices;

namespace dl.wm.suite.dms.contracts.Devices
{
    public interface IInquiryAllDevicesProcessor
    {
        Task<List<DeviceUiModel>> GetAllDevicesAsync();
        Task<List<DeviceUiModel>> GetActiveDevicesAsync(bool active);
        Task<PagedList<Device>> GetAllActivePagedDevicesAsync(DevicesResourceParameters vehiclesResourceParameters);
    }
}