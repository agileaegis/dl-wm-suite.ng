﻿using System.ComponentModel.DataAnnotations;

namespace dl.wm.suite.fleet.api.Redis.Models
{
    public class TrackingPointRedisModel
    {
        public string Audit { get; set; }
        public long Time { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Provider { get; set; }
        public int LocationProvider { get; set; }
        public double? Accuracy { get; set; }
        public double? Speed { get; set; }
        public double? Altitude { get; set; }
        public double? Course { get; set; }
    }
}
