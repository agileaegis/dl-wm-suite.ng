using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.EmployeeRoles
{
    public class InvalidEmployeeRoleException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidEmployeeRoleException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}
