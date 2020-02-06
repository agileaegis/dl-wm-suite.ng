using System;
using System.Windows.Forms;
using dl.wm.presenter.ViewModel.Configurations;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.Repositories;
using dl.wm.view.Controls.Configurations;
using DevExpress.Utils.Menu;

namespace dl.wm.suite.ui.Views.Modules
{
    public partial class UcConfiguration : BaseModule, IUcCnfigurationManagementView
    {
        public override string ModuleCaption => "Settings";
        public override bool AllowWaitDialog => false;

        private UcConfigurationManagementPresenter _ucConfigurationManagement;

        internal override void InitModule(IDXMenuManager manager, object data)
        {
            base.InitModule(manager, data);
            IsInitialized = true;
        }

        internal override void ShowModule(object item)
        {
            if (!IsInitialized)
                return;
            IsShown = true;
            base.ShowModule(item);
        }

        internal override void HideModule()
        {
            IsShown = false;
            base.HideModule();
        }


        public UcConfiguration()
        {
            InitializeComponent();
            InitializePresenters();
            InitializeLoad();
        }

        private void InitializePresenters()
        {
            _ucConfigurationManagement = new UcConfigurationManagementPresenter(this);
        }


        private void InitializeLoad()   
        {
            SelectedModuleItem = "DicomSettings";
            _ucConfigurationManagement.NavBarModuleLinkClicked();
        }

        public string SelectedModuleItem { get; set; }

        public bool PopulateUcCtrl
        {
            set
            {
                if (value)
                {
                    pnlCntrlSelectionSettings.Controls.Clear();

                   BaseModule ucModuleItem = ModuleViewRepository.ViewRepository[SelectedModuleItem];
                   ucModuleItem.Dock = DockStyle.Fill;
                    pnlCntrlSelectionSettings.Controls.Add(ucModuleItem);
                }
            }
        }

        private void NvBrCntrBook2BoardLinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            SelectedModuleItem = (string)e.Link.Item.Tag;
            _ucConfigurationManagement.NavBarModuleLinkClicked();
        }
    }
}