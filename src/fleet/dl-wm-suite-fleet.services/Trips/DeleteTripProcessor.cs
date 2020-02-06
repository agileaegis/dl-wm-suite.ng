using System;
using System.Threading.Tasks;
using dl.wm.suite.fleet.contracts.Trips;

namespace dl.wm.suite.fleet.services.Trips
{
    public class DeleteTripProcessor : IDeleteTripProcessor
    {
        public Task DeleteTripAsync(Guid vehicleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
