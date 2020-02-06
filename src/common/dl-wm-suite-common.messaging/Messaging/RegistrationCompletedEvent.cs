namespace dl.wm.suite.common.messaging.Messaging
{
  public class RegistrationCompletedEvent
  {
    public string Message { get; set; }
    public RegistrationCompletedEvent(string message)
    {
      Message = message;
    }
  }
}
