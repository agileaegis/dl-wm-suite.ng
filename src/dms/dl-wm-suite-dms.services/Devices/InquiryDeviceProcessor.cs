using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.dms.repository.ContractRepositories;
using dl.wms.uite.dms.contracts.Devices;

namespace dl.wm.suite.dms.services.Devices
{
    public class InquiryDeviceProcessor : IInquiryDeviceProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly IDeviceRepository _vehicleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryDeviceProcessor(IAutoMapper autoMapper,
            IDeviceRepository vehicleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<DeviceUiModel> GetDeviceAsync(Guid id)
        {
            return Task.Run(() =>_autoMapper.Map<DeviceUiModel>(_vehicleRepository.FindBy(id)));
        }
    }
}
