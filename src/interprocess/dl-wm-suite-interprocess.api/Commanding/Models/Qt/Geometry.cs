using System.Collections.Generic;
using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class Geometry
    {
        public Geometry()
        {
            this.Coordinates = new List<double>();
            this.Type = "";
        }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}