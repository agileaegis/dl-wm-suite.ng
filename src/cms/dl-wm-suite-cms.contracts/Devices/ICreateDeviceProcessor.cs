using dl.wm.suite.common.dtos.Vms.Devices;

namespace dl.wm.suite.cms.contracts.Devices
{
    public interface ICreateDeviceProcessor
    {
        DeviceUiModel CreateDeviceAsync(DeviceForCreationUiModel newDeviceUiModel);
    }
}