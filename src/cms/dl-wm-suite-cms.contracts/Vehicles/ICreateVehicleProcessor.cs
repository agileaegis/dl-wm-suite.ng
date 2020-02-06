using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Vehicles;

namespace dl.wm.suite.cms.contracts.Vehicles
{
    public interface ICreateVehicleProcessor
    {
        Task<VehicleUiModel> CreateVehicleAsync(VehicleForCreationUiModel newVehicleUiModel);
    }
}