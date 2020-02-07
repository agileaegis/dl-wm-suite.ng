using System.Collections.Generic;
using dl.wm.models.DTOs.Employees;
using dl.wm.models.DTOs.Employees;

namespace dl.wm.view.Controls.Employees
{
    public interface IEmployeesView : IMsgView
    {
        List<EmployeeUiModel> Employees { get; set; }
        bool NoneEmployeeWasRetrieved { set; }
    }
}