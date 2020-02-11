using System.Linq;
using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.view.Controls.Containers;
using dl.wm.presenter.Base;
using dl.wm.presenter.Commanding.Events.EventArgs.Containers;
using dl.wm.presenter.Commanding.Listeners;
using dl.wm.presenter.Commanding.Servers;
using dl.wm.presenter.Helpers;

namespace dl.wm.presenter.ViewModel.Containers
{
    public class UcContainerManagementPresenter : BasePresenter<IUcContainerManagementView, IContainersService>, 
        IContainerPutDetectionActionListener, IContainerPostDetectionActionListener
    {
        public UcContainerManagementPresenter(IUcContainerManagementView view)
            : this(view, new ContainersService())
        {
        }

        public UcContainerManagementPresenter(IUcContainerManagementView view, IContainersService service)
            : base(view, service)
        {
            CommandingInboundServer.GetCommandingInboundServer.Attach((IContainerPostDetectionActionListener)this);
            CommandingInboundServer.GetCommandingInboundServer.Attach((IContainerPutDetectionActionListener)this);
        }

        public void UcWasLoaded()
        {
            View.InitialLoadingWasCaught = true;
        }

        public void OpenFlyoutForAddContainerWasClicked()
        {
            View.OpenFlyoutForAddContainer = true;
        }

        public void OpenFlyoutForEditContainerWasClicked()
        {
            View.OpenFlyoutForEditContainer = true;
        }

        public void RemoveContainerWasClicked()
        {
        }
        public async void ContainerFromGridWasSelected()
        {
            var urls = await StorageHelper.GetThumbNailUrls(new AzureStorageConfig(), View.SelectedContainerImageName);
            if (urls.Count > 0)
            {
                View.PctContainerImageValue = urls.First();
            }

            View.OnPopulateContainerDataAfterSelection = true;
        }

        void IContainerPutDetectionActionListener.Update(object sender, ContainerEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        void IContainerPostDetectionActionListener.Update(object sender, ContainerEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}