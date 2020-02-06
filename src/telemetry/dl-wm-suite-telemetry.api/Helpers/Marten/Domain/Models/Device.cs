using System;

namespace dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Models
{
    public class Device
    {
        public Guid Id { get; private set; }
        public string Imei { get; private set; }


        public Device(string imei)
        {
            Id = Guid.NewGuid();
            Imei = imei;
        }
    }
}