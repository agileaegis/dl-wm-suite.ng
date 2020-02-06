using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Vehicles;

namespace dl.wm.suite.cms.services.Vehicles
{
    public class DeleteVehicleProcessor : IDeleteVehicleProcessor
    {
        public Task DeleteVehicleAsync(Guid vehicleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
