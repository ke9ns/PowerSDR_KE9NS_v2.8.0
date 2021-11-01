//=================================================================
// cwx.cs
//=================================================================
// CWX - new version of the old keyer memory and keyboard stuff
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
//
//         CWX written by Richard Allen, W5SXD
//    with various hooks from Eric Wachsman, KE5DTO, 
//          and Herr Doktor Bob McGwier, N4HY
//            November 2005 - February 2006
//
//=================================================================

#define SAVERESTORE
//#define CWX_DEBUG (Note: Please do not put all Debug.Writeline()under this. Leave them commented off.)

using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using FlexCW;

namespace PowerSDR
{
    /// <summary>
    ///  CWX is the cw memory and keyboard handler.
    /// </summary>
    public partial class CWX : System.Windows.Forms.Form
    {
        #region define element codes and constants
        private const byte EL_UNDERFLOW = 0x80; // underflow flag
        private const byte EL_PTT = 0x10;       // extra ptt delay command
        private const byte EL_PAUSE = 0x08;     // pause flag
        private const byte EL_END = 0x4;        // end flag
        private const byte EL_KEYUP = 0x2;      // key up flag
        private const byte EL_KEYDOWN = 0x3;    // key down

        private const int NKEYS = 120;          // width of the keyboard buffers
        private const int NKPL = 60;            // # keys per line

        private const char EMPTY_CODE = '_';    // empty display loc
        #endregion

        #region Variable Declarations
       
        private Console console;

        public static Mutex keydisplay = new Mutex();   // around the key display

        private int cwxwpm;                     // engine speed
        private uint[] mbits = new uint[64];    // the Morse element maps
        private string[] a2m2 = new string[64]; // in ASCII order from 32-95

        private bool quit, kquit;   // shutdown flags    
                                    //ke9ns kquit is set by ESC key
        private int pause;          // # cycles left to pause

        public static Mutex cwfifo2 = new Mutex();  // around the key fifo
        private byte[] fifo2 = new byte[32768];     // the key fifo
        private int infifo2;        // # entries in the fifo
        private int pin2;           // fifo input pointer
        private int pout2;          // fifo output pointer

        public static Mutex cwfifo = new Mutex();   // around the element fifo     ke9ns syncronized resource
        private byte[] elfifo = new byte[32768];    // the code element fifo
        private int infifo;         // # entries in the fifo
        private int pin;            // fifo input pointer
        private int pout;           // fifo output pointer

        private int tel;            // time of one element in ms
        private int ttx;            // # cycles left 'til ptt drops
        private int ttdel;          // tx timeout; keep ptt up this long after key up
        private int tpause;         // pause time in ms
        private string tqq;         // string currently being sent
        private bool altkey;        // true if alt key is pressed

        private int kkk;            // a counter for key up event handler
        private bool keying;        // key button keying radio
        private bool ptt;           // ptt is active
        private int newptt;         // have a new setting of ptt active timing it down
        private int pttdelay;       // delay from PTT to key down
        private bool pause_checked; // pause chekbox is checked
        private bool stopThreads;   // tell threads to shut down

        public bool stopPoll = true;      // ke9ns add  false = stop poll cw key

        // define the position and size of the keyboard area
        private int kylx = 12, kyty = 180;              // ulc of key area
        private int kyysz = 82, kyxsz = 665;            // extents
        private char[] kbufold = new char[NKEYS];       // sent keys
        private char[] kbufnew = new char[NKEYS];       // unsent keys

        private ASCIIEncoding AE = new ASCIIEncoding();

        #endregion

        #region Win32 Multimedia Timer Functions

        // Represents the method that is called by Windows when a timer event occurs.
        private delegate void TimeProc(int id, int msg, int user, int param1, int param2);

        /// Specifies constants for multimedia timer event types.

        public enum TimerMode
        {
            OneShot,    // Timer event occurs once.
            Periodic    // Timer event occurs periodically.
        };

        /// Represents information about the timer's capabilities.
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

        // Timer mode.
        //private volatile TimerMode mode;

        // Period between timer events in milliseconds.
        //private volatile int period;

        // Timer resolution in milliseconds.
        //private volatile int resolution;

        // Indicates whether or not the timer is running.
        //private bool running;

        // Called by Windows when a timer periodic event occurs.
        //	private TimeProc timeProcPeriodic;

        // Called by Windows when a timer one shot event occurs.
        //	private TimeProc timeProcOneShot;

        // Indicates whether or not the timer has been disposed.
        //private volatile bool disposed = false;

        // For implementing IComponent.
        //	private ISite site = null;

        // Multimedia timer capabilities.
        //private static TimerCaps caps;
        // Called by Windows when a timer periodic event occurs.
        private TimeProc timeProcPeriodic;

        private void setup_timer()
        {
            tel = wpmrate();
#if (CWX_DEBUG)
			Debug.WriteLine(tel+" ms");
#endif
            if (timerID != 0)
            {
                timeKillEvent(timerID);
            }

            timerID = timeSetEvent(tel, 1, timeProcPeriodic, 0, (int)TimerMode.Periodic);

            if (timerID == 0)
            {
                Debug.Fail("Timer creation failed.");
            }
        }
        #endregion

        #region radio interface and fifo functions

        private bool setptt_memory = false;
        private void setptt(bool state)
        {
            //console.Keyer.MemoryPTT = state;
            if (setptt_memory != state)
            {
                CWPTTItem item = new CWPTTItem(state, CWSensorItem.GetCurrentTime());
                CWKeyer.PTTEnqueue(item);

                Debug.WriteLine("1setptt = setptt_memory> " + state);

                ptt = state;
                if (state)
                {
                    if ((console.CWP == true) && (console.RX1DSPMode != DSPMode.CWL) && (console.RX1DSPMode != DSPMode.CWU))
                    {
                        Debug.WriteLine("KEY RADIO");

                        console.MOX = true;
                    }
                    pttLed.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    if ((console.CWP == true) && (console.RX1DSPMode != DSPMode.CWL) && (console.RX1DSPMode != DSPMode.CWU))
                    {
                        Debug.WriteLine("KEY RADIO");
                        console.MOX = false;
                    }
                    pttLed.BackColor = System.Drawing.Color.Black;
                }

                setptt_memory = state;
            }
            //			if (newptt) Thread.Sleep(200);
        } // setptt

        //=====================================================================================

