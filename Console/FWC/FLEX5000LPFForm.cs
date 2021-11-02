//=================================================================
// FLEX5000LPFForm.cs
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

namespace PowerSDR
{
    public partial class FLEX5000LPFForm : System.Windows.Forms.Form
    {
        

        #region Constructor and Destructor

        public FLEX5000LPFForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            grpRX2.Enabled = FWCEEPROM.RX2OK;
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

        

        #region RX1

        private void radRX1Auto_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1Auto.Checked)
            {
                FWC.SetManualRX1Filter(false);
                FWC.BypassRX1Filter(false);
            }
        }

        private void radRX1B160_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B160.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(1.9f);
            }
        }

        private void radRX1B80_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B80.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(3.6f);
            }
        }

        private void radRX1B60_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B60.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(5.3465f);
            }
        }

        private void radRX1B40_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B40.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(7.1f);
            }
        }

        private void radRX1B30_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B30.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(10.12f);
            }
        }

        private void radRX1B20_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B20.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(14.1f);
            }
        }

        private void radRX1B17_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B17.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(18.11f);
            }
        }

        private void radRX1B15_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B15.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(21.1f);
            }
        }

        private void radRX1B12_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B12.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(24.9f);
            }
        }

        private void radRX1B10_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B10.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(28.4f);
            }
        }

        private void radRX1B6_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1B6.Checked)
            {
                FWC.SetManualRX1Filter(true);
                FWC.BypassRX1Filter(false);
                FWC.SetRX1Filter(50.4f);
            }
        }

        private void radRX1Bypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX1Bypass.Checked)
            {
                FWC.SetManualRX1Filter(false);
                FWC.BypassRX1Filter(true);
            }
        }

        #endregion

        #region RX2

        private void radRX2Auto_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2Auto.Checked)
            {
                FWC.SetManualRX2Filter(false);
                FWC.BypassRX2Filter(false);
            }
        }

        private void radRX2B160_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B160.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(1.9f);
            }
        }

        private void radRX2B80_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B80.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(3.6f);
            }
        }

        private void radRX2B60_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B60.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(5.3465f);
            }
        }

        private void radRX2B40_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B40.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(7.1f);
            }
        }

        private void radRX2B30_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B30.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(10.12f);
            }
        }

        private void radRX2B20_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B20.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(14.1f);
            }
        }

        private void radRX2B17_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B17.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(18.11f);
            }
        }

        private void radRX2B15_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B15.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(21.1f);
            }
        }

        private void radRX2B12_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B12.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(24.9f);
            }
        }

        private void radRX2B10_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B10.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(28.4f);
            }
        }

        private void radRX2B6_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2B6.Checked)
            {
                FWC.SetManualRX2Filter(true);
                FWC.BypassRX2Filter(false);
                FWC.SetRX2Filter(50.4f);
            }
        }

        private void radRX2Bypass_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRX2Bypass.Checked)
            {
                FWC.SetManualRX2Filter(false);
                FWC.BypassRX2Filter(true);
            }
        }

        #endregion
    }
}
