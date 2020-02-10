using System;
using System.Windows.Forms;
using dl.wm.models.DTOs.Users;
using dl.wm.suite.ui.Controls;
using dl.wm.suite.ui.Views.FlyOuts;
using dl.wm.suite.ui.Views.FlyOuts.AddEditContainer;
using dl.wm.suite.ui.Views.FlyOuts.AddEditRole;
using dl.wm.suite.ui.Views.FlyOuts.AddEditUser;
using dl.wm.suite.ui.Views.FlyOuts.LoginRegister;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;

namespace dl.wm.suite.ui
{
    public partial class Main : XtraForm
    {
        private object _current;
        readonly UserUiModel _userToBeLoginIn = new UserUiModel();

        public Main()
        {
            StartPosition = FormStartPosition.Manual;
            Location = Screen.GetBounds(MousePosition).Location;
            InitializeComponent();

            mainWindowsUiView.ContentContainerActions.Add(new SetSkinAction("Metropolis", "Αλλάξτε Θέμα"));
            closeFlyout.Action = CreateCloseAction();

            this.OnLoginEventRequested += ModuleOnLoginEventRequested;
        }

        private FlyoutAction CreateCloseAction()
        {
            var closeAction = new FlyoutAction
            {
                Description = "Θέλετε να τερματίστε την εφαρμογή;"
            };

            closeAction.Commands.Add(FlyoutCommand.Yes);
            closeAction.Commands.Add(FlyoutCommand.No);
            return closeAction;
        }

        private void MainWindowsUiViewQueryStartupContentContainer(object sender, QueryContentContainerEventArgs e)
        {
            MainGrp.Caption = "dl-wm suite";
            e.ContentContainer = MainGrp;
        }

        private void MainLoad(object sender, EventArgs e)
        {
            LoginEventArgs args =
                new LoginEventArgs("OnStartupLogin");
            this.OnLoginRequested(args);

            if (_userToBeLoginIn.Message == "cancel")
            {
                Application.Exit();
            }

            //Todo: Get the response Roles and parameterize Tiles and Documents
            CorePg.Caption = _userToBeLoginIn.Login;
        }

        private void MainWindowsUiViewNavigatedTo(object sender, NavigationEventArgs e)
        {
        }
        private void MainWindowsUiViewNavigatedFrom(object sender, NavigationEventArgs e)
        {
        }

        private void MainWindowsUiViewFlyoutHidden(object sender, FlyoutResultEventArgs e)
        {
            if (mainWindowsUiView.ActiveFlyoutContainer == loginFlyout)
                mainWindowsUiView.ActivateContainer(MainGrp);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
                if (mainWindowsUiView.ShowFlyoutDialog(closeFlyout) != DialogResult.Yes)
                    e.Cancel = true;
        }

        private void WindowsUiViewQueryControl(object sender, QueryControlEventArgs e)
        {
            BaseModule module = e.Document.Tag is BaseModule ? (BaseModule)e.Document.Tag :
                    Activator.CreateInstance(typeof(Main).Assembly.GetType(e.Document.ControlTypeName)) as BaseModule;
            module.InitModule(mainBarManager, mainWindowsUiView);

            ShowModuleAfterActivation(e.Document, module);
            module.OnSigninEventRequested += ModuleOnSigninEventRequested;
            module.OnAddEditUserRoleRequested += new EventHandler<BaseModule.FlyoutAddEditRoleEventArgs>(ModuleUserRoleManagementAddEditRoleRequested);
            module.OnAddEditUserRequested += new EventHandler<BaseModule.FlyoutAddEditUserEventArgs>(ModuleUserManagementAddEditUserRequested);
            module.OnEvtAddEditEmployeeRequested += new EventHandler<BaseModule.FlyoutAddEmployeeEventArgs>(ModuleEvtAddNewEditEmployeeManagementRequested);
            module.OnAddEditContainerRequested += new EventHandler<BaseModule.FlyoutAddContainerEventArgs>(ModuleAddNewEditContainerManagementRequested);

            e.Document.Tag = module;
            e.Control = module;
        }

        #region flyouts

