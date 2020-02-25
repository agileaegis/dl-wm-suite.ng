using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Trackables
{
    public class TrackableUiModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Message { get; set; }

        [Required]
        [Editable(true)] public string TrackableName { get; set; }
        [Required]
        [Editable(true)] public string TrackableModel { get; set; }
        [Required]
        [Editable(true)] public string TrackableImei { get; set; }
        [Required]
        [Editable(true)] public string TrackablePhone { get; set; }
        [Required]
        [Editable(true)] public string TrackableOs { get; set; }
        [Required]
        [Editable(true)] public string TrackableVersion { get; set; }
        [Required]
        [Editable(true)] public string TrackableNotes { get; set; }
        [Required]
        [Editable(true)] public DateTime TrackableCreatedDate { get; set; }
  }
}