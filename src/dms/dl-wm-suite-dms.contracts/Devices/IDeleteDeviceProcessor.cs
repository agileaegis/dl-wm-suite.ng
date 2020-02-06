using System;
using System.Threading.Tasks;

namespace dl.wms.uite.dms.contracts.Devices
{
    public interface IDeleteDeviceProcessor
    {
        Task DeleteDeviceAsync(Guid vehicleToBeDeletedId);
    }
}