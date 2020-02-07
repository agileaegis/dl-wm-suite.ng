using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Employees
{
    public interface IInquiryAllEmployeesProcessor
    {
      Task<PagedList<Employee>> GetEmployeesAsync(EmployeesResourceParameters employeesResourceParameters);
    }
}