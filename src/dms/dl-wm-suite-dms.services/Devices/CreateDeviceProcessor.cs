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
    public class CreateDeviceProcessor : ICreateDeviceProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateDeviceProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDeviceRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public Task<DeviceUiModel> CreateDeviceAsync(
            DeviceForCreationUiModel newDeviceUiModel)
        {
            var response =
                new DeviceUiModel()
                {
                    Message = "START_CREATION"
                };

            if (newDeviceUiModel == null)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                return Task.Run(() => response);
            }

            try
            {

                var deviceToBeCreated = new Device();
                deviceToBeCreated = _autoMapper.Map<Device>(newDeviceUiModel);

                ThrowExcIfDeviceCannotBeCreated(deviceToBeCreated);
                ThrowExcIfThisDeviceAlreadyExist(deviceToBeCreated);

                Log.Debug(
                    $"Create Device: {newDeviceUiModel.DeviceNumPlate}" +
                    "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
                    "Message: Just Before MakeItPersistence");

                MakeDevicePersistent(deviceToBeCreated);

                Log.Debug(
                    $"Create Device: {newDeviceUiModel.DeviceNumPlate}" +
                    "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
                    "Message: Just After MakeItPersistence");
                response = ThrowExcIfDeviceWasNotBeMadePersistent(deviceToBeCreated);
                response.Message = "SUCCESS_CREATION";
            }
            catch (InvalidDeviceException e)
            {
                response.Message = "ERROR_INVALID_VEHICLE_MODEL";
                Log.Error(
                    $"Create Device: {newDeviceUiModel.DeviceNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
                    $"Broken rules: {e.BrokenRules}");
            }
            catch (DeviceAlreadyExistsException ex)
            {
                response.Message = "ERROR_VEHICLE_ALREADY_EXISTS";
                Log.Error(
                    $"Create Device: {newDeviceUiModel.DeviceNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateDevice--  @fail@ [CreateDeviceProcessor]. " +
                    $"@innerfault:{ex?.Message} and {ex?.InnerException}");
            }
            catch (DeviceDoesNotExistAfterMadePersistentException exx)
            {
                response.Message = "ERROR_VEHICLE_NOT_MADE_PERSISTENT";
                Log.Error(
                    $"Create Device: {newDeviceUiModel.DeviceNumPlate}" +
                    $"Error Message:{response.Message}" +
                    "--CreateDevice--  @fail@ [CreateDeviceProcessor]." +
                    $" @innerfault:{exx?.Message} and {exx?.InnerException}");
            }
            catch (Exception exxx)
            {
                response.Message = "UNKNOWN_ERROR";
                Log.Error(
                    $"Create Device: {newDeviceUiModel.DeviceNumPlate}" +
                    $"Error Message:{response.Message}" +
                    $"--CreateDevice--  @fail@ [CreateDeviceProcessor]. " +
                    $"@innerfault:{exxx.Message} and {exxx.InnerException}");
            }

            return Task.Run(() => response);
        }

        private void ThrowExcIfThisDeviceAlreadyExist(Device deviceToBeCreated)
        {
            var deviceRetrieved = _vehicleRepository.FindByImeiAndSerialNumber(deviceToBeCreated.Imei, deviceToBeCreated.SerialNumber);
            if (deviceRetrieved != null)
            {
                throw new DeviceAlreadyExistsException(deviceToBeCreated.Imei,
                    deviceToBeCreated.GetBrokenRulesAsString());
            }
        }

        private DeviceUiModel ThrowExcIfDeviceWasNotBeMadePersistent(Device deviceToBeCreated)
        {
            var retrievedDevice = _vehicleRepository.FindByImeiAndSerialNumber(deviceToBeCreated.Imei, deviceToBeCreated.SerialNumber);
            if (retrievedDevice  != null)
                return _autoMapper.Map<DeviceUiModel>(retrievedDevice);
            throw new DeviceDoesNotExistAfterMadePersistentException(deviceToBeCreated.Imei);
        }

        private void ThrowExcIfDeviceCannotBeCreated(Device vehicleToBeCreated)
        {
            bool canBeCreated = !vehicleToBeCreated.GetBrokenRules().Any();
            if (!canBeCreated)
                throw new InvalidDeviceException(vehicleToBeCreated.GetBrokenRulesAsString());
        }

        private void MakeDevicePersistent(Device vehicleToBeMadePersistence)
        {
            _vehicleRepository.Save(vehicleToBeMadePersistence);
            _uOf.Commit();
        }
    }
}
