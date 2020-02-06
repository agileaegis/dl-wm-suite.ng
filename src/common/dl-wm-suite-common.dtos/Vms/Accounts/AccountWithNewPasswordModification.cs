﻿using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Accounts
{
    public class AccountWithNewPasswordModification
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Password { get; set; }
    }
}