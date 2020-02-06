using System;
using dl.wm.suite.dms.model.Devices;
using dl.wm.suite.dms.model.Measurements;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.dms.model.Devices
{
    public class Device : EntityBase<Guid>, IAggregateRoot
    {
        public Device()
        {
            OnCreated();
        }

        private void OnCreated()
        {
        }

        public virtual string Imei { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual bool IsActivated { get; set; }
        public virtual Guid ActivationCode { get; set; }
        public virtual Guid ProvisionCode { get; set; }
        public virtual Guid ResetCode { get; set; }
        public virtual DateTime ActivationDate { get; set; }
        public virtual DateTime ProvisionDate { get; set; }
        public virtual DateTime ResetDate { get; set; }
        public virtual Guid ActivationBy { get; set; }
        public virtual Guid ProvisionBy { get; set; }
        public virtual Guid ResetBy { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual DeviceStatus Status { get; set; }

        public virtual Measurement Measurement { get; set; }

        protected override void Validate()
        {
        }
    }
}