//=================================================================
// eqform.cs
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

// ke9ns:
//   private void tbPEQ1_Scroll(object sender, EventArgs e)
//   PEQ get/set array of octave's, gains, freq

//   private void tbTXEQ_Scroll(object sender, System.EventArgs e) //  ALL EQ slider changes come here
//   TXEQ get/set array of gain values

//   private void tbTX28EQ15_Scroll(object sender, EventArgs e)
//   TXEQ28 get/set array of gain values

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for EQForm.
    /// </summary>
    public partial class EQForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
      
        #endregion

        #region Constructor and Destructor

        public EQForm(Console c) // ke9ns: form LOAD
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;
            Common.RestoreForm(this, "EQForm", false);

            // must do a eqform.TXEQ get
            // then come here and do a TXEQ28 set

            // ke9ns add: to load up arrays with slider values at startup, so you dont lose slider data when switching between EQ types

            if (rad3Band.Checked)
            {
                console.dsp.GetDSPRX(0, 0).RXEQ3 = RXEQ;
                console.dsp.GetDSPRX(0, 1).RXEQ3 = RXEQ;
                console.dsp.GetDSPRX(1, 0).RXEQ3 = RXEQ;

                //  console.dsp.GetDSPTX(0).TXEQ3 = TXEQ;
            }
            else
            {
                console.dsp.GetDSPRX(0, 0).RXEQ10 = RXEQ;
                console.dsp.GetDSPRX(0, 1).RXEQ10 = RXEQ;
                console.dsp.GetDSPRX(1, 0).RXEQ10 = RXEQ;

                //   console.dsp.GetDSPTX(0).TXEQ10 = TXEQ;
            }


            console.dsp.GetDSPTX(0).PEQ = PEQ;           // ke9ns add: get PEQ buffer in dsp.cs loaded right away
            console.dsp.GetDSPTX(0).TXEQ28 = TXEQ28;    // ke9ns add: get TXEQ28 buffer in dsp.cs loaded right away

            tbRXEQ_Scroll(this, EventArgs.Empty);


            if (rad28Band.Checked == true)
            {

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 28;

                rad28Band_CheckedChanged(this, EventArgs.Empty);
                tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.

            }
            else if (radPEQ.Checked == true)
            {

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 9;

                radPEQ_CheckedChanged(this, EventArgs.Empty);
                tbPEQ1_Scroll(this, EventArgs.Empty); //

            }
            else if (chkBothEQ.Checked == true)
            {

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 37;

                rad28Band_CheckedChanged(this, EventArgs.Empty);
                tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.
                radPEQ_CheckedChanged(this, EventArgs.Empty);
                tbPEQ1_Scroll(this, EventArgs.Empty); //

            }
            else
            {
                if (rad10Band.Checked == true)
                {
                    console.dsp.GetDSPTX(0).TXEQ10 = TXEQ;

                    console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                    console.dsp.GetDSPTX(0).TXEQNumBands = 10;

                    rad10Band_CheckedChanged(this, EventArgs.Empty);
                    tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.
                }
                else
                {
                    console.dsp.GetDSPTX(0).TXEQ3 = TXEQ;

                    console.dsp.GetDSPRX(0, 0).RXEQNumBands = 3;
                    console.dsp.GetDSPTX(0).TXEQNumBands = 3;

                    rad3Band_CheckedChanged(this, EventArgs.Empty);
                    tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.
                }

            }


            picRXEQ.Invalidate();
            picTXEQ.Invalidate();

            console.dsp.GetDSPTX(0).PEQ = PEQ;
            console.dsp.GetDSPTX(0).TXEQ28 = TXEQ28;


            int[] peq = PEQ; // ke9ns get all the slider values here and put in array of ints

            if (radPEQ.Checked == true)
            {
                console.dsp.GetDSPTX(0).PEQ = peq; // ke9ns refers to update SetGrphTXEQ10  txeq[0] is the preamp slider;
            }

            Debug.WriteLine("EQ====INITIALIZE");

        } // EQForm(Console c)


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


        //==========================================================================
        // ke9ns add: when new TX profile loaded up
        public void EQLoad()
        {
            console.dsp.GetDSPTX(0).PEQ = PEQ;           // ke9ns add: get PEQ buffer in dsp.cs loaded right away
          //  console.dsp.GetDSPTX(0).TXEQ10 = TXEQ;
            console.dsp.GetDSPTX(0).TXEQ28 = TXEQ28;    // ke9ns add: get TXEQ28 buffer in dsp.cs loaded right away

            tbRXEQ_Scroll(this, EventArgs.Empty);


            if (rad28Band.Checked == true)
            {

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 28;

                rad28Band_CheckedChanged(this, EventArgs.Empty);
                tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.


            }
            else if (radPEQ.Checked == true)
            {

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 9;

                radPEQ_CheckedChanged(this, EventArgs.Empty);
                tbPEQ1_Scroll(this, EventArgs.Empty); //

            }
            else if (chkBothEQ.Checked == true)
            {

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 37;

                rad28Band_CheckedChanged(this, EventArgs.Empty);
                tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.
                radPEQ_CheckedChanged(this, EventArgs.Empty);
                tbPEQ1_Scroll(this, EventArgs.Empty); //

            }
            else
            {

                if (rad10Band.Checked == true)
                {
                    console.dsp.GetDSPTX(0).TXEQ10 = TXEQ;

                    console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                    console.dsp.GetDSPTX(0).TXEQNumBands = 10;

                    rad10Band_CheckedChanged(this, EventArgs.Empty);
                    tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.
                }
                else
                {
                    console.dsp.GetDSPTX(0).TXEQ3 = TXEQ;

                    console.dsp.GetDSPRX(0, 0).RXEQNumBands = 3;
                    console.dsp.GetDSPTX(0).TXEQNumBands = 3;

                    rad3Band_CheckedChanged(this, EventArgs.Empty);
                    tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.

                }
            }


            picRXEQ.Invalidate();
            picTXEQ.Invalidate();

            console.dsp.GetDSPTX(0).PEQ = PEQ;
            console.dsp.GetDSPTX(0).TXEQ28 = TXEQ28;

       

        } // EQLoad

        //==========================================================================
        public int NumBands
        {
            get
            {
                if (rad3Band.Checked) return 3;
                else if (rad10Band.Checked) return 10;
                else if (radPEQ.Checked) return 9;
                else return 28;

            }
            set
            {
                switch (value)
                {
                    case 3:
                        rad3Band.Checked = true;
                        break;
                    case 9:
                        radPEQ.Checked = true;
                        break;
                    case 10:
                        rad10Band.Checked = true;
                        break;
                    case 28:
                        rad28Band.Checked = true;
                        break;
                }
            }
        } // NumBands

        //====================================================================
        public int[] RXEQ
        {
            get
            {
                if (rad3Band.Checked)
                {
                    int[] eq = new int[4];
                    eq[0] = tbRXEQPreamp.Value;
                    eq[1] = tbRXEQ1.Value;
                    eq[2] = tbRXEQ5.Value;
                    eq[3] = tbRXEQ9.Value;
                    return eq;
                }
                else //if(rad10Band.Checked) or 28band or PEQ
                {
                    int[] eq = new int[11];
                    eq[0] = tbRXEQPreamp.Value;
                    eq[1] = tbRXEQ1.Value;
                    eq[2] = tbRXEQ2.Value;
                    eq[3] = tbRXEQ3.Value;
                    eq[4] = tbRXEQ4.Value;
                    eq[5] = tbRXEQ5.Value;
                    eq[6] = tbRXEQ6.Value;
                    eq[7] = tbRXEQ7.Value;
                    eq[8] = tbRXEQ8.Value;
                    eq[9] = tbRXEQ9.Value;
                    eq[10] = tbRXEQ10.Value;
                    return eq;
                }
            }

            set
            {
                if (rad3Band.Checked)
                {
                    if (value.Length < 4)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Error setting RX EQ");
                        return;
                    }
                    tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
                    tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
                    tbRXEQ5.Value = Math.Max(tbRXEQ5.Minimum, Math.Min(tbRXEQ5.Maximum, value[2]));
                    tbRXEQ9.Value = Math.Max(tbRXEQ9.Minimum, Math.Min(tbRXEQ9.Maximum, value[3]));
                }
                else  //ke9ns mod: if(rad10Band.Checked)
                {
                    if (value.Length < 11)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Error setting RX EQ");
                        return;
                    }
                    tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
                    tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
                    tbRXEQ2.Value = Math.Max(tbRXEQ2.Minimum, Math.Min(tbRXEQ2.Maximum, value[2]));
                    tbRXEQ3.Value = Math.Max(tbRXEQ3.Minimum, Math.Min(tbRXEQ3.Maximum, value[3]));
                    tbRXEQ4.Value = Math.Max(tbRXEQ4.Minimum, Math.Min(tbRXEQ4.Maximum, value[4]));
                    tbRXEQ5.Value = Math.Max(tbRXEQ5.Minimum, Math.Min(tbRXEQ5.Maximum, value[5]));
                    tbRXEQ6.Value = Math.Max(tbRXEQ6.Minimum, Math.Min(tbRXEQ6.Maximum, value[6]));
                    tbRXEQ7.Value = Math.Max(tbRXEQ7.Minimum, Math.Min(tbRXEQ7.Maximum, value[7]));
                    tbRXEQ8.Value = Math.Max(tbRXEQ8.Minimum, Math.Min(tbRXEQ8.Maximum, value[8]));
                    tbRXEQ9.Value = Math.Max(tbRXEQ9.Minimum, Math.Min(tbRXEQ9.Maximum, value[9]));
                    tbRXEQ10.Value = Math.Max(tbRXEQ10.Minimum, Math.Min(tbRXEQ10.Maximum, value[10]));
                }

                picRXEQ.Invalidate();
                tbRXEQ_Scroll(this, EventArgs.Empty);
            }
        } // RXEQ





        // ===============================================================
        // ke9ns: copy all sliders into an array to pass to dsp.cs
        public int[] TXEQ// 3 or 10 band
        {
            get
            {
                if (rad3Band.Checked)
                {
                    int[] eq = new int[4];
                    eq[0] = tbTXEQPreamp.Value;

                    eq[1] = tbTXEQ1.Value;
                    eq[2] = tbTXEQ5.Value;
                    eq[3] = tbTXEQ9.Value;
                    return eq;
                }
                else //ke9ns if(rad10Band.Checked)
                {

                    int[] eq = new int[11];
                    eq[0] = tbTXEQPreamp.Value;

                    eq[1] = tbTXEQ1.Value;
                    eq[2] = tbTXEQ2.Value;
                    eq[3] = tbTXEQ3.Value;
                    eq[4] = tbTXEQ4.Value;
                    eq[5] = tbTXEQ5.Value;
                    eq[6] = tbTXEQ6.Value;
                    eq[7] = tbTXEQ7.Value;
                    eq[8] = tbTXEQ8.Value;
                    eq[9] = tbTXEQ9.Value;
                    eq[10] = tbTXEQ10.Value;
                    return eq;

                }
            } // get
            set
            {
                if (rad3Band.Checked)
                {
                    if (value.Length < 4)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Error setting TX EQ");
                        return;
                    }
                    tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));

                    tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
                    tbTXEQ5.Value = Math.Max(tbTXEQ5.Minimum, Math.Min(tbTXEQ5.Maximum, value[2]));
                    tbTXEQ9.Value = Math.Max(tbTXEQ9.Minimum, Math.Min(tbTXEQ9.Maximum, value[3]));
                }
                else
                {
                    if (value.Length < 11)
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Error setting TX EQ");
                        return;
                    }
                    tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));

                    tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
                    tbTXEQ2.Value = Math.Max(tbTXEQ2.Minimum, Math.Min(tbTXEQ2.Maximum, value[2]));
                    tbTXEQ3.Value = Math.Max(tbTXEQ3.Minimum, Math.Min(tbTXEQ3.Maximum, value[3]));
                    tbTXEQ4.Value = Math.Max(tbTXEQ4.Minimum, Math.Min(tbTXEQ4.Maximum, value[4]));
                    tbTXEQ5.Value = Math.Max(tbTXEQ5.Minimum, Math.Min(tbTXEQ5.Maximum, value[5]));
                    tbTXEQ6.Value = Math.Max(tbTXEQ6.Minimum, Math.Min(tbTXEQ6.Maximum, value[6]));
                    tbTXEQ7.Value = Math.Max(tbTXEQ7.Minimum, Math.Min(tbTXEQ7.Maximum, value[7]));
                    tbTXEQ8.Value = Math.Max(tbTXEQ8.Minimum, Math.Min(tbTXEQ8.Maximum, value[8]));
                    tbTXEQ9.Value = Math.Max(tbTXEQ9.Minimum, Math.Min(tbTXEQ9.Maximum, value[9]));
                    tbTXEQ10.Value = Math.Max(tbTXEQ10.Minimum, Math.Min(tbTXEQ10.Maximum, value[10]));
                }

                //   picTXEQ.Invalidate();
                //	tbTXEQ_Scroll(this, EventArgs.Empty);
                //    tbTX28EQ15_Scroll(this, EventArgs.Empty);
            } // set

        } // TXEQ


        // ===============================================================
        // ke9ns: copy all sliders into an array to pass to dsp.cs
        public int[] TXEQ28
        {
            get
            {
              //   Debug.WriteLine("=== 28band TXEQ GET " + tbTXEQ28Preamp.Value + " , " + tbTX28EQ1.Value);

                int[] eq9 = new int[29];
                eq9[0] = tbTXEQ28Preamp.Value;

                eq9[1] = tbTX28EQ1.Value;
                eq9[2] = tbTX28EQ2.Value;
                eq9[3] = tbTX28EQ3.Value;
                eq9[4] = tbTX28EQ4.Value;
                eq9[5] = tbTX28EQ5.Value;
                eq9[6] = tbTX28EQ6.Value;
                eq9[7] = tbTX28EQ7.Value;
                eq9[8] = tbTX28EQ8.Value;
                eq9[9] = tbTX28EQ9.Value;
                eq9[10] = tbTX28EQ10.Value;
                eq9[11] = tbTX28EQ11.Value;
                eq9[12] = tbTX28EQ12.Value;
                eq9[13] = tbTX28EQ13.Value;
                eq9[14] = tbTX28EQ14.Value;
                eq9[15] = tbTX28EQ15.Value;
                eq9[16] = tbTX28EQ16.Value;
                eq9[17] = tbTX28EQ17.Value;
                eq9[18] = tbTX28EQ18.Value;
                eq9[19] = tbTX28EQ19.Value;
                eq9[20] = tbTX28EQ20.Value;
                eq9[21] = tbTX28EQ21.Value;
                eq9[22] = tbTX28EQ22.Value;
                eq9[23] = tbTX28EQ23.Value;
                eq9[24] = tbTX28EQ24.Value;
                eq9[25] = tbTX28EQ25.Value;
                eq9[26] = tbTX28EQ26.Value;
                eq9[27] = tbTX28EQ27.Value;
                eq9[28] = tbTX28EQ28.Value;
                //  Debug.WriteLine("=== 28band TXEQ GET1 ");
                return eq9;


            } // get
            set
            {

                 //   Debug.WriteLine("PEQ=== 28band TXEQ SET ");

                if (value.Length < 28)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Error setting TX EQ");
                    return;
                }
                tbTXEQ28Preamp.Value = Math.Max(tbTXEQ28Preamp.Minimum, Math.Min(tbTXEQ28Preamp.Maximum, value[0])); 

                tbTX28EQ1.Value = Math.Max(tbTX28EQ1.Minimum, Math.Min(tbTX28EQ1.Maximum, value[1]));
                tbTX28EQ2.Value = Math.Max(tbTX28EQ2.Minimum, Math.Min(tbTX28EQ2.Maximum, value[2]));
                tbTX28EQ3.Value = Math.Max(tbTX28EQ3.Minimum, Math.Min(tbTX28EQ3.Maximum, value[3]));
                tbTX28EQ4.Value = Math.Max(tbTX28EQ4.Minimum, Math.Min(tbTX28EQ4.Maximum, value[4]));
                tbTX28EQ5.Value = Math.Max(tbTX28EQ5.Minimum, Math.Min(tbTX28EQ5.Maximum, value[5]));
                tbTX28EQ6.Value = Math.Max(tbTX28EQ6.Minimum, Math.Min(tbTX28EQ6.Maximum, value[6]));
                tbTX28EQ7.Value = Math.Max(tbTX28EQ7.Minimum, Math.Min(tbTX28EQ7.Maximum, value[7]));
                tbTX28EQ8.Value = Math.Max(tbTX28EQ8.Minimum, Math.Min(tbTX28EQ8.Maximum, value[8]));
                tbTX28EQ9.Value = Math.Max(tbTX28EQ9.Minimum, Math.Min(tbTX28EQ9.Maximum, value[9]));
                tbTX28EQ10.Value = Math.Max(tbTX28EQ10.Minimum, Math.Min(tbTX28EQ10.Maximum, value[10]));

                tbTX28EQ11.Value = Math.Max(tbTX28EQ11.Minimum, Math.Min(tbTX28EQ11.Maximum, value[11]));
                tbTX28EQ12.Value = Math.Max(tbTX28EQ12.Minimum, Math.Min(tbTX28EQ12.Maximum, value[12]));
                tbTX28EQ13.Value = Math.Max(tbTX28EQ13.Minimum, Math.Min(tbTX28EQ13.Maximum, value[13]));
                tbTX28EQ14.Value = Math.Max(tbTX28EQ14.Minimum, Math.Min(tbTX28EQ14.Maximum, value[14]));
                tbTX28EQ15.Value = Math.Max(tbTX28EQ15.Minimum, Math.Min(tbTX28EQ15.Maximum, value[15]));
                tbTX28EQ16.Value = Math.Max(tbTX28EQ16.Minimum, Math.Min(tbTX28EQ16.Maximum, value[16]));
                tbTX28EQ17.Value = Math.Max(tbTX28EQ17.Minimum, Math.Min(tbTX28EQ17.Maximum, value[17]));
                tbTX28EQ18.Value = Math.Max(tbTX28EQ18.Minimum, Math.Min(tbTX28EQ18.Maximum, value[18]));
                tbTX28EQ19.Value = Math.Max(tbTX28EQ19.Minimum, Math.Min(tbTX28EQ19.Maximum, value[19]));
                tbTX28EQ20.Value = Math.Max(tbTX28EQ20.Minimum, Math.Min(tbTX28EQ20.Maximum, value[20]));

                tbTX28EQ21.Value = Math.Max(tbTX28EQ21.Minimum, Math.Min(tbTX28EQ21.Maximum, value[21]));
                tbTX28EQ22.Value = Math.Max(tbTX28EQ22.Minimum, Math.Min(tbTX28EQ22.Maximum, value[22]));
                tbTX28EQ23.Value = Math.Max(tbTX28EQ23.Minimum, Math.Min(tbTX28EQ23.Maximum, value[23]));
                tbTX28EQ24.Value = Math.Max(tbTX28EQ24.Minimum, Math.Min(tbTX28EQ24.Maximum, value[24]));
                tbTX28EQ25.Value = Math.Max(tbTX28EQ25.Minimum, Math.Min(tbTX28EQ25.Maximum, value[25]));
                tbTX28EQ26.Value = Math.Max(tbTX28EQ26.Minimum, Math.Min(tbTX28EQ26.Maximum, value[26]));
                tbTX28EQ27.Value = Math.Max(tbTX28EQ27.Minimum, Math.Min(tbTX28EQ27.Maximum, value[27]));
                tbTX28EQ28.Value = Math.Max(tbTX28EQ28.Minimum, Math.Min(tbTX28EQ28.Maximum, value[28]));


            } // set

        } // TXEQ28

        // ===============================================================
        // ke9ns: copy all sliders into an array to pass to dsp.cs

        public int[] PEQ
        {
            get
            {
                //  Debug.WriteLine("PEQ===  9band PEQ GET " + tbTXEQ9Preamp.Value);

                int[] eq = new int[28];

                eq[0] = (int)tbTXEQ9Preamp.Value;

                eq[1] = (int)tbPEQ1.Value;  // gain
                eq[2] = (int)tbPEQ2.Value;
                eq[3] = (int)tbPEQ3.Value;
                eq[4] = (int)tbPEQ4.Value;
                eq[5] = (int)tbPEQ5.Value;
                eq[6] = (int)tbPEQ6.Value;
                eq[7] = (int)tbPEQ7.Value;
                eq[8] = (int)tbPEQ8.Value;
                eq[9] = (int)tbPEQ9.Value;

                eq[10] = (int)(udPEQoctave1.Value * 10); // octave of each band *10 since its an INT (/10 when using)
                eq[11] = (int)(udPEQoctave2.Value * 10);
                eq[12] = (int)(udPEQoctave3.Value * 10);
                eq[13] = (int)(udPEQoctave4.Value * 10);
                eq[14] = (int)(udPEQoctave5.Value * 10);
                eq[15] = (int)(udPEQoctave6.Value * 10);
                eq[16] = (int)(udPEQoctave7.Value * 10);
                eq[17] = (int)(udPEQoctave8.Value * 10);
                eq[18] = (int)(udPEQoctave9.Value * 10);


                eq[19] = (int)udPEQfreq1.Value; // freq of each band
                eq[20] = (int)udPEQfreq2.Value;
                eq[21] = (int)udPEQfreq3.Value;
                eq[22] = (int)udPEQfreq4.Value;
                eq[23] = (int)udPEQfreq5.Value;
                eq[24] = (int)udPEQfreq6.Value;
                eq[25] = (int)udPEQfreq7.Value;
                eq[26] = (int)udPEQfreq8.Value;
                eq[27] = (int)udPEQfreq9.Value;


                ;

                return eq;
            }
            set
            {
                // Debug.WriteLine("9band PEQ SET1 ");

                tbTXEQ9Preamp.Value = Math.Max(tbTXEQ9Preamp.Minimum, Math.Min(tbTXEQ9Preamp.Maximum, value[0]));

                tbPEQ1.Value = Math.Max(tbPEQ1.Minimum, Math.Min(tbPEQ1.Maximum, value[1])); // loads a min for the field or VALUE
                tbPEQ2.Value = Math.Max(tbPEQ2.Minimum, Math.Min(tbPEQ2.Maximum, value[2]));
                tbPEQ3.Value = Math.Max(tbPEQ3.Minimum, Math.Min(tbPEQ3.Maximum, value[3]));
                tbPEQ4.Value = Math.Max(tbPEQ4.Minimum, Math.Min(tbPEQ4.Maximum, value[4]));
                tbPEQ5.Value = Math.Max(tbPEQ5.Minimum, Math.Min(tbPEQ5.Maximum, value[5]));
                tbPEQ6.Value = Math.Max(tbPEQ6.Minimum, Math.Min(tbPEQ6.Maximum, value[6]));
                tbPEQ7.Value = Math.Max(tbPEQ7.Minimum, Math.Min(tbPEQ7.Maximum, value[7]));
                tbPEQ8.Value = Math.Max(tbPEQ8.Minimum, Math.Min(tbPEQ8.Maximum, value[8]));
                tbPEQ9.Value = Math.Max(tbPEQ9.Minimum, Math.Min(tbPEQ9.Maximum, value[9]));

                //----------------------------------------------------------------------------



                if ((double)udPEQoctave1.Value < .1 || (double)udPEQoctave1.Value > 4) udPEQoctave1.Value = 1;
                if ((double)udPEQoctave2.Value < .1 || (double)udPEQoctave2.Value > 4) udPEQoctave2.Value = 1;
                if ((double)udPEQoctave3.Value < .1 || (double)udPEQoctave3.Value > 4) udPEQoctave3.Value = 1;
                if ((double)udPEQoctave4.Value < .1 || (double)udPEQoctave4.Value > 4) udPEQoctave4.Value = 1;
                if ((double)udPEQoctave5.Value < .1 || (double)udPEQoctave5.Value > 4) udPEQoctave5.Value = 1;
                if ((double)udPEQoctave6.Value < .1 || (double)udPEQoctave6.Value > 4) udPEQoctave6.Value = 1;
                if ((double)udPEQoctave7.Value < .1 || (double)udPEQoctave7.Value > 4) udPEQoctave7.Value = 1;
                if ((double)udPEQoctave8.Value < .1 || (double)udPEQoctave8.Value > 4) udPEQoctave8.Value = 1;
                if ((double)udPEQoctave9.Value < .1 || (double)udPEQoctave9.Value > 4) udPEQoctave9.Value = 1;



                if (value[10] < 1) value[10] = 1;
                if (value[11] < 1) value[11] = 1;
                if (value[12] < 1) value[12] = 1;
                if (value[13] < 1) value[13] = 1;
                if (value[14] < 1) value[14] = 1;
                if (value[15] < 1) value[15] = 1;
                if (value[16] < 1) value[16] = 1;
                if (value[17] < 1) value[17] = 1;
                if (value[18] < 1) value[18] = 1;


                udPEQoctave1.Value = Math.Round((decimal)value[10] / 10, 1);
                udPEQoctave2.Value = Math.Round((decimal)value[11] / 10, 1);
                udPEQoctave3.Value = Math.Round((decimal)value[12] / 10, 1);
                udPEQoctave4.Value = Math.Round((decimal)value[13] / 10, 1);
                udPEQoctave5.Value = Math.Round((decimal)value[14] / 10, 1);
                udPEQoctave6.Value = Math.Round((decimal)value[15] / 10, 1);
                udPEQoctave7.Value = Math.Round((decimal)value[16] / 10, 1);
                udPEQoctave8.Value = Math.Round((decimal)value[17] / 10, 1);
                udPEQoctave9.Value = Math.Round((decimal)value[18] / 10, 1);



                if ((double)udPEQoctave1.Value < .1 || (double)udPEQoctave1.Value > 4) udPEQoctave1.Value = 1;
                if ((double)udPEQoctave2.Value < .1 || (double)udPEQoctave2.Value > 4) udPEQoctave2.Value = 1;
                if ((double)udPEQoctave3.Value < .1 || (double)udPEQoctave3.Value > 4) udPEQoctave3.Value = 1;
                if ((double)udPEQoctave4.Value < .1 || (double)udPEQoctave4.Value > 4) udPEQoctave4.Value = 1;
                if ((double)udPEQoctave5.Value < .1 || (double)udPEQoctave5.Value > 4) udPEQoctave5.Value = 1;
                if ((double)udPEQoctave6.Value < .1 || (double)udPEQoctave6.Value > 4) udPEQoctave6.Value = 1;
                if ((double)udPEQoctave7.Value < .1 || (double)udPEQoctave7.Value > 4) udPEQoctave7.Value = 1;
                if ((double)udPEQoctave8.Value < .1 || (double)udPEQoctave8.Value > 4) udPEQoctave8.Value = 1;
                if ((double)udPEQoctave9.Value < .1 || (double)udPEQoctave9.Value > 4) udPEQoctave9.Value = 1;

                //------------------------------------------------------------------------------
                if (udPEQfreq1.Value < 10) udPEQfreq1.Value = 30;
                if ((udPEQfreq2.Value <= udPEQfreq1.Value)) udPEQfreq2.Value = 63;
                if ((udPEQfreq3.Value <= udPEQfreq2.Value)) udPEQfreq3.Value = 125;
                if ((udPEQfreq4.Value <= udPEQfreq3.Value)) udPEQfreq4.Value = 250;
                if ((udPEQfreq5.Value <= udPEQfreq4.Value)) udPEQfreq5.Value = 500;
                if ((udPEQfreq6.Value <= udPEQfreq5.Value)) udPEQfreq6.Value = 1000;
                if ((udPEQfreq7.Value <= udPEQfreq6.Value)) udPEQfreq7.Value = 2000;
                if ((udPEQfreq8.Value <= udPEQfreq7.Value)) udPEQfreq8.Value = 4000;
                if ((udPEQfreq9.Value <= udPEQfreq8.Value)) udPEQfreq9.Value = 8000;

                udPEQfreq1.Value = Math.Max(udPEQfreq1.Minimum, Math.Min(udPEQfreq1.Maximum, value[19]));
                udPEQfreq2.Value = Math.Max(udPEQfreq2.Minimum, Math.Min(udPEQfreq2.Maximum, value[20]));
                udPEQfreq3.Value = Math.Max(udPEQfreq3.Minimum, Math.Min(udPEQfreq3.Maximum, value[21]));
                udPEQfreq4.Value = Math.Max(udPEQfreq4.Minimum, Math.Min(udPEQfreq4.Maximum, value[22]));
                udPEQfreq5.Value = Math.Max(udPEQfreq5.Minimum, Math.Min(udPEQfreq5.Maximum, value[23]));
                udPEQfreq6.Value = Math.Max(udPEQfreq6.Minimum, Math.Min(udPEQfreq6.Maximum, value[24]));
                udPEQfreq7.Value = Math.Max(udPEQfreq7.Minimum, Math.Min(udPEQfreq7.Maximum, value[25]));
                udPEQfreq8.Value = Math.Max(udPEQfreq8.Minimum, Math.Min(udPEQfreq8.Maximum, value[26]));
                udPEQfreq9.Value = Math.Max(udPEQfreq9.Minimum, Math.Min(udPEQfreq9.Maximum, value[27]));

                if (udPEQfreq1.Value < 10) udPEQfreq1.Value = 30;
                if ((udPEQfreq2.Value <= udPEQfreq1.Value)) udPEQfreq2.Value = 63;
                if ((udPEQfreq3.Value <= udPEQfreq2.Value)) udPEQfreq3.Value = 125;
                if ((udPEQfreq4.Value <= udPEQfreq3.Value)) udPEQfreq4.Value = 250;
                if ((udPEQfreq5.Value <= udPEQfreq4.Value)) udPEQfreq5.Value = 500;
                if ((udPEQfreq6.Value <= udPEQfreq5.Value)) udPEQfreq6.Value = 1000;
                if ((udPEQfreq7.Value <= udPEQfreq6.Value)) udPEQfreq7.Value = 2000;
                if ((udPEQfreq8.Value <= udPEQfreq7.Value)) udPEQfreq8.Value = 4000;
                if ((udPEQfreq9.Value <= udPEQfreq8.Value)) udPEQfreq9.Value = 8000;


            }

        } // PEQ



        public bool RXEQEnabled
        {
            get
            {
                if (chkRXEQEnabled != null) return chkRXEQEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkRXEQEnabled != null) chkRXEQEnabled.Checked = value;
            }
        }

        public bool TXEQEnabled
        {
            get
            {
                if (chkTXEQEnabled != null) return chkTXEQEnabled.Checked;
                else return false;
            }
            set
            {
                if (chkTXEQEnabled != null) chkTXEQEnabled.Checked = value;
            }
        }

        #endregion

        #region Event Handlers

        private void EQForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "EQForm");
        }

        private void tbRXEQ_Scroll(object sender, System.EventArgs e)
        {
            int[] rxeq = RXEQ;

            if (rad3Band.Checked)
            {
                console.dsp.GetDSPRX(0, 0).RXEQ3 = rxeq;
                console.dsp.GetDSPRX(0, 1).RXEQ3 = rxeq;
                console.dsp.GetDSPRX(1, 0).RXEQ3 = rxeq;
            }
            else
            {
                console.dsp.GetDSPRX(0, 0).RXEQ10 = rxeq;
                console.dsp.GetDSPRX(0, 1).RXEQ10 = rxeq;
                console.dsp.GetDSPRX(1, 0).RXEQ10 = rxeq;
            }
            picRXEQ.Invalidate();
        }




        //===============================================================================
        // 10band eq
        public void tbTXEQ_Scroll(object sender, System.EventArgs e) //  ALL EQ slider changes come here
        {
            int[] txeq = TXEQ; // ke9ns: get all the slider values here and put in array of ints

            this.toolTip1.SetToolTip(this.tbTXEQ1, tbTXEQ1.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ2, tbTXEQ2.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ3, tbTXEQ3.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ4, tbTXEQ4.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ5, tbTXEQ5.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ6, tbTXEQ6.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ7, tbTXEQ7.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ8, tbTXEQ8.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ9, tbTXEQ9.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTXEQ10, tbTXEQ10.Value.ToString() + "dB");


            this.toolTip1.SetToolTip(this.tbTXEQPreamp, tbTXEQPreamp.Value.ToString() + "dB");


            if (rad3Band.Checked)
            {
                console.dsp.GetDSPTX(0).TXEQ3 = txeq;
            }
            else
            {

                console.dsp.GetDSPTX(0).TXEQ10 = txeq; // ke9ns refers to update SetGrphTXEQ10  txeq[0] is the preamp slider;

            } // 10 band or 28 band 

            picTXEQ.Invalidate();
        } // touched a TX EQ scroll bar


        //=====================================================================================
        private void picRXEQ_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int[] rxeq = RXEQ;

            if (!chkRXEQEnabled.Checked)
            {
                for (int i = 0; i < rxeq.Length; i++)
                {
                    rxeq[i] = 0;
                }
            }

            Point[] points = new Point[rxeq.Length - 1];
            for (int i = 1; i < rxeq.Length; i++)
            {
                points[i - 1].X = (int)((i - 1) * picRXEQ.Width / (float)(rxeq.Length - 2));
                points[i - 1].Y = picRXEQ.Height / 2 - (int)(rxeq[i] * (picRXEQ.Height - 6) / 2 / 15.0f +
                    tbRXEQPreamp.Value * 3 / 15.0f);
            }


            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // ke9ns mod
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            e.Graphics.FillRectangle(new SolidBrush(SystemColors.ControlDark), 0, 0, picRXEQ.Width, picRXEQ.Height);

            e.Graphics.DrawLine(new Pen(Color.Red, 3), 0, picRXEQ.Size.Height / 2, picRXEQ.Size.Width, picRXEQ.Size.Height / 2);// ke9ns mod
            e.Graphics.DrawLines(new Pen(Color.Blue, 3), points);// ke9ns mod
            e.Graphics.DrawBeziers(new Pen(Color.GreenYellow, 3), points);// ke9ns mod

        } // picRXEQ_Paint




        //======================================================================================
        private void picTXEQ_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            Point[] points; // used to plot graph

            double LowF = 20; // ke9ns: PEQ 10 hz low end
            double HighF = 20000; // ke9ns PEQ20khz high end

            int midpointY = (picTXEQ.Height) / 2; // mid Y of display area


            if (radPEQ.Checked == true) // PEQ below
            {

                int[] peq = PEQ; //ke9ns:  0=preamp, 1-9=gain,10-18=octave*10,19-27=freq hz


                if (!chkTXEQEnabled.Checked)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        peq[i] = 0; // clear the GAIN sliers
                    }
                }


                int[] x = new int[28]; // storage for low,mid,high for 9 parametric sliders
                int[] y = new int[28];
                int ii = 0;
                int q = 0;

                for (ii = 0, q = 1; q < 10; q++) // 9 parametric eq settings
                {
                    int PEQMfreq = peq[q + 18];
                    double PEQoctave = ((double)peq[q + 9] / 10.0);
                    int PEQgain = peq[q];

                    int PEQLowf = (int)((double)PEQMfreq / Math.Pow(1.414, PEQoctave));
                    int PEQHighf = (int)((double)PEQMfreq * Math.Pow(1.414, PEQoctave));

                    // ke9ns add: calculate the log10 parametric position on the X axis band width (hz)
                    int xposL = (int)((double)picTXEQ.Width * (Math.Log10((double)PEQLowf) - Math.Log10(LowF)) / (Math.Log10(HighF) - Math.Log10(LowF)));   //X = X0 + (X1 - X0)(log(V) - log(V0)) / (log(V1) - log(V0))
                    int xposM = (int)((double)picTXEQ.Width * (Math.Log10((double)PEQMfreq) - Math.Log10(LowF)) / (Math.Log10(HighF) - Math.Log10(LowF)));
                    int xposH = (int)((double)picTXEQ.Width * (Math.Log10((double)PEQHighf) - Math.Log10(LowF)) / (Math.Log10(HighF) - Math.Log10(LowF)));


                    double pregain = (double)tbTXEQ9Preamp.Value * 3 / 15.0;  // gain value
                    double bandgain = (double)PEQgain / 15.0; // gain value

                    // ke9ns add: calculate the Y axis gain (dB) based on a slider that is +/- 15dB of gain + a preamp with the same gain range
                    int yposL = midpointY - (int)(bandgain * midpointY + pregain); // 3db drop
                    int yposM = midpointY - (int)(bandgain * midpointY + pregain);
                    int yposH = midpointY - (int)(bandgain * midpointY + pregain);

                    //   Debug.WriteLine("yposL "+ (bandgain * midpointY) + " , " + pregain +  " , " + yposL);
                    // ke9ns add: store values to allow combining these bands into 1 big band composite
                    x[ii] = xposL;
                    y[ii++] = yposL;

                    x[ii] = xposM;
                    y[ii++] = yposM;

                    x[ii] = xposH;
                    y[ii++] = yposH;


                    //   Debug.WriteLine("PEQ==== gain="+ PEQgain + " ,octave=" + PEQoctave + " ,Low=" + PEQLowf + " ,Mid=" + PEQMfreq + " ,High=" + PEQHighf);
                    //   Debug.WriteLine("PEQ==== OUTPUT= " + bandgain + " > " + xposL + "/" + yposL + "  ,  " + xposM + "/" + yposM + "  ,  " + xposH + "/" + yposH);
                    //   Debug.WriteLine(" ");


                } // for loop  9 parametric eq settings


                points = new Point[picTXEQ.Width]; // ke9ns: a bitmap of the display area


                for (int i = 0; i < picTXEQ.Width; i++) // this is the X of the display area 0 to width
                {
                    points[i].X = i; // create point for every x pixel across the display
                    points[i].Y = midpointY;


                    for (int a = 0; a < 27; a = a + 3) // scan entire x[] and y[] array for each pixel of the display area
                    {
                        if (i >= x[a] && i <= x[a + 1])  // is i between x[a] and x[a+2]  (where x[a]=low, x[a+1]=mid, x[a+2]=high)
                        {
                            points[i].Y = points[i].Y + (y[a] - midpointY);

                        }
                        else if (i >= x[a + 1] && i <= x[a + 2])
                        {
                            points[i].Y = points[i].Y + (y[a + 1] - midpointY);
                        }

                        if (points[i].Y > picTXEQ.Height) points[i].Y = picTXEQ.Height;
                        if (points[i].Y < 0) points[i].Y = 0;

                    } // loop


                } // for loop 


            }
            else // 10band or 28band or 3band  below
            {

                int[] txeq;

                if (rad28Band.Checked == true) txeq = TXEQ28;
                else txeq = TXEQ;

                if (!chkTXEQEnabled.Checked)
                {
                    for (int i = 0; i < txeq.Length; i++)
                    {
                        txeq[i] = 0;
                    }
                }

                points = new Point[txeq.Length - 1];

                for (int i = 1; i < txeq.Length; i++)
                {

                    points[i - 1].X = (int)((i - 1) * picTXEQ.Width / (float)(txeq.Length - 2)); // automatically divided into octaves (log)

                    if (rad28Band.Checked == true)
                        points[i - 1].Y = picTXEQ.Height / 2 - (int)(txeq[i] * (picTXEQ.Height - 6) / 2 / 15.0f + tbTXEQ28Preamp.Value * 3 / 15.0f); // 
                    else
                        points[i - 1].Y = picTXEQ.Height / 2 - (int)(txeq[i] * (picTXEQ.Height - 6) / 2 / 15.0f + tbTXEQPreamp.Value * 3 / 15.0f); // 

                    //  Debug.WriteLine("4444444444 band " + txeq[i]);
                }

            } // standard GEQ 10 band


            //-------------------------------------------------------------------------------------

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality; //	e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            e.Graphics.FillRectangle(new SolidBrush(SystemColors.ControlDark), 0, 0, picTXEQ.Width, picTXEQ.Height);

            e.Graphics.DrawLine(new Pen(Color.Red, 3), 0, picTXEQ.Size.Height / 2, picTXEQ.Size.Width, picTXEQ.Size.Height / 2);

            if (radPEQ.Checked == true)
            {
                int x1 = 0;
                int x2 = 0;
                double f = 0;


                f = LowF; //20

                for (int g = 0; g < 11; g++) // draw tick marks for each octave across the display area
                {
                    if (g > 0)
                    {
                        x1 = (int)((double)picTXEQ.Width * (Math.Log10((double)f) - Math.Log10(LowF)) / (Math.Log10(HighF) - Math.Log10(LowF)));   //X = X0 + (X1 - X0)(log(V) - log(V0)) / (log(V1) - log(V0))

                        e.Graphics.DrawLine(new Pen(Color.Red, 1), x1, picTXEQ.Height - 10, x1, picTXEQ.Height);
                        e.Graphics.DrawString(f.ToString() + "hz", PictureBox.DefaultFont, Brushes.Black, x1, picTXEQ.Height - 15);
                    }

                    f = f + f;


                } // for loop


            } //  if (radPEQ.Checked == true)

            //  if (radPEQ.Checked == false)  e.Graphics.DrawLines(new Pen(Color.Blue,3), points); // draw the line

            if (rad3Band.Checked == true)
                e.Graphics.DrawLines(new Pen(Color.Blue, 3), points);
            else
                e.Graphics.DrawBeziers(new Pen(Color.GreenYellow, 2), points); // needs even number of points or ODD value in array



        } //  picTXEQ_Paint


        //===========================================================================================
        private void chkRXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPRX(0, 0).RXEQOn = chkRXEQEnabled.Checked;
            picRXEQ.Invalidate();
            console.RXEQ = chkRXEQEnabled.Checked;
        }

        //==================================================
        // ke9ns TX enabled
        private void chkTXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).TXEQOn = chkTXEQEnabled.Checked;
            picTXEQ.Invalidate();
            console.TXEQ = chkTXEQEnabled.Checked;
        }

        private void btnRXEQReset_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to reset the Receive Equalizer\n" +
                "to flat (zero)?",
                "Are you sure?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            //	foreach(Control c in grpRXEQ.Controls)
            //	{
            //		if(c.GetType() == typeof(TrackBarTS))
            //				((TrackBarTS)c).Value = 0;
            //		}

            tbRXEQ1.Value = 0;
            tbRXEQ2.Value = 0;
            tbRXEQ3.Value = 0;
            tbRXEQ4.Value = 0;
            tbRXEQ5.Value = 0;
            tbRXEQ6.Value = 0;
            tbRXEQ7.Value = 0;
            tbRXEQ8.Value = 0;
            tbRXEQ9.Value = 0;
            tbRXEQ10.Value = 0;


            tbRXEQ_Scroll(this, EventArgs.Empty);
        }

        private void btnTXEQReset_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to reset the Transmit Equalizer\n" +
                "to flat (zero)?",
                "Are you sure?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            //	foreach(Control c in grpTXEQ.Controls)
            //	{
            //		if(c.GetType() == typeof(TrackBarTS))
            //			((TrackBarTS)c).Value = 0;
            //		}


            tbTXEQ1.Value = 0;
            tbTXEQ2.Value = 0;
            tbTXEQ3.Value = 0;
            tbTXEQ4.Value = 0;
            tbTXEQ5.Value = 0;
            tbTXEQ6.Value = 0;
            tbTXEQ7.Value = 0;
            tbTXEQ8.Value = 0;
            tbTXEQ9.Value = 0;
            tbTXEQ10.Value = 0;



            // ke9ns add


            tbTXEQ_Scroll(this, EventArgs.Empty);

        } //  btnTXEQReset_Click

        private void chkTXEQ160Notch_CheckedChanged(object sender, System.EventArgs e)
        {
            console.dsp.GetDSPTX(0).Notch160 = chkTXEQ160Notch.Checked;
        }

        // ke9ns: used to be checkchanged, but now just click
        private void rad3Band_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rad3Band.Checked)
            {

                chkBothEQ.Checked = false; // ke9ns .173

                Debug.WriteLine("---3Band:");

                this.Size = new Size(888, 737); // ke9ns: resize the window to fit the hidden 28eq

                // this.Size = new Size(590,737);
                picTXEQ.Size = new Size(384, 111);

                grpTXEQ28.Visible = false;
                grpTXEQ28.Invalidate();

                grpPEQ.Visible = false;
                grpPEQ.Invalidate();
                //............................................



                lblRXEQ1.Visible = true;
                lblRXEQ2.Visible = false;
                lblRXEQ3.Visible = false;
                lblRXEQ4.Visible = false;
                lblRXEQ5.Visible = true;
                lblRXEQ6.Visible = false;
                lblRXEQ7.Visible = false;
                lblRXEQ8.Visible = false;
                lblRXEQ9.Visible = true;
                lblRXEQ10.Visible = false;

                tbRXEQ1.Visible = true;
                tbRXEQ2.Visible = false;
                tbRXEQ3.Visible = false;
                tbRXEQ4.Visible = false;
                tbRXEQ5.Visible = true;
                tbRXEQ6.Visible = false;
                tbRXEQ7.Visible = false;
                tbRXEQ8.Visible = false;
                tbRXEQ9.Visible = true;
                tbRXEQ10.Visible = false;

                lblRXEQ1.Text = "Low";
                lblRXEQ5.Text = "Mid";
                lblRXEQ9.Text = "High";

                toolTip1.SetToolTip(lblRXEQ1, "0-400Hz");
                toolTip1.SetToolTip(tbRXEQ1, "0-400Hz");
                toolTip1.SetToolTip(lblRXEQ5, "400-1500Hz");
                toolTip1.SetToolTip(tbRXEQ5, "400-1500Hz");
                toolTip1.SetToolTip(lblRXEQ9, "1500-6000Hz");
                toolTip1.SetToolTip(tbRXEQ9, "1500-6000Hz");


                lblTXEQ1.Visible = true;
                lblTXEQ2.Visible = false;
                lblTXEQ3.Visible = false;
                lblTXEQ4.Visible = false;
                lblTXEQ5.Visible = true;
                lblTXEQ6.Visible = false;
                lblTXEQ7.Visible = false;
                lblTXEQ8.Visible = false;
                lblTXEQ9.Visible = true;
                lblTXEQ10.Visible = false;


                tbTXEQ1.Visible = true;
                tbTXEQ2.Visible = false;
                tbTXEQ3.Visible = false;
                tbTXEQ4.Visible = false;
                tbTXEQ5.Visible = true;
                tbTXEQ6.Visible = false;
                tbTXEQ7.Visible = false;
                tbTXEQ8.Visible = false;
                tbTXEQ9.Visible = true;
                tbTXEQ10.Visible = false;

                lblTXEQ1.Text = "Low";
                lblTXEQ5.Text = "Mid";
                lblTXEQ9.Text = "High";

                toolTip1.SetToolTip(lblTXEQ1, "0-400Hz");
                toolTip1.SetToolTip(tbTXEQ1, "0-400Hz");
                toolTip1.SetToolTip(lblTXEQ5, "400-1500Hz");
                toolTip1.SetToolTip(tbTXEQ5, "400-1500Hz");
                toolTip1.SetToolTip(lblTXEQ9, "1500-6000Hz");
                toolTip1.SetToolTip(tbTXEQ9, "1500-6000Hz");

                RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ3;
                TXEQ = console.dsp.GetDSPTX(0).TXEQ3;


                tbRXEQ_Scroll(this, EventArgs.Empty);
                tbTXEQ_Scroll(this, EventArgs.Empty);

                picRXEQ.Invalidate();
                picTXEQ.Invalidate();


                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 3;
                console.dsp.GetDSPTX(0).TXEQNumBands = 3;

            }
        } // rad3Band_CheckedChanged

        //========================================================================
        // ke9ns 10 band Enable
        private void rad10Band_CheckedChanged(object sender, System.EventArgs e)
        {

            if (rad10Band.Checked == true)
            {

                chkBothEQ.Checked = false; // ke9ns: .173

                //   Debug.WriteLine("---10Band:");

                this.Size = new Size(888, 737); // ke9ns: resize the window to fit the hidden 28eq

                // this.Size = new Size(590,737);
                picTXEQ.Size = new Size(384, 111);

                grpTXEQ28.Visible = false;
                grpTXEQ28.Invalidate();

                grpPEQ.Visible = false;
                grpPEQ.Invalidate();
                //..................................................

                lblRXEQ1.Visible = true;
                lblRXEQ2.Visible = true;
                lblRXEQ3.Visible = true;
                lblRXEQ4.Visible = true;
                lblRXEQ5.Visible = true;
                lblRXEQ6.Visible = true;
                lblRXEQ7.Visible = true;
                lblRXEQ8.Visible = true;
                lblRXEQ9.Visible = true;
                lblRXEQ10.Visible = true;

                tbRXEQ1.Visible = true;
                tbRXEQ2.Visible = true;
                tbRXEQ3.Visible = true;
                tbRXEQ4.Visible = true;
                tbRXEQ5.Visible = true;
                tbRXEQ6.Visible = true;
                tbRXEQ7.Visible = true;
                tbRXEQ8.Visible = true;
                tbRXEQ9.Visible = true;
                tbRXEQ10.Visible = true;

                lblRXEQ1.Text = "32";
                lblRXEQ5.Text = "500";
                lblRXEQ9.Text = "8K";

                toolTip1.SetToolTip(lblRXEQ1, "");
                toolTip1.SetToolTip(tbRXEQ1, "");
                toolTip1.SetToolTip(lblRXEQ5, "");
                toolTip1.SetToolTip(tbRXEQ5, "");
                toolTip1.SetToolTip(lblRXEQ9, "");
                toolTip1.SetToolTip(tbRXEQ9, "");

                lblTXEQ1.Visible = true;
                lblTXEQ2.Visible = true;
                lblTXEQ3.Visible = true;
                lblTXEQ4.Visible = true;
                lblTXEQ5.Visible = true;
                lblTXEQ6.Visible = true;
                lblTXEQ7.Visible = true;
                lblTXEQ8.Visible = true;
                lblTXEQ9.Visible = true;
                lblTXEQ10.Visible = true;

                tbTXEQ1.Visible = true;
                tbTXEQ2.Visible = true;
                tbTXEQ3.Visible = true;
                tbTXEQ4.Visible = true;
                tbTXEQ5.Visible = true;
                tbTXEQ6.Visible = true;
                tbTXEQ7.Visible = true;
                tbTXEQ8.Visible = true;
                tbTXEQ9.Visible = true;
                tbTXEQ10.Visible = true;

                lblTXEQ1.Text = "32";
                lblTXEQ5.Text = "500";
                lblTXEQ9.Text = "8K";

                toolTip1.SetToolTip(lblTXEQ1, "");
                toolTip1.SetToolTip(tbTXEQ1, "");
                toolTip1.SetToolTip(lblTXEQ5, "");
                toolTip1.SetToolTip(tbTXEQ5, "");
                toolTip1.SetToolTip(lblTXEQ9, "");
                toolTip1.SetToolTip(tbTXEQ9, "");

                RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ10;
                TXEQ = console.dsp.GetDSPTX(0).TXEQ10;

                tbRXEQ_Scroll(this, EventArgs.Empty);
                tbTXEQ_Scroll(this, EventArgs.Empty);

                picRXEQ.Invalidate();
                picTXEQ.Invalidate();


                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 10;

            } // if(rad10Band.Checked)

        } // rad10Band_CheckedChanged

        #endregion

        //================================================================
        // ke9ns add
        private void rad28Band_CheckedChanged(object sender, EventArgs e)
        {
            if (rad28Band.Checked)
            {

                //  Debug.WriteLine("---28Band:");

                this.Size = new Size(888, 737); // ke9ns: resize the window to fit the hidden 28eq
                picTXEQ.Size = new Size(712, 111);//ke9ns .162

                grpPEQ.Visible = false;
                grpPEQ.Invalidate();

                grpTXEQ28.Visible = true;
                grpTXEQ28.Invalidate();

                lblRXEQ2.Visible = true;
                lblRXEQ3.Visible = true;
                lblRXEQ4.Visible = true;
                lblRXEQ6.Visible = true;
                lblRXEQ7.Visible = true;
                lblRXEQ8.Visible = true;
                lblRXEQ10.Visible = true;

                tbRXEQ2.Visible = true;
                tbRXEQ3.Visible = true;
                tbRXEQ4.Visible = true;
                tbRXEQ6.Visible = true;
                tbRXEQ7.Visible = true;
                tbRXEQ8.Visible = true;
                tbRXEQ10.Visible = true;

                lblRXEQ1.Text = "32";
                lblRXEQ5.Text = "500";
                lblRXEQ9.Text = "8K";

                toolTip1.SetToolTip(lblRXEQ1, "");
                toolTip1.SetToolTip(tbRXEQ1, "");
                toolTip1.SetToolTip(lblRXEQ5, "");
                toolTip1.SetToolTip(tbRXEQ5, "");
                toolTip1.SetToolTip(lblRXEQ9, "");
                toolTip1.SetToolTip(tbRXEQ9, "");


                RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ10;
                TXEQ28 = console.dsp.GetDSPTX(0).TXEQ28;


                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 28;

                tbRXEQ_Scroll(this, EventArgs.Empty);

                tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.


            } // if(rad28Band.Checked)


        } // rad28Band_CheckedChanged

        //=====================================================================================
        // ke9ns add:
        private void radPEQ_CheckedChanged(object sender, EventArgs e)
        {
            if (radPEQ.Checked)
            {

                this.Size = new Size(888, 737); // ke9ns: resize the window to fit the hidden 28eq
                picTXEQ.Size = new Size(712, 111);//ke9ns .162

                grpTXEQ28.Visible = false;
                grpTXEQ28.Invalidate();



                grpPEQ.Visible = true;
                grpPEQ.Invalidate();


                lblRXEQ2.Visible = true;
                lblRXEQ3.Visible = true;
                lblRXEQ4.Visible = true;
                lblRXEQ6.Visible = true;
                lblRXEQ7.Visible = true;
                lblRXEQ8.Visible = true;
                lblRXEQ10.Visible = true;

                tbRXEQ2.Visible = true;
                tbRXEQ3.Visible = true;
                tbRXEQ4.Visible = true;
                tbRXEQ6.Visible = true;
                tbRXEQ7.Visible = true;
                tbRXEQ8.Visible = true;
                tbRXEQ10.Visible = true;

                lblRXEQ1.Text = "32";
                lblRXEQ5.Text = "500";
                lblRXEQ9.Text = "8K";

                toolTip1.SetToolTip(lblRXEQ1, "");
                toolTip1.SetToolTip(tbRXEQ1, "");
                toolTip1.SetToolTip(lblRXEQ5, "");
                toolTip1.SetToolTip(tbRXEQ5, "");
                toolTip1.SetToolTip(lblRXEQ9, "");
                toolTip1.SetToolTip(tbRXEQ9, "");


                RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ10; // load up starting values of sliders      
                PEQ = console.dsp.GetDSPTX(0).PEQ;

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 9;

                tbRXEQ_Scroll(this, EventArgs.Empty);

                tbPEQ1_Scroll(this, EventArgs.Empty);


            } // radPEQ


        } //  radPEQ_CheckedChanged



        //===================================================================
        // ke9ns when you move any of the 28 scroll bars for the 28 band eq  see dsp.cs and update.c and isoband.c
        public void tbTX28EQ15_Scroll(object sender, EventArgs e)
        {
            int[] txeq28 = TXEQ28; // ke9ns get all the slider values here and put in array of ints

            this.toolTip1.SetToolTip(this.tbTXEQ28Preamp, tbTXEQ28Preamp.Value.ToString() + "dB");

            this.toolTip1.SetToolTip(this.tbTX28EQ1, tbTX28EQ1.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ2, tbTX28EQ2.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ3, tbTX28EQ3.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ4, tbTX28EQ4.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ5, tbTX28EQ5.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ6, tbTX28EQ6.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ7, tbTX28EQ7.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ8, tbTX28EQ8.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ9, tbTX28EQ9.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ10, tbTX28EQ10.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ11, tbTX28EQ11.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ12, tbTX28EQ12.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ13, tbTX28EQ13.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ14, tbTX28EQ14.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ15, tbTX28EQ15.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ16, tbTX28EQ16.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ17, tbTX28EQ17.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ18, tbTX28EQ18.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ19, tbTX28EQ19.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ20, tbTX28EQ20.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ21, tbTX28EQ21.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ22, tbTX28EQ22.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ23, tbTX28EQ23.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ24, tbTX28EQ24.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ25, tbTX28EQ25.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ26, tbTX28EQ26.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ27, tbTX28EQ27.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbTX28EQ28, tbTX28EQ28.Value.ToString() + "dB");


            console.dsp.GetDSPTX(0).TXEQ28 = txeq28; // ke9ns: refers to update SetGrphTXEQ10  txeq[0] is the preamp slider;


            picTXEQ.Invalidate();

        } // tbTX28EQ15_Scroll




        private void btnTXEQReset28_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
            "Are you sure you want to reset the Transmit Equalizer\n" +
            "to flat (zero)?",
            "Are you sure?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            tbTXEQ28Preamp.Value = 0;

            tbTX28EQ1.Value = 0;
            tbTX28EQ2.Value = 0;
            tbTX28EQ3.Value = 0;
            tbTX28EQ4.Value = 0;
            tbTX28EQ5.Value = 0;
            tbTX28EQ6.Value = 0;
            tbTX28EQ7.Value = 0;
            tbTX28EQ8.Value = 0;
            tbTX28EQ9.Value = 0;
            tbTX28EQ10.Value = 0;
            tbTX28EQ11.Value = 0;
            tbTX28EQ12.Value = 0;
            tbTX28EQ13.Value = 0;
            tbTX28EQ14.Value = 0;
            tbTX28EQ15.Value = 0;
            tbTX28EQ16.Value = 0;
            tbTX28EQ17.Value = 0;
            tbTX28EQ18.Value = 0;
            tbTX28EQ19.Value = 0;
            tbTX28EQ20.Value = 0;
            tbTX28EQ21.Value = 0;
            tbTX28EQ22.Value = 0;
            tbTX28EQ23.Value = 0;
            tbTX28EQ24.Value = 0;
            tbTX28EQ25.Value = 0;
            tbTX28EQ26.Value = 0;
            tbTX28EQ27.Value = 0;
            tbTX28EQ28.Value = 0;

            // ke9ns add


            tbTXEQ_Scroll(this, EventArgs.Empty);
        } // reset


        private void ChkAlwaysOnTop1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop1.Checked;
        }

        private void EQForm_MouseEnter(object sender, EventArgs e)
        {
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop1.Checked == true) this.Activate();
        }

        // ke9ns: add below
        private void tbTXEQPreamp_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQPreamp, tbTXEQPreamp.Value.ToString() + "dB");

        }

        private void tbRXEQPreamp_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbRXEQPreamp, tbRXEQPreamp.Value.ToString() + "dB");
        }

        private void tbTX28EQ1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ1, tbTX28EQ1.Value.ToString() + "dB");

        }

        private void tbTX28EQ2_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ2, tbTX28EQ2.Value.ToString() + "dB");
        }

        private void tbTX28EQ3_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ3, tbTX28EQ3.Value.ToString() + "dB");
        }

        private void tbTX28EQ4_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ4, tbTX28EQ4.Value.ToString() + "dB");
        }

        private void tbTX28EQ5_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ5, tbTX28EQ5.Value.ToString() + "dB");
        }

        private void tbTX28EQ6_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ6, tbTX28EQ6.Value.ToString() + "dB");
        }

        private void tbTX28EQ7_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ7, tbTX28EQ7.Value.ToString() + "dB");
        }

        private void tbTX28EQ8_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ8, tbTX28EQ8.Value.ToString() + "dB");
        }

        private void tbTX28EQ9_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ9, tbTX28EQ9.Value.ToString() + "dB");
        }

        private void tbTX28EQ10_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ10, tbTX28EQ10.Value.ToString() + "dB");
        }

        private void tbTX28EQ11_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ11, tbTX28EQ11.Value.ToString() + "dB");
        }

        private void tbTX28EQ12_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ12, tbTX28EQ12.Value.ToString() + "dB");
        }

        private void tbTX28EQ13_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ13, tbTX28EQ13.Value.ToString() + "dB");
        }

        private void tbTX28EQ14_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ14, tbTX28EQ14.Value.ToString() + "dB");
        }

        private void tbTX28EQ15_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ15, tbTX28EQ15.Value.ToString() + "dB");
        }

        private void tbTX28EQ16_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ16, tbTX28EQ16.Value.ToString() + "dB");
        }

        private void tbTX28EQ17_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ17, tbTX28EQ17.Value.ToString() + "dB");
        }

        private void tbTX28EQ18_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ18, tbTX28EQ18.Value.ToString() + "dB");
        }

        private void tbTX28EQ19_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ19, tbTX28EQ19.Value.ToString() + "dB");
        }

        private void tbTX28EQ20_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ20, tbTX28EQ20.Value.ToString() + "dB");
        }

        private void tbTX28EQ21_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ21, tbTX28EQ21.Value.ToString() + "dB");
        }

        private void tbTX28EQ22_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ22, tbTX28EQ22.Value.ToString() + "dB");
        }

        private void tbTX28EQ23_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ23, tbTX28EQ23.Value.ToString() + "dB");
        }

        private void tbTX28EQ24_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ24, tbTX28EQ24.Value.ToString() + "dB");
        }

        private void tbTX28EQ25_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ25, tbTX28EQ25.Value.ToString() + "dB");
        }

        private void tbTX28EQ26_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ26, tbTX28EQ26.Value.ToString() + "dB");
        }

        private void tbTX28EQ27_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ27, tbTX28EQ27.Value.ToString() + "dB");
        }

        private void tbTX28EQ28_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTX28EQ28, tbTX28EQ28.Value.ToString() + "dB");
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        // ke9ns add:


        private void tbPEQ1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ1, tbPEQ1.Value.ToString() + "dB");
        }

        private void tbPEQ2_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ2, tbPEQ2.Value.ToString() + "dB");
        }

        private void tbPEQ3_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ3, tbPEQ3.Value.ToString() + "dB");
        }
        private void tbPEQ3_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ3, tbPEQ3.Value.ToString() + "dB");
        }

        private void tbPEQ4_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ4, tbPEQ4.Value.ToString() + "dB");
        }

        private void tbPEQ5_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ5, tbPEQ5.Value.ToString() + "dB");
        }

        private void tbPEQ6_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ6, tbPEQ6.Value.ToString() + "dB");
        }

        private void tbPEQ7_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ7, tbPEQ7.Value.ToString() + "dB");
        }

        private void tbPEQ8_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ8, tbPEQ8.Value.ToString() + "dB");
        }


        private void tbPEQ9_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ9, tbPEQ9.Value.ToString() + "dB");
        }



        private void tbTXEQ1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ1, tbTXEQ1.Value.ToString() + "dB");
        }

        private void tbTXEQ2_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ2, tbTXEQ2.Value.ToString() + "dB");
        }

        private void tbTXEQ3_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ3, tbTXEQ3.Value.ToString() + "dB");
        }

        private void tbTXEQ4_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ4, tbTXEQ4.Value.ToString() + "dB");
        }

        private void tbTXEQ5_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ5, tbTXEQ5.Value.ToString() + "dB");
        }

        private void tbTXEQ6_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ6, tbTXEQ6.Value.ToString() + "dB");
        }

        private void tbTXEQ7_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ7, tbTXEQ7.Value.ToString() + "dB");
        }

        private void tbTXEQ8_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ8, tbTXEQ8.Value.ToString() + "dB");
        }

        private void tbTXEQ9_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ9, tbTXEQ9.Value.ToString() + "dB");
        }

        private void tbTXEQ10_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQ10, tbTXEQ10.Value.ToString() + "dB");
        }


        // ke9ns add: any change to any control for the PEQ comes here
        public void tbPEQ1_Scroll(object sender, EventArgs e)
        {

            this.toolTip1.SetToolTip(this.tbTXEQ9Preamp, tbTXEQ9Preamp.Value.ToString() + "dB");

            this.toolTip1.SetToolTip(this.tbPEQ1, tbPEQ1.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ2, tbPEQ2.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ3, tbPEQ3.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ4, tbPEQ4.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ5, tbPEQ5.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ6, tbPEQ6.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ7, tbPEQ7.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ8, tbPEQ8.Value.ToString() + "dB");
            this.toolTip1.SetToolTip(this.tbPEQ9, tbPEQ9.Value.ToString() + "dB");


            if ((double)udPEQoctave1.Value < .1 || (double)udPEQoctave1.Value > 4) udPEQoctave1.Value = 1;
            if ((double)udPEQoctave2.Value < .1 || (double)udPEQoctave2.Value > 4) udPEQoctave2.Value = 1;
            if ((double)udPEQoctave3.Value < .1 || (double)udPEQoctave3.Value > 4) udPEQoctave3.Value = 1;
            if ((double)udPEQoctave4.Value < .1 || (double)udPEQoctave4.Value > 4) udPEQoctave4.Value = 1;
            if ((double)udPEQoctave5.Value < .1 || (double)udPEQoctave5.Value > 4) udPEQoctave5.Value = 1;
            if ((double)udPEQoctave6.Value < .1 || (double)udPEQoctave6.Value > 4) udPEQoctave6.Value = 1;
            if ((double)udPEQoctave7.Value < .1 || (double)udPEQoctave7.Value > 4) udPEQoctave7.Value = 1;
            if ((double)udPEQoctave8.Value < .1 || (double)udPEQoctave8.Value > 4) udPEQoctave8.Value = 1;
            if ((double)udPEQoctave9.Value < .1 || (double)udPEQoctave9.Value > 4) udPEQoctave9.Value = 1;



            int[] peq = PEQ; // ke9ns get all the slider values here and put in array of ints

            if (radPEQ.Checked == true)
            {
                console.dsp.GetDSPTX(0).PEQ = peq; // ke9ns refers to update SetGrphTXEQ10  txeq[0] is the preamp slider;
            }

            picTXEQ.Invalidate(); // update graph

        } // scroll (touch any PEQ control comes here


        //=======================================================================================
        private void btnTXPEQReset_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show(
            "Are you sure you want to reset the Transmit Equalizer\n" +
            "to flat (zero)?",
            "Are you sure?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;


            tbTXEQ9Preamp.Value = 0;

            tbPEQ1.Value = 0;
            tbPEQ2.Value = 0;
            tbPEQ3.Value = 0;
            tbPEQ4.Value = 0;
            tbPEQ5.Value = 0;
            tbPEQ6.Value = 0;
            tbPEQ7.Value = 0;
            tbPEQ8.Value = 0;
            tbPEQ9.Value = 0;


            udPEQoctave1.Value = 1;
            udPEQoctave2.Value = 1;
            udPEQoctave3.Value = 1;
            udPEQoctave4.Value = 1;
            udPEQoctave5.Value = 1;
            udPEQoctave6.Value = 1;
            udPEQoctave7.Value = 1;
            udPEQoctave8.Value = 1;
            udPEQoctave9.Value = 1;


            udPEQfreq1.Value = 30;
            udPEQfreq2.Value = 63;
            udPEQfreq3.Value = 125;
            udPEQfreq4.Value = 250;
            udPEQfreq5.Value = 500;
            udPEQfreq6.Value = 1000;
            udPEQfreq7.Value = 2000;
            udPEQfreq8.Value = 4000;
            udPEQfreq9.Value = 8000;

            // ke9ns add


            tbPEQ1_Scroll(this, EventArgs.Empty);

        }

        private void tbTXEQPreamp_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbTXEQPreamp, tbTXEQPreamp.Value.ToString() + "dB");
        }

        private void tbPEQ1_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ1, tbPEQ1.Value.ToString() + "dB");
        }

        private void tbPEQ2_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ2, tbPEQ2.Value.ToString() + "dB");
        }

        private void tbPEQ3_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ3, tbPEQ3.Value.ToString() + "dB");
        }

        private void tbPEQ4_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ4, tbPEQ4.Value.ToString() + "dB");
        }

        private void tbPEQ5_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ5, tbPEQ5.Value.ToString() + "dB");
        }

        private void tbPEQ6_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ6, tbPEQ6.Value.ToString() + "dB");
        }

        private void tbPEQ7_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ7, tbPEQ7.Value.ToString() + "dB");
        }

        private void tbPEQ8_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ8, tbPEQ8.Value.ToString() + "dB");
        }

        private void tbPEQ9_MouseDown(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPEQ9, tbPEQ9.Value.ToString() + "dB");
        }

        public void chkBothEQ_CheckedChanged(object sender, EventArgs e)
        {
            if (rad3Band.Checked == true || rad10Band.Checked == true) chkBothEQ.Checked = false; // ke9ns: turn off BOTH if your not using either 28band or 9 band


            if (chkBothEQ.Checked == true)
            {
              
                console.dsp.GetDSPTX(0).BOTH = chkBothEQ.Checked;
            }
            else // ke9ns == false
            {
                console.dsp.GetDSPTX(0).BOTH = chkBothEQ.Checked;

                if (rad10Band.Checked == true)
                {
                    rad10Band_CheckedChanged(this, EventArgs.Empty);
                    tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.

                }
                else if (rad3Band.Checked == true)
                {
                    rad3Band_CheckedChanged(this, EventArgs.Empty);
                    tbTXEQ_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.

                }
                else if (rad28Band.Checked == true)
                {
                    rad28Band_CheckedChanged(this, EventArgs.Empty);
                    tbTX28EQ15_Scroll(this, EventArgs.Empty); // ke9ns: when anyone moves a 28band slider, it goes there.

                }
                else if (radPEQ.Checked == true)
                {
                    radPEQ_CheckedChanged(this, EventArgs.Empty);
                    tbPEQ1_Scroll(this, EventArgs.Empty); //

                }
            }
        }

        private void lbl282_Click(object sender, EventArgs e)
        {

        }
    } // EQForm
} // PoweSDR