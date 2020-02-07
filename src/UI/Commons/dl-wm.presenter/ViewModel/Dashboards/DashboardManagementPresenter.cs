using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.view.Controls.Dashboards;
using dl.wm.view.Controls.Users;
using dl.wm.presenter.Base;

namespace dl.wm.presenter.ViewModel.Dashboards
{
    public class DashboardManagementPresenter : BasePresenter<IDashboardManagementView, IDashboardService>
    {
        public DashboardManagementPresenter(IDashboardManagementView view)
            : this(view, new DashboardService())
        {
        }

        public DashboardManagementPresenter(IDashboardManagementView view, DashboardService service)
            : base(view, service)
        {
        }


        public void UcDashboardWasLoaded()
        {
            View.OnDashboardLoaded = true;
        }
    }
}
