using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Users;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Users;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Users
{
    public class InquiryUserProcessor : IInquiryUserProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IUserRepository _userRepository;
        public InquiryUserProcessor(IUserRepository userRepository, IAutoMapper autoMapper)
        {
            _userRepository = userRepository;
            _autoMapper = autoMapper;
        }

        public Task<UserUiModel> GetUserByLoginAsync(string login)
        {
            return Task.Run(() => _autoMapper.Map<UserUiModel>(_userRepository.FindUserByLogin(login)));
        }

        public Task<UserForRetrievalUiModel> GetAuthUserByLoginAsync(string login)
        {
            return Task.Run(() => _autoMapper.Map<UserForRetrievalUiModel>(_userRepository.FindUserByLogin(login)));
        }
    }
}