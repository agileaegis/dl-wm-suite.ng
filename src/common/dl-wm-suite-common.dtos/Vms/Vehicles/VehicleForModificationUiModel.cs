using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Vehicles
{
    public class VehicleForModificationUiModel : IUiModel
    {
        [Editable(false)] public string VehicleValue => $"{VehicleBrand} -- {VehicleNumPlate}";

        [Editable(true)] public string VehicleBrand { get; set; }
        [Editable(true)] public string VehicleNumPlate { get; set; }
        [Editable(true)] public bool VehicleActive { get; set; }
        [Editable(true)] public string VehicleType { get; set; }
        [Editable(true)] public string VehicleStatus { get; set; }
        [Editable(true)] public string VehicleGas { get; set; }

        [Key] public Guid Id { get; set; }
        public string Message { get; set; }
    }
}