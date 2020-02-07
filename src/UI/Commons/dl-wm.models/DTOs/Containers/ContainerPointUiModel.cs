using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Containers
{
    public class ContainerPointUiModel : IUiModel
    {
        [Key] public Guid Id { get; set; }
        [Editable(true)] public string Message { get; set; }


        [Required] [Editable(true)] public virtual string ContainerPointType { get; set; }
        [Required] [Editable(true)] public virtual double ContainerLat { get; set; }
        [Required] [Editable(true)] public virtual double ContainerLon { get; set; }
    }
}