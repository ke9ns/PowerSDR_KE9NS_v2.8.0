//=================================================================
// master.cs
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
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace PowerSDR
{
    public class Master
    {
        private static DataSet ds = new DataSet("Master");
        private static string file_name = "";
        private static bool production = false;

        /// <summary>
        /// Initializes the master file and populates the RadiosAvailable list
        /// </summary>
        public static void Init()
        {
            if (ds.Tables.Count > 0) return; // don't allow init to run more than once

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                    + "\\FlexRadio Systems\\";
            file_name = path + "master.xml";

            if (File.Exists(path + "production"))
                production = true;

            if (File.Exists(file_name))
            {
                try
                {
                    ds.ReadXml(file_name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The Master database schema is corrupted and unusable.\n" +
                        "Exception error was:\n\n" + ex.Message + "\n\n" + "A new Master database will be created",
                        "ERROR: Master DB is Unusable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                    
                    ds = new DataSet("Master");
                    NewMaster();
                }
            }
            else // file doesn't exist -- create a new master
            {
                NewMaster();        
            }               
            
            foreach (DataRow dr in ds.Tables["Radios"].Rows)
            {
                try
                {
                    Model m = (Model)Enum.Parse(typeof(Model), (string)dr["Model"]);
                    string sn = (string)dr["Serial"];
                    string nick = (string)dr["Nickname"];
                    bool present = false;

                    switch (m)
                    {
                        case Model.DEMO:
                            present = true;
                            break;
                        default:
                            break;

                    }

                    RadiosAvailable.AddRadio(new Radio(m, null, sn, nick, present));
                }
                catch (Exception)
                {
                    // catch CDRX values here
                }
            }
        }

        /// <summary>
        /// Adds/Updates the master file with a list of radios
        /// </summary>
        /// <param name="radios">Array of radios to add/update</param>
        /// <returns>True if all radios added were already in the list, otherwise returns false</returns>
        public static bool AddRadio(Radio[] radios)
        {
            bool found = true;
            foreach (Radio r in radios)
            {
                bool b = AddRadio(r);
                if (!b) found = b;
            }

            return found;
        }
        
        /// <summary>
        /// Adds/Updates the master file with a single radio
        /// </summary>
        /// <param name="r">Radio to add/update</param>
        /// <returns>True if radio was already in the list</returns>
        public static bool AddRadio(Radio r)
        {
            if (production && r.Model != Model.DEMO) return false;
            bool found = false;
            foreach (DataRow dr in ds.Tables["Radios"].Rows)
            {
                if ((string)dr["Serial"] == r.SerialNumber) // serial number match
                {
                    Model m = (Model)Enum.Parse(typeof(Model), (string)dr["Model"]);
                    if (r.Model == m) // Model match
                    {
                        found = true;
                        dr["Nickname"] = r.Nickname;
                        break;
                    }
                }
            }

            if (!found)
            {
                DataRow dr = ds.Tables["Radios"].NewRow();
                dr["Model"] = r.Model.ToString();
                dr["Serial"] = r.SerialNumber;
                dr["Nickname"] = r.Nickname;

                ds.Tables["Radios"].Rows.Add(dr);
            }

            return found;
        }

        public static bool RemoveRadio(Radio r)
        {
            bool found = false;
            foreach (DataRow dr in ds.Tables["Radios"].Rows)
            {
                if ((string)dr["Model"] == r.Model.ToString() && // model match
                    (string)dr["Serial"] == r.SerialNumber) // serial number match
                {                    
                    found = true;
                    ds.Tables["Radios"].Rows.Remove(dr);
                    break;
                }
            }        
            
            return found;
        }

        public static void Commit()
        {
            try
            {
                ds.WriteXml(file_name, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                MessageBox.Show("A Master database write to file operation failed.  " +
                    "The exception error was:\n\n" + ex.Message,
                    "ERROR: Master Database Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void NewMaster()
        {
            // add dataset and write it to file
            ds.Tables.Add("Radios");
            ds.Tables["Radios"].Columns.Add("Model", typeof(string));
            ds.Tables["Radios"].Columns.Add("Serial", typeof(string));
            ds.Tables["Radios"].Columns.Add("Nickname", typeof(string));

            Radio r = new Radio(Model.DEMO, null, true);

            DataRow dr = ds.Tables["Radios"].NewRow();

            dr["Model"] = r.Model.ToString();
            dr["Serial"] = r.SerialNumber;
            dr["Nickname"] = r.Nickname;

            ds.Tables["Radios"].Rows.Add(dr);

            RadiosAvailable.AddRadio(r);

            try
            {
                ds.WriteXml(file_name, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An initial Master database write to file operation failed.  " +
                    "The exception error was:\n\n" + ex.Message,
                    "ERROR: Master Database Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}