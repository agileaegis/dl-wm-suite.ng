using System;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Repositories.Roles;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Impls
{
    public class DeleteRoleProcessor : IDeleteRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleProcessor(IUnitOfWork uOf,
            IRoleRepository roleRepository)
        {
            _uOf = uOf;
            _roleRepository = roleRepository;
        }

        public Task DeleteRoleAsync(Guid roleToBeDeletedId, Guid accountIdToDeleteThisRole)
        {
            throw new NotImplementedException();
        }
    }
}