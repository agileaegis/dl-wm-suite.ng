using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Devices.DeviceModels
{
    public class DeviceModelForModificationUiModel : IUiModel
    {
        [Editable(false)] public string DeviceModelValue => $"{DeviceModelBrand} -- {DeviceModelNumPlate}";

        [Editable(true)] public string DeviceModelBrand { get; set; }
        [Editable(true)] public string DeviceModelNumPlate { get; set; }
        [Editable(true)] public bool DeviceModelActive { get; set; }
        [Editable(true)] public string DeviceModelType { get; set; }
        [Editable(true)] public string DeviceModelStatus { get; set; }
        [Editable(true)] public string DeviceModelGas { get; set; }

        [Key] public Guid Id { get; set; }
        public string Message { get; set; }
    }
}