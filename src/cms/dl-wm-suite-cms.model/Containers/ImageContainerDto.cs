using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.cms.model.Containers
{
  public class ImageContainerDto
  {
    [Required] public bool IsStoredSuccessfully { get; set; }

    [Required] public string Path { get; set; }
    [Required] public string Message { get; set; }
  }
}