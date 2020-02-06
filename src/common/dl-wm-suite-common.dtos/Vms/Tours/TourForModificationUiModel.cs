using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Tours
{
    public class TourForModificationUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }
        [Editable(true)]
        public string Message { get; set; }

        [Editable(true)]
        public string PersonFirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PersonLastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PersonEmail { get; set; }
    }
}
