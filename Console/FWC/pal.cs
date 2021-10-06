//=================================================================
// pal.cs
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


// ke9ns  must be a way to send data over Firewire connection???


using System;
using System.Runtime.InteropServices;

namespace PowerSDR
{
    public class Pal
    {

#if(!NO_PAL)

        [DllImport("pal.dll", EntryPoint = "Init", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Init();				// initialize PAL system


        [DllImport("pal.dll", EntryPoint = "GetNumDevices", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNumDevices();



        [DllImport("pal.dll", EntryPoint = "GetDeviceInfo", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDeviceInfo(uint index, out uint model, out uint sn);



        [DllImport("pal.dll", EntryPoint = "SelectDevice", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SelectDevice(uint index);



        [DllImport("pal.dll", EntryPoint = "WriteOp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteOp(FWC.Opcode opcode, uint data1, uint data2);



        [DllImport("pal.dll", EntryPoint = "WriteOp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteOp(FWC.Opcode opcode, int data1, int data2);



        [DllImport("pal.dll", EntryPoint = "WriteOp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteOp(FWC.Opcode opcode, uint data1, float data2);



        [DllImport("pal.dll", EntryPoint = "WriteOp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WriteOp(FWC.Opcode opcode, float data1, uint data2);



        [DllImport("pal.dll", EntryPoint = "ReadOp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOp(FWC.Opcode opcode, uint data1, uint data2, out uint rtn);



        [DllImport("pal.dll", EntryPoint = "ReadOp", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadOp(FWC.Opcode opcode, uint data1, uint data2, out float rtn);

        [DllImport("pal.dll", EntryPoint = "Exit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Exit();				// cleanup and leave system in a stable state



        [DllImport("pal.dll", EntryPoint = "SetCallback", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCallback(NotificationCallback callback);



        [DllImport("pal.dll", EntryPoint = "SetBufferSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetBufferSize(uint val);



        public delegate void NotificationCallback(uint bitmap);


        public static Radio[] Scan()
        {
            int devs = Pal.GetNumDevices();                 // get numer of radios found 

            //    System.Diagnostics.Trace.WriteLine("pal=============================");

            if (devs == 0) return null;

            Radio[] radios = new Radio[devs];

            for (uint i = 0; i < devs; i++)
            {
                uint model;
                uint sn;

                if(!Pal.GetDeviceInfo(i, out model, out sn)) return null;

                Model m = Model.FLEX5000;
                if (model == 3) m = Model.FLEX3000;

                string serial = FWCEEPROM.SerialToString(sn);   // radios serial#

                radios[i] = new Radio(m, i, serial, true);
            }

            return radios;


        } // Scan()


#else
		public static bool Init()
        {
            return false;
        }

		public static int GetNumDevices()
        {
            return 0;
        }

		public static bool GetDeviceInfo(uint index, out uint model, out uint sn)
        {
            model = 0;
            sn = 0;
            return false;
        }

		public static bool SelectDevice(int index)
        {
            return false;
        }

		public static int WriteOp(FWC.Opcode opcode, uint data1, uint data2)
        {
            return -1;
        }

        public static int WriteOp(FWC.Opcode opcode, int data1, int data2)
        {
            return -1;
        }

		public static int WriteOp(FWC.Opcode opcode, uint data1, float data2)
        {
            return -1;
        }

		public static int WriteOp(FWC.Opcode opcode, float data1, uint data2)
        {
            return -1;
        }

		public static int ReadOp(FWC.Opcode opcode, uint data1, uint data2, out uint rtn)
        {
            rtn = 0;
            return -1;
        }

		public static int ReadOp(FWC.Opcode opcode, uint data1, uint data2, out float rtn)
        {
            rtn = 0;
            return -1;
        }


		public static void Exit() // cleanup and leave system in a stable state
        {
            
        }

		public static void SetCallback(NotificationCallback callback)
        {
            
        }

        public static void SetBufferSize(uint val)
        {

        }

        public static void Scan()
        {

        }

		public delegate void NotificationCallback(uint bitmap); 
#endif
    } // class pal


} // PowerSDR
