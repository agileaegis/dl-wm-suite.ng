using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Locations
{
    public class LocationrForCreationUiModel
    {
        [Required]
        [Editable(true)]
        public int ContainerFillLevel { get; set; }
        [Required]
        [Editable(true)]
        public double LocationLat { get; set; }
        [Required]
        [Editable(true)]
        public double LocationLong { get; set; }
    }
    public class LocationForCreationModel : LocationrForCreationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string LocationAddress { get; set; }
    }
}