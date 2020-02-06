using System.Collections.Generic;
using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class Telemetry
    {
        public Telemetry()
        {
            this.Features = new List<Feature>()
            {
                new Feature()
            };
            this.Type = "";
        }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } 
    }
}