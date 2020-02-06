using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.dtos.Vms.Vehicles;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Vehicles
{
    public interface IInquiryAllVehiclesProcessor
    {
        Task<List<VehicleUiModel>> GetAllVehiclesAsync();
        Task<List<VehicleUiModel>> GetActiveVehiclesAsync(bool active);
        Task<PagedList<Vehicle>> GetAllActivePagedVehiclesAsync(VehiclesResourceParameters vehiclesResourceParameters);
    }
}