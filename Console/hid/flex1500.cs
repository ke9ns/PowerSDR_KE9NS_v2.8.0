//=================================================================
// flex1500.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2003-2013  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

using FlexCW;
using System;
using System.Diagnostics;
using System.Threading;
using WDU_DEVICE_HANDLE = System.IntPtr;

namespace PowerSDR
{
    class Flex1500
    {
#if(!NO_1500)
        private const string PROD_TEST_SN = "1234-1234";
        private static WDU_DEVICE_HANDLE r = IntPtr.Zero;
        private static WDU_DEVICE_HANDLE prod_test = IntPtr.Zero;
        private static Flex1500USB F1500 = null;

        public static bool Init()
        {
            if (F1500 == null)
            {
                Flex1500USB.D_ATTACH_DETACH_CALLBACK attach = attachCallback;
                Flex1500USB.D_ATTACH_DETACH_CALLBACK detach = detachCallback;
                F1500 = new Flex1500USB(attach, detach);
                Thread.Sleep(1000);
            }

            foreach (IntPtr key in Flex1500USB.radioArray.Keys)
            {
                string sn = "";
                Flex1500USB.radioArray[key].GetSerialNumber(out sn);

                if (sn == PROD_TEST_SN)
                {
                    if (prod_test == IntPtr.Zero)
                    {
                        prod_test = key;
                        PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK stub_cb_del = CallbackStub;
                        PowerSDR.Flex1500USB.D_INTERCHANGE_BUFFER_CALLBACK AudioCallback = AudioCallbackStub;
                        Flex1500USB.radioArray[prod_test].Init(CallbackStub, CallbackStub, CallbackStub, CallbackStub, AudioCallback, true, false, true, (uint)Audio.BlockSize);

                        // set gen to 10MHz
                        Flex1500USB.radioArray[prod_test].WriteOp((uint)USBHID.Opcode.USB_OP_SET_RX1_FREQ_TW, 0x6AAAAAA, 0);
                    }
                }
            }

            return true;
        }

        /*// cleanup previous active radio
        if (r != IntPtr.Zero)
        {
            Flex1500USB.radioArray[r].StopAudioListener();
            Flex1500USB.radioArray[r].StopListener();
        }

        bool found = false;
        foreach (IntPtr key in Flex1500USB.radioArray.Keys)
        {
            string serial = "";
            Flex1500USB.radioArray[key].GetSerialNumber(out serial);
            if (sn == serial)
            {
                found = true;

                r = key;
                PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK PTT_Callback = MicPTT;
                PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK FlexPTT_Callback = FlexPTT;
                PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK Dash_Callback = Dash;
                PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK Dot_Callback = Dot;
                PowerSDR.Flex1500USB.D_INTERCHANGE_BUFFER_CALLBACK AudioCallback = Audio.Callback1500;
                Flex1500USB.radioArray[r].Init(MicPTT, FlexPTT, Dash, Dot, AudioCallback, true, false, false, (uint)Audio.BlockSize);
                break;
            }
        }

        return found;
    }*/

        public static bool SetActiveRadio(IntPtr key)
        {
            if (!Flex1500USB.radioArray.ContainsKey(key))
                return false;
            r = key;
            PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK PTT_Callback = MicPTT;
            PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK FlexPTT_Callback = FlexPTT;
            PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK Dash_Callback = Dash;
            PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK Dot_Callback = Dot;
            PowerSDR.Flex1500USB.D_INTERCHANGE_BUFFER_CALLBACK AudioCallback = Audio.Callback1500;
            Flex1500USB.radioArray[r].Init(MicPTT, FlexPTT, Dash, Dot, AudioCallback, true, false, true, (uint)Audio.BlockSize);
            return true;
        }

        public static bool SetActiveRadio(string sn)
        {
            foreach (IntPtr key in Flex1500USB.radioArray.Keys)
            {
                string s = GetSerial(key);
                if (s == sn)
                {
                    SetActiveRadio(key);
                    return true;
                }
            }

            return false;
        }

        public static int GetNumDevices()
        {
            int num = Flex1500USB.radioArray.Count;
            if (ProdTestPresent()) num--;
            return num;
        }

