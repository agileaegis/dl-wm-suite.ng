using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments
{
    public class DepartmentDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public DepartmentDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" Department with Name: {Name} was not made Persistent!";
    }
}