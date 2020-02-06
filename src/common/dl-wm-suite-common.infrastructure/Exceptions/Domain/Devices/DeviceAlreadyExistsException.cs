using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class DeviceAlreadyExistsException : Exception
    {
        public string NumPlate { get; }

        public string DeviceName { get; }


        public DeviceAlreadyExistsException(string vehicleName)
        {
            DeviceName = vehicleName;
        }

        public DeviceAlreadyExistsException(string message, string numPlate) : base(message)
        {
            this.NumPlate = numPlate;
        }

        public override string Message => $" Device with Brand: {DeviceName} and Plate Number:{NumPlate}" +
                                          " already Exists!";
    }
}