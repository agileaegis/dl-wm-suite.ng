using System.Collections.Generic;
using System.Threading.Tasks;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;
using dl.wm.suite.fleet.model.Trackables;

namespace dl.wm.suite.fleet.contracts.Trackables
{
  public interface IInquiryAllTrackablesProcessor
  {
    Task<PagedList<Trackable>> GetAllPagedTrackablesAsync(TrackablesResourceParameters trackablesResourceParameters);
  }
}