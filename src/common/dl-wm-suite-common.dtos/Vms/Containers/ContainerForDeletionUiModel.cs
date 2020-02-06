using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Containers
{
    public class ContainerForDeletionUiModel
    {
        [Required]
        [Editable(true)]
        public Guid Id { get; set; }
        [Required]
        [Editable(true)]
        public bool IsActive { get; set; }
        [Required]
        [Editable(true)]
        public bool DeletionStatus { get; set; }
        [Required]
        [Editable(true)]
        public string Message { get; set; }
    }
}
