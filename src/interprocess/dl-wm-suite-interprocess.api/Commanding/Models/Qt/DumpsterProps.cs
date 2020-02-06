using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Models.Qt
{
    public class DumpsterProps
    {
        public DumpsterProps()
        {
            this.Capacity = new long();
            this.Description = "";
            this.IsFixed = true;
            this.IsUnderground = true;
            this.Material = "";
            this.WasteType = "";
        }

        [JsonProperty("capacity")]
        public long Capacity { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isFixed")]
        public bool IsFixed { get; set; }

        [JsonProperty("isUnderground")]
        public bool IsUnderground { get; set; }

        [JsonProperty("material")]
        public string Material { get; set; }

        [JsonProperty("wasteType")]
        public string WasteType { get; set; }
    }
}
