using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Tours
{
  public class TourAlreadyExistsException : Exception
  {
    public string Name { get; }
    public string BrokenRules { get; }

    public TourAlreadyExistsException(string name)
       : this(name, "NO_BROKEN_RULES")
    {
    }
    public TourAlreadyExistsException(string name, string brokenRules)
    {
      Name = name;
      BrokenRules = brokenRules;
    }

    public override string Message => $" Tour with name:{Name} already Exists!\n Additional info:{BrokenRules}";
  }
}
