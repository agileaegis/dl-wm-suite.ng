using System;
using System.Threading.Tasks;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts
{
    public interface IDeleteRoleProcessor
    {
        Task DeleteRoleAsync(Guid roleToBeDeletedId, Guid accountIdToDeleteThisRole);
    }
}