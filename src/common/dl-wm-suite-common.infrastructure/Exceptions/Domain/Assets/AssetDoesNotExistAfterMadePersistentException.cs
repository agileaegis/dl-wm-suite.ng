using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Assets
{
    public class AssetDoesNotExistAfterMadePersistentException : Exception
    {
        public string NumPlate { get; }

        public AssetDoesNotExistAfterMadePersistentException(string numPlate)
        {
            NumPlate = numPlate;
        }

        public AssetDoesNotExistAfterMadePersistentException(string message, string numPlate) : base(message)
        {
            this.NumPlate = numPlate;
        }

        public override string Message => $" Asset with Number Plate:{NumPlate}, was not made Persistent!";
    }
}