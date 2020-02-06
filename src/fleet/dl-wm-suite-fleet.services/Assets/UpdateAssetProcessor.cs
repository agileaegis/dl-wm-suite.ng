using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Assets
{
    public class UpdateAssetProcessor : IUpdateAssetProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IAssetRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public UpdateAssetProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IAssetRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public Task<AssetUiModel> UpdateAssetAsync(Guid id, AssetForModificationUiModel updatedAsset)
        {
            throw new NotImplementedException();
        }
    }
}
