using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class SensorProps
    {
        public SensorProps()
        {
            this.Bat = new decimal();
            this.Fill = new decimal();
            this.Temp = new long();
            this.Firmware = "";
        }

        [JsonProperty("bat")]
        public decimal Bat { get; set; }

        [JsonProperty("fill")]
        public decimal Fill { get; set; }

        [JsonProperty("firmware")]
        public string Firmware { get; set; }

        [JsonProperty("temp")]
        public decimal Temp { get; set; }
    }
}