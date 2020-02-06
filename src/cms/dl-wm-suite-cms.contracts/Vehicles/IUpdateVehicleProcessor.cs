using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Vehicles;

namespace dl.wm.suite.cms.contracts.Vehicles
{
    public interface IUpdateVehicleProcessor
    {
        Task<VehicleUiModel> UpdateVehicleAsync(Guid id, VehicleForModificationUiModel updatedVehicle);
    }
}
