using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers
{
    public class ContainerDoesNotExistAfterMadeTransientException : Exception
    {
        public Guid ContainerId { get; private set; }
        public string Name { get; private set; }

        public ContainerDoesNotExistAfterMadeTransientException(string name)
        {
            Name = name;
        }
        public ContainerDoesNotExistAfterMadeTransientException(Guid containerId)
        {
            ContainerId = containerId;
        }

        public override string Message => $" Container with Name: {Name} or Id: {ContainerId} was not made Transient!";
    }
}