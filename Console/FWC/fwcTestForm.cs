//=================================================================
// fwcTestForm.cs
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
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class FWCTestForm : System.Windows.Forms.Form
    {
        #region Variable Declaration

        private Console console;
       
        #endregion

        #region Constructor and Destructor

        public FWCTestForm(Console c)
        {
            InitializeComponent();
            console = c;

            Thread t = new Thread(new ThreadStart(PollADC));
            t.Name = "Poll ADC Thread";
            t.IsBackground = true;
            t.Priority = ThreadPriority.BelowNormal;
            t.Start();
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

        private bool closing = false;
        private void PollADC()
        {
            int chan = 0;
            switch (console.CurrentModel)
            {
                case Model.FLEX5000:
                    chan = 4;
                    break;
                case Model.FLEX3000:
                    chan = 3;
                    break;
            }

            while (!closing)
            {
                int val;
                if (FWC.ReadPAADC(2, out val) == 0) break;
                float volts = (float)val / 4096 * 2.5f * 11;
                if (volts >= 15.0) txtVolts.BackColor = Color.Red;
                else txtVolts.BackColor = SystemColors.Control;
                if (!closing) txtVolts.Text = "Voltage: " + volts.ToString("f1");
                Thread.Sleep(1000);

                if (FWC.ReadPAADC(chan, out val) == 0) break;
                volts = (float)val / 4096 * 2.5f;
                double temp_c = 301 - volts * 1000 / 2.2;
                if (temp_c >= 100) txtTemp.BackColor = Color.Red;
                else txtTemp.BackColor = SystemColors.Control;
                if (!closing)
                {
                    switch (temp_format)
                    {
                        case TempFormat.Celsius:
                            txtTemp.Text = "Temp: " + temp_c.ToString("f0") + "° C";
                            break;
                        case TempFormat.Fahrenheit:
                            txtTemp.Text = "Temp: " + ((temp_c * 1.8) + 32).ToString("f0") + "° F";
                            break;
                    }
                }
                Thread.Sleep(1000);
            }
        }

        #endregion

        #region Event Handlers

        private void FWCTestForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
        }

        private enum TempFormat
        {
            Celsius = 0,
            Fahrenheit,
        }

        private TempFormat temp_format = TempFormat.Celsius;
        private void txtTemp_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (temp_format)
            {
                case TempFormat.Celsius:
                    temp_format = TempFormat.Fahrenheit;
                    break;
                case TempFormat.Fahrenheit:
                    temp_format = TempFormat.Celsius;
                    break;
            }
        }

        #endregion
    }
}
