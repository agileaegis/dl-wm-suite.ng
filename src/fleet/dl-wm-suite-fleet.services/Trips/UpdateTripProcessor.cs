using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.TrackingPoints;
using dl.wm.suite.common.dtos.Vms.Trips;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Assets;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.contracts.Trips;
using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.model.Trackables;
using dl.wm.suite.fleet.model.TrackingPoints;
using dl.wm.suite.fleet.model.Trips;
using dl.wm.suite.fleet.repository.ContractRepositories;
using NHibernate.Spatial.Linq.Functions;
using Serilog;

namespace dl.wm.suite.fleet.services.Trips
{
    public class UpdateTripProcessor : IUpdateTripProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ITripRepository _tripRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ITrackableRepository _trackableRepository; 
        private readonly IAutoMapper _autoMapper;

        public UpdateTripProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, ITripRepository tripRepository, IAssetRepository assetRepository, ITrackableRepository trackableRepository)
        {
            _uOf = uOf;
            _tripRepository = tripRepository;
            _assetRepository = assetRepository;
            _trackableRepository = trackableRepository;
            _autoMapper = autoMapper;
        }

        public Task<TripUiModel> UpdateTripAsync(string accountEmailToUpdateThisTrip, TripForModificationUiModel updatedTrip)
        {
            var response =
                new TripUiModel()
                {
                    Message = "START_REGISTRATION"
                };

            if (updatedTrip == null)
            {
                response.Message = "ERROR_INVALID_TRIP_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var tripToBeUpdated = _tripRepository.FindOneByCode(updatedTrip.TripCode);
                if (tripToBeUpdated == null)
                    throw new TripDoesNotExistException(updatedTrip.TripCode);

                var assetToBeInjected = _assetRepository.FindOneByNumPlate(updatedTrip.TripAssetNumPlate);
                if (assetToBeInjected == null)
                    throw new AssetDoesNotExistException(updatedTrip.TripCode);

                var trackableToBeInjected = _trackableRepository.FindOneByVendorId(updatedTrip.TripTracableVendorId);
                if (trackableToBeInjected == null)
                    throw new TrackableDoesNotExistException(updatedTrip.TripTracableVendorId);

                var alreadyRegisteredTrip =
                  trackableToBeInjected.TrackableAssets.FirstOrDefault(a =>
                    a.RegisteredDate.Date == DateTime.Today && a.IsEnabled);

                if (alreadyRegisteredTrip != null)
                    throw new TrackableAlreadyRegisteredException(updatedTrip.TripTracableVendorId);

                TrackableAsset newTrackableAsset = new TrackableAsset();

                newTrackableAsset.InjectWithCreationAudit(accountEmailToUpdateThisTrip);
                newTrackableAsset.InjectWithAsset(assetToBeInjected);
                newTrackableAsset.InjectWithTrackable(trackableToBeInjected);

                tripToBeUpdated.InjectWithDeviceAsset(newTrackableAsset);

                Log.Debug(
                    $"Update Trip: with Code: {updatedTrip.TripCode}" +
                    "--UpdateTrip--  @Ready@ [UpdateTripProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeTripPersistent(tripToBeUpdated);

                Log.Debug(
                    $"Update Trip: with Name: {updatedTrip.TripCode}" +
                    "--UpdateTrip--  @Ready@ [UpdateTripProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfTripWasNotBeMadePersistent(tripToBeUpdated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (TripDoesNotExistException e)
            {
                response.Message = "ERROR_TRIP_DOES_NOT_EXIST";
                Log.Error(
                    $"Update Trip: Code: {updatedTrip.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--UpdateTrip--  @NotComplete@ [UpdateTripProcessor]. " +
                    $"@innerfault:{e?.Message} and {e?.InnerException}");
            }
            catch (AssetDoesNotExistException ex)
            {
                response.Message = "ERROR_ASSET_DOES_NOT_EXIST";
                Log.Error(
                    $"Update Trip: Code: {updatedTrip.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--UpdateTrip--  @fail@ [UpdateTripProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (TrackableDoesNotExistException ex)
            {
                response.Message = "ERROR_DEVICE_DOES_NOT_EXIST";
                Log.Error(
                    $"Update Trip: Code: {updatedTrip.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--UpdateTrip--  @fail@ [UpdateTripProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (TrackableAlreadyRegisteredException ex1)
            {
                response.Message = "ERROR_DEVICE_ALREADY_TO_A_TRIP_TODAY";
                Log.Error(
                    $"Update Trip: Code: {updatedTrip.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--UpdateTrip--  @fail@ [UpdateTripProcessor]. " +
                    $"@innerfault:{ex1?.Message} and {ex1?.InnerException}");
            }
            catch (TripDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_TRIP_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Update Trip: Code: {updatedTrip.TripCode}" +
                    $"Error Message:{response.Message}" +
                    "--UpdateTrip--  @fail@ [UpdateTripProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update Trip: Code: {updatedTrip.TripCode}" +
                    $"Error Message:{response.Message}" +
                    $"--UpdateTrip--  @fail@ [UpdateTripProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }
            return Task.Run(() => response);
        }

        public Task<TripUiModel> UnregisterTripAsync(string accountEmailToUpdateThisTrip, string trackableImei)
        {
            var response =
                new TripUiModel()
                {
                    Message = "START_UNREGISTER"
                };

            if (String.IsNullOrEmpty(trackableImei))
            {
                response.Message = "ERROR_INVALID_TRIP_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var tripToBeUnregister = _tripRepository.FindOneEnabledByVendorId(trackableImei);
                if (tripToBeUnregister == null)
                    throw new TripDoesNotExistForImeiException(trackableImei);

                tripToBeUnregister.InjectWithAuditModification(accountEmailToUpdateThisTrip);
                tripToBeUnregister.DeviceAsset.UnregisterDeviceTrip();

                Log.Debug(
                    $"Update Trip: with Imei: {trackableImei}" +
                    "--UnregisterTrip--  @Ready@ [UpdateTripProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeTripPersistent(tripToBeUnregister);

                Log.Debug(
                    $"Update Trip: with Imei: {trackableImei}" +
                    "--UnregisterTrip--  @Ready@ [UpdateTripProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfTripWasNotBeMadePersistent(tripToBeUnregister);
                response.Message = "SUCCESS_CREATION";
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update Trip: Imei: {trackableImei}" +
                    $"Error Message:{response.Message}" +
                    $"--UnregisterTrip--  @fail@ [UpdateTripProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);

        }

        public Task<TripUiModel> CreateTrtackingPoints(string accountEmailToUpdateThisTrip, int tripId, TrackingPointDto[] points)
        {
            throw new NotImplementedException();
        }

        public Task<TripUiModel> CreateTrtackingPoint(string accountEmailToUpdateThisTrip, int tripId,
            TrackingPointDto point)
        {
            var response =
                new TripUiModel()
                {
                    Message = "START_UNREGISTER"
                };

            if (tripId <= 0 || point == null)
            {
                response.Message = "ERROR_INVALID_TRIP_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var tripToBeUpdated = _tripRepository.FindBy(tripId);
                if (tripToBeUpdated == null)
                    throw new TripDoesNotExistException(tripId);


                tripToBeUpdated.InjectWithAuditModification(accountEmailToUpdateThisTrip);

                Location newLocationToBeAdded = new Location();

                newLocationToBeAdded.InjectWithLocation(point.Latitude, point.Longitude);

                TrackingPoint newTrackingPointToBeAdded = new TrackingPoint()
                {
                    Accuracy = point.Accuracy,
                    Altitude = point.Altitude,
                    Course = point.Course,
                    Speed = point.Speed,
                    LocationProvider = point.LocationProvider.ToString(),
                    Provider = point.Provider,
                };
                
                newTrackingPointToBeAdded.InjectWithLocation(newLocationToBeAdded);

                tripToBeUpdated.InjectWithTrackingPoint(newTrackingPointToBeAdded);

                Log.Debug(
                    $"Update Trip: with Id: {tripId}" +
                    "--CreateTrackingPoint--  @Ready@ [UpdateTripProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeTripPersistent(tripToBeUpdated);

                Log.Debug(
                    $"Update Trip: with Id: {tripId}" +
                    "--CreateTrackingPoint--  @Ready@ [UpdateTripProcessor]. " +
                    "Message: Just After MakeItPersistence");

                response = ThrowExcIfTripWasNotBeMadePersistent(tripToBeUpdated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update Trip: Id: {tripId}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateTrackingPoint--  @fail@ [UpdateTripProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private TripUiModel ThrowExcIfTripWasNotBeMadePersistent(Trip tripToBeUpdated)
        {
            var retrievedTrip =
                _tripRepository.FindOneByCode(tripToBeUpdated.Code);
            if (retrievedTrip != null)
                return _autoMapper.Map<TripUiModel>(retrievedTrip);
            throw new TripDoesNotExistAfterMadePersistentException(tripToBeUpdated.Code);
        }


        private void MakeTripPersistent(Trip tripToBeUpdated)
        {
            _tripRepository.Save(tripToBeUpdated);
            _uOf.Commit();
        }
    }
}
