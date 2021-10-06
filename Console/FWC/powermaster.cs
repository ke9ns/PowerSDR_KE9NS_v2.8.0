//=================================================================
// powermaster.cs
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

// ke9ns:
// COM port 38400 8N1
// change modes on PowerMaster (mode button pushed) in hex: F = 02 4D 30 03 34 35 (0D 0A), M = 02 4D 31 03 46 34 (0D 0A),  S = 02 4D 32 03 39 36 (0D 0A),  L = 02 4D 33 03 32 37 (0D 0A),   V = 02 4D 34 03 35 32 (0D 0A)
// if you send: 02 44 33 03 31 33 <cr>  in text: _D3_13 <CR>  (02 and 03 have no visable characters)

// PowerMaster will start transmitting until forever:
// D,    0.0,    0.0, 0.00,0;0;0;0;006
// 02 44 2C 20 20 20 20   30 2E 30 2C 20 20 20 20 30 2e 30 2c 20 30 2e 30 30 2c 30 3b 30 3b 30 3b 30 3b 20 03   30 36
// zeros and space taken up by real values during testing.
// foward, reverse, swr, ?;?;?;?;? CRC? CRC?

// 
//D,   29.5,    0.6, 1.32,0;0;0;0;0BE
//D,   29.1,    0.5, 1.31,0;0;0;0;071
//D,   25.4,    0.5, 1.31,0;0;0;0;0F4
/*
If you want to emulate a PowerMaster this is what I found:


Its encoded like this:
0x02 0x44, Fwd Power, Rev power, SWR,0;0;0;0;0 0x03

The 06 is the CRC generated starting with the 2nd byte (44) and ending just before the 0x03 
SO 06 will change as values in the string change from all 0's.

You can refer to the PowerSDR source code powermaster.cs file for the CRC info


SO.... if I transmit 25.4 watts out at 1.31 SWR (on my G5RV) I have .5 watts ref and the CRC is F4

So the Powermaster sends this string:

"D,   25.4,    0.5, 1.31,0;0;0;0;0F4"

(I think the last 5 0's are not used for anything.)


PowerSDR only checks for a string (from the powermaster that is over 10 chars long) and has a D as the second character.
Then extracts the Floating number taking the 3rd char to the 7th    xxxxx.x is the value
Everything else is ignored

So if you place the powermaster.txt file in the programdata/flexradio/powersdr folder and place 0's for the correction data for all the bands
and write code to allow an external watt meter to send serial data to PowerSDR like 0x02+"D,00123.4E" + 0x03 where 00123.4 is the watts


 */


