using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dl.wm.view.Controls.Users
{
    public interface IUserManagementView : IView
    {
        bool UcUserWasLoadedOnDemand { set; }
        bool OpenFlyoutForAddUser { set; }
    }
}
