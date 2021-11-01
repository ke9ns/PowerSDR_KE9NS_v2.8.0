//=================================================================
// xvtr.cs
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
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class XVTRForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
        //private VUForm VUForm;
        
       

        //bool init = true;

        #endregion

        #region Constructor and Destructor

        public XVTRForm(Console c)
        {
            //
            // Required for Windows Form Designer support
            //

            InitializeComponent();
            console = c;
            SetupControlArrays();

            //this section is done in console with Common.Restore

            /*
            Common.RestoreForm(this, "XVTR", false);
            if (FWCEEPROM.VUOK)
            {
                udUCBAddr0.Value = 0;
                txtButtonText0.Text = "2m";
                udLOOffset0.Value = 125;
                udLOError0.Value = 0;
                udFreqBegin0.Value = 144;
                udFreqEnd0.Value = 148;
                //udRXGain0.Value = 32;
                chkRXOnly0.Checked = false;
                udPower0.Value = 100;
                //chkXVTRRF0.Checked = false;

                udUCBAddr1.Value = 1;
                txtButtonText1.Text = "70cm";
                udLOOffset1.Value = 400;
                udLOError1.Value = 0;
                udFreqBegin1.Value = 430;
                udFreqEnd1.Value = 450;
                //udRXGain1.Value = 28;
                chkRXOnly1.Checked = false;
                udPower1.Value = 100;
                //chkXVTRRF1.Checked = false;

                chkEnable0.Checked = true;
                chkEnable1.Checked = true;
                chkVHFIFGain.Enabled = true;
                chkUHFIFGain.Enabled = true;
                chkVHFPAEnable.Enabled = true;
                chkUHFPAEnable.Enabled = true;
                label_VUModulePresent.Visible = true;
                label_VUModuleNotPresent.Visible = false;

            }
            else
            {
                chkEnable0.Checked = false;
                chkEnable1.Checked = false;
                chkVHFIFGain.Enabled = false;
                chkUHFIFGain.Enabled = false;
                chkVHFPAEnable.Enabled = false;
                chkUHFPAEnable.Enabled = false;
                chkEnable0.Checked = false;
                chkEnable1.Checked = false;
                label_VUModulePresent.Visible = false;
                label_VUModuleNotPresent.Visible = true;
            }
         */
            if (console.fwc_init && console.CurrentModel == Model.FLEX5000)
                lblXVTRRF.Text = "Split RF";
            else if (console.fwc_init && console.CurrentModel == Model.FLEX3000)
            {
                lblXVTRRF.Visible = false;
                chkXVTRRF0.Visible = false;
                chkXVTRRF1.Visible = false;
                chkXVTRRF2.Visible = false;
                chkXVTRRF3.Visible = false;
                chkXVTRRF4.Visible = false;
                chkXVTRRF5.Visible = false;
                chkXVTRRF6.Visible = false;
                chkXVTRRF7.Visible = false;
                chkXVTRRF8.Visible = false;
                chkXVTRRF9.Visible = false;
                chkXVTRRF10.Visible = false;
                chkXVTRRF11.Visible = false;
                chkXVTRRF12.Visible = false;
                chkXVTRRF13.Visible = false;
                chkXVTRRF14.Visible = false;
                chkXVTRRF15.Visible = false;
            }

            Common.RestoreForm(this, "XVTR", false);
            if (FWCEEPROM.VUOK)
            {
                chkVHFIFGain_CheckedChanged(this, EventArgs.Empty);
                chkUHFIFGain_CheckedChanged(this, EventArgs.Empty);
                chkVHFPAEnable_CheckedChanged(this, EventArgs.Empty);
                chkUHFPAEnable_CheckedChanged(this, EventArgs.Empty);

                //FWC.SetEN2M(true); //XVINT
                udUCBAddr0.Value = 0;
                txtButtonText0.Text = "2m";
                udLOOffset0.Value = 125;              // ke9ns was 125 .217
                                                      //   udLOError0.Value = 0;  // ke9ns was not commented out
                udFreqBegin0.Value = 128;            // ke9ns was 134 144 .217
                udFreqEnd0.Value = 180;              // ke9ns was 148
                //udRXGain0.Value = 32;
                //chkRXOnly0.Checked = false;
                //udPower0.Value = 100;
                //chkXVTRRF0.Checked = false;

                udUCBAddr1.Value = 1;
                txtButtonText1.Text = "70cm";
                udLOOffset1.Value = 400;
                // udLOError1.Value = 0;    // ke9ns was not commented out
                udFreqBegin1.Value = 430;
                udFreqEnd1.Value = 470;  // .224 was 450
                //udRXGain1.Value = 28;
                //chkRXOnly1.Checked = false;
                //udPower1.Value = 100;
                //chkXVTRRF1.Checked = false;

                chkEnable0.Checked = true;
                chkEnable1.Checked = true;
                chkVHFIFGain.Enabled = true;
                chkUHFIFGain.Enabled = true;
                chkVHFPAEnable.Enabled = true;
                chkUHFPAEnable.Enabled = true;
                label_VUModulePresent.Visible = true;
                label_VUModuleNotPresent.Visible = false;

            }
            else
            {


                chkEnable0.Checked = false;
                chkEnable1.Checked = false;

                chkVHFIFGain.Enabled = false;
                chkUHFIFGain.Enabled = false;
                chkVHFPAEnable.Enabled = false;
                chkUHFPAEnable.Enabled = false;
                chkEnable0.Checked = false;
                chkEnable1.Checked = false;
                label_VUModulePresent.Visible = false;
                label_VUModuleNotPresent.Visible = true;
            }

            //init = false;
        }



        //    public XVTRForm(VUForm v)
        //    {
        //        VUForm = v;
        //    }

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

        public bool VIFGain
        {
            get { return chkVHFIFGain.Checked; }
        }

        public bool UIFGain
        {
            get { return chkUHFIFGain.Checked; }
        }

        public bool VPA
        {
            get { return chkVHFPAEnable.Checked; }
        }

        public bool UPA
        {
            get { return chkUHFPAEnable.Checked; }
        }

        #endregion

        #region Misc Routines

        /// <summary>
		/// Returns an index that indicates which band the frequency is in.
		/// </summary>
		/// <param name="freq">The frequency in MHz</param>
		/// <returns>The index of the band that contains the frequency or -1 if
		/// the frequency is not found.</returns>
		public int XVTRFreq(double freq)
        {
            for (int i = 0; i < 16; i++)
            {
                if (enabled[i].Checked)
                {
                    if (freq >= (double)begin[i].Value && freq <= (double)end[i].Value)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns a translated frequency based on the xvtr data.  Takes into
        /// account the LO Offset and correction.
        /// </summary>
        /// <param name="freq">Input Frequency in MHz</param>
        /// <returns>The translated frequency based on the xvtr data.  If the
        /// frequency is not within a defined xvtr setup, then just return the
        /// input freq.</returns>
        public double TranslateFreq(double freq)
        {
            for (int i = 0; i < 16; i++)
            {
                if (enabled[i].Checked)
                {
                    if (freq >= (double)begin[i].Value && freq <= (double)end[i].Value)
                    {
                        return (double)(freq - (double)lo_offset[i].Value + ((double)lo_error[i].Value / 1000.0));
                    }
                }
            }

            
            return freq;
        }

        private void SetupControlArrays()
        {
            ucb_addr = new NumericUpDownTS[16];
            ucb_addr[0] = udUCBAddr0;
            ucb_addr[1] = udUCBAddr1;
            ucb_addr[2] = udUCBAddr2;
            ucb_addr[3] = udUCBAddr3;
            ucb_addr[4] = udUCBAddr4;
            ucb_addr[5] = udUCBAddr5;
            ucb_addr[6] = udUCBAddr6;
            ucb_addr[7] = udUCBAddr7;
            ucb_addr[8] = udUCBAddr8;
            ucb_addr[9] = udUCBAddr9;
            ucb_addr[10] = udUCBAddr10;
            ucb_addr[11] = udUCBAddr11;
            ucb_addr[12] = udUCBAddr12;
            ucb_addr[13] = udUCBAddr13;
            ucb_addr[14] = udUCBAddr14;
            ucb_addr[15] = udUCBAddr15;

            begin = new NumericUpDownTS[16];
            begin[0] = udFreqBegin0;
            begin[1] = udFreqBegin1;
            begin[2] = udFreqBegin2;
            begin[3] = udFreqBegin3;
            begin[4] = udFreqBegin4;
            begin[5] = udFreqBegin5;
            begin[6] = udFreqBegin6;
            begin[7] = udFreqBegin7;
            begin[8] = udFreqBegin8;
            begin[9] = udFreqBegin9;
            begin[10] = udFreqBegin10;
            begin[11] = udFreqBegin11;
            begin[12] = udFreqBegin12;
            begin[13] = udFreqBegin13;
            begin[14] = udFreqBegin14;
            begin[15] = udFreqBegin15;

            end = new NumericUpDownTS[16];
            end[0] = udFreqEnd0;
            end[1] = udFreqEnd1;
            end[2] = udFreqEnd2;
            end[3] = udFreqEnd3;
            end[4] = udFreqEnd4;
            end[5] = udFreqEnd5;
            end[6] = udFreqEnd6;
            end[7] = udFreqEnd7;
            end[8] = udFreqEnd8;
            end[9] = udFreqEnd9;
            end[10] = udFreqEnd10;
            end[11] = udFreqEnd11;
            end[12] = udFreqEnd12;
            end[13] = udFreqEnd13;
            end[14] = udFreqEnd14;
            end[15] = udFreqEnd15;

            enabled = new CheckBoxTS[16];
            enabled[0] = chkEnable0;
            enabled[1] = chkEnable1;
            enabled[2] = chkEnable2;
            enabled[3] = chkEnable3;
            enabled[4] = chkEnable4;
            enabled[5] = chkEnable5;
            enabled[6] = chkEnable6;
            enabled[7] = chkEnable7;
            enabled[8] = chkEnable8;
            enabled[9] = chkEnable9;
            enabled[10] = chkEnable10;
            enabled[11] = chkEnable11;
            enabled[12] = chkEnable12;
            enabled[13] = chkEnable13;
            enabled[14] = chkEnable14;
            enabled[15] = chkEnable15;

            lo_offset = new NumericUpDownTS[16];
            lo_offset[0] = udLOOffset0;
            lo_offset[1] = udLOOffset1;
            lo_offset[2] = udLOOffset2;
            lo_offset[3] = udLOOffset3;
            lo_offset[4] = udLOOffset4;
            lo_offset[5] = udLOOffset5;
            lo_offset[6] = udLOOffset6;
            lo_offset[7] = udLOOffset7;
            lo_offset[8] = udLOOffset8;
            lo_offset[9] = udLOOffset9;
            lo_offset[10] = udLOOffset10;
            lo_offset[11] = udLOOffset11;
            lo_offset[12] = udLOOffset12;
            lo_offset[13] = udLOOffset13;
            lo_offset[14] = udLOOffset14;
            lo_offset[15] = udLOOffset15;

            lo_error = new NumericUpDownTS[16];
            lo_error[0] = udLOError0;
            lo_error[1] = udLOError1;
            lo_error[2] = udLOError2;
            lo_error[3] = udLOError3;
            lo_error[4] = udLOError4;
            lo_error[5] = udLOError5;
            lo_error[6] = udLOError6;
            lo_error[7] = udLOError7;
            lo_error[8] = udLOError8;
            lo_error[9] = udLOError9;
            lo_error[10] = udLOError10;
            lo_error[11] = udLOError11;
            lo_error[12] = udLOError12;
            lo_error[13] = udLOError13;
            lo_error[14] = udLOError14;
            lo_error[15] = udLOError15;

            rx_gain = new NumericUpDownTS[16];
            rx_gain[0] = udRXGain0;
            rx_gain[1] = udRXGain1;
            rx_gain[2] = udRXGain2;
            rx_gain[3] = udRXGain3;
            rx_gain[4] = udRXGain4;
            rx_gain[5] = udRXGain5;
            rx_gain[6] = udRXGain6;
            rx_gain[7] = udRXGain7;
            rx_gain[8] = udRXGain8;
            rx_gain[9] = udRXGain9;
            rx_gain[10] = udRXGain10;
            rx_gain[11] = udRXGain11;
            rx_gain[12] = udRXGain12;
            rx_gain[13] = udRXGain13;
            rx_gain[14] = udRXGain14;
            rx_gain[15] = udRXGain15;

            rx_only = new CheckBoxTS[16];
            rx_only[0] = chkRXOnly0;
            rx_only[1] = chkRXOnly1;
            rx_only[2] = chkRXOnly2;
            rx_only[3] = chkRXOnly3;
            rx_only[4] = chkRXOnly4;
            rx_only[5] = chkRXOnly5;
            rx_only[6] = chkRXOnly6;
            rx_only[7] = chkRXOnly7;
            rx_only[8] = chkRXOnly8;
            rx_only[9] = chkRXOnly9;
            rx_only[10] = chkRXOnly10;
            rx_only[11] = chkRXOnly11;
            rx_only[12] = chkRXOnly12;
            rx_only[13] = chkRXOnly13;
            rx_only[14] = chkRXOnly14;
            rx_only[15] = chkRXOnly15;

            power = new NumericUpDownTS[16];
            power[0] = udPower0;
            power[1] = udPower1;
            power[2] = udPower2;
            power[3] = udPower3;
            power[4] = udPower4;
            power[5] = udPower5;
            power[6] = udPower6;
            power[7] = udPower7;
            power[8] = udPower8;
            power[9] = udPower9;
            power[10] = udPower10;
            power[11] = udPower11;
            power[12] = udPower12;
            power[13] = udPower13;
            power[14] = udPower14;
            power[15] = udPower15;

            xvtr_rf = new CheckBoxTS[16];
            xvtr_rf[0] = chkXVTRRF0;
            xvtr_rf[1] = chkXVTRRF1;
            xvtr_rf[2] = chkXVTRRF2;
            xvtr_rf[3] = chkXVTRRF3;
            xvtr_rf[4] = chkXVTRRF4;
            xvtr_rf[5] = chkXVTRRF5;
            xvtr_rf[6] = chkXVTRRF6;
            xvtr_rf[7] = chkXVTRRF7;
            xvtr_rf[8] = chkXVTRRF8;
            xvtr_rf[9] = chkXVTRRF9;
            xvtr_rf[10] = chkXVTRRF10;
            xvtr_rf[11] = chkXVTRRF11;
            xvtr_rf[12] = chkXVTRRF12;
            xvtr_rf[13] = chkXVTRRF13;
            xvtr_rf[14] = chkXVTRRF14;
            xvtr_rf[15] = chkXVTRRF15;
        }

        public bool GetEnabled(int index)
        {
            return enabled[index].Checked;
        }

        public float GetBegin(int index)
        {
            return (float)begin[index].Value;
        }

        public float GetEnd(int index)
        {
            return (float)end[index].Value;
        }

        public int GetPower(int index)
        {
            return (int)power[index].Value;
        }

        public void SetPower(int index, int pwr)
        {
            power[index].Value = pwr;
        }

        public bool GetRXOnly(int index)
        {
            return rx_only[index].Checked;
        }

        public void SetRXOnly(int index, bool b)
        {
            rx_only[index].Checked = b;
        }

        public float GetRXGain(int index)
        {
            float ret_val = 0.0f;
            switch (index)
            {
                /*case 0:
                    if(!chkVHFIFGain.Checked)
                        ret_val = - (float)rx_gain[index].Value;
                    else
                        ret_val = - (float)rx_gainHigh[index].Value;
                    break;
                case 1:
                    if(!chkUHFIFGain.Checked)
                        ret_val = - (float)rx_gain[index].Value;
                    else
                        ret_val = - (float)rx_gainHigh[index].Value;
                    break;*/
                default:
                    ret_val = -(float)rx_gain[index].Value;
                    break;
            }
            return ret_val;
        }

        /*
        public float GetRXGainHigh(int index)
        {
            return (float)rx_gainHigh[index].Value;
        }
        */
        public byte GetXVTRAddr(int index)
        {
            return (byte)ucb_addr[index].Value;
        }

        public bool GetXVTRRF(int index)
        {
            return xvtr_rf[index].Checked;
        }

        #endregion

        #region Event Handlers

        private void XVTRForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "XVTR");
        }

        #region Enabled

        private void chkEnable0_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable0.Checked;
            udLOError0.Enabled = b;

            /*
			udUCBAddr0.Enabled = b;
			txtButtonText0.Enabled = b;
			udLOOffset0.Enabled = b;
			udLOError0.Enabled = b;
			udFreqBegin0.Enabled = b;
			udFreqEnd0.Enabled = b;
			udRXGain0.Enabled = b;
			chkRXOnly0.Enabled = b;
			udPower0.Enabled = b;
			chkXVTRRF0.Enabled = b;
            */

            console.SetVHFEnabled(0, b);
        }

        private void chkEnable1_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable1.Checked;

            udLOError1.Enabled = b;
            /*
           udUCBAddr1.Enabled = b;
           txtButtonText1.Enabled = b;
           udLOOffset1.Enabled = b;

           udFreqBegin1.Enabled = b;
           udFreqEnd1.Enabled = b;
           udRXGain1.Enabled = b;
           chkRXOnly1.Enabled = b;
           udPower1.Enabled = b;
           chkXVTRRF1.Enabled = b;
           */
            console.SetVHFEnabled(1, b);
        }

        private void chkEnable2_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable2.Checked;

            udUCBAddr2.Enabled = b;
            txtButtonText2.Enabled = b;
            udLOOffset2.Enabled = b;
            udLOError2.Enabled = b;
            udFreqBegin2.Enabled = b;
            udFreqEnd2.Enabled = b;
            udRXGain2.Enabled = b;
            chkRXOnly2.Enabled = b;
            udPower2.Enabled = b;
            chkXVTRRF2.Enabled = b;

            console.SetVHFEnabled(2, b);
        }

        private void chkEnable3_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable3.Checked;

            udUCBAddr3.Enabled = b;
            txtButtonText3.Enabled = b;
            udLOOffset3.Enabled = b;
            udLOError3.Enabled = b;
            udFreqBegin3.Enabled = b;
            udFreqEnd3.Enabled = b;
            udRXGain3.Enabled = b;
            chkRXOnly3.Enabled = b;
            udPower3.Enabled = b;
            chkXVTRRF3.Enabled = b;

            console.SetVHFEnabled(3, b);
        }

        private void chkEnable4_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable4.Checked;

            udUCBAddr4.Enabled = b;
            txtButtonText4.Enabled = b;
            udLOOffset4.Enabled = b;
            udLOError4.Enabled = b;
            udFreqBegin4.Enabled = b;
            udFreqEnd4.Enabled = b;
            udRXGain4.Enabled = b;
            chkRXOnly4.Enabled = b;
            udPower4.Enabled = b;
            chkXVTRRF4.Enabled = b;

            console.SetVHFEnabled(4, b);
        }

        private void chkEnable5_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable5.Checked;

            udUCBAddr5.Enabled = b;
            txtButtonText5.Enabled = b;
            udLOOffset5.Enabled = b;
            udLOError5.Enabled = b;
            udFreqBegin5.Enabled = b;
            udFreqEnd5.Enabled = b;
            udRXGain5.Enabled = b;
            chkRXOnly5.Enabled = b;
            udPower5.Enabled = b;
            chkXVTRRF5.Enabled = b;

            console.SetVHFEnabled(5, b);
        }

        private void chkEnable6_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable6.Checked;

            udUCBAddr6.Enabled = b;
            txtButtonText6.Enabled = b;
            udLOOffset6.Enabled = b;
            udLOError6.Enabled = b;
            udFreqBegin6.Enabled = b;
            udFreqEnd6.Enabled = b;
            udRXGain6.Enabled = b;
            chkRXOnly6.Enabled = b;
            udPower6.Enabled = b;
            chkXVTRRF6.Enabled = b;

            console.SetVHFEnabled(6, b);
        }

        private void chkEnable7_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable7.Checked;

            udUCBAddr7.Enabled = b;
            txtButtonText7.Enabled = b;
            udLOOffset7.Enabled = b;
            udLOError7.Enabled = b;
            udFreqBegin7.Enabled = b;
            udFreqEnd7.Enabled = b;
            udRXGain7.Enabled = b;
            chkRXOnly7.Enabled = b;
            udPower7.Enabled = b;
            chkXVTRRF7.Enabled = b;

            console.SetVHFEnabled(7, b);
        }

        private void chkEnable8_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable8.Checked;

            udUCBAddr8.Enabled = b;
            txtButtonText8.Enabled = b;
            udLOOffset8.Enabled = b;
            udLOError8.Enabled = b;
            udFreqBegin8.Enabled = b;
            udFreqEnd8.Enabled = b;
            udRXGain8.Enabled = b;
            chkRXOnly8.Enabled = b;
            udPower8.Enabled = b;
            chkXVTRRF8.Enabled = b;

            console.SetVHFEnabled(8, b);
        }

        private void chkEnable9_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable9.Checked;

            udUCBAddr9.Enabled = b;
            txtButtonText9.Enabled = b;
            udLOOffset9.Enabled = b;
            udLOError9.Enabled = b;
            udFreqBegin9.Enabled = b;
            udFreqEnd9.Enabled = b;
            udRXGain9.Enabled = b;
            chkRXOnly9.Enabled = b;
            udPower9.Enabled = b;
            chkXVTRRF9.Enabled = b;

            console.SetVHFEnabled(9, b);
        }

        private void chkEnable10_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable10.Checked;

            udUCBAddr10.Enabled = b;
            txtButtonText10.Enabled = b;
            udLOOffset10.Enabled = b;
            udLOError10.Enabled = b;
            udFreqBegin10.Enabled = b;
            udFreqEnd10.Enabled = b;
            udRXGain10.Enabled = b;
            chkRXOnly10.Enabled = b;
            udPower10.Enabled = b;
            chkXVTRRF10.Enabled = b;

            console.SetVHFEnabled(10, b);
        }

        private void chkEnable11_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable11.Checked;

            udUCBAddr11.Enabled = b;
            txtButtonText11.Enabled = b;
            udLOOffset11.Enabled = b;
            udLOError11.Enabled = b;
            udFreqBegin11.Enabled = b;
            udFreqEnd11.Enabled = b;
            udRXGain11.Enabled = b;
            chkRXOnly11.Enabled = b;
            udPower11.Enabled = b;
            chkXVTRRF11.Enabled = b;

            console.SetVHFEnabled(11, b);
        }

        private void chkEnable12_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable12.Checked;

            udUCBAddr12.Enabled = b;
            txtButtonText12.Enabled = b;
            udLOOffset12.Enabled = b;
            udLOError12.Enabled = b;
            udFreqBegin12.Enabled = b;
            udFreqEnd12.Enabled = b;
            udRXGain12.Enabled = b;
            chkRXOnly12.Enabled = b;
            udPower12.Enabled = b;
            chkXVTRRF12.Enabled = b;

            console.SetVHFEnabled(12, b);
        }

        private void chkEnable13_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable13.Checked;

            udUCBAddr13.Enabled = b;
            txtButtonText13.Enabled = b;
            udLOOffset13.Enabled = b;
            udLOError13.Enabled = b;
            udFreqBegin13.Enabled = b;
            udFreqEnd13.Enabled = b;
            udRXGain13.Enabled = b;
            chkRXOnly13.Enabled = b;
            udPower13.Enabled = b;
            chkXVTRRF13.Enabled = b;

            console.SetVHFEnabled(13, b);
        }

        private void chkEnable14_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable14.Checked;

            udUCBAddr14.Enabled = b;
            txtButtonText14.Enabled = b;
            udLOOffset14.Enabled = b;
            udLOError14.Enabled = b;
            udFreqBegin14.Enabled = b;
            udFreqEnd14.Enabled = b;
            udRXGain14.Enabled = b;
            chkRXOnly14.Enabled = b;
            udPower14.Enabled = b;
            chkXVTRRF14.Enabled = b;
        }

        private void chkEnable15_CheckedChanged(object sender, System.EventArgs e)
        {
            bool b = chkEnable15.Checked;

            udUCBAddr15.Enabled = b;
            txtButtonText15.Enabled = b;
            udLOOffset15.Enabled = b;
            udLOError15.Enabled = b;
            udFreqBegin15.Enabled = b;
            udFreqEnd15.Enabled = b;
            udRXGain15.Enabled = b;
            chkRXOnly15.Enabled = b;
            udPower15.Enabled = b;
            chkXVTRRF15.Enabled = b;
        }

        #endregion

        private void txtButtonText_TextChanged(object sender, System.EventArgs e)
        {
            int val = Int32.Parse(((Control)sender).Name.Substring(13));
            console.SetVHFText(val, ((TextBoxTS)sender).Text);
        }

        #endregion

        private void chkUseXVTRTUNPWR_CheckedChanged(object sender, System.EventArgs e)
        {
            console.XVTRTunePower = chkUseXVTRTUNPWR.Checked;
        }

        private void chkXVTRRF_CheckedChanged(object sender, System.EventArgs e)
        {
            int index = int.Parse(((Control)sender).Name.Substring(9)); // ke9ns: 0=vu5k 2m, 1= vu5k 70cm, 2 = external xvtr  chkXVTRRF2.Checked = xvtr_rf[x].Checked

            if (console.RX1XVTRIndex == index)
            {
                console.LastRX1XVTRIndex = -1; // force reset
                console.VFOAFreq = console.VFOAFreq;
            }

            if (console.RX2XVTRIndex == index)
            {
                console.LastRX2XVTRIndex = -1; // force reset
                console.VFOBFreq = console.VFOBFreq;
            }

            if (console.TXXVTRIndex == index)
            {
                console.LastTXXVTRIndex = -1; // force reset
                if (console.RX2Enabled && console.VFOSplit)
                    console.VFOASubFreq = console.VFOASubFreq;
                else if (console.VFOSplit)
                    console.VFOBFreq = console.VFOBFreq;
                else
                    console.VFOAFreq = console.VFOAFreq;
            }

            if ((console.RX1XVTRIndex > 1) && console.CurrentModel == Model.FLEX5000) // .213
            {
                if (GetXVTRRF(console.RX1XVTRIndex) == true)
                {

                    console.lblAntRX1a.Text = "XVRX";
                    console.lblAntTXa.Text = "XVTX";
                }
                else
                {
                    console.lblAntRX1a.Text = "XVTX/C";
                    console.lblAntTXa.Text = "XVTX/C";
                }

            }

        } //chkXVTRRF_CheckedChanged

        private void udRXGain_ValueChanged(object sender, System.EventArgs e)
        {
            int index = int.Parse(((Control)sender).Name.Substring(8));
            if (console.RX1XVTRIndex == index)
            {
                console.LastRX1XVTRIndex = -1; // force reset
                console.VFOAFreq = console.VFOAFreq;
            }

            if (console.RX2XVTRIndex == index)
            {
                console.LastRX2XVTRIndex = -1; // force reset
                console.VFOBFreq = console.VFOBFreq;
            }
        }

        private void udRXGainHigh0_ValueChanged(object sender, EventArgs e)
        {
            int index = 0;
            if (console.RX1XVTRIndex == index)
            {
                console.LastRX1XVTRIndex = -1; // force reset
                console.VFOAFreq = console.VFOAFreq;
            }

            if (console.RX2XVTRIndex == index)
            {
                console.LastRX2XVTRIndex = -1; // force reset
                console.VFOBFreq = console.VFOBFreq;
            }
        }
        private void udRXGainHigh1_ValueChanged(object sender, EventArgs e)
        {
            int index = 1;
            if (console.RX1XVTRIndex == index)
            {
                console.LastRX1XVTRIndex = -1; // force reset
                console.VFOAFreq = console.VFOAFreq;
            }

            if (console.RX2XVTRIndex == index)
            {
                console.LastRX2XVTRIndex = -1; // force reset
                console.VFOBFreq = console.VFOBFreq;
            }
        }

        private void chkVHFIFGain_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVHFIFGain.Checked)
            {
                FWC.SetVU_VIF(true);
                if (console.RX1XVTRIndex == 0)   //VHF
                    console.RX1XVTRGainOffset = console.vhf_level_table[1];
                else if (console.RX2XVTRIndex == 0)  //VHF
                    console.RX2XVTRGainOffset = console.vhf_level_table[1];
            }
            else
            {
                FWC.SetVU_VIF(false);
                if (console.RX1XVTRIndex == 0)   //VHF
                    console.RX1XVTRGainOffset = console.vhf_level_table[0];
                else if (console.RX2XVTRIndex == 0)  //VHF
                    console.RX2XVTRGainOffset = console.vhf_level_table[0];
            }
        }

        private void chkVHFPAEnable_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_VPA(chkVHFPAEnable.Checked);
        }

        private void chkUHFIFGain_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUHFIFGain.Checked)
            {
                FWC.SetVU_UIF(true);
                if (console.RX1XVTRIndex == 1)   //UHF
                    console.RX1XVTRGainOffset = console.uhf_level_table[1];
                else if (console.RX2XVTRIndex == 1)  //UHF
                    console.RX2XVTRGainOffset = console.uhf_level_table[1];
            }
            else
            {
                FWC.SetVU_UIF(false);
                if (console.RX1XVTRIndex == 1)   //UHF
                    console.RX1XVTRGainOffset = console.uhf_level_table[0];
                else if (console.RX2XVTRIndex == 1)  //UHF
                    console.RX2XVTRGainOffset = console.uhf_level_table[0];
            }


        }

        private void chkUHFPAEnable_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_UPA(chkUHFPAEnable.Checked);
        }

        private void chkRXOnly0_CheckedChanged(object sender, EventArgs e)
        {
            if (console.TXBand != Band.VHF0)
                return;

            console.setupForm.RXOnly = chkRXOnly0.Checked;
        }

        private void chkRXOnly1_CheckedChanged(object sender, EventArgs e)
        {
            if (console.TXBand != Band.VHF1)
                return;
            console.setupForm.RXOnly = chkRXOnly1.Checked;

        }

        private void udPower_ValueChanged(object sender, EventArgs e)
        {
            int index = int.Parse(((Control)sender).Name.Substring(7));
            console.SetPower((Band)((int)Band.VHF0 + index), (int)((NumericUpDownTS)sender).Value);
        }

        private void udLOOffset0_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ChkAlwaysOnTop1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop1.Checked;
        }

        private void XVTRForm_MouseEnter(object sender, EventArgs e)
        {
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop1.Checked == true) this.Activate();
        }

        private void chkFlexWire_CheckedChanged(object sender, EventArgs e)
        {
            if (console.ucbForm == null || console.ucbForm.IsDisposed)
                console.ucbForm = new UCBForm(console);

            if (chkFlexWire.Checked == true)
            {
                chkFlexWire.BackColor = Color.Yellow;
                console.ucbForm.Show();
                console.ucbForm.Focus();
            }
            else
            {
                chkFlexWire.BackColor = SystemColors.ControlLight;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtButtonText2_MouseHover(object sender, EventArgs e) // .229
        {
            float begin1 = GetBegin(2);
            float end1 = GetEnd(2);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText2, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString()  );

        }

        private void txtButtonText3_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(3);
            float end1 = GetEnd(3);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText3, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText4_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(4);
            float end1 = GetEnd(4);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText4, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText0_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(0);
            float end1 = GetEnd(0);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText0, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText1_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(1);
            float end1 = GetEnd(1);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText1, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText5_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(5);
            float end1 = GetEnd(5);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText5, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText6_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(6);
            float end1 = GetEnd(6);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText6, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText7_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(7);
            float end1 = GetEnd(7);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText1, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText8_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(8);
            float end1 = GetEnd(8);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText8, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText9_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(9);
            float end1 = GetEnd(9);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText9, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText10_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(10);
            float end1 = GetEnd(10);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText10, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText11_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(11);
            float end1 = GetEnd(11);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText11, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText12_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(12);
            float end1 = GetEnd(12);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText12, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText13_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(13);
            float end1 = GetEnd(13);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText13, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText14_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(14);
            float end1 = GetEnd(14);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText14, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }

        private void txtButtonText15_MouseHover(object sender, EventArgs e)
        {
            float begin1 = GetBegin(15);
            float end1 = GetEnd(15);
            double transbegin = TranslateFreq(begin1);
            double transend = TranslateFreq(end1);
            this.toolTip1.SetToolTip(this.txtButtonText15, "Translated Low Mhz: " + transbegin.ToString() + "\n" + "Translated High Mhz: " + transend.ToString());

        }
    }
}
