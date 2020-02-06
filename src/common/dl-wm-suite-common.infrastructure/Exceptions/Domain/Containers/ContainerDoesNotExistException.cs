using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers
{
    public class ContainerDoesNotExistException : Exception
    {
        public Guid ContainerId { get; }

        public ContainerDoesNotExistException(Guid containerId)
        {
            ContainerId = containerId;
        }

        public override string Message => $"Container with Id: {ContainerId}  doesn't exists!";
    }
}
