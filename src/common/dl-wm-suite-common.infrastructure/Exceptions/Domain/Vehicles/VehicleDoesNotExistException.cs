using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Vehicles
{
    public class VehicleDoesNotExistException : Exception
    {
        public Guid VehicleId { get; set; }

        public VehicleDoesNotExistException(Guid id)
        {
            VehicleId = id;
        }

        public override string Message => $"Vehicle with id: {VehicleId} doesn't exist";
    }
}