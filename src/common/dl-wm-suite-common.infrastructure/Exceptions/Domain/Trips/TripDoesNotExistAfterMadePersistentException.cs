using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips
{
    public class TripDoesNotExistAfterMadePersistentException : Exception
    {
        public string Code { get; }

        public TripDoesNotExistAfterMadePersistentException(string tripCode)
        {
            Code = tripCode;
        }

        public TripDoesNotExistAfterMadePersistentException(string message, string tripCode) : base(message)
        {
            this.Code = tripCode;
        }

        public override string Message => $" Trip with code:{Code}, was not made Persistent!";
    }
}