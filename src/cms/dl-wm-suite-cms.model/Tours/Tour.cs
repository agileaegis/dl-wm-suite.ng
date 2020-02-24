using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Employees;
using dl.wm.suite.cms.model.Tours.Trips;
using dl.wm.suite.cms.model.Vehicles;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Tours
{
  public class Tour : EntityBase<Guid>, IAggregateRoot
  {
    public Tour()
    {
      OnCreated();
    }

    private void OnCreated()
    {
      this.ScheduledDate = DateTime.Now;
      this.CreatedDate = DateTime.Now;
      this.ModifiedDate = DateTime.MinValue;
      this.Type = TourType.Auto;
      this.Status = TourStatus.Scheduled;
      this.EmployeesTours = new HashSet<EmployeeTour>();
      this.ContainerTours = new HashSet<ContainerTour>();
      this.TourLegs = new HashSet<TourLeg>();
    }

    public virtual string Name { get; set; }
    public virtual DateTime ScheduledDate { get; set; }
    public virtual TourType Type { get; set; }
    public virtual TourStatus Status { get; set; }

    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime ModifiedDate { get; set; }
    public virtual Guid CreatedBy { get; set; }
    public virtual Guid ModifiedBy { get; set; }
    public virtual bool IsActive { get; set; }

    public virtual Vehicle Asset { get; set; }
    public virtual Trip Trip { get; set; }
    public virtual ISet<EmployeeTour> EmployeesTours { get; set; }
    public virtual ISet<ContainerTour> ContainerTours { get; set; }
    public virtual ISet<TourLeg> TourLegs { get; set; }

    protected override void Validate()
    {
      if (Name == string.Empty)
      {
        AddBrokenRule(TourBusinessRules.Name);
      }

      if (ScheduledDate.Date < DateTime.Now.Date)
      {
        AddBrokenRule(TourBusinessRules.ScheduledDate);
      }
    }

    public virtual void ModifyWith(Tour tourWithModifiedValues)
    {
      this.Name = tourWithModifiedValues.Name;
      this.ScheduledDate = tourWithModifiedValues.ScheduledDate;
    }

    public virtual void InjectWithVehicle(Vehicle vehicleToBeInjected)
    {
      this.Asset = vehicleToBeInjected;
      vehicleToBeInjected.Tours.Add(this);
    }

    public virtual Tour UnInjectAndInjectWithNewVehicle(Vehicle newVehicleToBeInjected)
    {
      if (Asset != null)
      {
        UnInjectVehicle();
      }

      InjectWithVehicle(newVehicleToBeInjected);
      return this;
    }

    public virtual void UnInjectVehicle()
    {
      Asset.Tours.Remove(this);
      Asset = null;
    }

    public virtual void InjectWithAudit(Guid accountIdToCreateThisTour)
    {
      this.CreatedBy = accountIdToCreateThisTour;
      this.CreatedDate = DateTime.Now;
    }

    public virtual void InjectWithAsset(Vehicle assetToBeInjected)
    {
      this.Asset = assetToBeInjected;
      assetToBeInjected.Tours.Add(this);
    }

    public virtual void InjectWithEmployeeTour(EmployeeTour employeeTourToBeInjected)
    {
      this.EmployeesTours.Add(employeeTourToBeInjected);
      employeeTourToBeInjected.Tour = this;
    }

    public virtual void InjectWithContainerTour(ContainerTour containerTourToBeInjected)
    {
      this.ContainerTours.Add(containerTourToBeInjected);
      containerTourToBeInjected.Tour = this;
    }
  }
}