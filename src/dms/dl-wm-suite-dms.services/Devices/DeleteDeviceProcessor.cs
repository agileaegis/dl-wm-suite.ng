using System;
using System.Threading.Tasks;
using dl.wms.uite.dms.contracts.Devices;

namespace dl.wm.suite.dms.services.Devices
{
    public class DeleteDeviceProcessor : IDeleteDeviceProcessor
    {
        public Task DeleteDeviceAsync(Guid vehicleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
