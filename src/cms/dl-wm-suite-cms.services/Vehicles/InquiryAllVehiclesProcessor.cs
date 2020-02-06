using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Vehicles;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.cms.repository.ContractRepositories;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.Extensions;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.common.infrastructure.PropertyMappings;
using dl.wm.suite.common.infrastructure.TypeMappings;

namespace dl.wm.suite.cms.services.Vehicles
{
    public class InquiryAllVehiclesProcessor : IInquiryAllVehiclesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public InquiryAllVehiclesProcessor(IAutoMapper autoMapper,
            IVehicleRepository vehicleRepository, IPropertyMappingService propertyMappingService)
        {
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
            _propertyMappingService = propertyMappingService;
        }

        public Task<List<VehicleUiModel>> GetAllVehiclesAsync()
        {
            return Task.Run(() =>  _vehicleRepository.FindAll().Select(x => _autoMapper.Map<VehicleUiModel>(x)).ToList());
        }

        public Task<List<VehicleUiModel>> GetActiveVehiclesAsync(bool active)
        {
           return Task.Run(() =>  _vehicleRepository.FindAllActiveVehicles().Select(x => _autoMapper.Map<VehicleUiModel>(x)).ToList());
        }

        public Task<PagedList<Vehicle>> GetAllActivePagedVehiclesAsync(VehiclesResourceParameters vehiclesResourceParameters)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_vehicleRepository
                        .FindAllActiveVehiclesPagedOf(vehiclesResourceParameters.PageIndex,
                            vehiclesResourceParameters.PageSize),
                    vehiclesResourceParameters.OrderBy + " " + vehiclesResourceParameters.SortDirection,
                    _propertyMappingService.GetPropertyMapping<VehicleUiModel, Vehicle>());


            if (!string.IsNullOrEmpty(vehiclesResourceParameters.Filter) && !string.IsNullOrEmpty(vehiclesResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = vehiclesResourceParameters.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = vehiclesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Vehicle>)collectionBeforePaging.QueriedItems
                    .AsEnumerable().FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            return Task.Run(() => PagedList<Vehicle>.Create(collectionBeforePaging,
                vehiclesResourceParameters.PageIndex,
                vehiclesResourceParameters.PageSize));
        }
    }
}
