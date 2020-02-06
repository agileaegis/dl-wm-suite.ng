using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Containers
{
    public class ContainerServiceLog : EntityBase<Guid>, IAggregateRoot
    {
        public ContainerServiceLog()
        {
            OnCreated();
        }

        private void OnCreated()
        {
        }

        public virtual string Notes { get; set; }
        public virtual int Level { get; set; }
        public virtual double TimeRemain { get; set; }
        public virtual DateTime StartTimestampServicing { get; set; }
        public virtual DateTime EndTimestampServicing { get; set; }
        public virtual DateTime ServicedDate { get; set; }

        public virtual Container Container { get; set; }

        protected override void Validate()
        {

        }
    }
}

