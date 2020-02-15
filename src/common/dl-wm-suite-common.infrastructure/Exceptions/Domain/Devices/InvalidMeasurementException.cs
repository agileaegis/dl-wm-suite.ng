using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
  public class InvalidMeasurementException : Exception
  {
    public string MessageValue { get; }

    public InvalidMeasurementException(string messageValue)
    {
      MessageValue = messageValue;
    }

    public override string Message => MessageValue;
  }
}