using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.TrackingPoints;
using dl.wm.suite.fleet.model.TripLegs;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace dl.wm.suite.fleet.model.Locations
{
    public class Location : EntityBase<int>, IAggregateRoot
    {
        public Location()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.InterestLevel = 1;
        }

        //public virtual string Name { get; set; }
        //public virtual string Address { get; set; }
        public virtual int? MinimumWaitTime { get; set; }
        public virtual IGeometry Geo { get; set; }
        public virtual int InterestLevel { get; set; }
        public virtual TrackingPoint TrackingPoint { get; set; }
        public virtual TripLeg StartPoint { get; set; }
        public virtual TripLeg EndPoint { get; set; }

        protected override void Validate()
        {
            
        }

        public virtual void InjectWithLocation(double geoLat, double geoLong)
        {
            this.Geo = new Point(geoLat, geoLong)
            {
                SRID = 4326
            };
        }
    }
}
