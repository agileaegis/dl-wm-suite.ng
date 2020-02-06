using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Vehicles
{
    public class VehicleAlreadyExistsException : Exception
    {
        public string NumPlate { get; }

        public string VehicleName { get; }


        public VehicleAlreadyExistsException(string vehicleName)
        {
            VehicleName = vehicleName;
        }

        public VehicleAlreadyExistsException(string message, string numPlate) : base(message)
        {
            this.NumPlate = numPlate;
        }

        public override string Message => $" Vehicle with Brand: {VehicleName} and Plate Number:{NumPlate}" +
                                          " already Exists!";
    }
}