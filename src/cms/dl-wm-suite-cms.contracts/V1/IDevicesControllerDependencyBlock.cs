using dl.wm.suite.cms.contracts.Devices;

namespace dl.wm.suite.cms.contracts.V1
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