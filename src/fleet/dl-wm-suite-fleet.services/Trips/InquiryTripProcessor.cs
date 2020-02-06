using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Trips
{
    public class InquiryTripProcessor : IInquiryTripProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly ITripRepository _tripRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryTripProcessor(IAutoMapper autoMapper,
            ITripRepository tripRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _tripRepository = tripRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<TripUiModel> GetTripAsync(int id)
        {
            return Task.Run(() =>_autoMapper.Map<TripUiModel>(_tripRepository.FindBy(id)));
        }

        public Task<TripUiModel> GetTripByTrackableVendorIdAsync(string vendorId)
        {
            return Task.Run(() =>_autoMapper.Map<TripUiModel>(_tripRepository.FindOneEnabledByVendorId(vendorId)));
        }
    }
}
