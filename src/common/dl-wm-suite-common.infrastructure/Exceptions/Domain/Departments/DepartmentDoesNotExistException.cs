using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Departments
{
    public class DepartmentDoesNotExistException : Exception
    {
        public Guid DepartmentId { get; }

        public DepartmentDoesNotExistException(Guid departmentId)
        {
            DepartmentId = departmentId;
        }

        public override string Message => $"Department with Id: {DepartmentId}  doesn't exists!";
    }
}
