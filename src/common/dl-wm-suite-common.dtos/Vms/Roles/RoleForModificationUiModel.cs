using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Roles
{
    public class RoleForModificationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedName { get; set; }
    }
}