        /*
                FlexCW.CWKeyer:
               ------------------------------------------------------ 
                These are all the settings for FlexCW.dll

                public static double AudioLatency { get; set; }     
                public static double BreakInDelay { get; set; }
                public static IambicMode CurrentIambicMode { get; set; }
                 public enum IambicMode
                {
                    ModeA = 0,
                    ModeB = 1,
                    ModeBStrict = 2,
                    Bug = 3
                }

                public static int WPM { get; set; }
                public static bool BreakIn { get; set; }
                public static bool AutoCharSpace { get; set; }
                public static bool Iambic { get; set; }
                public static double HWKeyDownDelay { get; set; }
                public static double MinQSKTime { get; set; }
                public static int Weight { get; set; }

                ----------------------------------------------------
                These are the controls for FlexCW.dll

                public static void Advance(double time);              // NOT USED
                public static CWMuteItem MuteDequeue();               // NOT USED
                public static void MuteEnqueue(CWMuteItem item);      // NOT USED
                public static int MuteQueueCount();                   // NOT USED
                public static CWMuteItem MuteQueuePeek();             // NOT USED
                public static CWPTTItem PTTDequeue();                 // NOT USED
                public static void PTTEnqueue(CWPTTItem item);        //  CWPTTItem item = new CWPTTItem(state, CWSensorItem.GetCurrentTime());     CWKeyer.PTTEnqueue(item); // something to do with PTT of the radio based on breakin 
           
                public static int PTTQueueCount();                    // NOT USED
                public static CWPTTItem PTTQueuePeek();               // NOT USED
                public static void PTTQueuePrint();                   // NOT USED
                public static void Reset();                           // reset back to start
                public static void SensorEnqueue(CWSensorItem item);  // add to queue        CWKeyer.SensorEnqueue(new CWSensorItem(CWSensorItem.InputType.Dash, false)); // adds DOT or DASH to queue
                public static void SensorQueuePrint();                // *NOT USED

                public static CWToneItem ToneDequeue();               // NOT USED


                public static int ToneQueueCount();                   // NOT USED
                public static void ToneQueuePrint();                  // *NOT USED

            ==========================================================================================
             public CWSynth();

        public static int Pitch { get; set; }
        public static int SampleRate { get; set; }
        public static double RampTime { get; set; }
        public static double ImageGain { get; set; }
        public static double ImagePhase { get; set; }

        public static void Advance(float* buf_l, float* buf_r, int num_samples, double current_time);  // used in audio.cs routine 


        ==========================================================================================
         
        public CWPTT();

        public static event MoxCallback MoxChanged;
        public static event MuteCallback MuteChanged;

        public static void Init();
        public static void Start();                       // used
        public static void Stop();

        public delegate void MoxCallback(bool val);
        public delegate void MuteCallback(bool val);


   ==========================================================================================
     
        public CWSensorItem(InputType _type, bool _state);

        public double Time { get; }
        public InputType Type { get; }

        public enum InputType
        {
            Dot = 0,
            Dash = 1,
            StraightKey = 2
        }


        public bool State { get; }

        public static double GetCurrentTime();
        public static void Init();                // USED
        public override string ToString();

        

       ==========================================================================================
        public CWToneItem(bool _state, double _time);

        public bool State { get; }
        public double Time { get; }

        public override string ToString();

     
        */

        //   static bool test2= false;
        //   static double test3 = 0.0;
        //   CWToneItem test1 = new CWToneItem(test2, test3);


        //=====================================================================================

        private bool setkey_memory = false;

        private void setkey(bool state)                 // ke9ns   This is the CW key signal back to the flex radio itself (letter E set repeatedly at 60wpm = 20msec per element = 20msec ON, and 54msec OFF)
        {
            //  console.Keyer.MemoryKey = state;


            if (setkey_memory != state)                                          // only allow this to happen 1 time if state stays the same (once to turn ON, once to turn OFF)
            {


                if ((console.CWP == true) && (console.RX1DSPMode != DSPMode.CWL) && (console.RX1DSPMode != DSPMode.CWU))
                {
                    //    Debug.WriteLine("KEYDOT");
                    console.keydot = state;
                }

                //   if  (state == true) Debug.WriteLine("ON: " + T1.ElapsedMilliseconds);
                //  else Debug.WriteLine("OFF: " + T1.ElapsedMilliseconds);

                //   T1.Restart();

                // ke9ns this CWSensorItem and CWKeyer is for the TX out and MON tone 
                CWSensorItem item = new CWSensorItem(CWSensorItem.InputType.StraightKey, state); // ke9ns state = TRUE / False  = ON / OFF pulses to be processes by FlexCW.dll
                CWKeyer.SensorEnqueue(item); // ke9ns send on/off CW pulses to FlexCW.DLL code and it will process the proper speed & weight, etc. etc. and output to radio itself


                if (state) keyLed.BackColor = System.Drawing.Color.Yellow;
                else keyLed.BackColor = System.Drawing.Color.Black;

                setkey_memory = state;
            }
        } //  setkey(bool state)   



        private void quitshut()
        {
            Debug.WriteLine("quitshut 1");
            clear_fifo();
            clear_fifo2();
            setptt(false);
            setkey(false);
            ttx = 0; pause = 0; newptt = 0;
            keying = false;

            Debug.WriteLine("quitshut 2");

        }


        private void clear_fifo()
        {
            cwfifo.WaitOne();
            infifo = 0;
            pin = 0;
            pout = 0;
            cwfifo.ReleaseMutex();
        }


        private void push_fifo(byte data)
        {
            cwfifo.WaitOne();
            elfifo.SetValue(data, pin);
            pin++;
            if (pin >= elfifo.Length) pin = 0;
            infifo++;
            cwfifo.ReleaseMutex();

            //			Debug.WriteLine("push " + data);
        }



        private byte pop_fifo()
        {
            byte data;

            if (infifo < 1) data = EL_UNDERFLOW;
            else
            {
                cwfifo.WaitOne();
                data = (byte)elfifo.GetValue(pout);
                pout++;
                if (pout >= elfifo.Length) pout = 0;
                infifo--;
                cwfifo.ReleaseMutex();
            }
#if (CWX_DEBUG)
			Debug.WriteLine("pop " + data);
#endif

            return data;
        }

        private void clear_fifo2()
        {
            cwfifo2.WaitOne();
            infifo2 = 0;
            pin2 = 0;
            pout2 = 0;
            cwfifo2.ReleaseMutex();
        }
        private void push_fifo2(byte data)
        {
            cwfifo2.WaitOne();
            fifo2.SetValue(data, pin2);
            pin2++;
            if (pin2 >= fifo2.Length) pin2 = 0;
            infifo2++;
            cwfifo2.ReleaseMutex();
#if (CWX_DEBUG)
			Debug.WriteLine("push " + data);
#endif
        }
        private byte pop_fifo2()
        {
            byte data;

            if (infifo2 < 1) data = EL_UNDERFLOW;
            else
            {
                cwfifo2.WaitOne();
                data = (byte)fifo2.GetValue(pout2);
                pout2++;
                if (pout2 >= fifo2.Length) pout2 = 0;
                infifo2--;
                cwfifo2.ReleaseMutex();
            }
#if (CWX_DEBUG)
			Debug.WriteLine("pop " + data);
#endif

            return data;
        }
        #endregion

