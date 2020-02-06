using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Models
{
    public class DeviceForProvisioningModel
    {
        [Required]
        [Editable(true)]
        public string DeviceForProvisioningImei { get; set; }
        [Required]
        [Editable(true)]
        public string DeviceForProvisioningFirmwareVersion { get; set; }

    }
    public class TelemetryRowForCreationModel
    {
        [Required]
        [Editable(true)]
        public string TelemetryRowCommandType { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowHeaderVersion { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowTimestamp { get; set; }
        [Required]
        [Editable(true)]
        public decimal TelemetryRowBattery { get; set; }
        [Required]
        [Editable(true)]
        public decimal TelemetryRowTemperature { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowDistance { get; set; }
        [Required]
        [Editable(true)]
        public decimal TelemetryRowFillLevel { get; set; }
        [Required]
        [Editable(true)]
        public double TelemetryRowLatitude { get; set; }
        [Required]
        [Editable(true)]
        public double TelemetryRowLongitude { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowAltitude { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowSpeed { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowCourse { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowNumOfSatellites { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowTimeToFix { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowSignalLength { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowStatusFlags { get; set; }
        [Required]
        [Editable(true)]
        public string TelemetryRowLatestResetCause { get; set; }
        [Required]
        [Editable(true)]
        public string FirmwareVersion { get; set; }

    }
}