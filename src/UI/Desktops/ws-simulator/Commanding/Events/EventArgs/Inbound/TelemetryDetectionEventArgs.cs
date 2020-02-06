namespace ws.simulator.Commanding.Events.EventArgs.Inbound
{
    public class AckDetectionEventArgs : System.EventArgs
    {
        public byte[] Payload { get; private set; }
        public bool Success { get; private set; }

        public AckDetectionEventArgs(byte[] payload, bool success)
        {
          this.Payload = payload;
          Success = success;
        }
    }
}
