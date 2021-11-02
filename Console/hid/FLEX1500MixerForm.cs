//=================================================================
// FLEX1500MixerForm.cs
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
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FLEX1500MixerForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
      
        #endregion

        #region Constructor and Destructor

        public FLEX1500MixerForm(Console c)
        {
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "FLEX1500MixerForm", false);

            chkMicSel_CheckedChanged(this, EventArgs.Empty);
            chkFlexWireInSel_CheckedChanged(this, EventArgs.Empty);
            chkPhonesSel_CheckedChanged(this, EventArgs.Empty);
            chkFlexWireOutSel_CheckedChanged(this, EventArgs.Empty);
            tbMic_Scroll(this, EventArgs.Empty);
            tbFlexWireIn_Scroll(this, EventArgs.Empty);
            tbPhones_Scroll(this, EventArgs.Empty);
            tbFlexWireOut_Scroll(this, EventArgs.Empty);

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

        public int MicInput
        {
            get { return tbMic.Value; }
            set
            {
                tbMic.Value = value;
                tbMic_Scroll(this, EventArgs.Empty);
            }
        }

        public int FlexWireIn
        {
            get { return tbFlexWireIn.Value; }
            set
            {
                tbFlexWireIn.Value = value;
                tbFlexWireIn_Scroll(this, EventArgs.Empty);
            }
        }

        public bool MicInputSelected
        {
            get { return chkMicSel.Checked; }
            set
            {
                if (value) chkMicSel.Checked = true;
                else chkFlexWireInSel.Checked = true;
            }
        }

        public string MicInputSelectedStr
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
                if (chkFlexWireInSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkFlexWireInSel.Checked = true;
                else
                    chkFlexWireInSel.Checked = false;
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

        public int Phones
        {
            get { return tbPhones.Value; }
            set
            {
                tbPhones.Value = value;
                tbPhones_Scroll(this, EventArgs.Empty);
            }
        }

        public int FlexWireOut
        {
            get { return tbFlexWireOut.Value; }
            set
            {
                tbFlexWireOut.Value = value;
                tbFlexWireOut_Scroll(this, EventArgs.Empty);
            }
        }

        public bool PhonesSelected
        {
            get { return chkPhonesSel.Checked; }
            set { chkPhonesSel.Checked = value; }
        }

        public string PhonesSelectedStr
        {
            get
            {
                if (chkPhonesSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkPhonesSel.Checked = true;
                else
                    chkPhonesSel.Checked = false;
            }
        }

        public bool FlexWireOutSelected
        {
            get { return chkFlexWireOutSel.Checked; }
            set { chkFlexWireOutSel.Checked = value; }
        }

        public string FlexWireOutSelectedStr
        {
            get
            {
                if (chkFlexWireOutSel.Checked)
                    return "1";
                else
                    return "0";
            }

            set
            {
                if (value == "1")
                    chkFlexWireOutSel.Checked = true;
                else
                    chkFlexWireOutSel.Checked = false;
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
            {
                //Debug.WriteLine("Mic Vol: 0x" + tbMic.Value.ToString("X").PadLeft(2, '0'));
                USBHID.SetTXGain(tbMic.Value);
            }
        }

        private void tbFlexWireIn_Scroll(object sender, System.EventArgs e)
        {
            if (chkFlexWireInSel.Checked && !chkInputMuteAll.Checked)
            {
                int val = tbFlexWireIn.Value;
                double percent = (val - tbFlexWireIn.Minimum) / (double)(tbFlexWireIn.Maximum - tbFlexWireIn.Minimum);
                int reg_val = (int)(0x2D * percent);
                USBHID.SetTXGain(reg_val);
                Debug.WriteLine("reg_val: " + reg_val);
                //USBHID.SetTXGain(tbFlexWireIn.Value);
            }
        }

        private void chkMicSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkMicSel.Checked)
            {
                chkFlexWireInSel.Checked = false;

                if (!chkInputMuteAll.Checked)
                {
                    USBHID.SetMicSel(true);
                    //Thread.Sleep(10);
                    USBHID.SetTXGain(tbMic.Value);
                }
            }
            else if (!chkFlexWireInSel.Checked)
                chkMicSel.Checked = true;
        }

        private void chkFlexWireInSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkFlexWireInSel.Checked)
            {
                chkMicSel.Checked = false;

                if (!chkInputMuteAll.Checked)
                {
                    USBHID.SetMicSel(false);
                    //Thread.Sleep(10);
                    USBHID.SetTXGain(tbFlexWireIn.Value);
                }
            }
            else if (!chkMicSel.Checked)
                chkFlexWireInSel.Checked = true;
        }

        private void chkInputMuteAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkInputMuteAll.Checked)
            {
                chkInputMuteAll.BackColor = console.ButtonSelectedColor;
                USBHID.SetTXGain(0x80);
            }
            else
            {
                chkInputMuteAll.BackColor = SystemColors.Control;
                if (chkMicSel.Checked) tbMic_Scroll(this, EventArgs.Empty);
                if (chkFlexWireInSel.Checked) tbFlexWireIn_Scroll(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Output Event Handlers

        private void tbPhones_Scroll(object sender, System.EventArgs e)
        {
            USBHID.SetSpkGain(0x80 + 127 - tbPhones.Value);
        }

        private void tbFlexWireOut_Scroll(object sender, System.EventArgs e)
        {
            USBHID.SetLineOutGain(0x80 + 127 - tbFlexWireOut.Value);
        }

        private void chkPhonesSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!chkOutputMuteAll.Checked)
                USBHID.SetSpkOn(chkPhonesSel.Checked);
            else USBHID.SetSpkOn(false);
        }

        private void chkFlexWireOutSel_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!chkOutputMuteAll.Checked)
                USBHID.SetLineOutOn(chkFlexWireOutSel.Checked);
            else USBHID.SetLineOutOn(false);
        }

        private void chkOutputMuteAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkOutputMuteAll.Checked)
            {
                chkOutputMuteAll.BackColor = console.ButtonSelectedColor;
            }
            else
            {
                chkOutputMuteAll.BackColor = SystemColors.ControlLight;
            }

            chkPhonesSel_CheckedChanged(this, EventArgs.Empty); Thread.Sleep(20);
            chkFlexWireOutSel_CheckedChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Other Event Handlers

        private void FLEX1500MixerForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX1500MixerForm");
        }

        #endregion

        private int dump_num = 0;
        private void btnCodecDump_Click(object sender, EventArgs e)
        {
            CodecDebug.Dump("dump" + (dump_num++).ToString() + ".txt");
        }

        private void FLEX1500MixerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.D)
                btnCodecDump.Visible = true;
        }
    }
}
