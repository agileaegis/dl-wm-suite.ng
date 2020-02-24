using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Tours
{
  public class InvalidTourException : Exception
  {
    public string BrokenRules { get; private set; }

    public InvalidTourException(string brokenRules)
    {
      BrokenRules = brokenRules;
    }
  }
}