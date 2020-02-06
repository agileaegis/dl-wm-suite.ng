using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Locations;

namespace dl.wm.suite.fleet.contracts.Locations
{
    public interface IInquiryLocationProcessor
    {
        Task<LocationUiModel> GetLocationAsync(int id);
    }
}