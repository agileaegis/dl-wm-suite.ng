﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dl.wm.models.DTOs.Users.Roles;
using dl.wm.models.DTOs.Vehicles;
using dl.wm.presenter.ViewModel.Users.Roles;
using dl.wm.presenter.ViewModel.Vehicles;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Users;
using dl.wm.view.Controls.Users.Roles;
using dl.wm.view.Controls.Vehicles;
using DevExpress.XtraEditors;

namespace dl.wm.suite.ui.Views.Components.UsersRolesDepartments
{
    public partial class UcClientsRoles : BaseModule, IUcUserRoleManagementView, IUserRolesView
    {

        private UcUserRoleManagementPresenter _ucUserRoleManagementPresenter;
        private UserRolesPresenter _ucUserRolesPresenter;


        public UcClientsRoles()
        {
            InitializeComponent();
            InitializePresenters();

        }

        private void InitializePresenters()
        {
            _ucUserRoleManagementPresenter = new UcUserRoleManagementPresenter(this);
            _ucUserRolesPresenter = new UserRolesPresenter(this);
        }


        #region IUcUserRoleManagementView

        public bool OpenFlyoutForAddEditUserRole
        {
            set
            {
                if (value)
                {
                    FlyoutAddEditRoleEventArgs args =
                        new FlyoutAddEditRoleEventArgs( SelectedUserRoleId,  "OnAddEditUserRole");

                    this.OnUserRoleAddEditRoleRequested(args);;
                    if (args.IsAccepted)
                    {
                        MySaveMethod();
                    }
                }
            }
        }

        public bool BtnUserRoleAddEnabled
        {
            get => btnAddUserRole.Enabled;
            set => btnAddUserRole.Enabled = value;
        }

        public bool BtnUserRoleDeleteEnabled
        {
            get => btnRemoveUserRole.Enabled;
            set => btnRemoveUserRole.Enabled = value;
        }

        public bool BtnUserRoleEditCancelEnabled
        {
            get => btnEditUserRole.Enabled;
            set => btnEditUserRole.Enabled = value;
        }

        public Guid SelectedUserRoleId { get; set; }
        public UserRoleUiModel SelectedUserRole { get; set; }

        #endregion


        private void MySaveMethod()
        {
        }

        #region IUserRolesView

        public string OnGeneralMsg { get; set; }

        public List<UserRoleUiModel> UserRoles
        {
            get => (List<UserRoleUiModel>)gvRoles.DataSource;
            set => gcRoles.DataSource = value;

        }
        public bool NoneUserRoleWasRetrieved { get; set; }

        #endregion

        private void UcClientsRolesLoad(object sender, EventArgs e)
        {
            _ucUserRolesPresenter.LoadAllUserRoles();
        }

        private void BtnAddUserRoleClick(object sender, EventArgs e)
        {
            _ucUserRoleManagementPresenter.OpenFlyoutForAddUserRoleWasClicked();
        }


        private void BtnEditUserRoleClick(object sender, EventArgs e)
        {
            _ucUserRoleManagementPresenter.OpenFlyoutForEditUserRoleWasClicked();
        }

        private void BtnRemoveUserRoleClick(object sender, EventArgs e)
        {
            _ucUserRoleManagementPresenter.RemoveUserRoleWasClicked();
        }

        private void GvRolesFocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
            {
                SelectedUserRoleId = Guid.Empty;
            }
            else
            {
                SelectedUserRoleId = (Guid)gvRoles.GetRowCellValue(
                    e.FocusedRowHandle, "Id");
            }

            _ucUserRoleManagementPresenter.UserRoleFromGridWasSelected();
        }
    }
}
