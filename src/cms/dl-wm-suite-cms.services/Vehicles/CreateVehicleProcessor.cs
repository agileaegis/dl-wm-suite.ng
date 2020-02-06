using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Vehicles;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Vehicles;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Vehicles
{
    public class CreateVehicleProcessor : ICreateVehicleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateVehicleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IVehicleRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public Task<VehicleUiModel> CreateVehicleAsync(
            VehicleForCreationUiModel newVehicleUiModel)
        {
            var response =
                new VehicleUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newVehicleUiModel == null)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                return Task.Run(() => response);
            }

            try
            {
                var vehicleToBeCreated = _autoMapper.Map<Vehicle>(newVehicleUiModel);

                ThrowExcIfVehicleCannotBeCreated(vehicleToBeCreated);
                ThrowExcIfThisVehicleAlreadyExist(vehicleToBeCreated);

                Log.Debug(
                    $"Create Vehicle: {newVehicleUiModel.VehicleNumPlate}" +
                    "--CreateVehicle--  @NotComplete@ [CreateVehicleProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeVehiclePersistent(vehicleToBeCreated);

                Log.Debug(
                    $"Create Vehicle: {newVehicleUiModel.VehicleNumPlate}" +
                    "--CreateVehicle--  @NotComplete@ [CreateVehicleProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response = ThrowExcIfVehicleWasNotBeMadePersistent(vehicleToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidVehicleException e)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                Log.Error(
                    $"Create Vehicle: {newVehicleUiModel.VehicleNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateVehicle--  @NotComplete@ [CreateVehicleProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (VehicleAlreadyExistsException ex)
            {
                response.Message = "ERROR_VEHICLE_ALREADY_EXISTS";
                Log.Error(
                    $"Create Vehicle: {newVehicleUiModel.VehicleNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateVehicle--  @fail@ [CreateVehicleProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (VehicleDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_VEHICLE_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Vehicle: {newVehicleUiModel.VehicleNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateVehicle--  @fail@ [CreateVehicleProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Vehicle: {newVehicleUiModel.VehicleNumPlate}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateVehicle--  @fail@ [CreateVehicleProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void ThrowExcIfThisVehicleAlreadyExist(Vehicle vehicleToBeCreated)
        {
            var vehicleRetrieved = _vehicleRepository.FindByBrandAndNumPlate(vehicleToBeCreated.Brand, vehicleToBeCreated.NumPlate);
            if (vehicleRetrieved != null)
            {
                throw new VehicleAlreadyExistsException(vehicleToBeCreated.NumPlate,
                    vehicleToBeCreated.GetBrokenRulesAsString());
            }
        }

        private VehicleUiModel ThrowExcIfVehicleWasNotBeMadePersistent(Vehicle vehicleToBeCreated)
        {
            var retrievedVehicle = _vehicleRepository.FindByBrandAndNumPlate(vehicleToBeCreated.Brand, vehicleToBeCreated.NumPlate);
            if (retrievedVehicle  != null)
                return _autoMapper.Map<VehicleUiModel>(retrievedVehicle);
            throw new VehicleDoesNotExistAfterMadePersistentException(vehicleToBeCreated.NumPlate);
        }

        private void ThrowExcIfVehicleCannotBeCreated(Vehicle vehicleToBeCreated)
        {
            bool canBeCreated = !vehicleToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidVehicleException(vehicleToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeVehiclePersistent(Vehicle vehicleToBeMadePersistence)
        {
            _vehicleRepository.Save(vehicleToBeMadePersistence);
            _uOf.Commit();
        }
    }
}
