//=================================================================
// hidAntForm.cs
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
using System.Collections;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class HIDAntForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       

        #endregion

        #region Constructor and Destructor

        public HIDAntForm(Console c)
        {
            InitializeComponent();
            console = c;

            // Set mode first
            ArrayList a = DB.GetVars("HIDAnt");
            a.Sort();

            foreach (string s in a)
            {
                if (s.StartsWith("radModeExpert") && s.IndexOf("True") >= 0)
                {
                    radModeExpert.Checked = true;
                    break;
                }
            }

            Common.RestoreForm(this, "HIDAnt", false);

            if (radModeSimple.Checked)
                radModeSimple_CheckedChanged(this, EventArgs.Empty);

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

        

        #region Properties

        public AntMode CurrentAntMode
        {
            get
            {
                if (radModeSimple.Checked) return AntMode.Simple;
                else /*if(radModeExpert.Checked)*/ return AntMode.Expert;
            }
            set
            {
                switch (value)
                {
                    case AntMode.Simple:
                        radModeSimple.Checked = true;
                        break;
                    case AntMode.Expert:
                        radModeExpert.Checked = true;
                        break;
                }
            }
        }

        public HIDAnt RXAnt
        {
            get { return StringToAnt(comboRXAnt.Text); }
            set { comboRXAnt.Text = AntToString(value); }
        }

        public HIDAnt TXAnt
        {
            get { return StringToAnt(comboTXAnt.Text); }
            set { comboTXAnt.Text = AntToString(value); }
        }

        public bool PTTOut
        {
            get { return chkPTTOutEnable.Checked; }
            set { chkPTTOutEnable.Checked = value; }
        }

        public bool TX1DelayEnable
        {
            get { return chkTX1DelayEnable.Checked; }
            set { chkTX1DelayEnable.Checked = value; }
        }

        public bool AntLock
        {
            get { return chkLock.Checked; }
            set { chkLock.Checked = value; }
        }

        public uint TX1Delay
        {
            get { return (uint)udTX1Delay.Value; }
            set { udTX1Delay.Value = value; }
        }

        #endregion

        #region Misc Routines

        public void SetBand(Band b)
        {
            comboBand.Text = BandToString(b);
        }

        private string BandToString(Band b)
        {
            string ret_val = "";
            switch (b)
            {
                case Band.GEN: ret_val = "GEN"; break;  // ke9ns mod
                case Band.B160M: ret_val = "160m"; break;
                case Band.B80M: ret_val = "80m"; break;
                case Band.B60M: ret_val = "60m"; break;
                case Band.B40M: ret_val = "40m"; break;
                case Band.B30M: ret_val = "30m"; break;
                case Band.B20M: ret_val = "20m"; break;
                case Band.B17M: ret_val = "17m"; break;
                case Band.B15M: ret_val = "15m"; break;
                case Band.B12M: ret_val = "12m"; break;
                case Band.B10M: ret_val = "10m"; break;
                case Band.B6M: ret_val = "6m"; break;
                case Band.WWV: ret_val = "WWV"; break;

                case Band.VHF0: ret_val = "VU 2m"; break;
                case Band.VHF1: ret_val = "VU 70cm"; break;
                case Band.VHF2: ret_val = "VHF2"; break;
                case Band.VHF3: ret_val = "VHF3"; break;
                case Band.VHF4: ret_val = "VHF4"; break;
                case Band.VHF5: ret_val = "VHF5"; break;
                case Band.VHF6: ret_val = "VHF6"; break;
                case Band.VHF7: ret_val = "VHF7"; break;
                case Band.VHF8: ret_val = "VHF8"; break;
                case Band.VHF9: ret_val = "VHF9"; break;
                case Band.VHF10: ret_val = "VHF10"; break;
                case Band.VHF11: ret_val = "VHF11"; break;
                case Band.VHF12: ret_val = "VHF12"; break;
                case Band.VHF13: ret_val = "VHF13"; break;

                case Band.BLMF: ret_val = "LMF"; break;  // ke9ns add
                case Band.B120M: ret_val = "120m"; break;
                case Band.B90M: ret_val = "90m"; break;
                case Band.B61M: ret_val = "61m"; break;
                case Band.B49M: ret_val = "49m"; break;
                case Band.B41M: ret_val = "41m"; break;
                case Band.B31M: ret_val = "31m"; break;
                case Band.B25M: ret_val = "25m"; break;
                case Band.B22M: ret_val = "22m"; break;
                case Band.B19M: ret_val = "19m"; break;
                case Band.B16M: ret_val = "16m"; break;
                case Band.B14M: ret_val = "14m"; break;
                case Band.B13M: ret_val = "13m"; break;
                case Band.B11M: ret_val = "11m"; break;
            }
            return ret_val;
        }

        private Band StringToBand(string s)
        {
            Band b = Band.GEN;
            switch (s)
            {
                case "GEN": b = Band.GEN; break;  // ke9ns mod
                case "160m": b = Band.B160M; break;
                case "80m": b = Band.B80M; break;
                case "60m": b = Band.B60M; break;
                case "40m": b = Band.B40M; break;
                case "30m": b = Band.B30M; break;
                case "20m": b = Band.B20M; break;
                case "17m": b = Band.B17M; break;
                case "15m": b = Band.B15M; break;
                case "12m": b = Band.B12M; break;
                case "10m": b = Band.B10M; break;
                case "6m": b = Band.B6M; break;
                case "WWV": b = Band.WWV; break;

                case "VU 2m": b = Band.VHF0; break;
                case "VU 70cm": b = Band.VHF1; break;
                case "VHF2": b = Band.VHF2; break;
                case "VHF3": b = Band.VHF3; break;
                case "VHF4": b = Band.VHF4; break;
                case "VHF5": b = Band.VHF5; break;
                case "VHF6": b = Band.VHF6; break;
                case "VHF7": b = Band.VHF7; break;
                case "VHF8": b = Band.VHF8; break;
                case "VHF9": b = Band.VHF9; break;
                case "VHF10": b = Band.VHF10; break;
                case "VHF11": b = Band.VHF11; break;
                case "VHF12": b = Band.VHF12; break;
                case "VHF13": b = Band.VHF13; break;

                case "LMF": b = Band.BLMF; break; // ke9ns add
                case "120m": b = Band.B120M; break;
                case "90m": b = Band.B90M; break;
                case "61m": b = Band.B61M; break;
                case "49m": b = Band.B49M; break;
                case "41m": b = Band.B41M; break;
                case "31m": b = Band.B31M; break;
                case "25m": b = Band.B25M; break;
                case "22m": b = Band.B22M; break;
                case "19m": b = Band.B19M; break;
                case "16m": b = Band.B16M; break;
                case "14m": b = Band.B14M; break;
                case "13m": b = Band.B13M; break;
                case "11m": b = Band.B11M; break;
            }
            return b;
        }

        private string AntToString(HIDAnt ant)
        {
            string ret_val = "";
            switch (ant)
            {
                case HIDAnt.PA: ret_val = "PA"; break;
                case HIDAnt.XVTX_COM: ret_val = "XVTX/COM"; break;
                case HIDAnt.XVRX: ret_val = "XVRX"; break;
            }
            return ret_val;
        }

        private HIDAnt StringToAnt(string s)
        {
            HIDAnt ant = HIDAnt.PA;
            switch (s)
            {
                case "PA": ant = HIDAnt.PA; break;
                case "XVTX/COM": ant = HIDAnt.XVTX_COM; break;
                case "XVRX": ant = HIDAnt.XVRX; break;
            }
            return ant;
        }

        #endregion

        #region Event Handlers

        private void HIDAntForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "HIDAnt");
        }

        private void radModeSimple_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeSimple.Checked)
            {
                console.CurrentAntMode = AntMode.Simple;

                lblBand.Visible = false;
                comboBand.Visible = false;
                grpFlexWirePTTOut.Visible = true;

                comboRXAnt.Text = AntToString(console.RXAnt1500);
                comboTXAnt.Text = AntToString(console.TXAnt1500);

                if (console.TXBand == Band.B6M)
                    comboTXAnt.Enabled = false;

                chkRCATX1_CheckedChanged(this, EventArgs.Empty);

                txtStatus.Text = "Simple Mode: Settings are applied to all bands";
            }
        }

        private void radModeExpert_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radModeExpert.Checked)
            {
                console.CurrentAntMode = AntMode.Expert;

                lblBand.Visible = true;
                comboBand.Visible = true;
                grpFlexWirePTTOut.Visible = true;

                comboBand.Text = BandToString(console.RX1Band);
                comboRXAnt.Text = AntToString(console.RXAnt1500);
                comboTXAnt.Text = AntToString(console.TXAnt1500);

                txtStatus.Text = "Expert Mode: Settings applied only to selected band";
            }
        }

        private void comboBand_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Band band = StringToBand(comboBand.Text);
            if (!radModeSimple.Checked)
            {
                comboRXAnt.Text = AntToString(console.GetRXAnt1500(band));
                comboTXAnt.Text = AntToString(console.GetTXAnt1500(band));
                chkPTTOutEnable.Checked = console.GetTX1(band);
            }
        }

        private void comboRXAnt_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.SetRXAnt1500(StringToBand(comboBand.Text), StringToAnt(comboRXAnt.Text));

            if (chkLock.Checked)
            {
                switch (comboRXAnt.Text)
                {
                    case "PA":
                    case "XVTX/COM":
                        comboTXAnt.Text = comboRXAnt.Text;
                        break;
                }
            }

            if (radModeExpert.Checked) console.CurrentAntMode = AntMode.Expert; // ke9ns add: update console Ant display .119
            else console.CurrentAntMode = AntMode.Simple;
        }

        private void comboTXAnt_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            console.SetTXAnt1500(StringToBand(comboBand.Text), StringToAnt(comboTXAnt.Text));

            if (chkLock.Checked) comboRXAnt.Text = comboTXAnt.Text;

            if (radModeExpert.Checked) console.CurrentAntMode = AntMode.Expert; // ke9ns add: update console Ant display .119
            else console.CurrentAntMode = AntMode.Simple;
        }

        private void chkRCATX1_CheckedChanged(object sender, System.EventArgs e)
        {
            console.Set1500TX1(StringToBand(comboBand.Text), chkPTTOutEnable.Checked);
            //console.SetTX1(StringToBand(comboBand.Text), chkPTTOutEnable.Checked);
            chkTX1DelayEnable.Enabled = chkPTTOutEnable.Checked;
            // USBHID.EnableTXOutSeq(chkPTTOutEnable.Checked);                     
        }

        private void chkLock_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLock.Checked)
            {
                switch (comboRXAnt.Text)
                {
                    case "PA":
                    case "XVTX/COM":
                        comboTXAnt.Text = comboRXAnt.Text;
                        break;
                }
            }
        }

        private void chkTX1DelayEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            //USBHID.SetAmpTX1DelayEnable(chkTX1DelayEnable.Checked);
            udTX1Delay.Enabled = chkTX1DelayEnable.Checked;

            bool b = chkTX1DelayEnable.Checked;

            if (!console.hid_init) return;
            USBHID.EnableTXOutDelay(b);
            udTX1Delay.Enabled = b;
            udTX1Delay_ValueChanged(sender, e);

        }

        private void udTX1Delay_ValueChanged(object sender, System.EventArgs e)
        {
            //USBHID.SetAmpTX1Delay((uint)udTX1Delay.Value);
            if (!console.hid_init) return;
            USBHID.SetTXOutDelayValue((uint)udTX1Delay.Value);

        }

        #endregion

        // ke9ns add
        private void HIDAntForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {
                if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                console.helpboxForm.Show();
                console.helpboxForm.Focus();
                console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add

                console.helpboxForm.helpbox_message.Text = console.helpboxForm.AntennaDelay.Text;
            }
        }
        // ke9ns add
        private void textBoxTS1_Click(object sender, EventArgs e)
        {
            if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

            console.helpboxForm.Show();
            console.helpboxForm.Focus();
            console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add

            console.helpboxForm.helpbox_message.Text = console.helpboxForm.AntennaDelay.Text;
        }
    }
}