        private void ModuleUserManagementAddEditUserRequested(object sender, BaseModule.FlyoutAddEditUserEventArgs e)
        {
            FlyoutAction onDemandAddEditUserManagementFlyoutAction = new FlyoutAction {Description = $"{e.Text}"};
            CustomFlyoutDialog onDemandAddEEditUserManagementFlyoutDialog = new CustomFlyoutDialog(this, onDemandAddEditUserManagementFlyoutAction,
                control: new UcFlyUserAddNewEditUserManagement(e));
            onDemandAddEEditUserManagementFlyoutDialog.ShowDialog();
        }

        private void ModuleOnLoginEventRequested(object sender, LoginEventArgs e)
        {
            FlyoutAction onDemandSiginFlyoutAction = new FlyoutAction { Description = $"{e.Text}" };
            CustomFlyoutDialog onDemandSigninFlyoutDialog = new CustomFlyoutDialog(this, onDemandSiginFlyoutAction,
                new UcFlyLoginForget(e.Text, _userToBeLoginIn));
            onDemandSigninFlyoutDialog.ShowDialog();
        }

        private void ModuleOnSigninEventRequested(object sender, BaseModule.SiginEventArgs e)
        {
        }

        private void ModuleUserRoleManagementAddEditRoleRequested(object sender, BaseModule.FlyoutAddEditRoleEventArgs e)
        {
            FlyoutAction onDemandMessageManagementFlyoutAction = new FlyoutAction {Description = $"{e.Text}"};
            CustomFlyoutDialog onDemandMessageManagementFlyoutDialog = new CustomFlyoutDialog(this, onDemandMessageManagementFlyoutAction,
                new UcFlyRoleManagement(e));
            onDemandMessageManagementFlyoutDialog.ShowDialog();
        }

        private void ModuleEvtAddNewEditEmployeeManagementRequested(object sender, BaseModule.FlyoutAddEmployeeEventArgs e)
        {
            FlyoutAction evtAddNewEmployeeManagementFlyoutAction = new FlyoutAction {Description = $"{e.Text}"};
            CustomFlyoutDialog timeTableManagementFlyoutDialog = new CustomFlyoutDialog(this, evtAddNewEmployeeManagementFlyoutAction,
                new UcFlyEvtAddNewEditEmployeeManagement());
            timeTableManagementFlyoutDialog.ShowDialog();
        }

        private void ModuleAddNewEditContainerManagementRequested(object sender, BaseModule.FlyoutAddContainerEventArgs e)
        {
            FlyoutAction onAddNewContainerManagementFlyoutAction = new FlyoutAction {Description = $"{e.Text}"};
            CustomFlyoutDialog addEditContainerManagementFlyoutDialog = new CustomFlyoutDialog(this, onAddNewContainerManagementFlyoutAction,
                new UcFlyContainerAddNewEditContainerManagement(e));
            addEditContainerManagementFlyoutDialog.ShowDialog();
        }

        #endregion

        private void WindowsUiViewTileClick(object sender, TileClickEventArgs e)
        {
            var tile = e.Tile as Tile;
            var module = tile?.Document?.Control as BaseModule;
            if (module != null)
            {
                TileItemFrame frame = tile.CurrentFrame;
                object data = frame?.Tag;
                module.ShowModule(data);
            }
        }

        public delegate void NavRequestEventHandler(object sender, NavRequestEventArgs e);

        public class NavRequestEventArgs : EventArgs
        {
        }

        private void ShowModuleAfterActivation(BaseDocument baseDocument, BaseModule module)
        {
            BaseTile tile = null;

            if (mainWindowsUiView.Tiles.TryGetValue(baseDocument, out tile))
            {
                TileItemFrame frame = tile.CurrentFrame;
                object data = _current ?? frame?.Tag;
                module.ShowModule(data);
            }
        }

        #region LogIn

        protected virtual void OnLoginRequested(LoginEventArgs args)
        {
            this.RaiseLogin(args);
        }

        private void RaiseLogin(LoginEventArgs args)
        {
            EventHandler<LoginEventArgs> handler = OnLoginEventRequested;
            handler?.Invoke(this, args);
        }

        public event EventHandler<LoginEventArgs> OnLoginEventRequested;

        public class LoginEventArgs : EventArgs
        {
            public LoginEventArgs(string text)
            {
                Text = text;
            }

            public bool IsAccepted { get; set; }

            public string Text { get; }
        }

        #endregion


    }
}