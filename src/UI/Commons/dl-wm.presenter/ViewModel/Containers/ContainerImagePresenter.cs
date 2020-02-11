using System.Linq;
using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.view.Controls.Containers;
using dl.wm.presenter.Base;
using dl.wm.presenter.Helpers;

namespace dl.wm.presenter.ViewModel.Containers
{
    public class ContainerImagePresenter : BasePresenter<IContainerImageView, IContainersService>
    {
        public ContainerImagePresenter(IContainerImageView view)
            : this(view, new ContainersService())
        {
        }

        public ContainerImagePresenter(IContainerImageView view, IContainersService service)
            : base(view, service)
        {
        }

        public async void ContainerImagePopulate()
        {
            var urls = await StorageHelper.GetThumbNailUrls(new AzureStorageConfig(), View.SelectedContainerImageNameImageView);
            if (urls.Count > 0)
            {
                View.PctContainerImagePathValue = urls.First();
            }
        }
    }
}