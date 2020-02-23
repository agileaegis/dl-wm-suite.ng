using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Devices;
using dl.wm.suite.common.infrastructure.Domain;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace dl.wm.suite.cms.model.Containers
{
  public class Container : EntityBase<Guid>, IAggregateRoot
  {
    public Container()
    {
      OnCreated();
    }

    private void OnCreated()
    {
      this.IsActive = true;
      this.CreatedDate = DateTime.Now;
      this.ModifiedDate = CreatedDate;
      this.LastServicedDate = CreatedDate;

      this.Type = ContainerType.Waste;
      this.Status = ContainerStatus.Normal;
      this.WasteType = WasteType.Trash;
      this.Material = Material.HDPE;
      this.IsFixed = true;
      this.Capacity = 1100;
      this.UsefulLoad = 520;

      this.ContainerTours = new HashSet<ContainerTour>();
    }

    public virtual string Name { get; set; }
    public virtual int Level { get; set; }
    public virtual int FillLevel { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual IGeometry Geo { get; set; }
    public virtual double TimeFull { get; set; }
    public virtual DateTime LastServicedDate { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual Guid CreatedBy { get; set; }
    public virtual Guid ModifiedBy { get; set; }
    public virtual string ImagePath { get; set; }
    public virtual ContainerType Type { get; set; }
    public virtual ContainerStatus Status { get; set; }
    public virtual string Address { get; set; }
    public virtual DateTime MandatoryPickupDate { get; set; }
    public virtual bool MandatoryPickupActive { get; set; }
    public virtual int Capacity { get; set; }
    public virtual int UsefulLoad { get; set; }
    public virtual WasteType WasteType { get; set; }
    public virtual Material Material { get; set; }
    public virtual bool IsFixed { get; set; }
    public virtual string Description { get; set; }


    public virtual Sensor Sensor { get; set; }
    public virtual Device Device { get; set; }
    public virtual ISet<ContainerTour> ContainerTours { get; set; }
    public virtual ISet<ContainerServiceLog> ServiceLogs { get; set; }

    protected override void Validate()
    {
    }

    public virtual void InjectWithAudit(Guid accountIdToCreateThisContainer)
    {
      this.CreatedBy = accountIdToCreateThisContainer;
    }

    public virtual void InjectWithLocation(double geoLat, double geoLong)
    {
      this.Geo = new Point(geoLat, geoLong)
      {
        SRID = 4326
      };
    }

    public virtual void SoftDeleted()
    {
      this.IsActive = false;
    }

    public virtual void UpdateWithAudit(Guid userAuditId)
    {
      this.ModifiedBy = userAuditId;
      this.ModifiedDate = DateTime.Now;
    }

    public virtual void InjectWithDevice(Device deviceToBeProvisioned)
    {
      this.Device = deviceToBeProvisioned;
      deviceToBeProvisioned.Container = this;
    }
  }
}

