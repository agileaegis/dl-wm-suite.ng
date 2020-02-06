using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Assets
{
    public class AssetAlreadyExistsException : Exception
    {
        public string NumPlate { get; }

        public string Name { get; }


        public AssetAlreadyExistsException(string assetName)
        {
            Name = assetName;
        }

        public AssetAlreadyExistsException(string message, string numPlate) : base(message)
        {
            this.NumPlate = numPlate;
        }

        public override string Message => $" Asset with Name: {Name} and NumPlate:{NumPlate}" +
                                          " already Exists!";
    }
}