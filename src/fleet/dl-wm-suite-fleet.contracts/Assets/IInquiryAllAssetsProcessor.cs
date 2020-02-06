using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.fleet.model.Assets;

namespace dl.wm.suite.fleet.contracts.Assets
{
    public interface IInquiryAllAssetsProcessor
    {
        Task<PagedList<Asset>> GetAllAssetsAsync(AssetsResourceParameters assetsResourceParameters);
    }
}