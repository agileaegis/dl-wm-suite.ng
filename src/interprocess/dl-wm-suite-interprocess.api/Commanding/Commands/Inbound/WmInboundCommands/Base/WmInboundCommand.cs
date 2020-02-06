using dl.wm.suite.interprocess.api.Commanding.Commands.Base;
using dl.wm.suite.interprocess.api.Commanding.Events.Inbound.Base;
using dl.wm.suite.interprocess.api.Commanding.Servers.Base;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base
{
    public abstract class WmInboundCommand : WmCommand
    {
        public IWmInboundEventRaisingBehavior EventRaisingBehavior { get; set; }

        public void RaiseWmEvent(WmInboundBaseServer inboundEventServer)
        {
            EventRaisingBehavior.RaiseWmEvent(inboundEventServer);
        }
    }
}