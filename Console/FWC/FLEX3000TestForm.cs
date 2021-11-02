//=================================================================
// FLEX3000TestForm.cs
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
using System.Threading;

namespace PowerSDR
{
    public partial class FLEX3000TestForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
        

        #endregion

        #region Constructor and Destructor

        public FLEX3000TestForm(Console c)
        {
            InitializeComponent();
            console = c;

            chkATUEnable_CheckedChanged(this, EventArgs.Empty);
            chkHiZ_CheckedChanged(this, EventArgs.Empty);
            //udATUC_ValueChanged(this, EventArgs.Empty);
            //udATUL_ValueChanged(this, EventArgs.Empty);
            chkC_CheckedChanged(this, EventArgs.Empty);
            chkL_CheckedChanged(this, EventArgs.Empty);
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

        

        #region Event Handlers

        private void FLEX3000TestForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private bool hw_enable = true;
        private void chkL_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!hw_enable) return;
            byte data = 0;

            if (chkL2.Checked) data |= 0x01;
            if (chkL3.Checked) data |= 0x02;
            if (chkL4.Checked) data |= 0x04;
            if (chkL5.Checked) data |= 0x08;
            if (chkL6.Checked) data |= 0x10;
            if (chkL7.Checked) data |= 0x20;
            if (chkL8.Checked) data |= 0x40;
            if (chkL9.Checked) data |= 0x80;

