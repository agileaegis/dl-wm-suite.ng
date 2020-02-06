﻿namespace dl.wm.suite.interprocess.api.Commanding.Events.EventArgs.Inbound
{
    public class TelemetryDetectionEventArgs : System.EventArgs
    {
        public string Payload { get; private set; }
        public bool Success { get; private set; }
        public string Imei { get; private set; }

        public TelemetryDetectionEventArgs(string payload, bool success, string imei)
        {
          this.Payload = payload;
          this.Success = success;
          this.Imei = imei;
        }
    }
}
