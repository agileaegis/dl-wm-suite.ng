using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Containers;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Containers;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Containers
{
  public class UpdateContainerProcessor : IUpdateContainerProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly IContainerRepository _containerRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IAutoMapper _autoMapper;

    public UpdateContainerProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IContainerRepository containerRepository, IDeviceRepository deviceRepository)
    {
      _uOf = uOf;
      _containerRepository = containerRepository;
      _deviceRepository = deviceRepository;
      _autoMapper = autoMapper;
    }

    public Task<ContainerUiModel> UpdateContainerAsync(Guid accountIdToUpdateThisContainer, Guid containerToBeModified,
      ContainerForModificationUiModel updatedContainer)
    {
      var response =
        new ContainerUiModel()
        {
          Message = "START_UPDATE"
        };

      if (updatedContainer == null)
      {
        response.Message = "ERROR_INVALID_CONTAINER_MODEL";
        return Task.Run(() => response);
      }

      if (containerToBeModified == Guid.Empty)
      {
        response.Message = "ERROR_INVALID_CONTAINER_ID";
        return Task.Run(() => response);
      }

      try
      {
        var containerToBeUpdated = ThrowExceptionIfContainerDoesNotExist(containerToBeModified);
        _autoMapper.Map(updatedContainer, containerToBeUpdated);

        ThrowExcIfContainerCanNotBeUpdated(containerToBeUpdated);
        ThrowExcIfThisContainerAlreadyExist(containerToBeUpdated);
        MakeContainerPersistent(containerToBeUpdated);

        response = ThrowExcIfContainerWasNotBeMadePersistent(containerToBeUpdated);
        response.Message = "SUCCESS_UPDATE";
      }
      catch (ContainerDoesNotExistException e)
      {
        response.Message = "ERROR_CONTAINER_NOT_EXIST";
        Log.Error(
          $"Update Container: {updatedContainer.ContainerName}" +
          "does not exist --UpdateContainer--  @NotComplete@ [UpdateContainerProcessor]." +
          $"\nException message:{e.Message}");
      }
      catch (InvalidContainerException ex)
      {
        response.Message = "ERROR_INVALID_CONTAINER_MODEL";
        Log.Error(
          $"Update Container: {updatedContainer.ContainerName}" +
          "--UpdateContainer--  @NotComplete@ [UpdateContainerProcessor]. " +
          $"Broken rules: {ex.BrokenRules}");
      }
      catch (ContainerDoesNotExistAfterMadePersistentException exx)
      {
        response.Message = "ERROR_CONTAINER_NOT_MADE_PERSISTENT";
        Log.Error(
          $"Update Container: {updatedContainer.ContainerName}" +
          $"Error Message:{response.Message}" +
          "--UpdateContainer--  @fail@ [UpdateContainerProcessor]." +
          $" @innerfault:{exx?.Message} and {exx?.InnerException}");
      }
      catch (ContainerAlreadyExistsException exx)
      {
        response.Message = "ERROR_Container_ALREADY_EXISTS";
        Log.Error(
          $"Update Container: {updatedContainer.ContainerName}" +
          "already exists --UpdateContainer--  @NotComplete@ [UpdateContainerProcessor]." +
          $"\nException message:{exx.Message}");
      }
      catch (Exception exxx)
      {
        response.Message = "UNKNOWN_ERROR";
        Log.Error(
          $"Update Container: {updatedContainer.ContainerName}" +
          $"unknown error. " +
          $"Exception message: {exxx.Message} --UpdateContainer--  @NotComplete@ [UpdateContainerProcessor].");
      }

      return Task.Run(() => response);
    }

    public Task<ContainerDeviceProvisioningUiModel> ProvisioningDeviceToContainerAsync(Guid userAuditId, Guid id,
      Guid deviceId,
      ContainerForModificationProvisioningModel containerForModificationProvisioningModel)
    {
      var response =
        new ContainerDeviceProvisioningUiModel()
        {
          Message = "START_UPDATE"
        };

      if (containerForModificationProvisioningModel == null)
      {
        response.Message = "ERROR_INVALID_CONTAINER_DEVICE_PROVISIONING_MODEL";
        return Task.Run(() => response);
      }

      if (id == Guid.Empty)
      {
        response.Message = "ERROR_INVALID_CONTAINER_ID";
        return Task.Run(() => response);
      }

      if (deviceId == Guid.Empty)
      {
        response.Message = "ERROR_INVALID_DEVICE_ID";
        return Task.Run(() => response);
      }

      try
      {
        var containerToBeUpdated = ThrowExceptionIfContainerDoesNotExist(id);
        var deviceToBeProvisioned = ThrowExceptionIfDeviceDoesNotExist(deviceId);

        ThrowExcIfContainerCanNotBeUpdated(containerToBeUpdated);
        ThrowExcIfThisContainerHaveAlreadyAnActiveDevice(containerToBeUpdated);

        ThrowExcIfDeviceCanNotAbleToBeProvisioned(deviceToBeProvisioned, containerForModificationProvisioningModel);

        containerToBeUpdated.UpdateWithAudit(userAuditId);
        deviceToBeProvisioned.UpdateWithAudit(userAuditId);
        deviceToBeProvisioned.ProvisioningWith(userAuditId);

        containerToBeUpdated.InjectWithDevice(deviceToBeProvisioned);

        MakeContainerPersistent(containerToBeUpdated);

        response = ThrowExcIfProvisioningWasNotBeMadePersistent(containerToBeUpdated);
        response.Message = "SUCCESS_PROVISIONING";
      }
      catch (ContainerDoesNotExistException e)
      {
        response.Message = "ERROR_CONTAINER_NOT_EXIST";
      }
      catch (DeviceDoesNotExistException e)
      {
        response.Message = "ERROR_DEVICE_NOT_EXIST";
      }
      catch (InvalidContainerException e)
      {
        response.Message = "ERROR_INVALID_CONTAINER_MODEL";
      }
      catch (ContainerDoesNotExistAfterMadePersistentException e)
      {
        response.Message = "ERROR_PROVISIONING_NOT_MADE_PERSISTENT";
      }
      catch (Exception e)
      {
        response.Message = "UNKNOWN_ERROR";
      }

      return Task.Run(() => response);
    }

    private ContainerDeviceProvisioningUiModel ThrowExcIfProvisioningWasNotBeMadePersistent(Container containerToHaveBeenProvisioned)
    {
      return new ContainerDeviceProvisioningUiModel()
      {
        ActivationCode = containerToHaveBeenProvisioned.Device.ActivationCode.ToString(),
        Message = "SUCCESS_PROVISIONING",
        ContainerId = containerToHaveBeenProvisioned.Id,
        DeviceId = containerToHaveBeenProvisioned.Device.Id,
        ProvisioningStatus = "SUCCESS_PROVISIONING"
      };
    }

    private void ThrowExcIfDeviceCanNotAbleToBeProvisioned(Device deviceToBeProvisioned, ContainerForModificationProvisioningModel containerForModificationProvisioningModel)
    {
      if(deviceToBeProvisioned.ProvisioningCode != containerForModificationProvisioningModel.ContainerDeviceProvisioningCode)
        throw new DeviceHaveDifferentProvisioningException(deviceToBeProvisioned.ProvisioningCode, containerForModificationProvisioningModel.ContainerDeviceProvisioningCode);
    }

    private void ThrowExcIfThisContainerHaveAlreadyAnActiveDevice(Container containerToBeUpdated)
    {
      if (containerToBeUpdated.Device != null)
      {
        if(containerToBeUpdated.Device.IsActivated || containerToBeUpdated.Device.IsEnabled)
          throw new ContainerHaveAlreadyAnAssignedActiveDeviceException(containerToBeUpdated.Name, containerToBeUpdated.Device.Imei);
      }
    }

    private void MakeContainerPersistent(Container containerToBeMadePersistence)
    {
      _containerRepository.Save(containerToBeMadePersistence);
      _uOf.Commit();
    }

    private Container ThrowExceptionIfContainerDoesNotExist(Guid idContainer)
    {
      var containerToBeUpdated = _containerRepository.FindBy(idContainer);
      if (containerToBeUpdated == null)
        throw new ContainerDoesNotExistException(idContainer);
      return containerToBeUpdated;
    }

    private Device ThrowExceptionIfDeviceDoesNotExist(Guid idDevice)
    {
      var deviceToBeProvisioned = _deviceRepository.FindBy(idDevice);
      if (deviceToBeProvisioned == null)
        throw new DeviceDoesNotExistException(idDevice);
      return deviceToBeProvisioned;
    }

    private ContainerUiModel ThrowExcIfContainerWasNotBeMadePersistent(Container containerToBeUpdated)
    {
      var retrievedContainer = _containerRepository.FindOneByName(containerToBeUpdated.Name);
      if (retrievedContainer != null)
        return _autoMapper.Map<ContainerUiModel>(retrievedContainer);
      throw new ContainerDoesNotExistAfterMadePersistentException(containerToBeUpdated.Name);
    }

    private void ThrowExcIfContainerCanNotBeUpdated(Container containerToBeUpdated)
    {
      var canBeUpdated = !containerToBeUpdated.GetBrokenRules().Any();
      if (!canBeUpdated)
        throw new InvalidContainerException(containerToBeUpdated.GetBrokenRulesAsString());
    }

    private void ThrowExcIfThisContainerAlreadyExist(Container containerToBeUpdated)
    {
      var Container =
        _containerRepository.FindOneByName(containerToBeUpdated.Name);
      if (Container != null && Container.Id != containerToBeUpdated.Id)
      {
        throw new ContainerAlreadyExistsException(containerToBeUpdated.Name);
      }
    }
  }
}
