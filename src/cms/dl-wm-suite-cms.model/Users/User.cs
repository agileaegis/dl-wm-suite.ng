using System;
using System.Collections.Generic;
using System.Linq;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Users
{
    public class User : EntityBase<Guid>, IAggregateRoot
    {
        public User()
        {
            OnCreate();
        }

        private void OnCreate()
        {
            this.IsActive = true;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
            this.ResetDate = DateTime.UtcNow;
        }

        public virtual string Login { get; set; }
        public virtual bool IsActivated { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual Guid ResetKey { get; set; }
        public virtual Guid ActivationKey { get; set; }
        public virtual DateTime ResetDate { get; set; }
        public virtual bool IsActive { get; set; }

        protected override void Validate()
        {

        }
    }
}


