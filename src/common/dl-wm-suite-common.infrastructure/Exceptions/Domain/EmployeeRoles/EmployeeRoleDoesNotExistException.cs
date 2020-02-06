using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.EmployeeRoles
{
    public class EmployeeRoleDoesNotExistException : Exception
    {
        public Guid EmployeeRoleId { get; }

        public EmployeeRoleDoesNotExistException(Guid employeeRoleId)
        {
            EmployeeRoleId = employeeRoleId;
        }

        public override string Message => $"EmployeeRole with Id: {EmployeeRoleId}  doesn't exists!";
    }
}
