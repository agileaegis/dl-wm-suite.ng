using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.models.DTOs.Employees.EmployeeRoles;
using dl.wm.presenter.ServiceAgents.Contracts.Base;

namespace dl.wm.presenter.ServiceAgents.Contracts
{
    public interface IEmployeeRolesService : IEntityService<EmployeeRoleUiModel>
    {
        Task<IList<EmployeeRoleUiModel>> GetAllActiveEmployeeRolesAsync(bool active);
    }
}