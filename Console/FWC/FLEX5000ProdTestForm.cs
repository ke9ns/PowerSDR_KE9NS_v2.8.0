//=================================================================
// FLEX5000ProdTestForm.cs
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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    unsafe public partial class FLEX5000ProdTestForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Progress p;
        private Console console;
       
        private string common_data_path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\FlexRadio Systems\\PowerSDR\\";

        #endregion

        #region Constructor and Destructor

        public FLEX5000ProdTestForm(Console c)
        {
            InitializeComponent();
            console = c;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    this.Text += "  (TRX: " + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ")";
                    break;
                case Model.FLEX1500:
                    this.Text += "  (TRX: " + HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial) + ")";
                    break;
            }
            Common.RestoreForm(this, "FLEX5000ProdTestForm", false);

            if (console.fwc_init &&
                (console.CurrentModel == Model.FLEX3000 || console.CurrentModel == Model.FLEX5000) &&
                FWCEEPROM.TRXSerial == 0)
            {
                MessageBox.Show("No TRX Serial Found.  Please enter and try again.",
                    "No TRX S/N Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Enabled = false;
            }
            else if (console.hid_init &&
                (console.CurrentModel == Model.FLEX1500) &&
                HIDEEPROM.TRXSerial == 0)
            {
                MessageBox.Show("No TRX Serial Found.  Please enter and try again.",
                    "No TRX S/N Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Enabled = false;
            }

            if (console.fwc_init &&
                (console.CurrentModel == Model.FLEX3000 || console.CurrentModel == Model.FLEX5000) &&
                console.SampleRate1 < 96000)
                MessageBox.Show("Warning: Sample Rate should be at least 96kHz before calibrating.",
                    "Warning: Sample Rate Low",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            else if (console.hid_init && console.CurrentModel == Model.FLEX1500)
                console.SampleRate1 = 48000;

            /*if (console.SetupForm.DSPPhoneRXBuffer != 4096)
                /*MessageBox.Show("Warning: DSP RX Buffer size should be at least 4096 before calibrating.",
                    "Warning: DSP RX Buffer Size Low",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);*/
            //console.SetupForm.DSPPhoneRXBuffer = 4096;*/

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    udLevel.Value = -24.0m;
                    ckTestTXPA.Visible = false;
                    ckTestTXPA.Checked = false;
                    break;
                case Model.FLEX3000:
                    btnPLL.Visible = false;
                    ckTestGenPLL.Visible = false;
                    ckTestGenPLL.Checked = false;
                    btnGenATTN.Visible = true;
                    ckTestGenATTN.Visible = true;
                    ckTestGenATTN.Checked = true;
                    btnImpulse.Visible = false;
                    ckTestGenImpulse.Visible = false;
                    ckTestGenImpulse.Checked = false;
                    btnTXFilter.Visible = false;
                    ckTestTXFilter.Visible = false;
                    ckTestTXFilter.Checked = false;
                    ckTestTXPA.Visible = false;
                    ckTestTXPA.Checked = false;
                    this.Text = this.Text.Replace("FLEX-5000", "FLEX-3000");
                    btnIOExtRef.Visible = false;
                    btnIORCAInOut.Visible = false;
                    btnIOMicUp.Visible = true;
                    btnIOMicDown.Visible = true;
                    btnIOMicFast.Visible = true;
                    btnPostFence.Visible = false;
                    udLevel.Value = -37.4m;
                    break;
                case Model.FLEX1500:
                    btnRunAll1500.Visible = true;
                    btnPLL.Visible = false;
                    ckTestGenPLL.Visible = false;
                    ckTestGenPLL.Checked = false;
                    btnGenATTN.Visible = true;
                    ckTestGenATTN.Visible = true;
                    ckTestGenATTN.Checked = true;
                    btnGenPreamp.Visible = false;
                    ckTestGenPreamp.Visible = false;
                    ckTestGenPreamp.Checked = false;
                    btnImpulse.Visible = false;
                    ckTestGenImpulse.Visible = false;
                    ckTestGenImpulse.Checked = false;
                    btnTXFilter.Visible = false;
                    ckTestTXFilter.Visible = false;
                    ckTestTXFilter.Checked = false;
                    btnTXCarrier.Visible = false;
                    ckTestTXCarrier.Visible = false;
                    ckTestTXCarrier.Checked = false;
                    ckTestTXPA.Visible = true;
                    this.Text = this.Text.Replace("FLEX-5000", "FLEX-1500");
                    btnIORCAInOut.Visible = false;
                    btnIOHeadphone.Visible = false;
                    udLevel.Value = -35.0m;
                    grpComPort.Visible = true;
                    btnTX1500PA.Visible = true;

                    //btnRXImage.Enabled = false;

                    //btnTXImage.Enabled = false;
                    //ckTestTXImage.Visible = false;
                    //ckTestTXImage.Checked = false;
                    //btnIOExtRef.Enabled = false;
                    //btnIOFWInOut.Enabled = false;
                    btnIOPwrSpkr.Enabled = false;
                    btnIORCAPTT.Enabled = false;
                    btnIOFWPTT.Visible = true;
                    btnIOFWPTT.Enabled = true;

                    if (console.hid_init)
                    {
                        if (!Flex1500.ProdTestPresent())
                        {
                            MessageBox.Show("No production generator test unit found.\n" +
                                "Disabling tests which require gen unit.");
                            btnRXImage.Enabled = false;
                            ckTestRXImage.Visible = false;
                            ckTestRXImage.Checked = false;

                            btnTXImage.Enabled = false;
                            ckTestTXImage.Visible = false;
                            ckTestTXImage.Checked = false;

                            btnIOExtRef.Enabled = false;
                        }
                    }
                    break;
            }

            // populate COM port selection with only ports that are available
            comboCOMPort.Items.Clear();
            comboCOMPort.Items.AddRange(Common.SortedComPorts());
            if (comboCOMPort.Items.Count > 0)
                comboCOMPort.SelectedIndex = 0;

            // set powermaster COM port based on data in powermaster.txt file if it exists
            string pm_file_path = Path.Combine(common_data_path, "powermaster.txt");
            if (File.Exists(pm_file_path))
            {
                StreamReader reader = new StreamReader(pm_file_path);
                string[] temp = reader.ReadToEnd().Split('\n');
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].StartsWith("COM"))
                    {
                        comboCOMPort.Text = temp[i];
                        break;
                    }
                }
                reader.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        
        #region Misc Routines

        private string BandToString(Band b)
        {
            string ret_val = "";
            switch (b)
            {
                case Band.B160M: ret_val = " 160m "; break;
                case Band.B80M: ret_val = " 80m "; break;
                case Band.B60M: ret_val = " 60m "; break;
                case Band.B40M: ret_val = " 40m "; break;
                case Band.B30M: ret_val = " 30m "; break;
                case Band.B20M: ret_val = " 20m "; break;
                case Band.B17M: ret_val = " 17m "; break;
                case Band.B15M: ret_val = " 15m "; break;
                case Band.B12M: ret_val = " 12m "; break;
                case Band.B10M: ret_val = " 10m "; break;
                case Band.B6M: ret_val = " 6m "; break;
            }
            return ret_val;
        }

        private void SetBandFromString(string s)
        {
            ck160.Checked = (s.IndexOf(" 160m ") >= 0);
            ck80.Checked = (s.IndexOf(" 80m ") >= 0);
            ck60.Checked = (s.IndexOf(" 60m ") >= 0);
            ck40.Checked = (s.IndexOf(" 40m ") >= 0);
            ck30.Checked = (s.IndexOf(" 30m ") >= 0);
            ck20.Checked = (s.IndexOf(" 20m ") >= 0);
            ck17.Checked = (s.IndexOf(" 17m ") >= 0);
            ck15.Checked = (s.IndexOf(" 15m ") >= 0);
            ck12.Checked = (s.IndexOf(" 12m ") >= 0);
            ck10.Checked = (s.IndexOf(" 10m ") >= 0);
            ck6.Checked = (s.IndexOf(" 6m ") >= 0);
        }

        private void SetBandFrom1500FilterString(string s)
        {
            ck160.Checked = (s.IndexOf("11") >= 0);
            ck80.Checked = (s.IndexOf("10") >= 0);
            ck60.Checked = (s.IndexOf("09") >= 0);
            ck40.Checked = (s.IndexOf("08") >= 0);
            ck30.Checked = (s.IndexOf("07") >= 0);
            ck20.Checked = (s.IndexOf("06") >= 0);
            ck17.Checked = (s.IndexOf("05") >= 0);
            ck15.Checked = (s.IndexOf("04") >= 0);
            ck12.Checked = (s.IndexOf("03") >= 0);
            ck10.Checked = (s.IndexOf("02") >= 0);
            ck6.Checked = (s.IndexOf("01") >= 0);
        }

        private string GetStringFromBands()
        {
            string s = " ";
            if (ck160.Checked) s += "160m ";
            if (ck80.Checked) s += "80m ";
            if (ck60.Checked) s += "60m ";
            if (ck40.Checked) s += "40m ";
            if (ck30.Checked) s += "30m ";
            if (ck20.Checked) s += "20m ";
            if (ck17.Checked) s += "17m ";
            if (ck15.Checked) s += "15m ";
            if (ck12.Checked) s += "12m ";
            if (ck10.Checked) s += "10m ";
            if (ck6.Checked) s += "6m ";
            return s;
        }

        #endregion

        #region General Tests

        #region PLL

        private string test_pll = "PLL Test: Not Run";
        private void btnPLL_Click(object sender, System.EventArgs e)
        {
            console.VFOSplit = false;
            bool b;
            FWC.WriteClockReg(0x08, 0x47);
            //Thread.Sleep(50);
            FWC.GetPLLStatus2(out b);
            //Thread.Sleep(50);
            if (b) btnPLL.BackColor = Color.Green;
            else btnPLL.BackColor = Color.Red;
            if (b) test_pll = "PLL Test: Passed";
            else test_pll = " PLL Test: Failed (not locked)";
            toolTip1.SetToolTip(btnPLL, test_pll);
            lstDebug.Items.Insert(0, test_pll);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            /*path += "\\PLL";
            if(!Directory.Exists(path))	Directory.CreateDirectory(path);*/
            path += "\\pll.csv";
            bool file_exists = File.Exists(path);
            StreamWriter writer = new StreamWriter(path, true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, Locked");
            writer.WriteLine(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + b.ToString());
            writer.Close();
        }

        #endregion

        #region Gen/Bal

        private void btnGenBal_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnGenBal.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(TestGenBal));
            t.Name = "Test Gen/Bal Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }


        // ke9ns mod: to allow continuous test run checkgenbal
        private string test_genbal = "Gen/Bal Test: Not Run";
        private void TestGenBal()
        {

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync) console.VFOSync = false;

            console.VFOSplit = false;
            double vfob_freq = console.VFOBFreq;

            bool full_duplex = console.FullDuplex;
            console.FullDuplex = false;

            double vfoa_freq = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            DSPMode dsp = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            PreampMode preamp = console.RX1PreampMode;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000: console.RX1PreampMode = PreampMode.OFF; break;
                case Model.FLEX3000: console.RX1PreampMode = PreampMode.LOW; break;
                case Model.FLEX1500: console.RX1PreampMode = (PreampMode)FLEX1500PreampMode.ZERO; break;
            }

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.RX1Filter = Filter.VAR1;

            HIDAnt hid_ant = console.RXAnt1500;

            // ke9ns add do loop .157 checkGENBAL 

            float adc_l = 0.0f;
            float adc_r = 0.0f;
            float off_l = 0.0f;
            bool b = true;


            adc_l = 0.0f;
            adc_r = 0.0f;
            off_l = 0.0f;
            b = true;

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    console.RX1FilterLow = -1000;
                    console.RX1FilterHigh = 1000;

                    FWC.SetTest(true);
                    FWC.SetSig(true);
                    FWC.SetGen(true);
                    FWC.SetFullDuplex(true);
                    FWC.SetTXMon(false);
                    FWC.SetRX1Filter(-1.0f);
                    Thread.Sleep(1000); // ke9ns 1000
                    break;
                case Model.FLEX1500:
                    console.RX1FilterLow = -(int)(console.IFFreq * 1e6) + console.CWPitch - 100;
                    console.RX1FilterHigh = -(int)(console.IFFreq * 1e6) + console.CWPitch + 100;

                    console.RXAnt1500 = HIDAnt.BITE;
                    Thread.Sleep(100);

                    USBHID.SetGen(true);
                    Thread.Sleep(100);

                    USBHID.SetQSE(true);
                    Thread.Sleep(100);

                    USBHID.SetTest(true);
                    Thread.Sleep(100);

                    USBHID.SetPreamp(FLEX1500PreampMode.MINUS_10);
                    Thread.Sleep(100);

                    DttSP.SetCorrectIQEnable(0); // turn off I/Q correction
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 0);
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 1);

                    Audio.SourceScale = 0.178;
                    Audio.RX1OutputSignal = Audio.SignalSource.SINE;

                    USBHID.WriteI2C2Value(0x30, 0x0C, 0x50); // turn on hardware HPF
                    Thread.Sleep(1000);
                    break;
            }


            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                adc_l += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                if (j != 4) Thread.Sleep(50); // 50
            }
            adc_l /= 5;


            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_IMAG);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                adc_r += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_IMAG);
                if (j != 4) Thread.Sleep(50);// 50
            }
            adc_r /= 5;

            do
            {
                Thread.Sleep(50); // wait until you stop it.
            }
            while (checkGENBAL.Checked == true);


            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.SetTest(false);
                    //Thread.Sleep(50);
                    FWC.SetSig(false);
                    //Thread.Sleep(50);
                    FWC.SetGen(false);
                    Thread.Sleep(500); // ke9ns 500
                    break;
                case Model.FLEX1500:
                    Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
                    USBHID.SetTest(false); Thread.Sleep(20);
                    USBHID.SetGen(false); Thread.Sleep(10);
                    USBHID.SetQSE(false); Thread.Sleep(10);
                    console.RXAnt1500 = hid_ant; Thread.Sleep(10);
                    Thread.Sleep(1000);
                    break;
            }

            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
            Thread.Sleep(50);

            for (int j = 0; j < 5; j++)
            {
                off_l += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                if (j != 4) Thread.Sleep(50);
            }
            off_l /= 5;

            float target = 0.0f;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000: target = 55.0f; break;
                case Model.FLEX3000: target = 47.0f; break;
                case Model.FLEX1500: target = 40.0f; break;
            }

            if (console.CurrentModel == Model.FLEX1500) USBHID.WriteI2C2Value(0x30, 0x0C, 0x00); // turn off hardware HPF

            test_genbal = "Gen/Bal Test: ";

            if (adc_l - off_l < target)
            {
                lstDebug.Items.Insert(0, " Gen/Bal Test: Failed ADC_L S/N <" + target.ToString("f0") + "dB (" + (adc_l - off_l).ToString("f1") + ")");
                test_genbal += "ADC_L S/N <" + target.ToString("f0") + "dB (" + (adc_l - off_l).ToString("f1") + ")\n";
                b = false;
            }
            if (adc_r - off_l < target)
            {
                lstDebug.Items.Insert(0, " Gen/Bal Test: Failed ADC_R S/N <" + target.ToString("f0") + "dB (" + (adc_r - off_l).ToString("f1") + ")");
                test_genbal += "ADC_R S/N <" + target.ToString("f0") + "dB (" + (adc_r - off_l).ToString("f1") + ")\n";
                b = false;
            }
            if (Math.Abs(adc_r - adc_l) > 1.0f)
            {
                lstDebug.Items.Insert(0, " Gen/Bal Test: Failed Chan Bal >1dB (" + Math.Abs(adc_r - adc_l).ToString("f1") + ")");
                test_genbal += "Chan Bal >1dB (" + Math.Abs(adc_r - adc_l).ToString("f1") + ")\n";
                b = false;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000: target = -26.6f; break;
                case Model.FLEX3000: target = -27.1f; break;
                case Model.FLEX1500: target = -10.0f; break;
            }
            if (Math.Abs(target - adc_l) > 1.0f)
            {
                lstDebug.Items.Insert(0, " Gen/Bal Test: Failed ADC_L " + target.ToString("f1") + "+/-1.0 (" + adc_l.ToString("f1") + ")");
                test_genbal += "ADC_L " + target.ToString("f1") + "+/-1.0 (" + adc_l.ToString("f1") + ")\n";
                b = false;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000: target = -26.1f; break;
                case Model.FLEX3000: target = -26.8f; break;
                case Model.FLEX1500: target = -10.0f; break;
            }
            if (Math.Abs(target - adc_r) > 1.0f)
            {
                lstDebug.Items.Insert(0, " Gen/Bal Test: Failed ADC_R " + target.ToString("f1") + "+/-1.0 (" + adc_r.ToString("f1") + ")");
                test_genbal += "ADC_R " + target.ToString("f1") + "+/-1.0 (" + adc_r.ToString("f1") + ")\n";
                b = false;
            }

            if (b)
            {
                btnGenBal.BackColor = Color.Green;
                test_genbal = "Gen/Bal Test: Passed (" + adc_l.ToString("f1") + ", " + adc_r.ToString("f1") + ")";
                lstDebug.Items.Insert(0, test_genbal);

                //   if (checkGENBAL.Checked == false) break;
            }
            else
            {
                btnGenBal.BackColor = Color.Red;
            }
            toolTip1.SetToolTip(btnGenBal, test_genbal);


            test_genbal = "                              \r\n";
            lstDebug.Items.Insert(0, test_genbal);



            DttSP.SetCorrectIQEnable(1); // turn on I/Q correction

            console.VFOAFreq = vfoa_freq;
            console.FullDuplex = full_duplex;
            console.VFOBFreq = vfob_freq;
            console.RX1DSPMode = dsp;
            console.RX1PreampMode = preamp;
            console.RX1Filter = filter;
            if (filter == Filter.VAR1 || filter == Filter.VAR2)
            {
                console.RX1FilterLow = var_low;
                console.RX1FilterHigh = var_high;
            }

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            /*path += "\\DDS";
            if(!Directory.Exists(path))	Directory.CreateDirectory(path);*/

            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
            }

            bool file_exists = File.Exists(path + "\\genbal.csv");
            StreamWriter writer = new StreamWriter(path + "\\genbal.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, ADC_L, ADC_R, Off_L, Passed");
            writer.WriteLine(console.CurrentModel.ToString() + ","
                + serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + adc_l.ToString("f1") + "," + adc_r.ToString("f1") + "," + off_l.ToString("f1") + ","
                + b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;


        } //TestGenBal()

        #endregion

        #region Noise

        private void btnNoise_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnNoise.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(TestNoise));
            t.Name = "Test Noise Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_noise = "Noise Test: Not Run";
        private void TestNoise()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on to run this test.",
                    "Power not on",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfob_freq = console.VFOBFreq;

            bool full_duplex = console.FullDuplex;
            console.FullDuplex = false;

            double vfoa_freq = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            DSPMode dsp = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            PreampMode preamp = console.RX1PreampMode;
            console.RX1PreampMode = PreampMode.HIGH;

            MeterTXMode tx_meter = console.CurrentMeterTXMode;
            console.CurrentMeterTXMode = MeterTXMode.OFF;

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.RX1Filter = Filter.VAR1;
            console.RX1FilterLow = -1000;
            console.RX1FilterHigh = 1000;

            Thread.Sleep(500);

            float[] a = new float[Display.BUFFER_SIZE];
            float sum = 0.0f;

            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);

            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                Thread.Sleep(50);
            }

            float avg = sum / 5;

            Debug.WriteLine("Noise Test: " + avg.ToString("f1") + " dB");
            bool b = (avg > -75.0);
            if (b)
            {
                btnNoise.BackColor = Color.Red;
                test_noise = " Noise Test: Failed dbFS > -75dB (" + avg.ToString("f1") + ")";
            }
            else
            {
                btnNoise.BackColor = Color.Green;
                test_noise = "Noise Test: Passed (" + avg.ToString("f1") + "dBFS)";
            }
            toolTip1.SetToolTip(btnNoise, test_noise);
            lstDebug.Items.Insert(0, test_noise);

            console.CurrentMeterTXMode = tx_meter;
            console.VFOAFreq = vfoa_freq;
            console.FullDuplex = full_duplex;
            console.VFOBFreq = vfob_freq;
            console.RX1DSPMode = dsp;
            console.RX1PreampMode = preamp;
            console.RX1Filter = filter;
            if (filter == Filter.VAR1 || filter == Filter.VAR2)
            {
                console.RX1FilterLow = var_low;
                console.RX1FilterHigh = var_high;
            }

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            /*path += "\\Noise";
            if(!Directory.Exists(path))	Directory.CreateDirectory(path);*/
            path += "\\noise.csv";
            bool file_exists = File.Exists(path);
            StreamWriter writer = new StreamWriter(path, true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, dBFS, Passed");
            writer.WriteLine(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + avg.ToString("f1") + ","
                + b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Impulse

        private void btnImpulse_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnImpulse.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(TestImpulse));
            t.Name = "Test Impulse Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_impulse = "Impulse Test: Not Run";
        private void TestImpulse()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on to run this test.",
                    "Power not on",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfob_freq = console.VFOBFreq;

            bool full_duplex = console.FullDuplex;
            console.FullDuplex = false;

            double vfoa_freq = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            MeterTXMode tx_meter = console.CurrentMeterTXMode;
            console.CurrentMeterTXMode = MeterTXMode.OFF;

            DSPMode dsp = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            PreampMode preamp = console.RX1PreampMode;
            console.RX1PreampMode = PreampMode.HIGH;

            bool nb = console.NB;
            console.NB = false;

            bool nb2 = console.NB2;
            console.NB2 = false;

            bool polyphase = console.setupForm.Polyphase;				// save current polyphase setting
            console.setupForm.Polyphase = false;						// disable polyphase

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.RX1Filter = Filter.VAR1;
            console.RX1FilterLow = -1000;
            console.RX1FilterHigh = 1000;

            FWC.SetTest(true);
            Thread.Sleep(500);

            float[] a = new float[Display.BUFFER_SIZE];
            float sum = 0.0f;

            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);

            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                Thread.Sleep(50);
            }

            float noise = sum / 5;

            Thread t = new Thread(new ThreadStart(Impulse));
            t.Name = "Impulse Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            Thread.Sleep(200);

            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);

            sum = 0.0f;
            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                Thread.Sleep(50);
            }
            t.Abort();
            FWC.SetTest(false);
            Thread.Sleep(50);

            sum /= 5;

            Debug.WriteLine("Impulse Test: " + (sum - noise).ToString("f1") + " dB");
            bool b = (sum - noise < 6.0);
            if (b)
            {
                btnImpulse.BackColor = Color.Red;
                test_impulse = " Impulse Test: Failed impulse not 6dB+ (" + (sum - noise).ToString("f1") + ")";
            }
            else
            {
                btnImpulse.BackColor = Color.Green;
                test_impulse = "Impulse Test: Passed";
            }
            toolTip1.SetToolTip(btnImpulse, test_impulse);
            lstDebug.Items.Insert(0, test_impulse);

            console.CurrentMeterTXMode = tx_meter;
            console.VFOBFreq = vfob_freq;
            console.FullDuplex = full_duplex;
            console.RX1DSPMode = dsp;
            console.RX1PreampMode = preamp;
            console.RX1FilterLow = var_low;
            console.RX1FilterHigh = var_high;
            console.RX1Filter = filter;
            console.NB = nb;
            console.NB2 = nb2;
            console.setupForm.Polyphase = polyphase;
            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            /*path += "\\Impulse";
            if(!Directory.Exists(path))	Directory.CreateDirectory(path);*/
            path += "\\impulse.csv";
            bool file_exists = File.Exists(path);
            StreamWriter writer = new StreamWriter(path, true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, Impulse, Noise, Diff, Passed");
            writer.WriteLine(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + sum.ToString("f1") + "," + noise.ToString("f1") + "," + (sum - noise).ToString("f1") + ","
                + b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        private void Impulse()
        {
            try
            {
                while (true)
                {
                    FWC.SetImpulse(true);
                    FWC.SetImpulse(false);
                }
            }
            catch (Exception) { }
        }

        #endregion

        #region Preamp

        private void btnGenPreamp_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnGenPreamp.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(TestPreamp));
            t.Name = "Test Preamp Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_preamp = "Preamp Test: Not Run";
        private void TestPreamp()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on to run this test.",
                    "Power not on",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfob_freq = console.VFOBFreq;

            bool full_duplex = console.FullDuplex;
            console.FullDuplex = false;

            double vfoa_freq = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            DSPMode dsp = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            PreampMode preamp = console.RX1PreampMode;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000: console.RX1PreampMode = PreampMode.OFF; break;
                case Model.FLEX3000: console.RX1PreampMode = PreampMode.LOW; break;
            }

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.RX1Filter = Filter.VAR1;
            console.RX1FilterLow = -1000;
            console.RX1FilterHigh = 1000;

            FWC.SetTest(true);
            //Thread.Sleep(50);
            FWC.SetSig(true);
            //Thread.Sleep(50);
            FWC.SetGen(true);
            //Thread.Sleep(50);
            FWC.SetFullDuplex(true);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            Thread.Sleep(1000);

            float[] a = new float[Display.BUFFER_SIZE];
            console.calibration_mutex.WaitOne();
            fixed (float* ptr = &a[0])
                DttSP.GetSpectrum(0, ptr);// get the spectrum values
            console.calibration_mutex.ReleaseMutex();

            float peak = float.MinValue;
            int peak_bin = -1;
            for (int i = 0; i < Display.BUFFER_SIZE; i++)
            {
                if (a[i] > peak)
                {
                    peak = a[i];
                    peak_bin = i;
                }
            }

            console.RX1PreampMode = PreampMode.HIGH;
            Thread.Sleep(500);

            console.calibration_mutex.WaitOne();
            fixed (float* ptr = &a[0])
                DttSP.GetSpectrum(0, ptr);// get the spectrum values
            console.calibration_mutex.ReleaseMutex();

            double target = 0.0;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    target = 14.0;
                    break;
                case Model.FLEX3000:
                    if (FWCEEPROM.TRXRev >> 8 < 6) // before rev G
                        target = 24.5;
                    else target = 14.4;
                    break;
            }

            Debug.WriteLine("Preamp Test: " + (a[peak_bin] - peak).ToString("f1") + " dB");
            bool b = (Math.Abs(target - (a[peak_bin] - peak)) <= 3.0); // > +/- 3.0dB
            if (!b)
            {
                btnGenPreamp.BackColor = Color.Red;
                test_preamp = " Preamp Test: Failed > " + target.ToString("f1") + " +/- 3dB (" + (a[peak_bin] - peak).ToString("f1") + ")";
            }
            else
            {
                btnGenPreamp.BackColor = Color.Green;
                test_preamp = "Preamp Test: Passed (" + (a[peak_bin] - peak).ToString("f1") + ")";
            }
            toolTip1.SetToolTip(btnGenPreamp, test_preamp);
            lstDebug.Items.Insert(0, test_preamp);

            // end
            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetFullDuplex(false);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);

            console.VFOAFreq = vfoa_freq;
            console.FullDuplex = full_duplex;
            console.VFOBFreq = vfob_freq;
            console.RX1DSPMode = dsp;
            console.RX1PreampMode = preamp;
            console.RX1Filter = filter;
            if (filter == Filter.VAR1 || filter == Filter.VAR2)
            {
                console.RX1FilterLow = var_low;
                console.RX1FilterHigh = var_high;
            }

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
            }

            bool file_exists = File.Exists(path + "\\preamp.csv");
            StreamWriter writer = new StreamWriter(path + "\\preamp.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, High, Off, Diff, Passed");
            writer.WriteLine(console.CurrentModel.ToString() + ","
                + serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + peak.ToString("f1") + "," + a[peak_bin].ToString("f1") + "," + (peak - a[peak_bin]).ToString("f1") + ","
                + b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region ATTN

        private void btnGenATTN_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnGenATTN.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(TestATTN));
            t.Name = "Test ATTN Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_attn = "Attenuator Test: Not Run";
        private void TestATTN()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on to run this test.",
                    "Power not on",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfob_freq = console.VFOBFreq;

            bool full_duplex = console.FullDuplex;
            console.FullDuplex = false;

            double vfoa_freq = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            DSPMode dsp = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            bool mute = console.MUT;
            console.MUT = false;

            PreampMode preamp = console.RX1PreampMode;
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    console.RX1PreampMode = PreampMode.LOW;
                    break;
                case Model.FLEX1500:
                    console.RX1PreampMode = (PreampMode)FLEX1500PreampMode.ZERO;
                    break;
            }

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.RX1Filter = Filter.VAR1;

            HIDAnt hid_ant = console.RXAnt1500;
            double scale = Audio.SourceScale;

            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    console.RX1FilterLow = -1000;
                    console.RX1FilterHigh = 1000;

                    FWC.SetTest(true);
                    //Thread.Sleep(50);
                    FWC.SetSig(true);
                    //Thread.Sleep(50);
                    FWC.SetGen(true);
                    //Thread.Sleep(50);
                    FWC.SetFullDuplex(true);
                    //Thread.Sleep(50);
                    FWC.SetTXMon(false);
                    Thread.Sleep(1000);
                    break;
                case Model.FLEX1500:
                    console.RX1FilterLow = -(int)(console.IFFreq * 1e6) + console.CWPitch - 100;
                    console.RX1FilterHigh = -(int)(console.IFFreq * 1e6) + console.CWPitch + 100;

                    console.RXAnt1500 = HIDAnt.BITE;
                    Thread.Sleep(100);

                    USBHID.SetGen(true);
                    Thread.Sleep(100);

                    USBHID.SetQSE(true);
                    Thread.Sleep(100);

                    USBHID.SetTest(true);
                    Thread.Sleep(100);

                    USBHID.SetPreamp(FLEX1500PreampMode.PLUS_20);
                    Thread.Sleep(100);

                    DttSP.SetCorrectIQEnable(0); // turn off I/Q correction
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 0);
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 1);

                    Audio.SourceScale = 0.01;
                    Audio.RX1OutputSignal = Audio.SignalSource.SINE;

                    Thread.Sleep(400);
                    break;
            }

            float[] a = new float[Display.BUFFER_SIZE];
            console.calibration_mutex.WaitOne();
            fixed (float* ptr = &a[0])
                DttSP.GetSpectrum(0, ptr); // get the spectrum values
            console.calibration_mutex.ReleaseMutex();

            console.calibration_mutex.WaitOne();
            fixed (float* ptr = &a[0])
                DttSP.GetSpectrum(0, ptr); // get the spectrum values again
            console.calibration_mutex.ReleaseMutex();

            float peak = float.MinValue;
            int peak_bin = -1;
            for (int i = 0; i < Display.BUFFER_SIZE; i++)
            {
                if (a[i] > peak)
                {
                    peak = a[i];
                    peak_bin = i;
                }
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX1500:
                    console.RX1PreampMode = (PreampMode)FLEX1500PreampMode.ZERO;
                    break;
                default:
                    console.RX1PreampMode = PreampMode.OFF;
                    break;
            }
            Thread.Sleep(500);

            console.calibration_mutex.WaitOne();
            fixed (float* ptr = &a[0])
                DttSP.GetSpectrum(0, ptr);// get the spectrum values
            console.calibration_mutex.ReleaseMutex();

            double target = 0.0;
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    if (FWCEEPROM.TRXRev >> 8 < 6) // before rev G
                        target = -20.0;
                    else target = -9.0;
                    break;
                case Model.FLEX1500:
                    target = -20.0;
                    break;
            }

            Debug.WriteLine("ATTN Test: " + (a[peak_bin] - peak).ToString("f1") + " dB");
            bool b = (Math.Abs(target - (a[peak_bin] - peak)) <= 3.0); // > +/- 3.0dB
            if (!b)
            {
                btnGenATTN.BackColor = Color.Red;
                test_attn = " ATTN Test: Failed " + target.ToString("f1") + " +/- 3dB (" + (a[peak_bin] - peak).ToString("f1") + ")";
            }
            else
            {
                btnGenATTN.BackColor = Color.Green;
                test_attn = "ATTN Test: Passed (" + (a[peak_bin] - peak).ToString("f1") + ")";
            }
            toolTip1.SetToolTip(btnGenATTN, test_attn);
            lstDebug.Items.Insert(0, test_attn);

            // end
            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    FWC.SetTest(false);
                    //Thread.Sleep(50);
                    FWC.SetSig(false);
                    //Thread.Sleep(50);
                    FWC.SetGen(false);
                    //Thread.Sleep(50);
                    FWC.SetFullDuplex(false);
                    //Thread.Sleep(50);
                    FWC.SetTXMon(false);
                    //Thread.Sleep(50);
                    break;
                case Model.FLEX1500:
                    Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
                    USBHID.SetTest(false); Thread.Sleep(10);
                    USBHID.SetGen(false); Thread.Sleep(10);
                    USBHID.SetQSE(false); Thread.Sleep(10);
                    console.RXAnt1500 = hid_ant; Thread.Sleep(10);
                    break;
            }

            DttSP.SetCorrectIQEnable(1); // turn on I/Q correction

            Audio.SourceScale = scale;
            console.MUT = mute;
            console.VFOAFreq = vfoa_freq;
            console.FullDuplex = full_duplex;
            console.VFOBFreq = vfob_freq;
            console.RX1DSPMode = dsp;
            console.RX1PreampMode = preamp;
            console.RX1Filter = filter;
            if (filter == Filter.VAR1 || filter == Filter.VAR2)
            {
                console.RX1FilterLow = var_low;
                console.RX1FilterHigh = var_high;
            }

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\attn.csv");
            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
            }
            StreamWriter writer = new StreamWriter(path + "\\attn.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, Off, On, Diff, Passed");
            writer.WriteLine(console.CurrentModel.ToString() + ","
                + serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + peak.ToString("f1") + "," + a[peak_bin].ToString("f1") + "," + (peak - a[peak_bin]).ToString("f1") + ","
                + b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #endregion

        #region RX Tests

        #region RX Filter

        private void btnRXFilter_Click(object sender, System.EventArgs e)
        {
            p = new Progress("Test RX Filter");
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnRXFilter.BackColor = Color.Green;
            Thread t = null;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    t = new Thread(new ThreadStart(TestRXFilter));
                    break;
                case Model.FLEX1500:
                    t = new Thread(new ThreadStart(Test1500Filter));
                    break;
            }

            t.Name = "Test RX Filter Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            Invoke(new MethodInvoker(p.Show));
        }

        private string test_rx_filter = "RX Filter Test: Not Run";
        private void TestRXFilter()
        {
            float[] avg = { 1.4f, 1.3f, 0.9f, 1.2f, 0.6f, 0.7f, 0.7f, 0.6f, 0.5f, 0.7f, 0.5f };	// avg filter loss in dB
            float[] avg2 = { 5.0f, 4.2f, 3.9f, 3.1f, 2.5f, 1.7f, 2.6f, 1.8f, 2.4f, 2.1f, 1.8f };  // avg filter loss for FLEX-3000

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.VFOSplit = false;
            test_rx_filter = "RX Filter Test: Passed";

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            int tx_low = console.TXFilterLow;
            console.TXFilterLow = 100;

            int tx_high = console.TXFilterHigh;
            console.TXFilterHigh = 200;

            bool spur_red = console.SpurReduction;
            console.SpurReduction = false;

            string display = console.DisplayModeText;
            console.DisplayModeText = "Spectrum";

            // PreampMode preamp = console.RX1PreampMode;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            Filter filter = console.RX1Filter;
            console.RX1Filter = Filter.VAR1;

            int filt_low = console.RX1FilterLow;
            int filt_high = console.RX1FilterHigh;

            console.UpdateRX1Filters(-500, 500);

            bool rit_on = console.RITOn;
            console.RITOn = false;

            bool mute = console.MUT;
            console.MUT = false;

            int dsp_buf_size = console.setupForm.DSPPhoneRXBuffer;		// save current DSP buffer size
            console.setupForm.DSPPhoneRXBuffer = 4096;					// set DSP Buffer Size to 2048

            bool polyphase = console.setupForm.Polyphase;				// save current polyphase setting
            console.setupForm.Polyphase = false;						// disable polyphase

            FWC.SetTest(true);
            //Thread.Sleep(50);
            FWC.SetSig(true);
            //Thread.Sleep(50);
            FWC.SetGen(true);
            //Thread.Sleep(50);
            FWC.SetFullDuplex(true);
            Thread.Sleep(500);

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };
            float[] on_table = new float[bands.Length];
            float[] off_table = new float[bands.Length];
            float[] a = new float[Display.BUFFER_SIZE];
            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = (10 + 10) * num_bands;

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "RX Filter Test - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    PreampMode pre = console.RX1PreampMode;

                    switch (console.CurrentModel)
                    {
                        case Model.FLEX5000:
                            console.RX1PreampMode = PreampMode.OFF;
                            break;
                        case Model.FLEX3000:
                            console.RX1PreampMode = PreampMode.LOW;
                            break;
                    }
                    Thread.Sleep(500);

                    float sum = 0.0f;
                    for (int j = 0; j < 10; j++)
                    {
                        console.calibration_mutex.WaitOne();
                        fixed (float* ptr = &a[0])
                            DttSP.GetSpectrum(0, ptr);		// read again to clear out changed DSP
                        console.calibration_mutex.ReleaseMutex();
                        if (j != 9) Thread.Sleep(100);
                        if (!p.Visible)
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter / (float)total_counts);

                        sum += a[2048];
                    }

                    float with_filter = sum / 10;

                    FWC.SetRX1Filter(-1.0f);
                    Thread.Sleep(500);

                    sum = 0.0f;
                    for (int j = 0; j < 10; j++)
                    {
                        console.calibration_mutex.WaitOne();
                        fixed (float* ptr = &a[0])
                            DttSP.GetSpectrum(0, ptr);		// read again to clear out changed DSP
                        console.calibration_mutex.ReleaseMutex();
                        if (j != 4) Thread.Sleep(100);
                        if (!p.Visible)
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter / (float)total_counts);

                        sum += a[2048];
                    }

                    float no_filter = sum / 10;

                    on_table[i] = with_filter;
                    off_table[i] = no_filter;

                    float target = 0.0f;
                    switch (console.CurrentModel)
                    {
                        case Model.FLEX5000:
                            target = avg[i];
                            break;
                        case Model.FLEX3000:
                            target = avg2[i];
                            break;
                    }

                    if ((no_filter < -10.0f || with_filter < -10.0f) ||
                        ((no_filter - with_filter) > target + 1.0f) ||
                        ((no_filter - with_filter) < -3.0f))
                    {
                        btnRXFilter.BackColor = Color.Red;
                        if (!test_rx_filter.StartsWith("RX Filter Test: Failed ("))
                            test_rx_filter = "RX Filter Test: Failed (";
                        test_rx_filter += BandToString(bands[i]) + ",";
                        lstDebug.Items.Insert(0, "RX Filter Test - " + BandToString(bands[i]) + ": Failed ("
                            + no_filter.ToString("f1") + ", " + with_filter.ToString("f1") + ", " + (no_filter - with_filter).ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "RX Filter Test - " + BandToString(bands[i]) + ": Passed ("
                            + no_filter.ToString("f1") + ", " + with_filter.ToString("f1") + ", " + (no_filter - with_filter).ToString("f1") + ")");
                    }

                    Debug.Write((no_filter - with_filter).ToString("f1") + " ");
                    //Debug.WriteLine(band_freqs[i].ToString("f6")+" diff: "+(no_filter-with_filter).ToString("f1")+"dB");
                    FWC.SetRX1Filter(band_freqs[i]);
                    //Thread.Sleep(50);

                    console.RX1PreampMode = pre;
                }
            }
            Debug.WriteLine("");

        end:
            if (test_rx_filter.StartsWith("RX Filter Test: Failed ("))
                test_rx_filter = test_rx_filter.Substring(0, test_rx_filter.Length - 1) + ")";
            toolTip1.SetToolTip(btnRXFilter, test_rx_filter);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\rx_filter.csv");
            StreamWriter writer = new StreamWriter(path + "\\rx_filter.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, "
                                  + "160m off, 160m on, 160m diff,"
                                  + "80m off, 80m on, 80m diff,"
                                  + "60m off, 60m on, 60m diff,"
                                  + "40m off, 40m on, 40m diff,"
                                  + "30m off, 30m on, 30m diff,"
                                  + "20m off, 20m on, 20m diff,"
                                  + "17m off, 17m on, 17m diff,"
                                  + "15m off, 15m on, 15m diff,"
                                  + "12m off, 12m on, 12m diff,"
                                  + "10m off, 10m on, 10m diff,"
                                  + "6m off, 6m on, 6m diff");
            writer.Write(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(off_table[i].ToString("f1") + ",");
                writer.Write(on_table[i].ToString("f1") + ",");
                writer.Write((off_table[i] - on_table[i]).ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\RX Filter";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX3000: model = "F3K"; break;
                case Model.FLEX5000: model = "F5K"; break;
            }
            writer = new StreamWriter(path + "\\rx_filter_" + model + "_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv");
            writer.WriteLine("Band, Off, On, Diff");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                writer.Write(off_table[i].ToString("f1") + ",");
                writer.Write(on_table[i].ToString("f1") + ",");
                writer.WriteLine((off_table[i] - on_table[i]).ToString("f1"));
            }
            writer.Close();

            console.TXFilterLow = tx_low;
            console.TXFilterHigh = tx_high;

            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            FWC.SetFullDuplex(false);
            //Thread.Sleep(50);

            console.MUT = mute;
            console.DisplayModeText = display;
            console.RX1DSPMode = dsp_mode;
            console.RX1FilterHigh = filt_high;
            console.RX1FilterLow = filt_low;
            console.RX1Filter = filter;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            // console.RX1PreampMode = preamp; // Commented out to preserve the initial F5K preamp state for 10 & 6m.  Was being forced to off.
            console.RITOn = rit_on;
            console.setupForm.DSPPhoneRXBuffer = dsp_buf_size;
            console.setupForm.Polyphase = polyphase;
            console.SpurReduction = spur_red;

            Invoke(new MethodInvoker(p.Hide));
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        private string test_1500_filter = "RX Filter Test: Not Run";
        private void Test1500Filter()
        {
            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.VFOSplit = false;
            test_1500_filter = "Filter Test: Passed";

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            int tx_low = console.TXFilterLow;
            int tx_high = console.TXFilterHigh;
            console.TXFilterLow = console.CWPitch - 100;
            console.TXFilterHigh = console.CWPitch + 100;

            bool spur_red = console.SpurReduction;
            console.SpurReduction = false;

            string display = console.DisplayModeText;
            console.DisplayModeText = "Spectrum";

            bool rit_on = console.RITOn;
            console.RITOn = false;

            bool mute = console.MUT;
            console.MUT = false;

            bool polyphase = console.setupForm.Polyphase;				// save current polyphase setting
            console.setupForm.Polyphase = false;						// disable polyphase

            USBHID.SetGen(true);
            Thread.Sleep(100);

            USBHID.SetQSE(true);
            Thread.Sleep(100);

            USBHID.SetTest(true);
            Thread.Sleep(100);

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] freqs_low = { 0.49f, 0.88f, 1.6f, 2.3f, 3.5f, 5.2f, 7.7f, 11.4f, 17.0f, 25.3f, 37.6f };
            float[] freqs_high = { 0.88f, 1.6f, 2.3f, 3.5f, 5.2f, 7.7f, 11.4f, 17.0f, 25.3f, 37.6f, 54.0f };
            float[] low_target_min = { 0.0f, -1.0f, 1.0f, -1.0f, 2.0f, 2.0f, -1.0f, 0.0f, 0.0f, -1.0f, -1.0f };
            float[] low_target_max = { 6.0f, 6.0f, 5.5f, 6.0f, 5.0f, 4.5f, 4.5f, 2.5f, 4.5f, 1.0f, 1.0f };
            float[] high_target_min = { 1.0f, 0.0f, 1.0f, -1.0f, 2.0f, 2.0f, 1.0f, 0.0f, -1.0f, 0.0f, 0.0f };
            float[] high_target_max = { 4.5f, 6.5f, 6.0f, 6.0f, 5.0f, 4.5f, 4.5f, 2.5f, 3.0f, 4.0f, 2.0f };
            float[] low_loss = new float[bands.Length];
            float[] high_loss = new float[bands.Length];
            float[] a = new float[Display.BUFFER_SIZE];
            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < freqs_low.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = (10 + 10) * num_bands;

            for (int i = 0; i < freqs_low.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "Filter Test - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    // check low side
                    console.VFOAFreq = freqs_low[i] * 1.02;
                    console.VFOBFreq = freqs_low[i] * 1.02;

                    int hid_filter = console.HIDTRXFilter;

                    PreampMode preamp = console.RX1PreampMode;
                    console.RX1PreampMode = PreampMode.OFF;

                    DSPMode dsp_mode = console.RX1DSPMode;
                    console.RX1DSPMode = DSPMode.DSB;
                    Thread.Sleep(10);

                    Filter filter = console.RX1Filter;
                    console.RX1Filter = Filter.VAR1;

                    int filt_low = console.RX1FilterLow;
                    int filt_high = console.RX1FilterHigh;
                    console.UpdateRX1Filters(-(int)(console.IFFreq * 1e6) + console.CWPitch - 100,
                        -(int)(console.IFFreq * 1e6) + console.CWPitch + 100);

                    HIDAnt hid_ant = console.RXAnt1500;
                    console.RXAnt1500 = HIDAnt.BITE;
                    Thread.Sleep(10);

                    DttSP.SetCorrectIQEnable(0); // turn off I/Q correction
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 0);
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 1);

                    double vol = Audio.SourceScale;
                    Audio.SourceScale = 0.178;

                    Audio.RX1OutputSignal = Audio.SignalSource.SINE;
                    USBHID.SetRXFilter(11 - i);

                    Thread.Sleep(500);

                    float num = 0.0f;
                    for (int j = 0; j < 5; j++)
                    {
                        num += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        Thread.Sleep(50);
                        if (!p.Visible)
                            goto end;
                        else p.SetPercent((float)((float)++counter / (float)total_counts));
                    }
                    float low_on = num / 5.0f;

                    USBHID.SetRXFilter(0); // bypass the filter
                    Thread.Sleep(500);

                    num = 0.0f;
                    for (int j = 0; j < 5; j++)
                    {
                        num += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        Thread.Sleep(50);
                        if (!p.Visible)
                            goto end;
                        else p.SetPercent((float)((float)++counter / (float)total_counts));
                    }
                    float low_off = num / 5.0f;

                    USBHID.SetRXFilter(hid_filter); Thread.Sleep(10);
                    Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
                    Audio.SourceScale = vol;
                    //DttSP.SetCorrectIQEnable(1);
                    console.RXAnt1500 = hid_ant;
                    console.UpdateRX1Filters(filt_low, filt_high);
                    console.RX1Filter = filter;
                    console.RX1DSPMode = dsp_mode;
                    console.RX1PreampMode = preamp;


                    // check high side
                    console.VFOAFreq = freqs_high[i] * 0.98;
                    console.VFOBFreq = freqs_high[i] * 0.98;

                    hid_filter = console.HIDTRXFilter;

                    preamp = console.RX1PreampMode;
                    console.RX1PreampMode = PreampMode.OFF;
                    Thread.Sleep(10);

                    dsp_mode = console.RX1DSPMode;
                    console.RX1DSPMode = DSPMode.DSB;
                    Thread.Sleep(10);

                    filter = console.RX1Filter;
                    console.RX1Filter = Filter.VAR1;

                    filt_low = console.RX1FilterLow;
                    filt_high = console.RX1FilterHigh;
                    console.UpdateRX1Filters(-(int)(console.IFFreq * 1e6) + console.CWPitch - 100,
                        -(int)(console.IFFreq * 1e6) + console.CWPitch + 100);

                    hid_ant = console.RXAnt1500;
                    console.RXAnt1500 = HIDAnt.BITE;
                    Thread.Sleep(10);

                    DttSP.SetCorrectIQEnable(0); // turn off I/Q correction
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 0);
                    DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 1);

                    vol = Audio.SourceScale;
                    Audio.SourceScale = 0.178;

                    Audio.RX1OutputSignal = Audio.SignalSource.SINE;
                    USBHID.SetRXFilter(11 - i);

                    Thread.Sleep(500);

                    num = 0.0f;
                    for (int j = 0; j < 5; j++)
                    {
                        num += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        Thread.Sleep(50);
                        if (!p.Visible)
                            goto end;
                        else p.SetPercent((float)((float)++counter / (float)total_counts));
                    }
                    float high_on = num / 5.0f;

                    USBHID.SetRXFilter(0); // bypass the filter
                    Thread.Sleep(500);

                    num = 0.0f;
                    for (int j = 0; j < 5; j++)
                    {
                        num += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        Thread.Sleep(50);
                        if (!p.Visible)
                            goto end;
                        else p.SetPercent((float)((float)++counter / (float)total_counts));
                    }
                    float high_off = num / 5.0f;

                    low_loss[i] = low_off - low_on;
                    high_loss[i] = high_off - high_on;

                    if (low_loss[i] < low_target_min[i] || low_loss[i] > low_target_max[i] ||
                        high_loss[i] < high_target_min[i] || high_loss[i] > high_target_max[i] ||
                        low_off < -40.0f || low_on < -40.0f || high_off < -40.0f || high_on < -40.0f)
                    {
                        btnRXFilter.BackColor = Color.Red;
                        if (!test_1500_filter.StartsWith("RX Filter Test: Failed ("))
                            test_1500_filter = "RX Filter Test: Failed (";
                        test_1500_filter += (11 - i).ToString().PadLeft(2, '0') + ", ";
                        lstDebug.Items.Insert(0, "Filter Test - " + (11 - i).ToString() + ": Failed ("
                            + low_loss[i].ToString("f1") + ", " + high_loss[i].ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "Filter Test - " + (11 - i).ToString() + ": Passed ("
                            + low_loss[i].ToString("f1") + ", " + high_loss[i].ToString("f1") + ")");
                    }

                end:
                    USBHID.SetRXFilter(hid_filter); // reset filter
                    Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
                    Audio.SourceScale = vol;
                    DttSP.SetCorrectIQEnable(1);
                    console.RXAnt1500 = hid_ant; Thread.Sleep(10);
                    console.UpdateRX1Filters(filt_low, filt_high);
                    console.RX1Filter = filter;
                    console.RX1DSPMode = dsp_mode; Thread.Sleep(10);
                    console.RX1PreampMode = preamp; Thread.Sleep(10);
                }

                if (!p.Visible) break;
            }
            Debug.WriteLine("");

            if (test_1500_filter.StartsWith("RX Filter Test: Failed ("))
                test_1500_filter = test_1500_filter.Substring(0, test_1500_filter.Length - 2) + ")";
            toolTip1.SetToolTip(btnRXFilter, test_1500_filter);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\rx_filter.csv");
            StreamWriter writer = new StreamWriter(path + "\\rx_filter.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, "
                                  + "1 low, 1 high, "
                                  + "2 low, 2 high, "
                                  + "3 low, 3 high, "
                                  + "4 low, 4 high, "
                                  + "5 low, 5 high, "
                                  + "6 low, 6 high, "
                                  + "7 low, 7 high, "
                                  + "8 low, 8 high, "
                                  + "9 low, 9 high, "
                                  + "10 low, 10 high, "
                                  + "11 low, 11 high,");
            writer.Write(console.CurrentModel.ToString() + ","
                + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(low_loss[i].ToString("f1") + ",");
                writer.Write(high_loss[i].ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\RX Filter";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "F1.5K";
            writer = new StreamWriter(path + "\\rx_filter_" + model + "_" + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber) + ".csv");
            writer.WriteLine("Band, Low, High");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write((11 - i).ToString() + ",");
                writer.Write(low_loss[i].ToString("f1") + ",");
                writer.Write(high_loss[i].ToString("f1") + ",");
            }
            writer.Close();

            console.TXFilterLow = tx_low;
            console.TXFilterHigh = tx_high;

            USBHID.SetTest(false); Thread.Sleep(10);
            USBHID.SetGen(false); Thread.Sleep(10);
            USBHID.SetQSE(false); Thread.Sleep(10);

            console.MUT = mute;
            console.DisplayModeText = display;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            console.RITOn = rit_on;
            console.setupForm.Polyphase = polyphase;
            console.SpurReduction = spur_red;

            Invoke(new MethodInvoker(p.Hide));
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region RX Level

        private void btnRXLevel_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnRXLevel.BackColor = Color.Green;
            CallCalRXLevel();
        }

        public void CallCalRXLevel()
        {
            p = new Progress("Calibrate RX Level");
            Thread t = new Thread(new ThreadStart(RunCalRXLevel));
            t.Name = "Calibrate RX Level Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_rx_level = "RX Level Test: Not Run";
        public void RunCalRXLevel()
        {
            //float[] offset_avg = {-59.3f, -60.5f, -61.1f, -61.4f, -60.8f, -60.5f, -60.0f, -59.5f, -59.5f, -59.5f, -59.6f};
            //float[] preamp_avg = {-13.4f, -7.0f, -13.3f, -13.6f, -14.0f, -14.0f, -14.0f, -13.8f, -13.8f, -13.8f, -13.7f};
            //float offset_tol = 2.5f;	// maximum distance from the average value
            //float preamp_tol = 1.5f;

            test_rx_level = "RX Level Test: Passed";
            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            int tx_low = console.TXFilterLow;
            console.TXFilterLow = 100;

            int tx_high = console.TXFilterHigh;
            console.TXFilterHigh = 200;

            /*int dsp_phone_buf_size = console.SetupForm.DSPPhoneRXBuffer;			// save current DSP buffer size
            int dsp_cw_buf_size = console.SetupForm.DSPCWRXBuffer;			// save current DSP buffer size
            int dsp_dig_buf_size = console.SetupForm.DSPDigRXBuffer;			// save current DSP buffer size
            console.SetupForm.DSPPhoneRXBuffer = 2048;						// set DSP Buffer Size to 2048
            console.SetupForm.DSPCWRXBuffer = 2048;
            console.SetupForm.DSPDigRXBuffer = 2048;*/

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();
            Band[] bands = { Band.B6M, Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M };
            float[] band_freqs = { 50.11f, 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f };

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            float level = (float)udLevel.Value;

            if (console.VFOSync)
                console.VFOSync = false;

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "RX Level Test - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    p.SetPercent(0.0f);
                    Invoke(new MethodInvoker(p.Show));
                    Application.DoEvents();
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];
                    console.CalibrateLevel(level, band_freqs[i], p, true);
                    if (p.Text == "") break;

                    if (console.CurrentModel == Model.FLEX1500)
                    {
                        if (console.GetRX1Level(bands[i], 0) >= -96.0 &&
                            console.GetRX1Level(bands[i], 0) <= -84.0)
                        {
                            lstDebug.Items.Insert(0, "RX Level Test - " + BandToString(bands[i]) + ": Passed ("
                                + console.GetRX1Level(bands[i], 0).ToString("f1") + ", "
                                + console.GetRX1Level(bands[i], 1).ToString("f1") + ", "
                                + console.GetRX1Level(bands[i], 2).ToString("f1") + ")");
                        }
                        else
                        {
                            btnRXLevel.BackColor = Color.Red;
                            if (!test_rx_level.StartsWith("RX Level Test: Failed ("))
                                test_rx_level = "RX Level Test: Failed (";
                            test_rx_level += BandToString(bands[i]) + ",";
                            lstDebug.Items.Insert(0, "RX Level Test - " + BandToString(bands[i]) + ": Failed ("
                                + console.GetRX1Level(bands[i], 0).ToString("f1") + ", "
                                + console.GetRX1Level(bands[i], 1).ToString("f1") + ", "
                                + console.GetRX1Level(bands[i], 2).ToString("f1") + ")");
                        }
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "RX Level Test - " + BandToString(bands[i]) + ": Passed ("
                                + console.GetRX1Level(bands[i], 0).ToString("f1") + ", "
                                + console.GetRX1Level(bands[i], 1).ToString("f1") + ", "
                                + console.GetRX1Level(bands[i], 2).ToString("f1") + ")");
                    }

                    Thread.Sleep(500);
                }
            }

            /*console.SetupForm.DSPPhoneRXBuffer = dsp_phone_buf_size;
            console.SetupForm.DSPCWRXBuffer = dsp_cw_buf_size;
            console.SetupForm.DSPDigRXBuffer = dsp_dig_buf_size;*/
            console.TXFilterLow = tx_low;
            console.TXFilterHigh = tx_high;

            if (test_rx_level.StartsWith("RX Level Test: Failed ("))
                test_rx_level = test_rx_level.Substring(0, test_rx_level.Length - 2) + ")";
            toolTip1.SetToolTip(btnRXLevel, test_rx_level);

            console.VFOAFreq = vfoa; Thread.Sleep(10);
            console.VFOBFreq = vfob;

            t1.Stop();
            Debug.WriteLine("RX Level Timer: " + t1.Duration);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\rx_level.csv");
            StreamWriter writer = new StreamWriter(path + "\\rx_level.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, "
                                  + "6m display offset, 6m preamp offset, 6m multimeter offset, "
                                  + "160m display offset, 160m preamp offset, 160m multimeter offset, "
                                  + "80m display offset, 80m preamp offset, 80m multimeter offset, "
                                  + "60m display offset, 60m preamp offset, 60m multimeter offset, "
                                  + "40m display offset, 40m preamp offset, 40m multimeter offset, "
                                  + "30m display offset, 30m preamp offset, 30m multimeter offset, "
                                  + "20m display offset, 20m preamp offset, 20m multimeter offset, "
                                  + "17m display offset, 17m preamp offset, 17m multimeter offset, "
                                  + "15m display offset, 15m preamp offset, 15m multimeter offset, "
                                  + "12m display offset, 12m preamp offset, 12m multimeter offset, "
                                  + "10m display offset, 10m preamp offset, 10m multimeter offset");
            writer.Write(console.CurrentModel.ToString() + ",");
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ",");
                    break;
                case Model.FLEX1500:
                    writer.Write(HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial) + ",");
                    break;
            }
            writer.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    writer.Write(console.GetRX1Level(bands[i], j).ToString("f1"));
                    if (i != bands.Length - 1 || j != 2) writer.Write(",");
                }
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\RX Level";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string filename = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    filename = path + "\\rx_level_F5K_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv";
                    break;
                case Model.FLEX3000:
                    filename = path + "\\rx_level_F3K_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv";
                    break;
                case Model.FLEX1500:
                    filename = path + "\\rx_level_F1.5K_" + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber) + ".csv";
                    break;
            }
            writer = new StreamWriter(filename);
            writer.WriteLine("Band, Display Offset, Preamp Offset, Multimeter Offset");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                writer.Write(console.GetRX1Level(bands[i], 0).ToString("f1") + ",");
                writer.Write(console.GetRX1Level(bands[i], 1).ToString("f1") + ",");
                writer.WriteLine(console.GetRX1Level(bands[i], 2).ToString("f1"));
            }
            writer.Close();

            lstDebug.Items.Insert(0, "Saving Level data to EEPROM...");
            byte checksum = 0;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWCEEPROM.WriteRXLevel(console.rx1_level_table, out checksum);
                    break;
                case Model.FLEX1500:
                    HIDEEPROM.WriteRXLevel(console.rx1_level_table, out checksum);
                    break;
            }
            console.rx1_level_checksum = checksum;
            console.SyncCalDateTime(); Thread.Sleep(10);
            lstDebug.Items[0] = "Saving Level data to EEPROM...done";

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region RX Image

        private void btnRXImage_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnRXImage.BackColor = Color.Green;
            CallCalFWCRXImage();
        }

        public Thread CallCalFWCRXImage()
        {
            p = new Progress("Calibrate RX Image");
            Thread t = new Thread(new ThreadStart(RunCalFWCRXImage));
            t.Name = "Calibrate RX Image Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            return t;
        }

        private string test_rx_image = "RX Image Test: Not Run";
        public void RunCalFWCRXImage()
        {
            test_rx_image = "RX Image Test: Passed";

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            int tx_low = console.TXFilterLow;
            console.TXFilterLow = 100;

            int tx_high = console.TXFilterHigh;
            console.TXFilterHigh = 200;

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };
            for (int i = 0; i < band_freqs.Length; i++)
            {
                float rejection_tol = 80.0f;	// rejection from worst to null
                float floor_tol = 10.0f;		// from null to noise floor

                if (console.CurrentModel == Model.FLEX3000 &&
                    FWCEEPROM.TRXRev >> 8 < 6) // before rev G
                {
                    switch (bands[i])
                    {
                        case Band.B160M:
                        case Band.B80M:
                        case Band.B60M:
                            rejection_tol = 77.0f;
                            break;
                    }
                }

                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "RX Image - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    p.SetPercent(0.0f);
                    Invoke(new MethodInvoker(p.Show));
                    Application.DoEvents();
                    console.VFOAFreq = band_freqs[i] + 2 * console.IFFreq;
                    console.VFOBFreq = band_freqs[i];
                    bool b = console.CalibrateRXImage(band_freqs[i], p, true);

                    if (!b || console.rx_image_rejection[(int)bands[i]] < rejection_tol ||
                        console.rx_image_from_floor[(int)bands[i]] > floor_tol)
                    {
                        if (!test_rx_image.StartsWith("RX Image Test: Failed ("))
                            test_rx_image = "RX Image Test: Failed (";
                        test_rx_image += BandToString(bands[i]) + ",";
                        btnRXImage.BackColor = Color.Red;
                        lstDebug.Items.Insert(0, "RX Image - " + BandToString(bands[i]) + ": Failed ("
                            + console.rx_image_rejection[(int)bands[i]].ToString("f1") + ", "
                            + console.rx_image_from_floor[(int)bands[i]].ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "RX Image - " + BandToString(bands[i]) + ": Passed ("
                            + console.rx_image_rejection[(int)bands[i]].ToString("f1") + ", "
                            + console.rx_image_from_floor[(int)bands[i]].ToString("f1") + ")");
                    }

                    if (p.Text == "")
                    {
                        if (console.CurrentModel == Model.FLEX1500 && !b &&
                            console.rx_image_rejection[(int)bands[i]] == 0.0f &&
                            console.rx_image_from_floor[(int)bands[i]] == 100.0f)
                            MessageBox.Show("Error finding Signal.  Double check Sig Gen cabling.",
                                "Error: Signal Not Found",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        break;
                    }

                    Thread.Sleep(500);
                }
            }

            console.TXFilterLow = tx_low;
            console.TXFilterHigh = tx_high;

            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            if (test_rx_image.StartsWith("RX Image Test: Failed ("))
                test_rx_image = test_rx_image.Substring(0, test_rx_image.Length - 1) + ")";
            toolTip1.SetToolTip(btnRXImage, test_rx_image);

            t1.Stop();
            Debug.WriteLine("RX Image Timer: " + t1.Duration);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\rx_image.csv");
            StreamWriter writer = new StreamWriter(path + "\\rx_image.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, "
                                  + "160m gain, 160m phase, 160m rejection, 160m noise distance, "
                                  + "80m gain, 80m phase, 80m rejection, 80m noise distance, "
                                  + "60m gain, 60m phase, 60m rejection, 60m noise distance, "
                                  + "40m gain, 40m phase, 40m rejection, 40m noise distance, "
                                  + "30m gain, 30m phase, 30m rejection, 30m noise distance, "
                                  + "20m gain, 20m phase, 20m rejection, 20m noise distance, "
                                  + "17m gain, 17m phase, 17m rejection, 17m noise distance, "
                                  + "15m gain, 15m phase, 15m rejection, 15m noise distance, "
                                  + "12m gain, 12m phase, 12m rejection, 12m noise distance, "
                                  + "10m gain, 10m phase, 10m rejection, 10m noise distance, "
                                  + "6m gain, 6m phase, 6m rejection, 6m noise distance");

            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
            }

            writer.Write(console.CurrentModel.ToString() + ","
                + serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(console.rx1_image_gain_table[(int)bands[i]].ToString("f11") + ",");
                writer.Write(console.rx1_image_phase_table[(int)bands[i]].ToString("f11") + ",");
                writer.Write(console.rx_image_rejection[(int)bands[i]].ToString("f1") + ",");
                writer.Write(console.rx_image_from_floor[(int)bands[i]].ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\RX Image";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    model = "F5K" + "_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX3000:
                    model = "F3K" + "_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    model = "F1.5K" + "_" + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
            }
            writer = new StreamWriter(path + "\\rx_image_" + model + ".csv");
            writer.WriteLine("Band, Gain, Phase, Rejection, Noise Distance");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                writer.Write(console.rx1_image_gain_table[(int)bands[i]].ToString("f11") + ",");
                writer.Write(console.rx1_image_phase_table[(int)bands[i]].ToString("f11") + ",");
                writer.Write(console.rx_image_rejection[(int)bands[i]].ToString("f1") + ",");
                writer.WriteLine(console.rx_image_from_floor[(int)bands[i]].ToString("f1"));
            }
            writer.Close();

            lstDebug.Items.Insert(0, "Saving RX Image data to EEPROM...");
            byte gain_sum = 0, phase_sum = 0;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWCEEPROM.WriteRXImage(console.rx1_image_gain_table, console.rx1_image_phase_table, out gain_sum, out phase_sum);
                    break;
                case Model.FLEX1500:
                    HIDEEPROM.WriteRXImage(console.rx1_image_gain_table, console.rx1_image_phase_table, out gain_sum, out phase_sum);
                    break;
            }
            console.rx1_image_gain_checksum = gain_sum;
            console.rx1_image_phase_checksum = phase_sum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving RX Image data to EEPROM...done";

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region MDS

        /*private void btnRXMDS_Click(object sender, System.EventArgs e)
		{
			grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
			btnRXMDS.BackColor = Color.Green;
			CallCalFWCRXMDS();
		}

		public void CallCalFWCRXMDS()
		{
			p = new Progress("Calibrate RX MDS");
			Thread t = new Thread(new ThreadStart(RunCalFWCRXMDS));
			t.Name = "Calibrate RX Image Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}

		private string test_rx_mds = "RX MDS Test: Not Run";
		public void RunCalFWCRXMDS()
		{
			if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on to run this test.",
					"Power not on",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			
			float[] avg = {-128.8f, -133.7f, -134.4f, -135.7f, -134.3f, -131.6f, -132.4f, -132.0f, -131.4f, -131.7f, -129.2f};
			float tol = 20.0f;	// maximum distance from average noise floor
			test_rx_mds = "RX MDS Test: Passed";

			double vfoa = console.VFOAFreq;
			double vfob = console.VFOBFreq;
			HiPerfTimer t1 = new HiPerfTimer();
			t1.Start();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};
			float[] mds_table = new float[bands.Length];
			for(int i=0; i<band_freqs.Length; i++)
			{
				bool do_band = false;
				switch(bands[i])
				{
					case Band.B160M: do_band = chk160.Checked; break;
					case Band.B80M: do_band = chk80.Checked; break;
					case Band.B60M: do_band = chk60.Checked; break;
					case Band.B40M: do_band = chk40.Checked; break;
					case Band.B30M: do_band = chk30.Checked; break;
					case Band.B20M: do_band = chk20.Checked; break;
					case Band.B17M: do_band = chk17.Checked; break;
					case Band.B15M: do_band = chk15.Checked; break;
					case Band.B12M: do_band = chk12.Checked; break;
					case Band.B10M:	do_band = chk10.Checked; break;
					case Band.B6M: do_band = chk6.Checked; break;
				}

				if(do_band)
				{
					p.SetPercent(0.0f);
					Invoke(new MethodInvoker(p.Show));
					Application.DoEvents();
					console.VFOAFreq = band_freqs[i];
					float val = CheckRXMDS(band_freqs[i], p);
					mds_table[i] = val;
					if(p.Text == "" || !p.Visible) break;

					if(val - avg[i] > tol)
					{
						if(!test_rx_mds.StartsWith("RX MDS Test: Failed ("))
							test_rx_mds = "RX MDS Test: Failed (";
						test_rx_mds += BandToString(bands[i])+", ";
						btnRXMDS.BackColor = Color.Red;
						lstDebug.Items.Insert(0, "RX MDS - "+BandToString(bands[i])+": Failed ("
							+val.ToString("f1")+" > target "+(avg[i]+tol).ToString("f1")+")");
					}
					else
					{
						lstDebug.Items.Insert(0, "RX MDS - "+BandToString(bands[i])+": Passed ("
							+val.ToString("f1")+")");
					}

					Thread.Sleep(500);
				}
			}
			
			console.VFOAFreq = vfoa;
			console.VFOBFreq = vfob;
			if(test_rx_mds.StartsWith("RX MDS Test: Failed ("))
				test_rx_mds = test_rx_mds.Substring(0, test_rx_mds.Length-2)+")";
			toolTip1.SetToolTip(btnRXMDS, test_rx_mds);

			t1.Stop();
			Debug.WriteLine("RX Image Timer: "+t1.Duration);

			string path = app_data_path+"Tests";
			if(!Directory.Exists(path))	Directory.CreateDirectory(path);
			bool file_exists = File.Exists(path+"\\rx_mds.csv");
			StreamWriter writer = new StreamWriter(path+"\\rx_mds.csv", true);
			if(!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, "
								 +"160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m, 6m");

			writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial)+","
				+DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+","
				+console.Text+",");
			for(int i=0; i<bands.Length; i++)
				writer.Write(mds_table[i].ToString("f1")+",");
			writer.WriteLine("");
			writer.Close();

			p.Hide();
			grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
		}

		private float CheckRXMDS(float freq, Progress progress)
		{
			double vfob_freq = console.VFOBFreq;

			bool full_duplex = console.FullDuplex;
			console.FullDuplex = false;

			double vfoa_freq = console.VFOAFreq;
			console.VFOAFreq = freq;

			DSPMode dsp = console.RX1DSPMode;
			console.RX1DSPMode = DSPMode.USB;

			PreampMode preamp = console.RX1PreampMode;
			console.RX1PreampMode = PreampMode.HIGH;

			Filter filter = console.RX1Filter;
			int var_low = console.RX1FilterLow;
			int var_high = console.RX1FilterHigh;
			console.RX1Filter = Filter.VAR1;
			console.RX1FilterLow = 50;
			console.RX1FilterHigh = 550;

			Thread.Sleep(500);

			float[] a = new float[Display.BUFFER_SIZE];
			float sum = 0.0f;
			int counter = 0;

			for(int i=0; i<50; i++)
			{
				sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
				Thread.Sleep(50);
				if(!progress.Visible)
					return 1.0f;
				else progress.SetPercent((float)((float)++counter/50));
			}
			float avg = sum / 50.0f;

			avg = avg + console.rx1_meter_cal_offset
				+ console.GetRXLevel(console.CurrentBand, 1)
				+ console.RXPathOffset; // adjust for offset;

			console.VFOAFreq = vfoa_freq;
			console.FullDuplex = full_duplex;
			console.VFOBFreq = vfob_freq;
			console.RX1DSPMode = dsp;
			console.RX1PreampMode = preamp;
			console.RX1Filter = filter;
			if(filter == Filter.VAR1 || filter == Filter.VAR2)
			{
				console.RX1FilterLow = var_low;
				console.RX1FilterHigh = var_high;
			}
			
			return avg;
		}*/

        #endregion

        #endregion

        #region TX Tests

        #region Filter

        private void btnTXFilter_Click(object sender, System.EventArgs e)
        {
            p = new Progress("Test TX Filter");
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnTXFilter.BackColor = Color.Green;
            Thread t = new Thread(new ThreadStart(TestTXFilter));
            t.Name = "Test TX Filter Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            Invoke(new MethodInvoker(p.Show));
        }

        private string test_tx_filter = "TX Filter Test: Not Run";
        private void TestTXFilter()
        {
            float[] avg = { };
            float tol = 1.0f;	// maximum acceptable filter loss in dB
            /*if(!console.PowerOn)
            {
                if(!console.PowerOn)
                {
                    p.Hide();
                    MessageBox.Show("Power must be on to run this test.",
                        "Power not on",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);					
                    grpGeneral.Enabled = true;
                    grpReceiver.Enabled = true;
                    grpTransmitter.Enabled = true;
                    return;
                }
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.VFOSplit = false;
            test_tx_filter = "TX Filter Test: Passed";

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            bool spur_red = console.SpurReduction;
            console.SpurReduction = false;

            string display = console.DisplayModeText;
            console.DisplayModeText = "Spectrum";

            // PreampMode preamp = console.RX1PreampMode;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            Filter filter = console.RX1Filter;
            console.RX1Filter = Filter.VAR1;

            int filt_high = console.RX1FilterHigh;
            console.RX1FilterHigh = 700;

            int filt_low = console.RX1FilterLow;
            console.RX1FilterLow = 500;

            console.UpdateRX1Filters(500, 700);

            bool rit_on = console.RITOn;
            console.RITOn = false;

            int dsp_buf_size = console.setupForm.DSPPhoneRXBuffer;			// save current DSP buffer size
            console.setupForm.DSPPhoneRXBuffer = 4096;						// set DSP Buffer Size to 2048

            bool polyphase = console.setupForm.Polyphase;				// save current polyphase setting
            console.setupForm.Polyphase = false;						// disable polyphase

            double tone_scale = Audio.SourceScale;
            Audio.SourceScale = 1.0;

            double tone_freq = Audio.SineFreq1;				// save tone freq
            Audio.SineFreq1 = 600.0;						// set freq

            Audio.TXInputSignal = Audio.SignalSource.SINE;

            console.FullDuplex = true;
            //Thread.Sleep(50);
            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(true);
            //Thread.Sleep(50);
            FWC.SetTR(true);
            //Thread.Sleep(50);
            FWC.SetSig(true);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(true);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            Thread.Sleep(500);

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };
            float[] on_table = new float[bands.Length];
            float[] off_table = new float[bands.Length];
            float[] a = new float[Display.BUFFER_SIZE];
            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = (5 + 5) * num_bands;

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "TX Filter Test - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];
                    PreampMode pre = console.RX1PreampMode;
                    console.RX1PreampMode = PreampMode.OFF;
                    Audio.RadioVolume = 0.075;
                    Thread.Sleep(500);

                    int peak_bin = -1;
                    float max = float.MinValue;
                    float sum = 0.0f;
                    for (int j = 0; j < 5; j++)
                    {
                        console.calibration_mutex.WaitOne();
                        fixed (float* ptr = &a[0])
                            DttSP.GetSpectrum(0, ptr);		// read again to clear out changed DSP
                        console.calibration_mutex.ReleaseMutex();
                        if (j != 4) Thread.Sleep(100);
                        if (!p.Visible) goto end;
                        p.SetPercent(++counter / (float)total_counts);

                        if (j == 0)
                        {
                            for (int k = 0; k < Display.BUFFER_SIZE; k++)
                            {
                                if (a[k] > max)
                                {
                                    max = a[k];
                                    peak_bin = k;
                                }
                            }
                        }

                        sum += a[peak_bin];
                    }

                    float with_filter = sum / 5;

                    FWC.SetTXFilter(-1.0f);
                    Thread.Sleep(500);
                    sum = 0.0f;
                    for (int j = 0; j < 5; j++)
                    {
                        console.calibration_mutex.WaitOne();
                        fixed (float* ptr = &a[0])
                            DttSP.GetSpectrum(0, ptr);		// read again to clear out changed DSP
                        console.calibration_mutex.ReleaseMutex();
                        if (j != 4) Thread.Sleep(100);
                        if (!p.Visible) goto end;
                        p.SetPercent(++counter / (float)total_counts);

                        sum += a[peak_bin];
                    }

                    float no_filter = sum / 5;

                    on_table[i] = with_filter;
                    off_table[i] = no_filter;

                    if (bands[i] == Band.B160M || bands[i] == Band.B80M || bands[i] == Band.B60M)
                        tol = 2.5f;
                    else
                        tol = 1.0f;

                    if ((with_filter < -10.0f || no_filter < -10.0f) ||
                        ((no_filter - with_filter) > tol) ||
                        ((no_filter - with_filter) < -0.5f))
                    {
                        btnTXFilter.BackColor = Color.Red;
                        if (!test_tx_filter.StartsWith("TX Filter Test: Failed ("))
                            test_tx_filter = "TX Filter Test: Failed (";
                        test_tx_filter += BandToString(bands[i]) + ",";
                        lstDebug.Items.Insert(0, "TX Filter Test - " + BandToString(bands[i]) + ": Failed ("
                            + no_filter.ToString("f1") + ", " + with_filter.ToString("f1") + ", " + (no_filter - with_filter).ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "TX Filter Test - " + BandToString(bands[i]) + ": Passed ("
                            + no_filter.ToString("f1") + ", " + with_filter.ToString("f1") + ", " + (no_filter - with_filter).ToString("f1") + ")");
                    }

                    Debug.Write((no_filter - with_filter).ToString("f1") + " ");
                    //Debug.WriteLine(band_freqs[i].ToString("f6")+" diff: "+(no_filter-with_filter).ToString("f1")+"dB");
                    FWC.SetTXFilter(band_freqs[i]);
                    //Thread.Sleep(50);

                    console.RX1PreampMode = pre;
                }
            }
            Debug.WriteLine("");

        end:
            if (test_tx_filter.StartsWith("TX Filter Test: Failed ("))
                test_tx_filter = test_tx_filter.Substring(0, test_tx_filter.Length - 1) + ")";
            toolTip1.SetToolTip(btnTXFilter, test_tx_filter);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\tx_filter.csv");
            StreamWriter writer = new StreamWriter(path + "\\tx_filter.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version"
                                  + "160m off, 160m on, 160m diff,"
                                  + "80m off, 80m on, 80m diff,"
                                  + "60m off, 60m on, 60m diff,"
                                  + "40m off, 40m on, 40m diff,"
                                  + "30m off, 30m on, 30m diff,"
                                  + "20m off, 20m on, 20m diff,"
                                  + "17m off, 17m on, 17m diff,"
                                  + "15m off, 15m on, 15m diff,"
                                  + "12m off, 12m on, 12m diff,"
                                  + "10m off, 10m on, 10m diff,"
                                  + "6m off, 6m on, 6m diff");
            writer.Write(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(off_table[i].ToString("f1") + ",");
                writer.Write(on_table[i].ToString("f1") + ",");
                writer.Write((off_table[i] - on_table[i]).ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\TX Filter";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX3000: model = "F3K"; break;
                case Model.FLEX5000: model = "F5K"; break;
            }
            writer = new StreamWriter(path + "\\tx_filter_" + model + "_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv");
            writer.WriteLine("Band, Off, On, Diff");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                writer.Write(off_table[i].ToString("f1") + ",");
                writer.Write(on_table[i].ToString("f1") + ",");
                writer.WriteLine((off_table[i] - on_table[i]).ToString("f1"));
            }
            writer.Close();

            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            console.FullDuplex = false;
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            Audio.SourceScale = tone_scale;
            Audio.SineFreq1 = tone_freq;
            console.DisplayModeText = display;
            console.RX1DSPMode = dsp_mode;
            console.RX1FilterHigh = filt_high;
            console.RX1FilterLow = filt_low;
            console.RX1Filter = filter;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            // console.RX1PreampMode = preamp;  // Commented out to preserve the initial F5K preamp state for 10 & 6m.  Was being forced to off.
            console.RITOn = rit_on;
            console.setupForm.DSPPhoneRXBuffer = dsp_buf_size;
            console.setupForm.Polyphase = polyphase;
            console.SpurReduction = spur_red;

            Invoke(new MethodInvoker(p.Hide));
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Image

        private string test_tx_image = "TX Image Test: Not Run";
        private void btnTXImage_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnTXImage.BackColor = Color.Green;
            CallCalFWCTXImage();
        }

        public void CallCalFWCTXImage()
        {
            p = new Progress("Calibrate TX Image");
            Thread t = new Thread(new ThreadStart(RunCalFWCTXImage));
            t.Name = "Calibrate TX Image Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        public void RunCalFWCTXImage()
        {
            float tol = -55.0f;
            test_tx_image = "TX Image Test: Passed";

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;
            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "TX Image - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    p.SetPercent(0.0f);
                    Invoke(new MethodInvoker(p.Show));
                    Application.DoEvents();
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];
                    console.CalibrateTXImage(band_freqs[i], p, true);
                    if (p.Text == "") break;

                    if (console.tx_image_rejection[(int)bands[i]] > tol)
                    {
                        if (!test_tx_image.StartsWith("TX Image Test: Failed ("))
                            test_tx_image = "TX Image Test: Failed (";
                        test_tx_image += BandToString(bands[i]) + ",";
                        btnTXImage.BackColor = Color.Red;
                        lstDebug.Items.Insert(0, "TX Image - " + BandToString(bands[i]) + ": Failed ("
                            + console.tx_image_rejection[(int)bands[i]].ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "TX Image - " + BandToString(bands[i]) + ": Passed ("
                            + console.tx_image_rejection[(int)bands[i]].ToString("f1") + ")");
                    }

                    Thread.Sleep(500);
                }
            }
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;

            t1.Stop();
            Debug.WriteLine("TX Image Timer: " + t1.Duration);

            if (test_tx_image.StartsWith("TX Image Test: Failed ("))
                test_tx_image = test_tx_image.Substring(0, test_tx_image.Length - 1) + ")";
            toolTip1.SetToolTip(btnTXImage, test_tx_image);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\tx_image.csv");
            StreamWriter writer = new StreamWriter(path + "\\tx_image.csv", true);

            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber);
                    break;
            }

            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, "
                                  + "160m gain, 160m phase, 160m rejection, "
                                  + "80m gain, 80m phase, 80m rejection, "
                                  + "60m gain, 60m phase, 60m rejection, "
                                  + "40m gain, 40m phase, 40m rejection, "
                                  + "30m gain, 30m phase, 30m rejection, "
                                  + "20m gain, 20m phase, 20m rejection, "
                                  + "17m gain, 17m phase, 17m rejection, "
                                  + "15m gain, 15m phase, 15m rejection, "
                                  + "12m gain, 12m phase, 12m rejection, "
                                  + "10m gain, 10m phase, 10m rejection, "
                                  + "6m gain, 6m phase, 6m rejection, ");
            writer.Write(console.CurrentModel.ToString() + ","
                + serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(console.tx_image_gain_table[(int)bands[i]].ToString("f3") + ",");
                writer.Write(console.tx_image_phase_table[(int)bands[i]].ToString("f3") + ",");
                writer.Write(console.tx_image_rejection[(int)bands[i]].ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\TX Image";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX3000: model = "F3K"; break;
                case Model.FLEX5000: model = "F5K"; break;
                case Model.FLEX1500: model = "F1.5K"; break;
            }
            writer = new StreamWriter(path + "\\tx_image_" + model + "_" + serial + ".csv");
            writer.WriteLine("Band, Gain, Phase, Rejection dBc");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                writer.Write(console.tx_image_gain_table[(int)bands[i]].ToString("f3") + ",");
                writer.Write(console.tx_image_phase_table[(int)bands[i]].ToString("f3") + ",");
                writer.WriteLine(console.tx_image_rejection[(int)bands[i]].ToString("f1"));
            }
            writer.Close();

            lstDebug.Items.Insert(0, "Saving TX Image data to EEPROM...");
            byte gain_sum = 0, phase_sum = 0;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWCEEPROM.WriteTXImage(console.tx_image_gain_table, console.tx_image_phase_table, out gain_sum, out phase_sum);
                    break;
                case Model.FLEX1500:
                    HIDEEPROM.WriteTXImage(console.tx_image_gain_table, console.tx_image_phase_table, out gain_sum, out phase_sum);
                    break;
            }
            console.tx_image_gain_checksum = gain_sum;
            console.tx_image_phase_checksum = phase_sum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving TX Image data to EEPROM...done";

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Carrier

        private string test_tx_carrier = "TX Carrier Test: Not Run";
        private void btnTXCarrier_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnTXCarrier.BackColor = Color.Green;
            CallCalFWCTXCarrier();
        }

        public void CallCalFWCTXCarrier()
        {
            p = new Progress("Calibrate TX Carrier");
            Thread t = new Thread(new ThreadStart(RunCalFWCTXCarrier));
            t.Name = "Run Calibrate TX Carrier Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        public void RunCalFWCTXCarrier()
        {
            float tol = -105.0f;
            test_tx_carrier = "TX Carrier Test: Passed";

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            int tx_low = console.TXFilterLow;
            console.TXFilterLow = 100;

            int tx_high = console.TXFilterHigh;
            console.TXFilterHigh = 200;

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            double[] band_freqs = { 1.85, 3.75, 5.357, 7.15, 10.125, 14.175, 18.1, 21.300, 24.9, 28.4, 50.11 };
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "TX Carrier - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    p.SetPercent(0.0f);
                    Invoke(new MethodInvoker(p.Show));
                    Application.DoEvents();
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];
                    console.CalibrateTXCarrier(band_freqs[i], p, true);
                    //if(console.min_tx_carrier[(int)bands[i]] > tol) // try again
                    //	console.CalibrateTXCarrier(band_freqs[i], p, true);

                    if (p.Text == "") break;

                    if (console.min_tx_carrier[(int)bands[i]] > tol)
                    {
                        if (!test_tx_carrier.StartsWith("TX Carrier Test: Failed ("))
                            test_tx_carrier = "TX Carrier Test: Failed (";
                        test_tx_carrier += BandToString(bands[i]) + ",";
                        btnTXCarrier.BackColor = Color.Red;
                        lstDebug.Items.Insert(0, "TX Carrier - " + BandToString(bands[i]) + ": Failed ("
                            + console.min_tx_carrier[(int)bands[i]].ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "TX Carrier - " + BandToString(bands[i]) + ": Passed ("
                            + console.min_tx_carrier[(int)bands[i]].ToString("f1") + ")");
                    }
                    Thread.Sleep(500);
                }
            }

            console.TXFilterLow = tx_low;
            console.TXFilterHigh = tx_high;

            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;

            if (test_tx_carrier.StartsWith("TX Carrier Test: Failed ("))
                test_tx_carrier = test_tx_carrier.Substring(0, test_tx_carrier.Length - 1) + ")";
            toolTip1.SetToolTip(btnTXCarrier, test_tx_carrier);

            t1.Stop();
            Debug.WriteLine("TX Carrier Timer: " + t1.Duration);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\tx_carrier.csv");
            StreamWriter writer = new StreamWriter(path + "\\tx_carrier.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version, "
                                  + "160m C0, 160m C1, 160m C2, 160m C3, 160m From Noise, "
                                  + "80m C0, 80m C1, 80m C2, 80m C3, 80m From Noise, "
                                  + "60m C0, 60m C1, 60m C2, 60m C3, 60m From Noise, "
                                  + "40m C0, 40m C1, 40m C2, 40m C3, 40m From Noise, "
                                  + "30m C0, 30m C1, 30m C2, 30m C3, 30m From Noise, "
                                  + "20m C0, 20m C1, 20m C2, 20m C3, 20m From Noise, "
                                  + "17m C0, 17m C1, 17m C2, 17m C3, 17m From Noise, "
                                  + "15m C0, 15m C1, 15m C2, 15m C3, 15m From Noise, "
                                  + "12m C0, 12m C1, 12m C2, 12m C3, 12m From Noise, "
                                  + "10m C0, 10m C1, 10m C2, 10m C3, 10m From Noise, "
                                  + "6m C0, 6m C1, 6m C2, 6m C3, 6m From Noise");
            writer.Write(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                    writer.Write((console.tx_carrier_cal[Math.Round(band_freqs[i], 3)] >> 8 * (3 - j)).ToString() + ",");
                writer.Write(console.min_tx_carrier[(int)bands[i]].ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\TX Carrier";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX3000: model = "F3K"; break;
                case Model.FLEX5000: model = "F5K"; break;
            }
            writer = new StreamWriter(path + "\\tx_carrier_" + model + "_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv");
            writer.WriteLine("Band, C0, C1, C2, C3, From Noise");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                for (int j = 0; j < 4; j++)
                    writer.Write((console.tx_carrier_cal[Math.Round(band_freqs[i], 3)] >> 8 * (3 - j)).ToString() + ",");
                writer.WriteLine(console.min_tx_carrier[(int)bands[i]].ToString("f1"));
            }
            writer.Close();

            lstDebug.Items.Insert(0, "Saving Carrier data to EEPROM...");
            byte checksum;
            FWCEEPROM.WriteTXCarrier(console.tx_carrier_cal, out checksum);
            console.tx_carrier_checksum = checksum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving Carrier data to EEPROM...done";

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Gain

        private void btnTXGain_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnTXGain.BackColor = Color.Green;

            p = new Progress("Test TX Gain");
            Thread t = new Thread(new ThreadStart(TestTXGain));
            t.Name = "Test TX Gain Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            Invoke(new MethodInvoker(p.Show));
        }

        private string test_tx_gain = "TX Gain Test: Not Run";
        private void TestTXGain()
        {
            float tol = 1.0f;	// maximum acceptable filter loss in dB
            float[] target = { -26.3f, -25.6f, -25.9f, -24.7f, -25.3f, -26.1f, -27.2f, -27.4f, -27.9f, -28.5f, -36.2f };

            /*if(!console.PowerOn)
            {
                if(!console.PowerOn)
                {
                    p.Hide();
                    MessageBox.Show("Power must be on to run this test.",
                        "Power not on",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);					
                    grpGeneral.Enabled = true;
                    grpReceiver.Enabled = true;
                    grpTransmitter.Enabled = true;
                    return;
                }
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };
            float[] gain_table = new float[bands.Length];

            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = (5) * num_bands;

            console.VFOSplit = false;
            test_tx_gain = "TX Gain Test: Passed";

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            bool spur_red = console.SpurReduction;
            console.SpurReduction = true;

            bool rx2 = console.RX2Enabled;
            console.RX2Enabled = false;

            string display = console.DisplayModeText;
            console.DisplayModeText = "Panadapter";

            PreampMode preamp = console.RX1PreampMode;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            Filter filter = console.RX1Filter;
            console.RX1Filter = Filter.VAR1;

            int filt_high = console.RX1FilterHigh;
            console.RX1FilterHigh = 800;

            int filt_low = console.RX1FilterLow;
            console.RX1FilterLow = 400;

            console.UpdateRX1Filters(400, 800);

            bool rit_on = console.RITOn;
            console.RITOn = false;

            int dsp_buf_size = console.setupForm.DSPPhoneRXBuffer;			// save current DSP buffer size
            console.setupForm.DSPPhoneRXBuffer = 4096;						// set DSP Buffer Size to 2048

            bool polyphase = console.setupForm.Polyphase;				// save current polyphase setting
            console.setupForm.Polyphase = false;						// disable polyphase

            MeterRXMode rx_meter = console.CurrentMeterRXMode;			// save current RX Meter mode
            console.CurrentMeterRXMode = MeterRXMode.OFF;				// turn RX Meter off

            double tone_scale = Audio.SourceScale;
            Audio.SourceScale = 0.050;

            double tone_freq = Audio.SineFreq1;				// save tone freq
            Audio.SineFreq1 = 600.0;						// set freq

            Audio.TXOutputSignal = Audio.SignalSource.SINE;

            console.FullDuplex = true;
            //Thread.Sleep(50);
            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(true);
            //Thread.Sleep(50);
            FWC.SetTR(true);
            //Thread.Sleep(50);
            FWC.SetSig(true);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(true);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);
            FWC.SetRX1Filter(-1.0f);
            //Thread.Sleep(50);
            FWC.SetTXFilter(-1.0f);
            Thread.Sleep(500);

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M:
                        {
                            if (console.CurrentRegion == FRSRegion.US) { do_band = ck60.Checked; break; }
                            else
                            {
                                lstDebug.Items.Insert(0, "TX Gain Test - " + BandToString(bands[i]) + ": Result OK");
                                continue;
                            }
                        }
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];
                    console.RX1PreampMode = PreampMode.OFF;
                    Thread.Sleep(500);

                    float[] gain = new float[5];
                    for (int ii = 0; ii < 5; ii++) gain[ii] = 0.0f;

                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for (int j = 0; j < 5; j++)
                    {
                        gain[j] = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if (j != 4) Thread.Sleep(50);
                        if (!p.Visible) goto end;
                        p.SetPercent(++counter / (float)total_counts);
                    }

                    Array.Sort(gain);

                    //gain += console.rx1_meter_cal_offset;
                    gain_table[i] = gain[2];

                    if (Math.Abs(gain[2] - target[i]) > tol)
                    {
                        btnTXGain.BackColor = Color.Red;
                        if (!test_tx_gain.StartsWith("TX Gain Test: Failed ("))
                            test_tx_gain = "TX Gain Test: Failed (";
                        test_tx_gain += BandToString(bands[i]) + ",";
                        lstDebug.Items.Insert(0, "TX Gain Test - " + BandToString(bands[i]) + ": Failed ("
                            + gain[2].ToString("f1") + ", " + (gain[2] - target[i]).ToString("f1") + ")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "TX Gain Test - " + BandToString(bands[i]) + ": Passed ("
                            + gain[2].ToString("f1") + ", " + (gain[2] - target[i]).ToString("f1") + ")");
                    }

                    //Debug.WriteLine(gain.ToString("f1"));
                }
            }

        end:
            if (test_tx_gain.StartsWith("TX Gain Test: Failed ("))
                test_tx_gain = test_tx_gain.Substring(0, test_tx_gain.Length - 1) + ")";
            toolTip1.SetToolTip(btnTXGain, test_tx_gain);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\tx_gain.csv");
            StreamWriter writer = new StreamWriter(path + "\\tx_gain.csv", true);
            if (!file_exists) writer.WriteLine("Model, Serial Num, Date/Time, Version"
                                  + "160m,"
                                  + "80m,"
                                  + "60m,"
                                  + "40m,"
                                  + "30m,"
                                  + "20m,"
                                  + "17m,"
                                  + "15m,"
                                  + "12m,"
                                  + "10m,"
                                  + "6m");
            writer.Write(console.CurrentModel.ToString() + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(gain_table[i].ToString("f1") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\TX Gain";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string model = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX3000: model = "F3K"; break;
                case Model.FLEX5000: model = "F5K"; break;
            }
            writer = new StreamWriter(path + "\\tx_gain_" + model + "_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv");
            writer.WriteLine("Band, Gain");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                writer.Write(gain_table[i].ToString("f1") + ",");
            }
            writer.Close();

            FWC.SetRX1Filter((float)vfoa);
            //Thread.Sleep(50);
            FWC.SetTXFilter((float)vfoa);
            //Thread.Sleep(50);

            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            console.FullDuplex = false;
            Audio.TXOutputSignal = Audio.SignalSource.RADIO;
            Audio.SourceScale = tone_scale;
            Audio.SineFreq1 = tone_freq;
            console.DisplayModeText = display;
            console.RX1DSPMode = dsp_mode;
            console.RX1FilterHigh = filt_high;
            console.RX1FilterLow = filt_low;
            console.RX1Filter = filter;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            console.RX1PreampMode = preamp;
            console.RITOn = rit_on;
            console.setupForm.DSPPhoneRXBuffer = dsp_buf_size;
            console.setupForm.Polyphase = polyphase;
            console.SpurReduction = spur_red;
            console.CurrentMeterRXMode = rx_meter;

            Invoke(new MethodInvoker(p.Hide));
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region 1500 PA

        private void btnTX1500PA_Click(object sender, EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnTX1500PA.BackColor = Color.Green;

            p = new Progress("Cal 1500 PA");
            Thread t = new Thread(new ThreadStart(Cal1500PA));
            t.Name = "Cal 1500 PA Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            Invoke(new MethodInvoker(p.Show));
        }

        string test_pa_power = "PA Power Cal: Passed";
        private void Cal1500PA()
        {
            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.RXOnly)
            {
                p.Text = "";
                p.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnTX1500PA.BackColor = Color.Red;
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                p.Text = "";
                p.Hide();
                MessageBox.Show("Invalid COM Port selected.  A valid COM Port connected to a PowerMaster is required.",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnTX1500PA.BackColor = Color.Red;
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                return;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            bool disable_ptt = console.DisablePTT;
            console.DisablePTT = true;

            bool leveler = console.dsp.GetDSPTX(0).TXLevelerOn;
            console.dsp.GetDSPTX(0).TXLevelerOn = false;

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text, ckPM2.Checked); // ke9ns add .212
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for PowerMaster",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                p.Hide();
                btnTX1500PA.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                p.Hide();
                btnTX1500PA.BackColor = Color.Red;
                return;
            }

            Band[] bands = { Band.B6M, Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M };
            float[] band_freqs = { 50.11f, 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f };
            float target = 5.1f; // 5W + some overhead

            float[] pm_trim = { 1.04f, 1.03f, 1.03f, 1.03f, 1.02f, 1.01f, 1.01f, 1.0f, 1.0f, 1.0f, 0.9f };
            try
            {
                string pm_file_path = Path.Combine(common_data_path, "powermaster.txt");
                StreamReader reader = new StreamReader(pm_file_path);
                string temp = reader.ReadLine();

                int start = temp.IndexOf(":") + 1;
                int length = temp.Length - start;
                lstDebug.Items.Insert(0, "PowerMaster S/N: " + temp.Substring(start, length).Trim());

                for (int i = 0; i < 11; i++)
                {
                    temp = reader.ReadLine();
                    start = temp.IndexOf(":") + 1;
                    length = temp.Length - start;
                    int index = i + 1;
                    if (index == 11) index = 0;
                    pm_trim[index] = 1.0f + 0.01f * float.Parse(temp.Substring(start, length).Trim());
                    lstDebug.Items.Insert(0, BandToString(bands[index]) + ": " + pm_trim[index].ToString("f2"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading Array Solutions Power Master calibration file.  Using defaults.");
            }

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            p.SetPercent(0.0f);

            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = num_bands * 28;

            for (int i = 0; i < band_freqs.Length; i++) // main loop
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    float saved_value = console.power_table[(int)bands[i]][0];
                    console.power_table[(int)bands[i]][0] = 0.0f;

                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    DSPMode dsp_mode = console.RX1DSPMode;
                    console.RX1DSPMode = DSPMode.USB;

                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    HIDAnt tx_ant = console.TXAnt1500;
                    console.TXAnt1500 = HIDAnt.PA;

                    int tx_filter_high = console.TXFilterHigh;
                    console.TXFilterHigh = 700;

                    int tx_filter_low = console.TXFilterLow;
                    console.TXFilterLow = 500;

                    bool cpdr = console.CPDR;
                    console.CPDR = false;

                    double freq1 = Audio.SineFreq1;
                    Audio.SineFreq1 = 600.0;

                    Audio.RadioVolume = 0.00;

                    for (int j = 0; j < 10; j++)
                    {
                        if (!p.Visible) goto end;
                        Thread.Sleep(50);
                    }

                    Audio.RadioVolume = 0.16; // need to check on this init value
                    double last_watts = 0.0;

                    int try_again_count = 0;
                    int zero_count = 0;
                    float tol = 0.1f;

                    console.MOX = true;
                    Audio.TXInputSignal = Audio.SignalSource.SINE;
                    Audio.SourceScale = 1.0;

                    float pow = 0.0f;

                    while (Math.Abs(target - pow) > tol)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (!p.Visible) goto end;
                            Thread.Sleep(50);
                        }

                        pow = pm.Watts * pm_trim[i];
                        Debug.WriteLine("pow: " + pow.ToString("f1"));

                        if (pow == 0.0f)
                        {
                            zero_count++;
                            if (zero_count > 2)
                            {
                                console.MOX = false;
                                //Thread.Sleep(50);
                                btnTX1500PA.BackColor = Color.Red;
                                if (!test_pa_power.StartsWith("PA Power Cal: Failed ("))
                                    test_pa_power = "PA Power Cal: Failed(";
                                test_pa_power += BandToString(bands[i]) + ",";
                                lstDebug.Items.Insert(0, "PA - " + BandToString(bands[i]) + ": Failed (" +
                                    Audio.RadioVolume.ToString("f3") + ", " + pow.ToString("f1") + ")");
                                MessageBox.Show("No power read.  If the radio is connected to the PowerMaster \n" +
                                    "coupler correctly, this may indicate a failed PA on this band (" + bands[i].ToString() + ").",
                                    "No Power Read",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                console.power_table[(int)bands[i]][0] = 0.0f;
                                break;
                            }
                            pow = 0.01f;
                        }

                        if (Math.Abs(target - pow) > tol) // not within tolerance yet
                        {
                            if ((pow < 5.0f && Audio.RadioVolume == 1.0) || try_again_count++ == 20) // this is as far as we need to go
                            {
                                float limit = 4.9f;
                                switch (bands[i])
                                {
                                    case Band.B40M:
                                        limit = 4.6f;
                                        break;
                                    default:
                                        limit = 4.9f;
                                        break;
                                }

                                if (pow >= limit) // we'll call this a pass
                                {
                                    lstDebug.Items.Insert(0, "PA - " + BandToString(bands[i]) + ": Passed (" +
                                        Audio.RadioVolume.ToString("f3") + ", " + pow.ToString("f1") + ")");
                                    console.power_table[(int)bands[i]][0] = 1.0f;
                                    break;
                                }
                                else // failure 
                                {
                                    btnTX1500PA.BackColor = Color.Red;
                                    if (!test_pa_power.StartsWith("PA Power Cal: Failed ("))
                                        test_pa_power = "PA Power Cal: Failed(";
                                    test_pa_power += BandToString(bands[i]) + ",";
                                    lstDebug.Items.Insert(0, "PA - " + BandToString(bands[i]) + ": Failed (" +
                                        Audio.RadioVolume.ToString("f3") + ", " + pow.ToString("f1") + ")");
                                    console.power_table[(int)bands[i]][0] = 1.0f;
                                    break;
                                }
                            }
                            else // keep going
                            {
                                lstDebug.Items.Insert(0, "PA - " + BandToString(bands[i]) + ": In Progress (" +
                                Audio.RadioVolume.ToString("f3") + ", " + pow.ToString("f1") + ")");

                                double new_val = Audio.RadioVolume * Math.Sqrt(target / pow);
                                if (new_val > Audio.RadioVolume * 2) new_val = Audio.RadioVolume * 2;
                                if (new_val < Audio.RadioVolume / 2) new_val = Audio.RadioVolume / 2;
                                if (new_val > 1.0) new_val = 1.0;
                                Audio.RadioVolume = new_val;
                            }

                            last_watts = pow;
                        }
                        else // passed
                        {
                            lstDebug.Items.Insert(0, "PA - " + BandToString(bands[i]) + ": Passed (" +
                                Audio.RadioVolume.ToString("f3") + ", " + pow.ToString("f1") + ")");
                        }

                        p.SetPercent(++counter / (float)total_counts);

                        console.power_table[(int)bands[i]][0] = (float)Math.Round(Audio.RadioVolume, 4);
                    }

                end:
                    if (!p.Visible)
                        console.power_table[(int)bands[i]][0] = saved_value;

                    console.MOX = false;
                    console.RX1DSPMode = dsp_mode;
                    Audio.SineFreq1 = freq1;
                    console.CPDR = cpdr;
                    console.TXFilterLow = tx_filter_low;
                    console.TXFilterHigh = tx_filter_high;
                    console.TXAnt1500 = tx_ant;

                    if (!p.Visible) break;
                }
            }

            //end2:
            p.Hide();
            console.MOX = false;
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            console.VFOAFreq = 0.590;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;

            Thread.Sleep(100);
            console.DisablePTT = disable_ptt;

            try
            {
                pm.Close();
            }
            catch (Exception) { }

            console.dsp.GetDSPTX(0).TXLevelerOn = leveler;

            test_pa_power = "PA Power Test: Passed";

            /*bool pwr_pass = true;
            if (!test_pa_power.StartsWith("PA Power Cal: Failed ("))
                pwr_pass = false;*/

            if (test_pa_power.StartsWith("PA Power Test: Failed ("))
                test_pa_power = test_pa_power.Substring(0, test_pa_power.Length - 2) + ")";
            toolTip1.SetToolTip(btnTX1500PA, test_pa_power);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\pa_power.csv");
            StreamWriter writer = new StreamWriter(path + "\\pa_power.csv", true);
            if (!file_exists) writer.WriteLine("Model, PA Serial Num, Date/Time, Version, "
                                  + "6m, 160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m,");
            writer.Write(console.CurrentModel.ToString() + ",");
            writer.Write(HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(console.power_table[(int)bands[i]][0].ToString("f4") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\PA Power";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            writer = new StreamWriter(path + "\\pa_power_" + HIDEEPROM.SerialToString(HIDEEPROM.SerialNumber) + ".csv");
            writer.WriteLine("Model, Date/Time, Version, 6m, 160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m,");
            writer.Write(console.CurrentModel.ToString() + ",");
            writer.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ",");
            writer.Write(console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(console.power_table[(int)bands[i]][0].ToString("f4") + ",");
                writer.WriteLine("");
            }
            writer.Close();

            if (p.Text != "")
            {
                lstDebug.Items.Insert(0, "Saving Power data to EEPROM...");
                byte checksum;
                HIDEEPROM.WritePAPower(console.power_table, out checksum);
                console.pa_power_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving Power data to EEPROM...done";
            }

            writer.Close();

            //progress.Hide();	
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #endregion

        #region Run Selected Tests

        private void btnRunSelectedTests_Click(object sender, System.EventArgs e)
        {
            console.PowerOn = true;
            btnRunSelectedTests.BackColor = console.ButtonSelectedColor;
            btnRunSelectedTests.Enabled = false;
            Thread t = new Thread(new ThreadStart(RunSelectedTests));
            t.Name = "Run All Tests Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void RunSelectedTests()
        {
            bool rx2 = console.RX2Enabled;
            console.RX2Enabled = false;

            console.VFOSplit = false;
            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            string start_bands = GetStringFromBands();

            #region Gen

            // run all general tests
            if (ckTestGenPLL.Checked)
            {
                Invoke(new MethodInvoker(btnPLL.PerformClick));
                Thread.Sleep(1000);
            }

            if (ckTestGenBal.Checked)
            {
                Invoke(new MethodInvoker(btnGenBal.PerformClick));
                Thread.Sleep(5000);
            }

            if (ckTestGenNoise.Checked)
            {
                Invoke(new MethodInvoker(btnNoise.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenImpulse.Checked)
            {
                Invoke(new MethodInvoker(btnImpulse.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenPreamp.Checked)
            {
                Invoke(new MethodInvoker(btnGenPreamp.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenATTN.Checked)
            {
                Invoke(new MethodInvoker(btnGenATTN.PerformClick));
                Thread.Sleep(5000);
            }

            // re-run any Gen tests that failed
            if (ckTestGenPLL.Checked && btnPLL.BackColor != Color.Green)
            {
                Invoke(new MethodInvoker(btnPLL.PerformClick));
                Thread.Sleep(1000);
            }

            if (ckTestGenBal.Checked && btnGenBal.BackColor != Color.Green)
            {
                Invoke(new MethodInvoker(btnGenBal.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenNoise.Checked && btnNoise.BackColor != Color.Green)
            {
                Invoke(new MethodInvoker(btnNoise.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenImpulse.Checked && btnImpulse.BackColor != Color.Green)
            {
                Invoke(new MethodInvoker(btnImpulse.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenPreamp.Checked && btnGenPreamp.BackColor != Color.Green)
            {
                Invoke(new MethodInvoker(btnGenPreamp.PerformClick));
                Thread.Sleep(3000);
            }

            if (ckTestGenATTN.Checked && btnGenATTN.BackColor != Color.Green)
            {
                Invoke(new MethodInvoker(btnGenATTN.PerformClick));
                Thread.Sleep(3000);
            }

            if ((ckTestGenPLL.Checked && btnPLL.BackColor != Color.Green) ||
                (ckTestGenBal.Checked && btnGenBal.BackColor != Color.Green) ||
                (ckTestGenPreamp.Checked && btnGenPreamp.BackColor != Color.Green) ||
                (ckTestGenATTN.Checked && btnGenATTN.BackColor != Color.Green))
                goto end;

            #endregion

            #region RX

            // run all RX tests
            if (ckTestRXFilter.Checked)
            {
                Invoke(new MethodInvoker(btnRXFilter.PerformClick));
                while (true)
                {
                    while (p.Visible) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (!p.Visible) break;
                }
                if (p.Text == "") goto end;
            }

            // re-run any RX tests that failed
            if (ckTestRXFilter.Checked && btnRXFilter.BackColor != Color.Green)
            {
                if (console.CurrentModel == Model.FLEX1500)
                    SetBandFrom1500FilterString(toolTip1.GetToolTip(btnRXFilter));
                else
                    SetBandFromString(toolTip1.GetToolTip(btnRXFilter));
                Invoke(new MethodInvoker(btnRXFilter.PerformClick));
                while (true)
                {
                    while (p.Visible) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (!p.Visible) break;
                }
                if (p.Text == "") goto end;

                // reset bands back to start
                SetBandFromString(start_bands);
            }

            if (ckTestRXImage.Checked)
            {
                Invoke(new MethodInvoker(btnRXImage.PerformClick));
                while (true)
                {
                    while (!btnRXImage.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnRXImage.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestRXImage.Checked && btnRXImage.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnRXImage));
                Invoke(new MethodInvoker(btnRXImage.PerformClick));
                while (true)
                {
                    while (!btnRXImage.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnRXImage.Enabled) break;
                }
                if (p.Text == "") goto end;

                // reset bands back to start
                SetBandFromString(start_bands);
            }

            if (ckTestRXLevel.Checked)
            {
                Invoke(new MethodInvoker(btnRXLevel.PerformClick));
                while (true)
                {
                    while (!btnRXLevel.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnRXLevel.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestRXLevel.Checked && btnRXLevel.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnRXLevel));
                Invoke(new MethodInvoker(btnRXLevel.PerformClick));
                while (true)
                {
                    while (!btnRXLevel.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnRXLevel.Enabled) break;
                }
                if (p.Text == "") goto end;

                // reset bands back to start
                SetBandFromString(start_bands);
            }

            /*if((ckTestRXFilter.Checked && btnRXFilter.BackColor != Color.Green) ||
                (ckTestRXLevel.Checked && btnRXLevel.BackColor != Color.Green) ||
                (ckTestRXImage.Checked && btnRXImage.BackColor != Color.Green) ||
                (ckTestRXMDS.Checked && btnRXMDS.BackColor != Color.Green))
                goto end;*/

            #endregion

            #region TX

            // run all TX tests
            if (ckTestTXFilter.Checked)
            {
                Invoke(new MethodInvoker(btnTXFilter.PerformClick));
                while (true)
                {
                    while (p.Visible) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (!p.Visible) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXCarrier.Checked)
            {
                Invoke(new MethodInvoker(btnTXCarrier.PerformClick));
                while (true)
                {
                    while (!btnTXCarrier.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTXCarrier.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXImage.Checked)
            {
                Invoke(new MethodInvoker(btnTXImage.PerformClick));
                while (true)
                {
                    while (!btnTXImage.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTXImage.Enabled) break;
                }
                if (p.Text == "") goto end;
            }


            if (ckTestTXGain.Checked)
            {
                Invoke(new MethodInvoker(btnTXGain.PerformClick));
                while (true)
                {
                    while (!btnTXGain.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTXGain.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXPA.Checked)
            {
                Invoke(new MethodInvoker(btnTX1500PA.PerformClick));
                while (true)
                {
                    while (!btnTX1500PA.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTX1500PA.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            // re-run any TX tests that failed
            if (ckTestTXFilter.Checked && btnTXFilter.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnTXFilter));
                Invoke(new MethodInvoker(btnTXFilter.PerformClick));
                while (true)
                {
                    while (p.Visible) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (!p.Visible) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXImage.Checked && btnTXImage.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnTXImage));
                Invoke(new MethodInvoker(btnTXImage.PerformClick));
                while (true)
                {
                    while (!btnTXImage.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTXImage.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXCarrier.Checked && btnTXCarrier.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnTXCarrier));
                Invoke(new MethodInvoker(btnTXCarrier.PerformClick));
                while (true)
                {
                    while (!btnTXCarrier.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTXCarrier.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXGain.Checked && btnTXGain.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnTXGain));
                Invoke(new MethodInvoker(btnTXGain.PerformClick));
                while (true)
                {
                    while (!btnTXGain.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTXGain.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

            if (ckTestTXPA.Checked && btnTX1500PA.BackColor != Color.Green)
            {
                SetBandFromString(toolTip1.GetToolTip(btnTX1500PA));
                Invoke(new MethodInvoker(btnTX1500PA.PerformClick));
                while (true)
                {
                    while (!btnTX1500PA.Enabled) Thread.Sleep(2000);
                    Thread.Sleep(2000);
                    if (btnTX1500PA.Enabled) break;
                }
                if (p.Text == "") goto end;
            }

        #endregion

        end:
            SetBandFromString(start_bands);
            btnRunSelectedTests.BackColor = SystemColors.Control;
            btnRunSelectedTests.Enabled = true;
            console.RX2Enabled = rx2;

            t1.Stop();
            MessageBox.Show("Run All Tests Time: " + ((int)(t1.Duration / 60)).ToString() + ":" + (((int)t1.Duration) % 60).ToString("00"));
        }

        #endregion

        #region IO Tests

        #region Pwr Spkr

        private void btnIOPwrSpkr_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOPwrSpkr.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOPwrSpkr));
            t.Name = "IO Pwr Spkr Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_pwrspkr = "IO Pwr Spkr Test: Not Run";
        private void CheckIOPwrSpkr()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                grpIO.Enabled = true;
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                btnIOPwrSpkr.BackColor = Color.Red;
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.MOX = false;

            console.FullDuplex = true;

            double scale = Audio.SourceScale;
            Audio.SourceScale = 0.1;

            double monitor = Audio.MonitorVolume;
            Audio.MonitorVolume = 1.0;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            int in_tx_l = Audio.IN_TX_L;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    Audio.IN_TX_L = 6;
                    break;
                case Model.FLEX3000:
                    Audio.IN_TX_L = 3;
                    break;
            }

            byte reg_11 = 0;
            FWC.ReadCodecReg(0x11, out reg_11);
            FWC.WriteCodecReg(0x11, 0x80);

            byte reg_12 = 0;
            FWC.ReadCodecReg(0x12, out reg_12);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.WriteCodecReg(0x12, 0x80);
                    break;
                case Model.FLEX3000:
                    FWC.WriteCodecReg(0x12, 0x30); // set input side of mixer
                    break;
            }

            byte reg_13 = 0;
            FWC.ReadCodecReg(0x13, out reg_13);
            FWC.WriteCodecReg(0x13, 0x80);

            byte reg_14 = 0;
            FWC.ReadCodecReg(0x14, out reg_14);
            FWC.WriteCodecReg(0x14, 0x80);

            byte reg_15 = 0;
            FWC.ReadCodecReg(0x15, out reg_15);

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.WriteCodecReg(0x15, 0x30); // set input side of mixer
                    break;
                case Model.FLEX3000:
                    FWC.WriteCodecReg(0x15, 0x80);
                    break;
            }

            byte reg_16 = 0;
            FWC.ReadCodecReg(0x16, out reg_16);
            FWC.WriteCodecReg(0x16, 0x80);

            byte reg_7 = 0;
            FWC.ReadCodecReg(7, out reg_7);
            FWC.WriteCodecReg(7, 0xCF);

            byte reg_CD = 0;
            FWC.ReadCodecReg(0x0C, out reg_CD);
            FWC.WriteCodecReg(0x0C, 0);
            FWC.WriteCodecReg(0x0D, 0);
            Audio.RX1OutputSignal = Audio.SignalSource.SILENCE;
            Thread.Sleep(500);

            float sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float off = sum / 5;

            float on = 0.0f, unbal = 0.0f, left = 0.0f, right = 0.0f;
            bool b = true;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    Audio.RX1OutputSignal = Audio.SignalSource.SINE;
                    Thread.Sleep(500);
                    sum = 0.0f;
                    DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                    Thread.Sleep(50);
                    for (int j = 0; j < 5; j++)
                    {
                        sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                        Thread.Sleep(50);
                    }
                    on = sum / 5;

                    Audio.RX1OutputSignal = Audio.SignalSource.SAWTOOTH;
                    Thread.Sleep(500);

                    sum = 0.0f;
                    DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                    Thread.Sleep(50);
                    for (int j = 0; j < 5; j++)
                    {
                        sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                        Thread.Sleep(50);
                    }
                    unbal = sum / 5;

                    if (on - off < 50.0f) b = false;
                    if (on - unbal < 20.0f) b = false;
                    break;
                case Model.FLEX3000:
                    Audio.RX1OutputSignal = Audio.SignalSource.SINE_LEFT_ONLY;
                    Thread.Sleep(500);
                    sum = 0.0f;
                    DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                    Thread.Sleep(50);
                    for (int j = 0; j < 5; j++)
                    {
                        sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                        Thread.Sleep(50);
                    }
                    left = sum / 5;

                    Audio.RX1OutputSignal = Audio.SignalSource.SINE_RIGHT_ONLY;
                    Thread.Sleep(500);
                    sum = 0.0f;
                    DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                    Thread.Sleep(50);
                    for (int j = 0; j < 5; j++)
                    {
                        sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                        Thread.Sleep(50);
                    }
                    right = sum / 5;

                    if (right - off < 50.0f) b = false;
                    if (left - off < 50.0f) b = false;

                    on = left; unbal = right;
                    break;
            }

            if (b)
            {
                btnIOPwrSpkr.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Pwr Spkr - Passed: (" + off.ToString("f1") + ", " + on.ToString("f1") + ", " + unbal.ToString("f1") + ")");
                test_io_pwrspkr = "IO Pwr Spkr Test: Passed";
            }
            else
            {
                btnIOPwrSpkr.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Pwr Spkr - Failed: (" + off.ToString("f1") + ", " + on.ToString("f1") + ", " + unbal.ToString("f1") + ")");
                test_io_pwrspkr = "IO Pwr Spkr Test: Failed";
            }
            toolTip1.SetToolTip(btnIOPwrSpkr, test_io_pwrspkr);

            // end
            Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
            Audio.IN_TX_L = in_tx_l;
            FWC.WriteCodecReg(0x11, reg_11);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x12, reg_12);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x13, reg_13);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x14, reg_14);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x15, reg_15);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x16, reg_16);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(7, reg_7);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0C, reg_CD);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0D, reg_CD);
            //Thread.Sleep(50);
            Audio.SourceScale = scale;
            console.FullDuplex = false;
            console.RX1DSPMode = dsp_mode;
            Audio.MonitorVolume = monitor;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_pwrspkr.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_pwrspkr.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Off, Out Of Phase, In Phase, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(off.ToString("f1") + ",");
            writer.Write(on.ToString("f1") + ",");
            writer.Write(unbal.ToString("f1") + ",");
            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region RCA In/Out

        private void btnIORCAInOut_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIORCAInOut.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIORCAInOut));
            t.Name = "IO RCA In/Out Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_rcainout = "IO RCA In/Out Test: Not Run";
        private void CheckIORCAInOut()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                grpIO.Enabled = true;
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                btnIORCAInOut.BackColor = Color.Red;
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.MOX = false;

            bool full_duplex = console.FullDuplex;
            console.FullDuplex = true;

            double scale = Audio.SourceScale;
            Audio.SourceScale = 0.1;

            double monitor = Audio.MonitorVolume;
            Audio.MonitorVolume = 1.0;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            int in_tx_l = Audio.IN_TX_L;
            Audio.IN_TX_L = 5;

            byte reg_11 = 0;
            FWC.ReadCodecReg(0x11, out reg_11);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x11, 0x80);
            //Thread.Sleep(50);

            byte reg_12 = 0;
            FWC.ReadCodecReg(0x12, out reg_12);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x12, 0x80);
            //Thread.Sleep(50);

            byte reg_13 = 0;
            FWC.ReadCodecReg(0x13, out reg_13);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x13, 0x80);
            //Thread.Sleep(50);

            byte reg_14 = 0; // set input side of mixer
            FWC.ReadCodecReg(0x14, out reg_14);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x14, 0x30);
            //Thread.Sleep(50);

            byte reg_15 = 0;
            FWC.ReadCodecReg(0x15, out reg_15);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x15, 0x80);
            //Thread.Sleep(50);

            byte reg_16 = 0;
            FWC.ReadCodecReg(0x16, out reg_16);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x16, 0x80);
            //Thread.Sleep(50);

            byte old_codec_reg_7 = 0;
            FWC.ReadCodecReg(7, out old_codec_reg_7);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(7, 0xBF);
            //Thread.Sleep(50);

            byte old_codec_reg_0E = 0;
            FWC.ReadCodecReg(0x0E, out old_codec_reg_0E);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0E, 0);
            //Thread.Sleep(50);
            Audio.RX1OutputSignal = Audio.SignalSource.SILENCE;
            Thread.Sleep(500);

            float sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float off = sum / 5;

            Audio.RX1OutputSignal = Audio.SignalSource.SINE;
            Thread.Sleep(500);

            sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float on = sum / 5;


            bool b = true;
            if (on - off < 50.0f) b = false;

            if (b)
            {
                btnIORCAInOut.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO RCA In/Out: Passed (" + on.ToString("f1") + ", " + off.ToString("f1") + ")");
                test_io_rcainout = "IO RCA In/Out Test: Passed";
            }
            else
            {
                btnIORCAInOut.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO RCA In/Out: Failed (" + on.ToString("f1") + ", " + off.ToString("f1") + ")");
                test_io_rcainout = "IO RCA In/Out Test: Failed";
            }
            toolTip1.SetToolTip(btnIORCAInOut, test_io_rcainout);

            // end
            Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
            Audio.IN_TX_L = in_tx_l;
            FWC.WriteCodecReg(0x11, reg_11);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x12, reg_12);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x13, reg_13);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x14, reg_14);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x15, reg_15);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x16, reg_16);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(7, old_codec_reg_7);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0E, old_codec_reg_0E);
            //Thread.Sleep(50);
            Audio.SourceScale = scale;
            console.FullDuplex = false;
            console.RX1DSPMode = dsp_mode;
            Audio.MonitorVolume = monitor;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_rcainout.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_rcainout.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Off, On, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(off.ToString("f1") + ",");
            writer.Write(on.ToString("f1") + ",");
            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region FlexWire In/Out

        private void btnIOFWInOut_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOFWInOut.BackColor = console.ButtonSelectedColor;
            Thread t;
            if (console.hid_init && console.CurrentModel == Model.FLEX1500)
                t = new Thread(new ThreadStart(CheckIOFWInOut1500));
            else t = new Thread(new ThreadStart(CheckIOFWInOut));
            t.Name = "IO FW In/Out Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_fwinout = "FlexWire In/Out Test: Not Run";
        private void CheckIOFWInOut()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                grpIO.Enabled = true;
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                btnIOFWInOut.BackColor = Color.Red;
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.MOX = false;

            console.FullDuplex = true;

            double scale = Audio.SourceScale;
            Audio.SourceScale = 0.1;

            double monitor = Audio.MonitorVolume;
            Audio.MonitorVolume = 1.0;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            int in_tx_l = Audio.IN_TX_L;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    Audio.IN_TX_L = 4;
                    break;
                case Model.FLEX3000:
                    Audio.IN_TX_L = 2;
                    break;
            }

            byte reg_11 = 0;
            FWC.ReadCodecReg(0x11, out reg_11);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.WriteCodecReg(0x11, 0x80);
                    break;
                case Model.FLEX3000:
                    FWC.WriteCodecReg(0x11, 0x30); // set input side of mixer
                    break;
            }

            byte reg_12 = 0;
            FWC.ReadCodecReg(0x12, out reg_12);
            FWC.WriteCodecReg(0x12, 0x80);

            byte reg_13 = 0;
            FWC.ReadCodecReg(0x13, out reg_13);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.WriteCodecReg(0x13, 0x30); // set input side of mixer
                    break;
                case Model.FLEX3000:
                    FWC.WriteCodecReg(0x13, 0x80);
                    break;
            }


            byte reg_14 = 0;
            FWC.ReadCodecReg(0x14, out reg_14);
            FWC.WriteCodecReg(0x14, 0x80);

            byte reg_15 = 0;
            FWC.ReadCodecReg(0x15, out reg_15);
            FWC.WriteCodecReg(0x15, 0x80);

            byte reg_16 = 0;
            FWC.ReadCodecReg(0x16, out reg_16);
            FWC.WriteCodecReg(0x16, 0x80);

            byte reg_7 = 0;
            FWC.ReadCodecReg(7, out reg_7);
            FWC.WriteCodecReg(7, 0xBF);

            byte reg_0E = 0;
            FWC.ReadCodecReg(0x0E, out reg_0E);
            FWC.WriteCodecReg(0x0E, 0);

            Audio.RX1OutputSignal = Audio.SignalSource.SILENCE;
            Thread.Sleep(500);

            float sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float off = sum / 5;

            Audio.RX1OutputSignal = Audio.SignalSource.SINE;
            Thread.Sleep(500);

            sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float on = sum / 5;

            bool b = true;
            if (on - off < 50.0f) b = false;

            if (b)
            {
                btnIOFWInOut.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO FW In/Out: Passed (" + on.ToString("f1") + ", " + off.ToString("f1") + ")");
                test_io_fwinout = "FlexWire In/Out Test: Passed";
            }
            else
            {
                btnIOFWInOut.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO FW In/Out: Failed (" + on.ToString("f1") + ", " + off.ToString("f1") + ")");
                test_io_fwinout = "FlexWire In/Out Test: Failed";
            }
            toolTip1.SetToolTip(btnIOFWInOut, test_io_fwinout);

            // end
            Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
            Audio.IN_TX_L = in_tx_l;
            FWC.WriteCodecReg(0x11, reg_11);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x12, reg_12);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x13, reg_13);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x14, reg_14);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x15, reg_15);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x16, reg_16);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(7, reg_7);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0E, reg_0E);
            //Thread.Sleep(50);
            Audio.SourceScale = scale;
            console.FullDuplex = false;
            console.RX1DSPMode = dsp_mode;
            Audio.MonitorVolume = monitor;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_fwinout.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_fwinout.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Off, On, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(off.ToString("f1") + ",");
            writer.Write(on.ToString("f1") + ",");
            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        private void KickCodec()
        {
            Thread.Sleep(10);
            byte b1;
            USBHID.ReadI2CValue(0x30, 0x63, out b1);
        }

        private void CheckIOFWInOut1500()
        {
            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            bool disable_ptt = console.DisablePTT;
            console.DisablePTT = true;

            double scale = Audio.SourceScale;
            Audio.SourceScale = 0.25;

            DttSP.SetCorrectIQEnable(0); // turn off I/Q correction
            DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 0);
            DttSP.SetCorrectRXIQw(0, 0, 0.0f, 0.0f, 1);

            double freq = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            console.MOX = true; Thread.Sleep(20);
            Audio.TXOutputSignal = Audio.SignalSource.SINE;

            // input to FlexWire In, Output to phones
            // setup input side of codec
            USBHID.WriteI2C2Value(0x30, 0x0F, 20); Thread.Sleep(20); // Left ADC PGA Gain Control Register
            USBHID.WriteI2C2Value(0x30, 0x14, 0x00); Thread.Sleep(20); // LINE2L to Left(1) ADC Control Register
            USBHID.WriteI2C2Value(0x30, 0x11, 0xFF); Thread.Sleep(20); // MIC3L/R to Left ADC Control Register

            // setup output side of codec
            USBHID.WriteI2C2Value(0x30, 0x52, 0xB1); Thread.Sleep(20);
            USBHID.WriteI2C2Value(0x30, 0x5C, 0xB1); Thread.Sleep(20);
            USBHID.WriteI2C2Value(0x30, 0x4B, 0xB1); Thread.Sleep(20);

            USBHID.WriteI2C2Value(0x30, 0x56, 0x89); Thread.Sleep(20); // LEFT_LOP/M Output Level Control Register
            USBHID.WriteI2C2Value(0x30, 0x5D, 0x89); Thread.Sleep(20); // RIGHT_LOP/M Output Level Control Register
            USBHID.WriteI2C2Value(0x30, 0x4F, 0x81); Thread.Sleep(20); // MONO_LOP/M Output Level Control Register
            //USBHID.WriteI2C2Value(0x30, 0x63, 0x48); Thread.Sleep(20); // GPIO2 Control Register

            KickCodec();
            Thread.Sleep(500);

            Audio.ResetMinMax();
            Thread.Sleep(100);

            float on = Audio.MaxInL - Audio.MinInL;
            //Debug.WriteLine("on: " + on.ToString("f1"));

            // turn off Phones output
            USBHID.WriteI2C2Value(0x30, 0x56, 0x81); Thread.Sleep(20); // LEFT_LOP/M Output Level Control Register
            USBHID.WriteI2C2Value(0x30, 0x5D, 0x81); Thread.Sleep(20); // RIGHT_LOP/M Output Level Control Register
            KickCodec();
            Thread.Sleep(500);

            Audio.ResetMinMax();
            Thread.Sleep(100);

            float off = Audio.MaxInL - Audio.MinInL;
            //Debug.WriteLine("off: " + off.ToString("f1"));

            bool b = (20 * Math.Log10(on / off) >= 30.0f);
            if (b)
            {
                btnIOFWInOut.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO FW In: Passed (" + (20 * Math.Log10(on / off)).ToString("f1") + ")");
                test_io_fwinout = "FlexWire In Test: Passed";
            }
            else
            {
                btnIOFWInOut.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO FW In: Failed (" + (20 * Math.Log10(on / off)).ToString("f1") + ")");
                test_io_fwinout = "FlexWire In Test: Failed";
            }
            toolTip1.SetToolTip(btnIOFWInOut, test_io_fwinout);

            // input to Mic, Output to FlexWire Out
            // setup input side of codec
            USBHID.WriteI2C2Value(0x30, 0x14, 0x78); Thread.Sleep(20); // LINE2L to Left(1) ADC Control Register
            USBHID.WriteI2C2Value(0x30, 0x11, 0x0F); Thread.Sleep(20); // MIC3L/R to Left ADC Control Register

            // setup output side of codec
            USBHID.WriteI2C2Value(0x30, 0x4F, 0x89); Thread.Sleep(20); // LEFT_LOP/M Output Level Control Register
            USBHID.WriteI2C2Value(0x30, 0x63, 0x48); Thread.Sleep(20); // GPIO2 Control Register

            KickCodec();
            Thread.Sleep(500);

            Audio.ResetMinMax();
            Thread.Sleep(100);

            on = Audio.MaxInL - Audio.MinInL;
            //Debug.WriteLine("on: " + on.ToString("f1"));

            USBHID.WriteI2C2Value(0x30, 0x4F, 0x81); Thread.Sleep(20); // LEFT_LOP/M Output Level Control Register
            Thread.Sleep(500);

            Audio.ResetMinMax();
            Thread.Sleep(100);

            off = Audio.MaxInL - Audio.MinInL;

            bool b2 = (20 * Math.Log10(on / off) >= 30.0f);

            if (b2)
            {
                if (b) btnIOFWInOut.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO FW Out: Passed (" + (20 * Math.Log10(on / off)).ToString("f1") + ")");
                test_io_fwinout += "\nFlexWire Out Test: Passed";
            }
            else
            {
                btnIOFWInOut.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO FW Out: Failed (" + (20 * Math.Log10(on / off)).ToString("f1") + ")");
                test_io_fwinout += "\nFlexWire Out Test: Failed";
            }
            toolTip1.SetToolTip(btnIOFWInOut, test_io_fwinout);

            // end
            Audio.TXOutputSignal = Audio.SignalSource.RADIO;
            console.MOX = false; // this should reset all of the codec registers
            Audio.SourceScale = scale;
            console.RX1DSPMode = dsp_mode;
            console.VFOAFreq = freq;

            Thread.Sleep(100);
            console.DisablePTT = disable_ptt;

            DttSP.SetCorrectIQEnable(1); // turn on I/Q correction

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_fwinout.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_fwinout.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Off, On, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(off.ToString("f1") + ",");
            writer.Write(on.ToString("f1") + ",");
            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Headphones

        private void btnIOHeadphone_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOHeadphone.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOHeadphone));
            t.Name = "IO Headphone Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_headphone = "IO Headphone Test: Not Run";
        private void CheckIOHeadphone()
        {
            /*if(!console.PowerOn)
            {
                MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                grpIO.Enabled = true;
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                btnIOFWInOut.BackColor = Color.Red;
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            console.MOX = false;

            console.FullDuplex = true;

            double scale = Audio.SourceScale;
            Audio.SourceScale = 0.1;

            double monitor = Audio.MonitorVolume;
            Audio.MonitorVolume = 1.0;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            int in_tx_l = Audio.IN_TX_L;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    Audio.IN_TX_L = 7;
                    break;
                case Model.FLEX3000:
                    Audio.IN_TX_L = 3;
                    break;
            }

            byte reg_11 = 0;
            FWC.ReadCodecReg(0x11, out reg_11);
            FWC.WriteCodecReg(0x11, 0x80);

            byte reg_12 = 0;
            FWC.ReadCodecReg(0x12, out reg_12);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.WriteCodecReg(0x12, 0x80);
                    break;
                case Model.FLEX3000:
                    FWC.WriteCodecReg(0x12, 0x30); // set input side of mixer
                    break;
            }

            byte reg_13 = 0;
            FWC.ReadCodecReg(0x13, out reg_13);
            FWC.WriteCodecReg(0x13, 0x80);

            byte reg_14 = 0;
            FWC.ReadCodecReg(0x14, out reg_14);
            FWC.WriteCodecReg(0x14, 0x80);

            byte reg_15 = 0;
            FWC.ReadCodecReg(0x15, out reg_15);
            FWC.WriteCodecReg(0x15, 0x80);

            byte reg_16 = 0;
            FWC.ReadCodecReg(0x16, out reg_16);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.WriteCodecReg(0x16, 0x30); // set input side of mixer
                    break;
                case Model.FLEX3000:
                    FWC.WriteCodecReg(0x16, 0x80);
                    break;
            }

            byte reg_7 = 0;
            FWC.ReadCodecReg(7, out reg_7);
            FWC.WriteCodecReg(7, 0xF3);

            byte reg_AB = 0;
            FWC.ReadCodecReg(0x0A, out reg_AB);
            FWC.WriteCodecReg(0x0A, 0);
            FWC.WriteCodecReg(0x0B, 0);

            byte reg_C = 0;
            FWC.ReadCodecReg(0x0C, out reg_C);
            FWC.WriteCodecReg(0x0C, 0);

            byte reg_D = 0;
            FWC.ReadCodecReg(0x0D, out reg_D);
            FWC.WriteCodecReg(0x0D, 0);

            Audio.RX1OutputSignal = Audio.SignalSource.SILENCE;
            Thread.Sleep(1000);

            float sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float off = sum / 5;

            Audio.RX1OutputSignal = Audio.SignalSource.SINE_LEFT_ONLY; // check left
            Thread.Sleep(1000);

            sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float left = sum / 5;

            Audio.RX1OutputSignal = Audio.SignalSource.SINE_RIGHT_ONLY; // check right
            Thread.Sleep(1000);

            sum = 0.0f;
            DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += -DttSP.CalculateTXMeter(1, DttSP.MeterType.MIC);
                Thread.Sleep(50);
            }
            float right = sum / 5;

            bool b = true;
            if (left - off < 40.0f) b = false;
            if (right - off < 40.0f) b = false;

            if (b)
            {
                btnIOHeadphone.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Headphone: Passed (" + off.ToString("f1") + ", " + left.ToString("f1") + ", " + right.ToString("f1") + ")");
                test_io_headphone = "IO Headphone Test: Passed";
            }
            else
            {
                btnIOHeadphone.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Headphone: Failed (" + off.ToString("f1") + ", " + left.ToString("f1") + ", " + right.ToString("f1") + ")");
                test_io_headphone = "IO Headphone Test: Failed";
            }
            toolTip1.SetToolTip(btnIOHeadphone, test_io_headphone);

            // end
            Audio.RX1OutputSignal = Audio.SignalSource.RADIO;
            Audio.IN_TX_L = in_tx_l;
            FWC.WriteCodecReg(0x11, reg_11);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x12, reg_12);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x13, reg_13);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x14, reg_14);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x15, reg_15);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x16, reg_16);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(7, reg_7);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0A, reg_AB);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0B, reg_AB);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0C, reg_C);
            //Thread.Sleep(50);
            FWC.WriteCodecReg(0x0D, reg_D);
            //Thread.Sleep(50);
            Audio.SourceScale = scale;
            console.FullDuplex = false;
            console.RX1DSPMode = dsp_mode;
            Audio.MonitorVolume = monitor;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_headphone.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_headphone.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Off, Left, Right, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(off.ToString("f1") + ",");
            writer.Write(left.ToString("f1") + ",");
            writer.Write(right.ToString("f1") + ",");
            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Dot

        private void btnIODot_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIODot.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIODot));
            t.Name = "IO Dot Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_dot = "IO Dot Test: Not Run";
        private void CheckIODot()
        {
            bool retval = true;
            bool power = console.PowerOn;

            bool disable_ptt = console.DisablePTT;
            if (console.CurrentModel == Model.FLEX1500)
            {
                console.DisablePTT = true;
            }
            else
            {
                console.PowerOn = false;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000: FWC.SetAmpTX1(false); break;
                case Model.FLEX1500: USBHID.SetTXOut(false); break;
            }
            Thread.Sleep(100);

            bool dot = false;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReadDot(out dot);
                    break;
                case Model.FLEX1500:
                    uint val;
                    USBHID.ReadPTT(out val);
                    dot = ((val & 0x20) == 0);
                    break;
            }
            Thread.Sleep(50);
            if (dot)
            {
                retval = false;
                goto end;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.SetRCATX1(true);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(true);
                    break;
            }
            Thread.Sleep(100);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReadDot(out dot);
                    break;
                case Model.FLEX1500:
                    uint val;
                    USBHID.ReadPTT(out val);
                    dot = ((val & 0x20) == 0);
                    break;
            }
            Thread.Sleep(50);
            if (!dot)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.SetRCATX1(false);
                    FWC.SetAmpTX1(true);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(false);
                    break;
            }
            if (power)
                console.PowerOn = true;
            Thread.Sleep(50);

            if (console.CurrentModel == Model.FLEX1500)
                console.DisablePTT = disable_ptt;

            if (retval)
            {
                btnIODot.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Dot: Passed");
                test_io_dot = "IO Dot Test: Passed";
            }
            else
            {
                btnIODot.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Dot: Failed");
                test_io_dot = "IO Dot Test: Failed";
            }
            toolTip1.SetToolTip(btnIODot, test_io_dot);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_dot.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_dot.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial);
                    break;
            }
            writer.Write(serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Dash

        private void btnIODash_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIODash.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIODash));
            t.Name = "IO Dash Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_dash = "IO Dash Test: Not Run";
        private void CheckIODash()
        {
            bool retval = true;
            bool power = console.PowerOn;

            bool disable_ptt = console.DisablePTT;
            if (console.CurrentModel == Model.FLEX1500)
            {
                console.DisablePTT = true;
            }
            else
            {
                console.PowerOn = false;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetAmpTX2(false);
                    break;
                case Model.FLEX3000:
                    FWC.SetAmpTX1(false);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(false);
                    break;
            }
            Thread.Sleep(100);

            bool dash = false;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReadDash(out dash);
                    Thread.Sleep(50);
                    break;
                case Model.FLEX1500:
                    uint val;
                    USBHID.ReadPTT(out val);
                    dash = ((val & 0x10) == 0);
                    break;
            }

            if (dash)
            {
                retval = false;
                goto end;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetRCATX2(true);
                    break;
                case Model.FLEX3000:
                    FWC.SetRCATX1(true);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(true);
                    break;
            }
            Thread.Sleep(100);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReadDash(out dash);
                    Thread.Sleep(50);
                    break;
                case Model.FLEX1500:
                    uint val;
                    USBHID.ReadPTT(out val);
                    dash = ((val & 0x10) == 0);
                    break;
            }

            if (!dash)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetRCATX2(false);
                    FWC.SetAmpTX2(true);
                    break;
                case Model.FLEX3000:
                    FWC.SetRCATX1(false);
                    FWC.SetAmpTX1(true);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(false);
                    break;
            }
            if (power) console.PowerOn = true;
            Thread.Sleep(50);

            if (console.CurrentModel == Model.FLEX1500)
                console.DisablePTT = disable_ptt;

            if (retval)
            {
                btnIODash.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Dash: Passed");
                test_io_dash = "IO Dash Test: Passed";
            }
            else
            {
                btnIODash.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Dash: Failed");
                test_io_dash = "IO Dash Test: Failed";
            }
            toolTip1.SetToolTip(btnIODash, test_io_dash);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_dash.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_dash.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial);
                    break;
            }
            writer.Write(serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region RCA PTT

        private void btnIORCAPTT_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIORCAPTT.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIORCAPTT));
            t.Name = "IO RCA PTT Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_rcaptt = "IO RCA PTT Test: Not Run";
        private void CheckIORCAPTT()
        {
            bool retval = true;
            //FWC.SetMOX(true);
            //Thread.Sleep(50);
            //FWC.SetPABias(false);
            //Thread.Sleep(50);
            bool power = console.PowerOn;
            if (power) console.PowerOn = false;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetAmpTX3(false);
                    break;
                case Model.FLEX3000:
                    FWC.SetAmpTX1(false);
                    break;
            }
            Thread.Sleep(100);

            bool ptt;
            FWC.ReadRCAPTT(out ptt);
            Thread.Sleep(50);
            if (ptt)
            {
                retval = false;
                goto end;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetRCATX3(true);
                    break;
                case Model.FLEX3000:
                    FWC.SetRCATX1(true);
                    break;
            }
            Thread.Sleep(100);
            FWC.ReadRCAPTT(out ptt);
            Thread.Sleep(50);
            if (!ptt)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetRCATX3(false);
                    FWC.SetAmpTX3(true);
                    break;
                case Model.FLEX3000:
                    FWC.SetRCATX1(false);
                    FWC.SetAmpTX1(true);
                    break;
            }
            if (power) console.PowerOn = true;
            Thread.Sleep(50);

            if (retval)
            {
                btnIORCAPTT.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO RCA PTT: Passed");
                test_io_rcaptt = "IO RCA PTT Test: Passed";
            }
            else
            {
                btnIORCAPTT.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO RCA PTT: Failed");
                test_io_rcaptt = "IO RCA PTT Test: Failed";
            }
            toolTip1.SetToolTip(btnIORCAPTT, test_io_rcaptt);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_rcaptt.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_rcaptt.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region FW PTT

        private void btnIOFWPTT_Click(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX1500) return;
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOFWPTT.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOFWPTT));
            t.Name = "IO FW PTT Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_fwptt = "IO FW PTT Test: Not Run";
        private void CheckIOFWPTT()
        {
            if (console.CurrentModel != Model.FLEX1500) return;
            bool retval = true;
            bool power = console.PowerOn;

            bool disable_ptt = console.DisablePTT;
            console.DisablePTT = true;

            USBHID.SetTXOut(false);
            Thread.Sleep(100);

            uint val;
            USBHID.ReadPTT(out val);
            bool ptt = ((val & 0x08) == 0);

            if (ptt)
            {
                retval = false;
                goto end;
            }

            USBHID.SetTXOut(true);
            Thread.Sleep(100);

            USBHID.ReadPTT(out val);
            ptt = ((val & 0x08) == 0);

            if (!ptt)
            {
                retval = false;
                goto end;
            }

        end:
            USBHID.SetTXOut(false);
            //if (power) console.PowerOn = true;
            Thread.Sleep(50);
            console.DisablePTT = disable_ptt;

            if (retval)
            {
                btnIOFWPTT.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO FW PTT: Passed");
                test_io_fwptt = "IO FW PTT Test: Passed";
            }
            else
            {
                btnIOFWPTT.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO FW PTT: Failed");
                test_io_fwptt = "IO FW PTT Test: Failed";
            }
            toolTip1.SetToolTip(btnIOFWPTT, test_io_fwptt);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_fwptt.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_fwptt.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Mic PTT

        private void btnIOMicPTT_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOMicPTT.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOMicPTT));
            t.Name = "IO Mic PTT Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_micptt = "IO Mic PTT Test: Not Run";
        private void CheckIOMicPTT()
        {
            bool retval = true;

            bool power = console.PowerOn;

            bool disable_ptt = console.DisablePTT;
            if (console.CurrentModel == Model.FLEX1500)
            {
                console.DisablePTT = true;
            }
            else
            {
                console.PowerOn = false;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetAmpTX3(false);
                    break;
                case Model.FLEX3000:
                    FWC.SetAmpTX1(false);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(false);
                    break;
            }
            Thread.Sleep(100);

            bool ptt = false;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReadMicPTT(out ptt);
                    Thread.Sleep(50);
                    break;
                case Model.FLEX1500:
                    uint val;
                    USBHID.ReadPTT(out val);
                    ptt = ((val & 0x01) == 0);
                    break;
            }

            if (ptt)
            {
                retval = false;
                goto end;
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetRCATX3(true);
                    break;
                case Model.FLEX3000:
                    FWC.SetRCATX1(true);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(true);
                    break;
            }
            Thread.Sleep(100);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    FWC.ReadMicPTT(out ptt);
                    Thread.Sleep(50);
                    break;
                case Model.FLEX1500:
                    uint val;
                    USBHID.ReadPTT(out val);
                    ptt = ((val & 0x01) == 0);
                    break;
            }
            if (!ptt)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    FWC.SetRCATX3(false);
                    FWC.SetAmpTX3(true);
                    break;
                case Model.FLEX3000:
                    FWC.SetRCATX1(false);
                    FWC.SetAmpTX1(true);
                    break;
                case Model.FLEX1500:
                    USBHID.SetTXOut(false);
                    break;
            }
            if (power) console.PowerOn = true;
            Thread.Sleep(50);

            if (console.CurrentModel == Model.FLEX1500)
                console.DisablePTT = disable_ptt;

            if (retval)
            {
                btnIOMicPTT.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Mic PTT: Passed");
                test_io_micptt = "IO Mic PTT Test: Passed";
            }
            else
            {
                btnIOMicPTT.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Mic PTT: Failed");
                test_io_micptt = "IO Mic PTT Test: Failed";
            }
            toolTip1.SetToolTip(btnIOMicPTT, test_io_micptt);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_micptt.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_micptt.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            string serial = "";
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    serial = FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial);
                    break;
                case Model.FLEX1500:
                    serial = HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial);
                    break;
            }
            writer.Write(serial + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Ext Ref

        private void btnIOExtRef_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOExtRef.BackColor = console.ButtonSelectedColor;
            Thread t = null;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    t = new Thread(new ThreadStart(CheckIOExtRef));
                    break;
                case Model.FLEX1500:
                    t = new Thread(new ThreadStart(Check1500IOExtRef));
                    break;
            }

            t.Name = "IO Ext Ref Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_extref = "IO Ext Ref Test: Not Run";
        private void CheckIOExtRef()
        {
            FWC.SetXREF(true);
            //Thread.Sleep(50);
            int old_reg = 0;
            if (FWC.ReadClockReg(0x08, out old_reg) == 0)
                MessageBox.Show("Error in ReadClockReg");
            //Thread.Sleep(50);
            int val = (int)((old_reg & 0xC3) | (int)(1 << 2));
            if (old_reg != val)
            {
                if (FWC.WriteClockReg(0x08, val) == 0)
                    MessageBox.Show("Error in WriteClockReg");
            }
            Thread.Sleep(1000);

            bool b = true;
            FWC.GetPLLStatus2(out b);
            //Thread.Sleep(50);

            FWC.WriteClockReg(0x08, old_reg);
            //Thread.Sleep(50);
            FWC.SetXREF(false);
            //Thread.Sleep(50);

            if (b)
            {
                btnIOExtRef.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Ext Ref: Passed");
                test_io_extref = "IO Ext Ref Test: Passed";
            }
            else
            {
                btnIOExtRef.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Ext Ref: Failed");
                test_io_extref = "IO Ext Ref Test: Failed";
            }
            toolTip1.SetToolTip(btnIOExtRef, test_io_extref);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_extref.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_extref.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        private void Check1500IOExtRef()
        {
            btnIOExtRef.BackColor = SystemColors.Control;

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            double vfoa_freq = console.VFOAFreq;
            console.VFOAFreq = 10.0;

            Flex1500.ProdTestWriteOp(USBHID.Opcode.USB_OP_SET_RX1_FREQ_TW, (uint)console.Freq2TW(10.0 / 2), 0);

            DSPMode dsp = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            console.VFOAFreq = 10.0;

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.RX1Filter = Filter.VAR1;

            console.RX1FilterLow = -500;
            console.RX1FilterHigh = 500;

            PreampMode preamp = console.RX1PreampMode;
            console.RX1PreampMode = (PreampMode)FLEX1500PreampMode.ZERO;

            HIDAnt ant = console.RXAnt1500;
            console.RXAnt1500 = HIDAnt.XVRX;

            USBHID.SetXref(true);
            console.FLEX1500Xref = true;
            Thread.Sleep(500);

            float adc_l = 0.0f;
            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                adc_l += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                if (j != 4) Thread.Sleep(50);
            }
            adc_l /= 5;

            bool b = (Math.Abs(-33.0 - adc_l) < 10);
            if (!b)
            {
                lstDebug.Items.Insert(0, " IO Ext Ref: Failed (" + adc_l.ToString("f1") + " dBFS)");
                test_genbal += "IO Ext Ref: Failed (" + adc_l.ToString("f1") + " dBFS)";
                btnIOExtRef.BackColor = Color.Red;
            }
            else
            {
                lstDebug.Items.Insert(0, "IO Ext Ref: Passed (" + adc_l.ToString("f1") + " dBFS)");
                test_genbal += "IO Ext Ref: Passed (" + adc_l.ToString("f1") + " dBFS)";
                btnIOExtRef.BackColor = Color.Green;
            }

            USBHID.SetXref(false);
            console.FLEX1500Xref = false;
            console.RXAnt1500 = ant;
            console.RX1PreampMode = preamp;

            console.RX1FilterLow = var_low;
            console.RX1FilterHigh = var_high;
            console.RX1Filter = filter;

            console.RX1DSPMode = dsp;
            console.VFOAFreq = vfoa_freq;

            Flex1500.ProdTestWriteOp(USBHID.Opcode.USB_OP_SET_RX1_FREQ_TW, 0, 0);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_extref.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_extref.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(HIDEEPROM.SerialToString(HIDEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(b.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Mic Up

        private void btnIOMicUp_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOMicUp.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOMicUp));
            t.Name = "IO Mic Up Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_micup = "IO Mic Up Test: Not Run";
        private void CheckIOMicUp()
        {
            bool retval = true;
            //FWC.SetMOX(true);
            //Thread.Sleep(50);
            //FWC.SetPABias(false);
            //Thread.Sleep(50);
            bool power = console.PowerOn;
            if (power) console.PowerOn = false;
            FWC.SetAmpTX1(false);
            Thread.Sleep(100);

            bool b;
            FWC.ReadMicUp(out b);
            Thread.Sleep(50);
            if (b)
            {
                retval = false;
                goto end;
            }

            FWC.SetRCATX1(true);
            Thread.Sleep(100);
            FWC.ReadMicUp(out b);
            Thread.Sleep(50);
            if (!b)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            FWC.SetRCATX1(false);
            FWC.SetAmpTX1(true);
            if (power) console.PowerOn = true;
            Thread.Sleep(50);

            if (retval)
            {
                btnIOMicUp.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Mic Up: Passed");
                test_io_micup = "IO Mic Up Test: Passed";
            }
            else
            {
                btnIOMicUp.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Mic Up: Failed");
                test_io_micup = "IO Mic Up Test: Failed";
            }
            toolTip1.SetToolTip(btnIOMicUp, test_io_micup);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_micup.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_micup.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Mic Down

        private void btnIOMicDown_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOMicDown.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOMicDown));
            t.Name = "IO Mic Down Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_micdown = "IO Mic Down Test: Not Run";
        private void CheckIOMicDown()
        {
            bool retval = true;
            //FWC.SetMOX(true);
            //Thread.Sleep(50);
            //FWC.SetPABias(false);
            //Thread.Sleep(50);
            bool power = console.PowerOn;
            if (power) console.PowerOn = false;
            FWC.SetAmpTX1(false);
            Thread.Sleep(100);

            bool b;
            FWC.ReadMicDown(out b);
            Thread.Sleep(50);
            if (b)
            {
                retval = false;
                goto end;
            }

            FWC.SetRCATX1(true);
            Thread.Sleep(100);
            FWC.ReadMicDown(out b);
            Thread.Sleep(50);
            if (!b)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            FWC.SetRCATX1(false);
            FWC.SetAmpTX1(true);
            if (power) console.PowerOn = true;
            Thread.Sleep(50);

            if (retval)
            {
                btnIOMicDown.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Mic Down: Passed");
                test_io_micdown = "IO Mic Down Test: Passed";
            }
            else
            {
                btnIOMicDown.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Mic Down: Failed");
                test_io_micdown = "IO Mic Down Test: Failed";
            }
            toolTip1.SetToolTip(btnIOMicDown, test_io_micdown);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_micdown.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_micdown.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Mic Fast

        private void btnIOMicFast_Click(object sender, System.EventArgs e)
        {
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            grpIO.Enabled = false;
            btnIOMicFast.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOMicFast));
            t.Name = "IO Mic Fast Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_micfast = "IO Mic Fast Test: Not Run";
        private void CheckIOMicFast()
        {
            bool retval = true;
            //FWC.SetMOX(true);
            //Thread.Sleep(50);
            //FWC.SetPABias(false);
            //Thread.Sleep(50);
            bool power = console.PowerOn;
            if (power) console.PowerOn = false;
            FWC.SetAmpTX1(false);
            Thread.Sleep(100);

            bool b;
            FWC.ReadMicFast(out b);
            Thread.Sleep(50);
            if (b)
            {
                retval = false;
                goto end;
            }

            FWC.SetRCATX1(true);
            Thread.Sleep(100);
            FWC.ReadMicFast(out b);
            Thread.Sleep(50);
            if (!b)
            {
                retval = false;
                goto end;
            }

        end:
            //FWC.SetMOX(false);
            FWC.SetRCATX1(false);
            FWC.SetAmpTX1(true);
            if (power) console.PowerOn = true;
            Thread.Sleep(50);

            if (retval)
            {
                btnIOMicFast.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Mic Fast: Passed");
                test_io_micfast = "IO Mic Fast Test: Passed";
            }
            else
            {
                btnIOMicFast.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Mic Fast: Failed");
                test_io_micfast = "IO Mic Fast Test: Failed";
            }
            toolTip1.SetToolTip(btnIOMicFast, test_io_micfast);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_micfast.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_micfast.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.WriteLine(retval.ToString());
            writer.Close();

            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            grpIO.Enabled = true;
        }

        #endregion

        #region Run All

        private void btnIORunAll_Click(object sender, System.EventArgs e)
        {
            btnIORunAll.Enabled = false;
            console.PowerOn = true;
            btnIORunAll.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(IORunAll));
            t.Name = "Run All IO Tests Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void IORunAll()
        {
            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            if (btnIOExtRef.Visible && btnIOExtRef.Enabled)
            {
                Invoke(new MethodInvoker(btnIOExtRef.PerformClick));
                Thread.Sleep(2000);
            }

            if (btnIOPwrSpkr.Visible && btnIOPwrSpkr.Enabled)
            {
                Invoke(new MethodInvoker(btnIOPwrSpkr.PerformClick));
                Thread.Sleep(3500);
            }

            if (btnIORCAInOut.Visible && btnIORCAInOut.Enabled)
            {
                Invoke(new MethodInvoker(btnIORCAInOut.PerformClick));
                Thread.Sleep(3000);
            }

            if (btnIOFWInOut.Visible && btnIOFWInOut.Enabled)
            {
                Invoke(new MethodInvoker(btnIOFWInOut.PerformClick));
                Thread.Sleep(5000);
            }

            if (btnIOHeadphone.Visible && btnIOHeadphone.Enabled)
            {
                Invoke(new MethodInvoker(btnIOHeadphone.PerformClick));
                Thread.Sleep(5000);
            }

            if (btnIODot.Visible && btnIODot.Enabled)
            {
                Invoke(new MethodInvoker(btnIODot.PerformClick));
                Thread.Sleep(2000);
            }

            if (btnIODash.Visible && btnIODash.Enabled)
            {
                Invoke(new MethodInvoker(btnIODash.PerformClick));
                Thread.Sleep(2000);
            }

            if (btnIORCAPTT.Visible && btnIORCAPTT.Enabled)
            {
                Invoke(new MethodInvoker(btnIORCAPTT.PerformClick));
                Thread.Sleep(1000);
            }

            if (btnIOFWPTT.Visible && btnIOFWPTT.Enabled)
            {
                Invoke(new MethodInvoker(btnIOFWPTT.PerformClick));
                Thread.Sleep(1000);
            }

            if (btnIOMicPTT.Visible && btnIOMicPTT.Enabled)
            {
                Invoke(new MethodInvoker(btnIOMicPTT.PerformClick));
                Thread.Sleep(1000);
            }

            if (btnIOMicUp.Visible && btnIOMicUp.Enabled)
            {
                Invoke(new MethodInvoker(btnIOMicUp.PerformClick));
                Thread.Sleep(1000);
            }

            if (btnIOMicDown.Visible && btnIOMicDown.Enabled)
            {
                Invoke(new MethodInvoker(btnIOMicDown.PerformClick));
                Thread.Sleep(1000);
            }

            if (btnIOMicFast.Visible && btnIOMicFast.Enabled)
            {
                Invoke(new MethodInvoker(btnIOMicFast.PerformClick));
                Thread.Sleep(1000);
            }

            btnIORunAll.BackColor = SystemColors.Control;
            btnIORunAll.Enabled = true;
            t1.Stop();
            MessageBox.Show("Run All Tests Time: " + ((int)(t1.Duration / 60)).ToString() + ":" + (((int)t1.Duration) % 60).ToString("00"));
        }

        #endregion

        #endregion

        #region Event Handlers

        private void FLEX5000ProdTestForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX5000ProdTestForm");
        }

        private void btnCheckAll_Click(object sender, System.EventArgs e)
        {
            ck160.Checked = true;
            ck80.Checked = true;
            ck60.Checked = true;
            ck40.Checked = true;
            ck30.Checked = true;
            ck20.Checked = true;
            ck17.Checked = true;
            ck15.Checked = true;
            ck12.Checked = true;
            ck10.Checked = true;
            ck6.Checked = true;
        }

        private void btnClearAll_Click(object sender, System.EventArgs e)
        {
            ck160.Checked = false;
            ck80.Checked = false;
            ck60.Checked = false;
            ck40.Checked = false;
            ck30.Checked = false;
            ck20.Checked = false;
            ck17.Checked = false;
            ck15.Checked = false;
            ck12.Checked = false;
            ck10.Checked = false;
            ck6.Checked = false;
        }

        private void btnPrintReport_Click(object sender, System.EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int V = 80;
            string text = "TRX Serial Number: " + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial)
                + "  Date: " + DateTime.Today.ToShortDateString()
                + "  Time: " + DateTime.Now.ToString("HH:mm:ss")
                + "  Tech: " + txtTech.Text + "\n\n";

            text += "\n" + test_pll + "\n";
            text += test_genbal + "\n";
            text += test_noise + "\n";
            text += test_impulse + "\n";
            text += test_preamp + "\n";

            text += "\n" + test_rx_filter + "\n";
            text += test_rx_level + "\n";
            text += test_rx_image + "\n";
            //text += test_rx_mds+"\n";

            text += "\n" + test_tx_filter + "\n";
            text += test_tx_image + "\n";
            text += test_tx_carrier + "\n";

            e.Graphics.DrawString(text,
                new Font("Lucida Console", 11), Brushes.Black, 80, V);
        }

        private void btnTestGenAll_Click(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    ckTestGenPLL.Checked = true;
                    ckTestGenImpulse.Checked = true;
                    ckTestGenPreamp.Checked = true;
                    break;
                case Model.FLEX3000:
                    ckTestGenPreamp.Checked = true;
                    ckTestGenATTN.Checked = true;
                    break;
                case Model.FLEX1500:
                    ckTestGenATTN.Checked = true;
                    break;
            }
            ckTestGenBal.Checked = true;
            //ckTestGenNoise.Checked = true;            
        }

        private void btnTestGenNone_Click(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    ckTestGenPLL.Checked = false;
                    ckTestGenImpulse.Checked = false;
                    break;
                case Model.FLEX3000:
                case Model.FLEX1500:
                    ckTestGenATTN.Checked = false;
                    break;
            }
            ckTestGenPLL.Checked = false;
            ckTestGenBal.Checked = false;
            //ckTestGenNoise.Checked = false;				
            ckTestGenPreamp.Checked = false;
        }

        private void btnTestRXAll_Click(object sender, System.EventArgs e)
        {
            ckTestRXFilter.Checked = true;
            ckTestRXLevel.Checked = true;
            ckTestRXImage.Checked = true;
            //ckTestRXMDS.Checked = true;
        }

        private void btnTestRXNone_Click(object sender, System.EventArgs e)
        {
            ckTestRXFilter.Checked = false;
            ckTestRXLevel.Checked = false;
            ckTestRXImage.Checked = false;
            //ckTestRXMDS.Checked = false;
        }

        private void btnTestTXAll_Click(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    ckTestTXFilter.Checked = true;
                    ckTestTXImage.Checked = true;
                    ckTestTXCarrier.Checked = true;
                    ckTestTXGain.Checked = true;
                    break;
                case Model.FLEX3000:
                    ckTestTXImage.Checked = true;
                    ckTestTXCarrier.Checked = true;
                    ckTestTXGain.Checked = true;
                    break;
                case Model.FLEX1500:
                    if (ckTestTXImage.Visible)
                        ckTestTXImage.Checked = true;
                    ckTestTXPA.Checked = true;
                    break;
            }
        }

        private void btnTestTXNone_Click(object sender, System.EventArgs e)
        {
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    ckTestTXFilter.Checked = false;
                    ckTestTXImage.Checked = false;
                    ckTestTXCarrier.Checked = false;
                    ckTestTXGain.Checked = false;
                    break;
                case Model.FLEX3000:
                    ckTestTXImage.Checked = false;
                    ckTestTXCarrier.Checked = false;
                    ckTestTXGain.Checked = false;
                    break;
                case Model.FLEX1500:
                    if (ckTestTXImage.Visible)
                        ckTestTXImage.Checked = false;
                    ckTestTXPA.Checked = false;
                    break;
            }
        }

        private void btnTestAll_Click(object sender, System.EventArgs e)
        {
            btnTestGenAll.PerformClick();
            btnTestRXAll.PerformClick();
            btnTestTXAll.PerformClick();
        }

        private void btnTestNone_Click(object sender, System.EventArgs e)
        {
            btnTestGenNone.PerformClick();
            btnTestRXNone.PerformClick();
            btnTestTXNone.PerformClick();
        }

        #endregion

        #region Post Fence

        private void btnPostFence_Click(object sender, System.EventArgs e)
        {
            //p = new Progress("Post Fence Test");
            grpGeneral.Enabled = false;
            grpReceiver.Enabled = false;
            grpTransmitter.Enabled = false;
            btnPostFence.BackColor = Color.Green;
            btnPostFence.Enabled = false;
            Thread t = new Thread(new ThreadStart(PostFenceTest));
            t.Name = "Post Fence Test Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            //Invoke(new MethodInvoker(p.Show));
        }

        private string test_post_fence = "Post Fence: Not Run";
        private void PostFenceTest()
        {
            /*if(!console.PowerOn)
            {
                p.Hide();
                MessageBox.Show("Power must be on to run this test.",
                    "Power not on",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);					
                grpGeneral.Enabled = true;
                grpReceiver.Enabled = true;
                grpTransmitter.Enabled = true;
                btnPostFence.Enabled = true;
                return;
            }*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            HiPerfTimer t1 = new HiPerfTimer();
            t1.Start();

            test_post_fence = "Post Fence: ";

            console.VFOSplit = false;

            btnPLL_Click(this, EventArgs.Empty);
            Thread.Sleep(1000);

            btnGenBal_Click(this, EventArgs.Empty);
            Thread.Sleep(3000);

            btnGenPreamp_Click(this, EventArgs.Empty);
            Thread.Sleep(3000);

            btnCheckAll_Click(this, EventArgs.Empty);
            btnRXFilter_Click(this, EventArgs.Empty);
            while (true)
            {
                while (p.Visible) Thread.Sleep(2000);
                Thread.Sleep(2000);
                if (!p.Visible) break;
            }
            if (p.Text == "") goto end;

            btnClearAll_Click(this, EventArgs.Empty);
            ck20.Checked = true;
            btnTXImage_Click(this, EventArgs.Empty);
            while (true)
            {
                while (!btnTXImage.Enabled) Thread.Sleep(2000);
                Thread.Sleep(2000);
                if (btnTXImage.Enabled) break;
            }
            if (p.Text == "") goto end;

            if (btnPLL.BackColor == Color.Green &&
                btnGenBal.BackColor == Color.Green &&
                btnRXFilter.BackColor == Color.Green &&
                btnTXImage.BackColor == Color.Green)
            {
                btnPostFence.BackColor = Color.Green;
                test_post_fence += "Passed";
            }
            else
            {
                btnPostFence.BackColor = Color.Red;
                test_post_fence += "Failed";
            }

        end:
            p.Hide();
            FWC.SetFullDuplex(false);
            //Thread.Sleep(50);
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            btnPostFence.Enabled = true;

            t1.Stop();
            MessageBox.Show("Post Fence Time: " + ((int)(t1.Duration / 60)).ToString() + ":" + (((int)t1.Duration) % 60).ToString("00"));

            /*string test_verify_rx_level = "";
            string test_verify_rx_image = "";
            string test_verify_tx_image = "";
            string test_verify_tx_carrier = "";
            bool pass_rx_level = false;
            bool pass_rx_image = false;
            bool pass_tx_image = false;
            bool pass_tx_carrier = false;
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f};
            float[] a = new float[Display.BUFFER_SIZE];
            int counter = 0;

            bool spur_red = console.SpurReduction;
            console.SpurReduction = false;

            string display = console.DisplayModeText;
            console.DisplayModeText = "Spectrum";
            PreampMode preamp = console.RX1PreampMode;
            DSPMode dsp_mode = console.RX1DSPMode;
            Filter filter = console.RX1Filter;
            int rx_filt_high = console.RX1FilterHigh;
            int rx_filt_low = console.RX1FilterLow;	
            int tx_filt_high = console.SetupForm.TXFilterHigh;
            int tx_filt_low = console.SetupForm.TXFilterLow;
		
            bool rit_on = console.RITOn;
            console.RITOn = false;
            bool xit_on = console.XITOn;
            console.XITOn = false;
            bool split = console.VFOSplit;

            int dsp_buf_size = console.SetupForm.DSPRXBufferSize;			// save current DSP buffer size
            console.SetupForm.DSPRXBufferSize = 4096;						// set DSP Buffer Size to 2048

            bool polyphase = console.SetupForm.Polyphase;				// save current polyphase setting
            console.SetupForm.Polyphase = false;						// disable polyphase

            int num_bands = 0;
            for(int i=0; i<band_freqs.Length; i++)
            {
                bool do_band = false;
                switch(bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M:	do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if(do_band) num_bands++;
            }
            int total_counts = num_bands*(10+10+10+5);

            for(int i=0; i<bands.Length; i++)
            {
                bool do_band = false;
                switch(bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M:	do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if(do_band)
                {
                    // verify RX Level
                    FWC.SetTest(true);
                    FWC.SetSig(true);
                    FWC.SetGen(true);
                    FWC.SetTXMon(false);
                    FWC.SetFullDuplex(true);

			
                    console.VFOAFreq = band_freqs[i];
                    Thread.Sleep(50);
                    console.VFOBFreq = band_freqs[i];

                    console.RX1PreampMode = PreampMode.HIGH;
                    console.RX1DSPMode = DSPMode.DSB;
                    console.RX1Filter = Filter.VAR1;
                    console.RX1FilterLow = -500;
                    console.RX1FilterHigh = 500;
                    Thread.Sleep(750);

                    int peak_bin = -1;
                    console.calibration_mutex.WaitOne();
                    fixed(float* ptr = &a[0])
                        DttSP.GetSpectrum(0, ptr);
                    console.calibration_mutex.ReleaseMutex();
                    float max = float.MinValue;
                    for(int j=0; j<Display.BUFFER_SIZE; j++)
                    {
                        if(a[j] > max)
                        {
                            max = a[j];
                            peak_bin = j;
                        }
                    }

                    float sum_d = 0.0f;
                    for(int j=0; j<5; j++)
                    {
                        console.calibration_mutex.WaitOne();
                        fixed(float* ptr = &a[0])
                            DttSP.GetSpectrum(0, ptr);
                        console.calibration_mutex.ReleaseMutex();
                        sum_d += a[peak_bin];
                        if(j != 4) Thread.Sleep(100);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    sum_d /= 5;
                    sum_d = sum_d + Display.DisplayCalOffset + Display.PreampOffset;

                    float sum_m = 0.0f;
                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for(int j=0; j<5; j++)
                    {
                        sum_m += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if(j != 4) Thread.Sleep(50);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    sum_m /= 5;
                    sum_m = sum_m + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

                    pass_rx_level = (Math.Abs(-25.0 - sum_d) < 1.0f) && (Math.Abs(-25.0 - sum_m) < 1.0f);

                    if(pass_rx_level)
                    {
                        lstDebug.Items.Insert(0, "Verify RX Level "+BandToString(bands[i])+": Passed ("+sum_d.ToString("f1")+", "+sum_m.ToString("f1")+")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "Verify RX Level "+BandToString(bands[i])+": Failed ("+sum_d.ToString("f1")+", "+sum_m.ToString("f1")+")");
                        if(!test_verify_rx_level.StartsWith("RX Level Failed ("))
                            test_verify_rx_level = "RX Level Failed (";
                        test_verify_rx_level += BandToString(bands[i])+", ";
                        if(btnPostFence.BackColor != Color.Red)
                            btnPostFence.BackColor = Color.Red;
                    }

                    // verify RX Image
                    console.VFOSplit = true;
                    FWC.SetQSE(false);
			
                    console.VFOAFreq = band_freqs[i];
                    Thread.Sleep(50);
                    console.VFOBFreq = band_freqs[i];
                    Thread.Sleep(750);

                    float fundamental = 0.0f;
                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for(int j=0; j<5; j++)
                    {
                        fundamental += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if(j != 4) Thread.Sleep(50);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    fundamental /= 5;
                    fundamental = fundamental + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

                    console.VFOAFreq = band_freqs[i]+2*console.IFFreq;
                    Thread.Sleep(500);

                    float image = 0.0f;
                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for(int j=0; j<5; j++)
                    {
                        image += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if(j != 4) Thread.Sleep(50);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    image /= 5;
                    image = image + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

                    float rejection = fundamental - image;
                    pass_rx_image = (rejection >= 60.0f);
 
                    if(pass_rx_image)
                    {
                        lstDebug.Items.Insert(0, "Verify RX Image "+BandToString(bands[i])+": Passed ("+fundamental.ToString("f1")+", "+image.ToString("f1")+", "+rejection.ToString("f1")+")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "Verify RX Image "+BandToString(bands[i])+": Failed ("+fundamental.ToString("f1")+", "+image.ToString("f1")+", "+rejection.ToString("f1")+")");
                        if(!test_verify_rx_image.StartsWith("RX Image Failed ("))
                            test_verify_rx_image = "RX Image Failed (";
                        test_verify_rx_image += BandToString(bands[i])+", ";
                        if(btnPostFence.BackColor != Color.Red)
                            btnPostFence.BackColor = Color.Red;
                    }

                    console.VFOSplit = false;

                    // verify TX Image
                    console.FullDuplex = true;
                    Audio.TXInputSignal = Audio.SignalSource.SINE;
                    double last_scale = Audio.SourceScale;			// saved audio scale
                    Audio.SourceScale = 1.0;						
                    double tone_freq = Audio.SineFreq1;				// save tone freq
                    Audio.SineFreq1 = 1500.0;						// set freq

                    int pwr = console.PWR;
                    console.PWR = 100;
                    Audio.RadioVolume = 0.200;

                    FWC.SetQSD(true);
                    FWC.SetQSE(true);
                    FWC.SetTR(true);
                    FWC.SetSig(true);
                    FWC.SetGen(false);
                    FWC.SetTest(true);
                    FWC.SetTXMon(false);

                    console.SetupForm.TXFilterLow = 300;
                    console.SetupForm.TXFilterHigh = 3000;

                    console.VFOAFreq = band_freqs[i];
                    Thread.Sleep(50);
                    console.VFOBFreq = band_freqs[i];
                    console.RX1DSPMode = DSPMode.USB;
                    console.UpdateFilters(300, 3000);
                    Thread.Sleep(750);

                    fundamental = 0.0f;
                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for(int j=0; j<5; j++)
                    {
                        fundamental += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if(j != 4) Thread.Sleep(50);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    fundamental /= 5;
                    fundamental = fundamental + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

                    console.UpdateFilters(-1550, -1450);
                    Thread.Sleep(500);

                    image = 0.0f;
                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for(int j=0; j<5; j++)
                    {
                        image += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if(j != 4) Thread.Sleep(50);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    image /= 5;
                    image = image + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

                    rejection = fundamental - image;

                    pass_tx_image = (rejection >= 60.0);

                    if(pass_tx_image)
                    {
                        lstDebug.Items.Insert(0, "Verify TX Image "+BandToString(bands[i])+": Passed ("+fundamental.ToString("f1")+", "+image.ToString("f1")+", "+rejection.ToString("f1")+")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "Verify TX Image "+BandToString(bands[i])+": Failed ("+fundamental.ToString("f1")+", "+image.ToString("f1")+", "+rejection.ToString("f1")+")");
                        if(!test_verify_tx_image.StartsWith("TX Image Failed ("))
                            test_verify_tx_image = "TX Image Failed (";
                        test_verify_tx_image += BandToString(bands[i])+", ";
                        if(btnPostFence.BackColor != Color.Red)
                            btnPostFence.BackColor = Color.Red;
                    }
                    Audio.TXInputSignal = Audio.SignalSource.RADIO;
                    Audio.SourceScale = last_scale;						// recall tone scale
                    Audio.SineFreq1 = tone_freq;						// recall tone freq
                    console.PWR = pwr;
                    FWC.SetTR(false);

                    // verify TX Carrier
                    FWC.SetQSD(true);
                    FWC.SetQSE(true);
                    FWC.SetSig(true);
                    FWC.SetGen(false);
                    FWC.SetTest(true);
                    FWC.SetTXMon(false);
                    Audio.TXInputSignal = Audio.SignalSource.SILENCE;

                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];
                    console.RX1DSPMode = DSPMode.DSB;
                    console.UpdateFilters(-500, 500);
                    Thread.Sleep(500);

                    float carrier = 0.0f;
                    DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    Thread.Sleep(50);
                    for(int j=0; j<5; j++)
                    {
                        carrier += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        if(j != 4) Thread.Sleep(50);
                        if(!p.Visible) 
                        {
                            p.Text = "";
                            goto end;
                        }
                        p.SetPercent(++counter/(float)total_counts);
                    }
                    carrier /= 5;
                    carrier = carrier + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

                    pass_tx_carrier = (carrier <= -105.0);

                    if(pass_tx_carrier)
                    {
                        lstDebug.Items.Insert(0, "Verify TX Carrier "+BandToString(bands[i])+": Passed ("+carrier.ToString("f1")+")");
                    }
                    else
                    {
                        lstDebug.Items.Insert(0, "Verify TX Carrier "+BandToString(bands[i])+": Failed ("+carrier.ToString("f1")+")");
                        if(!test_verify_tx_carrier.StartsWith("TX Carrier Failed ("))
                            test_verify_tx_carrier = "TX Carrier Failed (";
                        test_verify_tx_carrier += BandToString(bands[i])+", ";
                        if(btnPostFence.BackColor != Color.Red)
                            btnPostFence.BackColor = Color.Red;
                    }

                    Audio.TXInputSignal = Audio.SignalSource.RADIO;
                }
            }
	
            if(!test_verify_rx_level.StartsWith("RX Level Failed ("))
                test_verify_rx_level = "RX Level Passed\n";
            else test_verify_rx_level = test_verify_rx_level.Substring(0, test_verify_rx_level.Length-2)+")\n";

            if(!test_verify_rx_image.StartsWith("RX Image Failed ("))
                test_verify_rx_image = "RX Image Passed\n";
            else test_verify_rx_image = test_verify_rx_image.Substring(0, test_verify_rx_image.Length-2)+")\n";	

            if(!test_verify_tx_image.StartsWith("TX Image Failed ("))
                test_verify_tx_image = "TX Image Passed\n";
            else test_verify_tx_image = test_verify_tx_image.Substring(0, test_verify_tx_image.Length-2)+")\n";

            if(!test_verify_tx_carrier.StartsWith("TX Carrier Failed ("))
                test_verify_tx_carrier = "TX Carrier Passed\n";
            else test_verify_tx_carrier = test_verify_tx_carrier.Substring(0, test_verify_tx_carrier.Length-2)+")";

            test_post_fence = test_verify_rx_level + test_verify_rx_image + test_verify_tx_image + test_verify_tx_carrier;

            end:
                FWC.SetTest(false);
            FWC.SetTR(false);
            FWC.SetGen(false);
            FWC.SetSig(false);
            FWC.SetTXMon(false);
            FWC.SetFullDuplex(false);
            console.FullDuplex = false;
            console.SpurReduction = spur_red;
            console.DisplayModeText = display;
            console.RX1PreampMode = preamp;
            console.RX1DSPMode = dsp_mode;
            console.RX1Filter = filter;
            if(filter == Filter.VAR1 || filter == Filter.VAR2)
            {
                console.RX1FilterHigh = rx_filt_high;
                console.RX1FilterLow = rx_filt_low;			
            }
            console.RITOn = rit_on;
            console.XITOn = xit_on;
            console.VFOSplit = split;
            console.SetupForm.DSPRXBufferSize = dsp_buf_size;				// set DSP Buffer Size to 2048
            console.SetupForm.Polyphase = polyphase;					// disable polyphase

            toolTip1.SetToolTip(btnPostFence, test_post_fence);

            p.Hide();
            FWC.SetFullDuplex(false);
            grpGeneral.Enabled = true;
            grpReceiver.Enabled = true;
            grpTransmitter.Enabled = true;
            btnPostFence.Enabled = true;

            t1.Stop();
            MessageBox.Show("Verify Time: "+((int)(t1.Duration/60)).ToString()+":"+(((int)t1.Duration)%60).ToString("00"));
            */
        }

        #endregion

        private void FLEX5000ProdTestForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.L)
            {
                udLevel.Visible = true;
            }
        }

        private void btnRunAll1500_Click(object sender, EventArgs e)
        {
            console.PowerOn = true;
            btnRunAll1500.BackColor = console.ButtonSelectedColor;
            btnRunAll1500.Enabled = false;
            Thread t = new Thread(new ThreadStart(RunAll1500Tests));
            t.Name = "Run All Tests Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void RunAll1500Tests()
        {
            Invoke(new MethodInvoker(btnTestGenAll.PerformClick));
            Thread.Sleep(10);
            Invoke(new MethodInvoker(btnTestRXAll.PerformClick));
            Thread.Sleep(10);
            Invoke(new MethodInvoker(btnTestTXAll.PerformClick));
            Thread.Sleep(10);
            Invoke(new MethodInvoker(btnCheckAll.PerformClick));
            Thread.Sleep(10);

            Invoke(new MethodInvoker(btnRunSelectedTests.PerformClick));
            while (true)
            {
                while (!btnRunSelectedTests.Enabled) Thread.Sleep(1000);
                Thread.Sleep(1000);
                if (btnRunSelectedTests.Enabled) break;
            }

            Invoke(new MethodInvoker(btnIORunAll.PerformClick));
            while (true)
            {
                while (!btnIORunAll.Enabled) Thread.Sleep(1000);
                Thread.Sleep(1000);
                if (btnIORunAll.Enabled) break;
            }

            btnRunAll1500.BackColor = SystemColors.Control;
            btnRunAll1500.Enabled = true;
        }
    }
}