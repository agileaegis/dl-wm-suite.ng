using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace dl.wm.suite.cms.model.Devices
{
    public class Location : EntityBase<Guid>, IAggregateRoot
    {

        public Location()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.CreatedDate = DateTime.Now;
            this.ModifiedDate = DateTime.MinValue;
        }

        public virtual IGeometry Point { get; set; }
        public virtual double Altitude { get; set; }
        public virtual double Angle { get; set; }
        public virtual int Satellites { get; set; }
        public virtual double Speed { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual double Bearing { get; set; }
        public virtual int TimeToFix { get; set; }
        public virtual int StatusFlag { get; set; }
        public virtual int SignalLength { get; set; }
        public virtual DateTime Timestamp { get; set; }

        public virtual Device Device { get; set; }
        
        protected override void Validate()
        {
        }

        public virtual void InjectWithInitialAttributes(double geoLat, double geoLon, double altitude, double angle, int satellites, double speed, 
          double bearing, int timeToFix, int statusFlags, int signalLength, in DateTime timestamp)
        {
          this.Point = new Point(geoLat, geoLon)
          {
            SRID = 4326
          };

          this.Altitude = altitude;
          this.Angle = angle;
          this.Satellites = satellites;
          this.Speed = speed;
          this.Bearing = bearing;
          this.TimeToFix = timeToFix;
          this.StatusFlag = statusFlags;
          this.SignalLength = signalLength;
          this.Timestamp = timestamp;
        }

        public virtual void ModifiedWith()
        {
          this.ModifiedDate = DateTime.Now;
        }

    }
}
