using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips
{
    public class TripDoesNotExistForImeiException : Exception
    {
        public string TrackableImei { get; set; }

        public TripDoesNotExistForImeiException(string imei)
        {
            TrackableImei = imei;
        }

        public override string Message => $"Trip for Trackable with Imei: {TrackableImei}doesn't exist";

    }
}