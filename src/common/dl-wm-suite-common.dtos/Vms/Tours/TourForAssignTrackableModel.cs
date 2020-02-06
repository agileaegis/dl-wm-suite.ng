using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Tours
{
    public class TourForAssignTrackableModel
    {
        [Required]
        [Editable(true)]
        public Guid EmployeeId { get; set; }
        [Required]
        [Editable(true)]
        public Guid TourId { get; set; }
        [Required]
        [Editable(true)]
        public DateTime ScheduledDate { get; set; }
        [Required]
        [Editable(true)]
        public string AssetNumplate { get; set; }
    }
}
