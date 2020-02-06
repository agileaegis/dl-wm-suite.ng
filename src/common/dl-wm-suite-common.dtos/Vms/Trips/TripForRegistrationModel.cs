using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Trips
{
    public class TripForRegistrationModel
    {
        [Required] [Editable(false)] public string AssetNumPlate { get; set; }
        [Required] [Editable(false)] public string TrackableVendorId { get; set; }
    }
}