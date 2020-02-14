using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;
using GeoAPI.Geometries;

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
            this.ModifiedDate = DateTime.Now;
        }

        public virtual IGeometry Point { get; set; }
        public virtual double Altitude { get; set; }
        public virtual double Angle { get; set; }
        public virtual int Satellites { get; set; }
        public virtual double Speed { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }

        public virtual Device Device { get; set; }
        
        protected override void Validate()
        {
        }
    }
}
