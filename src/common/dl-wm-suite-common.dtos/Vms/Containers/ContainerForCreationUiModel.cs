using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Containers
{
  public class ContainerForCreationUiModel
  {
    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public string ContainerName { get; set; }

    [Required] [Editable(true)] public int ContainerLevel { get; set; }
    [Required] [Editable(true)] public string ContainerFillLevel { get; set; }
    [Required] [Editable(true)] public double ContainerLat { get; set; }
    [Required] [Editable(true)] public double ContainerLong { get; set; }
    [Required] [Editable(true)] public string ContainerType { get; set; }
    [Required] [Editable(true)] public string ContainerStatus { get; set; }
    [Editable(true)] public DateTime ContainerPickupDate { get; set; }
    [Required] [Editable(true)] public string ContainerImagePath { get; set; }
    [Required] [Editable(true)] public string ContainerImageName { get; set; }
    [Editable(true)] public string ContainerPickupOption { get; set; }
    [Required] [Editable(true)] public bool ContainerPickupActive { get; set; }
  }

  public class ContainerForCreationModel : ContainerForCreationUiModel
    {
      [Required(AllowEmptyStrings = false)]
      [Editable(true)]
      public string ContainerAddress { get; set; }
    }
}
