using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class UpdateVehicleProcessor : IUpdateVehicleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public UpdateVehicleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IVehicleRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public Task<VehicleUiModel> UpdateVehicleAsync(Guid id, VehicleForModificationUiModel updatedVehicle)
        {
            var response =
                new VehicleUiModel()
                {
                    Message = "START_UPDATE"
                };

            if (updatedVehicle == null)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                return Task.Run(() => response);
            }

            if (id == Guid.Empty)
            {
                response.Message = "ERROR_INVALID_VEHICLE_ID";
                return Task.Run(() => response);
            }

            try
            {
                var vehicleToBeUpdated = ThrowExceptionIfVehicleDoesNotExist(id);
                _autoMapper.Map(updatedVehicle, vehicleToBeUpdated);

                ThrowExcIfVehicleCanNotBeUpdated(vehicleToBeUpdated);
                ThrowExcIfThisVehicleAlreadyExist(vehicleToBeUpdated);
                MakeVehiclePersistent(vehicleToBeUpdated);

                response = ThrowExcIfVehicleWasNotBeMadePersistent(vehicleToBeUpdated);
                response.Message = "SUCCESS_UPDATE";
            }
            catch (VehicleDoesNotExistException e)
            {
                response.Message = "ERROR_VEHICLE_NOT_EXIST";
                Log.Error(
                    $"Update Vehicle: {updatedVehicle.VehicleBrand}  with Plate Number:{updatedVehicle.VehicleNumPlate}" +
                    "does not exist --UpdateVehicle--  @NotComplete@ [UpdateVehicleProcessor]." +
                    $"\nException message:{e.Message}");
            }
            catch (InvalidVehicleException ex)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                Log.Error(
                    $"Update Vehicle: {updatedVehicle.VehicleBrand}  with Plate Number:{updatedVehicle.VehicleNumPlate}" +
                    "--UpdateVehicle--  @NotComplete@ [UpdateVehicleProcessor]. " +
                    $"Broken rules: {ex.BrokenRules}");
            }
            catch (VehicleDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_VEHICLE_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Vehicle: {updatedVehicle.VehicleNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateVehicle--  @fail@ [CreateVehicleProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (VehicleAlreadyExistsException exx)
            {
                response.Message = "ERROR_VEHICLE_ALREADY_EXISTS";
                Log.Error(
                    $"Update Vehicle: {updatedVehicle.VehicleBrand}  with Plate Number:{updatedVehicle.VehicleNumPlate}" +
                    "already exists --UpdateVehicle--  @NotComplete@ [UpdateVehicleProcessor]." +
                    $"\nException message:{exx.Message}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update Vehicle: {updatedVehicle.VehicleBrand}  with Plate Number:{updatedVehicle.VehicleNumPlate}" +
                    $"unknown error. " +
                    $"Exception message: {exxx.Message} --UpdateVehicle--  @NotComplete@ [UpdateVehicleProcessor].");
            }

            return Task.Run(() => response);
        }

        private void MakeVehiclePersistent(Vehicle vehicleToBeMadePersistence)
        {
            _vehicleRepository.Save(vehicleToBeMadePersistence);
            _uOf.Commit();
        }

        private Vehicle ThrowExceptionIfVehicleDoesNotExist(Guid idVehicle)
        {
            var vehicleToBeUpdated = _vehicleRepository.FindBy(idVehicle);
            if (vehicleToBeUpdated == null)
                throw new VehicleDoesNotExistException(idVehicle);
            return vehicleToBeUpdated;
        }

        private VehicleUiModel ThrowExcIfVehicleWasNotBeMadePersistent(Vehicle vehicleToBeCreated)
        {
            var retrievedVehicle = _vehicleRepository.FindByBrandAndNumPlate(vehicleToBeCreated.Brand, vehicleToBeCreated.NumPlate);
            if (retrievedVehicle != null)
                return _autoMapper.Map<VehicleUiModel>(retrievedVehicle);
            throw new VehicleDoesNotExistAfterMadePersistentException(vehicleToBeCreated.NumPlate);
        }

        private void ThrowExcIfVehicleCanNotBeUpdated(Vehicle vehicleToBeUpdated)
        {
            var canBeUpdated = !vehicleToBeUpdated.GetBrokenRules().Any();
            if (!canBeUpdated)
                throw new InvalidVehicleException(vehicleToBeUpdated.GetBrokenRulesAsString());
        }

        private void ThrowExcIfThisVehicleAlreadyExist(Vehicle vehicleToBeUpdated)
        {
            var vehicle =
                _vehicleRepository.FindByBrandAndNumPlate(vehicleToBeUpdated.Brand, vehicleToBeUpdated.NumPlate);
            if (vehicle != null && vehicle.Id != vehicleToBeUpdated.Id)
            {
                throw new VehicleAlreadyExistsException(vehicleToBeUpdated.Brand, vehicleToBeUpdated.NumPlate);
            }
        }
    }
}
