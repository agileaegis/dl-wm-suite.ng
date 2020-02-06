using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Trips
{
  public class TripForModificationUiModel
  {
    [Key] public int Id { get; set; }
    [Required] [Editable(false)] public string TripCode { get; set; }
    [Required] [Editable(false)] public string TripAssetNumPlate { get; set; }
    [Required] [Editable(false)] public string TripTracableVendorId { get; set; }
  }
}