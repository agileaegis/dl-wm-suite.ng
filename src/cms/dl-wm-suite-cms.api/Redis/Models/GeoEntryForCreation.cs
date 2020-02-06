using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.cms.api.Redis.Models
{
    public class MunicipalityModel 
    {
        [Required]
        [Editable(true)]
        public Guid Id { get; set; }

        [Required]
        [Editable(true)]
        public string Name { get; set; }
    }
    public class MapUiModel : IUiModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }
        [Required]
        [Editable(true)]
        public double Latitude { get; set; }
        [Required]
        [Editable(true)]
        public double Longitude { get; set; }
        public string Name { get; set; }
    }

    public class GeofenceForModification
    {
        [Required]
        [Editable(true)]
        public List<MapUiModel> GeofenceMapPointForModification { get; set; }
    }
    public class GeofenceForCreation
    {
        [Required]
        [Editable(false)]
        public List<GeoEntryUiModel> GeoPoints { get; set; }
        [Required]
        [Editable(false)]
        public string PointId { get; set; }
    }
    public class GeoEntryForCreation
    {
        [Required]
        [Editable(false)]
        public GeoPointUiModel GeoPoint { get; set; }
        [Required]
        [Editable(false)]
        public string PointId { get; set; }
    }
    public class GeoPointUiModel
    {
        [Required]
        [Editable(false)]
        public double Lat { get; set; }
        [Required]
        [Editable(false)]
        public double Long { get; set; }
    }
    public class GeoEntryUiModel
    {
        [Required]
        [Editable(false)]
        public double Lat { get; set; }
        [Required]
        [Editable(false)]
        public double Long { get; set; }
        [Required]
        [Editable(false)]
        public string Name { get; set; }
    }
}
