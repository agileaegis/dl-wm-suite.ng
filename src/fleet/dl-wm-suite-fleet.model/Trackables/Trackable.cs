using System;
using System.Collections.Generic;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.fleet.model.Trackables
{
  public class Trackable : EntityBase<int>, IAggregateRoot
  {
    public Trackable()
    {
      OnCreated();
    }

    private void OnCreated()
    {
      this.CreatedDate = DateTime.Now;
      this.IsActive = true;
      this.TrackableAssets = new HashSet<TrackableAsset>();
    }

    public virtual string Name { get; set; }
    public virtual string Model { get; set; }
    public virtual string VendorId { get; set; }
    public virtual string Phone { get; set; }
    public virtual string Os { get; set; }
    public virtual string Version { get; set; }
    public virtual string Notes { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual bool IsActive { get; set; }

    public virtual ISet<TrackableAsset> TrackableAssets { get; set; }


    public virtual void InjectWithAudit(string createdBy)
    {
      this.CreatedBy = createdBy;
    }

    protected override void Validate()
    {

    }
  }
}