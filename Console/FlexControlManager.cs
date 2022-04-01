

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PowerSDR
{
    public class FlexControlManager
    {
        public delegate void DeviceCountChangedHandler();

        private static List<string> ignore_list = new List<string>();

        private static List<FlexControl> flex_control_list = new List<FlexControl>();

        private static object sync_obj = new object();

        private static uint min_version = 196612u;

        public static int DeviceCount => flex_control_list.Count;

        public static event DeviceCountChangedHandler DeviceCountChanged;

        private static bool FlexControlAvailable(out string port_name)
        {
            port_name = "";
            string text = "Com port Ignore List: {";
            foreach (string item in ignore_list)
            {
                text = text + item + ", ";
            }

            string str = text.TrimEnd(',', ' ');
            str += "}";
            lock (sync_obj)
            {
                try
                {
                    foreach (string item2 in SerialPort.GetPortNames().Reverse())
                    {
                        if (ignore_list.Contains(item2))
                        {
                            continue;
                        }

                        bool flag = false;
                        foreach (FlexControl item3 in flex_control_list)
                        {
                            if (item3.SerialPortName == item2)
                            {
                                flag = true;
                                break;
                            }
                        }

                        if (!flag && FindKnob(item2))
                        {
                            port_name = item2;
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        private static void OnDeviceCountChanged()
        {
            if (FlexControlManager.DeviceCountChanged != null)
            {
                FlexControlManager.DeviceCountChanged();
            }
        }

        public static FlexControl GetFlexControl(int index)
        {
            if (index < 0 || index > flex_control_list.Count - 1)
            {
                return null;
            }

            return flex_control_list[index];
        }

        private static bool CheckVersion(FlexControl flexControl)
        {
            uint firmwareVersion = flexControl.GetFirmwareVersion();
            if (firmwareVersion < min_version)
            {
                string str = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\FlexRadio Systems\\FLEX Firmware\\FlexControl\\";
                string file_name = str + "FlexControl_Firmware_v" + RevToString(min_version) + ".hex";
                if (flexControl.UpdateFirmware(file_name))
                {
                    firmwareVersion = flexControl.GetFirmwareVersion();
                    if (firmwareVersion < min_version)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static string RevToString(uint rev)
        {
            return (byte)(rev >> 24) + "." + (byte)(rev >> 16) + "." + (byte)(rev >> 8) + "." + (byte)rev;
        }

        private static bool FindKnob(string port_name)
        {
            Regex regex = new Regex("F[0-9][0-9A-F]{3};");
            Regex regex2 = new Regex("FF[0-9A-F]{2};");
            SerialPort serialPort = null;
            try
            {
                serialPort = new SerialPort(port_name);
                serialPort.Encoding = Encoding.ASCII;
                serialPort.Open();
                Thread.Sleep(20);
                if (serialPort.BytesToRead > 0)
                {
                    string input = serialPort.ReadExisting();
                    Match match = regex.Match(input);
                    if (match.Success)
                    {
                        serialPort.Close();
                        return true;
                    }

                    match = regex2.Match(input);
                    if (match.Success)
                    {
                        serialPort.Close();
                        return true;
                    }
                }

                serialPort.Close();
                ignore_list.Add(port_name);
            }
            catch (Exception)
            {
                try
                {
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                }
                catch
                {
                }

                serialPort = null;
            }

            return false;
        }

        private static void FlexControl_Disconnect(FlexControl flexControl)
        {
            if (flex_control_list.Contains(flexControl))
            {
                flex_control_list.Remove(flexControl);
                OnDeviceCountChanged();
            }
        }

        public static void Close()
        {
            while (flex_control_list.Count > 0)
            {
                flex_control_list[0].Cleanup();
            }
        }

        public static void Rescan()
        {
            string port_name = "";
            bool flag = false;
            for (int i = 0; i < flex_control_list.Count; i++)
            {
                flex_control_list[i].GetFirmwareVersion();
            }

            try
            {
                do
                {
                    flag = FlexControlAvailable(out port_name);
                    if (flag)
                    {
                        FlexControl flexControl = new FlexControl(port_name);
                        if (CheckVersion(flexControl))
                        {
                            flexControl.Disconnected += FlexControl_Disconnect;
                            flex_control_list.Add(flexControl);
                            OnDeviceCountChanged();
                        }
                    }
                }
                while (flag);
            }
            catch (Exception)
            {
            }
        }
    }
}
