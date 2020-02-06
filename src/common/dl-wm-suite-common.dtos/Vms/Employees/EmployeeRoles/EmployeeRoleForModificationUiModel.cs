using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles
{
    public class EmployeeRoleForModificationUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }
        [Editable(true)]
        public string Message { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string EmployeeRoleName { get; set; }
    }
}
