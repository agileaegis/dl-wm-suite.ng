using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Trackables;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Trackables;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Trackables
{
    public class CreateTrackableProcessor : ICreateTrackableProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ITrackableRepository _trackableRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateTrackableProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ITrackableRepository trackableRepository)
        {
            _uOf = uOf;
            _trackableRepository = trackableRepository;
            _autoMapper = autoMapper;
        }

        public Task<TrackableUiModel> CreateTrackableAsync(string accountEmailToCreateThisTrackable,
            TrackableForCreationUiModel newTrackableUiModel)
        {
            var response =
                new TrackableUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newTrackableUiModel == null)
            {
                response.Message = "ERROR_INVALID_TRACKABLE_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var trackableToBeCreated = _autoMapper.Map<Trackable>(newTrackableUiModel);

                ThrowExcIfTackableCannotBeCreated(trackableToBeCreated);
                ThrowExcIfThisTrackableAlreadyExist(trackableToBeCreated);

                trackableToBeCreated.InjectWithAudit(accountEmailToCreateThisTrackable);

                Log.Debug(
                    $"Create Trackable: with Name: {newTrackableUiModel.TrackableName} and imei: {newTrackableUiModel.TrackableImei}" +
                    "--CreateTrackable--  @Ready@ [CreateTrackableProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeTrackablePersistent(trackableToBeCreated);

                Log.Debug(
                    $"Create Trackable: with Name: {newTrackableUiModel.TrackableName} and imei: {newTrackableUiModel.TrackableImei}" +
                    "--CreateTrackable--  @Ready@ [CreateTrackableProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfTrackableWasNotBeMadePersistent(trackableToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidTrackableException e)
            {
                response.Message = "ERROR_INVALID_TRACKABLE_MODEL";
                Log.Error(
                    $"Create Trackable: Imei: {newTrackableUiModel.TrackableImei} - Phone: {newTrackableUiModel.TrackablePhone}" +
                    $"Error Message:{response.Message}" +
                    "--CreateTrackable--  @NotComplete@ [CreateTrackableProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (TrackableAlreadyExistsException ex)
            {
                response.Message = "ERROR_TRACKABLE_ALREADY_EXISTS";
                Log.Error(
                    $"Create Trackable: Imei: {newTrackableUiModel.TrackableImei} - Phone: {newTrackableUiModel.TrackablePhone}" +
                    $"Error Message:{response.Message}" +
                    "--CreateTrackable--  @fail@ [CreateTrackableProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (TrackableDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_TRACKABLE_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Trackable: Imei: {newTrackableUiModel.TrackableImei} - Phone: {newTrackableUiModel.TrackablePhone}" +
                    $"Error Message:{response.Message}" +
                    "--CreateTrackable--  @fail@ [CreateTrackableProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Trackable: Imei: {newTrackableUiModel.TrackableImei} - Phone: {newTrackableUiModel.TrackablePhone}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateTrackable--  @fail@ [CreateTrackableProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void ThrowExcIfThisTrackableAlreadyExist(Trackable trackableToBeCreated)
        {
            var trackableRetrieved =
                _trackableRepository.FindAtLeastOneByImeiOrPhone(trackableToBeCreated.VendorId, trackableToBeCreated.Phone);
            if (trackableRetrieved.Count != 0)
            {
                throw new TrackableAlreadyExistsException(trackableToBeCreated.GetBrokenRulesAsString(),
                    trackableToBeCreated.VendorId);
            }
        }

        private TrackableUiModel ThrowExcIfTrackableWasNotBeMadePersistent(Trackable trackableToBeCreated)
        {
            var retrievedTrackable =
                _trackableRepository.FindOneByImeiOrPhone(trackableToBeCreated.VendorId, trackableToBeCreated.Phone);
            if (retrievedTrackable != null)
                return _autoMapper.Map<TrackableUiModel>(retrievedTrackable);
            throw new TrackableDoesNotExistAfterMadePersistentException(trackableToBeCreated.VendorId);
        }

        private void ThrowExcIfTackableCannotBeCreated(Trackable trackableToBeCreated)
        {
            bool canBeCreated = !trackableToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidTrackableException(trackableToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeTrackablePersistent(Trackable trackableToBeCreated)
        {
            _trackableRepository.Save(trackableToBeCreated);
            _uOf.Commit();
        }
    }
}
