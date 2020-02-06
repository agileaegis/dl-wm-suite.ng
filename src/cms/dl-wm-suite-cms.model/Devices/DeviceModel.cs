using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class DeviceModel : EntityBase<Guid>, IAggregateRoot
    {

        public DeviceModel()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.IsActive = true;
            this.Devices = new HashSet<Device>();
        }

        public virtual string Name { get; set; }
        public virtual string CodeName { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual ISet<Device> Devices { get; set; }


        protected override void Validate()
        {
        }
    }
}
