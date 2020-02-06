using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Tours
{
    public interface IInquiryAllToursProcessor
    {
        Task<IList<Tour>> GetAllToursAsync();
        Task<IList<Tour>> GetActiveToursAsync(bool active);
        Task<PagedList<Tour>> GetToursAsync(ToursResourceParameters toursResourceParameters);
    }
}