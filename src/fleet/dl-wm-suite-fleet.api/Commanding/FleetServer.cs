using dl.wm.suite.fleet.api.Commanding;

public sealed class FleetServer : FleetBaseServer
{
    private FleetServer()
    {

    }
    public static FleetServer GetFleetServer { get; } = new FleetServer();
}