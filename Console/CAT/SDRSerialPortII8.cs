//=================================================================
// SDRSerialPort.cs ke9ns clone for port 8 (RX A TX B)
// this is for standard CAT COM port Communication
//=================================================================
// Copyright (C) 2006  Bill Tracey
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
//================================================================= 
// Serial port support for PowerSDR support of CAT and serial port control  
//=================================================================

#define DBG_PRINT

using FlexCW;
using System;
using System.Diagnostics;
using System.IO.Ports;

namespace PowerSDR
{
    public class SDRSerialPort8
    {
        public static event SerialRXEventHandler serial_rx_event8;


        private SerialPort commPort;
        public SerialPort BasePort
        {
            get { return commPort; }
        }

        private bool isOpen = false;
        private bool bitBangOnly = false;

        //Added 2/14/2008 BT
        public bool IsOpen
        {
            get { return commPort.IsOpen; }
        }

        public void Open()
        {
            commPort.Open();
        }

        public void Close()
        {
            commPort.Close();
        }

        public static Parity StringToParity(string s)
        {
            if (s == "none") return Parity.None;
            if (s == "odd") return Parity.Odd;
            if (s == "even") return Parity.Even;
            if (s == "space") return Parity.Space;
            if (s == "mark") return Parity.Mark;
            return Parity.None;  // error -- default to none
        }

        public static StopBits StringToStopBits(string s)
        {
            if (s == "0") return StopBits.None;
            if (s == "1") return StopBits.One;
            if (s == "1.6") return StopBits.OnePointFive;
            if (s == "2") return StopBits.Two;
            return StopBits.One; // error -- default 
        }

        public SDRSerialPort8(int portidx)
        {
            commPort = new SerialPort();
            commPort.Encoding = System.Text.Encoding.ASCII;
            commPort.RtsEnable = true; // hack for soft rock ptt 
            commPort.DtrEnable = true; // set dtr off 
            //commPort.ErrorReceived += new SerialErrorReceivedEventHandler(this.SerialErrorReceived);
            commPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialReceivedData8);
            commPort.PinChanged += new SerialPinChangedEventHandler(this.SerialPinChanged);

            commPort.PortName = "COM" + portidx.ToString();

            commPort.Parity = Parity.None;
            commPort.StopBits = StopBits.One;
            commPort.Handshake = Handshake.None;
            commPort.DataBits = 8;
            commPort.BaudRate = 9600;
            commPort.ReadTimeout = 6000;
            commPort.WriteTimeout = 600;
            commPort.ReceivedBytesThreshold = 1;
        }



        // set the comm parms ... can only be done if port is not open -- silently fails if port is open (fixme -- add some error checking) 
        // 
        public void setCommParms(int baudrate, Parity p, int databits, StopBits stop, Handshake handshake)
        {
            if (commPort.IsOpen) return; // bail out if it's already open 

            commPort.BaudRate = baudrate;
            commPort.Parity = p;
            commPort.StopBits = stop;
            commPort.DataBits = databits;
            commPort.Handshake = handshake;
        }

        public uint put(string s)
        {
            if (bitBangOnly) return 0;  // fixme -- throw exception?			
            commPort.Write(s);

            return (uint)s.Length; // wjt fixme -- hack -- we don't know if we actually wrote things 			
        }

        // ke9ns add
        public string put1(string s)
        {
            string answer = "---";

            if (bitBangOnly) return answer;  // fixme -- throw exception?			

            commPort.Write("AI1;");

            try
            {

                byte[] test = new byte[10];

                //  var tes1 = commPort.Read(test, 0, 4);

                //  test[0] = commPort.ReadByte();


                // answer = test.ToString();

                //  Debug.WriteLine("BEAM: " + test[0] + " , " + test[1] + " , " + test[2] + " , " + test[3]);

                // answer = commPort.ReadExisting();

                Debug.WriteLine("BEAM: " + answer);

                //  answer = System.Text.Encoding.Default.GetString(test);

            }
            catch (Exception e)
            {
                //  Debug.WriteLine("BEAM: " + e);
                answer = "===";
            }

            return answer; // wjt fixme -- hack -- we don't know if we actually wrote things 			
        } // put1

        public int Create()
        {
            return Create(false);
        }

