using ws.simulator.Commanding.Events.EventArgs.Inbound;

namespace ws.simulator.Commanding.Listeners.Inbounds
{
    public interface IAckDetectionActionListener
    {
        void Update(object sender, AckDetectionEventArgs e);
    }
}