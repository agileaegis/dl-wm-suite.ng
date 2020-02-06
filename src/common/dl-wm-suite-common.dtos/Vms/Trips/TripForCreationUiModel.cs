using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Trips
{
    public class TripForCreationUiModel
    {
        [Required] [Editable(false)] public string TripCode { get; set; }
    }
}