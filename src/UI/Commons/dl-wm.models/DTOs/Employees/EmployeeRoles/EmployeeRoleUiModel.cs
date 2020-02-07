using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Employees.EmployeeRoles
{
    public class EmployeeRoleUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }
        [Editable(true)]
        public string Message { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
        [Editable(false)]
        public string CreatedDate { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string ModifiedDate { get; set; }
        [Required]
        [Editable(false)]
        public bool Active { get; set; }
    }
}