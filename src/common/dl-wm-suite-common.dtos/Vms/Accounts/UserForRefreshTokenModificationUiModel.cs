using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Accounts
{
    public class UserForRefreshTokenModificationUiModel
    {
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string RefreshToken { get; set; }
    }
}