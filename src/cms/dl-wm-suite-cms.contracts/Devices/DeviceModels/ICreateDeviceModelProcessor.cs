using dl.wm.suite.common.dtos.Vms.Devices.DeviceModels;

namespace dl.wm.suite.cms.contracts.Devices.DeviceModels
{
    public interface ICreateDeviceModelProcessor
    {
        DeviceModelUiModel CreateDeviceModelAsync(DeviceModelForCreationUiModel newDeviceModelUiModel);
    }
}