using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.models.DTOs.Employees;
using dl.wm.presenter.ServiceAgents.Contracts;
using dl.wm.presenter.ServiceAgents.Impls.Base;

namespace dl.wm.presenter.ServiceAgents.Impls
{
    public class EmployeesService : BaseService<EmployeeUiModel>, IEmployeesService
    {
        private static readonly string _serviceName = "EmployeesService";

        public EmployeesService() : base(_serviceName)
        {

        }

        public async Task<IList<EmployeeUiModel>> GetAllActiveEmployeesAsync(bool active)
        {
            UriBuilder builder = CreateUriBuilder();
            builder.Path += active.ToString();
            return await RequestProvider.GetAsync<IList<EmployeeUiModel>>(builder.ToString());
        }
    }
}