        #region Morse definition, table builders, and help display
        private int wpmrate()   // Tel in ms from wpm (based on PARIS method
        {
            return (1200 / cwxwpm);   // 20mSec = 60wpm
        }
        private void help()
        {
            string t;

            t = "                  Memory and Keyboard Keyer Notes\n";
            t += "\n";
            t += "Radio must be running in a valid cw mode and frequency.\n";
            t += "Speed is for this form only (may change later).\n";
            t += "PTT Delay (ms) is time from PTT to first key down.\n";
            t += "Drop Delay (ms) is time for radio to drop out of transmit when keying stops.\n";
            t += "Drop Delay cannot be set less than PTT Delay * 1.5 and should\n";
            t += "be kept high enough so that PTT does not drop out between words.\n";
            t += "Note that weight settings on Setup Form do not affect CWX at this time.\n";
            t += "\n";
            t += "Macros: Transmit Marcros 1-9 by keyboard F1-F9 or Clickin on macro1-9 buttons\n";
            t += "\n";
            t += "Messages may be edited while sending but take effect after restarting the message.\n";
            t += "\n";
            t += "\n";
            t += "In the keyer memories special characters are:\n\n";
            t += "    # sends a long dash a bit longer than a zero\n";
            t += "    $ send a long space as above\n";
            t += "    \" at the message end means loop it continuously\n";
            t += "      and Repeat Delay sets time between repeats in seconds\n";
            t += "\n";

            t += "    + is AR        ( is KN         * is SK\n";
            t += "    ! is SN        = is BT         \\ is BK\n\n";
            t += "The other specials can be changed in morsedef.txt.\n";
            t += "They are & ' ) : ; < > [  ] and ^\n";
            t += "All others without regular Morse defs send a space.\n";
            t += "\nPress button in lower right corner to show keyboard and more memories.\n\n";
            t += "Keyboard button must have the focus for characters\n";
            t += "to be entered in the keyboard buffer. Cyan indicator on.\n\n";
            t += "Keyboard sending will pause if the checkbox is checked.\n";
            t += "F1 flips the pause state.  F2 clears the keyboard.\n";
            t += "Alt 1 thru Alt 9 will load memories 1 to 9 into keyboard buffer.\n";
            t += "Right click on a message button will also load it into the keyboard\n";
            t += "\n";
            t += "Esc stops any output and clears the keyboard buffer.\n";
            t += "\n";
            t += "\n";
            t += "    << Sugar Land, Texas 2006-02-16 - Richard Allen, W5SXD >>\n";

            MessageBox.Show(new Form { TopMost = true }, t, "  CWX Notes ...");
        }

        private void notesButton_Click(object sender, System.EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(help));
            t.Name = "help thread";
            t.IsBackground = true;                  // if app closes, kill this thread
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }



        private void build_mbits2()
        {
            uint els;
            uint nel;
            uint mask;
            int ndd;
            string s, st;

            for (int i = 0; i < 64; i++)    // for each code
            {
                nel = 0; els = 0; mask = 0x80000000;
                /*
				012345678901234
				64|@|.--.-.   |
				*/

                st = (string)a2m2.GetValue(i);
                s = st.Substring(5, 9);

                ndd = s.Length;
#if (CWX_DEBUG)
				Debug.WriteLine(s);
#endif

                for (int k = 0; k < ndd; k++)
                {
                    if (String.CompareOrdinal(s, k, "-", 0, 1) == 0)
                    {
                        nel += 4;
                        els |= mask; mask >>= 1;
                        els |= mask; mask >>= 1;
                        els |= mask; mask >>= 1;
                        mask >>= 1;
                    }
                    else if (String.CompareOrdinal(s, k, ".", 0, 1) == 0)
                    {
                        nel += 2;
                        els |= mask; mask >>= 1;
                        mask >>= 1;
                    }
                    else continue;

                }
                // a space will have a 4 element count + 1 element space + 2 letter spaces
                // making a total of 7 elements in a word space

                if (i == 0) nel = 4;    // value for ' ' character
                else nel += 2;          // add letter space

                els &= 0xffffffe0;
                els += nel & 0x1f;
#if (CWX_DEBUG)
				Debug.WriteLine(ndd+": "+s+" "+sbin(els));
#endif
                mbits.SetValue(els, i);
            }
        }

        private string sfile = "morsedef.txt";

