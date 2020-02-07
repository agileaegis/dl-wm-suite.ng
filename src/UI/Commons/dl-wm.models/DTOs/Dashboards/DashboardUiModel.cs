using System;
using System.ComponentModel.DataAnnotations;
using dl.wm.models.DTOs.Base;

namespace dl.wm.models.DTOs.Dashboards
{
    public class DashboardUiModel : IUiModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}