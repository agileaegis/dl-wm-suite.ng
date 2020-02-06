using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers
{
    public class InvalidContainerException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidContainerException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}
