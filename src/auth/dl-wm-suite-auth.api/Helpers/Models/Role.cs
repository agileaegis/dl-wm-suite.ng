using System;
using System.Collections.Generic;
using dl.wm.suite.common.dtos.Vms.Roles;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.auth.api.Helpers.Models
{
    public class Role : EntityBase<Guid>, IAggregateRoot
    {
        public Role()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.IsActive = true;
            this.CreatedDate = DateTime.UtcNow;
            this.Usersroles = new HashSet<UserRole>();
        }

        public virtual string Name { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual Guid ModifiedBy { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual ISet<UserRole> Usersroles { get; set; }

        protected override void Validate()
        {
        }

        public virtual void InjectWithInitialAttributes(RoleForCreationUiModel newRoleUiModel)
        {
            this.Name = newRoleUiModel.Name;
        }

        public virtual void InjectWithAudit(Guid accountIdToCreateThisRole)
        {
            this.CreatedBy = accountIdToCreateThisRole;
        }
    }
}
