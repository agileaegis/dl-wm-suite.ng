using System;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.auth.api.Helpers.Models
{
    public class Person : EntityBase<Guid>, IAggregateRoot
    {
        public Person()
        {
            OnCreate();
        }

        private void OnCreate()
        {
            this.IsActive = true;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
        }

        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Email { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual string Phone { get; set; }
        public virtual string ExtPhone { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string ExtMobile { get; set; }
        public virtual string Notes { get; set; }
        public virtual Address Address { get; set; }

        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual EmployeeRole EmployeeRole { get; set; }
        public virtual Department Department { get; set; }

        public virtual User User { get; set; }

        protected override void Validate()
        {

        }

        public virtual void InjectWithAuditCreation(Guid accountIdToCreateThisUser)
        {
            this.CreatedBy = accountIdToCreateThisUser;
            this.ModifiedBy = Guid.Empty;
        }

        public virtual void InjectWithEmployeeRole(EmployeeRole employeeRoleToBeInjected)
        {
            this.EmployeeRole = employeeRoleToBeInjected;
            employeeRoleToBeInjected.Persons.Add(this);
        }

        public virtual void InjectWithDepartment(Department departmentToBeInjected)
        {
            this.Department = departmentToBeInjected;
            departmentToBeInjected.Persons.Add(this);
        }
    }
}
