using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers
{
    public class ContainerAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public ContainerAlreadyExistsException(string name)
           : this(name, "NO_BROKEN_RULES")
        {
        }
        public ContainerAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Container with name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}
