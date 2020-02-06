using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Locations;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.fleet.contracts.Locations;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Locations
{
    public class InquiryLocationProcessor : IInquiryLocationProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly ILocationRepository _locationRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryLocationProcessor(IAutoMapper autoMapper,
            ILocationRepository locationRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _locationRepository = locationRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<LocationUiModel> GetLocationAsync(int id)
        {
            var x = _locationRepository.FindPoint(id);



            return null;
            //return Task.Run(() =>_autoMapper.Map<LocationUiModel>(_locationRepository.FindBy(id)));
        }
    }
}
