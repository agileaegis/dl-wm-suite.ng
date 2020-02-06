using System;
using System.Threading.Tasks;
using dl.wm.suite.cms.contracts.Devices;

namespace dl.wm.suite.cms.services.Devices
{
    public class DeleteDeviceProcessor : IDeleteDeviceProcessor
    {
        public Task DeleteDeviceAsync(Guid deviceToBeDeletedId)
        {
            throw new NotImplementedException();
        }
    }
}
