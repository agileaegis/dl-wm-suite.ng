using System.Collections.Generic;
using dl.wm.models.DTOs.Employees.EmployeeRoles;

namespace dl.wm.view.Controls.Employees.EmployeeRoles
{
    public interface IEmployeeRolesView : IMsgView
    {
        List<EmployeeRoleUiModel> EmployeeRoles { get; set; }
        bool NoneEmployeeRoleWasRetrieved { set; }
    }
}