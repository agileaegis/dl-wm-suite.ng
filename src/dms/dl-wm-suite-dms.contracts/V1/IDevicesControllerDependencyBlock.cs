using dl.wm.suite.dms.contracts.Devices;
using dl.wms.uite.dms.contracts.Devices;

namespace dl.wms.uite.dms.contracts.V1
{
    public interface IDevicesControllerDependencyBlock
    {
        ICreateDeviceProcessor CreateDeviceProcessor { get; }
        IInquiryDeviceProcessor InquiryDeviceProcessor { get; }
        IUpdateDeviceProcessor UpdateDeviceProcessor { get; }
        IInquiryAllDevicesProcessor InquiryAllDevicesProcessor { get; }
        IDeleteDeviceProcessor DeleteDeviceProcessor { get; }
    }
}