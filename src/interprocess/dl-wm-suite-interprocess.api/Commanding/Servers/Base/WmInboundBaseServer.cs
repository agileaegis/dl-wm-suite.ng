using System;
using dl.wm.suite.interprocess.api.Commanding.Events.EventArgs.Inbound;
using dl.wm.suite.interprocess.api.Commanding.Listeners.Inbounds;

namespace dl.wm.suite.interprocess.api.Commanding.Servers.Base
{
    public abstract class WmInboundBaseServer
    {
        public event EventHandler<TelemetryRowDetectionEventArgs> TelemetryRowDetector;
        public event EventHandler<TelemetryDetectionEventArgs> TelemetryDetector;
        public event EventHandler<AttributeDetectionEventArgs> AttributeDetector;

        #region Telemetry detection Event Manipulation

        private void OnTelemetryDetection(TelemetryDetectionEventArgs e)
        {
            TelemetryDetector?.Invoke(this, e);
        }

        public void RaiseTelemetryDetection(string payload, string imei)
        {
            OnTelemetryDetection(new TelemetryDetectionEventArgs(payload, true, imei));
        }

        public void Attach(ITelemetryDetectionActionListener listener)
        {
            TelemetryDetector += listener.Update;
        }

        public void Detach(ITelemetryDetectionActionListener listener)
        {
            TelemetryDetector -= listener.Update;
        }

        #endregion

        #region Telemetry Row detection Event Manipulation

        private void OnTelemetryRowDetection(TelemetryRowDetectionEventArgs e)
        {
            TelemetryRowDetector?.Invoke(this, e);
        }

        public void RaiseTelemetryRowDetection(string payload, string imei)
        {
            OnTelemetryRowDetection(new TelemetryRowDetectionEventArgs(payload, true, imei));
        }

        public void Attach(ITelemetryRowDetectionActionListener listener)
        {
            TelemetryRowDetector += listener.Update;
        }

        public void Detach(ITelemetryRowDetectionActionListener listener)
        {
            TelemetryRowDetector -= listener.Update;
        }

        #endregion

        #region Attribute detection Event Manipulation

        private void OnAttributeDetection(AttributeDetectionEventArgs e)
        {
            AttributeDetector?.Invoke(this, e);
        }

        public void RaiseAttributeDetection(string payload, string imei)
        {
            OnAttributeDetection(new AttributeDetectionEventArgs(payload, true, imei));
        }

        public void Attach(IAttributeDetectionActionListener listener)
        {
            AttributeDetector += listener.Update;
        }

        public void Detach(IAttributeDetectionActionListener listener)
        {
            AttributeDetector -= listener.Update;
        }

        #endregion
    }
}