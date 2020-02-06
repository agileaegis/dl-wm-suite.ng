using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class Feature
    {
        public Feature()
        {
            this.Geometry = new Geometry();
            this.Properties = new Properties();
            this.Type = "";
        }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}