//=================================================================
// fwcmixform.cs
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
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for fwcmixform.
    /// </summary>
    public partial class FWCMixForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       

        #endregion

        #region Constructor and Destructor

        public FWCMixForm(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;

            if (FWCEEPROM.Model == 0)
            {
                lblIntSpkr.Enabled = false;
                tbIntSpkr.Enabled = false;
                chkIntSpkrSel.Enabled = false;

            }



            Common.RestoreForm(this, "FWCMixer", false);

            chkMicSel_CheckedChanged(this, EventArgs.Empty);
            chkLineInRCASel_CheckedChanged(this, EventArgs.Empty);
            chkLineInPhonoSel_CheckedChanged(this, EventArgs.Empty);
            chkLineInDB9Sel_CheckedChanged(this, EventArgs.Empty);
            chkIntSpkrSel_CheckedChanged(this, EventArgs.Empty);
            chkExtSpkrSel_CheckedChanged(this, EventArgs.Empty);
            chkHeadphoneSel_CheckedChanged(this, EventArgs.Empty);
            chkLineOutRCASel_CheckedChanged(this, EventArgs.Empty);
            tbMic_Scroll(this, EventArgs.Empty);
            tbLineInRCA_Scroll(this, EventArgs.Empty);
            tbLineInPhono_Scroll(this, EventArgs.Empty);
            tbLineInDB9_Scroll(this, EventArgs.Empty);
            tbIntSpkr_Scroll(this, EventArgs.Empty);
            tbExtSpkr_Scroll(this, EventArgs.Empty);
            tbHeadphone_Scroll(this, EventArgs.Empty);
            tbLineOutRCA_Scroll(this, EventArgs.Empty);

            this.TopMost = true; // ke9ns .174
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

        private byte InputSliderToRegVal(int slider)
        {
            byte retval;
            if (slider < 0) retval = (byte)(0x100 + slider);
            else retval = (byte)slider;
            //Debug.WriteLine("slider: "+slider+" Reg: "+retval.ToString("X"));
            return retval;
        }

        #endregion

        #region Properties

        public int MicInput
        {
            get { return tbMic.Value; }
            set
            {
                tbMic.Value = value;
                tbMic_Scroll(this, EventArgs.Empty);
            }
        }

        public int LineInRCA
        {
            get { return tbLineInRCA.Value; }
            set
            {
                tbLineInRCA.Value = value;
                tbLineInRCA_Scroll(this, EventArgs.Empty);
            }
        }

        public int LineInPhono
        {
            get { return tbLineInPhono.Value; }
            set
            {
                tbLineInPhono.Value = value;
                tbLineInPhono_Scroll(this, EventArgs.Empty);
            }
        }

        public int LineInDB9
        {
            get { return tbLineInDB9.Value; }
            set
            {
                tbLineInDB9.Value = value;
                tbLineInDB9_Scroll(this, EventArgs.Empty);
            }
        }

        public string MicInputSelected
        {
            get
            {
                if (chkMicSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkMicSel.Checked = true;
                else
                    chkMicSel.Checked = false;
            }
        }

        public string LineInRCASelected
        {
            get
            {
                if (chkLineInRCASel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkLineInRCASel.Checked = true;
                else
                    chkLineInRCASel.Checked = false;
            }
        }

        public string LineInPhonoSelected
        {
            get
            {
                if (chkLineInPhonoSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkLineInPhonoSel.Checked = true;
                else
                    chkLineInPhonoSel.Checked = false;
            }
        }

        public string LineInDB9Selected
        {
            get
            {
                if (chkLineInDB9Sel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkLineInDB9Sel.Checked = true;
                else
                    chkLineInDB9Sel.Checked = false;
            }
        }

        public string InputMuteAll
        {
            get
            {
                if (chkInputMuteAll.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkInputMuteAll.Checked = true;
                else
                    chkInputMuteAll.Checked = false;
            }
        }

        public int InternalSpkr
        {
            get { return tbIntSpkr.Value; }
            set
            {
                tbIntSpkr.Value = value;
                tbIntSpkr_Scroll(this, EventArgs.Empty);
            }
        }

        public int ExternalSpkr
        {
            get { return tbExtSpkr.Value; }
            set
            {
                tbExtSpkr.Value = value;
                tbExtSpkr_Scroll(this, EventArgs.Empty);
            }
        }

        public int Headphone
        {
            get { return tbHeadphone.Value; }
            set
            {
                tbHeadphone.Value = value;
                tbHeadphone_Scroll(this, EventArgs.Empty);
            }
        }

        public int LineOutRCA
        {
            get { return tbLineOutRCA.Value; }
            set
            {
                tbLineOutRCA.Value = value;
                tbLineOutRCA_Scroll(this, EventArgs.Empty);
            }
        }

        public string InternalSpkrSelected
        {
            get
            {
                if (chkIntSpkrSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkIntSpkrSel.Checked = true;
                else
                    chkIntSpkrSel.Checked = false;
            }
        }

        public string ExternalSpkrSelected
        {
            get
            {
                if (chkExtSpkrSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkExtSpkrSel.Checked = true;
                else
                    chkExtSpkrSel.Checked = false;
            }
        }

        public string HeadphoneSelected
        {
            get
            {
                if (chkHeadphoneSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkHeadphoneSel.Checked = true;
                else
                    chkHeadphoneSel.Checked = false;
            }
        }

        public string LineOutRCASelected
        {
            get
            {
                if (chkLineOutRCASel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkLineOutRCASel.Checked = true;
                else
                    chkLineOutRCASel.Checked = false;
            }
        }

        public string OutputMuteAll
        {
            get
            {
                if (chkOutputMuteAll.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkOutputMuteAll.Checked = true;
                else
                    chkOutputMuteAll.Checked = false;
            }
        }




        #endregion Properties

        #region Input Event Handlers


        //====================================================================
        // ke9ns MIC input from Flex radio
        //====================================================================
        private void tbMic_Scroll(object sender, System.EventArgs e)
        {
            if (chkMicSel.Checked && !chkInputMuteAll.Checked)
                FWC.WriteCodecReg(0x16, InputSliderToRegVal(tbMic.Value));

            this.toolTip1.SetToolTip(this.tbMic, "Mic Volume: " + tbMic.Value);
        }



        private void tbLineInRCA_Scroll(object sender, System.EventArgs e)
        {
            if (chkLineInRCASel.Checked && !chkInputMuteAll.Checked)
                FWC.WriteCodecReg(0x14, InputSliderToRegVal(tbLineInRCA.Value));

            this.toolTip1.SetToolTip(this.tbLineInRCA, "Line Volume: " + tbLineInRCA.Value);
        }

        private void tbLineInPhono_Scroll(object sender, System.EventArgs e)
        {
            if (chkLineInPhonoSel.Checked && !chkInputMuteAll.Checked)
                FWC.WriteCodecReg(0x15, InputSliderToRegVal(tbLineInPhono.Value));

            this.toolTip1.SetToolTip(this.tbLineInPhono, "Bal Line In Volume: " + tbLineInPhono.Value);
        }

        private void tbLineInDB9_Scroll(object sender, System.EventArgs e)
        {
            if (chkLineInDB9Sel.Checked && !chkInputMuteAll.Checked)
                FWC.WriteCodecReg(0x13, InputSliderToRegVal(tbLineInDB9.Value));

            this.toolTip1.SetToolTip(this.tbLineInDB9, "FlexWire Volume: " + tbLineInDB9.Value);
        }

        private void chkMicSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMicSel.Checked)
            {
                chkLineInRCASel.Checked = false;
                chkLineInPhonoSel.Checked = false;
                chkLineInDB9Sel.Checked = false;

                if (!chkInputMuteAll.Checked)
                    FWC.WriteCodecReg(0x16, InputSliderToRegVal(tbMic.Value));
                Audio.IN_TX_L = 7;
            }
            else FWC.WriteCodecReg(0x16, 0x80);
        }

        private void chkLineInRCASel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLineInRCASel.Checked)
            {
                chkMicSel.Checked = false;
                chkLineInPhonoSel.Checked = false;
                chkLineInDB9Sel.Checked = false;

                if (!chkInputMuteAll.Checked)
                    FWC.WriteCodecReg(0x14, InputSliderToRegVal(tbLineInRCA.Value));
                Audio.IN_TX_L = 5;
            }
            else FWC.WriteCodecReg(0x14, 0x80);
        }

        private void chkLineInPhonoSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLineInPhonoSel.Checked)
            {
                chkMicSel.Checked = false;
                chkLineInRCASel.Checked = false;
                chkLineInDB9Sel.Checked = false;

                if (!chkInputMuteAll.Checked)
                    FWC.WriteCodecReg(0x15, InputSliderToRegVal(tbLineInPhono.Value));
                Audio.IN_TX_L = 6;
            }
            else FWC.WriteCodecReg(0x15, 0x80);
        }

        private void chkLineInDB9Sel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLineInDB9Sel.Checked)
            {
                chkMicSel.Checked = false;
                chkLineInRCASel.Checked = false;
                chkLineInPhonoSel.Checked = false;

                if (!chkInputMuteAll.Checked)
                    FWC.WriteCodecReg(0x13, InputSliderToRegVal(tbLineInDB9.Value));
                Audio.IN_TX_L = 4;
            }
            else FWC.WriteCodecReg(0x13, 0x80);
        }

        private void chkInputMuteAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkInputMuteAll.Checked)
            {
                chkInputMuteAll.BackColor = console.ButtonSelectedColor;
                for (int i = 0x13; i <= 0x16; i++)
                    FWC.WriteCodecReg(i, 0x80);
            }
            else
            {
                chkInputMuteAll.BackColor = SystemColors.Control;
                if (chkMicSel.Checked) tbMic_Scroll(this, EventArgs.Empty);
                if (chkLineInRCASel.Checked) tbLineInRCA_Scroll(this, EventArgs.Empty);
                if (chkLineInPhonoSel.Checked) tbLineInPhono_Scroll(this, EventArgs.Empty);
                if (chkLineInDB9Sel.Checked) tbLineInDB9_Scroll(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Output Event Handlers

        private void tbIntSpkr_Scroll(object sender, System.EventArgs e)
        {
            FWC.WriteCodecReg(0x0F, (byte)(0xFF - tbIntSpkr.Value));
        }

        private void tbExtSpkr_Scroll(object sender, System.EventArgs e)
        {
            FWC.WriteCodecReg(0x0C, (byte)(0xFF - tbExtSpkr.Value));
            FWC.WriteCodecReg(0x0D, (byte)(0xFF - tbExtSpkr.Value));
        }

        private void tbHeadphone_Scroll(object sender, System.EventArgs e)
        {
            FWC.WriteCodecReg(0x0A, (byte)(0xFF - tbHeadphone.Value));
            FWC.WriteCodecReg(0x0B, (byte)(0xFF - tbHeadphone.Value));
        }

        private void tbLineOutRCA_Scroll(object sender, System.EventArgs e)
        {
            FWC.WriteCodecReg(0x0E, (byte)(0xFF - tbLineOutRCA.Value));
        }

        private void chkIntSpkrSel_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            FWC.ReadCodecReg(0x07, out val);
            if (chkIntSpkrSel.Checked)
            {
                FWC.SetIntSpkr(true);
                FWC.WriteCodecReg(0x07, (byte)(val & 0x7C));
            }
            else
            {
                FWC.WriteCodecReg(0x07, (byte)(val | 0x80));
                FWC.SetIntSpkr(false);
            }
        }

        private void chkExtSpkrSel_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            FWC.ReadCodecReg(0x07, out val);

            Debug.WriteLine("chkextspkrsel " + chkExtSpkrSel.Checked);

            if (chkExtSpkrSel.Checked)
                FWC.WriteCodecReg(0x07, (byte)(val & 0xCC));
            else
                FWC.WriteCodecReg(0x07, (byte)(val | 0x30));
        }

        private void chkHeadphoneSel_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            FWC.ReadCodecReg(0x07, out val);
            if (chkHeadphoneSel.Checked)
            {
                FWC.SetHeadphone(true);
                FWC.WriteCodecReg(0x07, (byte)(val & 0xF0));
            }
            else
            {
                FWC.WriteCodecReg(0x07, (byte)(val | 0x0C));
                FWC.SetHeadphone(false);
            }
        }

        private void chkLineOutRCASel_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            FWC.ReadCodecReg(0x07, out val);
            if (chkLineOutRCASel.Checked)
                FWC.WriteCodecReg(0x07, (byte)(val & 0xBC));
            else
                FWC.WriteCodecReg(0x07, (byte)(val | 0x40));
        }

        private void chkOutputMuteAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkOutputMuteAll.Checked)
            {
                chkOutputMuteAll.BackColor = console.ButtonSelectedColor;
                FWC.WriteCodecReg(0x07, 0xFC);
            }
            else
            {
                chkOutputMuteAll.BackColor = SystemColors.Control;
                chkIntSpkrSel_CheckedChanged(this, EventArgs.Empty);
                chkExtSpkrSel_CheckedChanged(this, EventArgs.Empty);
                chkHeadphoneSel_CheckedChanged(this, EventArgs.Empty);
                chkLineOutRCASel_CheckedChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Other Event Handlers

        private void FWCMixForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FWCMixer");
        }

        #endregion

        private void tbLineInPhono_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInPhono, "Bal Line In Volume: " + tbLineInPhono.Value);

        }

        private void tbLineInPhono_MouseHover(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInPhono, "Bal Line In Volume: " + tbLineInPhono.Value);
        }

        private void tbMic_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbMic, "Mic Volume: " + tbMic.Value);
        }

        private void tbMic_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbMic, "Mic Volume: " + tbMic.Value);
        }

        private void tbMic_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbMic, "Mic Volume: " + tbMic.Value);
        }

        private void tbLineInRCA_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInRCA, "Line Volume: " + tbLineInRCA.Value);
        }

        private void tbLineInRCA_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInRCA, "Line Volume: " + tbLineInRCA.Value);
        }

        private void tbLineInRCA_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInRCA, "Line Volume: " + tbLineInRCA.Value);
        }

        private void tbLineInDB9_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInDB9, "FlexWire Volume: " + tbLineInDB9.Value);
        }

        private void tbLineInDB9_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInDB9, "FlexWire Volume: " + tbLineInDB9.Value);
        }

        private void tbLineInDB9_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbLineInDB9, "FlexWire Volume: " + tbLineInDB9.Value);
        }
    }
}
