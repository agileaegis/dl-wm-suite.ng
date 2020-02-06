using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Trips
{
  public class TripUiModel
  {
    [Key] public int Id { get; set; }

    public string Message { get; set; }
    [Required]
    [Editable(false)]

    public string TripCode { get; set; }
    [Required] 
    [Editable(false)] 
    public DateTime TripStartTime { get; set; }
    [Required]
    [Editable(false)]
    public DateTime TripEndTime { get; set; }
    [Required]
    [Editable(false)]
    public int TripAssetId { get; set; }
    [Required]
    [Editable(false)]
    public int TripDeviceId { get; set; }
  }
}