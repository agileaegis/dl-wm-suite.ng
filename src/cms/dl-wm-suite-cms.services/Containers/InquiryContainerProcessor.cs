using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Containers
{
    public class InquiryContainerProcessor : IInquiryContainerProcessor
    {

        private readonly IAutoMapper _autoMapper;
        private readonly IContainerRepository _containerRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryContainerProcessor(IAutoMapper autoMapper,
            IContainerRepository containerRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _containerRepository = containerRepository;
            _propertyMappingService = propertyMappingService;
        }


        public Task<ContainerUiModel> GetContainerAsync(Guid id)
        {
            return Task.Run(() =>_autoMapper.Map<ContainerUiModel>(_containerRepository.FindOneBy(id)));
        }
    }
}