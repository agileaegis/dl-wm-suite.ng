using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices.DeviceModels;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Devices.DeviceModels;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices.DeviceModels;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Devices.DeviceModels
{
  public class CreateDeviceModelProcessor : ICreateDeviceModelProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly IDeviceModelRepository _deviceModelRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateDeviceModelProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      IDeviceModelRepository deviceModelRepository)
    {
      _uOf = uOf;
      _autoMapper = autoMapper;
      _deviceModelRepository = deviceModelRepository;
    }

    public Task<DeviceModelUiModel> CreateDeviceModelAsync(Guid accountIdToCreateThisDeviceModel,
      DeviceModelForCreationUiModel newDeviceModelUiModel)
    {
      var response =
        new DeviceModelUiModel()
        {
          Message = "START_CREATION"
        };

      if (newDeviceModelUiModel == null)
      {
        response.Message = "ERROR_INVALID_DEVICE_MODEL_MODEL";
        return Task.Run(() => response);
      }

      var deviceModelToBeCreated = new DeviceModel();

      try
      {
        deviceModelToBeCreated.InjectWithInitialAttributes(newDeviceModelUiModel.DeviceModelName, newDeviceModelUiModel.DeviceModelCodeName);
        deviceModelToBeCreated.InjectWithAudit(accountIdToCreateThisDeviceModel);

        ThrowExcIfDeviceModelCannotBeCreated(deviceModelToBeCreated);
        ThrowExcIfThisDeviceModelAlreadyExist(deviceModelToBeCreated);

        Log.Debug(
          $"Create DeviceModel: {newDeviceModelUiModel.DeviceModelName}" +
          "--CreateDeviceModel--  @NotComplete@ [CreateDeviceModelProcessor]. " +
          "Message: Just Before MakeItPersistence");

        MakeDeviceModelPersistent(deviceModelToBeCreated);

        Log.Debug(
          $"Create DeviceModel: {newDeviceModelUiModel.DeviceModelName}" +
          "--CreateDeviceModel--  @NotComplete@ [CreateDeviceModelProcessor]. " +
          "Message: Just After MakeItPersistence");

        response = ThrowExcIfDeviceModelWasNotBeMadePersistent(deviceModelToBeCreated);
        response.Message = "SUCCESS_CREATION";
      }
      catch (InvalidDeviceModelException e)
      {
        response.Message = "ERROR_INVALID_DeviceModel_MODEL";
        Log.Error(
          $"Create DeviceModel: {newDeviceModelUiModel.DeviceModelName}" +
          "--CreateDeviceModel--  @NotComplete@ [CreateDeviceModelProcessor]. " +
          $"Broken rules: {e.BrokenRules}");
      }
      catch (DeviceModelAlreadyExistsException ex)
      {
        response.Message = "ERROR_DeviceModel_ALREADY_EXISTS";
        Log.Error(
          $"Create DeviceModel: {newDeviceModelUiModel.DeviceModelName}" +
          "--CreateDeviceModel--  @fail@ [CreateDeviceModelProcessor]. " +
          $"@innerfault:{ex?.Message} and {ex?.InnerException}");
      }
      catch (DeviceModelDoesNotExistAfterMadePersistentException exx)
      {
        response.Message = "ERROR_DeviceModel_NOT_MADE_PERSISTENT";
        Log.Error(
          $"Create DeviceModel: {newDeviceModelUiModel.DeviceModelName}" +
          "--CreateDeviceModel--  @fail@ [CreateDeviceModelProcessor]." +
          $" @innerfault:{exx?.Message} and {exx?.InnerException}");
      }
      catch (Exception exxx)
      {
        response.Message = "UNKNOWN_ERROR";
        Log.Error(
          $"Create DeviceModel: {newDeviceModelUiModel.DeviceModelName}" +
          $"--CreateDeviceModel--  @fail@ [CreateDeviceModelProcessor]. " +
          $"@innerfault:{exxx.Message} and {exxx.InnerException}");
      }
      
      return Task.Run(() => response);
    }

    private void ThrowExcIfThisDeviceModelAlreadyExist(DeviceModel departmentToBeCreated)
    {
      var deviceModelRetrieved = _deviceModelRepository.FindByName(departmentToBeCreated.Name);
      if (deviceModelRetrieved != null)
      {
        throw new DeviceModelAlreadyExistsException(departmentToBeCreated.Name,
          departmentToBeCreated.GetBrokenRulesAsString());
      }
    }

    private DeviceModelUiModel ThrowExcIfDeviceModelWasNotBeMadePersistent(DeviceModel deviceModelToBeCreated)
    {
      var retrievedDeviceModel = _deviceModelRepository.FindByName(deviceModelToBeCreated.Name);
      if (retrievedDeviceModel != null)
        return _autoMapper.Map<DeviceModelUiModel>(retrievedDeviceModel);
      throw new DeviceModelDoesNotExistAfterMadePersistentException(deviceModelToBeCreated.Name);
    }

    private void ThrowExcIfDeviceModelCannotBeCreated(DeviceModel deviceModelToBeCreated)
    {
      bool canBeCreated = !deviceModelToBeCreated.GetBrokenRules().Any();
      if (!canBeCreated)
        throw new InvalidDeviceModelException(deviceModelToBeCreated.GetBrokenRulesAsString());
    }

    private void MakeDeviceModelPersistent(DeviceModel deviceModelToBeMadePersistence)
    {
      _deviceModelRepository.Save(deviceModelToBeMadePersistence);
      _uOf.Commit();
    }
  }
}
