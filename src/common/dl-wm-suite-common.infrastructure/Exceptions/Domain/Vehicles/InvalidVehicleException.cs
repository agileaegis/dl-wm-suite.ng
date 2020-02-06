using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Vehicles
{
    public class InvalidVehicleException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidVehicleException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}