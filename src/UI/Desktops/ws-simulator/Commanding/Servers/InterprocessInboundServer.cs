using ws.simulator.Commanding.Servers.Base;

namespace ws.simulator.Commanding.Servers
{
    public sealed class InterprocessInboundServer : InterprocessInboundBaseServer
    {
        private InterprocessInboundServer()
        {
            
        }
        public static InterprocessInboundServer GetInterprocessInboundServer { get; } = new InterprocessInboundServer();
    }
}