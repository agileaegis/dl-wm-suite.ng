using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;

namespace dl.wm.suite.fleet.contracts.Assets
{
    public interface IInquiryAssetProcessor
    {
        Task<AssetUiModel> GetAssetAsync(int id);
    }
}