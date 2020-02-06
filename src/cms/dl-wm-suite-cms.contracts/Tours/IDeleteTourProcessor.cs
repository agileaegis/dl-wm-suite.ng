using System;
using System.Threading.Tasks;

namespace dl.wm.suite.cms.contracts.Tours
{
    public interface IDeleteTourProcessor
    {
        Task DeleteTourAsync(Guid tourToBeDeletedId);
    }
}