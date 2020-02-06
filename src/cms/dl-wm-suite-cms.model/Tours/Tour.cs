using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Containers;
using dl.wm.suite.cms.model.Employees;
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
            this.ScheduledDate = DateTime.UtcNow;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
            this.Type = TourType.Auto;
            this.Status = TourStatus.Scheduled;
            this.EmployeesTours = new HashSet<EmployeeTour>();
            this.ContainerTours = new HashSet<ContainerTour>();
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

        public virtual Vehicle Vehicle { get; set; }
        public virtual ISet<EmployeeTour> EmployeesTours { get; set; }
        public virtual ISet<ContainerTour> ContainerTours { get; set; }

        protected override void Validate()
        {
            if (Name == string.Empty)
            {
                AddBrokenRule(TourBusinessRules.Name);
            }
        }
        public virtual void ModifyWith(Tour tourWithModifiedValues)
        {
            this.Name = tourWithModifiedValues.Name;
            this.ScheduledDate = tourWithModifiedValues.ScheduledDate;
        }

        public virtual void InjectWithVehicle(Vehicle vehicleToBeInjected)
        {
            this.Vehicle = vehicleToBeInjected;
            vehicleToBeInjected.Tours.Add(this);
        }
        public virtual Tour UnInjectAndInjectWithNewVehicle(Vehicle newVehicleToBeInjected)
        {
            if (Vehicle != null)
            {
                UnInjectVehicle();
            }
            InjectWithVehicle(newVehicleToBeInjected);
            return this;
        }

        public virtual void UnInjectVehicle()
        {
            Vehicle.Tours.Remove(this);
            Vehicle = null;
        }

    }
}