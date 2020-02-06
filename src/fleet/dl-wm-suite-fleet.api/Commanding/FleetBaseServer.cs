using System;
using dl.wm.suite.fleet.api.Commanding.Events.Args;
using dl.wm.suite.fleet.api.Commanding.Listeners;

namespace dl.wm.suite.fleet.api.Commanding
{
  public abstract class FleetBaseServer
  {
    public event EventHandler<PointStoringEventArgs> PointStoringDetector;

    #region Keep Alive Request detection Event Manipulation

    private void OnPointStoringDetection(PointStoringEventArgs e)
    {
        PointStoringDetector?.Invoke(this, e);
    }

    public void RaisePointStoringDetection()
    {
        OnPointStoringDetection(new PointStoringEventArgs());
    }

    public void Attach(IPointStoringActionListener listener)
    {
        PointStoringDetector += listener.Update;
    }

    public void Detach(IPointStoringActionListener listener)
    {
        PointStoringDetector -= listener.Update;
    }

    #endregion
  }
}