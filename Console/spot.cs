﻿//=================================================================
// Spot.cs
// created by Darrin Kohn ke9ns
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
//
//=================================================================
// Fractional year:
//g = (360/365.25)*(N + hour/24)    //  N=day number 1-365


// declination:
// D = 0.396372 - 22.91327 * cos(g) + 4.02543 * sin(g) - 0.387205 * cos( 2 * g)+
//   + 0.051967 * sin(2 * g) - 0.154527 * cos( 3 * g) + 0.084798 * sin( 3 * g)


//=================================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;                    // ke9ns add for stringbuilder
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
//using System.Runtime.Serialization.Json;

//reference Nuget Package NAudio.Lame
using NAudio;
using NAudio.Wave;
using NAudio.Lame;


using System.Net.Sockets;                // ke9ns add for tcpip internet connections
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Policy;
//using Microsoft.JScript;

namespace PowerSDR
{
   
    //==========================================================
    // ke9ns used by NIST time sync routine to allow update of PC timeclock
    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

      
        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }
       
        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
        
        public static DateTime ToDateTime(SystemTime time)
        {
            return time.ToDateTime();
        }

    } // struct SystemTime


    //=======================================================================================
    public class SpotControl : System.Windows.Forms.Form
    {

        // ke9ns multimedia timer functions copied from cwx.cs file

        #region Win32 Multimedia Timer Functions

        private int tel;            // time of one element in ms
     
        // Represents the method that is called by Windows when a timer event occurs.
        private delegate void TimeProc(int id, int msg, int user, int param1, int param2);

        // Specifies constants for multimedia timer event types.

        public enum TimerMode
        {
            OneShot,    // Timer event occurs once.
            Periodic    // Timer event occurs periodically.
        };

        // Represents information about the timer's capabilities.
        [StructLayout(LayoutKind.Sequential)]
        public struct TimerCaps
        {
            public int periodMin;   // Minimum supported period in milliseconds.
            public int periodMax;   // Maximum supported period in milliseconds.
        }


        // Gets timer capabilities.
        [DllImport("winmm.dll")]
        private static extern int timeGetDevCaps(ref TimerCaps caps,
            int sizeOfTimerCaps);

        // Creates and starts the timer.
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution,
            TimeProc proc, int user, int mode);

        // Stops and destroys the timer.
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);

        // Indicates that the operation was successful.
        private const int TIMERR_NOERROR = 0;

        // Timer identifier.
        private int timerID;
        private MainMenu mainMenu1;
        public MenuItem mnuSpotOptions;
        public CheckBoxTS chkBoxContour;
        private Button button2;
        public Button DXPost;
        private TextBox textBoxDXCall;
        private MenuItem menuTimeServers;
        private MenuItem chkTimeServer1;
        private MenuItem chkTimeServer2;
        private MenuItem chkTimeServer3;
        private MenuItem chkTimeServer4;
        private MenuItem chkTimeServer5;
        private TextBox textBox4;
        public CheckBoxTS chkMoon;
        public CheckBoxTS chkISS;
        private TimeProc timeProcPeriodic;

        // ke9ns run this to kill the prior timer and start a new timer 
        private void setup_timer(int cwxwpm)
        {
            
            tel = cwxwpm;    // (1200 / cwxwpm);

            if (timerID != 0)
            {
                timeKillEvent(timerID);
            }

                    // (delay, resolution, proc, user, mode)
            timerID = timeSetEvent(tel, 1, timeProcPeriodic, 0, (int)TimerMode.Periodic);
            
            if (timerID == 0)
            {
                Debug.Fail("Timer creation failed.");
            }
        }


        #endregion




        private static System.Reflection.Assembly myAssembly2 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream Map_image = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.picD1.png");     // MAP

        public static Stream Map_image2 = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.picD2.png");     // MAP with lat / long on it

        private static System.Reflection.Assembly myAssembly1 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream sun_image = myAssembly1.GetManifestResourceStream("PowerSDR.Resources.sun.png");       // SUN

        private static System.Reflection.Assembly myAssembly4 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream moon_image = myAssembly4.GetManifestResourceStream("PowerSDR.Resources.moon1.png");       // MOON

        private static System.Reflection.Assembly myAssembly5 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream iss_image = myAssembly5.GetManifestResourceStream("PowerSDR.Resources.ISS.png");       // ISS

        private static System.Reflection.Assembly myAssembly3 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream star_image = myAssembly3.GetManifestResourceStream("PowerSDR.Resources.star.png");      // star to indicate your transmitter based on your lat and long

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen

        public static SpotOptions SpotOptions;
        public static Conrec conrec;


        public Setup setupForm;                             // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 
        public StackControl StackForm;                      // ke9ns add  communications with spot.cs and stack
        public SwlControl SwlForm;                          // ke9ns add  communications with spot.cs and swl

        public DXMemList dxmemlist;                         //  ke9ns add comm with dx cluster list

      
        //   public static Display display;


        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";
        public Button SWLbutton;
        public Button SSBbutton;
        public TextBox textBox1;
        public TextBox nodeBox1;
        private RichTextBox textBox3;
        public TextBox callBox;
        public TextBox portBox2;
        private TextBox statusBox;
        private Button button1;
        private TextBox statusBoxSWL;
        private Label label1;
        private Label label2;
        private CheckBoxTS chkAlwaysOnTop;
        public CheckBoxTS chkDXMode;
        public CheckBoxTS chkSUN;
        private ToolTip toolTip1;
        public CheckBoxTS chkGrayLine;
        public Button btnTrack;
        public CheckBoxTS chkPanMode;
        public TextBox nameBox;
        public CheckBoxTS chkMapCall;
        public CheckBoxTS chkMapCountry;
        public CheckBoxTS chkMapBand;
        public CheckBoxTS chkBoxCW;
        public CheckBoxTS chkBoxSSB;
        public CheckBoxTS chkBoxDIG;
        public CheckBoxTS chkBoxPan;
        private DataGridView dataGridView1;
        public CheckBoxTS chkBoxMem;
        public DataGridView dataGridView2;
        private Button SWLbutton2;
        public CheckBoxTS chkBoxNA;
        public CheckBoxTS chkBoxWrld;
        private NumericUpDownTS udDisplayLat;
        private NumericUpDownTS udDisplayLong;
        private Label label3;
        private Label label4;
        public CheckBoxTS chkBoxBeam;
        private Label label5;
        private Button btnBeacon;
        public CheckBoxTS BoxBScan;
        public CheckBoxTS BoxBFScan;
        private NumericUpDownTS numericUpDownTS1;
        private Button btnTime;
        public CheckBoxTS checkBoxWWV;
        private NumericUpDownTS udDisplayWWV;
        private IContainer components;
        private RadioButton checkBoxTone;
        private TextBox textBox2;
        public CheckBoxTS checkBoxMUF;
        public CheckBoxTS chkBoxAnt;
        public TrackBarTS tbPanPower;
        public DSP dsp;

        #region Constructor and Destructor

        public SpotControl(Console c)
        {
            InitializeComponent();
            console = c;

            SpotOptions.SpotForm = this; // allows Spotoptions to see public data 

            Display.SpotForm = this;  // allows Display to see public data (not public static data)
            StackControl.SpotForm = this; // allows Stack to see public data from spot
                                          //  SwlControl.SpotForm = this; // allows swl to see public data from spot

            Common.RestoreForm(this, "SpotForm", true);


            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FlexRadio Systems\\";
            string file_name = path + "DXMemory.xml";

            // dataGridView1.Dock = DockStyle.Fill;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.DataSource = console.DXMemList.List; // ke9ns get list of memories from memorylist.cs is where the file is opened and saved

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AutoGenerateColumns = false;


            if (!File.Exists(file_name))
            {
                console.DXMemList.List.Add(new DXMemRecord("k1rfi.com:7300"));
                console.DXMemList.List.Add(new DXMemRecord("ve7cc.net:23"));
                console.DXMemList.List.Add(new DXMemRecord("telnet.reversebeacon.net:7000"));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));


                console.DXMemList.Save1();

                Debug.WriteLine("create DXURL File");

            }



        } // spotcontrol

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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SWLbutton = new System.Windows.Forms.Button();
            this.SSBbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.nodeBox1 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.RichTextBox();
            this.callBox = new System.Windows.Forms.TextBox();
            this.portBox2 = new System.Windows.Forms.TextBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusBoxSWL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnTrack = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.SWLbutton2 = new System.Windows.Forms.Button();
            this.btnBeacon = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.checkBoxTone = new System.Windows.Forms.RadioButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.DXPost = new System.Windows.Forms.Button();
            this.textBoxDXCall = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.chkISS = new System.Windows.Forms.CheckBoxTS();
            this.chkMoon = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxContour = new System.Windows.Forms.CheckBoxTS();
            this.tbPanPower = new System.Windows.Forms.TrackBarTS();
            this.chkBoxAnt = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxDIG = new System.Windows.Forms.CheckBoxTS();
            this.checkBoxMUF = new System.Windows.Forms.CheckBoxTS();
            this.udDisplayWWV = new System.Windows.Forms.NumericUpDownTS();
            this.checkBoxWWV = new System.Windows.Forms.CheckBoxTS();
            this.numericUpDownTS1 = new System.Windows.Forms.NumericUpDownTS();
            this.BoxBFScan = new System.Windows.Forms.CheckBoxTS();
            this.BoxBScan = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxBeam = new System.Windows.Forms.CheckBoxTS();
            this.udDisplayLong = new System.Windows.Forms.NumericUpDownTS();
            this.udDisplayLat = new System.Windows.Forms.NumericUpDownTS();
            this.chkBoxMem = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxPan = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxSSB = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxCW = new System.Windows.Forms.CheckBoxTS();
            this.chkMapBand = new System.Windows.Forms.CheckBoxTS();
            this.chkMapCountry = new System.Windows.Forms.CheckBoxTS();
            this.chkMapCall = new System.Windows.Forms.CheckBoxTS();
            this.chkPanMode = new System.Windows.Forms.CheckBoxTS();
            this.chkGrayLine = new System.Windows.Forms.CheckBoxTS();
            this.chkSUN = new System.Windows.Forms.CheckBoxTS();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuSpotOptions = new System.Windows.Forms.MenuItem();
            this.menuTimeServers = new System.Windows.Forms.MenuItem();
            this.chkTimeServer1 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer2 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer3 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer4 = new System.Windows.Forms.MenuItem();
            this.chkTimeServer5 = new System.Windows.Forms.MenuItem();
            this.chkBoxWrld = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxNA = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.chkDXMode = new System.Windows.Forms.CheckBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayWWV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLat)).BeginInit();
            this.SuspendLayout();
            // 
            // SWLbutton
            // 
            this.SWLbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SWLbutton.Location = new System.Drawing.Point(615, 386);
            this.SWLbutton.Name = "SWLbutton";
            this.SWLbutton.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton.TabIndex = 2;
            this.SWLbutton.Text = "Spot SWL";
            this.toolTip1.SetToolTip(this.SWLbutton, resources.GetString("SWLbutton.ToolTip"));
            this.SWLbutton.UseVisualStyleBackColor = false;
            this.SWLbutton.Click += new System.EventHandler(this.SWLbutton_Click);
            // 
            // SSBbutton
            // 
            this.SSBbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SSBbutton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SSBbutton.Location = new System.Drawing.Point(12, 485);
            this.SSBbutton.Name = "SSBbutton";
            this.SSBbutton.Size = new System.Drawing.Size(75, 23);
            this.SSBbutton.TabIndex = 1;
            this.SSBbutton.Text = "Spot DX";
            this.toolTip1.SetToolTip(this.SSBbutton, "Click to Turn On/Off Dx Cluster Spotting (on both this DX Spotting window and Pan" +
        "adapter)\r\nRequires Internet to work.\r\n");
            this.SSBbutton.UseVisualStyleBackColor = false;
            this.SSBbutton.Click += new System.EventHandler(this.spotSSB_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(12, 143);
            this.textBox1.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.textBox1.MaxLength = 10000000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(759, 186);
            this.textBox1.TabIndex = 6;
            this.textBox1.TabStop = false;
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // nodeBox1
            // 
            this.nodeBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nodeBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeBox1.Location = new System.Drawing.Point(658, 384);
            this.nodeBox1.MaxLength = 50;
            this.nodeBox1.Name = "nodeBox1";
            this.nodeBox1.Size = new System.Drawing.Size(84, 22);
            this.nodeBox1.TabIndex = 6;
            this.nodeBox1.Text = "spider.ham-radio-deluxe.com";
            this.toolTip1.SetToolTip(this.nodeBox1, "Enter in a DX Cluster URL address here");
            this.nodeBox1.Visible = false;
            this.nodeBox1.TextChanged += new System.EventHandler(this.nodeBox_TextChanged);
            this.nodeBox1.Leave += new System.EventHandler(this.nodeBox_Leave);
            this.nodeBox1.MouseEnter += new System.EventHandler(this.nodeBox_MouseEnter);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(298, 7);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(473, 130);
            this.textBox3.TabIndex = 8;
            this.textBox3.TabStop = false;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            this.textBox3.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.textBox3_LinkClicked);
            // 
            // callBox
            // 
            this.callBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.callBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callBox.Location = new System.Drawing.Point(682, 488);
            this.callBox.MaxLength = 20;
            this.callBox.Name = "callBox";
            this.callBox.Size = new System.Drawing.Size(87, 22);
            this.callBox.TabIndex = 5;
            this.callBox.Text = "Callsign";
            this.toolTip1.SetToolTip(this.callBox, "Enter Your Call sign to login to the DX Cluster here");
            this.callBox.TextChanged += new System.EventHandler(this.callBox_TextChanged);
            this.callBox.Leave += new System.EventHandler(this.callBox_Leave);
            this.callBox.MouseEnter += new System.EventHandler(this.callBox_MouseEnter);
            // 
            // portBox2
            // 
            this.portBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.portBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portBox2.Location = new System.Drawing.Point(696, 386);
            this.portBox2.MaxLength = 7;
            this.portBox2.Name = "portBox2";
            this.portBox2.Size = new System.Drawing.Size(56, 22);
            this.portBox2.TabIndex = 7;
            this.portBox2.Text = "0";
            this.toolTip1.SetToolTip(this.portBox2, "Enter in Dx Cluster URL Port# here");
            this.portBox2.Visible = false;
            this.portBox2.TextChanged += new System.EventHandler(this.portBox_TextChanged);
            this.portBox2.Leave += new System.EventHandler(this.portBox_Leave);
            this.portBox2.MouseEnter += new System.EventHandler(this.portBox_MouseEnter);
            // 
            // statusBox
            // 
            this.statusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.Location = new System.Drawing.Point(13, 356);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(156, 22);
            this.statusBox.TabIndex = 11;
            this.statusBox.Text = "Off";
            this.toolTip1.SetToolTip(this.statusBox, "Click to Test connection\r\nIf it goes back to \"Spotting\" then the connection is go" +
        "od");
            this.statusBox.Click += new System.EventHandler(this.statusBox_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(101, 485);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pause";
            this.toolTip1.SetToolTip(this.button1, "Click to Pause the DX Text window (if spots are coming through too fast)\r\nUpdates" +
        " to the Panadapter will still occur");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusBoxSWL
            // 
            this.statusBoxSWL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBoxSWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBoxSWL.Location = new System.Drawing.Point(614, 358);
            this.statusBoxSWL.Name = "statusBoxSWL";
            this.statusBoxSWL.Size = new System.Drawing.Size(156, 22);
            this.statusBoxSWL.TabIndex = 16;
            this.statusBoxSWL.Text = "Off";
            this.toolTip1.SetToolTip(this.statusBoxSWL, "Status of ShortWave spotter list transfer to PowerSDR memory\r\n");
            this.statusBoxSWL.Click += new System.EventHandler(this.statusBoxSWL_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(13, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Status of DX Cluster";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(612, 342);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Status of SWL Spotter";
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrack.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTrack.Location = new System.Drawing.Point(258, 440);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(75, 23);
            this.btnTrack.TabIndex = 62;
            this.btnTrack.Text = "Track";
            this.toolTip1.SetToolTip(this.btnTrack, resources.GetString("btnTrack.ToolTip"));
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameBox.Location = new System.Drawing.Point(687, 386);
            this.nameBox.MaxLength = 20;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(46, 22);
            this.nameBox.TabIndex = 64;
            this.nameBox.Text = "name";
            this.toolTip1.SetToolTip(this.nameBox, "Enter Your Call sign to login to the DX Cluster here");
            this.nameBox.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(12, 7);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Size = new System.Drawing.Size(280, 130);
            this.dataGridView1.TabIndex = 72;
            this.toolTip1.SetToolTip(this.dataGridView1, "Enter DX address : port#\r\nExample:  k1rfi.com:7300\r\n");
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowDrop = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView2.Location = new System.Drawing.Point(498, 143);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView2.Size = new System.Drawing.Size(254, 94);
            this.dataGridView2.TabIndex = 75;
            this.toolTip1.SetToolTip(this.dataGridView2, "memories");
            this.dataGridView2.Visible = false;
            // 
            // SWLbutton2
            // 
            this.SWLbutton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SWLbutton2.Location = new System.Drawing.Point(696, 386);
            this.SWLbutton2.Name = "SWLbutton2";
            this.SWLbutton2.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton2.TabIndex = 76;
            this.SWLbutton2.Text = "SWL list";
            this.toolTip1.SetToolTip(this.SWLbutton2, resources.GetString("SWLbutton2.ToolTip"));
            this.SWLbutton2.UseVisualStyleBackColor = false;
            this.SWLbutton2.Click += new System.EventHandler(this.SWLbutton2_Click);
            // 
            // btnBeacon
            // 
            this.btnBeacon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBeacon.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBeacon.Location = new System.Drawing.Point(192, 485);
            this.btnBeacon.Name = "btnBeacon";
            this.btnBeacon.Size = new System.Drawing.Size(75, 23);
            this.btnBeacon.TabIndex = 85;
            this.btnBeacon.Text = "Beacon Chk";
            this.toolTip1.SetToolTip(this.btnBeacon, resources.GetString("btnBeacon.ToolTip"));
            this.btnBeacon.UseVisualStyleBackColor = false;
            this.btnBeacon.Click += new System.EventHandler(this.btnBeacon_Click);
            // 
            // btnTime
            // 
            this.btnTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTime.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTime.Location = new System.Drawing.Point(511, 344);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(75, 23);
            this.btnTime.TabIndex = 89;
            this.btnTime.Text = "Time Sync";
            this.toolTip1.SetToolTip(this.btnTime, resources.GetString("btnTime.ToolTip"));
            this.btnTime.UseVisualStyleBackColor = false;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // checkBoxTone
            // 
            this.checkBoxTone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTone.AutoSize = true;
            this.checkBoxTone.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.checkBoxTone.Location = new System.Drawing.Point(559, 397);
            this.checkBoxTone.Name = "checkBoxTone";
            this.checkBoxTone.Size = new System.Drawing.Size(46, 17);
            this.checkBoxTone.TabIndex = 93;
            this.checkBoxTone.TabStop = true;
            this.checkBoxTone.Text = "Tick";
            this.toolTip1.SetToolTip(this.checkBoxTone, "ON = BCD sub-Carrier Tick\r\nOFF = no Tick");
            this.checkBoxTone.UseVisualStyleBackColor = true;
            this.checkBoxTone.CheckedChanged += new System.EventHandler(this.checkBoxTone_CheckedChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(559, 417);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(37, 20);
            this.textBox2.TabIndex = 94;
            this.textBox2.Text = "0";
            this.toolTip1.SetToolTip(this.textBox2, "Length of Tone in mSec\r\n");
            this.textBox2.Visible = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button2.Location = new System.Drawing.Point(297, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(68, 23);
            this.button2.TabIndex = 99;
            this.button2.Text = "Pause";
            this.toolTip1.SetToolTip(this.button2, "Click to Pause the DX Text window (if spots are coming through too fast)\r\nUpdates" +
        " to the Panadapter will still occur");
            this.button2.UseVisualStyleBackColor = false;
            // 
            // DXPost
            // 
            this.DXPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DXPost.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.DXPost.Enabled = false;
            this.DXPost.Location = new System.Drawing.Point(12, 397);
            this.DXPost.Name = "DXPost";
            this.DXPost.Size = new System.Drawing.Size(64, 23);
            this.DXPost.TabIndex = 100;
            this.DXPost.Text = "Callout";
            this.toolTip1.SetToolTip(this.DXPost, "Posts your Contact to the DX Cluster\r\n\r\nFill in DX Callsign with Contact \r\nOR\r\nse" +
        "lect DX spot line above\r\n");
            this.DXPost.UseVisualStyleBackColor = false;
            this.DXPost.Click += new System.EventHandler(this.DXPost_Click);
            // 
            // textBoxDXCall
            // 
            this.textBoxDXCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxDXCall.Enabled = false;
            this.textBoxDXCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDXCall.Location = new System.Drawing.Point(82, 398);
            this.textBoxDXCall.Name = "textBoxDXCall";
            this.textBoxDXCall.Size = new System.Drawing.Size(87, 22);
            this.textBoxDXCall.TabIndex = 101;
            this.textBoxDXCall.Text = "DX Callsign";
            this.toolTip1.SetToolTip(this.textBoxDXCall, "Enter the Call Sign of the DX station you made Contact with.\r\n\r\nIf you do not ent" +
        "er a Call Sign, you must select a DX station from a line above.");
            this.textBoxDXCall.Click += new System.EventHandler(this.textBoxDXCall_Click);
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(480, 489);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(37, 20);
            this.textBox4.TabIndex = 102;
            this.textBox4.Text = "0";
            this.toolTip1.SetToolTip(this.textBox4, "Moons Ecliptic Longitude\r\n");
            this.textBox4.Visible = false;
            // 
            // chkISS
            // 
            this.chkISS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkISS.Checked = true;
            this.chkISS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkISS.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkISS.Image = null;
            this.chkISS.Location = new System.Drawing.Point(367, 343);
            this.chkISS.Name = "chkISS";
            this.chkISS.Size = new System.Drawing.Size(46, 24);
            this.chkISS.TabIndex = 104;
            this.chkISS.Text = "ISS";
            this.toolTip1.SetToolTip(this.chkISS, resources.GetString("chkISS.ToolTip"));
            this.chkISS.CheckedChanged += new System.EventHandler(this.chkISS_CheckedChanged);
            // 
            // chkMoon
            // 
            this.chkMoon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMoon.Checked = true;
            this.chkMoon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMoon.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMoon.Image = null;
            this.chkMoon.Location = new System.Drawing.Point(320, 343);
            this.chkMoon.Name = "chkMoon";
            this.chkMoon.Size = new System.Drawing.Size(58, 24);
            this.chkMoon.TabIndex = 103;
            this.chkMoon.Text = "Moon";
            this.toolTip1.SetToolTip(this.chkMoon, resources.GetString("chkMoon.ToolTip"));
            this.chkMoon.CheckedChanged += new System.EventHandler(this.chkMoon_CheckedChanged);
            // 
            // chkBoxContour
            // 
            this.chkBoxContour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxContour.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxContour.Image = null;
            this.chkBoxContour.Location = new System.Drawing.Point(343, 413);
            this.chkBoxContour.Name = "chkBoxContour";
            this.chkBoxContour.Size = new System.Drawing.Size(70, 24);
            this.chkBoxContour.TabIndex = 98;
            this.chkBoxContour.Text = "Contour";
            this.toolTip1.SetToolTip(this.chkBoxContour, "VOACAP: Check for Contour instead of dots\r\n\r\n");
            this.chkBoxContour.CheckedChanged += new System.EventHandler(this.chkBoxContour_CheckedChanged);
            // 
            // tbPanPower
            // 
            this.tbPanPower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPanPower.AutoSize = false;
            this.tbPanPower.Location = new System.Drawing.Point(339, 461);
            this.tbPanPower.Maximum = 1500;
            this.tbPanPower.Minimum = 1;
            this.tbPanPower.Name = "tbPanPower";
            this.tbPanPower.Size = new System.Drawing.Size(66, 18);
            this.tbPanPower.TabIndex = 97;
            this.tbPanPower.TickFrequency = 90;
            this.toolTip1.SetToolTip(this.tbPanPower, "VOACAP: 400 Watts");
            this.tbPanPower.Value = 400;
            this.tbPanPower.Scroll += new System.EventHandler(this.tbPanPower_Scroll);
            this.tbPanPower.MouseEnter += new System.EventHandler(this.tbPanPower_MouseEnter);
            this.tbPanPower.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbPanPower_MouseUp);
            // 
            // chkBoxAnt
            // 
            this.chkBoxAnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxAnt.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxAnt.Image = null;
            this.chkBoxAnt.Location = new System.Drawing.Point(343, 437);
            this.chkBoxAnt.Name = "chkBoxAnt";
            this.chkBoxAnt.Size = new System.Drawing.Size(55, 24);
            this.chkBoxAnt.TabIndex = 96;
            this.chkBoxAnt.Text = "Beam";
            this.toolTip1.SetToolTip(this.chkBoxAnt, "Check this box if your using a Beam Antenna instead of a Dipole\r\n\r\nUse VOACAP to " +
        "map Signal Strength from your station, \r\nbased on your Lat & Long\r\n\r\nView using " +
        "TRACK button\r\n\r\n");
            this.chkBoxAnt.CheckedChanged += new System.EventHandler(this.chkBoxAnt_CheckedChanged);
            // 
            // chkBoxDIG
            // 
            this.chkBoxDIG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxDIG.Checked = true;
            this.chkBoxDIG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxDIG.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxDIG.Image = null;
            this.chkBoxDIG.Location = new System.Drawing.Point(182, 414);
            this.chkBoxDIG.Name = "chkBoxDIG";
            this.chkBoxDIG.Size = new System.Drawing.Size(85, 24);
            this.chkBoxDIG.TabIndex = 70;
            this.chkBoxDIG.Text = "Spot Digital";
            this.toolTip1.SetToolTip(this.chkBoxDIG, "Show Digital spots when checked (like RTTY, PSK, etc)\r\n");
            // 
            // checkBoxMUF
            // 
            this.checkBoxMUF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMUF.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxMUF.Image = null;
            this.checkBoxMUF.Location = new System.Drawing.Point(273, 410);
            this.checkBoxMUF.Name = "checkBoxMUF";
            this.checkBoxMUF.Size = new System.Drawing.Size(75, 24);
            this.checkBoxMUF.TabIndex = 95;
            this.checkBoxMUF.Text = "VOACAP";
            this.toolTip1.SetToolTip(this.checkBoxMUF, "Hit F1 for a Full Description\r\n\r\nUse VOACAP to map Signal Strength from your stat" +
        "ion, \r\nbased on your Lat & Long\r\n\r\nView using TRACK button\r\n\r\n");
            this.checkBoxMUF.CheckedChanged += new System.EventHandler(this.checkBoxMUF_CheckedChanged);
            this.checkBoxMUF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxMUF_MouseDown);
            // 
            // udDisplayWWV
            // 
            this.udDisplayWWV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayWWV.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayWWV.Location = new System.Drawing.Point(511, 394);
            this.udDisplayWWV.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udDisplayWWV.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayWWV.Name = "udDisplayWWV";
            this.udDisplayWWV.Size = new System.Drawing.Size(39, 20);
            this.udDisplayWWV.TabIndex = 91;
            this.toolTip1.SetToolTip(this.udDisplayWWV, "If you check the \"use WWV HF\" Box:\r\nSelect a WWV station with a stron non-fading " +
        "signal.\r\nUsually 10mhz and 15mhz are the cleanest signals\r\n1=2.5mhz \r\n2=5.0mhz\r\n" +
        "3=10.0mhz\r\n4=15.0mhz\r\n\r\n");
            this.udDisplayWWV.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBoxWWV
            // 
            this.checkBoxWWV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxWWV.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxWWV.Image = null;
            this.checkBoxWWV.Location = new System.Drawing.Point(511, 373);
            this.checkBoxWWV.Name = "checkBoxWWV";
            this.checkBoxWWV.Size = new System.Drawing.Size(98, 24);
            this.checkBoxWWV.TabIndex = 90;
            this.checkBoxWWV.Text = "Use WWV HF";
            this.toolTip1.SetToolTip(this.checkBoxWWV, resources.GetString("checkBoxWWV.ToolTip"));
            this.checkBoxWWV.CheckedChanged += new System.EventHandler(this.checkBoxWWV_CheckedChanged);
            // 
            // numericUpDownTS1
            // 
            this.numericUpDownTS1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownTS1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.Location = new System.Drawing.Point(417, 488);
            this.numericUpDownTS1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTS1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.Name = "numericUpDownTS1";
            this.numericUpDownTS1.Size = new System.Drawing.Size(39, 20);
            this.numericUpDownTS1.TabIndex = 88;
            this.toolTip1.SetToolTip(this.numericUpDownTS1, "Which Band to Start Slow Beacaon Scan on:\r\n1=14.1mhz\r\n2=18.11mhz\r\n3=21.15mhz\r\n4=2" +
        "4.93mhz\r\n5=28.2mhz\r\n");
            this.numericUpDownTS1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.ValueChanged += new System.EventHandler(this.numericUpDownTS1_ValueChanged);
            // 
            // BoxBFScan
            // 
            this.BoxBFScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BoxBFScan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BoxBFScan.Image = null;
            this.BoxBFScan.Location = new System.Drawing.Point(343, 485);
            this.BoxBFScan.Name = "BoxBFScan";
            this.BoxBFScan.Size = new System.Drawing.Size(87, 24);
            this.BoxBFScan.TabIndex = 87;
            this.BoxBFScan.Text = "Slow Scan";
            this.toolTip1.SetToolTip(this.BoxBFScan, resources.GetString("BoxBFScan.ToolTip"));
            this.BoxBFScan.CheckedChanged += new System.EventHandler(this.BoxBFScan_CheckedChanged);
            // 
            // BoxBScan
            // 
            this.BoxBScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BoxBScan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.BoxBScan.Image = null;
            this.BoxBScan.Location = new System.Drawing.Point(273, 486);
            this.BoxBScan.Name = "BoxBScan";
            this.BoxBScan.Size = new System.Drawing.Size(81, 24);
            this.BoxBScan.TabIndex = 86;
            this.BoxBScan.Text = "Fast Scan";
            this.toolTip1.SetToolTip(this.BoxBScan, "Check to Scan all 18 Beacon Stations 5 Frequecies at each 10 second Interval\r\nPow" +
        "erSDR will move across all 5 Beacon Frequencies in 1 sec intervals \r\n\r\nTotal bea" +
        "con map is compled in 3 minutes.\r\n\r\n");
            this.BoxBScan.CheckedChanged += new System.EventHandler(this.BoxBScan_CheckedChanged);
            // 
            // chkBoxBeam
            // 
            this.chkBoxBeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxBeam.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxBeam.Image = null;
            this.chkBoxBeam.Location = new System.Drawing.Point(417, 437);
            this.chkBoxBeam.Name = "chkBoxBeam";
            this.chkBoxBeam.Size = new System.Drawing.Size(88, 24);
            this.chkBoxBeam.TabIndex = 83;
            this.chkBoxBeam.Text = "Map Beam°";
            this.toolTip1.SetToolTip(this.chkBoxBeam, "Check To Show Beam heading on map in (deg)\r\n");
            this.chkBoxBeam.CheckedChanged += new System.EventHandler(this.chkBoxBeam_CheckedChanged);
            // 
            // udDisplayLong
            // 
            this.udDisplayLong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayLong.DecimalPlaces = 2;
            this.udDisplayLong.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayLong.Location = new System.Drawing.Point(614, 489);
            this.udDisplayLong.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.udDisplayLong.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.udDisplayLong.Name = "udDisplayLong";
            this.udDisplayLong.Size = new System.Drawing.Size(62, 20);
            this.udDisplayLong.TabIndex = 80;
            this.toolTip1.SetToolTip(this.udDisplayLong, "Enter Longitude in deg (-180 to 180) for Beam Heading\r\n- for West of 0 GMT line\r\n" +
        "+ for East of 0 GMT line\r\n\r\nLeft Click on PowerSDR Display and Hit SHIFT key to " +
        "\r\ntoggle Lat/Long map");
            this.udDisplayLong.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDisplayLong.ValueChanged += new System.EventHandler(this.udDisplayLong_ValueChanged);
            // 
            // udDisplayLat
            // 
            this.udDisplayLat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayLat.DecimalPlaces = 2;
            this.udDisplayLat.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayLat.Location = new System.Drawing.Point(538, 489);
            this.udDisplayLat.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.udDisplayLat.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.udDisplayLat.Name = "udDisplayLat";
            this.udDisplayLat.Size = new System.Drawing.Size(58, 20);
            this.udDisplayLat.TabIndex = 79;
            this.toolTip1.SetToolTip(this.udDisplayLat, "Enter Latitude in deg (90 to -90) for Beam Heading\r\n+ for Northern Hemisphere\r\n- " +
        "for Southern Hemisphere\r\n\r\nLeft Click on PowerSDR Display and Hit SHIFT key to \r" +
        "\ntoggle Lat/Long map");
            this.udDisplayLat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDisplayLat.ValueChanged += new System.EventHandler(this.udDisplayLat_ValueChanged);
            // 
            // chkBoxMem
            // 
            this.chkBoxMem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxMem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxMem.Image = null;
            this.chkBoxMem.Location = new System.Drawing.Point(417, 459);
            this.chkBoxMem.Name = "chkBoxMem";
            this.chkBoxMem.Size = new System.Drawing.Size(123, 24);
            this.chkBoxMem.TabIndex = 74;
            this.chkBoxMem.Text = "MEMORIES to Pan";
            this.toolTip1.SetToolTip(this.chkBoxMem, resources.GetString("chkBoxMem.ToolTip"));
            this.chkBoxMem.CheckedChanged += new System.EventHandler(this.chkBoxMem_CheckedChanged);
            // 
            // chkBoxPan
            // 
            this.chkBoxPan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxPan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxPan.Image = null;
            this.chkBoxPan.Location = new System.Drawing.Point(417, 414);
            this.chkBoxPan.Name = "chkBoxPan";
            this.chkBoxPan.Size = new System.Drawing.Size(100, 24);
            this.chkBoxPan.TabIndex = 71;
            this.chkBoxPan.Text = "Map just Pan";
            this.toolTip1.SetToolTip(this.chkBoxPan, "Show Country or Calls on Map for just the Panadapter freq you are viewing.\r\n");
            this.chkBoxPan.CheckedChanged += new System.EventHandler(this.chkBoxPan_CheckedChanged);
            // 
            // chkBoxSSB
            // 
            this.chkBoxSSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSSB.Checked = true;
            this.chkBoxSSB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxSSB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxSSB.Image = null;
            this.chkBoxSSB.Location = new System.Drawing.Point(182, 384);
            this.chkBoxSSB.Name = "chkBoxSSB";
            this.chkBoxSSB.Size = new System.Drawing.Size(85, 24);
            this.chkBoxSSB.TabIndex = 69;
            this.chkBoxSSB.Text = "Spot Phone";
            this.toolTip1.SetToolTip(this.chkBoxSSB, "Show SSB spots when checked\r\n");
            // 
            // chkBoxCW
            // 
            this.chkBoxCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxCW.Checked = true;
            this.chkBoxCW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxCW.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxCW.Image = null;
            this.chkBoxCW.Location = new System.Drawing.Point(182, 356);
            this.chkBoxCW.Name = "chkBoxCW";
            this.chkBoxCW.Size = new System.Drawing.Size(85, 24);
            this.chkBoxCW.TabIndex = 68;
            this.chkBoxCW.Text = "Spot CW";
            this.toolTip1.SetToolTip(this.chkBoxCW, "Show CW spots when checked\r\n");
            // 
            // chkMapBand
            // 
            this.chkMapBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapBand.Checked = true;
            this.chkMapBand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMapBand.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapBand.Image = null;
            this.chkMapBand.Location = new System.Drawing.Point(417, 391);
            this.chkMapBand.Name = "chkMapBand";
            this.chkMapBand.Size = new System.Drawing.Size(113, 24);
            this.chkMapBand.TabIndex = 67;
            this.chkMapBand.Text = "Map just Band";
            this.toolTip1.SetToolTip(this.chkMapBand, "Show Country or Calls on Map for the Band you are on.\r\n");
            this.chkMapBand.CheckedChanged += new System.EventHandler(this.chkMapBand_CheckedChanged);
            // 
            // chkMapCountry
            // 
            this.chkMapCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapCountry.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapCountry.Image = null;
            this.chkMapCountry.Location = new System.Drawing.Point(417, 345);
            this.chkMapCountry.Name = "chkMapCountry";
            this.chkMapCountry.Size = new System.Drawing.Size(88, 22);
            this.chkMapCountry.TabIndex = 66;
            this.chkMapCountry.Text = "Map Country";
            this.toolTip1.SetToolTip(this.chkMapCountry, "Show Dx spot Countries on Map\r\n");
            this.chkMapCountry.CheckedChanged += new System.EventHandler(this.chkMapCountry_CheckedChanged);
            // 
            // chkMapCall
            // 
            this.chkMapCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapCall.Checked = true;
            this.chkMapCall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMapCall.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkMapCall.Image = null;
            this.chkMapCall.Location = new System.Drawing.Point(417, 368);
            this.chkMapCall.Name = "chkMapCall";
            this.chkMapCall.Size = new System.Drawing.Size(88, 24);
            this.chkMapCall.TabIndex = 65;
            this.chkMapCall.Text = "Map Calls";
            this.toolTip1.SetToolTip(this.chkMapCall, "Show DX Spot Call signs on Map");
            this.chkMapCall.CheckedChanged += new System.EventHandler(this.chkMapCall_CheckedChanged);
            // 
            // chkPanMode
            // 
            this.chkPanMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPanMode.Checked = true;
            this.chkPanMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPanMode.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkPanMode.Image = null;
            this.chkPanMode.Location = new System.Drawing.Point(273, 387);
            this.chkPanMode.Name = "chkPanMode";
            this.chkPanMode.Size = new System.Drawing.Size(148, 23);
            this.chkPanMode.TabIndex = 63;
            this.chkPanMode.Text = "Special PanaFall Mode\r\n";
            this.toolTip1.SetToolTip(this.chkPanMode, "When Checked, will Display RX1 in Panafall mode, with a small waterfall for bette" +
        "r viewing of the map");
            this.chkPanMode.CheckedChanged += new System.EventHandler(this.chkPanMode_CheckedChanged);
            // 
            // chkGrayLine
            // 
            this.chkGrayLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGrayLine.Checked = true;
            this.chkGrayLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGrayLine.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkGrayLine.Image = null;
            this.chkGrayLine.Location = new System.Drawing.Point(273, 368);
            this.chkGrayLine.Name = "chkGrayLine";
            this.chkGrayLine.Size = new System.Drawing.Size(105, 17);
            this.chkGrayLine.TabIndex = 61;
            this.chkGrayLine.Text = "GrayLine Track";
            this.toolTip1.SetToolTip(this.chkGrayLine, "GrayLine will show on Panadapter Display\r\nBut only when using KE9SN6_World skin o" +
        "nly\r\nAnd only when RX1 is in Panadapter Mode with RX2 Display OFF");
            this.chkGrayLine.CheckedChanged += new System.EventHandler(this.chkGrayLine_CheckedChanged);
            // 
            // chkSUN
            // 
            this.chkSUN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSUN.Checked = true;
            this.chkSUN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSUN.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkSUN.Image = null;
            this.chkSUN.Location = new System.Drawing.Point(273, 343);
            this.chkSUN.Name = "chkSUN";
            this.chkSUN.Size = new System.Drawing.Size(46, 24);
            this.chkSUN.TabIndex = 60;
            this.chkSUN.Text = "Sun";
            this.toolTip1.SetToolTip(this.chkSUN, "True (Zenith) position of the Sun will show on Panadapter screen \r\nWhen you Click" +
        " on the TRACK button to activate the World Map\r\nIn Panadapter or Special PanaFal" +
        "l modes");
            this.chkSUN.CheckedChanged += new System.EventHandler(this.chkSUN_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(537, 467);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Your Lat and Long (+/- deg)\r\n";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(693, 467);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 82;
            this.label4.Text = "Your Call sign";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Location = new System.Drawing.Point(541, 448);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "Setup->CAT Control->DDUtil , for Rotor Control";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSpotOptions,
            this.menuTimeServers});
            // 
            // mnuSpotOptions
            // 
            this.mnuSpotOptions.Index = 0;
            this.mnuSpotOptions.Text = "VOACAP Override";
            this.mnuSpotOptions.Click += new System.EventHandler(this.mnuSpotOptions_Click);
            // 
            // menuTimeServers
            // 
            this.menuTimeServers.Index = 1;
            this.menuTimeServers.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.chkTimeServer1,
            this.chkTimeServer2,
            this.chkTimeServer3,
            this.chkTimeServer4,
            this.chkTimeServer5});
            this.menuTimeServers.Text = "Time Servers List";
            // 
            // chkTimeServer1
            // 
            this.chkTimeServer1.Index = 0;
            this.chkTimeServer1.Text = "nist.netservicesgroup.com";
            this.chkTimeServer1.Click += new System.EventHandler(this.chkTimeServer1_Click);
            // 
            // chkTimeServer2
            // 
            this.chkTimeServer2.Index = 1;
            this.chkTimeServer2.Text = "utcnist.colorado.edu";
            this.chkTimeServer2.Click += new System.EventHandler(this.chkTimeServer2_Click);
            // 
            // chkTimeServer3
            // 
            this.chkTimeServer3.Index = 2;
            this.chkTimeServer3.Text = "time-c.nist.gov";
            this.chkTimeServer3.Click += new System.EventHandler(this.chkTimeServer3_Click);
            // 
            // chkTimeServer4
            // 
            this.chkTimeServer4.Index = 3;
            this.chkTimeServer4.Text = "time-b.nist.gov";
            this.chkTimeServer4.Click += new System.EventHandler(this.chkTimeServer4_Click);
            // 
            // chkTimeServer5
            // 
            this.chkTimeServer5.Index = 4;
            this.chkTimeServer5.Text = "time-a.nist.gov";
            this.chkTimeServer5.Click += new System.EventHandler(this.chkTimeServer5_Click);
            // 
            // chkBoxWrld
            // 
            this.chkBoxWrld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxWrld.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxWrld.Image = null;
            this.chkBoxWrld.Location = new System.Drawing.Point(12, 451);
            this.chkBoxWrld.Name = "chkBoxWrld";
            this.chkBoxWrld.Size = new System.Drawing.Size(194, 24);
            this.chkBoxWrld.TabIndex = 78;
            this.chkBoxWrld.Text = "Exclude North American Spotters";
            this.chkBoxWrld.CheckedChanged += new System.EventHandler(this.chkBoxWrld_CheckedChanged);
            // 
            // chkBoxNA
            // 
            this.chkBoxNA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxNA.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkBoxNA.Image = null;
            this.chkBoxNA.Location = new System.Drawing.Point(12, 433);
            this.chkBoxNA.Name = "chkBoxNA";
            this.chkBoxNA.Size = new System.Drawing.Size(175, 19);
            this.chkBoxNA.TabIndex = 77;
            this.chkBoxNA.Text = "North American Spotters only";
            this.chkBoxNA.CheckedChanged += new System.EventHandler(this.chkBoxNA_CheckedChanged);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(668, 415);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 24);
            this.chkAlwaysOnTop.TabIndex = 58;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // chkDXMode
            // 
            this.chkDXMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDXMode.Checked = true;
            this.chkDXMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDXMode.Image = null;
            this.chkDXMode.Location = new System.Drawing.Point(682, 488);
            this.chkDXMode.Name = "chkDXMode";
            this.chkDXMode.Size = new System.Drawing.Size(91, 24);
            this.chkDXMode.TabIndex = 59;
            this.chkDXMode.Text = "Parse \"DX Spot\" Mode";
            this.chkDXMode.UseVisualStyleBackColor = true;
            this.chkDXMode.Visible = false;
            // 
            // SpotControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(784, 520);
            this.Controls.Add(this.chkISS);
            this.Controls.Add(this.chkMoon);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBoxDXCall);
            this.Controls.Add(this.DXPost);
            this.Controls.Add(this.chkBoxContour);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.tbPanPower);
            this.Controls.Add(this.chkBoxAnt);
            this.Controls.Add(this.chkBoxDIG);
            this.Controls.Add(this.checkBoxMUF);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.checkBoxTone);
            this.Controls.Add(this.udDisplayWWV);
            this.Controls.Add(this.checkBoxWWV);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.numericUpDownTS1);
            this.Controls.Add(this.BoxBFScan);
            this.Controls.Add(this.BoxBScan);
            this.Controls.Add(this.btnBeacon);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkBoxBeam);
            this.Controls.Add(this.SWLbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.udDisplayLong);
            this.Controls.Add(this.udDisplayLat);
            this.Controls.Add(this.chkBoxWrld);
            this.Controls.Add(this.chkBoxNA);
            this.Controls.Add(this.SWLbutton2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.chkBoxMem);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.chkBoxPan);
            this.Controls.Add(this.chkBoxSSB);
            this.Controls.Add(this.chkBoxCW);
            this.Controls.Add(this.chkMapBand);
            this.Controls.Add(this.chkMapCountry);
            this.Controls.Add(this.chkMapCall);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.chkPanMode);
            this.Controls.Add(this.chkGrayLine);
            this.Controls.Add(this.chkSUN);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusBoxSWL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.portBox2);
            this.Controls.Add(this.callBox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.nodeBox1);
            this.Controls.Add(this.SSBbutton);
            this.Controls.Add(this.chkDXMode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(800, 1000);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "SpotControl";
            this.Text = "DX / SWL Spotter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpotControl_FormClosing);
            this.Load += new System.EventHandler(this.SpotControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpotControl_KeyDown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SpotControl_Layout);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPanPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayWWV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        } //initializecomponents





        #endregion

        #region Properties



        #endregion

        #region Event Handlers







        #endregion



        public static string callB = "callsign";                     // ke9ns add call sign for dx spotter
        public static string nodeB = "spider.ham-radio-deluxe.com";  // ke9ns add node for dx spotter
        public static string portB = "8000";                        // ke9ns add port# 
        public static string nameB = "HB9DRV-9>";                   // ke9ns add port# 


        public static string DXCALL  // this is called or set in console
        {
            get { return callB; }
            set
            {
                callB = value;

            } // set
        } // callsign

        public static string DXNODE  // this is called or set in console
        {
            get { return nodeB; }
            set
            {
                nodeB = value;

            } // set
        } // callsign
        public static string DXNAME  // this is called or set in console
        {
            get { return nameB; }
            set
            {
                nameB = value;

            } // set
        } // callsign
        public static string DXPORT  // this is called or set in console
        {
            get { return portB; }
            set
            {
                portB = value;

            } // set

        } // callsign



        private void SpotControl_Load(object sender, EventArgs e)
        {

            nameBox.Text = nameB;
            callBox.Text = callB;
            nodeBox1.Text = nodeB;
            portBox2.Text = portB;

            try
            {
                if (Convert.ToInt16(portBox2.Text) < 20)
                {
                    dataGridView1.CurrentCell = dataGridView1[0, Convert.ToInt16(portBox2.Text)];
                    Debug.WriteLine("retrieved the index from storage");

                }
            }
            catch (Exception)
            {
                dataGridView1.CurrentCell = dataGridView1[0, 0];

            }

        } // SpotControl_Load

        //=======================================================================================================================
        private void SpotControl_FormClosing(object sender, FormClosingEventArgs e)
        {
          //timer  SP5_Active = 0; // turn off map
         
            //  VOARUN = false; // voacap not running
           
          //  checkBoxMUF.Checked = false; // turn voacap mapping off
            

            if (timerID != 0)
            {
                timeKillEvent(timerID);     // kill the mmtimer
            }

            callB = callBox.Text;  // values to save in ke9ns.dat file
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;


            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SpotForm");

            console.DXMemList.Save1(); // save dx spotter list



        }

        public static byte SP_Active = 0;  // 1= DX Spot feature ON, 2=logging in 3=waiting for spots
        public static byte SP2_Active = 0; // DX Spot: 0=closed so ok to open again if you want, 1=in process of shutting down
        public static byte SP4_Active = 0; // 1=processing valid DX spot. 0=not processing new DX spot

        public static byte SP1_Active = 0; // SWL active
        public static byte SP3_Active = 0; // 1=SWL database loaded up, so no need to reload if you turn if OFF

        public static byte SP5_Active = 0; // 1= turn on track map (sun, grayline, voacap, or beacon)

        //=======================================================================================
        //=======================================================================================
        // ke9ns SWL spotter // www.eibispace.de to get sked.csv file to read
        public void SWLbutton_Click(object sender, EventArgs e)
        {
            string file_name = " ";


            Debug.WriteLine("LOOK FOR SWL FILE " + console.AppDataPath);
            file_name = console.AppDataPath + "SWL.csv"; //   eibispace.de  sked - b15.csv


            if (!File.Exists(file_name))
            {
                Debug.WriteLine("problem no SWL.CSV file found ");
                statusBoxSWL.ForeColor = Color.Red;

                statusBoxSWL.Text = "No SWL.csv file found";

                return;
            }

            if ((SP1_Active == 0))
            {
                SP1_Active = 1;


                //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                //   Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                Thread t = new Thread(new ThreadStart(SWLSPOTTER));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "SWL Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                //  while (t.IsAlive)
                //  {
                //      Thread.Sleep(50);
                //      Application.DoEvents();
                //  }


            }
            else
            {

               // console.swlsearchMenuItem1.Enabled = false;

                SP1_Active = 0; // turn off SWL spotter

                statusBoxSWL.ForeColor = Color.Red;
                statusBoxSWL.Text = "Off";


                if (SP_Active == 0)
                {
                    console.spotterMenu.ForeColor = Color.Red;
                    console.spotterMenu.Text = "Spotter";
                }


            } // SWL not active  



        } // SWLbutton_Click



        // these are pulled from SWL2.csv file
        public static string[] SWL2_Station = new string[20000];       // Station name
        public static int[] SWL2_Freq = new int[20000];              // in hz
        public static byte[] SWL2_Band = new byte[20000];              // in Mhz

        public static string[] SWL2_Lang = new string[20000];          // language of transmitter
        public static int[] SWL2_TimeN = new int[20000];                // UTC time of operation ON air
        public static int[] SWL2_TimeF = new int[20000];                // UTC time of operation OFF air
        public static string[] SWL2_Mode = new string[20000];          // operating mode
        public static string[] SWL2_Day = new string[20000];          // days of operation
        public static byte[] SWL2_Day1 = new byte[20000];          // days of operation mo,tu,we,th,fr,sa,su = 1,2,4,8,16,32,64


        public static string[] SWL2_Loc = new string[20000];          // location of transmitter
        public static string[] SWL2_Target = new string[20000];          // target area of station
        public static int SWL2_Index1;  // local index that reset back to 0 after reaching max
        public static byte Flag21 = 0; // flag to skip header line in SWL.csv file


        // these are pulled from SWL.csv file
        public static string[] SWL_Station = new string[20000];       // Station name
        public static int[] SWL_Freq = new int[20000];              // in hz
        public static byte[] SWL_Band = new byte[20000];              // in Mhz
        public static int[] SWL_BandL = new int[31];              // index for each start of mhz listed in swl.csv

        public static string[] SWL_Lang = new string[20000];          // language of transmitter
        public static int[] SWL_TimeN = new int[20000];                // UTC time of operation ON air
        public static int[] SWL_TimeF = new int[20000];                // UTC time of operation OFF air
        public static string[] SWL_Mode = new string[20000];          // operating mode
        public static string[] SWL_Day = new string[20000];          // days of operation
        public static byte[] SWL_Day1 = new byte[20000];          // days of operation mo,tu,we,th,fr,sa,su = 1,2,4,8,16,32,64

        public static string[] SWL_Loc = new string[20000];          // location of transmitter
        public static string[] SWL_Target = new string[20000];          // target area of station

        public static int[] SWL_Pos = new int[20000];                // related to W on the panadapter screen

        public static int SWL_Index;  //  max number of spots in memory currently
        public static int SWL_Index1;  // local index that reset back to 0 after reaching max
        public static int SWL_Index3;  //  
        public static int VFOHLast;

        public static int Lindex; // low index spot
        public static int Hindex; // high index spot

        public static FileStream stream2;          // for reading SWL.csv file
        public static BinaryReader reader2;

        public static byte Flag1 = 0; // flag to skip header line in SWL.csv file

        public static DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        public static byte UTCDD = (byte)(1 << ((byte)UTCD.DayOfWeek));   // this is the day. SUn = 0, Mon = 1



        public static string FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW

        public static int UTCNEW1 = Convert.ToInt16(FD);                                       // convert 24hr UTC to int





        //=======================================================================================
        //=======================================================================================
        //ke9ns start SWL spotting THREAD 1 and done
        private void SWLSPOTTER()
        {

            string file_name = " ";
            string file_name1 = " ";

            int FLAG22 = 0;


            file_name = console.AppDataPath + "SWL.csv"; //  sked - b15.csv  
            file_name1 = console.AppDataPath + "SWL2.csv"; // ke9ns extra swl freq that eibispace.de wont add



            //-------------------------------------- SWL2.csv (ke9ns list of extra swl freqs to load)
            if (File.Exists(file_name1))
            {

                try
                {
                    stream2 = new FileStream(file_name1, FileMode.Open); // open file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);


                }
                catch (Exception)
                {
                    goto SWL1; // no extra SWL2 file, so just use SWL.csv file instead

                }

                var result = new StringBuilder();

                if (SP3_Active == 0) // dont reset if already scanned in  database
                {
                    SWL2_Index1 = 0; // how big is the SWL.CSV data file in lines
                    Flag21 = 0;

                }

                statusBoxSWL.Text = "Reading SWL2 ";

                Debug.WriteLine(" Reading SWL2 file");

                //------------------------------------------------------------------
                for (;;)
                {

                    if (SP3_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    if (SP1_Active == 0)
                    {

                        reader2.Close();    // close  file
                        stream2.Close();   // close stream

                        statusBoxSWL.ForeColor = Color.Black;
                        statusBoxSWL.Text = "Off";

                        if (SP_Active == 0)
                        {
                            console.spotterMenu.ForeColor = Color.White;
                            console.spotterMenu.Text = "Spotter";
                        }

                        return;
                    }

                    statusBoxSWL.ForeColor = Color.Red;
                    //    statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Yellow;
                        console.spotterMenu.Text = "Reading";
                    }

                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            if (Flag21 == 1)
                            {

                                string[] values;

                                values = result.ToString().Split(';'); // split line up into segments divided by ;

                                SWL2_Freq[SWL2_Index1] = (int)(Convert.ToDouble(values[0]) * 1000); // get freq and convert to hz
                                SWL2_Band[SWL2_Index1] = (byte)(SWL2_Freq[SWL2_Index1] / 1000000); // get freq and convert to mhz

                                SWL2_TimeN[SWL2_Index1] = Convert.ToInt16(values[1].Substring(0, 4)); // get time ON (24hr 4 digit UTC)
                                SWL2_TimeF[SWL2_Index1] = Convert.ToInt16(values[1].Substring(5, 4)); // get time OFF
                                SWL2_Day[SWL2_Index1] = values[2]; // get days ON

                                SWL2_Day1[SWL2_Index1] = 127; // digital signals are on 7 days

                                SWL2_Loc[SWL2_Index1] = values[3]; // get location of station
                                SWL2_Mode[SWL2_Index1] = "USB"; // get opeating mode
                                SWL2_Station[SWL2_Index1] = values[4]; // get station name
                                SWL2_Lang[SWL2_Index1] = values[5]; // get language
                                SWL2_Target[SWL2_Index1] = values[6]; // get station target area

                                SWL2_Index1++;

                            } // SWL Spots
                            else Flag21 = 1;

                            result = new StringBuilder(); // clean up for next line

                        }
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        SWL2_Index1--;
                        // textBox1.Text = "End of SWL FILE at "+ SWL_Index1.ToString();
                        Debug.WriteLine("END OF SWL2_Freq[SWL2_Index1] " + SWL2_Freq[SWL2_Index1] +" and last line# is " + SWL2_Index1);
                        break; // done with file
                    }
                    catch (Exception)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        break; // done with file
                    }


                } // for loop until end of file is reached


                // Debug.WriteLine("reached SWL end of file");


                reader2.Close();    // close  file
                stream2.Close();   // close stream


            } // if file exists SWL2


            // at this point we have SWL2 data read in

            SWL1:;



            //-------------------------------------- SWL.csv
            // SWL.CSV now requires a high freq end of file to allow merging SWL2.csv
            //This needs to be added to the bottom of SWL.csv:  900000; 0000 - 2400; ; USA; ENDOFFILE; E; USA; ; 1;;

            if (File.Exists(file_name))
            {

                try
                {
                    stream2 = new FileStream(file_name, FileMode.Open); // open file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);
                }
                catch (Exception)
                {
                    SP1_Active = 0; // turn off SWL spotter

                    statusBoxSWL.ForeColor = Color.Red;
                    statusBoxSWL.Text = "Off";


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Red;
                        console.spotterMenu.Text = "Spotter";
                    }
                    return;
                }

                var result = new StringBuilder();

                if (SP3_Active == 0) // dont reset if already scanned in  database
                {
                    SWL_Index1 = 0; // how big is the SWL.CSV data file in lines
                    SWL_Index = 0; // was 0  start at 1 mhz
                    Flag1 = 0;

                }
                statusBoxSWL.Text = "Reading ";

                Debug.WriteLine("READING SWL file");

                //------------------------------------------------------------------
                for (;;)
                {

                    if (SP3_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    if (SP1_Active == 0)
                    {

                        reader2.Close();    // close  file
                        stream2.Close();   // close stream

                        statusBoxSWL.ForeColor = Color.Black;
                        statusBoxSWL.Text = "Off";

                        if (SP_Active == 0)
                        {
                            console.spotterMenu.ForeColor = Color.White;
                            console.spotterMenu.Text = "Spotter";
                        }

                        return;
                    }

                    statusBoxSWL.ForeColor = Color.Red;
                    //    statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Yellow;
                        console.spotterMenu.Text = "Reading";
                    }


                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                         //   Debug.WriteLine("SWL LINE: " + result);
                            
                            if (Flag1 == 1)
                            {

                                string[] values;

                                values = result.ToString().Split(';'); // split line up into segments divided by ;

                                SWL_Freq[SWL_Index1] = (int)(Convert.ToDouble(values[0]) * 1000); // get freq and convert to hz
                                SWL_Band[SWL_Index1] = (byte)(SWL_Freq[SWL_Index1] / 1000000); // get freq and convert to mhz

                                /*
                                                                    if (SWL_Band[SWL_Index1] > SWL_Index)
                                                                    {
                                                                        //  Debug.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1);
                                                                        SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                                                        VFOHLast = 0; // refresh pan screen while loading
                                                                        SWL_Index++;
                                                                    }
                                */
                                SWL_TimeN[SWL_Index1] = Convert.ToInt16(values[1].Substring(0, 4)); // get time ON (24hr 4 digit UTC)
                                SWL_TimeF[SWL_Index1] = Convert.ToInt16(values[1].Substring(5, 4)); // get time OFF


                                SWL_Day1[SWL_Index1] = 0;


                                //---------------------------------------------------------------------------------------
                                // ke9ns look at daysofweek on the air sun=0,mon=1,tue=2, etc
                                if (values[2].Contains("+"))
                                {
                                    SWL_Day1[SWL_Index1] = 0; // to detect +-

                                }
                                else if (values[2].Contains("-"))
                                {
                                    byte temp3 = 0;
                                    byte temp4 = 0;


                                    string temp1 = values[2].Substring(0, 2); // start day
                                    string temp2 = values[2].Substring(3, 2); // end day


                                    if (temp1 == "Tu") temp3 = 2;             // get your start day position
                                    else if (temp1 == "We") temp3 = 3;
                                    else if (temp1 == "Th") temp3 = 4;
                                    else if (temp1 == "Fr") temp3 = 5;
                                    else if (temp1 == "Sa") temp3 = 6;
                                    else if (temp1 == "Su") temp3 = 0;
                                    else temp3 = 1; // mon

                                    if (temp2 == "Tu") temp4 = 2;           // get your end day position
                                    else if (temp2 == "We") temp4 = 3;
                                    else if (temp2 == "Th") temp4 = 4;
                                    else if (temp2 == "Fr") temp4 = 5;
                                    else if (temp2 == "Sa") temp4 = 6;
                                    else if (temp2 == "Su") temp4 = 0;
                                    else temp4 = 1; // mon

                                    if (temp3 < temp4) // example su thru fr
                                    {
                                        for (int x = temp3; x <= temp4; x++)
                                        {
                                            SWL_Day1[SWL_Index1] |= (byte)(1 << x);
                                        }
                                    } // example su thru sa
                                    else // example fr thru tu
                                    {
                                        for (int x = temp3; x < 7; x++)
                                        {
                                            SWL_Day1[SWL_Index1] |= (byte)(1 << x);
                                        }
                                        for (int x = 0; x <= temp4; x++)
                                        {
                                            SWL_Day1[SWL_Index1] |= (byte)(1 << x);
                                        }


                                    } // example fr thru tu


                                } // contains -
                                else
                                {
                                    if (values[2].Contains("Mo")) SWL_Day1[SWL_Index1] |= 2;
                                    if (values[2].Contains("Tu")) SWL_Day1[SWL_Index1] |= 4;
                                    if (values[2].Contains("We")) SWL_Day1[SWL_Index1] |= 8;
                                    if (values[2].Contains("Th")) SWL_Day1[SWL_Index1] |= 16;
                                    if (values[2].Contains("Fr")) SWL_Day1[SWL_Index1] |= 32;
                                    if (values[2].Contains("Sa")) SWL_Day1[SWL_Index1] |= 64;
                                    if (values[2].Contains("Su")) SWL_Day1[SWL_Index1] |= 1; // 64

                                } // this checks for Mo,Tu,Sa  etc. etc.



                                if (SWL_Day1[SWL_Index1] == 0) SWL_Day1[SWL_Index1] = 127; // if no days then all 7 days
                                SWL_Day[SWL_Index1] = values[2]; // get days ON

                                //--------------------------------------------------------------------
                                //  if (SWL_Freq[SWL_Index1] == 7315000) Debug.WriteLine("station found" + SWL_Freq[SWL_Index1] + " , "+ SWL_Day1[SWL_Index1]);


                                SWL_Loc[SWL_Index1] = values[3]; // get location of station
                                SWL_Mode[SWL_Index1] = "AM"; // get opeating mode
                                SWL_Station[SWL_Index1] = values[4]; // get station name
                                SWL_Lang[SWL_Index1] = values[5]; // get language
                                SWL_Target[SWL_Index1] = values[6]; // get station target area



                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                // Ke9ns MERGE SWL and SWL2
                                FLAG22 = 0; // reset for next line

                                if ((SWL2_Index1 > 0) && (SWL_Index1 > 2)) // only try and merge SWL2 into SWL if SWL2 exists
                                {
                                    int lowfreq = SWL_Freq[SWL_Index1 - 1]; // prior freq read in
                                    int highfreq = SWL_Freq[SWL_Index1]; // freq just read in now


                                    // now check to see if any SWL2 freqs can fit in between lines of the SWL file
                                    for (int q = 0; q <= SWL2_Index1; q++)
                                    {

                                        if ((SWL2_Freq[q] < highfreq)) // are you below the current (just read in) swl listing?
                                        {
                                            if ((SWL2_Freq[q] >= lowfreq)) // are you above the last read in swl listing?
                                            {
                                                // move this "just read in" SWL line forward in the index to insert SWL2 entry
                                                SWL_Freq[SWL_Index1 + 1] = SWL_Freq[SWL_Index1];

                                                SWL_Band[SWL_Index1 + 1] = SWL_Band[SWL_Index1];

                                                SWL_TimeN[SWL_Index1 + 1] = SWL_TimeN[SWL_Index1];
                                                SWL_TimeF[SWL_Index1 + 1] = SWL_TimeF[SWL_Index1];
                                                SWL_Day[SWL_Index1 + 1] = SWL_Day[SWL_Index1];
                                                SWL_Day1[SWL_Index1 + 1] = SWL_Day1[SWL_Index1];
                                                SWL_Loc[SWL_Index1 + 1] = SWL_Loc[SWL_Index1];
                                                SWL_Mode[SWL_Index1 + 1] = SWL_Mode[SWL_Index1];
                                                SWL_Station[SWL_Index1 + 1] = SWL_Station[SWL_Index1];
                                                SWL_Lang[SWL_Index1 + 1] = SWL_Lang[SWL_Index1];
                                                SWL_Target[SWL_Index1 + 1] = SWL_Target[SWL_Index1];


                                                // save SWL2 entry into SWL listing 
                                                SWL_Freq[SWL_Index1] = SWL2_Freq[q];

                                                SWL_Band[SWL_Index1] = SWL2_Band[q];

                                                SWL_TimeN[SWL_Index1] = SWL2_TimeN[q];
                                                SWL_TimeF[SWL_Index1] = SWL2_TimeF[q];
                                                SWL_Day[SWL_Index1] = SWL2_Day[q];
                                                SWL_Day1[SWL_Index1] = SWL2_Day1[q];
                                                SWL_Loc[SWL_Index1] = SWL2_Loc[q];
                                                SWL_Mode[SWL_Index1] = SWL2_Mode[q];
                                                SWL_Station[SWL_Index1] = SWL2_Station[q];
                                                SWL_Lang[SWL_Index1] = SWL2_Lang[q];
                                                SWL_Target[SWL_Index1] = SWL2_Target[q];

                                                //  Debug.WriteLine("INSERT 2 HERE= index=" + SWL_Index1 + " Freq=" + SWL_Freq[SWL_Index1] + " station name=" + SWL_Station[SWL_Index1]);

                                                FLAG22 = 1; // flag that you inserted a new SWL2 line into SWL

                                                if (SWL_Band[SWL_Index1] > SWL_Index) // MHZ of the current examined spot > the mhz your looking at?
                                                {
                                                    //  Debug.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1 + "swl_Band[]" + SWL_Band[SWL_Index1] + " station name: " + SWL_Station[SWL_Index1] + " Freq: " + SWL_Freq[SWL_Index1]);
                                                    SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                                    VFOHLast = 0; // refresh pan screen while loading
                                                    SWL_Index++;
                                                }

                                                SWL_Index1++; // add SWL2 into the SWL list

                                            } // if ( (SWL2_Freq[q] >= lowfreq)) // are you above the last read in swl listing?

                                        } //  if ((SWL2_Freq[q] < highfreq) ) // are you below the current (just read in) swl listing?
                                        else
                                        {
                                            break; // break out of this SWL2 loop
                                        }


                                    } // for loop through SWL2

                                } // if ((SWL2_Index1 > 0) && (SWL_Index1 > 2))

                                // below is to shorten the amount of time the swl routine in display.cs needs to find the swl listings to display in the pan
                                if (SWL_Band[SWL_Index1] > SWL_Index) // MHZ of the current examined spot > the mhz your looking at?
                                {
                                    //  Debug.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1 +"swl_Band[]" + SWL_Band[SWL_Index1] + " station name: " + SWL_Station[SWL_Index1] + " Freq: " + SWL_Freq[SWL_Index1]);
                                    SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                    VFOHLast = 0; // refresh pan screen while loading
                                    SWL_Index++;
                                }

                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                // check for DUPS
                                if (SWL_Index > 0)
                                {
                                    //-------------------------------------------------------------------------------------------------
                                    // ke9ns if the Station Name and Station Freq and Station Days are the same, then check time (below)
                                    if ((SWL_Station[SWL_Index1 - 1] == SWL_Station[SWL_Index1]) &&     // same station NAME
                                        (SWL_Freq[SWL_Index1 - 1] == SWL_Freq[SWL_Index1]) &&           // same Freq
                                        (SWL_Day1[SWL_Index1 - 1] == SWL_Day1[SWL_Index1]))             // same Days on the air
                                    {
                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] < SWL_TimeN[SWL_Index1]))       // first spot has earlier start time than this spot, then do below
                                        {
                                            if (SWL_TimeF[SWL_Index1 - 1] >= SWL_TimeN[SWL_Index1])    // if the first spot stays on the air past the start of the new spot, then do below
                                            {
                                                if (SWL_TimeF[SWL_Index1 - 1] < SWL_TimeF[SWL_Index1]) // if the first spot leaves the air before the new spot leaves the air then use the new spots finish time
                                                {
                                                    SWL_TimeF[SWL_Index1 - 1] = SWL_TimeF[SWL_Index1];
                                                }

                                                goto BYPASS; // duplicate
                                            }
                                        }

                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] > SWL_TimeN[SWL_Index1]))       // first spot has later start time than this new spot
                                        {
                                            if (SWL_TimeF[SWL_Index1 - 1] >= SWL_TimeN[SWL_Index1])    // if the first spot stays on the air past the start of the new spot, then do below
                                            {
                                                SWL_TimeN[SWL_Index1 - 1] = SWL_TimeN[SWL_Index1];     // use earlier time from new spot

                                                if (SWL_TimeF[SWL_Index1 - 1] < SWL_TimeF[SWL_Index1])
                                                {
                                                    SWL_TimeF[SWL_Index1 - 1] = SWL_TimeF[SWL_Index1];
                                                }

                                                goto BYPASS; // duplicate
                                            }

                                        }

                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] == SWL_TimeN[SWL_Index1]))   // if the start time matches do below
                                        {
                                            if (SWL_TimeF[SWL_Index1 - 1] < SWL_TimeF[SWL_Index1]) // if the next in the list stays on the air longer, use its end time and bypass
                                            {
                                                SWL_TimeF[SWL_Index1 - 1] = SWL_TimeF[SWL_Index1];
                                                goto BYPASS; // duplicate
                                            }

                                        } // if time on matches

                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] == SWL_TimeN[SWL_Index1])             // same ON time
                                            && (SWL_TimeF[SWL_Index1 - 1] == SWL_TimeF[SWL_Index1]))         //smae OFF time
                                        {
                                            goto BYPASS; // duplicate
                                        }


                                    } // name and freq match

                                }  //  if (SWL_Index > 0)

                                SWL_Index1++; // save this

                                BYPASS:;


                            } // SWL Spots
                            else Flag1 = 1;

                            result = new StringBuilder(); // clean up for next line

                        }
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        SWL_Index1--;

                        break; // done with file
                    }
                    catch (Exception)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        break; // done with file
                    }


                } // for loop until end of file is reached


                // Debug.WriteLine("reached SWL end of file");


                reader2.Close();    // close  file
                stream2.Close();   // close stream





                SP3_Active = 1; // done loading swl database (Good)

                statusBoxSWL.ForeColor = Color.Blue;
                statusBoxSWL.Text = "SWL Spotting " + SWL_Index1.ToString();

             //   console.swlsearchMenuItem1.Enabled = true;


                if (SP_Active == 0)
                {
                    console.spotterMenu.ForeColor = Color.Yellow;
                    console.spotterMenu.Text = "SWL Spot";
                }

                //==============================================================================
                //==============================================================================
                // ke9ns process lines of SWL data here

            } // if file exists





            //-----------------------------------------
            // ke9ns track ISS



            /*
            try
            {

                // http://api.open-notify.org/iss-now.json

                textBox1.Text += "Attempt to connect to ISS Now http \r\n";

                serverPath = "http://api.open-notify.org/iss-now.json";

            
                HttpWebRequest request = WebRequest.Create(serverPath) as HttpWebRequest;

                textBox1.Text += "Attempt to download ISS position \r\n";

                using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());


                   Response jsonResponse = objResponse as Response;

                }




                //  Stream responseStream = response.GetResponseStream();
                //   StreamReader reader = new StreamReader(responseStream);
                //    noaa = reader.ReadToEnd();

                //  reader.Close();
                //   response.Close();
                //   Debug.WriteLine("noaa=== " + noaa);

                textBox1.Text += "ISS  Download complete \r\n";

            }
            catch (Exception ex)
            {
                //   Debug.WriteLine("ISS fault=== " + ex);
                textBox1.Text += "Failed to get ISS data \r\n";

            }
*/

        } // SWLSPOTTER








        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //ke9ns start DX spotting
        public void spotSSB_Click(object sender, EventArgs e)
        {
            //   Debug.WriteLine("TESt");
            //  Debug.WriteLine("========row " + dataGridView1.CurrentCell.RowIndex);
            //   Debug.WriteLine("========URL " + (string)dataGridView1["dxurl", dataGridView1.CurrentCell.RowIndex].Value);


            button2.Focus();
          

            if ((SP2_Active == 0) && (SP_Active == 0) && (callBox.Text != "callsign") && (callBox.Text != null) ) // dont allow dx spotting while beacon is on.
            {

                Debug.WriteLine("DX SPOTTER ON start THREAD");

                Thread t = new Thread(new ThreadStart(SPOTTER));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                SP_Active = 1;
                t.Name = "Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal; // normal
                t.Start();

                textBox1.Text = "Clicked to Open DX Spider \r\n";

            }
            else if ((callBox.Text == "callsign") || (callBox.Text == null))
            {

                Debug.WriteLine("callbox " + callBox.Text);

                textBox1.Text += "Must put your CALL Sign in the CALLSIGN box (lower Right of this window)\r\n";
                callBox.ForeColor = Color.Red;
                callBox.BackColor = Color.Yellow;


            }
            else
            {
                Debug.WriteLine("DX SPOTTER OFF, Thread not started " + SP2_Active + " , " + SP_Active + " , " + beacon1);

                SP_Active = 0; // turn off DX Spotter

                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                console.spotterMenu.Text = "Closing";
                statusBox.Text = "Closing";

                textBox1.Text += "Clicked to Close Socket (click again to Force Closed)\r\n";

                if (SP2_Active != 0)
                {
                    textBox1.Text += "Force closed \r\n";

                    try
                    {
                        SP_writer.Close();
                        SP_reader.Close();
                    }
                    catch (Exception)
                    {
                        Debug.Write("writer/reader was not open to close");
                    }

                    try
                    {

                        networkStream.Close();
                        client.Close();
                    }
                    catch (Exception)
                    {
                        Debug.Write("networkstream was never open to close");
                    }

                    SP_Active = 0; // turn off DX Spotter
                    SP2_Active = 0; // turn off DX Spotter
                }
                else SP2_Active = 1; // in process of shutting down.


            } // turn DX spotting off


        } //  spotSSB_Click




        //====================================================================================================
        //====================================================================================================
        // ke9ns add Thread routine

        public static string[] SP_Time;
        public static string[] SP_Freq;
        public static string[] SP_Call;
        public static TcpClient client;                                               //           ' socket

        public static NetworkStream networkStream;                                   //         ' stream
        public static BinaryWriter SP_writer;
        public static BinaryReader SP_Reader;

        public static StreamReader SP_reader;
        public static StreamWriter SP_Writer;

        public static string message1; // DX messages
        public static string message2; // blank start
        public static string message3; // login messages


        public static string[] DX_FULLSTRING = new string[1000];    // full undecoded message

        public static string[] DX_Station = new string[1000];       // Extracted: dx call sign
        public static int[] DX_Freq = new int[1000];                // Extracted: Freq in hz
        public static string[] DX_Spotter = new string[1000];       // Extracted: spotter call sign
        public static string[] DX_Message = new string[1000];       // Extracted: message
        public static int[] DX_Mode = new int[1000];                // Parse: Mode from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am
        public static int[] DX_Mode2 = new int[1000];               // Parse: split parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] DX_Time = new int[1000];                // Extracted: GMT (Unreliable because its the time submitted by the spotter)
        public static string[] DX_Grid = new string[1000];          // Extracted and Parsed: grid square 
        public static string[] DX_Age = new string[1000];           // Calculated: how old is the spot

        public static int[] DX_Beam = new int[1000];                // Calculated: beam heading from your lat/long

        public static int[] DX_X = new int[1000];                   // Calculated: x pixel location on map (before any scaling) Longitude
        public static int[] DX_Y = new int[1000];                   // Calculated: y pixel location on map (before any scaling) Latitude
        public static string[] DX_country = new string[1000];       // Calculated: country  by matching the callsign pulled from DXLOC.txt file

      


        //-------------------------------------------------------------------------------------

        // holds information from NCDXF/IARU Beacon stations
        // PowerSDR will scan through 14.1, 18.11, 21.15, 24.93, 28.2 mhz
        // looking for 18 stations: 4U1UN, VE8AT, W6WX, KH6RS, ZL6B, VK6RBP, 
        // JA1IGY, RR9O, VR2B, 4S7B, ZS6DN, 5Z4B, 4X6TU, OH2B, CS3B, LU4AA,
        // OA4B, and YV5B in 10 second intervals.Thats 5 frequecies and 18 stations
        // rotating in 10 intervals = 10 * 18 = 180second = 3minutes until a repeat.
        //
        public static string[] BX_FULLSTRING = new string[100];    // full undecoded message

        public static string[] BX_Station = new string[100];       // Extracted: dx call sign
        public static int[] BX_Freq = new int[100];                // Extracted: Freq in hz
        public static string[] BX_Spotter = new string[100];       // Extracted: spotter call sign
        public static string[] BX_Message = new string[100];       // Extracted: message
        public static int[] BX_Mode = new int[100];                // Parse: Mode from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am
        public static int[] BX_Mode2 = new int[100];               // Parse: split parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] BX_Time = new int[100];                // Extracted: GMT (Unreliable because its the time submitted by the spotter)
        public static string[] BX_Grid = new string[100];          // Extracted and Parsed: grid square 
        public static string[] BX_Age = new string[100];           // Calculated: how old is the spot

        public static int[] BX_Beam = new int[100];                // Calculated: beam heading from your lat/long

        public static int[] BX_X = new int[100];                   // Calculated: x pixel location on map (before any scaling) Longitude
        public static int[] BX_Y = new int[100];                   // Calculated: y pixel location on map (before any scaling) Latitude
        public static string[] BX_country = new string[100];       // Calculated: country  by matching the callsign pulled from DXLOC.txt file

        public static int[] BX_dBm = new int[100];                // place to record the signal strength of received stations in dbm
        public static int[] BX_dBm1 = new int[100];               // place to record the noise floor of each freq
        public static int   BX_dBm2 =0;                           // avg value base line db passed back from display.cs
        public static int[] BX_dBm3 = new int[100];               // place to record the background signal strength of received stations as a S#

        public static int[] BX_TSlot = new int[100];              // time slot to hear this particular station of this particular freq (0 to 180 seconds)

        public static int[] BX_TSlot1 = new int[100];              // time slot for each of the 5 freq and 18 stations (0 to 170) but there are 5 of every time, 5 0's, 5 10sec, 5 20sec

        public static int BX_TSlot2 = 0;                         // time slot currently viewed 0 to 170 in 10sec increments

        public static int[] BX_Index = new int[5];                //  keep track of which freq (0-4) you are on for each station 

        public int BX1_Index = 0;                                 // should always be 90 for NCDXF beacons (5 x 18). Index for entire Beacon list (just like DX_Index for Dx spotter) 

        public bool BX_Load = false;   // true = BX_ values above all loaded 1 time, so no need to do it again. this way you can flip beacon on/off and see your last scan data

        //-------------------------------------------------------------------------------------


        public static int DX_Index = 0;                             //  max number of spots in memory currently
        public static int DX_Index1 = 0;                            //  static temp index holder....always 250
        public static int DX_Last = 0;                              //  last # in DX_Index (used for DXLOC_Mapper)spotter(
        public static int Map_Last = 0;                             //  last map checkbox change (used for DXLOC_Mapper) 1=update grayline 2=update spots on map only
        public static int DXK_Last = 0;                             //  last # in console.DXK (used for DXLOC_Mapper)


 
        public static int UTCNEW = Convert.ToInt16(FD);                        // convert 24hr UTC to int
        public static int UTCLAST = 0;                                        // last utc time for determining when to check again
        public static int UTCLASTMIN = 0;                                        // last utc time for determining when to check again

        private bool detectEncodingFromByteOrderMarks = true;

        private bool pause = false; // true = pause dx spot window update.

       
        //====================================================================================================
        //====================================================================================================
        // ke9ns add Thread routine (get DX spots)

        private void SPOTTER()  // ke9ns Thread opeation (runs in en-us culture) opens internet connection to genearte list of dx spots
        {

           DXLOC_FILE(); // open DXLOC.txt file and put into array of lat/lon values vs prefix

         
            try // opening socket
            {
                textBox1.Text += "Attempt Opening socket \r\n";

                client = new TcpClient(); // for new socket

            
                DXMemRecord nodeBox5 = new DXMemRecord(console.DXMemList.List[dataGridView1.CurrentCell.RowIndex]); // ke9ns 

                nodeBox1.Text = nodeBox5.DXURL; // get string from DXMemory file based on current pointed to index

                Debug.WriteLine("node " + nodeBox1.Text);
              
             
                if (nodeBox1.Text.Contains(":") == true)
                {
                    int ind = nodeBox1.Text.IndexOf(":") + 1;
                    int ind1 = nodeBox1.Text.Length;

                    portBox2.Text = dataGridView1.CurrentCell.RowIndex.ToString(); // to store

                    string PORT1 = nodeBox1.Text.Substring(ind, ind1 - ind);
                    string URL1 = nodeBox1.Text.Substring(0, ind - 1);

                    Debug.WriteLine("url " + URL1);
                    Debug.WriteLine("port " + PORT1);
                    Debug.WriteLine("index " + portBox2.Text);


                    client.Connect(URL1, Convert.ToInt16(PORT1));      // 'EXAMPLE  client.Connect("192.168.0.149", 230) 
                }
                else
                {
                    Debug.WriteLine("NO PORT# detected us 7000 " );

                    client.Connect(nodeBox1.Text, 7000);      // 'EXAMPLE  client.Connect("192.168.0.149", 230) 
                }

               
                networkStream = client.GetStream();

                SP_reader = new StreamReader(networkStream,Encoding.ASCII,detectEncodingFromByteOrderMarks); //Encoding.UTF8  or detectEncodingFromByteOrderMarks
                SP_writer = new BinaryWriter(networkStream,Encoding.UTF7);
   
               
                var sb = new StringBuilder(message2);
                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                textBox1.Text += "Got Socket \r\n";


                for (; SP_Active > 0;) // shut down socket and thread if SP_Active = 0  (0=off, 1=turned on, 2=logging , 3=waiting for spots)
                {
   
                    if (SP_Active == 1) // if you shut down dont attempt to read next spot
                    {
                        sb.Append((char)SP_reader.Read(), 1);

                        message3 = sb.ToString();

                        if ((message3.Contains("login: ")) || (message3.Contains("Please enter your call: ")))
                        {
                            textBox1.Text += "Got login: prompt \r\n";

                           
                            sb = new StringBuilder(message2); // clear sb string over

                          
                            char[] message5 = callBox.Text.ToCharArray(); // get your call sign

                            for (int i = 0; i < message5.Length; i++)    // do it this way because telnet server wants slow typing
                            {
                                SP_writer.Write((char)message5[i]);
                             
                            }  // for loop length of your call sign

                            SP_writer.Write((char)13);
                        
                            SP_writer.Write((char)10);
                       
                           
                            statusBox.ForeColor = Color.Red;
                            console.spotterMenu.ForeColor = Color.Red;

                            statusBox.Text = "Login";
                            console.spotterMenu.Text = "Login";


                            SP_Active = 2; // logging in

                        } // look for login:

                    } // SP_active = 1
                    else if (SP_Active == 2)
                    {
                        SP_Active = 3; // logging in
                   
                        statusBox.ForeColor = Color.Green;
                        console.spotterMenu.ForeColor = Color.Blue;

                        statusBox.Text = "Spotting";
                        console.spotterMenu.Text = "Spotting";

                        textBox1.Text += "Waiting for DX Spots \r\n";

                        DXPost.Enabled = true;
                        textBoxDXCall.Enabled = true;

                      //  DX_Index = 0; // start at begining

                    } // SP_active == 2


                    //------------------------------------------------------------------------
                    // ke9nsformat:  
                    // 0     6            23 26          39                  70    76   80818283 (83 with everything or 79 with no Grid)
                    // DX de ke9ns:  7003.5  kc9ffv      up 5                0204Z en52 \a\a\r\n
                    //------------------------------------------------------------------------
                    else if (SP_Active > 2)
                    {
                        sb = new StringBuilder(); // clear sb string over again

                        try // use try since its a socket and can fail
                        {
                         //  SP_reader.BaseStream.ReadTimeout = 3000; // 5000 cause character Read to break every 5 seconds to check age of DX spots

                      //-------------------------------------------------------------------------------------------------------------------------------------
                      // ke9ns wait for a new message

                            for (;!(sb.ToString().Contains("\r\n"));) //  wait for end of line
                            {
                                processDXAGE();

                                Thread.Sleep(50); // slow down the thread here

                                sb.Append((char)SP_reader.Read());  // get next char from socket and add it to build the next dx spot string to parse out 
                             

                                if (SP_Active == 0)
                                 {
                                    Debug.WriteLine("break====="); // if user wants to shut down operation 
                                    break;
                                }

                                if (sb.ToString().Length > 90)
                                {
                                    Debug.WriteLine("Leng ====="); // string too long (something happened
                                    sb = new StringBuilder(); // clear sb string over again
                                }


                            }// for (;!(sb.ToString().Contains("\r\n"));) //  wait for end of line
                      //-------------------------------------------------------------------------------------------------------------------------------------


                            statusBox.ForeColor = Color.Green;
                            statusBox.Text = "Spotting";
                            SP_Active = 3;

                          
                            sb.Replace("\a", "");// get rig of bell 
                            sb.Replace("\r", "");// get rig of cr 
                            sb.Replace("\n", "");// get rig of line feed 

                            int qq = sb.Length;
                          // Debug.WriteLine("message1 length " + qq);

                            if (qq == 75) // if no grid, then add spaces and CR and line feed
                            {
                                sb.Append("     "); // keep all strings the same length
                            }
                           
                            message1 = sb.ToString(); // post message
                            message1.TrimEnd('\0');

                            // ke9ns so at this point all messages are 82 characters long (as though they have a grid#, even if they dont)
                            //   Debug.WriteLine("message2 length " + message1.Length);

                        }
                        catch // read timeout comes here
                        {
                            //  processDXAGE();
                            //   if (Flag8 == 0) continue; // if DX_Index value changed due to age, then proceess otherwise continue around
                            //  Flag8 = 0;

                            continue;

                        } // end of catch (read timeout comes here)


                        Debug.WriteLine("message " + message1);


                        //-------------------------------------------------------------------------------------------------------------------------------------
                        // ke9ns process received message
                        if ((message1.StartsWith("DX de ") == true) && (message1.Length > 76)) // string can be 77 (with no grid) or 82 (with grid)
                        {
                          
                            DX_Index1 = 250; // use 900 as a temp holding spot. always fill from the top


                            // grab DX_Spotter=======================================================================================
                            try
                            {
                                DX_Spotter[DX_Index1] = message1.Substring(6, 10); // get dx call with : at the end
                                 Debug.WriteLine("DX_Call " + DX_Station[DX_Index1]);

                                int pos = 10;
                                if (DX_Spotter[DX_Index1].Contains(":"))
                                {
                                     pos = DX_Spotter[DX_Index1].IndexOf(':'); // find the :
                                }
                                else
                                {
                                     pos = DX_Spotter[DX_Index1].IndexOf(' '); // find the first space instead of the :
                                }
                               

                                DX_Spotter[DX_Index1] = DX_Spotter[DX_Index1].Substring(0, pos); // reduce the call without the :

                                sb = new StringBuilder(DX_Spotter[DX_Index1]); // clear sb string over again
                                sb.Append('>');
                                sb.Insert(0, '<'); // to differentiate the spotter from the spotted

                                DX_Spotter[DX_Index1] = sb.ToString();

                            }
                            catch (FormatException e)
                            {
                                DX_Spotter[DX_Index1] = "NA";
                           
                            //    textBox1.Text = e.ToString();
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                           
                            //    textBox1.Text = e.ToString();
                            }
                            //    Debug.WriteLine("DX_Call " + DX_Station[DX_Index1]);

                            // grab DX_Freq========================================================================================
                            try
                            {
                               
                                DX_Freq[DX_Index1] = (int)((double)Convert.ToDouble(message1.Substring(15, 9)) * (double) 1000.0    ); //  get dx freq 7016.0  in khz 
                        
                                if ((DX_Freq[DX_Index1] >= 1800000) && (DX_Freq[DX_Index1] <= 1830000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 3500000) && (DX_Freq[DX_Index1] <= 3600000))
                                {
                                     DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 7000000) && (DX_Freq[DX_Index1] <= 7125000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 10000000) && (DX_Freq[DX_Index1] <= 11000000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 14000000) && (DX_Freq[DX_Index1] <= 14150000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 18000000) && (DX_Freq[DX_Index1] <= 18110000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 21000000) && (DX_Freq[DX_Index1] <= 21200000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 24800000) && (DX_Freq[DX_Index1] <= 24930000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 28000000) && (DX_Freq[DX_Index1] <= 28300000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 50000000) && (DX_Freq[DX_Index1] <= 50100000))
                                {
                                     DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 144000000) && (DX_Freq[DX_Index1] <= 144100000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if (
                                    (DX_Freq[DX_Index1] == 1885000) || (DX_Freq[DX_Index1] == 1900000) || (DX_Freq[DX_Index1] == 1945000) || (DX_Freq[DX_Index1] == 1985000)
                                    || (DX_Freq[DX_Index1] == 3825000) || (DX_Freq[DX_Index1] == 3870000) || (DX_Freq[DX_Index1] == 3880000) || (DX_Freq[DX_Index1] == 38850000)
                                     || (DX_Freq[DX_Index1] == 7290000) || (DX_Freq[DX_Index1] == 7295000) || (DX_Freq[DX_Index1] == 14286000) || (DX_Freq[DX_Index1] == 18150000)
                                     || (DX_Freq[DX_Index1] == 21285000) || (DX_Freq[DX_Index1] == 21425000) ||( (DX_Freq[DX_Index1] >= 29000000) && (DX_Freq[DX_Index1] < 29200000))
                                     || (DX_Freq[DX_Index1] == 50400000) || (DX_Freq[DX_Index1] == 50250000) || (DX_Freq[DX_Index1] == 144400000) || (DX_Freq[DX_Index1] == 144425000)
                                     || (DX_Freq[DX_Index1] == 144280000) || (DX_Freq[DX_Index1] == 144450000)

                                    )
                                {
                                    DX_Mode[DX_Index1] = 14; // AM mode
                                }
                                else if (
                                         ((DX_Freq[DX_Index1] >= 146000000) && (DX_Freq[DX_Index1] <= 148000000)) || ((DX_Freq[DX_Index1] >= 29200000) && (DX_Freq[DX_Index1] <= 29270000))
                                       || ((DX_Freq[DX_Index1] >= 144500000) && (DX_Freq[DX_Index1] <= 144900000))|| ((DX_Freq[DX_Index1] >= 145100000) && (DX_Freq[DX_Index1] <= 145500000))
                                       )
                                {
                                    DX_Mode[DX_Index1] = 114; // FM mode
                                }

                                else
                                {
                                    DX_Mode[DX_Index1] = 0; // ssb mode
                                }


                            } // try to determine if in the cw portion or ssb portion of each band
                            catch (FormatException e)
                            {
                                DX_Freq[DX_Index1] = 0;
                                DX_Mode[DX_Index1] = 0; // ssb mode
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                DX_Freq[DX_Index1] = 0;
                                DX_Mode[DX_Index1] = 0; // ssb mode
                            }
                            Debug.WriteLine("DX_Freq " + DX_Freq[DX_Index1]);


                            // grad DX_Station=========================================================================================

                            try
                            {
                                DX_Station[DX_Index1] = message1.Substring(26, 13); // get dx call with : at the end
                                int pos = DX_Station[DX_Index1].IndexOf(' '); // find the
                                DX_Station[DX_Index1] = DX_Station[DX_Index1].Substring(0, pos); // reduce the call without the
                            }
                            catch (FormatException )
                            {
                                DX_Spotter[DX_Index1] = "NA";
                              }
                            catch (ArgumentOutOfRangeException )
                            {
                                DX_Spotter[DX_Index1] = "NA";
                            }
                              Debug.WriteLine("DX_Spotter " + DX_Spotter[DX_Index1]);

                            // grab comments
                            try
                            {
                                DX_Mode2[DX_Index1] = 0; // reset split hz

                                DX_Message[DX_Index1] = message1.Substring(39, 29).ToLower(); // get dx call with : at the end
                              

                                if (DX_Message[DX_Index1].Contains("cw"))
                                {
                                      DX_Mode[DX_Index1] = 1; // cw mode

                                }
                                else if (DX_Message[DX_Index1].Contains(" rty ") || DX_Message[DX_Index1].Contains("rtty"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 2; // RTTY mode

                                }
                                else if (DX_Message[DX_Index1].Contains("psk"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 3; // psk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("oliv"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 4; // olivia mode

                                }
                                else if (DX_Message[DX_Index1].Contains("jt65"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 5; // jt65 mode

                                }
                                else if (DX_Message[DX_Index1].Contains("contesa"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 6; // contesa mode

                                }
                                else if (DX_Message[DX_Index1].Contains("fsk"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 7; // fsk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("mt63"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 8; // mt63 mode

                                }
                                else if (DX_Message[DX_Index1].Contains("domi"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 9; // domino mode

                                }
                                else if (DX_Message[DX_Index1].Contains("packact")|| DX_Message[DX_Index1].Contains("packtor")||DX_Message[DX_Index1].Contains("amtor"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 10; // pactor mode

                                }
                                else if (DX_Message[DX_Index1].Contains("fm "))
                                {
                                    if (chkBoxSSB.Checked != true) continue; // check for a SSB mode spot
                                    DX_Mode[DX_Index1] = 11; // fm mode

                                }
                                else if (DX_Message[DX_Index1].Contains("drm"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 12; // DRM mode

                                }
                                else if (DX_Message[DX_Index1].Contains("sstv"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 13; // sstv mode

                                }
                                else if (DX_Message[DX_Index1].Contains("easypal"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 12; // drm mode

                                }
                                else if (DX_Message[DX_Index1].Contains(" am ")|| DX_Message[DX_Index1].Contains(" sam "))
                                {
                                    if (chkBoxSSB.Checked != true) continue; // check for a SSB mode spot
                                    DX_Mode[DX_Index1] = 14; // AM mode

                                }




                                if (DX_Mode[DX_Index1] == 0)
                                {

                                    if (chkBoxSSB.Checked != true)
                                    {
                                       Debug.WriteLine("bypass ssb because not looking for ssb");
                                        continue; // check for a SSB mode spot
                                    }

                                }

                                if (DX_Mode[DX_Index1] == 1)
                                {

                                    if (chkBoxCW.Checked != true)
                                    {
                                       Debug.WriteLine("bypass CW because not looking for CW");
                                        continue; // check for a CW mode spot
                                    }

                                }

                                //----------------------------------------------------------

                                // grab GRID #
                                DX_Grid[DX_Index1] = message1.Substring(76,4); // get grid

                              sb = new StringBuilder(DX_Grid[DX_Index1]); // clear sb string over again
                              sb.Append(')');
                              sb.Insert(0, '('); // to differentiate the spotter from the spotted

                              DX_Grid[DX_Index1] = sb.ToString();
                      


                          
                                if (DX_Message[DX_Index1].Contains("<") && DX_Message[DX_Index1].Contains(">") ) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf(">") + 1;

                                    try // 
                                    {
                                        DX_Grid[DX_Index1] = DX_Message[DX_Index1].Substring(ind, 6);
                                        Debug.WriteLine("FOUND COMMENT GRID " + DX_Grid[DX_Index1]);
                                       
                                    }
                                    catch // 
                                    {

                                    }

                                } // get Grid from comments


                                //----------------------------------------------------------

                                DX_Mode2[DX_Index1] = 0;
                                //  resultString = Regex.Match(subjectString, @"\d+").Value;  Int32.Parse(resultString) will then give you the number.

                                if (DX_Message[DX_Index1].Contains("up")) // check for split
                                {
                                  
                                    int ind = DX_Message[DX_Index1].IndexOf("up") + 2;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4))* 1000));
                                       Debug.WriteLine("Found UP split hz" +split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                          Debug.WriteLine("Found UP split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                              Debug.WriteLine("Found UP split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("up") - 4; //

                                                try // try 4
                                                {

                                                    int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1,4)) * 1000));
                                                   Debug.WriteLine("Found UP split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {

                                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                       Debug.WriteLine("Found UP split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {

                                                            int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                           Debug.WriteLine("Found UP split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                          Debug.WriteLine("failed to find up value================");
                                                            DX_Mode2[DX_Index1] = 1000; // 1khz up

                                                        } // catch6   (2 digits to left side)
                                                    
                                                    } // catch5   (3 digits to left side)
                                                   
                                                } // catch4   (4 digits to left side)
                                        
                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split up
                               
                                else if ( DX_Message[DX_Index1].Contains("dn")) // check for split down
                                {
                                 
                                    int ind = DX_Message[DX_Index1].IndexOf("dn") + 2;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                          Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                            Debug.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                          Debug.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split down
                                else if (DX_Message[DX_Index1].Contains("dwn")) // check for split down
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("dwn") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                       Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                            Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                                Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dwn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                  Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000);
                                                      Debug.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                           Debug.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split down
                                else if (DX_Message[DX_Index1].Contains("down")) // check for split down
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("down") + 4;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("down") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                       Debug.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                           Debug.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)

                                } // split down
                                else if ((DX_Message[DX_Index1].Contains("9+")) || (DX_Message[DX_Index1].Contains("59+") )) // check for split
                                {
                                    // ignore + if its part of s9+
                                }

                                else if (DX_Message[DX_Index1].Contains("+")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("+") + 1;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000);
                                       Debug.WriteLine("Found UP split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000);
                                            Debug.WriteLine("Found UP split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000);
                                              Debug.WriteLine("Found UP split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 


                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)

                                  //  if (DX_Mode2[DX_Index1] > 9000) DX_Mode2[DX_Index1] = 0;

                                } // split up+

                                else if (DX_Message[DX_Index1].Contains(" -")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("-") + 1;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000);
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000);
                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000);
                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 


                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split dwn -

                                else if ((DX_Message[DX_Index1].Contains("qsx"))  ) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("qsx") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 8)) * 1000);
                                       Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;

                                        DX_Mode2[DX_Index1] =  DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 7)) * 1000);

                                            if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QsX .412  then treat it with the same mhz as DX_Freq
                                            else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QsX 18.412  then it must be in mhz

                                          Debug.WriteLine("Found qsx split hz" + split_hz);

                                            DX_Mode2[DX_Index1] = split_hz;
                                            DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                        }
                                        catch // catch 2
                                        {
                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 6)) * 1000);

                                                if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                                else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                               Debug.WriteLine("Found dn split hz" + split_hz);

                                                DX_Mode2[DX_Index1] = split_hz;
                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                               Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)
                                          
                                        } // catch 2   (7 digits to right side)
                                       
                                    } // catch 1   (8 digits to right side)


                                } // split qSx
                                else if ((DX_Message[DX_Index1].Contains("QSX"))) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("QSX") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 8)) * 1000);
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;

                                        DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 7)) * 1000);

                                            if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QsX .412  then treat it with the same mhz as DX_Freq
                                            else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QsX 18.412  then it must be in mhz

                                            Debug.WriteLine("Found qsx split hz" + split_hz);

                                            DX_Mode2[DX_Index1] = split_hz;
                                            DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                        }
                                        catch // catch 2
                                        {
                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 6)) * 1000);

                                                if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                                else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                                Debug.WriteLine("Found dn split hz" + split_hz);

                                                DX_Mode2[DX_Index1] = split_hz;
                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)

                                        } // catch 2   (7 digits to right side)

                                    } // catch 1   (8 digits to right side)


                                } // split qSx

                                else if (DX_Message[DX_Index1].Contains("qrz")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("qrz") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 8)) * 1000);

                                        if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                        else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                        Debug.WriteLine("Found qrz split hz" + split_hz);

                                        DX_Mode2[DX_Index1] = split_hz;
                                        DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                    }
                                    catch // catch 1
                                    {
                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 7)) * 1000);

                                            if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                            else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                          Debug.WriteLine("Found qrz split hz" + split_hz);

                                            DX_Mode2[DX_Index1] = split_hz;
                                            DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];

                                        }
                                        catch // catch 2
                                        {
                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 6)) * 1000);

                                                if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                                else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;

                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)
                                          
                                        } // catch 2   (7 digits to right side)
                                      
                                    } // catch 1   (8 digits to right side)


                                } // split qrz

                                else if (DX_Message[DX_Index1].Contains("QRZ")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("QRZ") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 8)) * 1000);

                                        if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                        else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                        Debug.WriteLine("Found qrz split hz" + split_hz);

                                        DX_Mode2[DX_Index1] = split_hz;
                                        DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                    }
                                    catch // catch 1
                                    {
                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 7)) * 1000);

                                            if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                            else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                            Debug.WriteLine("Found qrz split hz" + split_hz);

                                            DX_Mode2[DX_Index1] = split_hz;
                                            DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];

                                        }
                                        catch // catch 2
                                        {
                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 6)) * 1000);

                                                if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX .412  then treat it with the same mhz as DX_Freq
                                                else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QSX 18.412  then it must be in mhz

                                                Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;

                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)

                                        } // catch 2   (7 digits to right side)

                                    } // catch 1   (8 digits to right side)


                                } // split qrz




                            } // try to parse dx spot message above
                            catch (FormatException e)
                            {
                               Debug.WriteLine("mode issue" + e);

                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Debug.WriteLine("mode1 issue" + e);

                            }
                          //  Debug.WriteLine("DX_Message " + DX_Message[DX_Index1]);

                            // grab time
                            try
                            {
                                //  DX_Time[DX_Index1] = Convert.ToInt16(message1.Substring(70, 4)); // get time from dx spot

                                DX_Time[DX_Index1] = UTCNEW; // use the my UTC because some spotters have issues and the spot has the wrong time in it.


                            }
                            catch (Exception)
                            {
                                DX_Time[DX_Index1] = UTCNEW;
                          
                            }
                           
                            //   Debug.WriteLine("DX_Time " + DX_Time[DX_Index1])
                           
                           
                      
                            // set age of spot to 0;
                            DX_Age[DX_Index1] = "00"; // reset to start




                            //=================================================================================================
                            //=================================================================================================

                            // CHECK HERE FOR (NA) NORTH AMERICAN,  OR EXCLUDE NORTH AMERICAN SPOTS

                            //=================================================================
                            //=================================================================
                            //=================================================================
                            // ke9ns DX SPOT FILTERS (EXCLUDE NA HERE)
                            if (chkBoxWrld.Checked) // filter out US calls signs
                            {

                                string us1 = DX_Spotter[DX_Index1].Substring(1, 1); // grab first char of Spotter callsign becuase I added a < > around the spotter callsign
                                string us2 = DX_Spotter[DX_Index1].Substring(2, 1); // grab second char of Spotter callsign

                             //   Debug.WriteLine("us1 " + us1 + " us2 " + us2);

                                Regex r = new Regex("[KNWAX]"); // first char (include X for Mexico in the NA spots)
                                Regex r1 = new Regex("[A-Z0-9]"); // 2nd char to select as a NA spot
                                Regex r2 = new Regex("[ABCDEFGYO]"); // 2nd char // for V as the first char for Canada
                                Regex r3 = new Regex("[YGFIJK]"); // 2nd char // for C as the first char


                                if ((us1 == "V") || (us1 == "C")) // check for Canada (NA)
                                {
                                    if ( ((us1 == "V") && (r2.IsMatch(us2)) ) || ((us1 == "C") && (r3.IsMatch(us2))))
                                    {
                                        Debug.WriteLine("bypass4a " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                    goto PASS2; // if the 1st letter is not a US letter then GOOD use SPOT

                                }
                                else
                                {
                                    if ((r.IsMatch(us1)))
                                    {
                                        Debug.WriteLine("bypass3 " + DX_Spotter[DX_Index1]);
                                        continue;// dont show spot if not on the r list
                                    }
                                 
                                   // Debug.WriteLine("============CHECK3, fist us1 letter good for not being NA " + DX_Spotter[DX_Index1]);
                                    goto PASS2; // if the 1st letter is not a US letter then GOOD use SPOT

                                }

                                if ((r1.IsMatch(us2)))
                                {
                                     Debug.WriteLine("bypass4 " + DX_Spotter[DX_Index1]);
                                    continue; // dont show spot if not on the r1 list
                                }


                            }
                            else if (chkBoxNA.Checked) // filter out call signs outside of NA
                            {

                                string us1 = DX_Spotter[DX_Index1].Substring(1, 1);// was 0,1 now 1,1 because I added <>
                                string us2 = DX_Spotter[DX_Index1].Substring(2, 1);// was 1,1
                                string us3 = DX_Spotter[DX_Index1].Substring(3, 1);// 3rc char


                                Debug.WriteLine("us1 " + us1 + " us2 " + us2);


                                Regex r = new Regex("[KNWAVX]"); // first char
                                Regex r1 = new Regex("[A-Z0-9]"); // 2nd char
                                Regex r2 = new Regex("[ABCDEFGYO]"); // 2nd char // for V as the first char
                                Regex r3 = new Regex("[YGFIJK]"); // 2nd char // for C as the first char
                                Regex r4 = new Regex("[0-9]"); // 3rd char to filter out A71xx for example

                                if (!(r.IsMatch(us1)))
                                {
                                     Debug.WriteLine("bypass1 " + DX_Spotter[DX_Index1]);
                                    continue;// dont show spot if not on the r list
                                }

                                if ((r4.IsMatch(us2)) && (r4.IsMatch(us3))) // if call has 2 numbers in it, then its not North america
                                {
                                    Debug.WriteLine("bypass3 " + DX_Spotter[DX_Index1]);
                                    continue;// dont show spot if not on the r list

                                }
                                else if ((us1 == "V") || (us1 == "C"))
                                {
                                    if ( ((us1 == "V") && !(r2.IsMatch(us2)) ) || ((us1 == "C") && !(r3.IsMatch(us2))))
                                    {
                                        Debug.WriteLine("bypass2a " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                }
                                else
                                {
                                    if (!(r1.IsMatch(us2)))
                                    {
                                        Debug.WriteLine("bypass2 " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                }


                            } // checkBoxUSspot.Checked)

                            SP4_Active = 1; // processing message

                            //=================================================================
                            //=================================================================
                            //=================================================================
                            // ke9ns check for STATION DUPLICATES , there can only be 1 possible duplicate per spot added (but IGNORE if on 2nd band)
                            PASS2: int xx = 0;

                           
                            for (int ii = 0; ii <= DX_Index; ii++)
                            {
                                if ((xx == 0) && (DX_Station[DX_Index1] == DX_Station[ii]))
                                {
                                    if ( (Math.Abs(DX_Freq[DX_Index1] - DX_Freq[ii])) < 1000000 )
                                    {
                                  
                                        xx = 1;
                                    Debug.WriteLine("station dup============" + DX_Freq[ii] + " dup "+ DX_Freq[DX_Index1] + " dup " + DX_Station[DX_Index1] + " dup " + DX_Station[ii]);
                                    } // freq too close so its a dup
                                }

                                if (xx == 1)
                                {
                                    DX_FULLSTRING[ii] = DX_FULLSTRING[ii + 1]; // 

                                    DX_Station[ii] = DX_Station[ii + 1];
                                    DX_Freq[ii] = DX_Freq[ii + 1];
                                    DX_Spotter[ii] = DX_Spotter[ii + 1];
                                    DX_Message[ii] = DX_Message[ii + 1];
                                    DX_Grid[ii] = DX_Grid[ii + 1];
                                    DX_Time[ii] = DX_Time[ii + 1];
                                    DX_Age[ii] = DX_Age[ii + 1];
                                    DX_Mode[ii] = DX_Mode[ii + 1];
                                    DX_Mode2[ii] = DX_Mode2[ii + 1];

                                    DX_country[ii] = DX_country[ii + 1];

                                    DX_X[ii] = DX_X[ii + 1];
                                    DX_Y[ii] = DX_Y[ii + 1];

                                    DX_Beam[ii] = DX_Beam[ii + 1];

                                }

                            } // for ii check for dup in list
                            DX_Index = (DX_Index - xx);  // readjust the length of the spot list after taking out the duplicates

                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns check for FREQ DUPLICATES , there can only be 1 possible duplicate per spot added (but IGNORE if on 2nd band)
                            xx = 0;
                            for (int ii = 0; ii <= DX_Index; ii++)
                            {
                                if ((xx == 0) && (DX_Freq[DX_Index1] == DX_Freq[ii])) // if you already have this station in the spot list on the screen remove the old spot
                                {
                                    xx = 1;
                                    Debug.WriteLine("freq dup============");
                                }

                                if (xx == 1)
                                {
                                    DX_FULLSTRING[ii] = DX_FULLSTRING[ii + 1]; // 

                                    DX_Station[ii] = DX_Station[ii + 1];
                                    DX_Freq[ii] = DX_Freq[ii + 1];
                                    DX_Spotter[ii] = DX_Spotter[ii + 1];
                                    DX_Message[ii] = DX_Message[ii + 1];
                                    DX_Grid[ii] = DX_Grid[ii + 1];
                                    DX_Time[ii] = DX_Time[ii + 1];
                                    DX_Age[ii] = DX_Age[ii + 1];
                                    DX_Mode[ii] = DX_Mode[ii + 1];
                                    DX_Mode2[ii] = DX_Mode2[ii + 1];

                                    DX_country[ii] = DX_country[ii + 1];
                                    DX_X[ii] = DX_X[ii + 1];
                                    DX_Y[ii] = DX_Y[ii + 1];
                                    DX_Beam[ii] = DX_Beam[ii + 1];
                                }

                            } // for ii check for dup in list

                            DX_Index = (DX_Index - xx);  // readjust the length of the spot list after taking out the duplicates


                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns  passed the spotter, dx station , freq, and time test
                           
                            DX_Index++; // jump to PASS2 if it passed the valid call spotter test

                           
                            if (DX_Index > 90)
                            {
                                 Debug.WriteLine("DX SPOT REACH 90 ");
                                DX_Index = 90; // you have reached max spots
                            }

                            //   Debug.WriteLine("index "+ DX_Index);



                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns FILO buffer after taking out duplicate from above
                            for (int ii = DX_Index; ii > 0; ii--)
                            {
                                DX_FULLSTRING[ii] = DX_FULLSTRING[ii - 1]; // move array stack down one (oldest dropped off)

                                DX_Station[ii] = DX_Station[ii - 1];
                                DX_Freq[ii] = DX_Freq[ii - 1];
                                DX_Spotter[ii] = DX_Spotter[ii - 1];
                                DX_Message[ii] = DX_Message[ii -1];
                                DX_Grid[ii] = DX_Grid[ii - 1];
                                DX_Time[ii] = DX_Time[ii - 1];
                                DX_Age[ii] = DX_Age[ii - 1];
                                DX_Mode[ii] = DX_Mode[ii - 1];
                                DX_Mode2[ii] = DX_Mode2[ii - 1];

                                DX_country[ii] = DX_country[ii - 1];
                                DX_X[ii] = DX_X[ii - 1];
                                DX_Y[ii] = DX_Y[ii - 1];
                                DX_Beam[ii] = DX_Beam[ii - 1];

                            } // for ii

                            DX_FULLSTRING[0] = message1; // add newest message to top

                          
                            DX_Station[0] = DX_Station[DX_Index1];    //insert new spot on top of list now
                            DX_Freq[0] = DX_Freq[DX_Index1];
                            DX_Spotter[0] = DX_Spotter[DX_Index1];
                            DX_Message[0] = DX_Message[DX_Index1];
                            DX_Grid[0] = DX_Grid[DX_Index1];
                            DX_Time[0] = DX_Time[DX_Index1];
                            DX_Age[0] = DX_Age[DX_Index1];
                            DX_Mode[0] = DX_Mode[DX_Index1];
                            DX_Mode2[0] = DX_Mode2[DX_Index1];

                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            // Crosscheck Station Call sign Prefix with data from DXLOC.txt (lat and lon) 
                            // and create a list of Country, Callsign, X, Y on unscaled map


                            updatemapspots();


                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------

                            Debug.WriteLine("INSTALL NEW [0]=========== " + DX_Index);


                            processTCPMessage(); // send to spot window
                            Debug.WriteLine("INSTALL NEW [1]=========== " + DX_Index);


                            SP4_Active = 0; // done processing message

                          
                            //      Debug.WriteLine("Aindex " + DX_Index);

                        } // (message1.StartsWith("DX de ") valid message


                        else if (message1.Contains(" disconnected"))
                        {
                            textBox1.Text += "Your Socket was disconnected \r\n";

                            statusBox.ForeColor = Color.Red;
                            console.spotterMenu.ForeColor = Color.Red;

                            console.spotterMenu.Text = "Closed12345";
                            statusBox.Text = "Closed";


                            SP_writer.Close();                  // close down now
                            SP_reader.Close();
                            networkStream.Close();

                            client.Close();
                            //   Debug.WriteLine("END DX SPOT thread");

                            statusBox.ForeColor = Color.Black;
                            console.spotterMenu.ForeColor = Color.White;

                            console.spotterMenu.Text = "Spotter";
                            statusBox.Text = "Off";
                            textBox1.Text += "All closed \r\n";
                            SP_Active = 0;
                            SP2_Active = 0;

                            DXPost.Enabled = false;
                            textBoxDXCall.Enabled = false;

                            return;
                        } // if disconnected

     

                    } // SP_active == 3 (getting spots here)


                } // for loop forever for this spotter thread

                // if you reach here, its because your closing down the socket

                //    Debug.WriteLine("END DX SPOT thread");


                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                console.spotterMenu.Text = "Closing";
                statusBox.Text = "Closing";

                textBox1.Text += "Asked to Close \r\n";


                SP_writer.Close();                  // close down now
                SP_reader.Close();
                networkStream.Close();
              

                client.Close();
                //   Debug.WriteLine("END DX SPOT thread");

                statusBox.ForeColor = Color.Black;
                console.spotterMenu.ForeColor = Color.White;

                console.spotterMenu.Text = "Spotter";
                statusBox.Text = "Off";
                textBox1.Text += "All closed \r\n";
                SP2_Active = 0;
                SP_Active = 0;

                DXPost.Enabled = false;
                textBoxDXCall.Enabled = false;

                return;


            } // try
            catch (SocketException SE)
            {
                textBox1.Text += "Socket Forced closed \r\n";

                Debug.WriteLine("cannot open socket or socket closed on me" + SE);
                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                try
                {
                    SP_writer.Close();
                    SP_reader.Close();
                }
                catch(Exception)
                {
                  

                }

                try
                {
                    networkStream.Close();
                    client.Close();
                }
                catch(Exception)
                {

                }

               
                SP_Active = 0;
                SP2_Active = 0;

                //   textBox1.Text += "Socket crash Done \r\n";
                statusBox.Text = "Closed";
                console.spotterMenu.Text = "Spotter";

                //    textBox1.Text = SE.ToString();

                DXPost.Enabled = false;
                textBoxDXCall.Enabled = false;

                return;

            }
            catch (Exception e1)
            {
                textBox1.Text += "Socket Forced closed \r\n";

                Debug.WriteLine("socket exception issue" + e1);

                 statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                try
                {
                    SP_writer.Close();
                    SP_reader.Close();
                }
                catch (Exception)
                {


                }

                try
                {
                    networkStream.Close();
                    client.Close();
                }
                catch (Exception)
                {

                }

                SP2_Active = 0;

                statusBox.Text = "Closed";
                console.spotterMenu.Text = "Spotter";

                DXPost.Enabled = false;
                textBoxDXCall.Enabled = false;

                return;
            }


        } // SPOTTER Thread



        //===================================================================================
        // ke9ns add update dx call and country on map
        private void updatemapspots()
        {


            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            // Crosscheck Station Call sign Prefix with data from DXLOC.txt (lat and lon) 
            // and create a list of Country, Callsign, X, Y on unscaled map

            if ((SP8_Active == 1) && (SP_Active > 2)  && (DX_Index > 0 )) // do if dxloc.txt file loaded in        && (SP5_Active == 1 )
            {

                int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map

                int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                Debug.WriteLine("MAPPING======");

                DX_Y[0] = 0;
                DX_X[0] = 0;
                DX_country[0] = null;
                DX_Beam[0] = 0; 

                int kk = 0;

                for (; kk < DXLOC_Index1; kk++)  // list of call sign prefixes and there corresponding LAT/LON  DXLOC_Index1 is from when file was read into memory
                {
                    if (DX_Station[0].StartsWith(DXLOC_prefix[kk]) == true) // look for a dx spot callsign prefix to make a match with the dxloc.txt list
                    {
                        if (DXLOC_prefix1[kk] != null)
                        {

                            if (DX_Station[0].Contains(DXLOC_prefix1[kk]) == false) continue; // dont choose if not a match

                        }

                        DX_Y[0] = (int)(((180 - (DXLOC_LAT[kk] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S

                        DX_X[0] = (int)(((DXLOC_LON[kk] + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E

                        DX_country[0] = DXLOC_country[kk]; // save country into dx spotter list (pulled from dxloc list)

                        DX_Beam[0] = BeamHeading(DXLOC_LAT[kk], DXLOC_LON[kk]);

                        Debug.WriteLine("MAPPER " + DX_Station[0] + " " + DX_X[0] + " " + DX_Y[0] + " cntry " + DX_country[0] + " prefix " + DXLOC_prefix[kk] + " lat " + DXLOC_LAT[kk] + " lon " + DXLOC_LON[kk] + " BEAM "+DX_Beam[0]);

                        break; // got a match so break

                    }

                } // for kk loop for DXLOC in memory

                if (kk == DXLOC_Index1) // no match found
                {
                    DX_country[0] = " -- "; // dont have a match so need to add to list

                    DX_Beam[0] = 0;
                    Debug.WriteLine("MAPPER NO MACH FOR Station" + DX_Station[0]);

                }

            } // sp8_active = 1
            else
            {
                DX_country[0] = " -- "; // dont have a match so need to add to list
                DX_Beam[0] = 0;
                //  Debug.WriteLine("mapper OFF");

            }


        } //  updatemapspots()








        //===================================================================================
        // ke9ns add convert all the DX spots in the array to text messages for the DX spotter window
        public void processTCPMessage()
        {

            string bigmessage = null;

            for (int ii = 0; ii < DX_Index; ii++)
            {

                if (DX_Age[ii] == null) DX_Age[ii] = "00";
                else if (DX_Age[ii] == "  ") DX_Age[ii] = "00";

                if (DX_Age[ii].Length == 1) DX_Age[ii] = "0" + DX_Age[ii];

                //----------------------------------------------------------
                string DXmode = "    "; // 5 spaces

                if (DX_Mode[ii] == 0)       DXmode = " ssb ";
                else if (DX_Mode[ii] == 1)  DXmode = " cw  ";
                else if (DX_Mode[ii] == 2)  DXmode = " rtty";
                else if (DX_Mode[ii] == 3)  DXmode = " psk ";
                else if (DX_Mode[ii] == 4)  DXmode = " oliv";
                else if (DX_Mode[ii] == 5)  DXmode = " jt65";
                else if (DX_Mode[ii] == 6)  DXmode = " cont";
                else if (DX_Mode[ii] == 7)  DXmode = " fsk ";
                else if (DX_Mode[ii] == 8)  DXmode = " mt63";
                else if (DX_Mode[ii] == 9)  DXmode = " domi";
                else if (DX_Mode[ii] == 10) DXmode = " pack";
                else if (DX_Mode[ii] == 11) DXmode = " fm  ";
                else if (DX_Mode[ii] == 12) DXmode = " drm ";
                else if (DX_Mode[ii] == 13) DXmode = " sstv";
                else if (DX_Mode[ii] == 14) DXmode = " am  ";

                else DXmode = "     ";

              // old   bigmessage += (DX_FULLSTRING[ii]+ DXmode+ " " + (DX_country[ii].PadRight(8)).Substring(0,8) + " :"+ DX_Age[ii]  + "\r\n" );
                bigmessage += (DX_FULLSTRING[ii] + DXmode + " " + (DX_country[ii].PadRight(8)).Substring(0, 8) + ": "+ DX_Beam[ii].ToString().PadLeft(3) + "° :" + DX_Age[ii] + "\r\n"); // adds 6

            } // for loop to update dx spot window

            if ((pause == false) && (beacon == false) && (WTime == false))
            {
                textBox1.Text = bigmessage; // update screen

                Debug.WriteLine("DX_TEXT " + DX_TEXT + " , " + DX_SELECTED);

                for (int ii = 0; ii < DX_Index; ii++)
                {
                    if (  DX_TEXT == textBox1.Text.Substring((ii * LineLength) +16, 40) ) // just check freq and dx call sign for match
                    {
                        DX_SELECTED = ii;
                    
                        textBox1.SelectionStart = DX_SELECTED * LineLength;      // start of each dx spot line
                        textBox1.SelectionLength = LineLength;                    // length of each dx spot  line

                       // textBox1.ScrollToCaret();

                        break; //get out of for loop ii
                    }
                    
                } // for loop ii

              

            }
        } //processTCPMessage



        //===================================================================================
        // ke9ns add process message for beacon system
        private void processTCPMessage1()
        {

            string bigmessage = null;

            for (int ii = 0; ii < BX1_Index; ii++)
            {

                if (BX_Age[ii] == null) BX_Age[ii] = "00";
                else if (DX_Age[ii] == "  ") BX_Age[ii] = "00";

                if (BX_Age[ii].Length == 1) BX_Age[ii] = "0" + BX_Age[ii];

                //----------------------------------------------------------
                string DXmode = "    "; // 5 spaces

                if (BX_Mode[ii] == 0) DXmode = " ssb ";
                else if (BX_Mode[ii] == 1) DXmode = " cw  ";
                else if (BX_Mode[ii] == 2) DXmode = " rtty";
                else if (BX_Mode[ii] == 3) DXmode = " psk ";
                else if (BX_Mode[ii] == 4) DXmode = " oliv";
                else if (BX_Mode[ii] == 5) DXmode = " jt65";
                else if (BX_Mode[ii] == 6) DXmode = " cont";
                else if (BX_Mode[ii] == 7) DXmode = " fsk ";
                else if (BX_Mode[ii] == 8) DXmode = " mt63";
                else if (BX_Mode[ii] == 9) DXmode = " domi";
                else if (BX_Mode[ii] == 10) DXmode = " pack";
                else if (BX_Mode[ii] == 11) DXmode = " fm  ";
                else if (BX_Mode[ii] == 12) DXmode = " drm ";
                else if (BX_Mode[ii] == 13) DXmode = " sstv";
                else if (BX_Mode[ii] == 14) DXmode = " am  ";

                else DXmode = "     ";

                bigmessage += (BX_FULLSTRING[ii] + DXmode + " " + (BX_country[ii].PadRight(8)).Substring(0, 8) + ": " + BX_Beam[ii].ToString().PadLeft(3) + "° :" + BX_Age[ii] + "\r\n"); // adds 6

            } // for loop to update dx spot window

             textBox1.Text = bigmessage; // update screen


        } //processTCPMessage1 beacon

        //===================================================================================
        // ke9ns add process age of dx spot

        private void processDXAGE()
        {

            
            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

          
            FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW
            UTCNEW = Convert.ToInt16(FD);                                    // convert 24hr UTC to int



            int hh = Convert.ToInt16(UTCD.ToString("HH"));
            int mm = Convert.ToInt16(UTCD.ToString("mm"));

            int UTCNEWMIN = mm + (60 * hh);
         //   Debug.WriteLine("time in minutes only " + UTCNEWMIN);



            if ((UTCLAST == 0) && (UTCNEW != 0))  // for startup only (ie. run 1 time)
            {
                UTCLAST = UTCNEW;        
                DX_Time[0] = UTCNEW;
                UTCLASTMIN = UTCNEWMIN; 

            }

            if ((DX_Index > 0) && (UTCNEW != UTCLAST))
            {
                int xxx = 0;

                UTCLAST = UTCNEW;

                int UTCDIFFMIN = UTCNEWMIN - UTCLASTMIN;  // difference in minutes from last time we checked the dx spots for age

                Debug.WriteLine("Time to Check DX Spot Age =========== " + DX_Index + " UTCNEWMIN " + UTCNEWMIN + " UTCLASTMIN " + UTCLASTMIN + " UTCDIFFMIN " + UTCDIFFMIN);

                UTCLASTMIN = UTCNEWMIN; // save for next go around

                if (UTCDIFFMIN < 0) // this indicates you crossed the timeline to the next day
                {
                    UTCDIFFMIN = 1440 - UTCDIFFMIN; // make positive again
                }

                for (int ii = DX_Index - 1; ii >= 0; ii--) // move from bottom of list up toward top of list
                {

                    //  int UTCDIFF = Math.Abs(UTCNEW - DX_Time[ii]); // time difference 
                    //DX_Age[ii] = UTCDIFF.ToString("00"); // 2 digits
                   

                   int  UTCAGE = Convert.ToInt32(DX_Age[ii]) + UTCDIFFMIN; // current age difference for DX spots

                    DX_Age[ii] = UTCAGE.ToString(); // age your DX spot

                                       
                    int kk = 0; // look at very bottom of list + 1

                    
                    if (UTCAGE > 35) // if its an old SPOT then remove it from the list
                    {

                      //  Flag8 = 1; // signal that the DX_Index will change due to an old spot being removed

                        kk = ii; // 

                        xxx++; //shorten dx_Index by 1

                        Debug.WriteLine("time expire, remove=========spot " + DX_Time[ii] + " current time " + UTCLAST + " UTCDIFFMIN " + UTCDIFFMIN + " ii " + ii + " station " + DX_Station[ii]);
                     //   Debug.WriteLine("KK " + kk);
                     //   Debug.WriteLine("XXX " + xxx);


                        for (; kk < (DX_Index - xxx); kk++)
                        {

                            DX_FULLSTRING[kk] = DX_FULLSTRING[kk + xxx]; // 

                            DX_Station[kk] = DX_Station[kk + xxx];
                            DX_Freq[kk] = DX_Freq[kk + xxx];
                            DX_Spotter[kk] = DX_Spotter[kk + xxx];
                            DX_Message[kk] = DX_Message[kk + xxx];
                            DX_Grid[kk] = DX_Grid[kk + xxx];
                            DX_Time[kk] = DX_Time[kk + xxx];
                            DX_Age[kk] = DX_Age[kk + xxx];
                            DX_Mode[kk] = DX_Mode[kk + xxx];
                            DX_Mode2[kk] = DX_Mode2[kk + xxx];

                        } // for loop:  push OK Spots from bottom of list up as you delete old spots from list

                    } // TIMEOUT exceeded remove old spot



                } // for ii check for dup in list

                DX_Index = DX_Index - xxx;  // update DX_Index list (shorten if any old spots deleted)

             //   Debug.WriteLine("END=========== " + DX_Index);

             //   Debug.WriteLine(" ");

                processTCPMessage(); // update spot window (remove old spots)

                return;

            } // UTC NEW != LAST
            else
            {
                return; // skip
            }

        } // processDXAGE


        private void nameBox_MouseEnter(object sender, EventArgs e)
        {
           // ToolTip tt = new ToolTip();
          //  tt.Show("Name Name of DX Spider node with a > symbol at the end: Example: HB9DRV-9> or NN1D> ", nameBox, 10, 60, 2000);

        }

        private void callBox_MouseEnter(object sender, EventArgs e)
        {
           // ToolTip tt = new ToolTip();
          //  tt.Show("Your Call Sign to login to DX Spider node. Note: you must have used this call with this node prior to this first time ", callBox, 10, 60, 2000);
        }

        private void nodeBox_MouseEnter(object sender, EventArgs e)
        {
           // ToolTip tt = new ToolTip();
          //  tt.Show("Dx Spider node address ", nodeBox, 10, 60, 2000);
        }

        private void portBox_MouseEnter(object sender, EventArgs e)
        {
          //  ToolTip tt = new ToolTip();
          //  tt.Show("Port # that goes with the node address", portBox, 10, 60, 2000);
        }



        private void callBox_TextChanged(object sender, EventArgs e)
        {
            DX_Index = 0; // start over if change node

        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {

            DX_Index = 0; // start over if change node


        }

        private void portBox_TextChanged(object sender, EventArgs e)
        {
            DX_Index = 0; // start over if change node

        }

        private void nodeBox_TextChanged(object sender, EventArgs e)
        {

            DX_Index = 0; // start over if change node


        }


        private void SpotControl_Layout(object sender, LayoutEventArgs e)
        {




        }

        private void nameBox_Leave(object sender, EventArgs e)
        {
             callB = callBox.Text;
             nodeB = nodeBox1.Text;
             portB = portBox2.Text;
             nameB = nameBox.Text;
        }

        private void callBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;
        }

        private void nodeBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;
        }

        private void portBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;
        }

   
        private void chkBoxNA_CheckedChanged(object sender, EventArgs e)
        {
      
                if (chkBoxNA.Checked == true)
                {

                    //   Debug.WriteLine("US SPOT CHECKED");
                    chkBoxWrld.Checked = false;

                }
                else
                {
                    //   Debug.WriteLine("US SPOT UN-CHECKED");
                }
           
        }

        private void chkBoxWrld_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxWrld.Checked == true)
            {
                chkBoxNA.Checked = false;
                //   Debug.WriteLine("world SPOT CHECKED");
            }
            else
            {
                //   Debug.WriteLine("world SPOT UN-CHECKED");
            }
        }
       


        private void statusBox_Click(object sender, EventArgs e)
        {
            statusBox.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click

            if ((SP_Active == 3 )) // if DX cluster active then test it by sending a CR
            {

                try
                {
                    statusBox.ForeColor = Color.Red;

                    statusBox.Text = "Test Sent <CR>";

                    SP_writer.Write((char)13);
                    SP_writer.Write((char)10);
                }
                catch(Exception)
                {
                    statusBox.Text = "Failed Test";

                }
     
            } // if connection was supposed to be active

        } // statusBox_Click

        public int DX_SELECTED = 0; // line on the dx spot window that was click on last
        public int LineLength = 106; // was 105
        public string DX_TEXT;


        //===============================================================================
        public bool beam_selected = false; // ke9ns if you clicked on the beam angle

        public void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click

            chkDXMode.Checked = true;  // the callsign box

            if (e.Button == MouseButtons.Left)
            {

                int ii = textBox1.SelectionStart; // character position in the text you clicked on 

                byte iii = (byte)(ii / LineLength); // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added but now /105 with DX_Beam heading

              //  Debug.WriteLine("test " + DX_Index + " , "+iii);


                if (iii >= DX_Index) return; // dont allow to click on blank area

                DX_SELECTED = (int)iii;  // store the last line you clicked on to keep highlighted

              
                textBox1.SelectionStart = DX_SELECTED * LineLength;      // start of each dx spot line
                textBox1.SelectionLength = LineLength;                    // length of each dx spot  line

                DX_TEXT = textBox1.Text.Substring((DX_SELECTED * LineLength)+16, 40); // just check freq and callsign of dx station

             //   Debug.WriteLine("1DX_SELECTED " + DX_SELECTED + " , "+ DX_TEXT);

                int gg = ii % LineLength;  // get remainder for checking beam heading

             //   Debug.WriteLine("position in line" + gg);

                if (gg > (LineLength - 10)) beam_selected = true; // did user Left click over the beam heading on the dx spot list ?
                else beam_selected = false;

       
                if ((DX_Index > iii) && (beacon1 == false))
                {
                    int freq1 = DX_Freq[iii]; // in hz

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {
                            if (DX_Mode[iii] == 0)       console.RX1DSPMode = DSPMode.LSB;
                            else if (DX_Mode[iii] == 1)  console.RX1DSPMode = DSPMode.CWL;
                            else if (DX_Mode[iii] == 2)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 3)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 4)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 5)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 6)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 7)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 8)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 9)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (DX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.LSB;
                            else if (DX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else console.RX1DSPMode = DSPMode.LSB;


                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.LSB;
                        }
                    } // LSB
                    else
                    {
                        if (chkDXMode.Checked == true)
                        {

                            if (DX_Mode[iii] == 0)       console.RX1DSPMode = DSPMode.USB;
                            else if (DX_Mode[iii] == 1)  console.RX1DSPMode = DSPMode.CWU;
                            else if (DX_Mode[iii] == 2)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 3)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 4)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 5)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 6)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 7)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 8)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 9)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (DX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.USB;
                            else if (DX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else console.RX1DSPMode = DSPMode.USB;

                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.USB;
                        }
                    }


                    console.VFOAFreq = (double)freq1 / 1000000; // convert to MHZ

                    if (chkDXMode.Checked == true)
                    {

                        if (DX_Mode2[iii] != 0)
                        {
                           
                            console.VFOBFreq = (double)(freq1 + DX_Mode2[iii]) / 1000000; // convert to MHZ
                            console.chkVFOSplit.Checked = true; // turn on  split

                            Debug.WriteLine("split here" + (freq1 + DX_Mode2[iii]));

                        }
                        else
                        {
                            console.chkVFOSplit.Checked = false; // turn off split

                        }


                    } // dxmode checked

                    if (beam_selected == true)    // ke9ns add send hygain rotor command to DDUtil via the CAT port setup in PowerSDR
                    {
                        Debug.WriteLine("BEAM HEADING TRANSMIT");

                        console.spotDDUtil_Rotor = "AP1" + DX_Beam[iii].ToString().PadLeft(3, '0') + ";";
                      // console.spotDDUtil_Rotor = ";";
                        console.spotDDUtil_Rotor = "AM1;";

                   } //  if (chkBoxRotor.Checked == true)
                    button2.Focus();


                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range

                else  if ((BX1_Index > iii) && (beacon1 == true))
                {
                    int freq1 = BX_Freq[iii];

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {
                            if (BX_Mode[iii] == 0) console.RX1DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWL;
                            else if (BX_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else console.RX1DSPMode = DSPMode.LSB;


                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.LSB;
                        }
                    } // LSB
                    else
                    {
                        if (chkDXMode.Checked == true)
                        {

                            if (BX_Mode[iii] == 0) console.RX1DSPMode = DSPMode.USB;
                            else if (BX_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWU;
                            else if (BX_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.USB;
                            else if (BX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else console.RX1DSPMode = DSPMode.USB;

                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.USB;
                        }
                    }


                    console.VFOAFreq = (double)freq1 / 1000000; // convert to MHZ

                    if (chkDXMode.Checked == true)
                    {

                        if (BX_Mode2[iii] != 0)
                        {

                            console.VFOBFreq = (double)(freq1 + BX_Mode2[iii]) / 1000000; // convert to MHZ
                            console.chkVFOSplit.Checked = true; // turn on  split

                            Debug.WriteLine("split here" + (freq1 + BX_Mode2[iii]));

                        }
                        else
                        {
                            console.chkVFOSplit.Checked = false; // turn off split

                        }


                    } // dxmode checked

                    if (beam_selected == true)    // ke9ns add send hygain rotor command to DDUtil via the CAT port setup in PowerSDR
                    {
                        Debug.WriteLine("BEAM HEADING TRANSMIT");

                        console.spotDDUtil_Rotor = "AP1" + BX_Beam[iii].ToString().PadLeft(3, '0') + ";";
                        console.spotDDUtil_Rotor = ";";
                        console.spotDDUtil_Rotor = "AM1;";

                    } //  if (chkBoxRotor.Checked == true)
                    button2.Focus();


                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range (BEACON)



            } // left mouse button
            else if (e.Button == MouseButtons.Right)
            {

                if ((SP4_Active == 0) && (beacon1 == false))// only process lookup if not processing a new spot which might cause issue
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    byte iii = (byte)(ii / LineLength);  // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added
                    
                    if (DX_Index > iii)
                    {
                        string DXName = DX_Station[iii];

                      //  Debug.WriteLine("Line " + iii + " Name " + DXName);

                        try
                        {
                            System.Diagnostics.Process.Start("https://www.qrz.com/db/" + DXName);   // System.Diagnostics.Process.Start("http://www.microsoft.com");
                        }
                        catch
                        {
                       //     Debug.WriteLine("bad station");
                            // if not a URL then ignore
                        }
                    }
                   
                } // not actively processing a new spot
               else  if ((SP4_Active == 0) && (beacon1 == true))// only process lookup if not processing a new spot which might cause issue
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    byte iii = (byte)(ii / LineLength);  // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added

                    if (BX1_Index > iii)
                    {
                        string DXName = BX_Station[iii];

                        //  Debug.WriteLine("Line " + iii + " Name " + DXName);

                        try
                        {
                            System.Diagnostics.Process.Start("https://www.qrz.com/db/" + DXName);   // System.Diagnostics.Process.Start("http://www.microsoft.com");
                        }
                        catch
                        {
                            //     Debug.WriteLine("bad station");
                            // if not a URL then ignore
                        }
                    }

                } // not actively processing a new spot






            }

        } // textBox1_MouseUp



        //=========================================================================================
        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {

            this.TopMost = chkAlwaysOnTop.Checked;
        }


        //=========================================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            if (pause == true)
            {
                pause = false;
                button1.Text = "Pause";
            }
            else
            {
                pause = true;
                button1.Text = "Paused";
            }

        }




        public static Image Skin1 = null; // temp holder for orignal skin image in picdisplay

        //=========================================================================================
        private void chkSUN_CheckedChanged(object sender, EventArgs e)
        {

            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image
            }
            if (SP_Active != 0 ) 
            {
             
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)
                        console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);

                    }
                    else console.picDisplay.BackgroundImage = MAP;
 
                } // SUN or GRAY LINE checked

            } // dx spot on

            Map_Last = 1;

        } // chkSUN_CheckedChanged



        //=========================================================================================
        private void chkGrayLine_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image
            }

            if ( SP_Active != 0) 
            {
              
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {
                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);

                      //  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                    }
                    else console.picDisplay.BackgroundImage = MAP;
      
                } // only do if SUN or GRAY LINE checked

            } // dx spot active

            Map_Last = 1;

        } // chkGrayLine_CheckedChanged





        private static DisplayMode LastDisplayMode = 0;

        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        // turn on/off tracking (sun and/or grayline)
        public void btnTrack_Click(object sender, EventArgs e)
        {
            button2.Focus();

            if (SP5_Active == 0)  // if OFF then turn ON
            {

                if (chkPanMode.Checked == true) Display.map = 1; // special panafall mode (80 - 20)
                else Display.map = 0;

                btnTrack.Text = "Track ON";

                LastDisplayMode = Display.CurrentDisplayMode; // save the display mode that you were in before you turned on special panafall mode

                if (chkPanMode.Checked == true)  Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                Display.GridOff = 1; // Force Gridlines off but dont change setupform setting
                
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)   console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);
                        //  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                    }
                    else console.picDisplay.BackgroundImage = MAP;

                }

                if (chkISS.Checked == true)
                {
                    Thread t1 = new Thread(new ThreadStart(ISSOrbit));                                // turn on track map (sun, grayline, voacap, or beacon)

                    t1.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t1.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                    t1.Name = "ISS Orbit Plot Thread";
                    t1.IsBackground = true;
                    t1.Priority = ThreadPriority.BelowNormal;
                    t1.Start();

                } //  if (chkISS.Checked == true)

                Thread t = new Thread(new ThreadStart(TrackSun));  // turn on track map (sun, grayline, voacap, or beacon)

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                SP5_Active = 1; // turn on track map (sun, grayline, voacap, or beacon)

                t.Name = "Track Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                textBox1.Text = "Clicked to Turn on GrayLine Sun Tracker\r\n";

               //

            }
            else // map was already on so turn off
            {
              //  console.propMenuItem1.Enabled = false;


                SP5_Active = 0;                     // turn off tracking

                Display.map = 0;                    // tell display program to got back to standard panafall mode

                if (chkPanMode.Checked == true) Display.CurrentDisplayMode = LastDisplayMode;
               
                if (console.setupForm.gridBoxTS.Checked == true) Display.GridOff = 1; // put gridlines back the way they were
                else Display.GridOff = 0; // gridlines ON
            
                btnTrack.Text = "Track";
     
                textBox1.Text += "Click to turn off GrayLine Sun Tracking\r\n";
             
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image

            } // turn Tracking  off


        } //  btnTrack_Click





        public static int UTCLAST2 = 0;                                        // for Sun Tracker

        public static double UTC100A = 0;                                       // UTC time in hours.100th of min for grayline

        public static double UTC100 = 0;                                       // UTC time as a factor of 1 (1=2400 UTC, 0= 0000UTC) 
        public static double UTCDAY = 0;                                       // UTC day as a factor of 1 (0=1day, 1=365)
        public static int[] Gray_X = new int[400];                             // for mapping the gray line x and y 
        public static int[] Gray_Y = new int[400];

        public static int Sun_Right = 0;
        public static int Sun_Left = 0;

        public static int[,] GrayLine_Pos = new int[1000, 3];                      // [0,]=is lat 180 to 0 to -180 (top to bottom) dark
        public static int[,] GrayLine_Pos1 = new int[1000, 3];                      // [0,]=is lat 180 to 0 to -180 (top to bottom) dusk
        public static int[] GrayLine_Pos2 = new int[1000];
        public static int[] GrayLine_Pos3 = new int[1000];

        public static int GrayWinterStart = 270;          // day#'s where the northern area is always gray or dark
        public static int GrayWinterStop = 78;

        public static int GraySummerStart = 150;        // day#'s where the souther area is always gray or dark
        public static int GraySummerStop = 180;


        public static int zz = 0; // determine the y pixel for this latitude grayline


        public static int Sun_X = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin)
        public static int Sun_Y = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin) 
        public static int Sun_Top1 = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin)
        public static int Sun_Bot1 = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin) 

        public static int Moon_X = 0;  // position of MOON in picDisplay window (based on ke9ns world map skin)
        public static int Moon_Y = 0;  // position of MOON in picDisplay window (based on ke9ns world map skin) 
        public static int Moon_AZ = 0;     // beam heading of the current Moons position
        public static int Moon_ALT = 0;    // elevation of the current Moons position
        public static int Moon_Top1 = 0;  // position of MOON in picDisplay window (based on ke9ns world map skin)
        public static int Moon_Bot1 = 0;  // position of MOON in picDisplay window (based on ke9ns world map skin) 

        public static int ISS_X = 0;  // position of ISS in picDisplay window (based on ke9ns world map skin)
        public static int ISS_Y = 0;  // position of ISS in picDisplay window (based on ke9ns world map skin) 
        public static int ISS_XL = 0;  // position of ISS in picDisplay window (based on ke9ns world map skin)
        public static int ISS_YL = 0;  // position of ISS in picDisplay window (based on ke9ns world map skin) 

        public static int ISS_Top1 = 0;  // position of ISS in picDisplay window (based on ke9ns world map skin)
        public static int ISS_Bot1 = 0;  // position of ISS in picDisplay window (based on ke9ns world map skin) 
        public static int ISS_AZ = 0;     // beam heading of the current ISS position
        public static int ISS_ALT = 0;    // elevation of the current ISS position
        public static int ISS_DIST = 0;    // distance of the current ISS position
        public static double ISS_LAT = 0;     // LATitude of the current ISS position
        public static double ISS_LON = 0;    // longitude of the current ISS position

        public static bool SUN = false; // true = on
        public static bool MOON = false; // true = on
        public static bool ISS = false; // true = on
        public static bool GRAYLINE = false; // true = on
       
        public static int suncounter = 0; // for space weather
        public static int SFI = 0;       // for Space weather
        public static int SN = 0;        // for Space weather
        public static int Aindex = 0;    // for Space weather
        public static int Kindex = 0;    // for Space weather
        public static string RadioBlackout = " ";
        public static string GeoBlackout = " ";
        private string serverPath;       // for Space weather



        //====================================================================================================
        //====================================================================================================
        // ke9ns add compute angle to sun (for dusk grayline) // equations in this section below provided by Roland Leigh
        private int SUNANGLE(double lat, double lon)  // ke9ns Thread opeation (runs in en-us culture) opens internet connection to genearte list of dx spots
        {

            double N = 0.017214206321 * DateTime.UtcNow.DayOfYear; // 2 * PI / 365 * Day

            double latitude = lat;
            double longitude = lon;
            latitude = latitude / 57.296;                         // lat * Math.PI / 180;  convert angle to rads

            double EQT = 0.000075 + (0.001868 * Math.Cos(N)) - (0.032077 * Math.Sin(N)) - (0.014615 * Math.Cos(2 * N)) - (0.040849 * Math.Sin(2 * N));

            double th = Math.PI * ((UTC100A / 12.0) - 1.0 + (longitude / 180.0)) + EQT;

            double delta = 0.006918 - (0.399912 * Math.Cos(N)) + (0.070257 * Math.Sin(N)) - (0.006758 * Math.Cos(2 * N)) + (0.000907 * Math.Sin(2 * N)) - (0.002697 * Math.Cos(3 * N)) + (0.00148 * Math.Sin(3 * N));

            double cossza = (Math.Sin(delta) * Math.Sin(latitude)) + (Math.Cos(delta) * Math.Cos(latitude) * Math.Cos(th));

            double SZA = (Math.Atan(-cossza / Math.Sqrt(-cossza * cossza + 1.0)) + 2.0 * Math.Atan(1)) * 57.296;  // ' the 57.296 converts this SZA to degrees

            return (int)Math.Abs(SZA); // 90=horizon, 108=total darkness, 100=dusk


        } // sunangle


        //===============================================================================================================================
        //===============================================================================================================================
        int ISSCount = 0; // get 360 deg of tracking data to draw a orbital plot
        Stopwatch ISSTimer = new Stopwatch();


        double ISSLONMap = 0;
        double ISSLATMap = 0;
        int ISSTime = 0;

        Stream stream;
        StreamReader reader;
        String content1;
        
        int[] ISS_XX = new int[300];
        int[] ISS_YY = new int[300];
      

        bool ISSUPDATE = false;   // true = update when SUN/MOON/ISS move udpates (once per minute)

        int ISSTimeStampStep = 60;

        private void ISSOrbit()   // plot orbit of ISS
        {
          
            ISSTimer.Restart();

            ISSCount = 0;

            while(ISSCount < 20) // was 360 for 90min orbit period 
            {

                Thread.Sleep(50);
              

                if (ISSTimer.ElapsedMilliseconds >= 50)
                {
                    ISSTimer.Restart();
                 
                    try
                    {

                        WebClient client = new WebClient();

                        if (ISSCount == 0) // very first data point to get timestamp
                        {
                            Debug.WriteLine("ISS First TRY");
                            stream = client.OpenRead("https://api.wheretheiss.at/v1/satellites/25544"); // get the current timestamp

                            reader = new StreamReader(stream);
                            content1 = reader.ReadToEnd();
                            reader.Close();


                            int ind2 = content1.IndexOf("timestamp") + 11;
                            string lon1 = content1.Substring(ind2, 10);
                            Debug.WriteLine("READ timestamp0= " + lon1);
                            ISSTime = Convert.ToInt32(lon1);

                        
                        }
                        else  // get all the remaining data points to plot orbit
                        {

                            Debug.WriteLine("SEND timestamp1= " + ISSTime);

                            stream = client.OpenRead("https://api.wheretheiss.at/v1/satellites/25544/positions?timestamps=" + ISSTime); //  "timestamp":1493561091

                            reader = new StreamReader(stream);
                            content1 = reader.ReadToEnd();
                            reader.Close();

                            Debug.WriteLine("internet data stream: " + content1);

                            int ind = content1.IndexOf("latitude") + 10;
                            string lat = content1.Substring(ind, 9);
                            Debug.WriteLine("GOT latitude= " + lat);
                            ISSLATMap = Convert.ToDouble(lat);
      
                            int ind1 = content1.IndexOf("longitude") + 11;
                            string lon = content1.Substring(ind1, 9);
                            Debug.WriteLine("GOT longitude= " + lon);
                            ISSLONMap = Convert.ToDouble(lon);
                                  

                            int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map
                            int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                            ISS_YY[ISSCount] = ISS_Y = (int)(((180 - (ISSLATMap + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                            ISS_XX[ISSCount] = ISS_X = (int)(((ISSLONMap + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E
                           
                            g.DrawLine(OrgPen, ISS_X, ISS_Y, ISS_X + 1, ISS_Y + 1);

                            
                        } //get next data point

                        ISSTime = ISSTime + ISSTimeStampStep;

                        ISSCount++;

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Could not read internet ISS data: " + ex);

                        textBox1.Text = "Could not Get Internet ISS data.";
                         
                    }

                  

                } // wait 1 sec , take next reading to plot orbit

                

            } // WHILE still getting readings to plot orbit

            ISSTimer.Stop();  // done ploting orbit

        } // ISSOrbit




        public static Color GrayLine_Last = Color.FromArgb(70, Color.Black);                       // used to check if setup.cs changed the color
        public static Band RX1Band_Last = 0;                                                      // to track a change in RX1 band

        private static Font font1 = new Font("Ariel", 10.5f, FontStyle.Regular, GraphicsUnit.Pixel);  // ke9ns add dx spot call sign font style
        private static Font font2 = new Font("Ariel", 9.0f, FontStyle.Regular, GraphicsUnit.Pixel);  // ke9ns add dx spot call sign font style

        private static Color grid_text_color = Color.Yellow;
        SolidBrush grid_text_brush = new SolidBrush(grid_text_color);

        private static Color Beacon_color = Color.Violet;
        SolidBrush Beacon_brush = new SolidBrush(Beacon_color);       // color when scanning a beacon station

        SolidBrush greenbrush = new SolidBrush(Color.Green);     // beacon signal strength color STRONG
        SolidBrush orangebrush = new SolidBrush(Color.Orange);        // beacon signal strength color not checked yet
        SolidBrush yellowbrush = new SolidBrush(Color.Yellow);   // beacon signal strength color LIGHT
        SolidBrush redbrush = new SolidBrush(Color.Red);              // normal red dot color on map  (beacon signal strength color no signal)

        SolidBrush graybrush = new SolidBrush(Color.DarkGray);        // beacon signal strength color not checked yet
        SolidBrush bluebrush = new SolidBrush(Color.Blue);       // beacon signal strength color not checked yet

        SolidBrush brownbrush = new SolidBrush(Color.SaddleBrown);              // 

        public static Image MAP = null; // holds bitmap image for SUN and GRAY LINE

        private static int[] spots = new int[100];  // holder for all the spots current on your entire band.

        public static int VFOLOW = 0;   // set in console rx1band for use in the mapper
        public static long VFOHIGH = 0;

        string[] country = new string[200];
        string[] call = new string[200];

        int[] yy = new int[200];  // increments the y axis down to allow multiple station names under a red dot

        const double DEG_to_RAD = Math.PI / 180.0; // 0.01745329251   converting angles to radians
        const double RAD_to_DEG = 180.0 / Math.PI; // 57.2957795131   converting radians back to angles


        Graphics g;

        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        private void TrackSun()  // ke9ns Thread opeation (runs in en-us culture) To create and draw Sun and/or Grayline Track
        {
           

            //-------------------------------------------------------------------
            // ke9ns grayline check 
            // horizontal lines top to bottom (North)90 to 0 to (-SOUTH)90
            // vertical lines left to right  -West(180) to 0 to +East(180)
            // Solstice June and Dec  (reach northern or southern extreme (about +/-24deg)
            // Equinox: March and Sept  (pass the equator)
            //
            // Sunset:                SUNANGLE = 90    (horizon = 90deg)
            // Civil Twilight:        SUNANGLE = 91 to 95 
            // Civil Dusk:            SUNANGLE = 96 
            // Nautical Twilight:     SUNANGLE = 97 to 101
            // Nautical Dusk:         SUNANGLE = 102
            // Astronomical Twilight: SUNANGLE = 103 to 107
            // Astronomical Dusk:     SUNANGLE = 108
            // Night:                 SUNANGLE > 108

            // ftp://ftp.swpc.noaa.gov/pub/latest/wwv.txt

            /*
            :Product: Geophysical Alert Message wwv.txt
            : Issued: 2016 Mar 31 2110 UTC
            # Prepared by the US Dept. of Commerce, NOAA, Space Weather Prediction Center
            #
            #          Geophysical Alert Message
            #
            Solar - terrestrial indices for 31 March follow.
              Solar flux 82 and estimated planetary A - index 7.
              The estimated planetary K - index at 2100 UTC on 31 March was 0.

              No space weather storms were observed for the past 24 hours.

              No space weather storms are predicted for the next 24 hours

         */

            console.setupForm.clrbtnGrayLine_Changed(this, EventArgs.Empty);               // get color from setup at startup

          
            if (Console.noaaON == 0) // only do this if space weather is OFF on the main console window
            {
                textBox1.Text += "Attempt login to:  NOAA Space Weather Prediction Center \r\n";

                NOAA(); // get noaa space data

            }

         //   textBox1.Text += "NOAA Download complete \r\n";


            //--------------------------------------------------------------------------------------------
            // stay in this thread loop until you turn off tracking
            for (; SP5_Active == 1;) // turn on track map (sun, grayline, voacap, or beacon)
            {

                Thread.Sleep(50);

                if (SP5_Active == 0) continue; // tracking turned OFF

                // ke9ns add Check if you need a VOACAP update, but only if your not in the middel of doing a VOACAP update and the VOACAP is turned ON
                if ((checkBoxMUF.Checked == true) && (VOARUN == false))  // 
                {
                    if ((int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")) != console.last_MHZ)
                    {

                        if (((int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")) > 0) && ((int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")) < 30))
                        {
                            console.last_MHZ = (int)double.Parse(console.txtVFOAFreq.Text.Replace(",", "."));
                            VOACAP_CHECK();
                        }
                    }
                    else if ((console.RX1DSPMode != console.last_MODE))
                    {
                        console.last_MODE = console.RX1DSPMode;
                        VOACAP_CHECK();
                    }
                    else if (Last_HOUR != DateTime.UtcNow.Hour.ToString().PadLeft(2))
                    {
                        VOACAP_CHECK();
                    }
                   


                } // ke9ns voacap

                if (  ((beacon4 == true)) ||((chkSUN.Checked == true) || (chkGrayLine.Checked == true)) &&
                       ((Display.CurrentDisplayMode == DisplayMode.PANADAPTER) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL))
                   )
                {

                    UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW
                    UTCNEW = Convert.ToInt16(FD);                                    // convert 24hr UTC to int


                    if (chkSUN.Checked == true)
                    {
                        SUN = true; // activate display
                      
                    }
                    else
                    {
                        SUN = false;
                     
                    }

                    if (chkMoon.Checked == true)
                    {
                        MOON = true; // activate display

                    }
                    else
                    {
                        MOON = false;

                    }

                    if (chkISS.Checked == true)
                    {
                        ISS = true; // activate display

                    }
                    else
                    {
                        ISS = false;

                    }


                    if (chkGrayLine.Checked == true) GRAYLINE = true; // activate display
                    else GRAYLINE = false;



                   


                        // Do a SUN, GRAYLINE, or DX country/callsign update

                        // Check for TIME CHANGE
                        // Check for GrayLine COLOR change
                        // Check for DX spot list change
                        // Check in any checkboxes changed state
                        // Check if the number of spots on map changed (DXK is the # of spots on the current panadapter)
                        // Check for Transmitting (dont update if transmitting)

                        if ((!console.MOX) && ((UTCNEW != UTCLAST2) || (Setup.DisplayGrayLineColor != GrayLine_Last) || (Map_Last > 0) ||
                        (((DX_Index != DX_Last) || (Console.DXK != DXK_Last) || (console.RX1Band != RX1Band_Last)) && (SP8_Active == 1)))
                        || ((beacon4 == true))      )  // Beacon scanner
                    {

                      
                        Debug.WriteLine("Update DX Spots on Screen=================");

                        GrayLine_Last = Setup.DisplayGrayLineColor; // store last color for compare next time

                        DXK_Last = Console.DXK;

                        DX_Last = DX_Index;                    // if the DX spot list changed
                      
                        RX1Band_Last = console.RX1Band;

                      
                        if ( (UTCNEW != UTCLAST2) || (Setup.DisplayGrayLineColor != GrayLine_Last) || (Map_Last == 1))
                        {
                            Debug.WriteLine("==================== SUN and GrayLine calculations=================");

                            if (UTCNEW != UTCLAST2) ISSUPDATE = true;


                                UTCLAST2 = UTCNEW;                            // store time for compare next time

                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns Position of SUN (viewed from SUN) using the ke9ns world skin 
                            // X  left edge starts 5.6% in, right edge ends 94.7%  (with Display.DIS_X at 100%)
                            // y  top edge starts 7.8% in, bottom edge ends 90.1%  (with Display.DIS_Y at 100%)
                            // equirectangle project map has longitude lines every 15deg (1 per hour) center is 0 UTC and sun moves to the left
                            // left edge is 2359 UTC (right edge is 0 UTC)

                            //  Debug.WriteLine("Mouse x " + Console.DX_X); // mouse on picDisplay coordinates
                            //  Debug.WriteLine("mouse y " + Console.DX_Y);

                            //   Debug.WriteLine("x pos " + Display.DIS_X); // 
                            //  Debug.WriteLine("y pos " + Display.DIS_Y);

                            double g1 = (double)(360 / 365.25 * (DateTime.UtcNow.DayOfYear + (DateTime.UtcNow.Hour / 24))); // convert days to angle

                            g1 = g1 * Math.PI / 180;                             // convert angle to rads

                            double D = (double)(0.396372 - (22.91327 * Math.Cos(g1)) + (4.02543 * Math.Sin(g1)) - (0.387205 * Math.Cos(2 * g1))
                                + (0.051967 * Math.Sin(2 * g1)) - (0.154527 * Math.Cos(3 * g1)) + (0.084798 * Math.Sin(3 * g1)));

                            D = D / 24;                                        // convert to percent of 100

                            Sun_Top1 = 26;                                     // 26  45 Y pixel location of top of map
                            Sun_Bot1 = 465;                                    // 465  485 Y pixel locaiton of bottom of map 

                            int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map


                            int Sun_Top = 187;                                 // 207 y pixel location North Hem summer solstice
                            int Sun_Bot = 303;                                 // 324 Y pixel location of North Hem winter solstice

                            int Sun_WidthY = Sun_Bot - Sun_Top;                // # of Y pixes width between solstices
                            int Sun_WidthHalf = (Sun_WidthY / 2) + Sun_Top;    // y= 265 put you at equator

                            int Sun_Diff = Sun_WidthHalf - Sun_Top;            // 

                            Sun_Y = (int)(Sun_WidthHalf - (double)(Sun_Diff * D)); // position of SUN on longitude of map (based of time of year)

                            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                            FD = UTCD.ToString("HH");
                            UTC100 = Convert.ToInt16(FD);
                            FD = UTCD.ToString("mm");

                            UTC100A = (UTC100 + (Convert.ToInt16(FD) / 60F));  // used by SUNANGLE routine

                            UTC100 = (UTC100 + (Convert.ToInt16(FD) / 60F)) / 24F; // used by SUN track routine convert to 100% 2400 = 100%

                            Sun_Left = 57;                                       // Left side at equator used by Grayline routine
                            Sun_Right = 939;

                            int Sun_Width = Sun_Right - Sun_Left; //used by sun track routine

                            Sun_X = (int)(Sun_Left + (float)(Sun_Width * (1.0 - UTC100))); // position of SUN on equator based on time of day


                        
                            if ((GRAYLINE == true))
                            {

                                int qq = 0;       // index for accumulating lat edges
                                int ww = 0;       // 0=no edges found, 1=1 edge found, 2=2 edges found

                                int tt = 0;
                                bool check_for_light = true; // true = in the dark, so looking for light, false = in the light, so looking for the dark
                                int tempsun_ang = 0; // temp holder for sun angle


                                //----------------------------------------------------------------------------------
                                //----------------------------------------------------------------------------------
                                // check for Dark (Right side of map)
                                //----------------------------------------------------------------------------------
                                //----------------------------------------------------------------------------------

                                for (double lat = 90.0; lat >= -90.0; lat = lat - 0.5)  // horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                {

                                    if ((SUNANGLE(lat, -180.0) >= 96) && (SUNANGLE(lat, 180.0) >= 96)) tt = 1; // dark on edges of screen 
                                    else tt = 0; // light on at least 1 side

                                    zz = (int)((qq / 360.0 * Sun_WidthY1) + Sun_Top1); // 360 = number of latitude points, determine the y pixel for this latitude grayline

                                    GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 1] = 0;

                                    if (SUNANGLE(lat, 0.0) < 96) check_for_light = false; // your in light so check for dark
                                    else check_for_light = true; // >= 96 your in dark so check for light


                                    for (double lon = 0.0; lon <= 180.0; lon = lon + 0.5)
                                    {
                                        tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120


                                        if (check_for_light == true) // in dark, looking for light
                                        {

                                            if (tempsun_ang < 96) // found light
                                            {

                                                GrayLine_Pos[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels

                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 40.0; // 40.0 jump a little to save time
                                                check_for_light = false; // now in light so check for dark

                                            } // found light

                                        } // your in dark so check for light
                                        else // in light so check for dark
                                        {

                                            if (tempsun_ang >= 96) // in Dark (found it)
                                            {

                                                GrayLine_Pos[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels

                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 40.0;         // 40.0 jump a little to save time
                                                check_for_light = true; // in dark so now check for light

                                            } // found dark


                                        }// in light so check for dark


                                    } // for lon (right side of map)

                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------
                                    // check for Dark (left side of map
                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------

                                    if (ww < 2) // still need at least 1 edge (maybe 2)
                                    {

                                        if (SUNANGLE(lat, 0.0) < 96) check_for_light = false; // your in light so check for dark
                                        else check_for_light = true; // >= 90 your in dark so check for light

                                        for (double lon = 0.0; lon >= -180.0; lon = lon - 0.5)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                        {
                                            tempsun_ang = SUNANGLE(lat, lon);

                                            if (check_for_light == true)
                                            {

                                                if (tempsun_ang < 96) // found light
                                                {

                                                    GrayLine_Pos[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                    GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

                                                    ww++;
                                                    if (ww == 2) break;      // if we have 2 edge then done

                                                    lon = lon - 40.0;           // 40.0 jump a little to save time
                                                    check_for_light = false; // now in light so check for dark

                                                } // found light

                                            } // your in dark so check for light
                                            else // in light so check for dark
                                            {

                                                if (tempsun_ang >= 96) // in Dark (found it)
                                                {
                                                    GrayLine_Pos[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                    GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

                                                    ww++;
                                                    if (ww == 2) break;    // if we have 2 edge then done

                                                    lon = lon - 40;         // 40.0 jump a little to save time
                                                    check_for_light = true; // in dark so now check for light

                                                } // found dark


                                            }// in light so check for dark


                                        } // for lon  (left side of map)


                                    } // ww as < 2 on the right side attempt


                                    if (ww == 0) // if still less than 2 edges then just zero out
                                    {
                                        GrayLine_Pos[zz + 2, 0] = GrayLine_Pos[zz + 1, 0] = GrayLine_Pos[zz, 0] = 0;
                                        GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1] = 0;
                                    }
                                    else if (ww == 1)
                                    {
                                        GrayLine_Pos[zz + 2, 0] = GrayLine_Pos[zz + 1, 0] = GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 0] + GrayLine_Pos[zz, 1];
                                        GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1] = GrayLine_Pos[zz, 0];
                                    }

                                    ww = 0; // start over for next lat

                                    if (tt == 1) // if dark on both edges then figure out which is which and signal display
                                    {

                                        if ((GrayLine_Pos[zz, 0] - GrayLine_Pos[zz, 1]) > 0)
                                        {
                                            GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 1; // ,0 is on right side, ,1 is on left side
                                        }
                                        else if ((GrayLine_Pos[zz, 1] - GrayLine_Pos[zz, 0]) > 0)
                                        {
                                            GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 2; // ,0 is on left side, ,1 is on right side
                                        }
                                        else
                                        {
                                            GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 0;             // dark in center of map, (standard)
                                        }

                                    }
                                    else
                                    {
                                        GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 0;             // dark in center of map, (standard)
                                    }

                                    qq++; // get next lat

                                } //  for (int lat = 90;lat >= -90;lat--)   horizontal lines top to bottom (North)90 to 0 to -90 (South)



                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                // check for dusk (right side first)
                                //-------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                qq = 0;
                                ww = 0;

                                for (double lat = 90.0; lat >= -90.0; lat = lat - 0.5)  // 0.5 horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                {
                                  
                                    if ((SUNANGLE(lat, -180.0) >= 90) && (SUNANGLE(lat, 180.0) >= 90)) tt = 1; // dark on edges of screen 
                                    else tt = 0; // light on at least 1 side

                                    zz = (int)((qq / 360.0 * Sun_WidthY1) + Sun_Top1); // 360 = number of latitude points, determine the y pixel for this latitude grayline

                                 
                                    GrayLine_Pos1[zz, 0] = GrayLine_Pos1[zz, 1] = 0;
                                     
                                    if (SUNANGLE(lat, 0.0) < 90) check_for_light = false; // your in light so check for dark
                                    else check_for_light = true; // >= 96 your in dark so check for light

                                
                                    for (double lon = 0.0; lon <= 180.0; lon = lon + 0.5)   // 0.5
                                    {
                                        tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120

                                     
                                        if (check_for_light == true) // in dark, looking for light
                                        {

                                            if ((tempsun_ang < 90) ) // found light
                                            {

                                                GrayLine_Pos1[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww]; // make sure to cover unused pixels

                                             
                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 1.0; // 40.0  jump a little to save time

                                                check_for_light = false; // now in light so check for dark

                                            } // found light

                                        } // your in dark so check for light
                                        else // in light so check for dark
                                        {

                                            if ((tempsun_ang >= 90))  // in Dark (found it)
                                            {

                                                GrayLine_Pos1[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww]; // make sure to cover unused pixels
                                             
                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 1.0;         // 40.0 jump a little to save time
                                                check_for_light = true; // in dark so now check for light

                                            } // found dark


                                        }// in light so check for dark


                                    } // for lon (right side of map)

                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------
                                    // check for gray (left side of map
                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------

                                    if (ww < 2) // still need at least 1 edge (maybe 2)
                                    {

                                        if (SUNANGLE(lat, 0.0) < 90) check_for_light = false; // your in light so check for dark
                                        else check_for_light = true; // >= 90 your in dark so check for light

                                        for (double lon = 0.0; lon >= -180.0; lon = lon - 0.5)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                        {
                                            tempsun_ang = SUNANGLE(lat, lon);

                                            if (check_for_light == true)
                                            {

                                                if ((tempsun_ang < 90)) // found light
                                                {

                                                    GrayLine_Pos1[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline
                                                    GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww];
                                                 
                                                    ww++;
                                                    if (ww == 2) break;      // if we have 2 edge then done

                                                    lon = lon - 1.0;           // 40.0 jump a little to save time
                                                    check_for_light = false; // now in light so check for dark

                                                } // found light

                                            } // your in dark so check for light
                                            else // in light so check for dark
                                            {

                                                if (tempsun_ang >= 90) // in Dark (found it)
                                                {
                                                    GrayLine_Pos1[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                    GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww];
                                                 
                                                    ww++;
                                                    if (ww == 2) break;    // if we have 2 edge then done

                                                    lon = lon - 1.0;         //40.0  jump a little to save time
                                                    check_for_light = true; // in dark so now check for light

                                                } // found dark


                                            }// in light so check for dark


                                        } // for lon  (left side of map)


                                    } // ww as < 2 on the right side attempt


                                    if (ww == 0) // if still less than 2 edges then just zero out
                                    {
                                        GrayLine_Pos1[zz + 2, 0] = GrayLine_Pos1[zz + 1, 0] = GrayLine_Pos1[zz, 0] = 0;
                                        GrayLine_Pos1[zz + 2, 1] = GrayLine_Pos1[zz + 1, 1] = GrayLine_Pos1[zz, 1] = 0;
                                    }
                                    else if (ww == 1)
                                    {
                                        GrayLine_Pos1[zz + 2, 0] = GrayLine_Pos1[zz + 1, 0] = GrayLine_Pos1[zz, 0] = GrayLine_Pos1[zz, 0] + GrayLine_Pos1[zz, 1];
                                        GrayLine_Pos1[zz + 2, 1] = GrayLine_Pos1[zz + 1, 1] = GrayLine_Pos1[zz, 1] = GrayLine_Pos1[zz, 0];
                                    }

                                    ww = 0; // start over for next lat

                                    if (tt == 1) // if gray on both edges then figure out which is which and signal display
                                    {

                                        if ((GrayLine_Pos1[zz, 0] - GrayLine_Pos1[zz, 1]) > 0)
                                        {
                                            GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 1; // ,0 is on right side, ,1 is on left side
                                        }
                                        else if ((GrayLine_Pos1[zz, 1] - GrayLine_Pos1[zz, 0]) > 0)
                                        {
                                            GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 2; // ,0 is on left side, ,1 is on right side
                                        }
                                        else
                                        {
                                            GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 0;             // gray in center of map, (standard)
                                        }

                                    }
                                    else
                                    {
                                        GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 0;             // gray in center of map, (standard)
                                    }

                                    qq++; // get next lat

                                } //  for (int lat = 90;lat >= -90;lat--)   horizontal lines top to bottom (North)90 to 0 to -90 (South)


                            } // GRAYLINE = true

                        } //  if ( (UTCNEW != UTCLAST2) || (Setup.DisplayGrayLineColor != GrayLine_Last) )  time changeed or color change


                        if ((Map_Last == 2) && (SP4_Active == 0)) updatemapspots(); // if you need a map update make sure your not in the middle of updating your dx spot list

                        Map_Last = 0;

                        if (SP5_Active == 0) continue;


                        //-------------------------------------------------------------------------------------------------
                        //-------------------------------------------------------------------------------------------------
                        // draw sun tracker and gray line
                        //-------------------------------------------------------------------------------------------------
                        //-------------------------------------------------------------------------------------------------

                        if ((Console.DXR == 0))  MAP = new Bitmap(Map_image); // load up Map image
                        else MAP = new Bitmap(Map_image2); // load up Map image

                         g = Graphics.FromImage(MAP);

                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        //  g.SmoothingMode = SmoothingMode.AntiAlias;


                        //------------------------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------------------------

                      
                            //------------------------------------------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------------------------------------------

                            if (SUN == true)
                        {
                            Debug.WriteLine("=====================SUN CALCULATIONS");

                            Image src = new Bitmap(sun_image); // load up SUN image ( use PNG to allow transparent background)

                            g.DrawImage(src, Sun_X - 11, Sun_Y - 13, 23, 27); // was -10 , -10      draw SUN 20 x 20 pixel

                            if (Console.noaaON == 0) // do below if console space weather OFF
                            {
                                g.DrawString("SFI " + SFI.ToString("D"), font1, grid_text_brush, Sun_X + 15, Sun_Y - 10);
                                g.DrawString("A " + Aindex.ToString("D") + ", " + RadioBlackout, font1, grid_text_brush, Sun_X + 15, Sun_Y);

                                if (suncounter > 40) // check every 40 minutes
                                {
                                    NOAA();
                                    suncounter = 0;
                                    Debug.WriteLine("NOAA GET=============");

                                }
                                else
                                {
                                    suncounter++;
                                }
                            }
                            else // console space weather ON
                            {

                                g.DrawString("SFI " + Console.SFI.ToString("D"), font1, grid_text_brush, Sun_X + 15, Sun_Y - 10);
                                g.DrawString("A " + Console.Aindex.ToString("D") + ", " + Console.RadioBlackout, font1, grid_text_brush, Sun_X + 15, Sun_Y);

                            }

                        } //  sun tracker enabled



                      

                        //---------------------------------------------------------------------------
                        if (GRAYLINE == true)
                        {

                            Pen p3 = new Pen(GrayLine_Last, 1.0f); // dark
                            Pen p4 = new Pen(GrayLine_Last, 1.0f); // dusk


                            for (int ee = Sun_Top1; ee < Sun_Bot1; ee++)
                            {

                                //-----------------------------------------------------------------
                                // ke9ns dusk
                                if (GrayLine_Pos3[ee] == 0) // not dusk on edges on screen
                                {
                                    if ( (GrayLine_Pos1[ee, 0]) == 0 && (GrayLine_Pos1[ee, 1] == 0) &&
                                        ( (ee < (Sun_Top1 + 100)) && ((DateTime.UtcNow.DayOfYear > GrayWinterStart ) || (DateTime.UtcNow.DayOfYear < GrayWinterStop)) ||  // winter time northern area is always dark
                                        (ee > (Sun_Bot1 - 100)) && (((DateTime.UtcNow.DayOfYear >= GraySummerStart) && (DateTime.UtcNow.DayOfYear < GraySummerStop))  ) )   // summer time southern area is always dark
                                       ) // if the line is empty but your on top in the winter then draw a line anyway
                                    {
                                        g.DrawLine(p4, Sun_Left, ee, Sun_Right, ee);
                                    }
                                    else
                                    {
                                        g.DrawLine(p4, GrayLine_Pos1[ee, 0], ee, GrayLine_Pos1[ee, 1], ee);    // draw line between the left and right sides (starting from the middle)
                                    }
                                
                                }
                                else if (GrayLine_Pos3[ee] == 1)
                                {
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 0], ee, Sun_Right, ee);
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 1], ee, Sun_Left, ee);
                                  
                                }
                                else
                                {
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 1], ee, Sun_Right, ee);
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 0], ee, Sun_Left, ee);
                                 
                                }

                                // ((ee < (Sun_Top1 + 100)) && (DateTime.UtcNow.DayOfYear > 270) || (ee > (Sun_Bot1 - 100)) && (DateTime.UtcNow.DayOfYear < 150))
                                //     (ee > (Sun_Bot1 - 100)) && (((DateTime.UtcNow.DayOfYear >= GrayWinterStop) && (DateTime.UtcNow.DayOfYear < GraySummerStart)) || ((DateTime.UtcNow.DayOfYear > GraySummerStop) && (DateTime.UtcNow.DayOfYear < GrayWinterStart))))   // summer time southern area is always dark

                                //-----------------------------------------------------------------
                                // ke9ns dark
                                if (GrayLine_Pos2[ee] == 0)  // not dark on edges on screen
                                {
                                    if ((GrayLine_Pos[ee, 0]) == 0 && (GrayLine_Pos[ee, 1] == 0) &&
                                       ((ee < (Sun_Top1 + 100)) && ((DateTime.UtcNow.DayOfYear > GrayWinterStart) || (DateTime.UtcNow.DayOfYear < GrayWinterStop)) ||  // winter time northern area is always dark
                                        (ee > (Sun_Bot1 - 100)) && (((DateTime.UtcNow.DayOfYear >= GraySummerStart) && (DateTime.UtcNow.DayOfYear < GraySummerStop)) ))   // summer time southern area is always dark
                                     )
                                    {
                                        g.DrawLine(p3, Sun_Left, ee, Sun_Right, ee);
                                    }
                                    else
                                    {
                                        g.DrawLine(p3, GrayLine_Pos[ee, 0], ee, GrayLine_Pos[ee, 1], ee);       // draw line between the left and right sides (starting from the middle)
                                    }
                                 
                                }
                                else if (GrayLine_Pos2[ee] == 1)
                                {
                                    g.DrawLine(p3, GrayLine_Pos[ee, 0], ee, Sun_Right, ee);
                                    g.DrawLine(p3, GrayLine_Pos[ee, 1], ee, Sun_Left, ee);
                                  
                                }
                                else
                                {
                                    g.DrawLine(p3, GrayLine_Pos[ee, 1], ee, Sun_Right, ee);
                                    g.DrawLine(p3, GrayLine_Pos[ee, 0], ee, Sun_Left, ee);
                                 
                                }


                            }  // for loop  

                        } // if GRAYLINE enabled

                        if (MOON == true)
                        {

                            Debug.WriteLine("=====================MOON CALCULATIONS");

                            //--------------------------------------------------------------------
                            // Convert Universal Time to Ephemeris Time
                            // jd = juliandate(UTC, 'yyyy/mm/dd HH:MM:SS');


                            double y = DateTime.UtcNow.Year; //  year 2017
                            double m = DateTime.UtcNow.Month; // month = 1 to 12
                            double D1 = DateTime.UtcNow.Day; // day of month 1 to 31
                            double H = DateTime.UtcNow.Hour; // hour 0 to 24

                            double JD = DateTime.UtcNow.ToOADate() + 2415018.5; // convert OLE AUtomation Date to Julian Date
                            Debug.WriteLine("Julian day " + JD);

                            double d = JD - 2451543.5; // convert Julian Date to something the orbital routine can use. (the Day number)
                            Debug.WriteLine("Current Day number " + d);

                            //  d = -3543; // for April 19th 1990 test

                            //--------------------------------------------------------------------------------------------
                            // Moon oribital elements data set
                            //--------------------------------------------------------------------------------------------

                            // http://aa.usno.navy.mil/data/docs/AltAz.php
                            // data set for MOON in DEG (must be converted to Radians before a SIN or COS or TAN is used)
                            // https://spaceflight.nasa.gov/realdata/elements/

                            double N = (125.1228 - 0.0529538083 * d);  // DEG RA of the long ascending node defines the ascending and descending orbit locations with respect to the earths equatorial plane
                            double i = 5.1454;                          // DEG Inclination to the ecliptic deg  defines the orientation of the orbit with respect to the earths equator <
                            double w = (318.0634 + 0.1643573223 * d);   // DEG Arg of Perigee (Perihelion) defines the low point,perigee of the orbit is with respect to the earths surface.
                            double a = 60.2666;                         //     Mean distance (Earth equitorial radii) Semi-major axis defines size of the orbit
                            double e = 0.0549006;                       //     Eccentricity defines the shape of the orbit
                            double M = (115.3654 + 13.0649929509 * d);  // DEG True/mean anomaly defines where the satellite is within the orbit with respect to perigee (low point)
                            double ecl = 23.43704;                      // DEG Angle of the obliquity of the ecliptic plane (obliquity of the earth)


                            //--------------------------------------
                            Debug.WriteLine("Starting N " + N);
                            Debug.WriteLine("Starting w " + w);
                            Debug.WriteLine("Starting M " + M);

                            while (N > 360) // normalize values
                            {
                                N = N - 360.0;
                            }
                            while (N < 0)
                            {
                                N = N + 360.0;
                            }

                            while (w > 360)
                            {
                                w = w - 360.0;
                            }
                            while (w < 0)
                            {
                                w = w + 360.0;
                            }

                            while (M > 360)
                            {
                                M = M - 360.0;
                            }
                            while (M < 0)
                            {
                                M = M + 360.0;
                            }


                            //-----------------------------------------------------------------------------------------
                            // Compute E, the eccentric anomaly 
                            // E0 is the eccentric anomaly approximation estimate  (this will initially have a relativly high error)

                            double E0 = M + (RAD_to_DEG) * e * (Math.Sin(M * (DEG_to_RAD))) * (1.0 + e * (Math.Cos(M * (DEG_to_RAD))));

                            // Compute E1 and set it to E0 until the E1 == E0
                            double E1 = E0 - (E0 - (RAD_to_DEG) * e * (Math.Sin(E0 * (DEG_to_RAD))) - M) / (1 - e * (Math.Cos(E0 * (DEG_to_RAD))));

                            while (E1 - E0 > .000005) // compare in DEG
                            {
                                E0 = E1;
                                E1 = E0 - (E0 - (RAD_to_DEG) * e * (Math.Sin(E0 * (DEG_to_RAD))) - M) / (1 - e * (Math.Cos(E0 * (DEG_to_RAD))));
                            }

                            double E = E1;

                            //---------------------------------------------------------------------------------
                            // Compute rectangular coordinates (x, y) in the plane of the lunar orbit
                            double x1 = a * (Math.Cos(E * (DEG_to_RAD)) - e);
                            double y1 = a * (Math.Sqrt(1 - e * e) * Math.Sin(E * (DEG_to_RAD)));

                            //---------------------------------------------------------------------------------
                            // convert this to distance and true anomaly
                            double r = Math.Sqrt(y1 * y1 + x1 * x1);
                            double v = RAD_to_DEG * Math.Atan2(y1, x1);


                            while (v > 360)
                            {
                                v = v - 360.0;
                            }
                            while (v < 0)
                            {
                                v = v + 360.0;
                            }


                            //---------------------------------------------------------------------------------
                            // Compute moon's position in ecliptic coordinates
                            double xeclip = r * (Math.Cos(N * (DEG_to_RAD)) * Math.Cos((v + w) * (DEG_to_RAD)) - Math.Sin(N * (DEG_to_RAD)) * Math.Sin((v + w) * (DEG_to_RAD)) * Math.Cos(i * (DEG_to_RAD)));
                            double yeclip = r * (Math.Sin(N * (DEG_to_RAD)) * Math.Cos((v + w) * (DEG_to_RAD)) + Math.Cos(N * (DEG_to_RAD)) * Math.Sin((v + w) * (DEG_to_RAD)) * Math.Cos(i * (DEG_to_RAD)));
                            double zeclip = r * (Math.Sin((v + w) * (DEG_to_RAD)) * Math.Sin(i * (DEG_to_RAD)));

                            //---------------------------------------------------------------------------------
                            // Ecliptic LONG and LAT    (RAD TO DEG = 57.2957795131)

                            double MoonLON = RAD_to_DEG * Math.Atan2(yeclip, xeclip); //longitude  geocentric (earth centered) position in the ecliptic coordinate system.
                            double MoonLAT = RAD_to_DEG * Math.Atan2(zeclip, Math.Sqrt(xeclip * xeclip + yeclip * yeclip)); //latitude

                            while (MoonLON > 360) // normalize
                            {
                                MoonLON = MoonLON - 360.0;
                            }
                            while (MoonLON < 0)
                            {
                                MoonLON = MoonLON + 360.0;
                            }


                            //-----------------------------------------------------------------------------------------
                            // Perturbations
                            double Lm = (N + w + M) % 360; // Moons mean long
                            double F = (Lm - N) % 360;    // Moons argument of latitude

                            double wSun = (282.9404 + 4.70935E-5 * d) % 360; // longitude of perihelion
                            double Ms = (356.0470 + 0.9856002585 * d) % 360; // suns mean anomaly
                            double Ls = (wSun + Ms) % 360;   // Suns mean longitude
                            double D = Lm - Ls; // Moons mean elongation


                            // Add these terms to the Moon's longitude (degrees):
                            double LunarPLon = -1.274 * Math.Sin((M - 2 * D) * DEG_to_RAD) // (the Evection)
                               + 0.658 * Math.Sin((2 * D) * DEG_to_RAD)                     //(the Variation)
                               - 0.186 * Math.Sin((Ms) * DEG_to_RAD)                        //(the Yearly Equation)
                               - 0.059 * Math.Sin((2 * M - 2 * D) * DEG_to_RAD)
                               - 0.057 * Math.Sin((M - 2 * D + Ms) * DEG_to_RAD)
                               + 0.053 * Math.Sin((M + 2 * D) * DEG_to_RAD)
                               + 0.046 * Math.Sin((2 * D - Ms) * DEG_to_RAD)
                               + 0.041 * Math.Sin((M - Ms) * DEG_to_RAD)
                               - 0.035 * Math.Sin((D) * DEG_to_RAD)                         //(the Parallactic Equation)
                               - 0.031 * Math.Sin((M + Ms) * DEG_to_RAD)
                               - 0.015 * Math.Sin((2 * F - 2 * D) * DEG_to_RAD)
                               + 0.011 * Math.Sin((M - 4 * D) * DEG_to_RAD);

                            // Add these terms to the Moon's latitude (degrees):
                            double LunarPLat = -0.173 * Math.Sin(F - 2 * D)
                              - 0.055 * Math.Sin((M - F - 2 * D) * DEG_to_RAD)
                              - 0.046 * Math.Sin((M + F - 2 * D) * DEG_to_RAD)
                              + 0.033 * Math.Sin((F + 2 * D) * DEG_to_RAD)
                              + 0.017 * Math.Sin((2 * M + F) * DEG_to_RAD);

                            MoonLON = MoonLON + LunarPLon;
                            MoonLAT = MoonLAT + LunarPLat;

                            //-------------------------------------------------------------------------------------

                            while (MoonLON > 360) // re-normalize
                            {
                                MoonLON = MoonLON - 360.0;
                            }
                            while (MoonLON < 0)
                            {
                                MoonLON = MoonLON + 360.0;
                            }


                            //--------------------------------------------------------------------------------------------------
                            // Geocentric (earth centered) coordinates(ie standing at the center of the earth)
                            r = 1.0;

                            double xh = r * Math.Cos(MoonLON * DEG_to_RAD) * Math.Cos(MoonLAT * DEG_to_RAD);
                            double yh = r * Math.Sin(MoonLON * DEG_to_RAD) * Math.Cos(MoonLAT * DEG_to_RAD);
                            double zh = r * Math.Sin(MoonLAT * DEG_to_RAD);

                            double xg = xh;
                            double yg = yh;
                            double zg = zh;

                            Debug.WriteLine("xg " + xg);
                            Debug.WriteLine("yg " + yg);
                            Debug.WriteLine("zg " + zg);

                            Debug.WriteLine(" ");
                            //--------------------------------------------------------------------------------------------------
                            // Equatorial coordinates (rectangular coordinates)
                            // ecl = angle of the obliquity of the ecliptic  23.43704

                            double xe = xg;
                            double ye = yg * Math.Cos(ecl * DEG_to_RAD) - zg * Math.Sin(ecl * DEG_to_RAD);
                            double ze = yg * Math.Sin(ecl * DEG_to_RAD) + zg * Math.Cos(ecl * DEG_to_RAD);

                            Debug.WriteLine("xe " + xe);
                            Debug.WriteLine("ye " + ye);
                            Debug.WriteLine("ze " + ze);

                            double RA = RAD_to_DEG * Math.Atan2(ye, xe);                            // Right Ascension deg, and Declination deg
                            double dec = RAD_to_DEG * Math.Atan2(ze, Math.Sqrt(xe * xe + ye * ye));

                            Debug.WriteLine("start RA " + RA);   // should be 309.5 for test but in Hours divide by 15
                            Debug.WriteLine("dec " + dec);       // should be -19.1 for test

                            while (RA > 360) // normalize
                            {
                                RA = RA - 360.0;
                            }
                            while (RA < 0)
                            {
                                RA = RA + 360.0;
                            }

                            Debug.WriteLine("RA " + RA); // should be 309.5 for test
                            Debug.WriteLine("dec " + dec); // should be -19.1 for test

                            Debug.WriteLine(" ");


                            //--------------------------------------------------------------------------------------------------
                            // vernal (spring) equinox position = 0deg RA and counts eastward 0-360 as 24hours (15deg per hour)

                            double MJD = JD - 2400000.5;

                            double T = (MJD - 51544.5) / 36525.0;

                            Debug.WriteLine("Current T " + T);

                            double GMST = ((280.46061837 + 360.98564736629 * (MJD - 51544.5)) + 0.000387933 * T * T - T * T * T / 38710000.0) % 360.0;  // ST in degrees
                            if (GMST < 0) // normalize
                            {
                                GMST += 360.0;
                            }

                            Debug.WriteLine("Current GMST in deg " + GMST);

                            double ST = GMST / 15; // current Sidereal time in hours

                            Debug.WriteLine("Current GMST (hours) " + ST);  // ST in hours
                            Debug.WriteLine(" ");


                            //------------------------------------------------------------------------------------------------
                            // FIND LST and AZIMUTH and ELEVATION

                            double LST = GMST + (double)udDisplayLong.Value; // use your local Longitude to find LST and your azimuth
                            Debug.WriteLine("ORIG LST " + LST); // 

                            // your local long is already in -180+180 but GMST is 0-360 already where 0-180 is east, 181-360 is west

                            if (LST > 360)
                            {
                                LST = LST - 180;
                            }
                            if (LST < 0)
                            {
                                LST = LST - 360.0;
                            }

                            Debug.WriteLine("NEW LST " + LST);
                            Debug.WriteLine(" ");


                            double HA = LST - RA;

                            Debug.WriteLine("ORIG HA " + HA);

                            // normalize -360+360 to -180+180
                            HA = (HA > 180) ? HA - 360 : (HA < -180) ? HA + 360 : HA; // in deg

                            Debug.WriteLine("NEW HA " + HA);
                            Debug.WriteLine(" ");

                            double x2 = Math.Cos(HA * DEG_to_RAD) * Math.Cos(dec * DEG_to_RAD);
                            double y2 = Math.Sin(HA * DEG_to_RAD) * Math.Cos(dec * DEG_to_RAD);
                            double z2 = Math.Sin(dec * DEG_to_RAD);

                            double xhor = x2 * Math.Sin((double)udDisplayLat.Value * DEG_to_RAD) - z2 * Math.Cos((double)udDisplayLat.Value * DEG_to_RAD);
                            double yhor = y2;
                            double zhor = x2 * Math.Cos((double)udDisplayLat.Value * DEG_to_RAD) + z2 * Math.Sin((double)udDisplayLat.Value * DEG_to_RAD);

                            double az = RAD_to_DEG * Math.Atan2(yhor, xhor) + 180.0; // 0=north, 90=East, 180=South, 270=West (just like your rotor controller)
                            double alt = RAD_to_DEG * Math.Asin(zhor);  // 0=Horizon, 90=Zenith, -90 = Straight Down to heck

                            Debug.WriteLine("Azimuth deg " + az);
                            Debug.WriteLine("Altitude " + alt);
                            Debug.WriteLine(" ");

                            Moon_AZ = (int)az; // beam heading value
                            Moon_ALT = (int)alt;  // altitude value

                            //-----------------------------------------------

                            Debug.WriteLine("Moon RA (hours) " + (RA / 15));


                            double MoonLONMap = 0;

                            MoonLONMap = (GMST - RA);

                            //---------------------------------------------------------------
                            // normalize -360 +360 and convert directly to -180 +180
                            // if value > 180 then it must be on East side. if value < -180 must be on West
                            // if value > 0 but < 180 then must be on west side
                            // if value > -180 but < 0 then must be on east side
                            MoonLONMap = (MoonLONMap > 180) ? MoonLONMap - 360 : (MoonLONMap < -180) ? MoonLONMap + 360 : MoonLONMap;

                            MoonLONMap = -MoonLONMap;

                            Debug.WriteLine("Moonlonmap0 " + MoonLONMap);

                            if (MoonLONMap < -180) MoonLONMap = MoonLONMap + 360;
                            else if (MoonLONMap > 180) MoonLONMap = MoonLONMap - 360;

                            Debug.WriteLine("Moonlonmap1 " + MoonLONMap);

                            double MoonLATMap = dec; // latitude

                            //---------------------------------------------------------------------------------------------

                            Debug.WriteLine("Moon lon map " + MoonLONMap);
                            Debug.WriteLine("Moon lat map " + MoonLATMap);
                            Debug.WriteLine(" ");

                            //---------------------------------------------------------------------------------------------

                            //  Sun_Top1 = 26;                                     // 45 Y pixel location of top of map
                            //  Sun_Bot1 = 465;                                    // 485 Y pixel locaiton of bottom of map 
                            //  Sun_Left = 57;                                       // Left side at equator used by Grayline routine
                            //  Sun_Right = 939;

                            int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map
                            int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine


                            Moon_Y = (int)(((180 - (MoonLATMap + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                            Moon_X = (int)(((MoonLONMap + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E


                            Image src1 = new Bitmap(moon_image); // load up MOON image ( use PNG to allow transparent background)

                            // need to convert lat and long to x, y points

                            g.DrawImage(src1, Moon_X - 11, Moon_Y - 13, 23, 27); // was -10 , -10      draw Moon 20 x 20 pixel
                            g.DrawString("Beam:" + Moon_AZ.ToString(), font1, grid_text_brush, Moon_X + 15, Moon_Y - 10);
                            g.DrawString("Elev:" + Moon_ALT.ToString(), font1, grid_text_brush, Moon_X + 15, Moon_Y);


                        } // MOON = true






                        //------------------------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------------------------

                        if (ISS == true)
                        {
                            Debug.WriteLine("=====================ISS CALCULATIONS");

                            // this allows you to get the actual zenith location of the ISS directly from the internet
                            //# Get current ISS position 
                            //17      req_iss = requests.get('http://api.open-notify.org/iss-now.json')
                            //18      dict = req_iss.json()
                            //19      latlong = dict['iss_position'];
                            //                          20      lat1 = latlong['latitude']
                            //21      lon1 = latlong['longitude']
                            //22 	    #Calculate Distance to Home 

                            // ISS(ZARYA)
                            //1 25544U 98067A   14361.11356206  .00017231  00000 - 0  28015 - 3 0  6359
                            //2 25544  51.6460 222.7302 0007398 178.4506 330.5659 15.52766391921266

                            double ISSSLONMap = 0;
                            double ISSSLATMap = 0;
                            double ISSALTMap = 0;
                            double ISSVELMap = 0;
                            string ISSVISMap = "";
                            double ISSFOOTMap = 0;

                            // http://wheretheiss.at/w/developer
                            // https://api.wheretheiss.at/v1/satellites/25544
                            // https://api.wheretheiss.at/v1/satellites/25544/positions?timestamps=1436029902
                            // https://api.wheretheiss.at/v1/satellites/25544/positions?daynum=2457871.1669097


                            //{"name":"iss","id":25544,"latitude":-32.739945364219,"longitude":-107.25136003491,"altitude":417.65983955229,"velocity":27571.144857654,"visibility":"eclipsed","footprint":4495.5385611307,"timestamp":1493174688,"daynum":2457869.6144444,"solar_lat":13.543779931213,"solar_lon":138.26084380559,"units":"kilometers"}
                            // "map_url": "https://maps.google.com/maps?q=37.795517,-122.393693&z=4"



                            try
                            {

                                WebClient client = new WebClient();
                                Stream stream = client.OpenRead("https://api.wheretheiss.at/v1/satellites/25544");
                                StreamReader reader = new StreamReader(stream);
                                String content = reader.ReadToEnd();
                                reader.Close();


                                Debug.WriteLine("internet data stream: " + content);


                                if (content.Contains("latitude") == true)
                                {

                                    int ind = content.IndexOf("latitude") + 10;
                                    string lat = content.Substring(ind, 9);
                                    Debug.WriteLine("Found latitude= " + lat);
                                    ISSLATMap = Convert.ToDouble(lat);


                                    if (content.Contains("longitude") == true)
                                    {

                                        int ind1 = content.IndexOf("longitude") + 11;
                                        string lon = content.Substring(ind1, 9);
                                        Debug.WriteLine("found longitude= " + lon);
                                        ISSLONMap = Convert.ToDouble(lon);


                                        if (content.Contains("altitude") == true)
                                        {

                                            int ind2 = content.IndexOf("altitude") + 10;
                                            string alt1 = content.Substring(ind2, 9);
                                            Debug.WriteLine("found  altitude= " + alt1);
                                            ISSALTMap = Convert.ToDouble(alt1) * 1000;


                                            if (content.Contains("velocity") == true) // in KM
                                            {

                                                int ind3 = content.IndexOf("velocity") + 10;
                                                string vel = content.Substring(ind3, 9);
                                                Debug.WriteLine("found velocity= " + vel);
                                                ISSVELMap = Convert.ToDouble(vel);


                                                if (content.Contains("visibility") == true)
                                                {
                                                    int ind4 = content.IndexOf("visibility") + 13;
                                                    string vis = content.Substring(ind4, 9);
                                                    Debug.WriteLine("found visibility= " + vis);
                                                    ISSVISMap = vel;

                                                    if (content.Contains("footprint") == true)
                                                    {
                                                        int ind5 = content.IndexOf("footprint") + 11;
                                                        string foot = content.Substring(ind5, 9);
                                                        Debug.WriteLine("found footprint= " + foot);
                                                        ISSFOOTMap = Convert.ToDouble(foot);

                                                        if (content.Contains("solar_lat") == true)
                                                        {
                                                            int ind6 = content.IndexOf("solar_lat") + 11;
                                                            string slat = content.Substring(ind6, 9);
                                                            Debug.WriteLine("found solar_lat= " + slat);
                                                            ISSSLATMap = Convert.ToDouble(slat);

                                                            if (content.Contains("solar_lon") == true)
                                                            {
                                                                int ind7 = content.IndexOf("solar_lon") + 11;
                                                                string slon = content.Substring(ind7, 9);
                                                                Debug.WriteLine("found solar_lon= " + slon);
                                                                ISSSLONMap = Convert.ToDouble(slon);

                                                            } // solar_lon

                                                        } // solar_lat

                                                    } // footprint

                                                } // visibility

                                            } // velocity

                                        } // altitude

                                    } // longitude

                                } // longitude



                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Could not read internet ISS data: " + ex);

                                textBox1.Text = "Could not Get Internet ISS data. Turning off ISS display";

                                chkISS.Checked = false; // turn off ISS internet lat and long since its not reading correctly
                                ISSLONMap = 0;
                                ISSLATMap = 0;
                            }


                            //-------------------------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------------------------


                            double y = DateTime.UtcNow.Year; //  year 2017
                            double m = DateTime.UtcNow.Month; // month = 1 to 12
                            double D1 = DateTime.UtcNow.Day; // day of month 1 to 31
                            double H = DateTime.UtcNow.Hour; // hour 0 to 24

                            double JD = DateTime.UtcNow.ToOADate() + 2415018.5; // convert OLE AUtomation Date to Julian Date

                            double d = JD - 2451543.5; // convert Julian Date to something the orbital routine can use. (the Day number)


                            Debug.WriteLine("Jday " + JD);

                            Debug.WriteLine("day " + d);


                            //--------------------------------------------------------------------------------------------
                            // ISS oribital elements data set
                            //--------------------------------------------------------------------------------------------

                            const double EarthRadEq = 6378.1370; // Declare Earth Equatorial Radius Measurements in km

                            // data set for ISS in DEG (must be converted to Radians before a SIN or COS or TAN is used)
                            // https://spaceflight.nasa.gov/realdata/elements/

                            //    Satellite: ISS
                            //  Catalog Number: 25544
                            //    Epoch time:      17111.63786841 = yrday.fracday
                            //Element set:     900
                            //Inclination: 51.6386  deg
                            //RA of node:       320.1717  deg
                            //Eccentricity:     .0006757
                            //  Arg of perigee: 78.1062  deg
                            //Mean anomaly: 282.0848  deg
                            //Mean motion: 15.54123175  rev / day
                            //  Decay rate:    1.67170E-04  rev / day ^ 2
                            // Epoch rev:            1295
                            //Checksum: 309
                            // double N = (125.1228 - 0.0529538083 * d);  // for MOON

                            double bc = 0.00016717;     // Ballistic Coefficient derivative of mean motion (daily rate of change in the numbe of revs / day)
                            double BSTAR = 0.00010270;   // drag Term 
                            double mm = 15.54189435;    // how many orbits are completed each day

                            double N = (295.8085);     // DEG RA of the long ascending node defines the ascending and descending orbit locations with respect to the earths equatorial plane
                            double i = 51.6369;         // DEG Inclination to the ecliptic deg  defines the orientation of the orbit with respect to the earths equator <
                            double w = (94.0665);     // DEG Arg of Perigee (Perihelion) defines the low point,perigee of the orbit is with respect to the earths surface.
                            double a = 1.06;            //  6777.943 kmeters earth is 6371km radius   Mean distance (Earth equitorial radii) Semi-major axis defines size of the orbit
                            double e = 0.0006891;       //     Eccentricity defines the shape of the orbit
                            double M = (266.1275);     // DEG v = true, mean anomaly defines where the satellite is within the orbit with respect to perigee (low point)

                            double ecl = 23.43704;      // DEG Angle of the obliquity of the ecliptic plane (this is static)


                            //--------------------------------------------------------------------------------------------------
                            // vernal (spring) equinox position = 0deg RA and counts eastward 0-360 as 24hours (15deg per hour)


                            double MJD = JD - 2400000.5;
                            double T = (MJD - 51544.5) / 36525.0;

                            Debug.WriteLine("Current T " + T);

                            double GMST = ((280.46061837 + 360.98564736629 * (MJD - 51544.5)) + 0.000387933 * T * T - T * T * T / 38710000.0) % 360.0;
                            if (GMST < 0)
                            {
                                GMST += 360.0;
                            }

                            Debug.WriteLine("Current GMST in deg " + GMST);

                            double ST = GMST / 15; // current Sidereal time in hours

                            Debug.WriteLine("Current GMST (hours) " + ST);

                            Debug.WriteLine(" ");



                            //---------------------------------------------------------------------------------------------

                            ISS_LAT = ISSLATMap; // values from internet site
                            ISS_LON = ISSLONMap;

                            Debug.WriteLine("ISS lon map " + ISSLONMap);
                            Debug.WriteLine("ISS lat map " + ISSLATMap);
                            Debug.WriteLine("ISS ALT map " + ISSALTMap);
                            Debug.WriteLine("MY lon map " + (double)udDisplayLong.Value);
                            Debug.WriteLine("MY lat map " + (double)udDisplayLat.Value);
                            Debug.WriteLine(" ");


                            Calculate(ISSLATMap, ISSLONMap, ISSALTMap, (double)udDisplayLat.Value, (double)udDisplayLong.Value, 30); // get the ISS_AZ and ISS_ALT values based on 

                            Debug.WriteLine("ISS AZ map " + ISS_AZ);
                            Debug.WriteLine("ISS ELV map " + ISS_ALT);

                            //---------------------------------------------------------------------------------------------


                            int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map
                            int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine


                            ISS_Y = (int)(((180 - (ISSLATMap + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                            ISS_X = (int)(((ISSLONMap + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E


                            Image src1 = new Bitmap(iss_image); // load up ISS image ( use PNG to allow transparent background)

                            // need to convert lat and long to x, y points

                            g.DrawImage(src1, ISS_X - 25, ISS_Y - 16, 50, 33); // was -10 , -10      draw ISS 20 x 20 pixel

                            g.DrawString("Beam:" + ISS_AZ.ToString(), font1, grid_text_brush, ISS_X + 8, ISS_Y + 10);
                            g.DrawString("Elev:" + ISS_ALT.ToString(), font1, grid_text_brush, ISS_X + 8, ISS_Y + 20);

                            // int area = Math.PI *( R * R);  111.36 km per deg of land 

                            // int Radius = (int)Math.Sqrt(ISSFOOTMap/Math.PI); // radiius = sqrt(4495km / 3.14) = 37.8 km radius
                            // around 40075 km from 0 to 360 deg around the earth
                            // X Radius = (int)((double)Radius * .4); // 882 pixels represent 360 deg or 2.45 pixels per deg or 2.45pixels per 111.32km or 45.4km per pixel
                            // Y 439 pixel for 180 deg or 2.45 pixel per deg per 11.32km or 91km per pixel 
                            int Radius = (int)((double)ISSFOOTMap / 2);

                            int Radius_X = (int)((double)Radius / 45.4);


                            g.DrawEllipse(OrgPen, ISS_X - Radius_X, ISS_Y - Radius_X, Radius_X * 2, Radius_X * 2);  // draw circle showing coverage area for ISS

                            if (ISSUPDATE == true) // update only 1 time per minute
                            {

                                ISSUPDATE = false;


                                try
                                {

                                    WebClient client = new WebClient();

                                    Debug.WriteLine("SEND timestamp1= " + ISSTime);

                                    stream = client.OpenRead("https://api.wheretheiss.at/v1/satellites/25544/positions?timestamps=" + ISSTime); //  "timestamp":1493561091

                                    reader = new StreamReader(stream);
                                    content1 = reader.ReadToEnd();
                                    reader.Close();

                                    Debug.WriteLine("internet data stream: " + content1);

                                    int ind = content1.IndexOf("latitude") + 10;
                                    string lat = content1.Substring(ind, 9);
                                    Debug.WriteLine("GOT latitude= " + lat);
                                    ISSLATMap = Convert.ToDouble(lat);

                                    int ind1 = content1.IndexOf("longitude") + 11;
                                    string lon = content1.Substring(ind1, 9);
                                    Debug.WriteLine("GOT longitude= " + lon);
                                    ISSLONMap = Convert.ToDouble(lon);

                                    ISS_YY[ISSCount] = (int)(((180 - (ISSLATMap + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                                    ISS_XX[ISSCount] = (int)(((ISSLONMap + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E

                                    ISSTime = ISSTime + ISSTimeStampStep;

                                    for (int ii = 0; ii < ISSCount; ii++) // draw arc
                                    {
                                        ISS_XX[ii] = ISS_XX[ii + 1]; // shift old data down to put new data at top
                                        ISS_YY[ii] = ISS_YY[ii + 1];

                                        g.DrawLine(OrgPen, ISS_XX[ii], ISS_YY[ii], ISS_XX[ii] + 1, ISS_YY[ii] + 1);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Could not read internet ISS data: " + ex);


                                }
                            }
                            else
                            {
                                for (int ii = 0; ii < ISSCount; ii++) // draw arc  just refresh screen
                                {

                                    g.DrawLine(OrgPen, ISS_XX[ii], ISS_YY[ii], ISS_XX[ii] + 1, ISS_YY[ii] + 1);
                                }

                            }

                        } // ISS == true



                        //---------------------------------------------------------------------------
                        //---------------------------------------------------------------------------
                        //---------------------------------------------------------------------------
                        //---------------------------------------------------------------------------
                        //---------------------------------------------------------------------------
                        // VOACAP Propagation Plot

                        if (checkBoxMUF.Checked == true)
                        {

                            VOA_Color[0] = 70;

                            Pen p5 = new Pen(Color.FromArgb(70, Color.Yellow), 1.0f); // dusk
                            Font font7 = new Font("Ariel", 10.5f, FontStyle.Regular, GraphicsUnit.Pixel);  // ke9ns 

                            //      g.DrawString("SDBW: " + VOA_S[y].ToString("D"), font7, grid_text_brush,VOA_X[x] , VOA_Y[y]);

                            Image src1 = new Bitmap(star_image); // load up SUN image ( use PNG to allow transparent background)

                            g.DrawImage(src1, VOA_MyX - 6, VOA_MyY - 6, 12, 12); // draw star  of your station transmitter location based on input lat and long

                            g.DrawString("VOACAP Propagation map", font7, grid_text_brush, Sun_Left, Sun_Top1);


                            Debug.WriteLine("SSS");


                            if (chkBoxContour.Checked == false)
                            {
                                for (int z = 1; z <= 9; z++)  // go through each S unit S1 through S9
                                {
                                    int q = 0;

                                    while (VOA_Y[z, q] != 0)
                                    {
                                       
                                        if (((z == 1) || (z == 2)) && (CR < 70)) g.FillEllipse(graybrush, VOA_X[z, q] - 1, VOA_Y[z, q] - 1, 2, 2);
                                        else if ((z == 3) || (z == 4)) g.FillEllipse(orangebrush, VOA_X[z, q] - 1, VOA_Y[z, q] - 1, 3, 3);
                                        else if ((z == 5) || (z == 6)) g.FillEllipse(yellowbrush, VOA_X[z, q] - 2, VOA_Y[z, q] - 2, 4, 4);
                                        else if ((z == 7) || (z == 8)) g.FillEllipse(greenbrush, VOA_X[z, q] - 3, VOA_Y[z, q] - 3, 5, 5);
                                        else if (z == 9) g.FillEllipse(bluebrush, VOA_X[z, q] - 3, VOA_Y[z, q] - 3, 5, 5);

                                        q++;

                                    }


                                } // for z (S readings S1 to S9)

                            }
                            else   // Draw only contours of signal strength on the map (insted of the dots)
                            {
                                try
                                {

                                 
                                    for (int a = 1200; a < cnt; a++) // the first 900 lines 3D contours, so skip them  a= 900 is for 10 S countours
                                    {

                                        // large signal reduction if above 80m and K=4  (because 160m is not effected at all by F2, and 80m very little)
                                        if (((Console.Kindex > 4) || (Console.RadioBlackout.Contains("R") == true) || (Console.GeoBlackout.Contains("G") == true)) && (console.last_MHZ > 4) )
                                        {

                                            if (S[a] == 12) // like an S9
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 11) // S8
                                            {
                                                g.DrawLine(GrnPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 10) // S7
                                            {
                                                g.DrawLine(GrnPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 9) // normally -103 dBw but if K=4 or greater then its really an S6
                                            {
                                                g.DrawLine(YelPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 8) // S5
                                            {
                                                g.DrawLine(YelPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 7)  // S4
                                            {
                                                g.DrawLine(OrgPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 6)  // S3
                                            {
                                                g.DrawLine(OrgPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if ((S[a] == 5)) // S2
                                            {
                                                g.DrawLine(GryPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if ((S[a] == 4) && (CR < 70))  // S1        (only show this if in CW mode)
                                            {
                                                g.DrawLine(GryPen, x3[a], y3[a], x4[a], y4[a]);
                                            }


                                        }

                                        // medium signal reduction if K=4 and on 80m  OR K=3 above 80m
                                        else if (
                                           ( ((Console.Kindex > 4) || (Console.RadioBlackout.Contains("R") == true) || (Console.GeoBlackout.Contains("G") == true)) && (console.last_MHZ > 2) ) ||

                                            ((Console.Kindex > 3) && (console.last_MHZ > 7))
                                            
                                            )
                                        {

                                            if (S[a] == 12) // S10
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 11) // S9
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 10)  // S8
                                            {
                                                g.DrawLine(GrnPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 9) // normally -103 dBw but if K=4 or greater then its really an S7
                                            {
                                                g.DrawLine(GrnPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 8) // S6
                                            {
                                                g.DrawLine(YelPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 7)  // S5
                                            {
                                                g.DrawLine(YelPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 6)  // S4
                                            {
                                                g.DrawLine(OrgPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 5)  // S3
                                            {
                                                g.DrawLine(OrgPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 4)  // S2
                                            {
                                                g.DrawLine(GryPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if ((S[a] == 3) && (CR < 70)) // S1  (only show this if in CW mode)
                                            {
                                                g.DrawLine(GryPen, x3[a], y3[a], x4[a], y4[a]);
                                            }

                                           
                                        }
                                        else // normal conditions K = 0,1,2
                                        {
                                            if (S[a] == 12)
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 11)
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 10)
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 9) // normally -103 dBw but if K=4 or greater then its really an S7
                                            {
                                                g.DrawLine(BluPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 8)
                                            {
                                                g.DrawLine(GrnPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 7)
                                            {
                                                g.DrawLine(GrnPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 6)
                                            {
                                                g.DrawLine(YelPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 5)
                                            {
                                                g.DrawLine(YelPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if (S[a] == 4)
                                            {
                                                g.DrawLine(OrgPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if ((S[a] == 3))
                                            {
                                                g.DrawLine(OrgPen, x3[a], y3[a], x4[a], y4[a]);
                                            }

                                            else if ((S[a] == 2) && (CR < 70))  //  (only show this if in CW mode)
                                            {
                                                g.DrawLine(GryPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                            else if ((S[a] == 1) && (CR < 70)) //  (only show this if in CW mode)
                                            {
                                                g.DrawLine(GryPen, x3[a], y3[a], x4[a], y4[a]);
                                            }
                                        }

                                    } // for a loop

                                } 
                                catch(Exception)
                                {
                                    textBox1.Text = "problem with Contour map";

                                }

                            } // end of else (draw contour insted of dots)




                        } // MUF PLOT (actually signal strength)



                        if (SP8_Active == 1) // parse map display just by band, but red dots are for all bands
                        {
                            //-------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------
                            // draw country or call sign on map

                      
                            int Flag11 = 0;

                            int kk = 0;
                            int rr = 0;

                            int zz = 0;

                            //  Debug.WriteLine("Band " + console.RX1Band);
                            //  Debug.WriteLine("BandLOW " + VFOLOW);
                            //  Debug.WriteLine("BandHIGH " + VFOHIGH);

                            //   Debug.WriteLine(">>>>>>>>BEACON: check red dot");

                          //  Array.Clear(yy,0,200);

                            if(beacon1 == true) // show time slot for beacons
                            {
                     
                                for (int x = 0; x < 18; x++)
                                {
                                    g.DrawString(Beacon_Call[x].ToString() + " " + Beacon_Country[x].ToString(), font2, grid_text_brush, 55, 20+(x*10)); // use Pandapdater holder[] data
                                }
                                for (int x = 0; x < 5; x++)
                                {
                                    int y = BX_Index[x] / 5;
                                    int y1 = BX_Index[x] % 5; // get remainder

                                    g.DrawString((Beacon_Freq[y1]/1e6).ToString("f2"), font2, grid_text_brush, 27, 20 + (y * 10)); // use Pandapdater holder[] data
                                }
      
                            } // if beacon scan turned on show list of staations and which ones are on right now

                            if (beacon1 == false)
                            {
                                for (int ii = 0; ii < DX_Index; ii++) // red dot always all bands
                                {
     
                                    if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))
                                    {
       
                                        g.FillRectangle(redbrush, DX_X[ii], DX_Y[ii], 3, 3);  // place red dot on map (all bands)
                                       

                                        if ((chkMapBand.Checked == true)) // map just the band, 
                                        {

                                            if ((DX_Freq[ii] >= VFOLOW) && (DX_Freq[ii] <= VFOHIGH))  //find band your on and its low and upper limits
                                            {
                                                spots[zz++] = ii;                    // ii is the actual DX_INdex pos the the KK holds
                                                                                     // in the display routine this is Display.holder[kk] = ii
                                            }

                                        } // map band only


                                    } // have a lat/long for the spot

                                } // for ii DX_Index  (full dx spot list)
                            }
                            else // beacon below
                            {

                                for (int ii = 0; ii < BX1_Index; ii++) // red dot always all bands
                                {

                                    int dBaboveNoiseFloor = 0;

                                    if ((beacon1 == true))
                                    {
                                        dBaboveNoiseFloor = BX_dBm[ii] - BX_dBm1[ii];
                                    }


                                    if ((BX_X[ii] != 0) && (BX_Y[ii] != 0))
                                    {

                                        if ((beacon1 == true) && (BX_dBm[ii] == -150)) // not checked yet (gray
                                        {
                                            g.FillRectangle(graybrush, BX_X[ii], BX_Y[ii], 3, 3);  // place blue dot on map (all bands)

                                        }
                                        else if ((beacon1 == false)) // Standard DX Spotter RED DOT
                                        {
                                            g.FillRectangle(redbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place red dot on map (all bands)
                                        }
                                        else if ((dBaboveNoiseFloor > Grn_dBm) && (BX_dBm3[ii] > Grn_S)) // strong green  BX_dBm2
                                        {
                                            g.FillRectangle(greenbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place green dot on map (all bands)

                                        }
                                        else if ((dBaboveNoiseFloor > Yel_dBm) && (BX_dBm3[ii] > Yel_S)) // med yellow
                                        {
                                            g.FillRectangle(orangebrush, BX_X[ii], BX_Y[ii], 3, 3);  // place green dot on map (all bands)

                                        }
                                        else if ((dBaboveNoiseFloor >= Org_dBm) && (BX_dBm3[ii] > Org_S)) // week orange
                                        {
                                            g.FillRectangle(yellowbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place yellow dot on map (all bands)

                                        }
                                        else      //if ((beacon1 == false) || ((BX_dBm[ii] - BX_dBm1[ii]) < 10)) // cannot hear signal RED if rx dbm is less then 10db above noise floor
                                        {
                                            g.FillRectangle(redbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place red dot on map (all bands)
                                        }

                                        if ((chkMapBand.Checked == true)) // map just the band, 
                                        {

                                            if ((BX_Freq[ii] >= VFOLOW) && (BX_Freq[ii] <= VFOHIGH))  //find band your on and its low and upper limits
                                            {
                                                spots[zz++] = ii;                    // ii is the actual BX_INdex pos the the KK holds
                                                                                     // in the display routine this is Display.holder[kk] = ii
                                            }

                                        } // map band only


                                    } // have a lat/long for the spot

                                } // for ii BX1_Index  (full dx spot list)


                            } // beacon here above




                            if ((chkBoxPan.Checked == true) && (beacon1 == false)) // show spots on map for just your panadapter
                            {
                                for (int qq = 0; qq < 100; qq++)
                                {
                                    yy[qq] = 0;
                                }

                                for (int ii = 0; ii < Console.DXK; ii++) // dx call sign or country name on map is just for the band your on
                                {

                                    if ((DX_X[Display.holder[ii]] != 0) && (DX_Y[Display.holder[ii]] != 0))  // dont even bother with the spot if X and Y = 0 since that means no data to plot
                                    {

                                        if (chkMapCountry.Checked == true) // spot country on map
                                        {


                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_country[Display.holder[ii]] + "(" + DX_Beam[Display.holder[ii]] + "°)", font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]]); // use Pandapdater holder[] data
                                            else g.DrawString(DX_country[Display.holder[ii]], font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]]); // use Pandapdater holder[] data


                                        } // chkMapCountry true = draw country name on map

                                        else if (chkMapCall.Checked == true)  // else show call signs on map
                                        {

                                            for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current DX_index list
                                            {
                                                if (country[rr] == DX_country[Display.holder[ii]])  // use Pandapdater holder[] data
                                                {
                                                    yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                    Flag11 = 1;
                                                    break;
                                                }


                                            } // for rr loop


                                            if (Flag11 == 0)
                                            {
                                                country[kk] = DX_country[Display.holder[ii]]; // add to list
                                                yy[kk] = 0;
                                            }

                                            kk++; // increment for next country

                                            Flag11 = 0; // reset flag

                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_Station[Display.holder[ii]] + "(" + DX_Beam[Display.holder[ii]] + "°)", font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]] + yy[rr]); // Station  name
                                            else g.DrawString(DX_Station[Display.holder[ii]], font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]] + yy[rr]); // Station  name

                                          

                                        } // chkMapCall true = draw all sign on map


                                    } //  if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))


                                } // for ii index loop

                            } // chkboxpan

                            else if ((chkMapBand.Checked == true) && (beacon1 == false)) //  show spots on map for your entire band
                            {

                                for (int qq = 0;qq < 100;qq++)
                                {
                                    yy[qq] = 0;
                                }

                                for (int ii = 0; ii < zz; ii++) // dx call sign or country name on map is just for the band your on
                                {

                                    if ((DX_X[spots[ii]] != 0) && (DX_Y[spots[ii]] != 0))  // dont even bother with the spot if X and Y = 0 since that means no data to plot
                                    {

                                        if (chkMapCountry.Checked == true) // spot country on map
                                        {
                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_country[spots[ii]] + "(" + DX_Beam[spots[ii]] + "°)", font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]]); // use Pandapdater holder[] data
                                            else g.DrawString(DX_country[spots[ii]], font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]]); // use Pandapdater holder[] data

                                        } // chkMapCountry true = draw country name on map

                                        else if (chkMapCall.Checked == true)  // else show call signs on map
                                        {

                                            for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current DX_index list
                                            {
                                                if (country[rr] == DX_country[spots[ii]])  // use Pandapdater holder[] data
                                                {
                                                    yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                    Flag11 = 1;
                                                    break;
                                                }


                                            } // for rr loop


                                            if (Flag11 == 0)
                                            {
                                                country[kk] = DX_country[spots[ii]]; // add to list
                                                yy[kk] = 0;
                                            }

                                            kk++; // increment for next country

                                            Flag11 = 0; // reset flag

                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_Station[spots[ii]] + "(" + DX_Beam[spots[ii]] + "°)", font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]] + yy[rr]); // Station  name
                                            else g.DrawString(DX_Station[spots[ii]], font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]] + yy[rr]); // Station  name

                                           
                                        } // chkMapCall true = draw all sign on map


                                    } //  if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))


                                } // for ii index loop
                            } // chkMapBand true = just show spots on map for the band you can see

                            //---------------------------------------------------
                            // display data on red dots for all HF below  and for Beacon Scanning
                            else
                            {


                             if (beacon1 == false)
                             {
                                for (int qq = 0; qq < 100; qq++)
                                {
                                    yy[qq] = 0;
                                }

                                for (int ii = 0; ii < DX_Index; ii++) // dx call sign or country name on map is for all HF
                                {

                              
                                    if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))
                                    {

                                        if (chkMapCountry.Checked == true) // spot country on map
                                        {

                                            
                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_country[ii] + "(" + DX_Beam[ii] + "°)", font2, grid_text_brush, DX_X[ii], DX_Y[ii]); // country name
                                            else g.DrawString(DX_country[ii], font2, grid_text_brush, DX_X[ii], DX_Y[ii]); // country name
                                   

                                        } // chkMapCountry true = draw country name on map

                                        else if (chkMapCall.Checked == true)  // show call signs on map
                                        {

                                            for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current DX_index list
                                            {
                                                if (country[rr] == DX_country[ii])
                                                {
                                                    yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward

                                                     //   Debug.WriteLine("rr " + rr + ", yy " + yy[rr]);

                                                    Flag11 = 1;
                                                    break;
                                                }


                                            } // for rr loop


                                            if (Flag11 == 0)
                                            {
                                                country[kk] = DX_country[ii]; // add to list
                                                yy[kk] = 0;
                                            }

                                            kk++; // increment for next country

                                            Flag11 = 0; // reset flag

                                           
                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_Station[ii] + "(" + DX_Beam[ii] + "°)", font2, grid_text_brush, DX_X[ii], DX_Y[ii] + yy[rr]); // Station  name
                                            else g.DrawString(DX_Station[ii], font2, grid_text_brush, DX_X[ii], DX_Y[ii] + yy[rr]); // Station  name

                                  

                                        } // chkMapCall true = draw all sign on map


                                    } //  if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))


                                } // for ii index loop
                             } // beacon1 == false
                                //=========================================================================================

                             else
                             {
                                    for (int ii = 0; ii < BX1_Index; ii++) // dx call sign or country name on map is for all HF
                                    {

                                        int dBaboveNoiseFloor = 0;

                                        if ((beacon1 == true))
                                        {
                                            dBaboveNoiseFloor = BX_dBm[ii] - BX_dBm1[ii];
                                        }

                                        if ((BX_X[ii] != 0) && (BX_Y[ii] != 0))
                                        {

                                            if (chkMapCountry.Checked == true) // spot country on map
                                            {
                                                
                                                    if (
                                                        ((beacon11 > 0) && (ii >= ((BX_Index[beacon11 - 1] / 5) * 5)) && (ii <= ((BX_Index[beacon11 - 1] / 5) * 5) + 4)) ||  // for slow beacon scanning

                                                        ((beacon11 == 0) &&
                                                        (ii >= ((BX_Index[0] / 5) * 5)) && (ii <= ((BX_Index[0] / 5) * 5) + 4) ||        // for fast beacon scanning
                                                        (ii >= ((BX_Index[1] / 5) * 5)) && (ii <= ((BX_Index[1] / 5) * 5) + 4) ||
                                                        (ii >= ((BX_Index[2] / 5) * 5)) && (ii <= ((BX_Index[2] / 5) * 5) + 4) ||
                                                        (ii >= ((BX_Index[3] / 5) * 5)) && (ii <= ((BX_Index[3] / 5) * 5) + 4) ||
                                                        (ii >= ((BX_Index[4] / 5) * 5)) && (ii <= ((BX_Index[4] / 5) * 5) + 4))
                                                        )

                                                    {
                                                        if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, Beacon_brush, BX_X[ii], BX_Y[ii]); // country name (violet)
                                                        else g.DrawString(BX_country[ii], font2, Beacon_brush, BX_X[ii], BX_Y[ii]); // country name

                                                    }
                                                    else
                                                    {


                                                        if (BX_TSlot[ii] == 0)        // ((BX_dBm[ii] == -150)) // not checked yet gray
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, graybrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, graybrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Grn_dBm) && (BX_dBm3[ii] > Grn_S)) // strong green
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, greenbrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, greenbrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Yel_dBm) && (BX_dBm3[ii] > Yel_S)) // med yellow
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, orangebrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, orangebrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else if ((dBaboveNoiseFloor >= Org_dBm) && (BX_dBm3[ii] > Org_S)) // week orange
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, grid_text_brush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, grid_text_brush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else            // if ((dBaboveNoiseFloor < 10)) // cannot hear signal red
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, redbrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, redbrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                    }

                                               
                                               


                                            } // chkMapCountry true = draw country name on map

                                            else if (chkMapCall.Checked == true)  // show call signs on map
                                            {

                                                for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current BX1_index list
                                                {
                                                    if (country[rr] == BX_country[ii])
                                                    {
                                                        yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                        Flag11 = 1;
                                                        break;
                                                    }


                                                } // for rr loop


                                                if (Flag11 == 0)
                                                {
                                                    country[kk] = BX_country[ii]; // add to list
                                                    yy[kk] = 0;
                                                }

                                                kk++; // increment for next country

                                                Flag11 = 0; // reset flag

                                               

                                                    // violet when its scanning this spot
                                                    if (
                                                          ((beacon11 > 0) && (BX_Index[beacon11 - 1] == ii)) ||    // for slow beacon scanning

                                                          (beacon11 == 0) && ((BX_Index[0] == ii) || (BX_Index[1] == ii) || (BX_Index[2] == ii) || (BX_Index[3] == ii) || (BX_Index[4] == ii))  // for fast beacon scanning
                                                        )
                                                    {
                                                        if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, Beacon_brush, BX_X[ii], BX_Y[ii] + yy[rr]); // VIOLET  Station name
                                                        else g.DrawString(BX_Station[ii], font2, Beacon_brush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                    }
                                                    else
                                                    {

                                                        if (BX_TSlot[ii] == 0)        // ((BX_dBm[ii] == -150)) // not checked yet GRAY
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, graybrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, graybrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Grn_dBm) && (BX_dBm3[ii] > Grn_S)) // strong GREEN
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, greenbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, greenbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Yel_dBm) && (BX_dBm3[ii] > Yel_S)) //med Yel
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, orangebrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, orangebrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else if ((dBaboveNoiseFloor >= Org_dBm) && (BX_dBm3[ii] > Org_S)) // week Org
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, yellowbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, yellowbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else           //if ((dBaboveNoiseFloor < 10)) // cannot hear signal RED
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, redbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, redbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                    }
                                               
                                               

                                            } // chkMapCall true = draw all sign on map


                                        } //  if ((BX_X[ii] != 0) && (BX_Y[ii] != 0))


                                    } // for ii index loop

                                } // beacon here above



                            } // chkMapBand false = show all spots on map

                        } // SP8_Active = 1


                        //----------------------------------------------------------------------------------------------------
                        // update MAP background

                        console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;           // put image back onto picDisplay background image
                        console.picDisplay.BackgroundImage = MAP;                                  // MAP.Save("test.bmp");  save modified map_image to actual file on hard drive


                        beacon4 = false; // reset the beacon scanner flag (you just updated the map)


                    } // check every 1 minutes or unless spots change

                } // only check in in panadapter mode since you cant see it in any other mode
                else
                {
                    SUN = false;
                    GRAYLINE = false;
                }

            } // for loop (SP5_Active == 1)

        } // TrackSun


       
   


        //==================================================================================================
        // special panafall mode (80-20)
        public void chkPanMode_CheckedChanged(object sender, EventArgs e)
        {

            if (SP5_Active == 1) // only check if tracking is already on
            {
                if (chkPanMode.Checked == true)
                {

                    Display.map = 1;
                    Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                }
                else if (chkPanMode.Checked == false) // turn off special pan and put back original display mode
                {
                    Display.map = 0;
                    Display.CurrentDisplayMode = LastDisplayMode;
                }
            }
        } // chkPanMode_CheckedChanged



        //=========================================================================================


        private static int DXLOC_Index1 = 0;
        private static int SP8_Active = 0;    // 1=DX LOC scanned into memory

        // data obtained from DXLOC.txt file
        public static string[] DXLOC_prefix = new string[2000];       // prefix (must start with)
        public static string[] DXLOC_prefix1 = new string[2000];      // prefix (must also contain) /
        public static string[] DXLOC_prefix2 = new string[2000];      // prefix (must exclude) \

        public static string[] DXLOC_lat = new string[2000];          // text of lat
        public static string[] DXLOC_lon = new string[2000];          // text of  lon
        public static double[] DXLOC_LAT = new double[2000];          // latitude  
        public static double[] DXLOC_LON = new double[2000];          //  longitude
        public static string[] DXLOC_country = new string[2000];      // country
        public static string[] DXLOC_continent = new string[2000];    // continent

     
        //=======================================================================================
        //=======================================================================================
        //ke9ns Open and read DXLOC.txt file here (put into array of prefix vs lat/lon value)
        //   Fields:
        //
        //0	    DXCC number
        //1*	ARRL DXCC prefix,
        //2*    DXCC Entity name,
        //3*	Continent,
        //4*	Latitude,
        //5*	Longitude,
        //6	    CQ Zone,
        //7	    ITU Zone,
        //8	Active (A) or Deleted (D),
        //9	Date from becoming a valid Entity,
        //10	Possible prefixes from ITU Assigned Blocks to Sovereign UN Territory(s)
        public void DXLOC_FILE()
        {

            string file_name = console.AppDataPath + "DXLOC.txt"; // //  sked - b15.csv

            textBox1.Text += "Attempting to open DX Location list\r\n";

            if (File.Exists(file_name))
            {

                textBox1.Text += "Reading DX Location list\r\n";

                try
                {
                    stream2 = new FileStream(file_name, FileMode.Open); // open  file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);

                }
                catch(Exception)
                {
                    SP8_Active = 0;
                    Debug.WriteLine("NO DX LOC FILE============================");
                    return;


                }
                var result = new StringBuilder();

                if (SP8_Active == 0) // dont reset if already scanned in  database
                {
                    DXLOC_Index1 = 0; // how big is the DXLOC data file in lines

                }

                for (;;) // read file and extract data from it and close it and set sp8_active = 1 when done
                {

                    if (SP8_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    if (SP_Active == 0)
                    {
                        reader2.Close();    // close  file
                        stream2.Close();   // close stream

                        return;
                    }



                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if ((newChar == '\r'))
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            string[] values = result.ToString().Split(','); // split line up into segments divided by ,


                            //   Debug.Write(DXLOC_Index1.ToString());

                            DXLOC_prefix[DXLOC_Index1] = values[1].Substring(1, values[1].Length - 2);                       // call sign prefix
                                                                                                                             //    Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);


                            if (DXLOC_prefix[DXLOC_Index1].Contains("/")) // indicating an extra character the call sign must contain
                            {
                                DXLOC_prefix1[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(DXLOC_prefix[DXLOC_Index1].Length - 1, 1);
                                DXLOC_prefix[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(0, DXLOC_prefix[DXLOC_Index1].Length - 2);

                            }
                            else DXLOC_prefix1[DXLOC_Index1] = null;

                            if (DXLOC_prefix[DXLOC_Index1].Contains("\\")) // indicating an extra character the call sign must not contain
                            {
                                DXLOC_prefix2[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(DXLOC_prefix[DXLOC_Index1].Length - 1, 1);
                                DXLOC_prefix[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(0, DXLOC_prefix[DXLOC_Index1].Length - 2);


                            }
                            else DXLOC_prefix2[DXLOC_Index1] = null;

                            //    Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);

                            //    Debug.Write(" pre/ " + DXLOC_prefix1[DXLOC_Index1]);
                            //   Debug.Write(" pre\\ " + DXLOC_prefix2[DXLOC_Index1]);



                            DXLOC_country[DXLOC_Index1] = values[2].Substring(1, values[2].Length - 2);                       // call sign country
                                                                                                                              // Debug.Write(" country>" + DXLOC_country[DXLOC_Index1]);


                            DXLOC_continent[DXLOC_Index1] = values[3].Substring(1, values[3].Length - 2);                     // call sign continent
                                                                                                                              //  Debug.Write(" continent>" + DXLOC_continent[DXLOC_Index1]);

                            DXLOC_lat[DXLOC_Index1] = values[4];                          // call sign lat
                                                                                          //  Debug.Write(" lat>" + DXLOC_lat[DXLOC_Index1]);

                            DXLOC_lon[DXLOC_Index1] = values[5];                          // call sign lon
                                                                                          //  Debug.Write(" lon>" + DXLOC_lon[DXLOC_Index1]);

                            // horizontal lines top to bottom (North)90 to 0 to (-SOUTH)90
                            // vertical lines left to right  -West(180) to 0 to +East(180)


                            if (DXLOC_lat[DXLOC_Index1].Contains("N")) // pos 90 North
                            {
                                int ff = DXLOC_lat[DXLOC_Index1].IndexOf('N') - 1;

                                try
                                {
                                    DXLOC_LAT[DXLOC_Index1] = Convert.ToDouble(DXLOC_lat[DXLOC_Index1].Substring(1, ff));
                                    //     Debug.Write(" LAT>" + DXLOC_LAT[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {

                                       Debug.WriteLine(" NORTH " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LAT[DXLOC_Index1] = 0;


                                }

                            }  // pos 90 North
                            else if (DXLOC_lat[DXLOC_Index1].Contains("S")) // neg 90 North
                            {

                                int ff = DXLOC_lat[DXLOC_Index1].IndexOf('S') - 1;

                                try
                                {
                                    DXLOC_LAT[DXLOC_Index1] = -Convert.ToDouble(DXLOC_lat[DXLOC_Index1].Substring(1, ff));
                                    //    Debug.Write(" LAT>" + DXLOC_LAT[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                    DXLOC_LAT[DXLOC_Index1] = 0;
                                       Debug.WriteLine(" SOUTH " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                        Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);

                                }

                            }  // neg 90 North


                            if (DXLOC_lon[DXLOC_Index1].Contains("W")) // neg 180 west
                            {
                                int ff = DXLOC_lon[DXLOC_Index1].IndexOf('W') - 1;

                                try
                                {

                                    DXLOC_LON[DXLOC_Index1] = -Convert.ToDouble(DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    //  Debug.WriteLine(" LON>" + DXLOC_LON[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                        Debug.WriteLine(" WEST " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LON[DXLOC_Index1] = 0;
                                }

                            }  // neg 180 West
                            else if (DXLOC_lon[DXLOC_Index1].Contains("E")) // pos 180 East
                            {
                                int ff = DXLOC_lon[DXLOC_Index1].IndexOf('E') - 1;

                                try
                                {

                                    DXLOC_LON[DXLOC_Index1] = Convert.ToDouble(DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    //  Debug.WriteLine(" LON>" + DXLOC_LON[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                       Debug.WriteLine(" EAST " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LON[DXLOC_Index1] = 0;
                                }

                            }  // pos 180 East



                            DXLOC_Index1++;

                            if (DXLOC_Index1 > 3000) break;

                            result = new StringBuilder(); // clean up for next line

                        } // \r
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        DXLOC_Index1--;

                        if (DXLOC_Index1 < 10) textBox1.Text += "No DXLOC.txt list file found in database folder\r\n";
                        else   textBox1.Text += "End of DX LOC FILE at " + DXLOC_Index1.ToString() + "\r\n";

                        break; // done with file
                    }
                    catch (Exception)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        if (DXLOC_Index1 < 10) textBox1.Text += "No DXLOC.txt list file found in database folder\r\n";

                        break; // done with file
                    }


                } // for loop until end of file is reached


             //   Debug.WriteLine("reached DXLOC end of file" + DXLOC_Index1.ToString());
                textBox1.Text += "Reached End of DXLOC.txt FILE with # " + DXLOC_Index1.ToString() + "\r\n";


                reader2.Close();    // close  file
                stream2.Close();   // close stream


                SP8_Active = 1; // done loading DXLOC database (Good)



            } // if file exists
            else
            {
                SP8_Active = 0;
                Debug.WriteLine("NO DX LOC FILE============================");
            }



        } // DXMAP()

        private void chkMapCountry_CheckedChanged(object sender, EventArgs e)
        {

            if (chkMapCountry.Checked == true) chkMapCall.Checked = false;
            Map_Last = 1;

        }

        private void chkMapCall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMapCall.Checked == true) chkMapCountry.Checked = false;
            Map_Last = 1;

        }

        private void chkMapBand_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMapBand.Checked == true) chkBoxPan.Checked = false;
            Map_Last = 1;

        }

        private void chkBoxPan_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxPan.Checked == true) chkMapBand.Checked = false;
            Map_Last = 1;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {

            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click

        }





        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (console.DXMemList.List.Count == 0) return; // nothing in the list, exit

            int index = dataGridView1.CurrentCell.RowIndex;


            if ((index < 0) || (index > (console.DXMemList.List.Count - 1))) return;// index out of range

            //   DXMemRecord recordToRestore = new DXMemRecord((DXMemRecord)DXURL.SelectedItem);

            Debug.WriteLine("Double CLick=" + index);
        }


        public static int RIndex1 = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RIndex1 = e.RowIndex; // last row you clicked on 

        }

      
      


        public static byte SP6_Active = 0; // 1= turn on MEMORY in panadapter

        private void chkBoxMem_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxMem.Checked == true)
            {
              
                  dataGridView2.DataSource = console.MemoryList.List;   // ke9ns get list of memories from memorylist.cs is where the file is opened and saved

                SP6_Active = 1;

                //  comboFMMemory.DataSource = MemoryList.List;
                //  comboFMMemory.DisplayMember = "Name";
                //  comboFMMemory.ValueMember = "Name";


                //  Debug.WriteLine("comboFM " + (string)dataGridView1["Name", dataGridView1.CurrentCell.RowIndex].Value);

                //  Debug.WriteLine("comboFM " + (string)dataGridView2[2, 0].Value);
                //  Debug.WriteLine("comboFM1 " + dataGridView2[1, 3].ToString());
                  Debug.WriteLine("Rows Count " + dataGridView2.Rows.Count);

            }
            else
            {
                SP6_Active = 0;
            }

        //  MemoryList X = console.MemoryList.List;
         //   MemoryRecord recordToRestore = new MemoryRecord((MemoryRecord)comboFMMemory.SelectedItem);
           
              //  console.RecallMemory(recordToRestore);

        } //chkboxmem_checked



        //=====================================================
        private void SWLbutton2_Click(object sender, EventArgs e)
        {
            console.SWLFORM = true; // open up SWL search window


        }


        //=====================================================
        // Your station Lat and Long used in Beam heading for Spots
        private void udDisplayLat_ValueChanged(object sender, EventArgs e)
        {
            Map_Last = 1;
            if (checkBoxMUF.Checked == true)
            {
                VOACAP_CHECK(); // rescan a new map since your changing your antenna type
            }
        }

        private void udDisplayLong_ValueChanged(object sender, EventArgs e)
        {
            Map_Last = 1;
            if (checkBoxMUF.Checked == true)
            {
                VOACAP_CHECK(); // rescan a new map since your changing your antenna type
            }
        }

        // ke9ns put beam heading on map
        private void chkBoxBeam_CheckedChanged(object sender, EventArgs e)
        {
            Map_Last = 1;

        }

        //========================================================================
        //========================================================================

        public void NOAA()
        {

            RadioBlackout = " ";
            GeoBlackout = " ";

            serverPath = "ftp://ftp.swpc.noaa.gov/pub/latest/wwv.txt";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverPath);

            textBox1.Text += "Attempt to download Space Weather \r\n";

            request.KeepAlive = true;
            request.UsePassive = true;
            request.UseBinary = true;

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            string username = "anonymous";
            string password = "guest";
            request.Credentials = new NetworkCredential(username, password);

            string noaa = null;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                noaa = reader.ReadToEnd();

                reader.Close();
                response.Close();
             //   Debug.WriteLine("noaa=== " + noaa);

             //   textBox1.Text += "NOAA Download complete \r\n";

           

            //--------------------------------------------------------------------
            if (noaa.Contains("Solar flux ")) // 
            {

                int ind = noaa.IndexOf("Solar flux ") + 11;

                try
                {
                    SFI = (int)(Convert.ToDouble(noaa.Substring(ind, 3)));
                    Debug.WriteLine("SFI " + SFI);
                }
                catch (Exception)
                {
                    SFI = 0;
                }


            } // SFI

            if (noaa.Contains("A-index ")) // 
            {

                int ind = noaa.IndexOf("A-index ") + 8;

                try
                {
                    Aindex = (int)(Convert.ToDouble(noaa.Substring(ind, 2)));
                  Debug.WriteLine("Aindex " + Aindex);
                }
                catch (Exception)
                {
                    Aindex = 0;
                }


            } // Aindex

            if (noaa.Contains("Radio blackouts reaching the ")) // 
            {

                int ind = noaa.IndexOf("Radio blackouts reaching the ") + 29;

                try
                {
                    RadioBlackout = noaa.Substring(ind, 2);
                      Debug.WriteLine("Radio Blackout " + RadioBlackout);
                }
                catch (Exception)
                {
                    RadioBlackout = " ";
                }


            } // radio blackouts
           
          
            if (!noaa.Contains("No space weather storms ") && noaa.Contains("Geomagnetic storms reaching the ")) // 
            {

                int ind = noaa.IndexOf("Geomagnetic storms reaching the ") + 32;

                try
                {
                    GeoBlackout = noaa.Substring(ind, 2);
                    Debug.WriteLine("Geomagnetic storms" + GeoBlackout);
                }
                catch (Exception)
                {
                    GeoBlackout = " ";
                }


            } // radio blackouts

                if (RadioBlackout != " ")
                {
                    RadioBlackout = RadioBlackout + GeoBlackout;
                    Debug.WriteLine("radio-geo " + RadioBlackout);


                }
                else
                {
                    RadioBlackout = GeoBlackout;
                    Debug.WriteLine("geo " + RadioBlackout);
                }

            }
            catch (Exception ex)
            {
                //   Debug.WriteLine("noaa fault=== " + ex);
                textBox1.Text += "Failed to download Space Weather \r\n";

            }


    } // NOAA


        //=====================================================
        //ke9ns formulat to calculate beam heading
        // thanks to: http://www.movable-type.co.uk/scripts/latlong.html 
        //Formula: θ = atan2( sin Δλ ⋅ cos φ2 , cos φ1 ⋅ sin φ2 − sin φ1 ⋅ cos φ2 ⋅ cos Δλ ) 
        //  where φ1, λ1 is the start point (lat, long), φ2,λ2 (lat, long) the end point       (Δλ is the difference in longitude)
        //For final bearing, simply take the initial bearing from the end point to the start point and reverse it (using θ = (θ+180) % 360).

        double LatDest = 0; // radians.  but equals -90 to 90 deg
        double LongDest = 0;  // radians. but equals -180 to 180 deg

        double LatStart = 0; // radians.  but equals -90 to 90 deg
        double LongStart = 0;  // radians. but equals -180 to 180 deg

        public int BeamHeading(double SpotLat, double SpotLong)
        {

            LatStart = (Math.PI / 180.0) * (double)udDisplayLat.Value; // convert degree's to radians
            LongStart = (Math.PI / 180.0) * (double)udDisplayLong.Value;

            LatDest = (Math.PI / 180.0) * SpotLat;  // convert degree's to rads
            LongDest = (Math.PI / 180.0) * SpotLong;

            double y = Math.Sin(LongDest - LongStart) * Math.Cos(LatDest);
            double x = Math.Cos(LatStart) * Math.Sin(LatDest) - Math.Sin(LatStart) * Math.Cos(LatDest) * Math.Cos(LongDest - LongStart);

            int Bearing = (int)(Math.Atan2(y, x) * (180.0 / Math.PI));
         //   Debug.WriteLine("Init Bearing=" + Bearing);

            int FBearing = (int)(((Math.Atan2(y, x) * (180.0 / Math.PI)) + 360) % 360.0 );

         //   Debug.WriteLine("Final Bearing=" + FBearing);

            return FBearing;


        } // Beamheading



        //==============================================================================
        //==============================================================================
        //==============================================================================
        // BEACON CHECK ON/OFF
        //==============================================================================
        //==============================================================================
        //==============================================================================
        // PowerSDR will scan through 14.1, 18.11, 21.15, 24.93, 28.2 mhz
        // looking for 18 stations: 4U1UN, VE8AT, W6WX, KH6RS, ZL6B, VK6RBP, 
        // JA1IGY, RR9O, VR2B, 4S7B, ZS6DN, 5Z4B, 4X6TU, OH2B, CS3B, LU4AA,
        // OA4B, and YV5B in 10 second intervals.Thats 5 frequecies and 18 stations
        // rotating in 10 intervals = 10 * 18 = 180second = 3minutes until a repeat.
        // must have 18 stations so 4U1UN repeats on 14100 20 times/ hour exactly
   
        public int Grn_dBm = 35;   // green indicator when this dBm above noise floor
        public int Grn_S = 3;      // green indicator when this S units or above

        public int Yel_dBm = 25;   // Yellow indicator when this dBm above noise floor
        public int Yel_S = 2;      // Yellow indicator when this S units or above

        public int Org_dBm = 15;   // Orange indicator when this dBm above noise floor
        public int Org_S = 1;      // Orange indicator when this S units or above
      
        public int Red_dBm = 5;    // Red indicator when this dBm above noise floor
        public int Red_S = 0;      // Red indicator when this S units or above

        public int[] Beacon_Freq = new int[] { 14100000, 18110000, 21150000, 24930000, 28200000 }; // ke9ns NCDXF/IARU beacon channels 0-4

        public string[] Beacon_Call = new string[]               
          { "4U1UN", "VE8AT", "W6WX", "KH6RS", "ZL6B", "VK6RBP",   //  "4U1UN", "VE8AT", "W6WX", "KH6RS", "ZL6B", "VK6RBP",
           "JA2IGY", "RR9O", "VR2B", "4S7B", "ZS6DN", "5Z4B", //  "JA2IGY", "RR9O", "VR2B", "4S7B", "ZS6DN", "5Z4B",
              "4X6TU", "OH2B", "CS3B", "LU4AA", "0A4B", "YV5B" };    //  "4X6TU", "OH2B", "CS3B", "LU4AA", "0A4B", "YV5B"               // BEACON CALL SIGN

        public string[] Beacon_Country = new string[]
        { "USA1", "CANADA", "USA6", "HAWAII", "NEW ZEALAND", "AUSTRALIA", //  "USA1", "CANADA", "USA6", "HAWAII", "NEW ZEALAND", "Australia",
           "JAPAN", "RUSSIA", "HONG KONG", "SRI LANKA", "S.AFRICA", "KENYA", //  "Japan", "RUSSIA", "HONG KONG", "SRI LANKA", "S.AFRICA", "KENYA",
            "ISRAEL", "FINLAND",  "MADEIRA", "ARGENTINA", "PERU", "VENEZUELA" };   // "ISRAEL", "FINLAND",  "MADEIRA", "ARGENTINA", "PERU", "VENEZUELA" // BEACON COUNTRY

        public string[] Beacon_Grid = new string[] 
        { "FN30", "EQ78", "CM97", "BL10", "RE78", "OF87",
            "PM84", "NO14", "OL72", "MJ96", "KG44", "KI88",
            "KM72", "KP20", "IM12", "GF05", "FH17", "FJ69" };             // BEACON GRID LOCATION

        public double[] Beacon_Lat = new double[] 
        { 40.75, 79.978, 37.145, 20.77, -41.06, -32.105,
            34.436, 54.978, 22.27, 6.895, -25.896, -1.23,
            32.06, 62.989, 32.728, -34.645, -12.063, 9.103 };                       // always 18 stations

        public double[] Beacon_Lon = new double[]
        { -73.96, -85.96, -121.876, -156.376, 175.623, 116.04,
            136.79, 82.873, 114.123, 79.873, 28.29, 36.873,
            34.79, 25.75, -16.793, -58.413, -76.96, -67.793 };

        //S9+10dB 160.0 -63 44 
        //S9 50.2 -73 34 
        //S8 25.1 -79 28 
        //S7 12.6 -85 22 
        //S6 6.3 -91 16 
        //S5 3.2 -97 10 
        //S4 1.6 -103 4 
        //S3 0.8 -109 -2 
        //S2 0.4 -115 -8 
        //S1 0.2 -121 -14 

        private bool beacon = false; // true = pause DX spotting while doing a beacon test (3min max)
        public bool beacon1 = false; // flag storage for beacon loaded up or not true=beacon test now running
        public bool beacon2 = false; // flag storage for sp8_active
        public bool beacon3 = false; // flag storage for was map already on or not
        public bool beacon4 = false; // true = need map update now.
        public int beacon5 = 0;       // 1-5 indicates which fast scan freq your on 1 to 5, 6=done with 1 slot (18 slots total)
        public int beacon6 = 0;       // counter to try and ignore the pulse in the signal that happens when you change bands

        public int beacon11 = 0;       // 1-5 indicates which slow scan freq your on 1 to 5, 6=done with 1 slot (18 slots total)
        public int beacon12 = 0;       // 1-18 indicates which beacon slot (station) your looking at currently
        public int[] beacon13 = new int[100]; // slow beacon scan place holder

        public int beacon14 = 0;     // prior time slot position in slot stack (for updating the spotter information)
        public int beacon15 = 0;     // cause a 1 cycle delay in startup on SLOW scan routine

        public bool beacon16 = false; // true=PTT was diable prior to running a beacon chk, false = PPT was not disabled prior to running beacon chk


        public PreampMode beacon44;  // to store preamp mode before running beacon scan
        public DSPMode beacon7;       // to store prior operating mode before running beacon scan
        public int beacon8 = 0;       // to store prior high filter before running beacon scan
        public int beacon9= 0;        //to store prior low filter before running beacon scan
        public Filter beacon89;       // to store filter name before running beacon scan
        public Filter beacon89a;       // to store filter name before running beacon scan

        public int beacon77 = 0;      // to store cw pitch before running beacon scan
        public double beacon88 = 0;   // to store vfoa
        public int beacon66 = 0;      //  to store blocksize
        public int beacon33 = 0;      // to store rx buffer size
        public PreampMode beacon55;


        public bool beacon10 = false; //true indicates op mode has changed (ie scan was run)

        //=====================================================================
        private void btnBeacon_Click(object sender, EventArgs e)
        {
            if (beacon == true)
            {
                beacon = false;
                btnBeacon.Text = "Beacon Chk";
                btnBeacon.ForeColor = Color.Black;

               
                stopWatch.Stop();
                stopWatch1.Stop();

                Debug.WriteLine(">>>>>>>>BEACON: TURN BACK OFF");


            }
            else // if the beacon chk was OFF,then turn it on
            {
            
                beacon = true;
                btnBeacon.Text = "Beacon Run";
                btnBeacon.ForeColor = Color.Red;

                beacon11 = 0; // reset freq for slow scan since you may have changed the freq
                beacon12 = 0; // reset the station your looking at


                Debug.WriteLine(">>>>>>>>BEACON: START");

                //--------------------------------------
                // turn on mapping if it wasnt on when you click on the beacon button

                if (SP5_Active == 0)  // if OFF then turn ON
                {
                    Debug.WriteLine(">>>>>>>>BEACON: turn on mapping");

                    if (chkPanMode.Checked == true) Display.map = 1;
                    else Display.map = 0;

                //    btnTrack.Text = "Track ON";

                    LastDisplayMode = Display.CurrentDisplayMode; // save the display mode that you were in before you turned on special panafall mode

                    if (chkPanMode.Checked == true) Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                    Display.GridOff = 1; // Force Gridlines off but dont change setupform setting

                    if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                    {

                        if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                        console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                        if (MAP == null)
                        {
                            if (Console.DXR == 0) console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                            else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);
                            //  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        }
                        else console.picDisplay.BackgroundImage = MAP;

                    }
                    
                    if (chkISS.Checked == true)
                    {
                        Thread t1 = new Thread(new ThreadStart(ISSOrbit));                                // turn on track map (sun, grayline, voacap, or beacon)

                        t1.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                        t1.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                        t1.Name = "ISS Orbit Plot Thread";
                        t1.IsBackground = true;
                        t1.Priority = ThreadPriority.BelowNormal;
                        t1.Start();

                    } //  if (chkISS.Checked == true)

                    Thread t = new Thread(new ThreadStart(TrackSun)); // turn on track map (sun, grayline, voacap, or beacon)

                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                    SP5_Active = 1; // turn on track map (sun, grayline, voacap, or beacon)
                    t.Name = "Track Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();


                  

                    textBox1.Text = "Clicked to Turn on GrayLine Sun Tracker\r\n";

                    Debug.WriteLine(">>>>>>>>BEACON:  mapping turned on");

                    btnTrack.Text = "Track-ON";


                }//  if (SP5_Active == 0) map was off (above) so turn it on 
                else
                {
                    beacon3 = true; // map is already on
                }


             
            } //  if (beacon != true) from here and above is all part of the Beacon ON button clicking

            //------------------------------------------------
            //------------------------------------------------
            //------------------------------------------------
            //------------------------------------------------

            if (beacon == true) // you just clicked to turn on beacon scan
            {

                Sun_Top1 = 26;                                     // 45 Y pixel location of top of map
                Sun_Bot1 = 465;                                    // 485 Y pixel locaiton of bottom of map 
                Sun_Left = 57;                                       // Left side at equator used by Grayline routine
                Sun_Right = 939;

                int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map
                int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                Debug.WriteLine(">>>>>>>>BEACON: start loading BX");

                int x1 = 0;

                for (int x = 0; x < 18; x++)
                {

                    if (BX_Load == false)
                    {
                        BX_Time[x * 5 + 4] = BX_Time[x * 5 + 3] = BX_Time[x * 5 + 2] = BX_Time[x * 5 + 1] = BX_Time[x * 5] = UTCNEW;

                        BX_TSlot[x * 5] = 0;        // #=how many times the station was checked  (0=never checked yet, gray) (1=checked color = signal)
                        BX_TSlot[x * 5 + 1] = 0;   // 
                        BX_TSlot[x * 5 + 2] = 0;   // 
                        BX_TSlot[x * 5 + 3] = 0;   // 
                        BX_TSlot[x * 5 + 4] = 0;   // 

                        BX_Freq[x * 5] = Beacon_Freq[0];                          // Beacon freq in hz
                        BX_Freq[x * 5 + 1] = Beacon_Freq[1];                      // Beacon freq in hz
                        BX_Freq[x * 5 + 2] = Beacon_Freq[2];                      // Beacon freq in hz
                        BX_Freq[x * 5 + 3] = Beacon_Freq[3];                      // Beacon freq in hz
                        BX_Freq[x * 5 + 4] = Beacon_Freq[4];                      // Beacon freq in hz

                        BX_Station[x * 5 + 4] = Beacon_Call[x] + " 10m";          // Beacon station name
                        BX_Station[x * 5 + 3] = Beacon_Call[x] + " 12m";          // Beacon station name
                        BX_Station[x * 5 + 2] = Beacon_Call[x] + " 15m";          // Beacon station name
                        BX_Station[x * 5 + 1] = Beacon_Call[x] + " 17m";          // Beacon station name
                        BX_Station[x * 5] = Beacon_Call[x] + " 20m";              // Beacon station name

                        BX_Spotter[x * 5 + 4] = BX_Spotter[x * 5 + 3] = BX_Spotter[x * 5 + 2] = BX_Spotter[x * 5 + 1] = BX_Spotter[x * 5] = callBox.Text;               // PowerSDR callsign station (spotter)
                        BX_Message[x * 5 + 4] = BX_Message[x * 5 + 3] = BX_Message[x * 5 + 2] = BX_Message[x * 5 + 1] = BX_Message[x * 5] = "NCDXF/IARU Beacon";        // message field
                        BX_Mode[x * 5 + 4] = BX_Mode[x * 5 + 3] = BX_Mode[x * 5 + 2] = BX_Mode[x * 5 + 1] = BX_Mode[x * 5] = 1;                                         // operating mode (cw), 
                        BX_Mode2[x * 5 + 4] = BX_Mode2[x * 5 + 3] = BX_Mode2[x * 5 + 2] = BX_Mode2[x * 5 + 1] = BX_Mode2[x * 5] = 0;                                    // operating mode2  (split) no

                        BX_Grid[x * 5 + 4] = BX_Grid[x * 5 + 3] = BX_Grid[x * 5 + 2] = BX_Grid[x * 5 + 1] = BX_Grid[x * 5] = Beacon_Grid[x];                            // Beacon Grid location 
                        BX_country[x * 5 + 4] = BX_country[x * 5 + 3] = BX_country[x * 5 + 2] = BX_country[x * 5 + 1] = BX_country[x * 5] = Beacon_Country[x];          // Beacon Country
                        BX_Beam[x * 5 + 4] = BX_Beam[x * 5 + 3] = BX_Beam[x * 5 + 2] = BX_Beam[x * 5 + 1] = BX_Beam[x * 5] = BeamHeading(Beacon_Lat[x], Beacon_Lon[x]); // Beam heading to Beacon from spotter station

                        BX_Y[x * 5 + 4] = BX_Y[x * 5 + 3] = BX_Y[x * 5 + 2] = BX_Y[x * 5 + 1] = BX_Y[x * 5] = (int)(((180 - (Beacon_Lat[x] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;   //latitude 90N to -90S

                        BX_X[x * 5 + 4] = BX_X[x * 5 + 3] = BX_X[x * 5 + 2] = BX_X[x * 5 + 1] = BX_X[x * 5] = (int)(((Beacon_Lon[x] + 180.0) / 360.0) * Sun_Width) + Sun_Left;         // longitude -180W to +180E

                        //-------------------------------------------
                        // this below does not need to be swapped in/out of DX_

                        BX_dBm[x * 5] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 1] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 2] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 3] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 4] = -150; // signal strength reading for station & freq


                        BX_dBm1[x * 5] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 1] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 2] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 3] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 4] = -150; // signal strength reading for station & freq

                        x1 = x * 10;
                        BX_TSlot1[x * 5] = x1;     // time slot for this station on 14mhz

                        x1 += 10;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 1] = x1;     // time slot for this station on 18mhz

                        x1 += 10
                            ;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 2] = x1;     // time slot for this station on 21mhz

                        x1 += 10;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 3] = x1;     // time slot for this station on 24mhz

                        x1 += 10;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 4] = x1;     // time slot for this station on 28mhz

                        BX_FULLSTRING[x * 5] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[0] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 1] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[1] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 1].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 2] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[2] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 2].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 3] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[3] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 3].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 4] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[4] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 4].ToString("D4") + "z " + Beacon_Grid[x];

                    } // BX_Load = false
           
                } // for loop. load up data on all 18 Beacon stations

                Debug.WriteLine(">>>>>>>>BEACON: loaded up BX");

                BX_Load = true; // flag to not reload again

                if (SP8_Active == 0) // check if dxloc.txt loaded into memory already
                {
                    beacon2 = false;
                    SP8_Active = 1; // fake it for the red dots
                }
                else beacon2 = true; // dxloc was already loaded so SP8_active is 1
                 
                BX1_Index = 90; // this is always 90 unless they change the number of beacons

                beacon1 = true; // flag it so we know we ran a beacon check

                Debug.WriteLine(">>>>>>>>BEACON: ALL LOADED UP");

                //-----------------------------------------------------
                // THREAD START UP FOR CHECKING TIME SLOT

                Thread t = new Thread(new ThreadStart(BeaconSlot)); // show beacons (turn on tracking map)

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                SP5_Active = 1; // turn on track map (sun, grayline, voacap, or beacon)
                t.Name = "Beacon slot tracking";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                textBox1.Text = "Clicked on Beacon Tracking\r\n";

                processTCPMessage1();

            } //  if (beacon == true)

            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            // beacon scanner turned OFF
            else
            {
                if (beacon2 == false)
                {
                    SP8_Active = 0; // sp8_active as originally 0, so return it to 0
                    Debug.WriteLine(">>>>>>>>BEACON: SP8 was originaly 0 so return back to 0");
                }

                Debug.WriteLine(">>>>>>>>BEACON: TURN BACK OFF2");

                beacon4 = true; // do map update

                beacon1 = false; // turn beacon back off

                //------------------------------------------------------

                if (beacon3 == false) // map was off so turn back off
                {

                    SP5_Active = 0;                     // turn off tracking

                    Display.map = 0;                    // tell display program to got back to standard panafall mode

                    if (chkPanMode.Checked == true) Display.CurrentDisplayMode = LastDisplayMode;

                    if (console.setupForm.gridBoxTS.Checked == true) Display.GridOff = 1; // put gridlines back the way they were
                    else Display.GridOff = 0; // gridlines ON

                    btnTrack.Text = "Track";

                    textBox1.Text += "Click to turn off GrayLine Sun Tracking\r\n";

                    if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image

                } // map was off so turn back off after doen with beacon

                beacon4 = true; // do map update

                processTCPMessage();


            }//  beacon button clicked OFF


        } // btnBeacon_Click()  (Beacon Check on/off)




        //==============================================================================
        //==============================================================================
        //==============================================================================
        // BEACON Scanner ON/OFF  (Fast or Slow)
        //==============================================================================
        //==============================================================================
        //==============================================================================
        // ke9ns THREAD

        // 18 Stations scan 5 Frequencies on 10sec intervals (starting with 4U1UN and 14.1 mhz)
        // So it takes 4U1UN 50 seconds to complete its transmission and then waits silent for the remaining 130 seconds

        // The listener can sit on 14.1 and here 18 stations (in succession) for a total of 3 minutes
        // That would take a total of 20minutes to hear all 18 stations on all 5 frequencies

        // The listener can pick 1 station and jump to all 5 frequencies when its that stations time (starting at 14.1 mhz)
        // That would take a total of 3 minutes to hear but may require up to 3min to start, for a total of 6minutes worst case

        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts;

        Stopwatch stopWatch1 = new Stopwatch();
        TimeSpan ts1;

        double tsTime = 0;
        double LasttsTime = 0;

        double tsTime1 = 0;                                             // replaced beacon6 for ant switch glitch which causes S meter spike
        double BandSwitchDelay = 0.24;                                   // replaced beacon6 amount of delay to get past the ant switch glitch

        public static string SEC1;                                       // get 24hr 4 digit UTC NOW
      
        public static int SECNEW1;                                       // convert 24hr UTC to int
        public static int seconds;
        public static int minutes;                                       // get 24hr 4 digit UTC NOW
        public static int Last_seconds;
        public static int Totseconds;
        public static int SlotSeconds;  // conversion of mmss down to 3min intervals
        public static int TSlot; // 10 second intervals
        public static int Last_TSlot; // 10 second intervals
        public static int Last_TSlot1; // 10 second intervals for slow scan

        //========================================================================================== 
        private void BeaconSlot()
        {
            Debug.WriteLine(">>>>>>>>BEACON:  thread started");

            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            SEC1= UTCD.ToString("mm:ss");
            minutes = Convert.ToInt16( SEC1.Substring(0, 2));
            seconds = Convert.ToInt16(SEC1.Substring(3, 2));
   
            // seconds = TimeSpan.Parse(SEC1).TotalSeconds; // get total seconds of the day
            // example: 23:19 = 23min 19sec = min%3 = modulo of 2minutes into a 3 min block + 19sec = 139 seconds into a 180second block

            SlotSeconds = ((minutes % 3)*60)+seconds;
            TSlot = (SlotSeconds / 10) * 10;

            Debug.WriteLine(">>>TIME1: THREAD START TIME " + SEC1);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in minutes " + minutes);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in seconds " + seconds);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in total seconds per 3 minute intervals: " + SlotSeconds);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in total seconds per 3 minute intervals/10: " + TSlot);


            //---------------------------------------------------------------------
            for (int x = 0; x < 18;x++) // find starting station then BX_Index will keep track
            {
             
                if ((BX_TSlot1[x*5] >= TSlot) && (BX_TSlot1[x * 5] < TSlot+10))
                {
                    BX_Index[0] = x * 5; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[0] 14mhz station# " +  BX_Index[0] + " , "+BX_Station[BX_Index[0]]);
                }
                else if ((BX_TSlot1[x * 5 + 1] >= TSlot) && (BX_TSlot1[x * 5 + 1] < TSlot + 10))
                {
                    BX_Index[1] = x * 5+1; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[1] 18mhz station# " + BX_Index[1] + " , " + BX_Station[BX_Index[1]]);
                }
                else if ((BX_TSlot1[x * 5 + 2] >= TSlot) && (BX_TSlot1[x * 5 + 2] < TSlot + 10))
                {
                    BX_Index[2] = x * 5+2; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[2] 21mhz station# " + BX_Index[2] + " , " + BX_Station[BX_Index[2]]);
                }
                else if ((BX_TSlot1[x * 5 + 3] >= TSlot) && (BX_TSlot1[x * 5 + 3] < TSlot + 10))
                {
                    BX_Index[3] = x * 5+3; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[3] 24mhz station#  " + BX_Index[3] + " , " + BX_Station[BX_Index[3]]);
                }
                else if ((BX_TSlot1[x * 5 + 4] >= TSlot) && (BX_TSlot1[x * 5 + 4] < TSlot + 10))
                {
                    BX_Index[4] = x * 5+4; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[4] 28mhz station# " + BX_Index[4] + " , " + BX_Station[BX_Index[4]]);
                }

            } // for loop of 90

            Last_TSlot1 = BX_TSlot2 = Last_TSlot = TSlot;

          
            beacon4 = true;
            beacon5 = 0;


            beacon7 = console.RX1DSPMode;           // get mode so you can restore it when you turn off the beacon check
            beacon8 = console.RX1FilterHigh;        // get high filter so you can restore it when you turn off the beacon check
            beacon9 = console.RX1FilterLow;         // get low filter so you can restore it when you turn off the beacon check
            beacon89 = console.RX1Filter;           // get filter name so you can restore
            beacon77 = (int)console.udCWPitch.Value;     // get filter name so you can restore
            beacon66 = console.BlockSize1;          // get blocksize (must be 2048 during wwv bcd read)
            beacon88 = console.VFOAFreq;            // get vfoa


            beacon66 = console.BlockSize1;          // get blocksize (must be 2048 during wwv bcd read)
            oldSR = console.SampleRate1;            // get SR


            GoertzelCoef(600.0, console.SampleRate1);  // comes up with the Coeff values for the freq and sample rate used

            //-----------------------------------------------------------------------
            //-----------------------------------------------------------------------
            while ( (beacon1 == true) ) // only do while the beacon testing is going on
            {
                Thread.Sleep(10); // slow down the thread here

                UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                SEC1 = UTCD.ToString("mm:ss");
                minutes = Convert.ToInt16(SEC1.Substring(0, 2));
                seconds = Convert.ToInt16(SEC1.Substring(3, 2));


                 SlotSeconds = ((minutes % 3) * 60) + seconds;
                 TSlot = (SlotSeconds / 10) * 10;


                if (Last_TSlot != TSlot)
                {
                    Last_TSlot = TSlot; // update 1 time per 10seconds

                 //  Debug.WriteLine(">>>TIME2: THREAD START TIME in total seconds per 3 minute intervals/10: " + TSlot);

                    for (int x = 0; x < 18; x++) // find starting station then BX_Index will keep track
                    {

                        if ((BX_TSlot1[x * 5] >= TSlot) && (BX_TSlot1[x * 5] < TSlot + 10)) // find the 5 stations currently transmitting for this 10 second slot
                        {
                            BX_Index[0] = x * 5; // this is the start index
                           Debug.WriteLine(">>>TIME2: Current BX_Index[0] 14mhz station# " + BX_Index[0] + " , " + BX_Station[BX_Index[0]]);

                        }
                        else if ((BX_TSlot1[x * 5 + 1] >= TSlot) && (BX_TSlot1[x * 5 + 1] < TSlot + 10))
                        {
                            BX_Index[1] = x * 5 + 1; // this is the start index
                            Debug.WriteLine(">>>TIME2: Current BX_Index[1] 18mhz station# " + BX_Index[1] + " , " + BX_Station[BX_Index[1]]);

                        }
                        else if ((BX_TSlot1[x *5 + 2] >= TSlot) && (BX_TSlot1[x * 5 + 2] < TSlot + 10))
                        {
                            BX_Index[2] = x * 5 + 2; // this is the start index
                           Debug.WriteLine(">>>TIME2: Current BX_Index[2] 21mhz station# " + BX_Index[2] + " , " + BX_Station[BX_Index[2]]);

                        }
                        else if ((BX_TSlot1[x * 5 + 3] >= TSlot) && (BX_TSlot1[x * 5 + 3] < TSlot + 10))
                        {
                            BX_Index[3] = x * 5 + 3; // this is the start index
                          Debug.WriteLine(">>>TIME2: Current BX_Index[3] 24mhz station#  " + BX_Index[3] + " , " + BX_Station[BX_Index[3]]);

                        }
                        else if ((BX_TSlot1[x * 5 + 4] >= TSlot) && (BX_TSlot1[x * 5 + 4] < TSlot + 10))
                        {
                            BX_Index[4] = x * 5 + 4; // this is the start index
                           Debug.WriteLine(">>>TIME2: Current BX_Index[4] 28mhz station# " + BX_Index[4] + " , " + BX_Station[BX_Index[4]]);

                        }

                    } // for loop of 90 

                    //   Debug.WriteLine(">>>TIME: New SLot: " + UTCD);
                    BX_TSlot2 = TSlot;

                    beacon4 = true; // force map update

                    if (BoxBScan.Checked == true) // fast 3 minute complete scan (5 freq over 18 periods)
                    {

                        if (console.SampleRate1 == 192000)  // need to reduce the 192SR because the Tone detection needs a longer sample time to detect weak signals at 192k and 2048 buffer size limit
                        {
                         //   console.setupForm.comboAudioSampleRate1.Text = "96000"; // select 96000
                         //   if (console.BlockSize1 != 2048) console.BlockSize1 = 2048;  // need the largest buffer size for the Tone detection to work.

                        }


                        LasttsTime = 0;   // time period for fast scanning each of 5 frequencies
                        stopWatch.Restart(); // reset every time Slot (10 seconds)

                      
                        if (beacon5 == 0) // only do one time
                        {
                           
                            beacon5 = 1; // scan all 5 freq fast fast
                            beacon10 = true; // you will put back original op mode when done with scan
                            Debug.WriteLine(">>>BEACON5 RESET..................... ");

                            beacon11 = 0;
                            beacon12 = 0;
                            beacon14 = 0;
                        }
                        else if (beacon5 == 7) beacon5 = 1; // scan next Tslot as long as the checkbox is still checked

                    }
                    else if (BoxBFScan.Checked == true) // Long slow 15 minute complete scan (1 freq over 18 periods, 5 times)
                    {

                        if (console.SampleRate1 == 192000)  // need to reduce the 192SR because the Tone detection needs a longer sample time to detect weak signals at 192k and 2048 buffer size limit
                        {
                         //   console.setupForm.comboAudioSampleRate1.Text = "96000"; // select 96000
                         //   if (console.BlockSize1 != 2048) console.BlockSize1 = 2048;  // need the largest buffer size for the Tone detection to work.

                        }


                        if (beacon11 == 0) // only do one time
                        {
                            beacon11 = (int)numericUpDownTS1.Value; // 1-5  scan all 5 freq fast fast (6=done)  (normally set to 1

                            stopWatch.Stop();
                            stopWatch1.Stop();

                            beacon12 = 0; // 1-18 scan all 18 slots (stations) for 5 times
                            beacon5 = 0; // turn off fast scanner
                            beacon10 = true; // you will put back original op mode when done with scan
                            beacon14 = 0; // slot position holder (0-85 = start of each station in stack )
                            beacon15 = 0; // reset
                        }
                    }
                    else // if both check boxes are off
                    {
                        beacon5 = 0;  // clear all scanner functions since your not scanning
                        beacon11 = 0; // 1-5 freq
                        beacon12 = 0; // 1-18 slots
                        beacon14 = 0;

                        stopWatch.Stop();
                        stopWatch1.Stop();

                        if (beacon10 == true) // put back original op mode, now that the beacon scanner was turned from ON to OFF
                        {
                            console.udCWPitch.Value = beacon77;     // restore cw pitch value
                          
                            console.UpdateRX1Filters(beacon9, beacon8); // restore filter
                            console.RX1Filter = beacon89;           // restore filter name
                            console.RX1DSPMode = beacon7;           //  restore  mode  when you turn off the beacon check

                            console.VFOAFreq = beacon88;             // restore VfoA

                            if (oldSR == 192000)            // 
                            {
                                console.setupForm.comboAudioSampleRate1.Text = "192000"; // select 192000 again when done
                                console.BlockSize1 = beacon66;          // get blocksize (must be 2048 during wwv bcd read)

                            }
                            console.UpdateDisplay();

                            beacon10 = false;
                        }
                    }

                    Last_seconds = 1000; // reset so the first time through the scanner it selects a freq

                } // new TSLOT came up 



                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                // ke9ns  add SLOW beacon scan
                // BX_Index[0] = is the slot (0 to 89) up now on 14mhz
                // BX_Index[1] = is the slot (0 to 89) up now on 18mhz, etc., etc.
                // beacon13[] = 0 to 89 slots (0=14m, 1=18m, 2=21m, 3=24m, 4=28mhz repeating)(with every 5th slot is next station)

                if (beacon11 > 0) // do a slow scan
                {
                  
                    if (Last_TSlot1 != TSlot)
                    {
                        Last_TSlot1 = TSlot; // update 1 time per 10seconds

                       
                        if (beacon11 < 6) // scan through all 5 beacon freq
                        {

                            // set mode and freq
                            if (console.RX1DSPMode != DSPMode.CWU) console.RX1DSPMode = DSPMode.CWU;

                            if (console.udCWPitch.Value != 600)   console.udCWPitch.Value = 600;

                            if (console.RX1Filter != Filter.VAR1) console.RX1Filter = Filter.VAR1;

                            if ( (console.RX1FilterHigh != 650) || (console.RX1FilterLow != 550) )
                            {
                                console.UpdateRX1Filters(550, 650);   // sete cw filter
                            }

                            console.VFOAFreq = (double)Beacon_Freq[beacon11 - 1] / 1e6; // shift 0hz down 600 for cw mode and convert to MHZ

                            console.UpdateDisplay();

                            Debug.WriteLine(">>>freq:beacon11, BX_Index[beacon11 - 1] , beacon14: " + beacon11+" , "+ BX_Index[beacon11 - 1] + " , " + beacon14);

                            //------------------------------------------

                            if (beacon15 > 0) // start processing after the 1st 10 seconds of each freq change
                            {
                                if (BX_dBm[beacon14] >= -73)      BX_dBm3[beacon14] = 9;
                                else if (BX_dBm[beacon14] >= -79) BX_dBm3[beacon14] = 8;
                                else if (BX_dBm[beacon14] >= -85) BX_dBm3[beacon14] = 7;
                                else if (BX_dBm[beacon14] >= -91) BX_dBm3[beacon14] = 6;
                                else if (BX_dBm[beacon14] >= -97) BX_dBm3[beacon14] = 5;
                                else if (BX_dBm[beacon14] >= -103) BX_dBm3[beacon14] = 4;
                                else if (BX_dBm[beacon14] >= -109) BX_dBm3[beacon14] = 3;
                                else if (BX_dBm[beacon14] >= -115) BX_dBm3[beacon14] = 2;
                                else if (BX_dBm[beacon14] >= -121) BX_dBm3[beacon14] = 1;
                                else BX_dBm3[beacon14] = 0;

                              
                                if ((BX_dBm1[beacon14] - BX_dBm[beacon14]) > -1)
                                {
                                  //  Debug.WriteLine("dbm " + BX_dBm1[beacon14] + " , " + BX_dBm[beacon14]);

                                    BX_dBm1[beacon14] = -151; // noise floor
                                    BX_dBm[beacon14] = -150; // signal 
                                }

                                //  Debug.WriteLine("Slow: Call, freq, dbm, S, noise floor: " + BX_Station[ beacon14] + " , " + BX_Freq[ beacon14] + " , " + BX_dBm[ beacon14] + " , " + BX_dBm3[ beacon14] + " , " + BX_dBm1[ beacon14]); // 20m

                                BX_TSlot[beacon14] = 1; // set indicator for panadapter display

                                BX_FULLSTRING[ beacon14] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[ beacon14] / 1e3).ToString("f1")).PadRight(9) + BX_Station[ beacon14].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[ beacon14] + " " + (BX_dBm1[beacon14] - BX_dBm[beacon14]).ToString("D3") + " dBm " + BX_Time[ beacon14].ToString("D4") + "z " + BX_Grid[beacon14];

                                processTCPMessage1(); // update dx spotter window for the beacon system
                           

                            } // if beacon15 > 0

                            beacon15 = 1;       // skip first time through after a freq change

                            beacon14 = BX_Index[beacon11 - 1];

                            beacon12++;         // increment station counter every 10 sec (make sure we scan all 18 stations)

                            if (beacon12 == 18) // when we have all 18 stations then go to next freq
                            {
                               
                                beacon12 = 0;   // reset counter
                                beacon11++;     // go to next freq the the slot

                                beacon6 = 0;   // reset noise pulse ignore
                                stopWatch1.Stop();
                                stopWatch1.Reset();

                                              
                                Debug.WriteLine(">>>RESET BEACON11: "+beacon11);

                                if (beacon11 == 6)
                                {
                                    Debug.WriteLine(">>>SLOW BEACON DONE<<<<<<<<<<<<<<<<<<<<<<");

                                    BoxBFScan.Checked = false; // turn off slow scan
                                    BoxBScan.Checked = false; // turn off slow scan

                                    beacon12 = 0;
                                    beacon11 = 0;
                                    beacon14 = 0;
                                    beacon15 = 0;

                                }

                            } // if beacon12  18 stations (slots)


                        } // if beacon11 < 6  5 freq
                        else // when done scanning all 5 freq
                        {
                            Debug.WriteLine(">>>SLOW BEACON DONE<<<<<<<<<<<<<<<<<<<<<<");

                            BoxBFScan.Checked = false; // turn off slow scan
                            BoxBScan.Checked = false; // turn off slow scan

                            beacon12 = 0;
                            beacon11 = 0;
                        }

                    } // if (last TSLOT != TSLOT)  10 second intervals
                    else // search for a signal
                    {

                        if (BoxBFScan.Checked == true) // full scan
                        {
                            stopWatch1.Start();

                            ts1 = stopWatch1.Elapsed;
                            tsTime1 = (double)ts1.Seconds + ((double)ts1.Milliseconds / 1000.0); // total time in seconds

                            if (tsTime1 >= BandSwitchDelay)    // (beacon6 > 25) // wait for band switching pulse to disapate
                            {
                                int tempDB = 0;
                                int tempDB1 = 0;

                               
                                tempDB = console.ReadAvgStrength(0);    // get beacon CW signal strength, but this does not factor out the noise floor (i.e. S5 signal might just be the noise floor at S5)

                              //  tempDB1 = console.WWVTone;  // get Magnitude value from audio.cs and Goertzel routine  (i.e. this will determine if we are actually hearing a CW signal at 600hz and not just an S5 noise floor)

                             //   Debug.WriteLine("BEACON TONE Detection: " + tempDB1);


                                if (tempDB > BX_dBm[beacon14])
                                {
                                    BX_dBm[beacon14] = tempDB; // get signal strengh avg reading to match avg floor reading
                                }

                                if (BX_dBm2 > BX_dBm1[beacon14] )
                                    BX_dBm1[beacon14] = BX_dBm2; // value passed back from display.cs noise floor (avg value)

                              //  WWVThreshold = BX_dBm1[beacon14]; // display.cs the floor
                            }
                            else   beacon6++;

                        }

                    } // in between TSLOT changes (in between 10 second slots)



                } // if (beacon11 > 0) slow scan


                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                // ke9ns  add FAST beacon scan

                if (beacon5 > 0)
                {
                 
                    ts = stopWatch.Elapsed;
                    tsTime = (double)ts.Seconds + ((double)ts.Milliseconds / 1000.0);

                    if (tsTime >= LasttsTime )
                    {

                      //   Debug.WriteLine("RunTime1 " + tsTime);

                        LasttsTime = LasttsTime + 1.15;  // FAST SCAN time delay of 1.4 seconds

                        if (beacon5 < 6)
                        {
                            // set mode and freq
                            if (console.RX1DSPMode != DSPMode.CWU) console.RX1DSPMode = DSPMode.CWU;
                            if (console.udCWPitch.Value != 600) console.udCWPitch.Value = 600;
                            if (console.RX1Filter != Filter.VAR1) console.RX1Filter = Filter.VAR1;

                            if ((console.RX1FilterHigh != 650) || (console.RX1FilterLow != 550))
                            {
                                console.UpdateRX1Filters(550, 650);   // sete cw filter
                            }

                            console.VFOAFreq = (double)Beacon_Freq[beacon5 - 1] / 1e6; //  convert to MHZ

                            console.UpdateDisplay();

                            beacon6 = 0; // reset noise pulse ignore
                            stopWatch1.Stop();
                            stopWatch1.Reset();

         
                        }

                        if (beacon5 == 6)
                        {


                            //S9+10dB 160.0 -63 44 
                            //S9 50.2 -73 34 
                            //S8 25.1 -79 28 
                            //S7 12.6 -85 22 
                            //S6 6.3 -91 16 
                            //S5 3.2 -97 10 
                            //S4 1.6 -103 4 
                            //S3 0.8 -109 -2 
                            //S2 0.4 -115 -8 
                            //S1 0.2 -121 -14 

                            for (int u = 0; u < 5; u++)
                            {
                                if (BX_dBm[BX_Index[u]] >= -73) BX_dBm3[BX_Index[u]] = 9;
                                else if (BX_dBm[BX_Index[u]] >= -79) BX_dBm3[BX_Index[u]] = 8;
                                else if (BX_dBm[BX_Index[u]] >= -85) BX_dBm3[BX_Index[u]] = 7;
                                else if (BX_dBm[BX_Index[u]] >= -91) BX_dBm3[BX_Index[u]] = 6;
                                else if (BX_dBm[BX_Index[u]] >= -97) BX_dBm3[BX_Index[u]] = 5;
                                else if (BX_dBm[BX_Index[u]] >= -103) BX_dBm3[BX_Index[u]] = 4;
                                else if (BX_dBm[BX_Index[u]] >= -109) BX_dBm3[BX_Index[u]] = 3;
                                else if (BX_dBm[BX_Index[u]] >= -115) BX_dBm3[BX_Index[u]] = 2;
                                else if (BX_dBm[BX_Index[u]] >= -121) BX_dBm3[BX_Index[u]] = 1;
                                else BX_dBm3[BX_Index[u]] = 0;


                                if ((BX_dBm1[BX_Index[u]] - BX_dBm[BX_Index[u]]) > -1)
                                {
                                     Debug.WriteLine("dbm " + BX_dBm1[BX_Index[u]] + " , " + BX_dBm[BX_Index[u]]);

                                    BX_dBm1[BX_Index[u]] = -151; // noise floor
                                    BX_dBm[BX_Index[u]] = -150; // signal 
                                }
                            } // for loop all 5 freq

                            //   Debug.WriteLine(">>>beacon12: " + beacon12);

                            //    Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[0]] + " , " + BX_Freq[BX_Index[0]] + " , " + BX_dBm[BX_Index[0]] + " , " + BX_dBm3[BX_Index[0]] + " , " + BX_dBm1[BX_Index[0]]); // 20m
                            //    Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[1]] + " , " + BX_Freq[BX_Index[1]] + " , " + BX_dBm[BX_Index[1]] + " , " + BX_dBm3[BX_Index[1]] + " , " + BX_dBm1[BX_Index[1]]); // 17m
                            //   Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[2]] + " , " + BX_Freq[BX_Index[2]] + " , " + BX_dBm[BX_Index[2]] + " , " + BX_dBm3[BX_Index[2]] + " , " + BX_dBm1[BX_Index[2]]); // 15m
                            //    Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[3]] + " , " + BX_Freq[BX_Index[3]] + " , " + BX_dBm[BX_Index[3]] + " , " + BX_dBm3[BX_Index[3]] + " , " + BX_dBm1[BX_Index[3]]); //12m
                            //   Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[4]] + " , " + BX_Freq[BX_Index[4]] + " , " + BX_dBm[BX_Index[4]] + " , " + BX_dBm3[BX_Index[4]] + " , " + BX_dBm1[BX_Index[4]]); //10m


                            BX_TSlot[BX_Index[4]] = BX_TSlot[BX_Index[3]] = BX_TSlot[BX_Index[2]] = BX_TSlot[BX_Index[1]] = BX_TSlot[BX_Index[0]] = 1; //  // set indicator for panadapter display

                            BX_FULLSTRING[BX_Index[0]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[0]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[0]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[0]] + " " + (BX_dBm1[BX_Index[0]] - BX_dBm[BX_Index[0]]).ToString("D3") + " dBm " + BX_Time[BX_Index[0]].ToString("D4") + "z " + BX_Grid[BX_Index[0]];
                            BX_FULLSTRING[BX_Index[1]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[1]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[1]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[1]] + " " + (BX_dBm1[BX_Index[1]] - BX_dBm[BX_Index[1]]).ToString("D3") + " dBm " + BX_Time[BX_Index[1]].ToString("D4") + "z " + BX_Grid[BX_Index[1]];
                            BX_FULLSTRING[BX_Index[2]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[2]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[2]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[2]] + " " + (BX_dBm1[BX_Index[2]] - BX_dBm[BX_Index[2]]).ToString("D3") + " dBm " + BX_Time[BX_Index[2]].ToString("D4") + "z " + BX_Grid[BX_Index[2]];
                            BX_FULLSTRING[BX_Index[3]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[3]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[3]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[3]] + " " + (BX_dBm1[BX_Index[3]] - BX_dBm[BX_Index[3]]).ToString("D3") + " dBm " + BX_Time[BX_Index[3]].ToString("D4") + "z " + BX_Grid[BX_Index[3]];
                            BX_FULLSTRING[BX_Index[4]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[4]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[4]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[4]] + " " + (BX_dBm1[BX_Index[4]] - BX_dBm[BX_Index[4]]).ToString("D3") + " dBm " + BX_Time[BX_Index[4]].ToString("D4") + "z " + BX_Grid[BX_Index[4]];

                            processTCPMessage1(); // update dx spotter window for the beacon system

                             stopWatch.Stop(); // stop to reset
                             stopWatch1.Stop(); // stop to reset


                            beacon12++;
                            if (beacon12 == 18)
                            {
                                Debug.WriteLine(">>>TURN OFF beason fast");

                                BoxBScan.Checked = false; // turn off slow scan
                                BoxBFScan.Checked = false;
                                beacon12 = 0;
                                beacon11 = 0;
                                beacon5 = 0;
                            }
                            else
                            {
                                beacon5 = 7;
                                console.VFOAFreq = (double)Beacon_Freq[0] / 1e6; // reset to start beacon freq


                            }



                        } // if beacon5 == 6
                        else
                        {
                            if (beacon5 < 6)  beacon5++; // if not == 6 

                        }

                    } // seconds != lastseconds
                    else // search for a signal
                    {

                        stopWatch1.Start();
                        ts1 = stopWatch1.Elapsed;
                        tsTime1 = (double)ts1.Seconds + ((double)ts1.Milliseconds / 1000.0);

                        if ((beacon5 < 7) &&  (tsTime1 >= BandSwitchDelay))          //(beacon6 > 25)) // wait for band switching pulse to disapate
                        {
    
                            int tempDB = console.ReadAvgStrength(0);

                            if (tempDB > BX_dBm[beacon14])
                            {
                                BX_dBm[BX_Index[beacon5 - 2]] = tempDB; // get signal strengh avg reading to match avg floor reading
                            }

                            if (BX_dBm2 > BX_dBm1[BX_Index[beacon5 - 2]] )
                               BX_dBm1[BX_Index[beacon5 - 2]] = BX_dBm2;                           // this is the noise Floor value passed back from Display to spot.cs

                         //   WWVThreshold = BX_dBm1[BX_Index[beacon5 - 2]]; // display.cs floor

                        } //   wait for band switching pulse to disapate
                        
                        beacon6++;


                    } // in between each 1 second interval

                } // beacon5 > 0


            } //  while ( beacon1 == true)

            //--------------------------------------------------------------------


            Debug.WriteLine(">>>>>>>>BEACON:  thread STOPPED");
            beacon5 = 0;

            stopWatch.Stop();
            stopWatch1.Stop();

            if (beacon10 == true)
            {
                console.udCWPitch.Value = beacon77;     // restore cw pitch value

                console.UpdateRX1Filters(beacon9, beacon8); // restore filter
                console.RX1Filter = beacon89;           // restore filter name
                console.RX1DSPMode = beacon7;           //  restore  mode  when you turn off the beacon check

                console.VFOAFreq = beacon88;             // restore VfoA

                if (oldSR == 192000)                      // 
                {
                    console.setupForm.comboAudioSampleRate1.Text = "192000"; // select 192000 again when done
                    console.BlockSize1 = beacon66;          // get blocksize (must be 2048 during wwv bcd read)

                }

                console.UpdateDisplay();
            }

        } //  private void BeaconSlot()

        //=============================================================================================================
        private void BoxBFScan_CheckedChanged(object sender, EventArgs e)
        {
            BoxBScan.Checked = false;
            beacon11 = 0; // reset freq for slow scan since you may have changed the freq
            beacon12 = 0; // reset the station your looking at

            if (BoxBFScan.Checked == true)
            {
                for (int x = 0; x < 18; x++)  // reset time and reset back to GRAY since your now running a new test
                {
                    BX_Time[x * 5 + 4] = BX_Time[x * 5 + 3] = BX_Time[x * 5 + 2] = BX_Time[x * 5 + 1] = BX_Time[x * 5] = UTCNEW;

                    BX_TSlot[x * 5] = 0;        // #=how many times the station was checked  (0=never checked yet, gray) (1=checked color = signal)
                    BX_TSlot[x * 5 + 1] = 0;   // 
                    BX_TSlot[x * 5 + 2] = 0;   // 
                    BX_TSlot[x * 5 + 3] = 0;   // 
                    BX_TSlot[x * 5 + 4] = 0;   // 
                }
            }

        } // BoxBFScan_CheckedChanged

        private void BoxBScan_CheckedChanged(object sender, EventArgs e)
        {
            BoxBFScan.Checked = false;

            if (BoxBScan.Checked == true)
            {
                for (int x = 0; x < 18; x++)  // reset time and reset back to GRAY since your now running a new test
                {
                    BX_Time[x * 5 + 4] = BX_Time[x * 5 + 3] = BX_Time[x * 5 + 2] = BX_Time[x * 5 + 1] = BX_Time[x * 5] = UTCNEW;

                    BX_TSlot[x * 5] = 0;        // #=how many times the station was checked  (0=never checked yet, gray) (1=checked color = signal)
                    BX_TSlot[x * 5 + 1] = 0;   // 
                    BX_TSlot[x * 5 + 2] = 0;   // 
                    BX_TSlot[x * 5 + 3] = 0;   // 
                    BX_TSlot[x * 5 + 4] = 0;   // 
                }
            }
        } // BoxBScan_CheckedChanged

        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        // ke9ns Grab NIST Internet Time
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        public void SetInternetTime()
        {
            textBox1.Text = "Attempting Internet Connection to NIST Time Server!\r\n";

            DateTime startDT = DateTime.Now; 
           
            //Create a IPAddress object and port, create an IPEndPoint node:  
            int port = 13;
            string[] whost = { "nist.netservicesgroup.com", "utcnist.colorado.edu", "time-c.nist.gov", "time-b.nist.gov", "time-a.nist.gov"  };  //  
            string strHost;

            IPHostEntry iphostinfo;
            IPAddress ip;
            IPEndPoint ipe;
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Create Socket  

            c.ReceiveTimeout = 1100;    //Setting the timeout  

            byte[] RecvBuffer = new byte[1024];

            int nBytes = 0;
            int nTotalBytes = 0;

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            System.Text.Encoding myE = Encoding.UTF8;

            string EX1 = " ";
            TimeSpan k = new TimeSpan();
            TimeSpan k1 = new TimeSpan();

            SystemTime st = new SystemTime();

            DateTime SetDT = DateTime.Now;

            startDT = DateTime.Now; // record time you opened a connection to NIST

            //   foreach (string strHost in whost)   // try all the time servers until you get a response
            //  {

            if (chkTimeServer1.Checked == true) strHost = whost[0];
            else if (chkTimeServer2.Checked == true) strHost = whost[1];
            else if (chkTimeServer3.Checked == true) strHost = whost[2];
            else if (chkTimeServer4.Checked == true) strHost = whost[3];
            else if (chkTimeServer5.Checked == true) strHost = whost[4];
            else strHost = "nist.netservicesgroup.com";

            try
            {
              
                        iphostinfo = Dns.GetHostEntry(strHost);

                        ip = iphostinfo.AddressList[0];

                        ipe = new IPEndPoint(ip, port);

                       Debug.WriteLine("attempt connection to " + strHost + " , " + ipe);
                        textBox1.Text += "You can select a Time server from the Menu list above.\r\n" + "Attempt Connect to NIST Time Server> "+ strHost + ", " + ipe + "\r\n";

                        startDT = DateTime.Now; // record time you opened a connection to NIST

                        c.Connect(ipe);     // Connect to server which starts clock (NIST will now send back the correct Time)

                      
                        Debug.WriteLine("wait connection to " + strHost);
                   
                        if (c.Connected)
                        {
                       
                            textBox1.Text += "Connected to NIST Time Server!\r\n";

                            Debug.WriteLine("got connection to " + strHost);
                           // break;  // If the connection to the server is out of 
                       
                        }
                        else
                        {
                            Debug.WriteLine("no  connection to " + strHost);
                           textBox1.Text += "Could not Connect to NIST Time Server! Try another\r\n";

                        }



                }
                catch (Exception ex)
                {

                    textBox1.Text += "Error connecting to this NIST Time Server!\r\n";
                    if (c.Connected) c.Close(); // close the socket

                    EX1 = ex.Message;
                    // Debug.WriteLine("SOCKET ERROR: " + strHost + " , " + ex);
                  //  WTime = false;   // turn dx spotting back on
                  //  MessageBox.Show("Time server connection failed! 1 error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                  //  return;
                }

          //  } // for loop through time server addresses

            if (!c.Connected)
            {
                textBox1.Text += "Failure NIST Time Server!\r\n";

                if (c.Connected) c.Close(); // close the socket

                MessageBox.Show("Time server connection failed! 2 error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                WTime = false;   // turn dx spotting back on

                return;
            }

            try
            {
                //----------------------------------------------------------------
                // get NIST formated time
                while ((nBytes = c.Receive(RecvBuffer, 0, 1024, SocketFlags.None)) > 0)
                {
                    nTotalBytes += nBytes;
                    sb.Append(myE.GetString(RecvBuffer, 0, nBytes));
                }

            }
            catch(Exception)
            {

                if (c.Connected) c.Close(); // close the socket
                MessageBox.Show("Time server connection failed! /r error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                WTime = false;   // turn dx spotting back on

                return;
            }

            if (c.Connected)  c.Close(); // close the socket

            // example of downloaded time sync from NIST
            // <cr>57682 16-10-21 14:42:46 17 0 0 159.1 UTC(NIST) * 
            // o[0] = <cr>57682   MJD day since 4713 BC
            // o[1] = 16-10-21    date yy-mm-dd UTC
            // o[2] = 14:42:46    time hh:mm:ss UTC
            // o[3] = 17          0=ST 50=DT (in between indicates the number of days remaining in DT)
            // o[4] = 0           Leap second 0=no 1=leap second added at end of month (61 seconds), 2=leap second subtracted at end of month (59 seconds)
            // o[5] = 0           UT1
            // o[6] = 159.1       MSadv  milliseconds advance (if you send back the * to NIST, NIST will return the OTM # with the correct MSAdv value)
            // o[7] = *           OTM on time marker  #=ACTS successfully calibrated the path


            try
            {

                string[] o = sb.ToString().Split(' '); // Cut the string  

                string temp2 = o[6].Substring(0, 3); // get millisecond delay

                string temp1 = o[1] + " " + o[2] + ".000";

                SetDT = DateTime.ParseExact(temp1, "yy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
              
                k1 = TimeSpan.FromMilliseconds(Convert.ToInt32(temp2)); // convert NIST delay to k1 timespan
                k = (TimeSpan)(DateTime.Now - startDT);   // get the delay since we read the time from NIST (milliseconds)

                k1 = k1 - k; // Set your PC time based on time reported by NIST - the time it took to receive that time. 


                SetDT = Convert.ToDateTime(SetDT).Subtract(k1); // subtract this k1 value since NIST always reports a future time
              
                SetDT = SetDT.ToLocalTime(); // adjust UTC back to my time .NOW

                st.FromDateTime(SetDT); //Convert System.DateTime to SystemTime 
                Win32API.SetLocalTime(ref st);  //Call Win32 API to set the system time  

                textBox1.Text = "IMPORTANT: Your PC Time will NOT update unless PowerSDR is launched in ADMIN mode!!!!" + "\r\n" + 
                                "PC LOC TIME when Request sent to NIST: " + startDT.ToString("yy-MM-dd HH:mm:ss.fff") + "\r\n" +
                                "TIME UTC reported back from NIST : " + temp1 + "\r\n" +
                                "NIST reported this time: " + temp2 + " milliseconds Early" + "\r\n" +
                                "Time Delay: From request to Update is:" + k + " milliseconds" + "\r\n" +
                                "DONE: PC new LOC time: " + SetDT.ToString("yy-MM-dd HH:mm:ss.fff");


                Debug.WriteLine("DONE...Time delay from request to update:" + k + " milliseconds");
            }
            catch (Exception)
            {
                MessageBox.Show("Time server busy! Try Again.", " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                WTime = false;   // turn dx spotting back on

                return;

            }

            //    MessageBox.Show("Time synchronization, ", " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information);

            WTime = false;   // turn dx spotting back on

        } // SetInternetTime()



        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================


        // ke9ns request to update this PC's time clock
        private void btnTime_Click(object sender, EventArgs e)
        {

            if (checkBoxWWV.Checked == true) // Use WWV HF checkbox
            {
                if (WTime == false)
                {

                    textBox1.Text = "Will Attempt to read"; 

                    Thread t = new Thread(new ThreadStart(WWVTime));

                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                    WTime = true;   // enabled (let display know to get a Floor dbm

                   
                    t.Name = "WWV Time Sync";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.AboveNormal;
                    t.Start();

                    textBox1.Text += " Radio Station WWV !\r\n";
                }
                else
                {
                    checkBoxWWV.Checked = false;   // turn off WWV checking if you click on the Time sync button again
                    WTime = false;
                    WWVNewTime.Stop();
                    indexP = 0;                  
                    indexS = 0;
                }


            }
            else
            {
              
                Thread t = new Thread(new ThreadStart(SetInternetTime));  // get internet NIST time

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                WTime = true;   // enabled (let display know to get a Floor dbm

               
                t.Name = "NIST Time Sync";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            }

        } // btnTime_Click





        //====================================================================================================================
        //====================================================================================================================

        double[] WWV_Freq = { 2.500100, 5.000100, 10.000100, 15.000100 };  // listen to 100hz tone
        double[] WWV_Freq1 = { 2.5, 5.0, 10.0, 15.0 };                     // listen to 1000khz tone

        public bool WTime = false;
     
        Stopwatch tickON = new Stopwatch();        // WWV 100hz tick ON elapsed time to find if PCM BCD data stream pluse is 1 or 0
        Stopwatch tickOFF = new Stopwatch();       // WWV 100hz tick OFF elapsed time to find start of minute (HOLE)

        public Stopwatch WWVNewTime = new Stopwatch();    // This timer starts when a P frame is detected. If the HOLE is detected immediately after, then this time + .3sec is used as the marker

        DateTime WWVNT = DateTime.Now;

        public int WWVThreshold = 0; // the trip point where the PCM BCD data stream from WWV determines a 1 or 0

        
        int below_count = 0; // counter for how many times you got new data and it was below the threshold

        int[] storage = new int[200];

        public int indexP = 0;  // P frame index (with 10 seconds inside it)
        public int indexS = 0; // seconds index inside a P frame

        public int oldSR = 48000;     // to store original SR

        public bool WWVPitch = true;   // true = use pitch detection, false = use signal strength


        private void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
        {
            // this is the Event thats called when the setup_timer(ms) value is reached
        }

        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        // Thread
        // ke9ns Read WWV for Time Sync

        unsafe private void WWVTime()
        {


            //   timeProcPeriodic = new TimeProc(TimerPeriodicEventCallback);
            //   setup_timer(1000);

            beacon44 = console.RX1PreampMode;       // get preamp mode so you can restore it when you turn off wwvtime

            beacon7 = console.RX1DSPMode;           // get mode so you can restore it when you turn off wwvtime
            beacon8 = console.RX1FilterHigh;        // get high filter so you can restore it when you turn off wwvtime
            beacon9 = console.RX1FilterLow;         // get low filter so you can restore it when you turn off wwvtime

            beacon89 = console.RX1Filter;           // get filter name so you can restore

            beacon88 = console.VFOAFreq;            // get freq you were on before 
            beacon66 = console.BlockSize1;          // get blocksize (must be 2048 during wwv bcd read)

            beacon33 = console.DSPBufPhoneRX;       // get DSP RX buffer size (needs to be 4096 for good wwv pulses


            //   beacon55 = console.CATPreamp;


            console.RX1PreampMode = PreampMode.HIGH; // turn on Preamp

            oldSR = console.SampleRate1;            // get SR

            if (checkBoxTone.Checked == true)    // this would allow you to select the signal strength based detection instead of Pitch(tone) based detection. For experimenting
            {
          //      WWVPitch = false;
            }
            else
            {
                WWVPitch = true;  // only allow Pitch (tone) detection
            }

          // REDUCE SAMPLERATE (until I can figure out why I cant make it work at 192k)
            if (oldSR == 192000)  // need to reduce the 192SR because the Tone detection needs a longer sample time to detect weak signals at 192k and 2048 buffer size limit
            {
                console.setupForm.comboAudioSampleRate1.Text = "96000"; // select 96000
                
            }
          
            if (beacon33 != 4096)
            {
                console.DSPBufPhoneRX = 4096;
            }

            textBox2.Text = "";
            checkBoxTone.Checked = false;   // turn off tone marker when done.

            // MAX OUT AUDIO BUFFER SIZE
            if (console.BlockSize1 != 2048) console.BlockSize1 = 2048;  // need the largest buffer size for the Tone detection to work.


            Debug.WriteLine("WWV>>0");

            // SETUP UP TONE DETECTION TO CATCH SUB-CARRIER
            GoertzelCoef(100.0, console.SampleRate1);  // comes up with the Coeff values for the freq and sample rate used


            // SET MODE, THEN 
            if (WWVPitch == false)
            {
                console.RX1DSPMode = DSPMode.DIGU;
                beacon89a = console.RX1Filter;           // get filter name so you can restore

                console.RX1Filter = Filter.VAR1;

                console.UpdateRX1Filters(-30, 30);

                textBox1.Text += "Signal Strength detection. Waiting for Start of Minute!\r\n";

                console.VFOAFreq = WWV_Freq[(int)udDisplayWWV.Value - 1];         // WWV in CWU mode will center on 5000.1 khz

                console.chkEnableMultiRX.Checked = true;  // enable sub receiver
                console.VFOBFreq = WWV_Freq1[(int)udDisplayWWV.Value - 1];       // WWV in CWU mode will center on 5000 khz

         
            }
            else
            {
                console.chkEnableMultiRX.Checked = false;  // enable sub receiver

              //  console.CATPreamp = PreampMode.OFF;

                console.RX1DSPMode = DSPMode.USB;
                beacon89a = console.RX1Filter;           // get filter name so you can restore

                console.RX1Filter = Filter.VAR1;

                console.UpdateRX1Filters(70, 170);

                textBox1.Text += "Tone detection. Waiting for Start of Minute!\r\n";

                console.VFOAFreq = WWV_Freq1[(int)udDisplayWWV.Value - 1];         // main receiver: WWV in DIGU mode on  sub-Carrier

              
            }


          
            console.UpdateDisplay();

            int BCDSignal= 0;              // measured BCD data stream dBm signal
            int CarrierSignal = 0;         // measured Carrier dBm signal
            int CarrierSignalINIT = 0;         // measured Carrier dBm signal

            bool BCD1timeFlag = false;

            bool BCDONTrig = false;
            bool BCDOFFTrig = false;

            int tickTimeON = 0;
           
            int tickTimeOFF = 0;
          
            tickON.Reset();
            tickOFF.Reset();

            int BCDMax = 0;
            int BCDMin = 0;
                  
            int BCDSignalON = 0;           // BCD data steam high dbm signal found initially
            int BCDSignalOFF = 0;           // BCD data steam high dbm signal found initially

      

            double BCDAdj = 3;                // % adjustment to what it determined to be the High signal
            int BCDCount = 0;              // counter for the % adjustment

            int BCDSignalON1 = 0;         // BCD data steam high dbm signal found while running
            int BCDSignalOFF1 = 0;           // BCD data steam low dbm signal found while running

            int WWVCF = 0;               // fault counter for low signal strength fault

          
            int[,] P = new int[7, 20]; // 6 Position Identifiers with 10 seconds inside each Position frame 

            int newMinutes = 0;
            int newHours = 0;

            int newDay1 = 0;  // P3
            int newDay2 = 0;  // p4
            int newDay = 0;  // p3 + p4


            int BCD1 = 0; // false BCD value detected as (0), true BCD value detected as (1)   [for this last second]

            bool WWVStart = false; // true = got start of minute frame
            bool WWVStop = false; // true = got entire 1 minute frame
            bool[] WWVFault = { false, false, false, false, false, false }; // true = bad data bit somewhere in WWV frames
            bool WWVPos = false;  // true = indicates you got a Position indicator frame at least 1 time before you got the HOLE (i.e. before WWVStart == true)

            TimeSpan k1 = new TimeSpan();
            SystemTime st = new SystemTime();   // used to update PC time

            WWVCF = 0;

            Stopwatch ST = new Stopwatch();   // internal 1 second time keeper
            Stopwatch ST1 = new Stopwatch();

            Stopwatch ST2 = new Stopwatch();

            Debug.WriteLine("WWV>>1"); 


            ST.Restart();


            while (ST.ElapsedMilliseconds < 2000)    // wait for things to calm down after you make changes to the mode
            {
                CarrierSignalINIT = 0;
                BCDSignalOFF = 0;

            }

            BCDSignalON = 0;         // RESET BCD data steam high dbm signal found while running
            BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running

            BCDSignalOFF = 0;         // RESET BCD data steam high dbm signal found while running
            BCDSignalOFF1 = 0;         // RESET BCD data steam high dbm signal found while running

            BCDCount = 0;
            BCDAdj = 0;
            
            Debug.WriteLine("WWV>>2");

            //------------------------------------------------------------------
          
            ST.Restart();

            if (WWVPitch == false)  // signal strength based detected
            {
                BCDSignalOFF = 0;
                BCDSignalON = -150;
                BCDSignalOFF1 = 0;
                BCDSignalON1 = -150;
            }


            while ( ST.ElapsedMilliseconds < 1300)                          // get floor for bcd stream
            {

                if (WWVPitch == false)  // signal strength based detected
                {
                    BCDSignal = console.ReadStrength(0);            // read wwv 100 hz OFF of carrier point (BCD data stream)            
                    CarrierSignal = console.ReadStrength(1);        // read WWV 0hz carrier point

                    if ((BCDSignal < BCDSignalOFF) )       // check low dBm value
                    {
                        BCDSignalOFF = BCDSignal;                               // finding the OFF state of this bcd stream area if the WWV signal
                        CarrierSignalINIT = CarrierSignal;                      // find the carrier level at the same time
                    }
                    if ((BCDSignal > BCDSignalON) )        // check high dBm value
                    {
                        BCDSignalON = BCDSignal;                                // finding the ON state of this bcd stream area if the WWV signal
                    }
                } //  if (WWVPitch == false)  // signal strength based detected
                else
                {
                    BCDSignal = console.WWVTone;  // get Magnitude value from audio.cs and Goertzel routine

                    if (BCDSignal > BCDSignalON)
                    {
                        BCDSignalON = BCDSignal;  // get maximum magnitude
                    }

                }


            } // for loop 1.3 seconds to test levels

  
            BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
            BCDCount = 0;

            BCDAdj = 3.0;

            ST.Stop();
            Debug.WriteLine("WWV>>3");

            Debug.WriteLine("WWV>> Highest BCD Mag: " + BCDSignalON + " , SR:" + console.SampleRate1 + " , Buffer size: " + console.BlockSize1);

            if (WWVPitch == false)  // signal strength based detected
            {
                textBox1.Text += "WWV BCD Data stream : " + BCDSignalON + "dBm , Carrier: " + CarrierSignal + " dBm, SR:" + console.SampleRate1 + " , Buffer size: " + console.BlockSize1 + "\r\n";
            }
            else
            {
                textBox1.Text += "WWV BCD Data stream MAG: " + BCDSignalON + " , SR:" + console.SampleRate1 + " , Buffer size: " + console.BlockSize1 + "\r\n";

            }

            ST.Restart();

         //  ST2.Restart();

            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            while (WTime == true)
            {

                if (WWVPitch == false)  // signal strength based detected (need around S9 to work)
                {
                    BCDSignal = console.ReadStrength(0);            // read wwv 100 hz OFF of carrier point (BCD data stream)            
                    CarrierSignal = console.ReadStrength(1);        // read WWV 0hz carrier point

                    //------------------------------------------------------------------
                    // keep adjusting signal based on signal strength you are seeing

                    if ((BCDSignal < BCDSignalOFF1))   // check low dBm value
                    {
                        BCDSignalOFF1 = BCDSignal;                           // finding the OFF state of this bcd stream area if the WWV signal

                    }
                    if ((BCDSignal > BCDSignalON1))    // check high dBm value
                    {
                        BCDSignalON1 = BCDSignal;                            // finding the ON state of this bcd stream area if the WWV signal
                    }

                    if (ST.ElapsedMilliseconds > 1300)
                    {
                        CarrierSignalINIT = CarrierSignal;
                        BCDSignalOFF = BCDSignalOFF1;
                        BCDSignalON = BCDSignalON1;

                        BCDSignalON1 = -150;         // RESET BCD data steam high dbm signal found while running
                        BCDSignalOFF1 = 0;           // RESET BCD data steam low dbm signal found while running
                        ST.Restart();

                        //   textBox1.Text += "WWV BCD Data stream: (0)= " + BCDSignalOFF + "dBm, (1)= " + BCDSignalON + "dBm @ Carrier Level: " + CarrierSignal + "dBm\r\n";

                    }


                    if ((uint)(BCDSignalON - BCDSignalOFF) < 6) // if you loose the carrier, then NO GOOD
                    {
                        if (WWVCF > 1000) // FAIL if carrier stays LOW for too long
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Carrier signal too low, choose different Frequency\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVPos = false;
                            WWVStart = false;
                            WWVStop = false;
                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                            WTime = false;
                        }
                        else WWVCF++;

                    }
                    else
                    {
                        WWVCF = 0;
                    }

                    WWVThreshold = BCDSignalOFF + (3 * (BCDSignalON - BCDSignalOFF) / 7); // adjust the threshold based on the last seconds ON/OFF dBm values
                    WWVThreshold = WWVThreshold + ((CarrierSignal - CarrierSignalINIT) / 3); // adjust the threshold based on the last seconds Carrier dBm values


                } // if (WWVPitch == false)  // signal strength based detected
                else // WWVPitch == true (need around S5 or better to work)
                {

                    if (console.WWVReady == true)
                    {
                        BCDSignal = console.WWVTone;  // get Magnitude value from audio.cs and Goertzel routine

                      //  Debug.WriteLine("WWVTONE: " + ST2.ElapsedMilliseconds + " , "+BCDSignal);

                      //  ST2.Restart();

                        below_count++;   // counter for how many times you got new data and it was below the threshold
                        console.WWVReady = false;

                    }
               
                    //------------------------------------------------------------------
                    // keep adjusting signal based on signal strength you are seeing

                    if (BCDSignal > BCDSignalON1)
                    {
                        BCDSignalON1 = BCDSignal;  // get maximum magnitude
                    }


                    if (ST.ElapsedMilliseconds > 1300)
                    {

                        BCDSignalON = BCDSignalON1;

                        BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
                        BCDCount = 0;

                        ST.Restart();

                        //   Debug.WriteLine("WWV>>  Highest BCD Mag: " + BCDSignalON  );          // adjust the threshold based on the last seconds ON/OFF dBm values

                    }


                    //-------------------------------------------------
                    // check the mag  strength 


                    if (BCDSignalON < 900) // if you loose the carrier, then NO GOOD
                    {
                        if (WWVCF > 800) // FAIL if carrier stays LOW for too long
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Sub-Carrier signal too low, choose different Frequency\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVPos = false;
                            WWVStart = false;
                            WWVStop = false;
                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                            WTime = false;
                        }
                        else WWVCF++;

                    }
                    else
                    {
                        WWVCF = 0;
                    }

                    BCDAdj = 3.0;
                    WWVThreshold = (int)((double)BCDSignalON / BCDAdj);          // 33% of full scale adjust the threshold based on the last seconds ON/OFF dBm values

                } // WWVPitch == true (pitch detection)




                //------------------------------------------------------
                //------------------------------------------------------
                //------------------------------------------------------
                //------------------------------------------------------
                // WWVStart == TRUE when Receive the HOLE signal, but WWVNewTime timer started at end of P0 BCD Tone.
                // SO WWVNewTime timer started 230 milliseconds early AND now receiving the second #1 (always a short BCD Tone), but wont come until second # 2

                if (WWVStart == true)   // do below if got HOLE (start of new minute) WWVNewTime timer is running from 0 second
                {
                  
                    // the extra 230 is for the extra time starting WWVNewTime at the end of P0 to the start of the new Minute Second#0

                    if (WWVNewTime.ElapsedMilliseconds >= ( 230 + (indexS + (indexP*10))*1000 )   )   // 1000,2000,3000,4000,5000 milliseconds, etc
                    {
                        if (ST.IsRunning) ST.Stop();   // turn off init threshold timer

                        if (WWVPitch == false)  // signal strength based detected
                        {
                            CarrierSignalINIT = CarrierSignal;  // set new ON/OFF and Carrier levels at the start of every second
                            BCDSignalOFF = BCDSignalOFF1;
                            BCDSignalON = BCDSignalON1;

                            BCDSignalON1 = -150;         // RESET BCD data steam high dbm signal found while running
                            BCDSignalOFF1 = 0;           // RESET BCD data steam low dbm signal found while running
                        }
                        else // tone detection here
                        {

                            BCDSignalON = BCDSignalON1;  // tone should be on here always since its the start of every second (tick) (we just need to know how long it lasts)
                            BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
                        }



                        if (indexS == 10) // 9, 19,29,39, etc
                        {
                            BCD1 = 0;  // reset value

                            indexP++;
                            textBox1.Text += " P"+(indexP)+">";
                            indexS = 1;
                        }
                        else // first 9 seconds of every P frame
                        {

                            if (BCD1 > 1)   // BCD tone was med ( 440 msec)
                            {
                                BCD1 = 0;  // reset value
     
                                P[indexP, indexS-1] = 1;
                                textBox1.Text += "1";
                                
                            }
                            else // BCD tone was short ( 170 msec)
                            {
                                BCD1 = 0;  // reset value

                                P[indexP, indexS-1] = 0;
                                textBox1.Text += "0";
                            }

                            indexS++; // index the second marker

                        }  // first 9 seconds of every P frame

                        tickON.Restart();  // we no we will get a BCD tone, but for how long: 170msec= 0, 440msec = 1, or 770msec if it a P frame

                   } // if (WWVNewTime.ElapsedMilliseconds >= ( 230 + (indexS + (indexP*10))*1000 )   ) 


                    //--------------------------------------------------------------
                    //--------------------------------------------------------------
                    //--------------------------------------------------------------


                   // if (indexP == 5)
                    if ((indexP == 4) && (indexS > 3))
                    {

                        if ((WWVFault[1] == true) || (WWVFault[2] == true)||(WWVFault[3] == true) || (WWVFault[4] == true))
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Data No Good, will Try again.\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVCF = 0;
                            BCDAdj = 3;
                            WWVPos = false;
                            WWVStart = false;
                            WWVStop = false;
                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                            WWVNewTime.Stop();

                            // DO OVER AGAIN UNTIL YOU GET GOOD DATA
                        } // fault detected in PCM BCD data stream

                        else // no faults detected in PCM BCD data stream
                        {


                            newMinutes = (P[1, 0] * 1) + (P[1, 1] * 2) + (P[1, 2] * 4) + (P[1, 3] * 8) + (P[1, 5] * 10) + (P[1, 6] * 20) + (P[1, 7] * 40);     // WWV reported UTC minutes
                            newHours = (P[2, 0] * 1) + (P[2, 1] * 2) + (P[2, 2] * 4) + (P[2, 3] * 8) + (P[2, 5] * 10) + (P[2, 6] * 20);                        // WWV reported UTC hour

                            newDay1 = (P[3, 0] * 1) + (P[3, 1] * 2) + (P[3, 2] * 4) + (P[3, 3] * 8) + (P[3, 5] * 10) + (P[3, 6] * 20) + (P[3, 7] * 40) + (P[3, 8] * 80);
                            newDay2 = (P[4, 0] * 100) + (P[4, 1] * 200);

                            newDay = newDay1 + newDay2;                                                                                                        // WWV reported UTC day of the year


                            Debug.WriteLine("UTC Hours: " + newHours);
                            Debug.WriteLine("UTC Min: " + newMinutes);
                            Debug.WriteLine("UTC Day of year: " + newDay);

                            WWVStop = true;
                            WTime = false;  // DONE
                            WWVPos = false;

                            string ww1 = newHours.ToString("D2") + ":" + newMinutes.ToString("D2") + ":00.000";
                            DateTime WWVUTC = new DateTime();
                            DateTime theDate = new DateTime();
                            DateTime theTime = new DateTime();

                            try
                            {
                                int year = DateTime.UtcNow.Year;                                      // current UTC year that your PC is reporting
                                theDate = new DateTime(year, 1, 1).AddDays(newDay - 1);      // this is the current Date based on your PC year and WWV UTC day of the year.

                              //  theDate = theDate.Date;
                                theTime = DateTime.ParseExact(ww1, "HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);

                              //  DateTime date = Convert.ToDateTime(txtTrainDate.Text);
                              //  DateTime time = Convert.ToDateTime();

                                WWVUTC = new DateTime(theDate.Year, theDate.Month, theDate.Day, theTime.Hour, theTime.Minute, theTime.Second);

                            }
                            catch(Exception)
                            {
                                textBox1.Text += "WWV time not read correctly. Will Try again.\r\n";
                                WWVNewTime.Stop();
                                indexP = 0;     // reset data to catch next minute data stream
                                indexS = 0;
                                BCDAdj = 3;
                                WWVCF = 0;
                                WWVPos = false;
                                WWVStart = false;
                                WWVStop = false;
                                WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                                goto EXITOUT;
                            }

                            textBox1.Text += "\r\n";
                            textBox1.Text += "DONE: Radio Station WWV UTC Time: " + ww1 + "\r\n";

                            //-------------------------------------
                            // now computer real time and save it.
                            textBox1.Text += "IMPORTANT: Your PC Time will NOT update unless PowerSDR is launched in ADMIN mode!!!!" + "\r\n";

                           
                            DateTime startDT = DateTime.Now;   // get current PC time and date
 

                            DialogResult temp0 = MessageBox.Show("You must be running in ADMIN mode to set your PC Clock.\r\nYour Current LOCAL Date time: " + startDT.ToString("yy-MM-dd HH:mm:ss.fff") +
                                "\r\nDoes this UTC Time (below) look Correct?\r\nDo You Want to Update Your PC Clock?\r\n" +
                                "This is the Decoded WWV UTC Time > " + WWVUTC.ToString("yy-MM-dd HH:mm:ss.fff") + "\r\n An additional correction factor will be added if you select YES",
                                "WWV PC TIME UPDATE", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                            if (temp0 == DialogResult.Yes)
                            {
                                // update PC time here

                                textBox1.Text += "Elapsed time since WWV Sync Pulse> " + WWVNewTime.Elapsed + "\r\n";

                                k1 = TimeSpan.FromMilliseconds(Convert.ToInt32(230)); // convert o k1 timespan

                                WWVUTC = WWVUTC.Subtract(k1); // subtract the 230 millseconds from the HOLE
                                WWVUTC = WWVUTC + WWVNewTime.Elapsed;                                            // WWVNewTime actually started at the end of the P0 pulse which is actually 230msec before 0

                                textBox1.Text += "New UTC Time updated > " + WWVUTC + "\r\n";

                                WWVUTC = WWVUTC.ToLocalTime(); // adjust UTC back to my time .NOW

                                st.FromDateTime(WWVUTC); //Convert System.DateTime to SystemTime 
                                Win32API.SetLocalTime(ref st);  //Call Win32 API to set the system time  

                                textBox1.Text += "New Local Time updated to your PC clock> " + WWVUTC + "\r\n";
                                WWVNewTime.Stop();

                            } // user wants you to update PC time based on WWV radio results
                            else
                            {
                                // do not update PC time
                                textBox1.Text += "OK. PC Time Clock NOT Updated.\r\n";
                                WWVNewTime.Stop();

                            }


                        } // NO faults detected in PCM BCD data stream

                    } // indexP == 3 (got the hours and minutes)



                } //if (WWVStart == true) 

                EXITOUT:


                //---------------------------------------------------------------
                //---------------------------------------------------------------
                // use Threshold to check 100hz subcarrier for BCD data stream
                //---------------------------------------------------------------
                //---------------------------------------------------------------

                if (WWVStart == true) // this means we are trying to receive the time code now (we received the Start HOLE)
                {
                    if (BCDSignal >= WWVThreshold)             // this should be a 1 in the BCD data stream (or a P Frame signal)
                    {

                        // we already know the BCD tone is already ON
                        checkBoxTone.Checked = true;
                        BCDMax = BCDSignal;
                        below_count = 0;

                    }
                    else
                    {
                        tickTimeON = (int)tickON.ElapsedMilliseconds;

                        if (below_count > 2)
                        {
                            BCDMin = BCDSignal;

                            checkBoxTone.Checked = false;

                            if ((int)tickON.ElapsedMilliseconds < 60)
                            {
                                // keep going because the min BCD length is 170msec, we must have the threshold too high or lost signal in the noise

                            }
                            else
                            {
                                BCDONTrig = true; // NOW WE ARE SURE WE GOT THE FULL TICK LENGTH 
                                tickON.Stop();
                                //  tickTimeON = (int)tickON.ElapsedMilliseconds;

                            }
                        } // below_count > 2

                    }
                        //-----------------------------------------------------------
                    if (BCDONTrig == true) // got the Tick ON time
                    {
                        BCDONTrig = false; // do just 1 time per tick


                        if ((tickTimeON > 280) && (tickTimeON < 660))  // .440 seconds duration: weighted code digit (1)
                        {
                            WWVPos = false;

                            if (WWVStart == true)
                            {
                                BCD1 = 2;
                            }

                        }
                        else if ((tickTimeON > 60) && (tickTimeON <= 280))  // .170 seconds duration: (0) unweighted code digit, index marker, and unweighted control element
                        {
                            WWVPos = false;
                            if (WWVStart == true)
                            {
                                BCD1 = 1;
                            }
                        }
                        else // you have already found the Sync Hole, but this last tone too short
                        {
                            WWVPos = false;
                        
                            if (WWVPitch == true)
                            {
                                BCDSignalON = BCDSignalON1;
                                BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
                            }
                            else
                            {
                                CarrierSignalINIT = CarrierSignal;  // set new ON/OFF and Carrier levels at the start of every second

                                BCDSignalON = BCDSignalON1;
                                BCDSignalOFF = BCDSignalOFF1;
                                BCDSignalON1 = -150;
                                BCDSignalOFF1 = 0;
                            }

                            if (WWVStart == true)
                            {
                                WWVFault[indexP] = false;
                                //  textBox1.Text += "F";
                            }
                        }


                    } // BCDONTrig == true


                } //  if (WWVStart == true)
                else   // do below only to find HOLE(SYNC) to start minute
                {
                    if (BCDSignal >= WWVThreshold)             // this should be a 1 in the BCD data stream (or a P Frame signal)
                    {
                        checkBoxTone.Checked = true;
                        below_count = 0;
                        BCDMax = BCDSignal;

                        tickOFF.Stop();

                        if (BCD1timeFlag == false)
                        {
                            tickTimeOFF = (int)tickOFF.ElapsedMilliseconds; // 

                            BCDOFFTrig = true; // got an TICK TIMEOFF
                            BCDONTrig = false; // reset ON timer

                            BCD1timeFlag = true; // 1 time flag

                            tickON.Start();   // first time here, start timer to see how long the BCD tone lasts

                            tickOFF.Reset();  // we are no longer looking at an off signal

                        }
                    }
                    else  // no TONE  this should be a 0 in the BCD data stream
                    {
                        
                        if (below_count > 1)  // dont allow drop below threshold unless its for 3 times
                        {
                            checkBoxTone.Checked = false;
                            BCDMin = BCDSignal;

                            tickON.Stop();

                            if (BCD1timeFlag == true)
                            {
                                tickTimeON = (int)tickON.ElapsedMilliseconds;
                                BCD1timeFlag = false;

                                textBox2.Text = tickTimeON.ToString();

                                BCDOFFTrig = false;
                                BCDONTrig = true;     // GOT A TICK TIMEON 

                                tickON.Reset();
                                tickOFF.Start();


                                if (tickTimeON < 60)
                                {
                                    if (WWVPitch == true)
                                    {
                                        BCDSignalON = BCDSignalON1;
                                        BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
                                    }
                                    else
                                    {
                                        CarrierSignalINIT = CarrierSignal;  // set new ON/OFF and Carrier levels at the start of every second
                                        BCDSignalON = BCDSignalON1;
                                        BCDSignalOFF = BCDSignalOFF1;
                                        BCDSignalON1 = -150;
                                        BCDSignalOFF1 = 0;
                                    }
                                }
                                else if (tickTimeON > 1000)
                                {
                                  
                                    if (WWVPitch == true)
                                    {
                                        BCDSignalON = BCDSignalON1;
                                        BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
                                    }
                                    else
                                    {
                                        CarrierSignalINIT = CarrierSignal;  // set new ON/OFF and Carrier levels at the start of every second
                                        BCDSignalON = BCDSignalON1;
                                        BCDSignalOFF = BCDSignalOFF1;
                                        BCDSignalON1 = -150;
                                        BCDSignalOFF1 = 0;
                                    }
                                }

                            }
                        } // below_count > 2

                    } //if (BCDSignal >= WWVThreshold) NO

                    //-------------------------------------------------------------------------
                    if (BCDONTrig == true)
                    {
                        BCDONTrig = false; // false = dont check anymore until the next second

                        if ((tickTimeON >= 660) && (tickTimeON < 900))       // .770 seconds duration: position identifier (P0-P5)  (i.e. every 10 seconds)
                        {
                            if (WWVStart == false)
                            {
                                WWVPos = true; // allow detected of HOLE
                                WWVNewTime.Restart(); // zero and Start the stopwatch just in case this is actually the sync pulse (i.e. the HOLE is next)

                                textBox1.Text += "Pframe,";
                            }

                        } // P Frame above

                    } //  if (BCDONTrig == true)

                    //-----------------------------------------------------------------------
                    // LOOK FOR HOLE TO START WWV MINUTE
                    if (BCDOFFTrig == true)
                    {
                        BCDOFFTrig = false;

                        textBox2.Text = tickTimeON.ToString();

                        if ((WWVPos == true)  && (tickTimeOFF > 1010) && (tickTimeOFF < 1700))  // Long HOLE indicates start of new minute
                        {

                            Debug.WriteLine("WWV TIME>> Position HOLE: Start of new MINUTE============");

                        
                            textBox1.Text += " Got Start of new Minute Sync Pulse\r\n";
                            textBox1.Text += "Frame (1-6)#: P0>";

                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;

                            WWVStart = true;
                            indexP = 0;
                            indexS = 2; // this next tick will be second # 1 of the P0 Frame
                            BCD1 = 0;
                            P[indexP, 0] = 0;
                            textBox1.Text += "H";
                        }
                        else
                        {
                            if ((WWVPos == true) )  // Long HOLE indicates start of new minute
                            {
                                if (WWVPitch == true)
                                {
                                    textBox1.Text += " >> ON:" + tickTimeON + "mSec @ " + BCDMax + " Mag, OFF:" + tickTimeOFF + "mSec, @ " + BCDMin + " Mag\r\n";
                                }
                                else
                                {
                                    textBox1.Text += " >> ON:" + tickTimeON + "mSec @ " + BCDMax + " dBm, OFF:" + tickTimeOFF + "mSec, @ " + BCDMin + " dBm\r\n";
                                }
                            }
                            //  Debug.WriteLine("WWV TIME>> Position HOLE?: " + tickTimeOFF);
                            WWVPos = false;
                        }

                    } // BCDONTrig == false

                }  // WWVStart == false

                Thread.Sleep(1);

            } // while(WTime == true)


            //---------------------------------------------------------------
            //---------------------------------------------------------------
            // DONE WITH WWV THREAD HERE
            //---------------------------------------------------------------
            //---------------------------------------------------------------

            Debug.WriteLine("WWV Time Thread Ended ");


            checkBoxWWV.Checked = false; // turn off WWV checking

         
                if (oldSR == 192000)  // 192kSR will not work so reduce to 96k
                {
                    console.setupForm.comboAudioSampleRate1.Text = "192000"; // select 192000 again when done

                }

                console.chkEnableMultiRX.Checked = false;  // enable sub receiver

         
            textBox2.Text = "";
            checkBoxTone.Checked = false;   // turn off tone marker when done.

            //---------------------------------------------------------------
            console.RX1Filter = beacon89a;           // restore filter back to original for this mode

            console.UpdateRX1Filters(beacon9,beacon8); // restore filter
          
            console.RX1DSPMode = beacon7;           //  restore  mode  when you turn off the beacon check
            console.RX1Filter = beacon89;           // restore filter name

            console.BlockSize1 = beacon66;          // get blocksize (must be 2048 during wwv bcd read)
            console.VFOAFreq = beacon88;             // restore VfoA

            console.RX1PreampMode = beacon44; // restore preamp


            if (beacon33 != 4096)
            {
                console.DSPBufPhoneRX = beacon33;
            }

            //  console.CATPreamp = beacon55;

            console.UpdateDisplay();

            WWVNewTime.Stop();

        } // WWVTime()

        private void checkBoxWWV_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWWV.Checked == false)
            {
                WTime = false; // if you turn off checkbox, then shut down thread
                WWVNewTime.Stop();
                indexP = 0;     // reset data to catch next minute data stream
                indexS = 0;
               
            }

        }

        private void numericUpDownTS1_ValueChanged(object sender, EventArgs e)
        {
         //   beacon11 = 0; // reset when changing

        }




      //  double sPrev = 0.0;
      //  double sPrev2 = 0.0;

        double normalizedfreq = 0.0;
        public double Coeff = 0.0;

        /*
        //================================================================================================
        // ke9ns add to detect single Frequecy tones in a data stream
        public int GoertzelFilter(float[] samples, int start, int end)
        {
            sPrev = 0.0;
            sPrev2 = 0.0;
    
            for (int i = start; i < end; i++)   // feedback
            {
                double s = samples[i] + Coeff * sPrev - sPrev2;
                sPrev2 = sPrev;
                sPrev = s;
            }

            double power = (sPrev2 * sPrev2) + (sPrev * sPrev) - ((Coeff * sPrev) * sPrev2);  // feedforward

            return (int)power; // magnitude of frequency in question within the stream
        }

*/


        //========================================================================================
        public void GoertzelCoef( double freq, int SIGNAL_SAMPLE_RATE)
        {
   
            normalizedfreq = freq / SIGNAL_SAMPLE_RATE;
            Coeff = 2 * Math.Cos(2 * Math.PI * normalizedfreq);
        }

        private void checkBoxTone_CheckedChanged(object sender, EventArgs e)
        {

        }


        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        // F10.7 = 63.74 + 0.727*SSNf + 0.000895*SSNf**2 
        // SSNf = ((93918.4 + 1117.3 * SFI) ^ .5) - 416.37    this is the true SSN value since the optical SSN# wont have an effect on earth for up to 3 days (the time it takes for the suns matter to reach earth)

        // https://spawx.nwra.com/spawx/env_latest.html
        // ftp://ftp.ngdc.noaa.gov/STP/GEOMAGNETIC_DATA/INDICES/KP_AP/2016
        // Values of Kp Indices, Ap Indices, Cp Indices, C9 Indices, Sunspot Number, and 10.7 cm Flux 
        // 1610302499254033302337333020247 27 18 15  9 22 18 15  7 160.94---075.10
        // 16103124992627302323132017 7160 12 15  9  9  5  7  6  3  80.42---075.50

        //==========================================================================
        // ke9ns check if VOACAP map needs an update (i.e. you changed location, or band, etc)

        public bool VOARUN = false;   // true = VOACAP() routine running, false = ok to run VOACAP()

        int SSNf = 0;   // effective SSN
        public bool VOACAP_FORCE = false;  // true = using the VOACAP options window to set variables, false = use dx spotter window to set variables

        public void VOACAP_CHECK()
        {

            Debug.WriteLine("VOA_CHECK: " + VOARUN);


            if (VOARUN == true)
            {
                    return; // dont do a VOACAP update if your aleady doing one.
            }


VOACHECK_TOP:

            // int MHZ1 = (int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")); // get current freq value in hz, so convert to mhz

            int MHZ1 = console.last_MHZ;

            //Debug.WriteLine("MHZ1    : " + MHZ1);

            if ((MHZ1 > 29) || (MHZ1 < 1))
            {
                checkBoxMUF.Checked = false; // turn off voacap if you try to use it above 29mhz or below 1 mhz

                Map_Last = Map_Last | 2;    // force update of world map

                return; // dont do a propagation map unless less than 30mhz
            }

            if (VOACAP_FORCE == false)  // check if using the options panel
            {
                if ((Console.SSNE > 0) && (Console.SSNE < 250))
                {

                    statusBoxSWL.Text = "NWRA SSNe = " + Console.SSNE;
                    SSNf = Console.SSNE;

                }
                else
                {
                    if (Console.SFI == 0)
                    {
                        SSNf = 30;  // if you dont have space weather enabled, just use 30 for now
                    }
                    else
                    {
                        SSNf = (int)(Math.Pow((93918.4 + 1225.0 * (double)Console.SFI), 0.5) - 416.0); // convert SFI to SSN
                    }

                    statusBoxSWL.Text = "Using SSNf = " + SSNf;
                }

            }
            else
            {
                SSNf = (int)SpotOptions.udSSN.Value; // from options screen
                statusBoxSWL.Text = "Cstm SSNf = " + SSNf;
            }

            if (SSNf < 0) SSNf = 0;
            if (SSNf > 250) SSNf = 0;


            VOALAT = udDisplayLat.Value.ToString("##0.00").PadLeft(6);   // -90.00
            VOALNG = udDisplayLong.Value.ToString("###0.00").PadLeft(7);  // -180.00 
            MONTH = DateTime.UtcNow.Month.ToString().PadLeft(2);  // 00

            if (VOACAP_FORCE == false)  // check if using the options panel";
            {
             //   DAY = "00";
                DAY = DateTime.UtcNow.Day.ToString("00");  // 00       this forces voacap to use URSI 88 parameters which is not good, so keep day set to 00
            }
            else
            {
                // DAY = DateTime.UtcNow.Day.ToString("00");  // 00       this forces voacap to use URSI 88 parameters which is not good, so keep day set to 00
                DAY = ((int)SpotOptions.udDAY.Value).ToString("00");

            }


            HOUR = DateTime.UtcNow.Hour.ToString().PadLeft(2);  // 00

            MHZ = (MHZ1).ToString().PadLeft(2);   // 00

            double wattage = 0;

            if (VOACAP_FORCE == false)  // check if using the options panel
            {
                wattage = (double)tbPanPower.Value / 1000.0;  // get slider wattage info
            }
            else
            {
                wattage = (double)SpotOptions.udWATTS.Value / 1000.0;
            }

/*
            if ((Console.Kindex > 4) || (Console.RadioBlackout.Contains("R") == true) || (Console.GeoBlackout.Contains("G") == true))               // power reduction based on Kindex rise or radio blackouts
            {
                SSNf = (int)((double)SSNf * 0.6);

                statusBoxSWL.Text += "L";
            }
            else if (Console.Kindex > 2)
            {
                SSNf = (int)((double)SSNf * 0.8);

                statusBoxSWL.Text += "M";
            }
            else
            {
                statusBoxSWL.Text += "H";
            }

*/



            SSN = SSNf.ToString("##0").PadLeft(3);   // "{0,2}"

            WATTS = wattage.ToString("0.0000").PadLeft(6); // 0.0000


            if (
                (!console.MOX) && ((chkBoxAnt.Checked != Last_Ant) || (Last_SSN != SSN) || (Last_VOALAT != VOALAT) || (Last_VOALNG != VOALNG) ||
                (Last_MHZ != MHZ) || (Last_MONTH != MONTH) || (Last_DAY != DAY) || (Last_HOUR != HOUR) || (Last_WATTS != WATTS) ||
                (Last_MODE != console.RX1DSPMode) || (VOACAP_FORCE == true))
                )
            {

                VOARUN = true;                     // dont allow this to trigger until its finished

                Thread t = new Thread(new ThreadStart(VOACAP));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "VOACAP Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

            
            }


        } // update MUF map



        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        public int VOA_Index = 0;
        public int SP33_Active = 0;
        public int SP11_Active = 1;
        public int SP00_Active = 0;

        public static double[] VOA_LAT = new double[10000];    // ke9ns storage for VOACAP VG1 MUF data
        public static double[] VOA_LNG = new double[10000];
        public static double[] VOA_MUF = new double[10000];
        public static string[] VOA_MODE = new string[10000];
        public static double[] VOA_DBU = new double[10000];
        public static double[] VOA_SDBW = new double[10000];
        public static double[] VOA_SNR = new double[10000];
        public static int[] VOA_Color = new int[10000];

        public static int[] VOA_S = new int[10000];   // each point on map is converted to an S meter reading
        public static int[] VOA_S1 = new int[100];    // VOA_S1[1] = how many S1 readings in the map total, [2] = S2 readings 


        public static int[,] VOA_SS = new int[3000,3000];   // each point on map is converted to an S meter reading
        public static int[] VOA_SY = new int[3000];
        public static int[] VOA_SX = new int[3000];


        public static int[,] VOA_X = new int[10,1000];    // conversion of MUF LAT & LONG to X Y points  (VOA_S1[h] = number of each S unit found in map, h=S unit your checking)
        public static int[,] VOA_Y = new int[10,1000];     // [][] = S unit S0 to S9 then list of x,y points that fall under that S reading

      
        public static int[,] VOA_X1 = new int[10, 1000];    // PLOT routine dumps data here
        public static int[,] VOA_Y1 = new int[10, 1000];     //

        public int VOA_MyY = 0; // x y location of your station transmitter based on input lat and long
        public int VOA_MyX = 0;

        public int VOA_YLast = 0; // x y location of your station transmitter based on input lat and long
        public int VOA_XLast = 0;

        public static int VOA_Xsize = 31;
        public static int VOA_Ysize = 31;

        public static float max1 = -900;
        public static float min1 = 200;


        string VOALAT = "0.00";         // 5 digits (Right justified) your own location
        string VOALNG = "0.00";         // 6 digits     "             your own location
        string MHZ = "14";              // 2 digits     "             current band your on
        string MONTH = "11";            // 2 digits     "             current date
        string DAY = "00";
        string HOUR = "00";             // 2 digits     "              static
        string SSN = " 36";             // 3 digits     "             current sun spot #
        string WATTS = "0.5000";        // 6 digits     "              static


        string Last_VOALAT;  // used to see if you need to update the voacap map or not
        string Last_VOALNG;
        string Last_MHZ;
        string Last_SSN;
        string Last_MONTH;
        string Last_DAY;
        string Last_HOUR;
        string Last_WATTS;
        bool Last_Ant;
        DSPMode Last_MODE;
        int CR = 73;     // CR is the signal to noise level needed for contact


        // INPUT VOACAP variable to conrec.cs contour mapping
        private double[,] d1 = new double[31, 31]; // for use with conrec.cs program
        private double[] x1 = new double[31];  // each x axis location 
        private double[] y1 = new double[31];  // each y axis location 
        private double[] z1 = new double[13];  // number of contours for the map

        // OUTPUT VOACAP variable from conrec.cs contour mapping
        public static float[] x3 = new float[10000]; // storage for contour.cs map
        public static float[] x4 = new float[10000];
        public static float[] y3 = new float[10000];
        public static float[] y4 = new float[10000];
        public static float[] S = new float[10000];

        public static int cnt = 0;  // counter for the conrec.cs routine (how many lines to draw for the contour map)
       
        //=======================================================================================
        //=======================================================================================
        //ke9ns start voacap propagation spotting THREAD 1 and done
        private void VOACAP()
        {
            VOARUN = true; // dont allow this to trigger until its finished
            
            string file_name = " ";
            string file_name1 = " ";
           
         
            //-------------------------------------- create a ke9ns.voa file from your lat,long,callsign, date,time, ssn and band your on


            Thread.Sleep(10);

            Last_MHZ = MHZ;
            Last_VOALAT = VOALAT;
            Last_VOALNG = VOALNG;
            Last_MONTH = MONTH;
            Last_DAY = DAY;
            Last_HOUR = HOUR;
            Last_Ant = chkBoxAnt.Checked;
            Last_WATTS = WATTS;
            Last_MODE = console.RX1DSPMode;

          
            string[] VOA = new string[20];

            string VOA1 = "";

            string CRS = "73";    // SNR dbm required for contact (low for cw, high for AM)
            string REL = "90";   // reliablity % 
            string ANGLE = "0.100";
            string METHOD = "22"; // was 30
            string COEFF = "CCRI";  // CCRI if DAY = 00  or URSI if DAY > 0


            if (VOACAP_FORCE == false)
            {
                if ((Last_MODE == DSPMode.AM) || (Last_MODE == DSPMode.SAM) || (Last_MODE == DSPMode.FM)) CR = 75;
                else if ((Last_MODE == DSPMode.CWL) || (Last_MODE == DSPMode.CWU)) CR = 45;
                else if ((Last_MODE == DSPMode.DIGL) || (Last_MODE == DSPMode.DIGU)) CR = 52;
                else if ((Last_MODE == DSPMode.USB) || (Last_MODE == DSPMode.LSB) || (Last_MODE == DSPMode.DSB)) CR = 70;
                else CR = 70;

            }
            else
            {
                CR = (int)SpotOptions.udSNR.Value;  // signal to noise ratio in db
            }

            CRS = CR.ToString("00").PadLeft(2);

            Debug.WriteLine("CRS: " + CRS);

            if (VOACAP_FORCE == false)
            {
                REL = "90";
            }
            else
            {
                REL = ((int)SpotOptions.udRCR.Value).ToString("00");  // Required circuit reliability
            }

            Debug.WriteLine("REL: " + REL);

            if (VOACAP_FORCE == false)
            {
                ANGLE = "1.000"; // was 3.000
            }
            else
            {
                ANGLE = ((double)SpotOptions.udMTA.Value).ToString("0.000");   // min takeoff angle
            }
            Debug.WriteLine("ANGLE: " + ANGLE);

            if (VOACAP_FORCE == false)
            {
               METHOD = "22"; // was 30
            }
            else
            {
                METHOD = ((int)SpotOptions.udMethod.Value).ToString("00");  // Method used
            }
            Debug.WriteLine("METHOD: " + METHOD);

            if (VOACAP_FORCE == false)
            {
              //  COEFF = "CCRI";
                COEFF = "URSI";
            }
            else
            {
                if ((int)SpotOptions.udDAY.Value > 0)  COEFF = "URSI";
                else COEFF = "CCRI";
            }
            Debug.WriteLine("COEFF: " + COEFF);


            // ftp://ftp.ngdc.noaa.gov/STP/space-weather/solar-data/solar-indices/sunspot-numbers/predicted/table_international-sunspot-numbers_monthly-predicted.txt

            //------------------------------------------------
            // ke9ns.voa file
             VOA[0] = "Model    :VOACAP\r\n";
             VOA[1] = "Colors   :Black    :Blue     :Ignore   :Ignore   :Red      :Black with shading\r\n";
             VOA[2] = "Cities   :Receive.cty\r\n";
             VOA[3] = "Nparms   :    1\r\n";
             VOA[4] = "Parameter:SDBW     0\r\n";
             VOA[5] = "Transmit : " + VOALAT + "   " + VOALNG + "   ME                   Short\r\n";          // VOALAT = -00.00N  VOALNG = -000.00W
             VOA[6] = "Pcenter  :  0.00N     0.00E   center\r\n";
             VOA[7] = "Area     :    -180.0     180.0     -90.0      90.0\r\n";
             VOA[8] = "Gridsize :   31    1\r\n";
             VOA[9] = "Method   :   " + METHOD +"\r\n";
            VOA[10] = "Coeffs   :"+ COEFF +"\r\n";
            VOA[11] = "Months   :  " + MONTH + "." + DAY + "   0.00   0.00   0.00   0.00   0.00   0.00   0.00   0.00\r\n";   // MONTH = 00  HOUR = 00
            VOA[12] = "Ssns     :    " + SSN + "      0      0      0      0      0      0      0      0\r\n";               // SSN = 000
            VOA[13] = "Hours    :     "+ HOUR + "      0      0      0      0      0      0      0      0\r\n";
            VOA[14] = "Freqs    : " + MHZ + ".000  0.000  0.000  0.000  0.000  0.000  0.000  0.000  0.000\r\n";            // MHZ = 00

        //  VOA[15] = "System   :  145     0.100   90   73     3.000     0.100\r\n"; // this is the standard VOA settings
       //   VOA[15] = "System   :  140     3.000   90   70     3.000     0.100\r\n";  // this is supposed to be the prefered amateur settings
            VOA[15] = "System   :  145     "+ ANGLE +"   " + REL +"   "+ CRS +"     3.000     0.100\r\n"; // this is the standard VOA settings
        //  VOA[15] = Noise dbm, Takeoff Angle, Circuit Reliability, SNR dbm, Multipath Power Tol dbm, Time Delay msec

            VOA[16] = "Fprob    : 1.00 1.00 1.00 0.00\r\n";
            VOA[17] = "Rec Ants :[hamcap  \\Dipole35.N14]  gain=   0.0   0.0\r\n";

            if (Last_Ant == false)
            {
                VOA[18] = "Tx Ants  :[hamcap  \\Dipole35.N14]  0.000  57.0     " + WATTS + "\r\n";                               // WATTS = 0.0000
            }
            else
            {
                VOA[18] = "Tx Ants  :[hamcap  \\3Yagi35.N14 ]  0.000  57.0     " + WATTS + "\r\n";                               // WATTS = 0.0000
            }

            VOA1 = VOA[0] + VOA[1] + VOA[2] + VOA[3] + VOA[4] + VOA[5] + VOA[6] + VOA[7] + VOA[8] + VOA[9] + VOA[10] +
                VOA[11] + VOA[12] + VOA[13] + VOA[14] + VOA[15] + VOA[16] + VOA[17] + VOA[18];

/*
            textBox1.Text = "Method: " + METHOD + "\r\n";
            textBox1.Text += "Coeff: " + COEFF + "\r\n";
            textBox1.Text += "Month: " + MONTH + "\r\n";
            textBox1.Text += "Day: " + DAY + "\r\n";
            textBox1.Text += "Hour: " + HOUR + "\r\n";
            textBox1.Text += "SSN: " + SSN + "\r\n";
            textBox1.Text += "Freq: " + MHZ + "\r\n";
            textBox1.Text += "Mode: " + Last_MODE + "\r\n";
            textBox1.Text += "Angle: " + ANGLE + "\r\n";
            textBox1.Text += "Reliability: " + REL + "\r\n";
            textBox1.Text += "SNR: " + CRS + "\r\n";
            textBox1.Text += "Watts: " + WATTS + "\r\n";
*/
     //       textBox1.Text = "Method: " + METHOD + " Coeff: " + COEFF +  " Month: " + MONTH +  " Day: " + DAY + " Hour: " + HOUR + 
     //    " SSN: " + SSN + " Freq: " + MHZ + " Mode: " + Last_MODE +  " Angle: " + ANGLE +" Rel: " + REL + " SNR: " + CRS + " Watts: " + WATTS + "\r\n";


            //  file_name1 = console.AppDataPath + "ke9ns.voa"; //   

            file_name1 = console.AppDataPath + @"itshfbc\areadata\default\ke9ns.voa"; // voacap data to create table
            Debug.WriteLine("file1: " + file_name1 + " , watts: " + WATTS);

            try
            {
                File.WriteAllText(file_name1, VOA1);
                Debug.WriteLine("NEW VOA FILE CREATED");
            }
            catch(Exception q)
            {
                Debug.WriteLine("NEW VOA FILE NOT CREATED "+q);
                statusBoxSWL.Text = "NEW VOA1 FILE FAIL";

                goto VOACAP_FINISH;

             
            }
     

            //-------------------------------------- create a voacap data table from ke9ns.voa
        
            Debug.WriteLine(" Create a voacap data table from ke9ns.voa");

            string s1 = Environment.CurrentDirectory;
            Debug.WriteLine("s1: " + s1);
            Environment.CurrentDirectory = console.AppDataPath + "itshfbc\\bin_win\\";

            try
            {
             
                string file_name2 = "voacapw.exe";        // c:\itshfbc AREA CALC default\ke9ns.voa"; // voacap data to create table
                Debug.WriteLine("file2: " + file_name2);

                string argument = "SILENT c:.. AREA CALC default\\ke9ns.voa"; // voacap data to create table
                Debug.WriteLine("argument: " + argument);

                var proc1 =  System.Diagnostics.Process.Start(file_name2, argument);

                proc1.WaitForExit(5000); // wait no more than 5 seconds for the file to finish 
            }
            catch (Exception w)
            {
                statusBoxSWL.Text = "NEW VG1 FILE FAIL";
                textBox1.Text = "could not run VOACAPW, and generate a vg1 file: \n" + w;


                Debug.WriteLine("could not run VOACAPW: " + w);
                Environment.CurrentDirectory = s1;

                goto VOACAP_FINISH;

               
            }

            file_name = console.AppDataPath + @"itshfbc\areadata\default\ke9ns.vg1"; // voacap table  data

            int Flt1 = 0;

      
            
RT1:     

         
            Environment.CurrentDirectory = s1;

            //-------------------------------------- ke9ns.vg1 is a voa muf table for your lat/long location

           // file_name = console.AppDataPath + @"itshfbc\areadata\default\ke9ns.vg1"; // voacap table  data
           
            Debug.WriteLine("read ke9ns.vg1 is a voa muf table for your lat/long location");

            if ((File.Exists(file_name)) )
            {
               
                try
                {

                    stream2 = new FileStream(file_name, FileMode.Open); // open file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);

                    Debug.WriteLine("Read voacap data file ke9ns.vg1");

                }
                catch (Exception s)
                {
                    Debug.WriteLine("fault: "+ s);
                    statusBoxSWL.Text = "Read VG1 FILE FAIL";

                    if (Flt1++ > 10)
                    {
                        goto VOACAP_FINISH;
                    }
                    else
                    goto RT1; // try over again 10 time

                }

                var result = new StringBuilder();

               
                VOA_Index = 0; // how big is the ke9ns.vg1 data file in lines
                int Flag24 = 0;
              
                for (int h=0;h < 10;h++)
                {
                    VOA_S1[h] = 0; // reset number of each S unit reading found in the new map
                }

                //------------------------------------------------------------------
                Debug.WriteLine("reading VOACAP VG1 file");

                for (;;)
                {
                 
                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            if (Flag24 == 3)
                            {
                                //0        8        17
                                //  31 31  Latitude Longitude   MUF  MODE ANGLE DELAY VHITE MUFda  LOSS   DBU  SDBW  NDBW   SNR RPWRG   REL MPROB SPROB TGAIN RGAIN SNRxx    DU    DL SIGLW SIGUP PWRCTANGLER
                                //   1  1  -90.0000    0.0000 13.73  F2 E  8.00 50.34 176.7 0.992 188.3 -40.9-168.3-159.9  -8.4  96.7 0.000 0.000 0.000 17.00 -3.20 -23.7  9.66  5.89 11.87  5.06 0.000  6.00

                                try
                                {
                                
                                    VOA_LAT[VOA_Index] = (Convert.ToDouble(result.ToString().Substring(8, 8)));    // get lat reading
                                
                                    VOA_LNG[VOA_Index] = (Convert.ToDouble(result.ToString().Substring(17, 9)));   // get lat reading
                                  
                                    VOA_MUF[VOA_Index] = (Convert.ToDouble(result.ToString().Substring(27, 5)));   // get MUF reading
                                 
                                    VOA_MODE[VOA_Index] = result.ToString().Substring(34, 4);                      // get MODE reading
                                
                                    VOA_DBU[VOA_Index] = (Convert.ToDouble(result.ToString().Substring(68, 6)));   // get DBU reading
                                   
                                    VOA_SDBW[VOA_Index] = (Convert.ToDouble(result.ToString().Substring(74, 6)));  // get SDBW reading
                                  

                                    VOA_SNR[VOA_Index] = (Convert.ToDouble(result.ToString().Substring(86, 6)));  // get SNR reading
                                  

                                    if (VOA_SDBW[VOA_Index] >= -103) VOA_S[VOA_Index] = 9;
                                    else if (VOA_SDBW[VOA_Index] >= -109) VOA_S[VOA_Index] = 8;
                                    else if (VOA_SDBW[VOA_Index] >= -115) VOA_S[VOA_Index] = 7;
                                    else if (VOA_SDBW[VOA_Index] >= -121) VOA_S[VOA_Index] = 6;
                                    else if (VOA_SDBW[VOA_Index] >= -127) VOA_S[VOA_Index] = 5;
                                    else if (VOA_SDBW[VOA_Index] >= -133) VOA_S[VOA_Index] = 4;
                                    else if (VOA_SDBW[VOA_Index] >= -139) VOA_S[VOA_Index] = 3;
                                    else if (VOA_SDBW[VOA_Index] >= -145) VOA_S[VOA_Index] = 2;     // S2 meter reading
                                    else if (VOA_SDBW[VOA_Index] >= -151) VOA_S[VOA_Index] = 1;    // S1
                                    else VOA_S[VOA_Index] = 0;                                    // dead signal


                                    VOA_S1[VOA_S[VOA_Index]]++;  // increment the number of S unit reading in the map

                                     //    Debug.WriteLine("LAT:" + VOA_LAT[VOA_Index] + "  LNG:" + VOA_LNG[VOA_Index] + "  S:" + VOA_S[VOA_Index] + "  MUF:" + VOA_MUF[VOA_Index] + "  SNR:" + VOA_SNR[VOA_Index] + "  Mode:" + VOA_MODE[VOA_Index] + "  DBU:" + VOA_DBU[VOA_Index]+ "  SDBW:" + VOA_SDBW[VOA_Index]);

                                    VOA_Index++;

                                }
                                catch(Exception a)
                                {
                                    Debug.WriteLine("fault> "+ result.ToString());

                                }

                            } // SWL Spots
                            else Flag24++;

                            result = new StringBuilder(); // clean up for next line

                        }
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        VOA_Index--;
                        // textBox1.Text = "End of SWL FILE at "+ SWL_Index1.ToString();
                        // Debug.WriteLine(" SWL2_Freq[SWL2_Index1] " + SWL2_Freq[SWL2_Index1]);
                        break; // done with file
                    }
                    catch (Exception e)
                    {
                         Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        break; // done with file
                    }


                } // for loop until end of file is reached


                // Debug.WriteLine("reached SWL end of file");


                reader2.Close();    // close  file
                stream2.Close();   // close stream


                Debug.WriteLine("Done Reading .VG1 FILE");

                Debug.WriteLine("convert LAT LONG data to X and Y Contour data base on S readings");

                Debug.WriteLine("SSS INDEX LENGTH "+ VOA_Index); // should be 961 or 31 x 31

                for (int h = 0; h < 10; h++)
                {
                    Debug.WriteLine("Found number of S" + h +  " readings: " + VOA_S1[h]);
                }
             

                //-------------------------------------------------------------------



                int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map
                int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                //S9+10dB 160.0 -63 44 
                //S9 50.2 -73 34 
                //S8 25.1 -79 28 
                //S7 12.6 -85 22 
                //S6 6.3 -91 16 
                //S5 3.2 -97 10 
                //S4 1.6 -103 4 
                //S3 0.8 -109 -2 
                //S2 0.4 -115 -8 
                //S1 0.2 -121 -14  -151 for dbW


                //========================================================================
                // ke9ns data for voacap conrec.cs contour map INPUT

               for (int y = 2; y < VOA_Ysize - 2; y++) // latitude (down to up) (-90 to +90)
               {
                    for (int x = 0; x < VOA_Xsize; x++) // long (left to right) -180 to +180
                    {

                        int yy = y * VOA_Ysize;

                        yy = ((VOA_Ysize-1) * VOA_Xsize) - yy; // -1 because 0 is the first index 960 to 0
   
                       y1[y-2]=  (int)(((180 - (VOA_LAT[yy] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude -90S to +90N
                       x1[x] =    (int)(((VOA_LNG[x + (yy)] + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E
                       d1[x,y-2] = (float)VOA_SDBW[x + (yy)];
      

                    } // for x

              } // for y


                z1[0] = -161; // setup contour levels for conrec.cs
                z1[1] = -151;
                z1[2] = -145; // these come back as countours and seperatated by S[]
                z1[3] = -139;
                z1[4] = -133;
                z1[5] = -127;
                z1[6] = -121;
                z1[7] = -115;
                z1[8] = -109;
                z1[9] = -103;

                z1[10] = -93;
                z1[11] = -83;
                z1[12] = -73;  // ke9ns remember these are dBw not dBm readings here

                

                //========================================================================
                // ke9ns data for voacap Signal DOTS map

                            int[,] VOA_Z = new int[1000, 1000];     // 


                            for (int z = 9; z > 0; z--)  // go through each S unit S1 through S9
                            {
                                int q = 0;
                                //  Debug.WriteLine("S reading: " + z + " , value: " + (-127 + (z * 6))+" ,Z: "+VOA_Z[10,10] );

                                VOA_Y[z, q] = VOA_X[z, q] = 0;  // clear out first just in case there is no case of that S reading found in the data

                                for (int y = 0; y < VOA_Ysize; y++) // latitude (down to up) (-90 to +90)
                                {

                                    for (int x = 0; x < VOA_Xsize; x++) // long (left to right) -180 to +180
                                    {

                                        if ((VOA_Z[x, y] == 0))
                                        {

                                            if (VOA_SDBW[x + y * VOA_Ysize] >= (-157 + (z * 6)))  // was -127
                                            {
                                                VOA_Y[z, q] = (int)(((180 - (VOA_LAT[y * VOA_Ysize] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude -90S to +90N
                                                VOA_X[z, q] = (int)(((VOA_LNG[x + (y * VOA_Ysize)] + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E


                                                VOA_Z[x, y] = 1; // this value is now used, dont use it again
                                                q++;

                                                //     Debug.WriteLine("Found: " + z + " , " + q);

                                            }
                                            else
                                            {
                                                VOA_Z[x, y] = 0; // still unused
                                            }

                                        }


                                    } // for x


                                } // for y

                                VOA_Y[z, q] = VOA_X[z, q] = 0;  // clear out last to indicate end of this S reading curve

                            } // for z (S readings)

                            VOA_MyY = (int)(((180 - ((double)udDisplayLat.Value + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                            VOA_MyX = (int)((((double)udDisplayLong.Value + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E


                            Map_Last = Map_Last | 2;    // force update of world map


                        } // if file exists VOA ke9ns.vg1

                        Conrec.Contour(d1, x1, y1, z1); // create contours for map

                        VOARUN = false; // dont allow this to trigger until its finished

                        Debug.WriteLine("SSS8 ");

                        return;  // finished GOOD


     VOACAP_FINISH:      // jumps here if there was a problem      

                        VOARUN = false; // dont allow this to trigger until its finished


         } // VOACAP thread

 

        //================================================================================
        public void checkBoxMUF_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMUF.Checked == false)
            {
           
                Map_Last = Map_Last | 2;    // force update of world map
              
            }
            else
            {
                //  VOACAP_CHECK();
                console.last_MHZ = 0;
                Last_WATTS = "0";
            }

        } // checkBoxMUF_CheckedChanged

        private void chkBoxAnt_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMUF.Checked == true)
            {
                VOACAP_CHECK(); // rescan a new map since your changing your antenna type
            }
        }

        private bool VOACAP_SSN = false;

        //=======================================================================================
        private void checkBoxMUF_MouseDown(object sender, MouseEventArgs e)
        {

            MouseEventArgs me = (MouseEventArgs)e;

            if ((me.Button == System.Windows.Forms.MouseButtons.Right))
            {

                if (VOACAP_SSN == false) VOACAP_SSN = true;
                else VOACAP_SSN = false;


                if (checkBoxMUF.Checked == false)
                {
                    Map_Last = Map_Last | 2;    // force update of world map
                }
                else
                {
                    VOACAP_CHECK();
                }

            }

        } // checkBoxMUF_MouseDown


        //=======================================================================================
        // select the power level used by voacap
        private void tbPanPower_Scroll(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPanPower, "VOACAP: "+ ((int)tbPanPower.Value).ToString() +" watts" );

        }

        private void tbPanPower_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPanPower, "VOACAP: " + ((int)tbPanPower.Value).ToString() + " watts");

        }

      
        private void tbPanPower_MouseUp(object sender, MouseEventArgs e)
        {
            this.toolTip1.SetToolTip(this.tbPanPower, "VOACAP: " + ((int)tbPanPower.Value).ToString() + " watts");

            if (checkBoxMUF.Checked == true)
            {
                
                VOACAP_CHECK(); // rescan a new map since your changing your antenna type
                Debug.WriteLine("RELEASE MOUSE");

            }
        }


        //======================================================================

       public struct Point3F  // x, y, Z=s
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public int S { get; set; }


            public Point3F(float x, float y, float z, int s) : this()
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
                this.S = s;
            }

        }

    
       public Point3F[,] SSS = new Point3F[1000,1000];

        //======================================================================
        //======================================================================
        //======================================================================

        static Pen GryPen = new Pen(Color.FromArgb(255, Color.Gray), 1.0f); // S1-2
        static Pen OrgPen = new Pen(Color.FromArgb(255, Color.Orange), 1.0f); // S3-4
        static Pen YelPen = new Pen(Color.FromArgb(255, Color.Yellow), 1.0f); // S5-6
        static Pen GrnPen = new Pen(Color.FromArgb(255, Color.Green), 1.0f); // S7-8
        static Pen BluPen = new Pen(Color.FromArgb(255, Color.Blue), 1.0f); // S9+

    

        //======================================================================
        private void mnuSpotOptions_Click(object sender, EventArgs e)
        {
            if (SpotOptions == null || SpotOptions.IsDisposed)
                SpotOptions = new SpotOptions();

            SpotOptions.Show();
            SpotOptions.Focus();
        }

        private void chkBoxContour_CheckedChanged(object sender, EventArgs e)
        {
            console.last_MHZ = 0;

            Last_WATTS = "0";
            if (checkBoxMUF.Checked == true)
            {
                VOACAP_CHECK(); // rescan a new map since your changing your antenna type
            }
        } // chkBoxContour_CheckedChanged

        private void statusBoxSWL_Click(object sender, EventArgs e)
        {
            statusBoxSWL.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click

        }


        // ke9ns posts your call sign and VFOA freq to DX cluster
        private void DXPost_Click(object sender, EventArgs e)
        {

            if (SP_Active <= 2)
            {
                textBox1.Text = "Cannot Post a DX spot when your not connected to the DX Cluster";

                Debug.WriteLine("Spotter not ON");
                return;
            }



            string remarks = " ";
            string call = "";

            string freq1 = "00000";
            string freq2 = "00000";



            if ((textBoxDXCall.Text == "DX Callsign")|| (textBoxDXCall.Text == "TRY AGAIN")||(textBoxDXCall.Text == "") || (textBoxDXCall.Text.Length < 3)) // then try to use last dx spot if your on that freq still
            {
                call = DX_Station[DX_SELECTED]; // use current selected call

                if((DX_Freq[DX_SELECTED] == Display.VFOA)) // still on last DX spotter field selected
                {

                    try
                    {
                        freq1 = (console.FREQA * 1000).ToString("#.#");
                    }
                    catch (Exception)
                    {
                        textBoxDXCall.Text = "TRY AGAIN";

                        Debug.WriteLine("cannot parse VFOA");
                        return;
                    }

                    remarks = console.RX1DSPMode.ToString() + "p";  // send DSP MODE with - to indicate PowerSDR was sending it.

                    if (console.chkVFOSplit.Checked == true)
                    {

                        try
                        {
                            freq2 = (console.FREQB * 1000).ToString("#.#"); // in mhz so convert to khz
                            
                        }
                        catch (Exception)
                        {
                            textBoxDXCall.Text = "TRY AGAIN";

                            Debug.WriteLine("cannot parse VFOB");
                            return;

                        }

                        remarks = console.RX1DSPMode + "p qsx:" + freq2;
                    } // in split mode

                }
                else
                {
                    Debug.WriteLine("No longer on freq of last selected DX spotter line");
                    return;
                }




            } // use prior DX spot
            else // otherwise use the current VFOA freq
            {
                call = textBoxDXCall.Text;

                try
                {
                    freq1 = (console.FREQA * 1000).ToString("#.#"); // in hz so convert to khz
                }
                catch (Exception)
                {
                    textBoxDXCall.Text = "TRY AGAIN";

                    Debug.WriteLine("cannot parse VFOA");
                    return;
                }

                remarks = console.RX1DSPMode.ToString()+"p";

                if (console.chkVFOSplit.Checked == true)
                {

                    try
                    {
                        freq2 = (console.FREQB * 1000).ToString("#.#"); // in mhz so convert to khz
                        
                    }
                    catch (Exception)
                    {
                        textBoxDXCall.Text = "TRY AGAIN";

                        Debug.WriteLine("cannot parse VFOB");
                        return;

                    }

                    remarks = console.RX1DSPMode + "p qsx:"+ freq2;
                } // in split mode

            } // just use current VFOA freq (not a prior DX SPOT)


            DialogResult dr = MessageBox.Show("Post this DX Contact to the Cluster (Callout)? \n[YES] Post your Contact.\n[No] Post as SWL Contact.\n[Cancel] will NOT Post Contact.",
               " DX Callout ",
               MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Question);

            if (dr == DialogResult.Cancel) return;

           
            if (dr == DialogResult.No)
            {
                remarks = "SWLp "+ remarks;
            }
           

            char[] messageDXSPOT = ("DX " + freq1 + " " + call + " " + remarks).ToCharArray(); // convert hz to khz
            Debug.WriteLine("CREATE DX SPOT: " + "DX " + freq1 + " " + call + " " + remarks);



            for (int i = 0; i < messageDXSPOT.Length; i++)    // do it this way because telnet server wants slow typing
            {
               SP_writer.Write((char)messageDXSPOT[i]);

            }  // for loop length of your call sign

            SP_writer.Write((char)13);

            SP_writer.Write((char)10);

            textBoxDXCall.Text = "DX Callsign";

                

        } // DXPost_Click

        private void textBoxDXCall_Click(object sender, EventArgs e)
        {
            textBoxDXCall.Text = "";

        }

        private void chkTimeServer1_Click(object sender, EventArgs e)
        {
            chkTimeServer1.Checked = true;
            chkTimeServer2.Checked = false;
            chkTimeServer3.Checked = false;
            chkTimeServer4.Checked = false;
            chkTimeServer5.Checked = false;


        } //chkTimeServer1_Click

        private void chkTimeServer2_Click(object sender, EventArgs e)
        {
            chkTimeServer2.Checked = true;
            chkTimeServer1.Checked = false;
            chkTimeServer3.Checked = false;
            chkTimeServer4.Checked = false;
            chkTimeServer5.Checked = false;
        }

        private void chkTimeServer3_Click(object sender, EventArgs e)
        {
            chkTimeServer3.Checked = true;
            chkTimeServer2.Checked = false;
            chkTimeServer1.Checked = false;
            chkTimeServer4.Checked = false;
            chkTimeServer5.Checked = false;
        }

        private void chkTimeServer4_Click(object sender, EventArgs e)
        {
            chkTimeServer4.Checked = true;
            chkTimeServer2.Checked = false;
            chkTimeServer3.Checked = false;
            chkTimeServer1.Checked = false;
            chkTimeServer5.Checked = false;
        }

        private void chkTimeServer5_Click(object sender, EventArgs e)
        {
            chkTimeServer5.Checked = true;
            chkTimeServer2.Checked = false;
            chkTimeServer3.Checked = false;
            chkTimeServer4.Checked = false;
            chkTimeServer1.Checked = false;
        }

        private void chkMoon_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkMoon.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image
            }
            if (SP_Active != 0)
            {

                if ((chkMoon.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)
                            console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);

                    }
                    else console.picDisplay.BackgroundImage = MAP;

                } // SUN or GRAY LINE checked

            } // dx spot on

            Map_Last = 1;
        } // chkMoon_checkchanged

        public double MODULO(double a, double b)
        {
            return (a - b * Math.Floor(a / b));
        }

        private void chkISS_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkISS.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image
            }
            if (SP_Active != 0)
            {

                if ((chkISS.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)
                            console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);

                    }
                    else console.picDisplay.BackgroundImage = MAP;

                } // SUN or GRAY LINE checked

            } // dx spot on

            Map_Last = 1;





        } // chkISS_CheckedChanged



        //---------------------------------------------------------------
        //---------------------------------------------------------------
        //---------------------------------------------------------------
        //---------------------------------------------------------------
        //---------------------------------------------------------------
        // ke9ns to determine the AZ & elevation angles from on 2 points and elevation distance
        // see http://cosinekitty.com/compass.html

   
        public struct USER_LATLONELV
        {
            public double lat;
            public double lon;
            public double elv;
        }

      
        public struct USER_XYZRN
        {
            public double x;
            public double y;
            public double z;
            public double radius;

            public double nx;
            public double ny;
            public double nz;

        }
 

       

        //============================================================
        double Distance(USER_XYZRN ap, USER_XYZRN bp)
        {
            double dx = ap.x - bp.x;
            double dy = ap.y - bp.y;
            double dz = ap.z - bp.z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);

        } //  double Distance(USER_A ap, USER_B bp)


        //============================================================
        double EarthRadiusInMeters(double latitudeRadians)      // latitude is geodetic, i.e. that reported by GPS
        {
            // http://en.wikipedia.org/wiki/Earth_radius
            double a = 6378137.0;  // equatorial radius in meters
            double b = 6356752.3;  // polar radius in meters
            double cos = Math.Cos(latitudeRadians);
            double sin = Math.Sin(latitudeRadians);
            double t1 = a * a * cos;
            double t2 = b * b * sin;
            double t3 = a * cos;
            double t4 = b * sin;
            return Math.Sqrt((t1 * t1 + t2 * t2) / (t3 * t3 + t4 * t4));

        } //  double EarthRadiusInMeters(double latitudeRadians) 


        //============================================================
        double GeocentricLatitude(double lat)
        {
            // Convert geodetic latitude 'lat' to a geocentric latitude 'clat'.
            // Geodetic latitude is the latitude as given by GPS.
            // Geocentric latitude is the angle measured from center of Earth between a point and the equator.
            // https://en.wikipedia.org/wiki/Latitude#Geocentric_latitude
            double e2 = 0.00669437999014;
            double clat = Math.Atan((1.0 - e2) * Math.Tan(lat));
            return clat;

        } // double GeocentricLatitude(double lat)

        //============================================================
        // INPUT LAT,LONG,ELVATION
        // OUTPUT x,y,z,radius
        USER_XYZRN LocationToPoint(USER_LATLONELV c)
        {
            // Convert (lat, lon, elv) to (x, y, z).
            USER_XYZRN D;

            double lat = c.lat * DEG_to_RAD;
            double lon = c.lon * DEG_to_RAD;

            double radius = EarthRadiusInMeters(lat);
            double clat = GeocentricLatitude(lat);

            double cosLon = Math.Cos(lon);
            double sinLon = Math.Sin(lon);
            double cosLat = Math.Cos(clat);
            double sinLat = Math.Sin(clat);

            double x = radius * cosLon * cosLat;
            double y = radius * sinLon * cosLat;
            double z = radius * sinLat;

            // We used geocentric latitude to calculate (x,y,z) on the Earth's ellipsoid.
            // Now we use geodetic latitude to calculate normal vector from the surface, to correct for elevation.

            double cosGlat = Math.Cos(lat);
            double sinGlat = Math.Sin(lat);

            double nx = cosGlat * cosLon;
            double ny = cosGlat * sinLon;
            double nz = sinGlat;

            x += c.elv * nx;
            y += c.elv * ny;
            z += c.elv * nz;
   
            D.x = x;
            D.y = y;
            D.z = z;
            D.radius = radius;

            D.nx = nx;
            D.ny = ny;
            D.nz = nz;

            return D;      // return { 'x':x, 'y':y, 'z':z, 'radius':radius, 'nx':nx, 'ny':ny, 'nz':nz};


        } //  USER_XYZRN LocationToPoint(USER_LATLONELV c)


        //============================================================
        USER_XYZRN NormalizeVectorDiff(USER_XYZRN b, USER_XYZRN a)
        {
            USER_XYZRN E;

            // Calculate norm(b-a), where norm divides a vector by its length to produce a unit vector.
            double dx = b.x - a.x;
            double dy = b.y - a.y;
            double dz = b.z - a.z;

            double dist2 = dx * dx + dy * dy + dz * dz;

            if (dist2 == 0)
            {
                E.x = 0;
                E.y = 0;
                E.z = 0;
                E.radius = 0;
                E.nx = 0;
                E.ny = 0;
                E.nz = 0;


                return E;
            }

            double dist = Math.Sqrt(dist2);
           
            E.x = dx / dist;
            E.y = dy / dist;
            E.z = dz / dist;
            E.radius = 1.0;

            E.nx = 0;
            E.ny = 0;
            E.nz = 0;

            return E;    //  return { 'x':(dx / dist), 'y':(dy / dist), 'z':(dz / dist), 'radius':1.0 };

        } //  USER_XYZRN NormalizeVectorDiff(USER_B b, USER_A a)



        //============================================================
        USER_XYZRN RotateGlobe(USER_LATLONELV b, USER_LATLONELV a, double bradius, double aradius)
        {

            USER_XYZRN E;

            // Get modified coordinates of 'b' by rotating the globe so that 'a' is at lat=0, lon=0.
          
            USER_LATLONELV br;    // var br = { 'lat':b.lat, 'lon':(b.lon - a.lon), 'elv':b.elv};   


            br.lat = b.lat;
            br.lon = b.lon - a.lon;
            br.elv = b.elv;

            USER_XYZRN brp = LocationToPoint(br);

            // Rotate brp cartesian coordinates around the z-axis by a.lon degrees,
            // then around the y-axis by a.lat degrees.
            // Though we are decreasing by a.lat degrees, as seen above the y-axis,
            // this is a positive (counterclockwise) rotation (if B's longitude is east of A's).
            // However, from this point of view the x-axis is pointing left.
            // So we will look the other way making the x-axis pointing right, the z-axis
            // pointing up, and the rotation treated as negative.

            double alat = -a.lat * DEG_to_RAD;

            alat = GeocentricLatitude(alat);

            double acos = Math.Cos(alat);
            double asin = Math.Sin(alat);

            double bx = (brp.x * acos) - (brp.z * asin);
            double by = brp.y;
            double bz = (brp.x * asin) + (brp.z * acos);
      
            E.x = bx;
            E.y = by;
            E.z = bz;
            E.radius = bradius;
            E.nx = 0;
            E.ny = 0;
            E.nz = 0;

            return E;    //  return {'x':bx, 'y':by, 'z':bz, 'radius':bradius};


        } // USER_XYZRN RotateGlobe(USER_FB b, USER_FA a, double bradius, double aradius)



        //============================================================

        public void Calculate(double iss_lat, double iss_long, double iss_elv, double me_lat, double me_long,double me_elv)
         {
            // parselocation output location = lat, lon, elv

                USER_LATLONELV ISS;
                USER_LATLONELV ME;

                ISS.lat = iss_lat; // input data
                ISS.lon = iss_long;
                ISS.elv = iss_elv;

                ME.lat = me_lat; // input data
                ME.lon = me_long;
                ME.elv = me_elv;

                USER_XYZRN ap = LocationToPoint(ME); // IN= LAT,LON,ELV  OUT= X,Y,Z,Radius
                USER_XYZRN bp = LocationToPoint(ISS);

            
                double distKm = 0.001 * Distance(ap, bp); // ke9ns distance IN=x,y,z only

                ISS_DIST = (int)distKm;

                // Let's use a trick to calculate azimuth:
                // Rotate the globe so that point A looks like latitude 0, longitude 0.
                // We keep the actual radii calculated based on the oblate geoid,
                // but use angles based on subtraction.
                // Point A will be at x=radius, y=0, z=0.
                // Vector difference B-A will have dz = N/S component, dy = E/W component.                


                USER_XYZRN br = RotateGlobe(ISS, ME, bp.radius, ap.radius);

                if ((br.z * br.z + br.y * br.y) > 1.0e-6)
                {
                    double theta = Math.Atan2(br.z, br.y) * RAD_to_DEG;

                    double azimuth = 90.0 - theta;

                    if (azimuth < 0.0)
                    {
                        azimuth += 360.0;
                    }
                    if (azimuth > 360.0)
                    {
                        azimuth -= 360.0;
                    }
               
                    ISS_AZ = (int)azimuth;

                }

                USER_XYZRN bma = NormalizeVectorDiff(bp, ap);

                if ((bma.x != 0) && (bma.y != 0) && (bma.z != 0) && (bma.radius != 0))
                {
                    // Calculate altitude, which is the angle above the horizon of B as seen from A.
                    // Almost always, B will actually be below the horizon, so the altitude will be negative.
                    // The dot product of bma and norm = cos(zenith_angle), and zenith_angle = (90 deg) - altitude.
                    // So altitude = 90 - acos(dotprod).

                    double altitude = 90.0 - RAD_to_DEG * Math.Acos(bma.x * ap.nx + bma.y * ap.ny + bma.z * ap.nz);

                ISS_ALT = (int)altitude;

                   

                } //  if (bma != null)
                    
            } // Calculate(oblate)

        private void textBox3_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            var result = new StringBuilder(Environment.ExpandEnvironmentVariables("%userprofile%"));

            try
            {

                System.Diagnostics.Process.Start(e.LinkText);    // HTTP
            }
            catch
            {
                try
                {
                    var link = e.LinkText.Replace("file://%userprofile%", ""); //file
                    link = link.Replace("%20", " ");

                    result.Append(link);

                    Debug.WriteLine("link2 " + result.ToString());

                    Process.Start("explorer.exe", result.ToString());
                }
                catch
                {

                }
            }

        } // textbox3_linkclicked


        //=========================================
        // for F1 help sceen
        private void SpotControl_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("F1 key0");



            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {
              

                Debug.WriteLine("F1 key");

                if (MouseIsOverControl(checkBoxMUF) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                  
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.PropagationTextBox.Text;
                }
                else if (MouseIsOverControl(btnTrack) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.TRACKMap.Text;
                }



            } // if (e.KeyCode == Keys.F1)




        } // SpotControl_KeyDown


        public bool MouseIsOverControl(Control c)
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }

    } // Spotcontrol

    //============================================================
    // ke9ns used to set PC system time, but PowerSDR needs to be in ADMIn mode for it to take
    public class Win32API
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime Time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime Time);
    }

       
    } // powersdr
