using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.model.Trips;
using dl.wm.suite.fleet.repository.ContractRepositories;
using Serilog;

namespace dl.wm.suite.fleet.services.Trips
{
    public class CreateTripProcessor : ICreateTripProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ITripRepository _tripRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateTripProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ITripRepository tripRepository)
        {
            _uOf = uOf;
            _tripRepository = tripRepository;
            _autoMapper = autoMapper;
        }


        public Task<TripUiModel> CreateTripAsync(string accountEmailToCreateThisTrip,
            TripForCreationUiModel newTripUiModel)
        {
            var response =
                new TripUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newTripUiModel == null)
            {
                response.Message = "ERROR_INVALID_TRIP_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var tripToBeCreated = _autoMapper.Map<Trip>(newTripUiModel);

                ThrowExcIfTackableCannotBeCreated(tripToBeCreated);
                ThrowExcIfThisTripAlreadyExist(tripToBeCreated);

                tripToBeCreated.InjectWithAuditCreation(accountEmailToCreateThisTrip);
                tripToBeCreated.InjectWithAuditModification(accountEmailToCreateThisTrip);

                Log.Debug(
                    $"Create Trip: with Code: {newTripUiModel.TripCode}" +
                    "--CreateTrip--  @Ready@ [CreateTripProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeTripPersistent(tripToBeCreated);

                Log.Debug(
                    $"Create Trip: with Name: {newTripUiModel.TripCode}" +
                    "--CreateTrip--  @Ready@ [CreateTripProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfTripWasNotBeMadePersistent(tripToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidTripException e)
            {
                response.Message = "ERROR_INVALID_TRIP_MODEL";
                Log.Error(
                    $"Create Trip: Code: {newTripUiModel.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--CreateTrip--  @NotComplete@ [CreateTripProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (TripAlreadyExistsException ex)
            {
                response.Message = "ERROR_TRIP_ALREADY_EXISTS";
                Log.Error(
                    $"Create Trip: Code: {newTripUiModel.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--CreateTrip--  @fail@ [CreateTripProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (TripDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_TRIP_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Trip: Code: {newTripUiModel.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--CreateTrip--  @fail@ [CreateTripProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Trip: Code: {newTripUiModel.TripCode}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateTrip--  @fail@ [CreateTripProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }


            return Task.Run(() => response);
        }

        private void ThrowExcIfThisTripAlreadyExist(Trip tripToBeCreated)
        {
            var tripRetrieved =
                _tripRepository.FindAtLeastOneByCode(tripToBeCreated.Code);
            if (tripRetrieved.Count != 0)
            {
                throw new TripAlreadyExistsException(tripToBeCreated.GetBrokenRulesAsString(),
                    tripToBeCreated.Code);
            }
        }

        private TripUiModel ThrowExcIfTripWasNotBeMadePersistent(Trip tripToBeCreated)
        {
            var retrievedTrip =
                _tripRepository.FindOneByCode(tripToBeCreated.Code);
            if (retrievedTrip != null)
                return _autoMapper.Map<TripUiModel>(retrievedTrip);
            throw new TripDoesNotExistAfterMadePersistentException(tripToBeCreated.Code);
        }

        private void ThrowExcIfTackableCannotBeCreated(Trip tripToBeCreated)
        {
            bool canBeCreated = !tripToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidTripException(tripToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeTripPersistent(Trip tripToBeCreated)
        {
            _tripRepository.Save(tripToBeCreated);
            _uOf.Commit();
        }


    }
}
