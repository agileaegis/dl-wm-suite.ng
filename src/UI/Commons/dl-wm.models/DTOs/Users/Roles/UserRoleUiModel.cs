using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Users.Roles
{
    public class UserRoleUiModel : IUiModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public string Message { get; set; }

        [Required]
        [Editable(false)]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        [Editable(true)]
        public DateTime ModifiedDate { get; set; }
        [Required]
        [Editable(true)]
        public bool Active { get; set; }
    }
}