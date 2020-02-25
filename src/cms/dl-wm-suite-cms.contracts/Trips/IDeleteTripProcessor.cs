using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Trips
{
    public interface IDeleteTripProcessor
    {
        Task DeleteTripAsync(Guid tripToBeDeletedId);
    }
}