using System;
using System.Threading.Tasks;

namespace dl.wm.suite.fleet.contracts.Assets
{
    public interface IDeleteAssetProcessor
    {
        Task DeleteAssetAsync(Guid vehicleToBeDeletedId);
    }
}