using System;
using dl.wm.suite.cms.model.Tours;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Containers
{
    public class ContainerTour : EntityBase<Guid>, IAggregateRoot
    {
        public ContainerTour()
        {
            OnCreated();
        }

        private void OnCreated()
        {
        }

        public virtual Tour Tour { get; set; }
        public virtual Container Container { get; set; }

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual DateTime ServicedDate { get; set; }


        protected override void Validate()
        {

        }
    }
}

