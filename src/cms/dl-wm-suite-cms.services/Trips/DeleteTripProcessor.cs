using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Trips;

namespace dl.wm.suite.cms.services.Trips
{
    public class DeleteTripProcessor : IDeleteTripProcessor
    {
        public Task DeleteTripAsync(Guid tripToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
