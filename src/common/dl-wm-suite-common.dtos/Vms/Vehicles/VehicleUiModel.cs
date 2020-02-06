using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Vehicles
{
    public class VehicleUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }

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
        public int VehicleType { get; set; }
        [Editable(true)]
        public int VehicleStatus { get; set; }
        [Editable(true)]
        public int VehicleGas { get; set; }
        [Editable(true)]
        public DateTime VehicleRegisteredDate{ get; set; }

    }
}