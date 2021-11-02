//=================================================================
// wave_options.cs
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

using System.Diagnostics;
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class WaveOptions : System.Windows.Forms.Form
    {
       

        #region Constructor and Destructor

        public WaveOptions()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            comboSampleRate.Text = Audio.SampleRate1.ToString();
            Common.RestoreForm(this, "WaveOptions", false);
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

        public int SampleRate
        {
            get { return int.Parse(comboSampleRate.Text); }
        }

        public bool temp_record; // ke9ns add save the status of pre to put back when done
                                 //-------------------------------------------------------------------------------
                                 // ke9ns add  to force audio into POST mode (for quick audio and TX waterfall ID and scheduler)
        public bool RECPLAY1
        {
            get { return false; }
            set
            {

                temp_record = Audio.RecordRXPreProcessed; // save original value


                radRXPostProcessed.Checked = value;
                radTXPostProcessed.Checked = value;

                Audio.RecordRXPreProcessed = false;
                Audio.RecordTXPreProcessed = false;
            }

        }


        public bool RXPreProcessed
        {
            get { return radRXPreProcessed.Checked; }
        }

        #endregion

        #region Event Handler

        private void WaveOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "WaveOptions");
        }

        private void radRXPreProcessed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRXPreProcessed.Checked)
            {
                comboSampleRate.Enabled = false;
                Audio.RecordRXPreProcessed = true;
            }
            else Debug.WriteLine("Wav");

        }

        private void radRXPostProcessed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radRXPostProcessed.Checked)
            {
                comboSampleRate.Enabled = true;
                Audio.RecordRXPreProcessed = false;
            }
        }

        private void radTXPreProcessed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radTXPreProcessed.Checked)
            {
                Audio.RecordTXPreProcessed = true;
            }
        }

        private void radTXPostProcessed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radTXPostProcessed.Checked)
            {
                Audio.RecordTXPreProcessed = false;
            }
        }

        #endregion
    }
}
