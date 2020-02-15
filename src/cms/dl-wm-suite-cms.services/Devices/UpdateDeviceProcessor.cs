using System;
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
      var deviceToBeProvisioned = _deviceRepository.FindBy(idDevice);
      if (deviceToBeProvisioned == null)
        throw new DeviceDoesNotExistException(idDevice);
      return deviceToBeProvisioned;
    }


    private void MakeDevicePersistent(Device deviceToBeMadePersistence)
    {
      _deviceRepository.Save(deviceToBeMadePersistence);
      _uOf.Commit();
    }

  }
}
