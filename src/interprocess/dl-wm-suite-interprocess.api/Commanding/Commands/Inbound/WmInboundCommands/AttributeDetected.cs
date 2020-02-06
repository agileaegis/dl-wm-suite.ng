using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using dl.wm.suite.interprocess.api.Commanding.Events.Inbound;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands
{
  internal class AttributeDetected : WmInboundCommand
  {
    private readonly string _payload;
    private readonly string _imei;

    public AttributeDetected(string payload, string imei)
    {
      _payload = payload;
      _imei = imei;
      EventRaisingBehavior = new AttributeDetectionEventRaising(_payload, imei);
    }
  }
}