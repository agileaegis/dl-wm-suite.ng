using dl.wm.suite.interprocess.api.Commanding.Events.EventArgs.Inbound;
using dl.wm.suite.interprocess.api.Commanding.Listeners.Inbounds;
using dl.wm.suite.interprocess.api.Commanding.Servers;
using Coldairarrow.DotNettySocket;
using Serilog;

namespace dl.wm.suite.interprocess.api.WSs
{
  public class WsConfiguration : IWsConfiguration, ITelemetryDetectionActionListener, IAttributeDetectionActionListener
  {
    private IWebSocketServer _theServer;
    private IWebSocketConnection _theConnection;

    public WsConfiguration()
    {
      WmInboundServer.GetWmInboundServer.Attach((ITelemetryDetectionActionListener) this);
      WmInboundServer.GetWmInboundServer.Attach((IAttributeDetectionActionListener) this);
    }

    public async void EstablishConnection()
    {
      _theServer = await SocketBuilderFactory.GetWebSocketServerBuilder(6002)
        .OnConnectionClose((server, connection) =>
        {
          Log.Information(
            ($"Connection closed,Connection name[{connection.ConnectionName}],Current connection number:{server.GetConnectionCount()}"
            ));
        })
        .OnException(ex => { Log.Error($"Server exception:{ex.Message}"); })
        .OnNewConnection((server, connection) =>
        {
          connection.ConnectionName = $"1st Name{connection.ConnectionId}";
          _theConnection = connection;
          Log.Information(
            $"New connection:{connection.ConnectionName},Current connection number:{server.GetConnectionCount()}");
        })
        .OnRecieve((server, connection, msg) =>
        {
          _theConnection?.Send("ACK");
          Log.Information($"Server:Data{msg}");
        })
        .OnSend((server, connection, msg) =>
        {
          Log.Information($"Connection name[{connection.ConnectionName}]send Daa:{msg}");
        })
        .OnServerStarted(server => { Log.Information($"Service Start"); }).BuildAsync();
    }

    public void Update(object sender, TelemetryDetectionEventArgs e)
    {
      _theConnection?.Send(e.Payload);
      Log.Information($"Update for ITelemetryDetectionActionListener was caught");
    }

    public void Update(object sender, AttributeDetectionEventArgs e)
    {
      _theConnection?.Send(e.Payload);
      Log.Information($"Update for IAttributeDetectionActionListener was caught");
    }
  }
}
