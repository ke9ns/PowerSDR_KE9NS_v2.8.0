//=================================================================
// IDBOX.cs
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

namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class IDBOX : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen


        #region Constructor and Destructor

        public IDBOX(Console c)  // called the very first time
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            Common.RestoreForm(this, "IDBOX", false);

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

            console.TIMETOID = false;
        }

        #endregion

       
        #region Properties






        #endregion

        #region Event Handler

        private void IDBOX_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "IDBOX");
        }



        #endregion




        private void btnTrack_Click(object sender, EventArgs e)
        {
            console.TIMETOID = false;
            Common.SaveForm(this, "IDBOX");    // w4tme
            timerrunning = false;
            this.Close();

        }

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = true;
        }




        bool timerrunning = false;

        // start timer
        private void chkBoxTimed_CheckedChanged(object sender, EventArgs e)
        {

            if ((chkBoxTimed.Checked == true) && (timerrunning == false))
            {

                Thread t = new Thread(new ThreadStart(mompop_timer));  // turn on track map (sun, grayline, voacap, or beacon)

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                t.Name = "momentary popup timer";
                t.IsBackground = true;
                t.Priority = ThreadPriority.BelowNormal;
                t.Start();
            }



        } // chkBoxTimed_CheckedChanged



        //=======================================================
        // THREAD for 5 second
        private void mompop_timer()
        {
            timerrunning = true;

            Stopwatch poptimer = new Stopwatch();

            poptimer.Restart();

            while (chkBoxTimed.Checked == true)
            {
                Thread.Sleep(100);

                if (poptimer.ElapsedMilliseconds > 5000)
                {
                    mompop_end();
                    break;
                }


            } //  while (chkBoxTimed.Checked == true)

            poptimer.Stop();



            //  timerrunning = false;

        } // mompop_timer()

        //=======================================================
        private void mompop_end()
        {
            console.TIMETOID = false;
            Common.SaveForm(this, "IDBOX");    // w4tme
            timerrunning = false;
            this.Close();

        }



        private void IDBOX_VisibleChanged(object sender, EventArgs e)
        {


            if ((chkBoxTimed.Checked == true) && (timerrunning == false))
            {

                Thread t = new Thread(new ThreadStart(mompop_timer));  // turn on track map (sun, grayline, voacap, or beacon)

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                t.Name = "momentary popup timer";
                t.IsBackground = true;
                t.Priority = ThreadPriority.BelowNormal;
                t.Start();
            }


        } // IDBOX_VisibleChanged


    } // IDBOX

} // PowerSDR
