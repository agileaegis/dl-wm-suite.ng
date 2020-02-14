using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours.Trackables;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Tours.Trips
{
  public class Trip : EntityBase<Guid>, IAggregateRoot
  {
    public Trip()
    {
      OnCreated();
    }

    private void OnCreated()
    {
      this.CreatedDate = DateTime.Now;
      this.ModifiedDate = this.CreatedDate;
      this.StartTime = this.CreatedDate;
      this.EndTime = this.CreatedDate;
      this.TrackingPoints = new HashSet<TrackingPoint>();
    }

    public virtual string Code { get; set; }

    public virtual DateTime CreatedDate { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual double Duration { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual string ModifiedBy { get; set; }
    public virtual DateTime StartTime { get; set; }
    public virtual DateTime EndTime { get; set; }
    public virtual bool IsActive { get; set; }

    public virtual Tour Tour { get; set; }
    public virtual Trackable Trackable { get; set; }

    public virtual ISet<TrackingPoint> TrackingPoints { get; set; }

    protected override void Validate()
    {

    }

    public virtual void InjectWithAuditCreation(string accountEmailToCreateThisTrip)
    {
      this.CreatedBy = accountEmailToCreateThisTrip;
    }

    public virtual void InjectWithAuditModification(string accountEmailToCreateThisTrip)
    {
      this.ModifiedDate = DateTime.Now;
      this.ModifiedBy = accountEmailToCreateThisTrip;
    }

    public virtual void InjectWithTrtackable(Trackable newTrackable)
    {
      this.Trackable = newTrackable;
      newTrackable.Trips.Add(this);
    }

    public virtual void InjectWithTrackingPoint(TrackingPoint newTrackingPointToBeAdded)
    {
      this.TrackingPoints.Add(newTrackingPointToBeAdded);
      newTrackingPointToBeAdded.Trip = this;
    }
  }
}