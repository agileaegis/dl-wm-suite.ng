using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Employees.Departments
{
    public interface IInquiryAllDepartmentsProcessor
    {
        Task<IList<Department>> GetAllDepartmentsAsync();
        Task<IList<Department>> GetActiveDepartmentsAsync(bool active);
        Task<PagedList<Department>> GetDepartmentsAsync(DepartmentsResourceParameters departmentsResourceParameters);
    }
}