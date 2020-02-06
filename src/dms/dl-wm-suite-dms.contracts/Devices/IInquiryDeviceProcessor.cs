using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;

namespace dl.wms.uite.dms.contracts.Devices
{
    public interface IInquiryDeviceProcessor
    {
        Task<DeviceUiModel> GetDeviceAsync(Guid id);
    }
}