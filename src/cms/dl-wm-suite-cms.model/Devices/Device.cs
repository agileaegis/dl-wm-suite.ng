using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Devices.Firmware;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class Device : EntityBase<Guid>, IAggregateRoot
    {
        public Device()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.ContainerTours = new HashSet<ContainerTour>();
            this.Commands = new HashSet<Command>();
            this.Connections = new HashSet<ConnectionReport>();
            this.Calibrations = new HashSet<Calibration>();
            this.Firmwares = new HashSet<DeviceFirmware>();
        }

        public virtual string Imei { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual Guid ActivationCode { get; set; }
        public virtual Guid ProvisioningCode { get; set; }
        public virtual Guid ResetCode { get; set; }
        public virtual DateTime ActivationDate { get; set; }
        public virtual DateTime ProvisioningDate { get; set; }
        public virtual DateTime ResetDate { get; set; }
        public virtual Guid ActivatedBy{ get; set; }
        public virtual Guid ProvisioningBy{ get; set; }
        public virtual Guid ResetBy{ get; set; }
        public virtual bool IsActivated{ get; set; }
        public virtual bool IsEnabled{ get; set; }

        public virtual DeviceModel DeviceModel { get; set; }
        public virtual Measurement Measurement  { get; set; }
        public virtual Simcard Sim   { get; set; }
        public virtual Location Location   { get; set; }
        
        public virtual ISet<ContainerTour> ContainerTours { get; set; }
        public virtual ISet<Command> Commands { get; set; }
        public virtual ISet<ConnectionReport> Connections { get; set; }
        public virtual ISet<Calibration> Calibrations { get; set; }
        public virtual ISet<DeviceFirmware> Firmwares { get; set; }

        protected override void Validate()
        {

        }
    }
}

