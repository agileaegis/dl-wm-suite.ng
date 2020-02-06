using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Roles;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts
{
    public interface IInquiryRoleProcessor
    {
        Task<RoleUiModel> GetRoleByIdAsync(Guid id);
    }
}