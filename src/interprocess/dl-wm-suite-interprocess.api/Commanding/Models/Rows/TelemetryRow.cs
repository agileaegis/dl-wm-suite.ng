using System;
using dl.wm.suite.interprocess.api.Commanding.Models.Base;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Rows
{
    public class TelemetryRow : BaseModel
    {
        public string CommandType { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Battery { get; set; }
        public decimal Temperature { get; set; }
        public string Distance { get; set; }
        public decimal FillLevel { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Altitude { get; set; }
        public string Speed { get; set; }
        public string Course { get; set; }
        public string NumOfSatellites { get; set; }
        public string TimeToFix { get; set; }
        public string SignalLength { get; set; }
        public string StatusFlags { get; set; }
        public string LatestResetCause { get; set; }
        public string FirmwareVersion { get; set; }
    }
}