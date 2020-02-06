using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class ConnectionReport : EntityBase<Guid>, IAggregateRoot
    {
        public virtual DateTime ConnectionDate { get; set; }
        public virtual ConnectionType ConnectionType { get; set; }
        public virtual Device Device { get; set; }
        protected override void Validate()
        {
        }
    }
}
