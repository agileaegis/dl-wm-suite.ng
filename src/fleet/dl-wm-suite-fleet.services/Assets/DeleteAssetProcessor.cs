using System;
using System.Threading.Tasks;
using dl.wm.suite.fleet.contracts.Assets;

namespace dl.wm.suite.fleet.services.Assets
{
    public class DeleteAssetProcessor : IDeleteAssetProcessor
    {
        public Task DeleteAssetAsync(Guid vehicleToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
