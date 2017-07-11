//=================================================================
// tdxstub.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDxInput
{
#if(NO_TDX)
    public class Sensor
    {
        public Vector3D Translation;
        public AngleAxis Rotation;
    }

    public class Device
    {
        public Sensor Sensor;
        public Keyboard Keyboard;

        public bool Connect()
        {
            return false;
        }

        public bool IsConnected
        {
            get { return false; }
        }
    }

    public class DeviceClass : Device
    {
        
    }

    public class Vector3D
    {
        public double X;
        public double Y;
        public double Z;        
    }

    public class AngleAxis
    {
        public double Angle;
        public double Y;
        public double Z;
    }

    public class Keyboard
    {
        public bool IsKeyDown(int n)
        {
            return false;
        }
    }
#endif
}
