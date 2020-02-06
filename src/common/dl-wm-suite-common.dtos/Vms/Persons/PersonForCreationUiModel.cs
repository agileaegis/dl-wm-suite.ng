﻿using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Persons
{
    public class PersonForCreationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PersonLogin { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string PersonEmail { get; set; }
    }
}