using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.presenter.Utilities;
using dl.wm.view.Controls.Containers;
using dl.wm.presenter.Base;

namespace dl.wm.presenter.ViewModel.Containers
{
    public class ContainerPointsPresenter : BasePresenter<IContainersPointsView, IContainersService>
    {
        public ContainerPointsPresenter(IContainersPointsView view)
            : this(view, new ContainersService())
        {
        }

        public ContainerPointsPresenter(IContainersPointsView view, IContainersService service)
            : base(view, service)
        {
        }

        public async void LoadAllContainersPoints()
        {
            var containersPoints = await Service.GetAllActiveContainersPointsAsync(ClientSettingsSingleton.InstanceSettings().TokenConfigValue);

            if (containersPoints?.Count == 0)
                View.NoneContainerPointWasRetrieved = true;
            else
            {
                View.ContainersPoints = containersPoints;
            }
        }
    }
}