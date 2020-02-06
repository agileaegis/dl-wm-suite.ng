using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Accounts
{
    public class LoginUiModel
    {
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Password { get; set; }
    }
}