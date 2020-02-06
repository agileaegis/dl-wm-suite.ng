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

        public TelemetryRowPackageDetectedInboundCommandBuilder()
        {
        }

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
                Longitude = ((double)GetLittleEndianIntegerFromByteFourBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryLongOffset) / 10000000),
                Latitude = ((double)GetLittleEndianIntegerFromByteFourBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryLatOffset) / 10000000),
                Altitude = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryAltitudeOffset).ToString(),
                Speed = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetrySpeedOffset).ToString(),
                Timestamp = FromUnixTime(GetLittleEndianIntegerFromByteFourBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryTimestampOffset)),
                Battery = Decimal.Divide((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryBatteryOffset] * 10 + 3000, 1000),
                FillLevel = Decimal.Divide((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryFillLevelOffset], 100),
                Temperature = (decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryTempOffset],
                Distance = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryMeasuredDistanceOffset).ToString(),
                Imei = ImeiValue,
                Version = VersionValue,
                Course = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryCourseOffset]).ToString(),
                TimeToFix = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryTimeToFixOffset]).ToString(),
                NumOfSatellites =  ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryNumOfSatellitesOffset]).ToString(),
                StatusFlags = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryStatusFlagsOffset]).ToString(),
                SignalLength = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetrySignalOffset]).ToString(),
                LatestResetCause = ((decimal)_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryLatestResetCauseOffset]).ToString(),
                FirmwareVersion = GetLittleEndianIntegerFromByteTwoBytesArray(_wmPackage, WmPackageRepository.GetPackageRepository.PayloadTelemetryFwVerOffset).ToString(),
            };

            Message = JsonConvert.SerializeObject(telemetryRow);
        }
    }
}