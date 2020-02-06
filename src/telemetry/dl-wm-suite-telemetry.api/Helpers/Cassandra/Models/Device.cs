using System;
using Cassandra.Mapping.Attributes;

namespace dl.wm.suite.telemetry.api.Helpers.Cassandra.Models
{
    [Table("devices")]
    public class Device
    {
        [Column("id")]
        public string Id {get; set;}
        [Column("imei")]
        public string Imei { get; set; }
        [Column("provisioning_at")]
        public DateTime ProvisioningDate { get; set; }
        [Column("activation_at")]
        public DateTime ActivationDate { get; set; }
        [Column("is_activated")]
        public bool IsActivated { get; set; }
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }
        [Column("firmware_version")]
        public string FirmwareVer { get; set; }
    }
}