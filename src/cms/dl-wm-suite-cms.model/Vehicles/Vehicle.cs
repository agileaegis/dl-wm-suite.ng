using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Vehicles
{
    public class Vehicle : EntityBase<Guid>, IAggregateRoot
    {
        public Vehicle()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.Type = VehicleType.TruckMile; 
            this.Status = VehicleStatus.Normal;
            this.Gas = GasType.Diesel;
            this.Tours = new HashSet<Tour>();

            this.RegisteredDate = DateTime.UtcNow;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;

            this.CreatedBy = Guid.Empty;
            this.ModifiedBy = Guid.Empty;
        }

        public virtual string NumPlate { get; set; }
        public virtual string Brand { get; set; }
        public virtual DateTime RegisteredDate { get; set; }
        public virtual bool IsActive { get; set; }


        public virtual GasType Gas { get; set; }
        public virtual VehicleType Type { get; set; }
        public virtual VehicleStatus Status { get; set; }
        public virtual ISet<Tour> Tours { get; set; }


        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }

        protected override void Validate()
        {

        }
    }
}