using System;
using dl.wm.models.DTOs.Containers;
using dl.wm.presenter.Commanding.Events.EventArgs.Containers;
using dl.wm.presenter.Commanding.Listeners;

namespace dl.wm.presenter.Commanding.Servers.Base
{
    public abstract class CommandingInboundBaseServer
    {
        public event EventHandler<ContainerEventArgs> ContainerPostDetector;
        public event EventHandler<ContainerEventArgs> ContainerPutDetector;

        #region Container Post detection Event Manipulation

        private void OnContainerPostDetection(ContainerEventArgs e)
        {
            ContainerPostDetector?.Invoke(this, e);
        }

        public void RaiseContainerPostDetection(ContainerUiModel container)
        {
            OnContainerPostDetection(new ContainerEventArgs(container));
        }

        public void Attach(IContainerPostDetectionActionListener listener)
        {
            ContainerPostDetector += listener.Update;
        }

        public void Detach(IContainerPostDetectionActionListener listener)
        {
            ContainerPostDetector -= listener.Update;
        }

        #endregion


        #region Container Put detection Event Manipulation

        private void OnContainerPutDetection(ContainerEventArgs e)
        {
            ContainerPutDetector?.Invoke(this, e);
        }

        public void RaiseContainerPutDetection(ContainerUiModel container)
        {
            OnContainerPutDetection(new ContainerEventArgs(container));
        }

        public void Attach(IContainerPutDetectionActionListener listener)
        {
            ContainerPutDetector += listener.Update;
        }

        public void Detach(IContainerPutDetectionActionListener listener)
        {
            ContainerPutDetector -= listener.Update;
        }

        #endregion
    }
}