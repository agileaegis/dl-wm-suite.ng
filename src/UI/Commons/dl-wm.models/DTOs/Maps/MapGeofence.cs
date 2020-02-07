using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.models.DTOs.Maps
{
    public class MapGeofence
    {
        [Required]
        [Editable(true)]
        public string type { get; set; }
        [Required]
        [Editable(true)]
        public List<Geometry> geometries { get; set; }
    }
}