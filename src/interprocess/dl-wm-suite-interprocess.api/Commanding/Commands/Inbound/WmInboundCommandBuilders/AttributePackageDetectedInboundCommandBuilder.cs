using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
  public class AttributePackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
  {
    private byte[] _wmPackage;

    public WmInboundCommand Build(byte[] wmPackage)
    {
      _wmPackage = wmPackage;
      return new AttributeDetected(BuildMessage(wmPackage), ExtractImei(wmPackage));
    }

    public override void BuildPayload()
    {
      //Todo: Refactoring to support abstract builder for Attribute JSON
    }
  }
}