using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace PowerSDR
{
    public class PowerMaster
    {
        private SerialPort com_port;

        public PowerMaster(string s, bool p)
        {
            if (s.StartsWith("COM"))
                s = s.Substring(3);
            int port = int.Parse(s);
            InitComPort(port, p);
        }

        public PowerMaster(int port , bool p)
        {
            InitComPort(port, p);
        }

        private bool closing = false;
        public void Close()
        {
            closing = true;
        }

        private void InitComPort(int port, bool pm2)
        {
            com_port = new SerialPort();
            com_port.Encoding = System.Text.Encoding.ASCII;
            com_port.RtsEnable = true; // hack for soft rock ptt 
            com_port.DtrEnable = false; // set dtr off 
            com_port.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
            com_port.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            com_port.PinChanged += new SerialPinChangedEventHandler(SerialPort_PinChanged);
            com_port.PortName = "COM" + port.ToString();
            com_port.Parity = Parity.None;
            com_port.StopBits = StopBits.One;
            com_port.DataBits = 8;
            com_port.BaudRate = 38400;
            com_port.ReadTimeout = 5000;
            com_port.WriteTimeout = 500;
            com_port.ReceivedBytesThreshold = 1;
            com_port.Open();

            byte[] out_buffer = new byte[7];
            out_buffer[0] = 0x02;

            if (pm2) // ke9ns add: .212 if Powermaster 2
            {
                Encoding.ASCII.GetBytes("D1", 0, 2, out_buffer, 1); // ke9ns: D = Data out 3 =140 msec per reading automatically output (4=280msec per)
                out_buffer[3] = 0x03;
                //  byte crc = CRC(out_buffer);
                out_buffer[4] = 0x43;  // ByteToAscii((byte)(crc >> 4)); // ke9ns 13 >> 4 = 1 which in ascii = 49dec  
                out_buffer[5] = 0x30;  //ByteToAscii((byte)(crc & 0x0F)); // ke9ns 1 & F = 3 in ascii = 51dec
                out_buffer[6] = 0x0D;
            }
            else
            {
                Encoding.ASCII.GetBytes("D3", 0, 2, out_buffer, 1); // ke9ns: D = Data out 3 =140 msec per reading automatically output (4=280msec per)
                out_buffer[3] = 0x03;
                byte crc = CRC(out_buffer);
                out_buffer[4] = ByteToAscii((byte)(crc >> 4)); // ke9ns 13 >> 4 = 1 which in ascii = 49dec  
                out_buffer[5] = ByteToAscii((byte)(crc & 0x0F)); // ke9ns 1 & F = 3 in ascii = 51dec
                out_buffer[6] = 0x0D;
            }



            com_port.Write(out_buffer, 0, 7); // ke9ns: 02 44 33 03 CRC CRC 0D (in hex)
   

            t1.Start();

        } // InitComPort(int port)

        // D0 – data off		$02 $44 $30 $03 $37 $31 $0D 
        // D1 – data on		$02 $44 $31 $03 $43 $30 $0D 

        public void PMClose()
        {
            byte[] out_buffer = new byte[7];
            out_buffer[0] = 0x02;


            Encoding.ASCII.GetBytes("D0", 0, 2, out_buffer, 1); // ke9ns: D = Data out 3 =140 msec per reading automatically output (4=280msec per)
            out_buffer[3] = 0x03;
          
            out_buffer[4] = 0x37; // ByteToAscii((byte)(crc >> 4)); // ke9ns 13 >> 4 = 1 which in ascii = 49dec  
            out_buffer[5] = 0x31; //  ByteToAscii((byte)(crc & 0x0F)); // ke9ns 1 & F = 3 in ascii = 51dec
            out_buffer[6] = 0x0D;

            com_port.Write(out_buffer, 0, 7); // ke9ns: 02 44 33 03 CRC CRC 0D (in hex)

        }


        private byte CRC(byte[] b)
        {
            byte crc = 0;
            Debug.Assert(b[0] == 0x02);
            for (int i = 1; b[i] != 0x03 && i < b.Length; i++) // ke9ns: b[1]= 0x44 b[2] = 0x33
            {
                crc = crc8revtab[crc ^ b[i]]; // ke9ns: 0 ^ 0x44 = 44 = D6, D6 ^ 0x33  = E5 = EC  bitwise XOR
            }
            return (byte)(~crc); // FF - EC = 13;
        }

        private byte ByteToAscii(byte b)
        {
            byte ret_val = (byte)(b + 0x30);
            if (ret_val > 0x39) ret_val += 7;
            return ret_val;
        }

        private static byte[] crc8revtab =
        {
		//	0x00  0x01  0x02  0x03  0x04  0x05  0x06  0x07  0x08  0x09  0x0A  0x0B  0x0C  0x0D  0x0E  0x0F
/*0x0*/		0x00, 0xB1, 0xD3, 0x62, 0x17, 0xA6, 0xC4, 0x75, 0x2E, 0x9F, 0xFD, 0x4C, 0x39, 0x88, 0xEA, 0x5B,
/*0x1*/		0x5C, 0xED, 0x8F, 0x3E, 0x4B, 0xFA, 0x98, 0x29, 0x72, 0xC3, 0xA1, 0x10, 0x65, 0xD4, 0xB6, 0x07,
/*0x2*/		0xB8, 0x09, 0x6B, 0xDA, 0xAF, 0x1E, 0x7C, 0xCD, 0x96, 0x27, 0x45, 0xF4, 0x81, 0x30, 0x52, 0xE3,
/*0x3*/		0xE4, 0x55, 0x37, 0x86, 0xF3, 0x42, 0x20, 0x91, 0xCA, 0x7B, 0x19, 0xA8, 0xDD, 0x6C, 0x0E, 0xBF,
/*0x4*/		0xC1, 0x70, 0x12, 0xA3, 0xD6, 0x67, 0x05, 0xB4, 0xEF, 0x5E, 0x3C, 0x8D, 0xF8, 0x49, 0x2B, 0x9A,
/*0x5*/		0x9D, 0x2C, 0x4E, 0xFF, 0x8A, 0x3B, 0x59, 0xE8, 0xB3, 0x02, 0x60, 0xD1, 0xA4, 0x15, 0x77, 0xC6,
/*0x6*/		0x79, 0xC8, 0xAA, 0x1B, 0x6E, 0xDF, 0xBD, 0x0C, 0x57, 0xE6, 0x84, 0x35, 0x40, 0xF1, 0x93, 0x22,
/*0x7*/		0x25, 0x94, 0xF6, 0x47, 0x32, 0x83, 0xE1, 0x50, 0x0B, 0xBA, 0xD8, 0x69, 0x1C, 0xAD, 0xCF, 0x7E,
/*0x8*/		0x33, 0x82, 0xE0, 0x51, 0x24, 0x95, 0xF7, 0x46, 0x1D, 0xAC, 0xCE, 0x7F, 0x0A, 0xBB, 0xD9, 0x68,
/*0x9*/		0x6F, 0xDE, 0xBC, 0x0D, 0x78, 0xC9, 0xAB, 0x1A, 0x41, 0xF0, 0x92, 0x23, 0x56, 0xE7, 0x85, 0x34,
/*0xA*/		0x8B, 0x3A, 0x58, 0xE9, 0x9C, 0x2D, 0x4F, 0xFE, 0xA5, 0x14, 0x76, 0xC7, 0xB2, 0x03, 0x61, 0xD0,
/*0xB*/		0xD7, 0x66, 0x04, 0xB5, 0xC0, 0x71, 0x13, 0xA2, 0xF9, 0x48, 0x2A, 0x9B, 0xEE, 0x5F, 0x3D, 0x8C,
/*0xC*/		0xF2, 0x43, 0x21, 0x90, 0xE5, 0x54, 0x36, 0x87, 0xDC, 0x6D, 0x0F, 0xBE, 0xCB, 0x7A, 0x18, 0xA9,
/*0xD*/		0xAE, 0x1F, 0x7D, 0xCC, 0xB9, 0x08, 0x6A, 0xDB, 0x80, 0x31, 0x53, 0xE2, 0x97, 0x26, 0x44, 0xF5,
/*0xE*/		0x4A, 0xFB, 0x99, 0x28, 0x5D, 0xEC, 0x8E, 0x3F, 0x64, 0xD5, 0xB7, 0x06, 0x73, 0xC2, 0xA0, 0x11,
/*0xF*/		0x16, 0xA7, 0xC5, 0x74, 0x01, 0xB0, 0xD2, 0x63, 0x38, 0x89, 0xEB, 0x5A, 0x2F, 0x9E, 0xFC, 0x4D
        };

        private void SerialPort_ErrorReceived(object source, SerialErrorReceivedEventArgs e)
        {

        }

        private void SerialPort_PinChanged(object source, SerialPinChangedEventArgs e)
        {

        }

        private bool present = false;
        public bool Present
        {
            get { return present; }
        }

        private void SerialPort_DataReceived(object source, SerialDataReceivedEventArgs e)
        {
            if (closing)
            {
                try
                {
                    if (com_port != null && com_port.IsOpen)
                        com_port.Close();
                }
                catch (Exception) { }
                com_port = null;
                return;
            }

            while (com_port.BytesToRead > 0)
            {
                // drain buffer
                string s = com_port.ReadLine();

                if (s.Length > 10 && s[1] == 'D')
                {
                    watts = float.Parse(s.Substring(3, 7));
                    rev = float.Parse(s.Substring(11, 7)); // .212 add

                    watts1 = int.Parse(s.Substring(3, 5)); // .212 add dont grab 10th of watss

                    if (!present) present = true;

                    swr = float.Parse(s.Substring(20, 4)); // .212 add
                }
            }
        }

        private static HiPerfTimer t1 = new HiPerfTimer();
        private float watts = 0.0f;
        public float Watts
        {
            get { return watts; }
        }

        private int watts1 =0;
        public int Watts1 // .212 add
        {
            get { return watts1; }
        }

        private float rev = 0.0f;
        public float Rev // .212 add
        {
            get { return rev; }
        }

        private float swr = 0.0f;
        public float SWR // .212 add
        {
            get { return swr; }
        }
    }
}