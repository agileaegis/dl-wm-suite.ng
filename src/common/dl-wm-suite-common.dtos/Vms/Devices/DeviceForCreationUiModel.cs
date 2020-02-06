using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
    public class DeviceForCreationUiModel
    {

        [Editable(false)]
        public string DeviceValue => $"{DeviceBrand} -- {DeviceNumPlate}";

        [Editable(true)]
        public string DeviceBrand { get; set; }
        [Editable(true)]
        public string DeviceNumPlate { get; set; }
        [Editable(true)]
        public bool DeviceActive { get; set; }
        [Editable(true)]
        public int DeviceType { get; set; }
        [Editable(true)]
        public int DeviceStatus { get; set; }
        [Editable(true)]
        public int DeviceGas { get; set; }

    }
}