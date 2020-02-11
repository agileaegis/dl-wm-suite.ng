using dl.wm.presenter.Commanding.Events.EventArgs.Containers;

namespace dl.wm.presenter.Commanding.Listeners
{
    public interface IContainerPostDetectionActionListener
    {
        void Update(object sender, ContainerEventArgs e);
    }
    public interface IContainerPutDetectionActionListener
    {
        void Update(object sender, ContainerEventArgs e);
    }
}