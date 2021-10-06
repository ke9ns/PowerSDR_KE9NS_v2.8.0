//=================================================================
// helpbox1.cs
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
using System.Threading;
using System.IO;
using System.Drawing.Drawing2D;

//using CefSharp;            // ke9ns add to allow embedded chrome browser (for help videos)
//using CefSharp.WinForms;


namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public class helpbox1 : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen

        //   private static System.Reflection.Assembly myAssembly2 = System.Reflection.Assembly.GetExecutingAssembly();
        //  public static Stream Map_image2 = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.Wmap1.jpg");     // MAP with lat / long on it

        #region Variable Declaration
        private System.ComponentModel.IContainer components;
        private CheckBoxTS chkAlwaysOnTop;
        private TrackBar zoomSlider;
        private Panel panel1;
        private PictureBox pictureBox1;
        private ToolTip toolTip1;

        // public static helpbox1 helpbox1Form;                       // ke9ns add 

        #endregion

        #region Constructor and Destructor

        public helpbox1(Console c)  // called the very first time
        {

            InitializeComponent();

            Common.RestoreForm(this, "helpbox1", false);

            this.TopMost = true;

            imgOriginal = console.Wmap1;  // console loads original World map.

            panel1.AutoScroll = true;

            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            pictureBox1.Image = imgOriginal;

            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            this.DoubleBuffered = true;

            // pictureBox1.Image = null;
            pictureBox1.Image = PictureBoxZoom(imgOriginal, new Size(zoomSlider.Value, zoomSlider.Value));

        } // helpbox1(Console c)



        public Image PictureBoxZoom(Image img, Size size)
        {
            //   Debug.WriteLine("picture box " + img.Width + ", " + img.Height);

            if (size.Width < 1) size.Width = 1;
            if (size.Height < 1) size.Height = 1;

            //  if (size.Width > 1) pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

            Bitmap bm = new Bitmap(img, Convert.ToInt32(((float)img.Width * (float)size.Width / 4.0)), Convert.ToInt32((float)img.Height * (float)size.Height / 4.0));

            Graphics grap = Graphics.FromImage(bm);

            grap.InterpolationMode = InterpolationMode.HighQualityBicubic;

            return bm;

        }

        private void zoomSlider_Scroll(object sender, EventArgs e)
        {

            if (zoomSlider.Value > 0)
            {
                Debug.WriteLine("ZOOM SLIDER value " + zoomSlider.Value);
                //  pictureBox1.Image = null;

                pictureBox1.Image = PictureBoxZoom(imgOriginal, new Size(zoomSlider.Value, zoomSlider.Value));

            }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(helpbox1));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.zoomSlider = new System.Windows.Forms.TrackBar();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // zoomSlider
            // 
            this.zoomSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.zoomSlider.LargeChange = 1;
            this.zoomSlider.Location = new System.Drawing.Point(2, 444);
            this.zoomSlider.Minimum = 1;
            this.zoomSlider.Name = "zoomSlider";
            this.zoomSlider.Size = new System.Drawing.Size(136, 45);
            this.zoomSlider.TabIndex = 112;
            this.toolTip1.SetToolTip(this.zoomSlider, "Zoom Level");
            this.zoomSlider.Value = 1;
            this.zoomSlider.Scroll += new System.EventHandler(this.zoomSlider_Scroll);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAlwaysOnTop.BackColor = System.Drawing.Color.Transparent;
            this.chkAlwaysOnTop.ForeColor = System.Drawing.Color.White;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(537, 439);
            this.chkAlwaysOnTop.MaximumSize = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.MinimumSize = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.TabIndex = 100;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = false;
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 470);
            this.panel1.TabIndex = 115;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(616, 433);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // helpbox1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(30)))), ((int)(((byte)(63)))));
            this.ClientSize = new System.Drawing.Size(640, 470);
            this.Controls.Add(this.zoomSlider);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(656, 509);
            this.Name = "helpbox1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "PowerSDR World Map (Use slider to ZOOM), (Pull Corner to make larger)";
            this.toolTip1.SetToolTip(this, "World Map");
            this.Closing += new System.ComponentModel.CancelEventHandler(this.helpbox1_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.helpbox1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties






        #endregion

        #region Event Handler

        private void helpbox1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "helpbox1");
        }



        #endregion



        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }



        private void helpbox1_FormClosing(object sender, FormClosingEventArgs e)
        {



        }  // helpbox1_FormClosing

        private Image imgOriginal;


        public void HelpChrome1()
        {



        } // void InitChrome()


        private void helpbox1_message_TextChanged(object sender, EventArgs e)
        {



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    } // helpbox1

} // PowerSDR
