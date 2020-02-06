using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.Assets;
using dl.wm.suite.fleet.model.Trips;

namespace dl.wm.suite.fleet.model.Trackables
{
    public class TrackableAsset : EntityBase<int>
    {
        public TrackableAsset()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.IsEnabled = true;
            this.RegisteredDate = DateTime.Now;
            this.Trips = new HashSet<Trip>();
        }

        public virtual DateTime RegisteredDate { get; set; }
        public virtual string RegisteredBy { get; set; }
        public virtual bool IsEnabled { get; set; }

        public virtual Trackable Device { get; set; }
        public virtual Asset Asset { get; set; }

        public virtual ISet<Trip> Trips { get; set; }

        protected override void Validate()
        {
        }

        public virtual void InjectWithCreationAudit(string accountEmailToUpdateThisTrip)
        {
            this.RegisteredBy = accountEmailToUpdateThisTrip;
        }

        public virtual void InjectWithAsset(Asset assetToBeInjected)
        {
            this.Asset = assetToBeInjected;
            assetToBeInjected.TrackableAssets.Add(this);
        }
        public virtual void InjectWithTrackable(Trackable trackableToBeInjected)
        {
            this.Device = trackableToBeInjected;
            trackableToBeInjected.TrackableAssets.Add(this);
        }

        public virtual void UnregisterDeviceTrip()
        {
            this.IsEnabled = false;
        }
    }
}
