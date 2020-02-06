using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.EmployeesTours
{
    public class EmployeeTourUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }
        [Editable(true)]
        public string Message { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string LastName { get; set; }
        [Editable(false)]
        public string CreatedDate { get; set; }
    }
}
