using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments
{
    public class DepartmentAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public DepartmentAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Department with name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}
