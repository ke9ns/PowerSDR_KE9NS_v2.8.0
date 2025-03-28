//=================================================================
// CATCommands.cs
//=================================================================
// Copyright (C) 2012  Bob Tracy
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
// You may contact the author via email at: k5kdn@arrl.net
//=================================================================


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PowerSDR
{
    /// <summary>
    /// Summary description for CATCommands.
    /// </summary>
    public class CATCommands
    {
        #region Variable Definitions

        private Console console;
        private CATParser parser;
        private string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private Band[] BandList;
        private int LastBandIndex;
        private ASCIIEncoding AE = new ASCIIEncoding();
        private string lastFR = "0";
        private string lastFT = "0";

        //		public static Mutex CATmut = new Mutex();

        private bool verbose = false;
        public bool Verbose
        {
            get { return verbose; }
            set { verbose = value; }
        }


        #endregion Variable Definitions

        #region Constructors

        public CATCommands()
        {
        }

        public CATCommands(Console c, CATParser p)
        {
            console = c;
            parser = p;
            MakeBandList();
        }

        #endregion Constructors

        // Commands getting this far have been checked for a valid prefix, a correct suffix length,
        // and a terminator.  All we need to do in this class is to decide what kind of command
        // (read or set) and execute it.  Only read commands generate answers.

        #region Standard CAT Methods A-F

        // Sets or reads the Audio Gain control
        public string AG(string s)
        {
            if (s.Length == parser.nSet)    // if the length of the parameter legal for setting this prefix
            {
                int raw = Convert.ToInt32(s.Substring(1));
                int af = (int)Math.Round(raw / 2.55, 0);    // scale 255:100 (Kenwood vs SDR)
                console.AF = af;        // Set the console control
                return "";
            }
            else if (s.Length == parser.nGet)   // if this is a read command
            {
                int af = (int)Math.Round(console.AF / 0.392, 0);
                //				return AddLeadingZeros(console.AF);		// Get the console setting
                return AddLeadingZeros(af);
            }
            else
            {
                return parser.Error1;   // return a ?
            }
        }

        public string AI(string s)
        {
            return ZZAI(s);

            //			if(console.SetupForm.AllowFreqBroadcast)
            //			{
            //				if(s.Length == parser.nSet)
            //				{
            //					if(s == "0")
            //						console.KWAutoInformation = false;
            //					else 
            //						console.KWAutoInformation = true;
            //					return "";
            //				}
            //				else if(s.Length == parser.nGet)
            //				{
            //					if(console.KWAutoInformation)
            //						return "1";
            //					else
            //						return "0";
            //				}
            //				else
            //					return parser.Error1;
            //			}
            //			else
            //				return parser.Error1;
        }

        // Moves one band down from the currently selected band
        // write only
        public string BD()
        {
            //			BandDown();
            //			return "";
            return ZZBD();
        }

        // Moves one band up from the currently selected band
        // write only
        public string BU()
        {
            //			BandUp();
            //			return "";
            return ZZBU();
        }

        //Reads or sets the CTCSS frequency
        public string CN(string s)
        {
            return ZZTB(s);
        }

        //Reads or sets the CTCSS enable button
        public string CT(string s)
        {
            return ZZTA(s);
        }

        //Moves the VFO A frequency by the step size set on the console
        public string DN()
        {
            //			int step = console.StepSize;
            //			double[] wheel_tune_list;
            //			wheel_tune_list = new double[13];		// initialize wheel tuning list array
            //			wheel_tune_list[0]  =  0.000001;
            //			wheel_tune_list[1]  =  0.000010;
            //			wheel_tune_list[2]  =  0.000050;
            //			wheel_tune_list[3]  =  0.000100;
            //			wheel_tune_list[4]  =  0.000250;
            //			wheel_tune_list[5]  =  0.000500;
            //			wheel_tune_list[6]  =  0.001000;
            //			wheel_tune_list[7]  =  0.005000;
            //			wheel_tune_list[8]  =  0.009000;
            //			wheel_tune_list[9]  =  0.010000;
            //			wheel_tune_list[10] =  0.100000;
            //			wheel_tune_list[11] =  1.000000;
            //			wheel_tune_list[12] = 10.000000;
            //
            //			console.VFOAFreq = console.VFOAFreq - wheel_tune_list[step];
            //			return "";

            return ZZSA();
        }

        // Sets or reads the frequency of VFO A
        public string FA(string s)
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				s = s.Insert(5,separator);		//reinsert the global numeric separator
            //				console.VFOAFreq = double.Parse(s);
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //				return StrVFOFreq("A");
            //			else
            //				return parser.Error1;
            return ZZFA(s);
        }

        // Sets or reads the frequency of VFO B
        public string FB(string s)
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				s = s.Insert(5,separator);
            //				console.VFOBFreq = double.Parse(s);
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //				return StrVFOFreq("B");
            //			else
            //				return parser.Error1;
            return ZZFB(s);
        }

        // Sets VFO A to control rx
        // this is a dummy command to keep other software happy
        // since the SDR-1000 always uses VFO A for rx
        public string FR(string s)
        {
            if (s.Length == parser.nSet)
            {
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.SwapVFOA_BTX; // .278 whichever one is TX the other is obviously RX
                if (retval)
                  
                    return "0";
                  else
                    return "1";
   
            }
            else
                return parser.Error1;
        }

        // Sets or reads VFO B to control tx
        // another "happiness" command
        public string FT(string s) // ke9ns .200 mod (FT was returning ZZSP. which is wrong. FT is not ZZSP split, its get/set TX VFO)
        {

            return ZZSW(s);

        } //  FT(string s)

        // Sets or reads the DSP filter width
        //OBSOLETE
        public string FW(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.RX1Filter = String2Filter(s);
                return "";
            }
            else if (s.Length == parser.nGet)
                return Filter2String(console.RX1Filter);
            else
                return parser.Error1;
        }

        #endregion Standard CAT Methods A-F

        #region Standard CAT Methods G-M

        // Sets or reads the AGC constant
        // this is a wrapper that calls ZZGT
        public string GT(string s)
        {
            if (ZZGT(s).Length > 0)
                return ZZGT(s).PadLeft(3, '0');     //Added padleft fix 4/2/2007 BT
            else
                return "";
        }

        // Reads the transceiver ID number
        // this needs changing when 3rd party folks on line.
        public string ID()
        {
            string id;
            switch (console.CATRigType)
            {
                case 900:
                    id = "900";     //SDR-1000
                    break;
                case 13:
                    id = "013";     //TS-50S
                    break;
                case 19:
                    id = "019";     //TS-2000
                    break;
                case 20:
                    id = "020";     //TS-480
                    break;
                default:
                    id = "019";
                    break;
            }
            return (id);
        }

        // Reads the transceiver status
        // needs work in the split area
        public string IF() // 35 characters of data to return  P9=1 in kenwood, p9=2 in zzif
        {
            string rtn = "";
            string rit = "0";
            string xit = "0";
            string incr;
            string tx = "0";
            string tempmode = "";
            int ITValue = 0;
            //string temp;

            // Get the rit/xit status
            if (console.RITOn)  rit = "1";
            else if (console.XITOn) xit = "1";

            // Get the incremental tuning value for whichever control is selected
            if (rit == "1")  ITValue = console.RITValue;
            else if (xit == "1")  ITValue = console.XITValue;


            // Format the IT value
            if (ITValue < 0)  incr = "-" + Convert.ToString(Math.Abs(ITValue)).PadLeft(5, '0');
            else              incr = "+" + Convert.ToString(Math.Abs(ITValue)).PadLeft(5, '0');
            
            // Get the rx - tx status
            if (console.MOX)  tx = "1";
            
            // Get the step size
            int step = console.TuneStepIndex;
            string stepsize = Step2String(step);
            
            // Get the vfo split status
            string split = "0";
            
            bool retval = console.VFOSplit;
            if (retval) split = "1";

            //Get the mode
            //			temp = Mode2KString(console.RX1DSPMode);   //possible fix for SAM problem
            //			if(temp == parser.Error1)
            //				temp = " ";

            string f = ZZFA("");

            if (console.SpoofAB == true) // ke9ns: return VFOB freq instead when using RX2 VFOB COM port which spoofs the program into using RX2
            {
                f = ZZFB("");
            }

          //  if (console.SpoofRXATXB == true) //.311
          //  {
          //      f = ZZFB("");
          //  }

            if (f.Length > 11)
            {
                f = f.Substring(f.Length - 11, 11);
            }

            //------------------------------------------------ KE9NS put the IF cat string together below

            rtn += f;                                   // P1 VFO A frequency			11 bytes    	originally rtn += StrVFOFreq("A");	
            rtn += stepsize;                            // P2  Console step frequency	 4 bytes
            rtn += incr;                                // P3 incremental tuning value	 6 bytes
            rtn += rit;                                 // P4 RIT status				 1 byte
            rtn += xit;                                 // P5 XIT status				 1 byte
            rtn += "000";                               // P6 P7 dummy for memory bank	 3 bytes
            rtn += tx;                                  // P8 tx-rx status				 1 byte
                                                        //			rtn += temp;
                                                        //			rtn += Mode2KString(console.RX1DSPMode);	// current mode			 1 bytes

            if (console.SpoofAB == true) // return VFOB mode instead
            {
                tempmode = Mode2KString(console.RX2DSPMode);
            }
            else
            {
                tempmode = Mode2KString(console.RX1DSPMode);
            }

            if (tempmode == "?;")  rtn += "2";
            else rtn += tempmode;                        // P9 (1 byte kenwood, 2 bytes ZZIF)

            rtn += "0";                                 // P10 dummy for FR  			 1 byte
            rtn += "0";                                 // P11 dummy for scan status	 1 byte

            if (split == "0" && console.chkVFOBTX.Checked) //.278 you are in split mode if the 2nd receiver is on and its in TX mode
            {
                rtn += "1";
            }
            else
            {
                rtn += split;                               // P12 VFO Split status	FT		 1 byte
            }

            rtn += "0000";                              // P13,P14,P15 dummy for the balance	 4 bytes

            return rtn;                                 // total bytes				35

            //			// Initalize the command word
            //			string cmd_string = "";
            //			string temp;
            //
            //			// Get VFOA's frequency (P1 - 11 bytes)
            //			if(console.VFOSplit)
            //				cmd_string += StrVFOFreq("B");
            //			else
            //				cmd_string += StrVFOFreq("A");
            //
            //			// Get the step size index (P2 - 4 bytes)
            //			cmd_string += ZZST();
            //
            //			// Determine which incremental tuning control is active
            //			// and get the value for the active control 
            //			string rit = "0";
            //			string xit = "0";
            //			int ITValue = 0;
            //
            //			if(console.RITOn)
            //				rit = "1";
            //			else if(console.XITOn)
            //				xit = "1";
            //
            //			if(rit == "1")
            //				ITValue = console.RITValue;
            //			else if(xit == "1")
            //				ITValue = console.XITValue;
            //
            //			// Add the ITValue to the command string (P3 - 6 bytes
            //			if(ITValue < 0)
            //				cmd_string += "-"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
            //			else
            //				cmd_string += "+"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
            //
            //			// Add the RIT/XIT status bits (P4 and P5, one byte each)
            //			cmd_string += rit+xit;
            //				
            //			// Skip the memory channel stuff, the SDR1K doesn't use banks and channels per se
            //			// (P6 - 1 byte, P7 - 2 bytes)
            //			cmd_string += "000";
            //
            //			// Set the current MOX state (P8 - 1 byte)(what the heck is this for?)
            //			if(console.MOX)
            //				cmd_string += "1";
            //			else
            //				cmd_string += "0";
            //
            //			// Get the SDR mode.  (P9 - 1 byte)
            //			temp = Mode2KString(console.RX1DSPMode);
            //			if(temp.Length == 1)	// if the answer is not an error message ?;
            //				cmd_string += temp;
            //			else
            //				cmd_string += " ";	// return a blank if it's an error
            //
            //			// Set the FR/FT commands which determines the transmit and receive
            //			// VFO's. VFO A = 0, VFO B = 1. (P10 - 1 byte)
            //			if(console.VFOSplit)
            //				cmd_string += "1";
            //			else
            //				cmd_string += "0";
            //
            //
            //			// Set the Scan code to 0 
            //			// The Scan code might be implemented but the frequency range would
            //			// have to be manually entered. (P11 - 1 byte)
            //			cmd_string += "0";
            //
            //			// Set the Split operation code (P12 - 1 byte)
            //			cmd_string += ZZSP("");
            //
            //			// Set the remaining CTCSS tone and shift bits to 0 (P13 - P15, 4 bytes)
            //			cmd_string += "0000";
            //
            //			return cmd_string;
        }

        //Sets or reads the CWX CW speed
        public string KS(string s)
        {
            //			int cws = 0;
            //			// Make sure we have an instance of the form
            //			if(console.CWXForm == null || console.CWXForm.IsDisposed)
            //			{
            //				try
            //				{
            //					console.CWXForm = new CWX(console);
            //				}
            //				catch
            //				{
            //					return parser.Error1;
            //				}
            //			}
            //			if(s.Length == parser.nSet)
            //			{
            //				cws = Convert.ToInt32(s);
            //				cws = Math.Max(1, cws);
            //				cws = Math.Min(99, cws);
            //				console.CWXForm.WPM = cws;
            //				return "";
            //
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				return AddLeadingZeros(console.CWXForm.WPM);
            //			}
            //			else
            //				return parser.Error1;
            return ZZKS(s);
        }

        //Sends text data to CWX for conversion to Morse
        public string KY(string s)
        {
            // Make sure we have an instance of the form
            if (console.cwxForm == null || console.cwxForm.IsDisposed)
            {
                try
                {
                    console.cwxForm = new CWX(console);
                }
                catch
                {
                    return parser.Error1;
                }
            }

            // Make sure we are in a cw mode.
            switch (console.RX1DSPMode)
            {
                case DSPMode.AM:
                case DSPMode.DRM:
                case DSPMode.DSB:
                case DSPMode.FM:
                case DSPMode.SAM:
                case DSPMode.SPEC:
                case DSPMode.LSB:
                case DSPMode.USB:
                    if (console.RX1Band >= Band.B160M && console.RX1Band <= Band.B40M)
                        console.RX1DSPMode = DSPMode.CWL;
                    else
                        console.RX1DSPMode = DSPMode.CWU;
                    break;
                case DSPMode.CWL:
                case DSPMode.CWU:
                    break;
                default:
                    console.RX1DSPMode = DSPMode.CWU;
                    break;
            }

            if (s.Length == parser.nSet)
            {

                string trms = "";
                byte[] msg;
                string x = s.Trim();

                if (x.Length == 0)
                    trms = " ";
                else
                    trms = s.TrimEnd();

                if (trms.Length > 1)
                {
                    msg = AE.GetBytes(trms);
                    return console.cwxForm.RemoteMessage(msg);
                }
                else
                {
                    char ss = Convert.ToChar(trms);
                    return console.cwxForm.RemoteMessage(ss);
                }
            }
            else if (s.Length == parser.nGet)
            {
                int ch = console.cwxForm.Characters2Send;

                if (ch < 72)
                    return "0";
                else
                    return "1";
            }
            else
                return parser.Error1;
        }


        // Sets or reads the transceiver mode
        public string MD(string s)
        {
            if (console.SpoofAB == true)
            {
                if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
                {
                    if (s.Length == parser.nGet)
                    {
                        return Mode2String(console.RX2DSPMode);
                    }
                    else if (s.Length == parser.nSet && s != "8")
                    {
                        switch (s)
                        {
                            case "0":
                                console.RX2DSPMode = DSPMode.LSB;
                                break;
                            case "1":
                                console.RX2DSPMode = DSPMode.USB;
                                break;
                            case "2":
                                console.RX2DSPMode = DSPMode.DSB;
                                break;
                            case "3":
                                console.RX2DSPMode = DSPMode.CWL;
                                break;
                            case "4":
                                console.RX2DSPMode = DSPMode.CWU;
                                break;
                            case "5":
                                console.RX2DSPMode = DSPMode.FM;
                                break;
                            case "6":
                                console.RX2DSPMode = DSPMode.AM;
                                break;
                            case "7":
                                console.RX2DSPMode = DSPMode.DIGU;
                                break;
                            case "9":
                                console.RX2DSPMode = DSPMode.DIGL;
                                break;

                        }
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else
                {
                    parser.Verbose_Error_Code = 7;
                    return parser.Error1;
                }
            }
            else
            {
                if (s.Length == parser.nSet)
                {
                    if (Convert.ToInt32(s) > 0 && Convert.ToInt32(s) <= 9)
                    {
                        KString2Mode(s);
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else if (s.Length == parser.nGet)
                {

                    return Mode2KString(console.RX1DSPMode);

                }
                else
                    return parser.Error1;
            }
        }

        // Sets or reads the Mic Gain thumbwheel
        public string MG(string s)
        {
            int n;
            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                n = Math.Max(0, n);
                n = Math.Min(100, n);
                int mg = (int)Math.Round(n / 1.43, 0);  // scale 100:70 (Kenwood vs SDR)
                s = AddLeadingZeros(mg);
                return ZZMG(s);
            }
            else if (s.Length == parser.nGet)
            {
                s = ZZMG("");
                n = Convert.ToInt32(s);
                int mg = (int)Math.Round(n / .7, 0);
                s = AddLeadingZeros(mg);
                return s;
            }
            else
                return parser.Error1;
        }

        // Sets or reads the Monitor status
        public string MO(string s)
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				if(s == "0")
            //					console.MON = false;
            //				else if(s == "1")
            //					console.MON = true;
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				bool retval = console.MON;
            //				if(retval)
            //					return "1";
            //				else
            //					return "0";
            //			}
            //			else
            //				return parser.Error1;
            return ZZMO(s);
        }

        #endregion Standard CAT Methods G-M

        #region Standard CAT Methods N-Q

        // Sets or reads the Noise Blanker 1 status
        public string NB(string s)
        {
            //			if(s.Length == parser.nSet && (s == "0" || s == "1"))
            //			{
            //				console.CATNB1 = Convert.ToInt32(s);
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				return console.CATNB1.ToString();
            //			}
            //			else
            //			{
            //				return parser.Error1;
            //			}
            return ZZNA(s);
        }

        // Sets or reads the Automatic Notch Filter status
        public string NT(string s)
        {
            //			if(s.Length == parser.nSet && (s == "0" || s == "1"))
            //			{
            //				console.CATANF = Convert.ToInt32(s);
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				return console.CATANF.ToString();
            //			}
            //			else
            //			{
            //				return parser.Error1;
            //			}
            return ZZNT(s);
        }

        //Sets or reads the FM repeater offset frequency
        public string OF(string s)
        {
            return ZZOT(s);
        }

        //Sets or reads the repeater offset direction
        public string OS(string s)
        {
            return ZZOS(s);
        }


        // Sets or reads the PA output thumbwheel
        public string PC(string s)
        {
            //			int pwr = 0;
            //
            //			if(s.Length == parser.nSet)
            //			{
            //				pwr = Convert.ToInt32(s);
            //				console.PWR = pwr;
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				return AddLeadingZeros(console.PWR);
            //			}
            //			else
            //			{
            //				return parser.Error1;
            //			}
            return ZZPC(s);
        }

        // Sets or reads the Speech Compressor status
        //Reactivated 10/21/2012 for HRD compatibility BT
        public string PR(string s)
        {
            if (s.Length == parser.nGet)
            {
                return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the console power on/off status
        public string PS(string s)
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				if(s == "0")
            //					console.PowerOn = false;
            //				else if(s == "1")
            //					console.PowerOn = true;
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				bool pwr = console.PowerOn;
            //				if(pwr)
            //					return "1";
            //				else
            //					return "0";
            //			}
            //			else
            //			{
            //				return parser.Error1;
            //			}
            return ZZPS(s);
        }

        // Sets or reads the CW PITCH in 3 digit HZ
        public string PT(string s) //.311
        {
            if(s.Length == parser.nSet)
            {
               int i = 0;
               bool isValid = int.TryParse(s, out i);

               if (isValid)
               {
                   console.CWPitch = i;
               }
               else
                    return parser.Error1;
            }
            else if(s.Length == parser.nGet)
            {
                 return console.CWPitch.ToString("000");

            }
            else
            {
                return parser.Error1;
            }
            return parser.Error1;
        } //PT


        // Sets the Quick Memory with the current contents of VFO A
        public string QI()
        {
            //			console.CATMemoryQS();
            //			return "";
            return ZZQS();
        }

        #endregion Standard CAT Methods N-Q

        #region Standard CAT Methods R-Z

        // Clears the RIT value
        // write only
        public string RC()
        {
            //			console.RITValue = 0;
            //			return "";
            return ZZRC();
        }

        //Decrements RIT
        public string RD(string s)
        {
            return ZZRD(s);
        }


        // Sets or reads the RIT status (on/off)
        public string RT(string s)
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				if(s == "0")
            //					console.RITOn = false;
            //				else if(s == "1") 
            //					console.RITOn = true;
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				bool rit = console.RITOn;
            //				if(rit)
            //					return "1";
            //				else
            //					return "0";
            //			}
            //			else
            //			{
            //				return parser.Error1;
            //			}
            return ZZRT(s);
        }

        //Increments RIT
        public string RU(string s)
        {
            return ZZRU(s);
        }


        // Sets or reads the transceiver receive mode status
        // write only but spec shows an answer parameter for a read???
        public string RX(string s)
        {
            console.CATPTT = false;

            if (console.SpoofTX == true)
            {
                console.SpoofTX = false;
                if (console.LastVFOBTX == true)
                {
                    console.LastVFOBTX = false;
                    // set VFOTX back to VFOA if it was moved to B due to a spoof
                    console.SwapVFOA_BTX = false; // VFOA is TX
                                                  //   console.SwapVFOA_BTX = true; // VFOB is TX
                }

            }


            return "";
            //return ZZTX("0");
        }

        // Sets or reads the variable DSP filter high side
        public string SH(string s)
        {
            if (s.Length == parser.nSet)
            {
                SetFilter(s, "SH");
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                switch (console.RX1DSPMode)
                {
                    case DSPMode.AM:
                    case DSPMode.CWU:
                    case DSPMode.DRM:
                    case DSPMode.DSB:
                    case DSPMode.FM:
                    case DSPMode.SAM:
                    case DSPMode.USB:
                    case DSPMode.DIGU: // ke9ns add
                        return Frequency2Code(console.RX1FilterHigh, "SH");
                    case DSPMode.CWL:
                    case DSPMode.LSB:
                    case DSPMode.DIGL: // ke9ns add
                        return Frequency2Code(console.RX1FilterLow, "SH");
                    default:
                        return Frequency2Code(console.RX1FilterHigh, "SH");
                }
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the variable DSP filter low side
        public string SL(string s)
        {
            if (s.Length == parser.nSet)
            {
                SetFilter(s, "SL");
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                switch (console.RX1DSPMode)
                {
                    case DSPMode.AM:
                    case DSPMode.CWU:
                    case DSPMode.DRM:
                    case DSPMode.DSB:
                    case DSPMode.FM:
                    case DSPMode.SAM:
                    case DSPMode.USB:
                    case DSPMode.DIGU: // ke9ns add
                        return Frequency2Code(console.RX1FilterLow, "SL");
                    case DSPMode.CWL:
                    case DSPMode.LSB:
                    case DSPMode.DIGL: // ke9ns add
                        return Frequency2Code(console.RX1FilterHigh, "SL");
                    default:
                        return Frequency2Code(console.RX1FilterLow, "SL");
                }
            }
            else
            {
                return parser.Error1;
            }
        }

        // Reads the S Meter value
        public string SM(string s)
        {
            int sm = 0;
            double sx = 0.0;

            if (s == "0" || s == "2")   // read the main transceiver s meter
            {
                float num = 0f;
                if (console.PowerOn) num = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);

                num = num + console.MultiMeterCalOffset + console.PreampOffset;

                num = Math.Max(-140, num);
                num = Math.Min(-10, num);

                sx = (num + 127) / 6;
                if (sx < 0) sx = 0;

                if (sx <= 9.0F)
                {
                    sm = Math.Abs((int)(sx * 1.6667));
                }
                else
                {
                    double over_s9 = num + 73;
                    sm = 15 + (int)over_s9;
                }
                if (sm < 0) sm = 0;
                if (sm > 30) sm = 30;

                return sm.ToString().PadLeft(5, '0');
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the Squelch value
        public string SQ(string s)
        {
            string rx = s.Substring(0, 1);
            double level = 0.0;

            //Will need code to select receiver when n Receivers enabled.
            //for now, ignore rx number.

            if (s.Length == parser.nSet)
            //convert to a double and add the scale factor (160 = 255)
            {
                level = Convert.ToDouble(s.Substring(1));
                level = Math.Max(0, level);         // lower bound
                level = Math.Min(255, level);       // upper bound
                level = level * 0.62745;                // scale factor
                console.Squelch = Convert.ToInt32(Math.Round(level, 0));
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return rx + AddLeadingZeros(console.Squelch).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets the transmitter on, write only
        // will eventually need either Commander change or ZZ code
        // since it is not CAT compliant as it is
        public string TX(string s)
        {
            if (console.SpoofAB == true) // ke9ns add .200
            {
                // check which VFO is TX and save it here to return it back when done with TX
                //   console.SwapVFOA_BTX = false; // VFOA is TX
                console.SwapVFOA_BTX = true; // VFOB is TX
                console.SpoofTX = true;
            }

            console.CATPTT = true;

            return "";
            //return ZZTX("1");
        } // TX

        //Moves the VFO A frequency up by the step size set on the console
        public string UP()
        {
            //			int step = console.StepSize;
            //			double[] wheel_tune_list;
            //			wheel_tune_list = new double[13];		// initialize wheel tuning list array
            //			wheel_tune_list[0]  =  0.000001;
            //			wheel_tune_list[1]  =  0.000010;
            //			wheel_tune_list[2]  =  0.000050;
            //			wheel_tune_list[3]  =  0.000100;
            //			wheel_tune_list[4]  =  0.000250;
            //			wheel_tune_list[5]  =  0.000500;
            //			wheel_tune_list[6]  =  0.001000;
            //			wheel_tune_list[7]  =  0.005000;
            //			wheel_tune_list[8]  =  0.009000;
            //			wheel_tune_list[9]  =  0.010000;
            //			wheel_tune_list[10] =  0.100000;
            //			wheel_tune_list[11] =  1.000000;
            //			wheel_tune_list[12] = 10.000000;
            //
            //			console.VFOAFreq = console.VFOAFreq + wheel_tune_list[step];
            //			return "";

            return ZZSB();
        }


        // Sets or reads the transceiver XIT status (on/off)
        public string XT(string s)
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				if(s == "0")
            //					console.XITOn = false;
            //				else
            //					if(s == "1") 
            //					console.XITOn = true;
            //				return "";
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				bool xit = console.XITOn;
            //				if(xit)
            //					return "1";
            //				else
            //					return "0";
            //			}
            //			else
            //			{
            //				return parser.Error1;
            //			}
            return ZZXS(s);

        }

        #endregion Standard CAT Methods R-Z

        #region Extended CAT Methods ZZA-ZZF


        //Sets or reads the console step size (also see zzst(read only)
        public string ZZAC(string s)
        {
            int step = 0;
            if (s.Length == parser.nSet)
            {
                step = Convert.ToInt32(s);
                if (step >= 0 || step <= 14)
                {
                    console.TuneStepIndex = step;
                    return "";
                }
                else
                    return parser.Error1;
            }
            else if (s.Length == parser.nGet)
            {
                step = console.TuneStepIndex;
                return AddLeadingZeros(step);
            }
            else
                return parser.Error1;
        }

        //Sets VFO A down nn Tune Steps
        public string ZZAD(string s)
        {
            int step = 0;

         //   Debug.WriteLine("294 ZZAD " + s + " , " + s.Length );

            if (s.Length == parser.nSet)
            {
                step = Convert.ToInt32(s);

                if (step >= 0 || step <= 14)
                {

                    if (Console.CTUNIF == true) // ke9ns add .178 updated in .294
                    {
                        if (console.setupForm != null)
                        {
                          
                            if (console.setupForm.chkBoxWheelRev.Checked == true) console.Console_MouseWheel(this.console, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 120));
                            else console.Console_MouseWheel(this.console, new MouseEventArgs(MouseButtons.None, 0, 0, 0, -120));
                        }
                    }
                    else
                    {
                        console.VFOAFreq = console.CATVFOA - Step2Freq(step);
                    }
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;
        }

        // Sets or reads the SDR-1000 Audio Gain control
        public string ZZAG(string s)
        {
            int af = 0;

            if (s.Length == parser.nSet)    // if the length of the parameter legal for setting this prefix
            {
                af = Convert.ToInt32(s);
                af = Math.Max(0, af);
                af = Math.Min(100, af);
                console.AF = af;        // Set the console control
                return "";
            }
            else if (s.Length == parser.nGet)   // if this is a read command
            {
                return AddLeadingZeros(console.AF);     // Get the console setting
            }
            else
            {
                return parser.Error1;   // return a ?
            }

        }

        public string ZZAI(string s)
        {
            // if ( console.setupForm.AllowFreqBroadcast)
            if (console.KWAI1 && console.setupForm.AllowFreqBroadcast) // ke9ns mod .214
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation = false;
                    else
                        console.KWAutoInformation = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else if (console.KWAI2 && console.setupForm.AllowFreqBroadcast2) // ke9ns add .214
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation2 = false;
                    else
                        console.KWAutoInformation2 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation2)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else if (console.KWAI3 && console.setupForm.AllowFreqBroadcast3) // ke9ns add .214
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation3 = false;
                    else
                        console.KWAutoInformation3 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation3)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else if (console.KWAI4 && console.setupForm.AllowFreqBroadcast4) // ke9ns add .214
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation4 = false;
                    else
                        console.KWAutoInformation4 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation4)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else if (console.KWAI5 && console.setupForm.AllowFreqBroadcast5) // ke9ns add .214
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation5 = false;
                    else
                        console.KWAutoInformation5 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation5)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else if (console.KWAI6 && console.setupForm.AllowFreqBroadcast6) // ke9ns add .214
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation6 = false;
                    else
                        console.KWAutoInformation6 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation6)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else if (console.KWAI8 && console.setupForm.AllowFreqBroadcast8) // ke9ns add .311 (do below if the AI is checked for this port)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation8 = false;
                    else
                        console.KWAutoInformation8 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation8)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            } //.311
            else if (console.KWAI7 && console.setupForm.AllowFreqBroadcast7) // ke9ns add .214 for TCP/IP CAT
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.KWAutoInformation7 = false;
                    else
                        console.KWAutoInformation7 = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.KWAutoInformation7)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;


        } //  public string ZZAI(string s)

        //Sets or reads the AGC RF gain
        public string ZZAR(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-20, n);
                n = Math.Min(120, n);
            }

            if (s.Length == parser.nSet)
            {
                console.RF = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.RF;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the RX2 AGC-T
        public string ZZAS(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                int n = 0;
                int x = 0;
                string sign;

                if (s != "")
                {
                    n = Convert.ToInt32(s);
                    n = Math.Max(-20, n);
                    n = Math.Min(120, n);
                }

                if (s.Length == parser.nSet)
                {
                    console.RX2RF = n;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    x = console.RX2RF;
                    if (x >= 0)
                        sign = "+";
                    else
                        sign = "-";
                    // we have to remove the leading zero and replace it with the sign.
                    return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets VFO A up nn Tune Steps
        public string ZZAU(string s)
        {

            int step = 0;

            if (s.Length == parser.nSet)
            {
                step = Convert.ToInt32(s);

                if (step >= 0 || step <= 14)
                {

                    if (Console.CTUNIF == true) // ke9ns add .178 //.294
                    {
                        if (console.setupForm != null)
                        {
                            if (console.setupForm.chkBoxWheelRev.Checked == true) console.Console_MouseWheel(this.console, new MouseEventArgs(MouseButtons.None, 0, 0, 0, -120));
                            else console.Console_MouseWheel(this.console, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 120));
                        }
                    }
                    else
                    {

                        console.VFOAFreq = console.CATVFOA + Step2Freq(step);
                    }
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
            {
                return parser.Error1;
            }
        }

        //Moves the RX2 bandswitch down one band
        public string ZZBA()
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                console.CATRX2BandUpDown(-1);
                return "";
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Moves the RX2 bandswitch up one band
        public string ZZBB()
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                console.CATRX2BandUpDown(1);
                return "";
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        //Moves the RX1 bandswitch down one band
        public string ZZBD()
        {
            BandDown();
            return "";
        }

        // Sets the Band Group (HF/VHF)
        public string ZZBG(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATBandGroup = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATBandGroup.ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the BIN button status
        public string ZZBI(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATBIN = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATBIN.ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets VFO B down nn Tune Steps
        public string ZZBM(string s)
        {
            int step = 0;
            if (s.Length == parser.nSet)
            {
                step = Convert.ToInt32(s);
                if (step >= 0 || step <= 14)
                {
                    console.VFOBFreq = console.CATVFOB - Step2Freq(step);
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;
        }

        //Sets VFO B up nn Tune Steps
        public string ZZBP(string s)
        {
            int step = 0;
            if (s.Length == parser.nSet)
            {
                step = Convert.ToInt32(s);
                if (step >= 0 || step <= 14)
                {
                    console.VFOBFreq = console.CATVFOB + Step2Freq(step);
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;
        }

        //Sets or reads the BCI Rejection button status
        public string ZZBR(string s)
        {
            if (console.CurrentModel == Model.SDR1000)
            {
                int sx = 0;

                if (s != "")
                    sx = Convert.ToInt32(s);

                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.CATBCIReject = sx;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.CATBCIReject.ToString();
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
                return parser.Error1;

        }


        //Sets or reads the current band setting
        public string ZZBS(string s)
        {
            return GetBand(s);
        }

        //Sets or gets the current RX2 band setting
        public string ZZBT(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nGet)
                {
                    return Band2String(console.RX2Band);
                }
                else if (s.Length == parser.nSet)
                {
                    console.RX2Band = String2Band(s);
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        //Moves the bandswitch up one band
        public string ZZBU()
        {
            BandUp();
            return "";
        }

        //Shuts down the console
        public string ZZBY()
        {
            
            Environment.Exit(0); // .257
          
         //   this.console.Close();
                       
            return "";
        }

        // Sets or reads the CW Break In Enabled checkbox
        public string ZZCB(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.BreakInEnabled = true;
                else
                    console.BreakInEnabled = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.BreakInEnabled)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }


        // Sets or reads the CW Break In Delay
        public string ZZCD(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(150, n);
            n = Math.Min(5000, n);

            if (s.Length == parser.nSet)
            {
                console.setupForm.BreakInDelay = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.setupForm.BreakInDelay);
            }
            else
            {
                return parser.Error1;
            }

        }

        // Sets or reads the Show CW Frequency checkbox
        public string ZZCF(string s)
        {
            switch (console.RX1DSPMode)
            {
                case DSPMode.CWL:
                case DSPMode.CWU:
                    if (s.Length == parser.nSet && (s == "0" || s == "1"))
                    {
                        if (s == "1")
                            console.ShowCWTXFreq = true;
                        else
                            console.ShowCWTXFreq = false;
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                    {
                        if (console.ShowCWTXFreq)
                            return "1";
                        else
                            return "0";
                    }
                    else
                    {
                        return parser.Error1;
                    }
                default:
                    return parser.Error1;
            }
        }

        // Sets or reads the CW Iambic checkbox
        public string ZZCI(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.CWIambic = true;
                else
                    console.CWIambic = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.CWIambic)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

        // Sets or reads the CW Pitch thumbwheel
        public string ZZCL(string s)
        {
            int n = 0;
            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.setupForm.CATCWPitch = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.CATCWPitch);
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the CW Monitor Disable button status
        public string ZZCM(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.CWSidetone = false;
                else
                    console.CWSidetone = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.CWSidetone)
                    return "0";
                else
                    return "1";
            }
            else
            {
                return parser.Error1;
            }

        }

        // Sets or reads the compander button status
        public string ZZCP(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.CATCmpd = 0;
                else if (s == "1")
                    console.CATCmpd = 1;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATCmpd.ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the CW Speed thumbwheel
        public string ZZCS(string s)
        {
            int n = 1;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.CATCWSpeed = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.CATCWSpeed);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Reads or sets the compander threshold
        public string ZZCT(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(10, n);

            if (s.Length == parser.nSet)
            {
                console.CPDRVal = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.CPDRVal);
            }
            else
            {
                return parser.Error1;
            }

        }

        // Reads the CPU Usage
        public string ZZCU()
        {
            return console.CpuUsage.ToString("f").PadLeft(6, '0');
        }

        // Sets or reads the Display Average status
        public string ZZDA(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATDisplayAvg = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATDisplayAvg.ToString();
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the Diversity Form Enable Button
        public string ZZDE(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK && console.CATDiversityForm)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "1")
                    {
                        console.CATDiversityEnable = true;
                        return "";
                    }
                    else if (s == "0")
                    {
                        console.CATDiversityEnable = false;
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CATDiversityEnable)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
            {
                if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
                    parser.Verbose_Error_Code = 8;
                else
                    parser.Verbose_Error_Code = 7;

                return parser.Error1;
            }
        }


        //Opens or closes the Diversity Form
        public string ZZDF(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "1")
                    {
                        console.CATDiversityForm = true;
                        return "";
                    }
                    else if (s == "0")
                    {
                        console.CATDiversityForm = false;
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CATDiversityForm)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the current display mode
        public string ZZDM(string s)
        {
            int n = -1;


            //  PANADAPTER,
            //  PANAFALL,        
            //   WATERFALL,
            //   PANASCOPE,
            //  SPECTRUM,
            //SCOPE,
            //PHASE,
            //PHASE2,
            //	HISTOGRAM,
            //	OFF,
            //  CONTINUUM,

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                switch (n)
                {
                    case 0:
                        console.DisplayModeText = "Spectrum";
                        break;
                    case 1:
                        console.DisplayModeText = "Panadapter";
                        break;
                    case 2:
                        console.DisplayModeText = "Scope";
                        break;
                    case 3:
                        console.DisplayModeText = "Phase";
                        break;
                    case 4:
                        console.DisplayModeText = "Phase2";
                        break;
                    case 5:
                        console.DisplayModeText = "Waterfall";
                        break;
                    case 6:
                        console.DisplayModeText = "Histogram";
                        break;
                    case 7:
                        console.DisplayModeText = "Panafall";
                        break;
                    case 8:
                        console.DisplayModeText = "Panascope";
                        break;
                    case 9:
                        console.DisplayModeText = "Off";
                        break;
                    case 10:
                        console.DisplayModeText = "Continuum";
                        break;
                }

                //				if(n > (int) DisplayMode.FIRST && n < (int) DisplayMode.LAST)
                //					Display.CurrentDisplayMode = (DisplayMode) n;
                //				else
                //					return parser.Error1;
                //
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return ((int)Display.CurrentDisplayMode).ToString();
            }
            else
            {
                return parser.Error1;
            }

        } // ZZDM

        //Reads or sets the setup form Waterfall Low value
        public string ZZDN(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.setupForm.CATWFLo = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.CATWFLo;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Reads or sets the setup form Waterfall High value
        public string ZZDO(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.setupForm.CATWFHi = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.CATWFHi;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }


        //Reads or sets the setup form Spectrum Grid Max Value
        public string ZZDP(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.setupForm.CATSGMax = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.CATSGMax;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Reads or sets the setup form Spectrum Grid Min Value
        public string ZZDQ(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.setupForm.CATSGMin = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.CATSGMin;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the Spectrum Grid Step
        public string ZZDR(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.CATSGStep = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.CATSGStep);
            }
            else
            {
                return parser.Error1;
            }

        }



        //Constructs the state word for DDUtil
        //read only
        public string ZZDU()
        {
            int old = parser.nAns;
            string sep = ":";
            string status = "";

            parser.nAns = 1;
            status += ZZSW("") + sep;
            status += ZZSP("") + sep;
            status += ZZTU("") + sep;
            status += ZZTX("") + sep;
            if (console.CurrentModel == Model.FLEX5000)
            {
                status += ZZOA("") + sep;
                if (FWCEEPROM.RX2OK)
                    status += ZZOB("") + sep;
                else
                    status += "0:";
                status += ZZOC("") + sep;
                if (FWCEEPROM.RX2OK)
                    status += ZZRS("") + sep;
                else
                    status += "0:";
            }
            else
                status += "0:0:0:0:";
            if (FWCEEPROM.RX2OK)
                status += ZZRT("") + sep;
            else
                status += "0:";
            status += ZZDM("") + sep;
            status += ZZGT("") + sep;
            status += ZZMU("") + sep;
            status += ZZXS("") + sep;

            parser.nAns = 2;
            status += ZZAC("") + sep;
            status += ZZMD("") + sep;
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                status += ZZME("") + sep;
                status += ZZFJ("") + sep;
            }
            else
                status += "00:00:";
            status += ZZFI("") + sep;

            parser.nAns = 3;
            if (console.CurrentModel == Model.FLEX5000)
            {
                status += ZZOF("") + sep;
                if (FWCEEPROM.RX2OK)
                    status += ZZBT("") + sep;
                else
                    status += "000:";
            }
            else
                status += "000:000:";
            status += ZZPC("") + sep;
            status += ZZBS("") + sep;
            status += ZZAG("") + sep;
            status += ZZKS("") + sep;
            status += ZZTO("") + sep;

            parser.nAns = 4;
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000)
                status += ZZRV() + sep;
            else
                status += "0000:";
            status += ZZSM("0") + sep;

            parser.nAns = 5;
            status += ZZRF("") + sep;
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000)
                status += ZZTS() + sep;
            else
                status += "00000:";
            status += ZZXF("") + sep;

            parser.nAns = 6;
            status += ZZCU() + sep;

            parser.nAns = 11;
            status += ZZFA("") + sep;
            status += ZZFB("");
            parser.nAns = old;
            return status;
        }


        /// <summary>
        /// Sets or reads the DX button status
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZDX(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.CATPhoneDX = s;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATPhoneDX;
            }
            else
                return parser.Error1;
        }

        //Reads or sets the DX threshold
        public string ZZDY(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(10, n);

            if (s.Length == parser.nSet)
            {
                console.DXLevel = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.DXLevel);
            }
            else
            {
                return parser.Error1;
            }

        }

        /// <summary>
        /// Reads or sets the RX equalizer.
        /// The CAT suffix string is 36 characters constant.
        /// Each value in the string occupies exactly three characters
        /// starting with the number of bands (003 or 010) followed by
        /// the preamp setting (-12 to 015) followed by 3 or 10 three digit
        /// EQ thumbwheel positions.  If the number of bands is 3, the
        /// last seven positions (21 characters) are all set to zero.
        /// Example:  10 band ZZEA010-09009005000-04-07-09-05000005009;
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZEA(string s)
        {
            if (s.Length == parser.nSet)
            {
                int nb = Int32.Parse(s.Substring(0, 3));            //Get the number of bands
                int[] ans = new int[nb + 1];                        //Create the integer array
                s = s.Remove(0, 3);                             //Get rid of the band count

                for (int x = 0; x <= nb; x++)                       //Parse the string into the array
                {
                    ans[x] = Int32.Parse(s.Substring(0, 3));
                    s = s.Remove(0, 3);                         //Remove the last three used
                }
                console.eqForm.RXEQ = ans;                      //Send the array to the eq form
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                int[] eqarray = console.eqForm.RXEQ;            //Get the equalizer array
                int nb = console.eqForm.NumBands;               //Get the number of bands in the array
                int val;                                        //Holds a temporary value
                string ans = nb.ToString().PadLeft(3, '0');     //The return string with the number of bands added

                for (int x = 0; x <= nb; x++)                   //Loop thru the array
                {
                    if (eqarray[x] < 0)
                    {
                        val = Math.Abs(eqarray[x]);                 //If the value is negative, format the answer
                        ans += "-" + val.ToString().PadLeft(2, '0');
                    }
                    else
                        ans += eqarray[x].ToString().PadLeft(3, '0');
                }
                ans = ans.PadRight(36, '0');                            //Add the padding if it's a 3 band eq
                return ans;
            }
            else
                return parser.Error1;
        }

        //Sets or reads the TX EQ settings
        public string ZZEB(string s)
        {
            if (s.Length == parser.nSet)
            {
                int nb = Int32.Parse(s.Substring(0, 3));            //Get the number of bands
                int[] ans = new int[nb + 1];                        //Create the integer array
                s = s.Remove(0, 3);                             //Get rid of the band count

                for (int x = 0; x <= nb; x++)                       //Parse the string into the array
                {
                    ans[x] = Int32.Parse(s.Substring(0, 3));
                    s = s.Remove(0, 3);                         //Remove the last three used
                }
                console.eqForm.TXEQ = ans;                      //Send the array to the eq form
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                int[] eqarray = console.eqForm.TXEQ;            //Get the equalizer array
                int nb = console.eqForm.NumBands;               //Get the number of bands in the array
                int val;                                        //Holds a temporary value
                string ans = nb.ToString().PadLeft(3, '0');     //The return string with the number of bands added

                for (int x = 0; x <= nb; x++)                   //Loop thru the array
                {
                    if (eqarray[x] < 0)
                    {
                        val = Math.Abs(eqarray[x]);                 //If the value is negative, format the answer
                        ans += "-" + val.ToString().PadLeft(2, '0');
                    }
                    else
                        ans += eqarray[x].ToString().PadLeft(3, '0');
                }
                ans = ans.PadRight(36, '0');                            //Add the padding if it's a 3 band eq
                return ans;
            }
            else
                return parser.Error1;
        }

        //Provides verbose CAT error reporting
        public string ZZEM(string s)
        {
            if (s.Length == parser.nSet && (s == "1" || s == "0"))
            {
                if (s == "1")
                    parser.Verbose = true;
                else
                    parser.Verbose = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (parser.Verbose)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;
        }


        //Sets or reads the RXEQ button statusl
        public string ZZER(string s)
        {
            if (s.Length == parser.nSet && (s == "1" || s == "0"))
            {
                if (s == "1")
                    console.CATRXEQ = "1";
                else if (s == "0")
                    console.CATRXEQ = "0";
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATRXEQ;
            }
            else
                return parser.Error1;
        }

        //Sets or reads the TXEQ button status
        public string ZZET(string s)
        {
            if (s.Length == parser.nSet && (s == "1" || s == "0"))
            {
                if (s == "1")
                    console.CATTXEQ = "1";
                else if (s == "0")
                    console.CATTXEQ = "0";
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATTXEQ;
            }
            else
                return parser.Error1;
        }


        //Sets or reads VFO A frequency
        public string ZZFA(string s)
        {

            if (console.setupForm == null) return "";
           
            if (console.SpoofAB == true) // get VFOB freq instead
            {
                if (s.Length == parser.nSet)
                {
                    if (console.setupForm.RttyOffsetEnabledB && (console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
                    {
                        int f = int.Parse(s);
                        if (console.RX1DSPMode == DSPMode.DIGU) f = f - Convert.ToInt32(console.setupForm.RttyOffsetHigh);
                        else if (console.RX1DSPMode == DSPMode.DIGL)  f = f + Convert.ToInt32(console.setupForm.RttyOffsetLow);
                        s = AddLeadingZeros(f);
                        s = s.Insert(5, separator);
                    }
                    else
                        s = s.Insert(5, separator);

                    console.VFOBFreq = double.Parse(s);
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.setupForm.RttyOffsetEnabledB && (console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
                    {
                        int f = Convert.ToInt32(Math.Round(console.CATVFOB, 6) * 1e6);
                        if (console.RX1DSPMode == DSPMode.DIGU)  f = f + Convert.ToInt32(console.setupForm.RttyOffsetHigh);
                        else if (console.RX1DSPMode == DSPMode.DIGL)   f = f - Convert.ToInt32(console.setupForm.RttyOffsetLow);
                        return AddLeadingZeros(f);
                    }
                    else
                        return StrVFOFreq("B");
                }
                else
                    return parser.Error1;



            } // SpoofAB = true (VFOB above)
            else // get VFOA freq
            {

                if (s.Length == parser.nSet)
                {
                    if (console.setupForm.RttyOffsetEnabledA && (console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
                    {
                        int f = int.Parse(s);
                        if (console.RX1DSPMode == DSPMode.DIGU) f = f - Convert.ToInt32(console.setupForm.RttyOffsetHigh);
                        else if (console.RX1DSPMode == DSPMode.DIGL) f = f + Convert.ToInt32(console.setupForm.RttyOffsetLow);
                        s = AddLeadingZeros(f);
                        s = s.Insert(5, separator);
                    }
                    else
                        s = s.Insert(5, separator);

                    if (console.SpoofRXATXB == true) //.311
                    {

                        double tempA = double.Parse(s); // .311
                    
                        console.VFOBFreq = tempA; //.311 when mouse click on the CWSkimmer display to move frequency, move VFOB becuase of RX<A, TX>B CAT port
               
                    }


                    else console.VFOAFreq = double.Parse(s);

                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.setupForm.RttyOffsetEnabledA && (console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
                    {
                        int f = Convert.ToInt32(Math.Round(console.CATVFOA, 6) * 1e6);
                        if (console.RX1DSPMode == DSPMode.DIGU) f = f + Convert.ToInt32(console.setupForm.RttyOffsetHigh);
                        else if (console.RX1DSPMode == DSPMode.DIGL) f = f - Convert.ToInt32(console.setupForm.RttyOffsetLow);

                        return AddLeadingZeros(f);
                    }
                    else
                    {
                        return StrVFOFreq("A");

                    }
                }
                else
                    return parser.Error1;
            } // VFOA (above)

        } // ZZFA

        //Sets or reads VFO B frequency
        public string ZZFB(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (console.setupForm.RttyOffsetEnabledB &&
                    (console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
                {
                    int f = int.Parse(s);
                    if (console.RX1DSPMode == DSPMode.DIGU) f = f - Convert.ToInt32(console.setupForm.RttyOffsetHigh);
                    else if (console.RX1DSPMode == DSPMode.DIGL) f = f + Convert.ToInt32(console.setupForm.RttyOffsetLow);
                    s = AddLeadingZeros(f);
                    s = s.Insert(5, separator);
                }
                else
                    s = s.Insert(5, separator);

                console.VFOBFreq = double.Parse(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.setupForm.RttyOffsetEnabledB && (console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
                {
                    int f = Convert.ToInt32(Math.Round(console.CATVFOB, 6) * 1e6);
                    if (console.RX1DSPMode == DSPMode.DIGU) f = f + Convert.ToInt32(console.setupForm.RttyOffsetHigh);
                    else if (console.RX1DSPMode == DSPMode.DIGL) f = f - Convert.ToInt32(console.setupForm.RttyOffsetLow);
                    return AddLeadingZeros(f);
                }
                else
                    return StrVFOFreq("B");
            }
            else
                return parser.Error1;
        }

        //Selects or reads the FM deviation radio button
        public string ZZFD(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "1")
                    console.FMDeviation_Hz = 5000;
                else
                    console.FMDeviation_Hz = 2500;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.FMDeviation_Hz == 5000)
                    return "1";
                else if (console.FMDeviation_Hz == 2500)
                    return "0";
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;
        }


        //Sets or reads the current filter index number
        public string ZZFI(string s)
        {
            int n = 0;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                if (n < (int)Filter.LAST)
                    console.RX1Filter = (Filter)n;
                else
                    return parser.Error1;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.RX1Filter);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the current RX2 DSP filter
        public string ZZFJ(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                int n = 0;
                if (s != "")
                    n = Convert.ToInt32(s);

                if (s.Length == parser.nSet)
                {
                    if (n < (int)Filter.LAST)
                        console.RX2Filter = (Filter)n;
                    else
                        return parser.Error1;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros((int)console.RX2Filter);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        /// <summary>
        /// Reads or sets the DSP Filter Low value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZFL(string s)
        {
            string sign;
            int n;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                n = Math.Min(9999, n);
                n = Math.Max(-9999, n);
                console.RX1FilterLow = n;
                console.UpdateRX1Filters(n, console.RX1FilterHigh);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                n = console.RX1FilterLow;
                if (n < 0)
                    sign = "-";
                else
                    sign = "+";

                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(n)).Substring(1);
                //				return AddLeadingZeros((int) console.RX1FilterLow);
            }
            else
                return parser.Error1;
        }

        /// <summary>
        /// Reads or sets the RX1 DSP Filter High value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZFH(string s)
        {
            string sign;
            int n;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                n = Math.Min(9999, n);
                n = Math.Max(-9999, n);
                console.RX1FilterHigh = n;
                console.UpdateRX1Filters(console.RX1FilterLow, n);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                n = console.RX1FilterHigh;
                if (n < 0)
                    sign = "-";
                else
                    sign = "+";

                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(n)).Substring(1);
            }
            else
                return parser.Error1;
        }

        public string ZZFM()
        {
            string radio = console.CurrentModel.ToString();

            if (radio == "SDR1000")
                return "0";
            else if (radio == "FLEX5000")
                return "1";
            else if (radio == "FLEX3000")
                return "2";
            else if (radio == "FLEX1500")
                return "3";
            else
            {

                return parser.Error1;
            }

        }


        /// <summary>
        /// Reads or sets the RX2 DSP Filter High value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZFR(string s)
        {
            string sign;
            int n;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                n = Math.Min(9999, n);
                n = Math.Max(-9999, n);
                console.RX2FilterHigh = n;
                console.UpdateRX2Filters(console.RX2FilterLow, n);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                n = console.RX2FilterHigh;
                if (n < 0)
                    sign = "-";
                else
                    sign = "+";

                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(n)).Substring(1);
            }
            else
                return parser.Error1;
        }


        /// <summary>
        /// Reads or sets the RX2 DSP Filter Low value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZFS(string s)
        {
            string sign;
            int n;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                n = Math.Min(9999, n);
                n = Math.Max(-9999, n);
                console.RX2FilterLow = n;
                console.UpdateRX2Filters(n, console.RX2FilterHigh);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                n = console.RX2FilterLow;
                if (n < 0)
                    sign = "-";
                else
                    sign = "+";

                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(n)).Substring(1);
            }
            else
                return parser.Error1;
        }

        //Reads FlexWire single byte value commands
        public string ZZFV(string s)
        {
            if (s.Length == parser.nGet)
            {
                String pattern = "^[a-fA-F0-9][a-fA-F0-9]$";
                Regex sfxpattern = new Regex(pattern);
                if (!sfxpattern.IsMatch(s))
                    return parser.Error1;

                byte addr = byte.Parse(s, NumberStyles.HexNumber);
                uint val;
                FWC.FlexWire_ReadValue(addr, out val);
                string ans = String.Format("{0:X2}", (byte)val);
                return ans;
            }
            else
                return parser.Error1;
        }

        //Reds FlexWire double byte value commands
        public string ZZFW(String s)
        {
            if (s.Length == parser.nGet)
            {
                String pattern = "^[a-fA-F0-9][a-fA-F0-9]$";
                Regex sfxpattern = new Regex(pattern);
                if (!sfxpattern.IsMatch(s))
                    return parser.Error1;

                byte addr = byte.Parse(s, NumberStyles.HexNumber);
                uint val;
                FWC.FlexWire_Read2Value(addr, out val);
                string ans1 = String.Format("{0:X2}", val >> 8);
                string ans2 = String.Format("{0:X2}", (byte)val);
                string ans = String.Concat(ans1, ans2);
                return ans;

            }
            else
                return parser.Error1;
        }

        //Sends FlexWire single byte value commands
        public string ZZFX(string s)
        {
            if (s.Length == parser.nSet)
            {
                String pattern = "^[a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9]$";
                Regex sfxpattern = new Regex(pattern);
                if (!sfxpattern.IsMatch(s))
                    return parser.Error1;

                byte addr = byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
                byte val = byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber);


                FWC.FlexWire_WriteValue(addr, val);
                return "";
            }
            else
                return parser.Error1;
        }

        //Sends FlexWire double byte value commands
        public string ZZFY(String s)
        {
            if (s.Length == parser.nSet)
            {
                String pattern = "^[a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9]$";
                Regex sfxpattern = new Regex(pattern);
                if (!sfxpattern.IsMatch(s))
                    return parser.Error1;

                byte addr = byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
                byte val1 = byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber);
                byte val2 = byte.Parse(s.Substring(4, 2), NumberStyles.HexNumber);


                FWC.FlexWire_Write2Value(addr, val1, val2);
                return "";

            }
            else
                return parser.Error1;
        }

        #endregion Extended CAT Methods ZZA-ZZF

        #region Extended CAT Methods ZZG-ZZM


        // Sets or reads the noise gate enable button status
        public string ZZGE(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.NoiseGateEnabled = true;
                else
                    console.NoiseGateEnabled = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.NoiseGateEnabled)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the noise gate level control
        public string ZZGL(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-160, n);
                n = Math.Min(0, n);
            }

            if (s.Length == parser.nSet)
            {
                console.NoiseGate = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.NoiseGate;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the AGC constant
        public string ZZGT(string s)
        {
            if (s.Length == parser.nSet)
            {
                if ((Convert.ToInt32(s) > (int)AGCMode.FIRST && Convert.ToInt32(s) < (int)AGCMode.LAST))
                    console.RX1AGCMode = (AGCMode)Convert.ToInt32(s);
                else
                    return parser.Error1;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return ((int)console.RX1AGCMode).ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the Audio Buffer Size
        public string ZZHA(string s)
        {

            if (s.Length == parser.nSet)
            {
                console.setupForm.AudioBufferSize = Index2Width(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.setupForm.AudioBufferSize);
            }
            else
                return parser.Error1;
        }

        //Sets or reads the DSP Phone RX Buffer Size
        public string ZZHR(string s)
        {
            if (s.Length == parser.nSet)
            {
                int width = Index2Width(s);
                console.DSPBufPhoneRX = width;
                console.setupForm.DSPPhoneRXBuffer = width;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.DSPBufPhoneRX);
            }
            else
                return parser.Error1;
        }

        //Sets or reads the DSP Phone TX Buffer Size
        public string ZZHT(string s)
        {
            if (s.Length == parser.nSet)
            {
                int width = Index2Width(s);
                console.DSPBufPhoneTX = width;
                console.setupForm.DSPPhoneTXBuffer = width;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.DSPBufPhoneTX);
            }
            else
                return parser.Error1;
        }

        //Sets or reads the DSP CW RX Buffer Size
        public string ZZHU(string s)
        {
            if (s.Length == parser.nSet)
            {
                int width = Index2Width(s);
                console.DSPBufCWRX = width;
                console.setupForm.DSPCWRXBuffer = width;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.DSPBufCWRX);
            }
            else
                return parser.Error1;
        }

        //Sets or reads the DSP CW TX Buffer Size
        public string ZZHV(string s)
        {
            if (s.Length == parser.nSet)
            {
                int width = Index2Width(s);
                console.DSPBufCWTX = width;
                console.setupForm.DSPCWTXBuffer = width;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.DSPBufCWTX);
            }
            else
                return parser.Error1;
        }

        //Sets or reads the DSP Digital RX Buffer Size
        public string ZZHW(string s)
        {
            if (s.Length == parser.nSet)
            {
                int width = Index2Width(s);
                console.DSPBufDigRX = width;
                console.setupForm.DSPDigRXBuffer = width;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.DSPBufDigRX);
            }
            else
                return parser.Error1;
        }

        //Sets or reads the DSP Digital TX Buffer Size
        public string ZZHX(string s)
        {
            if (s.Length == parser.nSet)
            {
                int width = Index2Width(s);
                console.DSPBufDigTX = width;
                console.setupForm.DSPDigTXBuffer = width;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return Width2Index(console.DSPBufDigTX);
            }
            else
                return parser.Error1;
        }

        // Sets the CAT Rig Type to SDR-1000
        //Modified 10/12/08 BT changed "SDR-1000" to "PowerSDR"
        public string ZZID()
        {
            //			if(s.Length == parser.nSet)
            //			{
            //				return CAT2RigType(s);
            //			}
            //			else if(s.Length == parser.nGet)
            //			{
            //				return RigType2CAT();
            //			}
            //			else
            //				return parser.Error1;
            console.setupForm.CATSetRig("PowerSDR");
            return "";
        }

        // Reads the SDR-1000 transceiver status
        public string ZZIF(string s)
        {
            string rtn = "";
            string rit = "0";
            string xit = "0";
            string incr;
            string tx = "0";
            int ITValue = 0;

            // Get the rit/xit status
            if (console.RITOn)
                rit = "1";
            else if (console.XITOn)
                xit = "1";
            // Get the incremental tuning value for whichever control is selected
            if (rit == "1")
                ITValue = console.RITValue;
            else if (xit == "1")
                ITValue = console.XITValue;
            // Format the IT value
            if (ITValue < 0)
                incr = "-" + Convert.ToString(Math.Abs(ITValue)).PadLeft(5, '0');
            else
                incr = "+" + Convert.ToString(Math.Abs(ITValue)).PadLeft(5, '0');
            // Get the rx - tx status
            if (console.MOX)
                tx = "1";
            // Get the step size
            int step = console.TuneStepIndex;
            string stepsize = Step2String(step);
            // Get the vfo split status
            string split = "0";
            bool retval = console.VFOSplit;
            if (retval)
                split = "1";

            string f = ZZFA("");

            if (console.SpoofAB == true) //.278  ke9ns: return VFOB freq instead when using RX2 VFOB COM port which spoofs the program into using RX2
            {
                f = ZZFB("");
            }

            if (f.Length > 11)
            {
                f = f.Substring(f.Length - 11, 11);
            }

           
          
            rtn += f; // P1
            //			rtn += StrVFOFreq("A");						// VFO A frequency			11 bytes
            rtn += stepsize;                            // P2 Console step frequency	 4 bytes
            rtn += incr;                                // P3 incremental tuning value	 6 bytes
            rtn += rit;                                 // P4 RIT status				 1 byte
            rtn += xit;                                 // P5 XIT status				 1 byte
            rtn += "000";                               // P6P7 dummy for memory bank	 3 bytes
            rtn += tx;                                  // P8tx-rx status				 1 byte

           
            if (console.SpoofAB == true) // return VFOB mode instead
            {
                rtn += Mode2KString(console.RX2DSPMode); // P9
       
            }
            else
            {
                rtn += Mode2String(console.RX1DSPMode);      // P9 current mode				 2 bytes (only 1 in kenwood)
            }


            rtn += "0";                                 // P10 dummy for FR/FT			 1 byte
            rtn += "0";                                 // P11 dummy for scan status	 1 byte

            if (split == "0" && console.chkVFOBTX.Checked) //.278 you are in split mode if the 2nd receiver is on and its in TX mode
            {
                rtn += "1";
            }
            else
            {
                rtn += split;                               // P12 VFO Split status			 1 byte
            }
          

            rtn += "0000";                              // P13-P15 dummy for the balance	 4 bytes
            return rtn;
        }

        //Reads the installed options
        public string ZZIO()
        {
            if (console.CurrentModel == Model.SDR1000)
            {
                return "000";
            }
            else
            {
                string atu = "0";
                string rx2 = "0";
                string vu = "0";
                if (FWCEEPROM.ATUOK)
                    atu = "1";
                if (FWCEEPROM.RX2OK)
                    rx2 = "1";
                if (FWCEEPROM.VUOK)
                    vu = "1";
                return atu + rx2 + vu;
            }
        }


        // Sets or reads the IF width
        public string ZZIS(string s)
        {
            int n = 0;

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.CATFilterWidth = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.CATFilterWidth);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the IF Shift
        public string ZZIT(string s)
        {
            int n = 0;
            string sign = "-";

            if (s != "")
                n = Convert.ToInt32(s);

            if (s.Length == parser.nSet)
            {
                console.CATFilterShift = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                n = console.CATFilterShift;
                if (n >= 0)
                {
                    sign = "+";
                }
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(n)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        // Resets the Filter Shift to zero.  Write only
        public string ZZIU()
        {
            console.CATFilterShiftReset = 1;    //Fixed XML entry 4/2/2007 to prevent return value.  BT
            return "";
        }

        public string ZZKO(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "1")
                {
                    console.CATCWXForm = true;
                    return "";
                }
                else if (s == "0")
                {
                    console.CATCWXForm = false;
                    return "";
                }
                else
                    return parser.Error1;
            }
            else if (s.Length == parser.nGet)
            {
                if (console.CATCWXForm)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;
        }


        //Sends a CWX macro
        public string ZZKM(string s)
        {
            int qn = 0;
            if (s != "0" && s.Length > 0)
            {
                qn = Convert.ToInt32(s);
                // Make sure we have an instance of the form
                if (console.cwxForm == null || console.cwxForm.IsDisposed)
                {
                    try
                    {
                        console.cwxForm = new CWX(console);
                    }
                    catch
                    {
                        return parser.Error1;
                    }
                }
                if (qn > 0 || qn < 10)
                {
                    console.cwxForm.StartQueue = qn;
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;
        }

        public string ZZKV(string s) // .267
        {
            int qn = 0;
            if (s != "0" && s.Length > 0)
            {
                qn = Convert.ToInt32(s);

              

                if (qn == 1)
                {
                    return "";

                }
                else if (qn == 2)
                {
                    return "";
                }
                else if (qn == 3)
                {
                    console.buttonCall_Click(this, EventArgs.Empty); // Reply F3 button 
                    return "";
                }
                else if (qn == 4)
                {
                    console.btnTrack_Click(this, EventArgs.Empty);// CQCQ button F4
                    return "";
                }
                else if (qn == 5)
                {
                    console.buttonVK1_Click(this, EventArgs.Empty); // VK1 F5
                    return "";

                }
                else if (qn == 6)
                {
                    console.buttonVK2_Click(this, EventArgs.Empty); // VK2 F6
                    return "";

                }
                else if (qn == 7)
                {

                    if (console.RX1DSPMode == DSPMode.FM)
                    {
                        if (console.chkFM1750.Checked == false) console.chkFM1750.Checked = true;
                        else console.chkFM1750.Checked = false;
                        console.chkFM1750_Click(this, EventArgs.Empty); // ke9ns add: .188
                    }
                    return "";
                }
                else if (qn == 8)
                {

                    if (console.setupForm.checkQuindarToneON.Checked == true) console.setupForm.checkQuindarToneON.Checked = false;
                    else console.setupForm.checkQuindarToneON.Checked = true;
                    return "";

                }// F8

                else if (qn == 9)
                {
                    if (console.comboDisplayMode.Text == "Panafall8020")
                    {

                        if (console.setupForm.udSS1H.Value < ((decimal)(Console.H10 * 5 / 6) * .1M)) // .197
                        {
                            console.mouseholdY = console.mouseholdY + 5;
                            console.setupForm.udSS1H.Value = console.mouseholdY;
                        }



                    } //panafall8020
                    else if (console.comboDisplayMode.Text == "Panafall")
                    {


                        if (console.setupForm.udSS2H.Value < ((decimal)(Console.H10 / 2) * .8M)) // .197
                        {
                            console.mouseholdYY = console.mouseholdYY + 5;
                            console.setupForm.udSS2H.Value = console.mouseholdYY;
                        }


                    } // panafall
                    return "";
                } // F9
                else if (qn == 10)
                {

                    if (console.comboDisplayMode.Text == "Panafall8020")
                    {
                        if (console.setupForm.udSS1H.Value > -((decimal)(Console.H10 * 5 / 6) * .2M)) // .197
                        {
                            console.mouseholdY = console.mouseholdY - 5;
                            console.setupForm.udSS1H.Value = console.mouseholdY;
                        }

                    }
                    else if (console.comboDisplayMode.Text == "Panafall")
                    {


                        if (console.setupForm.udSS2H.Value > -((decimal)(Console.H10 / 2) * .7M)) // .197
                        {
                            console.mouseholdYY = console.mouseholdYY - 5;
                            console.setupForm.udSS2H.Value = console.mouseholdYY;
                        }

                    }
                    return "";
                } // F10
                else if (qn == 11)
                {
                    return "";
                }
                else if (qn == 12)
                {
                    return "";
                }
                return parser.Error1;
            }
            else
                return parser.Error1;

        } // .267 ZZKV


        //Sets or reads the CWX CW speed
        public string ZZKS(string s)
        {
            int cws = 0;
            // Make sure we have an instance of the form
            if (console.cwxForm == null || console.cwxForm.IsDisposed)
            {
                try
                {
                    console.cwxForm = new CWX(console);
                }
                catch
                {
                    return parser.Error1;
                }
            }

            if (s.Length == parser.nSet)
            {
                cws = Convert.ToInt32(s);
                cws = Math.Max(1, cws);
                cws = Math.Min(99, cws);
                console.cwxForm.WPM = cws;
                return "";

            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.cwxForm.WPM);
            }
            else
                return parser.Error1;
        }

        //Sends text to CWX for conversion to Morse
        public string ZZKY(string s)
        {
            // Make sure we have an instance of the form
            if (console.cwxForm == null || console.cwxForm.IsDisposed)
            {
                try
                {
                    console.cwxForm = new CWX(console);
                }
                catch
                {
                    return parser.Error1;
                }
            }


            if (s.Length == parser.nSet)
            {

                // Make sure we are in a cw mode.
                switch (console.RX1DSPMode)
                {
                    case DSPMode.AM:
                    case DSPMode.DRM:
                    case DSPMode.DSB:
                    case DSPMode.FM:
                    case DSPMode.SAM:
                    case DSPMode.SPEC:
                    case DSPMode.LSB:
                    case DSPMode.USB:
                    case DSPMode.DIGU: // ke9ns add
                    case DSPMode.DIGL: // ke9ns add
                        if (console.RX1Band >= Band.B160M && console.RX1Band <= Band.B40M)
                            console.RX1DSPMode = DSPMode.CWL;
                        else
                            console.RX1DSPMode = DSPMode.CWU;
                        break;
                    case DSPMode.CWL:
                    case DSPMode.CWU:
                        break;
                    default:
                        console.RX1DSPMode = DSPMode.CWU;
                        break;
                }


                string trms = "";
                byte[] msg;
                string x = s.Trim();

                if (x.Length == 0)
                    trms = " ";
                else
                    trms = s.TrimEnd();

                if (trms.Length > 1)
                {
                    msg = AE.GetBytes(trms);
                    return console.cwxForm.RemoteMessage(msg);
                }
                else
                {
                    char ss = Convert.ToChar(trms);
                    return console.cwxForm.RemoteMessage(ss);
                }
            }
            else if (s.Length == parser.nGet)
            {
                int ch = console.cwxForm.Characters2Send;
                if (ch > 0 && ch < 72)
                    return "0";
                else if (ch >= 72)
                    return "1";
                else if (ch == 0)
                    return "2";
                else
                    return parser.Error1;
            }
            else
                return parser.Error1;
        }

        //Sets or reads the RX0Gain level
        public string ZZLA(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(100, n);

            if (s.Length == parser.nSet)
            {
                console.RX0Gain = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.RX0Gain);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the RX0 (Main RX) stereo balance
        public string ZZLB(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(100, n);

            if (s.Length == parser.nSet)
            {
                console.PanMainRX = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.PanMainRX);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the RX1 (SubRX) Gain level
        public string ZZLC(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(100, n);

            if (s.Length == parser.nSet)
            {
                console.RX1Gain = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.RX1Gain);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the RX1 (Sub RX) stereo balance
        public string ZZLD(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(100, n);

            if (s.Length == parser.nSet)
            {
                console.PanSubRX = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.PanSubRX);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the RX2 Gain level
        public string ZZLE(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                int n = 0;

                if (s != null && s != "")
                    n = Convert.ToInt32(s);
                n = Math.Max(0, n);
                n = Math.Min(100, n);

                if (s.Length == parser.nSet)
                {
                    console.RX2Gain = n;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros((int)console.RX2Gain);
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the RX2 stereo balance
        public string ZZLF(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                int n = 0;

                if (s != null && s != "")
                    n = Convert.ToInt32(s);
                n = Math.Max(0, n);
                n = Math.Min(100, n);

                if (s.Length == parser.nSet)
                {
                    console.RX2Pan = n;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros((int)console.RX2Pan);
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the AutoMute RX1 on VFOB TX checkbox
        public string ZZLG(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.setupForm.AutoMuteRX1onVFOBTX = false;
                    else
                        console.setupForm.AutoMuteRX1onVFOBTX = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool ans = console.setupForm.AutoMuteRX1onVFOBTX;
                    if (ans)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the AutoMute RX2 on VFOA TX checkbox
        public string ZZLH(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.setupForm.AutoMuteRX2onVFOATX = false;
                    else
                        console.setupForm.AutoMuteRX2onVFOATX = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool ans = console.setupForm.AutoMuteRX2onVFOATX;
                    if (ans)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        // Sets or reads the RX1 MUT button on/off status
        public string ZZMA(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.MUT = false;
                else if (s == "1")
                    console.MUT = true;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.MUT;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

        // Sets or reads the RX2 MUT button on/off status
        public string ZZMB(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (s == "0")
                        console.MUT2 = false;
                    else if (s == "1")
                        console.MUT2 = true;

                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool retval = console.MUT2;
                    if (retval)
                        return "1";
                    else
                        return "0";
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }




        // Sets or reads the SDR-1000 DSP mode
        public string ZZMD(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (Convert.ToInt32(s) >= 0 && Convert.ToInt32(s) <= 11)
                {
                    String2Mode(s);
                    return "";
                }
                else
                    return parser.Error1;
            }
            else if (s.Length == parser.nGet)
            {
                return Mode2String(console.RX1DSPMode);
            }
            else
            {
                return parser.Error1;
            }
        }

        public string ZZME(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nGet)
                {
                    return Mode2String(console.RX2DSPMode);
                }
                else if (s.Length == parser.nSet && s != "08")
                {
                    switch (s)
                    {
                        case "00":
                            console.RX2DSPMode = DSPMode.LSB;
                            break;
                        case "01":
                            console.RX2DSPMode = DSPMode.USB;
                            break;
                        case "02":
                            console.RX2DSPMode = DSPMode.DSB;
                            break;
                        case "03":
                            console.RX2DSPMode = DSPMode.CWL;
                            break;
                        case "04":
                            console.RX2DSPMode = DSPMode.CWU;
                            break;
                        case "05":
                            console.RX2DSPMode = DSPMode.FM;
                            break;
                        case "06":
                            console.RX2DSPMode = DSPMode.AM;
                            break;
                        case "07":
                            console.RX2DSPMode = DSPMode.DIGU;
                            break;
                        case "09":
                            console.RX2DSPMode = DSPMode.DIGL;
                            break;
                        case "10":
                            console.RX2DSPMode = DSPMode.SAM;
                            break;
                        case "11":
                            console.RX2DSPMode = DSPMode.DRM;
                            break;
                    }
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the Mic gain control
        public string ZZMG(string s)
        {
            int n;
            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Min(70, n);
                n = Math.Max(0, n);
                s = AddLeadingZeros(n);
            }

            if (s.Length == parser.nSet)
            {
                console.CATMIC = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.CATMIC);
            }
            else
                return parser.Error1;
        }

        //Returns a list of modes with index
        public string ZZML()
        {
            string modeList = "";
            string thisMode = "";

            try
            {
                var values = (DSPMode[])Enum.GetValues(typeof(DSPMode));
                int n = 0;
                for (n = 0; values[n] != DSPMode.LAST; n++)
                {
                    if (values[n] != DSPMode.FIRST && values[n] != DSPMode.LAST)
                    {
                        if ((int)values[n] <= 9)
                            thisMode = values[n].ToString() + "0" + (int)values[n] + ":";
                        else
                            thisMode = values[n].ToString() + (int)values[n] + ":";
                    }
                    modeList += thisMode.PadLeft(7, ' ');
                }

                return modeList.Remove(modeList.Length - 1);
            }
            catch
            {
                parser.Verbose_Error_Code = 4;
                return parser.Error1;
            }
        }

        //Reads the DSP filter presets for filter index (s)
        //Returns 180 character length word for 12 filters x 15 characters each.
        //Format is name high low: ZZMN 5.0k 5150 -160...
        public string ZZMN(string s)
        {
            if (s.Length == parser.nGet)
            {
                return console.GetFilterPresets(Int32.Parse(s));
            }
            else
                return parser.Error1;
        }

        //Sets or reads the Monitor (MON) button status
        public string ZZMO(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.chkMON.CheckedChanged -= console.chkMON_CheckedChanged; // ke9ns mod turn eventoff

                if (s == "0") // ke9ns mod: .146 
                {
                    //console.MON = false;
                    console.chkMON.Checked = false;
                    console.chkMON.Text = "MON";
                    Audio.MON_PRE = 0;            // turn off MON_PRE
                }
                else if (s == "1") // ke9ns mod: .146 set MON to MONpr (this is standard MON mode, expect now you can use this with AM/FM modes)
                {
                    //	console.MON = true;
                    console.chkMON.Checked = true;
                    console.chkMON.Text = "MONpr";
                    Audio.MON_PRE = 1;            // turn ON MON_PRE
                }
                else if (s == "2") // ke9ns add: .146 set MON to MONps (flex 3000/5000 only)
                {
                    //	console.MON = true;
                    console.chkMON.Checked = true;
                    console.chkMON.Text = "MONps";
                    Audio.MON_PRE = 0;            // turn OFF MON_PRE
                }

                console.chkMON.CheckedChanged += console.chkMON_CheckedChanged; //ke9ns mod turn eventoff

                Audio.MON = console.chkMON.Checked; // ke9ns tell audio routine MON is on or OFF

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.MON; // ke9ns: get console.chkMON status
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;
        }

        // Sets or reads the RX meter mode
        public string ZZMR(string s)
        {
            int m = -1;
            if (s != "")
                m = Convert.ToInt32(s);

            if (s.Length == parser.nSet &&
                (m > (int)MeterRXMode.FIRST && m < (int)MeterRXMode.LAST))
            {
                String2RXMeter(m);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return RXMeter2String();
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the MultiRX Swap checkbox
        public string ZZMS(string s)
        {
            if (s.Length == parser.nSet && (s == "1" || s == "0"))
            {
                if (s == "1")
                    console.CATPanSwap = "1";
                else if (s == "0")
                    console.CATPanSwap = "0";
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATPanSwap;
            }
            else
                return parser.Error1;
        }

        // Sets or reads the TX meter mode
        public string ZZMT(string s)
        {
            int m = -1;
            if (s != "")
                m = Convert.ToInt32(s);

            if (s.Length == parser.nSet &&
                (m > (int)MeterTXMode.FIRST && m < (int)MeterTXMode.LAST))
            {
                String2TXMeter(m);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return TXMeter2String().PadLeft(2, '0');        //Added padleft 4/2/2007 BT
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the MultiRX button status
        public string ZZMU(string s)
        {
            if (s.Length == parser.nSet && (s == "1" || s == "0"))
            {
                if (s == "1")
                    console.CATMultRX = "1";
                else if (s == "0")
                    console.CATMultRX = "0";
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATMultRX;
            }
            else
                return parser.Error1;
        }

        //Returns the count of memory records
        //read only
        public string ZZMV()
        {
            try
            {
                return AddLeadingZeros(console.MemoryList.List.Count);
            }
            catch
            {
                parser.Verbose_Error_Code = 4;
                return parser.Error1;
            }
        }

        //Deletes a memory channel
        public string ZZMW(string s)
        {
            try
            {
                MemoryRecord rec = GetChannelRecord(s);
                if (rec.Comments.StartsWith(s))
                    console.MemoryList.List.Remove(rec);
                return "";
            }
            catch
            {
                parser.Verbose_Error_Code = 4;
                return parser.Error1;
            }

        }

        //Restores memory channel n
        public string ZZMX(string s)
        {
            try
            {
                int ndx = GetIndex(s);
                if (ndx >= 0)
                {
                    console.changeComboFMMemory(ndx);
                    return "";
                }
                else
                {
                    parser.Verbose_Error_Code = 9;
                    return parser.Error1;
                }
            }
            catch
            {
                parser.Verbose_Error_Code = 4;
                return parser.Error1;
            }
        }

        //Saves the current radio configuration to a new memory channel
        public string ZZMY()
        {
            try
            {
                int nextN = GetNextChannelNumber();
                int oldAns = parser.nAns;
                parser.nAns = 3;
                string newCh = AddLeadingZeros(nextN);

                console.MemoryList.List.Add(new MemoryRecord("", console.VFOAFreq, "", console.RX1DSPMode, true, console.TuneStepList[console.TuneStepIndex].Name,
                console.CurrentFMTXMode, console.FMTXOffsetMHz, console.dsp.GetDSPTX(0).CTCSSFlag, console.dsp.GetDSPTX(0).CTCSSFreqHz, console.PWR,
                (int)console.dsp.GetDSPTX(0).TXFMDeviation, console.VFOSplit, console.TXFreq, console.RX1Filter, console.RX1FilterLow,
                console.RX1FilterHigh, newCh + ":", console.dsp.GetDSPRX(0, 0).RXAGCMode, console.RF,

                 DateTime.Now, false, 0, false, false, false, 0 // ke9ns add for freq scheduler

                ));

                parser.nAns = oldAns;
                return "";
            }
            catch
            {
                parser.Verbose_Error_Code = 4;
                return parser.Error1;
            }
        }

        //Saves the radio configuration to a specific channel number (edit)
        public string ZZMZ(string s)
        {
            if (GetIndex(s) >= 0)
            {
                try
                {
                    MemoryRecord oldrec = GetChannelRecord(s);
                    MemoryRecord newrec = new MemoryRecord();
                    newrec.Group = oldrec.Group;
                    newrec.RXFreq = console.VFOAFreq;
                    newrec.Name = oldrec.Name;
                    newrec.DSPMode = console.RX1DSPMode;
                    newrec.Scan = oldrec.Scan;
                    newrec.TuneStep = console.TuneStepList[console.TuneStepIndex].Name;
                    newrec.RPTR = console.CurrentFMTXMode;
                    newrec.RPTROffset = console.FMTXOffsetMHz;
                    newrec.CTCSSOn = console.dsp.GetDSPTX(0).CTCSSFlag;
                    newrec.CTCSSFreq = console.dsp.GetDSPTX(0).CTCSSFreqHz;
                    newrec.Power = console.PWR;
                    newrec.Deviation = (int)console.dsp.GetDSPTX(0).TXFMDeviation;
                    newrec.Split = console.VFOSplit;
                    newrec.TXFreq = console.TXFreq;
                    newrec.RXFilter = console.RX1Filter;
                    newrec.RXFilterLow = console.RX1FilterLow;
                    newrec.RXFilterHigh = console.RX1FilterHigh;
                    newrec.Comments = oldrec.Comments;
                    newrec.AGCMode = console.dsp.GetDSPRX(0, 0).RXAGCMode;
                    newrec.AGCT = console.RF;
                    console.MemoryList.List.Remove(oldrec);
                    console.MemoryList.List.Add(newrec);
                    return "";
                }
                catch
                {
                    parser.Verbose_Error_Code = 4;
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 9;
                return parser.Error1;
            }
        }

        #endregion Extended CAT Methods ZZG-ZZM

        #region Extended CAT Methods ZZN-ZZQ

        //Sets or reads Noise Blanker 2 status
        public string ZZNA(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATNB1 = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATNB1.ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the Noise Blanker 2 status
        public string ZZNB(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATNB2 = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATNB2.ToString();
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the RX2 NB status
        public string ZZNC(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.CATRX2NB1 = Convert.ToInt32(s);
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.CATRX2NB1.ToString();
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the RX2 NB2 status
        public string ZZND(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.CATRX2NB2 = Convert.ToInt32(s);
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.CATRX2NB2.ToString();
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }



        // Sets or reads the Noise Blanker 1 threshold
        public string ZZNL(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.CATNB1Threshold = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.CATNB1Threshold);
            }
            else
            {
                return parser.Error1;
            }

        }

        // Sets or reads the Noise Blanker 2 threshold
        public string ZZNM(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.CATNB2Threshold = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.CATNB2Threshold);
            }
            else
            {
                return parser.Error1;
            }

        }


        // Sets or reads the Noise Reduction status
        //		public string ZZNR()
        //		{
        //			int nr = console.CATNR;
        //			return nr.ToString();
        //		}

        public string ZZNR(string s)
        {
            int sx = 0;

            if (s != "")
                sx = Convert.ToInt32(s);

            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATNR = sx;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATNR.ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the ANF button status
        public string ZZNT(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATANF = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATANF.ToString();
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the RX1 antenna
        public string ZZOA(string s)
        {
            int n = 0;
            if (s.Length > 0)
                n = Int32.Parse(s);

            if (console.CurrentModel == Model.FLEX5000)
            {
                if (console.fwcAntForm != null)
                {
                    if (s.Length == parser.nSet && (n >= 0 && n <= 4))
                    {
                        console.fwcAntForm.RX1Ant = String2FWCAntenna(s);
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                        return FWCAntenna2String(console.fwcAntForm.RX1Ant);
                    else
                        return parser.Error1;
                }
                else
                    return parser.Error1;
            }
            else if (console.CurrentModel == Model.FLEX1500)
            {
                if (console.hidAntForm != null)
                {
                    if (s.Length == parser.nSet && (n >= 0 && n <= 3))
                    {
                        console.hidAntForm.RXAnt = String2HIDAntenna(s);
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                        return HIDAntenna2String(console.hidAntForm.RXAnt);
                    else
                        return parser.Error1;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the RX2 antenna (if RX2 installed)
        public string ZZOB(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                int n = 0;
                if (s.Length > 0)
                    n = Int32.Parse(s);

                if (console.fwcAntForm != null)
                {
                    if (s.Length == parser.nSet && ((n >= 0 && n <= 1) || (n >= 5 && n <= 6)))
                    {
                        console.fwcAntForm.RX2Ant = String2FWCAntenna(s);
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                        return FWCAntenna2String(console.fwcAntForm.RX2Ant);
                    else
                        return parser.Error1;
                }
                else
                {
                    parser.Verbose_Error_Code = 8;
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the TX antenna
        public string ZZOC(string s)
        {
            int n = 0;
            if (s.Length > 0)
                n = Int32.Parse(s);
            if (console.CurrentModel == Model.FLEX5000)
            {

                if (console.fwcAntForm != null)
                {
                    if (s.Length == parser.nSet && (n >= 1 && n <= 3))
                    {
                        console.fwcAntForm.TXAnt = String2FWCAntenna(s);
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                        return FWCAntenna2String(console.fwcAntForm.TXAnt);
                    else
                        return parser.Error1;
                }
                else
                {
                    parser.Verbose_Error_Code = 8;
                    return parser.Error1;
                }
            }
            else if (console.CurrentModel == Model.FLEX1500)
            {
                if (console.hidAntForm != null)
                {
                    if (s.Length == parser.nSet && (n >= 0 && n <= 1))
                    {
                        console.hidAntForm.TXAnt = String2HIDAntenna(s);
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                        return HIDAntenna2String(console.hidAntForm.TXAnt);
                    else
                        return parser.Error1;
                }
                else
                {
                    parser.Verbose_Error_Code = 8;
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the current Antenna Mode
        public string ZZOD(string s)
        {
            int n = 0;
            if (s.Length > 0)
                n = Int32.Parse(s);

            if (console.CurrentModel == Model.FLEX5000)
            {

                if (s.Length == parser.nSet && (n == 0 || n == 1))
                {
                    console.fwcAntForm.CurrentAntMode = String2AntMode(s);
                    return "";
                }
                else if (s.Length == parser.nGet)
                    return AntMode2String(console.fwcAntForm.CurrentAntMode);
                else
                    return parser.Error1;
            }
            else if (console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (n == 0 || n == 1))
                {
                    console.hidAntForm.CurrentAntMode = String2AntMode(s);
                    return "";
                }
                else if (s.Length == parser.nGet)
                    return AntMode2String(console.hidAntForm.CurrentAntMode);
                else
                    return parser.Error1;

            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }

        //Sets or reads the RX1 External Antenna checkbox
        public string ZZOE(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.fwcAntForm.RX1Loop = false;
                    else
                        console.fwcAntForm.RX1Loop = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool ans = console.fwcAntForm.RX1Loop;
                    if (ans)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the TX relay RCA jack
        public string ZZOF(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                string ans = "";
                bool rly1 = false;
                bool rly2 = false;
                bool rly3 = false;

                if (s.Length == parser.nSet)
                {
                    if (s.Substring(0, 1) == "1")
                        console.fwcAntForm.RCATX1 = true;
                    else
                        console.fwcAntForm.RCATX1 = false;

                    if (s.Substring(1, 1) == "1")
                        console.fwcAntForm.RCATX2 = true;
                    else
                        console.fwcAntForm.RCATX2 = false;

                    if (s.Substring(2, 1) == "1")
                        console.fwcAntForm.RCATX3 = true;
                    else
                        console.fwcAntForm.RCATX3 = false;

                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    rly1 = console.fwcAntForm.RCATX1;
                    rly2 = console.fwcAntForm.RCATX2;
                    rly3 = console.fwcAntForm.RCATX3;

                    if (rly1)
                        ans += "1";
                    else
                        ans += "0";
                    if (rly2)
                        ans += "1";
                    else
                        ans += "0";
                    if (rly3)
                        ans += "1";
                    else
                        ans += "0";

                    return ans;
                }
                else
                    return parser.Error1;
            }
            else if (console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet)
                {
                    if (s.Substring(0, 1) == "1")
                        console.hidAntForm.PTTOut = true;
                    else
                        console.hidAntForm.PTTOut = false;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool rly1 = console.hidAntForm.PTTOut;
                    if (rly1)
                        return "100";
                    else
                        return "000";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        //Sets or reads the TX Relay Delay enables
        public string ZZOG(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                string ans = "";
                bool rly1 = false;
                bool rly2 = false;
                bool rly3 = false;

                if (s.Length == parser.nSet)
                {
                    if (s.Substring(0, 1) == "1")
                        console.fwcAntForm.TX1DelayEnable = true;
                    else
                        console.fwcAntForm.TX1DelayEnable = false;

                    if (s.Substring(1, 1) == "1")
                        console.fwcAntForm.TX2DelayEnable = true;
                    else
                        console.fwcAntForm.TX2DelayEnable = false;

                    if (s.Substring(2, 1) == "1")
                        console.fwcAntForm.TX3DelayEnable = true;
                    else
                        console.fwcAntForm.TX3DelayEnable = false;

                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    rly1 = console.fwcAntForm.TX1DelayEnable;
                    rly2 = console.fwcAntForm.TX2DelayEnable;
                    rly3 = console.fwcAntForm.TX3DelayEnable;

                    if (rly1)
                        ans += "1";
                    else
                        ans += "0";
                    if (rly2)
                        ans += "1";
                    else
                        ans += "0";
                    if (rly3)
                        ans += "1";
                    else
                        ans += "0";

                    return ans;
                }
                else
                    return parser.Error1;
            }
            else if (console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet)
                {
                    if (s.Substring(0, 1) == "1")
                        console.hidAntForm.TX1DelayEnable = true;
                    else
                        console.hidAntForm.TX1DelayEnable = false;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool rly1 = console.hidAntForm.TX1DelayEnable;
                    if (rly1)
                        return "100";
                    else
                        return "000";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }

        //Sets or reads the TX Relay Delays
        public string ZZOH(string s)
        {
            string ans = "";
            uint delay = 0;
            string relay = s.Substring(0, 1);

            if (console.CurrentModel == Model.FLEX5000)
            {

                if (s.Length == parser.nSet)
                {
                    delay = uint.Parse(s.Substring(1, 4));

                    if (relay == "1")
                        console.fwcAntForm.TX1Delay = delay;
                    else
                    if (relay == "2")
                        console.fwcAntForm.TX2Delay = delay;
                    else
                    if (relay == "3")
                        console.fwcAntForm.TX3Delay = delay;

                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (relay == "1")
                        delay = console.fwcAntForm.TX1Delay;
                    else
                    if (relay == "2")
                        delay = console.fwcAntForm.TX2Delay;
                    else
                    if (relay == "3")
                        delay = console.fwcAntForm.TX3Delay;

                    ans = AddLeadingZeros((int)delay);
                    return " " + ans.Substring(1, 4);
                }
                else
                    return parser.Error1;
            }
            if (console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet)
                {
                    delay = uint.Parse(s.Substring(1, 4));
                    console.hidAntForm.TX1Delay = delay;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    delay = console.hidAntForm.TX1Delay;
                    ans = AddLeadingZeros((int)delay);
                    return " " + ans.Substring(1, 4);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        public string ZZOJ(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.fwcAntForm.AntLock = false;
                    else
                        console.fwcAntForm.AntLock = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool Lock = console.fwcAntForm.AntLock;
                    if (Lock)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            if (console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.hidAntForm.AntLock = false;
                    else
                        console.hidAntForm.AntLock = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    bool Lock = console.hidAntForm.AntLock;
                    if (Lock)
                        return "1";
                    else
                        return "0";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the DigL Click Tune Offset
        public string ZZOL(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(9999, n);

            if (s.Length == parser.nSet)
            {
                console.setupForm.DigL_CT_Offset = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.setupForm.DigL_CT_Offset);
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the current repeater offset direction
        public string ZZOS(string s)
        {
            if (s.Length == parser.nSet)
            {
                String2OffsetDirection(s);
                return "";
            }
            else if (s.Length == parser.nGet)
                return OffsetDirection2String();
            else
                return parser.Error1;
        }

        //Sets for reads the repeater frequency offset
        //need to resolve the negative offset question.
        public string ZZOT(string s)
        {
            if (s.Length == parser.nSet)
            {
                s = s.Insert(3, ".");
                double n = double.Parse(s);
                console.FMTXOffsetMHz = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                int n = Convert.ToInt32(Math.Abs(console.FMTXOffsetMHz * 10e5));
                return AddLeadingZeros(n);
            }
            else
                return parser.Error1;
        }

        // Sets or reads the DigU Click Tune Offset
        public string ZZOU(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(9999, n);

            if (s.Length == parser.nSet)
            {
                console.setupForm.DigU_CT_Offset = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.setupForm.DigU_CT_Offset);
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the console ATU button
        public string ZZOV(string s)
        {
            if (FWCEEPROM.ATUOK)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "1")
                    {
                        if (console.PowerOn)
                        {
                            console.CATATU = true;
                            return "";
                        }
                        else
                        {
                            parser.Verbose_Error_Code = 10;
                            return parser.Error1;
                        }
                    }
                    else
                    {
                        ZZOW("1");
                        return "";
                    }
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CATATU)
                        return "1";
                    else
                        return "0";
                }
                else
                {
                    parser.Verbose_Error_Code = 6;
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the console ATU Bypass button
        public string ZZOW(string s)
        {
            if (FWCEEPROM.ATUOK)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "1")
                    {
                        console.CATBYP = true;
                        return "";
                    }
                    else
                    {
                        if (console.PowerOn)
                        {
                            ZZOV("1");
                            return "";
                        }
                        else
                        {
                            parser.Verbose_Error_Code = 10;
                            return parser.Error1;
                        }
                    }
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CATBYP)
                        return "1";
                    else
                        return "0";
                }
                else
                {
                    parser.Verbose_Error_Code = 6;
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the Preamp thumbwheel
        public string ZZPA(string s)
        {
            int n = 0;
            if (s != "")
                n = Convert.ToInt32(s);

            PreampMode e_mode = console.CATPreamp;

            if (s.Length == parser.nSet)
            {
                if (console.CurrentModel == Model.SDR1000)
                {
                    if ((n > (int)PreampMode.FIRST && n < (int)PreampMode.LAST))
                    {
                        switch (s)
                        {
                            case "0":
                                console.CATPreamp = PreampMode.OFF;
                                break;
                            case "1":
                                console.CATPreamp = PreampMode.LOW;
                                break;
                            case "2":
                                console.CATPreamp = PreampMode.MED;
                                break;
                            case "3":
                                console.CATPreamp = PreampMode.HIGH;
                                break;
                        }
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else if (console.CurrentModel == Model.FLEX5000)
                {
                    if (n == 0 || n == 1)
                    {
                        switch (s)
                        {
                            case "0":
                                console.CATPreamp = PreampMode.OFF;
                                break;
                            case "1":
                                console.CATPreamp = PreampMode.HIGH;
                                break;
                        }
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else if (console.CurrentModel == Model.FLEX3000)
                {
                    if (n <= 3 && n >= 0)
                    {
                        double freq = console.VFOAFreq;
                        if (FWCEEPROM.TRXRev >> 8 < 6)    //before Rev G.
                        {
                            switch (s)
                            {
                                case "0":
                                    console.CATPreamp = PreampMode.OFF;
                                    break;
                                case "1":
                                    console.CATPreamp = PreampMode.LOW;
                                    break;
                                default:
                                    break;
                            }
                            if (freq > 2.0)
                            {
                                switch (s)
                                {
                                    case "2":
                                        console.CATPreamp = PreampMode.MED;
                                        break;
                                    case "3":
                                        console.CATPreamp = PreampMode.HIGH;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else  //Rev G+
                        {
                            switch (s)
                            {
                                case "0":
                                    console.CATPreamp = PreampMode.OFF;
                                    break;
                                case "1":
                                    console.CATPreamp = PreampMode.LOW;
                                    break;
                                default:
                                    break;
                            }
                            if ((freq > 7.000 && freq < 13.000) && s == "2")
                            {
                                console.CATPreamp = PreampMode.MED;
                            }
                            else if (freq >= 13.000 && (s == "2" || s == "3"))
                                switch (s)
                                {
                                    case "2":
                                        console.CATPreamp = PreampMode.MED;
                                        break;
                                    case "3":
                                        console.CATPreamp = PreampMode.HIGH;
                                        break;
                                }

                        }
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else if (console.CurrentModel == Model.FLEX1500)
                {
                    if (n <= 4 && n >= 0)
                    {
                        switch (s)
                        {
                            case "0":
                                console.RX1PreampMode = PreampMode.OFF;
                                break;
                            case "1":
                                console.RX1PreampMode = PreampMode.LOW;
                                break;
                            case "2":
                                console.RX1PreampMode = PreampMode.MED;
                                break;
                            case "3":
                                console.RX1PreampMode = PreampMode.HIGH;
                                break;
                            case "4":
                                console.RX1PreampMode = PreampMode.LAST;
                                break;
                        }
                        return "";
                    }
                    else
                        return parser.Error1;
                }
                else
                    return parser.Error1;
            }
            else if (s.Length == parser.nGet)
            {
                int mode = (int)console.CATPreamp;
                string cat_mode = "";
                switch (console.CurrentModel)
                {
                    case Model.SDR1000:
                        cat_mode = mode.ToString();
                        break;
                    case Model.FLEX3000:
                        if (mode == 0)
                            cat_mode = "0";
                        else if (mode == 1)
                            cat_mode = "1";
                        else if (mode == 2)
                            cat_mode = "2";
                        else if (mode == 3)
                            cat_mode = "3";
                        else
                            return parser.Error1;
                        break;
                    case Model.FLEX5000:
                        if (mode > 0)
                            cat_mode = "1";
                        else
                            cat_mode = "0";
                        break;
                    case Model.FLEX1500:
                        switch ((FLEX1500PreampMode)e_mode)
                        {
                            case FLEX1500PreampMode.MINUS_10:
                                cat_mode = "0";
                                break;
                            case FLEX1500PreampMode.ZERO:
                                cat_mode = "1";
                                break;
                            case FLEX1500PreampMode.PLUS_10:
                                cat_mode = "2";
                                break;
                            case FLEX1500PreampMode.PLUS_20:
                                cat_mode = "3";
                                break;
                            case FLEX1500PreampMode.PLUS_30:
                                cat_mode = "4";
                                break;
                        }
                        break;
                }
                return cat_mode;
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the RX2 Preamp button
        public string ZZPB(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nGet)
                {
                    if (console.RX2PreampMode == PreampMode.OFF)
                        return "0";
                    else
                        return "1";
                }
                else if (s.Length == parser.nSet)
                {
                    if (s == "1")
                        console.chkRX2Preamp.Checked = true; //.300
                    //  console.RX2PreampMode = PreampMode.HIGH;
                    else
                        console.chkRX2Preamp.Checked = false; //.300
                    //   console.RX2PreampMode = PreampMode.OFF;
                    return "";
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }

        //Sets or reads the Drive level
        public string ZZPC(string s)
        {
            int pwr = 0;

            if (s.Length == parser.nSet)
            {
                pwr = Convert.ToInt32(s);
                if (!console.TUN)
                {
                    console.PWR = pwr;
                }
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.PWR);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Centers the Display Pan scroll
        public string ZZPD()
        {
            console.CATDispCenter = "1";
            return "";
        }

        // Sets or reads the Display Zoom control
        public string ZZPE(string s) // RX1 .221
        {
            int level = 0;

            if (s.Length == parser.nSet)
            {
                level = Convert.ToInt32(s);
                level = Math.Max(0, level);			// lower bound
                level = Math.Min(1000, level);		    // upper bound
                console.Pan = level;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.Pan);
            }
            else
            {
                return parser.Error1;
            }

        } //ZZPE rx1

        // Sets or reads the Display Zoom control add .221 RX2
        public string ZZPF(string s)
        {
            int level = 0;

            if (s.Length == parser.nSet)
            {
                level = Convert.ToInt32(s);
                level = Math.Max(0, level);			// lower bound
                level = Math.Min(1000, level);		    // upper bound
                console.Pan2 = level;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.Pan2);
            }
            else
            {
                return parser.Error1;
            }

        } //ZZPF rX2

        //		//Sets or reads the Speech Compressor button status
        //		public string ZZPK(string s)
        //		{
        //			if(s.Length == parser.nSet)
        //			{
        //				if(s == "0")
        //					console.COMP = false;
        //				else if(s == "1")
        //					console.COMP = true;
        //				return "";
        //			}
        //			else if(s.Length == parser.nGet)
        //			{
        //				bool comp = console.COMP;
        //				if(comp)
        //					return "1";
        //				else
        //					return "0";
        //			}
        //			else
        //			{
        //				return "";
        //			}
        //		}
        //
        //		// Sets or reads the Speech Compressor threshold
        //		public string ZZPL(string s)
        //		{
        //			if(s.Length == parser.nSet)
        //			{
        //				console.SetupForm.CATCompThreshold = Convert.ToInt32(s);
        //				return "";
        //			}
        //			else if(s.Length == parser.nGet)
        //			{
        //				return AddLeadingZeros(console.SetupForm.CATCompThreshold);
        //			}
        //			else
        //			{
        //				return parser.Error1;
        //			}
        //
        //		}

        // Sets or reads the Speech Compressor threshold
        //		public string ZZPL(string s)
        //		{
        //			if(s.Length == parser.nSet)
        //			{
        //				console.SetupForm.CATCompThreshold = Convert.ToInt32(s);
        //				return "";
        //			}
        //			else if(s.Length == parser.nGet)
        //			{
        //				return AddLeadingZeros(console.SetupForm.CATCompThreshold);
        //			}
        //			else
        //			{
        //				return parser.Error1;
        //			}
        //
        //		}

        //Sets or reads the Display Peak button status
        public string ZZPO(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.CATDispPeak = s;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATDispPeak;
            }
            else
                return parser.Error1;
        }

        //Sets or reads the Power button status
        public string ZZPS(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.PowerOn = false;
                else if (s == "1")
                    console.PowerOn = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool pwr = console.PowerOn;
                if (pwr)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the Display Zoom control (mod RX1 .221)
        public string ZZPY(string s)
        {
            int level = 0;

            if (s.Length == parser.nSet)
            {
                level = Convert.ToInt32(s);
                level = Math.Max(10, level);			// lower bound
                level = Math.Min(240, level);		    // upper bound
                console.Zoom = level;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.Zoom);
            }
            else
            {
                return parser.Error1;
            }

        } // ZZPY RX1

        public string ZZPW(string s) // .221 add RX2 Zoom Slider
        {
            int level = 0;

            if (s.Length == parser.nSet)
            {
                level = Convert.ToInt32(s);
                level = Math.Max(10, level);			// lower bound
                level = Math.Min(240, level);		    // upper bound
                console.Zoom2 = level;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.Zoom2);
            }
            else
            {
                return parser.Error1;
            }

        } //ZZPW RX2

        //Sets the Display Zoom buttons  RX1 .221
        public string ZZPZ(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.CATDispZoom = s;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATDispZoom;
            }
            else
                return parser.Error1;

        } // ZZPZ (RX1)

        public string ZZPX(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.CATDispZoom2 = s;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATDispZoom2;
            }
            else
                return parser.Error1;

        } // ZZPX (RX3)




        // Reads the Quick Memory Save value
        public string ZZQM()
        {
            return StrVFOFreq("C");
        }

        // Recalls Memory Quick Save
        public string ZZQR()
        {
            console.CATMemoryQR();
            return "";
        }

        //Saves Quick Memory value
        public string ZZQS()
        {
            console.CATMemoryQS();
            return "";
        }


        #endregion Extended CAT Methods ZZN-ZZQ

        #region Extended CAT Methods ZZR-ZZZ

        // Sets or reads the RTTY Offset Enable VFO A checkbox
        public string ZZRA(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.setupForm.RttyOffsetEnabledA = false;
                else if (s == "1")
                    console.setupForm.RttyOffsetEnabledA = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool ans = console.setupForm.RttyOffsetEnabledA;
                if (ans)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Sets or reads the RTTY Offset Enable VFO B checkbox
        public string ZZRB(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.setupForm.RttyOffsetEnabledB = false;
                else if (s == "1")
                    console.setupForm.RttyOffsetEnabledB = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool ans = console.setupForm.RttyOffsetEnabledB;
                if (ans)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        //Clears the RIT frequency
        public string ZZRC()
        {
            console.RITValue = 0;
            return "";
        }

        //Decrements RIT
        public string ZZRD(string s)
        {
            if (s.Length == parser.nSet)
            {
                return ZZRF(s);
            }
            else if (s.Length == parser.nGet && console.RITOn)
            {
                switch (console.RX1DSPMode)
                {
                    case DSPMode.CWL:
                    case DSPMode.CWU:
                        console.RITValue -= 10;
                        break;
                    case DSPMode.LSB:
                    case DSPMode.USB:
                        console.RITValue -= 50;
                        break;
                }
                return "";
            }
            else
                return parser.Error1;
        }

        // Sets or reads the RIT frequency value
        public string ZZRF(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-99999, n);
                n = Math.Min(99999, n);
            }

            if (s.Length == parser.nSet)
            {
                console.RITValue = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.RITValue;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }


        //Sets or reads the RTTY DIGH offset frequency ud counter
        public string ZZRH(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-3000, n);
                n = Math.Min(3000, n);
            }

            if (s.Length == parser.nSet)
            {
                console.setupForm.RttyOffsetHigh = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.RttyOffsetHigh;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the RTTY DIGL offset frequency ud counter
        public string ZZRL(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-3000, n);
                n = Math.Min(3000, n);
            }

            if (s.Length == parser.nSet)
            {
                console.setupForm.RttyOffsetLow = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.RttyOffsetLow;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }

        }

        // Reads the Console RX meter
        public string ZZRM(string s)
        {
            string output = parser.Error1;
            if (!console.MOX)
            {
                switch (s)
                {
                    case "0":
                        output = console.CATReadSigStrength().PadLeft(20);
                        break;
                    case "1":
                        output = console.CATReadAvgStrength().PadLeft(20);
                        break;
                    case "2":
                        output = console.CATReadADC_L().PadLeft(20);
                        break;
                    case "3":
                        output = console.CATReadADC_R().PadLeft(20);
                        break;
                }
            }
            else
            {
                switch (s)
                {
                    case "4":
                        output = console.CATReadALC().PadLeft(20);
                        break;
                    case "5":
                        output = console.CATReadFwdPwr().PadLeft(20);
                        break;
                    case "6":
                        output = parser.Error1;
                        break;
                    case "7":
                        output = console.CATReadRevPwr().PadLeft(20);
                        break;
                    case "8":
                        output = console.CATReadSWR().PadLeft(20);
                        break;
                }
            }
            return output;
        }
        //Sets or reads the RX2 button status
        public string ZZRS(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nSet)
                {
                    if (s == "0")
                        console.RX2Enabled = false;
                    else if (s == "1")
                        console.RX2Enabled = true;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.RX2Enabled)
                        return "1";
                    else
                        return "0";
                }
                else
                {
                    return parser.Error1;
                }
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        //Sets or reads the RIT button status
        public string ZZRT(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.RITOn = false;
                else if (s == "1")
                    console.RITOn = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool rit = console.RITOn;
                if (rit)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        //Increments RIT
        public string ZZRU(string s)
        {
            if (s.Length == parser.nSet)
            {
                return ZZRF(s);
            }
            else if (s.Length == parser.nGet && console.RITOn)
            {
                switch (console.RX1DSPMode)
                {
                    case DSPMode.CWL:
                    case DSPMode.CWU:
                        console.RITValue += 10;
                        break;
                    case DSPMode.LSB:
                    case DSPMode.USB:
                        console.RITValue += 50;
                        break;
                }
                return "";
            }
            else
                return parser.Error1;
        }

        //Reads the primary input voltate
        public string ZZRV()
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000)
            {
                int val = 0;
                decimal volts = 0.0m;
                FWC.ReadPAADC(2, out val);
                volts = (decimal)val / 4096m * 2.5m * 11m;
                return Decimal.Round(volts, 1).ToString();
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }

        //Moves VFO A down one Tune Step
        public string ZZSA()
        {
            try
            {
                int step = console.TuneStepIndex;
                List<TuneStep> tune_list = console.TuneStepList;
                console.VFOAFreq = console.CATVFOA - tune_list[step].StepHz * 10e-7;
                return "";
            }
            catch (Exception)
            {
                return parser.Error1;
            }
        }

        //Moves VFO A up one Tune Step
        public string ZZSB()
        {
            try
            {
                int step = console.TuneStepIndex;
                List<TuneStep> tune_list = console.TuneStepList;
                console.VFOAFreq = console.CATVFOA + tune_list[step].StepHz * 10e-7;
                return "";
            }
            catch (Exception)
            {
                return parser.Error1;
            }

        }

        //Moves the mouse wheel tuning step down
        public string ZZSD()
        {
            console.CATTuneStepDown();
            return "";
        }

        // ZZSFccccwwww  Set Filter, cccc=center freq www=width both in hz 
        public string ZZSF(string s)
        {
            int center = Convert.ToInt32(s.Substring(0, 4), 10);
            int width = Convert.ToInt32(s.Substring(4), 10);
            SetFilterCenterAndWidth(center, width);
            return "";
        }


        //Moves VFO B down one Tune Step
        public string ZZSG()
        {
            try
            {
                int step = console.TuneStepIndex;
                List<TuneStep> tune_list = console.TuneStepList;
                console.VFOBFreq = console.CATVFOB - tune_list[step].StepHz * 10e-7;
                return "";
            }
            catch (Exception)
            {
                return parser.Error1;
            }
        }


        //Moves VFO B up one Tune Step
        public string ZZSH()
        {
            try
            {
                int step = console.TuneStepIndex;
                List<TuneStep> tune_list = console.TuneStepList;
                console.VFOBFreq = console.CATVFOB + tune_list[step].StepHz * 10e-7;
                return "";
            }
            catch (Exception)
            {
                return parser.Error1;
            }
        }

        // Reads the S Meter value
        public string ZZSM(string s)
        {
            int sm = 0;

            if (s == "0" || s == "1")   // read the main transceiver s meter
            {
                float num = 0f;
                if (console.PowerOn)
                    if (s == "0")
                        num = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    else
                        num = DttSP.CalculateRXMeter(2, 0, DttSP.MeterType.SIGNAL_STRENGTH);

                switch (console.CurrentModel)
                {
                    case Model.FLEX5000:
                        if (s == "0")
                        {
                            num = num +
                            console.MultiMeterCalOffset +
                            Display.RX1PreampOffset +
                            console.RX1FilterSizeCalOffset +
                            console.RX1PathOffset +
                            console.RX1XVTRGainOffset;
                            if (console.RX1Loop)
                                num = num + console.LoopGain;
                        }
                        else if (s == "1")
                        {
                            num = num +
                            console.RX2MeterCalOffset +
                            Display.RX2PreampOffset +
                            console.RX2FilterSizeCalOffset +
                            console.RX2PathOffset +
                            console.RX2XVTRGainOffset;
                            if (console.RX1Loop)
                                num = num + console.LoopGain;
                        }
                        break;
                    case Model.FLEX3000:
                        num = num +
                        console.MultiMeterCalOffset +
                        Display.RX1PreampOffset +
                        console.RX1FilterSizeCalOffset +
                        console.RX1PathOffset +
                        console.RX1XVTRGainOffset;
                        if (console.RX1Loop)
                            num = num + console.LoopGain;
                        break;
                    case Model.FLEX1500:
                    case Model.SDR1000:
                        num = num +
                        console.MultiMeterCalOffset +
                        Display.RX1PreampOffset +
                        console.RX1FilterSizeCalOffset +
                        console.RX1XVTRGainOffset;
                        break;
                }
                num = Math.Max(-140, num);
                num = Math.Min(-10, num);
                sm = ((int)num + 140) * 2;
                return sm.ToString().PadLeft(3, '0');
            }
            else
            {
                return parser.Error1;
            }
        }

        //Reads the radio serial number
        public string ZZSN()
        {
            uint sn;
            string ret_val = "";
            switch (console.CurrentModel)
            {
                case Model.DEMO:
                    ret_val = "0000-0000";
                    break;
                case Model.FLEX1500:
                    sn = HIDEEPROM.SerialNumber;
                    if (sn > 0)
                        ret_val = HIDEEPROM.SerialToString(sn);
                    else
                        ret_val = parser.Error1;
                    break;
                case Model.FLEX3000:
                case Model.FLEX5000:
                    sn = FWCEEPROM.SerialNumber;
                    if (sn > 0)
                        ret_val = FWCEEPROM.SerialToString(sn);
                    else
                        ret_val = parser.Error1;
                    break;
                default:
                    parser.Verbose_Error_Code = 7;
                    ret_val = parser.Error1;
                    break;
            }

            return ret_val;
        }

        // Sets or reads the VFO Split status
        public string ZZSP(string s)
        {


            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.VFOSplit = false;
                else
                    console.VFOSplit = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.VFOSplit;
                if (!retval)
                    return "0";
                else
                    return "1";
            }
            else
            {
                return parser.Error1;
            }

        }

        // Sets or reads the Squelch on/off status
        public string ZZSO(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                console.CATSquelch = Convert.ToInt32(s);
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return console.CATSquelch.ToString();
            }
            else
                return parser.Error1;

        }

        // Sets or reads the SDR-1000 Squelch contorl
        public string ZZSQ(string s)
        {
            int level = 0;
            string OldSq;
            int ThisNset = 3;
            int TempNset = 1;

            if (s.Length == parser.nSet)
            {
                level = Convert.ToInt32(s);
                if (console.RX1DSPMode == DSPMode.FM)
                {
                    level = Math.Max(0, level);			// lower bound
                    level = Math.Min(100, level);		// upper bound
                    level = level * -1;
                }
                else
                {
                    level = Math.Max(0, level);
                    level = Math.Min(160, level);
                }
                OldSq = ZZSO("");
                if (OldSq == "0")
                {
                    parser.nSet = TempNset;
                    ZZSO("1");
                    parser.nSet = ThisNset;
                }
                console.Squelch = level * -1;
                parser.nSet = TempNset;
                ZZSO(OldSq);
                parser.nSet = ThisNset;
                return "";
            }
            else if (s.Length == parser.nGet)
                return AddLeadingZeros(Math.Abs(console.Squelch));
            else
                return parser.Error1;

        }

        //Reads or sets the Spur Reduction button status
        public string ZZSR(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.SpurReduction = true;
                else
                    console.SpurReduction = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.SpurReduction)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;


        }

        public string ZZSS()
        {
            console.cwxForm.CWXStop();
            return "";
        }

        // Reads the current console step size (read-only property)
        public string ZZST()
        {
            int step = console.TuneStepIndex;
            return Step2String(step);
        }

        // Moves the mouse wheel step tune up
        public string ZZSU()
        {
            console.CATTuneStepUp();
            return "";
        }

        // Sets or reads the RX2 Squelch button
        public string ZZSV(string s)
        {
            if (FWCEEPROM.RX2OK)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.CATSquelch2 = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.CATSquelch2;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }

        //Swaps VFO A/B TX buttons
        public string ZZSW(string s)
        {
            //  if (console.SpoofAB == true) // ke9ns add .200
            //  {
            //      s = "1";
            //  }


            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.SwapVFOA_BTX = false;
                else if (s == "1")
                    console.SwapVFOA_BTX = true;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.SwapVFOA_BTX;
                if (retval)
                    //  if (console.SpoofAB == true) // ke9ns add .200
                    //  {
                    //      return "0";
                    //   }
                    //  else
                    //  {
                    return "1";
                //  }
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        } // ZZSW

        //Sets or reads the RX2 Squelch threshold
        public string ZZSX(string s)
        {
            if (FWCEEPROM.RX2OK)
            {
                int level = 0;
                string OldSq;
                int ThisNset = 3;
                int TempNset = 1;

                if (s.Length == parser.nSet)
                {
                    level = Convert.ToInt32(s);
                    if (console.RX2DSPMode == DSPMode.FM)
                    {
                        level = Math.Max(0, level);			// lower bound
                        level = Math.Min(100, level);		// upper bound
                        level = level * -1;
                    }
                    else
                    {
                        level = Math.Max(0, level);
                        level = Math.Min(160, level);
                    }
                    OldSq = ZZSV("");
                    if (OldSq == "0")
                    {
                        parser.nSet = TempNset;
                        ZZSV("1");
                        parser.nSet = ThisNset;
                    }
                    console.Squelch2 = level * -1;
                    parser.nSet = TempNset;
                    ZZSV(OldSq);
                    parser.nSet = ThisNset;
                    return "";
                }
                else if (s.Length == parser.nGet)
                    return AddLeadingZeros(Math.Abs(console.Squelch2));
                else
                    return parser.Error1;

            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }



        //Reads or sets the VFO Sync button status
        public string ZZSY(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.VFOSync = true;
                else
                    console.VFOSync = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.VFOSync)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;
        }

        //Syncs the chosen vfo to the selected tune step
        //write only
        public string ZZSZ(string s)
        {
            double oldfreq;
            double newfreq;
            int size = console.CurrentTuneStepHz;
            if (s == "0")
                oldfreq = console.VFOAFreq;
            else
                oldfreq = console.VFOBFreq;
            newfreq = console.SnapTune(oldfreq, size, 1);
            if (s == "0")
                console.VFOAFreq = newfreq;
            else
                console.VFOBFreq = newfreq;
            return "";
        }

        //Sets or reads the CTCSS enable button
        public string ZZTA(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "1")
                {
                    console.CTCSSOn = true;
                    return "";
                }
                else if (s == "0")
                {
                    console.CTCSSOn = false;
                    return "";
                }
                else
                    return parser.Error1;
            }
            else if (s.Length == parser.nGet)
            {
                if (console.CTCSSOn)
                    return "1";
                else
                    return "0";
            }
            else return parser.Error1;
        }

        //Sets or reads the CTCSS tone frequency
        public string ZZTB(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (int.Parse(s) > 0 && int.Parse(s) <= 49)
                {
                    console.CTCSSFreq = String2CTCSSFreq(s);
                    return "";
                }
                else
                {
                    parser.Verbose_Error_Code = 9;
                    return parser.Error1;
                }
            }
            else if (s.Length == parser.nGet)
            {
                int freq = Convert.ToInt32(console.CTCSSFreq * 10);
                return CTCSSFreq2String(freq);
            }
            else
                return parser.Error1;

        }

        // Sets or reads the Show TX Filter checkbox
        public string ZZTF(string s)
        {
            switch (console.RX1DSPMode)
            {
                case DSPMode.CWL:
                case DSPMode.CWU:
                    return parser.Error1;
                default:
                    if (s.Length == parser.nSet && (s == "0" || s == "1"))
                    {
                        if (s == "1")
                            console.ShowTXFilter = true;
                        else
                            console.ShowTXFilter = false;
                        return "";
                    }
                    else if (s.Length == parser.nGet)
                    {
                        if (console.ShowTXFilter)
                            return "1";
                        else
                            return "0";
                    }
                    else
                        return parser.Error1;
            }
        }


        // Sets or reads the TX filter high setting
        public string ZZTH(string s)
        {
            int th = 0;

            if (s.Length == parser.nSet)    // check the min/max control settings
            {
                th = Convert.ToInt32(s);
                th = Math.Max(500, th);
                th = Math.Min(20000, th);
                console.setupForm.TXFilterHigh = th;
                return "";
            }
            else if (s.Length == parser.nGet)   // if this is a read command
            {
                return AddLeadingZeros(console.setupForm.TXFilterHigh);
            }
            else
            {
                return parser.Error1;   // return a ?
            }
        }

        //Inhibits power output when using external antennas, tuners, etc.
        public string ZZTI(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                {


                    console.RXOnly = false;
                }
                else if (s == "1")
                {
                    console.RXOnly = true;
                    console.MOX = false;
                }

                return "";
            }
            else
                return parser.Error1;
        }

        // Sets or reads the TX filter low setting
        public string ZZTL(string s)
        {
            int tl = 0;

            if (s.Length == parser.nSet)    // check the min/max control settings
            {
                tl = Convert.ToInt32(s);
                tl = Math.Max(0, tl);
                tl = Math.Min(2000, tl);
                console.setupForm.TXFilterLow = tl;
                return "";
            }
            else if (s.Length == parser.nGet)   // if this is a read command
            {
                return AddLeadingZeros(console.setupForm.TXFilterLow);
            }
            else
            {
                return parser.Error1;   // return a ?
            }
        }

        //Sets or reads the TX Monitor level
        public string ZZTM(string s)
        {
            int tm = 0;

            if (s.Length == parser.nSet)	// check the min/max control settings
            {
                tm = Convert.ToInt32(s);
                tm = Math.Max(0, tm);
                tm = Math.Min(100, tm);
                console.TXAF = tm;
                return "";
            }
            else if (s.Length == parser.nGet)	// if this is a read command
            {
                return AddLeadingZeros(console.TXAF);
            }
            else
            {
                return parser.Error1;	// return a ?
            }
        }


        //Sets or reads the Tune Power level
        public string ZZTO(string s)
        {
            int tl = 0;

            if (s.Length == parser.nSet)    // check the min/max control settings
            {
                tl = Convert.ToInt32(s);
                tl = Math.Max(0, tl);
                tl = Math.Min(100, tl);
                console.setupForm.TunePower = tl;
                return "";
            }
            else if (s.Length == parser.nGet)   // if this is a read command
            {
                return AddLeadingZeros(console.setupForm.TunePower);
            }
            else
            {
                return parser.Error1;   // return a ?
            }
        }


        //Sets or reads the TX Profile
        public string ZZTP(string s)
        {
            int items = console.CATTXProfileCount;
            int cnt = 0;
            if (s != "")
                cnt = Convert.ToInt32(s);

            if (s.Length == parser.nSet && cnt < items)
            {
                console.CATTXProfile = cnt;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.CATTXProfile);
            }
            else
                return parser.Error1;
        }

        // Reads the Flex 5000 temperature sensor
        public string ZZTS()
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX3000)
            {
                int val = 0;
                float volts = 0.0f;
                double temp2 = 0.0f;

                int chan = 4;
                if (console.CurrentModel == Model.FLEX3000)
                    chan = 3;

                FWC.ReadPAADC(chan, out val);
                volts = (float)val / 4096 * 2.5f;
                double temp = 301 - volts * 1000 / 2.2;

                if (temp >= 100f)
                    temp2 = Math.Round(temp, 1);
                else
                    temp2 = Math.Round(temp, 2);

                return temp2.ToString().PadLeft(5, '0');
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the TUN button on/off status
        public string ZZTU(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.TUN = false;
                else if (s == "1")
                    console.TUN = true;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.TUN;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

        //Sets or reads the transmit VFO frequency when in split with RX2 enabled
        public string ZZTV(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 && FWCEEPROM.RX2OK &&
                ZZRS("") == "1" && (ZZSP("") == "1" || ZZMU("") == "1"))
            {
                if (s.Length == parser.nSet)
                {
                    console.VFOASubFreq = double.Parse(s) / 1e6;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    int f = Convert.ToInt32(Math.Round(console.VFOASubFreq, 6) * 1e6);
                    return AddLeadingZeros(f);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }

        }

        //Sets or reads the MOX button status
        public string ZZTX(string s)
        {

            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                {
                    console.CATPTT = false;

                    if (console.SpoofTX == true)
                    {
                        console.SpoofTX = false;
                        if (console.LastVFOBTX == true)
                        {
                            console.LastVFOBTX = false;
                            // set VFOTX back to VFOA if it was moved to B due to a spoof
                            console.SwapVFOA_BTX = false; // VFOA is TX
                                                          //   console.SwapVFOA_BTX = true; // VFOB is TX
                        }

                    }
                }
                else if (s == "1")
                {
                    if (console.SpoofAB == true) // ke9ns add .200
                    {
                        // check which VFO is TX and save it here to return it back when done with TX
                        //   console.SwapVFOA_BTX = false; // VFOA is TX
                        console.SwapVFOA_BTX = true; // VFOB is TX
                        console.SpoofTX = true;
                    }

                    console.CATPTT = true;
                }

                return "";
            }
            else if (s.Length == parser.nGet)
            {

                if (console.CATPTT || console.MOX)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;

        }

        //Reads the XVTR Band Names
        public string ZZUA()
        {
            string ans = console.CATGetXVTRBandNames();
            return ans;
        }

        // Reads or sets the VAC Enable checkbox (Setup Form)
        public string ZZVA(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.setupForm.VACEnable = true;
                else
                    console.setupForm.VACEnable = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.setupForm.VACEnable)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }


        /// <summary>
        /// Sets or reads the VAC RX Gain 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVB(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-40, n);
                n = Math.Min(40, n);
            }

            if (s.Length == parser.nSet)
            {
                console.setupForm.VACRXGain = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.VACRXGain;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        /// <summary>
        /// Sets or reads the VAC TX Gain
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVC(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-40, n);
                n = Math.Min(40, n);
            }

            if (s.Length == parser.nSet)
            {
                console.setupForm.VACTXGain = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.VACTXGain;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        /// <summary>
        /// Sets or reads the VAC Sample Rate
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVD(string s)
        {
            int n = -1;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                switch (n)
                {
                    case 0:
                        console.setupForm.VACSampleRate = "6000";
                        break;
                    case 1:
                        console.setupForm.VACSampleRate = "8000";
                        break;
                    case 2:
                        console.setupForm.VACSampleRate = "11025";
                        break;
                    case 3:
                        console.setupForm.VACSampleRate = "12000";
                        break;
                    case 4:
                        console.setupForm.VACSampleRate = "24000";
                        break;
                    case 5:
                        console.setupForm.VACSampleRate = "22050";
                        break;
                    case 6:
                        console.setupForm.VACSampleRate = "44100";
                        break;
                    case 7:
                        console.setupForm.VACSampleRate = "48000";
                        break;
                    case 8:
                        console.setupForm.VACSampleRate = "96000";
                        break;
                    case 9:
                        console.setupForm.VACSampleRate = "192000";
                        break;
                }
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                string rate = console.setupForm.VACSampleRate;
                string ans = "";

                switch (rate)
                {
                    case "6000":
                        ans = "0";
                        break;
                    case "8000":
                        ans = "1";
                        break;
                    case "11025":
                        ans = "2";
                        break;
                    case "12000":
                        ans = "3";
                        break;
                    case "24000":
                        ans = "4";
                        break;
                    case "22050":
                        ans = "5";
                        break;
                    case "44100":
                        ans = "6";
                        break;
                    case "48000":
                        ans = "7";
                        break;
                    case "96000":
                        ans = "8";
                        break;
                    case "192000":
                        ans = "9";
                        break;
                    default:
                        ans = parser.Error1;
                        break;
                }
                return ans;
            }
            else
            {
                return parser.Error1;
            }
        }

        //Reads or sets the VOX Enable button status
        public string ZZVE(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.VOXEnable = true;
                else
                    console.VOXEnable = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.VOXEnable)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }


        /// <summary>
        /// Sets or reads the VAC Stereo checkbox
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVF(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.setupForm.VACStereo = true;
                else
                    console.setupForm.VACStereo = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.setupForm.VACStereo)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

        //Reads or set the VOX Gain control
        public string ZZVG(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(1000, n);

            if (s.Length == parser.nSet)
            {
                console.VOXSens = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.VOXSens);
            }
            else
            {
                return parser.Error1;
            }

        }

        // Reads or sets the I/Q to VAC checkbox on the setup form
        public string ZZVH(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.setupForm.IQOutToVAC = false;
                else if (s == "1")
                    console.setupForm.IQOutToVAC = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.setupForm.IQOutToVAC;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Reads or sets the VAC Input cable
        public string ZZVI(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.VACInputCable = Convert.ToInt32(s);
                return "";
            }

            if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.VACInputCable);
            }
            else
                return parser.Error1;
        }

        //Reads or sets the Direct I/Q Use RX2 checkbox
        public string ZZVJ(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1") && console.setupForm.IQOutToVAC)
            {
                if (s == "0")
                    console.setupForm.VACUseRX2 = false;
                else if (s == "1")
                    console.setupForm.VACUseRX2 = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.setupForm.VACUseRX2;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
                return parser.Error1;

        }

        // Reads or sets the VAC2 Enable checkbox (Setup Form)
        public string ZZVK(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.setupForm.VAC2Enable = true;
                else
                    console.setupForm.VAC2Enable = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.setupForm.VAC2Enable)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }


        // Reads or sets the VFO Lock button status
        public string ZZVL(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1" || s == "2" || s == "3")) //.283 1=vfoA lock (original function), 2= vfoB lock, 3= vfoAB lock both
            {
                if (s == "0")
                {
                   // console.CATVFOLock = false;
                    console.CATVFOLockAB = 0;
                }
                else if (s == "1")
                {
                  //  console.CATVFOLock = true;
                    console.CATVFOLockAB = 1;
                }
                else if (s == "2")
                {
                   
                    console.CATVFOLockAB = 2;
                     
                }
                else if (s == "3")
                {
                    console.CATVFOLockAB = 3;
                }

                return "";
            }
            else if (s.Length == parser.nGet) // ke9ns: if you send ZZVL with no number after then come here
            {

              
                if (console.setupForm != null && console.setupForm.chkBoxVFOLockAB.Checked) //.284
                {
                    int retval = console.CATVFOLockAB;
                    Debug.WriteLine("--ZZVL " + retval);

                  
                    if (retval == 0) return "0";
                    else if (retval == 1) return "1";
                    else if (retval == 2) return "2";
                    else return "3";
                }
                else
                {
                   
                    bool retval = console.CATVFOLock;
                    if (retval)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                

                
            }
            else
            {
                return parser.Error1;
            }
        }

        // Reads or sets the VAC Driver
        public string ZZVM(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.VACDriver = Convert.ToInt32(s);
                return "";
            }

            if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.VACDriver);
            }
            else
                return parser.Error1;
        }


        // Returns the version number of the PowerSDR program
        public string ZZVN()
        {
            return console.CATGetVersion().PadLeft(12, '0');
        }

        // Reads or sets the VAC Output cable
        public string ZZVO(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.VACOutputCable = Convert.ToInt32(s);
                return "";
            }

            if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.VACOutputCable);
            }
            else
                return parser.Error1;
        }

        // Reads or sets the VAC1 IQ Calibrate checkbox on the setup form
        public string ZZVP(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.setupForm.VAC1Calibrate = false;
                else if (s == "1")
                    console.setupForm.VAC1Calibrate = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.setupForm.VAC1Calibrate;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Reads or sets the VAC2 Driver
        public string ZZVQ(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.VAC2Driver = Convert.ToInt32(s);
                return "";
            }

            if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.VAC2Driver);
            }
            else
                return parser.Error1;
        }

        // Reads or sets the VAC2 Input cable
        public string ZZVR(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.VAC2InputCable = Convert.ToInt32(s);
                return "";
            }

            if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.VAC2InputCable);
            }
            else
                return parser.Error1;
        }


        // Sets the VFO swap status
        // write only
        public string ZZVS(string s)
        {
            if (s.Length == parser.nSet & Convert.ToInt32(s) <= 3)
            {
                console.CATVFOSwap(s);
                return "";
            }
            else
            {
                return parser.Error1;
            }

        }

        // Reads or sets the VAC2 Output cable
        public string ZZVT(string s)
        {
            if (s.Length == parser.nSet)
            {
                console.setupForm.VAC2OutputCable = Convert.ToInt32(s);
                return "";
            }

            if (s.Length == parser.nGet)
            {
                return AddLeadingZeros(console.setupForm.VAC2OutputCable);
            }
            else
                return parser.Error1;
        }

        /// <summary>
        /// Sets or reads the VAC2 Sample Rate
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVU(string s)
        {
            int n = -1;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                switch (n)
                {
                    case 0:
                        console.setupForm.VAC2SampleRate = "6000";
                        break;
                    case 1:
                        console.setupForm.VAC2SampleRate = "8000";
                        break;
                    case 2:
                        console.setupForm.VAC2SampleRate = "11025";
                        break;
                    case 3:
                        console.setupForm.VAC2SampleRate = "12000";
                        break;
                    case 4:
                        console.setupForm.VAC2SampleRate = "24000";
                        break;
                    case 5:
                        console.setupForm.VAC2SampleRate = "22050";
                        break;
                    case 6:
                        console.setupForm.VAC2SampleRate = "44100";
                        break;
                    case 7:
                        console.setupForm.VAC2SampleRate = "48000";
                        break;
                    case 8:
                        console.setupForm.VAC2SampleRate = "96000";
                        break;
                    case 9:
                        console.setupForm.VAC2SampleRate = "192000";
                        break;
                }
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                string rate = console.setupForm.VAC2SampleRate;
                string ans = "";

                switch (rate)
                {
                    case "6000":
                        ans = "0";
                        break;
                    case "8000":
                        ans = "1";
                        break;
                    case "11025":
                        ans = "2";
                        break;
                    case "12000":
                        ans = "3";
                        break;
                    case "24000":
                        ans = "4";
                        break;
                    case "22050":
                        ans = "5";
                        break;
                    case "44100":
                        ans = "6";
                        break;
                    case "48000":
                        ans = "7";
                        break;
                    case "96000":
                        ans = "8";
                        break;
                    case "192000":
                        ans = "9";
                        break;
                    default:
                        ans = parser.Error1;
                        break;
                }
                return ans;
            }
            else
            {
                return parser.Error1;
            }
        }

        /// Sets or reads the VAC2 Stereo checkbox
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVV(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "1")
                    console.setupForm.VAC2Stereo = true;
                else
                    console.setupForm.VAC2Stereo = false;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                if (console.setupForm.VAC2Stereo)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

        /// <summary>
        /// Sets or reads the VAC2 RX Gain 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVW(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-40, n);
                n = Math.Min(40, n);
            }

            if (s.Length == parser.nSet)
            {
                console.setupForm.VAC2RXGain = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.VAC2RXGain;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        /// <summary>
        /// Sets or reads the VAC2 TX Gain
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVX(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-40, n);
                n = Math.Min(40, n);
            }

            if (s.Length == parser.nSet)
            {
                console.setupForm.VAC2TXGain = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.setupForm.VAC2TXGain;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        /// <summary>
        /// Sets or reads the VAC1 Buffer Size
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVY(string s)
        {
            int n = -1;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                switch (n)
                {
                    case 0:
                        console.setupForm.VAC1BufferSize = "512";
                        break;
                    case 1:
                        console.setupForm.VAC1BufferSize = "1024";
                        break;
                    case 2:
                        console.setupForm.VAC1BufferSize = "2048";
                        break;
                }
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                string rate = console.setupForm.VAC1BufferSize;
                string ans = "";

                switch (rate)
                {
                    case "512":
                        ans = "0";
                        break;
                    case "1024":
                        ans = "1";
                        break;
                    case "2048":
                        ans = "2";
                        break;
                    default:
                        ans = parser.Error1;
                        break;
                }
                return ans;
            }
            else
            {
                return parser.Error1;
            }
        }

        /// <summary>
        /// Sets or reads the VAC1 Buffer Size
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ZZVZ(string s)
        {
            int n = -1;

            if (s.Length == parser.nSet)
            {
                n = Convert.ToInt32(s);
                switch (n)
                {
                    case 0:
                        console.setupForm.VAC2BufferSize = "512";
                        break;
                    case 1:
                        console.setupForm.VAC2BufferSize = "1024";
                        break;
                    case 2:
                        console.setupForm.VAC2BufferSize = "2048";
                        break;
                }
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                string rate = console.setupForm.VAC2BufferSize;
                string ans = "";

                switch (rate)
                {
                    case "512":
                        ans = "0";
                        break;
                    case "1024":
                        ans = "1";
                        break;
                    case "2048":
                        ans = "2";
                        break;
                    default:
                        ans = parser.Error1;
                        break;
                }
                return ans;
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Mixer Mic Gain
        public string ZZWA(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;
                string sign;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(-128, n);
                    n = Math.Min(0, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.MicInputSelected == "1")
                    {
                        console.fwcMixForm.MicInput = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.MicInput;
                    if (x >= 0)
                        sign = "+";
                    else
                        sign = "-";
                    return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Line In RCA level
        public string ZZWB(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;
                string sign;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(-128, n);
                    n = Math.Min(0, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.LineInRCASelected == "1")
                    {
                        console.fwcMixForm.LineInRCA = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.LineInRCA;
                    if (x >= 0)
                        sign = "+";
                    else
                        sign = "-";
                    return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Line In Phono level
        public string ZZWC(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;
                string sign;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(-128, n);
                    n = Math.Min(0, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.LineInPhonoSelected == "1")
                    {
                        console.fwcMixForm.LineInPhono = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.LineInPhono;
                    if (x >= 0)
                        sign = "+";
                    else
                        sign = "-";
                    return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Mixer Line In DB9 level
        public string ZZWD(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;
                string sign;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(-128, n);
                    n = Math.Min(0, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.LineInDB9Selected == "1")
                    {
                        console.fwcMixForm.LineInDB9 = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.LineInDB9;
                    if (x >= 0)
                        sign = "+";
                    else
                        sign = "-";
                    return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        // Sets or reads the F1500F5K Mixer Mic Selected Checkbox
        public string ZZWE(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        console.fwcMixForm.MicInputSelected = s;
                    else
                        console.flex1500MixerForm.MicInputSelectedStr = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        return console.fwcMixForm.MicInputSelected;
                    else
                        return console.flex1500MixerForm.MicInputSelectedStr;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F5K Mixer Line In RCA Checkbox
        public string ZZWF(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.fwcMixForm.LineInRCASelected = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.fwcMixForm.LineInRCASelected;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F5K Mixer Line In Phono Checkbox
        public string ZZWG(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.fwcMixForm.LineInPhonoSelected = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.fwcMixForm.LineInPhonoSelected;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F1500/F5K Mixer Line In FlexWire/DB9 Checkbox
        public string ZZWH(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        console.fwcMixForm.LineInDB9Selected = s;
                    else
                        console.flex1500MixerForm.LineInDB9Selected = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        return console.fwcMixForm.LineInDB9Selected;
                    else
                        return console.flex1500MixerForm.LineInDB9Selected;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        // Sets or reads the F5K Mixer Mute All Checkbox
        public string ZZWJ(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        console.fwcMixForm.InputMuteAll = s;
                    else
                        console.flex1500MixerForm.InputMuteAll = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        return console.fwcMixForm.InputMuteAll;
                    else
                        return console.flex1500MixerForm.InputMuteAll;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Mixer Internal Speaker level
        public string ZZWK(string s)
        {
            //			if(console.CurrentModel == Model.FLEX5000)
            if (FWCEEPROM.Model == 1) // FLEX-5000C
            {
                int n = 0;
                int x = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(128, n);
                    n = Math.Min(255, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.InternalSpkrSelected == "1")
                    {
                        console.fwcMixForm.InternalSpkr = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.InternalSpkr;
                    return AddLeadingZeros(x);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Mixer External Speaker level
        public string ZZWL(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(128, n);
                    n = Math.Min(255, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.ExternalSpkrSelected == "1")
                    {
                        console.fwcMixForm.ExternalSpkr = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.ExternalSpkr;
                    return AddLeadingZeros(x);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Mixer Headphone level
        public string ZZWM(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(128, n);
                    n = Math.Min(255, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.HeadphoneSelected == "1")
                    {
                        console.fwcMixForm.Headphone = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.Headphone;
                    return AddLeadingZeros(x);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F5K Mixer Line Out RCA level
        public string ZZWN(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                int n = 0;
                int x = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(128, n);
                    n = Math.Min(255, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.fwcMixForm.LineOutRCASelected == "1")
                    {
                        console.fwcMixForm.LineOutRCA = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    x = console.fwcMixForm.LineOutRCA;
                    return AddLeadingZeros(x);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F5KC Mixer Internal Speaker Selected Checkbox
        public string ZZWO(string s)
        {
            if (FWCEEPROM.Model == 1)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.fwcMixForm.InternalSpkrSelected = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.fwcMixForm.InternalSpkrSelected;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F5K Mixer External Speaker Selected Checkbox
        public string ZZWP(string s)
        {
            if (console.CurrentModel == Model.FLEX5000)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    console.fwcMixForm.ExternalSpkrSelected = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    return console.fwcMixForm.ExternalSpkrSelected;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F1500F5K Mixer Headphone Selected Checkbox
        public string ZZWQ(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        console.fwcMixForm.HeadphoneSelected = s;
                    else
                        console.flex1500MixerForm.PhonesSelectedStr = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        return console.fwcMixForm.HeadphoneSelected;
                    else
                        return console.flex1500MixerForm.PhonesSelectedStr;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F1500 FlexWire Out/F5K Mixer Line Out RCA Selected Checkbox
        public string ZZWR(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        console.fwcMixForm.LineOutRCASelected = s;
                    else
                        console.flex1500MixerForm.FlexWireOutSelectedStr = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        return console.fwcMixForm.LineOutRCASelected;
                    else
                        return console.flex1500MixerForm.FlexWireOutSelectedStr;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Sets or reads the F1500/F5K Mixer Output Mute All Checkbox
        public string ZZWS(string s)
        {
            if (console.CurrentModel == Model.FLEX5000 || console.CurrentModel == Model.FLEX1500)
            {
                if (s.Length == parser.nSet && (s == "0" || s == "1"))
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        console.fwcMixForm.OutputMuteAll = s;
                    else
                        console.flex1500MixerForm.OutputMuteAll = s;
                    return "";
                }
                else if (s.Length == parser.nGet)
                {
                    if (console.CurrentModel == Model.FLEX5000)
                        return console.fwcMixForm.OutputMuteAll;
                    else
                        return console.flex1500MixerForm.OutputMuteAll;
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }


        //Reads or sets the F1500 mixer form mic level
        public string ZZWT(string s)
        {
            if (console.CurrentModel == Model.FLEX1500)
            {
                int n = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(0, n);
                    n = Math.Min(119, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.flex1500MixerForm.MicInputSelectedStr == "1")
                    {
                        console.flex1500MixerForm.MicInput = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros(console.flex1500MixerForm.MicInput);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Reads or sets the F1500 Mixer Form FireWire Input Level
        public string ZZWU(string s)
        {
            if (console.CurrentModel == Model.FLEX1500)
            {
                int n = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(0, n);
                    n = Math.Min(119, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.flex1500MixerForm.LineInDB9Selected == "1")
                    {
                        console.flex1500MixerForm.FlexWireIn = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros(console.flex1500MixerForm.FlexWireIn);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets ir reads the F1500 Mixer Form Phones level
        public string ZZWV(string s)
        {
            if (console.CurrentModel == Model.FLEX1500)
            {
                int n = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(0, n);
                    n = Math.Min(127, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.flex1500MixerForm.PhonesSelectedStr == "1")
                    {
                        console.flex1500MixerForm.Phones = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros(console.flex1500MixerForm.Phones);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        //Sets or reads the F1500 Mixer Form FlexWire Out level
        public string ZZWW(string s)
        {
            if (console.CurrentModel == Model.FLEX1500)
            {
                int n = 0;

                if (s != "")
                {
                    n = Int32.Parse(s);
                    n = Math.Max(0, n);
                    n = Math.Min(127, n);
                }

                if (s.Length == parser.nSet)
                {
                    if (console.flex1500MixerForm.FlexWireOutSelectedStr == "1")
                    {
                        console.flex1500MixerForm.FlexWireOut = n;
                        return "";
                    }
                    else
                        return parser.Error1;

                }
                else if (s.Length == parser.nGet)
                {
                    return AddLeadingZeros(console.flex1500MixerForm.FlexWireOut);
                }
                else
                    return parser.Error1;
            }
            else
            {
                parser.Verbose_Error_Code = 7;
                return parser.Error1;
            }
        }

        // Clears the XIT frequency
        // write only
        public string ZZXC()
        {
            console.XITValue = 0;
            return "";
        }

        // Sets or reads the XIT frequency value
        public string ZZXF(string s)
        {
            int n = 0;
            int x = 0;
            string sign;

            if (s != "")
            {
                n = Convert.ToInt32(s);
                n = Math.Max(-99999, n);
                n = Math.Min(99999, n);
            }

            if (s.Length == parser.nSet)
            {
                console.XITValue = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                x = console.XITValue;
                if (x >= 0)
                    sign = "+";
                else
                    sign = "-";
                // we have to remove the leading zero and replace it with the sign.
                return sign + AddLeadingZeros(Math.Abs(x)).Substring(1);
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the XIT button status
        public string ZZXS(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.XITOn = false;
                else
                    if (s == "1")
                    console.XITOn = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool xit = console.XITOn;
                if (xit)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        //Sets or reads the X2TR button status
        public string ZZXT(string s)
        {
            if (s.Length == parser.nSet)
            {
                if (s == "0")
                    console.X2TR = false;
                else if (s == "1")
                    console.X2TR = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool x2tr = console.X2TR;
                if (x2tr)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Reads or sets the VAC2 Direct I/Q checkbox on the setup form
        public string ZZYA(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.setupForm.VAC2DirectIQ = false;
                else if (s == "1")
                    console.setupForm.VAC2DirectIQ = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.setupForm.VAC2DirectIQ;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        // Reads or sets the VAC2 IQ Calibrate checkbox on the setup form
        public string ZZYB(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.setupForm.VAC2Calibrate = false;
                else if (s == "1")
                    console.setupForm.VAC2Calibrate = true;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.setupForm.VAC2Calibrate;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }
        }

        //Reads or sets the FM mic gain
        public string ZZYC(string s)
        {
            int n = 0;

            if (s != null && s != "")
                n = Convert.ToInt32(s);
            n = Math.Max(0, n);
            n = Math.Min(70, n);

            if (s.Length == parser.nSet)
            {
                console.FMMic = n;
                return "";
            }
            else if (s.Length == parser.nGet)
            {
                return AddLeadingZeros((int)console.FMMic);
            }
            else
            {
                return parser.Error1;
            }

        }

        public string ZZZB()
        {
            if (console.CATDisplayAvg == 1)
                console.CATZB = "1";
            return "";
        }

        public string ZZZZ()
        {
            console.Siolisten.SIO.Close();
            return "";
        }
        #endregion Extended CAT Methods ZZR-ZZZ


        #region Helper methods

        #region General Helpers

        private string AddLeadingZeros(int n)
        {
            string num = n.ToString();

            while (num.Length < parser.nAns)
                num = num.Insert(0, "0");

            return num;
        }

        private string JustSuffix(string s)
        {
            string Sfx = "";
            Sfx = s.Substring(4);
            return Sfx.Substring(0, Sfx.Length - 1);
        }


        #endregion General Helpers

        #region Repeater Methods

        private string OffsetDirection2String()
        {
            string rtn = "";

            switch (console.CurrentFMTXMode)
            {
                case FMTXMode.Simplex:
                    rtn = "0";
                    break;
                case FMTXMode.High:
                    rtn = "1";
                    break;
                case FMTXMode.Low:
                    rtn = "2";
                    break;
                default:
                    rtn = "0";
                    break;
            }
            return rtn;
        }

        private void String2OffsetDirection(string s)
        {
            switch (s)
            {
                case "0":
                    console.CurrentFMTXMode = FMTXMode.Simplex;
                    break;
                case "1":
                    console.CurrentFMTXMode = FMTXMode.High;
                    break;
                case "2":
                    console.CurrentFMTXMode = FMTXMode.Low;
                    break;
                default:
                    console.CurrentFMTXMode = FMTXMode.Simplex;
                    break;
            }
        }

        private double String2CTCSSFreq(string s)
        {
            double freq = 00.0;
            switch (s)
            {
                case "01":
                    freq = 67.0;
                    break;
                case "02":
                    freq = 69.3;
                    break;
                case "03":
                    freq = 71.9;
                    break;
                case "04":
                    freq = 74.4;
                    break;
                case "05":
                    freq = 77.0;
                    break;
                case "06":
                    freq = 79.7;
                    break;
                case "07":
                    freq = 82.5;
                    break;
                case "08":
                    freq = 85.4;
                    break;
                case "09":
                    freq = 88.5;
                    break;
                case "10":
                    freq = 91.5;
                    break;
                case "11":
                    freq = 94.8;
                    break;
                case "12":
                    freq = 97.4;
                    break;
                case "13":
                    freq = 100.0;
                    break;
                case "14":
                    freq = 103.5;
                    break;
                case "15":
                    freq = 107.2;
                    break;
                case "16":
                    freq = 110.9;
                    break;
                case "17":
                    freq = 114.8;
                    break;
                case "18":
                    freq = 118.8;
                    break;
                case "19":
                    freq = 123.0;
                    break;
                case "20":
                    freq = 127.3;
                    break;
                case "21":
                    freq = 131.8;
                    break;
                case "22":
                    freq = 136.5;
                    break;
                case "23":
                    freq = 141.3;
                    break;
                case "24":
                    freq = 146.2;
                    break;
                case "25":
                    freq = 151.4;
                    break;
                case "26":
                    freq = 156.7;
                    break;
                case "27":
                    freq = 159.8;
                    break;
                case "28":
                    freq = 162.2;
                    break;
                case "29":
                    freq = 165.5;
                    break;
                case "30":
                    freq = 167.9;
                    break;
                case "31":
                    freq = 171.3;
                    break;
                case "32":
                    freq = 173.8;
                    break;
                case "33":
                    freq = 177.3;
                    break;
                case "34":
                    freq = 179.9;
                    break;
                case "35":
                    freq = 183.5;
                    break;
                case "36":
                    freq = 186.2;
                    break;
                case "37":
                    freq = 189.9;
                    break;
                case "38":
                    freq = 192.8;
                    break;
                case "39":
                    freq = 199.5;
                    break;
                case "40":
                    freq = 203.5;
                    break;
                case "41":
                    freq = 206.5;
                    break;
                case "42":
                    freq = 210.7;
                    break;
                case "43":
                    freq = 218.1;
                    break;
                case "44":
                    freq = 225.7;
                    break;
                case "45":
                    freq = 229.1;
                    break;
                case "46":
                    freq = 233.6;
                    break;
                case "47":
                    freq = 241.8;
                    break;
                case "48":
                    freq = 250.3;
                    break;
                case "49":
                    freq = 254.1;
                    break;
                default:
                    freq = 67.0;
                    break;
            }
            return freq;
        }

        private string CTCSSFreq2String(int freq)
        {
            string ans = "";
            switch (freq)
            {
                case 670:
                    ans = "01";
                    break;
                case 693:
                    ans = "02";
                    break;
                case 719:
                    ans = "03";
                    break;
                case 744:
                    ans = "04";
                    break;
                case 770:
                    ans = "05";
                    break;
                case 797:
                    ans = "06";
                    break;
                case 825:
                    ans = "07";
                    break;
                case 854:
                    ans = "08";
                    break;
                case 885:
                    ans = "09";
                    break;
                case 915:
                    ans = "10";
                    break;
                case 948:
                    ans = "11";
                    break;
                case 974:
                    ans = "12";
                    break;
                case 1000:
                    ans = "13";
                    break;
                case 1035:
                    ans = "14";
                    break;
                case 1072:
                    ans = "15";
                    break;
                case 1109:
                    ans = "16";
                    break;
                case 1148:
                    ans = "17";
                    break;
                case 1188:
                    ans = "18";
                    break;
                case 1230:
                    ans = "19";
                    break;
                case 1273:
                    ans = "20";
                    break;
                case 1318:
                    ans = "21";
                    break;
                case 1365:
                    ans = "22";
                    break;
                case 1413:
                    ans = "23";
                    break;
                case 1462:
                    ans = "24";
                    break;
                case 1514:
                    ans = "25";
                    break;
                case 1567:
                    ans = "26";
                    break;
                case 1598:
                    ans = "27";
                    break;
                case 1622:
                    ans = "28";
                    break;
                case 1655:
                    ans = "29";
                    break;
                case 1679:
                    ans = "30";
                    break;
                case 1713:
                    ans = "31";
                    break;
                case 1738:
                    ans = "32";
                    break;
                case 1773:
                    ans = "33";
                    break;
                case 1799:
                    ans = "34";
                    break;
                case 1835:
                    ans = "35";
                    break;
                case 1862:
                    ans = "36";
                    break;
                case 1899:
                    ans = "37";
                    break;
                case 1928:
                    ans = "38";
                    break;
                case 1995:
                    ans = "39";
                    break;
                case 2035:
                    ans = "40";
                    break;
                case 2065:
                    ans = "41";
                    break;
                case 2107:
                    ans = "42";
                    break;
                case 2181:
                    ans = "43";
                    break;
                case 2257:
                    ans = "44";
                    break;
                case 2291:
                    ans = "45";
                    break;
                case 2336:
                    ans = "46";
                    break;
                case 2418:
                    ans = "47";
                    break;
                case 2503:
                    ans = "48";
                    break;
                case 2541:
                    ans = "49";
                    break;
                default:
                    ans = "01";
                    break;
            }
            return ans;
        }

        private SortableBindingList<MemoryRecord> GetMemoryList()
        {
            try
            {
                return console.MemoryList.List;
            }
            catch
            {
                SortableBindingList<MemoryRecord> list = new SortableBindingList<MemoryRecord>();
                parser.Verbose_Error_Code = 4;
                return list;
            }
        }

        private MemoryRecord GetChannelRecord(string channel)
        {
            MemoryRecord rec = new MemoryRecord();
            try
            {
                SortableBindingList<MemoryRecord> list = GetMemoryList();
                int n = 0;
                for (n = 0; n < list.Count; n++)
                {
                    if (list[n].Comments.Substring(0, 4) == channel + ":")
                        rec = list[n];
                }
            }
            catch
            {
            }
            return rec;
        }

        private int GetIndex(string channel)
        {
            int ndx = -1;
            try
            {
                SortableBindingList<MemoryRecord> list = GetMemoryList();
                int n = 0;
                for (n = 0; n < list.Count; n++)
                {
                    if (list[n].Comments.Substring(0, 4) == channel + ":")
                        ndx = n;
                }
            }
            catch
            {
            }
            return ndx;
        }

        private int GetNextChannelNumber()
        {
            SortableBindingList<MemoryRecord> list = new SortableBindingList<MemoryRecord>();
            list = console.MemoryList.List;
            int n = 0;
            int last = 0;
            for (n = 0; n < list.Count; n++)
            {
                if (list[n].Comments.Contains(":"))
                {
                    int thisN = int.Parse(list[n].Comments.Substring(0, list[n].Comments.IndexOf(":")));
                    if (thisN > last)
                        last = thisN;
                }

            }
            return last + 1;
        }

        #endregion Repeater Methods

        #region Antenna Methods

        private string FWCAntenna2String(FWCAnt ant)
        {
            string ans = "";
            switch (ant)
            {
                case FWCAnt.NC:
                    ans = "0";
                    break;
                case FWCAnt.ANT1:
                    ans = "1";
                    break;
                case FWCAnt.ANT2:
                    ans = "2";
                    break;
                case FWCAnt.ANT3:
                    ans = "3";
                    break;
                case FWCAnt.RX1IN:
                    ans = "4";
                    break;
                case FWCAnt.RX2IN:
                    ans = "5";
                    break;
                case FWCAnt.RX1TAP:
                    ans = "6";
                    break;
                case FWCAnt.SIG_GEN:
                    ans = "7";
                    break;
            }
            return ans;
        }

        private FWCAnt String2FWCAntenna(string ant)
        {
            FWCAnt ans = FWCAnt.ANT1;
            switch (ant)
            {
                case "0":
                    ans = FWCAnt.NC;
                    break;
                case "1":
                    ans = FWCAnt.ANT1;
                    break;
                case "2":
                    ans = FWCAnt.ANT2;
                    break;
                case "3":
                    ans = FWCAnt.ANT3;
                    break;
                case "4":
                    ans = FWCAnt.RX1IN;
                    break;
                case "5":
                    ans = FWCAnt.RX2IN;
                    break;
                case "6":
                    ans = FWCAnt.RX1TAP;
                    break;
                case "7":
                    ans = FWCAnt.SIG_GEN;
                    break;
            }
            return ans;
        }

        private string HIDAntenna2String(HIDAnt ant)
        {
            string ans = "";
            switch (ant)
            {
                case HIDAnt.PA:
                    ans = "0";
                    break;
                case HIDAnt.XVTX_COM:
                    ans = "1";
                    break;
                case HIDAnt.XVRX:
                    ans = "2";
                    break;
                case HIDAnt.BITE:
                    ans = "3";
                    break;
            }
            return ans;
        }

        private HIDAnt String2HIDAntenna(string ant)
        {
            HIDAnt ans = HIDAnt.PA;
            switch (ant)
            {
                case "0":
                    ans = HIDAnt.PA;
                    break;
                case "1":
                    ans = HIDAnt.XVTX_COM;
                    break;
                case "2":
                    ans = HIDAnt.XVRX;
                    break;
                case "3":
                    ans = HIDAnt.BITE;
                    break;
            }
            return ans;
        }


        private string AntMode2String(AntMode ant)
        {
            string ans = "0";
            switch (ant)
            {
                case AntMode.Simple:
                    break;
                case AntMode.Expert:
                    ans = "1";
                    break;
            }
            return ans;
        }

        private AntMode String2AntMode(string amode)
        {
            AntMode ans = AntMode.Simple;
            switch (amode)
            {
                case "0":
                    break;
                case "1":
                    ans = AntMode.Expert;
                    break;
            }
            return ans;
        }

        #endregion Antenna Methods

        #region Split Methods

        private void SetKWSplitStatus(string s)
        {
            if (s == "0")
            {
                console.VFOSplit = false;
                lastFR = "0";
                lastFT = "0";
            }
            else if (s == "1" && !console.VFOSplit)
            {
                console.VFOSplit = true;
                lastFR = "0";
                lastFT = "1";
            }
            else if (s == "1" && lastFR == "1" && lastFT == "1" && console.VFOSplit)
            {
                console.VFOSplit = false;
                lastFR = "0";
                lastFT = "0";
            }
            Debug.WriteLine("lastFR = " + lastFR + " lastFT = " + lastFT);
        }

        private string GetP10()
        {
            return lastFT;
        }

        private string GetP12()
        {
            return lastFT;
        }

        #endregion Split Methods

        #region VFO Methods
        // Converts a vfo frequency to a proper CAT frequency string
        private string StrVFOFreq(string vfo)
        {
            double freq = 0;
            string cmd_string = "";

            if (vfo == "A")
                freq = Math.Round(console.CATVFOA, 6);
            else if (vfo == "B")
                freq = Math.Round(console.CATVFOB, 6);
            else if (vfo == "C")
                freq = Convert.ToDouble(console.CATQMSValue);


            if ((int)freq < 10)
            {
                cmd_string += "0000" + freq.ToString();
            }
            else if ((int)freq < 100)
            {
                cmd_string += "000" + freq.ToString();
            }
            else if ((int)freq < 1000)
            {
                cmd_string += "00" + freq.ToString();
            }
            else if ((int)freq < 10000)
            {
                cmd_string += "0" + freq.ToString();
            }
            else
                cmd_string = freq.ToString();

            // Get rid of the decimal separator and pad the right side with 0's 
            // Modified 05/01/05 BT for globalization
            if (cmd_string.IndexOf(separator) > 0)
                cmd_string = cmd_string.Remove(cmd_string.IndexOf(separator), 1);
            cmd_string = cmd_string.PadRight(11, '0');
            return cmd_string;
        }
        #endregion VFO Methods

        #region Filter Methods

        public string Filter2String(Filter f)
        {
            string fw = f.ToString();
            string strfilt = "";
            int retval = 0;
            switch (fw)
            {
                case "F6000":
                    strfilt = "6000";
                    break;
                case "F4000":
                    strfilt = "4000";
                    break;
                case "F2600":
                    strfilt = "2600";
                    break;
                case "F2100":
                    strfilt = "2100";
                    break;
                case "F1000":
                    strfilt = "1000";
                    break;
                case "F500":
                    strfilt = "0500";
                    break;
                case "F250":
                    strfilt = "0250";
                    break;
                case "F100":
                    strfilt = "0100";
                    break;
                case "F50":
                    strfilt = "0050";
                    break;
                case "F25":
                    strfilt = "0025";
                    break;
                case "VAR1":
                    retval = Math.Abs(console.RX1FilterHigh - console.RX1FilterLow);
                    strfilt = AddLeadingZeros(retval);
                    break;
                case "VAR2":
                    retval = Math.Abs(console.RX1FilterHigh - console.RX1FilterLow);
                    strfilt = AddLeadingZeros(retval);
                    break;
            }
            return strfilt;
        }

        public Filter String2Filter(string f)
        {
            Filter filter = Filter.FIRST;
            switch (f)
            {
                case "6000":
                    filter = Filter.F1;
                    break;
                case "4000":
                    filter = Filter.F2;
                    break;
                case "2600":
                    filter = Filter.F3;
                    break;
                case "2100":
                    filter = Filter.F4;
                    break;
                case "1000":
                    filter = Filter.F5;
                    break;
                case "0500":
                    filter = Filter.F6;
                    break;
                case "0250":
                    filter = Filter.F7;
                    break;
                case "0100":
                    filter = Filter.F8;
                    break;
                case "0050":
                    filter = Filter.F9;
                    break;
                case "0025":
                    filter = Filter.F10;
                    break;
                case "VAR1":
                    filter = Filter.VAR1;
                    break;
                case "VAR2":
                    filter = Filter.VAR2;
                    break;
            }
            return filter;
        }

        // set variable filter 1 to indicate center and width 
        // 
        // if either center or width is zero, current value of center or width is 
        // contained 
        // fixme ... what should this thing do for am, fm, dsb ... ignore width? 
        private void SetFilterCenterAndWidth(int center, int width)
        {
            int new_lo;
            int new_hi;

            if (center == 0 || width == 0)  // need to get current values 
            {
                return; // not implemented yet 
            }
            else
            {
                // Debug.WriteLine("center: " + center  + " width: " + width); 
                new_lo = center - (width / 2);
                new_hi = center + (width / 2);
                if (new_lo < 0) new_lo = 0;
            }

            // new_lo and new_hi calculated assuming a USB mode .. do the right thing 
            // for lsb and other modes 
            // fixme -- needs more thinking 
            switch (console.RX1DSPMode)
            {
                case DSPMode.LSB:
                    int scratch = new_hi;
                    new_hi = -new_lo;
                    new_lo = -scratch;
                    break;

                case DSPMode.AM:
                case DSPMode.SAM:
                    new_lo = -new_hi;
                    break;
            }


            // System.Console.WriteLine("zzsf: " + new_lo + " " + new_hi); 
            console.SelectRX1VarFilter();
            console.UpdateRX1Filters(new_lo, new_hi);

            return;
        }

        // Converts interger filter frequency into Kenwood SL/SH codes
        private string Frequency2Code(int f, string n)
        {
            f = Math.Abs(f);
            string code = "";
            switch (console.RX1DSPMode)
            {
                case DSPMode.CWL:
                case DSPMode.CWU:
                case DSPMode.LSB:
                case DSPMode.USB:
                case DSPMode.DIGU: // ke9ns add
                case DSPMode.DIGL: // ke9ns add
                    switch (n)
                    {
                        case "SH":
                            if (f >= 0 && f <= 1500)
                                code = "00";
                            else if (f > 1500 && f <= 1700)
                                code = "01";
                            else if (f > 1700 && f <= 1900)
                                code = "02";
                            else if (f > 1900 && f <= 2100)
                                code = "03";
                            else if (f > 2100 && f <= 2300)
                                code = "04";
                            else if (f > 2300 && f <= 2500)
                                code = "05";
                            else if (f > 2500 && f <= 2700)
                                code = "06";
                            else if (f > 2700 && f <= 2900)
                                code = "07";
                            else if (f > 2900 && f <= 3200)
                                code = "08";
                            else if (f > 3200 && f <= 3700)
                                code = "09";
                            else if (f > 3700 && f <= 4500)
                                code = "10";
                            else if (f > 4500)
                                code = "11";
                            break;
                        case "SL":
                            if (f >= 0 && f <= 25)
                                code = "00";
                            else if (f > 25 && f <= 75)
                                code = "01";
                            else if (f > 75 && f <= 150)
                                code = "02";
                            else if (f > 150 && f <= 250)
                                code = "03";
                            else if (f > 250 && f <= 350)
                                code = "04";
                            else if (f > 350 && f <= 450)
                                code = "05";
                            else if (f > 450 && f <= 550)
                                code = "06";
                            else if (f > 550 && f <= 650)
                                code = "07";
                            else if (f > 650 && f <= 750)
                                code = "08";
                            else if (f > 750 && f <= 850)
                                code = "09";
                            else if (f > 850 && f <= 950)
                                code = "10";
                            else if (f > 950)
                                code = "11";
                            break;
                    }
                    break;
                case DSPMode.AM:
                case DSPMode.DRM:
                case DSPMode.DSB:
                case DSPMode.FM:
                case DSPMode.SAM:
                    switch (n)
                    {
                        case "SH":
                            if (f >= 0 && f <= 2750)
                                code = "00";
                            else if (f > 2750 && f <= 3500)
                                code = "01";
                            else if (f > 3500 && f <= 4500)
                                code = "02";
                            else if (f > 4500)
                                code = "03";
                            break;
                        case "SL":
                            if (f >= 0 && f <= 50)
                                code = "00";
                            else if (f > 50 && f <= 150)
                                code = "01";
                            else if (f > 150 && f <= 350)
                                code = "02";
                            else if (f > 350)
                                code = "03";
                            break;
                    }
                    break;
            }
            return code;
        }

        // Converts a frequency code pair to frequency in hz according to
        // the Kenwood TS-2000 spec.  Receives code and calling methd as parameters
        private int Code2Frequency(string c, string n)
        {
            int freq = 0;
            string mode = "SSB";
            int fgroup = 0;

            // Get the current console mode
            switch (console.RX1DSPMode)
            {
                case DSPMode.LSB:
                case DSPMode.USB:
                    break;
                case DSPMode.AM:
                case DSPMode.DSB:
                case DSPMode.DRM:
                case DSPMode.FM:
                case DSPMode.SAM:
                    mode = "DSB";
                    break;
            }
            // Get the frequency group(SSB/SL, SSB/SH, DSB/SL, and DSB/SH)
            switch (n)
            {
                case "SL":
                    if (mode == "SSB")
                        fgroup = 1;
                    else
                        fgroup = 3;
                    break;
                case "SH":
                    if (mode == "SSB")
                        fgroup = 2;
                    else
                        fgroup = 4;
                    break;
            }
            // return the frequency for the current DSP mode and calling method
            switch (fgroup)
            {
                case 1:     //SL SSB
                    switch (c)
                    {
                        case "00":
                            freq = 0;
                            break;
                        case "01":
                            freq = 50;
                            break;
                        case "02":
                            freq = 100;
                            break;
                        case "03":
                            freq = 200;
                            break;
                        case "04":
                            freq = 300;
                            break;
                        case "05":
                            freq = 400;
                            break;
                        case "06":
                            freq = 500;
                            break;
                        case "07":
                            freq = 600;
                            break;
                        case "08":
                            freq = 700;
                            break;
                        case "09":
                            freq = 800;
                            break;
                        case "10":
                            freq = 900;
                            break;
                        case "11":
                            freq = 1000;
                            break;
                    }
                    break;
                case 2:     //SH SSB
                    switch (c)
                    {
                        case "00":
                            freq = 1400;
                            break;
                        case "01":
                            freq = 1600;
                            break;
                        case "02":
                            freq = 1800;
                            break;
                        case "03":
                            freq = 2000;
                            break;
                        case "04":
                            freq = 2200;
                            break;
                        case "05":
                            freq = 2400;
                            break;
                        case "06":
                            freq = 2600;
                            break;
                        case "07":
                            freq = 2800;
                            break;
                        case "08":
                            freq = 3000;
                            break;
                        case "09":
                            freq = 3400;
                            break;
                        case "10":
                            freq = 4000;
                            break;
                        case "11":
                            freq = 5000;
                            break;
                    }
                    break;
                case 3:     //SL DSB
                    switch (c)
                    {
                        case "00":
                            freq = 0;
                            break;
                        case "01":
                            freq = 100;
                            break;
                        case "02":
                            freq = 200;
                            break;
                        case "03":
                            freq = 500;
                            break;
                    }
                    break;
                case 4:     //SH DSB
                    switch (c)
                    {
                        case "00":
                            freq = 2500;
                            break;
                        case "01":
                            freq = 3000;
                            break;
                        case "02":
                            freq = 4000;
                            break;
                        case "03":
                            freq = 5000;
                            break;
                    }
                    break;
            }
            return freq;
            #region old code

            //			int freq = 0;
            //			switch(console.RX1DSPMode)
            //			{
            //				case DSPMode.CWL:
            //				case DSPMode.CWU:
            //				case DSPMode.LSB:
            //				case DSPMode.USB:
            //				{
            //					switch(c)	//c = filter code, n = SH or SL
            //					{
            //						case "00":
            //							if(n == "SL")
            //								freq = 10;
            //							else
            //								freq = 1400;
            //							break;
            //						case "01":
            //							if(n == "SL")
            //								freq = 50;
            //							else
            //								freq = 1600;
            //							break;
            //						case "02":
            //							if(n == "SL")
            //								freq = 100;
            //							else
            //								freq = 1800;
            //							break;
            //						case "03":
            //							if(n == "SL")
            //								freq = 200;
            //							else
            //								freq = 2000;
            //							break;
            //						case "04":
            //							if(n == "SL")
            //								freq = 300;
            //							else
            //								freq = 2200;
            //							break;
            //						case "05":
            //							if(n == "SL")
            //								freq = 400;
            //							else
            //								freq = 2400;
            //							break;
            //						case "06":
            //							if(n == "SL")
            //								freq = 500;
            //							else
            //								freq = 2600;
            //							break;
            //						case "07":
            //							if(n == "SL")
            //								freq = 600;
            //							else
            //								freq = 2800;
            //							break;
            //						case "08":
            //							if(n == "SL")
            //								freq = 700;
            //							else
            //								freq = 3000;
            //							break;
            //						case "09":
            //							if(n == "SL")
            //								freq = 800;
            //							else
            //								freq = 3400;
            //							break;
            //						case "10":
            //							if(n == "SL")
            //								freq = 900;
            //							else
            //								freq = 4000;
            //							break;
            //						case "11":
            //							if(n == "SL")
            //								freq = 1000;
            //							else
            //								freq = 5000;
            //							break;
            //						default:
            //							break;
            //					}
            //					break;
            //				}
            //				case DSPMode.AM:
            //				case DSPMode.DRM:
            //				case DSPMode.DSB:
            //				case DSPMode.FMN:
            //				case DSPMode.SAM:
            //				{
            //					switch(c)
            //					{
            //						case "00":
            //							if(n == "SL")
            //								freq = 10;
            //							else
            //								freq = 2500;
            //							break;
            //						case "01":
            //							if(n == "SL")
            //								freq = 100;
            //							else
            //								freq = 3000;
            //							break;
            //						case "02":
            //							if(n == "SL")
            //								freq = 200;
            //							else
            //								freq = 4000;
            //							break;
            //						case "03":
            //							if(n == "SL")
            //								freq = 500;
            //							else
            //								freq = 5000;
            //							break;
            //					}
            //				}
            //				break;
            //			}
            //			return freq;
            #endregion old code
        }

        private void SetFilter(string c, string n)
        {
            // c = code, n = SH or SL
            console.RX1Filter = Filter.VAR1;
            int freq = 0;
            int offset = 0;
            string code;

            switch (console.RX1DSPMode)
            {
                case DSPMode.USB:
                case DSPMode.CWU:
                    freq = Code2Frequency(c, n);
                    if (n == "SH")
                        console.RX1FilterHigh = freq;   //split the bandwidth at the center frequency
                    else
                        console.RX1FilterLow = freq;
                    break;
                case DSPMode.LSB:
                case DSPMode.CWL:
                    if (n == "SH")
                    {
                        freq = Code2Frequency(c, "SH"); // get the upper limit from the lower value set
                        console.RX1FilterLow = -freq;   // since we need the more positive value
                    }                                       // closest to the center freq in lsb modes
                    else
                    {
                        freq = Code2Frequency(c, "SL"); // do the reverse here, the less positive value
                        console.RX1FilterHigh = -freq; // is away from the center freq
                    }
                    break;
                case DSPMode.AM:
                case DSPMode.DRM:
                case DSPMode.DSB:
                case DSPMode.FM:
                case DSPMode.SAM:
                    if (n == "SH")
                    {
                        // Set the bandwith equally across the center freq
                        freq = Code2Frequency(c, "SH");
                        console.RX1FilterHigh = freq / 2;
                        console.RX1FilterLow = -freq / 2;
                    }
                    else
                    {
                        // reset the frequency to the nominal value (in case it's been changed)
                        freq = console.RX1FilterHigh * 2;
                        code = Frequency2Code(freq, "SH");
                        freq = Code2Frequency(code, "SH");
                        console.RX1FilterHigh = freq / 2;
                        console.RX1FilterLow = -freq / 2;
                        // subtract the SL value from the lower half of the bandwidth
                        offset = Code2Frequency(c, "SL");
                        console.RX1FilterLow = console.RX1FilterLow + offset;
                    }
                    break;
            }
        }

        #endregion Filter Methods

        #region Mode Methods

        public void String2Mode(string pIndex)
        {
            string s = pIndex;

            switch (s)
            {
                case "00":
                    console.RX1DSPMode = DSPMode.LSB;
                    break;
                case "01":
                    console.RX1DSPMode = DSPMode.USB;
                    break;
                case "02":
                    console.RX1DSPMode = DSPMode.DSB;
                    break;
                case "03":
                    console.RX1DSPMode = DSPMode.CWL;
                    break;
                case "04":
                    console.RX1DSPMode = DSPMode.CWU;
                    break;
                case "05":
                    console.RX1DSPMode = DSPMode.FM;
                    break;
                case "06":
                    console.RX1DSPMode = DSPMode.AM;
                    break;
                case "07":
                    console.RX1DSPMode = DSPMode.DIGU;
                    break;
                case "08":
                    console.RX1DSPMode = DSPMode.SPEC;
                    break;
                case "09":
                    console.RX1DSPMode = DSPMode.DIGL;
                    break;
                case "10":
                    console.RX1DSPMode = DSPMode.SAM;
                    break;
                case "11":
                    console.RX1DSPMode = DSPMode.DRM;
                    break;
            }
        }

        public string Mode2String(DSPMode pMode)
        {
            DSPMode s = pMode;
            string retval = "";

            switch (s)
            {
                case DSPMode.LSB:
                    retval = "00";
                    break;
                case DSPMode.USB:
                    retval = "01";
                    break;
                case DSPMode.DSB:
                    retval = "02";
                    break;
                case DSPMode.CWL:
                    retval = "03";
                    break;
                case DSPMode.CWU:
                    retval = "04";
                    break;
                case DSPMode.FM:
                    retval = "05";
                    break;
                case DSPMode.AM:
                    retval = "06";
                    break;
                case DSPMode.DIGU:
                    retval = "07";
                    break;
                case DSPMode.SPEC:
                    retval = "08";
                    break;
                case DSPMode.DIGL:
                    retval = "09";
                    break;
                case DSPMode.SAM:
                    retval = "10";
                    break;
                case DSPMode.DRM:
                    retval = "11";
                    break;
                default:
                    retval = parser.Error1;
                    break;
            }

            return retval;
        }

        // converts Kenwood single digit mode code to SDR mode
        public void KString2Mode(string pIndex)
        {
            string s = pIndex;

            switch (s)
            {
                case "1":
                    if (console.setupForm.DigUIsUSB)
                        console.RX1DSPMode = DSPMode.DIGL;
                    else
                        console.RX1DSPMode = DSPMode.LSB;
                    break;
                case "2":
                    if (console.setupForm.DigUIsUSB)
                        console.RX1DSPMode = DSPMode.DIGU;
                    else
                        console.RX1DSPMode = DSPMode.USB;
                    break;
                case "3":
                    console.RX1DSPMode = DSPMode.CWU;
                    break;
                case "4":
                    console.RX1DSPMode = DSPMode.FM;
                    break;
                case "5":
                    console.RX1DSPMode = DSPMode.AM;
                    break;
                case "6":
                    console.RX1DSPMode = DSPMode.DIGL;
                    break;
                case "7":
                    console.RX1DSPMode = DSPMode.CWL;
                    break;
                case "9":
                    console.RX1DSPMode = DSPMode.DIGU;
                    break;
                default:
                    console.RX1DSPMode = DSPMode.USB;
                    break;
            }
        }

        // converts SDR mode to Kenwood single digit mode code
        public string Mode2KString(DSPMode pMode)
        {
            DSPMode s = pMode;
            string retval = "";

            switch (s)
            {
                case DSPMode.LSB:
                    retval = "1";
                    break;
                case DSPMode.USB:
                    retval = "2";
                    break;
                case DSPMode.CWU:
                    retval = "3";
                    break;
                case DSPMode.FM:
                    retval = "4";
                    break;
                case DSPMode.AM:
                    //				case DSPMode.SAM:		//possible fix for SAM problem
                    retval = "5";
                    break;
                case DSPMode.DIGL:
                    if (console.setupForm.DigUIsUSB)
                        retval = "1";
                    else
                        retval = "6";
                    break;
                case DSPMode.CWL:
                    retval = "7";
                    break;
                case DSPMode.DIGU:
                    if (console.setupForm.DigUIsUSB)
                        retval = "2";
                    else
                        retval = "9";
                    break;
                default:
                    retval = parser.Error1;
                    break;
            }

            return retval;
        }

        #endregion Mode Methods

        #region Band Methods

        private void MakeBandList()
        //Construct an array of the PowerSDR.Band enums.
        //If the 2m xverter is present, set the last index to B2M
        //otherwise, set it to B6M.
        {
            int ndx = 0;
            BandList = new Band[(int)Band.LAST + 2];

            foreach (Band b in Enum.GetValues(typeof(Band)))
            {
                BandList.SetValue(b, ndx);
                ndx++;
            }
            if (console.XVTRPresent)
                LastBandIndex = Array.IndexOf(BandList, Band.B2M);
            else
                LastBandIndex = Array.IndexOf(BandList, Band.B6M);
        }

        private void SetBandGroup(int band)
        {
            int oldval = parser.nSet;
            parser.nSet = 1;
            if (band == 0)
                ZZBG("0"); //HF
            else
                ZZBG("1"); // VHF

            parser.nSet = oldval;
        }

        private string GetBand(string b)
        {
            if (b.Length == parser.nSet)
            {
                if (b.StartsWith("V") || b.StartsWith("v"))
                    SetBandGroup(1);
                else
                    SetBandGroup(0);
            }

            if (b.Length == parser.nSet)
            {
                console.SetCATBand(String2Band(b));
                return "";
            }
            else if (b.Length == parser.nGet)
            {
                return Band2String(console.RX1Band);
            }
            else
            {
                return parser.Error1;
            }


        }

        private void BandUp()
        {
            Band nextband;
            Band current = console.RX1Band;
            int currndx = Array.IndexOf(BandList, current);

            if (currndx == LastBandIndex) nextband = BandList[0];
            else nextband = BandList[currndx + 1];

            console.SetCATBand(nextband);
        }

        private void BandDown()
        {
            Band nextband;
            Band current = console.RX1Band;
            int currndx = Array.IndexOf(BandList, current);

            if (currndx > 0) nextband = BandList[currndx - 1];
            else nextband = BandList[LastBandIndex];

            console.SetCATBand(nextband);

        }

        private string Band2String(Band pBand)
        {
            Band band = pBand;
            string retval;

            switch (band)
            {
                case Band.GEN:
                    retval = "888";
                    break;
                case Band.B160M:
                    retval = "160";
                    break;
                case Band.B60M:
                    retval = "060";
                    break;
                case Band.B80M:
                    retval = "080";
                    break;
                case Band.B40M:
                    retval = "040";
                    break;
                case Band.B30M:
                    retval = "030";
                    break;
                case Band.B20M:
                    retval = "020";
                    break;
                case Band.B17M:
                    retval = "017";
                    break;
                case Band.B15M:
                    retval = "015";
                    break;
                case Band.B12M:
                    retval = "012";
                    break;
                case Band.B10M:
                    retval = "010";
                    break;
                case Band.B6M:
                    retval = "006";
                    break;
                case Band.B2M:
                    retval = "002";
                    break;
                case Band.WWV:
                    retval = "999";
                    break;
                case Band.VHF0:
                    retval = "V00";
                    break;
                case Band.VHF1:
                    retval = "V01";
                    break;
                case Band.VHF2:
                    retval = "V02";
                    break;
                case Band.VHF3:
                    retval = "V03";
                    break;
                case Band.VHF4:
                    retval = "V04";
                    break;
                case Band.VHF5:
                    retval = "V05";
                    break;
                case Band.VHF6:
                    retval = "V06";
                    break;
                case Band.VHF7:
                    retval = "V07";
                    break;
                case Band.VHF8:
                    retval = "V08";
                    break;
                case Band.VHF9:
                    retval = "V09";
                    break;
                case Band.VHF10:
                    retval = "V10";
                    break;
                case Band.VHF11:
                    retval = "V11";
                    break;
                case Band.VHF12:
                    retval = "V12";
                    break;
                case Band.VHF13:
                    retval = "V13";
                    break;
                default:
                    retval = "888";
                    break;
            }
            return retval;
        }

        private Band String2Band(string pBand)
        {
            string band = pBand.ToUpper(); ;
            Band retval;

            switch (band)
            {
                case "888":
                    retval = Band.GEN;
                    break;
                case "160":
                    retval = Band.B160M;
                    break;
                case "060":
                    retval = Band.B60M;
                    break;
                case "080":
                    retval = Band.B80M;
                    break;
                case "040":
                    retval = Band.B40M;
                    break;
                case "030":
                    retval = Band.B30M;
                    break;
                case "020":
                    retval = Band.B20M;
                    break;
                case "017":
                    retval = Band.B17M;
                    break;
                case "015":
                    retval = Band.B15M;
                    break;
                case "012":
                    retval = Band.B12M;
                    break;
                case "010":
                    retval = Band.B10M;
                    break;
                case "006":
                    retval = Band.B6M;
                    break;
                case "002":
                    retval = Band.B2M;
                    break;
                case "999":
                    retval = Band.WWV;
                    break;
                case "V00":
                    retval = Band.VHF0;
                    break;
                case "V01":
                    retval = Band.VHF1;
                    break;
                case "V02":
                    retval = Band.VHF2;
                    break;
                case "V03":
                    retval = Band.VHF3;
                    break;
                case "V04":
                    retval = Band.VHF4;
                    break;
                case "V05":
                    retval = Band.VHF5;
                    break;
                case "V06":
                    retval = Band.VHF6;
                    break;
                case "V07":
                    retval = Band.VHF7;
                    break;
                case "V08":
                    retval = Band.VHF8;
                    break;
                case "V09":
                    retval = Band.VHF9;
                    break;
                case "V10":
                    retval = Band.VHF10;
                    break;
                case "V11":
                    retval = Band.VHF11;
                    break;
                case "V12":
                    retval = Band.VHF12;
                    break;
                case "V13":
                    retval = Band.VHF13;
                    break;
                default:
                    retval = Band.GEN;
                    break;
            }
            return retval;
        }

        #endregion Band Methods

        #region Step Methods

        private double Step2Freq(int step)
        {
            double freq = 0.0;
            switch (step)
            {
                case 0:
                    freq = 0.000001;
                    break;
                case 1:
                    freq = 0.000010;
                    break;
                case 2:
                    freq = 0.000050;
                    break;
                case 3:
                    freq = 0.000100;
                    break;
                case 4:
                    freq = 0.000250;
                    break;
                case 5:
                    freq = 0.000500;
                    break;
                case 6:
                    freq = 0.001000;
                    break;
                case 7:
                    freq = 0.005000;
                    break;
                case 8:
                    freq = 0.009000;
                    break;
                case 9:
                    freq = 0.010000;
                    break;
                case 10:
                    freq = 0.100000;
                    break;
                case 11:
                    freq = 0.250000;
                    break;
                case 12:
                    freq = 0.500000;
                    break;
                case 13:
                    freq = 1.000000;
                    break;
                case 14:
                    freq = 10.000000;
                    break;
            }
            return freq;
        }

        private string Step2String(int pSize)
        {
            // Modified 2/25/07 to accomodate changes to console where odd step sizes added.  BT
            string stepval = "";
            int step = pSize;
            switch (step)
            {
                case 0:
                    stepval = "0000";   //10e0 = 1 hz
                    break;
                case 1:
                    stepval = "0001";   //10e1 = 10 hz
                    break;
                case 2:
                    stepval = "1000";   //special default for 50 hz
                    break;
                case 3:
                    stepval = "0010";   //10e2 = 100 hz
                    break;
                case 4:
                    stepval = "1001";   //special default for 250 hz
                    break;
                case 5:
                    stepval = "1010";   //10e3 = 1 kHz default for 500 hz
                    break;
                case 6:
                    stepval = "0011";   //10e3 = 1 kHz
                    break;
                case 7:
                    stepval = "1011";   //special default for 5 kHz
                    break;
                case 8:
                    stepval = "1100";   //special default for 9 kHz
                    break;
                case 9:
                    stepval = "0100";   //10e4 = 10 khZ
                    break;
                case 10:
                    stepval = "0101";   //10e5 = 100 kHz
                    break;
                case 11:
                    stepval = "1101";   //special default for 250 kHz
                    break;
                case 12:
                    stepval = "1110";   //special default for 500 kHz
                    break;
                case 13:
                    stepval = "0110";   //10e6 = 1 mHz
                    break;
                case 14:
                    stepval = "0111";   //10e7 = 10 mHz
                    break;
            }
            return stepval;
        }

        #endregion Step Methods

        #region Meter Methods

        private void String2RXMeter(int m)
        {
            console.CurrentMeterRXMode = (MeterRXMode)m;
        }

        private string RXMeter2String()
        {
            return ((int)console.CurrentMeterRXMode).ToString();
        }

        private void String2TXMeter(int m)
        {
            console.CurrentMeterTXMode = (MeterTXMode)m;
        }

        private string TXMeter2String()
        {
            return ((int)console.CurrentMeterTXMode).ToString();
        }

        #endregion Meter Methods

        #region Rig ID Methods

        private string CAT2RigType()
        {
            return "";
        }

        private string RigType2CAT()
        {
            return "";
        }

        #endregion Rig ID Methods

        #region DSP Filter Size Methods

        private string Width2Index(int txt)
        {
            string ans = "";
            switch (txt)
            {
                case 256:
                    ans = "0";
                    break;
                case 512:
                    ans = "1";
                    break;
                case 1024:
                    ans = "2";
                    break;
                case 2048:
                    ans = "3";
                    break;
                case 4096:
                    ans = "4";
                    break;
                default:
                    ans = "0";
                    break;
            }
            return ans;
        }

        private int Index2Width(string ndx)
        {
            int ans;
            switch (ndx)
            {
                case "0":
                    ans = 256;
                    break;
                case "1":
                    ans = 512;
                    break;
                case "2":
                    ans = 1024;
                    break;
                case "3":
                    ans = 2048;
                    break;
                case "4":
                    ans = 4096;
                    break;
                default:
                    ans = 256;
                    break;
            }
            return ans;
        }

        #endregion DSP Filter Size Methods

        #endregion Helper methods
    }
}

