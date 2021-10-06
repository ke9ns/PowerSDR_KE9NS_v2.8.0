//=================================================================
// spot_decoder.cs
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
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public class SpotDecoder : System.Windows.Forms.Form
    {
        #region Variable Declaration
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
        public TextBox textBox1;
        public CheckBoxTS checkBoxRTTY;

        public static SpotControl SpotForm;                       // ke9ns add DX spotter function
        public RadioButton radioButton1;
        public RadioButton radioButton2;
        public CheckBoxTS checkBoxTS1;
        public CheckBoxTS checkBoxTS2;
        public CheckBoxTS checkBoxCW;
        public RadioButton radioButton3;
        public NumericUpDownTS numRXCW;
        private Label label4;
        public PictureBox pictureBox1;
        public TextBox textBox2;
        public System.Windows.Forms.Timer timer_RTTY;
        public static Console console;                         // ke9ns mod  to allow console to pass back values to setup screen

        #endregion

        #region Constructor and Destructor

        public SpotDecoder(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            console = c;
            Common.RestoreForm(this, "SpotOptions", false);


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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotDecoder));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numRXCW = new System.Windows.Forms.NumericUpDownTS();
            this.checkBoxCW = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxTS2 = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxRTTY = new System.Windows.Forms.CheckBoxTS();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBoxTS1 = new System.Windows.Forms.CheckBoxTS();
            this.timer_RTTY = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numRXCW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // numRXCW
            // 
            this.numRXCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numRXCW.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRXCW.Location = new System.Drawing.Point(155, 154);
            this.numRXCW.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numRXCW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRXCW.Name = "numRXCW";
            this.numRXCW.Size = new System.Drawing.Size(39, 20);
            this.numRXCW.TabIndex = 113;
            this.toolTip1.SetToolTip(this.numRXCW, "Which Band to Start Slow Beacaon Scan on:\r\n1=14.1mhz\r\n2=18.11mhz\r\n3=21.15mhz\r\n4=2" +
        "4.93mhz\r\n5=28.2mhz\r\n");
            this.numRXCW.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRXCW.ValueChanged += new System.EventHandler(this.numRXCW_ValueChanged);
            // 
            // checkBoxCW
            // 
            this.checkBoxCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxCW.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxCW.Image = null;
            this.checkBoxCW.Location = new System.Drawing.Point(12, 147);
            this.checkBoxCW.Name = "checkBoxCW";
            this.checkBoxCW.Size = new System.Drawing.Size(109, 33);
            this.checkBoxCW.TabIndex = 111;
            this.checkBoxCW.Text = "CW decode";
            this.toolTip1.SetToolTip(this.checkBoxCW, "\r\n");
            this.checkBoxCW.CheckedChanged += new System.EventHandler(this.checkBoxCW_CheckedChanged);
            // 
            // checkBoxTS2
            // 
            this.checkBoxTS2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTS2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxTS2.Image = null;
            this.checkBoxTS2.Location = new System.Drawing.Point(12, 68);
            this.checkBoxTS2.Name = "checkBoxTS2";
            this.checkBoxTS2.Size = new System.Drawing.Size(128, 33);
            this.checkBoxTS2.TabIndex = 110;
            this.checkBoxTS2.Text = "RTTY decode 850";
            this.toolTip1.SetToolTip(this.checkBoxTS2, "\r\n");
            this.checkBoxTS2.CheckedChanged += new System.EventHandler(this.checkBoxTS2_CheckedChanged);
            // 
            // checkBoxRTTY
            // 
            this.checkBoxRTTY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxRTTY.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxRTTY.Image = null;
            this.checkBoxRTTY.Location = new System.Drawing.Point(12, 12);
            this.checkBoxRTTY.Name = "checkBoxRTTY";
            this.checkBoxRTTY.Size = new System.Drawing.Size(119, 33);
            this.checkBoxRTTY.TabIndex = 106;
            this.checkBoxRTTY.Text = "RTTY decode 170";
            this.toolTip1.SetToolTip(this.checkBoxRTTY, "\r\n");
            this.checkBoxRTTY.CheckedChanged += new System.EventHandler(this.checkBoxRTTY_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(313, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(482, 305);
            this.textBox1.TabIndex = 100;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radioButton1.Location = new System.Drawing.Point(176, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(49, 17);
            this.radioButton1.TabIndex = 107;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Mark";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radioButton2.Location = new System.Drawing.Point(240, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(56, 17);
            this.radioButton2.TabIndex = 108;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Space";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radioButton3.Location = new System.Drawing.Point(240, 154);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(50, 17);
            this.radioButton3.TabIndex = 112;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Tone";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(200, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 114;
            this.label4.Text = "WPM";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(23, 366);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(752, 114);
            this.pictureBox1.TabIndex = 115;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 186);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(291, 165);
            this.textBox2.TabIndex = 116;
            // 
            // checkBoxTS1
            // 
            this.checkBoxTS1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTS1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxTS1.Image = null;
            this.checkBoxTS1.Location = new System.Drawing.Point(12, 39);
            this.checkBoxTS1.Name = "checkBoxTS1";
            this.checkBoxTS1.Size = new System.Drawing.Size(128, 33);
            this.checkBoxTS1.TabIndex = 109;
            this.checkBoxTS1.Text = "RTTY decode 450";
            this.checkBoxTS1.CheckedChanged += new System.EventHandler(this.checkBoxTS1_CheckedChanged);
            // 
            // timer_RTTY
            // 
            this.timer_RTTY.Interval = 10;
            this.timer_RTTY.Tick += new System.EventHandler(this.timer_RTTY_Tick);
            // 
            // SpotDecoder
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(807, 504);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numRXCW);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.checkBoxCW);
            this.Controls.Add(this.checkBoxTS2);
            this.Controls.Add(this.checkBoxTS1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.checkBoxRTTY);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpotDecoder";
            this.Text = "Decoders";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SpotOptions_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.numRXCW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties






        #endregion

        #region Event Handler

        private void SpotOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SpotOptions");
        }



        #endregion

        public double RTTYShift = 0; // 1170=170, 1450 or 1850
        public double RTTYBase = 325;
        public double RTTYBaud = 45.45;


        private void checkBoxRTTY_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRTTY.Checked == true)
            {

                checkBoxTS1.Checked = false;
                checkBoxTS2.Checked = false;
                RTTYShift = 170;
                Debug.WriteLine("170======");
                RTTYBaud = 45.45;    // 45.5baud = 22msec/bit or .165second/char
                RTTYLaunch();
            }
            else
            {
                RTTYLaunch();
            }

        } // checkBoxRTTY_CheckedChanged

        private void checkBoxTS1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTS1.Checked == true)
            {

                checkBoxRTTY.Checked = false;
                checkBoxTS2.Checked = false;
                RTTYShift = 450;
                Debug.WriteLine("450======");
                RTTYLaunch();
            }
            else
            {
                RTTYLaunch();
            }
        } // checkBoxTS1_CheckedChanged

        private void checkBoxTS2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTS2.Checked == true)
            {

                checkBoxTS1.Checked = false;
                checkBoxRTTY.Checked = false;
                RTTYShift = 850;
                Debug.WriteLine("850======");

                RTTYLaunch();
            }
            else
            {
                RTTYLaunch();
            }
            Debug.WriteLine("===850======");

        }// checkBoxTS2_CheckedChanged

        //============================================================
        private void RTTYLaunch()
        {
            Debug.WriteLine("RTTYLAUNCH");

            if (console.RTTY == false)
            {

                SpotForm.GoertzelCoef(RTTYBase, console.SampleRate1);  // comes up with the Coeff values for the freq and sample rate used
                SpotForm.GoertzelCoef2((RTTYBase + RTTYShift), console.SampleRate1);  // comes up with the Coeff values for the freq and sample rate used

                Debug.WriteLine("Shift is " + RTTYShift);
                Debug.WriteLine("RTTY MARK is " + RTTYBase);
                Debug.WriteLine("RTTY SPACE is " + (RTTYBase + RTTYShift));

                console.RTTY = true;

                Debug.WriteLine("RTTY MARK COEFF is " + SpotForm.Coeff);
                Debug.WriteLine("RTTY SPACE COEFF is " + SpotForm.Coeff2);

                Debug.WriteLine("RTTY Audio Buffer size " + console.WWVframeCount + " , " + console.BlockSize1);

                time1.Restart(); //
                Debug.WriteLine("RTTYLAUNCH1");

                timer_RTTY.Enabled = true;
                Debug.WriteLine("RTTYLAUNCH2");

                timer_RTTY.Start();
                Debug.WriteLine("RTTYLAUNCH3");

            }
            else
            {

                console.RTTY = false;
                timer_RTTY.Enabled = false; // turn off timer

            }

        } // RTTYLaunch()

        //=======================================================================================================================================
        // RTTY Decoder
        // decode RTTY be reading Mark = 1000hz and Space = 1170hz (always decode in DIGU or L)
        // 45.5baud = 22msec/bit or .165second/char
        // space = 0, bit1,2,3,4,5, Mark = 1  (this is 1 character)
        // dec27 = Figures, dec31= Letters

        // n= IDLE (Blank)
        // f = shift to Figures
        // l = shift to Letters
        // b = BELL
        //  \n = LF          
        // \r = CR

        Stopwatch time1 = new Stopwatch();
        long timer1 = 0;

        public const string Letters = "nE\nA SIU\rDRJNFCKTZLWHYPQOBGfMXVl";  // 0-31
        public const string Figures = "n3\n- b87\r$4',!:(5*)2#6019?&f./;l"; // 0-31

        int Mark = 0;
        int Space = 0;
        bool IDENT = false; // Mark = true, Space = false

        private void timer_RTTY_Tick(object sender, EventArgs e)
        {
            //  Debug.WriteLine("TICK TICK");

            //   Debug.WriteLine("RTTY TIMER TICK " + timer1);

            if (console.WWVReady == true)
            {
                console.WWVReady = false;

                timer1 = time1.ElapsedMilliseconds;
                time1.Restart();

                //   Debug.WriteLine("ready elasped time " + timer1);


                Mark = console.WWVTone;  // get Magnitude value from audio.cs and Goertzel routine
                Space = console.WWVTone2;  // get Magnitude value from audio.cs and Goertzel routine

                if ((Mark * 2) > Space) IDENT = true;
                else IDENT = false;

                textBox1.AppendText("Mark: " + Mark.ToString() + "  , Space: " + Space.ToString() + "  , IDENT: " + IDENT + "\r\n");

                if (IDENT == true)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }


            } //  if (console.WWVReady == true)


        } //  timer_RTTY_Tick(


        //=======================================================================
        // CW deocder launch
        private void checkBoxCW_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCW.Checked == true)
            {
                CWLaunch();

            }
            else
            {
                CWLaunch();
            }



        } // checkBoxCW_CheckedChanged

        private void numRXCW_ValueChanged(object sender, EventArgs e)
        {
            SpotForm.WPMCW = (int)numRXCW.Value;

        } // numRXCW_ValueChanged

        private void CWLaunch()
        {

            if (console.RXCW == false)
            {
                console.RXCW = true;
                Debug.WriteLine("CW DECODER ON ");

                Thread t = new Thread(new ThreadStart(SpotForm.CWTime));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "CW reader";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Highest;
                t.Start();


            }
            else
            {
                Debug.WriteLine("CW DECODER OFF");
                console.RXCW = false;


            }

        } // CWLaunch()

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {



        } // pictureBox1_Paint




    } // SpotOptions


} // PowerSDR
