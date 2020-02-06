using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Persons;

namespace dl.wm.suite.auth.api.Helpers.Services.Persons
{
    public interface IInquiryPersonProcessor
    {
        Task<PersonUiModel> GetPersonAsync(Guid id);
        Task<PersonUiModel> GetPersonByEmailAsync(string email);
        Task<bool> SearchIfAnyPersonByEmailOrLoginExistsAsync(string login);
    }
}