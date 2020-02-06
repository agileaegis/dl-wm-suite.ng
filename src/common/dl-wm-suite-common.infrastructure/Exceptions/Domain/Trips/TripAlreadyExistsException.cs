using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips
{
    public class TripAlreadyExistsException : Exception
    {
        public string Code { get; }


        public TripAlreadyExistsException(string tripCode)
        {
            Code = tripCode;
        }

        public TripAlreadyExistsException(string message, string tripCode) : base(message)
        {
            this.Code = tripCode;
        }

        public override string Message => $" Trip with Code: {Code}" +
                                          " already Exists!";
    }
}