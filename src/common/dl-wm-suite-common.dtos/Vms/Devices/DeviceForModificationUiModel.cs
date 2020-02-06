using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
    public class DeviceForModificationUiModel : IUiModel
    {
        [Editable(false)] public string DeviceValue => $"{DeviceBrand} -- {DeviceNumPlate}";

        [Editable(true)] public string DeviceBrand { get; set; }
        [Editable(true)] public string DeviceNumPlate { get; set; }
        [Editable(true)] public bool DeviceActive { get; set; }
        [Editable(true)] public string DeviceType { get; set; }
        [Editable(true)] public string DeviceStatus { get; set; }
        [Editable(true)] public string DeviceGas { get; set; }

        [Key] public Guid Id { get; set; }
        public string Message { get; set; }
    }
}