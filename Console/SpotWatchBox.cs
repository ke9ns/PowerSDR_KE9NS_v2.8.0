//=================================================================
// SpotWatchBox.cs .269 created
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

using EnvDTE;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Thread = System.Threading.Thread;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class SpotWatchBox : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen


        #region Constructor and Destructor

        public  SpotWatchBox(Console c)  // called the very first time
        {
            //
            // Required for Windows Form Designer support
            //

            InitializeComponent();

            Common.RestoreForm(this, "SpotWatchBox", false);

            this.TopMost = true;


          
           
               console.SpotLive = true; //.269 true= stay in thread until the DX Spot times out or moves, then close this SpotWatchbox

               Thread t1 = new Thread(new ThreadStart(SpotTimer));                                // 

                t1.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t1.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t1.Name = "SpotTimer";
                t1.IsBackground = true;
                t1.Priority = ThreadPriority.BelowNormal;
                t1.Start();
            
           

        }

       

        private void SpotTimer()   // 
        {
            Debug.WriteLine("SPOTTIMER Start");


            while (console.SpotLive == true)
            {
               
                Thread.Sleep(250);

                int ii = 0;

                if (btnTrack.Text != "Not Yet")
                {
                    try
                    {
                       
                        for (ii = 0; ii <= SpotControl.DX_Index; ii++)
                        {
                            if (SpotControl.DX_FULLSTRING[ii] == btnTrack.Text)
                            {
                                break;
                            }
                        } // for

                        if (ii > SpotControl.DX_Index) // this indicates the SPOT is now gone, so close up box
                        {
                            Debug.WriteLine("SPOTTIMER reset");
                            btnTrack.Text = "Not Yet";
                           
                            this.Close();
                        }

                    }
                    catch (Exception f)
                    {
                        Debug.WriteLine("bad stationC: " + f);
                        btnTrack.Text = "Not Yet";
                        this.Close();
                    }
                }

               
            } // while 


            Debug.WriteLine("SPOTTIMER END");

        } // SpotTimer


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



        // ke9ns close big pop-up box
        private void  SpotWatchBox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, " SpotWatchBox");
         
        }



        #endregion





        // ke9ns close big pop-up box
        private void  SpotWatchBox_VisibleChanged(object sender, EventArgs e)
        {




        } //  SpotWatchBox_VisibleChanged





        // start timer
        private void chkBoxTimed_CheckedChanged(object sender, EventArgs e)
        {




        } // chkBoxTimed_CheckedChanged



        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void SpotWatchBox_Load(object sender, EventArgs e)
        {

        }

        private void SpotWatchBox_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Common.SaveForm(this, "SpotWatchBox");
          
        }

        private void btnTrack_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            Common.SaveForm(this, "SpotWatchBox");
           
            MouseEventArgs me = (MouseEventArgs)e;

            if ((me.Button == System.Windows.Forms.MouseButtons.Right))
            {

            }
            else if ((me.Button == System.Windows.Forms.MouseButtons.Left)) // .269 send to VFOA
            {
                console.SpotForm.SpotWatchGoVfoA = true;

                try
                {

                    for (int ii =0; ii <= SpotControl.DX_Index; ii++)
                    {
                        if (SpotControl.DX_FULLSTRING[ii] == btnTrack.Text)
                        {
                            btnTrack.Text = "Not Yet";
                            console.SpotForm.SpotWatchIndex = ii; // pass location back to spot
                            console.SpotForm.textBox1_MouseUp(this, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)); // process as though you clicked on the dx spot in the spotter window

                        }
                    }
           
                }
                catch (Exception f)
                {
                    Debug.WriteLine("bad stationA: " + f);
                    btnTrack.Text = "Not Yet";
                    this.Close();
                }



            }
            else if ((me.Button == System.Windows.Forms.MouseButtons.Middle)) //.269 send to VFOB
            {
                console.SpotForm.SpotWatchGoVfoB = true;

                try
                {

                    for (int ii = 0; ii <= SpotControl.DX_Index; ii++)
                    {
                        if (SpotControl.DX_FULLSTRING[ii] == btnTrack.Text)
                        {
                            btnTrack.Text = "Not Yet";
                            console.SpotForm.SpotWatchIndex = ii; // pass location back to spot
                            console.SpotForm.textBox1_MouseUp(this, new MouseEventArgs(MouseButtons.Middle, 0, 0, 0, 0)); // process as though you clicked on the dx spot in the spotter window

                        }
                    }

                }
                catch (Exception f)
                {
                    Debug.WriteLine("bad stationB: " + f);
                    btnTrack.Text = "Not Yet";
                    this.Close();
                }




            }

            btnTrack.Text = "Not Yet";
            this.Close();

        } // btnTrack_MouseDown

    } //  SpotWatchBox

} // PowerSDR
