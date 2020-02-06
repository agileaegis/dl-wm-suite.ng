using System;
using dl.wm.suite.common.infrastructure.Domain;

namespace dl.wm.suite.cms.model.Devices
{
    public class Measurement : EntityBase<Guid>, IAggregateRoot
    {
        public Measurement()
        {
            OnCreated();
        }

        private void OnCreated()
        {
         this.CreatedDate = DateTime.Now;   
        }


        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string JsonValue { get; set; }
        public virtual double Temperature { get; set; }
        public virtual double FillLevel { get; set; }
        public virtual double TiltX { get; set; }
        public virtual double TiltY { get; set; }
        public virtual double TiltZ { get; set; }
        public virtual int Light { get; set; }
        public virtual bool Tamper { get; set; }
        public virtual double Battery { get; set; }
        public virtual string Gps { get; set; }
        public virtual string NbIoT { get; set; }
        public virtual bool TempEnabled { get; set; }
        public virtual bool FillLevelEnabled { get; set; }
        public virtual bool TiltEnabled { get; set; }
        public virtual bool MagnetometerEnabled { get; set; }
        public virtual bool TamperEnabled { get; set; }
        public virtual bool LightEnabled { get; set; }
        public virtual bool GpsEnabled { get; set; }
        public virtual BatteryMode BatterySaveMode { get; set; }
        public virtual NBIoTMode NBIoTMode { get; set; }

        public virtual Device Device { get; set; }

        protected override void Validate()
        {
        }
    }
}
