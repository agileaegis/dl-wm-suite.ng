using System;
using System.Collections.Generic;
using System.Text;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class Simcard : EntityBase<Guid>, IAggregateRoot
    {
        public Simcard()
        {

        }

        public virtual string Iccid { get; set; }
        public virtual string Imsi { get; set; }
        public virtual string CountryIso { get; set; }
        public virtual string Number { get; set; }
        public virtual DateTime PurchaseDate { get; set; }
        public virtual SimCardType CardType { get; set; }
        public virtual SimNetworkType NetworkType { get; set; }
        public virtual bool IsActive { get; set; }
        
        public virtual Device Device { get; set; }

        protected override void Validate()
        {
        }
    }
}
