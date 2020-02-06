using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;

namespace dl.wms.uite.dms.contracts.Devices
{
    public interface ICreateDeviceProcessor
    {
        Task<DeviceUiModel> CreateDeviceAsync(DeviceForCreationUiModel newDeviceUiModel);
    }
}