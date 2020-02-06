using dl.wm.suite.dms.contracts.Devices;
using dl.wms.uite.dms.contracts.Devices;
using dl.wms.uite.dms.contracts.V1;

namespace dl.wm.suite.dms.services.V1
{
    public class DevicesControllerDependencyBlock : IDevicesControllerDependencyBlock
    {
        public DevicesControllerDependencyBlock(ICreateDeviceProcessor createDeviceProcessor,
                                                        IInquiryDeviceProcessor inquiryDeviceProcessor,
                                                        IUpdateDeviceProcessor updateDeviceProcessor,
                                                        IInquiryAllDevicesProcessor allDeviceProcessor,
                                                        IDeleteDeviceProcessor deleteDeviceProcessor)

        {
            CreateDeviceProcessor = createDeviceProcessor;
            InquiryDeviceProcessor = inquiryDeviceProcessor;
            UpdateDeviceProcessor = updateDeviceProcessor;
            InquiryAllDevicesProcessor = allDeviceProcessor;
            DeleteDeviceProcessor = deleteDeviceProcessor;
        }

        public ICreateDeviceProcessor CreateDeviceProcessor { get; private set; }
        public IInquiryDeviceProcessor InquiryDeviceProcessor { get; private set; }
        public IUpdateDeviceProcessor UpdateDeviceProcessor { get; private set; }
        public IInquiryAllDevicesProcessor InquiryAllDevicesProcessor { get; private set; }
        public IDeleteDeviceProcessor DeleteDeviceProcessor { get; private set; }
    }
}