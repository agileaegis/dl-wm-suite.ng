using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Dashboards
{
    public class MapUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Message { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
    }
}