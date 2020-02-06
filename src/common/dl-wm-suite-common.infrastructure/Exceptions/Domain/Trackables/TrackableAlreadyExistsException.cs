using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables
{
    public class TrackableAlreadyExistsException : Exception
    {
        public string Imei { get; }

        public string TrackableName { get; }


        public TrackableAlreadyExistsException(string trackableName)
        {
            TrackableName = trackableName;
        }

        public TrackableAlreadyExistsException(string message, string imei) : base(message)
        {
            this.Imei = imei;
        }

        public override string Message => $" Trackable with Name: {TrackableName} and Imei:{Imei}" +
                                          " already Exists!";
    }
}