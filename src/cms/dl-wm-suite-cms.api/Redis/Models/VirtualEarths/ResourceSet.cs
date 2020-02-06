using System.Collections.Generic;

namespace dl.wm.suite.cms.api.Redis.Models.VirtualEarths
{
    public class ResourceSet
    {
        public int estimatedTotal { get; set; }
        public List<Resource> resources { get; set; }
    }
}