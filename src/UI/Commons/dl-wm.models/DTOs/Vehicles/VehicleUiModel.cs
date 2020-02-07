using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Vehicles
{
    public class VehicleUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }

        [Editable(false)]
        public string Message { get; set; }

        [Editable(false)]
        public string VehicleValue => $"{VehicleBrand} -- {VehicleNumPlate}";

        [Editable(true)]
        public string VehicleBrand { get; set; }
        [Editable(true)]
        public string VehicleNumPlate { get; set; }
        [Editable(true)]
        public bool VehicleActive { get; set; }
        [Editable(true)]
        public string VehicleType { get; set; }
        [Editable(true)]
        public string VehicleStatus { get; set; }
        [Editable(true)]
        public DateTime VehicleRegisteredDate{ get; set; }
        [Editable(true)]
        public string VehicleGas { get; set; }

    }
}