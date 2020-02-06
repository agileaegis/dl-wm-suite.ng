using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Roles
{
    public class RoleForCreationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
    }
}