using System;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.Trips;
using GeoAPI.Geometries;

namespace dl.wm.suite.fleet.model.TripLegs
{
    public class TripLeg : EntityBase<int>, IAggregateRoot
    {
        public TripLeg()
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

        public virtual Trip Trip { get; set; }
        public virtual Location StartPoint { get; set; }
        public virtual Location EndPoint { get; set; }
        public virtual IGeometry Route { get; set; }

        protected override void Validate()
        {

        }
    }
}