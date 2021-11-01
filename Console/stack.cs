//=================================================================
// stack.cs
// Bandstacking form
// created by Darrin Kohn ke9ns
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
//
//=================================================================

//=================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class StackControl: System.Windows.Forms.Form
    {


        public static SpotControl SpotForm;                     // ke9ns add  communications with spot.cs 
        public ScanControl ScanForm;                            // ke9ns add freq Scanner function


        public static Console console;   // ke9ns mod  to allow console to pass back values to stack screen
        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

      
        //   public DXMemList dxmemlist;

        #region Constructor and Destructor

        public StackControl(Console c)
        {
            InitializeComponent();

            console = c;

            Common.RestoreForm(this, "StackForm", true);


            bandstackupdate();



        } // stackcontrol

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



        #endregion

        #region Event Handlers







        #endregion

        private void StackControl_Load(object sender, EventArgs e)
        {
            bandstackupdate();


            console.buttonbs.BackColor = Color.Blue; // ke9ns ad .211 active


        }


        //=======================================================================================================================
        private void StackControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (console.chkBoxBS.Checked) console.buttonbs.BackColor = Color.Green; // ke9ns ad .211 active
            else  console.buttonbs.BackColor = Color.Black; // ke9ns ad .211 active

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "StackForm");


        }

        //===================================================================================
        //===================================================================================
        //===================================================================================
        string filter, mode;
        double freq;

        string locker;

        public double[] freq1 = new double[20];
        public string[] filter1 = new string[20];
        public string[] filter2 = new string[20]; // ke9ns add   F4 would indicate a unlocked bandstack memory, but F4L would indicate its a locked bandstank memory
        public string[] mode1 = new string[20];

        // ke9ns add .209
        public double[] freq12 = new double[20];
        public string[] filter12 = new string[20];
        public string[] filter22 = new string[20]; // ke9ns add   F4 would indicate a unlocked bandstack memory, but F4L would indicate its a locked bandstank memory
        public string[] mode12 = new string[20];

        public int BSLength = 23;  // 22   31 length of a BandStack Line

        public int nnn = 0; // 0-41 based on last_band
        public int nnn2 = 0; // .209

        public string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
                                     "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
                                      "LMF","120M","90M","61M","49M","41M","31M","25M",
                                     "22M","19M","16M","14M","13M","11M",
                                     "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
                                     "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
                                     "VHF12", "VHF13" };

        //===========================================================================================================
        // ke9ns: update VFOA and B bandstacks
        public void bandstackupdate()
        {
           
            string bigmessage = null; // full textbox string (combine 1 and 2)
            string bigmessage1 = null; // each freq string
            string bigmessage2 = null; // each memory string
            string bigmessage3 = null; // each lock or unlock

            for (nnn = 0; nnn < 41; nnn++) // total number of possible Bands
            {
                if (band_list[nnn] == console.last_band) break; // this is the current band_list index 
            }


            for (int ii = 0; ii < console.band_stacks[nnn]; ii++) // VFOA
            {
                if (DB.GetBandStack(band_list[nnn], ii, out mode, out filter, out freq))
                {
                    //   Debug.WriteLine("Band " + band_list[nnn] + " index " + ii + " freq " + freq);
                    //  SetBand(mode, filter, freq);

                    mode1[ii] = mode;

                    filter1[ii] = filter;


                    if (filter.Contains("@"))
                    {
                        bigmessage3 = "Lock";
                        console.filter2[ii] = "@";
                    }
                    else
                    {
                        bigmessage3 = "----";
                        console.filter2[ii] = "";

                    }
                   //   Debug.WriteLine("BANDSTACK: " + freq);


                    freq1[ii] = freq;

                    //   if (ii >= 9) bigmessage1 = (ii + 1).ToString() + ": " + freq.ToString("N" + 6).PadLeft(11) + ":"; //.PadLeft(11)
                    //    else bigmessage1 = (ii + 1).ToString() + " : " + freq.ToString("N" + 6).PadLeft(11) + ":";

                    bigmessage1 = freq.ToString("#####.000000").PadLeft(12) + ":"; // was N6 4 less than having index numbers

                    bigmessage2 = "---:";
                    //----------------------------------------------------------------


                    if (SpotForm != null)
                    {
                        //  Debug.WriteLine("1Rows Count " + SpotForm.dataGridView2.Rows.Count);

                        for (int aa = 0; aa < SpotForm.dataGridView2.Rows.Count; aa++) // get current # of memories we have available; ii++)     // Index through entire DXspot to find what is on this panadapter (draw vert lines first)
                        {

                            if (freq == Convert.ToDouble(SpotForm.dataGridView2[1, aa].Value))
                            {
                               // Debug.WriteLine("found memoryA: " + SpotForm.dataGridView2[0, aa]);
                                bigmessage2 = "mem:";

                                break; // end the aa loop
                            }


                        } // for loop through MEMORIES

                    } // if (SpotForm != null)

                    //----------------------------------------------------------------

                    bigmessage1 = bigmessage1 + bigmessage2 + bigmessage3;

                    //    bigmessage1 = bigmessage1.PadRight(BSLength -2); // 29 was 28 char long

                    bigmessage += bigmessage1 + "\r\n"; // + 3 more was 31 now 33


                    //  Debug.WriteLine("LENGTH===== " + bigmessage.Length);


                } // if bandstack available for band
                else
                {
                    //  Debug.WriteLine("no bandstack for band "+band_list[nnn]);
                    break;
                }

            } // for


            textBox1.Text = bigmessage; // update screen
            console.textBox1.Text = bigmessage; // update screen


            //---------------------------------------------------------
            textBox1.Focus();
            textBox1.Show();


            int value;

            if (int.TryParse(console.regBox1.Text, out value))
            {

                if (value > 0)
                {
                    textBox1.SelectionStart = (value - 1) * BSLength;       // start of each bandstack line
                    textBox1.SelectionLength = BSLength;                    // length of each bandstack line

                    console.textBox1.SelectionStart = (value - 1) * BSLength;       // start of each bandstack line
                    console.textBox1.SelectionLength = BSLength;                    // length of each bandstack line


                    Debug.WriteLine("ValueA " + value + " , " + BSLength);

                }
                else
                {
                    textBox1.SelectionStart = 0;
                    textBox1.SelectionLength = BSLength;

                    console.textBox1.SelectionStart = 0;
                    console.textBox1.SelectionLength = BSLength;
                }

            }
            else
            {
                Debug.WriteLine("no value");
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = BSLength;

                console.textBox1.SelectionStart = 0;
                console.textBox1.SelectionLength = BSLength;
            }


            //===========================================================================
            //===========================================================================
            //===========================================================================
            // VFOB .209


            string bigmessage02 = null; // full textbox string (combine 1 and 2)

                string bigmessage12 = null; // each freq string
                string bigmessage22 = null; // each memory string
                string bigmessage32 = null; // each lock or unlock

                for (nnn2 = 0; nnn2 < 41; nnn2++) // total number of possible Bands
                {
                    if (band_list[nnn2] == console.last_band2) break; // this is the current band_list index   nnn2 set here
                }


                for (int ii = 0; ii < console.band_stacks[nnn2]; ii++) // VFOB
                {
                    if (DB.GetBandStack2(band_list[nnn2], ii, out mode, out filter, out freq))
                    {
                      
                        mode12[ii] = mode;

                        filter12[ii] = filter;


                        if (filter.Contains("@"))
                        {
                            bigmessage32 = "Lock";
                            console.filter22[ii] = "@";
                        }
                        else
                        {
                            bigmessage32 = "----";
                            console.filter22[ii] = "";

                        }
                        //  Debug.WriteLine("BANDSTACK: " + freq);


                        freq12[ii] = freq;

                      
                        bigmessage12 = freq.ToString("#####.000000").PadLeft(12) + ":"; // was N6 4 less than having index numbers

                        bigmessage22 = "---:";
                        //----------------------------------------------------------------


                        if (SpotForm != null)
                        {
                            //  Debug.WriteLine("1Rows Count " + SpotForm.dataGridView2.Rows.Count);

                            for (int aa = 0; aa < SpotForm.dataGridView2.Rows.Count; aa++) // get current # of memories we have available; ii++)     // Index through entire DXspot to find what is on this panadapter (draw vert lines first)
                            {

                                if (freq == Convert.ToDouble(SpotForm.dataGridView2[1, aa].Value))
                                {
                                  //  Debug.WriteLine("found memoryB: " + SpotForm.dataGridView2[0, aa]);
                                    bigmessage22 = "mem:";

                                    break; // end the aa loop
                                }


                            } // for loop through MEMORIES

                        } // if (SpotForm != null)

                        //----------------------------------------------------------------

                        bigmessage12 = bigmessage12 + bigmessage22 + bigmessage32;
                       
                        bigmessage02 += bigmessage12 + "\r\n"; // + 3 more was 31 now 33


                        //  Debug.WriteLine("LENGTH===== " + bigmessage.Length);


                    } // if bandstack available for band
                    else
                    {
                        //  Debug.WriteLine("no bandstack for band "+band_list[nnn]);
                        break;
                    }

                } // for

            textBox2.Text = bigmessage02; // update screen
            console.textBox2.Text = bigmessage02; // update screen

                //---------------------------------------------------------
                textBox1.Focus();
                textBox1.Show();


                int value2;

                if (int.TryParse(console.regBox12, out value2))
                {

                    if (value2 > 0)
                    {
                       textBox2.SelectionStart = (value2 - 1) * BSLength;       // start of each bandstack line
                       textBox2.SelectionLength = BSLength;                    // length of each bandstack line

                        console.textBox2.SelectionStart = (value2 - 1) * BSLength;       // start of each bandstack line
                        console.textBox2.SelectionLength = BSLength;                    // length of each bandstack line

                        Debug.WriteLine("ValueB " + value2 + " , " + BSLength);

                    }
                    else
                    {
                        textBox2.SelectionStart = 0;
                        textBox2.SelectionLength = BSLength;

                        console.textBox2.SelectionStart = 0;
                        console.textBox2.SelectionLength = BSLength;
                    }

                }
                else
                {
                    Debug.WriteLine("no value");

                    textBox2.SelectionStart = 0;
                     textBox2.SelectionLength = BSLength;

                     console.textBox2.SelectionStart = 0;
                    console.textBox2.SelectionLength = BSLength;
                }

        

        } // bandstackupdate


        //======================================================================== 
        public void updateindex()
        {
            switch (console.RX1Band)
            {
                case Band.B160M:
                    console.band_160m_index = xxx;

                    break;
                case Band.B80M:
                    console.band_80m_index = xxx;

                    break;
                case Band.B60M:
                    console.band_60m_index = xxx;

                    break;
                case Band.B40M:
                    console.band_40m_index = xxx;
                    break;
                case Band.B30M:
                    console.band_30m_index = xxx;
                    break;
                case Band.B20M:
                    console.band_20m_index = xxx;
                    break;
                case Band.B17M:
                    console.band_17m_index = xxx;
                    break;
                case Band.B15M:
                    console.band_15m_index = xxx;
                    break;
                case Band.B12M:
                    console.band_12m_index = xxx;
                    break;
                case Band.B10M:
                    console.band_10m_index = xxx;
                    break;
                case Band.B6M:
                    console.band_6m_index = xxx;
                    break;
                case Band.B2M:
                    console.band_2m_index = xxx;
                    break;
                case Band.WWV:
                    console.band_wwv_index = xxx;
                    break;
                case Band.GEN:
                    console.band_gen_index = xxx;
                    break;


                case Band.VHF0:
                    console.band_vhf0_index = xxx;
                    break;
                case Band.VHF1:
                    console.band_vhf1_index = xxx;
                    break;
                case Band.VHF2:
                    console.band_vhf2_index = xxx;
                    break;
                case Band.VHF3:
                    console.band_vhf3_index = xxx;
                    break;
                case Band.VHF4:
                    console.band_vhf4_index = xxx;
                    break;
                case Band.VHF5:
                    console.band_vhf5_index = xxx;
                    break;
                case Band.VHF6:
                    console.band_vhf6_index = xxx;
                    break;
                case Band.VHF7:
                    console.band_vhf7_index = xxx;
                    break;
                case Band.VHF8:
                    console.band_vhf8_index = xxx;
                    break;
                case Band.VHF9:
                    console.band_vhf9_index = xxx;
                    break;
                case Band.VHF10:
                    console.band_vhf10_index = xxx;
                    break;
                case Band.VHF11:
                    console.band_vhf11_index = xxx;
                    break;
                case Band.VHF12:
                    console.band_vhf12_index = xxx;
                    break;
                case Band.VHF13:
                    console.band_vhf13_index = xxx;
                    break;



                case Band.BLMF:                                                                     // ke9ns add down below vhf
                    console.band_LMF_index = xxx;
                    break;
                case Band.B120M:
                    console.band_120m_index = xxx;
                    break;
                case Band.B90M:
                    console.band_90m_index = xxx;
                    break;
                case Band.B61M:
                    console.band_61m_index = xxx;
                    break;
                case Band.B49M:
                    console.band_49m_index = xxx;
                    break;
                case Band.B41M:
                    console.band_41m_index = xxx;
                    break;
                case Band.B31M:
                    console.band_31m_index = xxx;
                    break;
                case Band.B25M:
                    console.band_25m_index = xxx;
                    break;
                case Band.B22M:
                    console.band_22m_index = xxx;
                    break;

                case Band.B19M:
                    console.band_19m_index = xxx;
                    break;

                case Band.B16M:
                    console.band_16m_index = xxx;
                    break;
                case Band.B14M:
                    console.band_14m_index = xxx;
                    break;

                case Band.B13M:
                    console.band_13m_index = xxx;
                    break;

                case Band.B11M:
                    console.band_11m_index = xxx;
                    break;

            } // switch rx1band

        } // updateindex


        public void updateindex2() // .209
        {
            switch (console.RX2Band)
            {
                case Band.B160M:
                    console.band_160m_index = xxx2;

                    break;
                case Band.B80M:
                    console.band_80m_index = xxx2;

                    break;
                case Band.B60M:
                    console.band_60m_index = xxx2;

                    break;
                case Band.B40M:
                    console.band_40m_index = xxx2;
                    break;
                case Band.B30M:
                    console.band_30m_index = xxx2;
                    break;
                case Band.B20M:
                    console.band_20m_index = xxx2;
                    break;
                case Band.B17M:
                    console.band_17m_index = xxx2;
                    break;
                case Band.B15M:
                    console.band_15m_index = xxx2;
                    break;
                case Band.B12M:
                    console.band_12m_index = xxx2;
                    break;
                case Band.B10M:
                    console.band_10m_index = xxx2;
                    break;
                case Band.B6M:
                    console.band_6m_index = xxx2;
                    break;
                case Band.B2M:
                    console.band_2m_index = xxx2;
                    break;
                case Band.WWV:
                    console.band_wwv_index = xxx2;
                    break;
                case Band.GEN:
                    console.band_gen_index = xxx2;
                    break;


                case Band.VHF0:
                    console.band_vhf0_index = xxx2;
                    break;
                case Band.VHF1:
                    console.band_vhf1_index = xxx2;
                    break;
                case Band.VHF2:
                    console.band_vhf2_index = xxx2;
                    break;
                case Band.VHF3:
                    console.band_vhf3_index = xxx2;
                    break;
                case Band.VHF4:
                    console.band_vhf4_index = xxx2;
                    break;
                case Band.VHF5:
                    console.band_vhf5_index = xxx2;
                    break;
                case Band.VHF6:
                    console.band_vhf6_index = xxx2;
                    break;
                case Band.VHF7:
                    console.band_vhf7_index = xxx2;
                    break;
                case Band.VHF8:
                    console.band_vhf8_index = xxx2;
                    break;
                case Band.VHF9:
                    console.band_vhf9_index = xxx2;
                    break;
                case Band.VHF10:
                    console.band_vhf10_index = xxx2;
                    break;
                case Band.VHF11:
                    console.band_vhf11_index = xxx2;
                    break;
                case Band.VHF12:
                    console.band_vhf12_index = xxx2;
                    break;
                case Band.VHF13:
                    console.band_vhf13_index = xxx2;
                    break;



                case Band.BLMF:                                                                     // ke9ns add down below vhf
                    console.band_LMF_index = xxx2;
                    break;
                case Band.B120M:
                    console.band_120m_index = xxx2;
                    break;
                case Band.B90M:
                    console.band_90m_index = xxx2;
                    break;
                case Band.B61M:
                    console.band_61m_index = xxx2;
                    break;
                case Band.B49M:
                    console.band_49m_index = xxx2;
                    break;
                case Band.B41M:
                    console.band_41m_index = xxx2;
                    break;
                case Band.B31M:
                    console.band_31m_index = xxx2;
                    break;
                case Band.B25M:
                    console.band_25m_index = xxx2;
                    break;
                case Band.B22M:
                    console.band_22m_index = xxx2;
                    break;

                case Band.B19M:
                    console.band_19m_index = xxx2;
                    break;

                case Band.B16M:
                    console.band_16m_index = xxx2;
                    break;
                case Band.B14M:
                    console.band_14m_index = xxx2;
                    break;

                case Band.B13M:
                    console.band_13m_index = xxx2;
                    break;

                case Band.B11M:
                    console.band_11m_index = xxx2;
                    break;

            } // switch rx2band

        } // updateindex2
          //====================================================================================

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        }
        public int xxx = 0;
        public int xxx2 = 0; // .209
        public int yyy = 0;
        public int yyy2 = 0; // .209

        //=========================================================================================================
        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false;


            if (e.Button == MouseButtons.Left)
            {
                Debug.WriteLine("LEFT BUTTON");
                try
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    xxx = (ii / BSLength); //find row 

                    if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                    Debug.WriteLine("xxx " + xxx + " , " + ii);


                    textBox1.SelectionStart = (xxx * BSLength);
                    textBox1.SelectionLength = BSLength;

                    Debug.WriteLine("index at start of click " + console.iii);

                    if (console.filter2[console.iii] == "") // check if current index locked
                    {
                        yyy = 1;
                        console.SaveBandA(); // put away last freq you were on before moving
                        Debug.WriteLine("OPEN SO SAVE");
                    }
                    else
                    {
                        Debug.WriteLine("LOCKED SO DONT SAVE " + console.iii + " says " + console.filter2[console.iii]);
                    }

                    console.iii = xxx; // update new position in bandstack for checking if its locked

                    Debug.WriteLine("index after click " + console.iii);

                    yyy = 0;

                    updateindex();

                    console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                    console.UpdateWaterfallLevelValues();
                }
                catch
                {
                    Debug.WriteLine("Failed to determine index or cannot save bandstack because its locked");

                    if (yyy == 1)
                    {
                        updateindex();

                        console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                        console.UpdateWaterfallLevelValues();
                    }

                }

            } // LEFT CLICK MOUSE

            else if (e.Button == MouseButtons.Right) // ke9ns right click = lock or unlock bandstank memory
            {

               
                //-----------------------------------------------------------
                // This toggles the LOCK / UNLOCK and saves it
                try
                {

                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    Debug.WriteLine("BOX POS " + ii);

                    xxx = (ii / BSLength); //find row 

                    Debug.WriteLine("==CONSOLE RIGHT CLICK " + xxx + " , " + ii + " , " + console.band_stacks[nnn]);


                    if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.

                    textBox1.SelectionStart = (xxx * BSLength);
                    textBox1.SelectionLength = BSLength;

                    console.SaveBandA(); // put away last freq you were on before moving

                    updateindex();

                    console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                    console.UpdateWaterfallLevelValues();


                    if (filter1[xxx].Contains("@"))
                    {
                        filter1[xxx] = filter1[xxx].Substring(0, (filter1[xxx].Length) - 1); // toggle LOCK OFF
                    }
                    else
                    {
                        filter1[xxx] = filter1[xxx] + "@"; // toggle LOCK ON

                    }

                    DB.SaveBandStack(console.last_band, xxx, mode1[xxx], filter1[xxx], freq1[xxx]);

                    Debug.WriteLine("band== " + console.last_band);
                    Debug.WriteLine("xxx== " + xxx);
                    Debug.WriteLine("bandstack== " + filter);
                    Debug.WriteLine("freq== " + freq1[xxx]);

                    bandstackupdate(); // update bandstack screen
                    updateindex();

                }
                catch
                {
                    Debug.WriteLine("Bad location1");

                }

            } // RIGHT CLICK MOUSE (LOCK)
            
            buttonSort.Focus();  // put focus back on button

        } //textBox1_MouseUp


        //=========================================================================================================
        private void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.ShortcutsEnabled = false;


            if (e.Button == MouseButtons.Left)
            {
                Debug.WriteLine("LEFT BUTTON");
                try
                {
                    int ii = textBox2.GetCharIndexFromPosition(e.Location);

                    xxx2 = (ii / BSLength); //find row 

                    if (xxx2 >= console.band_stacks[nnn2]) return; // if you click past the last index freq, then do nothing.

                    Debug.WriteLine("xxx2 " + xxx2 + " , " + ii);

                    textBox2.SelectionStart = (xxx2 * BSLength);
                    textBox2.SelectionLength = BSLength;

                    Debug.WriteLine("index at start of click2 " + console.iii2);

                    if (console.filter22[console.iii2] == "") // check if current index locked
                    {
                        yyy2 = 1;
                        console.SaveBandB(); // put away last freq you were on before moving
                        Debug.WriteLine("OPEN SO SAVE2");
                    }
                    else
                    {
                        Debug.WriteLine("LOCKED SO DONT SAVE2 " + console.iii2 + " says " + console.filter22[console.iii2]);
                    }

                    console.iii2 = xxx2; // update new position in bandstack for checking if its locked

                    Debug.WriteLine("index after click2 " + console.iii2);

                    yyy2 = 0;

                    updateindex2();

                    console.SetBand2(mode12[xxx2], filter12[xxx2], freq12[xxx2]);

                    console.UpdateWaterfallLevelValues();
                }
                catch
                {
                    Debug.WriteLine("Failed to determine index or cannot save bandstack because its locked");

                    if (yyy2 == 1)
                    {
                        updateindex2();

                        console.SetBand2(mode12[xxx2], filter12[xxx2], freq12[xxx2]);

                        console.UpdateWaterfallLevelValues();
                    }

                }

            } // LEFT CLICK MOUSE

            else if (e.Button == MouseButtons.Right) // ke9ns right click = lock or unlock bandstank memory
            {
 
                //-----------------------------------------------------------
                // This toggles the LOCK / UNLOCK and saves it
                try
                {

                    int ii = textBox2.GetCharIndexFromPosition(e.Location);

                    Debug.WriteLine("BOX POS2 " + ii);

                    xxx2 = (ii / BSLength); //find row 

                    Debug.WriteLine("==CONSOLE RIGHT CLICK2 " + xxx2 + " , " + ii + " , " + console.band_stacks[nnn2]);


                    if (xxx2 >= console.band_stacks[nnn2]) return; // if you click past the last index freq, then do nothing.

                    textBox2.SelectionStart = (xxx2 * BSLength);
                    textBox2.SelectionLength = BSLength;

                    console.SaveBandB(); // put away last freq you were on before moving

                    updateindex2();

                    console.SetBand2(mode12[xxx2], filter12[xxx2], freq12[xxx2]);

                    console.UpdateWaterfallLevelValues();


                    if (filter12[xxx2].Contains("@"))
                    {
                        filter12[xxx2] = filter12[xxx2].Substring(0, (filter12[xxx2].Length) - 1); // toggle LOCK OFF
                    }
                    else
                    {
                        filter12[xxx2] = filter12[xxx2] + "@"; // toggle LOCK ON

                    }


                    DB.SaveBandStack2(console.last_band2, xxx2, mode12[xxx2], filter12[xxx2], freq12[xxx2]);

                    Debug.WriteLine("band== " + console.last_band2);
                    Debug.WriteLine("xxx== " + xxx2);
                    Debug.WriteLine("bandstack== " + filter);
                    Debug.WriteLine("freq== " + freq12[xxx2]);

                    bandstackupdate(); // update bandstack screen
                    updateindex2();

                }
                catch
                {
                    Debug.WriteLine("Bad location1");

                }

            } // RIGHT CLICK MOUSE (LOCK)
           

            buttonSort.Focus();  // put focus back on button

        } //textBox2_MouseUp



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //   Debug.WriteLine("key press1 " + e.KeyCode);


            if (e.KeyCode == Keys.Delete) // ke9ns add to check for DELETE key press
            {
                try
                {
                    if (console.band_stacks[nnn] < 3) return;    // dont allow removing all the bandstacks

                    if (xxx >= console.band_stacks[nnn]) return;    // if you click past the last index freq, then do nothing.

                    console.iii = xxx;                            // update new position in bandstack for checking if its locked

                    if (filter1[xxx].Contains("@") == false)      // can only delete an unlocked entry in the bandstack
                    {
                        DialogResult dr = MessageBox.Show("Are you sure you want to Delete the selected BandStack Entry?",
                                "Delete?",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                        if (dr == DialogResult.No) return;


                        console.PurgeBandStack(xxx, mode1[xxx], filter1[xxx], freq1[xxx].ToString());

                        console.BandStackUpdate();
                        bandstackupdate();
                        updateindex();
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("Bad location2");

                }

            } //  if (e.KeyCode == Keys.Delete)

        } // textBox1_KeyDown


        //====================================================================
        // ke9ns DEL button on Bandstack screen
        public void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (console.band_stacks[nnn] < 3) return;    // dont allow removing all the bandstacks

                if (xxx >= console.band_stacks[nnn]) return;    // if you click past the last index freq, then do nothing.

                console.iii = xxx;                            // update new position in bandstack for checking if its locked

                if (filter1[xxx].Contains("@") == false)      // can only delete an unlocked entry in the bandstack
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to Delete the selected BandStack Entry?",
                            "Delete?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (dr == DialogResult.No) return;


                    console.PurgeBandStack(xxx, mode1[xxx], filter1[xxx], freq1[xxx].ToString());

                    console.BandStackUpdate();
                    bandstackupdate();
                    updateindex();
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Bad location2");

            }

        } //  buttonDel_Click



        //=================================================================================
        // ke9ns ADD button
        public void button1_Click(object sender, EventArgs e)
        {
            bool dupfound = false;

            updateindex();
            if (console.band_stacks[nnn] < 12) // allow 12 bandstack entries each band
            {

                for (int ii = 0; ii < console.band_stacks[nnn]; ii++)  // check for freq dups, so dont add if a dup in freq
                {
                    if (freq1[ii] == Math.Round(console.VFOAFreq, 6))
                    {
                        Debug.WriteLine("BANDSTACK DUP FOUND");

                        dupfound = true;
                        break;
                    }

                }
                if (dupfound == false)
                {
                    DB.AddBandStack(band_list[nnn], console.RX1DSPMode.ToString(), console.RX1Filter.ToString(), Math.Round(console.VFOAFreq, 6)); // take current band, DSP mode, filter, and freq

                    Debug.WriteLine("BANDSTACK add: " + console.RX1DSPMode.ToString());

                    console.BandStackUpdate();
                    bandstackupdate();

                    Debug.WriteLine("BANDSTACK done");

                    xxx = console.band_stacks[nnn] - 1; // go to end of list and highlight it

                    textBox1.SelectionStart = (xxx * BSLength);
                    textBox1.SelectionLength = BSLength;
                    updateindex();
                    //  console.iii = xxx;                            // update new position in bandstack for checking if its locked

                }


                dupfound = false;

            } //  if (xxx < 12)



        } // button1_Click(


        //====================================================================================
        public bool bubble = false;
        // ke9ns SORT the bandstack for just the band your on
        public void buttonSort_Click(object sender, EventArgs e)
        {
            int index = console.band_stacks[nnn];

            //   Debug.WriteLine("buttonsort0000");

            try
            {
                if (index < 2) return; // nothing to sort


                // bubble sort
                for (int d = 0; d < index;)
                {

                    for (int f = index - 1; f > d; f--)  // check end of list first and work back to front
                    {
                        if (freq1[d] > freq1[f])
                        {

                            string tempmode = mode1[d];
                            string tempfilter = filter1[d];
                            double tempfreq = freq1[d];


                            freq1[d] = freq1[f];
                            mode1[d] = mode1[f];
                            filter1[d] = filter1[f];

                            freq1[f] = tempfreq;
                            mode1[f] = tempmode;
                            filter1[f] = tempfilter;

                            bubble = true;
                        }


                    } // for f

                    if (bubble == false) d++;
                    else bubble = false;  // reset

                } // for d

                for (int g = 0; g < index; g++)  // update database with new sorted bandstack
                {

                    console.SortBandStack(g, mode1[g], filter1[g], freq1[g]);     //   DB.SaveBandStack(console.last_band, g, mode1[g], filter1[g], freq1[g]);

                }

                console.BandStackUpdate();  // update the console with the new database sorted bandstack

                bandstackupdate();

            }
            catch (Exception)
            {
                Debug.WriteLine("Bad location3");

            }
        } // buttonSort_Click

        private void StackControl_MouseEnter(object sender, EventArgs e)
        {
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop.Checked == true) this.Activate();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public static int RIndex1 = 0;



    } // stackcontrol


} // powersdr
