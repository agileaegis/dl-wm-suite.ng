using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Assets
{
    public class AssetDoesNotExistException : Exception
    {
        public int AssetId { get; set; }
        public string AssetNumPlate { get; set; }

        public AssetDoesNotExistException(int id)
        {
            AssetId = id;
        }

        public AssetDoesNotExistException(string numPlate)
        {
            AssetNumPlate = numPlate;
        }

        public override string Message => $"Asset with id: {AssetId} or Num Plate: {AssetNumPlate} doesn't exist";
    }
}