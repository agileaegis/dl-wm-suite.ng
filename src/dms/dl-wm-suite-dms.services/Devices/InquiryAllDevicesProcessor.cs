using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Devices;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;
using dl.wm.suite.dms.contracts.Devices;
using dl.wm.suite.dms.model.Devices;
using dl.wm.suite.dms.repository.ContractRepositories;
using dl.wms.uite.dms.contracts.Devices;

namespace dl.wm.suite.dms.services.Devices
{
    public class InquiryAllDevicesProcessor : IInquiryAllDevicesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDeviceRepository _vehicleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllDevicesProcessor(IAutoMapper autoMapper,
            IDeviceRepository vehicleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<List<DeviceUiModel>> GetAllDevicesAsync()
        {
            return Task.Run(() =>  _vehicleRepository.FindAll().Select(x => _autoMapper.Map<DeviceUiModel>(x)).ToList());
        }

        public Task<List<DeviceUiModel>> GetActiveDevicesAsync(bool active)
        {
           return Task.Run(() =>  _vehicleRepository.FindAllActiveDevices().Select(x => _autoMapper.Map<DeviceUiModel>(x)).ToList());
        }

        public Task<PagedList<Device>> GetAllActivePagedDevicesAsync(DevicesResourceParameters vehiclesResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_vehicleRepository
                        .FindAllActiveDevicesPagedOf(vehiclesResourceParameters.PageIndex,
                            vehiclesResourceParameters.PageSize),
                    vehiclesResourceParameters.OrderBy + " " + vehiclesResourceParameters.SortDirection,
                    _propertyMappingService.GetPropertyMapping<DeviceUiModel, Device>());


            if (!string.IsNullOrEmpty(vehiclesResourceParameters.Filter) && !string.IsNullOrEmpty(vehiclesResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = vehiclesResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = vehiclesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Device>)collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Device>.Create(collectionBeforePaging,
                vehiclesResourceParameters.PageIndex,
                vehiclesResourceParameters.PageSize));
        }
    }
}
