using dl.wm.suite.interprocess.api.Commanding.Servers.Base;

namespace dl.wm.suite.interprocess.api.Commanding.Servers
{
    public sealed class WmInboundServer : WmInboundBaseServer
    {
        private WmInboundServer()
        {
            
        }
        public static WmInboundServer GetWmInboundServer { get; } = new WmInboundServer();
    }
}
