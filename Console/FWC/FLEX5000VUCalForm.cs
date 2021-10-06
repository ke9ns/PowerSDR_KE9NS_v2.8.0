//=================================================================
// FLEX5000VUCalForm.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
#if(!NO_MCL_PM)
using mcl_pm;
#endif

namespace PowerSDR
{
    public partial class FLEX5000VUCalForm : Form
    {
        private Progress p;
        private float barPercent = 0f;
        private Console console;

        public FLEX5000VUCalForm(Console c)
        {
            InitializeComponent();
            console = c;
            chkHighPowerCal.Checked = false;

            if (console.Production)
            {
                FLEX5000VUCalForm_KeyDown(this, new KeyEventArgs(Keys.Control | Keys.Alt | Keys.L));
                this.Height = 484;

                btnLevel.Enabled = true;
                btnPA.Enabled = true;
            }
        }

        #region Level

        private void btnLevel_Click(object sender, EventArgs e)
        {
            btnLevel.Enabled = false;
            btnPA.Enabled = false;
            btnTXCarrier.Enabled = false;
            btnLevel.BackColor = Color.Green;

            p = new Progress("Calibrate VU RX Level");
            p.Show();

            Thread t = new Thread(new ThreadStart(CalRXLevel));
            t.Name = "Calibrate RX Level Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void CalRXLevel()
        {
            const int NUM_LOOPS = 5;
            const int SETTLE_TIME = 200;
            const int SLEEP_TIME = 50;

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfoa = console.VFOAFreq;
            DSPMode dsp = console.RX1DSPMode;

            bool rx2 = console.RX2Enabled;
            console.RX2Enabled = false;

            bool rit_on = console.RITOn;						// save current RIT On
            console.RITOn = false;								// turn RIT off

            bool polyphase = console.setupForm.Polyphase;		// save current polyphase setting
            console.setupForm.Polyphase = false;				// disable polyphase

            int dsp_buf_size = console.setupForm.DSPPhoneRXBuffer;	// save current DSP buffer size
            console.setupForm.DSPPhoneRXBuffer = 4096;			// set DSP Buffer Size to 2048

            Filter filter = console.RX1Filter;					// save current filter

            PreampMode preamp = console.RX1PreampMode;

            DttSP.SetCorrectIQEnable(0); // turn off I/Q correction

            double[] freq = { 144.2, 432.6 };
            float[] level = { (float)udLevelV.Value, (float)udLevelU.Value };
            //float[] level = { -65.2f, -62.4f };

            int counter = 0;
            int total_counts = 0;
            int counts_per_band = NUM_LOOPS * 2 + SETTLE_TIME / SLEEP_TIME * 2;
            for (int i = 0; i < freq.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (ck2m.Checked)
                            total_counts += counts_per_band;
                        break;
                    case 1:
                        if (ck70cm.Checked)
                            total_counts += counts_per_band;
                        break;
                }
            }

            for (int i = 0; i < freq.Length; i++)
            {
                bool do_band = false;
                switch (i)
                {
                    case 0: do_band = ck2m.Checked; break;
                    case 1: do_band = ck70cm.Checked; break;
                }

                if (do_band)
                {
                    FWC.SetRCATX1(false);
                    switch (i)
                    {
                        case 0: FWC.SetRCATX2(false); break;
                        case 1: FWC.SetRCATX2(true); break;
                    }

                    double vfoa2 = console.VFOAFreq;
                    console.VFOAFreq = freq[i];

                    DSPMode dsp2 = console.RX1DSPMode;
                    console.RX1DSPMode = DSPMode.DSB;

                    Filter filter2 = console.RX1Filter;
                    console.RX1Filter = Filter.VAR1;

                    int var_low = console.RX1FilterLow;
                    int var_high = console.RX1FilterHigh;

                    console.RX1FilterLow = -1000;
                    console.RX1FilterHigh = 1000;

                    PreampMode preamp2 = console.RX1PreampMode;
                    console.RX1PreampMode = PreampMode.OFF;

                    switch (i)
                    {
                        case 0: FWC.SetVU_VIF(false); break;
                        case 1: FWC.SetVU_UIF(false); break;
                    }

                    for (int j = 0; j < SETTLE_TIME / SLEEP_TIME; j++)
                    {
                        Thread.Sleep(SLEEP_TIME); // allow things to settle
                        if (!p.Visible) goto end;
                        p.SetPercent((float)((float)++counter / total_counts));
                    }

                    // get the value of the signal strength meter
                    float num = 0.0f;
                    for (int j = 0; j < NUM_LOOPS; j++)
                    {
                        num += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        Thread.Sleep(50);
                        if (!p.Visible) goto end;
                        p.SetPercent((float)((float)++counter / total_counts));
                    }
                    float low_avg = num / NUM_LOOPS;

                    switch (i)
                    {
                        case 0: FWC.SetVU_VIF(true); break;
                        case 1: FWC.SetVU_UIF(true); break;
                    }

                    for (int j = 0; j < SETTLE_TIME / SLEEP_TIME; j++)
                    {
                        if (!p.Visible) goto end;
                        Thread.Sleep(SLEEP_TIME); // allow things to settle
                    }

                    num = 0.0f;
                    for (int j = 0; j < NUM_LOOPS; j++)
                    {
                        num += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                        Thread.Sleep(50);
                        if (!p.Visible) goto end;
                        p.SetPercent((float)((float)++counter / total_counts));
                    }
                    float high_avg = num / NUM_LOOPS;

                    // now we have the data ... do something useful with it
                    Debug.WriteLine(i + "- low: " + (low_avg + console.rx1_meter_cal_offset).ToString("f1") +
                        "  high: " + (high_avg + console.rx1_meter_cal_offset).ToString("f1"));

                    // insert pass/fail code here
                    bool b = ((Math.Abs(20.0 - (high_avg - low_avg)) < 1.0f) && // test for 20+/-1dB between low and high
                        (Math.Abs(-50.0 - (low_avg + console.rx1_meter_cal_offset)) < 10.0f)); // test for absolute value of low

                    string s = "Level Cal ";
                    switch (i)
                    {
                        case 0: s += "2m: "; break;
                        case 1: s += "70cm: "; break;
                    }

                    if (b)
                    {
                        s += "Passed";
                    }
                    else
                    {
                        s += "Failed";
                        btnLevel.BackColor = Color.Red;
                    }

                    float low_offset = level[i] - (low_avg + console.rx1_meter_cal_offset);
                    float high_offset = level[i] - (high_avg + console.rx1_meter_cal_offset);

                    switch (i)
                    {
                        case 0:
                            console.vhf_level_table[0] = low_offset;
                            console.vhf_level_table[1] = high_offset;
                            break;
                        case 1:
                            console.uhf_level_table[0] = low_offset;
                            console.uhf_level_table[1] = high_offset;
                            break;
                    }

                    s += " (" + (low_avg + console.rx1_meter_cal_offset).ToString("f1") + ", " +
                            low_offset.ToString("f1") + ", " +
                            high_offset.ToString("f1") + ", " +
                            (high_avg - low_avg).ToString("f1") + ")";

                    lstDebug.Items.Insert(0, s);

                end:
                    FWC.SetRCATX2(false);
                    switch (i)
                    {
                        case 0: FWC.SetVU_VIF(console.xvtrForm.VIFGain); break;
                        case 1: FWC.SetVU_UIF(console.xvtrForm.UIFGain); break;
                    }
                    console.RX1PreampMode = preamp2;
                    console.RX1FilterLow = var_low;
                    console.RX1FilterHigh = var_high;
                    console.RX1Filter = filter2;
                    console.RX1DSPMode = dsp2;
                    console.VFOAFreq = vfoa2;
                }
            }

            p.Hide();

            console.VFOAFreq = vfoa;
            console.RX1DSPMode = dsp;
            console.RX2Enabled = rx2;
            console.RITOn = rit_on;
            console.setupForm.Polyphase = polyphase;
            console.setupForm.DSPPhoneRXBuffer = dsp_buf_size;
            console.RX1Filter = filter;
            console.RX1PreampMode = preamp;

            DttSP.SetCorrectIQEnable(1); // turn I/Q correction back on

            if (p.Text != "") // test for abort
            {
                lstDebug.Items.Insert(0, "Saving Level data to EEPROM...");
                byte checksum;
                FWCEEPROM.WriteVULevel(console.vhf_level_table, console.uhf_level_table, out checksum);
                console.vu_level_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving Level data to EEPROM...done";
            }

            switch (console.RX1Band)
            {
                case Band.VHF0:
                    if (!console.xvtrForm.VIFGain)
                        console.RX1XVTRGainOffset = console.vhf_level_table[0];
                    else
                        console.RX1XVTRGainOffset = console.vhf_level_table[1];
                    break;
                case Band.VHF1:
                    if (!console.xvtrForm.UIFGain)
                        console.RX1XVTRGainOffset = console.uhf_level_table[0];
                    else
                        console.RX1XVTRGainOffset = console.uhf_level_table[1];
                    break;
            }

            switch (console.RX2Band)
            {
                case Band.VHF0:
                    if (!console.xvtrForm.VIFGain)
                        console.RX2XVTRGainOffset = console.vhf_level_table[0];
                    else
                        console.RX2XVTRGainOffset = console.vhf_level_table[1];
                    break;
                case Band.VHF1:
                    if (!console.xvtrForm.UIFGain)
                        console.RX2XVTRGainOffset = console.uhf_level_table[0];
                    else
                        console.RX2XVTRGainOffset = console.uhf_level_table[1];
                    break;
            }

            btnLevel.Enabled = true;
            btnPA.Enabled = true;
            btnTXCarrier.Enabled = true;
        }

