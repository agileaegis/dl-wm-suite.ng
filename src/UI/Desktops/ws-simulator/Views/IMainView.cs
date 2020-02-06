using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws.simulator.Views
{
    public interface IMainView : IView
    {
        bool IsIncomingMessage { get; set; }
        string TxtUdpServer { get; set; }
        string LogEventMessage { set; }
        string TxtVersionMessage { get; set; }
        string TxtMessageTypeMessage { get; set; }
        string TxtImeiOneMessage { get; set; }
        string TxtImeiTwoMessage { get; set; }
        string TxtImeiThreeMessage { get; set; }
        string TxtImeiFourMessage { get; set; }
        string TxtImeiFiveMessage { get; set; }
        string TxtImeiSixMessage { get; set; }
        string TxtImeiSevenMessage { get; set; }
        string TxtTimestampOneMessage { get; set; }
        string TxtTimestampTwoMessage { get; set; }
        string TxtTimestampThreeMessage { get; set; }
        string TxtTimestampFourMessage { get; set; }
        string TxtBatteryMessage { get; set; }
        string TxtTemperatureMessage { get; set; }
        string TxtLatOneMessage { get; set; }
        string TxtLatTwoMessage { get; set; }
        string TxtLatThreeMessage { get; set; }
        string TxtLatFourMessage { get; set; }
        string TxtLongOneMessage { get; set; }
        string TxtLongTwoMessage { get; set; }
        string TxtLongThreeMessage { get; set; }
        string TxtLongFourMessage { get; set; }
        string TxtAltOneMessage { get; set; }
        string TxtAltTwoMessage { get; set; }
        string TxtSpeedOneMessage { get; set; }
        string TxtSpeedTwoMessage { get; set; }
        string TxtCourseMessage { get; set; }
        string TxtNumberSatellitesMessage { get; set; }
        string TxtTimeToFixMessage { get; set; }
        string TxtMeasureDistanceOneMessage { get; set; }
        string TxtMeasureDistanceTwoMessage { get; set; }
        string TxtFillLevelMessage { get; set; }
        string TxtSignalMessage { get; set; }
        string TxtStatusFlagMessage { get; set; }
        string TxtResetCauseMessage { get; set; }
        string TxtFirmwareVersionOneMessage { get; set; }
        string TxtFirmwareVersionTwoMessage { get; set; }
        bool LocalServerIsSelected { get; set; }
        bool TxtUdpServerEnabled { get; set; }
    }
}
