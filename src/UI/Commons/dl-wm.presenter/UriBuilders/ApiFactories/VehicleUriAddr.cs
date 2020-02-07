﻿namespace dl.wm.presenter.UriBuilders.ApiFactories
{
    public class VehicleUriAddr : BaseClientUri
    {
        public VehicleUriAddr(string serverIp)
            : this(serverIp, "6200", "v1")
        {
        }

        public VehicleUriAddr(string serverIp, string serverPort, string versioning)
            : base(serverIp, serverPort)
        {
            Versioning = versioning;
        }

        protected sealed override string Versioning { get; set; }

        protected override string Segment => "vehicles";
    }
}