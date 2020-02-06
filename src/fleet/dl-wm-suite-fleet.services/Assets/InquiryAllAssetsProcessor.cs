using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Assets;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.fleet.contracts.Assets;
using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.repository.ContractRepositories;

namespace dl.wm.suite.fleet.services.Assets
{
    public class InquiryAllAssetsProcessor : IInquiryAllAssetsProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IAssetRepository _assetRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllAssetsProcessor(IAutoMapper autoMapper,
            IAssetRepository assetRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _assetRepository = assetRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<PagedList<Asset>> GetAllAssetsAsync(AssetsResourceParameters assetsResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_assetRepository
                        .FindAllActiveAssetsPagedOf(assetsResourceParameters.PageIndex,
                            assetsResourceParameters.PageSize),
                    assetsResourceParameters.OrderBy + " " + assetsResourceParameters.SortDirection,
                    _propertyMappingService.GetPropertyMapping<AssetUiModel, Asset>());


            if (!string.IsNullOrEmpty(assetsResourceParameters.Filter) && !string.IsNullOrEmpty(assetsResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = assetsResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = assetsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Asset>)collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Asset>.Create(collectionBeforePaging,
                assetsResourceParameters.PageIndex,
                assetsResourceParameters.PageSize));
        }
    }
}
