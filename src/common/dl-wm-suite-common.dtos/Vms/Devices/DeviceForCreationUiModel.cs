using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
  public class DeviceForCreationUiModel
  {
    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public string DeviceImei { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSerialNumber { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSimcardIccid { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSimcardImsi { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSimcardCountryIso { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSimcardNumber { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSimcardType { get; set; }
    [Required(AllowEmptyStrings = false)]
    [Editable(true)] public string DeviceSimcardNetworkType { get; set; }
  }
}