        private void load_alpha()
        {

            if (!File.Exists(console.AppDataPath + sfile))    // create default morsedef.txt   "\\ " +
            {
#if(CWX_DEBUG)
				MessageBox.Show(sfile+" not found, creating ...");
#endif
                using (StreamWriter sw = new StreamWriter(console.AppDataPath + sfile))  // "\\" +
                {
                    sw.WriteLine("32| |*        | space     ");
                    sw.WriteLine("33|!|...-.    | [SN]      ");
                    sw.WriteLine("34|\"|*        | loop      ");
                    sw.WriteLine("35|#|*        | long dash ");
                    sw.WriteLine("36|$|*        | long space");
                    sw.WriteLine("37|%|.-...    | [AS]      ");
                    sw.WriteLine("38|&|.........| 0123456789");
                    sw.WriteLine("39|'|         |           ");
                    sw.WriteLine("40|(|-.--.    | [KN]      ");
                    sw.WriteLine("41|)|         |           ");
                    sw.WriteLine("42|*|...-.-   | [SK]      ");
                    sw.WriteLine("43|+|.-.-.    | [AR]      ");
                    sw.WriteLine("44|,|--..--   |           ");
                    sw.WriteLine("45|-|-....-   |           ");
                    sw.WriteLine("46|.|.-.-.-   |           ");
                    sw.WriteLine("47|/|-..-.    |           ");
                    sw.WriteLine("48|0|-----    |           ");
                    sw.WriteLine("49|1|.----    |           ");
                    sw.WriteLine("50|2|..---    |           ");
                    sw.WriteLine("51|3|...--    |           ");
                    sw.WriteLine("52|4|....-    |           ");
                    sw.WriteLine("53|5|.....    |           ");
                    sw.WriteLine("54|6|-....    |           ");
                    sw.WriteLine("55|7|--...    |           ");
                    sw.WriteLine("56|8|---..    |           ");
                    sw.WriteLine("57|9|----.    |           ");
                    sw.WriteLine("58|:|         |           ");
                    sw.WriteLine("59|;|         |           ");
                    sw.WriteLine("60|<|         |           ");
                    sw.WriteLine("61|=|-...-    | [BT]      ");
                    sw.WriteLine("62|>|         |           ");
                    sw.WriteLine("63|?|..--..   |           ");
                    sw.WriteLine("64|@|.--.-.   |           ");
                    sw.WriteLine("65|A|.-       |           ");
                    sw.WriteLine("66|B|-...     |           ");
                    sw.WriteLine("67|C|-.-.     |           ");
                    sw.WriteLine("68|D|-..      |           ");
                    sw.WriteLine("69|E|.        |           ");
                    sw.WriteLine("70|F|..-.     |           ");
                    sw.WriteLine("71|G|--.      |           ");
                    sw.WriteLine("72|H|....     |           ");
                    sw.WriteLine("73|I|..       |           ");
                    sw.WriteLine("74|J|.---     |           ");
                    sw.WriteLine("75|K|-.-      |           ");
                    sw.WriteLine("76|L|.-..     |           ");
                    sw.WriteLine("77|M|--       |           ");
                    sw.WriteLine("78|N|-.       |           ");
                    sw.WriteLine("79|O|---      |           ");
                    sw.WriteLine("80|P|.--.     |           ");
                    sw.WriteLine("81|Q|--.-     |           ");
                    sw.WriteLine("82|R|.-.      |           ");
                    sw.WriteLine("83|S|...      |           ");
                    sw.WriteLine("84|T|-        |           ");
                    sw.WriteLine("85|U|..-      |           ");
                    sw.WriteLine("86|V|...-     |           ");
                    sw.WriteLine("87|W|.--      |           ");
                    sw.WriteLine("88|X|-..-     |           ");
                    sw.WriteLine("89|Y|-.--     |           ");
                    sw.WriteLine("90|Z|--..     |           ");
                    sw.WriteLine("91|[|         |           ");
                    sw.WriteLine("92|\\|-...-.-  | [BK]      ");
                    sw.WriteLine("93|]|         |           ");
                    sw.WriteLine("94|^|         |           ");
                    sw.WriteLine("95|_|*        | reserved  ");
                }
            }
            //MessageBox.Show(new Form { TopMost = true }, "reading ",sfile);
            using (StreamReader sr = new StreamReader(console.AppDataPath + sfile))  // "\\" +
            {
                String line;
                String t;
                int nl = 0;

                cbMorse.Items.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    cbMorse.Items.Add(line);
                    a2m2.SetValue(line, nl);
                    nl++;
                }
                if (nl != 64)
                {
                    t = sfile + " has incorrect length and may be corrupt\n";
                    t += "delete it and let it be rebuilt ...";
                    MessageBox.Show(new Form { TopMost = true }, t);
                }
                cbMorse.SelectedIndex = 0;
            }
        }

        #endregion

        #region CAT Interface
        // Added by Bob Tracy July, 2007 to implement a remote interface to send
        // a CW message via the CAT KY command.

        PowerSDR.RingBufferByte rb = new RingBufferByte(2048);

        public string RemoteMessage(byte[] msg)
        {
            rb.Write(msg, msg.Length);
            return "";
        }

        public string RemoteMessage(char msg)
        {
            loadchar(msg);
            return "";
        }

        private volatile bool stopSending;


        private void SendBufferMessage()                //CAT Read Thread
        {
            while (true)									//do forever
            {
                Thread.Sleep(10);
                if (!stopSending)
                {
                    byte[] buffer;							//holds the bytes read from the ringbuffer
                    char chr2send;

                    if (rb.ReadSpace() > 0)					//if we have data in the ringbuffer
                    {
                        buffer = new byte[1];				//an array to hold one byte of rb data
                        rb.Read(buffer, buffer.Length);		//read the ringbuffer
                        chr2send = (char)buffer[0];
                        loadchar(chr2send);
                        while (infifo > 2)					//number of elements left in the element fifo
                        {
                            Thread.Sleep(2);				//wait for the element fifo to catch up
                        }
                    }
                    Thread.Sleep(2);
                }
            }
        }

        public int WPM
        {
            get { return cwxwpm; }
            set { udWPM.Value = value; }
        }

        public int Characters2Send
        {
            get { return infifo; }
        }

        public void CWXStop()
        {
            stopSending = true;
            rb.Reset();
            stopSending = false;
        }

        public int StartQueue
        {
            set
            {
                queue_start(value);
            }
        }

        #endregion CAT Interface

        #region startup/shutdown stuff
        public CWX(Console c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            console = c;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            txtdummy1.Hide();
            clear_keys();

            txt1.Text = "### test de w5sxd/b el29ep.$$\"";
            txt2.Text = "cq cq test w5sxd test";
            txt3.Text = "5nn stx";
            txt4.Text = "k5sdr de w5sxd (";
            txt5.Text = "cq cq cq de w5sxd w5sxd w5sxd +k";
            txt6.Text = "The quick brown fox jumped over the lazy dog. 0123456789 ";
            txt7.Text = "?";
            txt8.Text = "agn";
            txt9.Text = "n6vs";

            //RestoreSettings();
            Common.RestoreForm(this, "CWX", true);

            //		cwxwpm = 20;
            //		udWPM.Value = cwxwpm;
            cwxwpm = (int)udWPM.Value;

            tpause = (int)udDelay.Value * 1000;
            if (tpause < 1) tpause = tel;
            ttdel = (int)udDrop.Value;
            pttdelay = (int)udPtt.Value;
            //udDrop.Minimum = pttdelay + pttdelay/2;



            //			RestoreSettings();

#if (CWX_DEBUG)
			Debug.WriteLine("CWX entry");
#endif

            timeProcPeriodic = new TimeProc(TimerPeriodicEventCallback);
            setup_timer();
            load_alpha();
            // build the mbits array from a2m2
            build_mbits2();

            stopThreads = false;

            Thread keyFifoThread = new Thread(new ThreadStart(keyboardFifo));
            keyFifoThread.Name = "keyboard fifo pop thread";
            keyFifoThread.IsBackground = true;                  // if app closes, kill this thread
            keyFifoThread.Priority = ThreadPriority.Normal;
            keyFifoThread.Start();

            Thread keyDisplayThread = new Thread(new ThreadStart(keyboardDisplay));
            keyDisplayThread.Name = "keyboard edit box handler thread";
            keyDisplayThread.IsBackground = true;                   // if app closes, kill this thread
            keyDisplayThread.Priority = ThreadPriority.Normal;
            keyDisplayThread.Start();

            Thread CATReadThread = new Thread(new ThreadStart(SendBufferMessage));
            CATReadThread.Name = "CAT Read Thread";
            CATReadThread.IsBackground = true;
            CATReadThread.Priority = ThreadPriority.Highest;
            CATReadThread.Start();

            Thread pollcw = new Thread(new ThreadStart(PollKey));
            pollcw.Name = "Poll Key Thread";
            pollcw.Priority = ThreadPriority.Normal;
            pollcw.IsBackground = true;
            pollcw.Start();

            //			ttdel = 50;
        } // CWX

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
#if (CWX_DEBUG)
			Debug.WriteLine("dispose cwx");
