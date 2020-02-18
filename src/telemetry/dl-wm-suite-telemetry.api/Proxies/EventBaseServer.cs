using System;
using dl.wm.suite.telemetry.api.Messaging.Events.EventArgs;
using dl.wm.suite.telemetry.api.Messaging.Events.Listeners;

namespace dl.wm.suite.telemetry.api.Proxies
{
    public abstract class EventBaseServer
    {
        public event EventHandler<TelemetryRowEventArgs> TelemetryRowDetector;


        #region ProcessingDetector

        private void OnTelemetryRowDetection(TelemetryRowEventArgs e)
        {
            TelemetryRowDetector?.Invoke(this, e);
        }

        public void RaiseTelemetryRowDetection(string imei, string payload)
        {
            OnTelemetryRowDetection(new TelemetryRowEventArgs(imei, payload));
        }

        public void Attach(ITelemetryRowDetectionActionListener listener)
        {
            if (TelemetryRowDetector == null)
            {
                TelemetryRowDetector += (sender, e) => listener.Update(sender, e);
            }
        }

        public void Detach(ITelemetryRowDetectionActionListener listener) =>
            TelemetryRowDetector -= (sender, e) => listener.Update(sender, e);

        #endregion
    }
}
