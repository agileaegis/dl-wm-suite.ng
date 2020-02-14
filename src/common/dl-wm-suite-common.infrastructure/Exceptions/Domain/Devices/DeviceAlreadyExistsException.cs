using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class DeviceAlreadyExistsException : Exception
    {
      public string BrokenRules { get; }
      public string DeviceImei { get; }


        public DeviceAlreadyExistsException(string imei, string brokenRules)
        {
          BrokenRules = brokenRules;
          BrokenRules = brokenRules;
        }

        public override string Message => $" Device with Imei: {DeviceImei} already Exists! Details : {BrokenRules}";
    }
}