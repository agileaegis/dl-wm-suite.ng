namespace dl.wm.suite.interprocess.api.Commanding.PackageRepository
{
    public sealed class WmPackageRepository
    {
        private WmPackageRepository()
        {
            
        }
        public static WmPackageRepository GetPackageRepository { get; } = new WmPackageRepository();


        #region Command Definition

        public byte HeaderVersionCode => 0x01;  
        public byte TelemetryRowTypeCode => 0x01;  
        public byte TelemetryTypeCode => 0x03;  // Todo: was 0x01
        public byte AttributeTypeCode => 0x02;  

        #endregion

        #region Command Lengths

        public int HeaderVersionLength => 1;
        public int HeaderMessageTypeLength => 1;
        public int HeaderImeiLength => 7;

        public int PayloadTelemetryTimestampLength => 4;
        public int PayloadTelemetryBatteryLength => 1;
        public int PayloadTelemetryTempLength => 1;
        public int PayloadTelemetryLatLength => 4;
        public int PayloadTelemetryLongLength => 4;
        public int PayloadTelemetryAltitudeLength => 2;
        public int PayloadTelemetrySpeedLength => 2;
        public int PayloadTelemetryCourseLength => 1;
        public int PayloadTelemetryNumOfSatellitesLength => 1;
        public int PayloadTelemetryTimeToFixLength => 1;
        public int PayloadTelemetryMeasuredDistanceLength => 2;
        public int PayloadTelemetryFillLevelLength => 1;
        public int PayloadTelemetrySignalLength => 1;
        public int PayloadTelemetryStatusFlagsLength => 1;
        public int PayloadTelemetryLatestResetCauseLength => 1;
        public int PayloadTelemetryFwVerLength => 2;
        
        
        public int PayloadAttributeReportIntervalDefaultLength => 2;
        public int PayloadAttributeReportIntervalAlternateLength => 2;
        public int PayloadAttributeAlternateFromLength => 4;
        public int PayloadAttributeAlternateToLength => 4;
        public int PayloadAttributeGpsFixTimeoutLength => 2;
        public int PayloadAttributeMinimumSatelliteCountLength => 1;
        public int PayloadAttributeOtmAccelerationLength => 1;
        public int PayloadAttributeDurationLength => 1;
        public int PayloadAttributeOtmReportIntervalLength => 1;
        public int PayloadAttributeOtmTimeoutLLength => 1;
        public int PayloadAttributeBinMaxHeightLength => 2;
        public int PayloadAttributeBinMinHeightLength => 2;
        public int PayloadAttributeReportTimeoutLength => 1;
        

        #endregion

        #region Command Offsets

        public byte HeaderVersionOffset => 0;
        public byte HeaderMessageTypeOffset => (byte)(HeaderVersionOffset + HeaderVersionLength);
        public byte HeaderImeiOffset => (byte)(HeaderMessageTypeOffset + HeaderMessageTypeLength);

        public byte PayloadTelemetryTimestampOffset => (byte)(HeaderImeiOffset + HeaderImeiLength);
        public byte PayloadTelemetryBatteryOffset => (byte)((PayloadTelemetryTimestampOffset) + PayloadTelemetryTimestampLength);
        public byte PayloadTelemetryTempOffset => (byte)(PayloadTelemetryBatteryOffset + PayloadTelemetryBatteryLength);
        public byte PayloadTelemetryLatOffset => (byte)(PayloadTelemetryTempOffset + PayloadTelemetryTempLength);
        public byte PayloadTelemetryLongOffset => (byte)(PayloadTelemetryLatOffset + PayloadTelemetryLatLength);
        public byte PayloadTelemetryAltitudeOffset => (byte)(PayloadTelemetryLongOffset + PayloadTelemetryLongLength);
        public byte PayloadTelemetrySpeedOffset => (byte)(PayloadTelemetryAltitudeOffset + PayloadTelemetryAltitudeLength);
        public byte PayloadTelemetryCourseOffset => (byte)(PayloadTelemetrySpeedOffset + PayloadTelemetrySpeedLength);
        public byte PayloadTelemetryNumOfSatellitesOffset => (byte)(PayloadTelemetryCourseOffset + PayloadTelemetryCourseLength);
        public byte PayloadTelemetryTimeToFixOffset => (byte)(PayloadTelemetryNumOfSatellitesOffset + PayloadTelemetryNumOfSatellitesLength);
        public byte PayloadTelemetryMeasuredDistanceOffset => (byte)(PayloadTelemetryTimeToFixOffset + PayloadTelemetryTimeToFixLength);
        public byte PayloadTelemetryFillLevelOffset => (byte)(PayloadTelemetryMeasuredDistanceOffset + PayloadTelemetryMeasuredDistanceLength);
        public byte PayloadTelemetrySignalOffset => (byte)(PayloadTelemetryFillLevelOffset + PayloadTelemetryFillLevelLength);
        public byte PayloadTelemetryStatusFlagsOffset => (byte)(PayloadTelemetrySignalOffset + PayloadTelemetrySignalLength);
        public byte PayloadTelemetryLatestResetCauseOffset => (byte)(PayloadTelemetryStatusFlagsOffset + PayloadTelemetryStatusFlagsLength);
        public byte PayloadTelemetryFwVerOffset => (byte)(PayloadTelemetryLatestResetCauseOffset + PayloadTelemetryLatestResetCauseLength);


        public byte PayloadAttributeReportIntervalDefaultOffset => (byte)(HeaderImeiOffset + HeaderImeiLength);
        public byte PayloadAttributeReportIntervalAlternateOffset => (byte)(PayloadAttributeReportIntervalDefaultOffset + PayloadAttributeReportIntervalDefaultLength);
        public byte PayloadAttributeAlternateFromOffset => (byte)(PayloadAttributeReportIntervalAlternateOffset + PayloadAttributeReportIntervalAlternateLength);
        public byte PayloadAttributeAlternateToOffset => (byte)(PayloadAttributeAlternateFromOffset + PayloadAttributeAlternateFromLength);
        public byte PayloadAttributeGpsFixTimeoutOffset => (byte)(PayloadAttributeAlternateToOffset + PayloadAttributeAlternateToLength);
        public byte PayloadAttributeMinimumSatelliteCountOffset => (byte)(PayloadAttributeGpsFixTimeoutOffset + PayloadAttributeGpsFixTimeoutLength);
        public byte PayloadAttributeOtmAccelerationOffset => (byte)(PayloadAttributeMinimumSatelliteCountOffset + PayloadAttributeMinimumSatelliteCountLength);
        public byte PayloadAttributeDurationOffset => (byte)(PayloadAttributeOtmAccelerationOffset + PayloadAttributeOtmAccelerationLength);
        public byte PayloadAttributeOtmReportIntervalOffset => (byte)(PayloadAttributeDurationOffset + PayloadAttributeDurationLength);
        public byte PayloadAttributeOtmTimeoutLOffset => (byte)(PayloadAttributeOtmReportIntervalOffset + PayloadAttributeOtmReportIntervalLength);
        public byte PayloadAttributeBinMaxHeightOffset => (byte)(PayloadAttributeOtmTimeoutLOffset + PayloadAttributeOtmTimeoutLLength);
        public byte PayloadAttributeBinMinHeightOffset => (byte)(PayloadAttributeBinMaxHeightOffset + PayloadAttributeBinMaxHeightLength);
        public byte PayloadAttributeReportTimeoutOffset => (byte)(PayloadAttributeBinMinHeightOffset + PayloadAttributeBinMinHeightLength);

        #endregion
    }
}
