using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound
{
    public interface IWmInboundCommandBuilder
    {
        WmInboundCommand Build(byte[] wmPackage);
    }
}