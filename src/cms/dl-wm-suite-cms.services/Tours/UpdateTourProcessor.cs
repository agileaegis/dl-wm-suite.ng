using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Tours;
using dl.wm.suite.common.dtos.Vms.Tours;

namespace dl.wm.suite.cms.services.Tours
{
    public class UpdateTourProcessor : IUpdateTourProcessor
    {
        public Task<TourUiModel> UpdateTourAsync(TourForModificationUiModel updatedTour)
        {
            throw new NotImplementedException();
        }
    }
}
