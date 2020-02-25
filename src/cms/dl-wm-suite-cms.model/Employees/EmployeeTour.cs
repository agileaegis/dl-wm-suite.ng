using System;
using System.Collections.Generic;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Employees
{
    public class EmployeeTour : EntityBase<Guid>
    {
        public EmployeeTour()
        {
            OnCreated();
        }

        private void OnCreated()
        {       
            this.IsActive = true;
            this.StatusType = EmployeeStatusType.Normal;
            this.Duration = 0;
        }


        public virtual DateTime RegisteredDate { get; set; }
        public virtual int Duration { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual EmployeeRoleType Role { get; set; }
        public virtual EmployeeStatusType StatusType { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Tour Tour { get; set; }
        protected override void Validate()
        {
        }

        public virtual void InjectWithEmployee(Employee employee)
        {
          this.Employee = employee;
          employee.EmployeesTours.Add(this);
        }

        public virtual void InjectWithAttributes()
        {
          this.RegisteredDate = DateTime.Now; 
        }
    }
}