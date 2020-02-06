using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Assets
{
  public class AssetForCreationUiModel
  {
    [Required]
    [Editable(true)]
    public string AssetName { get; set; }
    [Required]
    [Editable(true)]
    public string AssetNumPlate { get; set; }
    [Required]
    [Editable(true)]
    public string AssetType { get; set; }
    [Editable(true)]
    public double AssetHeight { get; set; }
    [Editable(true)]
    public double AssetWidth { get; set; }
    [Editable(true)]
    public double AssetLength { get; set; }
    [Editable(true)]
    public double AssetWeight { get; set; }
    [Editable(true)]
    public int AssetAxels { get; set; }
    [Editable(true)]
    public int AssetTrailers { get; set; }
    [Required]
    [Editable(true)]
    public bool AssetIsSemi { get; set; }
    [Editable(true)]
    public double AssetMaxGradient { get; set; }
    [Editable(true)]
    public double AssetMinTurnRadius { get; set; }
  }
}