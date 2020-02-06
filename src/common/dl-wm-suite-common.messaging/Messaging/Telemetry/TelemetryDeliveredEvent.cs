namespace dl.wm.suite.common.messaging.Messaging.Telemetry
{
    public class TelemetryDeliveredEvent
    {
        public string Imei { get; private set; }
        public string Payload { get; private set; }

        public TelemetryDeliveredEvent(string imei, string payload)
        {
            Imei = imei;
            Payload = payload;
        }
    }
}
