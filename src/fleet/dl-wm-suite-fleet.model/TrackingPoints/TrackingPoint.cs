using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.Locations;
using dl.wm.suite.fleet.model.Trips;

namespace dl.wm.suite.fleet.model.TrackingPoints
{
    public class TrackingPoint : EntityBase<int>, IAggregateRoot
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
        public virtual double? Course { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual Location Point { get; set; }

        protected override void Validate()
        {

        }

        public virtual void InjectWithLocation(Location newLocationToBeAdded)
        {
            this.Point = newLocationToBeAdded;
            newLocationToBeAdded.TrackingPoint = this;
        }
    }
}