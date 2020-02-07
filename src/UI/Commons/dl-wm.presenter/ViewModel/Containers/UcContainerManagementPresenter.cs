using System.Linq;
using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.view.Controls.Containers;
using dl.wm.presenter.Base;
using dl.wm.presenter.Helpers;

namespace dl.wm.presenter.ViewModel.Containers
{
    public class UcContainerManagementPresenter : BasePresenter<IUcContainerManagementView, IContainersService>
    {
        public UcContainerManagementPresenter(IUcContainerManagementView view)
            : this(view, new ContainersService())
        {
        }

        public UcContainerManagementPresenter(IUcContainerManagementView view, IContainersService service)
            : base(view, service)
        {
        }

        public void UcWasLoaded()
        {
            View.InitialLoadingWasCaught = true;
        }

        public void OpenFlyoutForAddContainerWasClicked()
        {
            View.OpenFlyoutForAddContainer = true;
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
    }
}