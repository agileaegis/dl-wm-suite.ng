using System.Collections.Generic;

namespace dl.wm.suite.cms.api.Redis.Models.VirtualEarths
{
    public class GeocodePoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string calculationMethod { get; set; }
        public List<string> usageTypes { get; set; }
    }
}