using System;

namespace dl.wm.suite.common.infrastructure.Exceptions.Domain.Devices
{
    public class InvalidImeiForDeviceMeasurementException : Exception
    {
    public override string Message => $" Invalid Imei for Device Store Measurements";
    }
    public class InvalidMeasurementModelForDeviceMeasurementException : Exception
    {
    public override string Message => $" Invalid Measurement Model for Device Store Measurements";
    }
}