//=================================================================
// helpbox.cs
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

//using CefSharp;            // ke9ns add to allow embedded chrome browser (for help videos)
//using CefSharp.WinForms;


namespace PowerSDR
{
    /// <summary>
    /// Summary description for WaveOptions.
    /// </summary>
    public partial class helpbox : System.Windows.Forms.Form
    {

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen

        #region Variable Declaration

      
        // public static helpbox helpboxForm;                       // ke9ns add 

        #endregion

        #region Constructor and Destructor

        public helpbox(Console c)  // called the very first time
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            // console = c;
            Common.RestoreForm(this, "helpbox", false);

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

            if (webBrowser1.Visible == true)
            {
                webBrowser1.Visible = false;
                webBrowser1.SendToBack();

                webBrowser1.Stop();
                webBrowser1.Dispose();

            }
        }

        #endregion

       
        #region Properties






        #endregion

        #region Event Handler

        private void helpbox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            this.Hide();
            //  e.Cancel = true; // remvoing this will kill the form when closed, as opposed to just hiding it.
            Common.SaveForm(this, "helpbox");
        }



        #endregion



        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = true;
        }



        private void helpbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.HELPMAP = false;
            Console.HELPSWR = false;

            this.Text = "PowerSDR F1 Text Help (F2 Video Help)";

            //   if (Cef.IsInitialized == true)
            // {
            //     chrome.Dispose();
            //chrome.Stop();
            // }

        }  // helpbox_FormClosing


        //  ChromiumWebBrowser chrome;
        //   CefSettings settings = new CefSettings();

        public string youtube_embed = @"https://ke9ns.com"; //"w5j6jh6c0_g";

        public void HelpChrome1()
        {

            if (console.INIT1 == false)    //  Only do 1 time period. if (Cef.IsInitialized == false)
            {
                console.INIT1 = true;
                //   Cef.Initialize(settings);

            }

            Debug.WriteLine("video address: " + youtube_embed);

            //  string temp = @"https://www.youtube.com/embed/" + youtube_embed + "?rel=0&amp"; // "?rel=0&amp;showinfo=0";
            //  string temp = @"https://www.youtube.com/embed/" + youtube_embed + "?rel=0&amp"; // "?rel=0&amp;showinfo=0";

            //   chrome = new ChromiumWebBrowser(youtube_embed);

            //  groupBox1.Controls.Add(chrome);    //  groupBox1.Controls.Clear();

            //  chrome.Dock = DockStyle.Fill;

        } // void InitChrome()


        private void helpbox_message_TextChanged(object sender, EventArgs e)
        {

            //  if (Cef.IsInitialized == true) chrome.Dispose();    //  Cef.Shutdown();

            //  HelpChrome1();

        }

    } // helpbox

} // PowerSDR
