using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Vehicles
{
    public interface IDeleteVehicleProcessor
    {
        Task DeleteVehicleAsync(Guid vehicleToBeDeletedId);
    }
}