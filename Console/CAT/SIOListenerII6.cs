//=================================================================
// SIOListener.cs ke9ns clone port2
//=================================================================
// Copyright (C) 2006  Bob Tracy
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
// Foundation, Inc., 69 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact the author via email at: k6kdn@arrl.net
//=================================================================

#define DBG_PRINT

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms; // needed for MessageBox (wjt)

namespace PowerSDR
{
    public class SIOListenerII6
    {
        #region Constructor

        public SIOListenerII6(Console c)
        {
            console = c;
            console.Activated += new EventHandler(console_Activated);
            console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
            parser = new CATParser(console);

            //event handler for Serial RX Events
            SDRSerialPort6.serial_rx_event6 += new SerialRXEventHandler(SerialRXEventHandler6);

            if (console.CATEnabled6)  // if CAT is on, fire it up 
            {
                try
                {
                    enableCAT6();
                }
                catch (Exception ex)
                {
                    // fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
                    //  seems ugly to me (wjt) 
                    console.CATEnabled6 = false;

                    if (console.setupForm != null)
                    {
                        console.setupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
                    }
                    MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

           

        }

        

        public void enableCAT6()
        {
            lock (this)
            {
                if (cat_enabled6) return; // nothing to do already enabled 
                cat_enabled6 = true;
            }
            Debug.WriteLine("==============CAT PORT OPEN");

            int port_num = console.CATPort6;

            SIO6 = new SDRSerialPort6(port_num);
            SIO6.setCommParms(console.CATBaudRate,
                            console.CATParity,
                            console.CATDataBits,
                            console.CATStopBits,
                            console.CATHandshake);

            Initialize();
        }

        public bool UseForKeyPTT
        {
            set
            {
                if (SIO6 != null)
                    SIO6.UseForKeyPTT = value;
            }
        }

        public bool UseForPaddles
        {
            set
            {
                if (SIO6 != null)
                    SIO6.UseForPaddles = value;
            }
        }

        public bool PTTOnDTR
        {
            set
            {
                if (SIO6 != null)
                    SIO6.PTTOnDTR = value;
            }
        }

        public bool PTTOnRTS
        {
            set
            {
                if (SIO6 != null)
                    SIO6.PTTOnRTS = value;
            }
        }

        public bool KeyOnDTR
        {
            set
            {
                if (SIO6 != null)
                    SIO6.KeyOnDTR = value;
            }
        }

        public bool KeyOnRTS
        {
            set
            {
                if (SIO6 != null)
                    SIO6.KeyOnRTS = value;
            }
        }


        // typically called when the end user has disabled CAT control through a UI element ... this 
        // closes the serial port and neutralized the listeners we have in place
        public void disableCAT6()
        {
            lock (this)
            {
                if (!cat_enabled6) return; /* nothing to do already disabled */
                cat_enabled6 = false;
            }
            Debug.WriteLine("==============CAT PORT CLOSED");

            if (SIO6 != null)
            {
                SIO6.Destroy();
                SIO6 = null;
            }
            Fpass = true; // reset init flag 
            return;
        }

        #endregion Constructor

        #region Variables

        //HiPerfTimer testTimer1 = new HiPerfTimer();
        //HiPerfTimer testTimer2 = new HiPerfTimer();
        public SDRSerialPort6 SIO6;
        Console console;
        ASCIIEncoding AE = new ASCIIEncoding();
        private bool Fpass = true;
        private bool cat_enabled6 = false;  // is cat currently enabled by user? 
                                           //		private System.Timers.Timer SIOMonitor;
        CATParser parser;
        //		private int SIOMonitorCount = 0;

        #endregion variables

        #region Methods

        private static void dbgWriteLine(string s)
        {
#if (!DBG_PRINT)
			Console.dbgWriteLine("SIOListener: " + s); 
#endif
        }

        // Called when the console is activated for the first time.  
        private void Initialize()
        {
            if (Fpass)
            {
                SIO6.Create();
                Fpass = false;
            }
        }
#if UseParser
		private char[] ParseLeftover = null; 

		// segment incoming string into CAT commands ... handle leftovers from when we read a parial 
		// 
		private void ParseString(byte[] rxdata, uint count) 
		{ 
			if ( count == 0 ) return;  // nothing to do 
			int cmd_char_count = 0; 
			int left_over_char_count = ( ParseLeftover == null ? 0 : ParseLeftover.Length ); 
			char[] cmd_chars = new char[count + left_over_char_count]; 			
			if ( ParseLeftover != null )  // seed with leftovers from last read 
			{ 
				for ( int j = 0; j < left_over_char_count; j++ )  // wjt fixme ... use C# equiv of System.arraycopy 
				{
					cmd_chars[cmd_char_count] = ParseLeftover[j]; 
					++cmd_char_count; 
				}
				ParseLeftover = null; 
			}
			for ( int j = 0; j < count; j++ )   // while we have chars to play with 
			{ 
				cmd_chars[cmd_char_count] = (char)rxdata[j]; 
				++cmd_char_count; 
				if ( rxdata[j] == ';' )  // end of cmd -- parse it and execute it 
				{ 
					string cmdword = new String(cmd_chars, 0, cmd_char_count); 
					dbgWriteLine("cmdword: >" + cmdword + "<");  
					// BT 06/08
					string answer = parser.Get(cmdword);
					byte[] out_string = AE.GetBytes(answer);
					uint result = SIO.put(out_string, (uint) out_string.Length);

					cmd_char_count = 0; // reset word counter 
				}
			} 
			// when we get here have processed all of the incoming buffer, if there's anyting 
			// in cmd_chars we need to save it as we've not pulled a full command so we stuff 
			// it in leftover for the next time we come through 
			if ( cmd_char_count != 0 ) 
			{ 
				ParseLeftover = new char[cmd_char_count]; 
				for ( int j = 0; j < cmd_char_count; j++ )  // wjt fixme ... C# equiv of Sytsem.arraycopy 
				{
					ParseLeftover[j] = cmd_chars[j]; 
				}
			} 
#if DBG_PRINT
			if ( ParseLeftover != null) 
			{
				dbgWriteLine("Leftover >" + new String(ParseLeftover) + "<"); 
			}
#endif
			return; 
		}

#endif

        #endregion Methods

        #region Events

        private void console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SIO6 != null)
            {
                SIO6.Destroy();
            }
        }

