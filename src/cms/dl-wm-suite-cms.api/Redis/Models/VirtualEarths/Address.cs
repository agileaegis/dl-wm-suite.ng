namespace dl.wm.suite.cms.api.Redis.Models.VirtualEarths
{
    public class Address
    {
        public string addressLine { get; set; }
        public string adminDistrict { get; set; }
        public string adminDistrict2 { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public Intersection intersection { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
    }
}