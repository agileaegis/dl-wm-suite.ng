using dl.wm.suite.common.dtos.Vms.Tours;

namespace dl.wm.suite.cms.contracts.Tours
{
    public interface ICreateTourProcessor
    {
        TourUiModel CreateTourAsync(TourForCreationUiModel newTourUiModel);
    }
}