using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Containers
{
  public class ContainerUiModel : IUiModel
  {
    [Key] public Guid Id { get; set; }
    [Editable(true)] public string Message { get; set; }


    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public virtual string ContainerName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public virtual string ContainerImageName { get; set; }

    [Required] [Editable(true)] public virtual int ContainerLevel { get; set; }
    [Required] [Editable(true)] public virtual int ContainerFillLevel { get; set; }
    [Required] [Editable(true)] public virtual double ContainerLocationLat { get; set; }
    [Required] [Editable(true)] public virtual double ContainerLocationLong { get; set; }
    [Required] [Editable(true)] public virtual double ContainerTimeFull { get; set; }
    [Required] [Editable(true)] public virtual DateTime ContainerLastServicedDate { get; set; }
    [Required] [Editable(true)] public virtual DateTime ContainerCreatedDate { get; set; }
    [Required] [Editable(true)] public virtual DateTime ContainerModifiedDate { get; set; }
    [Required] [Editable(true)] public virtual Guid ContainerCreatedBy { get; set; }
    [Required] [Editable(true)] public virtual Guid ContainerModifiedBy { get; set; }
    [Required] [Editable(true)] public virtual string ContainerType { get; set; }
    [Required] [Editable(true)] public virtual string ContainerTypeValue { get; set; }
    [Required] [Editable(true)] public virtual string ContainerStatus { get; set; }
    [Required] [Editable(true)] public virtual string ContainerStatusValue { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public virtual string ContainerAddress { get; set; }

    [Required] [Editable(true)] public virtual DateTime ContainerMandatoryPickupDate { get; set; }
    [Required] [Editable(true)] public virtual bool ContainerMandatoryPickupActive { get; set; }
  }
}
