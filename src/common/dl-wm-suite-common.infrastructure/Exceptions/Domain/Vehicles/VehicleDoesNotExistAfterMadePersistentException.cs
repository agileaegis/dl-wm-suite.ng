using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Vehicles
{
    public class VehicleDoesNotExistAfterMadePersistentException : Exception
    {
        public string VehicleNumPlate { get; }

        public string VehicleBrand { get; }
        public VehicleDoesNotExistAfterMadePersistentException(string vehicleBrand)
        {
            VehicleBrand = vehicleBrand;
        }

        public VehicleDoesNotExistAfterMadePersistentException(string message, string vehicleNumPlate) : base(message)
        {
            this.VehicleNumPlate = vehicleNumPlate;
        }

        public override string Message => $" Vehicle with Brand: {VehicleBrand} for Date:{VehicleNumPlate}, was not made Persistent!";
    }
}