using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.Trackables;
using dl.wm.suite.fleet.model.TrackingPoints;
using dl.wm.suite.fleet.model.TripLegs;

namespace dl.wm.suite.fleet.model.Trips
{
    public class Trip : EntityBase<int>, IAggregateRoot
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
            this.Legs = new HashSet<TripLeg>();
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
        public virtual TrackableAsset DeviceAsset { get; set; }

        public virtual ISet<TrackingPoint> TrackingPoints { get; set; }
        public virtual ISet<TripLeg> Legs { get; set; }

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

        public virtual void InjectWithDeviceAsset(TrackableAsset newTrackableAsset)
        {
            this.DeviceAsset = newTrackableAsset;
            newTrackableAsset.Trips.Add(this);
        }

        public virtual void InjectWithTrackingPoint(TrackingPoint newTrackingPointToBeAdded)
        {
            this.TrackingPoints.Add(newTrackingPointToBeAdded);
            newTrackingPointToBeAdded.Trip = this;
        }
    }
}