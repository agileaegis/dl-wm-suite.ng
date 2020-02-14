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
            
        }

        public virtual IGeometry Point { get; set; }
        public virtual int Altitude { get; set; }
        public virtual int Angle { get; set; }
        public virtual int Satellites { get; set; }
        public virtual int Speed { get; set; }

        public virtual Device Device { get; set; }
        
        protected override void Validate()
        {
        }
    }
}