        // will return false after a radio reboot (even after radio is powered back on until the radio is selected again)
        public static bool IsRadioPresent()
        {
            return (r != IntPtr.Zero);
        }

        public static int StopListener()
        {
            int x = Flex1500USB.radioArray[r].StopListener();
            Thread.Sleep(100);
            return x;
        }

        public static int StartListener()
        {
            Thread.Sleep(100);
            return Flex1500USB.radioArray[r].StartListener();
        }

        private static void attachCallback(WDU_DEVICE_HANDLE pDev)
        {
            Debug.WriteLine("attach");
            RadiosAvailable.AddPending1500(pDev);
            Init();
        }

        private static void detachCallback(WDU_DEVICE_HANDLE pDev)
        {
            Debug.WriteLine("detach");
            RadiosAvailable.RadioDetach(new Radio(Model.FLEX1500, pDev));
            if (pDev == r)
                r = IntPtr.Zero;
        }

        public static string GetSerial(IntPtr key)
        {
            try
            {
                string s;
                Flex1500USB.radioArray[key].GetSerialNumber(out s);
                return s;
            }
            catch (Exception)
            {
                return "";
            }
        }

        static double Iacc, Qacc;
        private static int AudioSine(float[] AudioInBufI, float[] AudioInBufQ,
            float[] AudioOutBufI, float[] AudioOutBufQ, uint paFlags)
        {
            double phaseStep = 1.0 / 48000.0 * 2 * Math.PI;
            for (int i = 0; i < AudioOutBufI.Length; i++)
            {
                Iacc += phaseStep * 350.0;
                Qacc += phaseStep * 440.0;
                AudioOutBufI[i] = (float)Math.Sin(Iacc);// / 10.0f;
                AudioOutBufQ[i] = (float)Math.Sin(Qacc);// / 10.0f;
                Debug.Assert(AudioInBufI.Length == AudioOutBufI.Length);
                Debug.Assert(AudioInBufQ.Length == AudioOutBufI.Length);
                Debug.Assert(AudioInBufI.Length == AudioOutBufQ.Length);

            }
            return 0;
            // put code here
        }

        public static bool ProdTestPresent()
        {
            return (prod_test != IntPtr.Zero);
        }

        private static void CallbackStub(bool b)
        {

        }

        private static int AudioCallbackStub(float[] AudioInBufI, float[] AudioInBufQ,
            float[] AudioOutBufI, float[] AudioOutBufQ, uint paFlags)
        {
            return 0;
        }

        private static Console console = null;
        public static Console Console
        {
            set { console = value; }
        }

        private static void MicPTT(bool state)
        {
            if (console == null) return;
            console.HIDMicPTT = state;
        }

        private static void FlexPTT(bool state)
        {
            if (console == null) return;
            console.HIDPTTIn = state;
        }

        private static bool ignore_dash = false;
        public static bool IgnoreDash
        {
            set { ignore_dash = value; }
        }

        private static bool reverse_paddles = false;
        public static bool ReversePaddles
        {
            set { reverse_paddles = value; }
        }

        private static void Dash(bool state)
        {
            if (ignore_dash) return;

            CWSensorItem.InputType type = CWSensorItem.InputType.Dash;
            if (reverse_paddles) type = CWSensorItem.InputType.Dot;

            CWSensorItem item = new CWSensorItem(type, state);
            CWKeyer.SensorEnqueue(item);
        }

        private static void Dot(bool state)
        {
            CWSensorItem.InputType type = CWSensorItem.InputType.Dot;
            if (reverse_paddles) type = CWSensorItem.InputType.Dash;

            CWSensorItem item = new CWSensorItem(type, state);
            CWKeyer.SensorEnqueue(item);
        }

        public static string SerialToString(uint serial)
        {
            string s = "";
            s += ((byte)(serial)).ToString("00");
            s += ((byte)(serial >> 8)).ToString("00") + "-";
            s += ((ushort)(serial >> 16)).ToString("0000");
            return s;
        }

        public static string RevToString(uint rev)
        {
            string s = "";
            s += ((byte)(rev >> 24)).ToString() + ".";
            s += ((byte)(rev >> 16)).ToString() + ".";
            s += ((byte)(rev >> 8)).ToString() + ".";
            s += ((byte)(rev >> 0)).ToString();
            return s;
        }


