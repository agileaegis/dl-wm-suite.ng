using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips
{
    public class InvalidTripException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidTripException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}