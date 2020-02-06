using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class DeviceDoesNotExistException : Exception
    {
        public Guid DeviceId { get; set; }

        public DeviceDoesNotExistException(Guid id)
        {
            DeviceId = id;
        }

        public override string Message => $"Device with id: {DeviceId} doesn't exist";
    }
}