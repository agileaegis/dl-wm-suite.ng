using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Containers
{
    public class ContainerForModificationUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }
        [Editable(true)]
        public string Message { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ContainerName { get; set; }
    }
    public class ContainerForModificationProvisioningModel
    {
        [Required]
        [Editable(true)]
        public Guid ContainerDeviceProvisioningCode { get; set; }
    }
}
