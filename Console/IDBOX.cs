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
	public class IDBOX : System.Windows.Forms.Form
	{

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen


        #region Variable Declaration
        private System.ComponentModel.IContainer components;
        private CheckBoxTS chkAlwaysOnTop;
        private CheckBoxTS chkBoxTimed;
        private ToolTip toolTip1;
        public Button btnTrack;

       // public static IDBOX IDBOXForm;                       // ke9ns add 

        #endregion

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

            console.TIMETOID = false;
        }

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDBOX));
            this.btnTrack = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkBoxTimed = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.SuspendLayout();
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrack.ForeColor = System.Drawing.Color.Black;
            this.btnTrack.Location = new System.Drawing.Point(69, 67);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(229, 45);
            this.btnTrack.TabIndex = 99;
            this.btnTrack.Text = ">>> Time To ID <<<";
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // chkBoxTimed
            // 
            this.chkBoxTimed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxTimed.Image = null;
            this.chkBoxTimed.Location = new System.Drawing.Point(12, 145);
            this.chkBoxTimed.Name = "chkBoxTimed";
            this.chkBoxTimed.Size = new System.Drawing.Size(112, 31);
            this.chkBoxTimed.TabIndex = 101;
            this.chkBoxTimed.Text = "5 sec timeout";
            this.toolTip1.SetToolTip(this.chkBoxTimed, "check to have this box disappear after 5 seconds");
            this.chkBoxTimed.CheckedChanged += new System.EventHandler(this.chkBoxTimed_CheckedChanged);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Checked = true;
            this.chkAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(252, 145);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 31);
            this.chkAlwaysOnTop.TabIndex = 100;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.Visible = false;
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // IDBOX
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(367, 188);
            this.Controls.Add(this.chkBoxTimed);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(383, 227);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(383, 227);
            this.Name = "IDBOX";
            this.Text = "Time to ID Your Station";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.IDBOX_Closing);
            this.VisibleChanged += new System.EventHandler(this.IDBOX_VisibleChanged);
            this.ResumeLayout(false);

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
           
                if ((chkBoxTimed.Checked == true)  && (timerrunning == false))
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
        private  void mompop_timer()
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
           
            
                if ((chkBoxTimed.Checked == true) && (timerrunning == false) )
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
