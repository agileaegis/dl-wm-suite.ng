using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Vehicles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Vehicles
{
    public class InquiryVehicleProcessor : IInquiryVehicleProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryVehicleProcessor(IAutoMapper autoMapper,
            IVehicleRepository vehicleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<VehicleUiModel> GetVehicleAsync(Guid id)
        {
            return Task.Run(() =>_autoMapper.Map<VehicleUiModel>(_vehicleRepository.FindBy(id)));
        }
    }
}
