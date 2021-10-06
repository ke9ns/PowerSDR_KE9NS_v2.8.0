//=================================================================
// radio.cs
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
using System.ComponentModel;

namespace PowerSDR
{
    /// <summary>
    /// Represents a single hardware radio instance (e.g. a FLEX-5000)
    /// </summary>
    public class Radio : IComparable, INotifyPropertyChanged
    {
        #region Constructors

        public Radio(Model m, object o)
        {
            model = m;
            access_obj = o;
        }

        public Radio(Model m, object o, bool p)
            : this(m, o)
        {
            present = p;
        }

        public Radio(Model m, object o, string sn)
            : this(m, o)
        {
            serial_number = sn;
        }

        public Radio(Model m, object o, string sn, string nick)
            : this(m, o, sn)
        {
            nickname = nick;
        }

        public Radio(Model m, object o, string sn, bool p)
            : this(m, o, sn)
        {
            present = p;
        }

        public Radio(Model m, object o, string sn, string nick, bool p)
            : this(m, o, sn, nick)
        {
            present = p;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged; // to implement the INotifyPropertyChanged interface

        #region Properties

        private bool present = false;
        public bool Present
        {
            get { return present; }
            set
            {
                present = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Present"));
            }
        }

        private Model model;
        public Model Model
        {
            get { return model; }
        }

        private string serial_number = "";
        public string SerialNumber
        {
            get { return serial_number; }
            set
            {
                serial_number = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SerialNumber"));
            }
        }

        private string nickname = "";
        public string Nickname
        {
            get { return nickname; }
            set
            {
                nickname = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Nickname"));
            }
        }

        private object access_obj;
        public object AccessObj
        {
            get { return access_obj; }
            set
            {
                access_obj = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AccessObj"));
            }
        }

        #endregion

        public int CompareTo(object obj) // to implement the IComparable interface
        {
            Radio r = (Radio)obj;
            if (this.model != r.model)
                return this.model.CompareTo(r.model);

            if(this.nickname != r.nickname)
                return this.nickname.CompareTo(r.nickname);

            return this.serial_number.CompareTo(r.serial_number);
        }

        public override string ToString() // for easier debugging in IDE
        {
            return Model.ToString() + " " + serial_number + " " + nickname;
        }


        //=======================================================================================================

        public string GetDBFilename()
        {
            string s = "";
            switch (model)
            {
                case Model.SDR1000: s = "S1K"; break;
                case Model.FLEX5000: s = "F5K"; break;
                case Model.FLEX3000: s = "F3K"; break;
                case Model.FLEX1500: s = "F1.5K"; break;
                case Model.SOFTROCK40: s = "SR40"; break;
                case Model.DEMO: s = "Demo"; break;
            }


            return "database_" + s + "_" + serial_number + ".xml"; // ke9ns here is the database that will be used 
        }

        //=======================================================================================================

        // ke9ns add for my own database  RevQ
        public string GetDBFilename1()
        {
            string s = "";
            switch (model)
            {
                case Model.SDR1000: s = "S1K"; break;
                case Model.FLEX5000: s = "F5K"; break;
                case Model.FLEX3000: s = "F3K"; break;
                case Model.FLEX1500: s = "F1.5K"; break;
                case Model.SOFTROCK40: s = "SR40"; break;
                case Model.DEMO: s = "Demo"; break;
            }

            return "database_RevQ_" + s + "_" + serial_number + ".xml"; // ke9ns here is the database that will be used 
        }



    }
}