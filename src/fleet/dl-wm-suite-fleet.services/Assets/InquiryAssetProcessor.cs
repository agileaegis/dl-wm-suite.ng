using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Assets
{
    public class InquiryAssetProcessor : IInquiryAssetProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly IAssetRepository _vehicleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAssetProcessor(IAutoMapper autoMapper,
            IAssetRepository vehicleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<AssetUiModel> GetAssetAsync(int id)
        {
            return Task.Run(() =>_autoMapper.Map<AssetUiModel>(_vehicleRepository.FindBy(id)));
        }
    }
}
