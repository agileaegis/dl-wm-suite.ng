using System;
using System.Threading.Tasks;
using dl.wm.suite.common.dtos.Vms.Tours;

namespace dl.wm.suite.cms.contracts.Tours
{
    public interface ICreateTourProcessor
    {
      Task<TourUiModel> CreateTourAsync(Guid accountIdToCreateThisTour, TourForCreationUiModel newTourUiModel);
    }
}