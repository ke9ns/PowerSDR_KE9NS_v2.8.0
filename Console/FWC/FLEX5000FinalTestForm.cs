//=================================================================
// FLEX5000FinalTestForm.cs
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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FLEX5000FinalTestForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
      
        private string common_data_path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\FlexRadio Systems\\PowerSDR\\";

        #endregion

        #region Constructor and Destructor

        public FLEX5000FinalTestForm(Console c)
        {
            InitializeComponent();
            console = c;
            this.Text += "  (PA:" + FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ")";

            if (FWCEEPROM.TRXSerial == 0)
            {
                MessageBox.Show("No TRX Serial Found.  Please enter and try again.",
                    "No TRX S/N Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Enabled = false;
            }

            if (FWCEEPROM.PASerial == 0)
            {
                MessageBox.Show("No PA Serial Found.  Please enter and try again.",
                    "No PA S/N Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Enabled = false;
            }

            if (FWCEEPROM.RFIOSerial == 0 && console.CurrentModel == Model.FLEX5000)
            {
                MessageBox.Show("No RFIO Serial Found.  Please enter and try again.",
                    "No RFIO S/N Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Enabled = false;
            }

            if (console.SampleRate1 < 96000)
                MessageBox.Show("Warning: Sample Rate should be at least 96kHz before calibrating.",
                    "Warning: Sample Rate Low",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            if (console.setupForm.DSPPhoneRXBuffer != 4096)
                /*	MessageBox.Show("Warning: DSP RX Buffer size should be at least 4096 before calibrating.",
                        "Warning: DSP RX Buffer Size Low",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);*/
                console.setupForm.DSPPhoneRXBuffer = 4096;

            /*MessageBox.Show("Production Reminder: Check the Power button to ensure\n"+
				"power off/on sequence works as intended",
				"Reminder: Check Power Button",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);*/

            // populate COM port selection with only ports that are available
            comboCOMPort.Items.Clear();
            comboCOMPort.Items.AddRange(Common.SortedComPorts());
            if (comboCOMPort.Items.Count > 0)
                comboCOMPort.SelectedIndex = 0;
            Common.RestoreForm(this, "FLEX5000FinalTestForm", false);

            // set powermaster COM port based on data in powermaster.txt file if it exists
            string pm_file_path = Path.Combine(common_data_path, "powermaster.txt");  // programData\flexradio systems\powersdr folder

            // ke9ns info:    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)  ==   %userprofile%\AppData\Roaming
            // path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\FlexRadio Systems\\PowerSDR";  // ke9ns this is the ProgramData folder
            // Application.StartupPath  = \Program Files (x86)\FlexRadio Systems\PowerSDR v2.8.0


            if (File.Exists(pm_file_path))
            {
                StreamReader reader = new StreamReader(pm_file_path);
                string[] temp = reader.ReadToEnd().Split('\n');
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].StartsWith("COM"))
                    {
                        comboCOMPort.Text = temp[i];
                        break;
                    }
                }
                reader.Close();
            }

            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    break;
                case Model.FLEX3000:
                    grpIO.Visible = false;
                    grpATU.Visible = true;
                    this.Text = this.Text.Replace("FLEX-5000", "FLEX-3000");
                    this.Text = this.Text.Replace("IO", "ATU");
                    break;
            }

            if (console.Production)
            {
                //btnPABias.Enabled = true;
                btnRunPACal.Enabled = true;
                btnPAPower.Enabled = true;
                btnPASWR.Enabled = true;
                btnATUSWR.Enabled = true;
                btnCheckEEPROM.Enabled = true;
            }
            else
            {
                //btnPABias.Enabled = false;
                btnRunPACal.Enabled = false;
                btnPAPower.Enabled = false;
                btnPASWR.Enabled = false;
                btnATUSWR.Enabled = false;
                btnCheckEEPROM.Enabled = false;
            }
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

        #region Misc Routines

        private string BandToString(Band b)
        {
            string ret_val = "";
            switch (b)
            {
                case Band.B160M: ret_val = "160m"; break;
                case Band.B80M: ret_val = "80m"; break;
                case Band.B60M: ret_val = "60m"; break;
                case Band.B40M: ret_val = "40m"; break;
                case Band.B30M: ret_val = "30m"; break;
                case Band.B20M: ret_val = "20m"; break;
                case Band.B17M: ret_val = "17m"; break;
                case Band.B15M: ret_val = "15m"; break;
                case Band.B12M: ret_val = "12m"; break;
                case Band.B10M: ret_val = "10m"; break;
                case Band.B6M: ret_val = "6m"; break;
            }
            return ret_val;
        }

        private bool IsRegionUS()
        {
            bool ret = false;
            if (console.CurrentRegion == FRSRegion.US) ret = true;
            return ret;
        }

        public void UpdateDriverBiasDebug(float f)
        {
            lstDebug.Items.Insert(0, "PA Bias Driver: " + f.ToString("f3") + " A");
        }

        public void UpdateFinalBiasDebug(float f)
        {
            lstDebug.Items.Insert(0, "PA Bias Final: " + f.ToString("f3") + " A");
        }



        #endregion

        #region Event Handlers

        Progress progress;
        private void FLEX5000FinalTestForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "FLEX5000FinalTestForm");
            if (progress != null) progress.Hide();
        }

        private void FLEX5000FinalTestForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (e.Control && e.Alt && e.KeyCode == Keys.B)
            {
                udBiasDriverTarget.Visible = true;
                udBiasFinalTarget.Visible = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.S)
            {
                btnPASWR.Visible = true;
            }


        }

        private void btnCheckAll_Click(object sender, System.EventArgs e)
        {
            ck160.Checked = true;
            ck80.Checked = true;
            ck60.Checked = true;
            ck40.Checked = true;
            ck30.Checked = true;
            ck20.Checked = true;
            ck17.Checked = true;
            ck15.Checked = true;
            ck12.Checked = true;
            ck10.Checked = true;
            ck6.Checked = true;
        }

        private void btnClearAll_Click(object sender, System.EventArgs e)
        {
            ck160.Checked = false;
            ck80.Checked = false;
            ck60.Checked = false;
            ck40.Checked = false;
            ck30.Checked = false;
            ck20.Checked = false;
            ck17.Checked = false;
            ck15.Checked = false;
            ck12.Checked = false;
            ck10.Checked = false;
            ck6.Checked = false;
        }

        private void picNullBridge_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picNullBridge.Width;
            int H = picNullBridge.Height;
            Graphics g = e.Graphics;

            g.DrawRectangle(new Pen(console.EdgeMeterBackgroundColor), 0, 0, W, H);

            SolidBrush low_brush = new SolidBrush(console.EdgeLowColor);
            SolidBrush high_brush = new SolidBrush(console.EdgeHighColor);

            g.FillRectangle(low_brush, 0, H - 4, (int)(W * 0.75), 2);
            g.FillRectangle(high_brush, (int)(W * 0.75), H - 4, (int)(W * 0.25) - 10, 2);
            double spacing = (W * 0.75 - 2.0) / 4.0;
            double string_height = 0;
            string[] list = { "10mV", "50mV", "250mV", "500mV" };
            for (int i = 1; i < 5; i++)
            {
                g.FillRectangle(low_brush, (int)(i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                g.FillRectangle(low_brush, (int)(i * spacing), H - 4 - 6, 2, 6);

                string s = list[i - 1];
                Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                SizeF size = g.MeasureString("0", f, 1, StringFormat.GenericTypographic);
                double string_width = size.Width - 2.0;
                string_height = size.Height - 2.0;

                //g.TextRenderingHint = TextRenderingHint.AntiAlias;
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawString(s, f, low_brush, (int)(i * spacing - string_width * s.Length + (int)(i / 3) + (int)(i / 4)), (int)(H - 4 - 8 - string_height));
                //g.SmoothingMode = SmoothingMode.None;
            }
            spacing = (W * 0.25 - 2.0 - 10.0) / 1.0;
            for (int i = 1; i < 2; i++)
            {
                g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing - spacing * 0.5), H - 4 - 3, 1, 3);
                g.FillRectangle(high_brush, (int)((double)W * 0.75 + i * spacing), H - 4 - 6, 2, 6);

                Font f = new Font("Arial", 7.0f, FontStyle.Bold);
                SizeF size = g.MeasureString("0", f, 3, StringFormat.GenericTypographic);
                double string_width = size.Width - 2.0;

                g.TextRenderingHint = TextRenderingHint.SystemDefault;
                g.DrawString("1V+", f, high_brush, (int)(W * 0.75 + i * spacing - (int)3.5 * string_width), (int)(H - 4 - 8 - string_height));
            }

            float num = bridge_null_volts;
            int pixel_x = 0;

            if (num <= 0.5f) // low area
            {
                spacing = (W * 0.75 - 2.0) / 4.0;
                if (num <= 0.01f)
                    pixel_x = (int)(num / 0.01f * (int)spacing);
                else if (num <= 0.05f)
                    pixel_x = (int)(spacing + (num - 0.01f) / 0.04f * spacing);
                else if (num <= 0.25f)
                    pixel_x = (int)(2 * spacing + (num - 0.05) / 0.2f * spacing);
                else
                    pixel_x = (int)(3 * spacing + (num - 0.25f) / 0.25f * spacing);
            }
            else
            {
                spacing = (W * 0.25 - 2.0 - 10.0) / 1.0;
                if (num <= 1.0f)
                    pixel_x = (int)(W * 0.75 + (num - 0.5f) / 0.5f * spacing);
                else
                    pixel_x = (int)(W * 0.75 + spacing + (num - 1.0) / 1.0 * spacing);
            }

            pixel_x = Math.Max(0, pixel_x);
            pixel_x = Math.Min(W, pixel_x);

            Pen line_pen = new Pen(console.EdgeAVGColor);
            Pen line_dark_pen = new Pen(
                Color.FromArgb((console.EdgeAVGColor.R + console.EdgeMeterBackgroundColor.R) / 2,
                (console.EdgeAVGColor.G + console.EdgeMeterBackgroundColor.G) / 2,
                (console.EdgeAVGColor.B + console.EdgeMeterBackgroundColor.B) / 2));

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawLine(line_dark_pen, pixel_x - 3, 0, pixel_x - 3, H);
            g.DrawLine(line_dark_pen, pixel_x - 2, 0, pixel_x - 2, H);
            g.DrawLine(line_pen, pixel_x - 1, 0, pixel_x - 1, H);
            g.DrawLine(line_pen, pixel_x, 0, pixel_x, H);
            g.DrawLine(line_pen, pixel_x + 1, 0, pixel_x + 1, H);
            g.DrawLine(line_dark_pen, pixel_x + 2, 0, pixel_x + 2, H);
            g.DrawLine(line_dark_pen, pixel_x + 3, 0, pixel_x + 3, H);
            g.InterpolationMode = InterpolationMode.Default;
            g.SmoothingMode = SmoothingMode.Default;

            if (num > 1.17f)
            {
                SolidBrush light_brush = new SolidBrush(console.EdgeAVGColor);

                g.FillRectangle(light_brush, W - 54, H / 2 - 3, 20, 3);
                g.FillRectangle(light_brush, W - 54, H / 2, 20, 3);
                Point[] points = { new Point(W - 32, H / 2), new Point(W - 38, H / 2 - 7), new Point(W - 38, H / 2) };
                g.FillPolygon(light_brush, points);
                Point[] points2 = { new Point(W - 32, H / 2), new Point(W - 38, H / 2 + 7), new Point(W - 38, H / 2) };
                g.FillPolygon(light_brush, points2);
            }
        }

        private void btnPrintReport_Click(object sender, System.EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int V = 80;
            string text = "PA Serial Number: " + FWCEEPROM.SerialToString(FWCEEPROM.PASerial)
                + "  Date: " + DateTime.Today.ToShortDateString()
                + "  Time: " + DateTime.Now.ToString("HH:mm:ss")
                + "  Tech: " + txtTech.Text + "\n\n";

            text += "\n" + test_pa_bias + "\n";
            text += test_pa_power + "\n";
            text += test_pa_swr + "\n";

            text += "\n" + test_io_xvrx + "\n";
            text += test_io_rx1inout + "\n";
            text += test_io_txmon + "\n";

            e.Graphics.DrawString(text,
                new Font("Lucida Console", 11), Brushes.Black, 80, V);
        }

        #endregion

        #region PA Tests

        #region PA Bias

        private void btnPABias_Click(object sender, System.EventArgs e)
        {
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnPABias.BackColor = console.ButtonSelectedColor;
            progress = new Progress("Calibrate PA Bias");
            Thread t = new Thread(new ThreadStart(CalFWCPABias));
            t.Name = "Cal PA Bias Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            progress.Show();
        }

        private string test_pa_bias = "PA Bias Test: Not Run";
        public void CalFWCPABias()
        {

            bool b = console.CalibratePABias(progress, (float)udBiasDriverTarget.Value, (float)udBiasFinalTarget.Value, 0.05f, 0);

            if (b)
            {
                btnPABias.BackColor = Color.Green;
                test_pa_bias = "PA Bias Test: Passed";
            }
            else
            {
                btnPABias.BackColor = Color.Red;
                test_pa_bias = "PA Bias Test: Failed";
            }
            toolTip1.SetToolTip(btnPABias, test_pa_bias);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += "\\pa_bias.csv";
            bool file_exists = File.Exists(path);
            StreamWriter writer = new StreamWriter(path, true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version, Q2 Coarse, Q2 Fine, Q3 Coarse, Q3 Fine, Q4 Coarse, Q4 Fine, Q1 Coarse, Q1 Fine,");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < 8; i++)
                writer.Write(console.pa_bias_table[0][i].ToString() + ",");
            writer.WriteLine(b.ToString());
            writer.Close();

            if (b)
            {
                lstDebug.Items.Insert(0, "Saving Bias data to EEPROM...");
                byte checksum;
                FWCEEPROM.WritePABias(console.pa_bias_table, out checksum);
                console.pa_bias_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving Bias data to EEPROM...done";
            }

            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetFan(false);
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
        }

        #endregion

        #region Null Bridge

        private void btnNullBridge_Click(object sender, System.EventArgs e)
        {
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnNullBridge.BackColor = console.ButtonSelectedColor;
            progress = new Progress("Null PA Bridge");
            Thread t = new Thread(new ThreadStart(NullBridge));
            t.Name = "Null PA Bridge Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            progress.Show();
        }

        private float bridge_null_volts = 0.0f;
        private void NullBridge()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to run Null Bridge.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				progress.Hide();
				grpPA.Enabled = true;
				grpIO.Enabled = true;
				return;
			}*/

            if (console.RXOnly)
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNullBridge.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Invalid COM Port selection.  A valid COM port connected to a PowerMaster is required.",
                    "Invalid COM Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNullBridge.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text,ckPM2.Checked); // ke9ns add .212
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for Power Master",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnNullBridge.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnNullBridge.BackColor = Color.Red;
                return;
            }

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] pm_trim = { 1.04f, 1.03f, 1.03f, 1.03f, 1.02f, 1.01f, 1.01f, 1.0f, 1.0f, 1.0f, 0.9f };
            try
            {
                string pm_file_path = Path.Combine(common_data_path, "powermaster.txt"); // private string common_data_path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\FlexRadio Systems\\PowerSDR\\";
                // ke9ns: // common_data_path = C:\ProgramData\FlexRadio Systems\PowerSDR\

                StreamReader reader = new StreamReader(pm_file_path);
                // Ke9ns: this is what is inside the powermaster.txt file in the folder: C:\ProgramData\FlexRadio Systems\PowerSDR\
                // correction values are finalized at 1 + (correction * .01)
            //S/N: 1497
            //160m: -3
            //80m: -3
            //60m: -3
            //40m: -3
            //30m: -3
            //20m: -2
            //17m: -2
            //15m: -1
            //12m: 3
            //10m: -3
            //6m:-7
            //COM1

                string temp = reader.ReadLine();

                int start = temp.IndexOf(":") + 1;
                int length = temp.Length - start;
                lstDebug.Items.Insert(0, "PowerMaster S/N: " + temp.Substring(start, length).Trim());

                for (int i = 0; i < 11; i++)
                {
                    temp = reader.ReadLine();
                    start = temp.IndexOf(":") + 1;
                    length = temp.Length - start;
                    pm_trim[i] = 1.0f + 0.01f * float.Parse(temp.Substring(start, length).Trim());
                    lstDebug.Items.Insert(0, BandToString(bands[i]) + ": " + pm_trim[i].ToString("f2"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading Array Solutions Power Master calibration file.  Using defaults.");
            }

            double vfoa = console.VFOAFreq;
            console.VFOAFreq = 28.1;

            double vfob = console.VFOBFreq;
            console.VFOBFreq = 28.1;

            console.FullDuplex = true;

            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(true);
            //Thread.Sleep(50);
            FWC.SetTR(true);
            //Thread.Sleep(50);
            FWC.SetSig(true);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(true);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);

            FWC.SetTXAnt(1);
            //Thread.Sleep(50);

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            int power = console.PWR;
            console.PWR = 100;

            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetAmpTX1(false);

            FWC.SetMOX(true);
            console.TXCal = true;
            //Thread.Sleep(50);
            Audio.TXInputSignal = Audio.SignalSource.SINE;
            double scale = Audio.SourceScale;
            Audio.SourceScale = 1.0;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    Audio.RadioVolume = 0.05;
                    break;
                case Model.FLEX3000:
                    Audio.RadioVolume = 0.075;
                    break;
            }

            grpBridgeNull.Visible = true; // ke9ns this is the picnullbridge mV meter
            Thread.Sleep(1500);

            lstDebug.Items.Insert(0, "PM: " + (pm.Watts * pm_trim[10]).ToString("f1"));
            if (pm.Watts * pm_trim[10] < 10.0f)
            {
                FWC.SetMOX(false);
                //Thread.Sleep(50);
                progress.Hide();
                MessageBox.Show("PA Bridge Null Error: Low Power from PowerMaster.\nCheck COM port and try again.",
                    "PA Bridge Null: Low Power",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                pm.Close();
            }
            else
            {
                pm.Close();
                while (progress.Visible)
                {
                    bridge_null_volts = bridge_null_volts * 0.8f + console.ReadRefPowerVolts(1) * 0.2f; // ke9ns: read value from the Flex radio here
                    picNullBridge.Invalidate();
                    Thread.Sleep(100);
                }
            }

            grpBridgeNull.Visible = false;

            FWC.SetMOX(false);
            console.TXCal = false;
            //Thread.Sleep(50);
            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(false);
            //Thread.Sleep(50);
            FWC.SetTR(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            Audio.SourceScale = scale;
            console.RX1DSPMode = dsp_mode;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            console.PWR = power;
            console.FullDuplex = false;

            if (console.CurrentModel == Model.FLEX3000)
            {
                FWC.SetFan(false);
                if (console.CurrentModel == Model.FLEX3000)
                    FWC.SetAmpTX1(true);
            }
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
            btnNullBridge.BackColor = Color.Green;
        }

        #endregion

        #region Cal Power

        private string test_pa_power = "PA Power Test: Not Run";
        private void btnPAPower_Click(object sender, System.EventArgs e)
        {
            if (!IsRegionUS())
            {
                if (MessageBox.Show("This radio must be TURFed for the US region for this verification test to run successfully on all bands.\r\n\r\n" +
                    "Do you want to terminate this verification test?",
                    "Power Test Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes) return;
            }

            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnPAPower.BackColor = console.ButtonSelectedColor;
            progress = new Progress("Calibrate PA Power");

            Thread t = new Thread(new ThreadStart(CalPAPower));
            t.Name = "PA Power Cal Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.AboveNormal;
            t.Start();

            if (console.PowerOn)
                progress.Show();
        }

        private void CalPAPower()
        {
            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.RXOnly)
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnPAPower.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Invalid COM Port selected.  A valid COM Port connected to a PowerMaster is required.",
                    "Error: Invalid COM Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRunPACal.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text, ckPM2.Checked);
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for Power Master",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            bool leveler = console.dsp.GetDSPTX(0).TXLevelerOn;
            console.dsp.GetDSPTX(0).TXLevelerOn = false;

            bool[] run = new bool[11];
            run[0] = ck160.Checked;
            run[1] = ck80.Checked;
            run[2] = ck60.Checked;
            run[3] = ck40.Checked;
            run[4] = ck30.Checked;
            run[5] = ck20.Checked;
            run[6] = ck17.Checked;
            run[7] = ck15.Checked;
            run[8] = ck12.Checked;
            run[9] = ck10.Checked;
            run[10] = ck6.Checked;
            console.CalibratePAGain2(progress, run, true);

            console.dsp.GetDSPTX(0).TXLevelerOn = leveler;

            test_pa_power = "PA Power Test: Passed";
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            bool b = true;
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (console.power_table[(int)bands[i]][j] == console.power_table[(int)bands[i]][j - 1])
                    {
                        if (!test_pa_power.StartsWith("PA Power Test: Failed ("))
                            test_pa_power = "PA Power Test: Failed (";
                        test_pa_power += BandToString(bands[i]) + ", ";
                        b = false;
                        j = 10;
                    }
                }
            }

            if (test_pa_power.StartsWith("PA Power Test: Failed ("))
                test_pa_power = test_pa_power.Substring(0, test_pa_power.Length - 2) + ")";
            toolTip1.SetToolTip(btnPAPower, test_pa_power);

            if (b) btnPAPower.BackColor = Color.Green;
            else btnPAPower.BackColor = Color.Red;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\pa_power.csv");
            StreamWriter writer = new StreamWriter(path + "\\pa_power.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version, "
                                 + "160-1, 160-2, 160-5, 160-10, 160-20, 160-30, 160-40, 160-50, 160-60, 160-70, 160-80, 160-90, 160-100,"
                                 + "80-1, 80-2, 80-5, 80-10, 80-20, 80-30, 80-40, 80-50, 80-60, 80-70, 80-80, 80-90, 80-100,"
                                 + "60-1, 60-2, 60-5, 60-10, 60-20, 60-30, 60-40, 60-50, 60-60, 60-70, 60-80, 60-90, 60-100,"
                                 + "40-1, 40-2, 40-5, 40-10, 40-20, 40-30, 40-40, 40-50, 40-60, 40-70, 40-80, 40-90, 40-100,"
                                 + "30-1, 30-2, 30-5, 30-10, 30-20, 30-30, 30-40, 30-50, 30-60, 30-70, 30-80, 30-90, 30-100,"
                                 + "20-1, 20-2, 20-5, 20-10, 20-20, 20-30, 20-40, 20-50, 20-60, 20-70, 20-80, 20-90, 20-100,"
                                 + "17-1, 17-2, 17-5, 17-10, 17-20, 17-30, 17-40, 17-50, 17-60, 17-70, 17-80, 17-90, 17-100,"
                                 + "15-1, 15-2, 15-5, 15-10, 15-20, 15-30, 15-40, 15-50, 15-60, 15-70, 15-80, 15-90, 15-100,"
                                 + "12-1, 12-2, 12-5, 12-10, 12-20, 12-30, 12-40, 12-50, 12-60, 12-70, 12-80, 12-90, 12-100,"
                                 + "10-1, 10-2, 10-5, 10-10, 10-20, 10-30, 10-40, 10-50, 10-60, 10-70, 10-80, 10-90, 10-100,"
                                 + "6-1, 6-2, 6-5, 6-10, 6-20, 6-30, 6-40, 6-50, 6-60, 6-70, 6-80, 6-90, 6-100");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 13; j++)
                    writer.Write(console.power_table[(int)bands[i]][j].ToString("f5") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\PA Power";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            writer = new StreamWriter(path + "\\pa_power_" + FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ".csv");
            writer.WriteLine("Band, 1w, 2w, 5w, 10w, 20w, 30w, 40w, 50w, 60w, 70w, 80w, 90w, 100w");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                for (int j = 0; j < 13; j++)
                    writer.Write(console.power_table[(int)bands[i]][j].ToString("f5") + ",");
                writer.WriteLine("");
            }
            writer.Close();

            lstDebug.Items.Insert(0, "Saving Power data to EEPROM...");
            byte checksum;
            FWCEEPROM.WritePAPower(console.power_table, out checksum);
            console.pa_power_checksum = checksum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving Power data to EEPROM...done";

            if (File.Exists(console.AppDataPath + "Compression.xls"))
            {
                Process.Start(console.AppDataPath + "power.csv");
                Thread.Sleep(100);
                Process.Start(console.AppDataPath + "Compression.xls");
            }

            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetFan(false);
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
        }

        #endregion

        #region Cal SWR

        private string test_pa_swr = "PA SWR Test: Not Run";
        private void btnPASWR_Click(object sender, System.EventArgs e)
        {
            if (!IsRegionUS())
            {
                if (MessageBox.Show("This radio must be TURFed for the US region for this calibration procedure to run successfully on all bands.\r\n\r\n" +
                    "Do you want to terminate this calibration procedure?",
                    "PA SWR Calibration Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes) return;
            }

            progress = new Progress("Cal SWR");
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnPASWR.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CalPASWR));
            t.Name = "Cal SWR Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            progress.Show();
        }

        private void CalPASWR()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to calibrate SWR.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				progress.Hide();
				btnPASWR.BackColor = Color.Red;
				return;
			}*/

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.RXOnly)
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnPASWR.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Invalid COM Port selected.  A valid COM Port connected to a PowerMaster is required.",
                    "Error: Invalid COM Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRunPACal.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text, ckPM2.Checked);
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for Power Master",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfoa = console.VFOAFreq;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            console.FullDuplex = false;
            console.VFOSplit = false;

            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = num_bands * 15;

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    console.VFOAFreq = band_freqs[i];

                    FWCAnt tx_ant = console.TXAnt;
                    console.TXAnt = FWCAnt.ANT2;
                    for (int j = 0; j < 5; j++)
                    {
                        Thread.Sleep(50);
                        if (!progress.Visible) goto end2;
                        progress.SetPercent(counter++ / (float)total_counts);
                    }

                    console.swr_table[(int)bands[i]] = 1.0f;
                    console.TUN = true;
                    int old_tun_pwr = console.PWR;
                    console.PWR = 50;
                    for (int j = 0; j < 10; j++)
                    {
                        Thread.Sleep(50);
                        if (!progress.Visible) goto end1;
                        progress.SetPercent(counter++ / (float)total_counts);
                    }

                    double f = console.ReadFwdPower(3);
                    double r = console.ReadRefPower(3);
                    Debug.WriteLine("f: " + f.ToString("f1") + "  r: " + r.ToString("f1"));

                    float alpha = (float)(f / r / 9);

                    console.swr_table[(int)bands[i]] = (float)Math.Round(alpha, 4);

                    if (alpha < 0.1f || alpha > 5.0f)
                        lstDebug.Items.Insert(0, "SWR Test Failed - " + BandToString(bands[i]) + ": " + alpha.ToString("f4"));
                    else lstDebug.Items.Insert(0, "SWR Test Passed - " + BandToString(bands[i]) + ": " + alpha.ToString("f4"));

                    end1:
                    console.PWR = old_tun_pwr;
                    console.TUN = false;
                end2:
                    console.TXAnt = tx_ant;
                    if (!progress.Visible) goto end3;
                }
            }
        end3:
            console.VFOAFreq = 0.590;
            console.VFOAFreq = vfoa;
            console.RX1DSPMode = dsp_mode;
            console.FullDuplex = false;

            bool fail = false;
            for (int i = 0; i < 11; i++)
            {
                if (console.swr_table[(int)bands[i]] < 0.1f ||
                    console.swr_table[(int)bands[i]] > 5.0f)
                {
                    btnPASWR.BackColor = Color.Red;
                    fail = true;
                }
            }

            if (!fail) btnPASWR.BackColor = Color.Green;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\pa_swr.csv");
            StreamWriter writer = new StreamWriter(path + "\\pa_swr.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version, "
                                  + "160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m, 6m");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
                writer.Write(console.swr_table[(int)bands[i]].ToString("f5") + ",");
            writer.WriteLine("");
            writer.Close();

            lstDebug.Items.Insert(0, "Saving SWR data to EEPROM...");
            byte checksum;
            FWCEEPROM.WritePASWR(console.swr_table, out checksum);
            console.pa_swr_checksum = checksum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving SWR data to EEPROM...done";

            progress.Hide();
            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetFan(false);
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
        }

        #endregion

        #region Verify

        private void btnPAVerify_Click(object sender, System.EventArgs e)
        {
            if (!IsRegionUS())
            {
                if (MessageBox.Show("This radio must be TURFed for the US region for this verification test to run successfully on all bands.\r\n\r\n" +
                    "Do you want to terminate this verification test?",
                    "Power Verify Test Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes) return;
            }

            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnPAVerify.BackColor = console.ButtonSelectedColor;
            progress = new Progress("Verify PA Power");
            Thread t = new Thread(new ThreadStart(VerifyPA));
            t.Name = "Verify PA Power";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            progress.Show();
        }

        private string test_pa_verify = "Verify PA Test: Not Run";
        private void VerifyPA()
        {
            //float tol = 0.05f; // 5%

            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to Verify PA.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				progress.Hide();
				btnPAVerify.BackColor = Color.Red;
				return;
			}*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.RXOnly)
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnPAVerify.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Invalid COM Port selection.  A valid COM port connected to a PowerMaster is required.",
                    "Error: Invalid COM Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnPAVerify.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            bool leveler = console.dsp.GetDSPTX(0).TXLevelerOn;
            console.dsp.GetDSPTX(0).TXLevelerOn = false;

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text, ckPM2.Checked);
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for Power Master",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnPAVerify.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnPAVerify.BackColor = Color.Red;
                return;
            }

            Band[] bands = { Band.B6M, Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M };
            float[] band_freqs = { 50.11f, 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f };
            int[] targets = { 10, 100 };
            float[][] bridge_power = new float[bands.Length][];
            for (int i = 0; i < bands.Length; i++)
                bridge_power[i] = new float[targets.Length];
            float[][] pm_power = new float[bands.Length][];
            for (int i = 0; i < bands.Length; i++)
                pm_power[i] = new float[targets.Length];

            float[] pm_trim = { 1.04f, 1.03f, 1.03f, 1.03f, 1.02f, 1.01f, 1.01f, 1.0f, 1.0f, 1.0f, 0.9f };
            try
            {
                string pm_file_path = Path.Combine(common_data_path, "powermaster.txt");
                StreamReader reader = new StreamReader(pm_file_path);
                string temp = reader.ReadLine();

                int start = temp.IndexOf(":") + 1;
                int length = temp.Length - start;
                lstDebug.Items.Insert(0, "PowerMaster S/N: " + temp.Substring(start, length).Trim());

                for (int i = 0; i < 11; i++)
                {
                    temp = reader.ReadLine();
                    start = temp.IndexOf(":") + 1;
                    length = temp.Length - start;
                    int index = i + 1;
                    if (index == 11) index = 0;
                    pm_trim[index] = 1.0f + 0.01f * float.Parse(temp.Substring(start, length).Trim());
                    lstDebug.Items.Insert(0, BandToString(bands[index]) + ": " + pm_trim[i].ToString("f2"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading Array Solutions Power Master calibration file.  Using defaults.");
            }

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;

            int tune_power = console.TunePower;
            console.TunePower = 100;

            MeterTXMode tx_meter = console.CurrentMeterTXMode;
            console.CurrentMeterTXMode = MeterTXMode.FORWARD_POWER;

            progress.SetPercent(0.0f);

            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = num_bands * targets.Length * 40;

            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    FWC.SetAmpTX1(false);
                    FWC.SetRCATX1(false);
                    break;
            }

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    switch (console.CurrentModel)
                    {
                        case Model.FLEX5000:
                            FWC.SetTXAnt(1);
                            break;
                    }
                    //Thread.Sleep(50);

                    DSPMode dsp_mode = console.RX1DSPMode;
                    console.RX1DSPMode = DSPMode.USB;

                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    for (int k = 0; k < targets.Length; k++)
                    {
                        console.TunePower = targets[k];
                        console.TUN = true;
                        for (int j = 0; j < 10; j++)
                        {
                            progress.SetPercent(++counter / (float)total_counts);
                            if (!progress.Visible) goto end;
                            Thread.Sleep(50);
                        }

                        pm_power[i][k] = pm.Watts * pm_trim[i];
                        float current = console.ReadFinalBias(3, false);

                        //bridge_power[i][k] = console.NewMeterData;
                        console.TUN = false;
                        for (int j = 0; j < 20; j++)
                        {
                            progress.SetPercent(++counter / (float)total_counts);
                            if (!progress.Visible) goto end;
                            Thread.Sleep(50);
                        }

                        if ((pm_power[i][k] < targets[k] - 5.0f) ||
                            (pm_power[i][k] > targets[k] + 20.0f))
                        {
                            btnPAVerify.BackColor = Color.Red;
                            if (!test_pa_verify.StartsWith("PA Verify Test: Failed ("))
                                test_pa_verify = "PA Verify Test: Failed (";
                            test_pa_verify += BandToString(bands[i]) + "-" + targets[k] + ", ";
                            lstDebug.Items.Insert(0, "PA Verify - " + BandToString(bands[i]) + ": Failed (" +
                                targets[k] + ", " + pm_power[i][k].ToString("f1") + ", " + current.ToString("f1") + "A)");
                        }
                        else
                        {
                            lstDebug.Items.Insert(0, "PA Verify - " + BandToString(bands[i]) + ": Passed (" +
                                targets[k] + ", " + pm_power[i][k].ToString("f1") + ", " + current.ToString("f1") + "A)");
                        }
                    }
                    console.RX1DSPMode = dsp_mode;
                }
            }

        end:
            console.TUN = false;
            console.VFOAFreq = 0.590;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            console.TunePower = tune_power;
            console.FullDuplex = false;

            console.CurrentMeterTXMode = tx_meter;

            switch (console.CurrentModel)
            {
                case Model.FLEX3000:
                    FWC.SetAmpTX1(true);
                    break;
            }

            try
            {
                pm.Close();
            }
            catch (Exception) { }

            console.dsp.GetDSPTX(0).TXLevelerOn = leveler;

            if (test_pa_verify.StartsWith("PA Verify Test: Failed ("))
                test_pa_verify = test_pa_verify.Substring(0, test_pa_verify.Length - 2) + ")";
            toolTip1.SetToolTip(btnPAVerify, test_pa_verify);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\pa_verify.csv");
            StreamWriter writer = new StreamWriter(path + "\\pa_verify.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version,"
                                  + "Bridge 6-10, PM 6-10, Bridge 6-100, PM 6-100,"
                                  + "Bridge 160-10, PM 160-10, Bridge 160-100, PM 160-100,"
                                  + "Bridge 80-10, PM 80-10, Bridge 80-100, PM 80-100,"
                                  + "Bridge 60-10, PM 60-10, Bridge 60-100, PM 60-100,"
                                  + "Bridge 40-10, PM 40-10, Bridge 40-100, PM 40-100,"
                                  + "Bridge 30-10, PM 30-10, Bridge 30-100, PM 30-100,"
                                  + "Bridge 20-10, PM 20-10, Bridge 20-100, PM 20-100,"
                                  + "Bridge 17-10, PM 17-10, Bridge 17-100, PM 17-100,"
                                  + "Bridge 15-10, PM 15-10, Bridge 15-100, PM 15-100,"
                                  + "Bridge 12-10, PM 12-10, Bridge 12-100, PM 12-100,"
                                  + "Bridge 10-10, PM 10-10, Bridge 10-100, PM 10-100,"
                                 );
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < targets.Length; j++)
                {
                    writer.Write(bridge_power[i][j].ToString("f1") + ",");
                    writer.Write(pm_power[i][j].ToString("f1") + ",");
                }
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\PA Verify";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            writer = new StreamWriter(path + "\\pa_bridge_" + FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ".csv");
            writer.WriteLine("Band, Bridge 10w, PM 10w, Bridge 100w, PM 100w");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                for (int j = 0; j < targets.Length; j++)
                {
                    writer.Write(bridge_power[i][j].ToString("f1") + ",");
                    writer.Write(pm_power[i][j].ToString("f1") + ",");
                }
                writer.WriteLine("");
            }
            writer.Close();

            string eeprom_file_path = Path.Combine(common_data_path, "EEPROM.exe");
            if (File.Exists(eeprom_file_path))
            {
                DialogResult dr = MessageBox.Show("Would you like to save EEPROM data to a file now?",
                    "Save EEPROM Data?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                    Process.Start(eeprom_file_path);
            }

            progress.Hide();
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
        }

        #endregion

        #region Run PA Cal

        string test_pa_bridge = "PA Bridge: Not Run";
        private void btnRunPACal_Click(object sender, System.EventArgs e)
        {
            if (!IsRegionUS())
            {
                if (MessageBox.Show("This radio must be TURFed for the US region for this calibration procedure to run successfully on all bands.\r\n\r\n" +
                    "Do you want to terminate this calibration procedure?",
                    "PA Power Calibration Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes) return;
            }

            console.PowerOn = true;
            btnRunPACal.Enabled = false;
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            progress = new Progress("Cal PA Bridge/Power/SWR");
            btnRunPACal.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(PARunCal));
            t.Name = "Run PA Cal Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            progress.Show();
        }

        private void PARunCal()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to calibrate PA.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				progress.Hide();
				btnRunPACal.BackColor = Color.Red;
				return;
			}*/


            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.RXOnly)
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRunPACal.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Invalid COM Port selected.  A valid COM Port connected to a PowerMaster is required.",
                    "Error: Invalid COM Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRunPACal.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text, ckPM2.Checked);
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for Power Master",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            bool leveler = console.dsp.GetDSPTX(0).TXLevelerOn;
            console.dsp.GetDSPTX(0).TXLevelerOn = false;

            Band[] bands = { Band.B6M, Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M };
            float[] band_freqs = { 50.11f, 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f };
            float[] targets = { 1.0f, 2.0f, 5.0f, 10.0f, 20.0f, 30.0f, 40.0f, 50.0f, 60.0f, 70.0f, 80.0f, 90.0f, 100.0f };

            float[] pm_trim = { 1.04f, 1.03f, 1.03f, 1.03f, 1.02f, 1.01f, 1.01f, 1.0f, 1.0f, 1.0f, 0.9f };
            try
            {
                string pm_file_path = Path.Combine(common_data_path, "powermaster.txt");
                StreamReader reader = new StreamReader(pm_file_path);
                string temp = reader.ReadLine();

                int start = temp.IndexOf(":") + 1;
                int length = temp.Length - start;
                lstDebug.Items.Insert(0, "PowerMaster S/N: " + temp.Substring(start, length).Trim());

                for (int i = 0; i < 11; i++)
                {
                    temp = reader.ReadLine();
                    start = temp.IndexOf(":") + 1;
                    length = temp.Length - start;
                    int index = i + 1;
                    if (index == 11) index = 0;
                    pm_trim[index] = 1.0f + 0.01f * float.Parse(temp.Substring(start, length).Trim());
                    lstDebug.Items.Insert(0, BandToString(bands[index]) + ": " + pm_trim[index].ToString("f2"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading Array Solutions Power Master calibration file.  Using defaults.");
            }

            double vfoa = console.VFOAFreq;
            double vfob = console.VFOBFreq;
            FWCAnt tx_ant = console.TXAnt;
            console.TXCal = true;

            console.FullDuplex = true;

            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(true);
            //Thread.Sleep(50);
            FWC.SetTR(true);
            //Thread.Sleep(50);
            FWC.SetSig(true);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(true);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);
            if (console.CurrentModel == Model.FLEX3000)
            {
                FWC.SetAmpTX1(false);
                FWC.SetRCATX1(false);
            }

            progress.SetPercent(0.0f);

            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = num_bands * 28;

            for (int i = 0; i < band_freqs.Length; i++) // main loop
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    for (int ii = 0; ii < 13; ii++)
                    {
                        console.power_table[(int)bands[i]][ii] = 0.0f;
                    }
                    for (int ii = 0; ii < 6; ii++)
                    {
                        console.pa_bridge_table[(int)bands[i]][ii] = 0.0f;
                    }
                    console.swr_table[(int)bands[i]] = 0.0f;

                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    console.FullDuplex = false;
                    console.FullDuplex = true;
                    FWC.SetTXAnt(1);
                    //Thread.Sleep(50);

                    DSPMode dsp_mode = console.RX1DSPMode;
                    console.RX1DSPMode = DSPMode.USB;

                    console.VFOAFreq = band_freqs[i];
                    console.VFOBFreq = band_freqs[i];

                    Audio.RadioVolume = 0.00;

                    for (int j = 0; j < 10; j++)
                    {
                        if (!progress.Visible) goto end;
                        Thread.Sleep(50);
                    }

                    Audio.RadioVolume = 0.04;
                    double last_watts = 0.0;
                    double last_volts = 0.04;

                    for (int k = 0; k < targets.Length; k++)
                    {
                        int try_again_count = 0;
                        int zero_count = 0;
                        float tol = 1.0f;
                        if (targets[k] < 60.0f) tol = 0.5f;
                        if (targets[k] < 15.0f) tol = 0.2f;
                        /*if(targets[k] == 100.0f && console.CurrentModel == Model.FLEX3000 && bands[i] == Band.B6M)
							tol = 5.0f;*/

                        FWC.SetMOX(true);
                        //Thread.Sleep(50);
                        Audio.TXInputSignal = Audio.SignalSource.SINE;
                        Audio.SourceScale = 1.0;

                        float p = 0.0f;

                        while (Math.Abs(targets[k] - p) > tol)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (!progress.Visible) goto end;
                                Thread.Sleep(50);
                            }

                            p = pm.Watts * pm_trim[i];
                            Debug.WriteLine("p: " + p.ToString("f1"));

                            if (p == 0.0f)
                            {
                                zero_count++;
                                if (zero_count > 2)
                                {
                                    FWC.SetMOX(false);
                                    //Thread.Sleep(50);
                                    MessageBox.Show("No power reading from PowerMaster.  Check cables and try again.");
                                    progress.Text = "";
                                    goto end;
                                }
                                p = 0.01f;
                            }

                            if (Math.Abs(targets[k] - p) > tol)
                            {
                                if (try_again_count++ == 20) // poop out
                                {
                                    FWC.SetMOX(false);
                                    //Thread.Sleep(50);
                                    for (int kk = k; kk < 12; kk++)
                                        lstDebug.Items.Insert(0, "Bridge/Power - " + BandToString(bands[i]) + " (" + targets[kk].ToString("f0") + "w): Failed");
                                    btnRunPACal.BackColor = Color.Red;
                                    k = 12;
                                    p = targets[k];
                                    if (k > 0)
                                        Audio.RadioVolume = console.power_table[(int)bands[i]][k - 1];
                                }
                                /*else if (targets[k] <= 10)
                                {
                                    double x1 = Math.Pow(last_volts, 2.0);
                                    double x2 = Math.Pow(Audio.RadioVolume, 2.0);
                                    double y1 = last_watts;
                                    double y2 = p;
                                    double next_volts = 0.0;

                                    lstDebug.Items.Insert(0, last_volts.ToString("f5") + " " + Audio.RadioVolume.ToString("f5") + " " + y1.ToString("f1") + " " + y2.ToString("f1"));

                                    if (y2 == y1)
                                    {
                                        if (y1 == 0.0) next_volts = last_volts * 2;
                                        else next_volts += (Audio.RadioVolume - last_volts);
                                    }
                                    else if (x2 == x1)
                                    {
                                        FWC.SetMOX(false);
                                        //Thread.Sleep(50);
                                        MessageBox.Show("Error in Bridge Cal");
                                        progress.Text = "";
                                        goto end;
                                    }
                                    else
                                    {
                                        double a = (y2 - y1) / (x2 - x1);

                                        if (a <= 0.0)
                                        {
                                            next_volts += (Audio.RadioVolume - last_volts);
                                        }
                                        else
                                        {
                                            double b = y2 - a * x2;

                                            //lstDebug.Items.Insert(0, a.ToString("f5")+" "+b.ToString("f5"));

                                            next_volts = Math.Sqrt((targets[k] - b) / a);
                                            if (double.IsNaN(next_volts))
                                            {
                                                // error out -- two times through the cap will do this
                                                //FWC.SetMOX(false);
                                                //MessageBox.Show("NaN Error in Bridge Cal");
                                                //progress.Text = "";
                                                //goto end;
                                                next_volts += (Audio.RadioVolume - last_volts);
                                            }
                                        }
                                    }

                                    last_volts = Audio.RadioVolume;
                                    if (next_volts / last_volts > 2.0)
                                        next_volts = last_volts * 2.0;
                                    else if (next_volts / last_volts < 0.5)
                                        next_volts = last_volts * 0.5;
                                    Audio.RadioVolume = Math.Min(0.5, next_volts); // 0.5 cap where 0.83 is max before overloading QSE
                                }*/
                                else
                                {
                                    double next_volts = Audio.RadioVolume * Math.Sqrt(targets[k] / p);
                                    if (next_volts / last_volts > 2.0)
                                        next_volts = last_volts * 2.0;
                                    else if (next_volts / last_volts < 0.5)
                                        next_volts = last_volts * 0.5;

                                    lstDebug.Items.Insert(0, last_volts.ToString("f5") + " " + Audio.RadioVolume.ToString("f5") + " " +
                                        last_watts.ToString("f2") + " " + p.ToString("f2"));

                                    last_volts = Audio.RadioVolume;
                                    Audio.RadioVolume = next_volts;
                                }

                                if (p > 50.0)
                                {
                                    FWC.SetMOX(false);
                                    //Thread.Sleep(50);
                                    for (int j = 0; j < 20; j++)
                                    {
                                        if (!progress.Visible)
                                        {
                                            progress.Text = "";
                                            goto end;
                                        }
                                        Thread.Sleep(50);
                                    }
                                    FWC.SetMOX(true);
                                    //Thread.Sleep(50);
                                }
                            }

                            last_watts = p;
                        }

                        progress.SetPercent(++counter / (float)total_counts);

                        bool fail = false;
                        if (k < 5 || k == 11)
                        {
                            int kk = k;
                            if (kk == 11) kk = 5;
                            float v1 = console.ReadFwdPowerVolts(3);
                            console.pa_bridge_table[(int)bands[i]][kk] = (float)Math.Round(v1, 4);

                            if (console.pa_bridge_table[(int)bands[i]][kk] == 0.0f || k > 0 && console.pa_bridge_table[(int)bands[i]][kk] <= console.pa_bridge_table[(int)bands[i]][kk - 1])
                            {
                                btnRunPACal.BackColor = Color.Red;
                                fail = true;
                            }
                        }
                        console.power_table[(int)bands[i]][k] = (float)Math.Round(Audio.RadioVolume, 4);

                        float current = console.ReadFinalBias(3, false);

                        if (k == 0 || (console.power_table[(int)bands[i]][k] > console.power_table[(int)bands[i]][k - 1] && !fail))
                        {
                            lstDebug.Items.Insert(0, "Power - " + BandToString(bands[i]) + " (" + targets[k].ToString("f0") + "w): Passed (" +
                                current.ToString("f1") + "A, " + Audio.RadioVolume.ToString("f3") + ")");
                        }
                        else
                        {
                            lstDebug.Items.Insert(0, "Power - " + BandToString(bands[i]) + " (" + targets[k].ToString("f0") + "w): Failed (" +
                                current.ToString("f1") + "A, " + Audio.RadioVolume.ToString("f3") + ")");
                            btnRunPACal.BackColor = Color.Red;
                        }
                    }

                    // do SWR cal here
                    FWC.SetMOX(false);
                    console.MOX = false;
                    Thread.Sleep(100);

                    switch (console.CurrentModel)
                    {
                        case Model.FLEX5000:
                            //console.TXAnt = FWCAnt.ANT2;
                            FWC.SetTXAnt(2);
                            break;
                        case Model.FLEX3000:
                            FWC.SetRCATX1(true);
                            break;
                    }
                    //Thread.Sleep(50);

                    for (int j = 0; j < 5; j++)
                    {
                        Thread.Sleep(50);
                        if (!progress.Visible) goto end2;
                        progress.SetPercent(counter++ / (float)total_counts);
                    }

                    console.swr_table[(int)bands[i]] = 1.0f;
                    console.TUN = true;
                    int old_tun_pwr = console.PWR;
                    console.PWR = 50;
                    for (int j = 0; j < 10; j++)
                    {
                        Thread.Sleep(50);
                        if (!progress.Visible) goto end1;
                        progress.SetPercent(counter++ / (float)total_counts);
                    }

                    double f = console.ReadFwdPower(3);
                    double r = console.ReadRefPower(3);
                    Debug.WriteLine("f: " + f.ToString("f1") + "  r: " + r.ToString("f1"));

                    float alpha = (float)(f / r / 9);

                    console.swr_table[(int)bands[i]] = (float)Math.Round(alpha, 4);

                    if (console.swr_table[(int)bands[i]] < 0.1f ||
                        console.swr_table[(int)bands[i]] > 5.0f)
                    {
                        btnRunPACal.BackColor = Color.Red;
                        lstDebug.Items.Insert(0, "SWR Test Failed - " + BandToString(bands[i]) + ": " + alpha.ToString("f4"));
                    }
                    else lstDebug.Items.Insert(0, "SWR Test Passed - " + BandToString(bands[i]) + ": " + alpha.ToString("f4"));

                    end1:
                    console.PWR = old_tun_pwr;
                    console.TUN = false;
                end2:
                    switch (console.CurrentModel)
                    {
                        case Model.FLEX5000:
                            //console.TXAnt = tx_ant;
                            break;
                        case Model.FLEX3000:
                            FWC.SetRCATX1(false);
                            break;
                    }
                    if (!progress.Visible) goto end;
                    console.RX1DSPMode = dsp_mode;
                }
            }
        end:
            progress.Hide();
            console.TUN = false;
            console.MOX = false;
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            console.VFOAFreq = 0.590;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;
            console.FullDuplex = false;
            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(false);
            //Thread.Sleep(50);
            FWC.SetTR(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);
            FWC.SetMOX(false);
            //Thread.Sleep(50);
            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetAmpTX1(true);

            console.TXAnt = tx_ant;
            console.TXCal = false;

            try
            {
                pm.Close();
            }
            catch (Exception) { }

            console.dsp.GetDSPTX(0).TXLevelerOn = leveler;

            // bool pass = true;
            test_pa_bridge = "PA Bridge Test: Passed";
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (console.pa_bridge_table[(int)bands[i]][j] == 0.0f)
                    {
                        // pass = false;
                        if (!test_pa_bridge.StartsWith("PA Bridge Test: Failed ("))
                            test_pa_bridge = "PA Bridge Test: Failed (";
                        test_pa_bridge += BandToString(bands[i]) + ", ";
                        j = 6;
                    }
                    else if (j != 0)
                    {
                        if (console.pa_bridge_table[(int)bands[i]][j] <= console.pa_bridge_table[(int)bands[i]][j - 1] ||
                            console.pa_bridge_table[(int)bands[i]][j] > 2.0f)
                        {
                            // pass = false;
                            if (!test_pa_bridge.StartsWith("PA Bridge Test: Failed ("))
                                test_pa_bridge = "PA Bridge Test: Failed (";
                            test_pa_bridge += BandToString(bands[i]) + ", ";
                            j = 6;
                        }
                    }
                }
            }

            /*if(pass) btnRunPACal.BackColor = Color.Green;
			else btnRunPACal.BackColor = Color.Red;*/

            if (test_pa_bridge.StartsWith("PA Bridge Test: Failed ("))
                test_pa_bridge = test_pa_bridge.Substring(0, test_pa_bridge.Length - 2) + ")";
            //toolTip1.SetToolTip(btnRunPACal, test_pa_bridge);

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\pa_bridge.csv");
            StreamWriter writer = new StreamWriter(path + "\\pa_bridge.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version,"
                                  + "6-1, 6-2, 6-5, 6-10, 6-20, 6-90,"
                                  + "160-1, 160-2, 160-5, 160-10, 160-20, 160-90,"
                                  + "80-1, 80-2, 80-5, 80-10, 80-20, 80-90,"
                                  + "60-1, 60-2, 60-5, 60-10, 60-20, 60-90,"
                                  + "40-1, 40-2, 40-5, 40-10, 40-20, 40-90,"
                                  + "30-1, 30-2, 30-5, 30-10, 30-20, 30-90,"
                                  + "20-1, 20-2, 20-5, 20-10, 20-20, 20-90,"
                                  + "17-1, 17-2, 17-5, 17-10, 17-20, 17-90,"
                                  + "15-1, 15-2, 15-5, 15-10, 15-20, 15-90,"
                                  + "12-1, 12-2, 12-5, 12-10, 12-20, 12-90,"
                                  + "10-1, 10-2, 10-5, 10-10, 10-20, 10-90,");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                    writer.Write(console.pa_bridge_table[(int)bands[i]][j].ToString("f2") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\PA Bridge";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            writer = new StreamWriter(path + "\\pa_bridge_" + FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ".csv");
            writer.WriteLine("Band, 1w, 2w, 5w, 10w, 20w, 90w");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                for (int j = 0; j < 6; j++)
                    writer.Write(console.pa_bridge_table[(int)bands[i]][j].ToString("f4") + ",");
                writer.WriteLine("");
            }
            writer.Close();

            if (progress.Text != "")
            {
                lstDebug.Items.Insert(0, "Saving Bridge data to EEPROM...");
                byte checksum;
                FWCEEPROM.WritePABridge(console.pa_bridge_table, out checksum);
                console.pa_bridge_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving Bridge data to EEPROM...done";
            }
            test_pa_power = "PA Power Test: Passed";

            // bool pwr_pass = true;
            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (console.power_table[(int)bands[i]][j] <= console.power_table[(int)bands[i]][j - 1])
                    {
                        if (!test_pa_power.StartsWith("PA Power Test: Failed ("))
                            test_pa_power = "PA Power Test: Failed (";
                        test_pa_power += BandToString(bands[i]) + ", ";
                        // pwr_pass = false;
                    }
                }
            }

            if (test_pa_power.StartsWith("PA Power Test: Failed ("))
                test_pa_power = test_pa_power.Substring(0, test_pa_power.Length - 2) + ")";
            toolTip1.SetToolTip(btnPAPower, test_pa_bridge + "\n\n" + test_pa_power);

            /*if(pass && pwr_pass) btnRunPACal.BackColor = Color.Green;
			else btnRunPACal.BackColor = Color.Red;*/

            path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            file_exists = File.Exists(path + "\\pa_power.csv");
            writer = new StreamWriter(path + "\\pa_power.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version, "
                                  + "6-1, 6-2, 6-5, 6-10, 6-20, 6-30, 6-40, 6-50, 6-60, 6-70, 6-80, 6-90, 6-100,"
                                  + "160-1, 160-2, 160-5, 160-10, 160-20, 160-30, 160-40, 160-50, 160-60, 160-70, 160-80, 160-90, 160-100,"
                                  + "80-1, 80-2, 80-5, 80-10, 80-20, 80-30, 80-40, 80-50, 80-60, 80-70, 80-80, 80-90, 80-100,"
                                  + "60-1, 60-2, 60-5, 60-10, 60-20, 60-30, 60-40, 60-50, 60-60, 60-70, 60-80, 60-90, 60-100,"
                                  + "40-1, 40-2, 40-5, 40-10, 40-20, 40-30, 40-40, 40-50, 40-60, 40-70, 40-80, 40-90, 40-100,"
                                  + "30-1, 30-2, 30-5, 30-10, 30-20, 30-30, 30-40, 30-50, 30-60, 30-70, 30-80, 30-90, 30-100,"
                                  + "20-1, 20-2, 20-5, 20-10, 20-20, 20-30, 20-40, 20-50, 20-60, 20-70, 20-80, 20-90, 20-100,"
                                  + "17-1, 17-2, 17-5, 17-10, 17-20, 17-30, 17-40, 17-50, 17-60, 17-70, 17-80, 17-90, 17-100,"
                                  + "15-1, 15-2, 15-5, 15-10, 15-20, 15-30, 15-40, 15-50, 15-60, 15-70, 15-80, 15-90, 15-100,"
                                  + "12-1, 12-2, 12-5, 12-10, 12-20, 12-30, 12-40, 12-50, 12-60, 12-70, 12-80, 12-90, 12-100,"
                                  + "10-1, 10-2, 10-5, 10-10, 10-20, 10-30, 10-40, 10-50, 10-60, 10-70, 10-80, 10-90, 10-100,");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 13; j++)
                    writer.Write(console.power_table[(int)bands[i]][j].ToString("f5") + ",");
            }
            writer.WriteLine("");
            writer.Close();

            path += "\\PA Power";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            writer = new StreamWriter(path + "\\pa_power_" + FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ".csv");
            writer.WriteLine("Band, 1w, 2w, 5w, 10w, 20w, 30w, 40w, 50w, 60w, 70w, 80w, 90w, 100w");
            for (int i = 0; i < bands.Length; i++)
            {
                writer.Write(BandToString(bands[i]) + ",");
                for (int j = 0; j < 13; j++)
                    writer.Write(console.power_table[(int)bands[i]][j].ToString("f5") + ",");
                writer.WriteLine("");
            }
            writer.Close();

            if (progress.Text != "")
            {
                lstDebug.Items.Insert(0, "Saving Power data to EEPROM...");
                byte checksum;
                FWCEEPROM.WritePAPower(console.power_table, out checksum);
                console.pa_power_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving Power data to EEPROM...done";
            }

            try
            {
                writer = new StreamWriter(console.AppDataPath + "power.csv");
                writer.WriteLine("Band, 1, 2, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100");
                for (int i = 1; i <= (int)Band.B6M; i++)
                {
                    writer.Write(((Band)i).ToString() + ",");
                    for (int j = 0; j < 13; j++)
                        writer.Write(console.power_table[i][j].ToString("f4") + ",");
                    writer.WriteLine("");
                }
                writer.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing power.csv file.  Please make sure this file is not open and try again.",
                    "Error writing power.csv",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (File.Exists(console.AppDataPath + "Compression.xls"))
            {
                Process.Start(console.AppDataPath + "power.csv");
                Thread.Sleep(100);
                Process.Start(console.AppDataPath + "Compression.xls");
            }

            path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            file_exists = File.Exists(path + "\\pa_swr.csv");
            writer = new StreamWriter(path + "\\pa_swr.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version, "
                                  + "6m, 160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
                writer.Write(console.swr_table[(int)bands[i]].ToString("f5") + ",");
            writer.WriteLine("");
            writer.Close();

            if (progress.Text != "")
            {
                lstDebug.Items.Insert(0, "Saving SWR data to EEPROM...");
                byte checksum;
                FWCEEPROM.WritePASWR(console.swr_table, out checksum);
                console.pa_swr_checksum = checksum;
                console.SyncCalDateTime();
                lstDebug.Items[0] = "Saving SWR data to EEPROM...done";
            }

            //progress.Hide();	
            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetFan(false);
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
            btnRunPACal.Enabled = true;
        }

        #endregion

        #endregion

        #region IO Tests

        #region XVRX

        private void btnIOXVRX_Click(object sender, System.EventArgs e)
        {
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnIOXVRX.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOXVRX));
            t.Name = "IO XVRX Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_xvrx = "IO XVRX Test: Not Run";
        private void CheckIOXVRX()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				btnIOXVRX.BackColor = Color.Red;
				return;
			}*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            //float target = -25.0f;
            float tol = 50.0f;
            //float tol2 = 40.0f;

            bool ret_val = true;

            PreampMode preamp = console.RX1PreampMode;
            console.RX1PreampMode = PreampMode.OFF;

            console.FullDuplex = true;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            double vfoa = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            double vfob = console.VFOBFreq;
            console.VFOBFreq = 14.2;

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.UpdateRX1Filters(400, 800);

            FWCAnt tx_ant = console.TXAnt;
            console.TXAnt = FWCAnt.ANT1;

            double scale = Audio.SourceScale;
            Audio.SourceScale = 1.0;

            double sine_freq = Audio.SineFreq1;
            Audio.SineFreq1 = 600.0;

            double radio_volume = Audio.RadioVolume;
            Audio.RadioVolume = 0.03;

            FWC.SetQSE(true);
            //Thread.Sleep(50);
            FWC.SetXVTR(true);
            //Thread.Sleep(50);
            FWC.SetXVTXEN(true);
            //Thread.Sleep(50);
            FWC.SetXVEN(true);
            //Thread.Sleep(50);

            Audio.TXInputSignal = Audio.SignalSource.SINE;
            Thread.Sleep(500);

            float sum = 0.0f;
            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
            }
            float on = sum / 5;
            on = on + console.MultiMeterCalOffset + console.PreampOffset + console.RX1PathOffset;

            //if(Math.Abs(on - target) > tol) ret_val = false;

            FWC.SetXVEN(false);
            Thread.Sleep(500);

            sum = 0.0f;
            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
            }
            float off = sum / 5;
            off = off + console.MultiMeterCalOffset + console.PreampOffset + console.RX1PathOffset;

            if ((on - off) < tol) ret_val = false;

            if (ret_val)
            {
                btnIOXVRX.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Test - XVRX: Passed (" + on.ToString("f1") + " " + off.ToString("f1") + ")");
                test_io_xvrx = "IO XVRX Test: Passed";
            }
            else
            {
                btnIOXVRX.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Test - XVRX: Failed (" + on.ToString("f1") + " " + off.ToString("f1") + ")");
                test_io_xvrx = "IO XVRX Test: Failed";
            }
            toolTip1.SetToolTip(btnIOXVRX, test_io_xvrx);

            // end
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            Audio.RadioVolume = radio_volume;
            Audio.SourceScale = scale;
            Audio.SineFreq1 = sine_freq;
            console.TXAnt = tx_ant;

            if (console.RX1Filter == filter)
                console.UpdateRX1Filters(var_low, var_high);
            else console.RX1Filter = filter;

            console.VFOAFreq = vfoa;
            console.RX1DSPMode = dsp_mode;

            FWC.SetXVTR(false);
            //Thread.Sleep(50);
            FWC.SetXVTXEN(false);
            //Thread.Sleep(50);
            FWC.SetXVEN(false);
            //Thread.Sleep(50);
            console.FullDuplex = false;

            console.RX1PreampMode = preamp;

            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_xvrx.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_xvrx.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, On, Off, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(on.ToString("f1") + ",");
            writer.Write(off.ToString("f1") + ",");
            writer.WriteLine(ret_val.ToString());
            writer.Close();
        }

        #endregion

        #region RX1 In/Out

        private void btnIORX1InOut_Click(object sender, System.EventArgs e)
        {
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnIORX1InOut.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIORX1InOut));
            t.Name = "IO RX1 In/Out Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_rx1inout = "IO RX1 In/Out Test: Not Run";
        private void CheckIORX1InOut()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				btnIORX1InOut.BackColor = Color.Red;
				return;
			}*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            //float tol = 1.0f;

            bool ret_val = true;

            PreampMode preamp = console.RX1PreampMode;
            console.RX1PreampMode = PreampMode.HIGH;

            double vfoa = console.VFOAFreq;
            console.VFOAFreq = 7.04;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.DSB;

            Filter filter = console.RX1Filter;
            int var_low = console.RX1FilterLow;
            int var_high = console.RX1FilterHigh;
            console.UpdateRX1Filters(-2000, 2000);

            FWCAnt tx_ant = console.TXAnt;
            console.TXAnt = FWCAnt.ANT1;

            FWCAnt rx1_ant = console.RX1Ant;
            console.RX1Ant = FWCAnt.ANT3;

            bool rx1_loop = console.RX1Loop;
            console.RX1Loop = false;

            Thread.Sleep(500);

            float sum = 0.0f;
            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
            }
            float on = sum / 5;
            on = on + console.MultiMeterCalOffset + console.PreampOffset + console.RX1PathOffset;

            //FWC.SetRX1Out(true);
            //Thread.Sleep(500);

            sum = 0.0f;
            /*DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
			Thread.Sleep(50);
			for(int j=0; j<5; j++)
			{
				sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
				Thread.Sleep(50);
			}*/
            float half = sum / 5;
            //half = half + console.MultiMeterCalOffset + console.PreampOffset + console.RXPathOffset;

            console.RX1Loop = true;
            Thread.Sleep(500);

            sum = 0.0f;
            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
            }
            float off = sum / 5;
            off = off + console.MultiMeterCalOffset + console.PreampOffset + console.RX1PathOffset;
            console.RX1Loop = false;

            if (Math.Abs(on - off) > 1.0 ||
                Math.Abs(-73.0 - on) > 1.5 ||
                Math.Abs(-73.0 - off) > 1.5)//|| on - half < 30.0f)
                ret_val = false;

            if (ret_val)
            {
                btnIORX1InOut.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO Test - RX1 In/Out: Passed (" + on.ToString("f1") + " " + half.ToString("f1") + " " + off.ToString("f1") + ")");
                test_io_rx1inout = "IO RX1 In/Out Test: Passed";
            }
            else
            {
                btnIORX1InOut.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO Test - RX1 In/Out: Failed (" + on.ToString("f1") + " " + half.ToString("f1") + " " + off.ToString("f1") + ")");
                test_io_rx1inout = "IO RX1 In/Out Test: Failed";
            }
            toolTip1.SetToolTip(btnIORX1InOut, test_io_rx1inout);

            // end
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            console.RX1Ant = rx1_ant;
            console.TXAnt = tx_ant;
            console.RX1PreampMode = preamp;

            console.RX1DSPMode = dsp_mode;
            if (console.RX1Filter == filter)
                console.UpdateRX1Filters(var_low, var_high);
            else console.RX1Filter = filter;

            console.VFOAFreq = vfoa;

            FWC.SetXVTR(false);
            //Thread.Sleep(50);
            FWC.SetXVTXEN(false);
            //Thread.Sleep(50);

            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_rx1inout.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_rx1inout.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, On, Off, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(on.ToString("f1") + ",");
            writer.Write(off.ToString("f1") + ",");
            writer.WriteLine(ret_val.ToString());
            writer.Close();
        }

        #endregion

        #region TX MON

        private void btnIOTXMon_Click(object sender, System.EventArgs e)
        {
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnIOTXMon.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckIOTXMon));
            t.Name = "IO TX Mon Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private string test_io_txmon = "IO TXMon Test: Not Run";
        private void CheckIOTXMon()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to run this test.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				btnIOTXMon.BackColor = Color.Red;
				return;
			}*/

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.VFOSync)
                console.VFOSync = false;

            //float target = -31.0f;
            //float tol = 6.0f;

            PreampMode preamp = console.RX1PreampMode;
            console.RX1PreampMode = PreampMode.HIGH;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            double vfoa = console.VFOAFreq;
            console.VFOAFreq = 14.2;

            double vfob = console.VFOBFreq;
            console.VFOBFreq = 14.2;

            int old_pwr = console.PWR;
            console.PWR = 10;

            Filter filter = console.RX1Filter;
            int filt_low = console.RX1FilterLow;
            int filt_high = console.RX1FilterHigh;
            console.UpdateRX1Filters(400, 800);

            console.FullDuplex = true;
            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(true);
            //Thread.Sleep(50);
            FWC.SetTR(true);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetTXMon(true);
            //Thread.Sleep(50);
            FWC.SetPABias(true);
            //Thread.Sleep(50);

            double scale = Audio.SourceScale;
            Audio.SourceScale = 1.0;

            double sine_freq = Audio.SineFreq1;
            Audio.SineFreq1 = 600.0;

            Audio.TXInputSignal = Audio.SignalSource.SINE;
            Thread.Sleep(1000);

            float sum = 0.0f;
            DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
            Thread.Sleep(50);
            for (int j = 0; j < 5; j++)
            {
                sum += DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                Thread.Sleep(50);
            }
            float on = sum / 5;
            on = on + console.MultiMeterCalOffset + console.PreampOffset + console.RX1PathOffset;

            //bool b = Math.Abs(on - target) <= tol;
            bool b = (on >= -55.0);
            if (b)
            {
                btnIOTXMon.BackColor = Color.Green;
                lstDebug.Items.Insert(0, "IO TX Mon: Passed (" + on.ToString("f1") + ")");
                test_io_txmon = "IO TXMon Test: Passed";
            }
            else
            {
                btnIOTXMon.BackColor = Color.Red;
                lstDebug.Items.Insert(0, "IO TX Mon: Failed (" + on.ToString("f1") + ")");
                test_io_txmon = "IO TXMon Test: Failed";
            }
            toolTip1.SetToolTip(btnIOTXMon, test_io_txmon);

            // end
            Audio.TXInputSignal = Audio.SignalSource.RADIO;
            Audio.SineFreq1 = sine_freq;
            Audio.SourceScale = scale;
            FWC.SetPABias(false);
            //Thread.Sleep(50);
            FWC.SetQSD(true);
            //Thread.Sleep(50);
            FWC.SetQSE(false);
            //Thread.Sleep(50);
            FWC.SetTR(false);
            //Thread.Sleep(50);
            FWC.SetSig(false);
            //Thread.Sleep(50);
            FWC.SetGen(false);
            //Thread.Sleep(50);
            FWC.SetTest(false);
            //Thread.Sleep(50);
            FWC.SetTXMon(false);
            //Thread.Sleep(50);
            console.FullDuplex = false;
            console.RX1Filter = filter;
            if (filter == Filter.VAR1 || filter == Filter.VAR2)
            {
                console.RX1FilterHigh = filt_high;
                console.RX1FilterLow = filt_low;
            }
            console.RX1DSPMode = dsp_mode;
            console.PWR = old_pwr;
            console.VFOAFreq = vfoa;
            console.VFOBFreq = vfob;

            grpIO.Enabled = true;
            grpPA.Enabled = true;
            grpATU.Enabled = true;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            bool file_exists = File.Exists(path + "\\io_txmon.csv");
            StreamWriter writer = new StreamWriter(path + "\\io_txmon.csv", true);
            if (!file_exists) writer.WriteLine("Serial Num, Date/Time, Version, Signal, Passed");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            writer.Write(on.ToString("f1") + ",");
            writer.WriteLine(b.ToString());
            writer.Close();
        }

        #endregion

        #region Run All

        private void btnIORunAll_Click(object sender, System.EventArgs e)
        {
            console.PowerOn = true;
            btnIORunAll.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(IORunAll));
            t.Name = "Run All IO Tests Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void IORunAll()
        {
            Invoke(new MethodInvoker(btnIOXVRX.PerformClick));
            Thread.Sleep(2000);
            Invoke(new MethodInvoker(btnIORX1InOut.PerformClick));
            Thread.Sleep(2000);
            Invoke(new MethodInvoker(btnIOTXMon.PerformClick));
            Thread.Sleep(2000);

            btnIORunAll.BackColor = SystemColors.Control;
        }

        #endregion

        #endregion

        #region ATU

        private string test_atu_swr = "ATU SWR Test: Not Run";
        private void btnATUCal_Click(object sender, System.EventArgs e)
        {
            if (!IsRegionUS())
            {
                if (MessageBox.Show("This radio must be TURFed for the US region for this calibration procedure to run successfully on all bands.\r\n\r\n" +
                    "Do you want to terminate this calibration procedure?",
                    "ATU SWR Calibration Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes) return;
            }

            progress = new Progress("Cal ATU SWR");
            grpPA.Enabled = false;
            grpIO.Enabled = false;
            grpATU.Enabled = false;
            btnATUSWR.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CalATUSWR));
            t.Name = "Cal ATU SWR Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            progress.Show();
        }

        private void CalATUSWR()
        {
            /*if(!console.PowerOn)
			{
				MessageBox.Show("Power must be on in order to calibrate SWR.", "Power Is Off",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
				grpIO.Enabled = true;
				grpPA.Enabled = true;
				progress.Hide();
				btnPASWR.BackColor = Color.Red;
				return;
			}*/

            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M, Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
            float[] band_freqs = { 1.85f, 3.75f, 5.357f, 7.15f, 10.125f, 14.175f, 18.1f, 21.300f, 24.9f, 28.4f, 50.11f };

            if (!console.PowerOn)
            {
                console.PowerOn = true;
                Thread.Sleep(500);
            }

            if (console.RXOnly)
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Cannot run this calibration while RX Only is selected\n(Setup Form -> General Tab)",
                    "Error: RX Only is active",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnATUSWR.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            if (!comboCOMPort.Text.StartsWith("COM"))
            {
                progress.Text = "";
                progress.Hide();
                MessageBox.Show("Invalid COM Port selected.  A valid COM Port connected to a PowerMaster is required.",
                    "Error: Invalid COM Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRunPACal.BackColor = Color.Red;
                grpPA.Enabled = true;
                grpIO.Enabled = true;
                grpATU.Enabled = true;
                return;
            }

            PowerMaster pm;
            try
            {
                pm = new PowerMaster(comboCOMPort.Text, ckPM2.Checked);
            }
            catch (Exception)
            {
                MessageBox.Show("Error opening COM Port for Power Master",
                    "COM Port Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            Thread.Sleep(500);
            if (!pm.Present)
            {
                MessageBox.Show("No data received from PowerMaster on " + comboCOMPort.Text + ".\n" +
                    "Please check COM port and PowerMaster connections and settings.\n\n" +
                    "Make sure the PowerMaster shows \"F\" on the upper left of the display.\n" +
                    "Verify the selected COM port is correct.  Verify port in Device Manager.",
                    "No Data From PowerMaster",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                grpIO.Enabled = true;
                grpPA.Enabled = true;
                progress.Hide();
                btnRunPACal.BackColor = Color.Red;
                return;
            }

            if (console.VFOSync)
                console.VFOSync = false;

            double vfoa = console.VFOAFreq;

            DSPMode dsp_mode = console.RX1DSPMode;
            console.RX1DSPMode = DSPMode.USB;

            console.FullDuplex = false;
            console.VFOSplit = false;

            int counter = 0;
            int num_bands = 0;
            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }
                if (do_band) num_bands++;
            }

            int total_counts = num_bands * 15;

            for (int i = 0; i < band_freqs.Length; i++)
            {
                bool do_band = false;
                switch (bands[i])
                {
                    case Band.B160M: do_band = ck160.Checked; break;
                    case Band.B80M: do_band = ck80.Checked; break;
                    case Band.B60M: do_band = ck60.Checked; break;
                    case Band.B40M: do_band = ck40.Checked; break;
                    case Band.B30M: do_band = ck30.Checked; break;
                    case Band.B20M: do_band = ck20.Checked; break;
                    case Band.B17M: do_band = ck17.Checked; break;
                    case Band.B15M: do_band = ck15.Checked; break;
                    case Band.B12M: do_band = ck12.Checked; break;
                    case Band.B10M: do_band = ck10.Checked; break;
                    case Band.B6M: do_band = ck6.Checked; break;
                }

                if (do_band)
                {
                    console.VFOAFreq = band_freqs[i];

                    FWCAnt tx_ant = console.TXAnt;
                    console.TXAnt = FWCAnt.ANT2;
                    for (int j = 0; j < 5; j++)
                    {
                        Thread.Sleep(50);
                        if (!progress.Visible) goto end2;
                        progress.SetPercent(counter++ / (float)total_counts);
                    }

                    console.atu_swr_table[(int)bands[i]] = 1.0f;
                    console.TUN = true;
                    int old_tun_pwr = console.PWR;
                    console.PWR = 10;
                    for (int j = 0; j < 10; j++)
                    {
                        Thread.Sleep(50);
                        if (!progress.Visible) goto end1;
                        progress.SetPercent(counter++ / (float)total_counts);
                    }

                    double f = console.ReadFwdPower(3);
                    double r = console.ReadRefPower(3);
                    Debug.WriteLine("f: " + f.ToString("f1") + "  r: " + r.ToString("f1"));

                    float alpha = (float)(f / r / 9);

                    console.atu_swr_table[(int)bands[i]] = (float)Math.Round(alpha, 4);
                    if (alpha < 0.1f || alpha > 5.0f)
                        lstDebug.Items.Insert(0, "ATU SWR Failed - " + BandToString(bands[i]) + ": " + alpha.ToString("f4"));
                    else lstDebug.Items.Insert(0, "ATU SWR Passed - " + BandToString(bands[i]) + ": " + alpha.ToString("f4"));

                    end1:
                    console.PWR = old_tun_pwr;
                    console.TUN = false;
                end2:
                    console.TXAnt = tx_ant;
                    if (!progress.Visible) goto end3;
                }
            }
        end3:
            console.VFOAFreq = 0.590;
            console.VFOAFreq = vfoa;
            console.RX1DSPMode = dsp_mode;
            console.FullDuplex = false;
            toolTip1.SetToolTip(btnATUSWR, test_atu_swr);

            bool fail = false;
            for (int i = 0; i < 11; i++)
            {
                if (console.atu_swr_table[(int)bands[i]] < 0.1f ||
                    console.atu_swr_table[(int)bands[i]] > 5.0f)
                {
                    btnATUSWR.BackColor = Color.Red;
                    fail = true;
                }
            }

            if (!fail) btnATUSWR.BackColor = Color.Green;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\atu_swr.csv");
            StreamWriter writer = new StreamWriter(path + "\\atu_swr.csv", true);
            if (!file_exists) writer.WriteLine("PA Serial Num, Date/Time, Version, "
                                  + "160m, 80m, 60m, 40m, 30m, 20m, 17m, 15m, 12m, 10m, 6m");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ",");

            for (int i = 0; i < bands.Length; i++)
                writer.Write(console.atu_swr_table[(int)bands[i]].ToString("f5") + ",");
            writer.WriteLine("");
            writer.Close();

            lstDebug.Items.Insert(0, "Saving ATU SWR data to EEPROM...");
            byte checksum;
            FWCEEPROM.WriteATUSWR(console.atu_swr_table, out checksum);
            console.atu_swr_checksum = checksum;
            console.SyncCalDateTime();
            lstDebug.Items[0] = "Saving ATU SWR data to EEPROM...done";

            progress.Hide();
            if (console.CurrentModel == Model.FLEX3000)
                FWC.SetFan(false);
            grpPA.Enabled = true;
            grpIO.Enabled = true;
            grpATU.Enabled = true;
        }

        #endregion

        private void btnCheckEEPROM_Click(object sender, EventArgs e)
        {
            btnCheckEEPROM.BackColor = console.ButtonSelectedColor;
            Thread t = new Thread(new ThreadStart(CheckEEPROM));
            t.Name = "Check EEPROM Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void CheckEEPROM()
        {
            string s = FWCEEPROM.SanityCheckAll();
            if (s.Contains("Failed"))
                btnCheckEEPROM.BackColor = Color.Red;
            else btnCheckEEPROM.BackColor = Color.Green;

            string path = console.AppDataPath + "Tests";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            bool file_exists = File.Exists(path + "\\sanity.csv");
            StreamWriter writer = new StreamWriter(path + "\\sanity.csv", true);
            if (!file_exists) writer.WriteLine("Radio Serial, TRX Serial, PA Serial, RFIO Serial, Date/Time, PowerSDR Version, Pass, Result");
            writer.Write(FWCEEPROM.SerialToString(FWCEEPROM.SerialNumber) + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.TRXSerial) + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.PASerial) + ","
                + FWCEEPROM.SerialToString(FWCEEPROM.RFIOSerial) + ","
                + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ","
                + console.Text + ","
                + (!s.Contains("Failed")).ToString() + ","
                + s.Replace(',', ' ').Replace('\n', ','));

            MessageBox.Show(s);

            writer.WriteLine("");
            writer.Close();
        }

        private void ckPM2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
