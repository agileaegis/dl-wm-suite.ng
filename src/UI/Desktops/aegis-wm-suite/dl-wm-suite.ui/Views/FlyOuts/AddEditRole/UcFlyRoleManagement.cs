﻿using System;
using System.Windows.Forms;
using dl.wm.models.DTOs.Users;
using dl.wm.models.DTOs.Users.Roles;
using dl.wm.presenter.ViewModel.Users.Roles;
using dl.wm.suite.ui.Controls;
using dl.wm.view.Controls.Users.Roles;

namespace dl.wm.suite.ui.Views.FlyOuts.AddEditRole
{
    public partial class UcFlyRoleManagement : BaseModule, IFoUserRoleManagementView
    {
        public FlyoutAddEditRoleEventArgs FlyoutAddEditRole { get; set; }

        #region Presenters

        private FoUserRoleManagementPresenter _foUserRoleManagementPresenter;
        #endregion


        public UcFlyRoleManagement(FlyoutAddEditRoleEventArgs e)
        {
            FlyoutAddEditRole = e;
            UserRoleIdToBeRetrieved = e.Id;
            InitializeComponent();
            InitializePresenters();
            InitializeLoads();
        }

        private void InitializeLoads()
        {
            _foUserRoleManagementPresenter.FoWasLoaded();
        }

        private void InitializePresenters()
        {
            this._foUserRoleManagementPresenter = new FoUserRoleManagementPresenter(this);
        }

        private void BtnUserManagementAddEditRoleCancelClick(object sender, System.EventArgs e)
        {
            FlyoutAddEditRole.IsAccepted = false;
            (this.Parent as CustomFlyoutDialog).Close();
        }

        private void BtnUserManagementAddEditRoleSaveClick(object sender, System.EventArgs e)
        {
            _foUserRoleManagementPresenter.AddEditRoleSaveWasClicked();
        }

        #region IFoUserRoleManagementView

        public bool TxtUserRoleNameEnabled
        {
            get => txtRoleName.Enabled;
            set => txtRoleName.Enabled = value;
        }

        public string TxtUserRoleNameValue
        {
            get => txtRoleName.Text;
            set => txtRoleName.Text = value;
        }

        public Guid UserRoleIdToBeRetrieved { get; set; }
        public UserRoleUiModel SelectedUserRole { get; set; }
        public UserRoleUiModel ChangedUserRole { get; set; }
        public string OnSaveUserRoleMsgError { get; set; }
        public Guid SelectedUserRoleId { get; set; }
        public UserRoleUiModel CreatedUserRole { get; set; }
        public bool OnSuccessUserRoleCreation { get; set; }
        public bool VerifyForTheUserRoleModification { get; set; }
        public bool ActionAfterVerifyForTheUserRoleModification { get; set; }
        public UserRoleUiModel ModifiedUserRole { get; set; }
        public string ChangedUserRoleName { get; set; }
        public string SelectedUserRoleName { get; set; }

        #endregion
    }
}
