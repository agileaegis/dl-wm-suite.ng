using System;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Repositories.Roles;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts;
using dl.wm.suite.common.dtos.Vms.Roles;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.common.infrastructure.UnitOfWorks;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Impls
{
    public class UpdateRoleProcessor : IUpdateRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IRoleRepository _roleRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IRoleRepository roleRepository)
        {
            _uOf = uOf;
            _roleRepository = roleRepository;
            _autoMapper = autoMapper;
        }

        public Task<RoleUiModel> UpdateRoleAsync(Guid roleIdToBeUpdated, Guid accountIdToBeUpdatedThisRole, RoleForModificationUiModel updatedRole)
        {
            throw new NotImplementedException();
        }
    }
}