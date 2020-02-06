using System;
using System.Threading.Tasks;

namespace dl.wm.suite.fleet.contracts.Trips
{
    public interface IDeleteTripProcessor
    {
        Task DeleteTripAsync(Guid assetToBeDeletedId);
    }
}