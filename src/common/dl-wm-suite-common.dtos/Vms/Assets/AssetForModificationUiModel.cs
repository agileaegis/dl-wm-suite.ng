using System;
using dl.wm.suite.common.dtos.Vms.Bases;

namespace dl.wm.suite.common.dtos.Vms.Assets
{
    public class AssetForModificationUiModel : IUiModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}