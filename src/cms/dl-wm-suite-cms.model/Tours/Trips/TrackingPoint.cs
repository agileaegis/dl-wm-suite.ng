using System;
using dl.wm.suite.common.infrastructure.Domain;
using GeoAPI.Geometries;

namespace dl.wm.suite.cms.model.Tours.Trips
{
  public class TrackingPoint : EntityBase<Guid>
  {
    public TrackingPoint()
    {
      OnCreated();
    }

    private void OnCreated()
    {
    }

    /// <summary>
    /// The provider such as GPS, network, passive or fused
    /// </summary>
    public virtual string Provider { get; set; }

    /// <summary>
    /// The location provider
    /// </summary>
    public virtual string LocationProvider { get; set; }

    /// <summary>
    /// Estimated accuracy of this location, in meters.
    /// </summary>
    public virtual double? Accuracy { get; set; }

    /// <summary>
    /// Speed if it is available, in meters/second over ground.
    /// </summary>
    public virtual double? Speed { get; set; }

    /// <summary>
    /// Altitude if available, in meters above the WGS 84 reference ellipsoid.
    /// </summary>
    public virtual double? Altitude { get; set; }

    /// <summary>
    /// Bearing, in degrees.. Tolerance
    /// </summary>
    public virtual double? Bearing { get; set; }

    public virtual int MinimumWaitTime { get; set; }
    public virtual int InterestLevel { get; set; }
    public virtual IGeometry GeoPoint { get; set; }


    public virtual Trip Trip { get; set; }


    protected override void Validate()
    {

    }
  }
}