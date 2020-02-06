using System;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Repositories.Roles;
using dl.wm.suite.auth.api.Helpers.Services.Roles.Contracts;
using dl.wm.suite.common.dtos.Vms.Roles;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.auth.api.Helpers.Services.Roles.Impls
{
    public class InquiryRoleProcessor : IInquiryRoleProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IRoleRepository _roleRepository;
        public InquiryRoleProcessor(IRoleRepository roleRepository, IAutoMapper autoMapper)
        {
            _roleRepository = roleRepository;
            _autoMapper = autoMapper;
        }

        public Task<RoleUiModel> GetRoleByIdAsync(Guid id)
        {
            return Task.Run(() => _autoMapper.Map<RoleUiModel>(_roleRepository.FindBy(id)));
        }
    }
}