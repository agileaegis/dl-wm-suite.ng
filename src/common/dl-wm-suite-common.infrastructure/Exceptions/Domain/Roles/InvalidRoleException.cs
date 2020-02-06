using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Roles
{
    public class InvalidRoleException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidRoleException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}