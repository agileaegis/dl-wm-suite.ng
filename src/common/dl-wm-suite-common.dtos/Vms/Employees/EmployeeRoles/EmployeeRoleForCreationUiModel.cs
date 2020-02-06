using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Employees.EmployeeRoles
{
    public class EmployeeRoleForCreationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string EmployeeRoleName { get; set; }
        [Editable(true)]
        public string EmployeeRoleNotes { get; set; }
    }
}
