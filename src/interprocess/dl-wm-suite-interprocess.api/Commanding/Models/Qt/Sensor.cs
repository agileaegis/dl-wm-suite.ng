using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class Sensor
    {
        public Sensor()
        {
            this.Geometry = null;
            this.SensorId = new long();
            this.Props = new SensorProps();
        }

        [JsonProperty("geometry")]
        public object Geometry { get; set; }

        [JsonProperty("props")]
        public SensorProps Props { get; set; }

        [JsonProperty("sensorId")]
        public long SensorId { get; set; }
    }
}