using System;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Devices;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments;
using dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;
using Serilog;

namespace dl.wm.suite.cms.services.Devices
{
  public class CreateDeviceProcessor : ICreateDeviceProcessor
  {
    private readonly IUnitOfWork _uOf;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateDeviceProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      IDeviceRepository deviceRepository)
    {
      _uOf = uOf;
      _deviceRepository = deviceRepository;
      _autoMapper = autoMapper;
    }

    public Task<DeviceUiModel> CreateDeviceAsync(Guid accountIdToCreateThisDevice,
      DeviceForCreationUiModel newDeviceUiModel)
    {
      var response =
        new DeviceUiModel()
        {
          Message = "START_CREATION"
        };

      if (newDeviceUiModel == null)
      {
        response.Message = "ERROR_INVALID_DEVICE_MODEL";
        return Task.Run(() => response);
      }

      var deviceToBeCreated = new Device();

      try
      {
        deviceToBeCreated.InjectWithInitialAttributes(newDeviceUiModel.DeviceImei, newDeviceUiModel.DeviceSerialNumber);
        deviceToBeCreated.InjectWithAudit(accountIdToCreateThisDevice);

        ThrowExcIfDeviceCannotBeCreated(deviceToBeCreated);
        ThrowExcIfThisDeviceAlreadyExist(deviceToBeCreated);

        var simcardToBeInjected = new Simcard();

        Log.Debug(
          $"Create Device: {newDeviceUiModel.DeviceImei}" +
          "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
          "Message: Just Before MakeItPersistence");

        MakeDevicePersistent(deviceToBeCreated);

        Log.Debug(
          $"Create Device: {newDeviceUiModel.DeviceImei}" +
          "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
          "Message: Just After MakeItPersistence");

        response = ThrowExcIfDeviceWasNotBeMadePersistent(deviceToBeCreated);
        response.Message = "SUCCESS_CREATION";
      }
      catch (InvalidDeviceException e)
      {
        response.Message = "ERROR_INVALID_DEVICE_MODEL";
        Log.Error(
          $"Create Device: {newDeviceUiModel.DeviceImei}" +
          "--CreateDevice--  @NotComplete@ [CreateDeviceProcessor]. " +
          $"Broken rules: {e.BrokenRules}");
      }
      catch (DeviceAlreadyExistsException ex)
      {
        response.Message = "ERROR_DEVICE_ALREADY_EXISTS";
        Log.Error(
          $"Create Device: {newDeviceUiModel.DeviceImei}" +
          "--CreateDevice--  @fail@ [CreateDeviceProcessor]. " +
          $"@innerfault:{ex?.Message} and {ex?.InnerException}");
      }
      catch (DeviceDoesNotExistAfterMadePersistentException exx)
      {
        response.Message = "ERROR_DEVICE_NOT_MADE_PERSISTENT";
        Log.Error(
          $"Create Device: {newDeviceUiModel.DeviceImei}" +
          "--CreateDevice--  @fail@ [CreateDeviceProcessor]." +
          $" @innerfault:{exx?.Message} and {exx?.InnerException}");
      }
      catch (Exception exxx)
      {
        response.Message = "UNKNOWN_ERROR";
        Log.Error(
          $"Create Device: {newDeviceUiModel.DeviceImei}" +
          $"--CreateDevice--  @fail@ [CreateDeviceProcessor]. " +
          $"@innerfault:{exxx.Message} and {exxx.InnerException}");
      }

      return Task.Run(() => response);
    }


    private void ThrowExcIfThisDeviceAlreadyExist(Device deviceToBeCreated)
    {
      var customerRetrieved = _deviceRepository.FindByImei(deviceToBeCreated.Imei);
      if (customerRetrieved != null)
      {
        throw new DeviceAlreadyExistsException(deviceToBeCreated.Imei, deviceToBeCreated.GetBrokenRulesAsString());
      }
    }

    private DeviceUiModel ThrowExcIfDeviceWasNotBeMadePersistent(Device deviceToBeCreated)
    {
      var retrievedDevice = _deviceRepository.FindByImei(deviceToBeCreated.Imei);
      if (retrievedDevice != null)
        return _autoMapper.Map<DeviceUiModel>(retrievedDevice);
      throw new DeviceDoesNotExistAfterMadePersistentException(deviceToBeCreated.Imei);
    }

    private void ThrowExcIfDeviceCannotBeCreated(Device deviceToBeCreated)
    {
      bool canBeCreated = !deviceToBeCreated.GetBrokenRules().Any();
      if (!canBeCreated)
        throw new InvalidDeviceException(deviceToBeCreated.GetBrokenRulesAsString());
    }

    private void MakeDevicePersistent(Device deviceToBeMadePersistence)
    {
      _deviceRepository.Save(deviceToBeMadePersistence);
      _uOf.Commit();
    }

  }
}
