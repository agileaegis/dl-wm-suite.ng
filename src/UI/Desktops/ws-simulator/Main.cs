using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ws.simulator.Presenters;
using ws.simulator.Views;

namespace ws.simulator
{
    public partial class Main : XtraForm, IMainView
    {
        private MainPresenter _mainPresenter;
        public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };
        private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };

        public Main()
        {
            InitializeComponent();
            InitializePresenters();
        }

        private void InitializePresenters()
        {
            _mainPresenter = new MainPresenter(this);
        }

        public bool IsIncomingMessage { get; set; }

        public string TxtUdpServer
        {
            get => txtUdpServer.Text;
            set => txtUdpServer.Text = value;
        }

        public string LogEventMessage
        {
            set => UpdateLogEventTxt(value, IsIncomingMessage ? LogMsgType.Incoming : LogMsgType.Outgoing);
        }

        public string TxtVersionMessage
        {
            get => txtVersion.Text;
            set => txtVersion.Text = value;
        }

        public string TxtMessageTypeMessage
        {
            get => txtMessageType.Text;
            set => txtMessageType.Text = value;
        }

        public string TxtImeiOneMessage
        {
            get => txtImeiOne.Text;
            set => txtImeiOne.Text = value;
        }

        public string TxtImeiTwoMessage
        {
            get => txtImeiTwo.Text;
            set => txtImeiTwo.Text = value;
        }

        public string TxtImeiThreeMessage
        {
            get => txtImeiThree.Text;
            set => txtImeiThree.Text = value;
        }

        public string TxtImeiFourMessage
        {
            get => txtImeiFour.Text;
            set => txtImeiFour.Text = value;
        }

        public string TxtImeiFiveMessage
        {
            get => txtImeiFive.Text;
            set => txtImeiFive.Text = value;
        }

        public string TxtImeiSixMessage
        {
            get => txtImeiSix.Text;
            set => txtImeiSix.Text = value;
        }

        public string TxtImeiSevenMessage
        {
            get => txtImeiSeven.Text;
            set => txtImeiSeven.Text = value;
        }

        public string TxtTimestampOneMessage
        {
            get => txtTimeStampOne.Text;
            set => txtTimeStampOne.Text = value;
        }

        public string TxtTimestampTwoMessage
        {
            get => txtTimeStampTwo.Text;
            set => txtTimeStampTwo.Text = value;
        }

        public string TxtTimestampThreeMessage
        {
            get => txtTimeStampThree.Text;
            set => txtTimeStampThree.Text = value;
        }

        public string TxtTimestampFourMessage
        {
            get => txtTimeStampFour.Text;
            set => txtTimeStampFour.Text = value;
        }

        public string TxtBatteryMessage
        {
            get => txtBattery.Text;
            set => txtBattery.Text = value;
        }

        public string TxtTemperatureMessage
        {
            get => txtTemperature.Text;
            set => txtTemperature.Text = value;
        }

        public string TxtLatOneMessage
        {
            get => txtLatOne.Text;
            set => txtLatOne.Text = value;
        }

        public string TxtLatTwoMessage
        {
            get => txtLatTwo.Text;
            set => txtLatTwo.Text = value;
        }

        public string TxtLatThreeMessage
        {
            get => txtLatThree.Text;
            set => txtLatThree.Text = value;
        }

        public string TxtLatFourMessage
        {
            get => txtLatFour.Text;
            set => txtLatFour.Text = value;
        }

        public string TxtLongOneMessage
        {
            get => txtLongOne.Text;
            set => txtLongOne.Text = value;
        }

        public string TxtLongTwoMessage
        {
            get => txtLongTwo.Text;
            set => txtLongTwo.Text = value;
        }

        public string TxtLongThreeMessage
        {
            get => txtLongThree.Text;
            set => txtLongThree.Text = value;
        }

        public string TxtLongFourMessage
        {
            get => txtLongFour.Text;
            set => txtLongFour.Text = value;
        }

        public string TxtAltOneMessage
        {
            get => txtAltOne.Text;
            set => txtAltOne.Text = value;
        }

        public string TxtAltTwoMessage
        {
            get => txtAltTwo.Text;
            set => txtAltTwo.Text = value;
        }

        public string TxtSpeedOneMessage
        {
            get => txtSpeedOne.Text;
            set => txtSpeedOne.Text = value;
        }

        public string TxtSpeedTwoMessage
        {
            get => txtSpeedTwo.Text;
            set => txtSpeedTwo.Text = value;
        }

        public string TxtCourseMessage
        {
            get => txtCourse.Text;
            set => txtCourse.Text = value;
        }

        public string TxtNumberSatellitesMessage
        {
            get => txtSatellites.Text;
            set => txtSatellites.Text = value;
        }

        public string TxtTimeToFixMessage
        {
            get => txtTimeToFix.Text;
            set => txtTimeToFix.Text = value;
        }

        public string TxtMeasureDistanceOneMessage
        {
            get => txtMeasureDistanceOne.Text;
            set => txtMeasureDistanceOne.Text = value;
        }

        public string TxtMeasureDistanceTwoMessage
        {
            get => txtMeasureDistanceTwo.Text;
            set => txtMeasureDistanceTwo.Text = value;
        }

        public string TxtFillLevelMessage
        {
            get => txtFillLevel.Text;
            set => txtFillLevel.Text = value;
        }

        public string TxtSignalMessage
        {
            get => txtSignal.Text;
            set => txtSignal.Text = value;
        }

        public string TxtStatusFlagMessage
        {
            get => txtStatusFlag.Text;
            set => txtStatusFlag.Text = value;
        }

        public string TxtResetCauseMessage
        {
            get => txtLastResetCause.Text;
            set => txtLastResetCause.Text = value;
        }

        public string TxtFirmwareVersionOneMessage
        {
            get => txtFwOne.Text;
            set => txtFwOne.Text = value;
        }

        public string TxtFirmwareVersionTwoMessage
        {
            get => txtFwTwo.Text;
            set => txtFwTwo.Text = value;
        }

        public bool LocalServerIsSelected
        {
            get => (bool)tgglServerSelection.EditValue;
            set => tgglServerSelection.EditValue = value;
        }

        public bool TxtUdpServerEnabled
        {
            get => txtUdpServer.Enabled;
            set => txtUdpServer.Enabled = value;
        }

        delegate void UpdateLogEventTxtDelegate(string mesg, LogMsgType type);

        private void UpdateLogEventTxt(string msg, LogMsgType type)
        {
            if (rchTxtBxLogs.InvokeRequired)
            {
                UpdateLogEventTxtDelegate del = new UpdateLogEventTxtDelegate(UpdateLogEventTxt);
                rchTxtBxLogs.Invoke(del, new object[] { msg, type });
            }
            else
            {
                rchTxtBxLogs.SelectedText = string.Empty;
                rchTxtBxLogs.SelectionFont = new Font(rchTxtBxLogs.SelectionFont, FontStyle.Bold);
                rchTxtBxLogs.SelectionColor = LogMsgTypeColor[(int)type];
                rchTxtBxLogs.AppendText(msg);
                rchTxtBxLogs.ScrollToCaret();
            }
        }


        private void BtnSendTelemetryClick(object sender, EventArgs e)
        {
            _mainPresenter.SendTelemetryMessage();
        }

        private void BtnSendAttributeClick(object sender, EventArgs e)
        {
            _mainPresenter.SendAttributeMessage();
        }

        private void TgglServerSelectionToggled(object sender, EventArgs e)
        {
            _mainPresenter.ServerSelected();
        }

        private void DtEdtDateTimestampEditValueChanged(object sender, EventArgs e)
        {

        }

        private void TmEdtTimeTimestampEditValueChanged(object sender, EventArgs e)
        {

        }

        private void SpnEdtBatteryEditValueChanged(object sender, EventArgs e)
        {

        }

        private void SpnEdtTemperatureEditValueChanged(object sender, EventArgs e)
        {

        }

        private void SpnEdtDistanceEditValueChanged(object sender, EventArgs e)
        {

        }

        private void SpnEdtFillLevelEditValueChanged(object sender, EventArgs e)
        {

        }

        private void CmbBxEdtResetEditValueChanged(object sender, EventArgs e)
        {

        }

        private void TxtEdtFirmwareEditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
