using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.common.dtos.Vms.Devices
{
  public class DeviceForMeasurementModel
  {
    [Required]
    [Editable(true)] public string MeasurementValueJson { get; set; }
    [Required]
    [Editable(true)] public double Temperature { get; set; }
    [Required]
    [Editable(true)] public double  FillLevel { get; set; }
    [Required]
    [Editable(true)] public double  TiltX { get; set; }
    [Required]
    [Editable(true)] public double  TiltY { get; set; }
    [Required]
    [Editable(true)] public double  TiltZ { get; set; }
    [Required]
    [Editable(true)] public int  Light { get; set; }
    [Required]
    [Editable(true)] public double  Battery { get; set; }
    [Required]
    [Editable(true)] public string  Gps { get; set; }
    [Required]
    [Editable(true)] public string  NbIot { get; set; }
    [Required]
    [Editable(true)] public int  BatterySafeMode { get; set; }
    [Editable(true)] public bool TemperatureEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  FillLevelEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  TiltYEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  MagnetometerEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  TamperEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  LightEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  BatteryEnabled { get; set; }
    [Required]
    [Editable(true)] public bool  GpsEnabled { get; set; }
    [Required]
    [Editable(true)] public double  GeoLat { get; set; }
    [Required]
    [Editable(true)] public double  GeoLon { get; set; }
    [Required]
    [Editable(true)] public double  Altitude { get; set; }
    [Required]
    [Editable(true)] public double  Angle { get; set; }
    [Required]
    [Editable(true)] public double  Speed { get; set; }
    [Required]
    [Editable(true)] public int  Satellites { get; set; }
  }
}