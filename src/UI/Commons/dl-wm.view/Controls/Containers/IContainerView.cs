using System;
using dl.wm.models.DTOs.Containers;

namespace dl.wm.view.Controls.Containers
{
    public interface IContainerView : IMsgView
    {
        ContainerUiModel Container { set; }
        Guid SelectedContainerId { get; set; }
    }
}