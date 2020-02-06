using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.fleet.model.Trackables;

namespace dl.wm.suite.fleet.model.Assets
{
    public class Asset : EntityBase<int>, IAggregateRoot
    {
        public Asset()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.IsActive = true;
            this.CreatedDate = DateTime.Now;
            this.TrackableAssets = new HashSet<TrackableAsset>();
        }

        public virtual string Name { get; set; }
        public virtual string NumPlate { get; set; }
        public virtual AssetType Type { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual double Height { get; set; }
        public virtual double Width { get; set; }
        public virtual double Length { get; set; }
        public virtual int Axels { get; set; }
        public virtual int Trailers { get; set; }
        public virtual bool IsSemi { get; set; }
        public virtual double MaxGradient { get; set; }
        public virtual double MinTurnRadius { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual ISet<TrackableAsset> TrackableAssets { get; set; }

        protected override void Validate()
        {

        }

        public virtual void InjectWithAudit(string accountEmailToCreateThisAsset)
        {
            this.CreatedBy = accountEmailToCreateThisAsset;
        }
    }
}