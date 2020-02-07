using System.Collections.Generic;
using dl.wm.models.DTOs.Employees.Departments;

namespace dl.wm.view.Controls.Employees.Departments
{
    public interface IDepartmentsView : IMsgView
    {
        List<DepartmentUiModel> Departments { get; set; }
        bool NoneDepartmentWasRetrieved { set; }
    }
}