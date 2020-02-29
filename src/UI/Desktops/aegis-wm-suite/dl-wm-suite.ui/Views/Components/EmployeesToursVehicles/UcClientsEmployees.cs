using System;
using System.Collections.Generic;
using dl.wm.models.DTOs.Employees;
using dl.wm.presenter.ViewModel.Employees;
using dl.wm.presenter.ViewModel.Etvs;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Employees;
using dl.wm.view.Controls.Evts;

namespace dl.wm.suite.ui.Views.Components.EmployeesToursVehicles
{
    public partial class UcClientsEmployees : BaseModule, IUcEvtEmployeeManagementView, IEmployeesView
    {

        private UcEvtEmployeeManagementPresenter _ucEvtEmployeeManagementPresenter;
        private EmployeesPresenter _employeesPresenter;
        public UcClientsEmployees()
        {
            InitializeComponent();
            InitializePresenter();
        }

        private void InitializePresenter()
        {
            _ucEvtEmployeeManagementPresenter = new UcEvtEmployeeManagementPresenter(this);
            _employeesPresenter = new EmployeesPresenter(this);
        }

        private void BtnEvtAddEmployeeClick(object sender, System.EventArgs e)
        {
            _ucEvtEmployeeManagementPresenter.OpenFlyoutForAddEmployeeWasClicked();
        }

        #region IUcEvtEmployeeManagementView

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

        public bool OnEmployeeManagementLoaded
        {
            set
            {
                if (value)
                {
                    _employeesPresenter.LoadAllActiveEmployees();
                }
            }
        }

        private void MySaveMethod()
        {
        }

        #endregion

        #region IEmployeesView

        public string OnGeneralMsg { get; set; }

        public List<EmployeeUiModel> Employees
        {
            get => (List<EmployeeUiModel>) gvAdvBndEvtEmployees.DataSource;
            set => gcAdvBndEvtEmployees.DataSource = value;
        }
        public bool NoneEmployeeWasRetrieved { get; set; }
        public string OnEmployeesMsgError { get; set; }

        #endregion

        #region Locals

        private void UcClientsEmployeesLoad(object sender, EventArgs e)
        {
            _ucEvtEmployeeManagementPresenter.UcWasLoaded();
        }

        #endregion
    }
}
