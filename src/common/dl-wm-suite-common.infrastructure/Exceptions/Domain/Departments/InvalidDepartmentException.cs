using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments
{
    public class InvalidDepartmentException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidDepartmentException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}
