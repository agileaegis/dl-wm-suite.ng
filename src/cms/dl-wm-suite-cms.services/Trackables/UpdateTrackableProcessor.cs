using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Trackables;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Trackables
{
    public class UpdateTrackableProcessor : IUpdateTrackableProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ITrackableRepository _trackableRepository;
        private readonly IAutoMapper _autoMapper;

        public UpdateTrackableProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ITrackableRepository trackableRepository)
        {
            _uOf = uOf;
            _trackableRepository = trackableRepository;
            _autoMapper = autoMapper;
        }

        public Task<TrackableUiModel> UpdateTrackableAsync(Guid id, TrackableForModificationUiModel updatedTrackable)
        {
            throw new NotImplementedException();
        }
    }
}
