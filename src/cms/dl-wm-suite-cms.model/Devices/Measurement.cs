using System;
using dl.wm.suite.cms.model.CustomTypes;
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
         this.ModifiedDate = DateTime.MinValue;   
         this.JsonbValue = new JsonbType();
        }


        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual JsonbType JsonbValue { get; set; }
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

        public virtual void InjectWithInitialAttributes(string measurementValueJson, double temperature, double fillLevel, 
          double tiltX, double tiltY, double tiltZ, int light, double battery, string gps, string nbIot, 
          int batterySafeMode, bool temperatureEnabled, bool fillLevelEnabled, 
          bool magnetometerEnabled, bool tamperEnabled, bool lightEnabled, bool gpsEnabled)
        {
          this.JsonbValue = new JsonbType()
          {
            JsonbProperty = measurementValueJson
          };
          this.Temperature = temperature;
          this.FillLevel = fillLevel;
          this.TiltX = tiltX;
          this.TiltY = tiltY;
          this.TiltZ = tiltZ;
          this.Light = light;
          this.Battery = battery;
          this.Gps = gps;
          this.NbIoT = nbIot;
          this.NBIoTMode = NBIoTMode.Normal;
          this.BatterySaveMode = (BatteryMode) batterySafeMode;
          this.TempEnabled = temperatureEnabled;
          this.FillLevelEnabled = fillLevelEnabled;
          this.MagnetometerEnabled = magnetometerEnabled;
          this.TiltEnabled = magnetometerEnabled;
          this.TamperEnabled = tamperEnabled;
          this.LightEnabled = lightEnabled;
          this.GpsEnabled = gpsEnabled;
        }

        public virtual void ModifiedWith()
        {
          this.ModifiedDate = DateTime.Now;
        }
    }
}
