using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;

namespace dl.wm.suite.fleet.contracts.Assets
{
    public interface ICreateAssetProcessor
    {
        Task<AssetUiModel> CreateAssetAsync(string accountEmailToCreateThisAsset, AssetForCreationUiModel newAssetUiModel);
    }
}