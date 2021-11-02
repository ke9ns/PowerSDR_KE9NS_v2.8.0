//=================================================================
// FLEX3000MixerForm.cs
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
    public partial class FLEX3000MixerForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
      
        #endregion

        #region Constructor and Destructor

        public FLEX3000MixerForm(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "FLEX3000MixerForm", false);

            chkMicSel_CheckedChanged(this, EventArgs.Empty);
            chkLineInDB9Sel_CheckedChanged(this, EventArgs.Empty);
            chkExtSpkrSel_CheckedChanged(this, EventArgs.Empty);
            chkHeadphoneSel_CheckedChanged(this, EventArgs.Empty);
            chkLineOutDB9Sel_CheckedChanged(this, EventArgs.Empty);
            tbMic_Scroll(this, EventArgs.Empty);
            tbLineInDB9_Scroll(this, EventArgs.Empty);
            tbExtSpkr_Scroll(this, EventArgs.Empty);
            tbHeadphone_Scroll(this, EventArgs.Empty);
            tbLineOutDB9_Scroll(this, EventArgs.Empty);

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

        public int LineOutDB9
        {
            get { return tbLineOutDB9.Value; }
            set
            {
                tbLineOutDB9.Value = value;
                tbLineOutDB9_Scroll(this, EventArgs.Empty);
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

        public string LineOutDB9Selected
        {
            get
            {
                if (chkLineOutDB9Sel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkLineOutDB9Sel.Checked = true;
                else
                    chkLineOutDB9Sel.Checked = false;
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

        private void tbMic_Scroll(object sender, System.EventArgs e)
        {
            if (chkMicSel.Checked && !chkInputMuteAll.Checked)
                FWC.WriteCodecReg(0x12, InputSliderToRegVal(tbMic.Value));
        }

        private void tbLineInDB9_Scroll(object sender, System.EventArgs e)
        {
            if (chkLineInDB9Sel.Checked && !chkInputMuteAll.Checked)
                FWC.WriteCodecReg(0x11, InputSliderToRegVal(tbLineInDB9.Value));
        }

        private void chkMicSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMicSel.Checked)
            {
                chkLineInDB9Sel.Checked = false;

                if (!chkInputMuteAll.Checked)
                    FWC.WriteCodecReg(0x12, InputSliderToRegVal(tbMic.Value));
                Audio.IN_TX_L = 3;
            }
            else FWC.WriteCodecReg(0x12, 0x80);
        }

        private void chkLineInDB9Sel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkLineInDB9Sel.Checked)
            {
                chkMicSel.Checked = false;

                if (!chkInputMuteAll.Checked)
                    FWC.WriteCodecReg(0x11, InputSliderToRegVal(tbLineInDB9.Value));
                Audio.IN_TX_L = 2;
            }
            else FWC.WriteCodecReg(0x11, 0x80);
        }

        private void chkInputMuteAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkInputMuteAll.Checked)
            {
                chkInputMuteAll.BackColor = console.ButtonSelectedColor;
                for (int i = 0x11; i <= 0x12; i++)
                    FWC.WriteCodecReg(i, 0x80);
            }
            else
            {
                chkInputMuteAll.BackColor = SystemColors.Control;
                if (chkMicSel.Checked) tbMic_Scroll(this, EventArgs.Empty);
                if (chkLineInDB9Sel.Checked) tbLineInDB9_Scroll(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Output Event Handlers

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

        private void tbLineOutDB9_Scroll(object sender, System.EventArgs e)
        {
            FWC.WriteCodecReg(0x0E, (byte)(0xFF - (byte)tbLineOutDB9.Value));
        }

        private void chkExtSpkrSel_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            FWC.ReadCodecReg(0x07, out val);
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

        private void chkLineOutDB9Sel_CheckedChanged(object sender, System.EventArgs e)
        {
            byte val = 0;
            FWC.ReadCodecReg(0x07, out val);
            if (chkLineOutDB9Sel.Checked)
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
                chkExtSpkrSel_CheckedChanged(this, EventArgs.Empty);
                chkHeadphoneSel_CheckedChanged(this, EventArgs.Empty);
                chkLineOutDB9Sel_CheckedChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Other Event Handlers

        private void FWCMixForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX3000MixerForm");
        }

        #endregion
    }
}
