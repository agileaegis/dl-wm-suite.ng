using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.contracts.Trackables;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Trackables
{
    public class UpdateTrackableProcessor : IUpdateTrackableProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ITrackableRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public UpdateTrackableProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ITrackableRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public Task<TrackableUiModel> UpdateTrackableAsync(Guid id, TrackableForModificationUiModel updatedTrackable)
        {
            throw new NotImplementedException();
        }
    }
}
