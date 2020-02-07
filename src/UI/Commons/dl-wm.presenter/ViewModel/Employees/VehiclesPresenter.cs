using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls;
using dl.wm.view.Controls.Employees;
using dl.wm.presenter.Base;

namespace dl.wm.presenter.ViewModel.Employees
{
    public class EmployeesPresenter : BasePresenter<IEmployeesView, IEmployeesService>
    {
        public EmployeesPresenter(IEmployeesView view)
            : this(view, new EmployeesService())
        {
        }

        public EmployeesPresenter(IEmployeesView view, IEmployeesService service)
            : base(view, service)
        {
        }

        public async void LoadAllEmployees()
        {
            var employees = await Service.GetEntitiesAsync();

            if (employees?.Count == 0)
                View.NoneEmployeeWasRetrieved = true;
            else
            {
                View.Employees = employees;
            }
        }
    }
}