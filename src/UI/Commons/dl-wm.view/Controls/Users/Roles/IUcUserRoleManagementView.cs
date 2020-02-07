using System;
using dl.wm.models.DTOs.Users.Roles;

namespace dl.wm.view.Controls.Users.Roles
{
    public interface IUcUserRoleManagementView : IView
    {
        bool OpenFlyoutForAddEditUserRole { set; }

        bool BtnUserRoleAddEnabled { get; set; }
        bool BtnUserRoleDeleteEnabled { get; set; }
        bool BtnUserRoleEditCancelEnabled { get; set; }

        Guid SelectedUserRoleId { get; set; }

        UserRoleUiModel SelectedUserRole { get; set; }
    }
}