using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Devices.DeviceModels
{
    public class DeviceModelForCreationUiModel
    {
        [Editable(true)]
        public string DeviceModelName { get; set; }
        [Editable(true)]
        public string DeviceModelCodeName { get; set; }
    }
}