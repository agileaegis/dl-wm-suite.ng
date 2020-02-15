using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
  public class DeviceForActivationModel
  {
    [Required]
    [Editable(true)] public Guid DeviceActivationCode { get; set; }
  }
}