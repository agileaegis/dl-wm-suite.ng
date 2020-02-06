using System;
using System.Threading.Tasks;
using dl.wm.suite.fleet.contracts.Trackables;

namespace dl.wm.suite.fleet.services.Trackables
{
    public class DeleteTrackableProcessor : IDeleteTrackableProcessor
    {
        public Task DeleteTrackableAsync(Guid vehicleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
