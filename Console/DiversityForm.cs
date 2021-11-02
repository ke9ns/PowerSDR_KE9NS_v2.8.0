//=================================================================
// DiversityForm.cs
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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for DiversityForm.
    /// </summary>
    public partial class DiversityForm : System.Windows.Forms.Form
    {
        private Point p = new Point(200, 200);
        private Color topColor = Color.FromArgb(0, 120, 0);
        private Color bottomColor = Color.FromArgb(0, 40, 0);
        private Color lineColor = Color.FromArgb(0, 255, 0);
        private double locked_r = 0.0f;
        private double locked_angle = 0.0f;
        //private Image crosshair = new Bitmap("C:\\crosshair.png");
        private Console console;

      
        public bool CATEnable
        {
            get { return chkEnable.Checked; }
            set
            {
                chkEnable.Checked = value;
            }
        }


        private Color imageColorTop, imageColorBottom, consoleColorBottom;
        private void picRadar_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int size = Math.Min(picRadar.Width, picRadar.Height);
            if (console.RadarColorUpdate)
            {
                string panadapterBackgroundPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\picDisplay.png";

                string consoleBackgroundPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\Console.png";

                string buttonOffPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-0.png";

                string buttonOnPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                    "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-1.png";

                Bitmap buttonOffImage = new Bitmap(buttonOffPath);
                Bitmap buttonOnImage = new Bitmap(buttonOnPath);
                chkEnable.BackgroundImage = buttonOffImage;
                btnSync.BackgroundImage = buttonOffImage;
                btnBump45.BackgroundImage = buttonOffImage;
                btnBump180.BackgroundImage = buttonOffImage;
                btnReset.BackgroundImage = buttonOffImage;  //w4tme

                btnSync.FlatAppearance.BorderColor = imageColorBottom;
                btnBump45.FlatAppearance.BorderColor = imageColorBottom;
                btnBump180.FlatAppearance.BorderColor = imageColorBottom;
                btnReset.FlatAppearance.BorderColor = imageColorBottom; //w4tme

                Bitmap panadapterBackground = new Bitmap(panadapterBackgroundPath);
                imageColorTop = panadapterBackground.GetPixel((int)(panadapterBackground.Width - 5), (int)(panadapterBackground.Height - 5));
                imageColorBottom = panadapterBackground.GetPixel((int)(panadapterBackground.Width / 9), (int)(panadapterBackground.Height / 9));


                Bitmap consoleBackground = new Bitmap(consoleBackgroundPath);
                consoleColorBottom = consoleBackground.GetPixel((int)(consoleBackground.Width - 5), (int)(consoleBackground.Height - 5));
                picRadar.BackgroundImage = consoleBackground;
                //this.BackgroundImage = consoleBackground;
                this.BackColor = consoleColorBottom;

                console.RadarColorUpdate = false;
            }

            Pen pen = new Pen(lineColor);
            // set a couple of graphics properties to make the
            // output image look nice
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.Bicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            // draw the background of the radar
            //g.FillEllipse(new LinearGradientBrush(new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1), topColor, bottomColor), 0, 0, size - 1, size - 1);
            g.FillEllipse(new LinearGradientBrush(new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1), imageColorTop, imageColorBottom), 0, 0, size - 1, size - 1);

            //TextureBrush tBrush = new TextureBrush(largeImage, new Rectangle(0, 20, largeImage.Width, largeImage.Height - 40));
            //tBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
            //g.FillEllipse(tBrush, 0, 0, size - 1, size - 1);

            // draw the outer ring (0° elevation)
            g.DrawEllipse(pen, 0, 0, size - 1, size - 1);
            // draw the inner ring (60° elevation)
            int interval = size / 3;
            g.DrawEllipse(pen, (size - interval) / 2, (size - interval) / 2, interval, interval);
            // draw the middle ring (30° elevation)
            interval *= 2;
            g.DrawEllipse(pen, (size - interval) / 2, (size - interval) / 2, interval, interval);
            // draw the x and y axis lines
            g.DrawLine(pen, new Point(0, (int)(size / 2)), new Point(size - 1, (int)(size / 2)));
            g.DrawLine(pen, new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1));

            Pen crosshairPen = new Pen(Color.Red, 2);
            //g.FillEllipse(Brushes.Yellow, p.X-4, p.Y-4, 8, 8);
            g.DrawLine(crosshairPen, p.X, p.Y - 16, p.X, p.Y - 2);
            g.DrawLine(crosshairPen, p.X, p.Y + 16, p.X, p.Y + 2);
            g.DrawLine(crosshairPen, p.X - 2, p.Y, p.X - 13, p.Y);
            g.DrawLine(crosshairPen, p.X + 2, p.Y, p.X + 13, p.Y);
            g.DrawEllipse(crosshairPen, p.X - 8, p.Y - 8, 16, 16);

            //p.X = p.X - 13;
            //p.Y = p.Y - 13;
            //g.DrawImage(crosshair, new Rectangle((p.X-(int)(crosshair.Width/2)), p.Y - (int)(crosshair.Height/2), 20, 20));
        }

        private Point PolarToXY(double r, double angle)
        {
            int L = (int)Math.Min(picRadar.Width, picRadar.Height);
            return new Point((int)(r * Math.Cos(angle) * L / 2) + L / 2, (int)(r * Math.Sin(angle) * L / 2) + L / 2);
        }

        private void picRadar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!mouse_down) return;

            int W = picRadar.Width;
            int H = picRadar.Height;
            int L = (int)Math.Min(W, H);

            // get coords relative to middle
            int x = e.X - L / 2;
            int y = e.Y - L / 2;

            // change coordinate space form pixels to percentage of half width
            double xf = (double)x / (L / 2);
            double yf = -(double)y / (L / 2);

            double _r = Math.Min(Math.Sqrt(Math.Pow(xf, 2.0) + Math.Pow(yf, 2.0)), 2.0);
            double _angle = Math.Atan2(yf, xf);
            //Debug.WriteLine("xf: "+xf.ToString("f2")+"  yf: "+yf.ToString("f2")+"  r: " + r.ToString("f4") + "  angle: " + angle.ToString("f4"));

            if (mouse_down)
            {
                if (chkLockR.Checked && chkLockAngle.Checked) return;
                if (chkLockR.Checked)
                {
                    p = PolarToXY(locked_r, _angle);
                    angle = _angle;
                }
                else if (chkLockAngle.Checked)
                {
                    if (_angle > 0 && locked_angle < 0)
                        locked_angle += Math.PI;
                    else if (_angle < 0 && locked_angle > 0)
                        locked_angle -= Math.PI;

                    p = PolarToXY(_r, locked_angle);
                    locked_r = _r;
                    r = _r;
                    angle = locked_angle;
                }
                else
                {
                    p = new Point(e.X, e.Y);
                    locked_r = _r;
                    //Debug.WriteLine("locked_r: " + r.ToString("f4"));
                    locked_angle = _angle;
                    r = _r;
                    angle = _angle;
                }

                udR.Value = (decimal)r;
                udAngle.Value = (decimal)angle;

                picRadar.Invalidate();
            }
        }

        private bool mouse_down = false;

        private void picRadar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouse_down = true;
            picRadar_MouseMove(sender, e);
        }

        private void picRadar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouse_down = false;
        }

        private void udR_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateDiversity();
        }

        private void udTheta_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateDiversity();
        }

        private double r = 0.0;
        private double angle = 0.0;

        private void UpdateDiversity()
        {
            DttSP.SetDiversityScalar((float)((r * 1.5) * Math.Cos(angle)), (float)((r * 1.5) * Math.Sin(angle)));

            int L = (int)Math.Min(picRadar.Width, picRadar.Height);
            p = new Point((int)(r * L / 2 * Math.Cos(angle)) + L / 2, -(int)(r * L / 2 * Math.Sin(angle)) + L / 2);
            picRadar.Invalidate();
        }

        private void btnSync_Click(object sender, System.EventArgs e)
        {
            console.RX2SpurReduction = console.SpurReduction;
            console.RX2DSPMode = console.RX1DSPMode;
            console.RX2Filter = console.RX1Filter;
            console.RX2PreampMode = console.RX1PreampMode;
            console.VFOSync = true;
            // console.RX2AGCMode = console.RX1AGCMode;    // no custom AGC mode for RX2 causes UHE
            console.RX2RF = console.RF;                 //W4TME
            console.dsp.GetDSPRX(1, 0).Copy(console.dsp.GetDSPRX(0, 0));

            string buttonOnPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-1.png";

            string buttonOffPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-0.png";

            Bitmap buttonOffImage = new Bitmap(buttonOffPath);
            Bitmap buttonOnImage = new Bitmap(buttonOnPath);
            this.btnSync.BackgroundImage = buttonOnImage;
            Thread.Sleep(200);
            this.btnSync.BackgroundImage = buttonOffImage;



        }

        private void chkEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            //if(chkEnable.Checked) chkEnable.BackColor = console.ButtonSelectedColor;
            //else chkEnable.BackColor = SystemColors.Control;

            string buttonOffPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-0.png";

            string buttonOnPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\FlexRadio Systems\\PowerSDR\\Skins\\" + console.CurrentSkin + "\\Console\\chkMON-1.png";

            Bitmap buttonOffImage = new Bitmap(buttonOffPath);
            Bitmap buttonOnImage = new Bitmap(buttonOnPath);

            if (chkEnable.Checked) chkEnable.BackgroundImage = buttonOnImage;
            else chkEnable.BackgroundImage = buttonOffImage;

            if (chkEnable.Checked)
            {
                if (!console.RX2Enabled) console.RX2Enabled = true;
            }
            DttSP.SetDiversity(Convert.ToInt16(chkEnable.Checked));
        }

        private void DiversityForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Common.SaveForm(this, "DiversityForm");
            console.VFOSync = false;    //W4TME
        }

        private void picRadar_Resize(object sender, EventArgs e)
        {
            UpdateDiversity();
        }

        private void btnBump45_Click(object sender, EventArgs e)
        {
            double _angle = angle;
            _angle += Math.PI / 4;
            if (_angle > 2 * Math.PI) _angle -= 2 * Math.PI;
            angle = _angle;
            UpdateDiversity();
        }

        private void btnBump180_Click(object sender, EventArgs e)
        {
            double _angle = angle;
            _angle += Math.PI;
            if (_angle > 2 * Math.PI)
                _angle -= 2 * Math.PI;
            angle = _angle;
            UpdateDiversity();
        }

        private void chkLockAngle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLockAngle.Checked)
                locked_angle = angle;
        }

        private void ChkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void DiversityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.SaveForm(this, "DiversityForm");
            console.VFOSync = false;    //W4TME
        }

        // ke9ns add for F2 video
        private void DiversityForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F2) // ke9ns add for help messages (F2 help screen)
            {
                Debug.WriteLine("F2 key");


                if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                console.helpboxForm.Show();
                console.helpboxForm.Focus();
                console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                console.helpboxForm.helpbox_message.Text = console.helpboxForm.helpbox_message.Text;            // ESC video

                string VideoID = "A0q9iK9RdMw";

                console.helpboxForm.webBrowser1.Visible = true;
                console.helpboxForm.webBrowser1.BringToFront();

                console.helpboxForm.webBrowser1.DocumentText = String.Format("<html><head>" +
                         "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                         "</head><body>" +
                         "<iframe width=\"100%\" height=\"425\"  src=\"https://www.youtube.com/embed/{0}?autoplay=1&enablejsapi=1\"" +
                         "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                         "</body></html>", VideoID);


            } // f2 key

        } // key down over ESC panel

        // ke9ns add: copy from console
        public bool MouseIsOverControl(Control c) // ke9ns keypreview must be TRUE and use MouseIsOverControl(Control c)
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }

        private void DiversityForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

            console.helpboxForm.Show();
            console.helpboxForm.Focus();
            console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
            console.helpboxForm.helpbox_message.Text = console.helpboxForm.helpbox_message.Text;            // ESC video

            string VideoID = "A0q9iK9RdMw";

            console.helpboxForm.webBrowser1.Visible = true;
            console.helpboxForm.webBrowser1.BringToFront();

            console.helpboxForm.webBrowser1.DocumentText = String.Format("<html><head>" +
                     "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                     "</head><body>" +
                     "<iframe width=\"100%\" height=\"425\"  src=\"https://www.youtube.com/embed/{0}?autoplay=1&enablejsapi=1\"" +
                     "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                     "</body></html>", VideoID);

        }

        private void chkLockR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLockR.Checked)
                locked_r = r;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            angle = 0;
            r = 0;
            UpdateDiversity();
        }
    }
}
