using System;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices.Firmware
{
    public class DeviceFirmware : EntityBase<Guid>, IAggregateRoot
    {
        public DeviceFirmware()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.IsCurrent = true;
            this.IsActive = true;
            this.CreatedDate = DateTime.Now;
            this.RegisteredDate = DateTime.Now;
        }

        public virtual bool IsCurrent { get; set; }
        public virtual DateTime RegisteredDate { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual FirmwareStatus Status { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual Device Device   { get; set; }
        public virtual Firmware Firmware   { get; set; }


        protected override void Validate()
        {
        }
    }
}
