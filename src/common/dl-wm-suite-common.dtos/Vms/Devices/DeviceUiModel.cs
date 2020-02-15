using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
  public class DeviceUiModel : IUiModel
  {
    [Key] public Guid Id { get; set; }

    public string Message { get; set; }

    [Editable(false)] public string DeviceValue => $"{DeviceBrand} -- {DeviceNumPlate}";

    [Editable(true)] public string DeviceBrand { get; set; }
    [Editable(true)] public string DeviceNumPlate { get; set; }
    [Editable(true)] public bool DeviceActive { get; set; }
    [Editable(true)] public int DeviceType { get; set; }
    [Editable(true)] public int DeviceStatus { get; set; }
    [Editable(true)] public int DeviceGas { get; set; }
    [Editable(true)] public DateTime DeviceRegisteredDate { get; set; }

  }
}