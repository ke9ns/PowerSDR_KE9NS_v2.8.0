//=================================================================
// swl.cs
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
using System.Linq;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class SwlControl : System.Windows.Forms.Form
    {

        public SpotControl SpotForm;                     // ke9ns add  communications with spot.cs 
        public ScanControl ScanForm;                            // ke9ns add freq Scanner function

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

       

        #region Constructor and Destructor

        public SwlControl(Console c)
        {
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "SwlForm", true);
           

            //   bandSwlupdate();


        } // swlcontrol


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

        private void SwlControl_Load(object sender, EventArgs e)
        {

            bandSwlupdate();

        }


        //=======================================================================================================================
        private void SwlControl_FormClosing(object sender, FormClosingEventArgs e)
        {

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SwlForm");


        }

        //===================================================================================
        //===================================================================================
        //===================================================================================

        int[] swl_index = new int[20000];

        public void bandSwlupdate()
        {

            int iii = 0;

            string bigmessage = ""; // full textbox string (combine 1 and 2)

            Debug.WriteLine("swl index size= " + SpotControl.SWL_Index1);

            richTextBox1.Text = richTextBox1.Text.TrimEnd('\r', '\n');

            DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            SpotControl.UTCNEW1 = Convert.ToInt16(UTCD.ToString("HHmm")); // convert 24hr UTC to int

            //  Debug.WriteLine("Day: " + SpotControl.UTCDD);
            //    Debug.WriteLine("Day1: " + (byte)UTCD.DayOfWeek);

            //   Debug.WriteLine("richtext " + richTextBox1.Text + " ,");

            iii = 0;

            int ii = 0;
            try
            {

                for (ii = 0; ii < SpotControl.SWL_Index1; ii++) // check all spots to see which ones are on at this particular time and day
                {

                    // station check 
                    if ((SpotControl.SWL_Station[ii].IndexOf(richTextBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) || (richTextBox1.Text == "") || (richTextBox1.Text == " "))
                    {

                        //   Debug.WriteLine("1SWL FREQ " + SpotControl.SWL_Freq[ii] + " , " + ii);


                        // station check days on air and time on air
                        if (
                            ((SpotControl.SWL_Day1[ii] & SpotControl.UTCDD) > 0) && (((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1)) ||
                            ((SpotControl.SWL_TimeN[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] < SpotControl.SWL_TimeN[ii])))
                             )
                        {

                            //  Debug.WriteLine("station found" + SpotControl.SWL_Freq[ii] + " , "+ SpotControl.SWL_Day1[ii]);


                            swl_index[iii++] = ii; // keep track of frequencies on at the moment

                            if (SpotControl.SWL_Station[ii].Length > 25) SpotControl.SWL_Station[ii] = SpotControl.SWL_Station[ii].Substring(0, 25);

                          //  Debug.WriteLine("station mode " + SpotControl.SWL_Mode[ii] + " , " + SpotControl.SWL_Station[ii]);

                            if ((SpotControl.SWL_Station[ii].IndexOf("volmet", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("FM", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "FM";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("DSC-", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("hfdl", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("ALE-", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("ALE4", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("WLO", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("Beacon-", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("Maritime", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("fax", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("marker", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("weather", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("number", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("military", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("stanag", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("uscg", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("gander", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("Meteo", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("propag", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("wx", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("cw", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "CWU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("rtty", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("SSTV", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("olivia", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("navy", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("army", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("force", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("digital", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("drm", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            else if ((SpotControl.SWL_Station[ii].IndexOf("FT8", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                SpotControl.SWL_Mode[ii] = "DIGU";
                            }
                            
                            else if ((SpotControl.SWL_Freq[ii] / 1000000) >= 29)
                            {
                              //  SpotControl.SWL_Mode[ii] = "USB";
                            }
                            else
                            {
                                SpotControl.SWL_Mode[ii] = "SAM";
                            }

                          //  Debug.WriteLine("station mode " + SpotControl.SWL_Mode[ii] + " , " + SpotControl.SWL_Station[ii]);


                            if (SpotControl.SWL_Band[ii] > 99)
                            {
                                bigmessage += (String.Format("{0:00.000000}", (double)(SpotControl.SWL_Freq[ii]) / 1000000.0) +
                                    " " + SpotControl.SWL_Station[ii].PadRight(25, ' ') + " " + SpotControl.SWL_Loc[ii].PadRight(3, ' ') +
                                    " " + SpotControl.SWL_TimeN[ii].ToString().PadLeft(4, '0') + ":" + SpotControl.SWL_TimeF[ii].ToString().PadLeft(4, '0') +
                                 "_" +   "\r\n");
                            }
                            else
                            {
                                bigmessage += (String.Format("{0:00.000000}", (double)(SpotControl.SWL_Freq[ii]) / 1000000.0) +
                                 "  " + SpotControl.SWL_Station[ii].PadRight(25, ' ') + " " + SpotControl.SWL_Loc[ii].PadRight(3, ' ') +
                                   " " + SpotControl.SWL_TimeN[ii].ToString().PadLeft(4, '0') + ":" + SpotControl.SWL_TimeF[ii].ToString().PadLeft(4, '0') +
                                "_" +  "\r\n");
                            }

                        } // check time
                    } // text search to narrow down


                } // for loop through SWL_Index
            }
            catch (Exception e)
            {
                //   MessageBox.Show(new Form() { TopMost = true }, "swlform is open10 " + ii + " , " + e);
                Debug.WriteLine("SWL Load problem " + ii + " , " + e);
            }

            Debug.WriteLine("SWL DONE");


            richTextBox2.Text = bigmessage; // update screen

          

        } // bandSwlupdate


        //======================================================================== 
        //====================================================================================

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }




        public static int RIndex1 = 0;

        private void button1_Click(object sender, EventArgs e)
        {

            bandSwlupdate();
            richTextBox2.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bandSwlupdate();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            bandSwlupdate();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                bandSwlupdate();
            }
        }

        private void richTextBox2_MouseDown(object sender, MouseEventArgs e)
        {

            richTextBox2.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up


        }


        int xxx = 0;

        int WidthOfText = 52; // was 52 .236
        public void richTextBox2_MouseUp(object sender, MouseEventArgs e)
        {

            richTextBox2.ShortcutsEnabled = false;


            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    int ii = richTextBox2.GetCharIndexFromPosition(e.Location);

                    xxx = (ii / WidthOfText); //find row 52

                    //   Debug.WriteLine("ii " + ii);
                    //  Debug.WriteLine("xxx " + xxx);

                    richTextBox2.SelectionStart = (xxx * WidthOfText);
                    richTextBox2.SelectionLength = WidthOfText-1;

                    //   console.SaveBand(); // put away last freq you were on before moving
                    Debug.WriteLine("INDEX YOU CLICKED ON " + ii);
                    Debug.WriteLine("INDEX YOU CLICKED ON2 " + xxx + " , " + swl_index[xxx]);

                    Debug.WriteLine("freq " + SpotControl.SWL_Freq[swl_index[xxx]]);

                    console.VFOAFreq = ((double)SpotControl.SWL_Freq[swl_index[xxx]]) / 1000000.0; // convert to MHZ

                    //   Display.VFOA = ((long)SpotControl.SWL_Freq[swl_index[xxx]]) ; // keep as hz
                 //   Debug.WriteLine("INDEX YOU CLICKED Station " + SpotControl.SWL_Station[swl_index[xxx]]);

                  //  Debug.WriteLine("INDEX YOU CLICKED MODE " + SpotControl.SWL_Mode[swl_index[xxx]]);

                    if (SpotControl.SWL_Mode[swl_index[xxx]] == "AM") console.RX1DSPMode = DSPMode.SAM;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "SAM") console.RX1DSPMode = DSPMode.SAM;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "USB") console.RX1DSPMode = DSPMode.USB;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "DIGU") console.RX1DSPMode = DSPMode.DIGU;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "FM") console.RX1DSPMode = DSPMode.FM;  // .219


                    richTextBox2.Focus();
                    richTextBox2.Show();


                }
                catch
                {
                    Debug.WriteLine("Bad location");

                }
            }
            else if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) // .222 RX2
            {

                //  const int WM_VSCROLL = 0x115;
                //  const int SB_ENDSCROLL = 8;
                Console.SendMessageW(this.Handle, 0x115, (IntPtr)0x08, this.Handle); // to prevent a silly windows scroll feature that normally comes from a mouse wheel click

                richTextBox2.Cursor = Cursors.Hand;

                try
                {
                    int ii = richTextBox2.GetCharIndexFromPosition(e.Location);
                  
                    xxx = (ii / WidthOfText); //find row 52

                    //   Debug.WriteLine("ii " + ii);
                    //   Debug.WriteLine("xxx " + xxx);


                    richTextBox2.SelectionStart = (xxx * WidthOfText);
                    richTextBox2.SelectionLength = WidthOfText-1;

                    //   console.SaveBand(); // put away last freq you were on before moving
                        Debug.WriteLine("2INDEX YOU CLICKED ON " + ii);
                       Debug.WriteLine("2INDEX YOU CLICKED ON2 " + xxx + " , " + swl_index[xxx]);

                    //   Debug.WriteLine("freq " + SpotControl.SWL_Freq[swl_index[xxx]]);

                    console.VFOBFreq = ((double)SpotControl.SWL_Freq[swl_index[xxx]]) / 1000000.0; // convert to MHZ

                    //   Display.VFOA = ((long)SpotControl.SWL_Freq[swl_index[xxx]]) ; // keep as hz
                    //   Debug.WriteLine("INDEX YOU CLICKED Station " + SpotControl.SWL_Station[swl_index[xxx]]);

                    //  Debug.WriteLine("INDEX YOU CLICKED MODE " + SpotControl.SWL_Mode[swl_index[xxx]]);

                    if (SpotControl.SWL_Mode[swl_index[xxx]] == "AM") console.RX2DSPMode = DSPMode.SAM;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "SAM") console.RX2DSPMode = DSPMode.SAM;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "USB") console.RX2DSPMode = DSPMode.USB;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "DIGU") console.RX2DSPMode = DSPMode.DIGU;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "FM") console.RX2DSPMode = DSPMode.FM;  // .219



                    richTextBox2.Focus();
                    richTextBox2.Show();


                }
                catch
                {
                    Debug.WriteLine("Bad location");

                }
            }

           // button3.Focus();

            //  button1.Focus();
        }

        private void richTextBox3_MouseClick(object sender, MouseEventArgs e)
        {

            string word = getWordAtIndex(richTextBox3, richTextBox3.SelectionStart);

            richTextBox1.Text = word;
            bandSwlupdate();

        } //richTextBox3_MouseClick


        string getWordAtIndex(RichTextBox RTB, int index)
        {
            string wordSeparators = " .,;-!?\r\n\"";
            int cp0 = index;
            int cp2 = RTB.Find(wordSeparators.ToCharArray(), index);
            for (int c = index; c > 0; c--)
            { if (wordSeparators.Contains(RTB.Text[c])) { cp0 = c + 1; break; } }
            int l = cp2 - cp0;
            if (l > 0) return RTB.Text.Substring(cp0, l); else return "";
        }

        private void SwlControl_MouseEnter(object sender, EventArgs e)
        {
          
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop.Checked == true) this.Activate();
        }

        private void richTextBox2_MouseMove(object sender, MouseEventArgs e)
        {
            richTextBox2.Cursor = Cursors.Hand;
        }

        private void richTextBox2_MouseHover(object sender, EventArgs e)
        {
            richTextBox2.Cursor = Cursors.Hand;
        }
    } // Swlcontrol


} // powersdr
