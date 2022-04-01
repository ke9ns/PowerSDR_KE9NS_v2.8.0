

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace PowerSDR //Flex.Control
{
    public class FlexControl
    {
        public enum RotateDirection
        {
            Clockwise,
            CounterClockwise
        }

        public delegate void KnobRotatedEventHandler(RotateDirection direction, int num_steps);

        public enum Button
        {
            Left,
            Middle,
            Right,
            Knob
        }

        public enum ClickType
        {
            Single,
            Double,
            Long
        }

        public delegate void ButtonClickedEventHandler(Button button, ClickType type);

        public delegate void DisconnectHandler(FlexControl flex_control);

        private SerialPort serial_port;

        private string knobOutBuffer = "";

        private List<string> replyBuffer = new List<string>();

        private object replyBufferLock = new object();

        private bool updating;

        public string SerialPortName
        {
            get
            {
                if (serial_port == null)
                {
                    return "";
                }

                return serial_port.PortName;
            }
        }

        public bool Present => serial_port != null;

        public event KnobRotatedEventHandler KnobRotated;

        public event ButtonClickedEventHandler ButtonClicked;

        public event DisconnectHandler Disconnected;

        private void OnKnobRotated(RotateDirection direction, int num_steps)
        {
            if (this.KnobRotated != null)
            {
                this.KnobRotated(direction, num_steps);
            }
        }

        private void OnButtonClicked(Button button, ClickType type)
        {
            if (this.ButtonClicked != null)
            {
                this.ButtonClicked(button, type);
            }
        }

        private void OnDisconnected(FlexControl flex_control)
        {
            if (this.Disconnected != null)
            {
                this.Disconnected(flex_control);
            }
        }

        public FlexControl(string port_name)
        {
            Init(port_name);
        }

        public bool Init(string port_name)
        {
            try
            {
                serial_port = new SerialPort(port_name);
                serial_port.Encoding = Encoding.ASCII;
                serial_port.ReadTimeout = 5000;
                serial_port.WriteTimeout = 500;
                serial_port.ReceivedBytesThreshold = 1;
                serial_port.DataReceived += serial_port_DataReceived;
                serial_port.Open();
                serial_port.Disposed += serial_port_Disposed;
                if (GetFirmwareVersion() != 0)
                {
                    return true;
                }

                serial_port = null;
                return false;
            }
            catch (Exception)
            {
                serial_port = null;
                return false;
            }
        }

        private void serial_port_Disposed(object sender, EventArgs e)
        {
            try
            {
                if (serial_port.IsOpen)
                {
                    serial_port.Close();
                }

                serial_port = null;
            }
            catch
            {
            }

            OnDisconnected(this);
        }

        public void Cleanup()
        {
            if (updating || !Present)
            {
                return;
            }

            try
            {
                if (serial_port.IsOpen)
                {
                    serial_port.Close();
                }

                serial_port = null;
            }
            catch
            {
            }
        }

        private void serial_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            knobOutBuffer += serial_port.ReadExisting();
            int num = -1;
            while ((num = knobOutBuffer.IndexOf(';')) >= 0)
            {
                string token = knobOutBuffer.Substring(0, num + 1);
                knobOutBuffer = knobOutBuffer.Substring(num + 1);
                Parse(token);
            }
        }

        private bool Parse(string token)
        {
            bool result = true;
            switch (token)
            {
                case "U;":
                    OnKnobRotated(RotateDirection.Clockwise, 1);
                    break;
                case "D;":
                    OnKnobRotated(RotateDirection.CounterClockwise, 1);
                    break;
                case "S;":
                    OnButtonClicked(Button.Knob, ClickType.Single);
                    break;
                case "C;":
                    OnButtonClicked(Button.Knob, ClickType.Double);
                    break;
                case "L;":
                    OnButtonClicked(Button.Knob, ClickType.Long);
                    break;
                default:
                    if (token.StartsWith("U") && token.Length == 4)
                    {
                        int num_steps = int.Parse(token.Substring(1, 2));
                        OnKnobRotated(RotateDirection.Clockwise, num_steps);
                        break;
                    }

                    if (token.StartsWith("D") && token.Length == 4)
                    {
                        int num_steps2 = int.Parse(token.Substring(1, 2));
                        OnKnobRotated(RotateDirection.CounterClockwise, num_steps2);
                        break;
                    }

                    if (token.StartsWith("X") && token.Length == 4)
                    {
                        Button button = Button.Left;
                        switch (token.Substring(1, 1))
                        {
                            case "1":
                                button = Button.Left;
                                break;
                            case "2":
                                button = Button.Middle;
                                break;
                            case "3":
                                button = Button.Right;
                                break;
                        }

                        ClickType type = ClickType.Single;
                        switch (token.Substring(2, 1))
                        {
                            case "S":
                                type = ClickType.Single;
                                break;
                            case "C":
                                type = ClickType.Double;
                                break;
                            case "L":
                                type = ClickType.Long;
                                break;
                        }

                        OnButtonClicked(button, type);
                        break;
                    }

                    if ((token.StartsWith("F") && token.Length == 6) || (token.StartsWith("Q") && token.Length == 5) || (token.StartsWith("I") && token.Length == 5) || (token.StartsWith("ZC") && token.Length == 5) || (token.StartsWith("ZL") && token.Length == 5) || (token.StartsWith("ZE") && token.Length == 5) || (token.StartsWith("ZR") && token.Length == 5) || (token.StartsWith("BL!;") && token.Length == 4) || (token.StartsWith("BL=;") && token.Length == 4) || (token.StartsWith("TK!;") && token.Length == 4) || (token.StartsWith("TK=;") && token.Length == 4) || (token.StartsWith("\u0013.;") && token.Length == 3) || (token.StartsWith("!OK!;") && token.Length == 5) || (token.StartsWith("!BAD!;") && token.Length == 6))
                    {
                        lock (replyBufferLock)
                        {
                            replyBuffer.Add(token);
                            return result;
                        }
                    }

                    result = false;
                    break;
            }

            return result;
        }

        private string getReply(string start, int length)
        {
            int num = 0;
            while (num++ < 100)
            {
                if (replyBuffer.Count > 0)
                {
                    for (int i = 0; i < replyBuffer.Count; i++)
                    {
                        if (replyBuffer[i].StartsWith(start) && replyBuffer[i].Length == length)
                        {
                            string result = replyBuffer[i];
                            lock (replyBuffer)
                            {
                                replyBuffer.RemoveAt(i);
                                return result;
                            }
                        }
                    }
                }

                Thread.Sleep(5);
            }

            return "";
        }

        private bool serialPortWrite(string s)
        {
            if (serial_port == null || !serial_port.IsOpen)
            {
                return false;
            }

            try
            {
                serial_port.Write(s);
            }
            catch (IOException)
            {
                try
                {
                    serial_port.Close();
                }
                catch
                {
                }

                serial_port = null;
                OnDisconnected(this);
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public uint GetFirmwareVersion()
        {
            if (!serialPortWrite("F;"))
            {
                return 0u;
            }

            string reply = getReply("FF", 6);
            if (reply != "")
            {
                return 1u;
            }

            reply = getReply("F", 6);
            if (reply == "")
            {
                return 0u;
            }

            uint num = 0u;
            for (int i = 0; i < 4; i++)
            {
                uint num2 = uint.Parse(reply.Substring(i + 1, 1), NumberStyles.HexNumber);
                num += num2 * (uint)Math.Pow(256.0, 3 - i);
            }

            return num;
        }

        public bool GetLEDStatus(int LEDnum)
        {
            if (!serialPortWrite("Q;"))
            {
                return false;
            }

            string reply = getReply("Q", 5);
            if (reply == "")
            {
                return false;
            }

            return reply.Substring(LEDnum, 1) == "1";
        }

        public bool SetLEDStatus(bool LED1on, bool LED2on, bool LED3on)
        {
            string str = "I";
            str += (LED1on ? "1" : "0");
            str += (LED2on ? "1" : "0");
            str += (LED3on ? "1;" : "0;");
            return serialPortWrite(str);
        }

        public bool SetDoubleClickTimer(int ms)
        {
            if (ms < 0 || ms > 1275)
            {
                return false;
            }

            byte b = (byte)(ms / 5);
            string str = "ZC";
            str = str + b.ToString("X2") + ";";
            return serialPortWrite(str);
        }

        public bool SetLongClickTimer(int ms)
        {
            if (ms < 0 || ms > 1275)
            {
                return false;
            }

            byte b = (byte)(ms / 5);
            string str = "ZL";
            str = str + b.ToString("X2") + ";";
            return serialPortWrite(str);
        }

        public bool SetEncoderStepMapping(byte num)
        {
            string str = "ZE";
            str = str + num.ToString("X2") + ";";
            return serialPortWrite(str);
        }

        public bool SetRateLimitPeriod(int ms)
        {
            if (ms < 0 || ms > 1275)
            {
                return false;
            }

            byte b = (byte)(ms / 5);
            string str = "ZR";
            str = str + b.ToString("X2") + ";";
            return serialPortWrite(str);
        }

        public string GetDoubleClickTimer()
        {
            if (!serialPortWrite("ZC;"))
            {
                return "";
            }

            string reply = getReply("ZC", 5);
            if (reply == "")
            {
                return "";
            }

            return reply.Substring(2, 2);
        }

        public string GetLongClickTimer()
        {
            if (!serialPortWrite("ZL;"))
            {
                return "";
            }

            string reply = getReply("ZL", 5);
            if (reply == "")
            {
                return "";
            }

            return reply.Substring(2, 2);
        }

        public string GetEncoderStepMapping()
        {
            if (!serialPortWrite("ZE;"))
            {
                return "";
            }

            string reply = getReply("ZE", 5);
            if (reply == "")
            {
                return "";
            }

            return reply.Substring(2, 2);
        }

        public string GetRateLimitPeriod()
        {
            if (!serialPortWrite("ZR;"))
            {
                return "";
            }

            string reply = getReply("ZR", 5);
            if (reply == "")
            {
                return "";
            }

            return reply.Substring(2, 2);
        }

        public bool UpdateFirmware(string file_name)
        {
            if (serial_port == null || !serial_port.IsOpen)
            {
                return false;
            }

            if (!File.Exists(file_name))
            {
                return false;
            }

            updating = true;
            bool isBackground = Thread.CurrentThread.IsBackground;
            Thread.CurrentThread.IsBackground = false;
            serial_port.Write("BL!;");
            string reply = getReply("BL", 4);
            if (reply == "")
            {
                return false;
            }

            if (reply == "BL!;")
            {
                serial_port.Close();
                Thread.Sleep(10000);
                serial_port.Open();
                Thread.Sleep(50);
                serial_port.Write("F;");
                reply = getReply("FF", 6);
                if (!reply.StartsWith("FF"))
                {
                    return false;
                }
            }

            TextReader textReader = new StreamReader(file_name, Encoding.ASCII);
            serial_port.DataReceived -= serial_port_DataReceived;
            string text = "";
            bool result = false;
            while ((text = textReader.ReadLine()) != null)
            {
                serial_port.WriteLine(text);
                for (int i = 0; i < 50; i++)
                {
                    reply = serial_port.ReadExisting();
                    if (reply.Length > 0 && reply.Contains("."))
                    {
                        break;
                    }

                    Thread.Sleep(1);
                }
            }

            if (text == null)
            {
                result = true;
            }

            serial_port.DataReceived += serial_port_DataReceived;
            textReader.Close();
            serial_port.Write("TK!;");
            reply = getReply("TK!;", 4);
            serial_port.Close();
            Thread.Sleep(10000);
            serial_port.Open();
            Thread.Sleep(50);
            reply = getReply("F", 6);
            updating = false;
            Thread.CurrentThread.IsBackground = isBackground;
            return result;
        }
    }
}
