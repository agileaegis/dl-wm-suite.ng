using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Containers
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
}
