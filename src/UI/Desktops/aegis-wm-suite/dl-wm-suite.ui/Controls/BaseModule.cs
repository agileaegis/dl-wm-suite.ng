using System;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPivotGrid;

namespace dl.wm.suite.ui.Controls
{
    public class BaseModule : XtraUserControl, ISupportNavigation
    {

        internal bool FirstShowing = true;
        public virtual string ModuleCaption => string.Empty;
        public virtual string ModuleName => ModuleCaption;

        public virtual bool IsInitialized { get; set; }
        public virtual bool IsShown { get; set; }
        public virtual bool IsHidden { get; set; }
        public virtual void Init(BarManager menuManager) { }

        internal virtual void ShowModule(object item)
        {
            FirstShowing = false;
        }

        internal virtual void HideModule()
        {
            
        }
        internal virtual void InitModule(IDXMenuManager manager, object data)
        {
            SetMenuManager(this.Controls, manager);
        }

        #region Flyouts

        #region SignIn

        protected virtual void OnSigninRequested(SiginEventArgs args)
        {
            (this.ParentForm.ActiveControl as BaseModule).RaiseSignin(args);
        }

        private void RaiseSignin(SiginEventArgs args)
        {
            EventHandler<SiginEventArgs> handler = OnSigninEventRequested;
            handler?.Invoke(this, args);
        }

        public event EventHandler<SiginEventArgs> OnSigninEventRequested;

        public class SiginEventArgs : EventArgs
        {
            public SiginEventArgs(string text)
            {
                Text = text;
            }

            public bool IsAccepted { get; set; }

            public string Text { get; }
        }

        #endregion

        #region AddEditContainer

        protected virtual void OnAddNewContainerRequested(FlyoutAddContainerEventArgs args) {
            (this.ParentForm.ActiveControl as BaseModule).RaiseAddNewContainer(args);
        }

        private void RaiseAddNewContainer(FlyoutAddContainerEventArgs args)
        {
            EventHandler<FlyoutAddContainerEventArgs> handler = OnAddEditContainerRequested;
            handler?.Invoke(this, args);
        }

        public event EventHandler<FlyoutAddContainerEventArgs> OnAddEditContainerRequested;

        public class FlyoutAddContainerEventArgs : EventArgs {
            public FlyoutAddContainerEventArgs(string text) {
                Text = text;
            }

            public bool IsAccepted { get; set; }

            public string Text { get; }
        }

        #endregion
        
        #region AddEditEmployee

        protected virtual void OnEvtAddNewEmployeeRequested(FlyoutAddEmployeeEventArgs args) {
            (this.ParentForm.ActiveControl as BaseModule).RaiseEvtAddNewEmployee(args);
        }

        private void RaiseEvtAddNewEmployee(FlyoutAddEmployeeEventArgs args)
        {
            EventHandler<FlyoutAddEmployeeEventArgs> handler = OnEvtAddEditEmployeeRequested;
            handler?.Invoke(this, args);
        }

        public event EventHandler<FlyoutAddEmployeeEventArgs> OnEvtAddEditEmployeeRequested;

        public class FlyoutAddEmployeeEventArgs : EventArgs {
            public FlyoutAddEmployeeEventArgs(string text) {
                Text = text;
            }

            public bool IsAccepted { get; set; }

            public string Text { get; }
        }

        #endregion

        #region AddEditUser

        protected virtual void OnAddNewUserRequested(FlyoutAddEditUserEventArgs args) {
            (this.ParentForm.ActiveControl as BaseModule).RaiseAddNewUser(args);
        }

        private void RaiseAddNewUser(FlyoutAddEditUserEventArgs args)
        {
            EventHandler<FlyoutAddEditUserEventArgs> handler = OnAddEditUserRequested;
            handler?.Invoke(this, args);
        }

        public event EventHandler<FlyoutAddEditUserEventArgs> OnAddEditUserRequested;

        public class FlyoutAddEditUserEventArgs : EventArgs {
            public FlyoutAddEditUserEventArgs(string text, Guid selectedUserId)
            {
                Text = text;
                SelectedUserId = selectedUserId;
            }

            public bool IsAccepted { get; set; }

            public string Text { get; }
            public Guid SelectedUserId { get; private set; }
        }

        #endregion

        #region AddEditUserRole

        protected virtual void OnUserRoleAddEditRoleRequested(FlyoutAddEditRoleEventArgs args) {
            (this.ParentForm.ActiveControl as BaseModule).RaiseUerRoleAddEditRole(args);
        }

        protected virtual void RaiseUerRoleAddEditRole(FlyoutAddEditRoleEventArgs args) {
            EventHandler<FlyoutAddEditRoleEventArgs> handler = OnAddEditUserRoleRequested;
            handler?.Invoke(this, args);
        }

        public event EventHandler<FlyoutAddEditRoleEventArgs> OnAddEditUserRoleRequested;

        public class FlyoutAddEditRoleEventArgs : EventArgs {
            public FlyoutAddEditRoleEventArgs(Guid id, string text) {
                Id = id;
                Text = text;
            }

            public bool IsAccepted { get; set; }

            public string Text { get; }
            public Guid Id { get; }
        }

        #endregion

        #endregion

        #region Others

        void SetMenuManager(ControlCollection controlCollection, IDXMenuManager manager)
        {
            foreach (Control ctrl in controlCollection)
            {
                GridControl grid = ctrl as GridControl;
                if (grid != null)
                {
                    grid.MenuManager = manager;
                    break;
                }
                PivotGridControl pivot = ctrl as PivotGridControl;
                if (pivot != null)
                {
                    pivot.MenuManager = manager;
                    break;
                }
                BaseEdit edit = ctrl as BaseEdit;
                if (edit != null)
                {
                    edit.MenuManager = manager;
                    break;
                }
                SetMenuManager(ctrl.Controls, manager);
            }
        }
        public virtual bool AllowWaitDialog => true;
        public void OnNavigatedTo(INavigationArgs args)
        {
        }

        public void OnNavigatedFrom(INavigationArgs args)
        {
        }

        #endregion

    }

    public class CustomFlyoutDialog : FlyoutDialog
    {
        public CustomFlyoutDialog(Form owner, FlyoutAction action, Control control)
            : base(owner, action)
        {
            FlyoutControl = control;
        }
    }
}
