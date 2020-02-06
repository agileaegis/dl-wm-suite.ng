using System.Threading.Tasks;

namespace ws.simulator.UDPs
{
    public interface IUdpConfiguration
    {
        Task EstablishConnection();
        Task SendMessageOnDemand(byte[] message, string udpServer);
    }
}