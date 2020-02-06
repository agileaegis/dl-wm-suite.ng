namespace dl.wm.suite.interprocess.api.Commanding.Events.EventArgs.Inbound
{
  public class AttributeDetectionEventArgs : System.EventArgs
  {
    public string Payload { get; private set; }
    public string Imei { get; private set; }
    public bool Success { get; private set; }

    public AttributeDetectionEventArgs(string payload, bool success, string imei)
    {
      this.Payload = payload;
      this.Imei = imei;
      this.Success = success;
    }
  }
}
