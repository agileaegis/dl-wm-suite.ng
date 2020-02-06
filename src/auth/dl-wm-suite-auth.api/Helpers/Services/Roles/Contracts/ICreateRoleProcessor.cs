using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Roles;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts
{
    public interface ICreateRoleProcessor
    {
        Task<RoleCreationUiModel> CreateRoleAsync(Guid accountIdToCreateThisRole, RoleForCreationUiModel newRoleUiModel);
    }
}