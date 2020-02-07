using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Dashboards;

namespace dl.wm.models.DTOs.Maps
{
    public class GeofenceForModification
    {
        [Required]
        [Editable(true)]
        public List<MapUiModel> GeofenceMapPointForModification { get; set; }
    }
}
