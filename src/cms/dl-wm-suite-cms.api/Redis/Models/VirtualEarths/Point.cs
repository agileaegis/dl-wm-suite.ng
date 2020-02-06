using System.Collections.Generic;

namespace dl.wm.suite.cms.api.Redis.Models.VirtualEarths
{
    public class Point
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }
}