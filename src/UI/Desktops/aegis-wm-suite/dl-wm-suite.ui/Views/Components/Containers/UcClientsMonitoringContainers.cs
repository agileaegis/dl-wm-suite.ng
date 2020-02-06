using System;
using dl.wm.presenter.ViewModel.Containers;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Containers;
using DevExpress.XtraMap;

namespace dl.wm.suite.ui.Views.Components.Containers
{
    public partial class UcClientsMonitoringContainers : BaseModule, IUcMonitoringContainerManagementView
    {

        private UcMonitoringContainerManagementPresenter _ucMonitoringContainerManagementPresenter;
        public UcClientsMonitoringContainers()
        {
            InitializeComponent();
            InitializePresenter();
        }

        private void InitializePresenter()
        {
            _ucMonitoringContainerManagementPresenter = new UcMonitoringContainerManagementPresenter(this);
        }

        private void UcClientsUcContainersLoad(object sender, EventArgs e)
        {
            OnLoaded();
        }

        private void OnLoaded()
        {
        }
    }
}
