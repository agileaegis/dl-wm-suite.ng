using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class DeviceDoesNotExistAfterMadePersistentException : Exception
    {
        public string DeviceNumPlate { get; }

        public string DeviceBrand { get; }
        public DeviceDoesNotExistAfterMadePersistentException(string vehicleBrand)
        {
            DeviceBrand = vehicleBrand;
        }

        public DeviceDoesNotExistAfterMadePersistentException(string message, string vehicleNumPlate) : base(message)
        {
            this.DeviceNumPlate = vehicleNumPlate;
        }

        public override string Message => $" Device with Brand: {DeviceBrand} for Date:{DeviceNumPlate}, was not made Persistent!";
    }
}