        // create port 
        public int Create(bool bit_bang_only)
        {
            bitBangOnly = bit_bang_only;
            if (isOpen) { return -1; }
            commPort.Open();
            isOpen = commPort.IsOpen;
            if (isOpen)
                return 0; // all is well
            else
                return -1;  //error
        }

        public void Destroy()
        {
            try
            {
                commPort.Close();
            }
            catch (Exception)
            {

            }
            isOpen = false;
        }

        public bool isCTS()
        {
            if (!isOpen) return false; // fixme error check 
            return commPort.CtsHolding;
        }

        public bool isDSR()
        {
            if (!isOpen) return false; // fixme error check 
            return commPort.DsrHolding;

        }
        public bool isRI()
        {
            if (!isOpen) return false; // fixme error check 
            return false;
        }

        public bool isRLSD()
        {
            if (!isOpen) return false; // fixme error check 
            return commPort.CDHolding;
        }

        public void setDTR(bool v)
        {
            if (!isOpen) return;
            commPort.DtrEnable = v;
        }

        void SerialErrorReceived(object source, SerialErrorReceivedEventArgs e)
        {

        }

        private bool use_for_keyptt = false;
        public bool UseForKeyPTT
        {
            get { return use_for_keyptt; }
            set { use_for_keyptt = value; }
        }

        private bool use_for_paddles = false;
        public bool UseForPaddles
        {
            get { return use_for_paddles; }
            set { use_for_paddles = value; }
        }

        private bool ptt_on_dtr = false;
        public bool PTTOnDTR
        {
            get { return ptt_on_dtr; }
            set { ptt_on_dtr = value; }
        }

        private bool ptt_on_rts = false;
        public bool PTTOnRTS
        {
            get { return ptt_on_rts; }
            set { ptt_on_rts = value; }
        }

        private bool key_on_dtr = false;
        public bool KeyOnDTR
        {
            get { return key_on_dtr; }
            set { key_on_dtr = value; }
        }

        private bool key_on_rts = false;
        public bool KeyOnRTS
        {
            get { return key_on_rts; }
            set { key_on_rts = value; }
        }

        private static bool reverse_paddles = false;
        public static bool ReversePaddles
        {
            get { return reverse_paddles; }
            set { reverse_paddles = value; }
        }

        void SerialPinChanged(object source, SerialPinChangedEventArgs e)
        {
            if (!use_for_keyptt && !use_for_paddles) return;

            if (use_for_keyptt)
            {
                switch (e.EventType)
                {
                    case SerialPinChange.DsrChanged:
                        bool b = commPort.DsrHolding;

                        if (ptt_on_dtr)
                        {
                            CWPTTItem item = new CWPTTItem(b, CWSensorItem.GetCurrentTime());
                            CWKeyer.PTTEnqueue(item);
                        }

                        if (key_on_dtr)
                        {
                            CWSensorItem item = new CWSensorItem(CWSensorItem.InputType.StraightKey, b);
                            CWKeyer.SensorEnqueue(item);
                        }
                        break;
                    case SerialPinChange.CtsChanged:
                        b = commPort.CtsHolding;

                        if (ptt_on_rts)
                        {
                            CWPTTItem item = new CWPTTItem(b, CWSensorItem.GetCurrentTime());
                            CWKeyer.PTTEnqueue(item);
                        }

                        if (key_on_rts)
                        {
                            CWSensorItem item = new CWSensorItem(CWSensorItem.InputType.StraightKey, b);
                            CWKeyer.SensorEnqueue(item);
                        }
                        break;
                }
            }
            else if (use_for_paddles)
            {
                switch (e.EventType)
                {
                    case SerialPinChange.DsrChanged:
                        CWSensorItem.InputType type = CWSensorItem.InputType.Dot;
                        if (reverse_paddles) type = CWSensorItem.InputType.Dash;
                        CWSensorItem item = new CWSensorItem(type, commPort.DsrHolding);
                        CWKeyer.SensorEnqueue(item);
                        break;
                    case SerialPinChange.CtsChanged:
                        type = CWSensorItem.InputType.Dash;
                        if (reverse_paddles) type = CWSensorItem.InputType.Dot;
                        item = new CWSensorItem(type, commPort.CtsHolding);
                        CWKeyer.SensorEnqueue(item);
                        break;
                }
            }
        }

        void SerialReceivedData8(object source, SerialDataReceivedEventArgs e)
        {
            serial_rx_event8(this, new SerialRXEvent(commPort.ReadExisting()));


        }


    }
}
