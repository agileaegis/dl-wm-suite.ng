using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.contracts.V1;

namespace dl.wm.suite.fleet.services.V1
{
    public class AssetsControllerDependencyBlock : IAssetsControllerDependencyBlock
    {
        public AssetsControllerDependencyBlock(ICreateAssetProcessor createAssetProcessor,
                                                        IInquiryAssetProcessor inquiryAssetProcessor,
                                                        IUpdateAssetProcessor updateAssetProcessor,
                                                        IInquiryAllAssetsProcessor allAssetProcessor,
                                                        IDeleteAssetProcessor deleteAssetProcessor)

        {
            CreateAssetProcessor = createAssetProcessor;
            InquiryAssetProcessor = inquiryAssetProcessor;
            UpdateAssetProcessor = updateAssetProcessor;
            InquiryAllAssetsProcessor = allAssetProcessor;
            DeleteAssetProcessor = deleteAssetProcessor;
        }

        public ICreateAssetProcessor CreateAssetProcessor { get; private set; }
        public IInquiryAssetProcessor InquiryAssetProcessor { get; private set; }
        public IUpdateAssetProcessor UpdateAssetProcessor { get; private set; }
        public IInquiryAllAssetsProcessor InquiryAllAssetsProcessor { get; private set; }
        public IDeleteAssetProcessor DeleteAssetProcessor { get; private set; }
    }
}