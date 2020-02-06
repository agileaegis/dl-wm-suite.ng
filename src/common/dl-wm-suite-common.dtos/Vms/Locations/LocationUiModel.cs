using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Locations
{
    public class LocationUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}