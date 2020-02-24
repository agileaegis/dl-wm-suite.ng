using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Tours
{
  public class TourDoesNotExistAfterMadePersistentException : Exception
  {
    public Guid TourId { get; private set; }
    public string Name { get; private set; }

    public TourDoesNotExistAfterMadePersistentException(string name)
    {
      Name = name;
    }
    public TourDoesNotExistAfterMadePersistentException(Guid tourId)
    {
      TourId = tourId;
    }

    public override string Message => $" Tour with Name: {Name} or Id: {TourId} was not made Persistent!";
  }
}