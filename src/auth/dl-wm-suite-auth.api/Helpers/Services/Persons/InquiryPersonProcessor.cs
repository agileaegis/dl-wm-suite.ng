using System;
using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Repositories.Persons;
using dl.wm.suite.common.dtos.Vms.Persons;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.auth.api.Helpers.Services.Persons
{
    public class InquiryPersonProcessor : IInquiryPersonProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPersonRepository _personRepository;
        public InquiryPersonProcessor(IPersonRepository personRepository, IAutoMapper autoMapper)
        {
            _personRepository = personRepository;
            _autoMapper = autoMapper;
        }

        public Task<PersonUiModel> GetPersonAsync(Guid id)
        {
            return Task.Run(() => _autoMapper.Map<PersonUiModel>(_personRepository.FindBy(id)));
        }

        public Task<PersonUiModel> GetPersonByEmailAsync(string email)
        {
            return Task.Run(() => _autoMapper.Map<PersonUiModel>(_personRepository.FindPersonByEmail(email)));
        }

        public Task<bool> SearchIfAnyPersonByEmailOrLoginExistsAsync(string login)
        {
            return Task.Run(() =>  _personRepository.FindPersonsByEmailOrLogin(login).Count > 0);
        }
    }
}