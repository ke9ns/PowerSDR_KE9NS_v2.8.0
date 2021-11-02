//=================================================================
// fwcatuform.cs
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
using System.Diagnostics;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for fwcatuform.
    /// </summary>
    public partial class FWCATUForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       

        #endregion

        #region Constructor and Destructor

        public FWCATUForm(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;
            comboSWRThresh.Text = "3.0 : 1";
            Common.RestoreForm(this, "FWCATU", false);

            if (radModeSemiAuto.Checked)
            {
                radModeBypass.Checked = true;
            }
            else if (radModeBypass.Checked)
            {
                radModeBypass_CheckedChanged(this, EventArgs.Empty);
            }
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

        

        #region Properties

        private FWCATUMode current_tune_mode = FWCATUMode.Bypass;
        public FWCATUMode CurrentTuneMode
        {
            get { return current_tune_mode; }
            set
            {
                switch (value)
                {
                    case FWCATUMode.Bypass:
                        radModeBypass.Checked = true;
                        break;
                    case FWCATUMode.SemiAutomatic:
                        radModeSemiAuto.Checked = true;
                        break;
                    case FWCATUMode.Automatic:
                        radModeAuto.Checked = true;
                        break;
                }
            }
        }
        public bool ATUEnabledOnBandChange()
        {
            if (chkATUEnabledOnBandChange.Checked) return true;
            else return false;
        }

        #endregion

        #region Event Handlers

        private void FWCATUForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FWCATU");
        }

        public void DoBypass()
        {
            radModeBypass.Checked = true;
        }

        private void radModeBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
            if (radModeBypass.Checked)
            {
                if (FWCATU.AutoStatus == 1)
                    FWCATU.AutoTuning(false);
                if (FWCATU.Active)
                    FWCATU.Activate(false);

                if (FWC.old_atu == true)
                {
                    FWC.ATUSendCmd(9, 0, 0);   //prevent
                    Thread.Sleep(200);
                }
                else
                {
                    //Debug.WriteLine("setting antenna 1...");
                    FWCATU.SelectAntenna1();
                }
                current_tune_mode = FWCATUMode.Bypass;
                console.FWCATUBypass();

            }
            grpTune.Enabled = !radModeBypass.Checked;
        }

        private void radModeSemiAuto_CheckedChanged(object sender, System.EventArgs e)
        {

            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
            if (radModeSemiAuto.Checked)
            {
                if (!FWCATU.Active)
                    FWCATU.Activate(true);
                if (FWCATU.AutoStatus == 1)
                    FWCATU.AutoTuning(false);

                if (FWC.old_atu == false)  //if new model
                {
                    Debug.WriteLine("setting antenna 2...");
                    FWCATU.SelectAntenna2();
                }

                current_tune_mode = FWCATUMode.SemiAutomatic;
            }
        }

        private void radModeAuto_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
            if (radModeAuto.Checked)
            {
                if (!FWCATU.Active)
                    FWCATU.Activate(true);
                if (FWCATU.AutoStatus == 0)
                    FWCATU.AutoTuning(true);

                if (FWC.old_atu == false)
                {
                    Debug.WriteLine("setting antenna 2...");
                    FWCATU.SelectAntenna2();
                }
                current_tune_mode = FWCATUMode.Automatic;
            }
        }

        public void DoTuneMemory()
        {
            radModeSemiAuto.Checked = true;
            chkUseTUN.Checked = true;
            btnTuneMemory_Click(this, EventArgs.Empty);
        }

        private void btnTuneMemory_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(TuneMemory));
            t.Name = "Memory Tune Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void TuneMemory()
        {
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
            int old_tun_pwr = 50;
            btnTuneMemory.BackColor = console.ButtonSelectedColor;
            if (chkUseTUN.Checked)
            {
                console.TUN = true;
                old_tun_pwr = console.PWR;
                console.PWR = 10;
            }
            FWCATU.MemoryTune();
            if (chkUseTUN.Checked)
            {
                console.PWR = old_tun_pwr;
                console.TUN = false;
            }
            btnTuneMemory.BackColor = SystemColors.Control;
            UpdateFeedback();
            console.FWCATUTuned();
        }

        public void DoTuneFull()
        {
            radModeSemiAuto.Checked = true;
            chkUseTUN.Checked = true;
            btnTuneFull_Click(this, EventArgs.Empty);
        }

        private void btnTuneFull_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(TuneFull));
            t.Name = "Full Tune Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void TuneFull()
        {
            if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;
            int old_tun_pwr = 50;
            btnTuneFull.BackColor = console.ButtonSelectedColor;
            if (chkUseTUN.Checked)
            {
                console.TUN = true;
                old_tun_pwr = console.PWR;
                console.PWR = 10;
            }
            FWCATU.FullTune();
            if (chkUseTUN.Checked)
            {
                console.PWR = old_tun_pwr;
                console.TUN = false;
            }
            btnTuneFull.BackColor = SystemColors.Control;
            UpdateFeedback();
            console.FWCATUTuned();
        }

        private void UpdateFeedback()
        {
            if (FWCATU.TunePass)
            {
                lblTuneComplete.ForeColor = Color.Green;
                lblTuneComplete.Text = "Tune Completed Successfully";
                lblFreq.Text = "Freq (MHz): " + FWCATU.TXFreq.ToString("f2");
                lblForward.Text = "Forward: " + FWCATU.ForwardPower.ToString("f0");
                lblReflected.Text = "Reflected: " + FWCATU.ReflectedPower.ToString("f0");

                lblSWR.Text = "SWR: " + FWCATU.SWR.ToString("f1");
                lblCAP.Text = "CAP: " + FWCATU.CapacitorValue.ToString();
                lblIND.Text = "IND: " + FWCATU.InductorValue.ToString();

            }
            else
            {
                lblTuneComplete.ForeColor = Color.Red;
                switch (FWCATU.TuneFail)
                {
                    case 0:
                        lblTuneComplete.Text = "Tune Failed: No RF Detected";
                        break;
                    case 1:
                        lblTuneComplete.Text = "Tune Failed: RF Carrier Lost";
                        break;
                    case 2:
                        lblTuneComplete.Text = "Tune Failed: Could Not Bring Down SWR";
                        break;
                }
                lblFreq.Text = "Freq (MHz):";
                lblForward.Text = "Forward:";
                lblReflected.Text = "Reflected:";
                lblSWR.Text = "SWR:";
                lblCAP.Text = "CAP:";
                lblIND.Text = "IND:";
            }
        }

        private void comboSWRThresh_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            double swr_thresh = 0.0;
            switch ((byte)comboSWRThresh.SelectedIndex)
            {
                case 0: swr_thresh = 1.1; break;
                case 1: swr_thresh = 1.3; break;
                case 2: swr_thresh = 1.5; break;
                case 3: swr_thresh = 1.7; break;
                case 4: swr_thresh = 2.0; break;
                case 5: swr_thresh = 2.5; break;
                case 6: swr_thresh = 3.0; break;
            }
            if (FWCATU.SWRThreshold != swr_thresh)
                FWCATU.SetSWRThreshold(swr_thresh);

            //     byte b1, b2, b3, b4;
            //     do
            //     {
            //         FWC.ATUGetResult(out b1, out b2, out b3, out b4, 200);
            //     } while (b4 > 0);
        }





        // ke9ns add to force a SWR read from the built in ATU tuner
        private void buttonTS1_Click(object sender, EventArgs e)
        {

            Thread t = new Thread(new ThreadStart(READSWR));
            t.Name = "SWR Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

        }

        public void READSWR()
        {
            SWRRUN = true;
            int old_tun_pwr = 50;

            do
            {
                if (ADJUST == 1) FWCATU.DecrementCapacitance(); // ADJUST set by <> buttons on FWC ATU screen
                else if (ADJUST == 2) FWCATU.IncrementCapacitance();
                else if (ADJUST == 3) FWCATU.DecrementInductance();
                else if (ADJUST == 4) FWCATU.IncrementInductance();


                ADJUST = 0; // reset back 

                if (!console.fwc_init || console.CurrentModel != Model.FLEX5000) return;

                btnTuneMemory.BackColor = console.ButtonSelectedColor;
                if (chkUseTUN.Checked)
                {
                    console.TUN = true;
                    //   old_tun_pwr = console.PWR;
                    //  console.PWR = 10;
                }
                FWCATU.ReadSWR(); // AT-200 sends all updated data to me
                UpdateFeedback();
            }
            while (ADJUST != 0); // if you push <> button before done, then keep going


            if (chkUseTUN.Checked)
            {
                //  console.PWR = old_tun_pwr;
                console.TUN = false;
            }
            btnTuneMemory.BackColor = SystemColors.Control;
            UpdateFeedback();

            SWRRUN = false; // reset to allow this thread to start again 

        } // buttonTS1_Click
        #endregion


        // ke9ns add

        public int ADJUST = 0; // 1=capdown, 2=capup, 3=ind down, 4=ind up
        public bool SWRRUN = false;

        private void capdown_Click(object sender, EventArgs e)
        {

            ADJUST = 1;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }


        }

        private void capup_Click(object sender, EventArgs e)
        {
            ADJUST = 2;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }

        private void inddown_Click(object sender, EventArgs e)
        {
            ADJUST = 3;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }

        }

        private void indup_Click(object sender, EventArgs e)
        {

            ADJUST = 4;

            if (SWRRUN == false)
            {
                Thread t = new Thread(new ThreadStart(READSWR));
                t.Name = "SWR Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }

        private void ChkAlwaysOnTop1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop1.Checked;
        }

        private void FWCATUForm_MouseEnter(object sender, EventArgs e)
        {
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop1.Checked == true) this.Activate();
        }
    } // class FWCATUForm

} // PowerSDR
