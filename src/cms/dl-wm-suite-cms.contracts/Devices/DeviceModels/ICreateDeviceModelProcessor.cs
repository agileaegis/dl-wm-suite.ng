using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices.DeviceModels;

namespace dl.wm.suite.cms.contracts.Devices.DeviceModels
{
  public interface ICreateDeviceModelProcessor
  {
    Task<DeviceModelUiModel> CreateDeviceModelAsync(Guid accountIdToCreateThisDeviceModel,
      DeviceModelForCreationUiModel newDeviceModelUiModel);
  }
}