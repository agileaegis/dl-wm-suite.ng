using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Employees.EmployeeRoles
{
    public interface IInquiryAllEmployeeRolesProcessor
    {
        Task<IList<EmployeeRole>> GetAllEmployeeRolesAsync();
        Task<IList<EmployeeRole>> GetActiveEmployeeRolesAsync(bool active);
        Task<PagedList<EmployeeRole>> GetEmployeeRolesAsync(EmployeeRolesResourceParameters employeeRolesResourceParameters);
    }
}