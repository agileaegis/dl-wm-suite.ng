using System;
using dl.wm.models.DTOs.Containers;

namespace dl.wm.view.Controls.Containers
{
    public interface IContainerImageView : IView
    {
        string PctContainerImagePathValue { set; }
        string SelectedContainerImageNameImageView { get; set; }
    }
}