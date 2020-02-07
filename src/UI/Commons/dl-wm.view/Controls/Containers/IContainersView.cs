using System.Collections.Generic;
using dl.wm.models.DTOs.Containers;

namespace dl.wm.view.Controls.Containers
{
    public interface IContainersView: IView

    {
    bool NoneContainerWasRetrieved { set; }
    List<ContainerUiModel> Containers { get; set; }
    }

    public interface IContainersPointsView : IView

    {
        bool NoneContainerPointWasRetrieved { set; }
        List<ContainerPointUiModel> ContainersPoints { set; }
    }
}
