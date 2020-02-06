using dl.wm.suite.cms.contracts.Vehicles;

namespace dl.wm.suite.cms.contracts.V1
{
    public interface IVehiclesControllerDependencyBlock
    {
        ICreateVehicleProcessor CreateVehicleProcessor { get; }
        IInquiryVehicleProcessor InquiryVehicleProcessor { get; }
        IUpdateVehicleProcessor UpdateVehicleProcessor { get; }
        IInquiryAllVehiclesProcessor InquiryAllVehiclesProcessor { get; }
        IDeleteVehicleProcessor DeleteVehicleProcessor { get; }
    }
}