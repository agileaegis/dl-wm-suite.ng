using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices.DeviceModels;

namespace dl.wm.suite.cms.contracts.Devices.DeviceModels
{
    public interface IUpdateDeviceModelProcessor
    {
        Task<DeviceModelUiModel> UpdateDeviceModelAsync(DeviceModelForModificationUiModel updatedDeviceModel);
    }
}
