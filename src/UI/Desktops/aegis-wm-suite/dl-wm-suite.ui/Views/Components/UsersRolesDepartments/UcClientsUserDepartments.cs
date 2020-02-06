using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dl.wm.models.DTOs.Vehicles;
using dl.wm.presenter.ViewModel.Vehicles;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Vehicles;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace dl.wm.suite.ui.Views.Components.UsersRolesDepartments
{
    public partial class UcClientsUserDepartments : BaseModule
    {
        public UcClientsUserDepartments()
        {
            InitializeComponent();
            InitializePresenters();
        }

        private void InitializePresenters()
        {
        }

    }
}
