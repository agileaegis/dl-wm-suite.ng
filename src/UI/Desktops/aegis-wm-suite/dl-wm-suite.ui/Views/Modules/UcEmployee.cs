using System.Windows.Forms;
using dl.wm.presenter.ViewModel.Etvs;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.Repositories;
using dl.wm.view.Controls.Evts;
using DevExpress.Utils.Menu;

namespace dl.wm.suite.ui.Views.Modules
{
    public partial class UcEmployees : BaseModule, IUcEvtManagementView
    {
        public override string ModuleCaption => "Employees";
        public override bool AllowWaitDialog => true;

        #region Presenters

        private UcEvtManagementPresenter _ucEvtManagementPresenter;

        #endregion

        internal override void InitModule(IDXMenuManager manager, object data)
        {
            IsInitialized = true;
            base.InitModule(manager, data);
        }

        internal override void ShowModule(object item)
        {
            if (!IsInitialized)
                return;
            IsShown = true;
            base.ShowModule(item);

            OnShowModuleLocal();
        }

        private void OnShowModuleLocal()
        {
        }

        internal override void HideModule()
        {
            IsShown = false;
            base.HideModule();
        }

        public UcEmployees()
        {
            InitializeComponent();
            InitializePresenter();
            InitializeLoad();
        }

        private void InitializePresenter()
        {
            _ucEvtManagementPresenter = new UcEvtManagementPresenter(this);
        }

        private void InitializeLoad()
        {
            SelectedModuleItem = "EmployeeManagement";
            _ucEvtManagementPresenter.NavBarModuleLinkClicked();
        }

        public string SelectedModuleItem { get; set; }

        public bool PopulateUcCtrl
        {
            set
            {
                if (value)
                {
                   pnlCntrlEtvSelectionProjection.Controls.Clear();

                    BaseModule ucModuleItem = ModuleEtvViewRepository.ViewRepository[SelectedModuleItem];
                    ucModuleItem.Dock = DockStyle.Fill;
                    pnlCntrlEtvSelectionProjection.Controls.Add(ucModuleItem);
                }
            }
        }

        private void NvBrCntrlEtvSelectionsLinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            SelectedModuleItem = (string)e.Link.Item.Tag;
            _ucEvtManagementPresenter.NavBarModuleLinkClicked();
        }

        private void nvBrCntrlEtvSelections_Click(object sender, System.EventArgs e)
        {

        }
    }
}
