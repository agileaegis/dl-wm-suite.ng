using System.Threading.Tasks;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.common.infrastructure.Helpers.ResourceParameters;
using dl.wm.suite.common.infrastructure.Paging;

namespace dl.wm.suite.cms.contracts.Trackables
{
  public interface IInquiryAllTrackablesProcessor
  {
    Task<PagedList<Trackable>> GetAllPagedTrackablesAsync(TrackablesResourceParameters trackablesResourceParameters);
  }
}