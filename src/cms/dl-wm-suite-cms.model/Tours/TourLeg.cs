using System;
using dl.wm.suite.common.infrastructure.Domain;
using GeoAPI.Geometries;

namespace dl.wm.suite.cms.model.Tours
{
  public class TourLeg : EntityBase<Guid>, IAggregateRoot
  {
    public TourLeg()
    {
      OnCreated();
    }

    private void OnCreated()
    {
      this.CreatedDate = DateTime.Now;
    }

    public virtual DateTime CreatedDate { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual DateTime StartTime { get; set; }
    public virtual DateTime EndTime { get; set; }
    public virtual double AverageSpeed { get; set; }
    public virtual bool IsActive { get; set; }

    public virtual Tour Tour { get; set; }
    public virtual IGeometry StartPoint { get; set; }
    public virtual IGeometry EndPoint { get; set; }
    public virtual IGeometry Route { get; set; }

    protected override void Validate()
    {

    }
  }
}