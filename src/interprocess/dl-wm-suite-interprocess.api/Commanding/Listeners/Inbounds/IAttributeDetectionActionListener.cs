using dl.wm.suite.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace dl.wm.suite.interprocess.api.Commanding.Listeners.Inbounds
{
    public interface IAttributeDetectionActionListener
    {
        void Update(object sender, AttributeDetectionEventArgs e);
    }
}