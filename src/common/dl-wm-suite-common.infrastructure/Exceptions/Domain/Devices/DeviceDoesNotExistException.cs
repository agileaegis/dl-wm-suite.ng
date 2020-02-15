using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class DeviceDoesNotExistException : Exception
    {
      public string Imei { get; }
      public Guid DeviceId { get; set; }

        public DeviceDoesNotExistException(Guid id)
        {
            DeviceId = id;
        }
        public DeviceDoesNotExistException(string imei)
        {
          Imei = imei;
        }

        public override string Message => $"Device with id: {DeviceId} or Imei: {Imei} doesn't exist";
    }
}