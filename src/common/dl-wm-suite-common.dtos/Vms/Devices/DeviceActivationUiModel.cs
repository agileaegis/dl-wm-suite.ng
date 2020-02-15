using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
  public class DeviceActivationUiModel : IUiModel
  {
    [Key] public Guid Id { get; set; }
    [Editable(true)] public string Message { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public virtual string ActivationStatus { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public virtual Guid DeviceId{ get; set; }
  }
}