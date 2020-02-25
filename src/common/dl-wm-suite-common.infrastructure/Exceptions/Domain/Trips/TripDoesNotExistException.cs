using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Trips
{
    public class TripDoesNotExistException : Exception
    {
        public Guid TripId { get; set; }
        public int TripIntId { get; set; }
        public string TripCode { get; set; }

        public TripDoesNotExistException(Guid id)
        {
            TripId = id;
        }     
        
        public TripDoesNotExistException(int id)
        {
          TripIntId = id;
        }
        public TripDoesNotExistException(string code)
        {
            TripCode = code;
        }

        public override string Message => $"Trip with id: {TripId} or Code :{TripCode} doesn't exist";

    }
}