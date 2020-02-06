using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class Dumpster
    {
        public Dumpster()
        {
            this.DumpsterId = new long();
            this.Props = new DumpsterProps();
        }

        [JsonProperty("dumpsterId")]
        public long DumpsterId { get; set; }

        [JsonProperty("props")]
        public DumpsterProps Props { get; set; }
    }
}