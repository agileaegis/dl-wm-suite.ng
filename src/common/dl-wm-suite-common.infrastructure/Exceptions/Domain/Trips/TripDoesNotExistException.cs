using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips
{
    public class TripDoesNotExistException : Exception
    {
        public int TripId { get; set; }
        public string TripCode { get; set; }

        public TripDoesNotExistException(int id)
        {
            TripId = id;
        }
        public TripDoesNotExistException(string code)
        {
            TripCode = code;
        }

        public override string Message => $"Trip with id: {TripId} or Code :{TripCode} doesn't exist";

    }
}