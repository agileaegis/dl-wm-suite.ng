using System;
using dl.wm.presenter.ViewModel.Etvs;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Evts;

namespace dl.wm.suite.ui.Views.Components.EmployeesToursVehicles
{
    public partial class UcClientsEmployees : BaseModule, IUcEvtEmployeeManagementView
    {

        private UcEvtEmployeeManagementPresenter _ucEvtEmployeeManagementPresenter;
        public UcClientsEmployees()
        {
            InitializeComponent();
            InitializePresenter();
        }

        private void InitializePresenter()
        {
            _ucEvtEmployeeManagementPresenter = new UcEvtEmployeeManagementPresenter(this);
        }

        private void BtnEvtAddEmployeeClick(object sender, System.EventArgs e)
        {
            _ucEvtEmployeeManagementPresenter.OpenFlyoutForAddEmployeeWasClicked();
        }

        public bool OpenFlyoutForAddEmployee
        {
            set
            {
                if (value)
                {
                    FlyoutAddEmployeeEventArgs args =
                        new FlyoutAddEmployeeEventArgs( "OnEvtAddNewEmployee");
                    this.OnEvtAddNewEmployeeRequested(args);
                    if (args.IsAccepted)
                    {
                        MySaveMethod();
                    }
                }
            }
        }
        private void MySaveMethod()
        {
        }
    }
}
