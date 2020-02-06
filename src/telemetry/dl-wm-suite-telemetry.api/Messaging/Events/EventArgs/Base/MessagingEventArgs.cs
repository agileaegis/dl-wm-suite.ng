using System;

namespace dl.wm.suite.telemetry.api.Messaging.Events.EventArgs.Base
{
    public abstract class MessagingEventArgs : System.EventArgs
    {
        public Guid? CorrelationId { get; private set; }

        protected MessagingEventArgs(Guid? correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}