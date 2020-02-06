using dl.wm.suite.interprocess.api.Commanding.Servers.Base;

namespace dl.wm.suite.interprocess.api.Commanding.Events.Inbound.Base

{
    public interface IWmInboundEventRaisingBehavior
    {
        void RaiseWmEvent(WmInboundBaseServer inboundEventServer);
    }
}