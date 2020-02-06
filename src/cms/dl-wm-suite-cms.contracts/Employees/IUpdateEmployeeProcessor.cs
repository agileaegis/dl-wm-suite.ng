using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Employees;

namespace dl.wm.suite.cms.contracts.Employees
{
    public interface IUpdateEmployeeProcessor
    {
        Task<EmployeeUiModel> UpdateEmployeeAsync(EmployeeForModificationUiModel updatedEmployee);
    }
}
