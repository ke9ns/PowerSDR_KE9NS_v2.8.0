//=================================================================
// SIOListenerIII.cs
// ke9ns add clone of CAT serial port SIOListenerII
// used to communicate with ANT rotor port of DDUtil program
// along with SDRSerialPortIII.cs
//=================================================================
// Copyright (C) 2005  Bob Tracy
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

#define DBG_PRINT

using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms; // needed for MessageBox (wjt)

namespace PowerSDR
{
    public class SIOListenerIII
    {
        #region Constructor

        public SIOListenerIII(Console c)
        {
            console = c;
            console.Activated += new EventHandler(console_Activated);
            console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
            parser = new CATParser(console);

            //event handler for Serial RX Events

            SDRSerialPort1.serial_rx_event1 += new SerialRXEventHandler(SerialRXEventHandler1);

            if (console.ROTOREnabled)  // if CAT is on, fire it up 
            {
                try
                {
                    enableROTOR();
                }
                catch (Exception ex)
                {
                    // fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
                    //  seems ugly to me (wjt) 
                    console.ROTOREnabled = false;
                    if (console.setupForm != null)
                    {
                        console.setupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
                    }
                    MessageBox.Show("Could not initialize ROTOR control.  Exception was:\n\n " + ex.Message +
                        "\n\nROTOR control has been disabled.", "Error Initializing ROTOR control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        } // SIOListenerIII(Console c)


        public void enableROTOR()
        {
            if (console.ROTORPort == 0) return;


            lock (this)
            {
                if (rotor_enabled) return; // nothing to do already enabled 
                rotor_enabled = true;
            }


            int port_num = console.ROTORPort;

            Debug.WriteLine("==============ROTOR PORT OPEN: " + port_num);

            SIO1 = new SDRSerialPort1(port_num);

            /*	SIO1.setCommParms(console.CATBaudRate, 
                                console.CATParity, 
                                console.CATDataBits, 
                                console.CATStopBits,
                                console.CATHandshake);
                                */

            SIO1.setCommParms(4800,                            // ant rotor port is always .181 4800 works for hygain ant rotor and ddutil (was 9600 prior)
                                console.CATParity,
                                console.CATDataBits,
                                console.CATStopBits,
                                console.CATHandshake);

            Initialize();
        }


        // typically called when the end user has disabled CAT control through a UI element ... this 
        // closes the serial port and neutralized the listeners we have in place
        public void disableROTOR()
        {
            lock (this)
            {
                if (!rotor_enabled) return; /* nothing to do already disabled */
                rotor_enabled = false;
            }

            Debug.WriteLine("==============ROTOR PORT CLOSED");

            if (SIO1 != null)
            {
                SIO1.Destroy();
                SIO1 = null;
            }
            Fpass = true; // reset init flag 
            return;
        }

        #endregion Constructor

        #region Variables

        //HiPerfTimer testTimer1 = new HiPerfTimer();
        //HiPerfTimer testTimer2 = new HiPerfTimer();
        public SDRSerialPort1 SIO1;

        Console console;
        ASCIIEncoding AE = new ASCIIEncoding();
        private bool Fpass = true;
        private bool rotor_enabled = false;  // is cat currently enabled by user? 
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
                SIO1.Create();
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
					uint result = SIO1.put(out_string, (uint) out_string.Length);

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
            if (SIO1 != null)
            {
                SIO1.Destroy();
            }
        }

        private void console_Activated(object sender, EventArgs e)
        {
            if (console.CATEnabled)
            {
                // Initialize();   // wjt enable CAT calls Initialize 
                enableROTOR();
            }
        }

        StringBuilder CommBuffer = new StringBuilder();//"";				//holds incoming serial data from the port
        private void SerialRXEventHandler1(object source, SerialRXEvent e)
        {

            CommBuffer.Append(e.buffer);                                        // put the data in the string

            if (CommBuffer.Length >= 4)
            {

                try
                {

                    console.RotorAngle = CommBuffer.ToString().TrimStart(';');


                    //  Debug.WriteLine("COMBUFFER2: " + console.RotorAngle);

                    console.RotorAngleRdy = true;

                    CommBuffer.Clear();

                }
                catch (Exception)
                {

                }
            }

        } // SerialRXEventHandler1


        #endregion Events
    }
}

