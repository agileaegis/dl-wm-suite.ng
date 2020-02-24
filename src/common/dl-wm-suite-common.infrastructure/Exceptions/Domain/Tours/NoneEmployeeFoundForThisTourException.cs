using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Tours
{
  public class NoneEmployeeFoundForThisTourException : Exception
  {
    public string Name { get; }

    public NoneEmployeeFoundForThisTourException(string name)
    {
      Name = name;
    }

    public override string Message => $"None valid Employee found for Tour with name:{Name}";
  }
}
