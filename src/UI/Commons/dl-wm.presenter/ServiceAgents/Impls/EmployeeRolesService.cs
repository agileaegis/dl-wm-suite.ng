using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.models.DTOs.Employees.EmployeeRoles;
using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls.Base;

namespace dl.wm.presenter.ServiceAgents.Impls
{
    public class EmployeeRolesService : BaseService<EmployeeRoleUiModel>, IEmployeeRolesService
    {
        private static readonly string _serviceName = "EmployeeRolesService";

        public EmployeeRolesService() : base(_serviceName)
        {

        }
        
        public async Task<IList<EmployeeRoleUiModel>> GetAllActiveEmployeeRolesAsync(bool active)
        {
            UriBuilder builder = CreateUriBuilder();
            builder.Path += active.ToString();
            return await RequestProvider.GetAsync<IList<EmployeeRoleUiModel>>(builder.ToString());
        }
    }
}