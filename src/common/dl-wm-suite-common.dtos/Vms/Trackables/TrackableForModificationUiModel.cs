using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Trackables
{
    public class TrackableForModificationUiModel : IUiModel
    {
        [Editable(true)] public string TrackableBrand { get; set; }
        [Editable(true)] public string TrackableNumPlate { get; set; }
        [Editable(true)] public bool TrackableActive { get; set; }
        [Editable(true)] public string TrackableType { get; set; }
        [Editable(true)] public string TrackableStatus { get; set; }
        [Editable(true)] public string TrackableGas { get; set; }

        [Key] public Guid Id { get; set; }
        public string Message { get; set; }
    }
}