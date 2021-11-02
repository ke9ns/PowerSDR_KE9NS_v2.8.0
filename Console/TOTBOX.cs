//=================================================================
// TOTBOX.cs
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
using System.Windows.Forms;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class TOTBOX : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen


        #region Constructor and Destructor

        public TOTBOX(Console c)  // called the very first time
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            Common.RestoreForm(this, "TOTBOX", false);

            this.TopMost = true;


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


            if (console.setupForm != null)
            {
                if (console.setupForm.chkBoxTOT.Checked) console.RXOnly = false; // reset radio
            }

            console.TOT_TRIP = false; // reset timer
        }

        #endregion

        

        #region Properties






        #endregion

        #region Event Handler



        // ke9ns close big pop-up box
        private void TOTBOX_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (console.setupForm != null)
            {
                if (console.setupForm.chkBoxTOT.Checked) console.RXOnly = false; // reset radio
            }
            console.TOT_TRIP = false; // reset timer

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "TOTBOX");


        }



        #endregion



        // ke9ns click on big button in center of pop-up box
        private void btnTrack_Click(object sender, EventArgs e)
        {

            if (console.setupForm != null)
            {
                if (console.setupForm.chkBoxTOT.Checked) console.RXOnly = false; // reset radio
            }
            console.TOT_TRIP = false; // reset timer

            Common.SaveForm(this, "TOTBOX");    // w4tme

            this.Close();

        }


        // ke9ns close big pop-up box
        private void TOTBOX_VisibleChanged(object sender, EventArgs e)
        {




        } // TOTBOX_VisibleChanged





        // start timer
        private void chkBoxTimed_CheckedChanged(object sender, EventArgs e)
        {




        } // chkBoxTimed_CheckedChanged








        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = true;
        }


    } // TOTBOX

} // PowerSDR
