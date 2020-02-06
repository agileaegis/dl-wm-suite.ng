using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Employees.Departments
{
    public class DepartmentForCreationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string DepartmentName { get; set; }
        [Editable(true)]
        public string DepartmentNotes { get; set; }
    }
}
