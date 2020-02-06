﻿using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Employees.Departments
{
    public class Department : EntityBase<Guid>, IAggregateRoot
    {
        public Department()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.CreatedDate = DateTime.Now;
            this.ModifiedDate = CreatedDate;
            this.IsActive = true;
            this.Employees = new HashSet<Employee>();
        }

        public virtual string Name { get; set; }
        public virtual string Notes { get; set; }


        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }
        
        public virtual bool IsActive { get; set; }
        public virtual ISet<Employee> Employees { get; set; }

        protected override void Validate()
        {

        }

        public virtual void InjectWithInitialAttributes(string name, string notes)
        {
            this.Name = name;
            this.Notes = notes;
            this.CreatedDate = DateTime.UtcNow;
        }

        public virtual void InjectWithAudit(Guid accountIdToCreateThisEmployeeRole)
        {
            this.CreatedBy = accountIdToCreateThisEmployeeRole;
        }
    }
}