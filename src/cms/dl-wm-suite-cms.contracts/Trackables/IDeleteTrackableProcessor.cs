using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Trackables
{
    public interface IDeleteTrackableProcessor
    {
        Task DeleteTrackableAsync(Guid trackableToBeDeletedId);
    }
}