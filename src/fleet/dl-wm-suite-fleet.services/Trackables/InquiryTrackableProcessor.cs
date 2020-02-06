using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.fleet.contracts.Trackables;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Trackables
{
    public class InquiryTrackableProcessor : IInquiryTrackableProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly ITrackableRepository _trackableRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryTrackableProcessor(IAutoMapper autoMapper,
            ITrackableRepository trackableRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _trackableRepository = trackableRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<TrackableUiModel> GetTrackableAsync(int id)
        {
            return Task.Run(() =>_autoMapper.Map<TrackableUiModel>(_trackableRepository.FindBy(id)));
        }

        public Task<TrackableUiModel> GetTrackableByImeiAsync(string imei)
        {
            return Task.Run(() =>_autoMapper.Map<TrackableUiModel>(_trackableRepository.FindOneByVendorId(imei)));
        }
    }
}
