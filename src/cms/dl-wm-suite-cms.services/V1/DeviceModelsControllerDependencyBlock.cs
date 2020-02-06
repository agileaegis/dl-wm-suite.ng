using dl.wm.suite.cms.contracts.Devices.DeviceModels;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class DeviceModelsControllerDependencyBlock : IDeviceModelsControllerDependencyBlock
    {
        public DeviceModelsControllerDependencyBlock(ICreateDeviceModelProcessor createDeviceModelProcessor,
                                                        IInquiryDeviceModelProcessor inquiryDeviceModelProcessor,
                                                        IUpdateDeviceModelProcessor updateDeviceModelProcessor,
                                                        IInquiryAllDeviceModelsProcessor allDeviceModelProcessor,
                                                        IDeleteDeviceModelProcessor deleteDeviceModelProcessor)

        {
            CreateDeviceModelProcessor = createDeviceModelProcessor;
            InquiryDeviceModelProcessor = inquiryDeviceModelProcessor;
            UpdateDeviceModelProcessor = updateDeviceModelProcessor;
            InquiryAllDeviceModelsProcessor = allDeviceModelProcessor;
            DeleteDeviceModelProcessor = deleteDeviceModelProcessor;
        }

        public ICreateDeviceModelProcessor CreateDeviceModelProcessor { get; private set; }
        public IInquiryDeviceModelProcessor InquiryDeviceModelProcessor { get; private set; }
        public IUpdateDeviceModelProcessor UpdateDeviceModelProcessor { get; private set; }
        public IInquiryAllDeviceModelsProcessor InquiryAllDeviceModelsProcessor { get; private set; }
        public IDeleteDeviceModelProcessor DeleteDeviceModelProcessor { get; private set; }
    }
}