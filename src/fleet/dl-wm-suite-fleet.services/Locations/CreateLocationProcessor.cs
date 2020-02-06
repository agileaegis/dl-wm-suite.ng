using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Locations;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.fleet.contracts.Locations;
using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.repository.ContractRepositories;
using Serilog;

namespace dl.wm.suite.fleet.services.Locations
{
    public class CreateLocationProcessor : ICreateLocationProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ILocationRepository _locationRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateLocationProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ILocationRepository locationRepository)
        {
            _uOf = uOf;
            _locationRepository = locationRepository;
            _autoMapper = autoMapper;
        }

        public Task<LocationUiModel> CreateLocationAsync(LocationForCreationModel newLocationUiModel)
        {
             var response =
                new VehicleUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newLocationUiModel == null)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                return null;
            }

            try
            {

                Location locationToBeCreated = new Location()
                {
                    //Address = $"Test-{Guid.NewGuid()}",
                    //InterestLevel = 1,
                    //Name = $"Test-{Guid.NewGuid()}",
                    //MinimumWaitTime = 1,
                };

                locationToBeCreated.InjectWithLocation(newLocationUiModel.LocationLat, newLocationUiModel.LocationLong);

                Log.Debug(
                    $"Create Vehicle: {newLocationUiModel.LocationAddress}" +
                    "--CreateVehicle--  @NotComplete@ [CreateVehicleProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeLocationPersistent(locationToBeCreated);

                Log.Debug(
                    $"Create Vehicle: {newLocationUiModel.LocationAddress}" +
                    "--CreateVehicle--  @NotComplete@ [CreateVehicleProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response.Message = "SUCCESS_CREATION";
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Vehicle: {newLocationUiModel.LocationAddress}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateVehicle--  @fail@ [CreateVehicleProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return null;
        }

        private void MakeLocationPersistent(Location locationToBeMadePersistence)
        {
            _locationRepository.Save(locationToBeMadePersistence);
            _uOf.Commit();
        }
    }
}
