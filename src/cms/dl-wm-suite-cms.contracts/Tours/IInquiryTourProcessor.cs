using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Tours;

namespace dl.wm.suite.cms.contracts.Tours
{
    public interface IInquiryTourProcessor
    {
        Task<TourUiModel> GetTourAsync(Guid id);
        Task<TourForAssignTrackableModel> GetTodayAssignedTourAsync(Guid userId);
    }
}