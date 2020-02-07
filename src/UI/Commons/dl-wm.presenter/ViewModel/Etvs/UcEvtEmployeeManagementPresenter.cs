using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.view.Controls.Evts;
using dl.wm.presenter.Base;

namespace dl.wm.presenter.ViewModel.Etvs
{
    public class UcEvtEmployeeManagementPresenter : BasePresenter<IUcEvtEmployeeManagementView, IEmployeesService>
    {

        public UcEvtEmployeeManagementPresenter(IUcEvtEmployeeManagementView view)
            : this(view, new EmployeesService())
        {
        }

        public UcEvtEmployeeManagementPresenter(IUcEvtEmployeeManagementView view, IEmployeesService service)
            : base(view, service)
        {
        }

        public void OpenFlyoutForAddEmployeeWasClicked()
        {
            View.OpenFlyoutForAddEmployee = true;
        }
    }
}