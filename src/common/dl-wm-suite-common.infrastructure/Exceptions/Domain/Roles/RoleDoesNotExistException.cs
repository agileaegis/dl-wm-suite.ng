using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Roles
{
    public class RoleDoesNotExistException : Exception
    {
        public Guid RoleId { get; }

        public RoleDoesNotExistException(Guid roleId)
        {
            RoleId = roleId;
        }

        public override string Message => $"Role with Id: {RoleId}  doesn't exists!";
    }
}
