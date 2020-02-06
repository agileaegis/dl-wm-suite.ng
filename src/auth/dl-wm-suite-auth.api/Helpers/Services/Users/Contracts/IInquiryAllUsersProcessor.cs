using System.Threading.Tasks;
using dl.wm.suite.auth.api.Helpers.Models;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.auth.api.Helpers.Services.Users.Contracts
{
    public interface IInquiryAllUsersProcessor
    {
        Task<PagedList<User>> GetUsersAsync(UsersResourceParameters personsResourceParameters);
    }
}