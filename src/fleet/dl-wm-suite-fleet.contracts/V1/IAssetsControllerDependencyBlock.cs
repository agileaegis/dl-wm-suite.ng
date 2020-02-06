using dl.wm.suite.fleet.contracts.Assets;

namespace dl.wm.suite.fleet.contracts.V1
{
    public interface IAssetsControllerDependencyBlock
    {
        ICreateAssetProcessor CreateAssetProcessor { get; }
        IInquiryAssetProcessor InquiryAssetProcessor { get; }
        IUpdateAssetProcessor UpdateAssetProcessor { get; }
        IInquiryAllAssetsProcessor InquiryAllAssetsProcessor { get; }
        IDeleteAssetProcessor DeleteAssetProcessor { get; }
    }
}