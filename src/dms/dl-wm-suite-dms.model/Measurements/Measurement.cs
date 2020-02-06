using System;
using dl.wm.suite.common.infrastructure.Domain;
using dl.wm.suite.dms.model.Devices;
using dl.wm.suite.dms.model.Devices;

namespace dl.wm.suite.dms.model.Measurements
{
    public class Measurement : EntityBase<Guid>, IAggregateRoot
    {
        public Measurement()
        {
            OnCreated();
        }

        private void OnCreated()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
        }

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual byte[] TemperatureValue { get; set; }
        public virtual byte[] FillLevelValue { get; set; }
        public virtual byte[] TiltValue { get; set; }
        public virtual byte[] LightValue { get; set; }
        public virtual byte[] BatteryValue { get; set; }
        public virtual byte[] GpsValue { get; set; }
        public virtual byte[] NbiotValue { get; set; }
        public virtual string MeasurementValue{ get; set; }
        public virtual float Temperature{ get; set; }
        public virtual float FillLevel{ get; set; }
        public virtual float TiltX{ get; set; }
        public virtual float TiltY{ get; set; }
        public virtual float TiltZ{ get; set; }
        public virtual int Light{ get; set; }
        public virtual float Battery{ get; set; }
        public virtual string Gps{ get; set; }
        public virtual string Nbiot{ get; set; }
        public virtual bool TemperatureEnabled{ get; set; }
        public virtual bool FillLevelEnabled{ get; set; }
        public virtual bool TiltEnabled{ get; set; }
        public virtual bool MagnetometerEnabled{ get; set; }
        public virtual bool TamperEnabled{ get; set; }
        public virtual bool LightEnabled{ get; set; }

        // Todo: Should be added some extra attributes

        public virtual Device Device { get; set; }

        protected override void Validate()
        {
        }
    }
}