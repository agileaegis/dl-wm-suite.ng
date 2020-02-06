using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables
{
    public class TrackableAlreadyRegisteredException : Exception
    {
        public TrackableAlreadyRegisteredException(string imei)
        {
            TrackableImei = imei;
        }

        public string TrackableImei { get; set; }

        public override string Message => $"Trackable with Imei: {TrackableImei} already registered to a Trip";
    }
}