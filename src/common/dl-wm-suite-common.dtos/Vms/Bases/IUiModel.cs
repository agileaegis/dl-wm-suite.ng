using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Bases
{
    public interface IUiModel
    {
        [Key]
        Guid Id { get; set; }
        [Editable(false)]
        string Message { get; set; }
    }
}
