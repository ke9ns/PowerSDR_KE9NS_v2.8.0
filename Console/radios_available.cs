//=================================================================
// radios_available.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace PowerSDR
{
    /// <summary>
    /// Represents the collection of currently available hardware radios
    /// </summary>
    public class RadiosAvailable
    {
        private static ThreadedBindingList<Radio> list = new ThreadedBindingList<Radio>();
        public static ThreadedBindingList<Radio> RadioList
        {
            get
            {
                return list;
            }
        }

        public static void Init()
        {
            // this method is here to force the list to be created in the GUI thread
            int i = 0;
            i++;
        }

        private static List<IntPtr> pending_1500_list = new List<IntPtr>();
        public static void AddPending1500(IntPtr key)
        {
            pending_1500_list.Add(key);
        }

        public static bool AddRadio(Radio radio)
        {
            foreach (Radio r in list)
            {
                // if radio is already in the list, update it
                if (r.SerialNumber == radio.SerialNumber &&
                    r.Model == radio.Model)
                {
                    if (!r.Present)
                    {
                        r.Present = radio.Present;
                        if (radio.AccessObj != null)
                            r.AccessObj = radio.AccessObj;
                    }
                    return false;
                }
            }

            list.Add(radio);

            return true;
        }

        public static bool AddRadios(Radio[] radios)
        {
            foreach (Radio radio in radios)
                AddRadio(radio);
            return true;
        }

        public static bool RadioDetach(Radio radio)
        {
            foreach (Radio r in list)
            {
                // if radio is in the list, make present = false
                if (r.Model == radio.Model)
                {
                    bool match = false;
                    switch (r.Model)
                    {
                        case Model.FLEX1500:
                            if (r.AccessObj != null && radio.AccessObj != null)
                            {
                                if ((IntPtr)r.AccessObj == (IntPtr)radio.AccessObj)
                                    match = true;
                            }
                            break;
                        case Model.FLEX5000:
                            if (radio.AccessObj != null)
                            {
                                if ((uint)r.AccessObj == (uint)radio.AccessObj)
                                    match = true;
                            }
                            break;
                    }

                    if (match)
                    {
                        r.Present = false;
                        r.AccessObj = null;
                        return true;
                    }
                }
            }

            return false;
        }

        public static void Clear()
        {
            list = null;
        }

        public static int NumDetected
        {
            get
            {
                if (list == null) return 0;
                return list.Count;
            }
        }

        public static int NumPresent
        {
            get
            {
                if (list == null || list.Count == 0)
                    return 0;

                int ret_val = 0;

                foreach (Radio r in list)
                {
                    switch (r.Model)
                    {
                        case Model.FLEX5000:
                        case Model.FLEX3000:
                        case Model.FLEX1500:
                            if (r.Present) ret_val++;
                            break;
                        case Model.SDR1000:
                        case Model.SOFTROCK40:
                            ret_val++;
                            break;
                    }
                }

                return ret_val;
            }
        }

        public static bool ScanPal()
        {
            Radio[] radios = Pal.Scan();
            if (radios == null)
            {
                foreach (Radio r in list)
                {
                    if (r.Model == Model.FLEX5000 || r.Model == Model.FLEX3000)
                        r.Present = false;
                }
                return false;
            }

            RadiosAvailable.AddRadios(radios);

            foreach (Radio r in list)
            {
                if (r.Model == Model.FLEX5000 || r.Model == Model.FLEX3000)
                {
                    bool match = false;
                    for (int i = 0; i < radios.Length; i++)
                    {
                        if (r.SerialNumber == radios[i].SerialNumber)
                        {
                            match = true;
                            break;
                        }
                    }

                    if (!match)
                        r.Present = false;
                }
            }

            return true;
        }

        public static void Scan1500()
        {
            foreach (IntPtr key in pending_1500_list)
            {
                string sn = "";

                for (int i = 0; i < 20; i++)
                {
                    sn = Flex1500.GetSerial(key);
                    if (sn != "0000-0000" && sn != "")
                        break;
                    Thread.Sleep(200);
                }

                if (sn == "1234-1234") continue; // ignore sig gen
                if (sn == "0000-0000") continue; // skip radio -- communication issue

                bool found = false;

                foreach (Radio r in list)
                {
                    if (sn == r.SerialNumber)
                    {
                        r.AccessObj = key;
                        r.Present = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    AddRadio(new Radio(Model.FLEX1500, key, sn, true));
            }

            pending_1500_list.Clear();
        }
    }
}