            FWC.I2C_Write2Value(0x4E, 0x02, data);
        }

        private void chkC_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!hw_enable) return;
            byte data = 0;

            if (chkC0.Checked) data |= 0x01;
            if (chkC1.Checked) data |= 0x02;
            if (chkC2.Checked) data |= 0x04;
            if (chkC3.Checked) data |= 0x08;
            if (chkC4.Checked) data |= 0x10;
            if (chkC5.Checked) data |= 0x20;
            if (chkC6.Checked) data |= 0x40;

            FWC.I2C_Write2Value(0x4E, 0x03, data);
        }

        private void chkHiZ_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetHiZ(chkHiZ.Checked);
        }

        private void chkATUEnable_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetATUEnable(chkATUEnable.Checked);
        }

        private void chkATUATTN_CheckedChanged(object sender, System.EventArgs e)
        {
            FWC.SetATUATTN(chkATUATTN.Checked);
        }

        private void udATUL_ValueChanged(object sender, System.EventArgs e)
        {
            hw_enable = false;
            int val = (int)udATUL.Value;
            chkL9.Checked = ((val & 0x01) == 0x01);
            chkL8.Checked = ((val & 0x02) == 0x02);
            chkL7.Checked = ((val & 0x04) == 0x04);
            chkL6.Checked = ((val & 0x08) == 0x08);
            chkL5.Checked = ((val & 0x10) == 0x10);
            chkL4.Checked = ((val & 0x20) == 0x20);
            chkL3.Checked = ((val & 0x40) == 0x40);
            chkL2.Checked = ((val & 0x80) == 0x80);
            hw_enable = true;
            chkL_CheckedChanged(this, EventArgs.Empty);
        }

        private void udATUC_ValueChanged(object sender, System.EventArgs e)
        {
            hw_enable = false;
            int val = (int)udATUC.Value;
            chkC0.Checked = ((val & 0x01) == 0x01);
            chkC1.Checked = ((val & 0x02) == 0x02);
            chkC2.Checked = ((val & 0x04) == 0x04);
            chkC3.Checked = ((val & 0x08) == 0x08);
            chkC4.Checked = ((val & 0x10) == 0x10);
            chkC5.Checked = ((val & 0x20) == 0x20);
            chkC6.Checked = ((val & 0x40) == 0x40);
            hw_enable = true;
            chkC_CheckedChanged(this, EventArgs.Empty);
        }

        private void chkPollPhaseMag_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkPollPhaseMag.Checked)
            {
                Thread t = new Thread(new ThreadStart(PollPhaseMag));
                t.Name = "Poll Phase/Mag Thread";
                t.Priority = ThreadPriority.Normal;
                t.IsBackground = true;
                t.Start();
            }
        }

        private double saved_phase = 999999.0;
        private double saved_mag = 999999.0;
        private void PollPhaseMag()
        {
            int new_phase = 0, new_mag = 0;
            double volts = 0.0f;
            while (chkPollPhaseMag.Checked)
            {
                FWC.ReadPAADC(6, out new_phase);
                if (saved_phase == 999999.0)
                    saved_phase = new_phase;
                else
                    saved_phase = 0.8 * saved_phase + 0.2 * new_phase;
                volts = (float)saved_phase / 4096 * 2.5f;
                txtPhase.Text = (volts - 1.25f).ToString("f2") + " V";
                picPhase.Invalidate();
                Thread.Sleep(100);

                FWC.ReadPAADC(7, out new_mag);
                if (saved_mag == 999999.0)
                    saved_mag = new_mag;
                else
                    saved_mag = 0.8 * saved_mag + 0.2 * new_mag;
                volts = (float)saved_mag / 4096 * 2.5f;
                txtMag.Text = (volts - 1.25f).ToString("f2") + " V";
                picMag.Invalidate();
                Thread.Sleep(100);
            }
        }

        #endregion

        private void picPhase_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picPhase.Width;
            int H = picPhase.Height;
            int x = (int)(saved_phase / 4096 * W);
            if (x < 1) x = 1;
            if (x > W - 1) x = W - 1;
            e.Graphics.DrawLine(Pens.Gold, x - 1, 0, x - 1, H);
            e.Graphics.DrawLine(Pens.Yellow, x, 0, x, H);
            e.Graphics.DrawLine(Pens.Gold, x + 1, 0, x + 1, H);
        }

        private void picMag_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picMag.Width;
            int H = picMag.Height;
            int x = (int)(saved_mag / 4096 * W);
            if (x < 1) x = 1;
            if (x > W - 1) x = W - 1;
            e.Graphics.DrawLine(Pens.Gold, x - 1, 0, x - 1, H);
            e.Graphics.DrawLine(Pens.Yellow, x, 0, x, H);
            e.Graphics.DrawLine(Pens.Gold, x + 1, 0, x + 1, H);
        }

        private void udFanPWM_ValueChanged(object sender, System.EventArgs e)
        {
            FWC.SetFanPWM((int)udFanPWMOn.Value, (int)udFanPWMOff.Value);
        }

        private void udFanSpeed_ValueChanged(object sender, System.EventArgs e)
        {
            FWC.SetFanSpeed((float)udFanSpeed.Value);
        }

        private void chkPollSWR_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkPollSWR.Checked)
            {
                Thread t = new Thread(new ThreadStart(PollSWR));
                t.Name = "Poll SWR Thread";
                t.Priority = ThreadPriority.Normal;
                t.IsBackground = true;
                t.Start();
            }
        }

        private double saved_swr = 999999.0;
        private void PollSWR()
        {
            double new_swr = 1.0;
            while (chkPollSWR.Checked)
            {
                /*FWC.ReadSWR(out new_swr);
				if(new_swr > 50.0f) new_swr = 50.0f;
				if(new_swr < 1.0f) new_swr = 1.0f;*/

                new_swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);

                if (saved_swr == 999999.0)
                    saved_swr = new_swr;
                else
                    saved_swr = 0.8 * saved_swr + 0.2 * new_swr;

                txtSWR.Text = saved_swr.ToString("f1") + " : 1";
                picSWR.Invalidate();
                Thread.Sleep(100);
            }
        }

        private void picSWR_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int W = picSWR.Width;
            int H = picSWR.Height;
            int x = (int)((saved_swr - 1) / 9.0 * W);
            if (x < 1) x = 1;
            if (x > W - 1) x = W - 1;
            e.Graphics.DrawLine(Pens.Gold, x - 1, 0, x - 1, H);
            e.Graphics.DrawLine(Pens.Yellow, x, 0, x, H);
            e.Graphics.DrawLine(Pens.Gold, x + 1, 0, x + 1, H);
        }

        private void btnTune_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(Tune));
            t.Name = "Tune Thread";
            t.Priority = ThreadPriority.Normal;
            t.IsBackground = true;
            t.Start();
        }

        private void Tune()
        {
            const int SLEEP_TIME = 500;

            chkATUEnable.Checked = false;
            int old_power = console.TunePower;
            console.TunePower = 5;
            console.TUN = true;
            Thread.Sleep(500);

            double swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);

            if (swr < 1.5)
                goto end;

            //console.TunePower = 0;
            //Thread.Sleep(50);
            chkATUEnable.Checked = true;
            udATUC.Value = 8;
            udATUL.Value = 8;
            chkHiZ.Checked = false;
            //console.TunePower = 5;
            Thread.Sleep(500);

            double lo_z_swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);

            //console.TunePower = 0;
            //Thread.Sleep(50);
            chkHiZ.Checked = true;
            //console.TunePower = 5;
            Thread.Sleep(SLEEP_TIME);
            double hi_z_swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);

            if (lo_z_swr < hi_z_swr)
                chkHiZ.Checked = false;

            Debug.WriteLine("lo: " + lo_z_swr.ToString("f1") + "  hi: " + hi_z_swr.ToString("f1"));

            if (chkHiZ.Checked)
                swr = hi_z_swr;
            else
                swr = lo_z_swr;

            if (swr < 1.2) goto end;

            double min_swr = double.MaxValue;
            if (chkHiZ.Checked)
            {
                udATUC.Value = 0;
                udATUL.Value = 0;
                if (ATUTuneC(min_swr, (int)udATUC.Value, 4)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneL(min_swr, (int)udATUL.Value, 4)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneC(min_swr, (int)udATUC.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneL(min_swr, (int)udATUL.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneC(min_swr, (int)udATUC.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneL(min_swr, (int)udATUL.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
            }
            else
            {
                udATUL.Value = 0;
                udATUC.Value = 0;
                if (ATUTuneL(min_swr, (int)udATUL.Value, 4)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneC(min_swr, (int)udATUC.Value, 4)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneL(min_swr, (int)udATUL.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneC(min_swr, (int)udATUC.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneL(min_swr, (int)udATUL.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
                if (ATUTuneC(min_swr, (int)udATUC.Value, 1)) goto end;
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                if (swr < min_swr) min_swr = swr;
                Debug.WriteLine("swr: " + swr.ToString("f1"));
            }

        end:
            swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
            Debug.WriteLine("swr: " + swr.ToString("f1"));
            // cleanup
            console.TUN = false;
            console.TunePower = old_power;
        }

        private bool ATUTuneC(double min_swr, int start, int step)
        {
            const int SLEEP_TIME = 500;
            double swr;
            double local_min = double.MaxValue;
            int min_val = start;

            while (udATUC.Value < udATUC.Maximum - step)
            {
                Thread.Sleep(SLEEP_TIME);
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                Debug.WriteLine("swr (" + udATUL.Value + ", " + udATUC.Value + "): " + swr.ToString("f1"));
                if (swr < 1.2) return true;
                if (swr < local_min)
                {
                    local_min = swr;
                    min_val = (int)udATUC.Value;
                }
                /*if(swr > min_swr+0.5)
					break;*/
                if (swr > local_min + 0.3)
                    break;
                //console.TunePower = 0;
                //Thread.Sleep(50);
                udATUC.Value += step;
                //console.TunePower = 5;
            }

            udATUC.Value = min_val;
            local_min = double.MaxValue;

            while (udATUC.Value > udATUC.Minimum + step)
            {
                Thread.Sleep(SLEEP_TIME);
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                Debug.WriteLine("swr (" + udATUL.Value + ", " + udATUC.Value + "): " + swr.ToString("f1"));
                if (swr < 1.2) return true;
                if (swr < local_min)
                {
                    local_min = swr;
                    min_val = (int)udATUC.Value;
                }
                /*if(swr > min_swr+0.5)
					break;*/
                if (swr > local_min + 0.3)
                    break;
                //console.TunePower = 0;
                //Thread.Sleep(50);
                udATUC.Value -= step;
                //console.TunePower = 5;
            }

            udATUC.Value = min_val;
            return false;
        }

        private bool ATUTuneL(double min_swr, int start, int step)
        {
            const int SLEEP_TIME = 500;
            double swr;
            double local_min = double.MaxValue;
            int min_val = start;

            while (udATUL.Value < udATUL.Maximum - step)
            {
                Thread.Sleep(SLEEP_TIME);
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                Debug.WriteLine("swr (" + udATUL.Value + ", " + udATUC.Value + "): " + swr.ToString("f1"));
                if (swr < 1.2) return true;
                if (swr < local_min)
                {
                    local_min = swr;
                    min_val = (int)udATUL.Value;
                }
                /*if(swr > min_swr+0.3)
					break;*/
                if (swr > local_min + 0.3)
                    break;
                //console.TunePower = 0;
                //Thread.Sleep(50);
                udATUL.Value += step;
                //console.TunePower = 5;
            }

            udATUL.Value = min_val;
            local_min = double.MaxValue;

            while (udATUL.Value > udATUL.Minimum + step)
            {
                Thread.Sleep(SLEEP_TIME);
                swr = console.FWCSWR(console.pa_fwd_power, console.pa_rev_power);
                Debug.WriteLine("swr (" + udATUL.Value + ", " + udATUC.Value + "): " + swr.ToString("f1"));
                if (swr < 1.2) return true;
                if (swr < local_min)
                {
                    local_min = swr;
                    min_val = (int)udATUL.Value;
                }
                /*if(swr > min_swr+0.3)
					break;*/
                if (swr > local_min + 0.3)
                    break;
                //console.TunePower = 0;
                //Thread.Sleep(50);
                udATUL.Value -= step;
                //console.TunePower = 5;
            }

            udATUL.Value = min_val;
            return false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
