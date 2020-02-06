using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers
{
    public class ContainerDoesNotExistAfterMadePersistentException : Exception
    {
        public Guid ContainerId { get; private set; }
        public string Name { get; private set; }

        public ContainerDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }
        public ContainerDoesNotExistAfterMadePersistentException(Guid containerId)
        {
            ContainerId = containerId;
        }

        public override string Message => $" Container with Name: {Name} or Id: {ContainerId} was not made Persistent!";
    }
}