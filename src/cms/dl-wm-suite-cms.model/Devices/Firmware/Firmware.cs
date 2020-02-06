using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices.Firmware
{
    public class Firmware : EntityBase<Guid>, IAggregateRoot
    {

        public Firmware()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.DeviceFirmware = new HashSet<DeviceFirmware>();
        }

        public virtual string Name { get; set; }
        public virtual Guid Code { get; set; }
        public virtual byte[] Data { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual ISet<DeviceFirmware> DeviceFirmware { get; set; }


        protected override void Validate()
        {
        }
    }
}
