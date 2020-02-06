using System;
using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Accounts
{
    public class AccountForActivationModification
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public Guid ActivationKey { get; set; }
    }
}