#endif
            timeKillEvent(timerID);

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

       
        #region event handlers and callbacks

        private void expandButton_Click(object sender, System.EventArgs e)
        {
            if (this.Width > 550)
            {
                this.Width = 456;
                this.Height = 176;
                expandButton.Left = 432;
                expandButton.Top = 136;
                toolTip1.SetToolTip(expandButton, "Expand Form");
            }
            else
            {
                this.Width = 704;
                this.Height = 304;
                expandButton.Left = 680;
                expandButton.Top = 264;
                toolTip1.SetToolTip(expandButton, "Compress Form");
            }
        }

        private void keyboardButton_Leave(object sender, System.EventArgs e)
        {
            keyboardButton.ForeColor = System.Drawing.Color.Gray;
            keyboardButton.Text = "Keys Off";
            keyboardLed.BackColor = System.Drawing.Color.Black;

        }

        private void keyboardButton_Enter(object sender, System.EventArgs e)
        {
            keyboardButton.ForeColor = System.Drawing.Color.Black;
            keyboardButton.Text = "KEYS ACTIVE";
            keyboardLed.BackColor = System.Drawing.Color.Cyan;

        }

        // this guy checks for the release of the Alt key
        private void CWX_KeyUp_1(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            kkk++;
            label6.Text = kkk.ToString() + " " +
                e.KeyCode.ToString() + " " +
                e.KeyData.ToString() + " " +
                e.KeyValue.ToString("x");

            if (e.KeyCode.ToString().Equals("Menu")) altkey = false;
        }

        // the Esc, F1, F2, and Alt 1 thru Alt 9 are handled anywhere on the form
        // the Alt key press is detected here and altkey set to true
        private void CWX_KeyDown_1(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            char key = (char)e.KeyValue;

            label5.Text = "KeyDown " + key + " " +
                e.KeyCode.ToString() + " " +
                e.KeyData.ToString() + " " +
                e.KeyValue.ToString("x");

            if (key == 0x7A)            // ke9ns mod: was F1=0x70 so F11=0x7A
            {
                chkPause.Checked = !chkPause.Checked;
            }
            else if (key == 0x7B)       //ke9ns mod: was F2=0x71 so F12=0x7B
            {
                clear_show();
            }
            else if (key == 27)         // Esc
            {
                clear_show();
                quit = true;
                kquit = true;
            }
            else if (key == 0x70) // F1-F9 plays Macros 1-9  .120
            {
                queue_start(1);
            }
            else if (key == 0x71)
            {
                queue_start(2);
            }
            else if (key == 0x72)
            {
                queue_start(3);
            }
            else if (key == 0x73)
            {
                queue_start(4);
            }
            else if (key == 0x74)
            {
                queue_start(5);
            }
            else if (key == 0x75)
            {
                queue_start(6);
            }
            else if (key == 0x76)
            {
                queue_start(7);
            }
            else if (key == 0x77)
            {
                queue_start(8);
            }
            else if (key == 0x78)
            {
                queue_start(9);
            }

            else if (e.KeyCode.ToString().Equals("Menu")) altkey = true;

            if (altkey)
            {   // Alt 1 thru 9 load messages 1-9 into the keyboard buffer
                if (e.KeyCode.ToString().Equals("D1")) msg2keys(1);
                else if (e.KeyCode.ToString().Equals("D2")) msg2keys(2);
                else if (e.KeyCode.ToString().Equals("D3")) msg2keys(3);
                else if (e.KeyCode.ToString().Equals("D4")) msg2keys(4);
                else if (e.KeyCode.ToString().Equals("D5")) msg2keys(5);
                else if (e.KeyCode.ToString().Equals("D6")) msg2keys(6);
                else if (e.KeyCode.ToString().Equals("D7")) msg2keys(7);
                else if (e.KeyCode.ToString().Equals("D8")) msg2keys(8);
                else if (e.KeyCode.ToString().Equals("D9")) msg2keys(9);
            }

        }

        private void keyboardButton_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            process_key(e.KeyChar);
        }

        //==============================================================================================
        // process the 'Key' button which start transmitter with key down
        private void keyButton_Click(object sender, System.EventArgs e) // the 'Key' button
        {
            if (keying)
            {
                Debug.WriteLine("Quit0");
                quitshut();
                return;
            }

            if ((console.CWP == false)) // ke9ns add
            {
                if ((console.RX1DSPMode != DSPMode.CWL) && (console.RX1DSPMode != DSPMode.CWU))
                {
                    MessageBox.Show(new Form { TopMost = true }, "Console is not in CW mode.  Please switch to either CWL or CWU and try again.",
                        "CWX Error: Wrong Mode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            quit = true;
            kquit = true;
            while (quit) Thread.Sleep(10);

            pause = 60000 / tel;

            tqq = " . ";

            setptt(true);

            setkey(true);

            keying = true;

        } // keyButton_Click



        private void CWX_Load(object sender, System.EventArgs e)
        {
#if (CWX_DEBUG)
			Debug.WriteLine("load cwx, queue is " + elfifo.Length);
#endif

            if (console.setupForm != null)
            {
                checkBoxCWD.Checked = console.setupForm.chkCWDisplay.Checked;
            }
        }


        //====================================================================================
        // ke9ns this event is called (event manually added to initialization at top of this code) but now turned off
        private void CWX_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            stopPoll = false; // ke9ns kill cw poll thread

            quitshut();
            Thread.Sleep(100);

            // shut downs
            // savesettings, close threads, kill mmtimer
            stopThreads = true;

            if (timerID != 0)
            {
                timeKillEvent(timerID);     // kill the mmtimer
            }
            Thread.Sleep(200);      // let it all stop

            //SaveSettings();
            Common.SaveForm(this, "CWX");

            Debug.WriteLine("CWX_Closing2");

            // don't do next two lines so we will shut down the form completely


        } //  CWX_Closing




        // Callback method called by the Win32 multimedia timer when a timer
        // periodic event occurs.

        Stopwatch T1 = new Stopwatch();

        private void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
        {

            process_element();
        }

        private void s1_Click(object sender, System.EventArgs e)
        {

            queue_start(1);

        }

        private void s2_Click(object sender, System.EventArgs e)
        {

            queue_start(2);

        }

        private void s3_Click(object sender, System.EventArgs e)
        {


            queue_start(3);

        }

        private void s4_Click(object sender, System.EventArgs e)
        {
            queue_start(4);
        }

        private void s5_Click(object sender, System.EventArgs e)
        {
            queue_start(5);
        }

        private void s6_Click(object sender, System.EventArgs e)
        {
            queue_start(6);
        }

        private void s7_Click(object sender, System.EventArgs e)
        {
            queue_start(7);
        }

        private void s8_Click(object sender, System.EventArgs e)
        {
            queue_start(8);
        }

        private void s9_Click(object sender, System.EventArgs e)
        {
            queue_start(9);
        }

        private void cbMorse_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) editit();
        }


        private void s1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(1);
        }
        private void s2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(2);
        }
        private void s3_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(3);
        }
        private void s4_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(4);
        }
        private void s5_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(5);
        }
        private void s6_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(6);
        }
        private void s7_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(7);
        }
        private void s8_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(8);
        }
        private void s9_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right)) msg2keys(9);
        }

        // stop button clicked
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            clear_show();
            quit = true;
            kquit = true;

        }

        // ke9ns mod  (link back to console cw keyer speed)
        public void udWPM_ValueChanged(object sender, System.EventArgs e)
        {
            cwxwpm = (int)udWPM.Value;

            if (console != null)
            {
                console.ptbCWSpeed.Value = cwxwpm; // ke9ns link the 2 places together
                console.lblCWSpeed.Text = "Speed:  " + console.ptbCWSpeed.Value.ToString() + " WPM";
                CWKeyer.WPM = console.ptbCWSpeed.Value;
            }

            setup_timer();

        } //udWPM_ValueChanged 

        private void udWPM_LostFocus(object sender, EventArgs e)
        {
            udWPM_ValueChanged(sender, e);
        }

        private void udDelay_ValueChanged(object sender, System.EventArgs e)
        {
            tpause = (int)udDelay.Value * 1000;
            if (tpause < 1) tpause = tel;
        }
        private void udDelay_LostFocus(object sender, EventArgs e)
        {
            udDelay_ValueChanged(sender, e);
        }

        private void udDrop_ValueChanged(object sender, System.EventArgs e)
        {
            ttdel = (int)udDrop.Value;
        }
        private void udDrop_LostFocus(object sender, EventArgs e)
        {
            udDrop_ValueChanged(sender, e);
        }

        private void udPtt_ValueChanged(object sender, System.EventArgs e)
        {
            pttdelay = (int)udPtt.Value;
            //udDrop.Minimum = pttdelay + pttdelay/2;
        }
        private void udPtt_LostFocus(object sender, EventArgs e)
        {
            udPtt_ValueChanged(sender, e);
        }

        private void CWX_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            label5.Text = ((int)e.X + " " + (int)e.Y);  // a tool for screen coords
        }

        private void CWX_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            show_keys();        // since this is not a regular control
        }

        private void chkPause_CheckedChanged(object sender, System.EventArgs e)
        {
            pause_checked = chkPause.Checked;
        }

        private void chkAlwaysOnTop_CheckedChanged(object sender, System.EventArgs e)
        {
            /*if(chkAlwaysOnTop.Checked)
			{
				Win32.SetWindowPos(this.Handle.ToInt32(),
					-1, this.Left, this.Top, this.Width, this.Height, 0);
			}
			else
			{
				Win32.SetWindowPos(this.Handle.ToInt32(),
					-2, this.Left, this.Top, this.Width, this.Height, 0);
			}*/
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        #endregion

        #region keyboard graphic display generator

        private void clear_keys()
        {
            int i;

            keydisplay.WaitOne();
            for (i = 0; i < NKEYS; i++)
            {
                kbufnew.SetValue(EMPTY_CODE, i);
                kbufold.SetValue(EMPTY_CODE, i);
            }
            keydisplay.ReleaseMutex();
        }

        private void show_keys()
        {
            string s;
            int i;
            int x, y, dx, dy;
            int kyrx = kylx + kyxsz + 1;
            int kyby = kyty + kyysz + 1;

            lock (this)
            {
                y = kyty + 2;
                dx = 11; dy = 19;

                System.Drawing.Graphics formGraphics = this.CreateGraphics();

                System.Drawing.Font drawFont = new System.Drawing.Font("Courier New", 14, FontStyle.Bold);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.SolidBrush grayBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
                System.Drawing.SolidBrush whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                formGraphics.FillRectangle(whiteBrush, new Rectangle(kylx, kyty, kyxsz + 1, kyysz + 1));

                // draw a box around the area
                Pen myPen = new Pen(Color.Gray, 1);
                formGraphics.DrawLine(myPen, kylx, kyty, kyrx, kyty);
                formGraphics.DrawLine(myPen, kyrx, kyty, kyrx, kyby);
                formGraphics.DrawLine(myPen, kyrx, kyby, kylx, kyby);
                formGraphics.DrawLine(myPen, kylx, kyby, kylx, kyty);
                myPen.Dispose();

                keydisplay.WaitOne();
                x = kylx;
                for (i = 0; i < NKEYS; i++)
                {
                    s = kbufold.GetValue(i).ToString();
                    formGraphics.DrawString(s, drawFont, grayBrush, (float)x, (float)y);
                    if ((i % NKPL) == (NKPL - 1))
                    {
                        x = kylx; y += dy;
                    }
                    else x += dx;
                }

                x = kylx;
                for (i = 0; i < NKEYS; i++)
                {
                    s = kbufnew.GetValue(i).ToString();
                    formGraphics.DrawString(s, drawFont, drawBrush, (float)x, (float)y);
                    if ((i % NKPL) == (NKPL - 1))
                    {
                        x = kylx; y += dy;
                    }
                    else x += dx;
                }
                keydisplay.ReleaseMutex();

                drawFont.Dispose();
                whiteBrush.Dispose();
                drawBrush.Dispose();
                grayBrush.Dispose();
                formGraphics.Dispose();
            }
        } // show_keys()

        private void clearButton_Click(object sender, System.EventArgs e)
        {
            clear_show();
        }
        private void clear_show()
        {
            clear_keys();
            show_keys();
        }


        #endregion

        #region Morse code definition editor interface

        public string editline;     // for passing definition line to the editor

        private void editit()
        {
            clear_show();
            Debug.WriteLine("Quit4");
            quitshut();

            editline = cbMorse.Text;
#if (CWX_DEBUG)
			Debug.WriteLine(editline);
			Debug.WriteLine(editline.Length);
#endif
            if (editline[5] == '*') //.Substring(5,1).Equals("*"))
            {
                MessageBox.Show(new Form { TopMost = true }, "Definitions that start with '*' cannot be edited");
            }
            else if (editline.Length != 26)
            {
                MessageBox.Show(new Form { TopMost = true }, "Selected line has invalid length");
            }
            else
            {
                cwedit cweditDialog = new cwedit(console);
                cweditDialog.ShowDialog();
#if (CWX_DEBUG)
				Debug.WriteLine(editline);
				Debug.WriteLine(editline.Length);
#endif
                if (editline.Length == 26) insert_and_reload(editline);
                else if (editline.Length > 0)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Edited line has invalid length and is not saved.");
                }
            }
        }

        private void insert_and_reload(string s)    // replace line s in a2m2 with s then re-write file
        {
            int id;
#if true
            //id = int.Parse("q");		// see an exception here
            id = int.Parse(s.Substring(0, 2));
            id -= 32;
            if (id < 0 || id > 63)
            {
                MessageBox.Show(new Form { TopMost = true }, "Edited line cannot be found in a2m2.");
                return;
            }
            a2m2[id] = s;       // replace with the new lines
            write_a2m2();       // write new files
            load_alpha();       // read it back in
            build_mbits2();     // and rebuild the mbits array
#else
			for (id = 0; id < 64; id++)		// find the old one in a2m2
			{
				if (a2m2[id].StartsWith(s.Substring(0,2)))
				{
					a2m2[id] = s;		// replace with the new lines
					write_a2m2();		// write new files
					load_alpha();		// read it back in
					build_mbits2();		// and rebuild the mbits array
					return;
				}
			}
			MessageBox.Show(new Form { TopMost = true }, "Edited line cannot be found in a2m2.");
#endif
        }
        private void write_a2m2()
        {
            if (File.Exists(console.AppDataPath + sfile)) File.Delete(console.AppDataPath + sfile);            // out withe the old   // "\\" + for both

            using (StreamWriter sw = new StreamWriter(console.AppDataPath + sfile))  // and in with the new // "\\" +
            {
                for (int i = 0; i < 64; i++)
                {
                    sw.WriteLine(a2m2[i]);
                }
            }
        }
        #endregion

        #region where all the fun work is done

        // the following three procedures are the only non-gui thread sections
        // process_element() is called by the mmtimer at element rate
        // keyboardFifo() pops keystrokes and set into Morse elements
        // keyboardDisplay() watches and keeps the keyboard display going
        // all three are started by the constructor and killed in CWX_Closing


        // process_element is called at the element rate (width of dot)
        // and pulls commands out of the element fifo.  These are basically
        // to determine the state of the key during the next element time.
        // The timeout pause is also processed here.

        private void process_element()      // called at the element rate  (uses windows multimedia event timer) this is always called even when not transmitting
        {
            byte data;


            if (quit)       // if true shut 'er all down
            {
                Debug.WriteLine("Quit1");
                quitshut();
                quit = false;
                return;
            }

            if (newptt > 0)
            {
                newptt--;

                ttx = ttdel / tel;

                if (newptt > 0) return;

                Debug.WriteLine("newppt delay over");

                setkey(true);               // this was the defered key down

                return;
            }

            if (pause > 0)  // time out the pause
            {
                pause--;
                if (pause > 0) return;      // not yet done
                                            // pause ended, load 'er up again
                loadmsg(tqq);
                push_fifo(EL_END);          // end
                return;
            }

            while (true)        // ke9ns    endless for loop
            {
                if (infifo < 1) break;

                Debug.WriteLine("process_element");

                data = pop_fifo();

                if (data == EL_UNDERFLOW) return;   // underflow

                if (data == EL_END)     // end command
                {
                    Debug.WriteLine("Quit2");
                    quitshut();
                    return;
                }

                if (data == EL_PAUSE)       // pause command
                {
                    ttx = 0;
                    pause = tpause / tel;
                    if (pause < 1) pause = tel;
                    break;
                }

                if (data == EL_PTT)     // ptt only command
                {
                    setptt(true);
                    ttx = ttdel / tel;
                }
                if ((data == EL_KEYDOWN) || (data == EL_KEYUP))     // key command
                {
                    Debug.WriteLine("KEYDOWN???");

                    if (data == EL_KEYDOWN) // key down?
                    {
                        if (!ptt)   // we're gonna need a ptt->key delay setup
                        {
                            newptt = pttdelay / tel;
                            Debug.WriteLine("start newptt");
                        }
                        setptt(true);
                        ttx = ttdel / tel;

                        if (newptt > 0) return;     // the key will get pressed after newptt

                        setkey(true); // ke9ns generate TX output, tones, and signal on Panadapter
                    }
                    else
                    {
                        Debug.WriteLine("KEYUP??");

                        setkey(false); // ke9ns generate TX output, tones, and signal on Panadapter
                        break;
                    }
                }
                return;     // ignore all others  (ke9ns only allow 1 time through)

            } // while (true) endless loop

            // X on flow
            if (ttx > 0) ttx--;         // time out timer down one element

            if (ttx > 0) return;        // not yet timed out

            setptt(false);          // cw timer timed out

            setkey(false);

        } // process_element()


        //===============================================================================================
        // keyboardFifo pops keys from fifo2 and then calls loadchar() to
        // convert to Morse elements in infifo.  The routine will sleep until
        // most of the Morse character has been output by watching infifo.

        private void keyboardFifo()     // THREAD to watch the keyboard fifo
        {
            byte b;
            char c;

            while (!stopThreads)
            {
                if (infifo2 > 0)
                {
                    b = pop_fifo2();
                    if ((b >= 'a') && (b <= 'z')) b = (byte)((int)b - 'a' + 'A');
                    c = (char)b;

                    if (kquit)  // escape
                    {
                        clear_fifo2();
                        kquit = false;
                    }
                    //else 
                    if (b >= 32) // ke9ns only take characters from the 'space' char on up.
                    {
                        loadchar(c);
                        while (infifo > 2)
                        {
                            Thread.Sleep(10);
                        }
                    }
                    //Thread.Sleep(1000);
                }
                else Thread.Sleep(20);  // this was originally 10 ms
            }

        } // thread keyboardFifo()


        //=====================================================================================
        // the keyboardDisplay() thread pulls keys from the left hand edge (top)
        // of the keyboard display and stuffs them into fifo2 then causes the display
        // to be updated.  The keys are put into the display buffer by the keystroke event
        // handler.

        private void keyboardDisplay()      // THREAD watch and maintain the the keyboard display
        {
            char topkey;
            int i;

            while (!stopThreads)
            {
                while (pause_checked) Thread.Sleep(100);

                if ((topkey = (char)kbufnew.GetValue(0)) != EMPTY_CODE)
                {
                    keydisplay.WaitOne();
                    // shift old left one and insert topkey at the right
                    for (i = 0; i < (NKEYS - 1); i++)
                    {
                        kbufold.SetValue(kbufold.GetValue(i + 1), i);
                    }
                    kbufold.SetValue(topkey, NKEYS - 1);
                    // shift new left one and insert EMPTY_CODE at the right
                    for (i = 0; i < (NKEYS - 1); i++)
                    {
                        kbufnew.SetValue(kbufnew.GetValue(i + 1), i);
                    }
                    kbufnew.SetValue(EMPTY_CODE, NKEYS - 1);
                    keydisplay.ReleaseMutex();
                    push_fifo2((byte)topkey);
                    show_keys();
                    while (infifo > 0) Thread.Sleep(10);
                    // somehow wait here 'till character has been sent
                }
                else Thread.Sleep(20);      // this was originally 10 ms
            }

        } // thread keyboardDisplay()



        //===============================================================================================
        private void queue_start(int qmsg)          // queue message n for start
        {
            Debug.WriteLine("start of queue===================");

            if ((console.CWP == false)) // ke9ns add
            {
                if ((console.RX1DSPMode != DSPMode.CWL) && (console.RX1DSPMode != DSPMode.CWU))
                {
                    MessageBox.Show(new Form { TopMost = true }, "Console is not in CW mode.  Please switch to either CWL or CWU and try again.",
                        "CWX Error: Wrong Mode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            //kquit = true;
            //quit = true;

            Debug.WriteLine("before middle of queue===================");

            while (quit)
            {
                Thread.Sleep(10);
            }

            Debug.WriteLine("middle of queue===================");

            switch (qmsg)
            {
                case 1: tqq = txt1.Text; break;
                case 2: tqq = txt2.Text; break;
                case 3: tqq = txt3.Text; break;
                case 4: tqq = txt4.Text; break;
                case 5: tqq = txt5.Text; break;
                case 6: tqq = txt6.Text; break;
                case 7: tqq = txt7.Text; break;
                case 8: tqq = txt8.Text; break;
                case 9: tqq = txt9.Text; break;
                default: tqq = "?bad msg?"; break;
            }

            if (tqq.Length < 1) return; // ke9ns add  return if clicked on empty string

            Debug.WriteLine("size of tqq= " + tqq.Length);

            loadmsg(tqq);

            push_fifo(0x4);			// end

            Debug.WriteLine("end of queue===================");

        } //queue_start


        //==========================================================================================
        private void loadchar(char cc)       // convert and load a single character
        {                                    // this is the guts of loadmsg and work much the same way
            uint v, n;
            int ic;

            v = 0;
            ic = cc - ' ';

            if ((cc >= 'a') && (cc <= 'z')) ic -= 32;   // toupper case

            if (ic < 0 || ic > 63) return;      // ignore bad codes

            ic &= 0x3f;     // isolate the 0-63 code  (we did that in the previous line>)


            if (ic == 2) return;    // ignore loop back
            if (ic == 3) return;    // ignore long dash
            if (ic == 4) return;    //  and long space

            v = (uint)mbits.GetValue(ic);   // look up code for the character
                                            // top 24 of v are marks, bottom 5 have count

            n = v & 0x1f;       // element count
            while (n > 0)   // push n marks and spaces to the element fifo
            {
                if ((v & 0x80000000) > 0) push_fifo(EL_KEYDOWN);
                else push_fifo(EL_KEYUP);
                v <<= 1;
                n--;
            }
        } // loadchar



        //==================================================================================
        // ke9ns load text of message into a buffer here
        private void loadmsg(string t)  // load string t to the element fifo
        {
            string s;
            int ii, nc, ic;
            bool npause;
            uint v, n;
            char cc;
            // Debug.WriteLine("send message1 ========================");
            nc = t.Length;
            if (nc < 1)         // handle zero length string
            {
                t = "?";
                nc = t.Length;
            }

            s = t.ToUpper();    // convert keys to upper case

            char[] c = new char[nc + 1];        // move the string
            s.CopyTo(0, c, 0, nc);              //  into a char array

            nc = t.Length;
            clear_fifo();               // clear the element fifo at start of message

            // the following inserts a slight delay between ptt and 1st element
            for (int nptt = pttdelay / tel; nptt > 0; nptt--)
            {
                push_fifo(EL_PTT);  // early ptt
            }

            ii = 0;
            npause = false;
            while (nc > 0)
            {
                cc = (char)c.GetValue(ii);      // fetch next character

                v = 0;                  // clear element builder
                ic = cc - ' ';          // convert character into
                ic &= 0x3f;             // 0-63 code

                if (ic == 2)    // set loop back
                {
                    push_fifo(EL_PAUSE);        // stuff a pause code
                    npause = true;
                    break;                  // all done
                }
                else if (ic == 3)       // a long dash
                {
                    v = 0xffffff00 + 23;    // 23 marks
                }
                else if (ic == 4)       // a long space
                {
                    v = 23;             // 23 spaces
                }
                else
                {
                    v = (uint)mbits.GetValue(ic); // fetch Morse bit pattern for the character
                }

                // top 24 of v are marks, bottom 5 have count

                n = v & 0x1f;       // isolate the element count
                while (n > 0)       // push n marks/spaces to the element fifo
                {
                    if ((v & 0x80000000) > 0) push_fifo(EL_KEYDOWN);
                    else push_fifo(EL_KEYUP);
                    v <<= 1;
                    n--;
                }

                ii++;           // bump fetch index
                nc--;           //  and tally me banana ...




                //   Debug.WriteLine("send message2 ========================");

            } // while


            if (npause == false) push_fifo(EL_END); // stuff an end command if no
                                                    // pauses in the message
        } // loadmsg





        private void process_key(char key)  // keys from keyboardButton_KeyPress event
        {
#if (CWX_DEBUG)
			Debug.WriteLine((char)key + "key " + (int)key);
#endif

            if (key >= ' ' && key <= '~')   // a possible code
            {
                if (key >= 'a' && key <= 'z')   // convert to upper case
                {
                    key -= 'a';
                    key += 'A';
                }
                insert_key(key);        // insert into unsent
                show_keys();
            }
            else if (key == 8) backspace();
        }

        private void insert_key(char key)
        {
            int i;

            keydisplay.WaitOne();
            for (i = 0; i < NKEYS; i++)     // find 1st EMPTY_CODE character
            {
                if ((char)kbufnew.GetValue(i) == EMPTY_CODE)
                {
                    kbufnew.SetValue(key, i);
                    keydisplay.ReleaseMutex();
                    return;
                }
            }
            // no empty place, put at the end
            kbufnew.SetValue(key, NKEYS - 1);
            keydisplay.ReleaseMutex();
        }

        //=======================================================================================
        // ke9ns add
        private void ckKeyPoll_CheckedChanged(object sender, EventArgs e)
        {

        }

        //==========================================================
        // called after CWX_Closing
        private void CWX_FormClosing(object sender, FormClosingEventArgs e)
        {
            //  stopPoll = false; // ke9ns add shut down cw polling
            Debug.WriteLine("CWX_FORMCLOSING");
            //  e.Cancel = true; // ke9ns add hide dont actually close
            //   this.Hide();

            //   Common.SaveForm(this, "CWX");
        }

        // ke9ns add
        private void checkBoxCWD_CheckedChanged(object sender, EventArgs e)
        {
            if (console.setupForm != null)
            {
                console.setupForm.chkCWDisplay.Checked = checkBoxCWD.Checked;
            }
        }

        private void CWX_MouseEnter(object sender, EventArgs e)
        {
            if (console.setupForm.chkBoxAutoFocus.Checked == true && chkAlwaysOnTop.Checked == true) this.Activate();
        }

        //================================================================================================

        private void backspace()
        {
            int i;

            for (i = NKEYS - 1; i >= 0; i--)        // from left find first non empty
            {
                if ((char)kbufnew.GetValue(i) != EMPTY_CODE)
                {
                    kbufnew.SetValue(EMPTY_CODE, i);
                    show_keys();
                    return;
                }
            }
        }


        private void msg2keys(int nmsg)
        {
            int i, nc;
            string qq, s;

            char cc;
            char[] c = new char[5];


            switch (nmsg)
            {
                case 1: qq = txt1.Text; break;
                case 2: qq = txt2.Text; break;
                case 3: qq = txt3.Text; break;
                case 4: qq = txt4.Text; break;
                case 5: qq = txt5.Text; break;
                case 6: qq = txt6.Text; break;
                case 7: qq = txt7.Text; break;
                case 8: qq = txt8.Text; break;
                case 9: qq = txt9.Text; break;
                default: qq = "?bad msg?"; break;
            }

            insert_key(' ');
            nc = qq.Length;
            for (i = 0; i < nc; i++)
            {
                s = qq.Substring(i, 1);
                s.CopyTo(0, c, 0, 1);
                cc = (char)c.GetValue(0);
                insert_key(cc);
            }
            show_keys();
        }

        #endregion

        //====================================================================
        // ke9ns ADD   THREAD to check CW KEY
        private void PollKey()
        {
            bool dot, dash, rca_ptt, mic_ptt;
            Debug.WriteLine("POLLKEY BEGIN===========");

            while (!stopThreads)
            {
                Thread.Sleep(10);

                if ((stopPoll == true) && (chkKeyPoll.Checked == true))
                {
                    if (FWC.ReadPTT(out dot, out dash, out rca_ptt, out mic_ptt) != 0)   // ke9ns read Flex radio TRS plug and PTT circuits
                    {

                        if ((dot == true) || (dash == true))
                        {
                            clear_show();
                            quit = true;
                            kquit = true;

                        }
                    }
                    else break;

                    //  chkRCAPTT.Checked = rca_ptt;
                    //  chkMicPTT.Checked = mic_ptt;
                }
                // else break;


            } // while

            Debug.WriteLine("POLLKEY END===========");

        } // Pollkey


    } // end class
} // end namespace