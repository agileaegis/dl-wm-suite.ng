using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Employees.Departments
{
    public class DepartmentUiModel : IUiModel
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
        public string Active { get; set; }
    }
}