        private void console_Activated(object sender, EventArgs e)
        {
            if (console.CATEnabled6)
            {
                // Initialize();   // wjt enable CAT calls Initialize 
                enableCAT6();
            }
        }

        StringBuilder CommBuffer = new StringBuilder();//"";				//holds incoming serial data from the port
        private void SerialRXEventHandler6(object source, SerialRXEvent e)
        {
            //			SIOMonitor.Interval = 6000;		// set the timer for 6 seconds
            //			SIOMonitor.Enabled = true;		// start or restart the timer

            //double T0 = 0.00;
            //double T1 = 0.00;
            //int bufferLen = 0;

            CommBuffer.Append(e.buffer);                                        // put the data in the string
            if (parser != null)                                                 // is the parser instantiated
            {
                //bufferLen = CommBuffer.Length;
                try
                {
                    Regex rex = new Regex(".*?;");                                      //accept any string ending in ;
                    string answer;
                    uint result;

                    StringBuilder temp = new StringBuilder();


                    for (Match m = rex.Match(CommBuffer.ToString()); m.Success; m = m.NextMatch())  //loop thru the buffer and find matches
                    {

                        // ke9ns .200 Spoof VFOA to B
                        // ZZSP READ THE SPLIT BUTTON
                        // ZZSW GET/SET THE VFOA / B TX BUTTON
                        // ZZTV  GET/SET TX VFO WHEN RX2 ON
                        // ZZAI  GET/SET AUTO INFO FUNCTION 
                        // AI
                        // ZZFM READ FLEX MODEL#
                        // ZZID GET/SET TRANCEIVER ID#
                        // ZZXT GET/SET RX2 BUTTON STATUS


                        // DN VFOA down by tune step
                        // FA get/set VFOA     FA00014074000;
                        // FB get/set VFOB
                        // FR get/set RX VFO
                        // FT get/set TX VFO
                        // ID get tranceiver ID  ID900; (900 for Flex-5000)
                        // IF get tranceiver status   IF000140740001010+0000000000020000000; // IF 00014074000 (FA) 1010(ZZST) +00000 0 0 0 00 0 2(MD) 0 0 0 0 00 0;
                        // 
                        // MD get/set tranceiver mode this is only 1 digit not 2
                        // SH get/set RX filter high
                        // SL get/set RX filter low
                        // UP VFOA increment by step size
                        // XT get/set XIT


                        // ZZAD > ZZBD DOWN BY SELECTED STEP
                        // ZZAU > ZZBU UP BY SELECTED STEP
                        // ZZFA > ZZFB GET/SET VFOA NOW B

                        // ZZSA > ZZSG DOWN BY 1 STEP
                        // ZZSB > ZZSH UP BY 1 STEP
                        // ZZMD > ZZME GET/SET RX1 NOW RX2
                        // ZZFH > ZZFR HIGH RX FILTER CUT
                        // ZZFI > ZZFS LOW RX FILTER CUT
                        // ZZOA > ZZOB GET/SET ANT CONNECTED TO RX1 SPOOF RX2

                   
                        Debug.WriteLine("S SIO6----->" + m.Value);

                        console.SpoofAB = true; // ke9ns add: .200 used by IF command in CATCommands.cs


                        console.KWAI6 = true; // .214
                        answer = parser.Get(m.Value);                                   //send the match to the parser
                        console.KWAI6 = false; // .214

                        if (answer.Length > 0)
                            result = SIO6.put(answer);                                   //send the answer to the serial port

                        console.SpoofAB = false;
                        Debug.WriteLine("R SIO6----->" + answer + "<---");

                        CommBuffer = CommBuffer.Replace(m.Value, "", 0, m.Length);                   //remove the match from the buffer
                                                                                                     //Debug.WriteLine("Parser decode time for "+m.Value.ToString()+":  "+T0.ToString()+ "ms");
                                                                                                     //Debug.WriteLine("SIO send answer time:  " + T1.ToString() + "ms");
                                                                                                     //Debug.WriteLine("CommBuffer Length:  " + bufferLen.ToString());
                                                                                                     //if (bufferLen > 100)
                                                                                                     //Debug.WriteLine("Buffer contents:  "+CommBuffer.ToString());
                    }
                }
                catch (Exception)
                {
                    //Add ex name to exception above to enable
                    //Debug.WriteLine("RX Event:  "+ex.Message);
                    //Debug.WriteLine("RX Event:  "+ex.StackTrace);
                }
            }
        }


        #endregion Events
    }
}

