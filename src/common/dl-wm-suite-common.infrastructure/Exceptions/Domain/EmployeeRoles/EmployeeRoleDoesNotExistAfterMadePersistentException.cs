using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.EmployeeRoles
{
    public class EmployeeRoleDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public EmployeeRoleDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" EmployeeRole with Name: {Name} was not made Persistent!";
    }
}