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
    public partial class helpbox1 : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen

        //   private static System.Reflection.Assembly myAssembly2 = System.Reflection.Assembly.GetExecutingAssembly();
        //  public static Stream Map_image2 = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.Wmap1.jpg");     // MAP with lat / long on it

        #region Variable Declaration
       

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
