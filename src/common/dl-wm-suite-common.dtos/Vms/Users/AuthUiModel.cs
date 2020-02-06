using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Users
{
    public class AuthUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string Token { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string RefreshToken { get; set; }
    }
}