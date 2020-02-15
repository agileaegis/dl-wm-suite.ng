using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;

namespace dl.wm.suite.cms.contracts.Devices
{
    public interface IUpdateDeviceProcessor
    {
        Task<DeviceUiModel> UpdateDeviceAsync(DeviceForModificationUiModel updatedDevice);
        Task<DeviceActivationUiModel> ActivatingDeviceAsync(Guid userAuditId, Guid id, DeviceForActivationModel deviceForActivationModel);
    }
}
