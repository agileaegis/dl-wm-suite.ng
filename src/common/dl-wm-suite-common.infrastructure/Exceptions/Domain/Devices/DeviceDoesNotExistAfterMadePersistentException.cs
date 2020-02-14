using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class DeviceDoesNotExistAfterMadePersistentException : Exception
    {
        public string DeviceImei { get; }

        public DeviceDoesNotExistAfterMadePersistentException(string deviceImei)
        {
          DeviceImei = deviceImei;
        }

        public override string Message => $" Device with Imei: {DeviceImei} was not made Persistent!";
    }
}