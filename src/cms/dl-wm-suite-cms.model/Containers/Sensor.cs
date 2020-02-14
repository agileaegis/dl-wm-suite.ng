using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Containers
{
    public class Sensor : EntityBase<Guid>
    {
        public Sensor()
        {
            OnCreated();
        }

        private void OnCreated()
        {
        }

        public virtual int HighThreshold { get; set; }
        public virtual int LowThreshold { get; set; }


        public virtual Container Container { get; set; }

        protected override void Validate()
        {

        }
    }
}

