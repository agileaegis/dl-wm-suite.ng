using System.Collections.Generic;
using dl.wm.models.DTOs.Users;
using dl.wm.models.DTOs.Vehicles;

namespace dl.wm.view.Controls.Users
{
    public interface IUsersView : IMsgView
    {
        List<UserForAllRetrievalUiModel> Users { get; set; }
        bool NoneUserWasRetrieved { set; }
    }
}