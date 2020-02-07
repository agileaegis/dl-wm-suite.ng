using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.models.DTOs.Base
{
    public interface IUiModel
    {
        [Key]
        Guid Id { get; set; }
        [Editable(false)]
        string Message { get; set; }

    }
}