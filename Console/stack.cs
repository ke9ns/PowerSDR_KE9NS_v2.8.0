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
using System.Diagnostics;
using System.Drawing;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PowerSDR
{
    public class StackControl : System.Windows.Forms.Form
    {

       
        public static SpotControl SpotForm;                     // ke9ns add  communications with spot.cs 
        public ScanControl ScanForm;                            // ke9ns add freq Scanner function
        
       
        public static Console console;   // ke9ns mod  to allow console to pass back values to stack screen
        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBoxTS grpPlayback;
        private System.Windows.Forms.GroupBox grpPlaylist;
        private System.Windows.Forms.MainMenu mainMenu1;
        private CheckBoxTS chkAlwaysOnTop;
        public TextBox textBox1;
        public Button buttonSort;
        public Button buttonAdd;
        public Button buttonDel;
        private TextBox textBox3;
        private IContainer components;

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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StackControl));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonSort = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(12, 112);
            this.textBox1.MaximumSize = new System.Drawing.Size(254, 222);
            this.textBox1.MaxLength = 1000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 210);
            this.textBox1.TabIndex = 60;
            this.textBox1.TabStop = false;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // buttonSort
            // 
            this.buttonSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSort.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSort.Location = new System.Drawing.Point(55, 333);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(45, 23);
            this.buttonSort.TabIndex = 61;
            this.buttonSort.Text = "Sort";
            this.buttonSort.UseVisualStyleBackColor = false;
            this.buttonSort.Click += new System.EventHandler(this.buttonSort_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAdd.Location = new System.Drawing.Point(3, 333);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(46, 23);
            this.buttonAdd.TabIndex = 62;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonDel
            // 
            this.buttonDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonDel.Location = new System.Drawing.Point(106, 333);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(45, 23);
            this.buttonDel.TabIndex = 63;
            this.buttonDel.Text = "Del";
            this.buttonDel.UseVisualStyleBackColor = false;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(154, 328);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(47, 35);
            this.chkAlwaysOnTop.TabIndex = 59;
            this.chkAlwaysOnTop.Text = "Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(12, 12);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(176, 94);
            this.textBox3.TabIndex = 9;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "Click on line to change freq.\r\nRight Click on line to LOCK/UNLOCK.\r\nDEL key or Wh" +
    "eel Click to Delete Entry.\r\nCTRL + Right Click on BAND button to ADD to BandStac" +
    "k";
            // 
            // StackControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(200, 361);
            this.Controls.Add(this.buttonDel);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonSort);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.textBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(274, 400);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "StackControl";
            this.Text = "Band Stack";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StackControl_FormClosing);
            this.Load += new System.EventHandler(this.StackControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        #region Properties



        #endregion

        #region Event Handlers







        #endregion

        private void StackControl_Load(object sender, EventArgs e)
        {
            bandstackupdate();
           
        }


        //=======================================================================================================================
        private void StackControl_FormClosing(object sender, FormClosingEventArgs e)
        {
           
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

       public int BSLength = 22;  // 31 length of a BandStack Line

       public int nnn = 0; // 0-41 based on last_band

       public string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
                                     "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
                                      "LMF","120M","90M","61M","49M","41M","31M","25M",
                                     "22M","19M","16M","14M","13M","11M",
                                     "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
                                     "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
                                     "VHF12", "VHF13" };

       //===========================================================================================================
        public void bandstackupdate()
        {
            string bigmessage = null; // full textbox string (combine 1 and 2)
            string bigmessagea = null; // full textbox string (combine 1 and 2)

            string bigmessage1a = null; // each freq string
            string bigmessage1 = null; // each freq string
            string bigmessage2 = null; // each memory string
            string bigmessage3 = null; // each lock or unlock

            for (nnn = 0; nnn < 41; nnn++) // total number of possible Bands
            {
                if (band_list[nnn] == console.last_band) break; // this is the current band_list index 
            }

      
            for (int ii = 0; ii < console.band_stacks[nnn]; ii++)
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
                  //  Debug.WriteLine("BANDSTACK: " + freq);

                  
                    freq1[ii] = freq;

                  //   if (ii >= 9) bigmessage1 = (ii + 1).ToString() + ": " + freq.ToString("N" + 6).PadLeft(11) + ":"; //.PadLeft(11)
                  //    else bigmessage1 = (ii + 1).ToString() + " : " + freq.ToString("N" + 6).PadLeft(11) + ":";

                    bigmessage1 = freq.ToString("####.000000").PadLeft(11) + ":"; // was N6 4 less than having index numbers

                    bigmessage2 = "---:";
                    //----------------------------------------------------------------


                    if (SpotForm != null)
                    {
                      //  Debug.WriteLine("1Rows Count " + SpotForm.dataGridView2.Rows.Count);

                        for (int aa = 0; aa < SpotForm.dataGridView2.Rows.Count; aa++) // get current # of memories we have available; ii++)     // Index through entire DXspot to find what is on this panadapter (draw vert lines first)
                        {

                            if (freq == Convert.ToDouble(SpotForm.dataGridView2[1, aa].Value))
                            {
                                Debug.WriteLine("found memory" + SpotForm.dataGridView2[0, aa]);
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


                    //   Debug.WriteLine("Value "+ value + " , " + BSLength);

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

        } // bandstackupdate


        //======================================================================== 
    public void updateindex()    
    {
        switch(console.RX1Band)
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
       //====================================================================================

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        }


        public int xxx = 0;
        public int yyy = 0;

        //=========================================================================================================
        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false;


            if (e.Button == MouseButtons.Left)
            {
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
                        console.SaveBand(); // put away last freq you were on before moving
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
                // This saves the bandstack (if unlocked)
                try
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    xxx = (ii / BSLength); //find row 

                    if (xxx >= console.band_stacks[nnn]) return; // if you click past the last index freq, then do nothing.


                    textBox1.SelectionStart = (xxx * BSLength);
                    textBox1.SelectionLength = BSLength;

                    Debug.WriteLine("index at start of click " + console.iii);

                    if (console.filter2[console.iii] == "") // check if current index locked
                    {
                        yyy = 1;
                        console.SaveBand(); // put away last freq you were on before moving
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

                    console.SaveBand(); // put away last freq you were on before moving

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

            } // RIGHT CLICK MOUSE
            else if (e.Button == MouseButtons.Middle) // ke9ns Middle erases bandstack entries 1 at a time
            {

                try
                {
                    if (console.band_stacks[nnn] < 3) return;    // dont allow removing all the bandstacks

                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    xxx = (ii / BSLength);                                // find row 

                    if (xxx >= console.band_stacks[nnn]) return;    // if you click past the last index freq, then do nothing.

                   
                    textBox1.SelectionStart = (xxx * BSLength);
                    textBox1.SelectionLength = BSLength;
                 
                    console.iii = xxx;                            // update new position in bandstack for checking if its locked


                    if (filter1[xxx].Contains("@") == false)      // can only delete an unlocked entry in the bandstack
                    {

                        console.PurgeBandStack(xxx, mode1[xxx], filter1[xxx], freq1[xxx].ToString());

                        console.BandStackUpdate();
                        bandstackupdate();
                        updateindex();
                    }
                }
                catch(Exception)
                {
                    Debug.WriteLine("Bad location2");

                }


            } // MIDDLE CLICK MOUSE


            buttonSort.Focus();  // put focus back on button

        } //textBox1_MouseUp



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
                        dupfound = true;
                        break;
                    }

                }
                if (dupfound == false)
                {
                    DB.AddBandStack(band_list[nnn], console.RX1DSPMode.ToString(), console.RX1Filter.ToString(), Math.Round(console.VFOAFreq, 6)); // take current band, DSP mode, filter, and freq

                    console.BandStackUpdate();
                    bandstackupdate();
                   

                  xxx =  console.band_stacks[nnn] - 1; // go to end of list and highlight it

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
                    else  bubble = false;  // reset

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



        public static int RIndex1 = 0;



    } // stackcontrol


} // powersdr
