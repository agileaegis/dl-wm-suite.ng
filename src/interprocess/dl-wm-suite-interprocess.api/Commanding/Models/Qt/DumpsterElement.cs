using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class DumpsterElement
    {
        public DumpsterElement()
        {
            this.Dumpster = new Dumpster();
            this.Sensor = new Sensor();
        }

        [JsonProperty("dumpster")]
        public Dumpster Dumpster { get; set; }

        [JsonProperty("sensor")]
        public Sensor Sensor { get; set; }
    }
}