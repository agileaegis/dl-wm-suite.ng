using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Containers
{
    public class ContainerHaveAlreadyAnAssignedActiveDeviceException : Exception
    {
        public string ContainerName { get; }
        public string DeviceImei { get; }

        public ContainerHaveAlreadyAnAssignedActiveDeviceException(string nameContainer, string imeiDevice)
        {
          ContainerName = nameContainer;
          DeviceImei = imeiDevice;
        }

        public override string Message => $" Container with name:{ContainerName} have already an assigned Device with imei: {DeviceImei}";
    }
}
