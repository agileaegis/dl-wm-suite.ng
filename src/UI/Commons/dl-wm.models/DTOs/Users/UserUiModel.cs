using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Employees;
using dl.wm.models.DTOs.Base;
using dl.wm.models.DTOs.Employees;

namespace dl.wm.models.DTOs.Users
{
    public class AccountErrorModel
    {
        public string errorMessage { get; set; }
    }


    public class UserUiModel : IUiModel
    {
        public UserUiModel()
        {
            Employee = new EmployeeUiModel();
        }

        [Key]
        public Guid Id { get; set; }

        [Editable(false)]
        public string Message { get; set; }
        [Required]
        [Editable(false)]
        public EmployeeUiModel Employee { get; set; }
        [Required]
        [Editable(false)]
        public string Login { get; set; }
        [Required]
        [Editable(false)]
        public string RefreshToken { get; set; }
        [Required]
        [Editable(false)]
        public string Token { get; set; }
        [Required]
        [Editable(false)]
        public Guid UserRoleId { get; set; }
    }
    public class AccountUiModel : UserUiModel
    {
        [Required]
        [Editable(false)]
        public string UserPassword { get; set; }
    }
}