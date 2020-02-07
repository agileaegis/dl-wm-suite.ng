using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.models.DTOs.Users
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