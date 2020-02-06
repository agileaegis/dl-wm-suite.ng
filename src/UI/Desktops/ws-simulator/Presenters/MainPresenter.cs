using System;
using System.Text;
using ws.simulator.Commanding.Events.EventArgs.Inbound;
using ws.simulator.Commanding.Listeners.Inbounds;
using ws.simulator.Commanding.Servers;
using ws.simulator.Helpers;
using ws.simulator.Presenters.Base;
using ws.simulator.UDPs;
using ws.simulator.Views;

namespace ws.simulator.Presenters
{
    public class MainPresenter : BasePresenter<IMainView>, IAckDetectionActionListener
    {
        private readonly IUdpConfiguration _udpConfiguration;

        public MainPresenter(IMainView view)
            : base(view)
        {
            _udpConfiguration = new UdpConfiguration();
            _udpConfiguration.EstablishConnection();

            InterprocessInboundServer.GetInterprocessInboundServer.Attach((IAckDetectionActionListener) this);
        }

        public void SendTelemetryMessage()
        {
            var message = new byte[] {
                HexStringToByteArray(View.TxtVersionMessage)[0], //0x01
                HexStringToByteArray(View.TxtMessageTypeMessage)[0], //0x01,
                HexStringToByteArray(View.TxtImeiOneMessage)[0],
                HexStringToByteArray(View.TxtImeiTwoMessage)[0],
                HexStringToByteArray(View.TxtImeiThreeMessage)[0],
                HexStringToByteArray(View.TxtImeiFourMessage)[0],
                HexStringToByteArray(View.TxtImeiFiveMessage)[0],
                HexStringToByteArray(View.TxtImeiSixMessage)[0],
                HexStringToByteArray(View.TxtImeiSevenMessage)[0],  //0x01,0x45,0x29,0x2a,0x2b,0x54,0xca,
                //0x4d,0x38,0x8f,0x5d,
                HexStringToByteArray(View.TxtTimestampOneMessage)[0], HexStringToByteArray(View.TxtTimestampTwoMessage)[0],
                HexStringToByteArray(View.TxtTimestampThreeMessage)[0], HexStringToByteArray(View.TxtTimestampFourMessage)[0],
                //0x63,
                HexStringToByteArray(View.TxtBatteryMessage)[0],
                //0x1f,
                HexStringToByteArray(View.TxtTemperatureMessage)[0],
                //0x29,0x1e,0x79,0x16,
                HexStringToByteArray(View.TxtLatOneMessage)[0], HexStringToByteArray(View.TxtLatTwoMessage)[0],
                HexStringToByteArray(View.TxtLatThreeMessage)[0], HexStringToByteArray(View.TxtLatFourMessage)[0],
                //0x93,0x33,0x09,0x10,
                HexStringToByteArray(View.TxtLongOneMessage)[0], HexStringToByteArray(View.TxtLongTwoMessage)[0],
                HexStringToByteArray(View.TxtLongThreeMessage)[0], HexStringToByteArray(View.TxtLongFourMessage)[0],
                //0x3d,0x00,
                HexStringToByteArray(View.TxtAltOneMessage)[0], HexStringToByteArray(View.TxtAltTwoMessage)[0],
                //0x00,0x00,
                HexStringToByteArray(View.TxtSpeedOneMessage)[0], HexStringToByteArray(View.TxtSpeedTwoMessage)[0],
                //0x00,
                HexStringToByteArray(View.TxtCourseMessage)[0],
                //0x05,
                HexStringToByteArray(View.TxtNumberSatellitesMessage)[0],
                //0x56,
                HexStringToByteArray(View.TxtTimeToFixMessage)[0],
                //0x64,0x00,
                HexStringToByteArray(View.TxtMeasureDistanceOneMessage)[0],
                HexStringToByteArray(View.TxtMeasureDistanceTwoMessage)[0],
                //0x00,
                HexStringToByteArray(View.TxtFillLevelMessage)[0],
                //0x16,
                HexStringToByteArray(View.TxtSignalMessage)[0],
                //0x4b,
                HexStringToByteArray(View.TxtStatusFlagMessage)[0],
                //0x10,
                HexStringToByteArray(View.TxtResetCauseMessage)[0],
                HexStringToByteArray(View.TxtFirmwareVersionOneMessage)[0],
                HexStringToByteArray(View.TxtFirmwareVersionTwoMessage)[0]       //0x65,0x00
            };

            _udpConfiguration.SendMessageOnDemand(message,
                View.LocalServerIsSelected ? "127.0.0.1" : View.TxtUdpServer);
        }

        public void SendAttributeMessage()
        {
            
        }

        public void Update(object sender, AckDetectionEventArgs e)
        {
            View.IsIncomingMessage = true;
            View.LogEventMessage = MessageBuilder.BuildMessage("Ack was Received!");
        }

        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        public void ServerSelected()
        {
            View.TxtUdpServerEnabled = !View.LocalServerIsSelected;
        }
    }
}