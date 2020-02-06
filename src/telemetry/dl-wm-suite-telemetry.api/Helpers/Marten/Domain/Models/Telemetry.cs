using System;

namespace dl.wm.suite.telemetry.api.Helpers.Marten.Domain.Models
{
    public class Telemetry
    {
        public Guid Id { get; private set; }
        public string Imei { get; private set; }


        public Telemetry(string imei)
        {
            Id = Guid.NewGuid();
            Imei = imei;
        }
    }
}