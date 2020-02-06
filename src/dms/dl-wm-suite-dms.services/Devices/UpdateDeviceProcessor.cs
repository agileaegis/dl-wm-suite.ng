using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using dl.wm.suite.dms.model.Devices;
using dl.wm.suite.dms.repository.ContractRepositories;
using dl.wms.uite.dms.contracts.Devices;
using Serilog;

namespace dl.wm.suite.dms.services.Devices
{
    public class UpdateDeviceProcessor : IUpdateDeviceProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public UpdateDeviceProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDeviceRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public Task<DeviceUiModel> UpdateDeviceAsync(Guid id, DeviceForModificationUiModel updatedDevice)
        {
            var response =
                new DeviceUiModel()
                {
                    Message = "START_UPDATE"
                };

            if (updatedDevice == null)
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
                var vehicleToBeUpdated = ThrowExceptionIfDeviceDoesNotExist(id);
                _autoMapper.Map(updatedDevice, vehicleToBeUpdated);

                ThrowExcIfDeviceCanNotBeUpdated(vehicleToBeUpdated);
                ThrowExcIfThisDeviceAlreadyExist(vehicleToBeUpdated);
                MakeDevicePersistent(vehicleToBeUpdated);

                response = ThrowExcIfDeviceWasNotBeMadePersistent(vehicleToBeUpdated);
                response.Message = "SUCCESS_UPDATE";
            }
            catch (DeviceDoesNotExistException e)
            {
                response.Message = "ERROR_VEHICLE_NOT_EXIST";
                Log.Error(
                    $"Update Device: {updatedDevice.DeviceBrand}  with Plate Number:{updatedDevice.DeviceNumPlate}" +
                    "does not exist --UpdateDevice--  @NotComplete@ [UpdateDeviceProcessor]." +
                    $"\nException message:{e.Message}");
            }
            catch (InvalidDeviceException ex)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                Log.Error(
                    $"Update Device: {updatedDevice.DeviceBrand}  with Plate Number:{updatedDevice.DeviceNumPlate}" +
                    "--UpdateDevice--  @NotComplete@ [UpdateDeviceProcessor]. " +
                    $"Broken rules: {ex.BrokenRules}");
            }
            catch (DeviceDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_VEHICLE_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Device: {updatedDevice.DeviceNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateDevice--  @fail@ [CreateDeviceProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (DeviceAlreadyExistsException exx)
            {
                response.Message = "ERROR_VEHICLE_ALREADY_EXISTS";
                Log.Error(
                    $"Update Device: {updatedDevice.DeviceBrand}  with Plate Number:{updatedDevice.DeviceNumPlate}" +
                    "already exists --UpdateDevice--  @NotComplete@ [UpdateDeviceProcessor]." +
                    $"\nException message:{exx.Message}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Update Device: {updatedDevice.DeviceBrand}  with Plate Number:{updatedDevice.DeviceNumPlate}" +
                    $"unknown error. " +
                    $"Exception message: {exxx.Message} --UpdateDevice--  @NotComplete@ [UpdateDeviceProcessor].");
            }

            return Task.Run(() => response);
        }

        private void MakeDevicePersistent(Device vehicleToBeMadePersistence)
        {
            _vehicleRepository.Save(vehicleToBeMadePersistence);
            _uOf.Commit();
        }

        private Device ThrowExceptionIfDeviceDoesNotExist(Guid idDevice)
        {
            var vehicleToBeUpdated = _vehicleRepository.FindBy(idDevice);
            if (vehicleToBeUpdated == null)
                throw new DeviceDoesNotExistException(idDevice);
            return vehicleToBeUpdated;
        }

        private DeviceUiModel ThrowExcIfDeviceWasNotBeMadePersistent(Device deviceToBeCreated)
        {
            var retrievedDevice = _vehicleRepository.FindByImeiAndSerialNumber(deviceToBeCreated.Imei, deviceToBeCreated.SerialNumber);
            if (retrievedDevice != null)
                return _autoMapper.Map<DeviceUiModel>(retrievedDevice);
            throw new DeviceDoesNotExistAfterMadePersistentException(deviceToBeCreated.Imei);
        }

        private void ThrowExcIfDeviceCanNotBeUpdated(Device deviceToBeUpdated)
        {
            var canBeUpdated = !deviceToBeUpdated.GetBrokenRules().Any();
            if (!canBeUpdated)
                throw new InvalidDeviceException(deviceToBeUpdated.GetBrokenRulesAsString());
        }

        private void ThrowExcIfThisDeviceAlreadyExist(Device vehicleToBeUpdated)
        {
            var vehicle =
                _vehicleRepository.FindByImeiAndSerialNumber(vehicleToBeUpdated.Imei, vehicleToBeUpdated.SerialNumber);
            if (vehicle != null && vehicle.Id != vehicleToBeUpdated.Id)
            {
                throw new DeviceAlreadyExistsException(vehicleToBeUpdated.Imei, vehicleToBeUpdated.SerialNumber);
            }
        }
    }
}
