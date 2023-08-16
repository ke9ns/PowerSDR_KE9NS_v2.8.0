//=================================================================
// SIOListenerIV.cs .275
// ke9ns add clone of CAT serial port SIOListenerII
// used to communicate with CXAuto Ant switch of DDUtil program
// along with SDRSerialPortIV.cs
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
    public class SIOListenerIV
    {
        #region Constructor

        public SIOListenerIV(Console c)
        {
            console = c;
            console.Activated += new EventHandler(console_Activated);
            console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
            parser = new CATParser(console);

            //event handler for Serial RX Events

            SDRSerialPort7.serial_rx_event7 += new SerialRXEventHandler(SerialRXEventHandler7);

            if (console.CXAutoEnabled)  // if CAT is on, fire it up 
            {
                try
                {
                    enableCXAuto();
                }
                catch (Exception ex)
                {
                   
                    console.CXAutoEnabled = false;
                    if (console.setupForm != null)
                    {
                        console.setupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
                    }
                    MessageBox.Show("Could not initialize CXAuto control.  Exception was:\n\n " + ex.Message +
                        "\n\nCXAuto control has been disabled.", "Error Initializing CXAuto control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        } // SIOListenerIV(Console c)


        public void enableCXAuto() //.275
        {
            if (console.CXAutoPort == 0) return;


            lock (this)
            {
                if (CXAuto_enabled) return; // nothing to do already enabled 
                CXAuto_enabled = true;
            }


            int port_num = console.CXAutoPort;

            Debug.WriteLine("==============CXAuto PORT OPEN: " + port_num);

            SIO7 = new SDRSerialPort7(port_num);

            /*	SIO7.setCommParms(console.CATBaudRate, 
                                console.CATParity, 
                                console.CATDataBits, 
                                console.CATStopBits,
                                console.CATHandshake);
                                */

            SIO7.setCommParms(9600,                            // CXAuto CI-V
                                console.CATParity,
                                console.CATDataBits,
                                console.CATStopBits,
                                console.CATHandshake);

            Initialize();


        } // CXAuto

        public void disableCXAuto() //.275
        {
            lock (this)
            {
                if (!CXAuto_enabled) return; /* nothing to do already disabled */
                CXAuto_enabled = false;
            }

            Debug.WriteLine("==============CXAuto PORT CLOSED");

            if (SIO7 != null)
            {
                SIO7.Destroy();
                SIO7 = null;
            }
            Fpass = true; // reset init flag 
            return;
        } //  disableCXAuto

       

        #endregion Constructor

        #region Variables

        //HiPerfTimer testTimer1 = new HiPerfTimer();
        //HiPerfTimer testTimer2 = new HiPerfTimer();
        public SDRSerialPort7 SIO7;

        Console console;
        ASCIIEncoding AE = new ASCIIEncoding();
        private bool Fpass = true;
                                             //		private System.Timers.Timer SIOMonitor;
        private bool CXAuto_enabled = false; // .275

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
                SIO7.Create();
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
            if (SIO7 != null)
            {
                SIO7.Destroy();
            }
        }

        private void console_Activated(object sender, EventArgs e)
        {
            if (console.CATEnabled)
            {
                // Initialize();   // wjt enable CAT calls Initialize 
                enableCXAuto();
            }
        }

        StringBuilder CommBuffer = new StringBuilder();//"";				//holds incoming serial data from the port
        private void SerialRXEventHandler7(object source, SerialRXEvent e)
        {

            CommBuffer.Append(e.buffer);                                        // put the data in the string

            if (CommBuffer.Length >= 6)
            {

                try
                {
                   
                    console.CXAuto_Rec = CommBuffer.ToString();


                    Debug.WriteLine("COMBUFFER CXAUto: ");
                    Debug.WriteLine("0 " + CommBuffer[0]);
                    Debug.WriteLine("1 " + CommBuffer[1]);
                    Debug.WriteLine("2 " + CommBuffer[2]);
                    Debug.WriteLine("3 " + CommBuffer[3]);
                    Debug.WriteLine("4 " + CommBuffer[4]);
                    Debug.WriteLine("5 " + CommBuffer[5]);
                    Debug.WriteLine("6 " + CommBuffer[6]);

                    Debug.WriteLine("7 " + int.Parse(CommBuffer[5].ToString()).ToString());

                    console.setupForm.txtCXAuto.Text = (int.Parse(CommBuffer[5].ToString()).ToString());


                    console.CXAuto_Ready = true;

                    CommBuffer.Clear();

                }
                catch (Exception)
                {

                }
            }

        } // SerialRXEventHandler7


        #endregion Events
    }
}

