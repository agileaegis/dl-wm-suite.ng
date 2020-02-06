using System;
using ws.simulator.Commanding.Events.EventArgs.Inbound;
using ws.simulator.Commanding.Listeners.Inbounds;

namespace ws.simulator.Commanding.Servers.Base
{
    public abstract class InterprocessInboundBaseServer
    {
        public event EventHandler<AckDetectionEventArgs> AckDetector;

        #region Ack detection Event Manipulation

        private void OnAckDetection(AckDetectionEventArgs e)
        {
            AckDetector?.Invoke(this, e);
        }

        public void RaiseAckDetection(byte[] payload)
        {
            OnAckDetection(new AckDetectionEventArgs(payload, true));
        }

        public void Attach(IAckDetectionActionListener listener)
        {
            AckDetector += listener.Update;
        }

        public void Detach(IAckDetectionActionListener listener)
        {
            AckDetector -= listener.Update;
        }

        #endregion
    }
}