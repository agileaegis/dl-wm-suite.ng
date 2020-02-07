using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Containers
{
    public class ContainerErrorModel
    {
        public string errorMessage { get; set; }
    }

    public class ContainerUiModel : IUiModel
    {
        [Key] public Guid Id { get; set; }
        [Editable(true)] public string Message { get; set; }


        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public virtual string ContainerName { get; set; }

        [Required] [Editable(true)] public virtual int ContainerLevel { get; set; }
        [Required] [Editable(true)] public virtual string ContainerFillLevel { get; set; }
        [Required] [Editable(true)] public virtual double ContainerLocationLat { get; set; }
        [Required] [Editable(true)] public virtual double ContainerLocationLong { get; set; }
        [Required] [Editable(true)] public virtual double ContainerTimeFull { get; set; }
        [Required] [Editable(true)] public virtual DateTime ContainerLastServicedDate { get; set; }
        [Required] [Editable(true)] public virtual DateTime ContainerCreatedDate { get; set; }
        [Required] [Editable(true)] public virtual DateTime ContainerModifiedDate { get; set; }
        [Required] [Editable(true)] public virtual Guid ContainerCreatedBy { get; set; }
        [Required] [Editable(true)] public virtual Guid ContainerModifiedBy { get; set; }
        [Required] [Editable(true)] public virtual string ContainerType { get; set; }
        [Required] [Editable(true)] public virtual string ContainerStatus { get; set; }
        [Required] [Editable(true)] public virtual string ContainerTypeValue { get; set; }
        [Required] [Editable(true)] public virtual string ContainerStatusValue { get; set; }

        [Required] [Editable(true)] public virtual string ContainerImagePath { get; set; }
        [Required] [Editable(true)] public virtual string ContainerImageName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public virtual string ContainerAddress { get; set; }

        [Editable(true)] public virtual DateTime ContainerMandatoryPickupDate { get; set; }
        [Editable(true)] public virtual string ContainerMandatoryPickupOption { get; set; }
        [Required] [Editable(true)] public virtual bool ContainerMandatoryPickupActive { get; set; }
    }
}
