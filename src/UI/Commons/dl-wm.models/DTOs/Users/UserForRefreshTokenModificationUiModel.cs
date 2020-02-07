using System.ComponentModel.DataAnnotations;

namespace dl.wm.models.DTOs.Users
{
    public class UserForRefreshTokenModificationUiModel
    {
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string RefreshToken { get; set; }
    }
}