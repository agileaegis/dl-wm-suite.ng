using System;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class Command : EntityBase<Guid>, IAggregateRoot
    {

        public Command()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            
        }

        public virtual string Name { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual DateTime DeliveredDate { get; set; }
        public virtual Guid DeliveredBy { get; set; }
        public virtual bool IsDelivered { get; set; }
        public virtual CommandSevere Severe { get; set; }
        public virtual byte[] Value { get; set; }
        public virtual string JsonValue { get; set; }
        public virtual bool IsActive { get; set; }

        protected override void Validate()
        {
        }
    }
}
