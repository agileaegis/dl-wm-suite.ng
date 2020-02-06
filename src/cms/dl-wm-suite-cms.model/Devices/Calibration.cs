using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class Calibration : EntityBase<Guid>, IAggregateRoot
    {
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
