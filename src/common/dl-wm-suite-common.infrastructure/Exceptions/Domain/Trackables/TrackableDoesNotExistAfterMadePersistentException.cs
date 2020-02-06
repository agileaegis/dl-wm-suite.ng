using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables
{
    public class TrackableDoesNotExistAfterMadePersistentException : Exception
    {
        public string TrackableImei { get; }

        public TrackableDoesNotExistAfterMadePersistentException(string trackableImei)
        {
          TrackableImei = trackableImei;
        }

        public TrackableDoesNotExistAfterMadePersistentException(string message, string trackableImei) : base(message)
        {
            this.TrackableImei = trackableImei;
        }

        public override string Message => $" Trackable with Imei:{TrackableImei}, was not made Persistent!";
    }
}