        //==============================================================================
        public static int WriteOp(USBHID.Opcode opcode, uint param1, uint param2)
        {
            if (r == IntPtr.Zero) return -1;
            return Flex1500USB.radioArray[r].WriteOp((uint)opcode, param1, param2);
        }

        public static int ReadOp(USBHID.Opcode opcode, uint param1, uint param2, out uint result)
        {
            if (r == IntPtr.Zero)
            {
                result = 0;
                return -1;
            }
            return Flex1500USB.radioArray[r].ReadOp((uint)opcode, param1, param2, out result);
        }
        //==============================================================================

        public static int WriteEEPROM(ushort offset, byte[] data)
        {
            if (r == IntPtr.Zero) return -1;
            return Flex1500USB.radioArray[r].WriteEEPROM(offset, data);
        }

        unsafe public static int ReadEEPROM(ushort offset, byte num_bytes, out byte[] data)
        {
            if (r == IntPtr.Zero)
            {
                data = null;
                return -1;
            }
            return Flex1500USB.radioArray[r].ReadEEPROM(offset, num_bytes, out data);
        }

        public static void StartAudio()
        {
            if (r == IntPtr.Zero) return;
            Flex1500USB.radioArray[r].StartAudioListener();

            // commented while using hybrid driver
        }

        public static void StopAudio()
        {
            if (r == IntPtr.Zero) return;
            Flex1500USB.radioArray[r].StopAudioListener();

            // commented while using hybrid driver
        }

        public static int ProdTestWriteOp(USBHID.Opcode opcode, uint param1, uint param2)
        {
            if (prod_test == IntPtr.Zero) return -1;
            return Flex1500USB.radioArray[prod_test].WriteOp((uint)opcode, param1, param2);
        }

        public static int ProdTestReadOp(USBHID.Opcode opcode, uint param1, uint param2, out uint result)
        {
            if (prod_test == IntPtr.Zero)
            {
                result = 0;
                return -1;
            }
            return Flex1500USB.radioArray[prod_test].ReadOp((uint)opcode, param1, param2, out result);
        }

        public static void SetAudioBuffer(uint size)
        {
            if (r == IntPtr.Zero) return;
            Flex1500USB.radioArray[r].changeApplicationBufferSize(size);
        }

        public static void FlushAudioBuffers()
        {
            if (r == IntPtr.Zero) return;
            Flex1500USB.radioArray[r].FlushAudioBuffers();
        }

        public static double GetCurrentLatency()
        {
            if (r == IntPtr.Zero) return 0.0;
            double d = Flex1500USB.radioArray[r].GetCurrentLatency() / 192.0;
            Debug.WriteLine("CurrentLatency: " + d.ToString("f1"));
            return d;
        }

        public static void SetFinalPacketSize(uint val) // in ms
        {
            if (r == IntPtr.Zero) return;
            Flex1500USB.radioArray[r].FinalPacketSize = val;
        }
#else
        public static bool Init()
        {
            return false;
        }

        public static bool IsRadioPresent()
        {
            return false;
        }

        public static bool ProdTestPresent()
        {
            return false;
        }

        private static Console console = null;
        public static Console Console
        {
            set { console = value; }
        }

        public static int WriteOp(USBHID.Opcode opcode, uint param1, uint param2)
        {
            return -1;
        }

        public static int ReadOp(USBHID.Opcode opcode, uint param1, uint param2, out uint result)
        {
            result = 0;
            return -1;            
        }

        public static int WriteEEPROM(ushort offset, byte[] data)
        {
            return -1;            
        }

        unsafe public static int ReadEEPROM(ushort offset, byte num_bytes, out byte[] data)
        {
            data = new byte[0];
            return -1;            
        }

        public static void StartAudio()
        {
            
        }

        public static void StopAudio()
        {
            
        }

        public static int ProdTestWriteOp(USBHID.Opcode opcode, uint param1, uint param2)
        {
            return -1;            
        }

        public static int ProdTestReadOp(USBHID.Opcode opcode, uint param1, uint param2, out uint result)
        {
            result = 0;
            return -1;      
        }
#endif
    }
}
