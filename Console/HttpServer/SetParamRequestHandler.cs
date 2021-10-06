using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpServer
{
    public class SetParamRequestHandler : IHandler
    {
        public SetParamRequestHandler(String body, TcpClient tcpClient) :
            base(body, tcpClient)
        {

        }

        private const string LEFT_BUTTON_PANORAM_CLICK = "clickLeftOnPanoram";
        private const string RIGHT_BUTTON_PANORAM_CLICK = "clickRightOnPanoram";
        private const string SET_BAND = "setBand";
        private const string SET_MODE = "setMode";
        private const string SET_FILTER = "setFilter";
        private const string SCROLL_FREQ = "scrollFreq";

        private const string BAND160 = "band160";
        private const string BAND80 = "band80";
        private const string BAND60 = "band60";
        private const string BAND40 = "band40";
        private const string BAND30 = "band30";
        private const string BAND20 = "band20";
        private const string BAND17 = "band17";
        private const string BAND15 = "band15";
        private const string BAND12 = "band12";
        private const string BAND10 = "band10";
        private const string BAND6 = "band6";
        private const string BAND2 = "band2";

        private const string LSB = "LSB";
        private const string USB = "USB";
        private const string DSB = "DSB";
        private const string CWL = "CWL";
        private const string CWU = "CWU";
        private const string FM = "FM";
        private const string AM = "AM";
        private const string SAM = "SAM";
        private const string SPEC = "SPEC";
        private const string DIGL = "DIGL";
        private const string DIGU = "DIGU";
        private const string DRM = "DRM";

        private const string F1 = "F1";
        private const string F2 = "F2";
        private const string F3 = "F3";
        private const string F4 = "F4";
        private const string F5 = "F5";
        private const string F6 = "F6";
        private const string F7 = "F7";
        private const string F8 = "F8";
        private const string F9 = "F9";
        private const string F10 = "F10";

        private const Char DATA_SEPARATOR = '=';

        public override void handle()
        {

            if (m_tcpClient == null) return;

            string CodeStr = "200 " + ((HttpStatusCode)200).ToString();

            if (m_body != null && m_body.Length > 0)
            {
                try
                {
                    string[] data = m_body.Split(DATA_SEPARATOR);
                    if (data.Length != 2)
                    {
                        Debug.WriteLine("Receive error data from server");
                        CodeStr = "500" + ((HttpStatusCode)500).ToString();
                    }
                    else
                    {
                        setParam(data[0], data[1]);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }

            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nAccess-Control-Allow-Origin: *\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.UTF8.GetBytes(Str);
            sendAnswer(Buffer);

        }

        void setParam(string paramName, string data)
        {
            if (paramName.CompareTo(LEFT_BUTTON_PANORAM_CLICK) == 0)
            {
                m_console.setVFOAFreqByPixel(Int32.Parse(data));
            }
            else if (paramName.CompareTo(SET_BAND) == 0)
            {
                m_console.SetCATBand(getBand(data));
            }
            else if (paramName.CompareTo(SET_MODE) == 0)
            {
                m_console.RX1DSPMode = getMode(data);
            }
            else if (paramName.CompareTo(SET_FILTER) == 0)
            {
                m_console.RX1Filter = getFilter(data);
            }
            else if (paramName.CompareTo(SCROLL_FREQ) == 0)
            {
                mouseWhell(data);
            }
        }

        PowerSDR.Band getBand(string data)
        {
            switch (data)
            {
                case BAND160: return PowerSDR.Band.B160M;
                case BAND80: return PowerSDR.Band.B80M;
                case BAND60: return PowerSDR.Band.B60M;
                case BAND40: return PowerSDR.Band.B40M;
                case BAND30: return PowerSDR.Band.B30M;
                case BAND20: return PowerSDR.Band.B20M;
                case BAND17: return PowerSDR.Band.B17M;
                case BAND15: return PowerSDR.Band.B15M;
                case BAND12: return PowerSDR.Band.B12M;
                case BAND10: return PowerSDR.Band.B10M;
                case BAND6: return PowerSDR.Band.B6M;
                case BAND2: return PowerSDR.Band.B2M;
                default: return PowerSDR.Band.B160M;
            }
        }

        PowerSDR.DSPMode getMode(string data)
        {
            switch (data)
            {
                case LSB: return PowerSDR.DSPMode.LSB;
                case USB: return PowerSDR.DSPMode.USB;
                case DSB: return PowerSDR.DSPMode.DSB;
                case CWL: return PowerSDR.DSPMode.CWL;
                case CWU: return PowerSDR.DSPMode.CWU;
                case FM: return PowerSDR.DSPMode.FM;
                case AM: return PowerSDR.DSPMode.AM;
                case DIGL: return PowerSDR.DSPMode.DIGL;
                case DIGU: return PowerSDR.DSPMode.DIGU;
                case SAM: return PowerSDR.DSPMode.SAM;
                case SPEC: return PowerSDR.DSPMode.SPEC;
                case DRM: return PowerSDR.DSPMode.DRM;
                default: return PowerSDR.DSPMode.USB;
            }
        }

        PowerSDR.Filter getFilter(string data)
        {
            switch (data)
            {
                case F1: return PowerSDR.Filter.F1;
                case F2: return PowerSDR.Filter.F2;
                case F3: return PowerSDR.Filter.F3;
                case F4: return PowerSDR.Filter.F4;
                case F5: return PowerSDR.Filter.F5;
                case F6: return PowerSDR.Filter.F6;
                case F7: return PowerSDR.Filter.F7;
                case F8: return PowerSDR.Filter.F8;
                case F9: return PowerSDR.Filter.F9;
                case F10: return PowerSDR.Filter.F10;
                default: return PowerSDR.Filter.F1;
            }
        }

        void mouseWhell(string dir)
        {
            if (dir == "1")
            {
                m_console.wheelEventOnWeb(true);
            }
            else
            {
                m_console.wheelEventOnWeb(false);
            }
        }
    }
}
