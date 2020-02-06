using dl.wm.suite.fleet.api.Commanding.Events.Args;

namespace dl.wm.suite.fleet.api.Commanding.Listeners
{
    public interface IPointStoringActionListener
    {
        void Update(object sender, PointStoringEventArgs e);
    }
}