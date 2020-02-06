using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Vehicles
{
    public class VehicleForCreationUiModel
    {

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

    }
}