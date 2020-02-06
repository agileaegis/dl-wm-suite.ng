using System.Collections.Generic;
using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class Properties
    {
        public Properties()
        {
            this.Dumpsters = new List<DumpsterElement>()
            {
                new DumpsterElement()
            };
            this.DumpstersAreaFillLevel = new double();
            this.DumpstersAreaId = new long();
            this.Location = new long();
            this.Priority = new long();
            this.Radius = new long();
        }

        [JsonProperty("dumpsters")]
        public List<DumpsterElement> Dumpsters { get; set; }

        [JsonProperty("dumpstersAreaFillLevel")]
        public double DumpstersAreaFillLevel { get; set; }

        [JsonProperty("dumpstersAreaId")]
        public long DumpstersAreaId { get; set; }

        [JsonProperty("location")]
        public long Location { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }

        [JsonProperty("radius")]
        public long Radius { get; set; }
    }
}