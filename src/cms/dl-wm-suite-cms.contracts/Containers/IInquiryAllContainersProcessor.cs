using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Containers
{
    public interface IInquiryAllContainersProcessor
    {
        Task<PagedList<Container>> GetContainersAsync(ContainersResourceParameters containersResourceParameters);
    }
}