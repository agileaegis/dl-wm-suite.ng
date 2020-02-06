using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.services.Vehicles
{
    public class InquiryAllToursProcessor : IInquiryAllToursProcessor
    {
        public Task<IList<Tour>> GetAllToursAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Tour>> GetActiveToursAsync(bool active)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Tour>> GetToursAsync(ToursResourceParameters toursResourceParameters)
        {
            throw new NotImplementedException();
        }
    }
}
