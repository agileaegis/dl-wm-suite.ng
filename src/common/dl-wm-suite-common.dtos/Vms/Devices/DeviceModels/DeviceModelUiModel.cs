using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Devices.DeviceModels
{
    public class DeviceModelUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Message { get; set; }

        [Editable(false)]
        public string DeviceModelValue => $"{DeviceModelBrand} -- {DeviceModelNumPlate}";

        [Editable(true)]
        public string DeviceModelBrand { get; set; }
        [Editable(true)]
        public string DeviceModelNumPlate { get; set; }
        [Editable(true)]
        public bool DeviceModelActive { get; set; }
        [Editable(true)]
        public int DeviceModelType { get; set; }
        [Editable(true)]
        public int DeviceModelStatus { get; set; }
        [Editable(true)]
        public int DeviceModelGas { get; set; }
        [Editable(true)]
        public DateTime DeviceModelRegisteredDate{ get; set; }

    }
}