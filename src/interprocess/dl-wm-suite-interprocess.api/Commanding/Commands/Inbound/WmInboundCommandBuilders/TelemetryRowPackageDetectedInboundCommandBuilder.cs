using System;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using dl.wm.suite.interprocess.api.Commanding.Models.Rows;
using dl.wm.suite.interprocess.api.Commanding.PackageRepository;
using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
    public class TelemetryRowPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {
        private byte[] _wmPackage;

        public WmInboundCommand Build(byte[] wmPackage)
        {
            _wmPackage = wmPackage;
            return new TelemetryRowDetected(BuildMessage(wmPackage), ImeiValue);
        }

        public override void BuildPayload()
        {
            TelemetryRow telemetryRow = new TelemetryRow
            {
                CommandType = "01",
                Version = VersionValue,
                Imei = ImeiValue,

                Temperature = (double)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryTempOffset],
                FillLevel = (double)(Decimal.Divide((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryFillLevelOffset], 100)),
                TiltX = 90,
                TiltY = 00,
                TiltZ = 90,
                Light = 17,
                Battery = (double)(Decimal.Divide((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryBatteryOffset] * 10 + 3000, 1000)),
                Gps = "Gps Value",
                NbIoT = "NbIoT Value",
                Distance = (double)(GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryMeasuredDistanceOffset)),
                Tamper = 1,
                NbIoTSignalLength = 12,
                LatestResetCause = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryLatestResetCauseOffset]).ToString(),
                FirmwareVersion = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryFwVerOffset).ToString(),

                TemperatureEnable = true,
                DistanceEnable = true,
                TiltEnable = true,
                MagnetometerEnable = true,
                TamperEnable = true,
                LightEnable = true,
                GpsEnable = true,

                BatterySafeMode = 1,
                NbIoTMode = 1,

                Timestamp = FromUnixTime(GetLittleEndianIntegerFromByteFourBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryTimestampOffset)),
                Longitude = ((double)GetLittleEndianIntegerFromByteFourBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryLongOffset) / 10000000),
                Latitude = ((double)GetLittleEndianIntegerFromByteFourBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryLatOffset) / 10000000),
                Altitude = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryAltitudeOffset),
                Angle = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryAltitudeOffset),
                NumOfSatellites =  (int)((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryNumOfSatellitesOffset]),
                Speed = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetrySpeedOffset),
                Bearing = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryCourseOffset]),
                TimeToFix = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryTimeToFixOffset]),
                StatusFlags = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryStatusFlagsOffset]),
                SignalLength = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetrySignalOffset]),
            };

            Message = JsonConvert.SerializeObject(telemetryRow);
        }
    }
}