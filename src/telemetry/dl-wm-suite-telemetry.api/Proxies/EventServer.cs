namespace dl.wm.suite.telemetry.api.Proxies
{
    public sealed class EventServer : EventBaseServer
    {
        private EventServer()
        {
        }

        public static EventServer GetServer { get; } = new EventServer();
    }
}