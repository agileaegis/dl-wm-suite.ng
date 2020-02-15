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
      this.ActivationCode = Guid.NewGuid();
      this.ProvisioningCode = Guid.NewGuid();
      this.ResetCode = Guid.NewGuid();

      this.ActivationDate = this.ProvisioningDate = this.ResetDate = DateTime.MinValue;
      this.CreatedDate = DateTime.Now;
      this.ModifiedDate = DateTime.MinValue;

      this.ModifiedBy = this.ActivatedBy = this.ResetBy = this.ProvisioningBy = Guid.Empty;

      this.IsEnabled = true;
      this.IsActivated = false;
      this.IsActive = true;

      this.Commands = new HashSet<Command>();
      this.Connections = new HashSet<ConnectionReport>();
      this.Calibrations = new HashSet<Calibration>();
      this.Firmwares = new HashSet<DeviceFirmware>();
      this.Measurements = new HashSet<Measurement>();
    }

    public virtual string Imei { get; set; }
    public virtual string SerialNumber { get; set; }
    public virtual Guid ActivationCode { get; set; }
    public virtual Guid ProvisioningCode { get; set; }
    public virtual Guid ResetCode { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual Guid CreatedBy { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual Guid ModifiedBy { get; set; }
    public virtual DateTime ActivationDate { get; set; }
    public virtual DateTime ProvisioningDate { get; set; }
    public virtual DateTime ResetDate { get; set; }
    public virtual Guid ActivatedBy { get; set; }
    public virtual Guid ProvisioningBy { get; set; }
    public virtual Guid ResetBy { get; set; }
    public virtual bool IsActivated { get; set; }
    public virtual bool IsEnabled { get; set; }
    public virtual bool IsActive { get; set; }

    public virtual Container Container { get; set; }

    public virtual DeviceModel DeviceModel { get; set; }

    public virtual Simcard Sim { get; set; }
    public virtual Location Location { get; set; }

    public virtual ISet<Command> Commands { get; set; }
    public virtual ISet<ConnectionReport> Connections { get; set; }
    public virtual ISet<Calibration> Calibrations { get; set; }
    public virtual ISet<DeviceFirmware> Firmwares { get; set; }
    public virtual ISet<Measurement> Measurements { get; set; }

    protected override void Validate()
    {

    }

    public virtual void InjectWithInitialAttributes(string deviceImei, string deviceSerialNumber)
    {
      this.Imei = deviceImei;
      this.SerialNumber = deviceSerialNumber;
    }

    public virtual void InjectWithAudit(Guid accountIdToCreateThisDevice)
    {
      this.CreatedBy = accountIdToCreateThisDevice;
    }

    public virtual void InjectWithSimacard(Simcard simcardToBeInjected)
    {
      this.Sim = simcardToBeInjected;
      simcardToBeInjected.Device = this;
    }

    public virtual void InjectWithDeviceModel(DeviceModel deviceModelToBeInjected)
    {
      this.DeviceModel = deviceModelToBeInjected;
      deviceModelToBeInjected.Devices.Add(this);
    }

    public virtual void UpdateWithAudit(Guid userAuditId)
    {
      this.ModifiedBy = userAuditId;
      this.ModifiedDate = DateTime.Now;
    }

    public virtual void ProvisioningWith(Guid userAuditId)
    {
      this.ProvisioningBy = userAuditId;
      this.ProvisioningDate = DateTime.Now;
    }
  }
}

