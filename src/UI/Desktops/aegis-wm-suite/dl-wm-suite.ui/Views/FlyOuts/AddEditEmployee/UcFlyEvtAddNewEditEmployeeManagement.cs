using System;
using dl.wm.suite.ui.Controls;

namespace dl.wm.suite.ui.Views.FlyOuts.AddEditEmployee
{
    public partial class UcFlyEvtAddNewEditEmployeeManagement : BaseModule
    {
        public UcFlyEvtAddNewEditEmployeeManagement()
        {
            InitializeComponent();
        }

        private void BtnEvtAddEditEmployeeCancelClick(object sender, EventArgs e)
        {
            (this.Parent as CustomFlyoutDialog).Close();
        }
    }
}
