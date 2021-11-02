//=================================================================
// FilterForm.cs
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for FilterForm.
    /// </summary>
    public partial class FilterForm : System.Windows.Forms.Form
    {
        #region Variable Declaration 

        private Console console;
        private FilterPreset[] preset;
        private bool rx2;
        
        #endregion

        #region Constructor and Destructor

        public FilterForm(Console c, FilterPreset[] fp, bool _rx2)
        {
            //
            // Required for Windows Form Designer support
            //
            console = c;
            preset = fp;
            InitializeComponent();
            comboDSPMode.SelectedIndex = 0;
            radFilter1.Checked = true;
            rx2 = _rx2;
            if (rx2)
            {
                radFilter8.Enabled = false;
                radFilter9.Enabled = false;
                radFilter10.Enabled = false;
            }
            Common.RestoreForm(this, "FilterForm", false);
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

        private Filter current_filter = Filter.F1;
        public Filter CurrentFilter
        {
            get { return current_filter; }
            set
            {
                current_filter = value;

                switch (current_filter)
                {
                    case Filter.F1:
                        radFilter1.Checked = true;
                        break;
                    case Filter.F2:
                        radFilter2.Checked = true;
                        break;
                    case Filter.F3:
                        radFilter3.Checked = true;
                        break;
                    case Filter.F4:
                        radFilter4.Checked = true;
                        break;
                    case Filter.F5:
                        radFilter5.Checked = true;
                        break;
                    case Filter.F6:
                        radFilter6.Checked = true;
                        break;
                    case Filter.F7:
                        radFilter7.Checked = true;
                        break;
                    case Filter.F8:
                        radFilter8.Checked = true;
                        break;
                    case Filter.F9:
                        radFilter9.Checked = true;
                        break;
                    case Filter.F10:
                        radFilter10.Checked = true;
                        break;
                    case Filter.VAR1:
                        radFilterVar1.Checked = true;
                        break;
                    case Filter.VAR2:
                        radFilterVar2.Checked = true;
                        break;
                }

                GetFilterInfo();
            }
        }

        private DSPMode dsp_mode = DSPMode.FIRST;
        public DSPMode DSPMode
        {
            get { return dsp_mode; }
            set
            {
                dsp_mode = value;

                switch (dsp_mode)
                {
                    case DSPMode.LSB:
                        comboDSPMode.Text = "LSB";
                        break;
                    case DSPMode.USB:
                        comboDSPMode.Text = "USB";
                        break;
                    case DSPMode.DSB:
                        comboDSPMode.Text = "DSB";
                        break;
                    case DSPMode.CWL:
                        comboDSPMode.Text = "CWL";
                        break;
                    case DSPMode.CWU:
                        comboDSPMode.Text = "CWU";
                        break;
                    case DSPMode.FM:
                        comboDSPMode.Text = "FMN";
                        break;
                    case DSPMode.AM:
                        comboDSPMode.Text = "AM";
                        break;
                    case DSPMode.SAM:
                        comboDSPMode.Text = "SAM";
                        break;
                    case DSPMode.DIGL:
                        comboDSPMode.Text = "DIGL";
                        break;
                    case DSPMode.DIGU:
                        comboDSPMode.Text = "DIGU";
                        break;
                }

                radFilter1.Text = preset[(int)value].GetName(Filter.F1);
                radFilter2.Text = preset[(int)value].GetName(Filter.F2);
                radFilter3.Text = preset[(int)value].GetName(Filter.F3);
                radFilter4.Text = preset[(int)value].GetName(Filter.F4);
                radFilter5.Text = preset[(int)value].GetName(Filter.F5);
                radFilter6.Text = preset[(int)value].GetName(Filter.F6);
                radFilter7.Text = preset[(int)value].GetName(Filter.F7);
                radFilter8.Text = preset[(int)value].GetName(Filter.F8);
                radFilter9.Text = preset[(int)value].GetName(Filter.F9);
                radFilter10.Text = preset[(int)value].GetName(Filter.F10);
                radFilterVar1.Text = preset[(int)value].GetName(Filter.VAR1);
                radFilterVar2.Text = preset[(int)value].GetName(Filter.VAR2);
                GetFilterInfo();
            }
        }

        #endregion

        #region Misc Routines

        private void GetFilterInfo()
        {
            DSPMode m = DSPMode.FIRST;
            Filter f = Filter.FIRST;

            m = (DSPMode)Enum.Parse(typeof(DSPMode), comboDSPMode.Text);
            f = current_filter;

            txtName.Text = preset[(int)m].GetName(f);
            UpdateFilter(preset[(int)m].GetLow(f), preset[(int)m].GetHigh(f));
        }

        private int HzToPixel(float freq)
        {
            int low = (int)(-10000 * console.SampleRate1 / 48000.0);
            int high = (int)(10000 * console.SampleRate1 / 48000.0);

            return picDisplay.Width / 2 + (int)(freq / (high - low) * picDisplay.Width);
        }

        private float PixelToHz(float x)
        {
            int low = (int)(-10000 * console.SampleRate1 / 48000.0);
            int high = (int)(10000 * console.SampleRate1 / 48000.0);

            return (float)(low + ((double)x * (high - low) / picDisplay.Width));
        }

        private bool filter_updating = false;
        private void UpdateFilter(int low, int high)
        {
            filter_updating = true;
            if (low < udLow.Minimum) low = (int)udLow.Minimum;
            if (low > udLow.Maximum) low = (int)udLow.Maximum;
            if (high < udHigh.Minimum) high = (int)udHigh.Minimum;
            if (high > udHigh.Maximum) high = (int)udHigh.Maximum;

            udLow.Value = low;
            udHigh.Value = high;

            udWidth.Value = high - low;
            filter_updating = false;
        }

        #endregion

        #region Event Handlers

        private void radFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButtonTS r = (RadioButtonTS)sender;
            if (((RadioButtonTS)sender).Checked)
            {
                string filter = r.Name.Substring(r.Name.IndexOf("Filter") + 6);
                if (!filter.StartsWith("V")) filter = "F" + filter;
                else filter = filter.ToUpper();

                CurrentFilter = (Filter)Enum.Parse(typeof(Filter), filter);
                r.BackColor = console.ButtonSelectedColor;
            }
            else
            {
                r.BackColor = SystemColors.Control;
            }
        }

        private void comboDSPMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DSPMode = (DSPMode)Enum.Parse(typeof(DSPMode), comboDSPMode.Text);
        }

        private void txtName_LostFocus(object sender, System.EventArgs e)
        {
            preset[(int)dsp_mode].SetName(current_filter, txtName.Text);
            GetFilterInfo();
            if (!rx2)
            {
                if (console.RX1DSPMode == dsp_mode)
                    console.UpdateRX1FilterNames(current_filter);
            }
            else
            {
                if (console.RX2DSPMode == dsp_mode)
                    console.UpdateRX2FilterNames(current_filter);
            }

            switch (current_filter)
            {
                case Filter.F1:
                    radFilter1.Text = preset[(int)dsp_mode].GetName(Filter.F1);
                    break;
                case Filter.F2:
                    radFilter2.Text = preset[(int)dsp_mode].GetName(Filter.F2);
                    break;
                case Filter.F3:
                    radFilter3.Text = preset[(int)dsp_mode].GetName(Filter.F3);
                    break;
                case Filter.F4:
                    radFilter4.Text = preset[(int)dsp_mode].GetName(Filter.F4);
                    break;
                case Filter.F5:
                    radFilter5.Text = preset[(int)dsp_mode].GetName(Filter.F5);
                    break;
                case Filter.F6:
                    radFilter6.Text = preset[(int)dsp_mode].GetName(Filter.F6);
                    break;
                case Filter.F7:
                    radFilter7.Text = preset[(int)dsp_mode].GetName(Filter.F7);
                    break;
                case Filter.F8:
                    radFilter8.Text = preset[(int)dsp_mode].GetName(Filter.F8);
                    break;
                case Filter.F9:
                    radFilter9.Text = preset[(int)dsp_mode].GetName(Filter.F9);
                    break;
                case Filter.F10:
                    radFilter10.Text = preset[(int)dsp_mode].GetName(Filter.F10);
                    break;
                case Filter.VAR1:
                    radFilterVar1.Text = preset[(int)dsp_mode].GetName(Filter.VAR1);
                    break;
                case Filter.VAR2:
                    radFilterVar2.Text = preset[(int)dsp_mode].GetName(Filter.VAR2);
                    break;
            }
        }

        private void udLow_ValueChanged(object sender, System.EventArgs e)
        {
            if (udLow.Value + 10 > udHigh.Value && !filter_updating) udLow.Value = udHigh.Value - 10;

            preset[(int)dsp_mode].SetLow(current_filter, (int)udLow.Value);

            if (!rx2)
            {
                if (console.RX1DSPMode == dsp_mode && console.RX1Filter == current_filter)
                    console.UpdateRX1FilterPresetLow((int)udLow.Value);
            }
            else
            {
                if (console.RX2DSPMode == dsp_mode && console.RX2Filter == current_filter)
                    console.UpdateRX2FilterPresetLow((int)udLow.Value);
            }
            if (!filter_updating) UpdateFilter((int)udLow.Value, (int)udHigh.Value);
            picDisplay.Invalidate();
        }

        private void udHigh_ValueChanged(object sender, System.EventArgs e)
        {
            if (udHigh.Value - 10 < udLow.Value && !filter_updating) udHigh.Value = udLow.Value + 10;

            preset[(int)dsp_mode].SetHigh(current_filter, (int)udHigh.Value);

            if (!rx2)
            {
                if (console.RX1DSPMode == dsp_mode && console.RX1Filter == current_filter)
                    console.UpdateRX1FilterPresetHigh((int)udHigh.Value);
            }
            else
            {
                if (console.RX2DSPMode == dsp_mode && console.RX2Filter == current_filter)
                    console.UpdateRX2FilterPresetHigh((int)udHigh.Value);
            }
            if (!filter_updating) UpdateFilter((int)udLow.Value, (int)udHigh.Value);
            picDisplay.Invalidate();
        }

        private void udLow_LostFocus(object sender, EventArgs e)
        {
            udLow_ValueChanged(sender, e);
        }

        private void udHigh_LostFocus(object sender, EventArgs e)
        {
            udHigh_ValueChanged(sender, e);
        }

        private void picDisplay_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // draw background
            e.Graphics.FillRectangle(
                new SolidBrush(Display.DisplayBackgroundColor),
                0, 0, picDisplay.Width, picDisplay.Height);

            e.Graphics.FillRectangle(
                new SolidBrush(Display.DisplayFilterColor),
                HzToPixel((int)udLow.Value), 0,
                Math.Max(1, HzToPixel((int)udHigh.Value) - HzToPixel((int)udLow.Value)), picDisplay.Height);

            // draw center line
            e.Graphics.DrawLine(new Pen(Display.GridZeroColor, 1.0f),
                picDisplay.Width / 2, 0, picDisplay.Width / 2, picDisplay.Height);
        }

        private void picDisplay_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int low = HzToPixel((float)udLow.Value);
            int high = HzToPixel((float)udHigh.Value);

            if (Math.Abs(e.X - low) < 2 || Math.Abs(e.X - high) < 2)
                Cursor = Cursors.SizeWE;
            else if (e.X > low && e.X < high)
                Cursor = Cursors.NoMoveHoriz;
            else
                Cursor = Cursors.Arrow;

            if (drag_low) udLow.Value = Math.Max(Math.Min(udLow.Maximum, (int)PixelToHz((float)e.X)), udLow.Minimum);
            if (drag_high) udHigh.Value = Math.Max(Math.Min(udHigh.Maximum, (int)PixelToHz((float)e.X)), udHigh.Minimum); ;
            if (drag_filter)
            {
                int delta = (int)(PixelToHz((float)e.X) - PixelToHz(drag_filter_start));
                udLow.Value = Math.Max(Math.Min(udLow.Maximum, drag_filter_low + delta), udLow.Minimum);
                udHigh.Value = Math.Max(Math.Min(udHigh.Maximum, drag_filter_high + delta), udHigh.Minimum);
            }
        }

        private bool drag_low = false;
        private bool drag_high = false;
        private bool drag_filter = false;
        private int drag_filter_low = -1;
        private int drag_filter_high = -1;
        private int drag_filter_start = -1;

        private void picDisplay_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int low = HzToPixel((float)udLow.Value);
                int high = HzToPixel((float)udHigh.Value);

                if (Math.Abs(e.X - low) < 2)
                    drag_low = true;
                else if (Math.Abs(e.X - high) < 2)
                    drag_high = true;
                else if (e.X > low && e.X < high)
                {
                    drag_filter = true;
                    drag_filter_low = (int)udLow.Value;
                    drag_filter_high = (int)udHigh.Value;
                    drag_filter_start = e.X;
                }
            }
        }

        private void picDisplay_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag_low = false;
                drag_high = false;
                drag_filter = false;
                drag_filter_low = -1;
                drag_filter_high = -1;
                drag_filter_start = -1;
            }
        }

        private void udWidth_ValueChanged(object sender, System.EventArgs e)
        {
            if (udWidth.Focused)
            {
                int low = 0, high = 0;
                switch (comboDSPMode.Text)
                {
                    case "CWL":
                        low = (int)(-console.CWPitch - udWidth.Value / 2);
                        high = (int)(-console.CWPitch + udWidth.Value / 2);
                        break;
                    case "CWU":
                        low = (int)(console.CWPitch - udWidth.Value / 2);
                        high = (int)(console.CWPitch + udWidth.Value / 2);
                        break;
                    case "DIGL":
                        low = (int)(-console.DIGLClickTuneOffset - udWidth.Value / 2);  //W4TME
                        high = (int)(-console.DIGLClickTuneOffset + udWidth.Value / 2); //W4TME
                        break;
                    case "DIGU":
                        low = (int)(console.DIGUClickTuneOffset - udWidth.Value / 2);  //W4TME
                        high = (int)(console.DIGUClickTuneOffset + udWidth.Value / 2); //W4TME
                        break;
                    case "LSB":
                        high = -console.DefaultLowCut;
                        low = high - (int)udWidth.Value;
                        break;
                    case "USB":
                        low = console.DefaultLowCut;
                        high = low + (int)udWidth.Value;
                        break;
                    case "AM":
                    case "SAM":
                    case "FMN":
                        low = -(int)udWidth.Value / 2;
                        high = (int)udWidth.Value / 2;
                        break;
                }

                if (!filter_updating) UpdateFilter(low, high);
            }

        }

        #endregion
    }
}
