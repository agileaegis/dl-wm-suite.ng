using dl.wm.suite.cms.contracts.Devices.DeviceModels;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IDeviceModelsControllerDependencyBlock
    {
        ICreateDeviceModelProcessor CreateDeviceModelProcessor { get; }
        IInquiryDeviceModelProcessor InquiryDeviceModelProcessor { get; }
        IUpdateDeviceModelProcessor UpdateDeviceModelProcessor { get; }
        IInquiryAllDeviceModelsProcessor InquiryAllDeviceModelsProcessor { get; }
        IDeleteDeviceModelProcessor DeleteDeviceModelProcessor { get; }
    }
}