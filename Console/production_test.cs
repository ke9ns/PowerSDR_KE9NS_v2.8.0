//=================================================================
// production_test.cs  for SDR-1000
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
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for production_test.
    /// </summary>
    public partial class ProductionTest : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private ProductionDebug debug_form;
        private Console console;
        private Progress progress;


        #endregion

        #region Constructor and Destructor

        public ProductionTest(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;
            Common.RestoreForm(this, "ProdTest", false);
            udGenClockCorr_ValueChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        private void DisableTests(Control not_c)
        {
            foreach (Control c in grpTests.Controls)
            {
                if (c.Name.IndexOf("Run") > 0)
                {
                    if (c.Name != not_c.Name)
                        c.Enabled = false;
                }
            }
        }

        private void EnableTests()
        {
            foreach (Control c in grpTests.Controls)
            {
                if (c.Name.IndexOf("Run") > 0)
                {
                    c.Enabled = true;
                }
            }
        }

        private void ClearResults()
        {
            ArrayList controls = new ArrayList();
            Common.ControlList(this, ref controls);

            foreach (Control c in controls)
            {
                if (c.GetType() == typeof(ButtonTS))
                {
                    ButtonTS b = (ButtonTS)c;
                    if (b.BackColor == Color.Red || b.BackColor == Color.Green || b.BackColor == Color.Yellow)
                        b.BackColor = SystemColors.Control;
                }
                else if (c.GetType() == typeof(TextBoxTS))
                {
                    TextBoxTS txt = (TextBoxTS)c;
                    if (txt.BackColor == Color.Red || txt.BackColor == Color.Green || txt.BackColor == Color.Yellow)
                        txt.BackColor = SystemColors.Control;
                }
            }
        }

        #endregion

        #region Test Routines

        private void RunAll()
        {
            ClearResults();

            AD9854.PowerDownFullDigital = false;
            AD9854.ResetDDS();
            AD9854.IAmp = AD9854.QAmp = 5;
            AD9854.SetFreq(50.0);
            Thread.Sleep(1000);
            console.CalibrateFreq(50.0f, progress, false);

            SignalTest();

            //			if(progress.Text == "") goto end;
            //			progress.Text = "Noise Level Test";
            //			progress.SetPercent(0);
            //			progress.Show();
            //
            //			NoiseTest();

            if (progress.Text == "") goto end;
            progress.Text = "Preamp Level Test";
            progress.SetPercent(0);
            progress.Show();

            PreampTest();

            if (progress.Text == "") goto end;
            progress.Text = "Impulse Test";
            progress.SetPercent(0);
            progress.Show();

            ImpulseTest();

            if (progress.Text == "") goto end;
            progress.Text = "Balance Test";
            progress.SetPercent(0);
            progress.Show();

            BalanceTest();

            if (progress.Text == "") goto end;
            progress.Text = "Mic Test";
            progress.SetPercent(0);
            progress.Show();

            MicTest();

            if (progress.Text == "") goto end;
            progress.Text = "PTT Test";
            progress.SetPercent(0);
            progress.Show();

            PTTTest();

        end:
            progress.Hide();

            //AD9854.PowerDownFullDigital = true;
            //console.PowerOn = false;
        }

        private void SignalTest()
        {
            DisableTests(this);

            btnRunSignal.BackColor = Color.Yellow;

            const int DELAY = 1200;

            ArrayList txt = new ArrayList();
            txt.Add(txtSignal160H);
            txt.Add(txtSignal80H);
            txt.Add(txtSignal60H);
            txt.Add(txtSignal40H);
            txt.Add(txtSignal30H);
            txt.Add(txtSignal20H);
            txt.Add(txtSignal17H);
            txt.Add(txtSignal15H);
            txt.Add(txtSignal12H);
            txt.Add(txtSignal10H);
            txt.Add(txtSignal6H);

            ArrayList delta = new ArrayList();
            delta.Add(txtSigDelta160);
            delta.Add(txtSigDelta80);
            delta.Add(txtSigDelta60);
            delta.Add(txtSigDelta40);
            delta.Add(txtSigDelta30);
            delta.Add(txtSigDelta20);
            delta.Add(txtSigDelta17);
            delta.Add(txtSigDelta15);
            delta.Add(txtSigDelta12);
            delta.Add(txtSigDelta10);
            delta.Add(txtSigDelta6);

            ArrayList _ref = new ArrayList();
            _ref.Add(txtRefSig160);
            _ref.Add(txtRefSig80);
            _ref.Add(txtRefSig60);
            _ref.Add(txtRefSig40);
            _ref.Add(txtRefSig30);
            _ref.Add(txtRefSig20);
            _ref.Add(txtRefSig17);
            _ref.Add(txtRefSig15);
            _ref.Add(txtRefSig12);
            _ref.Add(txtRefSig10);
            _ref.Add(txtRefSig6);

            ArrayList chk = new ArrayList();
            chk.Add(chk160);
            chk.Add(chk80);
            chk.Add(chk60);
            chk.Add(chk40);
            chk.Add(chk30);
            chk.Add(chk20);
            chk.Add(chk17);
            chk.Add(chk15);
            chk.Add(chk12);
            chk.Add(chk10);
            chk.Add(chk6);

            float[] freq = { 2.0f, 4.0f, 5.4035f, 7.3f, 10.15f, 14.35f,
                18.168f, 21.45f, 24.99f, 29.7f, 54.0f };

            console.RX1DSPMode = DSPMode.AM;
            console.RX1Filter = Filter.F3;
            console.RX1PreampMode = PreampMode.HIGH;

            AD9854.PowerDownFullDigital = false;
            AD9854.ResetDDS();
            AD9854.IAmp = AD9854.QAmp = 5;
            Thread.Sleep(DELAY);

            for (int i = 0; i < freq.Length; i++)
            {
                if (!((CheckBox)chk[i]).Checked)
                {
                    ((TextBoxTS)txt[i]).BackColor = Color.White;
                    AD9854.SetFreq(freq[i]);
                    console.VFOAFreq = freq[i];
                    Thread.Sleep(2000);

                    float level_sum = 0.0f;
                    for (int j = 0; j < 100; j++)
                    {
                        Thread.Sleep(DELAY / 100);
                        float num = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        num += (console.rx1_meter_cal_offset + console.rx1_filter_size_cal_offset + console.PreampOffset);
                        level_sum += num;
                        if (!progress.Visible) goto end;
                        progress.SetPercent((i * 100 + j + 1) / 1100.0f);
                    }

                    float level = level_sum / 100.0f;
                    ((TextBoxTS)txt[i]).Text = level.ToString("f1");
                    ((TextBoxTS)delta[i]).Text = (level - TextBoxFloat((TextBoxTS)_ref[i])).ToString("f1");
                    ((TextBoxTS)txt[i]).BackColor = SystemColors.Control;
                }
            }

        end:
            btnRunSignal.BackColor = SystemColors.Control;
            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            EnableTests();

            CheckSignal();
        }

        private void NoiseTest()
        {
            DisableTests(this);

            btnRunNoise.BackColor = Color.Yellow;

            const int DELAY = 4200;

            console.RX1DSPMode = DSPMode.CWU;
            console.RX1Filter = Filter.F6;
            console.RX1PreampMode = PreampMode.HIGH;

            AD9854.ResetDDS();

            txtNoise20.BackColor = Color.White;
            console.VFOAFreq = 14.1;
            Thread.Sleep(DELAY / 4);

            float level_sum = 0.0f;

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(DELAY / 100);
                float num = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                num += (console.rx1_meter_cal_offset + console.rx1_filter_size_cal_offset + console.PreampOffset);
                level_sum += num;
                if (!progress.Visible) goto end;
                progress.SetPercent((i + 1) / 100.0f);
            }
            Thread.Sleep(DELAY / 4);

            float level = level_sum / 100.0f;

            txtNoise20.Text = level.ToString("f1");
            txtNoise20.BackColor = SystemColors.Control;
            if (!progress.Visible) goto end;
            progress.SetPercent(1.0f);

        end:
            btnRunNoise.BackColor = SystemColors.Control;
            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            EnableTests();

            CheckNoise();
        }

        private void PreampTest()
        {
            DisableTests(btnRunPreamp);

            btnRunPreamp.BackColor = Color.Yellow;

            const int DELAY = 1200;

            console.RX1DSPMode = DSPMode.AM;
            console.RX1Filter = Filter.F6;
            console.RX1PreampMode = PreampMode.OFF;

            console.VFOAFreq = 14.175;

            AD9854.ResetDDS();
            AD9854.IAmp = AD9854.QAmp = 5;
            AD9854.SetFreq(14.175);

            // Preamp Off			
            Thread.Sleep(DELAY);
            float off_level_sum = 0f;
            for (int i = 0; i < 50; i++)
            {
                off_level_sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
                if (!progress.Visible) goto end;
                progress.SetPercent((i + 1) / 200.0f);
            }

            // Preamp Low
            console.RX1PreampMode = PreampMode.LOW;
            Thread.Sleep(DELAY);
            float low_level_sum = 0f;
            for (int i = 0; i < 50; i++)
            {
                low_level_sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
                if (!progress.Visible) goto end;
                progress.SetPercent((51 + i) / 200.0f);
            }

            // Preamp Med
            console.RX1PreampMode = PreampMode.MED;
            Thread.Sleep(DELAY);
            float med_level_sum = 0f;
            for (int i = 0; i < 50; i++)
            {
                med_level_sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
                if (!progress.Visible) goto end;
                progress.SetPercent((101 + i) / 200.0f);
            }

            // Preamp High
            console.RX1PreampMode = PreampMode.HIGH;
            Thread.Sleep(DELAY);
            float high_level_sum = 0f;
            for (int i = 0; i < 50; i++)
            {
                high_level_sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
                if (!progress.Visible) goto end;
                progress.SetPercent((151 + i) / 200.0f);
            }

            txtPreGain.Text = Math.Abs(med_level_sum / 50.0 - off_level_sum / 50.0).ToString("f1");
            txtPreAtt.Text = Math.Abs(high_level_sum / 50.0 - med_level_sum / 50.0).ToString("f1");

        end:
            console.RX1PreampMode = PreampMode.HIGH;
            btnRunPreamp.BackColor = SystemColors.Control;
            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            EnableTests();

            CheckPreamp();
        }

        private void ImpulseTest()
        {
            DisableTests(this);

            btnRunImpulse.BackColor = Color.Yellow;

            console.RX1DSPMode = DSPMode.USB;
            console.RX1Filter = Filter.VAR1;
            console.RX1FilterLow = 7500;
            console.RX1FilterHigh = 8500;
            console.RX1PreampMode = PreampMode.OFF;

            console.VFOAFreq = 14.175;

            AD9854.ResetDDS();

            Thread.Sleep(1200);

            float level = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.AVG_SIGNAL_STRENGTH);

            console.Hdw.ImpulseEnable = true;

            for (int i = 0; i < 2000; i++)
            {
                console.Hdw.Impulse();
                Thread.Sleep(0);
                if (!progress.Visible) goto end;
            }

            float impulse = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.AVG_SIGNAL_STRENGTH);

            console.Hdw.ImpulseEnable = false;

            Debug.WriteLine("Impulse Diff: " + (impulse - level).ToString("f1"));

            if ((impulse - level) > TextBoxFloat(txtTolImpulse))
                btnRunImpulse.BackColor = Color.Green;
            else btnRunImpulse.BackColor = Color.Red;
            end:
            console.RX1PreampMode = PreampMode.HIGH;
            //btnRunImpulse.BackColor = SystemColors.Control;
            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            EnableTests();
        }

        private void MicTest()
        {
            DisableTests(this);

            btnRunMic.BackColor = Color.Yellow;
            console.PWR = 0;
            console.Mic = 0;
            console.MON = true;
            console.AF = 100;
            console.RX1DSPMode = DSPMode.USB;
            console.CurrentMeterTXMode = MeterTXMode.MIC;
            console.MOX = true;
            Thread.Sleep(50);
            console.Hdw.X2 |= 0x04;

            Thread.Sleep(200);

            float start = (float)Math.Max(-20.0, -DttSP.CalculateTXMeter(0, DttSP.MeterType.MIC));
            start = (float)Math.Max(-20.0, -DttSP.CalculateTXMeter(0, DttSP.MeterType.MIC));

            // begin outputting tone
            Audio.TXInputSignal = Audio.SignalSource.SINE;
            Audio.SourceScale = 1.0;

            Thread.Sleep(1000);
            float end = (float)Math.Max(-20.0, -DttSP.CalculateTXMeter(0, DttSP.MeterType.MIC));
            end = (float)Math.Max(-20.0, -DttSP.CalculateTXMeter(0, DttSP.MeterType.MIC));

            console.Hdw.X2 &= 0xFB;
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            console.MOX = false;
            Thread.Sleep(50);

            if (Math.Abs(end - start) < TextBoxFloat(txtTolMic))
                btnRunMic.BackColor = Color.Red;
            else btnRunMic.BackColor = Color.Green;

            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            EnableTests();
        }

        private void PTTTest()
        {
            DisableTests(this);

            btnRunPTT.BackColor = Color.Yellow;

            console.MOX = false;
            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;
            console.PreviousPWR = console.PWR;
            console.PWR = 0;

            if (console.MOX) goto fail;

            console.Hdw.X2 |= 0x10;
            Thread.Sleep(200);

            bool b = console.MOX;
            console.Hdw.X2 &= 0xEF;

            if (b) goto pass;
            else goto fail;

            fail:
            btnRunPTT.BackColor = Color.Red;
            goto end;
        pass:
            btnRunPTT.BackColor = Color.Green;
            goto end;

        end:

            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            console.PWR = console.PreviousPWR;
            console.RX1DSPMode = dsp_mode;
            EnableTests();
        }

        private void RFETest()
        {
            DisableTests(this);

            btnRunRFE.BackColor = Color.Yellow;

            do
            {
                console.Hdw.TestRFEIC11();
                Thread.Sleep(100);
            } while (progress.Visible);

            btnRunRFE.BackColor = SystemColors.Control;

            EnableTests();
        }

        private void BalanceTest()
        {
            DisableTests(this);

            btnRunBalance.BackColor = Color.Yellow;

            console.RX1DSPMode = DSPMode.AM;
            console.RX1Filter = Filter.F6;
            console.RX1PreampMode = PreampMode.HIGH;

            console.VFOAFreq = 14.175;

            AD9854.ResetDDS();
            AD9854.IAmp = AD9854.QAmp = 5;
            AD9854.SetFreq(14.175);

            Thread.Sleep(1200);

            float lsum = 0, rsum = 0;
            for (int i = 0; i < 100; i++)
            {
                lsum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_IMAG);
                rsum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.ADC_REAL);
                Thread.Sleep(50);
                if (!progress.Visible) goto end;
                progress.SetPercent((i + 1) / 100.0f);
            }

            float diff = Math.Abs(lsum / 100.0f - rsum / 100.0f);
            Debug.WriteLine("Balance Diff: " + diff.ToString("f1"));

            if (diff > TextBoxFloat(txtTolBalance))
                btnRunBalance.BackColor = Color.Red;
            else btnRunBalance.BackColor = Color.Green;

            end:
            if (!progress.Visible) progress.Text = "";
            progress.Hide();
            EnableTests();
        }

        private void TransmitTest()
        {
            DisableTests(this);
            btnRunTX.BackColor = Color.Yellow;

            double vfoa = console.VFOAFreq;

            double[] freq = { 2.0, 4.0, 5.4035, 7.3, 10.15, 14.35,
                               18.168, 21.30, 24.99, 29.0, 50.1 };

            ArrayList chk = new ArrayList();
            chk.Add(chk160);
            chk.Add(chk80);
            chk.Add(chk60);
            chk.Add(chk40);
            chk.Add(chk30);
            chk.Add(chk20);
            chk.Add(chk17);
            chk.Add(chk15);
            chk.Add(chk12);
            chk.Add(chk10);
            chk.Add(chk6);

            console.RX1DSPMode = DSPMode.USB;
            Thread.Sleep(1000);

            for (int i = 0; i < freq.Length; i++)
            {
                if (!((CheckBox)chk[i]).Checked)
                {
                    console.VFOAFreq = freq[i];
                    Thread.Sleep(100);
                    console.TUN = true;
                    console.PWR = 100;
                    for (int j = 0; j < udTXTestOnTime.Value; j += 10)
                    {
                        Thread.Sleep(9);
                        if (!progress.Visible) goto end;
                    }
                    console.TUN = false;
                    for (int j = 0; j < udTXTestOffTime.Value; j += 10)
                    {
                        Thread.Sleep(9);
                        if (!progress.Visible) goto end;
                    }
                }
                progress.SetPercent((i + 1) / 11.0f);
            }

        end:
            progress.Hide();
            console.VFOAFreq = vfoa;
            console.TUN = false;
            btnRunTX.BackColor = SystemColors.Control;
            EnableTests();
        }

        private void CheckSignal()
        {
            ArrayList txt = new ArrayList();
            txt.Add(txtSigDelta160);
            txt.Add(txtSigDelta80);
            txt.Add(txtSigDelta60);
            txt.Add(txtSigDelta40);
            txt.Add(txtSigDelta30);
            txt.Add(txtSigDelta20);
            txt.Add(txtSigDelta17);
            txt.Add(txtSigDelta15);
            txt.Add(txtSigDelta12);
            txt.Add(txtSigDelta10);
            txt.Add(txtSigDelta6);

            float low_tol = TextBoxFloat(txtTolSigLow);
            float high_tol = TextBoxFloat(txtTolSigHigh);

            foreach (TextBoxTS t in txt)
            {
                float num = TextBoxFloat(t);
                if (num > high_tol || num < -low_tol)
                    t.BackColor = Color.Red;
                else t.BackColor = SystemColors.Control;
            }
        }

        private void CheckNoise()
        {
            float tol = TextBoxFloat(txtTolNoise);

            if (Math.Abs(TextBoxFloat(txtNoise20) - TextBoxFloat(txtRefNoise20)) > tol)
                txtNoise20.BackColor = Color.Red;
            else txtNoise20.BackColor = SystemColors.Control;
        }

        private void CheckPreamp()
        {
            if (Math.Abs(TextBoxFloat(txtPreGain) - 26.0) > TextBoxFloat(txtTolPreamp))
                txtPreGain.BackColor = Color.Red;
            else txtPreGain.BackColor = SystemColors.Control;

            if (Math.Abs(TextBoxFloat(txtPreAtt) - 10.0) > TextBoxFloat(txtTolPreamp))
                txtPreAtt.BackColor = Color.Red;
            else txtPreAtt.BackColor = SystemColors.Control;
        }

        private float TextBoxFloat(TextBox txt)
        {
            try
            {
                return float.Parse(txt.Text);
            }
            catch (Exception)
            {
                return 0.0f;
            }
        }

        private float TextBoxFloat(TextBoxTS txt)
        {
            try
            {
                return float.Parse(txt.Text);
            }
            catch (Exception)
            {
                return 0.0f;
            }
        }

        #endregion

        #region Event Handlers

        private void ProductionTest_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "ProdTest");
        }

        private void btnCheckAll_Click(object sender, System.EventArgs e)
        {
            chk160.Checked = true;
            chk80.Checked = true;
            chk60.Checked = true;
            chk40.Checked = true;
            chk30.Checked = true;
            chk20.Checked = true;
            chk17.Checked = true;
            chk15.Checked = true;
            chk12.Checked = true;
            chk10.Checked = true;
            chk6.Checked = true;
        }

        private void btnClearAll_Click(object sender, System.EventArgs e)
        {
            chk160.Checked = false;
            chk80.Checked = false;
            chk60.Checked = false;
            chk40.Checked = false;
            chk30.Checked = false;
            chk20.Checked = false;
            chk17.Checked = false;
            chk15.Checked = false;
            chk12.Checked = false;
            chk10.Checked = false;
            chk6.Checked = false;
        }

        private void btnRunAllTests_Click(object sender, System.EventArgs e)
        {
            console.PowerOn = true;
            progress = new Progress("Signal Level Test");

            Thread t = new Thread(new ThreadStart(RunAll));
            t.Name = "ProdTestAll";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunSignal_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Signal Level Test");

            Thread t = new Thread(new ThreadStart(SignalTest));
            t.Name = "ProdTestSignal";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunNoise_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Noise Level Test");

            Thread t = new Thread(new ThreadStart(NoiseTest));
            t.Name = "ProdTestNoise";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunPreamp_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Preamp Level Test");

            Thread t = new Thread(new ThreadStart(PreampTest));
            t.Name = "ProdTestPreamp";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunImpulse_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Impulse Test");

            Thread t = new Thread(new ThreadStart(ImpulseTest));
            t.Name = "ImpulseTestPreamp";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void chkRunDot_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRunDot.Checked)
            {
                if (!console.PowerOn) console.PowerOn = true;
                chkRunDot.BackColor = Color.Yellow;
                DisableTests(chkRunDot);
                console.VFOAFreq = 14.2;
                console.RX1DSPMode = DSPMode.CWU;
                console.RX1Filter = Filter.F6;
                console.PWR = 100;

                console.Hdw.X2 |= 0x01;
            }
            else
            {
                EnableTests();
                console.Hdw.X2 &= 0xFE;
                chkRunDot.BackColor = SystemColors.Control;
            }
        }

        private void chkRunDash_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRunDash.Checked)
            {
                chkRunDash.BackColor = Color.Yellow;
                DisableTests(chkRunDash);
                console.VFOAFreq = 14.2;
                console.RX1DSPMode = DSPMode.CWU;
                console.RX1Filter = Filter.F6;
                console.PWR = 100;

                console.Hdw.X2 |= 0x02;
            }
            else
            {
                EnableTests();
                console.Hdw.X2 &= 0xFD;
                chkRunDash.BackColor = SystemColors.Control;
            }
        }

        private void btnRunRFE_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("RFE Test");

            Thread t = new Thread(new ThreadStart(RFETest));
            t.Name = "RFETest";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunBalance_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Balance Test");

            Thread t = new Thread(new ThreadStart(BalanceTest));
            t.Name = "BalanceTest";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunMic_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Mic Audio Test");

            Thread t = new Thread(new ThreadStart(MicTest));
            t.Name = "MicTest";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunPTT_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("PTT Test");

            Thread t = new Thread(new ThreadStart(PTTTest));
            t.Name = "PTTTest";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void btnRunTX_Click(object sender, System.EventArgs e)
        {
            progress = new Progress("Transmit Test");

            Thread t = new Thread(new ThreadStart(TransmitTest));
            t.Name = "TransmitTest";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void udGenFreq_ValueChanged(object sender, System.EventArgs e)
        {
            AD9854.SetFreq((double)udGenFreq.Value);
        }

        private void udGenLevel_ValueChanged(object sender, System.EventArgs e)
        {
            if ((int)udGenLevel.Value == 0) AD9854.PowerDownFullDigital = true;
            else AD9854.QAmp = AD9854.IAmp = (int)udGenLevel.Value;
        }

        private void btnGenReset_Click(object sender, System.EventArgs e)
        {
            AD9854.PowerDownFullDigital = false;
            AD9854.ResetDDS();
            udGenFreq_ValueChanged(this, EventArgs.Empty);
            udGenLevel_ValueChanged(this, EventArgs.Empty);
        }

        private void udGenClockCorr_ValueChanged(object sender, System.EventArgs e)
        {
            AD9854.ClockCorrection = (int)udGenClockCorr.Value;
            btnGenReset_Click(this, EventArgs.Empty);
        }

        private void mnuDebug_Click(object sender, System.EventArgs e)
        {
            if (debug_form == null || debug_form.IsDisposed)
                debug_form = new ProductionDebug(console);

            debug_form.Show();
            debug_form.Focus();
        }

        private void btnPrintResults_Click(object sender, System.EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.Landscape = true;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = "Date: " + DateTime.Today.ToShortDateString() + "  Time: " +
                DateTime.Now.ToString("HH:mm:ss") + "\n\n";
            text += "FlexRadio SDR-1000 Production Test Results";
            text += "\t\tSerial Number: " + txtSerialNum.Text + "\n";

            text += "\n   Signal Test:\n";
            text += "Band   |  160  |   80  |   60  |   40  |   30  |   20  |   17  |   15  |   12  |   10  |    6  |\n"
                + "------------------------------------------------------------------------------------------------\n"
                + "Ref    | " + txtRefSig160.Text + " | " + txtRefSig80.Text + " | " + txtRefSig60.Text + " | " + txtRefSig40.Text
                + " | " + txtRefSig30.Text + " | " + txtRefSig20.Text + " | " + txtRefSig17.Text + " | " + txtRefSig15.Text
                + " | " + txtRefSig12.Text + " | " + txtRefSig10.Text + " | " + txtRefSig6.Text + " |\n"
                + "Actual | " + txtSignal160H.Text + " | " + txtSignal80H.Text + " | " + txtSignal60H.Text + " | " + txtSignal40H.Text
                + " | " + txtSignal30H.Text + " | " + txtSignal20H.Text + " | " + txtSignal17H.Text + " | " + txtSignal15H.Text
                + " | " + txtSignal12H.Text + " | " + txtSignal10H.Text + " | " + txtSignal6H.Text + " |\n"
                + "------------------------------------------------------------------------------------------------\n"
                + "Delta  |  ";

            ArrayList delta = new ArrayList();
            delta.Add(txtSigDelta160);
            delta.Add(txtSigDelta80);
            delta.Add(txtSigDelta60);
            delta.Add(txtSigDelta40);
            delta.Add(txtSigDelta30);
            delta.Add(txtSigDelta20);
            delta.Add(txtSigDelta17);
            delta.Add(txtSigDelta15);
            delta.Add(txtSigDelta12);
            delta.Add(txtSigDelta10);
            delta.Add(txtSigDelta6);

            foreach (TextBoxTS txt in delta)
            {
                float num = TextBoxFloat(txt);
                if (num >= 0) text += " ";
                text += num.ToString("f1") + " |  ";
            }
            text += "\n";

            string impulse_str = null;
            if (btnRunImpulse.BackColor == Color.Green)
                impulse_str = "Passed.";
            else if (btnRunImpulse.BackColor == Color.Red)
                impulse_str = "FAILED.";
            else
                impulse_str = "Not Run";

            string balance_str = null;
            if (btnRunBalance.BackColor == Color.Green)
                balance_str = "Passed.";
            else if (btnRunBalance.BackColor == Color.Red)
                balance_str = "FAILED.";
            else
                balance_str = "Not Run";

            text += "\nNoise Test: \t\t\t\tPreamp Test:\t\t\tImpulse Test:\t\tBalance Test:\n";
            text += "Ref:    " + txtRefNoise20.Text + "\t\t\tGain: " + txtPreGain.Text + "\t\t\t" + impulse_str + "\t\t\t" + balance_str
                + "\nActual: " + txtNoise20.Text + "\t\t\tAtten: " + txtPreAtt.Text + "\n";

            if (txtComments.Text != "")
            {
                text += "\nComments: \n\n";
                for (int i = 0; i < txtComments.Text.Length; i += 80)
                    text += txtComments.Text.Substring(i, Math.Min(80, txtComments.Text.Length - i)) + "\n";
            }

            e.Graphics.DrawString(text, new Font("Lucida Console", 11), Brushes.Black, 80, 80);

        }

        #endregion
    }
}
