using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Coldairarrow.DotNettySocket;
using Serilog;
using ws.simulator.Commanding.Servers;

namespace ws.simulator.UDPs
{
  public class UdpConfiguration : IUdpConfiguration
  {
    private IUdpSocket _theClient;
    private EndPoint _point;

    public async Task EstablishConnection()
    {
      _theClient = await SocketBuilderFactory.GetUdpSocketBuilder()
        .OnClose(server => { Log.Information($"Server closed"); })
        .OnException(ex => { Log.Error($"Server exception:{ex.Message}"); })
        .OnRecieve((server, point, bytes) =>
        {
          Log.Information($"Server:Received from[{point.ToString()}]data:{Encoding.UTF8.GetString(bytes)}");

          _point = point;

          InterprocessInboundServer.GetInterprocessInboundServer.RaiseAckDetection(bytes);

        })
        .OnSend((server, point, bytes) =>
        {
          Log.Information(
            $"Server sends data:aims[{point.ToString()}]data:{Encoding.UTF8.GetString(bytes)}");
        })
        .OnStarted(server => { Log.Information($"Server startup"); }).BuildAsync();
    }

    public async Task SendMessageOnDemand(byte[] message, string udpServer)
    {
        await _theClient.Send(message, new IPEndPoint(IPAddress.Parse(udpServer), 6003));
        await Task.Delay(1000);
    }
  }
}
