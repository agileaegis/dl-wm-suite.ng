using System;
using dl.wm.suite.telemetry.api.Messaging.Events.EventArgs.Base;

namespace dl.wm.suite.telemetry.api.Messaging.Events.EventArgs
{
    public class TelemetryRowEventArgs : MessagingEventArgs
    {
        public string Imei { get; private set; }
        public string Payload { get; private set; }

        public TelemetryRowEventArgs(string imei, string payload)
            : base(Guid.NewGuid())
        {
            Payload = payload;
            Imei = imei;
        }
    }
}