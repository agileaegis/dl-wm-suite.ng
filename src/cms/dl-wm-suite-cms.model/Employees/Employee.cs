using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Addresses;
using dl.wm.suite.cms.model.Employees.Departments;
using dl.wm.suite.cms.model.Employees.EmployeeRoles;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Employees
{
    public class Employee : EntityBase<Guid>, IAggregateRoot
    {
        public Employee()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.Gender = GenderType.Male;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
            this.IsActive = true;
            this.EmployeesTours = new HashSet<EmployeeTour>();
        }

        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Email { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual string Phone { get; set; }
        public virtual string ExtPhone { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string ExtMobile { get; set; }
        public virtual string Notes { get; set; }
        public virtual Address Address { get; set; }


        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }
        
        public virtual bool IsActive { get; set; }
        
        public virtual EmployeeRole EmployeeRole { get; set; }
        public virtual Department Department { get; set; }
        public virtual ISet<EmployeeTour> EmployeesTours { get; set; }

        protected override void Validate()
        {

        }

        public virtual void InjectWithInitialAttributes(string email)
        {
            this.Email = email;
            this.CreatedDate = DateTime.UtcNow;
        }
    }
}

