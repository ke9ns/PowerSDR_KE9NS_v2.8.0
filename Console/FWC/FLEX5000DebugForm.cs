//=================================================================
// FLEX5000DebugForm.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FLEX5000DebugForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
        private FLEX5000LLHWForm flex5000LLHWForm;
        bool trx_ok = true;
        bool pa_ok = true;
        bool rfio_ok = true;
        //bool rx2_ok = true;

       
        #endregion
      
        #region Constructor and Destructor

        public FLEX5000DebugForm(Console c)
        {
            InitializeComponent();
            console = c;

            trx_ok = FWCEEPROM.TRXOK;
            pa_ok = FWCEEPROM.PAOK;
            rfio_ok = FWCEEPROM.RFIOOK;
            //rx2_ok = FWCEEPROM.RX2OK;

            grpDriverBias.Enabled = pa_ok;
            grpFinalBias.Enabled = pa_ok;
            ckPABias.Enabled = pa_ok;
            ckPAOff.Enabled = pa_ok;
            chkCTS.Enabled = pa_ok;
            chkRTS.Enabled = pa_ok;
            chkReset.Enabled = pa_ok;
            chkPCPwr.Enabled = pa_ok;
            grpADC.Enabled = pa_ok;

            bool error = !trx_ok || !pa_ok;
            if (console.CurrentModel == Model.FLEX5000)
                error |= !rfio_ok;
            if (error /*|| !rx2_ok*/)
            {
                string s = "";
                if (!trx_ok) s += "TRX: Error or not present\n";
                if (!pa_ok) s += "PA: Error or not present\n";
                if (!rfio_ok) s += "RFIO: Error or not present\n";
                //if(!rx2_ok) s += "RX2: Error or not present\n";
                MessageBox.Show(new Form { TopMost = true }, s);
            }

            Common.RestoreForm(this, "FLEX5000DebugForm", false);

            //cmboPLLRefClock.Text = "10";
            //cmboPLLCPMode.Text = "Normal";
            //comboPLLStatusMux.SelectedIndex = 0;
            comboADCChan.SelectedIndex = 0;

            if (console.CurrentModel == Model.FLEX3000)
            {
                grpChanSel.Visible = true;
                grpChanSel.BringToFront();
                menuItem2.Visible = true;

                comboADCChan.Items.Clear();
                comboADCChan.Items.Add("Final Bias");
                comboADCChan.Items.Add("Driver Bias");
                comboADCChan.Items.Add("13.8V Ref");
                comboADCChan.Items.Add("Temp");
                comboADCChan.Items.Add("Rev Pow");
                comboADCChan.Items.Add("Fwd Pow");
                comboADCChan.Items.Add("Phase");
                comboADCChan.Items.Add("Mag");
                comboADCChan.SelectedIndex = 0;

                lblQ1Coarse.Text = "Q2 Coarse:";
                lblQ1Fine.Text = "Q2 Fine:";
                lblQ2Coarse.Text = "Q3 Coarse:";
                lblQ2Fine.Text = "Q3 Fine:";
                lblQ3Coarse.Text = "Q4 Coarse:";
                lblQ3Fine.Text = "Q4 Fine:";
                lblQ4Coarse.Text = "Q5 Coarse:";
                lblQ4Fine.Text = "Q5 Fine:";
            }

            if (console.CurrentModel == Model.FLEX5000 /*&&
                FWCEEPROM.VUOK*/)
                menuItem3.Visible = true; // change to only if 5K and VU present

            this.TopMost = true; // ke9ns .174
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

        public void SetADCText(string s)
        {
            txtADCRead.Text = s;
        }

        public void SetTRXPot(int index, byte val)
        {
            switch (index)
            {
                case 0: udTRXPot0.Value = val; break;
                case 1: udTRXPot1.Value = val; break;
                case 2: udTRXPot2.Value = val; break;
                case 3: udTRXPot3.Value = val; break;
            }
        }

        public byte GetPAPot(int index)
        {
            byte ret_val = 0;
            switch (index)
            {
                case 0: ret_val = (byte)udPAPot0.Value; break;
                case 1: ret_val = (byte)udPAPot1.Value; break;
                case 2: ret_val = (byte)udPAPot2.Value; break;
                case 3: ret_val = (byte)udPAPot3.Value; break;
                case 4: ret_val = (byte)udPAPot4.Value; break;
                case 5: ret_val = (byte)udPAPot5.Value; break;
                case 6: ret_val = (byte)udPAPot6.Value; break;
                case 7: ret_val = (byte)udPAPot7.Value; break;
            }
            return ret_val;
        }

        public void SetPAPot(int index, byte val)
        {
            switch (index)
            {
                case 0: udPAPot0.Value = val; break;
                case 1: udPAPot1.Value = val; break;
                case 2: udPAPot2.Value = val; break;
                case 3: udPAPot3.Value = val; break;
                case 4: udPAPot4.Value = val; break;
                case 5: udPAPot5.Value = val; break;
                case 6: udPAPot6.Value = val; break;
                case 7: udPAPot7.Value = val; break;
            }
        }

        #endregion

        #region Event Handlers

        private void udFreq1_ValueChanged(object sender, System.EventArgs e)
        {
            //if(FWC.SetRX1Filter((float)udFreq1.Value) == 0)
            //    MessageBox.Show(new Form { TopMost = true }, "Error in SetRX1Freq");
        }

        private void udFreq2_ValueChanged(object sender, System.EventArgs e)
        {
            /*if(FWC.SetTXFreq((float)udFreq2.Value) == 0)
				MessageBox.Show(new Form { TopMost = true }, "Error in SetTXFreq");*/
        }

        private void chkTest_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetTest(ckTest.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetTest");
        }

        private void chkGen_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetGen(ckGen.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetGen");
        }

        private void chkSig_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetSig(ckSig.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetSig");
        }

        private void chkImpulse_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetImpulse(chkImpulse.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetImpulse");
        }

        private void chkXVEN_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetXVEN(ckXVEN.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetXVEN");
        }

        private void chkQSD_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetQSD(chkQSD.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetQSD");
        }

        private void chkQSE_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetQSE(ckQSE.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetQSE");
        }

        private void chkXREF_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetXREF(chkXREF.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetXREF");
        }

        private void chkIntSpkr_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetIntSpkr(chkIntSpkr.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetIntSpkr");
        }

        private void chkRX1Tap_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRX1Tap(chkRX1Tap.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRX1Tap");
        }

        private void udTRXPot0_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTRXPot0.Focused || tbTRXPot0.Focused)
            {
                if (FWC.TRXPotSetRDAC(0, (int)udTRXPot0.Value) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in TRXPotSetRDAC");
            }

            tbTRXPot0.Value = (int)udTRXPot0.Value;
        }

        private void tbTRXPot0_Scroll(object sender, System.EventArgs e)
        {
            udTRXPot0.Value = tbTRXPot0.Value;
        }

        private void udTRXPot1_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTRXPot1.Focused || tbTRXPot1.Focused)
            {
                if (FWC.TRXPotSetRDAC(1, (int)udTRXPot1.Value) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in TRXPotSetRDAC");
            }

            tbTRXPot1.Value = (int)udTRXPot1.Value;
        }

        private void tbTRXPot1_Scroll(object sender, System.EventArgs e)
        {
            udTRXPot1.Value = tbTRXPot1.Value;
        }

        private void udTRXPot2_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTRXPot2.Focused || tbTRXPot2.Focused)
            {
                if (FWC.TRXPotSetRDAC(2, (int)udTRXPot2.Value) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in TRXPotSetRDAC");
            }

            tbTRXPot2.Value = (int)udTRXPot2.Value;
        }

        private void tbTRXPot2_Scroll(object sender, System.EventArgs e)
        {
            udTRXPot2.Value = tbTRXPot2.Value;
        }

        private void udTRXPot3_ValueChanged(object sender, System.EventArgs e)
        {
            if (udTRXPot3.Focused || tbTRXPot3.Focused)
            {
                if (FWC.TRXPotSetRDAC(3, (int)udTRXPot3.Value) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in TRXPotSetRDAC");
            }

            tbTRXPot3.Value = (int)udTRXPot3.Value;
        }

        private void tbTRXPot3_Scroll(object sender, System.EventArgs e)
        {
            udTRXPot3.Value = tbTRXPot3.Value;
        }

        private void chkRX1FilterBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.BypassRX1Filter(ckRX1FilterBypass.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in BypassRX1Filter");
        }

        private void chkTXFilterBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.BypassTXFilter(ckTXFilterBypass.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in BypassTXFilter");
        }

        private void chkDot_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkDot.Checked) chkDot.BackColor = Color.Yellow;
            else chkDot.BackColor = SystemColors.Control;
        }

        private void chkDash_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkDash.Checked) chkDash.BackColor = Color.Yellow;
            else chkDash.BackColor = SystemColors.Control;
        }


        //====================================================================
        // ke9ns THREAD to check CW KEY
        private void PollKey()
        {
            bool dot, dash, rca_ptt, mic_ptt;
            while (ckKeyPoll.Checked)
            {
                if (FWC.ReadPTT(out dot, out dash, out rca_ptt, out mic_ptt) == 0)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Error in ReadKey");
                    break;
                }
                chkDot.Checked = dot;
                chkDash.Checked = dash;
                chkRCAPTT.Checked = rca_ptt;
                chkMicPTT.Checked = mic_ptt;
                Thread.Sleep(20);
            }
        }



        // ke9ns activate looking at the CW KEY
        private void ckKeyPoll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ckKeyPoll.Checked)
            {
                Thread t = new Thread(new ThreadStart(PollKey));
                t.Name = "Poll Key Thread";
                t.Priority = ThreadPriority.Normal;
                t.IsBackground = true;
                t.Start();
            }
        }



        private void chkHeadphone_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetHeadphone(chkHeadphone.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetHeadphone");
        }

        private void chkPLL_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetPLL(ckPLL.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetPLL");
        }

        private void chkRCATX1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRCATX1(chkRCATX1.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRCATX1");
        }

        private void chkRCATX2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRCATX2(chkRCATX2.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRCATX2");
        }

        private void chkRCATX3_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRCATX3(chkRCATX3.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRCATX3");
        }

        private void comboPLLRefClock_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            int val = 0;
            switch (cmboPLLRefClock.Text)
            {
                case "10": val = 0x01; break;
                case "20": val = 0x02; break;
            }
            int old_val;
            if (FWC.ReadClockReg(0x0C, out old_val) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in ReadClockReg.");
            if (old_val != val)
            {
                if (FWC.WriteClockReg(0x0C, val) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in WriteClockReg.");
            }
        }

        private void comboPLLCPMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            int old_val;
            if (FWC.ReadClockReg(0x08, out old_val) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in ReadClockReg");
            int val = (int)((old_val & 0xFC) | (int)cmboPLLCPMode.SelectedIndex);
            if (old_val != val)
            {
                if (FWC.WriteClockReg(0x08, val) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in WriteClockReg");
            }
        }

        private void comboPLLStatusMux_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            int old_val;
            if (FWC.ReadClockReg(0x08, out old_val) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in ReadClockReg");
            int val = (int)((old_val & 0xC3) | (int)(comboPLLStatusMux.SelectedIndex << 2));
            if (old_val != val)
            {
                if (FWC.WriteClockReg(0x08, val) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in WriteClockReg");
            }
        }

        private void chkPLLPFDPol_CheckedChanged(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            int old_val;
            if (FWC.ReadClockReg(0x08, out old_val) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in ReadClockReg");

            int val = 0;
            if (ckPLLPFDPol.Checked) val = 0x40;
            val = (int)((old_val & 0xBF) | val);
            if (old_val != val)
            {
                if (FWC.WriteClockReg(0x08, val) == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in WriteClockReg");
            }
        }

        private Thread poll_status_thread;
        private void ckPLLPollStatus_CheckedChanged(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            btnPLLStatus.Enabled = !ckPLLPollStatus.Checked;

            if (ckPLLPollStatus.Checked)
            {
                poll_status_thread = new Thread(new ThreadStart(PollStatus));
                poll_status_thread.Name = "Poll Status Thread";
                poll_status_thread.IsBackground = true;
                poll_status_thread.Priority = ThreadPriority.Normal;
                poll_status_thread.Start();
            }
        }

        private void PollStatus()
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            while (ckPLLPollStatus.Checked)
            {
                btnPLLStatus_Click(this, EventArgs.Empty);
                Thread.Sleep(250);
            }
        }

        private void btnPLLStatus_Click(object sender, System.EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000) return;
            bool b;
            FWC.GetPLLStatus2(out b);
            if (b) btnPLLStatus.BackColor = Color.Green;
            else btnPLLStatus.BackColor = Color.Red;
        }

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            if (flex5000LLHWForm == null || flex5000LLHWForm.IsDisposed)
                flex5000LLHWForm = new FLEX5000LLHWForm(console);
            flex5000LLHWForm.Show();
        }

        private void chkScanA_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ckScanA.Checked)
            {
                ckScanA.BackColor = console.ButtonSelectedColor;
                Thread t = new Thread(new ThreadStart(ScanA));
                t.Name = "Scan A Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
            else ckScanA.BackColor = SystemColors.Control;
        }

        private void ScanA()
        {
            console.VFOAFreq = (double)udScanStartA.Value;
            while (ckScanA.Checked)
            {
                Thread.Sleep((int)udScanDelayA.Value);
                console.VFOAFreq += (double)udScanStepA.Value;
                if (console.VFOAFreq >= (double)udScanStopA.Value)
                    ckScanA.Checked = false;
            }
        }

        private void chkScanB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ckScanB.Checked)
            {
                ckScanB.BackColor = console.ButtonSelectedColor;
                Thread t = new Thread(new ThreadStart(ScanB));
                t.Name = "Scan B Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
            else ckScanB.BackColor = SystemColors.Control;
        }

        private void ScanB()
        {
            console.VFOBFreq = (double)udScanStartB.Value;
            while (ckScanB.Checked)
            {
                Thread.Sleep((int)udScanDelayB.Value);
                console.VFOBFreq += (double)udScanStepB.Value;
                if (console.VFOBFreq >= (double)udScanStopB.Value)
                    ckScanB.Checked = false;
            }
        }

        private void chkRX1Out_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRX1Out(chkRX1Out.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRX1Out");
        }

        private void chkTR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetTR(ckTR.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetTR");
        }

        private void chkTXMon_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetTXMon(ckTXMon.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetTXMon");
        }

        private void chkXVCOM_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetXVCOM(chkXVCOM.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetXVCOM");
        }

        private void chkEN2M_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetXVINT(chkEN2M.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetEN2M");
        }

        private void chkKEY2M_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetKey2M(chkKEY2M.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetKEY2M");
        }

        private void chkRCAPTT_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRCAPTT.Checked) chkRCAPTT.BackColor = console.ButtonSelectedColor;
            else chkRCAPTT.BackColor = SystemColors.Control;
        }

        private void chkMicPTT_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMicPTT.Checked) chkMicPTT.BackColor = console.ButtonSelectedColor;
            else chkMicPTT.BackColor = SystemColors.Control;
        }

        private void chkXVTXEN_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetXVTXEN(chkXVTXEN.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetXVTXEN");
        }

        private void udPAPot0_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(0, (int)udPAPot0.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot0.Value = (int)udPAPot0.Value;
        }

        private void tbPAPot0_Scroll(object sender, System.EventArgs e)
        {
            udPAPot0.Value = tbPAPot0.Value;
        }

        private void udPAPot1_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(1, 255 - (int)udPAPot1.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot1.Value = (int)udPAPot1.Value;
        }

        private void tbPAPot1_Scroll(object sender, System.EventArgs e)
        {
            udPAPot1.Value = tbPAPot1.Value;
        }

        private void udPAPot2_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(2, (int)udPAPot2.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot2.Value = (int)udPAPot2.Value;
        }

        private void tbPAPot2_Scroll(object sender, System.EventArgs e)
        {
            udPAPot2.Value = tbPAPot2.Value;
        }

        private void udPAPot3_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(3, 255 - (int)udPAPot3.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot3.Value = (int)udPAPot3.Value;
        }

        private void tbPAPot3_Scroll(object sender, System.EventArgs e)
        {
            udPAPot3.Value = tbPAPot3.Value;
        }

        private void udPAPot4_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(4, (int)udPAPot4.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot4.Value = (int)udPAPot4.Value;
        }

        private void tbPAPot4_Scroll(object sender, System.EventArgs e)
        {
            udPAPot4.Value = tbPAPot4.Value;
        }

        private void udPAPot5_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(5, 255 - (int)udPAPot5.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot5.Value = (int)udPAPot5.Value;
        }

        private void tbPAPot5_Scroll(object sender, System.EventArgs e)
        {
            udPAPot5.Value = tbPAPot5.Value;
        }

        private void udPAPot6_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(6, (int)udPAPot6.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot6.Value = (int)udPAPot6.Value;
        }

        private void tbPAPot6_Scroll(object sender, System.EventArgs e)
        {
            udPAPot6.Value = tbPAPot6.Value;
        }

        private void udPAPot7_ValueChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.PAPotSetRDAC(7, 255 - (int)udPAPot7.Value) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in PAPotSetRDAC");

            tbPAPot7.Value = (int)udPAPot7.Value;
        }

        private void tbPAPot7_Scroll(object sender, System.EventArgs e)
        {
            udPAPot7.Value = tbPAPot7.Value;
        }

        private void FLEX5000DebugForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX5000DebugForm");
            if (flex5000LLHWForm != null)
            {
                flex5000LLHWForm.Hide();
                flex5000LLHWForm.Close();
            }
        }

        private void ckPABias_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.SetPABias(ckPABias.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetPABias");
            if (console.CurrentModel == Model.FLEX3000 && !ckPABias.Checked)
                FWC.SetFan(false);
        }

        private void chkPAOff_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (ckPAOff.Checked)
            {
                if (FWC.SetPowerOff() == 0)
                    MessageBox.Show(new Form { TopMost = true }, "Error in SetPowerOff");
            }
        }

        private void chkFPLED_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetFPLED(chkFPLED.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetFPLED");
        }

        private void chkCTS_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.SetCTS(chkCTS.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetCTS");
        }

        private void chkRTS_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.SetRTS(chkRTS.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRTS");
        }

        private void chkReset_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.SetPCReset(chkReset.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetPCReset");
        }

        private void chkPCPwr_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (FWC.SetPCPWRBT(chkPCPwr.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetPCPWRBT");
        }

        #endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(RunImpulse));
            t.Name = "Run Impulse Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Highest;
            t.Start();
        }

        private void RunImpulse()
        {
            HiPerfTimer t1 = new HiPerfTimer();
            for (int i = 0; i < 100; i++)
            {
                //t1.Start();
                FWC.SetImpulse(true);
                //t1.Stop();
                //while(t1.DurationMsec < 10) t1.Stop();

                //t1.Start();
                FWC.SetImpulse(false);
                //t1.Stop();
                //while(t1.DurationMsec < 10) t1.Stop();
            }
        }

        private void chkXVTR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetXVTR(chkXVTR.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetXVTR");
        }

        private void chkFan_CheckedChanged(object sender, System.EventArgs e)
        {
            /*if(FWC.SetFan(chkFan.Checked) == 0)
				MessageBox.Show(new Form { TopMost = true }, "Error in SetFan");*/
        }

        private void chkIntLED_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetIntLED(chkIntLED.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetIntLED");
        }

        private void chkPAFilter6m_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.BypassPAFilter(ckPAFilter6m.Checked);
        }

        private void btnADCRead_Click(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (comboADCChan.Text == "") return;
            int chan = comboADCChan.SelectedIndex;

            int val;
            if (FWC.ReadPAADC(chan, out val) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in ReadPAADC");

            float volts = (float)val / 4096 * 2.5f;
            string output = "";
            switch (comboADCChan.Text)
            {
                case "Final Bias": output = (volts * 10).ToString("f3") + " A"; break;
                case "Driver Bias":
                    if (((byte)(FWCEEPROM.PARev >> 8)) >= 0)
                        output = (volts).ToString("f3") + " A"; // 50 milliohm
                    else output = (volts / 2).ToString("f3") + " A"; // 100 milliohm
                    break;
                case "13.8V Ref": output = (volts * 11).ToString("f1") + " V"; break;
                case "Temp":
                    double temp_c = 301 - volts * 1000 / 2.2;
                    switch (temp_format)
                    {
                        case TempFormat.Fahrenheit:
                            output = ((temp_c * 1.8) + 32).ToString("f1") + "° F"; break;
                        case TempFormat.Celsius:
                            output = temp_c.ToString("f1") + "° C"; break;
                    }
                    break;
                default: output = volts.ToString("f3") + " V"; break;
            }
            txtADCRead.Text = output;
        }

        private void comboADCChan_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            btnADCRead_Click(this, EventArgs.Empty);
        }

        private void ckADCPoll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!pa_ok) return;
            if (ckADCPoll.Checked)
            {
                Thread t = new Thread(new ThreadStart(PollADC));
                t.Name = "Poll ADC Thread";
                t.Priority = ThreadPriority.Normal;
                t.IsBackground = true;
                t.Start();
            }
        }

        private void PollADC()
        {
            int val = 0;
            double save_val = 0;
            while (ckADCPoll.Checked)
            {
                if (comboADCChan.SelectedIndex >= 0)
                {
                    if (FWC.ReadPAADC(comboADCChan.SelectedIndex, out val) == 0)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Error in ReadPAADC");
                        break;
                    }

                    double avg_val = (save_val = 0.8 * save_val + 0.2 * val);
                    //double avg_val = val;

                    double volts = (float)avg_val / 4096 * 2.5f;
                    string output;
                    switch (comboADCChan.Text)
                    {
                        case "Final Bias": output = (volts * 10).ToString("f3") + " A"; break;
                        case "Driver Bias":
                            if (((byte)(FWCEEPROM.PARev >> 8)) > 0)
                                output = (volts).ToString("f3") + " A"; // 50 milliohm
                            else output = (volts / 2).ToString("f3") + " A"; // 100 milliohm
                            break;
                        case "13.8V Ref": output = (volts * 11).ToString("f1") + " V"; break;
                        case "Temp": output = (293 - volts * 1000 / 2.2f).ToString("f1") + "° C"; break;
                        default: output = volts.ToString("f3") + " V"; break;
                    }

                    txtADCRead.Text = output;
                }
                Thread.Sleep(100);
            }
        }

        private void radTapPreDriver_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radTapPreDriver.Checked)
            {
                console.FullDuplex = true;
                FWC.SetQSD(true);
                FWC.SetQSE(true);
                FWC.SetTR(true);
                FWC.SetSig(true);
                FWC.SetGen(false);
                FWC.SetTest(true);
                FWC.SetTXMon(false);
            }
        }

        private void radTapFinal_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radTapFinal.Checked)
            {
                console.FullDuplex = true;
                FWC.SetQSD(true);
                FWC.SetQSE(true);
                FWC.SetTR(true);
                FWC.SetSig(false);
                FWC.SetGen(false);
                FWC.SetTest(false);
                FWC.SetTXMon(true);
            }
        }

        private void radTapOff_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radTapOff.Checked)
            {
                console.FullDuplex = false;
                FWC.SetQSD(true);
                FWC.SetQSE(false);
                FWC.SetTR(false);
                FWC.SetSig(false);
                FWC.SetGen(false);
                FWC.SetTest(false);
                FWC.SetTXMon(false);
            }
        }

        private void udTXTrace_ValueChanged(object sender, System.EventArgs e)
        {
            //if(ckTXTrace.Checked)
            //Audio.SourceScale = (double)udTXTrace.Value;
            Audio.RadioVolume = (double)udTXTrace.Value; // uncomment this line and comment the 2 above for VU Trace
        }

        private void chkTXTrace_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ckTXTrace.Checked)
            {
                if (!console.PowerOn)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Power must be on in order to run TX Trace.", "Power Is Off",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ckTXTrace.Checked = false;
                    return;
                }

                ckTXTrace.BackColor = console.ButtonSelectedColor;
                console.FullDuplex = true;

                FWC.SetQSD(true);
                FWC.SetQSE(true);
                FWC.SetTR(true);
                FWC.SetSig(true);
                FWC.SetGen(false);
                FWC.SetTest(true);
                FWC.SetTXMon(false);
                FWC.SetPDrvMon(true);

                if (console.CurrentModel == Model.FLEX3000)
                    FWC.SetAmpTX1(false);

                console.RX1DSPMode = DSPMode.USB;

                FWC.SetMOX(true);
                Audio.TXOutputSignal = Audio.SignalSource.SINE;
                Audio.SourceScale = (double)udTXTrace.Value;
            }
            else
            {
                FWC.SetMOX(false);
                FWC.SetQSD(true);
                FWC.SetQSE(false);
                FWC.SetTR(false);
                FWC.SetSig(false);
                FWC.SetGen(false);
                FWC.SetTest(false);
                FWC.SetTXMon(false);
                FWC.SetPDrvMon(false);
                Audio.TXOutputSignal = Audio.SignalSource.RADIO;
                console.FullDuplex = false;
                ckTXTrace.BackColor = SystemColors.Control;
                if (console.CurrentModel == Model.FLEX3000)
                {
                    FWC.SetFan(false);
                    if (ckTXOut.Checked) FWC.SetAmpTX1(true);
                }
            }
        }

        private enum TempFormat
        {
            Celsius = 0,
            Fahrenheit,
        }

        private TempFormat temp_format = TempFormat.Celsius;
        private void txtADCRead_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (temp_format)
            {
                case TempFormat.Celsius:
                    temp_format = TempFormat.Fahrenheit;
                    break;
                case TempFormat.Fahrenheit:
                    temp_format = TempFormat.Celsius;
                    break;
            }
            if (comboADCChan.SelectedIndex == 4)
                btnADCRead_Click(this, EventArgs.Empty);
        }

        private void chkRX2On_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRX2On(chkRX2On.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRX2On");
        }

        private void ckRX2FilterBypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.BypassRX2Filter(ckRX2FilterBypass.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in BypassRX2Filter");
        }

        private void comboRXL_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Audio.IN_RX1_L = comboRXL.SelectedIndex;
        }

        private void comboRXR_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Audio.IN_RX1_R = comboRXR.SelectedIndex;
        }

        private void comboTXL_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Audio.IN_TX_L = comboTXL.SelectedIndex;
        }

        private void comboTXR_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Audio.IN_TX_R = comboTXR.SelectedIndex;
        }

        private void chkPDrvMon_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetPDrvMon(chkPDrvMon.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetPDrvMon");
        }

        private void chkRXAttn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (FWC.SetRXAttn(chkRXAttn.Checked) == 0)
                MessageBox.Show(new Form { TopMost = true }, "Error in SetRXAttn");
        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            if (console.flex3000TestForm == null || console.flex3000TestForm.IsDisposed)
                console.flex3000TestForm = new FLEX3000TestForm(console);
            console.flex3000TestForm.Show();
            console.flex3000TestForm.Focus();
        }

        private void ckTXOut_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetAmpTX1(ckTXOut.Checked);
        }

        private void ckPreamp_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetTRXPreamp(ckPreamp.Checked);
        }

        //private VUForm vuForm;
        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (console.vuForm == null || console.vuForm.IsDisposed) console.vuForm = new VUForm(console);
            console.vuForm.Show();
            console.vuForm.Focus();
        }

        private void FLEX5000DebugForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000 || !FWCEEPROM.ATUOK) return;
            if (e.Control && e.Alt && e.KeyCode == Keys.A)
            {
                grpATUUpgrade.Visible = true;
                grpATUUpgrade.BringToFront();
            }
        }

        private void btnATUUpdate_Click(object sender, EventArgs e)
        {
            if (console.CurrentModel != Model.FLEX5000 || !FWCEEPROM.ATUOK) return;

            if (Keyboard.IsKeyDown(Keys.LShiftKey) || Keyboard.IsKeyDown(Keys.RShiftKey))
            {
                // undo upgrade
                FWC.WriteTRXEEPROMUint(0x10, 0xFFFFFFFF);
                MessageBox.Show(new Form { TopMost = true }, "Undo ATU Upgrade complete.\n" +
                    "Please shutdown PowerSDR and cycle power to the radio\n" +
                    "for the changes to take effect.");
            }
            else
            {
                FWC.WriteTRXEEPROMByte(0x10, 0x34);
                FWC.WriteTRXEEPROMByte(0x11, 0x1);
                MessageBox.Show(new Form { TopMost = true }, "EEPROM Update for ATU Upgrade complete.\n" +
                    "Please shutdown PowerSDR and cycle power to the radio\n" +
                    "for the changes to take effect.");
            }
        }
    }
}
