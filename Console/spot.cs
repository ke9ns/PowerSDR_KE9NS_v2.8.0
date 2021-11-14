//=================================================================
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

//  t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
//  t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
//  CultureInfo.InvariantCulture
//  Convert.ToDouble(udDisplayLong.Value, NI)


//using Microsoft.JScript;

using RTF; // allows creating RTF strings just like you use stringbuilder. from Anton Rogue Trader (with RTF you can color the text of the LoTW call signs)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
//using System.Runtime.Serialization.Json;

//reference Nuget Package NAudio.Lame
using System.Net.Sockets;                // ke9ns add for tcpip internet connections
using System.Runtime.InteropServices;
using System.Text;                    // ke9ns add for stringbuilder
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


//using Syncfusion.Windows.Forms.Tools;


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
    public partial class SpotControl : System.Windows.Forms.Form
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

        public static Stream Map_D_Bar = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.bar1.png");     // .237

        public static Stream Map_F_Bar = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.bar2.png");     // .239

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
        public static SpotDecoder SpotDecoderForm;
        public static SpotAge SpotAge;

        public static Conrec conrec;

        public Setup setupForm;                             // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 
        public StackControl StackForm;                      // ke9ns add  communications with spot.cs and stack
        public SwlControl SwlForm;                          // ke9ns add  communications with spot.cs and swl

        public DXMemList dxmemlist;                         //  ke9ns add comm with dx cluster list

        //   public static Display display;

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

        private IContainer components;

        #region Constructor and Destructor


        public SpotControl(Console c)
        {
            Debug.WriteLine("SpotControl(Console c) here ");


            InitializeComponent();
            console = c;

            SpotOptions.SpotForm = this; // allows Spotoptions to see public data 
            SpotDecoder.SpotForm = this; // allows SpotDecoder to see public data 
            SpotAge.SpotForm = this; // 

            Display.SpotForm = this;  // allows Display to see public data (not public static data)
            StackControl.SpotForm = this; // allows Stack to see public data from spot
                                          //  SwlControl.SpotForm = this; // allows swl to see public data from spot

            Common.RestoreForm(this, "SpotForm", true);


            if (SpotAge == null || SpotAge.IsDisposed) SpotAge = new SpotAge();  // create spotage

            SpotAge.Show();
            SpotAge.Hide();

            // ke9ns doublebuffered 
            //  this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true );

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);


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
                console.DXMemList.List.Add(new DXMemRecord("wb8zrl.no-ip.org:7300")); // 
                console.DXMemList.List.Add(new DXMemRecord("ve7cc.net:23"));
                console.DXMemList.List.Add(new DXMemRecord("telnet.reversebeacon.net:7000"));
                console.DXMemList.List.Add(new DXMemRecord("n7od.pentux.net:7300"));
                console.DXMemList.List.Add(new DXMemRecord("dxspots.com:23"));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));


                console.DXMemList.Save1();

                Debug.WriteLine("create DXURL File");

            }

            // see console bottom of init where DX cluster, map, vocap, and swl are preloaded at lanuch

            Darken(); // set the world map brightness


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



        public NumberFormatInfo NI = CultureInfo.InvariantCulture.NumberFormat; // get info on culture use of , or . for floating point numbers


        public void SpotControl_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("SpotControl_Load here");


            if (console.RX2Band.ToString() == "GEN") // .157
            {
                hkBoxSpotRX2.Checked = false;
                hkBoxSpotRX2.Enabled = false;

            }



            Debug.WriteLine("retrieved the index from storage");

            nameBox.Text = nameB;
            callBox.Text = callB;
            nodeBox1.Text = nodeB;
            portBox2.Text = portB;

            try
            {
                if (Convert.ToInt16(portBox2.Text) < 20)
                {
                    dataGridView1.CurrentCell = dataGridView1[0, Convert.ToInt16(portBox2.Text)];
                    Debug.WriteLine("retrieved the index from storage1");

                }
            }
            catch (Exception)
            {
                dataGridView1.CurrentCell = dataGridView1[0, 0];

            }

            if (console.ROTOREnabled == true)
            {
                RotorHead.Visible = true;
                button3.Visible = true;
                numBeamHeading.Visible = true;

                string answer = console.spotDDUtil_Rotor1; // get rotor angle current position

                            RotorHead.Text = answer + "°";

                int temp1 = 0;

                try
                {
                    temp1 = Convert.ToInt16(answer);
                }
                catch
                {
                    temp1 = 0;
                }

                numBeamHeading.Value = temp1;

                RotorUpdate(); // start thread to update position of rotor on Spot screen

                
            }
            else
            {
                RotorHead.Visible = false;
                button3.Visible = false;
                numBeamHeading.Visible = false;
            }

            for (int x = 0; x < 500; x++)
            {
                DX_LoTW_RTF[x] = new RTFBuilder(RTFFont.CourierNew, 18f);
                DX_LoTW_Status[x] = 0; // .201

                DXt_LoTW_RTF[x] = new RTFBuilder(RTFFont.CourierNew, 18f); // .232
                DXt_LoTW_Status[x] = 0; // .232
            }

           


        } // SpotControl_Load

        public void RotorUpdate()
        {

            if (RotorUpdateRunnng == false)
            {
                Thread t = new Thread(new ThreadStart(UpdateRotor));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "Update Rotor angle Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();
            }
        }

        bool RotorUpdateRunnng = false;

        //===============================================================
        // ke9ns THREAD
        private void UpdateRotor()
        {
            RotorUpdateRunnng = true;

            do
            {
                //   Debug.WriteLine("UPDATE ROTOR POSITION");

                Thread.Sleep(700);

                string answer = console.spotDDUtil_Rotor1; // get rotor angle current position
                RotorHead.Text = answer + "°";


            } while (console.ROTOREnabled == true);

            RotorHead.Visible = false;
            button3.Visible = false;
            numBeamHeading.Visible = false;

            Debug.WriteLine("END ---- UPDATE ROTOR POSITION");
            RotorUpdateRunnng = false;

        } // UpdateRotor()


        //=======================================================================================================================
        private void SpotControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            VOATHREAD1 = true;

            Debug.WriteLine("SpotControl_FormClosing here");

            //timer  SP5_Active = 0; // turn off map

            //  VOARUN = false; // voacap not running

            //  checkBoxMUF.Checked = false; // turn voacap mapping off

            do
            {
                Debug.WriteLine("Waiting for VOACAP thread to finish");

            } while (VOATHREAD == true);


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


            Debug.WriteLine("SpotControl_FormClosing here DONE");

        } // SpotControl_FormClosing




        public static byte SP_Active = 0;  // 0= off, 1= DX Spot feature ON, 2=logging in 3=waiting for spots
        public static byte SP2_Active = 0; // DX Spot: 0=closed so ok to open again if you want, 1=in process of shutting down
        public static byte SP4_Active = 0; // 1=processing valid DX spot. 0=not processing new DX spot

        public static byte SP1_Active = 0; // SWL active
        public static byte SP3_Active = 0; // 1=SWL database loaded up, so no need to reload if you turn if OFF

        public static byte SP5_Active = 0; // 1= turn on track map (sun, grayline, voacap, or beacon)


        public void SWLLoad()  // ke9ns called by console at startup to load 
        {

            Debug.WriteLine("SpotControl  SWLLOAD() here ");

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

            if ((SP3_Active == 0))
            {
                //  SP1_Active = 1;

                Thread t = new Thread(new ThreadStart(SWLSPOTTER));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "SWL Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.AboveNormal;
                t.Start();



            }

        } // SWLLoad()

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

                statusBoxSWL.ForeColor = Color.Blue;
                statusBoxSWL.Text = "SWL Spotting " + SWL_Index1.ToString();

                if (SP3_Active == 0)
                {

                    Thread t = new Thread(new ThreadStart(SWLSPOTTER));

                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                    t.Name = "SWL Spotter Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.AboveNormal;
                    t.Start();

                }

                if (SP_Active == 0)
                {
                    console.spotterMenu.ForeColor = Color.Yellow;
                    console.spotterMenu.Text = "SWL Spot";
                }

            }
            else
            {

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
        public static string[] SWL2_Station = new string[2000];       // Station name
        public static int[] SWL2_Freq = new int[2000];              // in hz
        public static byte[] SWL2_Band = new byte[2000];              // in Mhz

        public static string[] SWL2_Lang = new string[2000];          // language of transmitter
        public static int[] SWL2_TimeN = new int[2000];                // UTC time of operation ON air
        public static int[] SWL2_TimeF = new int[2000];                // UTC time of operation OFF air
        public static string[] SWL2_Mode = new string[2000];          // operating mode
        public static string[] SWL2_Day = new string[2000];          // days of operation
        public static byte[] SWL2_Day1 = new byte[2000];          // days of operation mo,tu,we,th,fr,sa,su = 1,2,4,8,16,32,64


        public static string[] SWL2_Loc = new string[2000];          // location of transmitter
        public static string[] SWL2_Target = new string[2000];          // target area of station
        public static int SWL2_Index1;  // local index that reset back to 0 after reaching max
        public static byte Flag21 = 0; // flag to skip header line in SWL.csv file


        // these are pulled from SWL.csv file
        public static string[] SWL_Station = new string[16000];       // Station name
        public static int[] SWL_Freq = new int[16000];              // in hz
        public static byte[] SWL_Band = new byte[16000];              // in Mhz
        public static int[] SWL_BandL = new int[16000];              // index for each start of mhz listed in swl.csv 

        public static string[] SWL_Lang = new string[16000];          // language of transmitter
        public static int[] SWL_TimeN = new int[16000];                // UTC time of operation ON air
        public static int[] SWL_TimeF = new int[16000];                // UTC time of operation OFF air
        public static string[] SWL_Mode = new string[16000];          // operating mode
        public static string[] SWL_Day = new string[16000];          // days of operation
        public static byte[] SWL_Day1 = new byte[16000];          // days of operation mo,tu,we,th,fr,sa,su = 1,2,4,8,16,32,64

        public static string[] SWL_Loc = new string[16000];          // location of transmitter
        public static string[] SWL_Target = new string[16000];          // target area of station

        public static int[] SWL_Pos = new int[16000];                // related to W on the panadapter screen

        public static int SWL_Index;  //  max number of spots in memory currently
        public static int SWL_Index1;  // local index that reset back to 0 after reaching max
        public static int SWL_Index3;  //  
        public static int VFOHLast;

        public static int Lindex; // low index spot
        public static int Hindex; // high index spot

        public static FileStream stream2;          // for reading SWL2.csv file
        public static BinaryReader reader2;

        public static FileStream stream2a;          // for reading SWL.csv file
        public static BinaryReader reader2a;

        public static FileStream stream22;          // for reading DXLOC file
        public static BinaryReader reader22;

        public static FileStream stream222;          // for reading voacap file
        public static BinaryReader reader222;

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

            Debug.WriteLine("SpotControl THREAD SWLSPOTTER() here");


            string file_name = " ";
            string file_name1 = " ";

            //  int FLAG22 = 0;


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
                for (; ; )
                {

                    if (SP3_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    //  Thread.Sleep(10); // test1

                    statusBoxSWL.ForeColor = Color.Red;
                    // statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();

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

                                //  Debug.WriteLine("SWL2 FREQ " + SWL2_Freq[SWL2_Index1] + " , " + SWL2_Index1);

                                SWL2_TimeN[SWL2_Index1] = Convert.ToInt16(values[1].Substring(0, 4)); // get time ON (24hr 4 digit UTC)
                                SWL2_TimeF[SWL2_Index1] = Convert.ToInt16(values[1].Substring(5, 4)); // get time OFF
                                SWL2_Day[SWL2_Index1] = values[2]; // get days ON

                                SWL2_Day1[SWL2_Index1] = 127; // digital signals are on 7 days

                                SWL2_Loc[SWL2_Index1] = values[3]; // get location of station
                                SWL2_Mode[SWL2_Index1] = "USB"; // get opeating mode
                                SWL2_Station[SWL2_Index1] = values[4]; // get station name

                                if (SWL2_Station[SWL2_Index1].Contains("FM"))
                                {
                                    SWL2_Mode[SWL2_Index1] = "FM"; //.219
                                }

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
                        Debug.WriteLine("END OF SWL2_Freq[SWL2_Index1] " + SWL2_Freq[SWL2_Index1] + " and last line# is " + SWL2_Index1);
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

                statusBoxSWL.Text = "SWL2 file: " + SWL2_Index1.ToString();

                Debug.WriteLine(" Read SWL2 file " + SWL2_Index1);

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
                    stream2a = new FileStream(file_name, FileMode.Open); // open file
                    reader2a = new BinaryReader(stream2a, Encoding.ASCII);
                }
                catch (Exception)
                {
                    SP1_Active = 0; // turn off SWL spotter

                    statusBoxSWL.ForeColor = Color.Red;
                    statusBoxSWL.Text = "Off";


                    return;
                }

                var result = new StringBuilder();

                if (SP3_Active == 0) // dont reset if already scanned in  database
                {
                    SWL_Index1 = 0; // how big is the SWL.CSV data file in lines
                    SWL_Index = 0; // was 0  start at 1 mhz
                    Flag1 = 0;

                }
                statusBoxSWL.Text = "Reading SWL ";

                Debug.WriteLine("READING SWL file");

                //------------------------------------------------------------------
                for (; ; )
                {

                    if (SP3_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    //  Thread.Sleep(10);

                    statusBoxSWL.ForeColor = Color.Red;
                    //   statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();



                    try
                    {
                        var newChar = (char)reader2a.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader2a.ReadChar(); // read \n char to finishline

                             //  Debug.WriteLine("SWL LINE: " + result);

                            if (Flag1 == 1)
                            {

                                string[] values;

                                values = result.ToString().Split(';'); // split line up into segments divided by ;

                                SWL_Freq[SWL_Index1] = (int)(Convert.ToDouble(values[0]) * 1000); // get freq and convert to hz
                                SWL_Band[SWL_Index1] = (byte)(SWL_Freq[SWL_Index1] / 1000000); // get freq and convert to mhz

                              //    Debug.WriteLine("SWL INDEX , FREQ "+ SWL_Index1 + " , " + SWL_Freq[SWL_Index1]);

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
                                if (SWL_Station[SWL_Index1].Contains("FM"))
                                {
                                    SWL_Mode[SWL_Index1] = "FM"; //.219
                                }

                                SWL_Lang[SWL_Index1] = values[5]; // get language
                                SWL_Target[SWL_Index1] = values[6]; // get station target area

                             //   if (SWL_Index1 > 13000)
                             //   Debug.WriteLine("SWL MODE " + SWL_Mode[SWL_Index1] + " , " + SWL_Station[SWL_Index1] + " , " + SWL_Index1);

                                //  Debug.WriteLine("SWL INDEX: " + SWL_Index1 + "," + SWL_Freq[SWL_Index1] + "," + SWL_Day1[SWL_Index1].ToString() + "," + SWL_TimeN[SWL_Index1]+ "-" + SWL_TimeF[SWL_Index1]);
                                //  Debug.WriteLine("  ");


                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                // Ke9ns MERGE SWL and SWL2
                                //FLAG22 = 0; // reset for next line

                                if ((SWL2_Index1 > 0) && (SWL_Index1 > 2)) // only try and merge SWL2 into SWL if SWL2 exists
                                {
                                    int lowfreq = SWL_Freq[SWL_Index1 - 1]; // prior freq read in
                                    int highfreq = SWL_Freq[SWL_Index1]; // freq just read in now

                                    //  Debug.WriteLine("lowfreq= " + lowfreq);
                                    //  Debug.WriteLine("highfreq= " + highfreq);

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

                                                //   Debug.WriteLine("INSERT SWL FREQ " + SWL_Freq[SWL_Index1] + " , " + SWL_Index1 + " , " + SWL_Freq[SWL_Index1 + 1] + " , " +q + " , "+ SWL2_Index1);

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

                                                //FLAG22 = 1; // flag that you inserted a new SWL2 line into SWL

                                                if (SWL_Band[SWL_Index1] > SWL_Index) // MHZ of the current examined spot > the mhz your looking at?
                                                {
                                                    //   Debug.WriteLine("0INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1 + "swl_Band[]" + SWL_Band[SWL_Index1] + " station name: " + SWL_Station[SWL_Index1] + " Freq: " + SWL_Freq[SWL_Index1]);
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


                                    } // for loop q through SWL2

                                } // if ((SWL2_Index1 > 0) && (SWL_Index1 > 2))

                                // below is to shorten the amount of time the swl routine in display.cs needs to find the swl listings to display in the pan
                                if (SWL_Band[SWL_Index1] > SWL_Index) // MHZ of the current examined spot > the mhz your looking at?
                                {
                                    //  Debug.WriteLine("1INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1 +"swl_Band[]" + SWL_Band[SWL_Index1] + " station name: " + SWL_Station[SWL_Index1] + " Freq: " + SWL_Freq[SWL_Index1]);
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
                                    // ke9ns   if the Station Name and Station Freq and Station Days are the same, then check time (below)
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
                    catch (EndOfStreamException f)
                    {
                        SWL_Index1--;

                        Debug.WriteLine("SWL ERROR ERROR: "+ f);
                        break; // done with file
                    }
                    catch (Exception g)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();
                        Debug.WriteLine("-SWL ERROR ERROR: " + g);

                        break; // done with file
                    }


                } // for loop until end of file is reached


                // Debug.WriteLine("reached SWL end of file");


                reader2a.Close();    // close  file
                stream2a.Close();   // close stream





                SP3_Active = 1; // done loading swl database (Good)
                Debug.WriteLine("SP3_ACTIVE = 1");

                statusBoxSWL.ForeColor = Color.Blue;
                statusBoxSWL.Text = "SWL Spotting " + SWL_Index1.ToString();

                //   console.swlsearchMenuItem1.Enabled = true;


                //   if (SP_Active == 0)
                //   {
                //     console.spotterMenu.ForeColor = Color.Yellow;
                //      console.spotterMenu.Text = "SWL Spot";
                //  }

                //==============================================================================
                //==============================================================================
                // ke9ns process lines of SWL data here

            } // if file exists


            // SP3_ACTIVE = 1 at the end of this routine

        } // SWLSPOTTER








        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //ke9ns start DX spotting
        public void spotSSB_Click(object sender, EventArgs e)
        {

            Debug.WriteLine("SpotControl spotSSB_Click here");

            //   Debug.WriteLine("TESt");
            //  Debug.WriteLine("========row " + dataGridView1.CurrentCell.RowIndex);
            //   Debug.WriteLine("========URL " + (string)dataGridView1["dxurl", dataGridView1.CurrentCell.RowIndex].Value);


            button2.Focus();

            //  Debug.WriteLine("DX SPOTTER ON SPOTSSB CLICK");


            if ((SP2_Active == 0) && (SP_Active == 0) && (callBox.Text != "callsign") && (callBox.Text != null)) // dont allow dx spotting while beacon is on.
            {

                dxon = true;
                chkDXOn.Checked = true;


                //  Debug.WriteLine("DX SPOTTER ON start THREAD");
                SP_Active = 1;
                Thread t = new Thread(new ThreadStart(SPOTTER));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");



                t.Name = "Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal; // normal
                t.Start();

                textBox1.Text = "Clicked to Open DX Spider \r\n";

            }
            else if ((callBox.Text == "callsign") || (callBox.Text == null))
            {

                chkDXOn.Checked = false;

                Debug.WriteLine("callbox " + callBox.Text);

                textBox1.Text += "Must put your CALL Sign in the CALLSIGN box (lower Right of this window)\r\n";
                callBox.ForeColor = Color.Red;
                callBox.BackColor = Color.Yellow;


            }
            else
            {

                chkDXOn.Checked = false;

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


        //-------------------------------------------------------------------------------------
        // DXt_ is for spotter screen only
        // DX_ is incoming messages and Panadater screen info
        
        public static string[] DXt_FULLSTRING = new string[500];    // full undecoded message

        public static string[] DXt_Station = new string[500];       // Extracted: dx call sign
        public static int[] DXt_Freq = new int[500];                // Extracted: Freq in hz
        public static string[] DXt_Spotter = new string[500];       // Extracted: spotter call sign
        public static string[] DXt_Message = new string[500];       // Extracted: message
        public static int[] DXt_Mode = new int[500];                // Parse: Mode from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am, 15=ft8, 16=mfsk, 17=feld, 18=ft4,
        public static int[] DXt_Mode2 = new int[500];               // Parse: split parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] DXt_Time = new int[500];                // Extracted: GMT (Unreliable because its the time submitted by the spotter)

        public static string[] DXt_Age = new string[500];           // Calculated: how old is the spot

        public static int[] DXt_Beam = new int[500];                // Calculated: beam heading from your lat/long

        public static int[] DXt_X = new int[500];                   // Calculated: x pixel location on map (before any scaling) Longitude
        public static int[] DXt_Y = new int[500];                   // Calculated: y pixel location on map (before any scaling) Latitude
        public static string[] DXt_country = new string[500];       // Calculated: country  by matching the callsign pulled from DXLOC.txt file

        public static string[] DXt_modegroup = new string[500];     // LoTW based on decoded mode
        public static string[] DXt_band = new string[500];          // LoTW based on freq of DX station
        public static int[] DXt_LoTW = new int[500];                // LoTW bit0=worked call (light green)(light purple=0), bit1=worked call on this band (Green)(light purple=0),  bit2=need DXCC (purple), bit3=need State (yellow), bit4=need Grid (orange)

        RTFBuilderbase[] DXt_LoTW_RTF = new RTFBuilder[500];        // LoTW contains the RTF string of DX Callsign with color codes
        public static int[] DXt_LoTW_Status = new int[500];         // ke9ns add .201 LoTW Status (Number format in place of color)


        public static string[] DXt_dxcc = new string[500];          // determined from dxloc.txt dxcc entity vs prefix
        public static string[] DXt_state = new string[500];         // determined from fcc callsign database file vx dx station callsign 
        public static string[] DXt_Grid = new string[500];          // Extracted and Parsed: grid square 



        //---------------------------------------------------------------------------------
        // incoming messages go into DX_

        public static string[] DX_FULLSTRING = new string[500];    // full undecoded message

        public static string[] DX_Station = new string[500];       // Extracted: dx call sign
        public static int[] DX_Freq = new int[500];                // Extracted: Freq in hz
        public static string[] DX_Spotter = new string[500];       // Extracted: spotter call sign
        public static string[] DX_Message = new string[500];       // Extracted: message
        public static int[] DX_Mode = new int[500];                // Parse: Mode from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am, 15=ft8, 16=mfsk, 17=feld, 18=ft4,
        public static int[] DX_Mode2 = new int[500];               // Parse: split parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] DX_Time = new int[500];                // Extracted: GMT (Unreliable because its the time submitted by the spotter)

        public static string[] DX_Age = new string[500];           // Calculated: how old is the spot

        public static int[] DX_Beam = new int[500];                // Calculated: beam heading from your lat/long

        public static int[] DX_X = new int[500];                   // Calculated: x pixel location on map (before any scaling) Longitude
        public static int[] DX_Y = new int[500];                   // Calculated: y pixel location on map (before any scaling) Latitude
        public static string[] DX_country = new string[500];       // Calculated: country  by matching the callsign pulled from DXLOC.txt file

        public static string[] DX_modegroup = new string[500];     // LoTW based on decoded mode
        public static string[] DX_band = new string[500];          // LoTW based on freq of DX station
        public static int[] DX_LoTW = new int[500];                // LoTW bit0=worked call (light green)(light purple=0), bit1=worked call on this band (Green)(light purple=0),  bit2=need DXCC (purple), bit3=need State (yellow), bit4=need Grid (orange)

        RTFBuilderbase[] DX_LoTW_RTF = new RTFBuilder[500];        // LoTW contains the RTF string of DX Callsign with color codes
        public static int[] DX_LoTW_Status = new int[500];         // ke9ns add .201 LoTW Status (Number format in place of color)


        public static string[] DX_dxcc = new string[500];          // determined from dxloc.txt dxcc entity vs prefix
        public static string[] DX_state = new string[500];         // determined from fcc callsign database file vx dx station callsign 
        public static string[] DX_Grid = new string[500];          // Extracted and Parsed: grid square 


        //  0  1- Did I work this CALL before? (yes = light Green = worked call) (no = light purple = new call)
        //  1  2- yes worked CALL, BUT Did I work this CALL on this Band before? (yes = Green = worked call on this band) (no = light purple = new call)
        //  2  4- Didnt work this CALL, BUT Did I work this DXCC before? (purple = need DXCC)
        //  3  8- Didnt work this CALL, BUT Did I work this State before? (yellow = need state)
        //  4  16- Didnt work this CALL, BUT Did I work this Grid before? (orange = need grid)

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
        public static int BX_dBm2 = 0;                           // avg value base line db passed back from display.cs
        public static int[] BX_dBm3 = new int[100];               // place to record the background signal strength of received stations as a S#

        public static int[] BX_TSlot = new int[100];              // time slot to hear this particular station of this particular freq (0 to 180 seconds)

        public static int[] BX_TSlot1 = new int[100];              // time slot for each of the 5 freq and 18 stations (0 to 170) but there are 5 of every time, 5 0's, 5 10sec, 5 20sec

        public static int BX_TSlot2 = 0;                         // time slot currently viewed 0 to 170 in 10sec increments

        public static int[] BX_Index = new int[10];                //  keep track of which freq (0-4) you are on for each station 

        public int BX1_Index = 0;                                 // should always be 90 for NCDXF beacons (5 x 18). Index for entire Beacon list (just like DX_Index for Dx spotter) 

        public bool BX_Load = false;   // true = BX_ values above all loaded 1 time, so no need to do it again. this way you can flip beacon on/off and see your last scan data

        //-------------------------------------------------------------------------------------
        // DXt_ for processTCPMessage() display only

        public static int DXt_Index = 0;                             //  max number of spots in memory currently
        public static int DXt_Index1 = 0;                            //  static temp index holder....always 250
        public static int DXt_Last = 0;                              //  last # in DX_Index (used for DXLOC_Mapper)spotter(

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
            Debug.WriteLine("SpotControl THREAD SPOTTER() here, for spotting DX");


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
                    Debug.WriteLine("NO PORT# detected us 7000 ");

                    client.Connect(nodeBox1.Text, 7000);      // 'EXAMPLE  client.Connect("192.168.0.149", 230) 
                }


                networkStream = client.GetStream();

                SP_reader = new StreamReader(networkStream, Encoding.ASCII, detectEncodingFromByteOrderMarks); //Encoding.UTF8  or detectEncodingFromByteOrderMarks
                SP_writer = new BinaryWriter(networkStream, Encoding.UTF7);


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

                        //  textBox1.Text += message3 + "\r\n";

                        if (message3.Contains("login: ") || message3.Contains("Please enter your call: ") || message3.Contains("Please enter your callsign:"))
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
                    // sned string SH/DX or show/dx (shows last 30) oldest first
                    // 0         10          22          34    40                         67
                    //  14031.9  VE2GDI      05-Aug-2020 0158Z  CW 599 <>FM17rj Glouceste <AI4FH>
                    // xxxxxx.xx xxxxxxxxxxx xxxxxxxxxxx xxxxx xxxxxxxxxxxxxxxxxxxxxxxxxx xxxxxxxxxx
                    //------------------------------------------------------------------------
                    // ke9ns standard DX spotting format:  
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

                            for (; !(sb.ToString().Contains("\r\n"));) //  wait for end of line
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

                            DX_Index1 = 250; // use 250 as a temp holding spot. always fill from the top



                            // grab DX_Spotter callsign=======================================================================================
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
                            catch (FormatException)
                            {
                                DX_Spotter[DX_Index1] = "NA";

                                //    textBox1.Text = e.ToString();
                            }
                            catch (ArgumentOutOfRangeException)
                            {

                                //    textBox1.Text = e.ToString();
                            }
                            //    Debug.WriteLine("DX_Call " + DX_Station[DX_Index1]);

                            // grab DX_Freq ========================================================================================
                            try
                            {

                                DX_Freq[DX_Index1] = (int)((double)Convert.ToDouble(message1.Substring(15, 9)) * (double)1000.0); //  get dx freq 7016.0  in khz 

                                if ((DX_Freq[DX_Index1] >= 1800000) && (DX_Freq[DX_Index1] <= 2000000))
                                {

                                    DX_band[DX_Index1] = "160M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 3500000) && (DX_Freq[DX_Index1] <= 4000000))
                                {

                                    DX_band[DX_Index1] = "80M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 5000000) && (DX_Freq[DX_Index1] <= 6000000))
                                {

                                    DX_band[DX_Index1] = "60M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 7000000) && (DX_Freq[DX_Index1] <= 7300000))
                                {

                                    DX_band[DX_Index1] = "40M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 10000000) && (DX_Freq[DX_Index1] <= 11000000))
                                {

                                    DX_band[DX_Index1] = "30M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 14000000) && (DX_Freq[DX_Index1] <= 14400000))
                                {

                                    DX_band[DX_Index1] = "20M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 18000000) && (DX_Freq[DX_Index1] <= 18670000))
                                {

                                    DX_band[DX_Index1] = "17M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 21000000) && (DX_Freq[DX_Index1] <= 24500000))
                                {

                                    DX_band[DX_Index1] = "15M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 24800000) && (DX_Freq[DX_Index1] <= 24990000))
                                {
                                    DX_band[DX_Index1] = "12M";
                                }

                                else if ((DX_Freq[DX_Index1] >= 28000000) && (DX_Freq[DX_Index1] <= 30000000))
                                {

                                    DX_band[DX_Index1] = "10M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 50000000) && (DX_Freq[DX_Index1] <= 54000000))
                                {

                                    DX_band[DX_Index1] = "6M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 54000001) && (DX_Freq[DX_Index1] <= 98999999)) // .219
                                {

                                    DX_band[DX_Index1] = "4M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 99000000) && (DX_Freq[DX_Index1] <= 143999999)) //.219
                                {

                                    DX_band[DX_Index1] = "3M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 144000000) && (DX_Freq[DX_Index1] <= 148000000))
                                {

                                    DX_band[DX_Index1] = "2M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 148000001) && (DX_Freq[DX_Index1] <= 180000000)) //.219
                                {

                                    DX_band[DX_Index1] = "1M";
                                }
                                else if ((DX_Freq[DX_Index1] >= 440000000) && (DX_Freq[DX_Index1] <= 448000000))
                                {

                                    DX_band[DX_Index1] = "70CM";
                                }

                                //.................................................

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
                                     || (DX_Freq[DX_Index1] == 21285000) || (DX_Freq[DX_Index1] == 21425000) || ((DX_Freq[DX_Index1] >= 29000000) && (DX_Freq[DX_Index1] < 29200000))
                                     || (DX_Freq[DX_Index1] == 50400000) || (DX_Freq[DX_Index1] == 50250000) || (DX_Freq[DX_Index1] == 144400000) || (DX_Freq[DX_Index1] == 144425000)
                                     || (DX_Freq[DX_Index1] == 144280000) || (DX_Freq[DX_Index1] == 144450000)

                                    )
                                {
                                    DX_Mode[DX_Index1] = 14; // AM mode
                                }
                                else if (
                                         ((DX_Freq[DX_Index1] >= 146000000) && (DX_Freq[DX_Index1] <= 148000000)) || ((DX_Freq[DX_Index1] >= 29200000) && (DX_Freq[DX_Index1] <= 29270000))
                                       || ((DX_Freq[DX_Index1] >= 144500000) && (DX_Freq[DX_Index1] <= 144900000)) || ((DX_Freq[DX_Index1] >= 145100000) && (DX_Freq[DX_Index1] <= 145500000))
                                       )
                                {
                                    DX_Mode[DX_Index1] = 114; // FM mode
                                }

                                else
                                {
                                    DX_Mode[DX_Index1] = 0; // ssb mode
                                }


                            } // try to determine if in the cw portion or ssb portion of each band
                            catch (FormatException)
                            {
                                DX_Freq[DX_Index1] = 0;
                                DX_Mode[DX_Index1] = 0; // ssb mode
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                DX_Freq[DX_Index1] = 0;
                                DX_Mode[DX_Index1] = 0; // ssb mode
                            }
                            Debug.WriteLine("DX_Freq " + DX_Freq[DX_Index1]);


                            // grab DX_Station Call sign =========================================================================================

                            try
                            {
                                DX_Station[DX_Index1] = message1.Substring(26, 13); // get dx call with : at the end
                                int pos = DX_Station[DX_Index1].IndexOf(' '); // find the
                                DX_Station[DX_Index1] = DX_Station[DX_Index1].Substring(0, pos); // reduce the call without the


                            }
                            catch (FormatException)
                            {
                                DX_Spotter[DX_Index1] = "NA";
                            }
                            catch (ArgumentOutOfRangeException)
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
                                else if (DX_Message[DX_Index1].Contains("jt6"))

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
                                else if (DX_Message[DX_Index1].Contains("packact") || DX_Message[DX_Index1].Contains("packtor") || DX_Message[DX_Index1].Contains("amtor"))
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
                                else if (DX_Message[DX_Index1].Contains(" am ") || DX_Message[DX_Index1].Contains(" sam "))
                                {
                                    if (chkBoxSSB.Checked != true) continue; // check for a SSB mode spot
                                    DX_Mode[DX_Index1] = 14; // AM mode

                                }
                                else if (DX_Message[DX_Index1].Contains("ft8")
                                    || ((DX_Freq[DX_Index1] >= 1840000) && (DX_Freq[DX_Index1] <= 1865000))
                                    || ((DX_Freq[DX_Index1] >= 3573000) && (DX_Freq[DX_Index1] <= 3575500))
                                    // || ((DX_Freq[DX_Index1] >= 5357000)  && (DX_Freq[DX_Index1] <= 5359500))
                                    || ((DX_Freq[DX_Index1] >= 7074000) && (DX_Freq[DX_Index1] <= 7076500))
                                    || ((DX_Freq[DX_Index1] >= 10136000) && (DX_Freq[DX_Index1] <= 10138500))
                                    || ((DX_Freq[DX_Index1] >= 14074000) && (DX_Freq[DX_Index1] <= 14076500))
                                    || ((DX_Freq[DX_Index1] >= 18100000) && (DX_Freq[DX_Index1] <= 18102500))
                                    || ((DX_Freq[DX_Index1] >= 21074000) && (DX_Freq[DX_Index1] <= 21076500))
                                    || ((DX_Freq[DX_Index1] >= 24915000) && (DX_Freq[DX_Index1] <= 24917500))
                                    || ((DX_Freq[DX_Index1] >= 28074000) && (DX_Freq[DX_Index1] <= 28076500))
                                    || ((DX_Freq[DX_Index1] >= 50313000) && (DX_Freq[DX_Index1] <= 50315500))
                                    || ((DX_Freq[DX_Index1] >= 50323000) && (DX_Freq[DX_Index1] <= 50325500))
                                    )  // FT8 1.84, 3.573, 5.357, 7.074, 10.136, 14.074, 18.1, 21.074, 24.915, 28.074, 50.313

                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digital mode spot
                                    DX_Mode[DX_Index1] = 15; // ft8 mode

                                    /*
                                                                        if ((DX_Freq[DX_Index1] >= 1840000) && (DX_Freq[DX_Index1] <= 1865000))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 1840000; // because FT8 does the freq moving with the 2.5khz slice, just put 0hz on the start of the slice
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 3573000) && (DX_Freq[DX_Index1] <= 3575500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 3573000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 7074000) && (DX_Freq[DX_Index1] <= 7076500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 7074000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 10136000) && (DX_Freq[DX_Index1] <= 10138500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 10136000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 14074000) && (DX_Freq[DX_Index1] <= 14076500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 14074000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 18100000) && (DX_Freq[DX_Index1] <= 18102500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 18100000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 21074000) && (DX_Freq[DX_Index1] <= 21076500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 21074000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 24915000) && (DX_Freq[DX_Index1] <= 24917500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 24915000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 28074000) && (DX_Freq[DX_Index1] <= 28076500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 28074000;
                                                                        }
                                                                        else if ((DX_Freq[DX_Index1] >= 50313000) && (DX_Freq[DX_Index1] <= 50315500))
                                                                        {
                                                                            DX_Freq[DX_Index1] = 50313000;
                                                                        }

                                    */

                                } // FT8
                                else if (DX_Message[DX_Index1].Contains("ft4")
                                  || ((DX_Freq[DX_Index1] >= 3568000) && (DX_Freq[DX_Index1] <= 3571500))
                                  || ((DX_Freq[DX_Index1] >= 3575000) && (DX_Freq[DX_Index1] <= 3577500))
                                  || ((DX_Freq[DX_Index1] >= 7047000) && (DX_Freq[DX_Index1] <= 7050500))
                                  || ((DX_Freq[DX_Index1] >= 10140000) && (DX_Freq[DX_Index1] <= 10142500))
                                  || ((DX_Freq[DX_Index1] >= 14080000) && (DX_Freq[DX_Index1] <= 14082500))
                                  || ((DX_Freq[DX_Index1] >= 18104000) && (DX_Freq[DX_Index1] <= 18105500))
                                  || ((DX_Freq[DX_Index1] >= 21140000) && (DX_Freq[DX_Index1] <= 21142500))
                                  || ((DX_Freq[DX_Index1] >= 24919000) && (DX_Freq[DX_Index1] <= 24921500))
                                  || ((DX_Freq[DX_Index1] >= 28180000) && (DX_Freq[DX_Index1] <= 28182500))
                                  || ((DX_Freq[DX_Index1] >= 50313000) && (DX_Freq[DX_Index1] <= 50315500))
                                 
                                  )  // FT8 1.84, 3.573, 5.357, 7.074, 10.136, 14.074, 18.1, 21.074, 24.915, 28.074, 50.313

                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digital mode spot
                                    DX_Mode[DX_Index1] = 16; // ft8 mode

                                  
                                } // FT4
                                else if (DX_Message[DX_Index1].Contains("mfsk"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 16; // mfsk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("feld"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 17; // Feld hell mode

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

                                if (DX_Station[DX_Index1].EndsWith("/B") == true) // spot is a beacon
                                {

                                    if (chkBoxBeacon.Checked != true)
                                    {
                                        Debug.WriteLine("bypass Beacon because not looking for Beacons");
                                        continue; // check for a Beacon spot
                                    }

                                }

                                //----------------------------------------------------------

                                // grab GRID #
                                DX_Grid[DX_Index1] = message1.Substring(76, 4); // get grid

                                sb = new StringBuilder(DX_Grid[DX_Index1]); // clear sb string over again
                                sb.Append(')');
                                sb.Insert(0, '('); // to differentiate the spotter from the spotted

                                DX_Grid[DX_Index1] = sb.ToString();




                                //--------------------------------------------------------------------------
                                if (DX_Message[DX_Index1].Contains("<") && DX_Message[DX_Index1].Contains(">")) // check for Grid
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
                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                        Debug.WriteLine("Found UP split hz" + split_hz);
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

                                                    int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
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

                                else if ((DX_Message[DX_Index1].Contains("dn"))) // check for split down
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
                                            if (DX_Message[DX_Index1].Substring(ind - 2, 2) == "dn")
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000)); // 3 digits after the dnXXX (and dn XX)
                                                Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                if (DX_Message[DX_Index1].Substring(ind - 2, 2) == "dn")
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000)); // 2 digits after the dnXX
                                                    Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000)); // 4 digits dn  XXXX
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
                                else if ((DX_Message[DX_Index1].Contains("9+")) || (DX_Message[DX_Index1].Contains("59+"))) // check for split
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

                                else if ((DX_Message[DX_Index1].Contains("qsx"))) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("qsx") + 3;

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
                                    if (((us1 == "V") && (r2.IsMatch(us2))) || ((us1 == "C") && (r3.IsMatch(us2))))
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
                                    if (((us1 == "V") && !(r2.IsMatch(us2))) || ((us1 == "C") && !(r3.IsMatch(us2))))
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
                                if ((xx == 0) && (DX_Station[DX_Index1] == DX_Station[ii])) // if DX stations Match
                                {
                                    if ((Math.Abs(DX_Freq[DX_Index1] - DX_Freq[ii])) < 1000000) // if DX stations freq changes by less than 1mhz, then remove it as a dup, DX station moved freq
                                    {

                                        xx = 1;
                                        Debug.WriteLine("station dup============" + DX_Freq[ii] + " dup " + DX_Freq[DX_Index1] + " dup " + DX_Station[DX_Index1] + " dup " + DX_Station[ii]);
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


                                    DX_LoTW[ii] = DX_LoTW[ii + 1]; // used by LoTW
                                    DX_band[ii] = DX_band[ii + 1];
                                    DX_modegroup[ii] = DX_modegroup[ii + 1];
                                    DX_dxcc[ii] = DX_dxcc[ii + 1];
                                    DX_state[ii] = DX_state[ii + 1];

                                    DX_LoTW_RTF[ii] = DX_LoTW_RTF[ii + 1];
                                    DX_LoTW_Status[ii] = DX_LoTW_Status[ii + 1]; // .201

                                }

                            } // for ii check for dup in list
                            DX_Index = (DX_Index - xx);  // readjust the length of the spot list after taking out the duplicates

                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns check for FREQ DUPLICATES , there can only be 1 possible duplicate per spot added (but IGNORE if on 2nd band)
                            //   except for FT8 frequencies

                            xx = 0;
                            for (int ii = 0; ii <= DX_Index; ii++)
                            {

                                //                                if ((xx == 0) && (DX_Freq[DX_Index1] == DX_Freq[ii])) // if you already have this station in the spot list on the screen remove the old spot
                                if ((xx == 0) && ((DX_Freq[DX_Index1] == DX_Freq[ii]) &&
                                     (DX_Freq[ii] != 1840000) && (DX_Freq[ii] != 3573000) && (DX_Freq[ii] != 7074000) && (DX_Freq[ii] != 10136000) &&
                                     (DX_Freq[ii] != 14074000) && (DX_Freq[ii] != 18100000) && (DX_Freq[ii] != 21074000) && (DX_Freq[ii] != 24915000) &&
                                      (DX_Freq[ii] != 50313000) && (DX_Freq[ii] != 50323000))) // if you already have this station in the spot list on the screen remove the old spot
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


                                    DX_LoTW[ii] = DX_LoTW[ii + 1]; // used by LoTW
                                    DX_band[ii] = DX_band[ii + 1];
                                    DX_modegroup[ii] = DX_modegroup[ii + 1];
                                    DX_dxcc[ii] = DX_dxcc[ii + 1];
                                    DX_state[ii] = DX_state[ii + 1];

                                    DX_LoTW_RTF[ii] = DX_LoTW_RTF[ii + 1];
                                    DX_LoTW_Status[ii] = DX_LoTW_Status[ii + 1]; // .201
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
                                DX_Message[ii] = DX_Message[ii - 1];
                                DX_Grid[ii] = DX_Grid[ii - 1];
                                DX_Time[ii] = DX_Time[ii - 1];
                                DX_Age[ii] = DX_Age[ii - 1];
                                DX_Mode[ii] = DX_Mode[ii - 1];
                                DX_Mode2[ii] = DX_Mode2[ii - 1];

                                DX_country[ii] = DX_country[ii - 1];
                                DX_X[ii] = DX_X[ii - 1];
                                DX_Y[ii] = DX_Y[ii - 1];
                                DX_Beam[ii] = DX_Beam[ii - 1];


                                DX_LoTW[ii] = DX_LoTW[ii - 1]; // used by LoTW
                                DX_band[ii] = DX_band[ii - 1];
                                DX_modegroup[ii] = DX_modegroup[ii - 1];
                                DX_dxcc[ii] = DX_dxcc[ii - 1];
                                DX_state[ii] = DX_state[ii - 1];

                                DX_LoTW_RTF[ii] = DX_LoTW_RTF[ii - 1];
                                DX_LoTW_Status[ii] = DX_LoTW_Status[ii - 1]; // .201

                            } // for ii



                            //=================================================================================================
                            //=================================================================================================


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

                            DX_country[0] = DX_country[DX_Index1];
                            DX_X[0] = DX_X[DX_Index1];
                            DX_Y[0] = DX_Y[DX_Index1];
                            DX_Beam[0] = DX_Beam[DX_Index1];


                            DX_band[0] = DX_band[DX_Index1];
                            DX_modegroup[0] = DX_modegroup[DX_Index1];

                            //   DX_dxcc[0] = DX_dxcc[DX_Index1]; // updated in upatemapspots
                            //   DX_state[0] = DX_state[DX_Index1]; 
                            //   DX_LoTW[0] = DX_LoTW[DX_Index1]; // used by LoTW updated down below
                            //   DX_LoTW_RTF[0] = DX_LoTW_RTF[DX_Index1];
                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            // Crosscheck Station Call sign Prefix with data from DXLOC.txt (lat and lon) 
                            // and create a list of Country, Callsign, X, Y on unscaled map


                            updatemapspots();


                            /*
                               1=    confirmed LoTW qsl with DXcall 
                               2=    LoTW qsl with DXcall on this Band 
                               4=    nonconfirmed (but not new) LoTW qsl with DXcall on this Band awaiting confirmation
                               8=    indicates nothing in your LoTW log (purple)
                               16=   I have a confirmed contact with some other station at this DXCC entity
                               32=   I have a confirmed contact with some other station at this DXCC entity and the same Band 
                               64=   This DXcall grid is new to me
                               128=  This DXcall grid on this band is new to me 
                               256=   I have a confirmed contact with some other station of this US state
                               512=   I have a confirmed contact with some other station at this US state and same Band 
                               1024= beacon Spot cannot make a contact with a beacon

                               3 = I have a confirmed LoTW with this DX station and on this Band
                              48= I have a confirmed LoTW contact on this BAND with this DXCC entity (but not this DX station)



                            3=  Green: You have this DX Station confirmed on this Band(dont need this Dx Station)
                            1=  LightGreen: You have this DX Station confirmed on some other Band
                            1+48=  Yellow: You have this DX station confirmed on some other band, and some other station already confirmed on this band (dont need this DX Station)

                            48=  Orange: You have this DXCC country CONFIRMED on this Band (you dont need this DX Station)
                            16=  Brown: You have this DXCC country CONFIRMED on some other Band (you WANT this DX Station)
                            8=  Purple You WANT this DXCC country (you WANT this DX Station)

                              Pink: You have worked this DX Station on this Band(But they have not confirmed)
                              LightPink: You have worked DX Station on some other Band(But they have not confirmed)

                              */


                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns check new DX SPOT against the LoTW database

                            string dxtemp = "";

                            DX_LoTW[0] = 0;

                            DX_LoTW_RTF[0] = new RTFBuilder(RTFFont.CourierNew, 18f);

                            DX_LoTW_RTF[0].Clear();
                            DX_LoTW_RTF[0].Reset();

                            DX_LoTW_Status[0] = 0;  // .201

                            if (lotw_records > 0) // if you have LoTW files then check otherwise skip it
                            {

                                if ((DX_country[0].Contains("USA") == true) && (FCCSTATE_NUM > 0)) // only check USA stations for STATE
                                {

                                    try
                                    {
                                        //  int q = DX_Station[0].Length; // length of callsign

                                        DX_state[0] = "--";

                                        dxtemp = DX_Station[0];

                                        if (dxtemp.Contains("/"))
                                        {
                                            dxtemp = DX_Station[0].Substring(0, DX_Station[0].IndexOf('/')); // remove stuff after the /
                                            Debug.WriteLine("STATION/: " + dxtemp);
                                        }

                                        for (int v = 0; v < FCCSTATE_NUM; v++)
                                        {
                                            //  if (FCCCALL[v] == DX_Station[0])
                                            if (FCCCALL[v] == dxtemp)  // this is used to ignore /AM = air mobile, /MM = maritime mobile /B = beacon
                                            {
                                                DX_state[0] = FCCSTATE[v];
                                                Debug.WriteLine("STATE FOUND:" + dxtemp + ", " + DX_Station[0] + ", " + DX_state[0]);
                                                break;
                                            }


                                        } // for loop fccstate_num

                                        Debug.WriteLine("PROBLEM: Could not find station in FCC database: " + dxtemp + ", " + DX_Station[0]);
                                    }
                                    catch
                                    {
                                        Debug.WriteLine("STATION: " + dxtemp);
                                        DX_state[0] = "++";
                                    }

                                } // USA stations when FCC database is present
                                else
                                {
                                    Debug.WriteLine("STATION: " + dxtemp);
                                    Debug.WriteLine("NOT USA Station, Only USA stations checked against FCC database");
                                    DX_state[0] = "--";
                                }


                                Debug.WriteLine("+++++++++++++++++LoTW Parse START here: ");
                                // DX_LoTW[0]: definition
                                // 8= no match 
                                // 4= QSO & Match Call
                                // 2= Match Call & Band (sets in both QSO and QSL situations)
                                // 1= QSL & Match Call
                                // 16 = QSL & Match DX entity & NO Call match
                                // 32 = QSL & Match DX entity & Match Band & NO Call Match
                                // 256 = QSL & No Call Match & Match State
                                // 512 = QSL & No Call Match & Match State & Match Band
                                // 1024 = beacon
                                // 2048 =
                                // 4096 =

                                DX_LoTW[0] = 8;

                                if (DX_Station[0].EndsWith("/B")) // beacon dxspot (not a station you can contact
                                {
                                    DX_LoTW[0] = 1024; // beacon

                                }
                                else
                                {
                                    for (int x1 = 0; x1 < lotw_records; x1++) // check your entire LoTW QSO log against the DX SPOT
                                    {

                                        if (LoTW_qsl[x1] != "Y") //  check LoTW records that you have a confirmed QSL "NO"
                                        {
                                            // LoTW records that are un-confirmed QSO's (below)

                                            if (LoTW_callsign[x1] == DX_Station[0])  // if DX spot Station callsign matches (BUT not yet confirmed)
                                            {
                                                DX_LoTW[0] = DX_LoTW[0] | 4; // light Yellow = Not New, but not confirmed yet

                                                // Debug.WriteLine("DX_LoTW: 4 " + x1);

                                                if (LoTW_band[x1] == DX_band[0])
                                                {
                                                    DX_LoTW[0] = DX_LoTW[0] | 2; // Dark Yellow = Not new and on this band, but not confirmed yet
                                                                                 // Debug.WriteLine("DX_LoTW: 2+4 " + x1);
                                                }

                                            } //if (LoTW_callsign[x1] == DX_Station[DX_Index1])

                                            else // this LoTW QSO is not the same callsign as the DXspot  and not confirmed yet
                                            {
                                                // Dont need to check here since this is not a matching callsign and is not confirmed yet


                                            } // this LoTW QSO is not the same callsign as the DXspot

                                        } // LoTW records that are un-confirmed QSO's (above)

                                        else // LoTW records that are confirmed QSL's (below)
                                        {

                                            if (LoTW_callsign[x1] == DX_Station[0])  // check if confirmed LoTW QSL is this DX stations Call sign
                                            {
                                                DX_LoTW[0] = DX_LoTW[0] | 1;     // Light Green = confirmed contact with this DX Station
 
                                                if (LoTW_band[x1] == DX_band[0]) // if it was on this very band, then Dark green
                                                {
                                                    DX_LoTW[0] = DX_LoTW[0] | 2; // Dark Green = confirmed contact on this very Band
                                                                                 //  Debug.WriteLine("DX_LoTW: 1+2 " + x1);
 
                                                    Debug.WriteLine("Break");
                                                    break; // dont need this contact
                                                }

                                            } // if (LoTW_callsign[x1] == DX_Station[DX_Index1])

                                            else // QSL & NO CALL MATCH, so check to see if DXspot is needed for DXCC,state,grid,
                                            {
                                                if (LoTW_dxcc[x1] == DX_dxcc[0]) // check if I already have this DXCC entity confirmed with some other callsign (fed republic of Germany= 230
                                                {
                                                    DX_LoTW[0] = DX_LoTW[0] | 16; // I have a confirmed QSO with someone else from this DXCC entity but not nessesarily the band

                                                    if (LoTW_band[x1] == DX_band[0])
                                                    {
                                                        DX_LoTW[0] = DX_LoTW[0] | 32; // I have a confirmed QSO with some other callsign, at this DXCC entity and Band
                                                                                      //   Debug.WriteLine("DX_LoTW: 16+32 " + x1);
                                                    }
                                                } // matching DXCC entity

                                                if ((DX_country[0].Contains("USA") == true) && (FCCSTATE_NUM > 0))
                                                {
                                                    if (LoTW_state[x1] == DX_state[0]) // check if I already have this US state confirmed with some other callsign 
                                                    {
                                                        DX_LoTW[0] = DX_LoTW[0] | 256; // I have a confirmed QSL with someone else from this US state  but not nessesarily the band

                                                        if (LoTW_band[x1] == DX_band[0])
                                                        {
                                                            DX_LoTW[0] = DX_LoTW[0] | 512; // I have a confirmed QSL with some other callsign, at this US state and Band
                                                            Debug.WriteLine("QSO 512 " + LoTW_callsign[x1] + " worked on this band to this state already");

                                                        }
                                                        else Debug.WriteLine("QSO 256 " + LoTW_callsign[x1] + " worked on this state already but not this band");

                                                    } // matching US state
                                                }
                                            } // this LoTW QSO is not the same callsign as the DXspot

                                        } // LoTW records that are confirmed QSO's (above)

                                    } // for x1 loop thru LoTW database

                                    Debug.WriteLine("+++++++++++++++++LoTW Parse FINISH here: " + lotw_records +
                                        ",DX_INDEX1= " + DX_Index1 + ", " + DX_Station[0] + ", " + DX_band[0] +
                                        ", " + DX_dxcc[0] + ", " + DX_country[0] + ",DX_LoTW= " + DX_LoTW[0] + ",DX_state= " + DX_state[0]);


                                } // beacon check else (do the above)



                                Color DXColor = new Color();


                                // 1 Green: You have this DX Station confirmed on this Band(dont need this Dx Station)
                                // 2 LightGreen: You have this DX entity confirmed on this band
                                // 3 Yellow: You have this DX station confirmed on some other band, and some other station already confirmed on this band(dont need this DX Station)
                                // 4 Orange: You have QSL for DXCC country confirmed on this Band or QSL for this state
                                // 5 LightPurple (violet): You have this DXCC country CONFIRMED on some other Band(you WANT this DX Station)
                                // 6 Purple (mediumOrchid): You WANT this DXCC country(you WANT this DX Station)
                                // 7 DeepPink: You have worked this DX Station on this Band(But they have not  confirmed)
                                // 8 Pink: You have worked DX Station on some other Band(But they have not confirmed)
                                // 9 LightBlue: You have this US State confirmed on some other band(you WANT this DX Station)
                                // 10 Blue: You WANT this US State(You Want this DX Station).
                                //11 Gray: Beacon Station

                                // DX_LoTW[0]: definition
                                // 1= QSL & Match Call
                                // 2= Match Call & Band (sets in both QSO and QSL situations)
                                // 4= QSO & Match Call
                                // 8= No Match at all                                               (Dark purple) 
                                // 16 = QSL & Match DX entity & NO Call match
                                // 32 = QSL & Match DX entity & Match Band & NO Call Match
                                // 256 = QSL & No Call Match & Match State
                                // 512 = QSL & No Call Match & Match State & Match Band
                                // 1024 = beacon                                                      (gray)
                                // 2048 =
                                // 4096 =

                                DXColor = Color.LightYellow; // default


                                if ((DX_LoTW[0] == 8)) // if 8 then we dont have a QSL for any call matching this DX spots DXCC entity 
                                {
                                    DXColor = Color.MediumOrchid;
                                    DX_LoTW_Status[0] = 6;// dark purple
                                    goto GO1;

                                } // 8

                                if ((DX_LoTW[0] == 1024)) // beacon only
                                {
                                    DXColor = Color.SlateGray;
                                    DX_LoTW_Status[0] = 11;
                                    goto GO1;

                                } // 8


                                else if ((DX_LoTW[0] & 1) == 1) // confirmed you have worked this DX station
                                {
                                    if ((DX_LoTW[0] & 2) == 2) // confirmed you also worked this station on this very band
                                    {
                                        DXColor = Color.Green; // green DONE
                                        DX_LoTW_Status[0] = 1;
                                        goto GO1;

                                    } // 2
                                    else // Worked this station on some other band, BUT check if you worked some other call in this DX entity on this band?         
                                    {
                                        if ((DX_LoTW[0] & 48) == 48) // you have this DXCC entity confirmed on this band so you dont need this dx station
                                        {
                                            if (((DX_LoTW[0] & 256) == 256)) // I have this state but not necessarily on this band
                                            {
                                                if ((DX_LoTW[0] & 512) == 512) // I have someone else QSL this state and on this band
                                                {
                                                    DXColor = Color.Orange; // 
                                                    DX_LoTW_Status[0] = 4;
                                                    goto GO1;
                                                }

                                                DXColor = Color.SkyBlue; // light blue // I have QSL for state on some other band with someone else
                                                DX_LoTW_Status[0] = 9;
                                                goto GO1;
                                            }

                                            if ((FCCSTATE_NUM > 0) && (DX_Station.Contains("USA") == true)) // if your checking for states, then you dont have this state QSL yet
                                            {
                                                DXColor = Color.RoyalBlue; // dark blue (dont have this state, you want this state)
                                                DX_LoTW_Status[0] = 10;
                                                goto GO1;
                                            }
                                            else
                                            {
                                                DXColor = Color.Yellow; // yellow
                                                DX_LoTW_Status[0] = 3; // not a state and have someone else QSL on this band
                                                goto GO1;
                                            }


                                        } // 48
                                        else // you have worked this station, but on another band.
                                        {

                                            if ((DX_LoTW[0] & 256) == 256) // I have this state but not necessarily on this band
                                            {
                                                if ((DX_LoTW[0] & 512) == 512) // I have this state and on this band
                                                {
                                                    DXColor = Color.LightGreen; // 
                                                    DX_LoTW_Status[0] = 2; //
                                                    goto GO1;
                                                }

                                                DXColor = Color.SkyBlue; // you need this state on this band
                                                DX_LoTW_Status[0] = 9; //
                                                goto GO1;
                                            }

                                            if ((FCCSTATE_NUM > 0) && (DX_Station.Contains("USA") == true))
                                            {


                                            }
                                            else
                                            {
                                                DXColor = Color.LightGreen; // light green
                                                DX_LoTW_Status[0] = 2; //
                                                goto GO1;
                                            }
                                        }
                                    }
                                } // 1= QSL and a Match call
                                else if ((DX_LoTW[0] & 4) == 4)// You have worked this station but not confirmed
                                {

                                    if ((DX_LoTW[0] & 2) == 2) // worked unconfirmed DXstation on this very band
                                    {
                                        DXColor = Color.DeepPink; // this very DX station did not confirm you on this very band
                                        DX_LoTW_Status[0] = 7; // deep pink
                                        goto GO1;

                                    } // 2
                                    else // work this station on another band and not confirmed
                                    {
                                        DXColor = Color.HotPink; // light pink this very DX station did not confirm you on some other band
                                        DX_LoTW_Status[0] = 8; // pink
                                        goto GO1;

                                    }

                                } // 4

                                else if ((DX_LoTW[0] & 16) == 16) // You have some other confirmed station at this DXCC entity location
                                {

                                    if ((DX_LoTW[0] & 32) == 32) // You have some other confirmed station at this DXCC entity location and this Band.
                                    {
                                        if (((DX_LoTW[0] & 256) == 256)) // I have this state but not necessarily on this band
                                        {
                                            if ((DX_LoTW[0] & 512) == 512) // I have this state and on this band
                                            {
                                                DXColor = Color.Orange; // 
                                                DX_LoTW_Status[0] = 4; // orange
                                                goto GO1;
                                            }

                                            DXColor = Color.SkyBlue; //
                                            DX_LoTW_Status[0] = 9; // light blue
                                            goto GO1;
                                        }


                                        if ((FCCSTATE_NUM > 0) && (DX_Station.Contains("USA") == true))// if your checking for states, then you dont have this state QSL yet
                                        {
                                            DXColor = Color.RoyalBlue; // 
                                            DX_LoTW_Status[0] = 10; // blue
                                            goto GO1;
                                        }
                                        else
                                        {

                                            DXColor = Color.Orange;//Orange so you dont really need the DX station
                                            DX_LoTW_Status[0] = 4; // orange
                                            goto GO1;
                                        }

                                    } // 32
                                    else // you have a confirmed dxcc contact of some other station, but on a different band
                                    {
                                        DXColor = Color.Violet;
                                        DX_LoTW_Status[0] = 5; // light purple
                                        goto GO1;
                                    }

                                } // 16
                            GO1:;
                                DX_LoTW_RTF[0].BackColor(DXColor).Append(DX_Station[0].PadRight(11)); // Color the DXSPOT

                            } // lotw records available to check
                            else
                            {
                                DX_LoTW_RTF[0].BackColor(Color.LightYellow).Append(DX_Station[0].PadRight(11)); //  if LoTW button is OFF
                                DX_LoTW_Status[0] = 0;
                            }



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

            Debug.WriteLine("SpotControl updateMAPspots() here");

            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            // Crosscheck Station Call sign Prefix with data from DXLOC.txt (lat and lon) 
            // and create a list of Country, Callsign, X, Y on unscaled map

            if ((SP8_Active == 1) && (SP_Active > 2) && (DX_Index > 0)) // do if dxloc.txt file loaded in        && (SP5_Active == 1 )
            {

                int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map

                int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                Debug.Write("MAPPING====== ");

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

                        DX_dxcc[0] = Convert.ToInt16(DXLOC_dxcc[kk]).ToString(); // get DX stations DXCC entity from the DXLOC.txt file data(get rid of leading zeros


                        Debug.WriteLine(DX_Station[0] + " " + DX_X[0] + " " + DX_Y[0] + ", cntry " + DX_country[0] +
                            ", prefix " + DXLOC_prefix[kk] + ", lat " + DXLOC_LAT[kk] + ", lon " + DXLOC_LON[kk] + ", BEAM " + DX_Beam[0] +
                            ", DXCC " + DX_dxcc[0] + ",kk " + kk + ",DXLOC_Index1 " + DXLOC_Index1 + ", STATE " + DX_state[0]);

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
        // ke9ns add: convert all the DX spots in the array to text messages for the DX spotter window
        public void processTCPMessage()
        {
            if (pause == true || beacon == true || WTime == true) return;


            bool ListHide = false;

            Debug.WriteLine("SpotControl processTCPMessage() here");

            string bigmessage = null;
            RTFBuilderbase BIGM = new RTFBuilder(RTFFont.CourierNew, 18f);

           
            if (console.RX2Band.ToString() == "GEN") // .157
            {
                hkBoxSpotRX2.Checked = false;
                hkBoxSpotRX2.Enabled = false;

            }


            string band1 = console.RX1Band.ToString();
            if (band1.StartsWith("B")) band1 = band1.Remove(0, 1); // remove the B from Band.xxx in console


            string band2 = console.RX2Band.ToString();
            if (band2.StartsWith("B")) band2 = band2.Remove(0, 1); // remove the B from Band.xxx in console


            BIGM.Clear();
            BIGM.Reset();

            DXt_Index = DX_Index; //.232


            for (int ii = 0; ii < DX_Index; ii++)
            {
                // update DXt is Spotter display can update

              

                //--------------------------------------------


                if (DX_Age[ii] == null) DX_Age[ii] = "00";
                else if (DX_Age[ii] == "  ") DX_Age[ii] = "00";

                if (DX_Age[ii].Length == 1) DX_Age[ii] = "0" + DX_Age[ii];

                //----------------------------------------------------------
                string DXmode = "    "; // 5 spaces

                if (DX_Mode[ii] == 0) DXmode = " ssb ";
                else if (DX_Mode[ii] == 1) DXmode = " cw  ";
                else if (DX_Mode[ii] == 2) DXmode = " rtty";
                else if (DX_Mode[ii] == 3) DXmode = " psk ";
                else if (DX_Mode[ii] == 4) DXmode = " oliv";
                else if (DX_Mode[ii] == 5) DXmode = " jt65";
                else if (DX_Mode[ii] == 6) DXmode = " cont";
                else if (DX_Mode[ii] == 7) DXmode = " fsk ";
                else if (DX_Mode[ii] == 8) DXmode = " mt63";
                else if (DX_Mode[ii] == 9) DXmode = " domi";
                else if (DX_Mode[ii] == 10) DXmode = " pack";
                else if (DX_Mode[ii] == 11) DXmode = " fm  ";
                else if (DX_Mode[ii] == 12) DXmode = " drm ";
                else if (DX_Mode[ii] == 13) DXmode = " sstv";
                else if (DX_Mode[ii] == 14) DXmode = " am  ";
                else if (DX_Mode[ii] == 15) DXmode = " ft8 ";
                else if (DX_Mode[ii] == 16) DXmode = " mfsk";
                else if (DX_Mode[ii] == 17) DXmode = " feld";
                else if (DX_Mode[ii] == 18) DXmode = " ft4 ";

                else DXmode = "     ";

                ListHide = false;

                if (((hkBoxSpotBand.Checked == true) && (DX_band[ii] == band1)) || ((hkBoxSpotRX2.Checked == true) && (DX_band[ii] == band2)))
                {
                    ListHide = false; // show this spot in the list

                }
                else if ((hkBoxSpotBand.Checked == true) || (hkBoxSpotRX2.Checked == true))
                {
                    ListHide = true; // HIDE this spot from the listing
                }


                if ((FCCSTATE_NUM > 0) && (DX_country[ii].Contains("USA") == true)) // for USA only here
                {

                    if (ListHide == true)
                    {
                        // HIDE IN LISTING
                        BIGM.ForeColor(Color.LightYellow).BackColor(Color.LightYellow).AppendLine(DX_FULLSTRING[ii].Substring(0, 26) + DX_Station[0].PadRight(11) + DX_FULLSTRING[ii].Substring(37, DX_FULLSTRING[ii].Length - 37) + DXmode + " " + (("USA-" + DX_state[ii]).PadRight(8)).Substring(0, 8) + ": " + DX_Beam[ii].ToString().PadLeft(3) + "° :" + DX_Age[ii]); //.ForeColor(Color.LightYellow);
                    }
                    else
                    {
                        // SHOW in LISTING
                        BIGM.ForeColor(SystemColors.WindowText).Append(DX_FULLSTRING[ii].Substring(0, 26)).AppendRTFDocument(DX_LoTW_RTF[ii].ToString()).AppendLine(DX_FULLSTRING[ii].Substring(37, DX_FULLSTRING[ii].Length - 37) + DXmode + " " + (("USA-" + DX_state[ii]).PadRight(8)).Substring(0, 8) + ": " + DX_Beam[ii].ToString().PadLeft(3) + "° :" + DX_Age[ii]); //.ForeColor(SystemColors.WindowText);
                    }

                }
                else // for rest of world here below
                {

                    if (ListHide == true) // hide other bands
                    {
                        // HIDE IN LISTING
                        BIGM.ForeColor(Color.LightYellow).BackColor(Color.LightYellow).AppendLine(DX_FULLSTRING[ii].Substring(0, 26) + DX_Station[0].PadRight(11) + DX_FULLSTRING[ii].Substring(37, DX_FULLSTRING[ii].Length - 37) + DXmode + " " + (DX_country[ii].PadRight(8)).Substring(0, 8) + ": " + DX_Beam[ii].ToString().PadLeft(3) + "° :" + DX_Age[ii]).ForeColor(Color.LightYellow);

                    }
                    else
                    {
                        // SHOW IN LISTING
                        BIGM.ForeColor(SystemColors.WindowText).Append(DX_FULLSTRING[ii].Substring(0, 26)).AppendRTFDocument(DX_LoTW_RTF[ii].ToString()).AppendLine(DX_FULLSTRING[ii].Substring(37, DX_FULLSTRING[ii].Length - 37) + DXmode + " " + (DX_country[ii].PadRight(8)).Substring(0, 8) + ": " + DX_Beam[ii].ToString().PadLeft(3) + "° :" + DX_Age[ii]); //.ForeColor(SystemColors.WindowText);
                    }
                }

                //-----------------------------------------
                // update for spotter screen when unpaused

                DXt_Station[ii] = DX_Station[ii]; //.232
                DXt_Freq[ii] = DX_Freq[ii]; //.232
                DXt_Spotter[ii] = DX_Spotter[ii]; //.232
                DXt_Message[ii] = DX_Message[ii]; //.232
                DXt_Mode[ii] = DX_Mode[ii]; //.232
                DXt_Mode2[ii] = DX_Mode2[ii]; //.232
                DXt_Time[ii] = DX_Time[ii]; //.232
                DXt_Age[ii] = DX_Age[ii]; //.232
                DXt_Beam[ii] = DX_Beam[ii]; //.232
                DXt_country[ii] = DX_country[ii]; //.232
                DXt_modegroup[ii] = DX_modegroup[ii]; //.232
                DXt_band[ii] = DX_band[ii]; //.232
                DXt_LoTW[ii] = DX_LoTW[ii]; //.232
                DXt_LoTW_Status[ii] = DX_LoTW_Status[ii];
                DXt_LoTW_RTF[ii] = DX_LoTW_RTF[ii]; //.232
                DXt_dxcc[ii] = DX_dxcc[ii]; //.232
                DXt_state[ii] = DX_state[ii]; //.232
                DXt_Grid[ii] = DX_Grid[ii]; //.232


            } // for loop to update dx spot window


            //---------------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------------
            //  if ((pause == false) && (beacon == false) && (WTime == false))
            //  {
            //  textBox1.Text = bigmessage; // update screen
            //  textBox1.Select((ii * LineLength) + 26, 11); // select each line in turn on the entire Spotter listing


            textBox1.Rtf = BIGM.ToString(); // use RichTextFormat to allow for highlighting with color


                //---------------------------------------------------------------------------------------------------------
                //---------------------------------------------------------------------------------------------------------

                Debug.WriteLine("DX_TEXT " + DX_TEXT + " , " + DX_SELECTED);

                // highlighted line on spotter screen based on mouse selection of line, or CTRL click on red dot or spot on panadapter console 53903
                for (int ii = 0; ii < DX_Index; ii++)
                {
                    if (DX_TEXT == textBox1.Text.Substring((ii * LineLength) + 16, 40))// just check freq and dx call sign for match
                    {
                        Debug.WriteLine("GOT DX_TEXT " + ii);

                        if (DX_TEXT.Length > 3)
                        {
                            DX_SELECTED = ii;
                            Debug.WriteLine("DX_TEXT SELECTED NOW " + ii);

                            if (lastselectedON == true)
                            {
                                textBox1.SelectionStart = DX_SELECTED * LineLength;      // start of each dx spot line
                                textBox1.SelectionLength = LineLength;                    // length of each dx spot  line
                            }
                            // textBox1.ScrollToCaret();

                            break; //get out of for loop ii
                        }
                    }

                } // for loop ii



        //    } // pause




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
                else if (BX_Mode[ii] == 15) DXmode = " ft8 ";
                else if (BX_Mode[ii] == 16) DXmode = " mfsk";
                else if (BX_Mode[ii] == 17) DXmode = " feld";
                else if (BX_Mode[ii] == 18) DXmode = " ft4 ";

                else DXmode = "     ";

                bigmessage += (BX_FULLSTRING[ii] + DXmode + " " + (BX_country[ii].PadRight(8)).Substring(0, 8) + ": " + BX_Beam[ii].ToString().PadLeft(3) + "° :" + BX_Age[ii] + "\r\n"); // adds 6

            } // for loop to update dx spot window

            textBox1.Text = bigmessage; // update screen


        } //processTCPMessage1 beacon

        //===================================================================================
        // ke9ns add process age of dx spot
        // this is called as telnet data from the cluster is received

        int UTCAGE_MAX = 25; // overridden by spotage.udspotage

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


                    int UTCAGE = Convert.ToInt32(DX_Age[ii]) + UTCDIFFMIN; // current age difference for DX spots

                    DX_Age[ii] = UTCAGE.ToString(); // age your DX spot


                    int kk = 0; // look at very bottom of list + 1

                    if (SpotAge != null) UTCAGE_MAX = (int)SpotAge.udSpotAge.Value;

                    //  Debug.WriteLine("Spot Age " + UTCAGE_MAX);

                    if (UTCAGE > UTCAGE_MAX) // if its an old SPOT then remove it from the list
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

                            DX_country[kk] = DX_country[kk + xxx];
                            DX_X[kk] = DX_X[kk + xxx];
                            DX_Y[kk] = DX_Y[kk + xxx];
                            DX_Beam[kk] = DX_Beam[kk + xxx];

                            DX_LoTW[kk] = DX_LoTW[kk + xxx]; // LoTW
                            DX_band[kk] = DX_band[kk + xxx];
                            DX_modegroup[kk] = DX_modegroup[kk + xxx];
                            DX_dxcc[kk] = DX_dxcc[kk + xxx];
                            DX_state[kk] = DX_state[kk + xxx];

                            DX_LoTW_RTF[kk] = DX_LoTW_RTF[kk + xxx];
                            DX_LoTW_Status[kk] = DX_LoTW_Status[kk + xxx]; // .201


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

            lastselectedON = false; // turn off any dx spot highlight
            textBox1.DeselectAll();
            processTCPMessage(); // update the dx spotter screen if you click on this box (force an update)

            if ((SP_Active == 3)) // if DX cluster active then test it by sending a CR
            {

                try
                {
                    statusBox.ForeColor = Color.Red;

                    statusBox.Text = "Test Sent <CR>";

                    SP_writer.Write((char)13);
                    SP_writer.Write((char)10);
                }
                catch (Exception)
                {
                    statusBox.Text = "Failed Test";

                }

            } // if connection was supposed to be active

        } // statusBox_Click

        public int DX_SELECTED = 0; // line on the dx spot window that was click on last
        public int LineLength = 105; // was 105
        public string DX_TEXT;
        public bool DX_RX2 = false; // .170 add to allow a CTRL click on a spot to keep it in RX2 VFOB

        //===============================================================================
        public bool beam_selected = false; // ke9ns if you clicked on the beam angle

        public int lastselected = 10000;
        public bool lastselectedON = false;

        public void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
           
          
            chkDXMode.Checked = true;  // the callsign box
          

            if (e.Button == MouseButtons.Left)
            {
                int ii = textBox1.GetCharIndexFromPosition(e.Location);

              //  int ii = textBox1.SelectionStart; // character position in the text you clicked on 

                byte iii = (byte)(ii / LineLength); // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added but now /105 with DX_Beam heading

                //  Debug.WriteLine("testL " + DX_Index + " , "+iii);


                if (iii >= DX_Index) return; // dont allow to click on blank area

                DX_SELECTED = (int)iii;  // store the last line you clicked on to keep highlighted

                if (lastselected == DX_SELECTED * LineLength)
                {
                    textBox1.DeselectAll();
                    lastselectedON = false;
                }
                else
                {
                    lastselected = textBox1.SelectionStart = DX_SELECTED * LineLength;      // start of each dx spot line
                    textBox1.SelectionLength = LineLength;                    // length of each dx spot  line
                    lastselectedON = true;
                }

                DX_TEXT = textBox1.Text.Substring((DX_SELECTED * LineLength) + 16, 40); // just check freq and callsign of dx station

                //   Debug.WriteLine("1DX_SELECTED " + DX_SELECTED + " , "+ DX_TEXT);

                int gg = ii % LineLength;  // get remainder for checking beam heading

                //   Debug.WriteLine("position in line" + gg);

                if (gg > (LineLength - 10)) beam_selected = true; // did user Left click over the beam heading on the dx spot list ?
                else beam_selected = false;


                if ((DXt_Index > iii) && (beacon1 == false))
                {
                    int freq1 = DXt_Freq[iii]; // in hz

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {

                            // console has one of these as well
                            if (DXt_Mode[iii] == 0) console.RX1DSPMode = DSPMode.LSB;
                            else if (DXt_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWU; // was CWL, but making everything CWU now
                            else if (DXt_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGL; // rtty
                            else if (DXt_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGL; // psk
                            else if (DXt_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGU; // jt65
                            else if (DXt_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (DXt_Mode[iii] == 12) console.RX1DSPMode = DSPMode.LSB;
                            else if (DXt_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else if (DXt_Mode[iii] == 15) console.RX1DSPMode = DSPMode.DIGU; // FT8
                            else if (DXt_Mode[iii] == 16) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 17) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 18) console.RX1DSPMode = DSPMode.DIGU; // FT4
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

                            if (DXt_Mode[iii] == 0) console.RX1DSPMode = DSPMode.USB;
                            else if (DXt_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWU;
                            else if (DXt_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (DXt_Mode[iii] == 12) console.RX1DSPMode = DSPMode.USB;
                            else if (DXt_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else if (DXt_Mode[iii] == 15) console.RX1DSPMode = DSPMode.DIGU; // FT8
                            else if (DXt_Mode[iii] == 16) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 17) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 18) console.RX1DSPMode = DSPMode.DIGU; // FT4
                            else console.RX1DSPMode = DSPMode.USB;

                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.USB;
                        }
                    } // USB


                    if (DXt_Mode[iii] == 15) // FT8
                    {

                        if ((DXt_Freq[iii] >= 1840000) && (DXt_Freq[iii] <= 1865000))
                        {
                            freq1 = 1840000; // because FT8 does the freq moving with the 2.5khz slice, just put 0hz on the start of the slice
                        }
                        else if ((DXt_Freq[iii] >= 3573000) && (DXt_Freq[iii] <= 3575500))
                        {
                            freq1 = 3573000;
                        }
                        else if ((DXt_Freq[iii] >= 7074000) && (DXt_Freq[iii] <= 7076500))
                        {
                            freq1 = 7074000;
                        }
                        else if ((DXt_Freq[iii] >= 10136000) && (DXt_Freq[iii] <= 10138500))
                        {
                            freq1 = 10136000;
                        }
                        else if ((DXt_Freq[iii] >= 14074000) && (DXt_Freq[iii] <= 14076500))
                        {
                            freq1 = 14074000;
                        }
                        else if ((DXt_Freq[iii] >= 18100000) && (DXt_Freq[iii] <= 18102500))
                        {
                            freq1 = 18100000;
                        }
                        else if ((DXt_Freq[iii] >= 21074000) && (DXt_Freq[iii] <= 21076500))
                        {
                            freq1 = 21074000;
                        }
                        else if ((DXt_Freq[iii] >= 24915000) && (DXt_Freq[iii] <= 24917500))
                        {
                            freq1 = 24915000;
                        }
                        else if ((DXt_Freq[iii] >= 28074000) && (DXt_Freq[iii] <= 28076500))
                        {
                            freq1 = 28074000;
                        }
                        else if ((DXt_Freq[iii] >= 50313000) && (DXt_Freq[iii] <= 50315500))
                        {
                            freq1 = 50313000;
                        }
                        else if ((DXt_Freq[iii] >= 50323000) && (DXt_Freq[iii] <= 50325500)) // FT8 DXt freq
                        {
                            freq1 = 50323000;
                        }



                        DXt_Mode2[iii] = 0; // no split in FT8
                    } // FT8
                    else if (DXt_Mode[iii] == 18) // FT4
                    {

                        if ((DXt_Freq[iii] >= 3568000) && (DXt_Freq[iii] <= 3570500))
                        {
                            freq1 = 3568000;
                        }
                        else if ((DXt_Freq[iii] >= 3575000) && (DXt_Freq[iii] <= 3577500))
                        {
                            freq1 = 3575000;
                        }
                        else if ((DXt_Freq[iii] >= 7047000) && (DXt_Freq[iii] <= 7049500))
                        {
                            freq1 = 7047000;
                        }
                        else if ((DXt_Freq[iii] >= 10140000) && (DXt_Freq[iii] <= 10142500))
                        {
                            freq1 = 10140000;
                        }
                        else if ((DXt_Freq[iii] >= 14080000) && (DXt_Freq[iii] <= 14082500))
                        {
                            freq1 = 14080000;
                        }
                        else if ((DXt_Freq[iii] >= 18104000) && (DXt_Freq[iii] <= 18106500))
                        {
                            freq1 = 18104000;
                        }
                        else if ((DXt_Freq[iii] >= 21140000) && (DXt_Freq[iii] <= 21142500))
                        {
                            freq1 = 21140000;
                        }
                        else if ((DXt_Freq[iii] >= 24919000) && (DXt_Freq[iii] <= 24921500))
                        {
                            freq1 = 24919000;
                        }
                        else if ((DXt_Freq[iii] >= 28180000) && (DXt_Freq[iii] <= 28182500))
                        {
                            freq1 = 28180000;
                        }
                        else if ((DXt_Freq[iii] >= 50318000) && (DXt_Freq[iii] <= 50320500))
                        {
                            freq1 = 50318000;
                        }
                        

                        DXt_Mode2[iii] = 0; // no split in FT8
                    } // FT4


                    console.VFOAFreq = (double)freq1 / 1000000; // convert to MHZ

                    if (chkDXMode.Checked == true)
                    {

                        if (DXt_Mode2[iii] != 0)
                        {

                            console.VFOBFreq = (double)(freq1 + DXt_Mode2[iii]) / 1000000; // convert to MHZ
                            console.chkVFOSplit.Checked = true; // turn on  split

                            Debug.WriteLine("split here" + (freq1 + DXt_Mode2[iii]));

                        }
                        else
                        {
                            console.chkVFOSplit.Checked = false; // turn off split

                        }


                    } // DXtmode checked

                    if (beam_selected == true)    // ke9ns add send hygain rotor command to DDUtil via the CAT port setup in PowerSDR
                    {
                        Debug.WriteLine("BEAM HEADING TRANSMIT");

                        console.spotDDUtil_Rotor = "AP1" + DXt_Beam[iii].ToString().PadLeft(3, '0') + ";";
                        console.spotDDUtil_Rotor = "AM1;";

                    } //  if (chkBoxRotor.Checked == true)
                    button2.Focus();


                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range

                else if ((BX1_Index > iii) && (beacon1 == true))
                {
                    int freq1 = BX_Freq[iii];

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {
                            if (BX_Mode[iii] == 0) console.RX1DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWU; // was CWL
                            else if (BX_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else if (BX_Mode[iii] == 15) console.RX1DSPMode = DSPMode.DIGU; // ft8
                            else if (BX_Mode[iii] == 16) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 17) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 18) console.RX1DSPMode = DSPMode.DIGU; // ft4
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
                            else if (BX_Mode[iii] == 15) console.RX1DSPMode = DSPMode.DIGU; // ft8
                            else if (BX_Mode[iii] == 16) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 17) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 18) console.RX1DSPMode = DSPMode.DIGU; // ft4
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
                        // console.spotDDUtil_Rotor = ";"; // stop motion
                        console.spotDDUtil_Rotor = "AM1;"; // start moving

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

                    if (DXt_Index > iii)
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
                else if ((SP4_Active == 0) && (beacon1 == true))// only process lookup if not processing a new spot which might cause issue
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


            } // right click (above)
              //  else if (me.Button == System.Windows.Forms.MouseButtons.Middle) 
            else if (e.Button == MouseButtons.Middle) // // ke9ns add .202 sends spot to VFOB instead of VFOA
            {
                  lastselected = 10000;

               
               //  const int WM_VSCROLL = 0x115;
              //  const int SB_ENDSCROLL = 8;
                Console.SendMessageW(this.Handle, 0x115, (IntPtr)0x08, this.Handle); // to prevent a silly windows scroll feature that normally comes from a mouse wheel click



                int ii = textBox1.GetCharIndexFromPosition(e.Location);// character position in the text you clicked on 

                byte iii = (byte)(ii / LineLength); // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added but now /105 with DX_Beam heading

               //   Debug.WriteLine("testM " + DX_Index + " , "+iii);

                if (iii >= DXt_Index) return; // dont allow to click on blank area

                DX_SELECTED = (int)iii;  // store the last line you clicked on to keep highlighted

                if (lastselected == DX_SELECTED * LineLength)
                {
                    textBox1.DeselectAll();
                    lastselectedON = false;
                  
                }
                else
                {
                    lastselected = textBox1.SelectionStart = DX_SELECTED * LineLength;      // start of each dx spot line
                    textBox1.SelectionLength = LineLength;                    // length of each dx spot  line
                    lastselectedON = true;
                  

                }

                DX_TEXT = textBox1.Text.Substring((DX_SELECTED * LineLength) + 16, 40); // just check freq and callsign of dx station

                //   Debug.WriteLine("1DX_SELECTED " + DX_SELECTED + " , "+ DX_TEXT);

                int gg = ii % LineLength;  // get remainder for checking beam heading

                //   Debug.WriteLine("position in line" + gg);

                if (gg > (LineLength - 10)) beam_selected = true; // did user Left click over the beam heading on the dx spot list ?
                else beam_selected = false;


                if ((DXt_Index > iii) && (beacon1 == false))
                {
                    int freq1 = DXt_Freq[iii]; // in hz

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {

                            // console has one of these as well
                            if (DXt_Mode[iii] == 0) console.RX2DSPMode = DSPMode.LSB;
                            else if (DXt_Mode[iii] == 1) console.RX2DSPMode = DSPMode.CWU; // was CWL, but making everything CWU now
                            else if (DXt_Mode[iii] == 2) console.RX2DSPMode = DSPMode.DIGL; // rtty
                            else if (DXt_Mode[iii] == 3) console.RX2DSPMode = DSPMode.DIGL; // psk
                            else if (DXt_Mode[iii] == 4) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 5) console.RX2DSPMode = DSPMode.DIGU; // jt65
                            else if (DXt_Mode[iii] == 6) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 7) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 8) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 9) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 10) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 11) console.RX2DSPMode = DSPMode.FM;
                            else if (DXt_Mode[iii] == 12) console.RX2DSPMode = DSPMode.LSB;
                            else if (DXt_Mode[iii] == 13) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 14) console.RX2DSPMode = DSPMode.SAM;
                            else if (DXt_Mode[iii] == 15) console.RX2DSPMode = DSPMode.DIGU; // FT8
                            else if (DXt_Mode[iii] == 16) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 17) console.RX2DSPMode = DSPMode.DIGL;
                            else if (DXt_Mode[iii] == 18) console.RX2DSPMode = DSPMode.DIGU; // FT4
                            else console.RX2DSPMode = DSPMode.LSB;

                        }
                        else
                        {
                            console.RX2DSPMode = DSPMode.LSB;
                        }
                    } // LSB
                    else
                    {
                        if (chkDXMode.Checked == true)
                        {

                            if (DXt_Mode[iii] == 0) console.RX2DSPMode = DSPMode.USB;
                            else if (DXt_Mode[iii] == 1) console.RX2DSPMode = DSPMode.CWU;
                            else if (DXt_Mode[iii] == 2) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 3) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 4) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 5) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 6) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 7) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 8) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 9) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 10) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 11) console.RX2DSPMode = DSPMode.FM;
                            else if (DXt_Mode[iii] == 12) console.RX2DSPMode = DSPMode.USB;
                            else if (DXt_Mode[iii] == 13) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 14) console.RX2DSPMode = DSPMode.SAM;
                            else if (DXt_Mode[iii] == 15) console.RX2DSPMode = DSPMode.DIGU; // FT8
                            else if (DXt_Mode[iii] == 16) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 17) console.RX2DSPMode = DSPMode.DIGU;
                            else if (DXt_Mode[iii] == 18) console.RX2DSPMode = DSPMode.DIGU; // FT4
                            else console.RX2DSPMode = DSPMode.USB;

                        }
                        else
                        {
                            console.RX2DSPMode = DSPMode.USB;
                        }
                    } // USB


                    if (DXt_Mode[iii] == 15) // FT8
                    {

                        if ((DXt_Freq[iii] >= 1840000) && (DXt_Freq[iii] <= 1865000))
                        {
                            freq1 = 1840000; // because FT8 does the freq moving with the 2.5khz slice, just put 0hz on the start of the slice
                        }
                        else if ((DXt_Freq[iii] >= 3573000) && (DXt_Freq[iii] <= 3575500))
                        {
                            freq1 = 3573000;
                        }
                        else if ((DXt_Freq[iii] >= 7074000) && (DXt_Freq[iii] <= 7076500))
                        {
                            freq1 = 7074000;
                        }
                        else if ((DXt_Freq[iii] >= 10136000) && (DXt_Freq[iii] <= 10138500))
                        {
                            freq1 = 10136000;
                        }
                        else if ((DXt_Freq[iii] >= 14074000) && (DXt_Freq[iii] <= 14076500))
                        {
                            freq1 = 14074000;
                        }
                        else if ((DXt_Freq[iii] >= 18100000) && (DXt_Freq[iii] <= 18102500))
                        {
                            freq1 = 18100000;
                        }
                        else if ((DXt_Freq[iii] >= 21074000) && (DXt_Freq[iii] <= 21076500))
                        {
                            freq1 = 21074000;
                        }
                        else if ((DXt_Freq[iii] >= 24915000) && (DXt_Freq[iii] <= 24917500))
                        {
                            freq1 = 24915000;
                        }
                        else if ((DXt_Freq[iii] >= 28074000) && (DXt_Freq[iii] <= 28076500))
                        {
                            freq1 = 28074000;
                        }
                        else if ((DXt_Freq[iii] >= 50313000) && (DXt_Freq[iii] <= 50315500))
                        {
                            freq1 = 50313000;
                        }
                        else if ((DXt_Freq[iii] >= 50323000) && (DXt_Freq[iii] <= 50325500)) // FT8 DXt freq
                        {
                            freq1 = 50323000;
                        }


                        DXt_Mode2[iii] = 0; // no split in FT8
                    } // FT8
                    else if (DXt_Mode[iii] == 18) // FT4
                    {

                        if ((DXt_Freq[iii] >= 3568000) && (DXt_Freq[iii] <= 3570500))
                        {
                            freq1 = 3568000;
                        }
                        else if ((DXt_Freq[iii] >= 3575000) && (DXt_Freq[iii] <= 3577500))
                        {
                            freq1 = 3575000;
                        }
                        else if ((DXt_Freq[iii] >= 7047000) && (DXt_Freq[iii] <= 7049500))
                        {
                            freq1 = 7047000;
                        }
                        else if ((DXt_Freq[iii] >= 10140000) && (DXt_Freq[iii] <= 10142500))
                        {
                            freq1 = 10140000;
                        }
                        else if ((DXt_Freq[iii] >= 14080000) && (DXt_Freq[iii] <= 14082500))
                        {
                            freq1 = 14080000;
                        }
                        else if ((DXt_Freq[iii] >= 18104000) && (DXt_Freq[iii] <= 18106500))
                        {
                            freq1 = 18104000;
                        }
                        else if ((DXt_Freq[iii] >= 21140000) && (DXt_Freq[iii] <= 21142500))
                        {
                            freq1 = 21140000;
                        }
                        else if ((DXt_Freq[iii] >= 24919000) && (DXt_Freq[iii] <= 24921500))
                        {
                            freq1 = 24919000;
                        }
                        else if ((DXt_Freq[iii] >= 28180000) && (DXt_Freq[iii] <= 28182500))
                        {
                            freq1 = 28180000;
                        }
                        else if ((DXt_Freq[iii] >= 50318000) && (DXt_Freq[iii] <= 50320500))
                        {
                            freq1 = 50318000;
                        }


                        DXt_Mode2[iii] = 0; // no split in FT8
                    } // FT4


                    console.VFOBFreq = (double)freq1 / 1000000; // convert to MHZ

                    if (console.setupForm != null && console.setupForm.chkRX2AutoOn.Checked == true && console.chkRX2.Checked == false && console.chkRX2.Visible) // .229
                    {
                        console.chkRX2.Checked = true;
                        if (console.setupForm.chkRX2AutoVAC2.Checked)
                        {
                            console.setupForm.chkVAC2Enable.Checked = true; // .230
                        }
                    }

                    if (chkDXMode.Checked == true)
                    {

                        if (DXt_Mode2[iii] != 0)
                        {

                         //   console.VFOBFreq = (double)(freq1 + DX_Mode2[iii]) / 1000000; // convert to MHZ
                         //   console.chkVFOSplit.Checked = true; // turn on  split

                            Debug.WriteLine("split here" + (freq1 + DXt_Mode2[iii]));

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
                        console.spotDDUtil_Rotor = "AM1;";

                    } //  if (chkBoxRotor.Checked == true)
                    button2.Focus();


                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range
                else if ((BX1_Index > iii) && (beacon1 == true))
                {
                    int freq1 = BX_Freq[iii];

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {
                            if (BX_Mode[iii] == 0) console.RX2DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 1) console.RX2DSPMode = DSPMode.CWU; // was CWL
                            else if (BX_Mode[iii] == 2) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 3) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 4) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 5) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 6) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 7) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 8) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 9) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 10) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 11) console.RX2DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX2DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 13) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 14) console.RX2DSPMode = DSPMode.SAM;
                            else if (BX_Mode[iii] == 15) console.RX2DSPMode = DSPMode.DIGU; // ft8
                            else if (BX_Mode[iii] == 16) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 17) console.RX2DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 18) console.RX2DSPMode = DSPMode.DIGU; // ft4
                            else console.RX2DSPMode = DSPMode.LSB;


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

                            if (BX_Mode[iii] == 0) console.RX2DSPMode = DSPMode.USB;
                            else if (BX_Mode[iii] == 1) console.RX2DSPMode = DSPMode.CWU;
                            else if (BX_Mode[iii] == 2) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 3) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 4) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 5) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 6) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 7) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 8) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 9) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 10) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 11) console.RX2DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX2DSPMode = DSPMode.USB;
                            else if (BX_Mode[iii] == 13) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 14) console.RX2DSPMode = DSPMode.SAM;
                            else if (BX_Mode[iii] == 15) console.RX2DSPMode = DSPMode.DIGU; // ft8
                            else if (BX_Mode[iii] == 16) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 17) console.RX2DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 18) console.RX2DSPMode = DSPMode.DIGU; // ft4
                            else console.RX2DSPMode = DSPMode.USB;

                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.USB;
                        }
                    }


                    console.VFOBFreq = (double)freq1 / 1000000; // convert to MHZ

                    if (console.setupForm != null && console.setupForm.chkRX2AutoOn.Checked == true && console.chkRX2.Checked == false && console.chkRX2.Visible) // .229
                    {
                        console.chkRX2.Checked = true;
                        if (console.setupForm.chkRX2AutoVAC2.Checked)
                        {
                            console.setupForm.chkVAC2Enable.Checked = true; // .230
                        }
                    }

                    if (chkDXMode.Checked == true)
                    {

                        if (BX_Mode2[iii] != 0)
                        {

                         //   console.VFOBFreq = (double)(freq1 + BX_Mode2[iii]) / 1000000; // convert to MHZ
                         //   console.chkVFOSplit.Checked = true; // turn on  split

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
                        // console.spotDDUtil_Rotor = ";"; // stop motion
                        console.spotDDUtil_Rotor = "AM1;"; // start moving

                    } //  if (chkBoxRotor.Checked == true)
                    button2.Focus();


                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range (BEACON)




            } // middle wheel button (above)


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
            if (SP_Active != 0)
            {

                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;

                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    Darken(); // adjust map image bright/dark and grayline

                    console.picDisplay.BackgroundImage = MAP;

                } // SUN or GRAY LINE checked

            } // dx spot on

            Map_Last = 1;

        } // chkSUN_CheckedChanged



        //=========================================================================================
        private void chkGrayLine_CheckedChanged(object sender, EventArgs e)
        {
          

            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image held in skin1 if the SUN and grayline are both turned OFF
            }

            if (SP_Active != 0)
            {

                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {
                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage; // store original background in skin1

                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    Darken(); // adjust map image bright/dark and grayline

                    console.picDisplay.BackgroundImage = MAP; // replace screen background with equarectangular map

                } // only do if SUN or GRAY LINE checked

            } // dx spot active

            Map_Last = 1;

        } // chkGrayLine_CheckedChanged




        public bool mapon = false; // use to turn on world map when PowerSDR launched
        public bool dxon = false; // use to turn DX spotting on when PowerSDR launched
        public bool voaon = false; // use to turn voacap back on when PowerSDR launched

        private static DisplayMode LastDisplayMode = 0;
        private static int LastDisplayMap = 0;

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
                mapon = true;
                chkMapOn.Checked = true;

               
                if (chkPanMode.Checked == true) Display.map = 1; // special panafall mode (80 - 20)
                else
                {
                    if (console.comboDisplayMode.Text != "Panafall8020")
                    {
                        Display.map = 0;                 // tell display program to got back to standard panafall mode
                    }
                }

                btnTrack.Text = "Track ON";

                LastDisplayMode = Display.CurrentDisplayMode; // save the display mode that you were in before you turned on special panafall mode

                if (chkPanMode.Checked == true) Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                Display.GridOff = 1; // Force Gridlines off but dont change setupform setting

                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    Darken(); // adjust map image bright/dark and grayline

                    console.picDisplay.BackgroundImage = MAP;

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


                SP5_Active = 1; // turn on track map (sun, grayline, voacap, or beacon)
                UTCLAST2 = 0;

                VOATHREAD1 = false;

                Thread t = new Thread(new ThreadStart(TrackSun));  // turn on track map (sun, grayline, voacap, or beacon)

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                textBox1.Text = "Clicked to Turn ON GrayLine Sun Tracker\r\n";

                t.Name = "Track Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();


                if (SP5_Active == 0) textBox1.Text += "ON but just turned OFF1";
                //

            }
            else // map was already on so turn off
            {
                //  console.propMenuItem1.Enabled = false;

                chkMapOn.Checked = false;

                SP5_Active = 0;                     // turn off tracking

                if (console.comboDisplayMode.Text != "Panafall8020")
                {
                    Display.map = 0;                 // tell display program to got back to standard panafall mode
                }

                if (chkPanMode.Checked == true) Display.CurrentDisplayMode = LastDisplayMode;

                if (console.setupForm.gridBoxTS.Checked == true) Display.GridOff = 1; // put gridlines back the way they were
                else Display.GridOff = 0; // gridlines ON

                btnTrack.Text = "Track/Map";

                textBox1.Text += "Click to turn OFF GrayLine Sun Tracking\r\n";

                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image

               
            } // turn Tracking  off

            Debug.WriteLine("BTNTRK " + chkMapOn.Checked);

        } //  btnTrack_Click





        public static int UTCLAST2 = 0;                                        // for Sun Tracker

        public static double UTC100A = 0;                                       // UTC time in hours.100th of min for grayline

        public static double UTC100 = 0;                                       // UTC time as a factor of 1 (1=2400 UTC, 0= 0000UTC) 
        public static double UTCDAY = 0;                                       // UTC day as a factor of 1 (0=1day, 1=365)
                                                                               //   public static int[] Gray_X = new int[400];                             // for mapping the gray line x and y 
                                                                               //   public static int[] Gray_Y = new int[400];

        public static int Sun_Right = 0;
        public static int Sun_Left = 0;

        // GrayLine_Pos[,] = x pixel for a particular y pixel
        // zz = 0 to 1000 is the Y pixel position
        // ww= 0,1, or 2  edges for this y pixel  
        // Darken calls Lighten routines to draw data directly onto bitmap
        public static int[,] GrayLine_Pos = new int[1000, 3];                      // DARK [zz,ww] = is lat 180 to 0 to -180 (top to bottom) (left to right)
        public static int[] GrayLine_Pos2 = new int[1000];                         // data for DARK areas on map 0=center dark, 1=left, 2=right
      
        public static int[,] GrayLine_Pos1 = new int[1000, 3];                     // DUSK [zz,ww]  =is lat 180 to 0 to -180 (top to bottom) (left to right)
        public static int[] GrayLine_Pos3 = new int[1000];                         // data for Dusk areas on map 0=center dark, 1=left, 2=right

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
        public static bool GRAYDONE = false; // true = grayline computed

        public static int suncounter = 0; // for space weather
        public static int SFI = 0;       // for Space weather
        public static int Last_SFI = 0; // .197
        public static int SN = 0;        // for Space weather
        public static int Last_SN = 0; // .197
        public static int Aindex = 0;    // for Space weather
        public static int Last_Aindex = 0; // .197
        public static int Kindex = 0;    // for Space weather
        public static string RadioBlackout = " ";
        public static string GeoBlackout = " ";
        private string serverPath;       // for Space weather



        //====================================================================================================
        //====================================================================================================
        // ke9ns add: compute angle to sun (for dusk grayline) // equations in this section below provided by Roland Leigh
        private int SUNANGLE(double lat, double lon)  // ke9ns: Thread opeation (runs in en-us culture) opens internet connection to generate list of dx spots
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

            //  g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //  g.CompositingMode = CompositingMode.SourceOver;
            //  g.CompositingQuality = CompositingQuality.HighQuality;
            //   g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //  g.SmoothingMode = SmoothingMode.HighQuality;
            //   g.PixelOffsetMode = PixelOffsetMode.HighQuality;


            ISSTimer.Restart();

            ISSCount = 0;

            while (ISSCount < 20) // was 360 for 90min orbit period 
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
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;


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
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

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

                          if (g != null)  g.DrawLine(OrgPen, ISS_X, ISS_Y, ISS_X + 1, ISS_Y + 1);


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
        public static Image tool = null; // holds bitmap image for SUN and GRAY LINE

        private static int[] spots = new int[100];  // holder for all the spots current on your entire band.

        public static int VFOLOW = 0;   // set in console rx1band for use in the mapper
        public static long VFOHIGH = 0;

        string[] country = new string[200];
        string[] call = new string[200];

        int[] yy = new int[200];  // increments the y axis down to allow multiple station names under a red dot

        const double DEG_to_RAD = Math.PI / 180.0; // 0.01745329251   converting angles to radians
        const double RAD_to_DEG = 180.0 / Math.PI; // 57.2957795131   converting radians back to angles

        static int MB = 252; // ke9ns add for map brightness
                             // public int MB2 = 0; // ke9ns value used for initial map brightness

        static int MBG = 0; // .159 grayline map brightness

        public int hpos = 0; // horizontal pos of day of week indicator .100

        public int lastholdY = 0; // ke9ns add .193
        Graphics g;

        public bool TrackOFF = false; // ke9ns add .197

        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        private void TrackSun()  // ke9ns Thread opeation (runs in en-us culture) To create and draw Sun and/or Grayline Track
        {

            TrackOFF = false; 

            textBox1.Text += "Map turning ON \r\n";

            //      if (VOATHREAD1 == true) return; // form closing so close now

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

            // ftp://ftp.swpc.noaa.gov/pub/latest/wwv.txt obsolete as of Dec 9th, 2017 now uses http

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



            if (Console.noaaON == 0) // only do this if space weather is OFF on the main console window
            {
                textBox1.Text += "Attempt login to:  NOAA Space Weather Prediction Center \r\n";

                NOAA(); // get noaa space data

            }

            textBox1.Text += "NOAA Download complete \r\n";

            if (SP5_Active == 1) textBox1.Text += "Map turned ON \r\n";

            //--------------------------------------------------------------------------------------------
            // stay in this thread loop until you turn off tracking
            for (; SP5_Active == 1;) // turn on track map (sun, grayline, voacap, or beacon)
            {

                Thread.Sleep(100);

                if (SP5_Active == 0)
                {
                    UTCLAST2 = 0;
                    continue; // tracking turned OFF
                }


                // ke9ns add Check if you need a VOACAP update, but only if your not in the middle of doing a VOACAP update and the VOACAP is turned ON
                if ((checkBoxMUF.Checked == true) && (VOARUN == false) && (VOATHREAD1 == false))  // 
                {

                    console.VOAOFF = false;

                    if ((int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")) != console.last_MHZ)
                    {
                        

                        if (((int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")) > 0) && ((int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")) < 30))
                        {
                            console.last_MHZ = (int)double.Parse(console.txtVFOAFreq.Text.Replace(",", "."));

                            Debug.WriteLine("lastmhz " + console.last_MHZ);

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


                } //  if ((checkBoxMUF.Checked == true) && (VOARUN == false) && (VOATHREAD1 == false)) 
                else
                {
                    console.VOAOFF = true;
                }

                if (((beacon4 == true)) || ((chkSUN.Checked == true) || (chkGrayLine.Checked == true)) &&
                       ((Display.CurrentDisplayMode == DisplayMode.PANADAPTER) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL8020))
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


                    if (chkGrayLine.Checked == true)
                    {
                        GRAYLINE = true; // activate display

                    }
                    else
                    {
                        GRAYLINE = false;
                        GRAYDONE = false;

                    }


                    // Do a SUN, GRAYLINE, or DX country/callsign update

                    // Check for TIME CHANGE
                    // Check for GrayLine COLOR change
                    // Check for DX spot list change
                    // Check in any checkboxes changed state
                    // Check if the number of spots on map changed (DXK is the # of spots on the current panadapter)
                    // Check for Transmitting (dont update if transmitting)
                    // check if changed modes


                    if (
                        (!console.MOX) && ((UTCNEW != UTCLAST2) || (Map_Last > 0) ||
                    (((DX_Index != DX_Last) || (Console.DXK != DXK_Last) || (console.RX1Band != RX1Band_Last)) && (SP8_Active == 1)))
                    || ((beacon4 == true)) || ((Display.CurrentDisplayMode != LastDisplayMode)) || (Display.map != LastDisplayMap)
                    || (console.mouseholdY != lastholdY) //ke9ns .193 reupdate map when you move SS1 

                    )  // 
                    {
                        lastholdY = console.mouseholdY; //ke9ns  .193

                        LastDisplayMap = Display.map;

                        LastDisplayMode = Display.CurrentDisplayMode;

                        Debug.WriteLine("Update DX Spots on map=================");


                        DXK_Last = Console.DXK;

                        DX_Last = DX_Index;                    // if the DX spot list changed

                        RX1Band_Last = console.RX1Band;


                        if ((UTCNEW != UTCLAST2) || (Map_Last == 1))
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



                            if ((GRAYLINE == true)) // compute grayline below to display in the LIGHTEN() routine
                            {

                            
                                int tt = 0;
                                bool check_for_light = true; // true = in the dark, so looking for light, false = in the light, so looking for the dark
                                int tempsun_ang = 0; // temp holder for sun angle


                                //----------------------------------------------------------------------------------
                                //----------------------------------------------------------------------------------
                                // check for Dark (Right side of map)
                                //----------------------------------------------------------------------------------
                                //----------------------------------------------------------------------------------
                                int qq = 0;       // index for accumulating lat edges
                                int ww = 0;       // 0=no edges found, 1=1 edge found, 2=2 edges found

                                for (double lat = 96.0; lat >= -96.0; lat = lat - 0.5)  // 0.5 horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                {

                                    if ((SUNANGLE(lat, -180.0) >= 96) && (SUNANGLE(lat, 180.0) >= 96)) tt = 1; // dark on edges of screen 
                                    else tt = 0; // light on at least 1 side

                                    zz = (int)Math.Round(((double)qq / 360.0 * (double)Sun_WidthY1) + (double)Sun_Top1, MidpointRounding.AwayFromZero); // 360 = number of latitude points, determine the y pixel for this latitude grayline


                                    GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 1] = 0;

                                    if (SUNANGLE(lat, 0.0) < 96) check_for_light = false; // your in light so check for dark
                                    else check_for_light = true; // >= 96 your in dark so check for light


                                    for (double lon = 0.0; lon <= 180.0; lon = lon + 0.05)   // 0.5
                                    {
                                        tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120


                                        if (check_for_light == true) // in dark, looking for light
                                        {

                                            if ((tempsun_ang < 96)) // found light
                                            {

                                                GrayLine_Pos[zz, ww] = (int)Math.Round(((lon + 180.0) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline

                                                GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels


                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 1.0; // 40.0  jump a little to save time

                                                check_for_light = false; // now in light so check for dark

                                            } // found light

                                        } // your in dark so check for light
                                        else // in light so check for dark
                                        {

                                            if ((tempsun_ang >= 96))  // in Dark (found it)
                                            {

                                                GrayLine_Pos[zz, ww] = (int)Math.Round(((lon + 180.0) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline

                                                GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels

                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 1.0;         // 40.0 jump a little to save time
                                                check_for_light = true; // in dark so now check for light

                                            } // found dark


                                        }// in light so check for dark


                                    } // for lon (right side of map)

                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------
                                    // check for DARK (left side of map) neg long
                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------

                                    if (ww < 2) // still need at least 1 edge (maybe 2)
                                    {

                                        if (SUNANGLE(lat, 0.0) < 96) check_for_light = false; // your in light so check for dark
                                        else check_for_light = true; // >= 90 your in dark so check for light

                                        for (double lon = 0.0; lon >= -180.0; lon = lon - 0.05)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                        {
                                            tempsun_ang = SUNANGLE(lat, lon);

                                            if (check_for_light == true)
                                            {

                                                if ((tempsun_ang < 96)) // found light
                                                {
                                                    GrayLine_Pos[zz, ww] = (int)Math.Round(((180.0 + lon) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline
                                                    GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

                                                    ww++;
                                                    if (ww == 2) break;      // if we have 2 edge then done

                                                    lon = lon - 1.0;           // 40.0 jump a little to save time
                                                    check_for_light = false; // now in light so check for dark

                                                } // found light

                                            } // your in dark so check for light
                                            else // in light so check for dark
                                            {

                                                if (tempsun_ang >= 96) // in Dark (found it)
                                                {
                                                    GrayLine_Pos[zz, ww] = (int)Math.Round(((180.0 + lon) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline

                                                    GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

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
                                        GrayLine_Pos[zz + 2, 0] = GrayLine_Pos[zz + 1, 0] = GrayLine_Pos[zz, 0] = 0;
                                        GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1] = 0;
                                    }
                                    else if (ww == 1)
                                    {
                                        GrayLine_Pos[zz + 2, 0] = GrayLine_Pos[zz + 1, 0] = GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 0] + GrayLine_Pos[zz, 1];
                                        GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1] = GrayLine_Pos[zz, 0];
                                    }

                                    ww = 0; // start over for next lat

                                    if (tt == 1) // if gray on both edges then figure out which is which and signal display
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
                                            GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 0;             // gray in center of map, (standard)
                                        }

                                    }
                                    else
                                    {
                                        GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 0;             // gray in center of map, (standard)
                                    }

                                    qq++; // get next lat

                                } //  for (int lat = 90;lat >= -90;lat--)   horizontal lines top to bottom (North)90 to 0 to -90 (South)




                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                // check for dusk (right side first) pos long
                                //-------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                qq = 0;
                                ww = 0;

                                for (double lat = 90.0; lat >= -90.0; lat = lat - 0.5)  // 0.5 horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                {

                                    if ((SUNANGLE(lat, -180.0) >= 90) && (SUNANGLE(lat, 180.0) >= 90)) tt = 1; // dark on edges of screen 
                                    else tt = 0; // light on at least 1 side

                                    zz = (int)Math.Round(((double)qq / 360.0 * (double)Sun_WidthY1) + (double)Sun_Top1, MidpointRounding.AwayFromZero); // 360 = number of latitude points, determine the y pixel for this latitude grayline


                                    GrayLine_Pos1[zz, 0] = GrayLine_Pos1[zz, 1] = 0;

                                    if (SUNANGLE(lat, 0.0) < 90) check_for_light = false; // your in light so check for dark
                                    else check_for_light = true; // >= 96 your in dark so check for light


                                    for (double lon = 0.0; lon <= 180.0; lon = lon + 0.05)   // 0.5
                                    {
                                        tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120


                                        if (check_for_light == true) // in dark, looking for light
                                        {

                                            if ((tempsun_ang < 90)) // found light
                                            {

                                                GrayLine_Pos1[zz, ww] = (int)Math.Round(((lon + 180.0) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline

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

                                                GrayLine_Pos1[zz, ww] = (int)Math.Round(((lon + 180.0) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline

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
                                    // check for gray (left side of map) neg long
                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------

                                    if (ww < 2) // still need at least 1 edge (maybe 2)
                                    {

                                        if (SUNANGLE(lat, 0.0) < 90) check_for_light = false; // your in light so check for dark
                                        else check_for_light = true; // >= 90 your in dark so check for light

                                        for (double lon = 0.0; lon >= -180.0; lon = lon - 0.05)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                        {
                                            tempsun_ang = SUNANGLE(lat, lon);

                                            if (check_for_light == true)
                                            {

                                                if ((tempsun_ang < 90)) // found light
                                                {
                                                    GrayLine_Pos1[zz, ww] = (int)Math.Round(((180.0 + lon) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline
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
                                                    GrayLine_Pos1[zz, ww] = (int)Math.Round(((180.0 + lon) / 360.0 * (double)Sun_Width) + (double)Sun_Left, MidpointRounding.AwayFromZero); // determine x pixel for this longitude grayline

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

                                GRAYDONE = true;

                            } // GRAYLINE = true

                        } //  if ( (UTCNEW != UTCLAST2)  )  time changeed or color change


                        if ((Map_Last == 2) && (SP4_Active == 0)) updatemapspots(); // if you need a map update make sure your not in the middle of updating your dx spot list

                        Map_Last = 0;

                        if (SP5_Active == 0)
                        {
                            UTCLAST2 = 0;
                            continue;
                        }


                        //-------------------------------------------------------------------------------------------------
                        //-------------------------------------------------------------------------------------------------
                        // draw sun tracker and gray line
                        //-------------------------------------------------------------------------------------------------
                        //-------------------------------------------------------------------------------------------------


                        Darken();


                        //   if ((Console.DXR == 0))  MAP = new Bitmap(Map_image); // load up Map image
                        //   else MAP = new Bitmap(Map_image2); // load up Map image


                        g = Graphics.FromImage(MAP); //  MAP = Lighten(new Bitmap(Map_image), MB);  darken or light world map first


                        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        //   g.CompositingMode = CompositingMode.SourceOver;
                        //   g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;


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



                        //------------------------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------------------------

                        //=============================
                        // W=1605 full HD, W=1447 full HD with bandstack screen open
                        // 1440 min / day
                        // UTC time is time in London
                        // international date line is off of Alaska coast which is 720min from UTC
                        // day change takes place wherever 0 utc is (0300UTC= 0 Day line is 180min back)(2300UTC= 0 Day line is 1380min back)
                        //
                        Pen p44 = new Pen(Color.White, 1.0f);                  // ke9ns add color for vert line for TIME UTC pos

                        bool day = false; // ke9ns day of week indicator .100
                        double ScreenCenter = (((Sun_Right - Sun_Left) / 2.0) + Sun_Left); //  center of the map
                        double ScreenWidth = ((Sun_Right - Sun_Left)); // the width of the map area of picDisplay

                        double minfact = ScreenWidth / 1440.0; // find scale factor

                        int mins = UTCD.Hour * 60 + UTCD.Minute; // minutes of the current UTC day (ie current time in London): 0 UTC would place the DAY line over London

                        if (mins >= 720)
                        {
                            day = true; // <- UTCDAY | UTCDAY+1 ->
                        }
                        else day = false; // <- UTCDAY-1 | UTCDAY ->

                        int minpos = (int)((double)mins * minfact); // scale the UTC Day line to the Display area

                        int minpos2 = (int)(ScreenCenter - minpos); // 

                        int minpos1 = minpos2;

                        if (minpos2 <= Sun_Left) minpos1 = (int)ScreenWidth + minpos1; // move over to the right side

                        int temp = console.picDisplay.Height; // current height of the display area (no matter if pan or panafall or 8020)

                     

                        Debug.WriteLine("HPOS " + hpos + " , " + console.mouseholdY + " , " + temp);
                            
                        int hpos1 = (int)((double)hpos * (507.0 / temp)); // .100 hpos1 is the actual bottom of the picDisplay area after it has been resized (i.e. waterfall, 2nd receiver) 507=height of map before resizing

                        g.DrawLine(p44, minpos1, hpos1 - 15, minpos1, hpos1);   // draw vertical line


                        if (day == false) // .100
                        {
                            SizeF length1 = g.MeasureString("←" + UTCD.AddDays(-1).ToString("dddd"), font1); //  used for google lookups of SWL stations
                            g.DrawString("←" + UTCD.AddDays(-1).ToString("dddd"), font1, grid_text_brush, minpos1 - (length1.Width + 5), hpos1 - 15);
                            g.DrawString(UTCD.ToString("dddd") + "→", font1, grid_text_brush, minpos1 + 5, hpos1 - 15);

                        }
                        else
                        {
                            SizeF length1 = g.MeasureString("←" + UTCD.ToString("dddd"), font1); //  used for google lookups of SWL stations
                            g.DrawString("←" + UTCD.ToString("dddd"), font1, grid_text_brush, minpos1 - (length1.Width + 8), hpos1 - 15);
                            g.DrawString(UTCD.AddDays(1).ToString("dddd") + "→", font1, grid_text_brush, minpos1 + 8, hpos1 - 15);


                        }



                        //   Debug.WriteLine("day: " + ScreenWidth + " , "+ ScreenCenter + " , " + mins + " , " + minpos1 + " , " + day + " , " + hpos + " , " +Sun_Bot1 + " , " + temp + " , " + minpos2 + " , " +Sun_Left);


                        //==========================================================

                        if (MOON == true)
                        {

                            //   Debug.WriteLine("=====================MOON CALCULATIONS");

                            //--------------------------------------------------------------------
                            // Convert Universal Time to Ephemeris Time
                            // jd = juliandate(UTC, 'yyyy/mm/dd HH:MM:SS');


                            double y = DateTime.UtcNow.Year; //  year 2017
                            double m = DateTime.UtcNow.Month; // month = 1 to 12
                            double D1 = DateTime.UtcNow.Day; // day of month 1 to 31
                            double H = DateTime.UtcNow.Hour; // hour 0 to 24

                            double JD = DateTime.UtcNow.ToOADate() + 2415018.5; // convert OLE AUtomation Date to Julian Date
                                                                                //  Debug.WriteLine("Julian day " + JD);

                            double d = JD - 2451543.5; // convert Julian Date to something the orbital routine can use. (the Day number)
                                                       //   Debug.WriteLine("Current Day number " + d);

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
                            //    Debug.WriteLine("Starting N " + N);
                            //    Debug.WriteLine("Starting w " + w);
                            //    Debug.WriteLine("Starting M " + M);

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

                            //  Debug.WriteLine("xg " + xg);
                            //  Debug.WriteLine("yg " + yg);
                            //  Debug.WriteLine("zg " + zg);

                            //  Debug.WriteLine(" ");
                            //--------------------------------------------------------------------------------------------------
                            // Equatorial coordinates (rectangular coordinates)
                            // ecl = angle of the obliquity of the ecliptic  23.43704

                            double xe = xg;
                            double ye = yg * Math.Cos(ecl * DEG_to_RAD) - zg * Math.Sin(ecl * DEG_to_RAD);
                            double ze = yg * Math.Sin(ecl * DEG_to_RAD) + zg * Math.Cos(ecl * DEG_to_RAD);

                            //   Debug.WriteLine("xe " + xe);
                            //   Debug.WriteLine("ye " + ye);
                            //   Debug.WriteLine("ze " + ze);

                            double RA = RAD_to_DEG * Math.Atan2(ye, xe);                            // Right Ascension deg, and Declination deg
                            double dec = RAD_to_DEG * Math.Atan2(ze, Math.Sqrt(xe * xe + ye * ye));

                            //   Debug.WriteLine("start RA " + RA);   // should be 309.5 for test but in Hours divide by 15
                            //   Debug.WriteLine("dec " + dec);       // should be -19.1 for test

                            while (RA > 360) // normalize
                            {
                                RA = RA - 360.0;
                            }
                            while (RA < 0)
                            {
                                RA = RA + 360.0;
                            }

                            //    Debug.WriteLine("RA " + RA); // should be 309.5 for test
                            //    Debug.WriteLine("dec " + dec); // should be -19.1 for test

                            //    Debug.WriteLine(" ");


                            //--------------------------------------------------------------------------------------------------
                            // vernal (spring) equinox position = 0deg RA and counts eastward 0-360 as 24hours (15deg per hour)

                            double MJD = JD - 2400000.5;

                            double T = (MJD - 51544.5) / 36525.0;

                            //  Debug.WriteLine("Current T " + T);

                            double GMST = ((280.46061837 + 360.98564736629 * (MJD - 51544.5)) + 0.000387933 * T * T - T * T * T / 38710000.0) % 360.0;  // ST in degrees
                            if (GMST < 0) // normalize
                            {
                                GMST += 360.0;
                            }

                            //   Debug.WriteLine("Current GMST in deg " + GMST);

                            double ST = GMST / 15; // current Sidereal time in hours

                            //    Debug.WriteLine("Current GMST (hours) " + ST);  // ST in hours
                            //   Debug.WriteLine(" ");


                            //------------------------------------------------------------------------------------------------
                            // FIND LST and AZIMUTH and ELEVATION

                            //  double LST = GMST + (double)udDisplayLong.Value; // use your local Longitude to find LST and your azimuth
                            double LST = GMST + Convert.ToDouble(udDisplayLong.Value, NI); // use your local Longitude to find LST and your azimuth

                            //   Debug.WriteLine("ORIG LST " + LST); // 

                            // your local long is already in -180+180 but GMST is 0-360 already where 0-180 is east, 181-360 is west

                            if (LST > 360)
                            {
                                LST = LST - 180;
                            }
                            if (LST < 0)
                            {
                                LST = LST - 360.0;
                            }

                            //    Debug.WriteLine("NEW LST " + LST);
                            //    Debug.WriteLine(" ");


                            double HA = LST - RA;

                            //  Debug.WriteLine("ORIG HA " + HA);

                            // normalize -360+360 to -180+180
                            HA = (HA > 180) ? HA - 360 : (HA < -180) ? HA + 360 : HA; // in deg

                            //   Debug.WriteLine("NEW HA " + HA);
                            //    Debug.WriteLine(" ");



                            double x2 = Math.Cos(HA * DEG_to_RAD) * Math.Cos(dec * DEG_to_RAD);
                            double y2 = Math.Sin(HA * DEG_to_RAD) * Math.Cos(dec * DEG_to_RAD);
                            double z2 = Math.Sin(dec * DEG_to_RAD);

                            //  double xhor = x2 * Math.Sin((double)udDisplayLat.Value * DEG_to_RAD) - z2 * Math.Cos((double)udDisplayLat.Value * DEG_to_RAD);
                            //  double yhor = y2;
                            //  double zhor = x2 * Math.Cos((double)udDisplayLat.Value * DEG_to_RAD) + z2 * Math.Sin((double)udDisplayLat.Value * DEG_to_RAD);

                            double xhor = x2 * Math.Sin(Convert.ToDouble(udDisplayLat.Value, NI) * DEG_to_RAD) - z2 * Math.Cos(Convert.ToDouble(udDisplayLat.Value, NI) * DEG_to_RAD);
                            double yhor = y2;
                            double zhor = x2 * Math.Cos(Convert.ToDouble(udDisplayLat.Value, NI) * DEG_to_RAD) + z2 * Math.Sin(Convert.ToDouble(udDisplayLat.Value, NI) * DEG_to_RAD);

                            double az = RAD_to_DEG * Math.Atan2(yhor, xhor) + 180.0; // 0=north, 90=East, 180=South, 270=West (just like your rotor controller)
                            double alt = RAD_to_DEG * Math.Asin(zhor);  // 0=Horizon, 90=Zenith, -90 = Straight Down to heck

                            //   Debug.WriteLine("Azimuth deg " + az);
                            //   Debug.WriteLine("Altitude " + alt);
                            //   Debug.WriteLine(" ");

                            Moon_AZ = (int)az; // beam heading value
                            Moon_ALT = (int)alt;  // altitude value

                            //-----------------------------------------------

                            //   Debug.WriteLine("Moon RA (hours) " + (RA / 15));


                            double MoonLONMap = 0;

                            MoonLONMap = (GMST - RA);

                            //---------------------------------------------------------------
                            // normalize -360 +360 and convert directly to -180 +180
                            // if value > 180 then it must be on East side. if value < -180 must be on West
                            // if value > 0 but < 180 then must be on west side
                            // if value > -180 but < 0 then must be on east side
                            MoonLONMap = (MoonLONMap > 180) ? MoonLONMap - 360 : (MoonLONMap < -180) ? MoonLONMap + 360 : MoonLONMap;

                            MoonLONMap = -MoonLONMap;

                            //  Debug.WriteLine("Moonlonmap0 " + MoonLONMap);

                            if (MoonLONMap < -180) MoonLONMap = MoonLONMap + 360;
                            else if (MoonLONMap > 180) MoonLONMap = MoonLONMap - 360;

                            //   Debug.WriteLine("Moonlonmap1 " + MoonLONMap);

                            double MoonLATMap = dec; // latitude

                            //---------------------------------------------------------------------------------------------

                            //   Debug.WriteLine("Moon lon map " + MoonLONMap);
                            //   Debug.WriteLine("Moon lat map " + MoonLATMap);
                            //   Debug.WriteLine(" ");

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
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

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

                            //  const double EarthRadEq = 6378.1370; // Declare Earth Equatorial Radius Measurements in km

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

                            //  double bc = 0.00016717;     // Ballistic Coefficient derivative of mean motion (daily rate of change in the numbe of revs / day)
                            //  double BSTAR = 0.00010270;   // drag Term 
                            //  double mm = 15.54189435;    // how many orbits are completed each day

                            //   double N = (295.8085);     // DEG RA of the long ascending node defines the ascending and descending orbit locations with respect to the earths equatorial plane
                            //  double i = 51.6369;         // DEG Inclination to the ecliptic deg  defines the orientation of the orbit with respect to the earths equator <
                            //  double w = (94.0665);     // DEG Arg of Perigee (Perihelion) defines the low point,perigee of the orbit is with respect to the earths surface.
                            //   double a = 1.06;            //  6777.943 kmeters earth is 6371km radius   Mean distance (Earth equitorial radii) Semi-major axis defines size of the orbit
                            //  double e = 0.0006891;       //     Eccentricity defines the shape of the orbit
                            // double M = (266.1275);     // DEG v = true, mean anomaly defines where the satellite is within the orbit with respect to perigee (low point)

                            //  double ecl = 23.43704;      // DEG Angle of the obliquity of the ecliptic plane (this is static)


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
                            // Convert.ToDouble(udDisplayLong.Value, NI)

                            // Calculate(ISSLATMap, ISSLONMap, ISSALTMap, (double)udDisplayLat.Value, (double)udDisplayLong.Value, 30); // get the ISS_AZ and ISS_ALT values based on 
                            Calculate(ISSLATMap, ISSLONMap, ISSALTMap, Convert.ToDouble(udDisplayLat.Value, NI), Convert.ToDouble(udDisplayLong.Value, NI), 30); // get the ISS_AZ and ISS_ALT values based on 

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
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

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

                                      if (g != null)  g.DrawLine(OrgPen, ISS_XX[ii], ISS_YY[ii], ISS_XX[ii] + 1, ISS_YY[ii] + 1);
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

                                  if (g != null)  g.DrawLine(OrgPen, ISS_XX[ii], ISS_YY[ii], ISS_XX[ii] + 1, ISS_YY[ii] + 1);
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
                            //  g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                            //  g.CompositingMode = CompositingMode.SourceOver;
                            //  g.CompositingQuality = CompositingQuality.HighQuality;
                            //  g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            //   g.SmoothingMode = SmoothingMode.HighQuality;
                            //   g.PixelOffsetMode = PixelOffsetMode.HighQuality;


                            VOA_Color[0] = 70;

                            Pen p5 = new Pen(Color.FromArgb(70, Color.Yellow), 1.0f); // dusk
                            Font font7 = new Font("Ariel", 10.5f, FontStyle.Regular, GraphicsUnit.Pixel);  // ke9ns 

                            //      g.DrawString("SDBW: " + VOA_S[y].ToString("D"), font7, grid_text_brush,VOA_X[x] , VOA_Y[y]);

                            Image src1 = new Bitmap(star_image); // load up SUN image ( use PNG to allow transparent background)

                            g.DrawImage(src1, VOA_MyX - 6, VOA_MyY - 6, 12, 12); // draw star  of your station transmitter location based on input lat and long

                            g.DrawString("VOACAP Propagation map", font7, grid_text_brush, Sun_Left, Sun_Top1);
                            g.DrawString("Max+", font7, bluebrush, Sun_Left, Sun_Top1 + 10);
                            g.DrawString("Strong", font7, greenbrush, Sun_Left + 26, Sun_Top1 + 10);
                            g.DrawString("Fair", font7, yellowbrush, Sun_Left + 59, Sun_Top1 + 10);
                            g.DrawString("Weak", font7, orangebrush, Sun_Left + 80, Sun_Top1 + 10);
                            g.DrawString("Min-", font7, graybrush, Sun_Left + 108, Sun_Top1 + 10);


                        // Blue Strongest Signal(AM)
                        // Green Strong Signal(AM / SSB)
                        // Yellow Fair Signal(SSB, CW)
                        // Orange Weak Signal(CW, FT8)
                        // Gray Weakest Signal(CW, FT8)


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
                                        if (((Console.Kindex > 4) || (Console.RadioBlackout.Contains("R") == true) || (Console.GeoBlackout.Contains("G") == true)) && (console.last_MHZ > 4))
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
                                           (((Console.Kindex > 4) || (Console.RadioBlackout.Contains("R") == true) || (Console.GeoBlackout.Contains("G") == true)) && (console.last_MHZ > 2)) ||

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
                                catch (Exception)
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

                            if (beacon1 == true) // show time slot for beacons
                            {

                                for (int x = 0; x < 18; x++)
                                {
                                    g.DrawString(Beacon_Call[x].ToString() + " " + Beacon_Country[x].ToString(), font2, grid_text_brush, 55, 20 + (x * 10)); // use Pandapdater holder[] data
                                }
                                for (int x = 0; x < 5; x++)
                                {
                                    int y = BX_Index[x] / 5;
                                    int y1 = BX_Index[x] % 5; // get remainder

                                    g.DrawString((Beacon_Freq[y1] / 1e6).ToString("f2"), font2, grid_text_brush, 27, 20 + (y * 10)); // use Pandapdater holder[] data
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

                                for (int qq = 0; qq < 100; qq++)
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

                        //  Darken();

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

            TrackOFF = true;


            textBox1.Text += "Map turned OFF \r\n";

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
                    if (console.comboDisplayMode.Text != "Panafall8020")
                    {
                        Display.map = 0;                 // tell display program to got back to standard panafall mode
                    }
                    Display.CurrentDisplayMode = LastDisplayMode;
                }
            }
            else
            {
                UTCLAST2 = 0;
            }
        } // chkPanMode_CheckedChanged



        //=========================================================================================


        private static int DXLOC_Index1 = 0; // total number of records in the DXLOC.txt file
        public static int SP8_Active = 0;    // 1=DX LOC scanned into memory
        public static bool DXLOCDONE = false;

        // data obtained from DXLOC.txt file
        public static string[] DXLOC_prefix = new string[1400];       // prefix (must start with)
        public static string[] DXLOC_prefix1 = new string[1400];      // prefix (must also contain) /
        public static string[] DXLOC_prefix2 = new string[1400];      // prefix (must exclude) \

        public static string[] DXLOC_lat = new string[1400];          // text of lat
        public static string[] DXLOC_lon = new string[1400];          // text of  lon
        public static double[] DXLOC_LAT = new double[1400];          // latitude  
        public static double[] DXLOC_LON = new double[1400];          //  longitude
        public static string[] DXLOC_country = new string[1400];      // country
        public static string[] DXLOC_continent = new string[1400];    // continent

        public static string[] DXLOC_dxcc = new string[1400];        // dxcc entity for LoTW comparison


        //=======================================================================================
        //=======================================================================================
        //ke9ns Open and read DXLOC.txt file here (put into array of prefix vs lat/lon value)
        //   Fields:
        //
        //0	    DXCC Entity number (used by LoTW)
        //1*	ARRL DXCC prefix,
        //2*    DXCC Entity name,
        //3*	Continent,
        //4*	Latitude,
        //5*	Longitude,
        //6	    CQ Zone,
        //7	    ITU Zone,
        //8	    Active (A) or Deleted (D),
        //9	    Date from becoming a valid Entity,
        //10	Possible prefixes from ITU Assigned B
        //locks to Sovereign UN Territory(s)

        //  [MethodImpl(MethodImplOptions.Synchronized)]
        public void DXLOC_FILE()
        {
            Debug.WriteLine("(((((((DXLOC FILE READ IN))))))))))))))))");

            string file_name = console.AppDataPath + "DXLOC.txt"; // //  sked - b15.csv

            textBox1.Text += "Attempting to open DX Location list\r\n";

            if (File.Exists(file_name))
            {
                Debug.WriteLine("DX LOC read SP_Active=" + SP_Active);

                textBox1.Text += "Reading DX Location list\r\n";

                try
                {
                    stream22 = new FileStream(file_name, FileMode.Open); // open  file
                    reader22 = new BinaryReader(stream22, Encoding.ASCII);

                }
                catch (Exception)
                {
                    SP8_Active = 0;
                    Debug.WriteLine("NO DX LOC FILE============================");
                    DXLOCDONE = true;
                    TrackOFF = true;
                    return;


                }
                var result = new StringBuilder();



                if (SP8_Active == 0) // dont reset if already scanned in  database
                {
                    DXLOC_Index1 = 0; // how big is the DXLOC data file in lines

                }

                for (; ; ) // read file and extract data from it and close it and set sp8_active = 1 when done
                {

                    if (SP8_Active == 1) // aleady scanned database
                    {
                        Debug.WriteLine("DX LOC ALREADY SCANNED");
                        break; // dont rescan database over 
                    }

                    //     if (SP_Active == 0)
                    //     {
                    //      reader22.Close();    // close  file
                    //       stream22.Close();   // close stream

                    //     return;
                    //  }



                    try
                    {
                        var newChar = (char)reader22.ReadChar();

                        if ((newChar == '\r'))
                        {
                            newChar = (char)reader22.ReadChar(); // read \n char to finishline

                            string[] values = result.ToString().Split(','); // split line up into segments divided by ,


                            //   Debug.Write(DXLOC_Index1.ToString());
                            DXLOC_dxcc[DXLOC_Index1] = values[0].Substring(1, values[0].Length - 2); // dxcc entity number

                            //   Debug.WriteLine("DXLOC DXCC: " + DXLOC_dxcc[DXLOC_Index1]);

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
                                    DXLOC_LAT[DXLOC_Index1] = Convert.ToDouble(DXLOC_lat[DXLOC_Index1].Substring(1, ff), NI);
                                    //     Debug.Write(" LAT>" + DXLOC_LAT[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {

                                    Debug.WriteLine("BAD NORTH " + DXLOC_lon[DXLOC_Index1].Substring(1, ff), NI);
                                    DXLOC_LAT[DXLOC_Index1] = 0;


                                }

                            }  // pos 90 North
                            else if (DXLOC_lat[DXLOC_Index1].Contains("S")) // neg 90 North
                            {

                                int ff = DXLOC_lat[DXLOC_Index1].IndexOf('S') - 1;

                                try
                                {
                                    DXLOC_LAT[DXLOC_Index1] = -Convert.ToDouble(DXLOC_lat[DXLOC_Index1].Substring(1, ff), NI);
                                    //  Debug.Write(" LAT>" + DXLOC_LAT[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                    DXLOC_LAT[DXLOC_Index1] = 0;
                                    Debug.WriteLine("BAD SOUTH " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);

                                }

                            }  // neg 90 North


                            if (DXLOC_lon[DXLOC_Index1].Contains("W")) // neg 180 west
                            {
                                int ff = DXLOC_lon[DXLOC_Index1].IndexOf('W') - 1;

                                try
                                {

                                    DXLOC_LON[DXLOC_Index1] = -Convert.ToDouble(DXLOC_lon[DXLOC_Index1].Substring(1, ff), NI);
                                    //   Debug.WriteLine(" LON>" + DXLOC_LON[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                    Debug.WriteLine("BAD WEST " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LON[DXLOC_Index1] = 0;
                                }

                            }  // neg 180 West
                            else if (DXLOC_lon[DXLOC_Index1].Contains("E")) // pos 180 East
                            {
                                int ff = DXLOC_lon[DXLOC_Index1].IndexOf('E') - 1;

                                try
                                {

                                    DXLOC_LON[DXLOC_Index1] = Convert.ToDouble(DXLOC_lon[DXLOC_Index1].Substring(1, ff), NI);
                                    //    Debug.WriteLine(" LON>" + DXLOC_LON[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                    Debug.WriteLine("BAD EAST " + DXLOC_lon[DXLOC_Index1].Substring(1, ff), NI);
                                    DXLOC_LON[DXLOC_Index1] = 0;
                                }

                            }  // pos 180 East



                            DXLOC_Index1++;
                            //   Debug.WriteLine(result);

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
                        else textBox1.Text += "End of DX LOC FILE at " + DXLOC_Index1.ToString() + "\r\n";

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

                Debug.WriteLine("Reached End of DXLOC.txt FILE with # " + DXLOC_Index1.ToString());


                reader22.Close();    // close  file
                stream22.Close();   // close stream


                SP8_Active = 1; // done loading DXLOC database (Good)

                Debug.WriteLine("DX LOC DONE SP_Active=" + SP_Active);
                DXLOCDONE = true;

            } // if file exists
            else
            {
                DXLOCDONE = true;
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
          //  MouseEventArgs me = (MouseEventArgs)e;  // .202
            pause = true;
            button1.Text = "Paused";

          //  textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click

          
        } // textBox1_MouseDown





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
                Debug.WriteLine("Memory Rows Count " + dataGridView2.Rows.Count);

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
            if (SP_Active == 0)
            {
                console.spotterMenu.ForeColor = Color.Yellow;
                console.spotterMenu.Text = "SWL Spot";
                SP1_Active = 1;
            }

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

            console.NOAA(); // ke9ns .197 to get the SSN 


            RadioBlackout = " ";
            GeoBlackout = " ";

            Debug.WriteLine("NOAA HERE");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            serverPath = "https://services.swpc.noaa.gov/text/wwv.txt"; // new as of Sept 17th 2018
            /*
                        Uri noaaPath = new Uri("https://services.swpc.noaa.gov/text/wwv.txt");
                        HttpClient client = new HttpClient();
                        try
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
                           // client.Timeout = new TimeSpan(0, 5, 0); // normal timeout is set to 100 seconds

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("1noaa fault=== " + ex);
                            textBox1.Text += "1Failed to download Space Weather \r\n";
                        }

                        string noaa = " ";

                        try
                        {

                          //  result.Append(await client.GetStringAsync(url).ConfigureAwait(false)); //

                         //  noaa = await client.GetStringAsync(noaaPath).ConfigureAwait(false); //

                        }
                        catch
                        {

                        }
                        */

            try
            {

                Debug.WriteLine("NOAA try web request");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(serverPath);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                Stream responseStream1 = webResponse.GetResponseStream();
                StreamReader streamReader1 = new StreamReader(responseStream1);

                textBox1.Text += "Attempt to download Space Weather \r\n";

                string noaa = streamReader1.ReadToEnd();

                responseStream1.Close();
                streamReader1.Close();


                Debug.WriteLine("noaa=== " + noaa);

                //   textBox1.Text += "NOAA Download complete \r\n";

                /*
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

           */

                //--------------------------------------------------------------------
                if (noaa.Contains("Solar flux ")) // 
                {

                    int ind = noaa.IndexOf("Solar flux ") + 11;

                    try
                    {
                        SFI = (int)(Convert.ToDouble(noaa.Substring(ind, 3)));

                        if (Last_SFI != SFI) // .197
                        {
                            console.last_MHZ = 0;
                            Map_Last = Map_Last | 2;    // force update of world map
                        }
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
                        if (Last_Aindex != Aindex) // .197
                        {
                            console.last_MHZ = 0;
                            Map_Last = Map_Last | 2;    // force update of world map
                        }
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
                Debug.WriteLine("noaa fault=== " + ex);
                textBox1.Text += "Failed to download Space Weather \r\n";

            }


        } // NOAA


        //=====================================================
        //ke9ns formula to calculate beam heading
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
            // Convert.ToDouble(udDisplayLong.Value, NI)
            //  LatStart = (Math.PI / 180.0) * (double)udDisplayLat.Value; // convert degree's to radians
            //  LongStart = (Math.PI / 180.0) * (double)udDisplayLong.Value;

            LatStart = (Math.PI / 180.0) * Convert.ToDouble(udDisplayLat.Value, NI); // convert degree's to radians
            LongStart = (Math.PI / 180.0) * Convert.ToDouble(udDisplayLong.Value, NI);

            LatDest = (Math.PI / 180.0) * SpotLat;  // convert degree's to rads
            LongDest = (Math.PI / 180.0) * SpotLong;

            double y = Math.Sin(LongDest - LongStart) * Math.Cos(LatDest);
            double x = Math.Cos(LatStart) * Math.Sin(LatDest) - Math.Sin(LatStart) * Math.Cos(LatDest) * Math.Cos(LongDest - LongStart);

            int Bearing = (int)(Math.Atan2(y, x) * (180.0 / Math.PI));
            //   Debug.WriteLine("Init Bearing=" + Bearing);

            int FBearing = (int)(((Math.Atan2(y, x) * (180.0 / Math.PI)) + 360) % 360.0);

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
        public int beacon9 = 0;        //to store prior low filter before running beacon scan
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
                btnBeacon.Text = "NCDXF Beacon";
                btnBeacon.ForeColor = Color.Black;


                stopWatch.Stop();
                stopWatch1.Stop();

                Debug.WriteLine(">>>>>>>>BEACON: TURN BACK OFF");


            }
            else // if the beacon chk was OFF,then turn it on
            {

                beacon = true;
                btnBeacon.Text = "NCDXF Run";
                btnBeacon.ForeColor = Color.Red;

                beacon11 = 0; // reset freq for slow scan since you may have changed the freq
                beacon12 = 0; // reset the station your looking at


                Debug.WriteLine(">>>>>>>>BEACON: START");

                //--------------------------------------
                // turn on mapping if it wasnt on when you click on the beacon button

                if (SP5_Active == 0)  // if OFF then turn ON
                {
                    Debug.WriteLine(">>>>>>>>BEACON: turn on mapping");

                    UTCLAST2 = 0;

                    if (chkPanMode.Checked == true) Display.map = 1;
                    else
                    {
                        if (console.comboDisplayMode.Text != "Panafall8020")
                        {
                            Display.map = 0;                 // tell display program to got back to standard panafall mode
                        }
                    }

                    //    btnTrack.Text = "Track ON";

                    LastDisplayMode = Display.CurrentDisplayMode; // save the display mode that you were in before you turned on special panafall mode

                    if (chkPanMode.Checked == true) Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                    Display.GridOff = 1; // Force Gridlines off but dont change setupform setting

                    if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                    {

                        if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;

                        console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                        Darken(); // adjust map image bright/dark and grayline

                        console.picDisplay.BackgroundImage = MAP;

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



                    SP5_Active = 1; // turn on track map (sun, grayline, voacap, or beacon)
                    UTCLAST2 = 0;
                    VOATHREAD1 = false;

                    Thread t = new Thread(new ThreadStart(TrackSun)); // turn on track map (sun, grayline, voacap, or beacon)

                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                    UTCLAST2 = 0;


                    t.Name = "Track Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();




                    textBox1.Text = "Clicked to Turn On GrayLine Sun Tracker\r\n";

                    Debug.WriteLine(">>>>>>>>BEACON:  mapping turned on");

                    btnTrack.Text = "Track-ON";

                    if (SP5_Active == 0) textBox1.Text += "ON but just turned OFF";


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

                SP5_Active = 1; // turn on track map (sun, grayline, voacap, or beacon)
                Thread t = new Thread(new ThreadStart(BeaconSlot)); // show beacons (turn on tracking map)

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


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
                    UTCLAST2 = 0;
                    SP5_Active = 0;                     // turn off tracking

                    if (console.comboDisplayMode.Text != "Panafall8020")
                    {
                        Display.map = 0;                 // tell display program to got back to standard panafall mode
                    }

                    if (chkPanMode.Checked == true) Display.CurrentDisplayMode = LastDisplayMode;

                    if (console.setupForm.gridBoxTS.Checked == true) Display.GridOff = 1; // put gridlines back the way they were
                    else Display.GridOff = 0; // gridlines ON

                    btnTrack.Text = "Track/Map";

                    textBox1.Text += "Click to turn OFF GrayLine Sun Tracking\r\n";

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
            SEC1 = UTCD.ToString("mm:ss");
            minutes = Convert.ToInt16(SEC1.Substring(0, 2));
            seconds = Convert.ToInt16(SEC1.Substring(3, 2));

            // seconds = TimeSpan.Parse(SEC1).TotalSeconds; // get total seconds of the day
            // example: 23:19 = 23min 19sec = min%3 = modulo of 2minutes into a 3 min block + 19sec = 139 seconds into a 180second block

            SlotSeconds = ((minutes % 3) * 60) + seconds;
            TSlot = (SlotSeconds / 10) * 10;

            Debug.WriteLine(">>>TIME1: THREAD START TIME " + SEC1);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in minutes " + minutes);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in seconds " + seconds);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in total seconds per 3 minute intervals: " + SlotSeconds);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in total seconds per 3 minute intervals/10: " + TSlot);


            //---------------------------------------------------------------------
            for (int x = 0; x < 18; x++) // find starting station then BX_Index will keep track
            {

                if ((BX_TSlot1[x * 5] >= TSlot) && (BX_TSlot1[x * 5] < TSlot + 10))
                {
                    BX_Index[0] = x * 5; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[0] 14mhz station# " + BX_Index[0] + " , " + BX_Station[BX_Index[0]]);
                }
                else if ((BX_TSlot1[x * 5 + 1] >= TSlot) && (BX_TSlot1[x * 5 + 1] < TSlot + 10))
                {
                    BX_Index[1] = x * 5 + 1; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[1] 18mhz station# " + BX_Index[1] + " , " + BX_Station[BX_Index[1]]);
                }
                else if ((BX_TSlot1[x * 5 + 2] >= TSlot) && (BX_TSlot1[x * 5 + 2] < TSlot + 10))
                {
                    BX_Index[2] = x * 5 + 2; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[2] 21mhz station# " + BX_Index[2] + " , " + BX_Station[BX_Index[2]]);
                }
                else if ((BX_TSlot1[x * 5 + 3] >= TSlot) && (BX_TSlot1[x * 5 + 3] < TSlot + 10))
                {
                    BX_Index[3] = x * 5 + 3; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[3] 24mhz station#  " + BX_Index[3] + " , " + BX_Station[BX_Index[3]]);
                }
                else if ((BX_TSlot1[x * 5 + 4] >= TSlot) && (BX_TSlot1[x * 5 + 4] < TSlot + 10))
                {
                    BX_Index[4] = x * 5 + 4; // this is the start index
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
            while ((beacon1 == true)) // only do while the beacon testing is going on
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
                        else if ((BX_TSlot1[x * 5 + 2] >= TSlot) && (BX_TSlot1[x * 5 + 2] < TSlot + 10))
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

                            if (console.udCWPitch.Value != 600) console.udCWPitch.Value = 600;

                            if (console.RX1Filter != Filter.VAR1) console.RX1Filter = Filter.VAR1;

                            if ((console.RX1FilterHigh != 650) || (console.RX1FilterLow != 550))
                            {
                                console.UpdateRX1Filters(550, 650);   // sete cw filter
                            }

                            console.VFOAFreq = (double)Beacon_Freq[beacon11 - 1] / 1e6; // shift 0hz down 600 for cw mode and convert to MHZ

                            console.UpdateDisplay();

                            Debug.WriteLine(">>>freq:beacon11, BX_Index[beacon11 - 1] , beacon14: " + beacon11 + " , " + BX_Index[beacon11 - 1] + " , " + beacon14);

                            //------------------------------------------

                            if (beacon15 > 0) // start processing after the 1st 10 seconds of each freq change
                            {
                                if (BX_dBm[beacon14] >= -73) BX_dBm3[beacon14] = 9;
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

                                BX_FULLSTRING[beacon14] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[beacon14] / 1e3).ToString("f1")).PadRight(9) + BX_Station[beacon14].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[beacon14] + " " + (BX_dBm1[beacon14] - BX_dBm[beacon14]).ToString("D3") + " dBm " + BX_Time[beacon14].ToString("D4") + "z " + BX_Grid[beacon14];

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


                                Debug.WriteLine(">>>RESET BEACON11: " + beacon11);

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
                                // int tempDB1 = 0;


                                tempDB = console.ReadAvgStrength(0);    // get beacon CW signal strength, but this does not factor out the noise floor (i.e. S5 signal might just be the noise floor at S5)

                                //  tempDB1 = console.WWVTone;  // get Magnitude value from audio.cs and Goertzel routine  (i.e. this will determine if we are actually hearing a CW signal at 600hz and not just an S5 noise floor)

                                //   Debug.WriteLine("BEACON TONE Detection: " + tempDB1);


                                if (tempDB > BX_dBm[beacon14])
                                {
                                    BX_dBm[beacon14] = tempDB; // get signal strengh avg reading to match avg floor reading
                                }

                                if (BX_dBm2 > BX_dBm1[beacon14])
                                    BX_dBm1[beacon14] = BX_dBm2; // value passed back from display.cs noise floor (avg value)

                                //  WWVThreshold = BX_dBm1[beacon14]; // display.cs the floor
                            }
                            else beacon6++;

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

                    if (tsTime >= LasttsTime)
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
                            if (beacon5 < 6) beacon5++; // if not == 6 

                        }

                    } // seconds != lastseconds
                    else // search for a signal
                    {

                        stopWatch1.Start();
                        ts1 = stopWatch1.Elapsed;
                        tsTime1 = (double)ts1.Seconds + ((double)ts1.Milliseconds / 1000.0);

                        if ((beacon5 < 7) && (tsTime1 >= BandSwitchDelay))          //(beacon6 > 25)) // wait for band switching pulse to disapate
                        {

                            int tempDB = console.ReadAvgStrength(0);

                            if (tempDB > BX_dBm[beacon14])
                            {
                                BX_dBm[BX_Index[beacon5 - 2]] = tempDB; // get signal strengh avg reading to match avg floor reading
                            }

                            if (BX_dBm2 > BX_dBm1[BX_Index[beacon5 - 2]])
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
            string[] whost = { "utcnist.colorado.edu", "utcnist2.colorado.edu", "time-c.nist.gov", "time-b.nist.gov", "time-a.nist.gov" };  //  
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
            else strHost = "utcnist.colorado.edu";

            try
            {

                iphostinfo = Dns.GetHostEntry(strHost);

                ip = iphostinfo.AddressList[0];

                ipe = new IPEndPoint(ip, port);

                Debug.WriteLine("attempt connection to: " + strHost + " , " + ipe);
                textBox1.Text += "You can select a Time server from the Menu list above.\r\n" + "Attempt Connect to NIST Time Server> " + strHost + ", " + ipe + "\r\n";

                startDT = DateTime.Now; // record time you opened a connection to NIST

                c.Connect(ipe);     // Connect to server which starts clock (NIST will now send back the correct Time)


                Debug.WriteLine("wait connection to: " + strHost);

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
                //  MessageBox.Show(new Form { TopMost = true }, "Time server connection failed! 1 error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                //  return;
            }

            //  } // for loop through time server addresses

            if (!c.Connected)
            {
                Debug.WriteLine("Failure " + strHost);

                textBox1.Text += "Failure NIST Time Server!\r\n";

                if (c.Connected) c.Close(); // close the socket

                MessageBox.Show( "Time server connection failed! 2 error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
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
            catch (Exception)
            {
                Debug.WriteLine("Exception " + strHost);
                if (c.Connected) c.Close(); // close the socket

                MessageBox.Show( "Time server connection failed! /r error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                WTime = false;   // turn dx spotting back on

                return;
            }

            if (c.Connected) c.Close(); // close the socket

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
                Debug.WriteLine("TRY " + sb.ToString());

                //  string[] o = sb.ToString().Split(' '); // Cut the string  
                //  string temp2 = o[6].Substring(0, 3); // get millisecond delay
                //  string temp1 = o[1] + " " + o[2] + ".000";
                //  Debug.WriteLine("TRY>" + temp1 + "<"); // TRY>21-05-06 00:30:54.000<
                //  Debug.WriteLine("TRY>" + temp2 + "<"); // TRY>296<


                string temp2 = sb.ToString(32, 3);
                   Debug.WriteLine("TRY>" + temp2 + "<");
                  string temp1 = sb.ToString(7,17) + ".000";
                Debug.WriteLine("TRY>" + temp1 + "<");


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

                Debug.WriteLine("Exception2 " + sb.ToString());

             


                MessageBox.Show("Time server busy! Try Again.", " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                WTime = false;   // turn dx spotting back on

                return;

            }

            //    MessageBox.Show(new Form { TopMost = true }, "Time synchronization, ", " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                console.UpdateRX1Filters(80, 140);

                textBox1.Text += "Tone detection. Waiting for Start of Minute!\r\n";

                console.VFOAFreq = WWV_Freq1[(int)udDisplayWWV.Value - 1];         // main receiver: WWV in DIGU mode on  sub-Carrier


            }



            console.UpdateDisplay();

            int BCDSignal = 0;              // measured BCD data stream dBm signal
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
                                              //  int BCDCount = 0;              // counter for the % adjustment

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
                                   // bool WWVStop = false; // true = got entire 1 minute frame
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

            //BCDCount = 0;
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


            while (ST.ElapsedMilliseconds < 1300)                          // get floor for bcd stream
            {

                if (WWVPitch == false)  // signal strength based detected
                {
                    BCDSignal = console.ReadStrength(0);            // read wwv 100 hz OFF of carrier point (BCD data stream)            
                    CarrierSignal = console.ReadStrength(1);        // read WWV 0hz carrier point

                    if ((BCDSignal < BCDSignalOFF))       // check low dBm value
                    {
                        BCDSignalOFF = BCDSignal;                               // finding the OFF state of this bcd stream area if the WWV signal
                        CarrierSignalINIT = CarrierSignal;                      // find the carrier level at the same time
                    }
                    if ((BCDSignal > BCDSignalON))        // check high dBm value
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
                                                  //  Debug.WriteLine(">>>>>>WWVTONE: " + BCDSignal + " , " + BCDSignalON + " , " + BCDSignalON1 + " , " + checkBoxTone.Checked + " , " + WWVThreshold);

                    }

                }


            } // for loop 1.3 seconds to test levels


            BCDSignalON1 = 0;         // RESET BCD data steam high dbm signal found while running
            //BCDCount = 0;

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
                            // WWVStop = false;
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

                        if (BCDSignal > WWVThreshold)
                        {  // Debug.WriteLine("WWVTONE: " + BCDSignal + " , " + WWVThreshold+" , " + BCDSignalON + " , " + BCDSignalON1 + " , " + checkBoxTone.Checked);
                            Debug.WriteLine("WWVTONE: " + BCDSignal);

                        }
                        else if (BCDSignal > 50)
                            Debug.WriteLine("                                WWVTONE: " + BCDSignal);

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
                                                  // BCDCount = 0;

                        ST.Restart();

                        //   Debug.WriteLine("WWV>>  Highest BCD Mag: " + BCDSignalON  );          // adjust the threshold based on the last seconds ON/OFF dBm values

                    }


                    //-------------------------------------------------
                    // check the mag  strength 


                    if (BCDSignalON < 300) // if you loose the carrier, then NO GOOD
                    {
                        if (WWVCF > 800) // FAIL if carrier stays LOW for too long
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Sub-Carrier signal too low, choose different Frequency\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVPos = false;
                            WWVStart = false;
                            //WWVStop = false;
                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                            WTime = false;
                        }
                        else WWVCF++;

                    }
                    else
                    {
                        WWVCF = 0;
                    }

                    //  BCDAdj = 3.0; // was 3
                    WWVThreshold = (int)((double)BCDSignalON / BCDAdj);          // 33% of full scale adjust the threshold based on the last seconds ON/OFF dBm values

                    //   WWVThreshold = (int)numericUpDownTS2.Value;


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

                    if (WWVNewTime.ElapsedMilliseconds >= (230 + (indexS + (indexP * 10)) * 1000))   // 1000,2000,3000,4000,5000 milliseconds, etc
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
                            textBox1.Text += " P" + (indexP) + ">";
                            indexS = 1;
                        }
                        else // first 9 seconds of every P frame
                        {

                            if (BCD1 > 1)   // BCD tone was med ( 440 msec)
                            {
                                BCD1 = 0;  // reset value

                                P[indexP, indexS - 1] = 1;
                                textBox1.Text += "1";

                            }
                            else // BCD tone was short ( 170 msec)
                            {
                                BCD1 = 0;  // reset value

                                P[indexP, indexS - 1] = 0;
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

                        if ((WWVFault[1] == true) || (WWVFault[2] == true) || (WWVFault[3] == true) || (WWVFault[4] == true))
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Data No Good, will Try again.\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVCF = 0;
                            BCDAdj = 3;
                            WWVPos = false;
                            WWVStart = false;
                            // WWVStop = false;
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

                            //WWVStop = true;
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
                            catch (Exception)
                            {
                                textBox1.Text += "WWV time not read correctly. Will Try again.\r\n";
                                WWVNewTime.Stop();
                                indexP = 0;     // reset data to catch next minute data stream
                                indexS = 0;
                                BCDAdj = 3;
                                WWVCF = 0;
                                WWVPos = false;
                                WWVStart = false;
                                //WWVStop = false;
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
                        BCDAdj = 2.8;

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
                            BCDAdj = 3;
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

                        if ((WWVPos == true) && (tickTimeOFF > 1010) && (tickTimeOFF < 1700))  // Long HOLE indicates start of new minute
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
                            if ((WWVPos == true))  // Long HOLE indicates start of new minute
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

            console.UpdateRX1Filters(beacon9, beacon8); // restore filter

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
        public double Coeff2 = 0.0; // used for mark/space freq
        public double CoeffB = 0.0;
        public double Coeff2B = 0.0;
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
        public void GoertzelCoef(double freq, int SIGNAL_SAMPLE_RATE)
        {

            normalizedfreq = freq / SIGNAL_SAMPLE_RATE;
            Coeff = 2 * Math.Cos(2 * Math.PI * normalizedfreq);
            CoeffB = Math.Exp(-2 * Math.PI * normalizedfreq);

            Debug.WriteLine("COEFF= " + Coeff + ", freq= " + freq);

        }


        public void GoertzelCoef2(double freq, int SIGNAL_SAMPLE_RATE)
        {

            normalizedfreq = freq / SIGNAL_SAMPLE_RATE;
            Coeff2 = 2 * Math.Cos(2 * Math.PI * normalizedfreq);
            Coeff2B = Math.Exp(-2 * Math.PI * normalizedfreq);

            Debug.WriteLine("COEFF2= " + Coeff2 + ", freq2= " + freq);

        }



        private void checkBoxTone_CheckedChanged(object sender, EventArgs e)
        {

        }


        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        // F10.7 = 63.74 + 0.727*SSNf + 0.000895*SSNf**2 
        // SSNf = ((93918.4 + 1117.3 * SFI) ^ .5) - 416.37    this is the true SSN value since the optical SSN# wont have an effect on earth for up to 3 days 

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


            if (VOARUN == true || VOATHREAD1 == true)
            {
                return; // dont do a VOACAP update if your aleady doing one.
            }


        VOACHECK_TOP:

          
            int MHZ1 = console.last_MHZ;    // int MHZ1 = (int)double.Parse(console.txtVFOAFreq.Text.Replace(",", ".")); // get current freq value in hz, so convert to mhz


            Debug.WriteLine("MHZ1    : " + MHZ1);

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
                    if (Console.SSNE == 0) SSNf = 1;

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


            SSN = SSNf.ToString("##0").PadLeft(3);   // "{0,2}"

            
            //need  CultureInfo.InvariantCulture.NumberFormat
            WATTS = wattage.ToString("0.0000",NI).PadLeft(6); // 0.0000

            Debug.WriteLine("SSN and watts " + Console.SSNE + " , " + WATTS);

            if (
                (!console.MOX) && ((chkBoxAnt.Checked != Last_Ant) || (Last_SSN != SSN) || (Last_VOALAT != VOALAT) || (Last_VOALNG != VOALNG) ||
                (Last_MHZ != MHZ) || (Last_MONTH != MONTH) || (Last_DAY != DAY) || (Last_HOUR != HOUR) || (Last_WATTS != WATTS) ||
                (Last_MODE != console.RX1DSPMode) || (VOACAP_FORCE == true))
                )
            {

                VOARUN = true;                     // dont allow this to trigger until its finished

                if (VOATHREAD1 == false) // if true, then form must be closing
                {
                    Thread t = new Thread(new ThreadStart(VOACAP)); // generate the VOA data on the MAP

                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                    t.Name = "VOACAP Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();
                }

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

        public static double[] VOA_LAT = new double[9000];    // ke9ns storage for VOACAP VG1 MUF data
        public static double[] VOA_LNG = new double[9000];
        public static double[] VOA_MUF = new double[9000];
        public static string[] VOA_MODE = new string[9000];
        public static double[] VOA_DBU = new double[9000];
        public static double[] VOA_SDBW = new double[9000];
        public static double[] VOA_SNR = new double[9000];
        public static int[] VOA_Color = new int[9000];

        public static int[] VOA_S = new int[9000];   // each point on map is converted to an S meter reading
        public static int[] VOA_S1 = new int[100];    // VOA_S1[1] = how many S1 readings in the map total, [2] = S2 readings 


        public static int[,] VOA_SS = new int[3000, 3000];   // each point on map is converted to an S meter reading
        public static int[] VOA_SY = new int[3000];
        public static int[] VOA_SX = new int[3000];


        public static int[,] VOA_X = new int[10, 1000];    // conversion of MUF LAT & LONG to X Y points  (VOA_S1[h] = number of each S unit found in map, h=S unit your checking)
        public static int[,] VOA_Y = new int[10, 1000];     // [][] = S unit S0 to S9 then list of x,y points that fall under that S reading


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

        public static bool VOATHREAD = false;    // true = the VOACAP thread is in the middle of running, false = thread is not running currently
        public static bool VOATHREAD1 = false;  // true = form closing (thread is not allowed to start up), false = normal
        //=======================================================================================
        //=======================================================================================
        //ke9ns start voacap propagation spotting THREAD 1 and done
        private void VOACAP()
        {
            VOATHREAD = true; // running

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
                if ((int)SpotOptions.udDAY.Value > 0) COEFF = "URSI";
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
            VOA[9] = "Method   :   " + METHOD + "\r\n";
            VOA[10] = "Coeffs   :" + COEFF + "\r\n";
            VOA[11] = "Months   :  " + MONTH + "." + DAY + "   0.00   0.00   0.00   0.00   0.00   0.00   0.00   0.00\r\n";   // MONTH = 00  HOUR = 00
            VOA[12] = "Ssns     :    " + SSN + "      0      0      0      0      0      0      0      0\r\n";               // SSN = 000
            VOA[13] = "Hours    :     " + HOUR + "      0      0      0      0      0      0      0      0\r\n";
            VOA[14] = "Freqs    : " + MHZ + ".000  0.000  0.000  0.000  0.000  0.000  0.000  0.000  0.000\r\n";            // MHZ = 00

            //  VOA[15] = "System   :  145     0.100   90   73     3.000     0.100\r\n"; // this is the standard VOA settings
            //   VOA[15] = "System   :  140     3.000   90   70     3.000     0.100\r\n";  // this is supposed to be the prefered amateur settings
            VOA[15] = "System   :  145     " + ANGLE + "   " + REL + "   " + CRS + "     3.000     0.100\r\n"; // this is the standard VOA settings
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
            catch (Exception q)
            {
                Debug.WriteLine("NEW VOA FILE NOT CREATED " + q);
                statusBoxSWL.Text = "NEW VOA1 FILE FAIL";

                goto VOACAP_FINISH;


            }


            //-------------------------------------- create a voacap data table from ke9ns.voa      see folder %userprofile%\AppData\Roaming\FlexRadio Systems\PowerSDR v2.8.0\itshfbc\areadata\default\ke9ns.voa

            Debug.WriteLine(" Create a voacap data table from ke9ns.voa");

            string s1 = Environment.CurrentDirectory;
            Debug.WriteLine("s1: " + s1);
            Environment.CurrentDirectory = console.AppDataPath + "itshfbc\\bin_win\\";

            try
            {

                string file_name2 = "voacapw.exe";        // c:\itshfbc AREA CALC default\ke9ns.voa"; // voacap data to create table  see: %userprofile%\AppData\Roaming\FlexRadio Systems\PowerSDR v2.8.0\itshfbc\areadata\default
                Debug.WriteLine("file2: " + file_name2);

                string argument = "SILENT c:.. AREA CALC default\\ke9ns.voa"; // voacap data to create table
                Debug.WriteLine("argument: " + argument);

                var proc1 = System.Diagnostics.Process.Start(file_name2, argument);

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

            if ((File.Exists(file_name)))
            {

                try
                {

                    stream222 = new FileStream(file_name, FileMode.Open); // open file
                    reader222 = new BinaryReader(stream222, Encoding.ASCII);

                    Debug.WriteLine("Read voacap data file ke9ns.vg1");

                }
                catch (Exception s)
                {
                    Debug.WriteLine("fault: " + s);
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

                for (int h = 0; h < 10; h++)
                {
                    VOA_S1[h] = 0; // reset number of each S unit reading found in the new map
                }

                //------------------------------------------------------------------
                Debug.WriteLine("reading VOACAP VG1 file");

                for (; ; )
                {

                    try
                    {
                        var newChar = (char)reader222.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader222.ReadChar(); // read \n char to finishline

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
                                catch (Exception)
                                {
                                    Debug.WriteLine("fault> " + result.ToString());
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


                reader222.Close();    // close  file
                stream222.Close();   // close stream


                Debug.WriteLine("Done Reading .VG1 FILE");

                Debug.WriteLine("convert LAT LONG data to X and Y Contour data base on S readings");

                Debug.WriteLine("SSS INDEX LENGTH " + VOA_Index); // should be 961 or 31 x 31

                for (int h = 0; h < 10; h++)
                {
                    Debug.WriteLine("Found number of S" + h + " readings: " + VOA_S1[h]);
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

                        yy = ((VOA_Ysize - 1) * VOA_Xsize) - yy; // -1 because 0 is the first index 960 to 0

                        y1[y - 2] = (int)(((180 - (VOA_LAT[yy] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude -90S to +90N
                        x1[x] = (int)(((VOA_LNG[x + (yy)] + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E
                        d1[x, y - 2] = (float)VOA_SDBW[x + (yy)];


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

                //  Convert.ToDouble(udDisplayLong.Value, NI)

                //  VOA_MyY = (int)(((180 - ((double)udDisplayLat.Value + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                //   VOA_MyX = (int)((((double)udDisplayLong.Value + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E

                VOA_MyY = (int)(((180 - (Convert.ToDouble(udDisplayLat.Value, NI) + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S
                VOA_MyX = (int)(((Convert.ToDouble(udDisplayLong.Value, NI) + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E

                Map_Last = Map_Last | 2;    // force update of world map


            } // if file exists VOA ke9ns.vg1

            Conrec.Contour(d1, x1, y1, z1); // create contours for map

            VOARUN = false;   // DONE. OK to run again now

            Debug.WriteLine("SSS8 ");

            VOATHREAD = false;
            return;  // finished GOOD


        VOACAP_FINISH:      // jumps here if there was a problem      
            VOATHREAD = false;
            VOARUN = false; // DONE. false = you can try again if you want.


        } // VOACAP thread



        //================================================================================
        public void checkBoxMUF_CheckedChanged(object sender, EventArgs e)
        {
           
            if (checkBoxMUF.Checked == false)
            {
                chkVoacap.Checked = false;
                Map_Last = Map_Last | 2;    // force update of world map

            }
            else
            {
                chkVoacap.Checked = true;  // flag to store if voa is on/off at next startup
                voaon = true;

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

        private bool VOACAP_SSN = false; // not used at the moment

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
            this.toolTip1.SetToolTip(this.tbPanPower, "VOACAP: " + ((int)tbPanPower.Value).ToString() + " watts");

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


        public Point3F[,] SSS = new Point3F[1000, 1000];

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

            if ((textBoxDXCall.Text == "DX Callsign") || (textBoxDXCall.Text == "TRY AGAIN") || (textBoxDXCall.Text == "") || (textBoxDXCall.Text.Length < 3)) // then try to use last dx spot if your on that freq still
            {
                call = DX_Station[DX_SELECTED]; // use current selected call

                if ((DX_Freq[DX_SELECTED] == Display.VFOA)) // still on last DX spotter field selected
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

                } // VFOA
                else if ((DX_Freq[DX_SELECTED] == Display.VFOB)) // still on last DX spotter field selected
                {

                    try
                    {
                        freq1 = (console.FREQB * 1000).ToString("#.#");
                    }
                    catch (Exception)
                    {
                        textBoxDXCall.Text = "TRY AGAIN";

                        Debug.WriteLine("cannot parse VFOB");
                        return;
                    }

                    remarks = console.RX2DSPMode.ToString() + "p";  // send DSP MODE with - to indicate PowerSDR was sending it.

                   

                } // VFOB
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

                remarks = console.RX1DSPMode.ToString() + "p";

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

            } // just use current VFOA freq (not a prior DX SPOT)


            DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "Post this DX Contact to the Cluster (Callout)? \n[YES] Post your Contact.\n[No] Post as SWL Contact.\n[Cancel] will NOT Post Contact.",
               " DX Callout ",
               MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Question);

            if (dr == DialogResult.Cancel) return;


            if (dr == DialogResult.No)
            {
                remarks = "SWLp " + remarks;
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

        
        private void DXPost_MouseUp(object sender, MouseEventArgs e) // .211
        {
            Debug.WriteLine("CLICK DXPOST");

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

            // ke9ns: do below if you highlighted a DX SPot on the Spotter screen (your just respotting)
            if ((textBoxDXCall.Text == "DX Callsign") || (textBoxDXCall.Text == "TRY AGAIN") || (textBoxDXCall.Text == "") || (textBoxDXCall.Text.Length < 3)) // then try to use last dx spot if your on that freq still
            {
                Debug.WriteLine("DXPOST DX Callsign " + DX_Freq[DX_SELECTED] + " , " +  Display.VFOA + " , " + Display.VFOB);

                call = DX_Station[DX_SELECTED]; // use current selected call

                double freq;

                /*
                try  // ke9ns add  the try to prevent a crash
                {
                   
                    freq = (long) double.Parse(console.txtVFOBFreq.Text);  // ke9ns original
                    if ((freq > 65.0) && ((console.panelBandHFRX2.Visible == true || console.panelBandGNRX2.Visible == true)) && (console.txtVFOBFreq.Text.Contains(".") == false) && (console.txtVFOBFreq.Text.Contains(",") == false)) // check for khz entry instead of mhz
                    {
                        if (freq <= 999) // 3 digit must be khz 700 = 700 khz
                        {
                            freq = freq / 1000;     // 721 = .721 mhz

                        } // 3digite
                        else if (freq <= 9999) // 4 digits  7123 = 7.123 mhz
                        {
                            freq = freq / 1000;
                        }
                        else if (freq <= 99999) // 5 digits  12345 = 12.345 mhz
                        {
                            if (freq < 65000) freq = freq / 1000; // 30123 = 30.123 mhz
                            else freq = freq / 10000;  //65123 = 6.5123 mhz

                        }
                        else if (freq <= 999999) // 6 digits  123456  = .123456
                        {
                            freq = freq / 100000; // 123456  = .123456 
                        }
                        else if (freq <= 9999999) // 7 digits
                        {
                            freq = freq / 1000000; // 1212345 = 1.212345
                        }
                        else // 8 digits
                        {
                            freq = freq / 1000000; // 14123456 = 14.123456
                        }

                        Display.VFOB = (long)freq;

                     //   console.txtVFOBFreq.Text = freq.ToString("0.######");



                    } // assume anything over 65 is actually khz not mhz

                }
                catch (Exception e5)
                {
                    Debug.WriteLine("VFOAB issue " + e5);
                }

                */

                if ((DX_Freq[DX_SELECTED] == Display.VFOA)) // still on last DX spotter field selected
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

                } // VFOA
                else if ((DX_Freq[DX_SELECTED] == Display.VFOB)) // still on last DX spotter field selected
                {
                    Debug.WriteLine("Match VFOB");
                    try
                    {
                        freq1 = (console.FREQB * 1000).ToString("#.#");
                    }
                    catch (Exception)
                    {
                        textBoxDXCall.Text = "TRY AGAIN";

                        Debug.WriteLine("cannot parse VFOB");
                        return;
                    }

                    remarks = console.RX2DSPMode.ToString() + "p";  // send DSP MODE with - to indicate PowerSDR was sending it.



                } // VFOB
                else
                {
                    Debug.WriteLine("No longer on freq of last selected DX spotter line");
                    return;
                }




            } // use prior DX spot
            else if(e.Button == MouseButtons.Left) // otherwise use the current VFOA freq
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

                remarks = console.RX1DSPMode.ToString() + "p";

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

            } // just use current VFOA freq (not a prior DX SPOT)
            else if (e.Button == MouseButtons.Right) // otherwise use the current VFOB freq
            {
                call = textBoxDXCall.Text;

                try
                {
                    freq1 = (console.FREQB * 1000).ToString("#.#"); // in hz so convert to khz
                }
                catch (Exception)
                {
                    textBoxDXCall.Text = "TRY AGAIN";

                    Debug.WriteLine("cannot parse VFOB");
                    return;
                }

                remarks = console.RX2DSPMode.ToString() + "p";

              

            } // just use current VFOB freq (not a prior DX SPOT)


            DialogResult dr = MessageBox.Show(new Form { TopMost = true }, "Post this DX Contact to the Cluster (Callout)? \n[YES] Post your Contact.\n[No] Post as SWL Contact.\n[Cancel] will NOT Post Contact.",
               " DX Callout ",
               MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Question);

            if (dr == DialogResult.Cancel) return;


            if (dr == DialogResult.No)
            {
                remarks = "SWLp " + remarks;
            }


            char[] messageDXSPOT = ("DX " + freq1 + " " + call + " " + remarks).ToCharArray(); // convert hz to khz
            Debug.WriteLine("CREATE DX SPOT: " + "DX " + freq1 + " " + call + " " + remarks);


          
            for (int i = 0; i < messageDXSPOT.Length; i++)    // do it this way because telnet server wants slow typing
            {
                SP_writer.Write((char)messageDXSPOT[i]);

            }  // for loop length of your call sign

            SP_writer.Write((char)13);

            SP_writer.Write((char)10);

            textBoxDXCall.Text = "DX Callsign"; // reset back



        } // DXPost_MouseUp

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

                    Darken();  // adjust map image bright/dark and grayline

                    console.picDisplay.BackgroundImage = MAP;

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

                    Darken();

                    console.picDisplay.BackgroundImage = MAP;

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

        public void Calculate(double iss_lat, double iss_long, double iss_elv, double me_lat, double me_long, double me_elv)
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
        private void SpotControl_KeyDown(object sender, KeyEventArgs e) // ke9ns keypreview must be TRUE and use MouseIsOverControl(Control c)
        {
            Debug.WriteLine("F1 key0");



            if (e.KeyCode == Keys.F1) // ke9ns add for help messages (F1 help screen)
            {

                Debug.WriteLine("F1 key");

                if (MouseIsOverControl(checkBoxMUF) == true) // voacap checkbox
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/nBkeqs9No2E?rel=0&amp";
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.PropagationTextBox.Text;
                }
                else if (MouseIsOverControl(btnTrack) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/zninPwfSgJY?rel=0&amp";
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.TRACKMap.Text;
                }
                else if (MouseIsOverControl(button4) == true || MouseIsOverControl(textBox1) == true)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.youtube_embed = @"https://www.youtube.com/embed/zninPwfSgJY?rel=0&amp";
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.LoTW_help.Text;
                }


            } // if (e.KeyCode == Keys.F1)
            else if (e.KeyCode == Keys.F2) // ke9ns add for help messages (F2 help screen)
            {
                Debug.WriteLine("F2 key");

                if ((MouseIsOverControl(btnTime) == true) || (MouseIsOverControl(checkBoxWWV) == true) || (MouseIsOverControl(udDisplayWWV) == true))    // https://youtu.be/upUsAhYVoNg  (Time Sync VIDEO HELP)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.helpbox_message.Text;            // Time Sync

                    string VideoID = "upUsAhYVoNg";

                    console.helpboxForm.webBrowser1.Visible = true;
                    console.helpboxForm.webBrowser1.BringToFront();

                    console.helpboxForm.webBrowser1.DocumentText = String.Format("<html><head>" +
                             "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                             "</head><body>" +
                             "<iframe width=\"100%\" height=\"425\"  src=\"https://www.youtube.com/embed/{0}?autoplay=1&enablejsapi=1\"" +
                             "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                             "</body></html>", VideoID);

                } // if (MouseIsOverControl(txtTimer) == true)
                else if (MouseIsOverControl(button4) == true || MouseIsOverControl(textBox1) == true)    // https://youtu.be/zninPwfSgJY  (LoTW VIDEO HELP)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.helpbox_message.Text;            // LoTW

                    string VideoID = "zninPwfSgJY";

                    console.helpboxForm.webBrowser1.Visible = true;
                    console.helpboxForm.webBrowser1.BringToFront();

                    console.helpboxForm.webBrowser1.DocumentText = String.Format("<html><head>" +
                             "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                             "</head><body>" +
                             "<iframe width=\"100%\" height=\"425\"  src=\"https://www.youtube.com/embed/{0}?autoplay=1&enablejsapi=1\"" +
                             "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                             "</body></html>", VideoID);

                } // if (MouseIsOverControl(button4) == true)
                else if ((MouseIsOverControl(checkBoxMUF) == true) || (MouseIsOverControl(chkBoxContour) == true) || (MouseIsOverControl(chkBoxAnt) == true) || (MouseIsOverControl(tbPanPower) == true))    // https://youtu.be/nBkeqs9No2E  (VOACAP VIDEO HELP)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.helpbox_message.Text;            // VOACAP

                    string VideoID = "nBkeqs9No2E";

                    console.helpboxForm.webBrowser1.Visible = true;
                    console.helpboxForm.webBrowser1.BringToFront();

                    console.helpboxForm.webBrowser1.DocumentText = String.Format("<html><head>" +
                             "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                             "</head><body>" +
                             "<iframe width=\"100%\" height=\"425\"  src=\"https://www.youtube.com/embed/{0}?autoplay=1&enablejsapi=1\"" +
                             "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                             "</body></html>", VideoID);

                } // if (MouseIsOverControl(checkBoxMUF) == true)
                else if (MouseIsOverControl(btnTrack) == true)    // https://youtu.be/zninPwfSgJY  (World Map VIDEO HELP)
                {
                    if (console.helpboxForm == null || console.helpboxForm.IsDisposed) console.helpboxForm = new helpbox(console);

                    console.helpboxForm.Show();
                    console.helpboxForm.Focus();
                    console.helpboxForm.WindowState = FormWindowState.Normal; // ke9ns add
                    console.helpboxForm.helpbox_message.Text = console.helpboxForm.helpbox_message.Text;            // World Map

                    string VideoID = "zninPwfSgJY";

                    console.helpboxForm.webBrowser1.Visible = true;
                    console.helpboxForm.webBrowser1.BringToFront();

                    console.helpboxForm.webBrowser1.DocumentText = String.Format("<html><head>" +
                             "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                             "</head><body>" +
                             "<iframe width=\"100%\" height=\"425\"  src=\"https://www.youtube.com/embed/{0}?autoplay=1&enablejsapi=1\"" +
                             "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                             "</body></html>", VideoID);

                } // if (MouseIsOverControl(btnTrack) == true)



            } //  else if (e.KeyCode == Keys.F2) /

        } // SpotControl_KeyDown


        public bool MouseIsOverControl(Control c) // ke9ns keypreview must be TRUE and use MouseIsOverControl(Control c)
        {
            return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition));
        }

        private void btnTrack_MouseEnter(object sender, EventArgs e)
        {

        }

        private void SpotControl_MouseEnter(object sender, EventArgs e)
        {
            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click

            Console.HELPMAP = true;
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop.Checked == true) this.Activate();
            
        }

        private void btnTrack_MouseLeave(object sender, EventArgs e)
        {
            Console.HELPMAP = false;
        }




        //==================================================================================================================
        public double CWBase = 600; // 600 hz

        public int WPMCW = 10; // populated by spotDecoder.numRXCW

        public int ditTime = 0;
        public int dahTime = 0;
        public int letterTime = 0;
        public int wordTime = 0;

        public bool RECCW = false; // 
        public bool RECLast = false;

        public int elementAdvance = 0;
        public int letterAdvance = 0;
        public int wordAdvance = 0;

        //  public int[] LetterElement = new int[10];
        public int LetterFound = 0;

        public const string Alpha = ""; // 0-31 a=5,b=11,c=14,d=7,e=2,f=13,g=9,h=10,i=3,j=19,

        public int[] LetterHolder = new int[10];
        public string[,,,,] LetterElement = new string[4, 4, 4, 4, 4];


        public int RTTYSHIFT
        {

            get
            {
                return (int)SpotDecoderForm.RTTYShift;
            }
        }


        public int RTTYBASE
        {

            get
            {
                return (int)SpotDecoderForm.RTTYBase;
            }
        }

        //=====================================================================================
        unsafe public void CWTime()
        {
            GoertzelCoef(CWBase, console.SampleRate1);  // comes up with the Coeff values for the freq and sample rate used
            Coeff2 = 0.0;

            int Mark = 0; // value detected by the signal detector

            Stopwatch ditLength = new Stopwatch();        // WWV 100hz tick ON elapsed time to find if PCM BCD data stream pluse is 1 or 0
            Stopwatch spaceLength = new Stopwatch();       // WWV 100hz tick OFF elapsed time to find start of minute (HOLE)
            Stopwatch PaceLength = new Stopwatch();       // WWV 100hz tick OFF elapsed time to find start of minute (HOLE)
            Stopwatch timer1 = new Stopwatch();

            LetterElement[1, 2, 0, 0, 0] = "a";
            LetterElement[2, 1, 1, 1, 0] = "b";
            LetterElement[2, 1, 2, 1, 0] = "c";
            LetterElement[2, 1, 1, 0, 0] = "d";
            LetterElement[1, 0, 0, 0, 0] = "e";
            LetterElement[1, 1, 2, 1, 0] = "f";

            LetterElement[2, 2, 1, 0, 0] = "g";
            LetterElement[1, 1, 1, 1, 0] = "h";
            LetterElement[1, 1, 0, 0, 0] = "i";
            LetterElement[1, 2, 2, 2, 0] = "j";
            LetterElement[2, 1, 2, 0, 0] = "k";
            LetterElement[1, 2, 1, 1, 0] = "l";

            LetterElement[2, 2, 0, 0, 0] = "m";
            LetterElement[2, 1, 0, 0, 0] = "n";
            LetterElement[2, 2, 2, 0, 0] = "o";
            LetterElement[1, 2, 2, 1, 0] = "p";
            LetterElement[2, 2, 1, 2, 0] = "q";
            LetterElement[1, 2, 1, 0, 0] = "r";

            LetterElement[1, 1, 1, 0, 0] = "s";
            LetterElement[2, 0, 0, 0, 0] = "t";
            LetterElement[1, 1, 2, 0, 0] = "u";
            LetterElement[1, 1, 1, 2, 0] = "v";
            LetterElement[1, 2, 2, 0, 0] = "w";
            LetterElement[2, 1, 1, 2, 0] = "x";

            LetterElement[2, 1, 2, 2, 0] = "y";
            LetterElement[2, 1, 1, 1, 0] = "z";
            LetterElement[1, 2, 2, 2, 2] = "1";
            LetterElement[1, 1, 2, 2, 2] = "2";
            LetterElement[1, 1, 1, 2, 2] = "3";
            LetterElement[1, 1, 1, 1, 2] = "4";

            LetterElement[1, 1, 1, 1, 1] = "5";
            LetterElement[2, 1, 1, 1, 1] = "6";
            LetterElement[2, 2, 1, 1, 1] = "7";
            LetterElement[2, 2, 2, 1, 1] = "8";
            LetterElement[2, 2, 2, 2, 1] = "9";
            LetterElement[2, 2, 2, 2, 2] = "0";

            int dit = 80; // msec time for a dit
            int dah = 240; // msec time for a dah (3 dits)
            int CWSpace = 0; // msec time between words (7 dits in length)

            int CWLow = 300; // tone signal level 
            int CWHigh = 0; // tone signal level
            int CWAvg = 0;
            long RecordDITLength = 0;
            long RecordSpaceLength = 0;

            long TIMER1 = 0;
            long TIMER2 = 0;
            long TIMER3 = 0;

            bool IDENT = false; // Mark = true, Space = false

            ditLength.Reset();
            spaceLength.Reset();

            ditTime = 1200 / WPMCW; // dit time in mSec
            dahTime = ditTime * 3;
            wordTime = ditTime * 7;
            letterTime = dahTime;

            CWLow = 1000;

            Debug.WriteLine("DIT TIME= " + ditTime + ", dahTime= " + dahTime + ", WordTime=" + wordTime);

            timer1.Restart(); // reset and start timer now
            //--------------------------------------------
            while (console.RXCW == true)
            {
                ditTime = 1200 / WPMCW; // dit time in mSec
                dahTime = ditTime * 3;
                wordTime = ditTime * 7;
                letterTime = dahTime;

                TIMER1 = timer1.ElapsedMilliseconds;
                timer1.Restart();

                SpotDecoderForm.textBox2.Text = "DIT length (mSec): " + ditTime + "\r\n";
                SpotDecoderForm.textBox2.Text = SpotDecoderForm.textBox2.Text + "DAH length (mSec): " + dahTime + "\r\n";
                SpotDecoderForm.textBox2.Text = SpotDecoderForm.textBox2.Text + "Elapsed time to return to CWTIME THREAD: " + TIMER1 + "\r\n";


                if (console.WWVReady == true)              //   currFBox.AppendText(SWR_SLOT + " , " + ii.ToString("###0.000000") + " , " + temp9 + "\r\n");
                {
                    Mark = console.WWVTone;              // get Magnitude value from audio.cs and Goertzel routine

                    TIMER2 = PaceLength.ElapsedMilliseconds;
                    PaceLength.Restart();

                    SpotDecoderForm.textBox2.Text = SpotDecoderForm.textBox2.Text + "STRENGTH of SIGNAL: " + Mark + "\r\n";
                    SpotDecoderForm.textBox2.Text = SpotDecoderForm.textBox2.Text + "Elapsed time to get value data: " + TIMER2 + "\r\n";

                    Debug.WriteLine("MARK TONE SIZE= " + Mark + " , " + PaceLength.ElapsedMilliseconds);

                    console.WWVReady = false;

                    if (Mark > CWHigh)
                    {
                        CWHigh = (int)((double)Mark * 0.8); // threshold is 80% of last value received

                        //  Debug.WriteLine("MARK TONE SIZE= " + CWHigh);
                    }

                    if (Mark > CWLow)
                    {
                        RECCW = true;
                    }
                    else
                    {
                        RECCW = false;
                    }


                    if ((RECCW == true) || (RECLast == true)) // record length of dit or dah here
                    {

                        SpotDecoderForm.radioButton3.Checked = true;

                        if (ditLength.ElapsedMilliseconds == 0)
                        {
                            RecordSpaceLength = spaceLength.ElapsedMilliseconds; // record 
                                                                                 //   Debug.WriteLine("CW Last SpacLength: " + RecordSpaceLength);
                            ditLength.Start(); // start dit timer at start of each round
                            spaceLength.Reset(); // stops timer and resets to 0

                        }


                        if (RECCW == true)
                        {
                            RECLast = true;
                        }
                        else
                        {
                            RECLast = false; // allow running this routine only 1 timer after the signal appeas to be gone
                        }

                        if (ditLength.ElapsedMilliseconds >= ditTime)
                        {
                            LetterHolder[elementAdvance] = 1;
                            // dit
                        }
                        if (ditLength.ElapsedMilliseconds >= wordTime)
                        {
                            LetterHolder[elementAdvance] = 2;
                            // change to dah
                        }

                    } //
                    else // record length of empty space either A=dit/dah space, B=letter space, or C=word space
                    {

                        SpotDecoderForm.radioButton3.Checked = false;

                        if (spaceLength.ElapsedMilliseconds == 0)
                        {
                            RecordDITLength = ditLength.ElapsedMilliseconds; // record 
                                                                             // Debug.WriteLine("CW Last DitLength: " + RecordDITLength);

                            RecordSpaceLength = spaceLength.ElapsedMilliseconds;
                            spaceLength.Start(); // start space timer 1 time at start of each round
                            ditLength.Reset();
                        }


                        if (spaceLength.ElapsedMilliseconds >= ditTime)
                        {
                            elementAdvance++;

                        }// element

                        if (spaceLength.ElapsedMilliseconds >= letterTime)
                        {
                            elementAdvance = 0; // next letter so reset

                            LetterFound = 0;

                            if (LetterHolder[0] > 0)
                            {
                                Debug.WriteLine("CW GOT LETTER " + LetterHolder[0] + "," + LetterHolder[1] + "," + LetterHolder[2] + "," + LetterHolder[3] + "," + LetterHolder[4]);

                                string temp = LetterElement[LetterHolder[0], LetterHolder[1], LetterHolder[2], LetterHolder[3], LetterHolder[4]];
                                if ((temp != null) && (temp != ""))
                                {
                                    SpotDecoderForm.textBox2.Text = SpotDecoderForm.textBox2.Text + temp;
                                }
                            }

                            for (int i = 1; i < 5; i++)
                            {
                                LetterHolder[i] = 0; // reset it
                            }


                        } // letter

                        if (spaceLength.ElapsedMilliseconds >= wordTime)
                        {
                            SpotDecoderForm.textBox2.Text = SpotDecoderForm.textBox2.Text + " "; // space between words

                        } // word

                    } // SPACE



                } //  if (console.WWVReady == true)

                Thread.Sleep(10);

            } // while RXCW true

        } // CWTime



        //====================================================================================
        // DECODER (RTTY for now)
        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (SpotDecoderForm == null || SpotDecoderForm.IsDisposed)
                SpotDecoderForm = new SpotDecoder(console);

            SpotDecoderForm.Show();
            SpotDecoderForm.Focus();
        }

        private void numBeamHeading_ValueChanged(object sender, EventArgs e)
        {
            console.spotDDUtil_Rotor = ";"; // stop any prior rotor movement
        }


        private void button3_Click(object sender, EventArgs e)
        {
            //  console.spotDDUtil_Rotor = "AI1;";
            Debug.WriteLine("BEAM HEADING TRANSMIT");

            console.spotDDUtil_Rotor = "AP1" + numBeamHeading.Value.ToString().PadLeft(3, '0') + ";";
            console.spotDDUtil_Rotor = "AM1;"; // start moving

        }


        //==========================================================================
        // ke9ns add  (to allow you to lighten or darken the DX spotter world map)

        public int D1 = 0;
        public int D2 = 0;
        public int D3 = 0;
        public int D4 = 0;
        public void Darken()
        {
         //   Debug.WriteLine("DRAP " + D1 + "," + D2 + "," + D3 + "," + D4);
          
            MB = console.MB2; // map brightness
            MBG = console.MB3; // grayline brightness

            //.234
            // ke9ns world map = 1000,507 total image size (56,22 to 939,465  just the map inside the image) (map size = 883,443)
            // F layer map = 562,576 total image size (45,95 to 514,529 just the map inside the image) (map size = 469,434)
            // D layer map = 850,475 total image size,  (0,0 to 700,350  just the map inside the image) (map size = 700,350)
            // need to adjust F and D maps to fit within the 1000,507 rectangle image with the map of 883,443 inside it

            if (chkFLayerON.Checked) //.234
            {
                chkDLayerON.Checked = false;

                try
                {
                    Image imag = Image.FromFile(console.AppDataPath + "FRAP.gif"); // .235
                    Bitmap img7 = new Bitmap(imag);                 // ke9ns: To avoid indexed pixel format PNG issues
                    Rectangle r = new Rectangle(45, 95, 469, 434);    // this is the portion of the full F layer image that has the world map

                    Bitmap img8 = img7.Clone(r, img7.PixelFormat);  // make a new bitmap of just the F layer world map
                    Bitmap result = new Bitmap(1000, 507);          // this is the size we really want so it matches the built in world map
                    Rectangle r1 = new Rectangle(55, 20, 882, 449); // this is where to place the smaller F laywer world map into the big result bitmap

                    Bitmap Bar = new Bitmap(Map_F_Bar);              // .239 color bar legend for F-Layer
                    Rectangle r2 = new Rectangle(20, 50, 8, 300);    // .239 size of color legend bar

                    int x = 17;
                    int x1 = 27;

                    using (Graphics g = Graphics.FromImage(result))
                    {
                        
                     //   g.CompositingQuality = CompositingQuality.HighQuality;
                     //   g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                     //   g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawImage(img8, r1);  // this results in a d layer map with a empty boarder around all 4 edges.
                        
                        g.DrawString("SWS_AU", font2, new SolidBrush(Color.White), x, 10);
                        g.DrawString("foF2 NVIS", font2, new SolidBrush(Color.White), x, 20);
                        g.DrawString("Reflect:", font2, new SolidBrush(Color.White), x, 30);

                        g.DrawImage(Bar, r2); // .239draw legend color bar

                        g.DrawString("13mhz", font2, new SolidBrush(Color.Gray), x1, 55);
                        g.DrawString("12mhz", font2, new SolidBrush(Color.Violet), x1,  80);
                        g.DrawString("11mhz", font2, new SolidBrush(Color.Purple), x1,  105);
                        g.DrawString("10mhz", font2, new SolidBrush(Color.Maroon), x1, 125);
                        g.DrawString(" 9mhz", font2, new SolidBrush(Color.DarkBlue), x1, 150);
                        g.DrawString(" 8mhz", font2, new SolidBrush(Color.Blue), x1,  170);
                        g.DrawString(" 7mhz", font2, new SolidBrush(Color.DarkSlateBlue), x1,  195);
                        g.DrawString(" 6mhz", font2, new SolidBrush(Color.LightBlue), x1,  215);
                        g.DrawString(" 5mhz", font2, new SolidBrush(Color.LightGreen), x1,  240);
                        g.DrawString(" 4mhz", font2, new SolidBrush(Color.Green), x1,  265);
                        g.DrawString(" 3mhz", font2, new SolidBrush(Color.Olive), x1,  285);
                        g.DrawString(" 2mhz", font2, new SolidBrush(Color.Yellow), x1,  310);
                        g.DrawString(" 1mhz", font2, new SolidBrush(Color.Red), x1,  330);


                    }


                    MAP = Lighten(result, MBG, MB);
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                    MAP = Lighten(new Bitmap(Map_image), MBG, MB);
                }
            }
            else if (chkDLayerON.Checked) //.234
            {
                chkFLayerON.Checked = false;

                int x = 17;
                int x1 = 27;
               

                try
                {
                    Image imag = Image.FromFile(console.AppDataPath + "DRAP.png"); // bring in D-layer full image
                    Bitmap img7 = new Bitmap(imag);                 // ke9ns: To avoid indexed pixel format PNG issues
                    Rectangle r = new Rectangle(0, 0, 700, 350);    // this is the portion of the full D layer image that has the world map
                    Bitmap img8 = img7.Clone(r, img7.PixelFormat);  // make a new bitmap of just the D layer world map
                    Bitmap result = new Bitmap(1000, 507);          // this is the size we really want so it matches the built in world map
                    Rectangle r1 = new Rectangle(42, 16, 912, 456); // this is where to place the smaller d laywer world map into the big result bitmap

                    Bitmap Bar = new Bitmap(Map_D_Bar);              // .237 color bar legend for D-Layer
                    Rectangle r2 = new Rectangle(20, 50, 8, 300);    // .237 size of color legend bar

                    using (Graphics g = Graphics.FromImage(result)) 
                    {

                    //    g.CompositingQuality = CompositingQuality.HighQuality;
                    //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                     //   g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawImage(img8, r1);  // this results in a d layer map with a empty boarder around all 4 edges.
                       
                        g.DrawString("NOAA", font2, new SolidBrush(Color.White), x, 10);
                        g.DrawString("D-Layer", font2, new SolidBrush(Color.White), x, 20);
                        g.DrawString("Aborp:", font2, new SolidBrush(Color.White), x, 30);

                        g.DrawImage(Bar,r2); // .237 draw legend color bar
                     
                        g.DrawString("35mhz", font2, new SolidBrush(Color.Red), x1,  45);
                        g.DrawString("30mhz", font2, new SolidBrush(Color.Orange), x1, 90);
                        g.DrawString("25mhz", font2, new SolidBrush(Color.Yellow), x1,  130);
                        g.DrawString("20mhz", font2, new SolidBrush(Color.Green), x1,  170);
                        g.DrawString("15mhz", font2, new SolidBrush(Color.Cyan), x1,  220);
                        g.DrawString("10mhz", font2, new SolidBrush(Color.Blue), x1,  260);
                        g.DrawString(" 5mhz", font2, new SolidBrush(Color.Purple), x1,  300);
                        g.DrawString(" 0mhz", font2, new SolidBrush(Color.Gray), x1,  340);
                   

                    }

                    MAP = Lighten(result, MBG, MB);
                  
                }
                catch (Exception ex)
                {
                 //   MessageBox.Show(ex.Message);
                    MAP = Lighten(new Bitmap(Map_image), MBG, MB);
                }
            }
            else
            {
                if ((Console.DXR == 0))
                {
                    MAP = Lighten(new Bitmap(Map_image), MBG, MB);
                }
                else
                {
                    MAP = Lighten(new Bitmap(Map_image2), MBG, MB);
                }
            }

        } // Darken()



        //====================================================================
        // this takes the image and bitmaps it where you can alter the colors or brightness for each pixel
        public Bitmap Lighten(Bitmap bitmap, int gray, int amount)
        {
            if (amount < -255 || amount > 255) return bitmap;
            if (gray < -255 || gray > 255) return bitmap;
           

            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;           // get width of image

            System.IntPtr Scan0 = bmData.Scan0;    // handle of image

            int nVal = 0;

            unsafe // (working with pointers is unsafe, thats why we lockbits)
            {
                byte* p = (byte*)(void*)Scan0;         // pointer to byte inside image

                int nWidth = bitmap.Width * 3;         // each line is the width * 3 (RGB color data)
                int nOffset = stride - nWidth;        //  nOffset = difference between the Scan width and the RGB byte width of the image


                // ke9ns:  .159


                int tt = DateTime.UtcNow.DayOfYear;

                int ee = 0;  // for (int ee = Sun_Top1; ee < Sun_Bot1; ee++)
                int LightXX = 0;
                bool NoGray = false; // false= this RGB pixel is not part of the grayline

                for (int y = 0; y < bitmap.Height; ++y)    // each horizontal line from top to bottom  ( bitmap.Height = bottom)
                {
                    LightXX = 0;
                    int RRR = 0;

                    for (int x = 0; x < nWidth; ++x)       // 1 horizontal line from left to right  ( nWidth = right side)
                    {
                        if (++RRR == 3) // allow for every 3 RGB byte values
                        {
                            LightXX++;
                            RRR = 0;

                        }


                        if ((GRAYLINE == true) && (GRAYDONE == true) && ((ee >= Sun_Top1) && (ee <= Sun_Bot1) && (LightXX >= Sun_Left) && (LightXX <= Sun_Right))) // only change brightness of the image where the grayline resides
                        {
                            NoGray = false;

                            //-----------------------------------------------------------------
                            // ke9ns dusk  GrayLine_Pos3 & GrayLine_Pos1 data
                            if (GrayLine_Pos3[ee] == 0) // Dusk in center area
                            {
                                if ((GrayLine_Pos1[ee, 0]) == 0 && (GrayLine_Pos1[ee, 1] == 0) &&
                                    ((ee < (Sun_Top1 + 100)) && ((tt > GrayWinterStart) || (tt < GrayWinterStop)) ||  // winter time northern area is always dark
                                    (ee > (Sun_Bot1 - 100)) && (((tt >= GraySummerStart) && (tt < GraySummerStop))))   // summer time southern area is always dark
                                   ) // if the line is empty but your on top in the winter then draw a line anyway
                                {


                                    if ((LightXX >= Sun_Left) && (LightXX <= Sun_Right))
                                    {
                                        nVal = (int)(p[0] + gray);       // nVal =  current value of pixel + amount to brighten

                                        if (nVal < 0) nVal = 0;
                                        if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                        p[0] = (byte)nVal;            // store new value
                                        NoGray = true;
                                    }
                                }
                                else
                                {
                                    // draw line between the left and right sides (starting from the middle)

                                    if (GrayLine_Pos1[ee, 1] > GrayLine_Pos1[ee, 0]) // ,0 is smaller
                                    {
                                        if ((LightXX >= GrayLine_Pos1[ee, 0]) && (LightXX <= GrayLine_Pos1[ee, 1]))
                                        {
                                            nVal = (int)(p[0] + gray);       // nVal =  current value of pixel + amount to brighten

                                            if (nVal < 0) nVal = 0;
                                            if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                            p[0] = (byte)nVal;            // store new value
                                            NoGray = true;
                                        }
                                    }
                                    else // ,1 is the smaller
                                    {
                                        if ((LightXX >= GrayLine_Pos1[ee, 1]) && (LightXX <= GrayLine_Pos1[ee, 0]))
                                        {
                                            nVal = (int)(p[0] + gray);       // nVal =  current value of pixel + amount to brighten

                                            if (nVal < 0) nVal = 0;
                                            if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                            p[0] = (byte)nVal;            // store new value
                                            NoGray = true;
                                        }
                                    }

                                }

                            }
                            else if (GrayLine_Pos3[ee] == 1) //DUSK on LEFT
                            {
                                if ((LightXX <= GrayLine_Pos1[ee, 1]) && (LightXX >= Sun_Left) || (LightXX >= GrayLine_Pos1[ee, 0]) && (LightXX <= Sun_Right))
                                {
                                    nVal = (int)(p[0] + gray);       // nVal =  current value of pixel + amount to brighten

                                    if (nVal < 0) nVal = 0;
                                    if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                    p[0] = (byte)nVal;            // store new value
                                    NoGray = true;
                                }

                            }
                            else // DUSK on RIGHT side
                            {
                                if ((LightXX >= GrayLine_Pos1[ee, 1]) && (LightXX <= Sun_Right) || (LightXX <= GrayLine_Pos1[ee, 0]) && (LightXX >= Sun_Left))
                                {
                                    nVal = (int)(p[0] + gray);       // nVal =  current value of pixel + amount to brighten

                                    if (nVal < 0) nVal = 0;
                                    if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                    p[0] = (byte)nVal;            // store new value
                                    NoGray = true;
                                }


                            } //if (GrayLine_Pos3[ee] == 0) 

                            //-----------------------------------------------------------------
                            //-----------------------------------------------------------------
                            // ke9ns DARK

                            if (GrayLine_Pos2[ee] == 0) // DARK in center area
                            {
                                if ((GrayLine_Pos[ee, 0]) == 0 && (GrayLine_Pos[ee, 1] == 0) &&
                                    ((ee < (Sun_Top1 + 100)) && ((tt > GrayWinterStart) || (tt < GrayWinterStop)) ||  // winter time northern area is always dark
                                    (ee > (Sun_Bot1 - 100)) && (((tt >= GraySummerStart) && (tt < GraySummerStop))))   // summer time southern area is always dark
                                   ) // if the line is empty but your on top in the winter then draw a line anyway
                                {


                                    if ((LightXX >= Sun_Left) && (LightXX <= Sun_Right))
                                    {
                                        nVal = (int)(p[0] + gray - 10);       // nVal =  current value of pixel + amount to brighten

                                        if (nVal < 0) nVal = 0;
                                        if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                        p[0] = (byte)nVal;            // store new value
                                        NoGray = true;
                                    }
                                }
                                else
                                {
                                    // draw line between the left and right sides (starting from the middle)

                                    if (GrayLine_Pos[ee, 1] > GrayLine_Pos[ee, 0]) // ,0 is smaller
                                    {
                                        if ((LightXX >= GrayLine_Pos[ee, 0]) && (LightXX <= GrayLine_Pos[ee, 1]))
                                        {
                                            nVal = (int)(p[0] + gray - 10);       // nVal =  current value of pixel + amount to brighten

                                            if (nVal < 0) nVal = 0;
                                            if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                            p[0] = (byte)nVal;            // store new value
                                            NoGray = true;
                                        }
                                    }
                                    else // ,1 is the smaller
                                    {
                                        if ((LightXX >= GrayLine_Pos[ee, 1]) && (LightXX <= GrayLine_Pos[ee, 0]))
                                        {
                                            nVal = (int)(p[0] + gray - 10);       // nVal =  current value of pixel + amount to brighten

                                            if (nVal < 0) nVal = 0;
                                            if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                            p[0] = (byte)nVal;            // store new value
                                            NoGray = true;
                                        }
                                    }

                                }

                            }
                            else if (GrayLine_Pos2[ee] == 1) //Dark on LEFT
                            {
                                if ((LightXX <= GrayLine_Pos[ee, 1]) && (LightXX >= Sun_Left) || (LightXX >= GrayLine_Pos[ee, 0]) && (LightXX <= Sun_Right))
                                {
                                    nVal = (int)(p[0] + gray - 10);       // nVal =  current value of pixel + amount to brighten

                                    if (nVal < 0) nVal = 0;
                                    if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                    p[0] = (byte)nVal;            // store new value
                                    NoGray = true;
                                }

                            }
                            else // DARK on RIGHT side
                            {
                                if ((LightXX >= GrayLine_Pos[ee, 1]) && (LightXX <= Sun_Right) || (LightXX <= GrayLine_Pos[ee, 0]) && (LightXX >= Sun_Left))
                                {
                                    nVal = (int)(p[0] + gray - 10);       // nVal =  current value of pixel + amount to brighten

                                    if (nVal < 0) nVal = 0;
                                    if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                    p[0] = (byte)nVal;            // store new value
                                    NoGray = true;
                                }


                            } //if (GrayLine_Pos2[ee] == 0) 


                            //---------------------------------------------------
                            //---------------------------------------------------

                            if (NoGray == false) // this RGB pixel not part of the gray line 
                            {
                                nVal = (int)(p[0] + amount);       // nVal =  current value of pixel + amount to brighten

                                if (nVal < 0) nVal = 0;
                                if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                                p[0] = (byte)nVal;            // store new value

                            }
                        } //  if ((ee >= Sun_Top1) && (ee <= Sun_Bot1) && (LightXX >= Sun_Left) && (LightXX <= Sun_Right)) 
                        else // the rest of the map
                        {

                            nVal = (int)(p[0] + amount);       // nVal =  current value of pixel + amount to brighten

                            if (nVal < 0) nVal = 0;
                            if (nVal > 255) nVal = 255;  // make sure new value does not fall out of range

                            p[0] = (byte)nVal;            // store new value

                        }

                           
                        ++p;                          // get next byte (remember 3 bytes per pixel)

                    } //  for (int x = 0; x < nWidth; ++x) each horizontal line from left to right

                    p += nOffset; // get start of next line
                    ee++; // next line

                }  // for (int y = 0; y < bitmap.Height; ++y)





            } // unsafe

            bitmap.UnlockBits(bmData);

            return bitmap;

        } // lighten or darken a bitmap image



        //==========================================================================================================
        //==========================================================================================================
        //==========================================================================================================
        //KE9NS see HTTP.cs for routine to communicate with LoTW

        public string LoTWcall = "callsign"; // dxspot callsign your checking for dups
        public string LoTWPASS = "password"; // your password to login to LoTW
                                             // callbox.Text is the user name for LoTW

        // ke9ns this is to get ALL your LoTW data EVER uploaded
        public int LoTWResult = 0; // 2=good, 3=bad
        public bool LoTWDone = false; // TRUE = finished getting data from LoTW (but not nesessarily successful)


        public List<string> LoTW_callsign = new List<string>(); // create list to hold strings of callsign data   LoTW_callsign.Add(yourstringhere);  LoTW_callsign.Clear()
        public List<string> LoTW_mode = new List<string>();
        public List<string> LoTW_band = new List<string>();
        public List<string> LoTW_dxcc = new List<string>(); // country or entity listing
        public List<string> LoTW_state = new List<string>(); // USA state
        public List<string> LoTW_grid = new List<string>();
        public List<string> LoTW_qsl = new List<string>(); // Y or N


        RTFBuilderbase LoTWRTF = new RTFBuilder(); // this stringbuilder will holds dx callsign along with its color to specify the DXCC,WAS,DUP state as compared to FCCData and your LoTW database
                                                   // this is used to insert to textBox1 RTF


        bool LoTW_GET = false; // true = login to LoTW and download your QSO database to LoTW_LOG.adi the next time you Left click on the LoTW button
        public int lotw_records = 0; // how many records show up on lotw_log.adi file (on screen counters)
        public int lotw_records1 = 0; // how many with DXCC
        public int lotw_records2 = 0; // how many with grid
        public int lotw_records3 = 0; // how many qsls


        bool LoTW_timer = false;

        //==================================================
        // show timer ticks while waiting for LoTW download
        private void timer1()
        {
            Stopwatch LoTWTimer = new Stopwatch();
            // textBox1.Text += "\r\n";

            LoTWTimer.Restart();

            do
            {
                Thread.Sleep(100); // to measure time

                if (LoTWTimer.ElapsedMilliseconds > 5000)
                {
                    LoTWTimer.Restart();
                    textBox1.Text += "*";
                }

            } while (LoTW_timer == true);

            textBox1.Text += "\r\n";

            LoTWTimer.Stop();

        } // timer1


        //=====================================================================================
        //=====================================================================================
        //=====================================================================================
        // LoTW THREAD  (attempt to download a QSO file from LoTW)

        bool runLoTW = false; // true = thread is running
        bool runFCC = false; // true = fcc database present (found)

        // string FCC_STATES = ""; // string of ASCII from FCC callsign database

        List<string> FCCSTATE = new List<string>(); // parse EN.dat into FCC.dat with callsign,state
        List<string> FCCCALL = new List<string>(); // parse EN.dat into FCC.dat with callsign,state

        int FCCSTATE_NUM = 0; // number of callsigns in the FCC.dat database

        // http://wireless.fcc.gov/uls/data/complete/l_amat.zip    this is how you get en.dat, which powerSDR turns into FCCDATA.dat file

        private void UpdateLoTW()
        {

            if (runLoTW == true)
            {
                LoTW_timer = false;
                return;
            }
            runLoTW = true;
            Thread.Sleep(2);

            lotw_records = 0;
            lotw_records1 = 0;
            lotw_records2 = 0;
            lotw_records3 = 0;

            LoTW_timer = false;

            string file_name2 = console.AppDataPath + "LoTW_LOG.adi"; // your LoTW_LOG file

            //  string file_name5 = console.AppDataPath + "fcc_lotw_eqsl.sql"; // your FCC callsign database

            string file_name5 = console.AppDataPath + "EN.dat"; // your FCC callsign database
            string file_name6 = console.AppDataPath + "FCCDATA.dat"; // your FCC callsign database

            if (!File.Exists(file_name2) || ((LoTW_GET == true)) && (button4.BackColor == SystemColors.ButtonFace)) // if file does not exist or you want to (force) update it.
            {
                FCCCALL.Clear();
                FCCSTATE.Clear();
                LoTW_callsign.Clear();
                LoTW_mode.Clear();
                LoTW_band.Clear();
                LoTW_dxcc.Clear();
                LoTW_state.Clear();
                LoTW_grid.Clear();
                LoTW_qsl.Clear();


                LoTW_GET = false;
                button4.BackColor = Color.Yellow;
                Debug.WriteLine("Create new database file");

                textBox1.Text = "Contacting ARRL LoTW Server. Please wait.\r\n";

                LoTWDone = false;

                Debug.WriteLine("Contacting LoTW server");

                pause = true; // halt display
                button1.Text = "Paused";

                LoTW_timer = true;

                Thread t3 = new Thread(new ThreadStart(timer1));

                t3.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t3.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t3.Name = "Timer thread";
                t3.IsBackground = true;
                t3.Priority = ThreadPriority.BelowNormal;
                t3.Start();

                string currlotw = console.httpFile.Lotw1(); // contact ARRL LoTW and download LOG files from them here (see HTTP.cs)

                LoTW_timer = false;
              //  pause = false; // turn display back on
              //  button1.Text = "Pause";

                Debug.WriteLine("Finished talking to LoTW server");

                textBox1.Text += "\r\nDONE. Finished downloading your LoTW QSO's & QSL's, saving as LoTW_LOG.adi in the Database folder.\r\n";

                if (LoTWResult == 2 && currlotw != "NOT READY") // 2=good
                {

                    if (currlotw.Contains("You must have been issued a"))
                    {
                        button4.BackColor = Color.Red;

                        Debug.WriteLine("WRONG PASSWORD: " + currlotw);

                        textBox1.Text += "PROBLEM: Could NOT login to LoTW (password or user name incorrect).\r\n";

                        textBox1.Text += "You may not have entered your correct Callsign into the lower right corner box?\r\n" +
                                          "And/Or your have not entered in your LoTW password correctly?\r\n" +
                                          "Right Click on your Callsign (lower right box). A Password Box will open up just above your Callsign.\r\n" +
                                          "Enter in your LoTW password and Right Click on your Callsign, to close the Password box.\r\n" +
                                          "Now Try a Right Click, then a Left Click on the LoTW button.\r\n";
                        try
                        {
                            File.Delete(file_name2);
                        }
                        catch
                        {

                        }

                        goto LoTW1; // end if bad.
                    }


                }


            } //  if (!File.Exists(file_name2)) LoTW_LOG.adi file does not exist then download it from the LoTW server and save it

            //..............................................................................................................
            //..............................................................................................................
            //..............................................................................................................
            // ke9ns  once you have an LoTW LOG file created (downloaded) then open the file here


            if (File.Exists(file_name2)) // if file does exist, then parse the file
            {

                Debug.WriteLine("DONE WITH HTTP.cs LoTW, back to spot.cs");
              
                button4.BackColor = Color.Yellow;

                //   Debug.WriteLine("LoTW OUTPUT: " + currlotw.Substring(0,1000));

                // get QSL info from LoTW
                // https://lotw.arrl.org/lotwuser/lotwreport.adi?login=ke9ns&password=********&qso_query=1

                // get details on both QSO and QSL info from LoTW
                // https://lotw.arrl.org/lotwuser/lotwreport.adi?login=ke9ns&password=********&qso_query=1&qso_qsldetail=yes&qso_qsl=no&qso_query=1  (stopped working around Oct 2020)
                // https://lotw.arrl.org/lotwuser/lotwreport.adi?login=ke9ns&password=********&qso_qsl=no&qso_query=1&qso_qsorxsince=1901-01-01&qso_qsldetail=yes (now requires a start date


                // <CALL:4>WA7U
                // <BAND:2>6M
                // <FREQ:8>50.31420
                // <MODE:3>FT8
                // <APP_LoTW_MODEGROUP:4>DATA
                // <QSO_DATE:8>20180621
                // <TIME_ON:6>224700
                // <QSL_RCVD:1>Y
                // <QSLRDATE:8>20180624
                // <eor>

                /* SAMPLE
                 * 
                <PROGRAMID:4>LoTW
                <APP_LoTW_LASTQSL:19>2018-06-24 19:41:15
                <APP_LoTW_NUMREC:3>541
                <eoh>

                <CALL:4>WA7U
                <BAND:2>6M
                <FREQ:8>50.31420
                <MODE:3>FT8
                <APP_LoTW_MODEGROUP:4>DATA
                <QSO_DATE:8>20180621
                <TIME_ON:6>224700
                <QSL_RCVD:1>Y
                <QSLRDATE:8>20180624
                <DXCC:3>291
                <COUNTRY:24>UNITED STATES OF AMERICA
                <APP_LoTW_DXCC_ENTITY_STATUS:7>Current
                <PFX:3>WA7
                <APP_LoTW_2xQSL:1>Y
                <GRIDSQUARE:4>DN45
                <STATE:2>MT // Montana
                <CNTY:11>MT,GALLATIN // Gallatin
                <CQZ:2>04
                <APP_LoTW_CQZ_Inferred:1>Y // from DXCC entity and PAS
                <eor>
  
                 
                 */

                Debug.WriteLine("Read LOG master file: " + file_name2);


                string lotw_log = File.ReadAllText(file_name2);
            
               

                if (lotw_log.Contains("You must have been issued a"))
                {
                    button4.BackColor = Color.Red;
                    textBox1.Text += "You may not have entered your correct Callsign into the lower right corner box?\r\n" +
                                        "And/Or your have not entered in your LoTW password correctly?\r\n" +
                                        "Right Click on your Callsign (lower right box). A Password Box will open up just above your Callsign.\r\n" +
                                        "Enter in your LoTW password and Right Click on your Callsign, to close the Password box.\r\n" +
                                        "Now Try a Right Click, then a Left Click on the LoTW button.\r\n";


                    try
                    {
                        File.Delete(file_name2);
                    }
                    catch
                    {

                    }

                    lotw_records = 0; // keep feature OFF 
                    goto LoTW1; // cant open file so end it now.

                } //  if (lotw_log.Contains("You must have been issued a"))

                textBox1.Text += "Your LoTW_LOG.adi QSO file is being parsed... Please wait.. \r\n";

                int x1 = lotw_log.IndexOf("<APP_LoTW_NUMREC:"); // find header at top of string

                int x = lotw_log.IndexOf("<eoh>"); // find header at top of string

                var ss = lotw_log.Substring(x1 + 19, x - (x1 + 19)); // start and length

                try
                {
                    lotw_records = Convert.ToInt32(ss); // get total number of LoTW QSO records to parse

                    Debug.WriteLine("-LoTW database records count: " + lotw_records);


                    for (x1 = 0; x1 < lotw_records; x1++)  // get each QSO record
                    {

                        int y = lotw_log.IndexOf("<eor>", x + 5);

                        var lotwsubstring = lotw_log.Substring(x + 5, y - (x + 5)); // this is 1 QSO record

                        x = y; // move pointer to start of next


                        int z = lotwsubstring.IndexOf("<CALL:");
                        if (z < 1)
                        {
                            LoTW_callsign.Add("---");
                          
                        }
                        else
                        {
                            LoTW_callsign.Add(lotwsubstring.Substring(z + 8, Convert.ToInt32(lotwsubstring.Substring(z + 6, 1)))); // call callsign
                        }
                                                
                        int z2 = lotwsubstring.IndexOf("<APP_LoTW_MODEGROUP:");
                        if (z2 < 1)
                        {
                            LoTW_mode.Add("---");
                        }
                        else
                        {
                             LoTW_mode.Add(lotwsubstring.Substring(z2 + 22, Convert.ToInt32(lotwsubstring.Substring(z2 + 20, 1)))); // call mode group (data, phone)
                        }
                     
                        int z4 = lotwsubstring.IndexOf("<BAND:");
                        if (z4 < 1)
                        {
                            LoTW_band.Add("----");
                        }
                        else
                        {
                             LoTW_band.Add(lotwsubstring.Substring(z4 + 8, Convert.ToInt32(lotwsubstring.Substring(z4 + 6, 1)))); // call band
                        }
                                            
                        int z6 = lotwsubstring.IndexOf("<DXCC:");
                        if (z6 < 1)
                        {
                            LoTW_dxcc.Add("0");
                        }
                        else
                        {
                            LoTW_dxcc.Add(lotwsubstring.Substring(z6 + 8, Convert.ToInt32(lotwsubstring.Substring(z6 + 6, 1)))); // call dxcc entity #
                            lotw_records1++;
                        }
                      
                        int z8 = lotwsubstring.IndexOf("2xQSL:");
                        if (z8 < 1)
                        {
                            LoTW_qsl.Add("----");
                        }
                        else
                        { 
                            LoTW_qsl.Add(lotwsubstring.Substring(z8 + 8, Convert.ToInt32(lotwsubstring.Substring(z8 + 6, 1)))); // call 2 way QSL confirmed Y / N
                            lotw_records3++;
                        }

                        int z10 = lotwsubstring.IndexOf("STATE:");
                        if (z10 < 1) // this is because not all countries have states.
                        {
                            LoTW_state.Add("----");
                        }
                        else 
                        {
                            LoTW_state.Add(lotwsubstring.Substring(z10 + 8, Convert.ToInt32(lotwsubstring.Substring(z10 + 6, 1)))); // call dxcc entity #
                        }
                      
                        
                        int z12 = lotwsubstring.IndexOf("QUARE:");
                        if( z12 < 1)
                        {
                            LoTW_grid.Add("----");
                        }
                        else 
                        {
                            LoTW_grid.Add(lotwsubstring.Substring(z12 + 8, Convert.ToInt32(lotwsubstring.Substring(z12 + 6, 1)))); // grid
                            lotw_records2++;
                        }
                    

                    } // for x1 loop


                }
                catch (Exception e)
                {
                    button4.BackColor = Color.Red;
                    Debug.WriteLine("Failed reading lotw log from file1 e" + e);

                    textBox1.Text += "Failed to read LOG file: " + e;

                    lotw_records = 0; // keep feature OFF 
                }

                
                lotw_log = null;

                //..........................................................
                //..........................................................
                //..........................................................
                // open FCC callsign STATES database
                // http://wireless.fcc.gov/uls/data/complete/l_amat.zip
                // extract just the EN.dat file (text based)
                // then convert it to FCC.dat file with just the callsign and state



                if (File.Exists(file_name6)) // if FCC.dat file does exist, then parse the file
                {
                    button4.BackColor = Color.Yellow;

                    FCCSTATE_NUM = 0;


                    try
                    {
                        stream2 = new FileStream(file_name6, FileMode.Open); // open EN.dat file
                        reader2 = new BinaryReader(stream2, Encoding.ASCII); // ASCII so you dont get extra length binary characters added to strings

                    }
                    catch (Exception)
                    {
                        button4.BackColor = Color.Red;
                        Debug.WriteLine("Failed opening lotw log from file to read");
                        goto LoTW1; // cant open file so end it now.

                    }

                    Debug.WriteLine("GET FCC STATES: ");

                    try
                    {
                        for (; ; )
                        {
                            FCCCALL.Add(reader2.ReadString());
                            FCCSTATE.Add(reader2.ReadString());

                            //   Debug.WriteLine("CALL: " + FCCCALL[FCCSTATE_NUM]);
                            //   Debug.WriteLine("STATE: " + FCCSTATE[FCCSTATE_NUM]);

                            FCCSTATE_NUM++;
                        }
                    }
                    catch
                    {
                        Debug.WriteLine("End of FCC states file");
                    }

                    reader2.Dispose();
                    stream2.Dispose();
                    reader2.Close();    // close  file
                    stream2.Close();   // close stream


                    //   Debug.WriteLine("CALL: " + FCCCALL[1000]);
                    //   Debug.WriteLine("STATE: " + FCCSTATE[1000]);

                    textBox1.Text += "Found FCCDATA.dat callsign database file, callsigns found: " + FCCSTATE_NUM + "\r\n";


                } // if FCC.dat file exists
                else
                {
                    Debug.WriteLine("Cannot find FCC database file will try to create the file from EN.dat ");


                    if (File.Exists(file_name5)) // if FCC file does exist, then parse the file
                    {
                        var result = new StringBuilder();

                        Debug.WriteLine("OPENING EN.dat file");

                        textBox1.Text += "Found raw FCC EN.dat callsign database file, creating FCCDATA.dat file.\r\n";

                        button4.BackColor = Color.Yellow;

                        try
                        {
                            stream2 = new FileStream(file_name5, FileMode.Open); // open file
                            reader2 = new BinaryReader(stream2, Encoding.ASCII);

                        }
                        catch (Exception)
                        {
                            button4.BackColor = Color.Red;
                            textBox1.Text += "No US State FCC database, could not find EN.dat nor FCCDATA.dat files..\r\n";

                            Debug.WriteLine("Failed opening EN.dat file");
                            goto LoTW1; // cant open file so end it now.

                        }

                        for (; ; ) // read EN.dat file and extract data from it and close it
                        {

                            try
                            {
                                var newChar = (char)reader2.ReadChar();

                                if ((newChar == '\r')) // 0d = \r
                                {
                                    newChar = (char)reader2.ReadChar(); // read \n char to finishline and get the \n

                                    string[] values = result.ToString().Split('|'); // split line up into segments divided by ,

                                    FCCSTATE.Add(values[17]);
                                    FCCCALL.Add(values[4]);

                                    //   Debug.WriteLine("CALLSIGN #: " + FCCSTATE_NUM + " == " + FCCSTATE[FCCSTATE_NUM]);
                                    FCCSTATE_NUM++;

                                    result.Clear();

                                }
                                else
                                {
                                    result.Append(newChar);  // save char
                                }
                            }
                            catch
                            {
                                Debug.WriteLine("END OF EN.dat file #of calls: " + FCCSTATE_NUM);
                                break;
                            }

                        } // for loop 

                        reader2.Dispose();
                        stream2.Dispose();
                        reader2.Close();    // close  file
                        stream2.Close();   // close stream

                        //....................................................................
                        // save FCC.dat file to save time next time
                        try
                        {

                            stream2 = new FileStream(file_name6, FileMode.Create); // open file
                            BinaryWriter writer2 = new BinaryWriter(stream2, Encoding.ASCII);

                            for (int v = 0; v < FCCSTATE_NUM; v++)
                            {

                                //   byte[] temp1 = Encoding.ASCII.GetBytes(FCCCALL[v]+ "\r\n");
                                //   byte[] temp2 = Encoding.ASCII.GetBytes(FCCSTATE[v]+"\r\n");
                                //    writer2.Write(temp1);
                                //    writer2.Write(temp2);

                                writer2.Write(FCCCALL[v]);
                                writer2.Write(FCCSTATE[v]);

                            }

                            writer2.Dispose();
                            stream2.Dispose();
                            writer2.Close();    // close  file
                            stream2.Close();   // close stream

                           

                        }
                        catch (Exception)
                        {
                            button4.BackColor = Color.Red;
                            Debug.WriteLine("Failed to create FCC.dat file");
                            goto LoTW1; // cant open file so end it now.

                        }

                    } // create the FCCDATA.dat file for EN.dat


                } // could not find FCCDATA.dat so created using EN.dat 

                textBox1.Text += "LoTW_LOG.adi QSO File Parsed, and ready.\r\nFound " + lotw_records + " QSO's, and " + lotw_records3 + " QSL's, and QSO's with DXCC Entities= " + lotw_records1 + ",\r\nAnd QSO's with grids= " + lotw_records2 + " \r\n";

                button4.BackColor = Color.MediumSpringGreen;

            } // if LoTW_LOG.adi file exists then parse it

        LoTW1:; // allows a failure to jump to the end
            runLoTW = false; // done with thread

        } // UpdateLoTW()


        //===================================================================================
        private void callBox_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if ((me.Button == System.Windows.Forms.MouseButtons.Right))
            {
                if (txtLoTWpass.Visible == false) txtLoTWpass.Visible = true;
                else if (txtLoTWpass.Visible == true) txtLoTWpass.Visible = false;


            }
           

        } // callBox_MouseDown

        private void txtLoTWpass_TextChanged(object sender, EventArgs e)
        {
            LoTWPASS = txtLoTWpass.Text;
            Debug.WriteLine("LoTWPASSWORD HERE");

        }

        //==========================================================
        // download your LoTW QSO file
        private void button4_Click(object sender, EventArgs e)
        {

            if ((lotw_records == 0) && (runLoTW == false) && (LoTWPASS != "password") && (callBox.Text != "callsign"))
            {
                Debug.WriteLine("LoTW1 THREAD START ");
                Thread t = new Thread(new ThreadStart(UpdateLoTW));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "Update LoTW Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.BelowNormal;
                t.Start();




                //  button4.BackColor = Color.Yellow;
            }
            else // missing password, missing callsign, lotw_records > 0,
            {
                if (((button4.BackColor != Color.MediumSpringGreen) && (lotw_records > 0)) || (LoTWPASS == "password") || (callBox.Text == "callsign") || (button4.BackColor == Color.Red)) // only show this text if you clicked on LoTW button and it was not Yellow
                {
                    textBox1.Text = "Could not Start LoTW checker.\r\n" +
                        "You may not have entered your correct Callsign into the lower right corner box.\r\n" +
                        "And/Or your have not entered in your LoTW password yet.\r\n" +
                        "Right Click on your Callsign (lower right box). A Password Box will open up just above your Callsign.\r\n" +
                        "Enter in your LoTW password and Right Click on your Callsign, to close the Password box.\r\n" +
                        "Now Try a Right Click, then a Left Click on the LoTW button.\r\n";


                    Debug.WriteLine("LoTW THREAD  NO START: " + LoTWPASS + " , " + callBox.Text);
                    button4.BackColor = Color.Red;
                }
                else
                {
                    button4.BackColor = SystemColors.ButtonFace;
                    FCCCALL.Clear();
                    FCCSTATE.Clear();
                    LoTW_callsign.Clear();
                    LoTW_mode.Clear();
                    LoTW_band.Clear();
                    LoTW_dxcc.Clear();
                    LoTW_state.Clear();
                    LoTW_grid.Clear();
                    LoTW_qsl.Clear();
                  
                    lotw_records = 0;
                    lotw_records1 = 0;
                    lotw_records2 = 0;
                    lotw_records3 = 0;

                 //   textBox1.Text = "LoTW checker OFF.\r\n";

                }



                lotw_records = 0; // turn off LoTW checking system

            }

        } // button4_Click

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if ((me.Button == System.Windows.Forms.MouseButtons.Right) && button4.BackColor == SystemColors.ButtonFace)
            {
                LoTW_GET = true;
                textBox1.Text = "You have now selected to Download a new LoTW QSO/QSL file from ARRL LoTW. \r\n";
                button4_Click(this, EventArgs.Empty); // force a download of LoTW


            }
            else if (me.Button == System.Windows.Forms.MouseButtons.Right)
            {
                textBox1.Text = "Left click to turn OFF LoTW first, then Right Click to Download.\r\n";
            }

        } //  button4_MouseDown

        private void MenuItem4_Click(object sender, EventArgs e)
        {
            if (SpotAge == null || SpotAge.IsDisposed)
                SpotAge = new SpotAge();

            SpotAge.Show();
            SpotAge.Focus();
        }


        private void hkBoxSpotBand_CheckedChanged(object sender, EventArgs e)
        {

            processTCPMessage(); // using dx_band[] to list only spot on your current band

        } //  hkBoxSpotBand_CheckedChanged

        private void hkBoxSpotRX2_CheckedChanged(object sender, EventArgs e)
        {
            processTCPMessage(); // using dx_band[] to list only spot on your current band
        }

        // ke9ns add .201
        string LoTW_DXCall = "";
        int LoTW_DXCallIndex = 0;

        int ToolTipNumber = 0; //.232

        int LastLocation = 0; //.232

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
          //  textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click


            if ((SP4_Active == 0) && (beacon1 == false))// only process lookup if not processing a new spot which might cause issue
            {
                int ii = textBox1.GetCharIndexFromPosition(e.Location);

                byte iii = (byte)(ii / LineLength);  // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added

                if (LastLocation != iii) // .232 if you move cursor to new line, new tooltip
                {
                    LastLocation = iii;

                    if (ToolTipNumber != 499) ToolTipNumber = 0;
                }

                if (DXt_Index > iii)
                {
                    string DXName = DXt_Station[iii];

                 //  Debug.WriteLine("Line " + iii + " Name " + DXName + " ,X= " + e.X + " ,Y= " + e.Y + " , ");

                    LoTW_DXCall = DXName;
                    LoTW_DXCallIndex = iii;

                 //   Debug.WriteLine("LoTW HOVER " + LoTW_DXCallIndex + " , " + DX_LoTW_Status[LoTW_DXCallIndex]);

                    if (e.X > 175 && e.X < 260) // the callsign of the DX station (width)
                    {
                        if (ToolTipNumber != DXt_LoTW_Status[LoTW_DXCallIndex])
                        {

                            ToolTipNumber = DXt_LoTW_Status[LoTW_DXCallIndex]; // .232 to remove flicker

                            switch (DXt_LoTW_Status[LoTW_DXCallIndex])
                            {

                                case 0:
                                    // //  this.toolTip1.SetToolTip(this.textBox1, "Hit F1 (mouse over LoTW button) for more HELP.\r\n" +
                                    //   "LoTW Feature DUP, DXCC, and US States check(see LoTW button for setup):\r\n"  );


                                    break;

                                case 1: // 1 Green: You have this DX Station confirmed on this Band (dont need this Dx Station)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have this DX Call Sign confirmed on this Band.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");

                                    break;

                                case 2: // 2 LightGreen: You have this DX Station confirmed on some other Band 
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have this DX Call Sign confirmed on some other Band.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");

                                    break;

                                case 3: // 3 Yellow: You have this DX station confirmed on some other band, and some other station already confirmed on this band (dont need this DX Station)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have this DX Call Sign confirmed on some other band, and the same DX Entity confirmed on this band.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");

                                    break;

                                case 4: //4 Orange: You have this DXCC country confirmed on this Band (you dont need this DX Station
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have this DXCC Entity or State confirmed on this Band.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");

                                    break;

                                case 5: //5 LightPurple: You have this DXCC country CONFIRMED on some other Band (you WANT this DX Station)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have this DXCC Entity confirmed on some other Band but not this Band.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");

                                    break;

                                case 6: //6 Purple: You WANT this DXCC country (you WANT this DX Station)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You WANT this DXCC Entity (you WANT this DX Call Sign).\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");
                                    break;

                                case 7: //7 DeepPink: You have worked this DX Station on this Band (But they have not  confirmed)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have worked this DX Call Sign on this Band (But they have NOT Confirmed LoTW).\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");
                                    break;

                                case 8: //8 Pink: You have worked DX Station on some other Band (But they have not confirmed)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have worked DX Call Sign on some other Band (But they have NOT Confirmed LoTW).\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");
                                    break;

                                case 9: //9 LightBlue: You have this US State confirmed on some other band (you WANT this DX Station)
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ": You have this US State Confirmed on some other band, but NOT this Band.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");
                                    break;
                                case 10: //10 Blue: You WANT this US State (You Want this DX Station).
                                    this.toolTip1.SetToolTip(this.textBox1, "DX Call: " + LoTW_DXCall + ":  You WANT this US State (You Want this DX Call Sign).\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");
                                    break;
                                case 11: //11 Gray: Beacon
                                    this.toolTip1.SetToolTip(this.textBox1, "Spotted Beacon: " + LoTW_DXCall + ":  Beacon.\r\n" + "" +
                                        "Right Click to lookup up on QRZ.com");
                                    break;
                                default:

                                    break;

                            } // Switch
                        } // dont repeat a tooltip if you already are showing it.

                    } //  if (e.X > 165 && e.X < 250)
                    else // outside the width of the DX station callsign
                    {
                            if (ToolTipNumber != 499)
                            {
                                this.toolTip1.SetToolTip(this.textBox1, "Hit F1 or F2 for more HELP.\r\n" +
                                                         "LoTW Feature DUP, DXCC, and US States check(see LoTW button for setup):\r\n" +
                                                          "LEFT Click= VFOA, WHEEL Click= VFOB, RIGHT Click= QRZ lookup,\r\n");
                                ToolTipNumber = 499;
                            }
                     
                    }

                } //  if (DX_Index > iii)
               

            } // not actively processing a new spot



        } // textBox1_MouseMove

       

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
         //   textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up on a right click
            pause = true;
            button1.Text = "Paused";

        } // textBox1_MouseHover

        private void button1_TextChanged(object sender, EventArgs e) //.232
        {
            if (button1.Text == "Pause")
            {
                button1.BackColor = SystemColors.ButtonFace;
            }
            else
            {
                button1.BackColor = Color.Yellow;

            }
        }

        private void callBox_Click(object sender, EventArgs e) //.232
        {
            if (txtLoTWpass.Visible )
            {
                txtLoTWpass.Visible = false;
         
            }
        }

        private void SpotControl_MouseLeave(object sender, EventArgs e)
        {
            ToolTipNumber = 0;
            pause = false;
            button1.Text = "Pause";
            processTCPMessage();

        }

        private void SpotControl_Deactivate(object sender, EventArgs e)
        {
            Debug.WriteLine("Spot screen visible changed");
            ToolTipNumber = 0;
            pause = false;
            button1.Text = "Pause";
            processTCPMessage();
        }

        private void chkDLayerON_CheckedChanged(object sender, EventArgs e) //.234
        {
            Darken();
        }

        // turn off these when display F or D layer maps .239
        bool GrayLineLast = false; 

        bool MapJustBandLast = false; 
        bool MapJustPanLast = false;
        bool MapBeamLast = false;

        bool MemoriesPanLast = false; 
        bool VOACAPLast = false; 
        bool BandTextLast = false; 
        bool MapCallLast = false;
        bool MapCountryLast = false;

        bool GridBoxLast = false; // setup.gridboxTS (grid lines ON=false)
      


        public void chkFLayerON_CheckedChanged(object sender, EventArgs e) //.235
        {

            if (SP5_Active == 0)  // .239 if map was off when you turned on f-layer
                btnTrack_Click(this, EventArgs.Empty); // turn on Map first


             if (chkFLayerON.Checked && chkDLayerON.Checked) chkDLayerON.Checked = false;
            chkSUN_CheckedChanged(this, EventArgs.Empty);

            if (chkFLayerON.Checked == true) //.239
            {
                GrayLineLast = chkGrayLine.Checked;   // turn off Grayline on the F-layer
                chkGrayLine.Checked = false;

                MapJustBandLast = chkMapBand.Checked;  // turn off spots on F-layer
                chkMapBand.Checked = false;

                MapJustPanLast = chkBoxPan.Checked;
                chkBoxPan.Checked = false;

                MapBeamLast = chkBoxBeam.Checked;
                chkBoxBeam.Checked = false;

                MemoriesPanLast = chkBoxMem.Checked; // Turn off memories on F-layer
                chkBoxMem.Checked = false; 

                VOACAPLast = checkBoxMUF.Checked;  // turn off VOACAP on F-Layer
                checkBoxMUF.Checked = false;
              
                BandTextLast = chkBoxBandText.Checked; 
                chkBoxBandText.Checked = false;

                MapCallLast = chkMapCall.Checked;
                chkMapCall.Checked = false;

                MapCountryLast = chkMapCountry.Checked;
                chkMapCountry.Checked = false;

                if (console.setupForm != null)
                {
                    if (Display.GridOff == 0)
                    {
                        GridBoxLast = !console.setupForm.gridBoxTS.Checked; // if GRID ON then = false so set GridBoxLast = true
                        console.setupForm.gridBoxTS.Checked = true; // true = Grid OFF
                    }

                }


            }
            else  // turn them back on if they were on before the F-layer map was turned on
            {
                if (GrayLineLast)
                {
                    GrayLineLast = false;
                    chkGrayLine.Checked = true;
                }
               
                if (MapJustBandLast)
                {
                    MapJustBandLast = false;
                    chkMapBand.Checked = true;
                }

                if (MapJustPanLast)
                {
                    MapJustPanLast = false;
                    chkBoxPan.Checked = true;
                }

                if (MapBeamLast)
                {
                    MapBeamLast = false;
                    chkBoxBeam.Checked = true;
                }

                if (MemoriesPanLast)
                {
                    MemoriesPanLast = false;
                    chkBoxMem.Checked = true;
                }

                if (VOACAPLast)
                {
                    VOACAPLast = false;
                    checkBoxMUF.Checked = true;
                }

                if (BandTextLast)
                {
                    BandTextLast = false;
                    chkBoxBandText.Checked = true;
                }

                if (MapCallLast)
                {
                    MapCallLast = false;
                    chkMapCall.Checked = true;
                }

                if (MapCountryLast)
                {
                    MapCountryLast = false;
                    chkMapCountry.Checked = true;
                }

                if (console.setupForm != null)
                {
                    if (Display.GridOff == 0)
                    {
                        if (GridBoxLast) // grid was on before
                        {
                            GridBoxLast = false;
                            console.setupForm.gridBoxTS.Checked = false; // true = Grid OFF
                        }
                    }

                }

            } //  if (chkFLayerON.Checked == true) //.239


        } // chkFLayerON_CheckedChanged

        public void chkDLayerON_CheckedChanged_1(object sender, EventArgs e)
        {

            if (SP5_Active == 0)  // .239 if map was off when you turned on d-layer
                btnTrack_Click(this, EventArgs.Empty); // turn on Map first


            if (chkFLayerON.Checked && chkDLayerON.Checked) chkFLayerON.Checked = false;
            chkSUN_CheckedChanged(this, EventArgs.Empty);


            if (chkDLayerON.Checked == true) //.239
            {
                GrayLineLast = chkGrayLine.Checked;   // turn off Grayline on the d-layer
                chkGrayLine.Checked = false;

                MapJustBandLast = chkMapBand.Checked;  // turn off spots on d-layer
                chkMapBand.Checked = false;
              
                MapJustPanLast = chkBoxPan.Checked;
                chkBoxPan.Checked = false;

                MapBeamLast = chkBoxBeam.Checked;
                chkBoxBeam.Checked = false;

                MemoriesPanLast = chkBoxMem.Checked; // Turn off memories on d-layer
                chkBoxMem.Checked = false;

                VOACAPLast = checkBoxMUF.Checked;  // turn off VOACAP on d
                                                   // -Layer
                checkBoxMUF.Checked = false;
              
                BandTextLast = chkBoxBandText.Checked;
                chkBoxBandText.Checked = false;

                MapCallLast = chkMapCall.Checked;
                chkMapCall.Checked = false;

                MapCountryLast = chkMapCountry.Checked;
                chkMapCountry.Checked = false;

                if (console.setupForm != null)
                {
                    if (Display.GridOff == 0)
                    {
                        GridBoxLast = !console.setupForm.gridBoxTS.Checked; // if GRID ON then = false so set GridBoxLast = true
                        console.setupForm.gridBoxTS.Checked = true; // true = Grid OFF
                    }

                }


            }
            else  // turn them back on if they were on before the F-layer map was turned on
            {
                if (GrayLineLast)
                {
                    GrayLineLast = false;
                    chkGrayLine.Checked = true;
                }

                if (MapJustBandLast)
                {
                    MapJustBandLast = false;
                    chkMapBand.Checked = true;
                }

                if (MapJustPanLast)
                {
                    MapJustPanLast = false;
                    chkBoxPan.Checked = true;
                }

                if (MapBeamLast)
                {
                    MapBeamLast = false;
                    chkBoxBeam.Checked = true;
                }

                if (MemoriesPanLast)
                {
                    MemoriesPanLast = false;
                    chkBoxMem.Checked = true;
                }

                if (VOACAPLast)
                {
                    VOACAPLast = false;
                    checkBoxMUF.Checked = true;
                }

                if (BandTextLast)
                {
                    BandTextLast = false;
                    chkBoxBandText.Checked = true;
                }

                if (MapCallLast)
                {
                    MapCallLast = false;
                    chkMapCall.Checked = true;
                }

                if (MapCountryLast)
                {
                    MapCountryLast = false;
                    chkMapCountry.Checked = true;
                }


                if (console.setupForm != null)
                {
                    if (Display.GridOff == 0)
                    {
                        if (GridBoxLast) // grid was on before
                        {
                            GridBoxLast = false;
                            console.setupForm.gridBoxTS.Checked = false; // true = Grid OFF
                        }
                    }

                }

            } //  if (chkFLayerON.Checked == true) //.239

        } // chkDLayerON_CheckedChanged_1(object sender, EventArgs e)


    } //SPOTCONTROL


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
