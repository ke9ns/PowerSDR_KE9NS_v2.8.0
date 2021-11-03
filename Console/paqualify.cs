//=================================================================
// paqualify.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class PAQualify : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Progress progress;
        private Console console;
       
        #endregion

        

        #region Misc Routines

        private void DisableTests(CheckBoxTS chk)
        {
            foreach (Control c in grpTests.Controls)
            {
                if (c != chk)
                    c.Enabled = false;
            }
        }

        private void EnableTests()
        {
            foreach (Control c in grpTests.Controls)
                c.Enabled = true;
        }

        #endregion

        #region Test Event Handlers

        private void chkBiasSet_CheckedChanged(object sender, System.EventArgs e)
        {
            console.Hdw.PABias = chkBiasSet.Checked;
            if (chkBiasSet.Checked)
            {
                chkBiasSet.BackColor = console.ButtonSelectedColor;
                DisableTests(chkBiasSet);
                console.Hdw.X2 |= 0x20;
            }
            else
            {
                EnableTests();
                chkBiasSet.BackColor = SystemColors.Control;
                console.Hdw.X2 &= 0xDF;
            }
        }

        private void chkHarm30_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkHarm30.Checked)
            {
                chkHarm30.BackColor = console.ButtonSelectedColor;
                DisableTests(chkHarm30);
                console.PowerOn = true;
                console.RX1DSPMode = DSPMode.USB;
                console.VFOAFreq = 10.1f;
                console.TUN = true;
                console.PreviousPWR = console.PWR;
                console.PWR = 50;
                //console.Hdw.PA_LPF = PAFBand.B1210;	// set LPF to 12_10
                console.CurrentMeterTXMode = MeterTXMode.SWR;       // set multimeter to SWR
            }
            else
            {
                console.PWR = console.PreviousPWR;
                console.TUN = false;
                EnableTests();
                chkHarm30.BackColor = SystemColors.Control;
            }
        }

        private void chkHarm60_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkHarm60.Checked)
            {
                chkHarm60.BackColor = console.ButtonSelectedColor;
                DisableTests(chkHarm60);
                console.PowerOn = true;
                console.RX1DSPMode = DSPMode.USB;
                console.VFOAFreq = 5.3305f;
                console.TUN = true;
                console.PreviousPWR = console.PWR;
                console.PWR = 50;
                //console.Hdw.PA_LPF = PAFBand.B1210;	// set LPF to 12_10
                console.CurrentMeterTXMode = MeterTXMode.SWR;       // set multimeter to SWR
            }
            else
            {
                console.PWR = console.PreviousPWR;
                console.TUN = false;
                EnableTests();
                chkHarm60.BackColor = SystemColors.Control;
            }
        }

        private void btnHarmFil30_Click(object sender, System.EventArgs e)
        {
            btnCheckAll.PerformClick();
            chk30m.Checked = false;

            console.PowerOn = true;
            console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;
            progress = new Progress("Calibrate PA Gain");

            Thread t = new Thread(new ThreadStart(CalibratePAGain));
            t.Name = "PA Gain Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnHarmFil60_Click(object sender, System.EventArgs e)
        {
            btnCheckAll.PerformClick();
            chk60m.Checked = false;

            console.PowerOn = true;
            console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;
            progress = new Progress("Calibrate PA Gain");

            Thread t = new Thread(new ThreadStart(CalibratePAGain));
            t.Name = "PA Gain Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void chkFWDLow_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFWDLow.Checked)
            {
                chkFWDLow.BackColor = console.ButtonSelectedColor;
                DisableTests(chkFWDLow);
                console.PowerOn = true;
                console.PreviousPWR = console.PWR;
                console.RX1DSPMode = DSPMode.USB;
                console.PWR = 50;
                console.VFOAFreq = 14.2f;
                Audio.TXInputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = 1.0;
                console.MOX = true;
                Audio.MOX = true;
                console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;
            }
            else
            {
                Audio.MOX = false;
                console.MOX = false;
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                console.PWR = console.PreviousPWR;
                EnableTests();
                chkFWDLow.BackColor = SystemColors.Control;
            }
        }

        private void chkFWDHigh_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFWDHigh.Checked)
            {
                chkFWDHigh.BackColor = console.ButtonSelectedColor;
                DisableTests(chkFWDHigh);
                console.PowerOn = true;
                console.PreviousPWR = console.PWR;
                console.PWR = 100;
                console.VFOAFreq = 14.2f;
                Audio.TXInputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = 1.0;
                console.MOX = true;
                Audio.MOX = true;
                console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;
            }
            else
            {
                Audio.MOX = false;
                console.MOX = false;
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                console.PWR = console.PreviousPWR;
                EnableTests();
                chkFWDHigh.BackColor = SystemColors.Control;
            }
        }

        private void chkSWRCal_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkSWRCal.Checked)
            {
                chkSWRCal.BackColor = console.ButtonSelectedColor;
                DisableTests(chkSWRCal);
                console.PowerOn = true;
                console.PreviousPWR = console.PWR;
                console.VFOAFreq = 14.2;
                console.TUN = true;
                console.PWR = 50;
                console.CurrentMeterTXMode = MeterTXMode.SWR;
                console.Hdw.X2 |= 0x08;
            }
            else
            {
                console.TUN = false;
                EnableTests();
                chkSWRCal.BackColor = SystemColors.Control;
                console.Hdw.X2 &= 0xF7;
            }
        }

        private void btnBandSweep_Click(object sender, System.EventArgs e)
        {
            console.PowerOn = true;
            progress = new Progress("Low Power PA Sweep");

            Thread t = new Thread(new ThreadStart(LowPowerPASweep));
            t.Name = "PA Low Power PA Sweep Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void LowPowerPASweep()
        {
            bool done = console.LowPowerPASweep(progress, 10);
            if (done) MessageBox.Show("Low Power PA Sweep complete.");
        }

        private void btnGainCal_Click(object sender, System.EventArgs e)
        {
            console.PowerOn = true;
            console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;
            progress = new Progress("Calibrate PA Gain");

            Thread t = new Thread(new ThreadStart(CalibratePAGain));
            t.Name = "PA Gain Calibration Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void CalibratePAGain()
        {
            bool[] run = new bool[10];
            run[0] = !chk160m.Checked;
            run[1] = !chk80m.Checked;
            run[2] = !chk60m.Checked;
            run[3] = !chk40m.Checked;
            run[4] = !chk30m.Checked;
            run[5] = !chk20m.Checked;
            run[6] = !chk17m.Checked;
            run[7] = !chk15m.Checked;
            run[8] = !chk12m.Checked;
            run[9] = !chk10m.Checked;

            bool done = console.CalibratePAGain(progress, run, (int)udCalTarget.Value);
            if (done) MessageBox.Show("PA Gain Calibration complete.");
        }

        private void chkLFTest_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLFTest.Checked)
            {
                chkLFTest.BackColor = console.ButtonSelectedColor;
                DisableTests(chkLFTest);
                console.PowerOn = true;
                console.PreviousPWR = console.PWR;
                console.PWR = 1;
                console.VFOAFreq = 1.9f;
                console.MOX = true;
                Audio.MOX = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = 1.0;
                timer_LF_test.Enabled = true;
            }
            else
            {
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                Audio.MOX = false;
                console.MOX = false;
                timer_LF_test.Enabled = false;
                console.PWR = console.PreviousPWR;
                EnableTests();
                chkLFTest.BackColor = SystemColors.Control;
            }
        }

        private void chkIMDTest_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkIMDTest.Checked)
            {
                chkIMDTest.BackColor = console.ButtonSelectedColor;
                DisableTests(chkIMDTest);
                console.PowerOn = true;
                console.PreviousPWR = console.PWR;
                console.PWR = 50;
                console.VFOAFreq = 14.2f;
                Audio.SineFreq1 = 700.0;
                Audio.SineFreq2 = 1900.0;
                Audio.two_tone = true;
                console.MOX = true;
                Audio.MOX = true;
                Audio.TXInputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = 1.0;
                console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;
            }
            else
            {
                Audio.TXInputSignal = Audio.SignalSource.RADIO;
                Audio.MOX = false;
                console.MOX = false;
                console.PWR = console.PreviousPWR;
                Audio.SineFreq1 = (double)console.CWPitch;
                Audio.two_tone = false;
                EnableTests();
                chkIMDTest.BackColor = SystemColors.Control;
            }
        }

        #endregion

        #region Other Event Handlers

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            //			printDialog1.Document = printDocument1;
            //			if (printDialog1.ShowDialog() == DialogResult.OK)
            //			{
            //				printDocument1.Print();
            //			}
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int V = 80;
            string text = "Date: " + DateTime.Today.ToShortDateString() + "  Time: " +
                DateTime.Now.ToString("HH:mm:ss") + "\n\n";
            text += "FlexRadio SDR-100WPA Gain Test Results\n";
            text += "Serial Number: " + txtSerialNum.Text + "\n";

            text += "\nBand  |  Value\n" +
                "160m  |  " + console.setupForm.PAGain160.ToString("f1") + "\n" +
                "80m   |  " + console.setupForm.PAGain80.ToString("f1") + "\n" +
                "60m   |  " + console.setupForm.PAGain60.ToString("f1") + "\n" +
                "40m   |  " + console.setupForm.PAGain40.ToString("f1") + "\n" +
                "30m   |  " + console.setupForm.PAGain30.ToString("f1") + "\n" +
                "20m   |  " + console.setupForm.PAGain20.ToString("f1") + "\n" +
                "17m   |  " + console.setupForm.PAGain17.ToString("f1") + "\n" +
                "15m   |  " + console.setupForm.PAGain15.ToString("f1") + "\n" +
                "12m   |  " + console.setupForm.PAGain12.ToString("f1") + "\n" +
                "10m   |  " + console.setupForm.PAGain10.ToString("f1") + "\n";
            text += "\nEnter these values when prompted by the PowerSDR Setup wizard or\n" +
                "(preferably) if a 100W dummy load is present, run the PA Cal routine\n" +
                "to obtain accurate values for your environment at your location.";
            e.Graphics.DrawString(text,
                new Font("Lucida Console", 11), Brushes.Black, 80, V);
            V += 350;

            text = "Documentation Information";
            e.Graphics.DrawString(text,
                new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Black, 80, V);
            V += 30;

            text = "All manuals and software can be downloaded from our website at http://www.flexradio.com.\n" +
                "Sign up for an account by clicking on \"Log In\" to access the Users Only folder which\n" +
                "contains the schematics and other Hardware docmentation.";

            e.Graphics.DrawString(text,
                new Font("Times New Roman", 12), Brushes.Black, 80, V);
            V += 90;

            text = "There are several important documents on the Knowledge Base (KB):\n" +
                "\t1. SDR-1000 Operating Manual\n" +
                "\t2. Soundcard Quick Start Guide\n" +
                "\t3. PowerSDR Quick Start Guide\n" +
                "\nIf you have any problems downloading, please send an email to support@flexradio.com.\n" +
                "You may also call +1(512) 535-4713 ext 2 if you need telephone assistance.";
            e.Graphics.DrawString(text,
                new Font("Times New Roman", 12), Brushes.Black, 80, V);
            V += 200;

            text = "Warning:";
            e.Graphics.DrawString(text,
                new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Black, 80, V);
            V += 30;

            text = "Proper operation of the SDR-1000 depends on the use of a sound card that is officially\n" +
                "supported by FlexRadio Systems.  Refer to the Specifications page on www.flexradio.com\n" +
                "to determine which sound cards are currently supported.  Use only the specific model numbers\n" +
                "stated on the website because other models within the same family may not work properly with\n" +
                "the radio.  Officially supported sound cards may be updated on the website without notice.\n" +

                "\nNO WARRANTY IS IMPLIED WHEN THE SDR-1000 IS USED WITH ANY SOUND CARD\n" +
                "OTHER THAN THOSE CURRENTLY SUPPORTED AS STATED ON THE FLEXRADIO SYSTEMS\n" +
                "WEBSITE.  UNSUPPORTED SOUND CARDS MAY OR MAY NOT WORK WITH THE SDR-1000.\n" +
                "USE OF UNSUPPORTED SOUND CARDS IS AT THE CUSTOMERS OWN RISK.";
            e.Graphics.DrawString(text,
                new Font("Times New Roman", 12, FontStyle.Regular), Brushes.Black, 80, V);
        }

        private void btnClearCal_Click(object sender, System.EventArgs e)
        {
            chk160m.Checked = false;
            chk80m.Checked = false;
            chk60m.Checked = false;
            chk40m.Checked = false;
            chk30m.Checked = false;
            chk20m.Checked = false;
            chk17m.Checked = false;
            chk15m.Checked = false;
            chk12m.Checked = false;
            chk10m.Checked = false;
        }

        private void btnCheckAll_Click(object sender, System.EventArgs e)
        {
            chk160m.Checked = true;
            chk80m.Checked = true;
            chk60m.Checked = true;
            chk40m.Checked = true;
            chk30m.Checked = true;
            chk20m.Checked = true;
            chk17m.Checked = true;
            chk15m.Checked = true;
            chk12m.Checked = true;
            chk10m.Checked = true;
        }

        private void btnClearAll_Click(object sender, System.EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(GroupBoxTS))
                {
                    foreach (Control c2 in c.Controls)
                    {
                        if (c2.GetType() == typeof(CheckBoxTS))
                        {
                            CheckBoxTS chk = (CheckBoxTS)c2;
                            chk.Checked = false;
                        }
                    }
                }
                else if (c.GetType() == typeof(CheckBoxTS))
                {
                    CheckBoxTS chk = (CheckBoxTS)c;
                    chk.Checked = false;
                }
            }
            txtSerialNum.Text = "Serial Num";
        }

        private void timer_LF_test_Tick(object sender, System.EventArgs e)
        {
            console.PWR++;

            if (console.PWR == 100)
            {
                console.PWR = 1;
                if (console.VFOAFreq == 1.9)
                    console.VFOAFreq = 14.2;
                else if (console.VFOAFreq == 14.2)
                    console.VFOAFreq = 28.8;
                else if (console.VFOAFreq == 28.8)
                    chkLFTest.Checked = false;
            }
        }

        #endregion
    }
}
