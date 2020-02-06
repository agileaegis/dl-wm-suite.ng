using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.EmployeeRoles
{
    public class EmployeeRoleAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public EmployeeRoleAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" EmployeeRole with name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}
