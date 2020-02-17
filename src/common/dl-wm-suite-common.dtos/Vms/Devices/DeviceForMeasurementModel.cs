using System;
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
    [Editable(true)] public double  Light { get; set; }
    [Required]
    [Editable(true)] public double  Battery { get; set; }
    [Required]
    [Editable(true)] public string  Gps { get; set; }
    [Required]
    [Editable(true)] public string  NbIot { get; set; }
    [Required]
    [Editable(true)] public double  BatterySafeMode { get; set; }
    [Editable(true)] public double Distance { get; set; }
    [Editable(true)] public double Tamper { get; set; }
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
    [Editable(true)] 
    public double  Altitude { get; set; }
    [Required]
    [Editable(true)] public double  Angle { get; set; }
    [Required]
    [Editable(true)] public double  Speed { get; set; }
    [Required]
    [Editable(true)] public double  Bearing { get; set; }
    [Required]
    [Editable(true)] public int  Satellites { get; set; }

    [Required]
    [Editable(true)] 
    public double TimeToFix { get; set; }
    [Required]
    [Editable(true)] 
    public double SignalLength { get; set; }
    [Required]
    [Editable(true)] 
    public double StatusFlags { get; set; }
    [Required]
    [Editable(true)] 
    public DateTime Timestamp { get; set; }
    [Required]
    [Editable(true)] 
    public double NbIoTSignalLength { get; set; }
    [Required]
    [Editable(true)] 
    public string LatestResetCause { get; set; }
    [Required]
    [Editable(true)] 
    public string FirmwareVersion { get; set; }
    [Required]
    [Editable(true)] 
    public bool TemperatureEnable { get; set; }
    [Required]
    [Editable(true)] 
    public bool DistanceEnable { get; set; }
    [Required]
    [Editable(true)] 
    public bool TiltEnable { get; set; }
    [Required]
    [Editable(true)] 
    public bool MagnetometerEnable { get; set; }
    [Required]
    [Editable(true)] 
    public bool TamperEnable { get; set; }
    [Required]
    [Editable(true)] 
    public double NbIoTMode { get; set; }
  }
}