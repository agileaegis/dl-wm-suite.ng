using dl.wm.suite.cms.contracts.Vehicles;
using dl.wm.suite.cms.contracts.V1;

namespace dl.wm.suite.cms.services.V1
{
    public class VehiclesControllerDependencyBlock : IVehiclesControllerDependencyBlock
    {
        public VehiclesControllerDependencyBlock(ICreateVehicleProcessor createVehicleProcessor,
                                                        IInquiryVehicleProcessor inquiryVehicleProcessor,
                                                        IUpdateVehicleProcessor updateVehicleProcessor,
                                                        IInquiryAllVehiclesProcessor allVehicleProcessor,
                                                        IDeleteVehicleProcessor deleteVehicleProcessor)

        {
            CreateVehicleProcessor = createVehicleProcessor;
            InquiryVehicleProcessor = inquiryVehicleProcessor;
            UpdateVehicleProcessor = updateVehicleProcessor;
            InquiryAllVehiclesProcessor = allVehicleProcessor;
            DeleteVehicleProcessor = deleteVehicleProcessor;
        }

        public ICreateVehicleProcessor CreateVehicleProcessor { get; private set; }
        public IInquiryVehicleProcessor InquiryVehicleProcessor { get; private set; }
        public IUpdateVehicleProcessor UpdateVehicleProcessor { get; private set; }
        public IInquiryAllVehiclesProcessor InquiryAllVehiclesProcessor { get; private set; }
        public IDeleteVehicleProcessor DeleteVehicleProcessor { get; private set; }
    }
}