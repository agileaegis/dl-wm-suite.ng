using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.auth.api.Helpers.Models
{
    public class Department : EntityBase<Guid>, IAggregateRoot
    {
        public Department()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.IsActive = true;
            this.Persons = new HashSet<Person>();
        }

        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual ISet<Person> Persons { get; set; }

        protected override void Validate()
        {

        }
    }
}