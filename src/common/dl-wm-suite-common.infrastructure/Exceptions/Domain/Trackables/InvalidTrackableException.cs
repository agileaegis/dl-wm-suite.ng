using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trackables
{
    public class InvalidTrackableException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidTrackableException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}