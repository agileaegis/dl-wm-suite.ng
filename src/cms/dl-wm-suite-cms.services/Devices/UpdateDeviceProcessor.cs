using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Devices;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.cms.services.Devices
{
  public class UpdateDeviceProcessor : IUpdateDeviceProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IAutoMapper _autoMapper;

    public UpdateDeviceProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDeviceRepository deviceRepository)
    {
      _uOf = uOf;
      _deviceRepository = deviceRepository;
      _autoMapper = autoMapper;
    }

    public Task<DeviceUiModel> UpdateDeviceAsync(DeviceForModificationUiModel updatedDevice)
    {
      throw new NotImplementedException();
    }

    public Task<DeviceActivationUiModel> ActivatingDeviceAsync(Guid userAuditId, Guid id,
      DeviceForActivationModel deviceForActivationModel)
    {
      var response =
        new DeviceActivationUiModel()
        {
          Message = "START_UPDATE"
        };

      if (deviceForActivationModel == null)
      {
        response.Message = "ERROR_INVALID_CONTAINER_DEVICE_PROVISIONING_MODEL";
        return Task.Run(() => response);
      }

      if (id == Guid.Empty)
      {
        response.Message = "ERROR_INVALID_DEVICE_ID";
        return Task.Run(() => response);
      }

      try
      {
        var deviceToBeActivated = ThrowExceptionIfDeviceDoesNotExist(id);

        ThrowExcIfThisDeviceHaveAlreadyActivated(deviceToBeActivated);
        ThrowExcIfDeviceCanNotAbleToBeActivated(deviceToBeActivated, deviceForActivationModel);

        deviceToBeActivated.UpdateWithAudit(userAuditId);
        deviceToBeActivated.ActivateWith(userAuditId);

        MakeDevicePersistent(deviceToBeActivated);

        response = ThrowExcIfActivationWasNotBeMadePersistent(deviceToBeActivated);
        response.Message = "SUCCESS_ACTIVATION";
      }
      catch (DeviceDoesNotExistException e)
      {
        response.Message = "ERROR_DEVICE_NOT_EXIST";
      }
      catch (InvalidDeviceException e)
      {
        response.Message = "ERROR_INVALID_DEVICE_MODEL";
      }
      catch (DeviceDoesNotExistAfterMadePersistentException e)
      {
        response.Message = "ERROR_ACTIVATION_NOT_MADE_PERSISTENT";
      }
      catch (Exception e)
      {
        response.Message = "UNKNOWN_ERROR";
      }

      return Task.Run(() => response);
    }

    public async Task StoreMeasurement(string imei, DeviceForMeasurementModel deviceForMeasurementModel)
    {
      if (string.IsNullOrEmpty(imei))
        throw new InvalidImeiForDeviceMeasurementException();

      if(deviceForMeasurementModel == null)
        throw new InvalidMeasurementModelForDeviceMeasurementException();

      try
      {
        var deviceToBeUpdatedWithMeasurement = ThrowExceptionIfDeviceDoesNotExist(imei);

        var todayMeasurement =  deviceToBeUpdatedWithMeasurement.Measurements.FirstOrDefault(m => m.CreatedDate.Date == DateTime.Now.Date);

        if (todayMeasurement == null)
        {
          todayMeasurement = new Measurement();
        }

        todayMeasurement.InjectWithInitialAttributes(
          deviceForMeasurementModel.MeasurementValueJson,
          deviceForMeasurementModel.Temperature,
          deviceForMeasurementModel.FillLevel,
          deviceForMeasurementModel.TiltX,
          deviceForMeasurementModel.TiltY,
          deviceForMeasurementModel.TiltZ,
          deviceForMeasurementModel.Light,
          deviceForMeasurementModel.Battery,
          deviceForMeasurementModel.Gps,
          deviceForMeasurementModel.NbIot,
          deviceForMeasurementModel.Distance,
          deviceForMeasurementModel.Tamper,
          deviceForMeasurementModel.BatterySafeMode,
          deviceForMeasurementModel.TemperatureEnabled,
          deviceForMeasurementModel.FillLevelEnabled,
          deviceForMeasurementModel.MagnetometerEnabled,
          deviceForMeasurementModel.TamperEnabled,
          deviceForMeasurementModel.LightEnabled,
          deviceForMeasurementModel.GpsEnabled
          );

        todayMeasurement.ModifiedWith();

        var todayLocation =  deviceToBeUpdatedWithMeasurement.Locations.FirstOrDefault(m => m.CreatedDate.Date == DateTime.Now.Date);

        if (todayLocation == null)
        {
          todayLocation = new Location();
        }

        todayLocation.InjectWithInitialAttributes(
          deviceForMeasurementModel.GeoLat,
          deviceForMeasurementModel.GeoLon,
          deviceForMeasurementModel.Altitude,
          deviceForMeasurementModel.Angle,
          deviceForMeasurementModel.Satellites,
          deviceForMeasurementModel.Speed
        );

        todayLocation.ModifiedWith();

        if (todayMeasurement.Id == Guid.Empty)
        {
          deviceToBeUpdatedWithMeasurement.InjectWithMeasurement(todayMeasurement);
        }
        if (todayLocation.Id == Guid.Empty)
        {
          deviceToBeUpdatedWithMeasurement.InjectWithLocation(todayLocation);
        }

        MakeDevicePersistent(deviceToBeUpdatedWithMeasurement);
      }
      catch (DeviceDoesNotExistException e)
      {
        throw new InvalidMeasurementException("DEVICE_DOES_NOT_EXISTS");
      }
      catch (Exception e)
      {
        throw new InvalidMeasurementException("UNKNOWN_ERROR");
      }
    }

    private DeviceActivationUiModel ThrowExcIfActivationWasNotBeMadePersistent(Device deviceToBeActivated)
    {
      return new DeviceActivationUiModel()
      {
        Message = "SUCCESS_ACTIVATION",
        DeviceId = deviceToBeActivated.Id,
        ActivationStatus = "SUCCESS_ACTIVATION"
      };
    }

    private void ThrowExcIfDeviceCanNotAbleToBeActivated(Device deviceToBeActivated, DeviceForActivationModel deviceForActivationModel)
    {
      if(deviceToBeActivated.ActivationCode != deviceForActivationModel.DeviceActivationCode)
        throw new DeviceHaveDifferentActivationCodeException(deviceToBeActivated.ActivationCode, deviceForActivationModel.DeviceActivationCode);
    }

    private void ThrowExcIfThisDeviceHaveAlreadyActivated(Device deviceToBeActivated)
    {
      if(deviceToBeActivated.IsActivated)
        throw new DeviceIsAlreadyActivatedExistException(deviceToBeActivated.Id);
    }

    private Device ThrowExceptionIfDeviceDoesNotExist(Guid idDevice)
    {
      var deviceToBeRetrieved = _deviceRepository.FindBy(idDevice);
      if (deviceToBeRetrieved == null)
        throw new DeviceDoesNotExistException(idDevice);
      return deviceToBeRetrieved;
    }

    private Device ThrowExceptionIfDeviceDoesNotExist(string imei)
    {
      var deviceToBeRetrieved = _deviceRepository.FindByImei(imei);
      if (deviceToBeRetrieved == null)
        throw new DeviceDoesNotExistException(imei);
      return deviceToBeRetrieved;
    }


    private void MakeDevicePersistent(Device deviceToBeMadePersistence)
    {
      _deviceRepository.Save(deviceToBeMadePersistence);
      _uOf.Commit();
    }

  }
}
