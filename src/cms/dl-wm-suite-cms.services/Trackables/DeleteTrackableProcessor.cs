using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Trackables;

namespace dl.wm.suite.cms.services.Trackables
{
    public class DeleteTrackableProcessor : IDeleteTrackableProcessor
    {
        public Task DeleteTrackableAsync(Guid vehicleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
