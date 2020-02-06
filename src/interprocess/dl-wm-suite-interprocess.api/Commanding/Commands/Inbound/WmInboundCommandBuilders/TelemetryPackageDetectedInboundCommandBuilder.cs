using System;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands;
using dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using dl.wm.suite.interprocess.api.Commanding.Models.Qt;
using dl.wm.suite.interprocess.api.Commanding.PackageRepository;
using Newtonsoft.Json;

namespace dl.wm.suite.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
    public class TelemetryPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {
        private byte[] _wmPackage;

        public TelemetryPackageDetectedInboundCommandBuilder()
        {
        }

        public WmInboundCommand Build(byte[] wmPackage)
        {
            _wmPackage = wmPackage;
            return new TelemetryDetected(BuildMessage(wmPackage), ExtractImei(wmPackage));
        }

        public override void BuildPayload()
        {
            //Todo: Refactoring to support abstract builder for Telemetry JSON
            Telemetry telemetry = new Telemetry();
            telemetry.Features[0].Geometry.Type = "Point";
            telemetry.Features[0].Geometry.Coordinates.Add(37.85172981747897);
            telemetry.Features[0].Geometry.Coordinates.Add(23.75306636095047);
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.DumpsterId = 1;
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.Props.Capacity = 1100;
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.Props.Description = "";
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.Props.IsFixed = false;
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.Props.IsUnderground = false;
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.Props.Material = "Plastic";
            telemetry.Features[0].Properties.Dumpsters[0].Dumpster.Props.WasteType = "trash";
            telemetry.Features[0].Properties.Dumpsters[0].Sensor.Geometry = null;
            telemetry.Features[0].Properties.Dumpsters[0].Sensor.Props.Bat = Decimal.Divide(
                _wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryBatteryOffset] * 10 + 3000, 1000);
            telemetry.Features[0].Properties.Dumpsters[0].Sensor.Props.Fill =
                Decimal.Divide(_wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryFillLevelOffset],
                    100);
            telemetry.Features[0].Properties.Dumpsters[0].Sensor.Props.Firmware = "V1.0.1";
            telemetry.Features[0].Properties.Dumpsters[0].Sensor.Props.Temp =
                (decimal) _wmPackage[WmPackageRepository.GetPackageRepository.PayloadTelemetryTempOffset];
            telemetry.Features[0].Properties.Dumpsters[0].Sensor.SensorId = 1;

            Message = JsonConvert.SerializeObject(telemetry);
        }
    }
}