        #endregion

        #region PA

        private void btnPA_Click(object sender, EventArgs e)
        {

            btnLevel.Enabled = false;
            btnPA.Enabled = false;
            btnTXCarrier.Enabled = false;
            btnPA.BackColor = Color.Green;

            p = new Progress("Calibrate PA Gain");
            p.Show();

            Thread t = new Thread(new ThreadStart(CalPAGain));
            t.Name = "Calibrate PA Gain Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

        }

        private void CalPAGain()
        {
            #region INIT

            FWC.SetRCATX1(true);
            lstDebug.Items.Insert(0, "PA Gain Cal Begin...");
            barPercent = 0f;
#if(!NO_MCL_PM)
            USB_PM Sensor = new USB_PM();
            short val = Sensor.Open_AnySensor();
#else
            short val = 0;
#endif
            int errorCount = 0;
            int count_progressBar = 0;
            string powerString = "powerString not set";

            Thread.Sleep(50);

            if (val != 1)
            {
                p.Text = "";
                p.Hide();
                lstDebug.Items.Insert(0, "PA Gain Cal Failed: No power sensor found");
                MessageBox.Show("Error: Unable to find Power Sensor.\n" +
                    "Please ensure the power sensor is connected to the PC and try again.\n" +
                    "Make sure all Mini-Circuits Power Sensor application windows are closed.",
                    "Error: Sensor Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                btnLevel.Enabled = true;
                btnPA.Enabled = true;
                btnPA.BackColor = Color.Red;
                return;
            }

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(100);
            }

            if (console.RXOnly)
            {
                p.Text = "";
                p.Hide();
                lstDebug.Items.Insert(0, "PA Gain Cal Failed: RX Only selected");
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnPA.BackColor = Color.Red;
                btnPA.Enabled = true;
                btnLevel.Enabled = true;
                return;
            }

            float pad = (float)udSensorPad.Value;
            float low_tol;
            float high_tol;

            if (chkHighPowerCal.Checked)
            {
                low_tol = (float)udPAHPTargetTolLow.Value;
                high_tol = (float)udPAHPTargetTolHigh.Value;
            }
            else
            {
                low_tol = (float)udPALPTargetTolLow.Value;
                high_tol = (float)udPALPTargetTolHigh.Value;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            bool split = console.VFOSplit;
            console.VFOSplit = false;

            bool vfob_tx = console.VFOBTX;
            console.VFOBTX = false;

            bool leveler = console.dsp.GetDSPTX(0).TXLevelerOn;
            console.dsp.GetDSPTX(0).TXLevelerOn = false;

            double vfoa = console.VFOAFreq;
            DSPMode dsp = console.RX1DSPMode;

            Band[] bands = { Band.VHF0, Band.VHF1 };

            for (int i = 0; i < bands.Length; i++)
            {
                double[] freqs = new double[0];
                bool do_band = false;

                switch (bands[i])
                {
                    case Band.VHF0: do_band = ck2m.Checked; freqs = console.freqs_2m; break;
                    case Band.VHF1: do_band = ck70cm.Checked; freqs = console.freqs_70cm; break;
                }

                float target;
                if (do_band)
                {
                    float loss = 0.0f;
                    console.VFOAFreq = freqs[0];
                    if (chkHighPowerCal.Checked)
                    {
                        target = (float)udPAHPTarget.Value + pad;
                        FWC.SetVU_VPA(true);
                        FWC.SetVU_UPA(true);
                    }
                    else
                    {
                        target = (float)udPATarget.Value + pad;
                        FWC.SetVU_VPA(false);
                        FWC.SetVU_UPA(false);
                    }
                    Thread.Sleep(100);
                    console.MOX = true;
                    Thread.Sleep(200);
                    switch (i)
                    {
                        case 0:
                            FWC.SetRCATX2(false);
                            loss = (float)udVBoxLoss.Value;
                            target += loss;
                            break;
                        case 1:
                            FWC.SetRCATX2(true);
                            loss = (float)udUBoxLoss.Value;
                            target += loss;
                            break;
                    }
                    count_progressBar = 0;
                    for (int j = freqs.Length - 1; j >= 0; j--)
                    {
                        count_progressBar++;
#if(!NO_MCL_PM)
                        Sensor.Freq = freqs[j];
#endif
                        console.VFOAFreq = freqs[j];
                        DSPMode dsp_mode = console.RX1DSPMode;
                        console.RX1DSPMode = DSPMode.USB;
                        console.VFOAFreq = freqs[j]; // reset freq in case mode change moved it  
                        Audio.TXInputSignal = Audio.SignalSource.SINE;
                        Audio.SourceScale = 1.0;
                        //.Audio.RadioVolume = 0.02;
                        Audio.RadioVolume = (double)udRadioVolumeStarting.Value;

                        for (int x = 0; x < 10; x++)
                        {
                            if (!p.Visible) goto end;
                            Thread.Sleep(50);
                        }
                        #endregion

                        #region CALCULATE AND SAVE
#if (!NO_MCL_PM)
                        float read_db = Sensor.ReadPower();
#else
                        float read_db = 0.0f;
#endif
                        Debug.WriteLine("Uncorrected_read_db " + read_db);
                        float powerDifference_dB = target - read_db;
                        float vMultiplier = (float)Math.Pow(10, powerDifference_dB / 20);

                        if (vMultiplier * Audio.RadioVolume < (double)udMaxRadioVolume.Value)
                        {
                            Audio.RadioVolume = vMultiplier * Audio.RadioVolume;
                            for (int k = 0; k < 10; k++)
                            {
                                if (!p.Visible) goto end;
                                Thread.Sleep(50);
                            }
#if (!NO_MCL_PM)
                            read_db = Sensor.ReadPower();
#endif

                            //Calculate, correct and save Audio.RadioVolume (udAttempts.Value number of tries)

                            for (int k = 0; k < udAttempts.Value; k++)  //number of calibration attempts
                            {
                                //progress bar update
                                if (ck2m.Checked && ck70cm.Checked)
                                {
                                    barPercent = barPercent + 0.1f / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                                }
                                else
                                {
                                    if (i == 0)
                                        barPercent = barPercent + 0.1f / (float)(console.freqs_2m.Length);
                                    else if (i == 1)
                                        barPercent = barPercent + 0.1f / (float)(console.freqs_70cm.Length);
                                }
                                if (barPercent > 100)
                                    barPercent = 100.0f;
                                p.SetPercent(barPercent);

                                if (Audio.RadioVolume > (double)udMaxRadioVolume.Value)
                                {
                                    Audio.RadioVolume = (double)udMaxRadioVolume.Value;
                                    k = (int)udAttempts.Value; ;
                                }
                                if ((read_db > target + high_tol || read_db < target + low_tol)) //if tolerance not met, try to correct
                                {
                                    for (int x = 0; x < 10; x++)
                                    {
                                        if (!p.Visible) goto end;
                                        Thread.Sleep(50);
                                    }
#if(!NO_MCL_PM)
                                    read_db = Sensor.ReadPower();
#endif
                                    powerDifference_dB = target - read_db;
                                    vMultiplier = (float)Math.Pow(10, powerDifference_dB / 20);

                                    if (vMultiplier * Audio.RadioVolume < (double)udMaxRadioVolume.Value)
                                    {
                                        Audio.RadioVolume = vMultiplier * Audio.RadioVolume;
                                        for (int x = 0; x < 10; x++)
                                        {
                                            if (!p.Visible) goto end;
                                            Thread.Sleep(50);
                                        }
#if(!NO_MCL_PM)
                                        read_db = Sensor.ReadPower();
#endif
                                    }

                                    Debug.WriteLine("***k = " + k);
                                    if (chkHighPowerCal.Checked)
                                        powerString = " Power: " + dBm_to_watts(read_db - pad - loss).ToString("f1") + " W";
                                    else
                                        powerString = " Power: " + (read_db - pad - loss).ToString("f2") + " dBm";

                                    if (bands[i] == Band.VHF0)
                                        Debug.WriteLine("(2)At " + console.freqs_2m[j] + " Audio.RadioVolume = " + Audio.RadioVolume + powerString);
                                    else if (bands[i] == Band.VHF1)
                                        Debug.WriteLine("(2)At " + console.freqs_70cm[j] + " Audio.RadioVolume = " + Audio.RadioVolume + powerString);
                                }
                                else //tolerance has been met, save value
                                {
                                    if (chkHighPowerCal.Checked)
                                        powerString = " Power: " + dBm_to_watts(read_db - pad - loss).ToString("f1") + " W";
                                    else
                                        powerString = " Power: " + (read_db - pad - loss).ToString("f2") + " dBm";

                                    if (bands[i] == Band.VHF0)
                                    {
                                        lstDebug.Items.Insert(0, "PA " + console.freqs_2m[j].ToString("f1") + ": " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                        console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                    }
                                    else if (bands[i] == Band.VHF1)
                                    {
                                        lstDebug.Items.Insert(0, "PA " + console.freqs_70cm[j].ToString("f1") + ": " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                        console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                    }

                                    //progress bar update
                                    if (ck2m.Checked && ck70cm.Checked) //will take twice as long if both are checked
                                    {
                                        //barPercent = barPercent + 1f / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                                        if (i == 1)
                                            barPercent = (float)(console.freqs_2m.Length + count_progressBar + 1) / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                                        else if (i == 0)
                                            barPercent = (float)(count_progressBar + 1) / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                                    }
                                    else
                                    {
                                        if (bands[i] == Band.VHF0)
                                            barPercent = (j + 1) / (float)(console.freqs_2m.Length);
                                        else if (bands[i] == Band.VHF1)
                                            barPercent = (j + 1) / (float)(console.freqs_70cm.Length);
                                    }
                                    if (barPercent > 100)
                                        barPercent = 100.0f;
                                    p.SetPercent(barPercent);
                                    goto end;
                                }
                            }//end 10 tries
                            #endregion  //end region CALCULATE AND SAVE

                            #region DEAL WITH UNMET CONDITIONS

                            //ten tries over, conditions not met
                            if (chkHighPowerCal.Checked)
                                powerString = " Power: " + dBm_to_watts(read_db - pad - loss).ToString("f1") + " W";
                            else
                                powerString = " Power: " + (read_db - pad - loss).ToString("f2") + " dBm";

                            //low power
                            if (!chkHighPowerCal.Checked)
                            {
                                if (i == 0)
                                {
                                    lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_2m[j].ToString("f1") + " : " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                }
                                else if (i == 1)
                                {
                                    lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_70cm[j].ToString("f1") + " : " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                }

                                btnPA.BackColor = Color.Red;
                            }

                            //high power V (144 MHz - 148 MHz), Power < 53.95 W
                            else if ((read_db - pad - loss) <= 47.32 && chkHighPowerCal.Checked && i == 0)
                            {
                                lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_2m[j].ToString("f1") + " : " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                btnPA.BackColor = Color.Red;
                            }
                            //high power U (420 MHz - 440 MHz), Power < 53.95 W
                            else if ((read_db - pad - loss) <= 47.32 && chkHighPowerCal.Checked && i == 1 && console.freqs_70cm[j] <= 440)
                            {
                                lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_70cm[j].ToString("f1") + " : Power < 54W (Power: " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                btnPA.BackColor = Color.Red;
                            }
                            //high power, U (440 MHz - 448 MHz), Power < 49.89 W
                            else if ((read_db - pad - loss) <= 46.98 && chkHighPowerCal.Checked && i == 1 && console.freqs_70cm[j] >= 440 && console.freqs_70cm[j] <= 448)
                            {
                                lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_70cm[j].ToString("f1") + " : Power < 50W (Power: " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                btnPA.BackColor = Color.Red;
                            }
                            //high power, U (448 MHz - 450 MHz), Power < 39.99 W
                            else if ((read_db - pad - loss) <= 46.02 && chkHighPowerCal.Checked && i == 1 && console.freqs_70cm[j] >= 448)
                            {
                                lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_70cm[j].ToString("f1") + " : Power < 40W (Power: " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                btnPA.BackColor = Color.Red;
                            }

                            //Power > 63.09 W, V or U, Power is too high
                            else if ((read_db - pad - loss) > 48.0 && chkHighPowerCal.Checked)
                            {
                                if (i == 0)
                                {
                                    lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_2m[j].ToString("f1") + " : Power > 63W (Power:" + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                }
                                else if (i == 1)
                                {
                                    lstDebug.Items.Insert(0, "*PA Cal Failed " + console.freqs_70cm[j].ToString("f1") + " : Power > 63W (Power:" + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                }
                                btnPA.BackColor = Color.Red;
                            }

                            //successful, but original tight tolerance not met (60 W)
                            else
                            {
                                if (chkHighPowerCal.Checked)
                                    powerString = " Power: " + dBm_to_watts(read_db - pad - loss).ToString("f1") + " W";
                                else
                                    powerString = " Power: " + (read_db - pad - loss).ToString("f2") + " dBm";

                                Debug.WriteLine("60W requirement not met:  Audio.RadioVolume = " + Audio.RadioVolume + "  Power = " + (read_db - pad - loss).ToString("f2"));
                                if (bands[i] == Band.VHF0)
                                {
                                    Debug.WriteLine("(2)At " + console.freqs_2m[j].ToString("f1") + " Audio.RadioVolume = " + Audio.RadioVolume + " : " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    lstDebug.Items.Insert(0, "*PA " + console.freqs_2m[j].ToString("f1") + ": " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    //need to save to power table!
                                    console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                }
                                else if (bands[i] == Band.VHF1)
                                {
                                    Debug.WriteLine("(2)At " + console.freqs_70cm[j].ToString("f1") + " Audio.RadioVolume = " + Audio.RadioVolume + " : " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    lstDebug.Items.Insert(0, "*PA " + console.freqs_70cm[j].ToString("f1") + ": " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                                    //need to save to power table!
                                    console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                }
                            }
                            //progress bar update
                            if (ck2m.Checked && ck70cm.Checked) //will take twice as long if both are checked
                            {
                                //barPercent = barPercent + 1f / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                                if (i == 1)
                                    barPercent = (float)(console.freqs_2m.Length + count_progressBar + 1) / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                                else if (i == 0)
                                    barPercent = (float)(count_progressBar + 1) / (float)(console.freqs_2m.Length + console.freqs_70cm.Length);
                            }
                            else
                            {
                                if (bands[i] == Band.VHF0)
                                    barPercent = (float)(j + 1) / (float)(console.freqs_2m.Length);
                                else if (bands[i] == Band.VHF1)
                                    barPercent = (float)(j + 1) / (float)(console.freqs_70cm.Length);
                            }
                            if (barPercent > 100)
                                barPercent = 100.0f;
                            p.SetPercent(barPercent);
                        }

                        //volume too high at very beginning, power sensor probably not connected
                        else
                        {
                            if (chkHighPowerCal.Checked)
                                powerString = " Power: " + dBm_to_watts(read_db - pad - loss).ToString("f1") + " W";
                            else
                                powerString = " Power: " + (read_db - pad - loss).ToString("f2") + " dBm";

                            if (bands[i] == Band.VHF0)
                                lstDebug.Items.Insert(0, "*PA " + console.freqs_2m[j].ToString("f1") + ": Error, Check Connections " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");
                            else if (bands[i] == Band.VHF1)
                                lstDebug.Items.Insert(0, "*PA " + console.freqs_70cm[j].ToString("f1") + ": Error, Check Connections " + powerString + " (" + Audio.RadioVolume.ToString("f4") + ")");

                            //Audio.RadioVolume = 0.02;
                            //try this frequency 4 times before quitting
                            if (errorCount < 4)
                            {
                                errorCount++;
                                j--;
                            }

                            else
                            {
                                if (chkHighPowerCal.Checked)
                                    powerString = " Power: " + dBm_to_watts(read_db - pad - loss).ToString("f1") + " W";
                                else
                                    powerString = " Power: " + (read_db - pad - loss).ToString("f2") + " dBm";

                                Debug.WriteLine("Cal ERROR:  Audio.RadioVolume too high.  Is the Power Sensor connected to the correct port?");
                                if (bands[i] == Band.VHF0)
                                {
                                    lstDebug.Items.Insert(0, "*PA " + console.freqs_2m[j].ToString("f1") + ": Error, Check Connections " + powerString + " (" + (Audio.RadioVolume * vMultiplier).ToString("f4") + ")");

                                    if (Audio.RadioVolume * vMultiplier > (double)udMaxRadioVolume.Value)
                                    {
                                        lstDebug.Items.Insert(0, "***Setting Radio Volume to Max:" + udMaxRadioVolume.Value.ToString("f4") + "***");
                                        Audio.RadioVolume = (double)udMaxRadioVolume.Value;
                                        console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                    }
                                    //console.vhf_power_table[j] = (float)Audio.RadioVolume;
                                }
                                else if (bands[i] == Band.VHF1)
                                {
                                    lstDebug.Items.Insert(0, "*PA " + console.freqs_70cm[j].ToString("f1") + ": Error, Check Connections " + powerString + " (" + (Audio.RadioVolume * vMultiplier).ToString("f4") + ")");
                                    if (Audio.RadioVolume * vMultiplier > (double)udMaxRadioVolume.Value)
                                    {
                                        lstDebug.Items.Insert(0, "***Setting Radio Volume to Max:" + udMaxRadioVolume.Value.ToString("f4") + "***");
                                        Audio.RadioVolume = (double)udMaxRadioVolume.Value;
                                        console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                    }
                                    //console.uhf_power_table[j] = (float)Audio.RadioVolume;
                                }

                                btnPA.BackColor = Color.Red;
                                console.MOX = false;
                                goto end2;
                            }
                        }
                    #endregion

                    end:

                        Audio.TXInputSignal = Audio.SignalSource.RADIO;
                        console.RX1DSPMode = dsp_mode;
                        if (!p.Visible) break;
                    } // end of inside loop

                    console.MOX = false;
                    if (!p.Visible)
                        goto end2;
                }
            } // end of outside loop

        end2:
            p.Hide();

            console.dsp.GetDSPTX(0).TXLevelerOn = leveler;
            console.VFOBTX = vfob_tx;
            console.VFOSplit = split;
            console.VFOAFreq = vfoa;
            console.RX1DSPMode = dsp;
            console.VFOAFreq = vfoa;

            btnLevel.Enabled = true;
            btnPA.Enabled = true;
            btnTXCarrier.Enabled = true;

#if(!NO_MCL_PM)
            Sensor.Close_Sensor();
#endif
            FWC.SetRCATX1(false);
            FWC.SetRCATX2(false);

            //restore original PA states
            FWC.SetVU_VPA(console.xvtrForm.VPA);
            FWC.SetVU_UPA(console.xvtrForm.UPA);

            if (p.Text != "") // test for abort
            {
                lstDebug.Items.Insert(0, "Saving Power data to EEPROM...");
                byte checksum;
                FWCEEPROM.WriteVUPower(console.vhf_power_table, console.uhf_power_table, out checksum);
                writeVU5KPALog();
                console.vu_power_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving Power data to EEPROM...done";
            }
        }
        #endregion

        private void FLEX5000VUCalForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.L)
            {
                lblPATarget.Visible = true;
                udPATarget.Visible = true;
                lblPAHPTargetTolLow.Visible = true;
                udPAHPTargetTolLow.Visible = true;
                lblPAHPTargetTolHigh.Visible = true;
                udPAHPTargetTolHigh.Visible = true;
                lblPALPTargetTolLow.Visible = true;
                udPALPTargetTolLow.Visible = true;
                lblPALPTargetTolHigh.Visible = true;
                udPALPTargetTolHigh.Visible = true;
                lblSensorPad.Visible = true;
                udSensorPad.Visible = true;
                lblVBoxLoss.Visible = true;
                udVBoxLoss.Visible = true;
                lblUBoxLoss.Visible = true;
                udUBoxLoss.Visible = true;
                chkHighPowerCal.Visible = true;
                udPAHPTarget.Visible = true;
                lblPAHPTarget.Visible = true;
                lblMaxRadioVolume.Visible = true;
                udMaxRadioVolume.Visible = true;
                lblLevelV.Visible = true;
                udLevelV.Visible = true;
                lblLevelU.Visible = true;
                udLevelU.Visible = true;
                lblAttampts.Visible = true;
                udAttempts.Visible = true;
                lblRadioVolumeStarting.Visible = true;
                udRadioVolumeStarting.Visible = true;
                ckPwrRemap.Visible = true;
            }
        }

        private void FLEX5000VUCalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void chkHighPowerCal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHighPowerCal.Checked)
            {

                DialogResult dr = MessageBox.Show("Warning: Enabling High Power Cal.\n" +
                                "Power Sensor may get damaged if not properly connected.\n",
                                "Warning: High Power Cal",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    chkHighPowerCal.Checked = false;

            }
        }

        private float dBm_to_watts(float power_dBm)
        {
            return (float)Math.Pow(10, ((power_dBm - 30) / 10));
        }

        private void writeVU5KPALog()
        {
            //Band[] bands = { Band.VHF0, Band.VHF1 };
            //double[] freqs = new double[0];

            string path = console.AppDataPath + "\\Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\VU5K_PA_Cal.csv");
            StreamWriter writer = new StreamWriter(path + "\\VU5K_PA_Cal.csv", true);
            //if (!file_exists) 
            writer.WriteLine("PA Serial Num, Date/Time, Version,");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.SerialNumber) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            writer.WriteLine("");
            writer.Write("vhf_power_table,");
            for (int j = 0; j < console.vhf_power_table.Length; j++)
            {
                writer.Write(console.vhf_power_table[j].ToString() + ",");
            }
            writer.WriteLine("");
            writer.Write("uhf_power_table,");
            for (int j = 0; j < console.uhf_power_table.Length; j++)
            {
                writer.Write(console.uhf_power_table[j].ToString() + ",");
            }
            writer.WriteLine("");
            writer.Close();
        }

        private void btnTXCarrier_Click(object sender, EventArgs e)
        {
            btnLevel.Enabled = false;
            btnPA.Enabled = false;
            btnTXCarrier.Enabled = false;
            btnTXCarrier.BackColor = Color.Green;

            p = new Progress("Calibrate VU TX Carrier");
            p.Show();

            Thread t = new Thread(new ThreadStart(CalTXCarrier));
            t.Name = "Calibrate VU TX Carrier Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        string test_tx_carrier = "TX Carrier Test: Not Run";
        public void CalTXCarrier()
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
            List<double> band_freqs = new List<double>();

            band_freqs.Add(144.2);
            const int NUM_V_POINTS = 9;
            for (int i = 0; i < NUM_V_POINTS; i++)
                band_freqs.Add(144.0 + (148.0 - 144.0) / (NUM_V_POINTS - 1) * i);

            band_freqs.Add(432.1);
            band_freqs.Add(432.2);
            const int NUM_U_POINTS = 41;
            for (int i = 0; i < NUM_U_POINTS; i++)
                band_freqs.Add(430.0 + (450.0 - 430.0) / (NUM_U_POINTS - 1) * i);

            int begin = 0;
            if (!ck2m.Checked && ck70cm.Checked) // skip V freqs
                begin = NUM_V_POINTS + 1;

            int end = band_freqs.Count;
            if (ck2m.Checked && !ck70cm.Checked)
                end = NUM_V_POINTS + 1;
            else if (!ck2m.Checked && !ck70cm.Checked)
                end = 0;

            for (int i = begin; i < end; i++)
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

                float min = console.tx_carrier_min;
                if (min > tol)
                {
                    if (!test_tx_carrier.StartsWith("TX Carrier: Failed ("))
                        test_tx_carrier = "TX Carrier: Failed (";
                    test_tx_carrier += band_freqs[i].ToString("f1") + ",";
                    btnTXCarrier.BackColor = Color.Red;
                    lstDebug.Items.Insert(0, "TX Carrier - " + band_freqs[i].ToString("f1") + ": Failed ("
                        + min.ToString("f1") + ")");
                }
                else
                {
                    lstDebug.Items.Insert(0, "TX Carrier - " + band_freqs[i].ToString("f1") + ": Passed ("
                        + min.ToString("f1") + ")");
                }
                Thread.Sleep(500);
            }

            console.TXFilterLow = tx_low;
            console.TXFilterHigh = tx_high;

            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;

            if (test_tx_carrier.StartsWith("TX Carrier Test: Failed ("))
                test_tx_carrier = test_tx_carrier.Substring(0, test_tx_carrier.Length - 1) + ")";

            t1.Stop();
            Debug.WriteLine("TX Carrier Timer: " + t1.Duration);

            string path = console.AppDataPath + "\\Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "\\TX Carrier";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            StreamWriter writer = new StreamWriter(path + "\\vu_tx_carrier_F5K_" + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ".csv");
            writer.WriteLine("Freq, C0, C1, C2, C3, From Noise");
            for (int i = 0; i < band_freqs.Count; i++)
            {
                writer.Write(band_freqs[i].ToString("f6") + ",");
                for (int j = 0; j < 4; j++)
                {
                    double key;
                    console.FindNearestKey(band_freqs[i], console.tx_carrier_cal, out key);
                    uint val = console.tx_carrier_cal[key];
                    writer.Write((val >> 8 * (3 - j)).ToString() + ",");
                    //writer.Write((console.tx_carrier_cal[Math.Round(band_freqs[i], 3)] >> 8 * (3 - j)).ToString() + ",");
                }
                //writer.WriteLine(console.min_tx_carrier[(int)bands[i]].ToString("f1"));
            }
            writer.Close();

            lstDebug.Items.Insert(0, "Saving Carrier data to EEPROM...");
            byte checksum;
            FWCEEPROM.WriteTXCarrier(console.tx_carrier_cal, out checksum);
            console.tx_carrier_checksum = checksum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving Carrier data to EEPROM...done";

            MessageBox.Show("TX Carrier Done: " + t1.Duration);

            btnLevel.Enabled = true;
            btnPA.Enabled = true;
            btnTXCarrier.Enabled = true;
        }

        private void chkPwrRemap_CheckedChanged(object sender, EventArgs e)
        {
            console.Enable_VU_Power_Curve = ckPwrRemap.Checked;
        }
    }
}
