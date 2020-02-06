using System;
using Cassandra.Mapping.Attributes;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Models
{
    [Table("telemetryrow")]
    public class TelemetryRow
    {
        [Column("id")]
        public string Id {get; set;}
        [Column("correlation_id")]
        public string CorrelationId { get; set;}
        [Column("created_date")]
        public DateTime CreatedDate { get; set;}
        [Column("created_date_utc")]
        public DateTime CreatedDateUtc { get; set;}
        [Column("imei")]
        public string Imei { get; set; }
        [Column("command")]
        public string CommandType { get; set; }
        [Column("header_version")]
        public string Version { get; set; }
        [Column("timestamp")]
        public string Timestamp { get; set; }
        [Column("battery")]
        public decimal Battery { get; set; }
        [Column("temperature")]
        public decimal Temperature { get; set; }
        [Column("distance")]
        public string Distance { get; set; }
        [Column("fill_level")]
        public decimal FillLevel { get; set; }
        [Column("latitude")]
        public double Latitude { get; set; }
        [Column("longitude")]
        public double Longitude { get; set; }
        [Column("altitude")]
        public string Altitude { get; set; }
        [Column("speed")]
        public string Speed { get; set; }
        [Column("course")]
        public string Course { get; set; }
        [Column("num_of_satellites")]
        public string NumOfSatellites { get; set; }
        [Column("time_to_fix")]
        public string TimeToFix { get; set; }
        [Column("signal_length")]
        public string SignalLength { get; set; }
        [Column("status_flags")]
        public string StatusFlags { get; set; }
        [Column("latest_reset_cause")]
        public string LatestResetCause { get; set; }
        [Column("firmware_version")]
        public string FirmwareVersion { get; set; }
    }
}