using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables
{
    public class TrackableDoesNotExistException : Exception
    {
        public int TrackableId { get; set; }
        public string TrackableImei { get; set; }

        public TrackableDoesNotExistException(int id)
        {
            TrackableId = id;
        }

        public TrackableDoesNotExistException(string imei)
        {
            TrackableImei = imei;
        }

        public override string Message => $"Trackable with id: {TrackableId} or Imei: {TrackableImei} doesn't exist";
    }
}