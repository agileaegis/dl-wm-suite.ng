using System.Collections.Generic;
using dl.wm.models.DTOs.Users.Roles;
using dl.wm.models.DTOs.Vehicles;

namespace dl.wm.view.Controls.Users.Roles
{
    public interface IUserRolesView : IMsgView
    {
        List<UserRoleUiModel> UserRoles { get; set; }
        bool NoneUserRoleWasRetrieved { set; }
    }
}