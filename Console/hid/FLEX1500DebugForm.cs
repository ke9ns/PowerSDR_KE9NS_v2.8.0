//=================================================================
// FLEX1500DebugForm.cs
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

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using WDU_DEVICE_HANDLE = System.IntPtr;


namespace PowerSDR
{

    unsafe public partial class FLEX1500DebugForm : Form
    {
        float tone1 = 350.0f;
        float tone2 = 440.0f;
        Console console;

        public FLEX1500DebugForm(Console c)
        {
            InitializeComponent();
            console = c;

            this.TopMost = true; // ke9ns .174
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
        /*
                private void FLEX1500DebugForm_Load(object sender, EventArgs e)
                {
                    PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK PTT_Callback = PTT;
                    PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK FlexPTT_Callback = FlexPTT;
                    PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK Dash_Callback = Dash;
                    PowerSDR.Flex1500USB.D_STATE_CHANGE_CALLBACK Dot_Callback = Dot;
                    PowerSDR.Flex1500USB.D_INTERCHANGE_BUFFER_CALLBACK InterchangeCallback = AudioSine;
                    PowerSDR.Flex1500USB.D_ATTACH_DETACH_CALLBACK attach = attachCallback;
                    PowerSDR.Flex1500USB.D_ATTACH_DETACH_CALLBACK detach = detachCallback;

                    try
                    {
                        Flex1500USB Flex1500 = new Flex1500USB(attach, detach);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    Thread.Sleep(1000);
                    radioCollection = Flex1500USB.radioArray;

                    if (Flex1500USB.number_devices == 2 && radioCollection.Count == 2)
                    {
                        int i = 0;
                        foreach (var item in radioCollection.Keys)
                        {
                            myRadios[i++] = item;
                        }
                        Flex1500.Init(PTT_Callback, FlexPTT_Callback, Dash_Callback, Dot_Callback, InterchangeCallback, true, false, false, 1024);
                        string sn;
                        Flex1500.GetSerialNumber(out sn);
                        sn1.Text = sn;
                        radioCollection[myRadios[1]].Init(PTT_Callback, FlexPTT_Callback, Dash_Callback, Dot_Callback, InterchangeCallback, false, false, false, 1024);
                        radioCollection[myRadios[1]].GetSerialNumber(out sn);
                        sn2.Text = sn;
                    }
                    else if (Flex1500USB.number_devices == 1 && radioCollection.Count == 1)
                    {
                        int i = 0;
                        foreach (var item in radioCollection.Keys)
                        {
                            myRadios[i++] = item;
                        }
                        string sn;
                        Flex1500.Init(PTT_Callback, FlexPTT_Callback, Dash_Callback, Dot_Callback, InterchangeCallback, true, false, false, 2048);
                        Flex1500.GetSerialNumber(out sn);
                        myRadios[1] = (WDU_DEVICE_HANDLE)0;
                    }
                    else
                    {
                        MessageBox.Show("No hardware found");
                        foreach (Control c in this.Controls)
                            c.Enabled = false;
                        return;
                    }


                    uint rev;
                    Flex1500.GetFirmwareRev(out rev);
                    this.Text += "   " + RevToString(rev);            
                }
                */
        private void attachCallback(WDU_DEVICE_HANDLE pDev)
        {
            Debug.Print("attach");
        }

        private void detachCallback(WDU_DEVICE_HANDLE pDev)
        {
            Debug.Print("detach");
        }

        static double Iacc, Qacc;
        private int AudioSine(float[] AudioInBufI, float[] AudioInBufQ,
            float[] AudioOutBufI, float[] AudioOutBufQ, uint paFlags)
        {
            double phaseStep = 1.0 / 48000.0 * 2 * Math.PI;
            for (int i = 0; i < AudioOutBufI.Length; i++)
            {
                Iacc += phaseStep * tone1;
                Qacc += phaseStep * tone2;
                AudioOutBufI[i] = (float)Math.Sin(Iacc);// / 10.0f;
                AudioOutBufQ[i] = (float)Math.Sin(Qacc);// / 10.0f;
                Debug.Assert(AudioInBufI.Length == AudioOutBufI.Length);
                Debug.Assert(AudioInBufQ.Length == AudioOutBufI.Length);
                Debug.Assert(AudioInBufI.Length == AudioOutBufQ.Length);

            }
            return 0;
            // put code here
        }

        private int AudioSawtooth(float[] AudioInBufI, float[] AudioInBufQ,
            float[] AudioOutBufI, float[] AudioOutBufQ, uint paFlags)
        {
            double phaseStep = 0.03125;
            for (int i = 0; i < AudioOutBufI.Length; i++)
            {
                Iacc += phaseStep;
                Qacc += phaseStep * 1.5;
                if (Iacc > 0.999) Iacc -= 2.0;
                if (Qacc > 0.999) Qacc -= 2.0;
                AudioOutBufI[i] = (float)Iacc;// Math.Sin(Iacc);// / 10.0f;
                AudioOutBufQ[i] = (float)Qacc;// Math.Sin(Qacc);// / 10.0f;
                Debug.Assert(AudioInBufI.Length == AudioOutBufI.Length);
                Debug.Assert(AudioInBufQ.Length == AudioOutBufI.Length);
                Debug.Assert(AudioInBufI.Length == AudioOutBufQ.Length);

            }
            return 0;
            // put code here
        }
        /*
        private void PTT(bool state)
        {
            if (state)
            {
                butPTT.BackColor = Color.GreenYellow;
            }
            else
            {
                butPTT.BackColor = SystemColors.Control;
            }
        }

        private void FlexPTT(bool state)
        {
            if (state)
            {
                butFlexPTT.BackColor = Color.GreenYellow;
            }
            else
            {
                butFlexPTT.BackColor = SystemColors.Control;
            }
        }

        private void Dash(bool state)
        {
            if (state)
            {
                butDash.BackColor = Color.GreenYellow;
            }
            else
            {
                butDash.BackColor = SystemColors.Control;
            }
            ToggleTones();
        }
      
        private void Dot(bool state)
        {
            if (state)
            {
                butDot.BackColor = Color.GreenYellow;
            }
            else
            {
                butDot.BackColor = SystemColors.Control;
            }
            ToggleTones();
        }

        private void ToggleTones()
        {
            if (tone1 == 350.0f)
            {
                tone1 = 941.0f;
                tone2 = 1209.0f;
            }
            else
            {
                tone1 = 350.0f;
                tone2 = 440.0f;
            }
        }
          */
        private void btnWrite_Click(object sender, EventArgs e)
        {
            uint op = uint.Parse(txtOpcode.Text);
            uint p1 = uint.Parse(txtParam1.Text, NumberStyles.HexNumber);
            uint p2 = uint.Parse(txtParam2.Text, NumberStyles.HexNumber);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val = Flex1500.WriteOp((USBHID.Opcode)op, p1, p2);

            t1.Stop();
            Debug.WriteLine("WriteOp: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            uint op = uint.Parse(txtOpcode.Text);
            uint p1 = uint.Parse(txtParam1.Text, NumberStyles.HexNumber);
            uint p2 = uint.Parse(txtParam2.Text, NumberStyles.HexNumber);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            uint result;
            int val = Flex1500.ReadOp((USBHID.Opcode)op, p1, p2, out result);

            t1.Stop();

            Debug.WriteLine("ReadOp: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
            txtResult.Text = result.ToString();
        }

        private void chkLED_CheckedChanged(object sender, EventArgs e)
        {
            USBHID.SetLED(chkLED.Checked);
            if (chkLED.Checked)
                chkLED.BackColor = Color.LightBlue;
            else chkLED.BackColor = SystemColors.Control;
        }

        private void btnEEWrite_Click(object sender, EventArgs e)
        {
            ushort offset = ushort.Parse(txtOffset.Text, NumberStyles.HexNumber);
            byte num_bytes = byte.Parse(txtNumBytes.Text, NumberStyles.HexNumber);

            if (num_bytes > txtData.Text.Length / 2)
            {
                MessageBox.Show("Error: Not enough data");
                return;
            }

            byte[] data = new byte[num_bytes];
            for (int i = 0; i < num_bytes; i++)
                data[i] = byte.Parse(txtData.Text.Substring(i * 2, 2), NumberStyles.HexNumber);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val;
            //fixed(byte* ptr = &data[0])
            val = Flex1500.WriteEEPROM(offset, data);

            t1.Stop();
            Debug.WriteLine("EEWrite: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnEERead_Click(object sender, EventArgs e)
        {
            ushort offset = ushort.Parse(txtOffset.Text, NumberStyles.HexNumber);
            byte num_bytes = byte.Parse(txtNumBytes.Text, NumberStyles.HexNumber);
            byte[] data;

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val;
            val = Flex1500.ReadEEPROM(offset, num_bytes, out data);

            t1.Stop();

            txtEERead.Text = "";
            for (int i = 0; i < num_bytes; i++)
                txtEERead.Text += data[i].ToString("X").PadLeft(2, '0');

            Debug.WriteLine("EERead: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnEEReadFloat_Click(object sender, EventArgs e)
        {
            ushort offset = ushort.Parse(txtOffset.Text, NumberStyles.HexNumber);
            //byte num_bytes = 4;

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val;
            byte[] temp = new byte[4];
            val = Flex1500.ReadEEPROM((ushort)offset, 4, out temp);
            float f = BitConverter.ToSingle(temp, 0);

            t1.Stop();

            txtEERead.Text = f.ToString("f6");

            Debug.WriteLine("EERead Float: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnReadEEFile_Click(object sender, EventArgs e)
        {
            FileStream fs = File.Create("firmware.dat");
            BinaryWriter writer = new BinaryWriter(fs);

            byte[] data;
            for (ushort i = 0; i < 8192; i += 32)
            {
                Flex1500.ReadEEPROM(i, 32, out data);
                writer.Write(data);
            }

            writer.Close();
            fs.Close();
        }

        /*private void btnEELoad_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                c.Enabled = false;
            btnEELoad.BackColor = Color.Red;
            Application.DoEvents();

            FileStream fs = File.Open("FLEX1500.bin", FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);
            byte[] data = new byte[32];
            ushort offset = 0;
            bool eof = false;

            do
            {
                int num_read = reader.Read(data, 0, 32);
                fixed (byte* ptr = &data[0])
                    Flex1500.WriteEEPROM(offset, (byte)num_read, ptr);
                Thread.Sleep(10);
                offset += (ushort)num_read;
                eof = (num_read != 32);
            } while (!eof);

            reader.Close();
            fs.Close();

            btnEELoad.BackColor = Color.Yellow;
            Application.DoEvents();

            // write freq check tuning word boundaries
            bool b = true;
            offset = 0x1830; // region 0: US
            b &= WriteTWBoundary(1.8, false, offset); offset += 4;
            b &= WriteTWBoundary(2.0, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(4.0, false, offset); offset += 4;
            b &= WriteTWBoundary(5.330, false, offset); offset += 4;
            b &= WriteTWBoundary(5.334, false, offset); offset += 4;
            b &= WriteTWBoundary(5.346, false, offset); offset += 4;
            b &= WriteTWBoundary(5.350, false, offset); offset += 4;
            b &= WriteTWBoundary(5.366, false, offset); offset += 4;
            b &= WriteTWBoundary(5.370, false, offset); offset += 4;
            b &= WriteTWBoundary(5.371, false, offset); offset += 4;
            b &= WriteTWBoundary(5.375, false, offset); offset += 4;
            b &= WriteTWBoundary(5.403, false, offset); offset += 4;
            b &= WriteTWBoundary(5.407, false, offset); offset += 4;
            b &= WriteTWBoundary(7.0, false, offset); offset += 4;
            b &= WriteTWBoundary(7.3, false, offset); offset += 4;
            b &= WriteTWBoundary(10.1, false, offset); offset += 4;
            b &= WriteTWBoundary(10.15, false, offset); offset += 4;
            b &= WriteTWBoundary(14.0, false, offset); offset += 4;
            b &= WriteTWBoundary(14.35, false, offset); offset += 4;
            b &= WriteTWBoundary(18.068, false, offset); offset += 4;
            b &= WriteTWBoundary(18.168, false, offset); offset += 4;
            b &= WriteTWBoundary(21.0, false, offset); offset += 4;
            b &= WriteTWBoundary(21.45, false, offset); offset += 4;
            b &= WriteTWBoundary(24.89, false, offset); offset += 4;
            b &= WriteTWBoundary(24.99, false, offset); offset += 4;
            b &= WriteTWBoundary(28.0, false, offset); offset += 4;
            b &= WriteTWBoundary(29.7, false, offset); offset += 4;
            b &= WriteTWBoundary(50.0, false, offset); offset += 4;
            b &= WriteTWBoundary(54.0, false, offset); offset += 4;

            offset = 0x18B0;
            b &= WriteTWBoundary(1.8, true, offset); offset += 4;
            b &= WriteTWBoundary(2.0, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(4.0, true, offset); offset += 4;
            b &= WriteTWBoundary(5.330, true, offset); offset += 4;
            b &= WriteTWBoundary(5.334, true, offset); offset += 4;
            b &= WriteTWBoundary(5.346, true, offset); offset += 4;
            b &= WriteTWBoundary(5.350, true, offset); offset += 4;
            b &= WriteTWBoundary(5.366, true, offset); offset += 4;
            b &= WriteTWBoundary(5.370, true, offset); offset += 4;
            b &= WriteTWBoundary(5.371, true, offset); offset += 4;
            b &= WriteTWBoundary(5.375, true, offset); offset += 4;
            b &= WriteTWBoundary(5.403, true, offset); offset += 4;
            b &= WriteTWBoundary(5.407, true, offset); offset += 4;
            b &= WriteTWBoundary(7.0, true, offset); offset += 4;
            b &= WriteTWBoundary(7.3, true, offset); offset += 4;
            b &= WriteTWBoundary(10.1, true, offset); offset += 4;
            b &= WriteTWBoundary(10.15, true, offset); offset += 4;
            b &= WriteTWBoundary(14.0, true, offset); offset += 4;
            b &= WriteTWBoundary(14.35, true, offset); offset += 4;
            b &= WriteTWBoundary(18.068,  true, offset); offset += 4;
            b &= WriteTWBoundary(18.168,  true, offset); offset += 4;
            b &= WriteTWBoundary(21.0, true, offset); offset += 4;
            b &= WriteTWBoundary(21.45, true, offset); offset += 4;
            b &= WriteTWBoundary(24.89, true, offset); offset += 4;
            b &= WriteTWBoundary(24.99, true, offset); offset += 4;
            b &= WriteTWBoundary(28.0, true, offset); offset += 4;
            b &= WriteTWBoundary(29.7, true, offset); offset += 4;
            b &= WriteTWBoundary(50.0, true, offset); offset += 4;
            b &= WriteTWBoundary(54.0, true, offset); offset += 4;

            offset = 0x1930; // region 1: Spain
            b &= WriteTWBoundary(1.81, false, offset); offset += 4;
            b &= WriteTWBoundary(2.0, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(7.0, false, offset); offset += 4;
            b &= WriteTWBoundary(7.2, false, offset); offset += 4;
            b &= WriteTWBoundary(10.1, false, offset); offset += 4;
            b &= WriteTWBoundary(10.15, false, offset); offset += 4;
            b &= WriteTWBoundary(14.0, false, offset); offset += 4;
            b &= WriteTWBoundary(14.35, false, offset); offset += 4;
            b &= WriteTWBoundary(18.068, false, offset); offset += 4;
            b &= WriteTWBoundary(18.168, false, offset); offset += 4;
            b &= WriteTWBoundary(21.0, false, offset); offset += 4;
            b &= WriteTWBoundary(21.45, false, offset); offset += 4;
            b &= WriteTWBoundary(24.89, false, offset); offset += 4;
            b &= WriteTWBoundary(24.99, false, offset); offset += 4;
            b &= WriteTWBoundary(28.0, false, offset); offset += 4;
            b &= WriteTWBoundary(29.7, false, offset); offset += 4;
            b &= WriteTWBoundary(50.0, false, offset); offset += 4;
            b &= WriteTWBoundary(54.0, false, offset); offset += 4;

            offset = 0x19B0;
            b &= WriteTWBoundary(1.81, true, offset); offset += 4;
            b &= WriteTWBoundary(2.0, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(7.0, true, offset); offset += 4;
            b &= WriteTWBoundary(7.2, true, offset); offset += 4;
            b &= WriteTWBoundary(10.1, true, offset); offset += 4;
            b &= WriteTWBoundary(10.15, true, offset); offset += 4;
            b &= WriteTWBoundary(14.0, true, offset); offset += 4;
            b &= WriteTWBoundary(14.35, true, offset); offset += 4;
            b &= WriteTWBoundary(18.068, true, offset); offset += 4;
            b &= WriteTWBoundary(18.168, true, offset); offset += 4;
            b &= WriteTWBoundary(21.0, true, offset); offset += 4;
            b &= WriteTWBoundary(21.45, true, offset); offset += 4;
            b &= WriteTWBoundary(24.89, true, offset); offset += 4;
            b &= WriteTWBoundary(24.99, true, offset); offset += 4;
            b &= WriteTWBoundary(28.0, true, offset); offset += 4;
            b &= WriteTWBoundary(29.7, true, offset); offset += 4;
            b &= WriteTWBoundary(50.0, true, offset); offset += 4;
            b &= WriteTWBoundary(54.0, true, offset); offset += 4;

            offset = 0x1A30; // region 2: Germany
            b &= WriteTWBoundary(1.81, false, offset); offset += 4;
            b &= WriteTWBoundary(2.0, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(7.0, false, offset); offset += 4;
            b &= WriteTWBoundary(7.2, false, offset); offset += 4;
            b &= WriteTWBoundary(10.1, false, offset); offset += 4;
            b &= WriteTWBoundary(10.15, false, offset); offset += 4;
            b &= WriteTWBoundary(14.0, false, offset); offset += 4;
            b &= WriteTWBoundary(14.35, false, offset); offset += 4;
            b &= WriteTWBoundary(18.068, false, offset); offset += 4;
            b &= WriteTWBoundary(18.168, false, offset); offset += 4;
            b &= WriteTWBoundary(21.0, false, offset); offset += 4;
            b &= WriteTWBoundary(21.45, false, offset); offset += 4;
            b &= WriteTWBoundary(24.89, false, offset); offset += 4;
            b &= WriteTWBoundary(24.99, false, offset); offset += 4;
            b &= WriteTWBoundary(28.0, false, offset); offset += 4;
            b &= WriteTWBoundary(29.7, false, offset); offset += 4;
            b &= WriteTWBoundary(50.08, false, offset); offset += 4;
            b &= WriteTWBoundary(51.0, false, offset); offset += 4;

            offset = 0x1AB0;
            b &= WriteTWBoundary(1.81, true, offset); offset += 4;
            b &= WriteTWBoundary(2.0, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(7.0, true, offset); offset += 4;
            b &= WriteTWBoundary(7.2, true, offset); offset += 4;
            b &= WriteTWBoundary(10.1, true, offset); offset += 4;
            b &= WriteTWBoundary(10.15, true, offset); offset += 4;
            b &= WriteTWBoundary(14.0, true, offset); offset += 4;
            b &= WriteTWBoundary(14.35, true, offset); offset += 4;
            b &= WriteTWBoundary(18.068, true, offset); offset += 4;
            b &= WriteTWBoundary(18.168, true, offset); offset += 4;
            b &= WriteTWBoundary(21.0, true, offset); offset += 4;
            b &= WriteTWBoundary(21.45, true, offset); offset += 4;
            b &= WriteTWBoundary(24.89, true, offset); offset += 4;
            b &= WriteTWBoundary(24.99, true, offset); offset += 4;
            b &= WriteTWBoundary(28.0, true, offset); offset += 4;
            b &= WriteTWBoundary(29.7, true, offset); offset += 4;
            b &= WriteTWBoundary(50.08, true, offset); offset += 4;
            b &= WriteTWBoundary(51.0, true, offset); offset += 4;

            offset = 0x1B30; // region 3: UK
            b &= WriteTWBoundary(1.8, false, offset); offset += 4;
            b &= WriteTWBoundary(2.0, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(4.0, false, offset); offset += 4;
            b &= WriteTWBoundary(5.258, false, offset); offset += 4;
            b &= WriteTWBoundary(5.262, false, offset); offset += 4;
            b &= WriteTWBoundary(5.278, false, offset); offset += 4;
            b &= WriteTWBoundary(5.282, false, offset); offset += 4;
            b &= WriteTWBoundary(5.288, false, offset); offset += 4;
            b &= WriteTWBoundary(5.292, false, offset); offset += 4;
            b &= WriteTWBoundary(5.366, false, offset); offset += 4;
            b &= WriteTWBoundary(5.375, false, offset); offset += 4;
            b &= WriteTWBoundary(5.398, false, offset); offset += 4;
            b &= WriteTWBoundary(5.407, false, offset); offset += 4;
            b &= WriteTWBoundary(7.0, false, offset); offset += 4;
            b &= WriteTWBoundary(7.3, false, offset); offset += 4;
            b &= WriteTWBoundary(10.1, false, offset); offset += 4;
            b &= WriteTWBoundary(10.15, false, offset); offset += 4;
            b &= WriteTWBoundary(14.0, false, offset); offset += 4;
            b &= WriteTWBoundary(14.35, false, offset); offset += 4;
            b &= WriteTWBoundary(18.068, false, offset); offset += 4;
            b &= WriteTWBoundary(18.168, false, offset); offset += 4;
            b &= WriteTWBoundary(21.0, false, offset); offset += 4;
            b &= WriteTWBoundary(21.45, false, offset); offset += 4;
            b &= WriteTWBoundary(24.89, false, offset); offset += 4;
            b &= WriteTWBoundary(24.99, false, offset); offset += 4;
            b &= WriteTWBoundary(28.0, false, offset); offset += 4;
            b &= WriteTWBoundary(29.7, false, offset); offset += 4;
            b &= WriteTWBoundary(50.0, false, offset); offset += 4;
            b &= WriteTWBoundary(54.0, false, offset); offset += 4;

            offset = 0x1BB0;
            b &= WriteTWBoundary(1.8, true, offset); offset += 4;
            b &= WriteTWBoundary(2.0, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(4.0, true, offset); offset += 4;
            b &= WriteTWBoundary(5.258, true, offset); offset += 4;
            b &= WriteTWBoundary(5.262, true, offset); offset += 4;
            b &= WriteTWBoundary(5.278, true, offset); offset += 4;
            b &= WriteTWBoundary(5.282, true, offset); offset += 4;
            b &= WriteTWBoundary(5.288, true, offset); offset += 4;
            b &= WriteTWBoundary(5.292, true, offset); offset += 4;
            b &= WriteTWBoundary(5.366, true, offset); offset += 4;
            b &= WriteTWBoundary(5.375, true, offset); offset += 4;
            b &= WriteTWBoundary(5.398, true, offset); offset += 4;
            b &= WriteTWBoundary(5.407, true, offset); offset += 4;
            b &= WriteTWBoundary(7.0, true, offset); offset += 4;
            b &= WriteTWBoundary(7.3, true, offset); offset += 4;
            b &= WriteTWBoundary(10.1, true, offset); offset += 4;
            b &= WriteTWBoundary(10.15, true, offset); offset += 4;
            b &= WriteTWBoundary(14.0, true, offset); offset += 4;
            b &= WriteTWBoundary(14.35, true, offset); offset += 4;
            b &= WriteTWBoundary(18.068, true, offset); offset += 4;
            b &= WriteTWBoundary(18.168, true, offset); offset += 4;
            b &= WriteTWBoundary(21.0, true, offset); offset += 4;
            b &= WriteTWBoundary(21.45, true, offset); offset += 4;
            b &= WriteTWBoundary(24.89, true, offset); offset += 4;
            b &= WriteTWBoundary(24.99, true, offset); offset += 4;
            b &= WriteTWBoundary(28.0, true, offset); offset += 4;
            b &= WriteTWBoundary(29.7, true, offset); offset += 4;
            b &= WriteTWBoundary(50.0, true, offset); offset += 4;
            b &= WriteTWBoundary(54.0, true, offset); offset += 4;

            offset = 0x1C30; // region 4: Italy
            b &= WriteTWBoundary(1.81, false, offset); offset += 4;
            b &= WriteTWBoundary(2.0, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4; 
            b &= WriteTWBoundary(3.5, false, offset); offset += 4;
            b &= WriteTWBoundary(3.8, false, offset); offset += 4;
            b &= WriteTWBoundary(6.975, false, offset); offset += 4;
            b &= WriteTWBoundary(7.2, false, offset); offset += 4;
            b &= WriteTWBoundary(10.1, false, offset); offset += 4;
            b &= WriteTWBoundary(10.15, false, offset); offset += 4;
            b &= WriteTWBoundary(14.0, false, offset); offset += 4;
            b &= WriteTWBoundary(14.35, false, offset); offset += 4;
            b &= WriteTWBoundary(18.068, false, offset); offset += 4;
            b &= WriteTWBoundary(18.168, false, offset); offset += 4;
            b &= WriteTWBoundary(21.0, false, offset); offset += 4;
            b &= WriteTWBoundary(21.45, false, offset); offset += 4;
            b &= WriteTWBoundary(24.89, false, offset); offset += 4;
            b &= WriteTWBoundary(24.99, false, offset); offset += 4;
            b &= WriteTWBoundary(28.0, false, offset); offset += 4;
            b &= WriteTWBoundary(29.7, false, offset); offset += 4;
            b &= WriteTWBoundary(50.08, false, offset); offset += 4;
            b &= WriteTWBoundary(51.0, false, offset); offset += 4;

            offset = 0x1CB0;
            b &= WriteTWBoundary(1.81, true, offset); offset += 4;
            b &= WriteTWBoundary(2.0, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(3.5, true, offset); offset += 4;
            b &= WriteTWBoundary(3.8, true, offset); offset += 4;
            b &= WriteTWBoundary(6.975, true, offset); offset += 4;
            b &= WriteTWBoundary(7.2, true, offset); offset += 4;
            b &= WriteTWBoundary(10.1, true, offset); offset += 4;
            b &= WriteTWBoundary(10.15, true, offset); offset += 4;
            b &= WriteTWBoundary(14.0, true, offset); offset += 4;
            b &= WriteTWBoundary(14.35, true, offset); offset += 4;
            b &= WriteTWBoundary(18.068, true, offset); offset += 4;
            b &= WriteTWBoundary(18.168, true, offset); offset += 4;
            b &= WriteTWBoundary(21.0, true, offset); offset += 4;
            b &= WriteTWBoundary(21.45, true, offset); offset += 4;
            b &= WriteTWBoundary(24.89, true, offset); offset += 4;
            b &= WriteTWBoundary(24.99, true, offset); offset += 4;
            b &= WriteTWBoundary(28.0, true, offset); offset += 4;
            b &= WriteTWBoundary(29.7, true, offset); offset += 4;
            b &= WriteTWBoundary(50.08, true, offset); offset += 4;
            b &= WriteTWBoundary(51.0, true, offset); offset += 4;

            if (!b) MessageBox.Show("Error Writing Tuning Word Boundaries");

            Flex1500.Exit();
            Flex1500.WriteOp((uint)Flex1500.Opcode.USB_OP_REBOOT, 0, 0);
            //Thread.Sleep(2000);

            if (Flex1500.Init() == 0)
            {
                MessageBox.Show("No hardware found");
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                return;
            }
            
            btnEELoad.BackColor = SystemColors.Control;
            foreach (Control c in this.Controls)
                c.Enabled = true;
        }*/

        private static uint SwapBytes(uint x)
        {
            return (x & 0xff) << 24
                | (x & 0xff00) << 8
                | (x & 0xff0000) >> 8
                | (x & 0xff000000) >> 24;
        }

        /*private bool WriteTWBoundary(double freq, bool xref, uint ee_offset)
        {
            double clock = 384.0;
            if(xref) clock = 400.0;

            uint tw = (uint)(Math.Pow(2.0, 32) * freq * 2 / clock);
            tw = SwapBytes(tw);

            uint check_tw = 0;
            int count = 0;
            do
            {
                count++;
                Flex1500.WriteTRXEEPROMUint(ee_offset, tw);
                Thread.Sleep(10);
                Flex1500.ReadTRXEEPROMUint(ee_offset, out check_tw);
            } while(check_tw != tw && count < 10);

            if(count == 10) return false;
            else return true;
        }*/

        private void btnI2CWrite1_Click(object sender, EventArgs e)
        {
            byte addr = byte.Parse(txtI2CAddr.Text, NumberStyles.HexNumber);
            byte b1 = byte.Parse(txtI2CByte1.Text, NumberStyles.HexNumber);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val = USBHID.WriteI2CValue(addr, b1);

            t1.Stop();
            Debug.WriteLine("I2C1: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnI2CWrite2_Click(object sender, EventArgs e)
        {
            byte addr = byte.Parse(txtI2CAddr.Text, NumberStyles.HexNumber);
            byte b1 = byte.Parse(txtI2CByte1.Text, NumberStyles.HexNumber);
            byte b2 = byte.Parse(txtI2CByte2.Text, NumberStyles.HexNumber);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val = USBHID.WriteI2C2Value(addr, b1, b2);

            t1.Stop();
            Debug.WriteLine("I2C2: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnI2CRead_Click(object sender, EventArgs e)
        {
            byte addr = byte.Parse(txtI2CAddr.Text, NumberStyles.HexNumber);
            byte b1 = byte.Parse(txtI2CByte1.Text, NumberStyles.HexNumber);
            byte b2;

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val = USBHID.ReadI2CValue(addr, b1, out b2);

            t1.Stop();
            Debug.WriteLine("I2C Read: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
            txtI2CResult.Text = b2.ToString("X").PadLeft(2, '0');
        }

        private void btnGPIOWrite1_Click(object sender, EventArgs e)
        {
            byte b = byte.Parse(txtGPIOByte.Text, NumberStyles.HexNumber);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val = USBHID.WriteGPIO(b);

            t1.Stop();
            Debug.WriteLine("GPIO1: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        private void btnGPIOWrite3_Click(object sender, EventArgs e)
        {
            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            byte b;
            int val = USBHID.ReadGPIO(out b);

            t1.Stop();
            Debug.WriteLine("GPIO3: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
            txtGPIOResult.Text = b.ToString("X");
        }

        //==========================================================================
        // TURF selector
        private void buttonTS3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Warning: You are changing your Turf Region.\n",
                                        "Do you have authorization?",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Question);
            int val = 0;
            byte[] data = new byte[1]; // 1 byte long

            if (dr == DialogResult.Yes)
            {
                data[0] = (byte)numericUpDown1.Value;
                Debug.WriteLine("Byte value " + (byte)numericUpDown1.Value);

                val = Flex1500.WriteEEPROM(0x1819, data);

            }

            MessageBox.Show("You must close PowerSDR and cycle power to the radio. Then go to setup->General->Options->BandText Udpate", "Cycle Power",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        } // buttonTS3_Click (TURF)

        //===============================================================================
        // Extended selector
        private void buttonTS1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Warning: You must have Authorization as a MARS/CAP/SHARES Licensed Operator or permission to operate 630m band.\n",
                                          "Do you have authorization?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
            int val = 0;
            byte[] data = new byte[1];

            data[0] = 0x78;   // 0xff is normal value  0x78 is extended

            if (dr == DialogResult.Yes)
            {
                val = Flex1500.WriteEEPROM(0x1818, data);  // x1818, 
            }


            MessageBox.Show("You must cycle power to the radio", "Cycle Power",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        } // buttonTS1_Click

        private void buttonTS2_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("This will reset any MARS/CAP/SHARES operate back to Normal Standard Operation\n",

                         "Yes?",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);

            int val = 0;
            byte[] data = new byte[1];

            data[0] = 0xff;

            if (dr == DialogResult.Yes)
            {

                val = Flex1500.WriteEEPROM(0x1818, data); // return to normal

            }

        } // buttonTS2_Click

        private void btnEEPROMRead1_Click(object sender, EventArgs e)
        {
            // if (txtEEPROMOffset.Text == "") return;
            //   txtEEPROMRead.BackColor = Color.Red;
            Application.DoEvents();
            //  uint offset = uint.Parse(txtEEPROMOffset.Text, System.Globalization.NumberStyles.HexNumber);



            Debug.WriteLine("TRYING TO OPEN EEPROM TXT FILE TO WRITE TO ");


            string file_name2 = console.AppDataPath + "USBEEEPROMDATA.txt"; // save data for my mods

            FileStream stream2 = new FileStream(file_name2, FileMode.Create); // open   file
            BinaryWriter writer2 = new BinaryWriter(stream2);

            Debug.WriteLine("OPENED EEPROM TXT FILE TO WRITE TO ");

            ushort offset; // 16bit int

            byte[] data;
            string datastring;
            string final;
            string offsetstring;
            int val;

            for (offset = 0x0; offset < 0x4000; offset++) // valid data here  
            {

                Debug.Write("   Reading offset-> " + offset);

                val = Flex1500.ReadEEPROM(offset, 1, out data);

                //  MessageBox.Show("Error in ReadTRXEEPROM.");

                // if (FWC.ReadTRXEEPROMByte(offset, out data) == 0)




                Debug.WriteLine("   Data-> " + data[0]);

                //   txtEEPROMRead.BackColor = SystemColors.Control;

                datastring = String.Format("{0:X4}", data[0]);

                //   txtEEPROMRead.Text = datastring;

                offsetstring = String.Format("{0:X4}", offset);

                //   txtEEPROMOffset.Text = offsetstring;

                final = offsetstring + " , " + datastring + "\n";

                writer2.Write(final);


            } // for offset loop


            writer2.Close();    // close  file
            stream2.Close();   // close stream


        } // btneepromread1_click

        private void btnTune_Click(object sender, EventArgs e)
        {
            if (txtTune.Text == "") return;

            double freq;
            bool b = double.TryParse(txtTune.Text, out freq);
            if (!b) return;

            uint ftw = (uint)(Math.Pow(2.0, 32) * freq * 2 / 384.0);

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            int val = USBHID.SetFreqTW(ftw);

            t1.Stop();
            Debug.WriteLine("Tune: " + val + "  (" + t1.DurationMsec.ToString("f2") + ")");
        }

        /*
        private void button1_Click(object sender, EventArgs e)
        {
            Flex1500.StartAudioListener();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Flex1500.loopbackAudio();
        }

        private void button3_Click(object sender, EventArgs e)
        {*
            Flex1500.StopAudioListener();
        }
         */
    }
}
