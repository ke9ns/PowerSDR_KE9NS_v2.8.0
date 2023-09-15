//=================================================================
// database.cs
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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PowerSDR
{
    class DB
    {
        #region Variable Declaration


        public static Console console;   // ke9ns mod  to allow console to pass back values to stack screen

        public static DataSet ds;

        /// <summary>
        /// the complete filename of the datbase file to use including the full path
        /// </summary>
        private static string file_name = "";
        public static string FileName
        {
            set { file_name = value; }
        }


        //==============================================================
        // ke9ns add tell functions not to try and re-add "bandtext" only new bandtext data
        private static bool bandtextrefresh = false;

        private static bool bandstackrefresh = true; // default is true to allow initial database creation of stack

        public static bool BandTextRefresh // true = reset bandtext
        {
            get
            {
                return bandtextrefresh;
            }
            set
            {
                bandtextrefresh = value;
            }

        }

        public static bool BandStackRefresh // true = reset your bandstack
        {
            get
            {
                return bandstackrefresh;
            }
            set
            {
                bandstackrefresh = value;
            }

        }
        //=========================================================================
        // ke9ns add my own database RevQ
        private static string file_name1 = "";
        public static string FileName1
        {
            set { file_name1 = value; }
        }



        #endregion

        #region Private Member Functions
        // ======================================================
        // Private Member Functions
        // ======================================================

        private static void VerifyTables(Model model) // ke9ns: for FRSRegion.US
        {
            if (!ds.Tables.Contains("BandText"))
                AddBandTextTable();

            if (!ds.Tables.Contains("BandStack"))
            {
                AddBandStackTable();
                AddBandStackSWL(); // ke9ns add
            }

            if (!ds.Tables.Contains("Memory"))
                AddMemoryTable();

            if (!ds.Tables.Contains("GroupList"))
                AddGroupListTable();

            Debug.WriteLine("VERIFY backtext TABLE HERE");

            // ke9ns: at startup check to see if your database has TXprofile tables (add them if not)

            if (!ds.Tables.Contains("TXProfile")) AddTXProfileTable(model);

            if (!ds.Tables.Contains("TXProfileDef")) AddTXProfileDefTable(model);

            Update(); // write database


        } // VerifyTables(Model model)

        //========================================================
        // ke9ns add: to update bandtext
        public static void RefreshTables(FRSRegion temp)
        {

            // CultureInfo.CurrentCulture.NumberFormat


            Debug.WriteLine("RefreshTables= " + bandtextrefresh + " , " + bandstackrefresh);

            if (temp == FRSRegion.US)
            {
                ClearBandText();

                Debug.WriteLine("bandtext cleared");

                AddBandTextTable(); //  AddBandTextSWB() is added here at the end of this function

                Debug.WriteLine("bandtext added");

                if (bandstackrefresh == true)
                {
                    ds.Tables["BandStack"].Clear();

                    AddBandStackTable(); // 

                    AddBandStackSWL(); // ke9ns add
                }

                Update();
                Debug.WriteLine("bandtext updated");


            }
            else // 
            {
                Debug.WriteLine("bandtext not US region");

                UpdateRegion(temp); // this will do the above for other FRSRegions

            }

        } // refreshtables


        private static void AddFormTable(string name)
        {
            ds.Tables.Add(name);
            ds.Tables[name].Columns.Add("Key", typeof(string));
            ds.Tables[name].Columns.Add("Value", typeof(string));
        }

        //======================================================================================
        private static void AddBandTextSWB()
        {
            // SW Broadcast & Misc. Band Plan
            DataTable t = ds.Tables["BandText"];

            object[] data = {

                                0.060000, 0.060000, "WWVB Time",                false,  // ke9ns add
                                2.500000, 2.500000, "WWV Night Time",           false,
                                5.000000, 5.000000, "WWV Night Time",           false,
                                10.000000, 10.000000, "WWV Day-Evening Time",   false,
                                15.000000, 15.000000, "WWV Day Time",           false,
                                20.000000, 20.000000, "WWV Day Time",           false,
                                25.000000, 25.000000, "WWV Day Time",           false,  // ke9ns add
                                3.330000, 3.330000, "CHU Night Time",           false,
                                7.850000, 7.850000, "CHU Night Time",           false,
                                14.670000, 14.670000, "CHU Day Time",           false,
                                4.996000, 4.996000, "RWM Night Time",                      false,
                                9.996000, 9.996000, "RWM Day-Evening Time",                      false,
                                14.996000, 14.996000, "RWM Day Time",                    false,
                                4.998000, 4.998000, "EBC Night Time",                      false,
                                15.006000, 15.006000, "EBC Day Time",                    false,

                                0.135700, 0.137799, "2.2kM CW & Narrow Band",   true, // 2200m ham ke9ns change

                                0.137800, 0.148499, "Long Wave",                false, // ke9ns add
                                0.148500, 0.283500, "International AM LW",      false, // ke9ns mod
                                0.283501, 0.414999, "Long Wave - Beacons",      false, // ke9ns add
                                0.415000, 0.471999, "Maritime Band",            false,

                                0.472000, 0.478999, "630M CW/JT9/WSPR/Narrow",    true, // 630m ham ke9ns change

                                0.479000, 0.526400, "Maritime Band",            false,
                                0.526401, 0.529999, "Long Wave - Beacons",      false,   // ke9ns add

                                0.530000, 0.530000, "TIS Travelers info stat",  false, // ke9ns add
								0.530001, 1.609999, "Broadcast AM Med Wave",    false,
                                1.610000, 1.610000, "TIS Travelers info stat",  false,
                                1.610001, 1.710000, "Extd Bcast AM Med Wave",   false,
                                1.710001, 1.799999, "Medium Wave",              false, // ke9ns add
                                // 160m ham
								2.000000, 2.499999, "120M Tropical Short Wave", false,
                                // wwv 2.5000 mhz
                                2.500001, 2.999999, "120M Tropical Short Wave", false, // ke9ns add
                                3.000000, 3.199999, "90M Tropical Short Wave",  false, // ke9ns add
                                3.200000, 3.329999, "90M Tropical Short Wave",  false,
                                // chu 3.3
                                3.330001, 3.499999, "90M Tropical Short Wave",  false,
                                // 3.5 - 4.0 80-75m ham
                                4.000001, 4.745999, "61M Night Short Wave",     false, // ke9ns add
                                4.750000, 4.995999, "61M Night Short Wave",     false,
                                4.996001, 4.997999, "61M Night Short Wave",     false,
                                4.998001, 4.999999, "61M Night Short Wave",     false,
                                // wwv
                               
                                5.000001, 5.167499, "61M Night Short Wave",     false, // ke9ns add
                                5.167500, 5.167500, "61M USB Alaska Emergency",  true, // ke9ns add
                                5.167501, 5.249999, "61M Night Short Wave",     false, // ke9ns add

                                // 60m ham 5.250-5.450
                                5.450001, 6.999999, "49M Night Short Wave",     false,
                                // 40m ham
								
                                7.300000, 7.453499, "41M Night Short Wave",     false, // ke9ns mod
                                7.453500, 7.456500, "41M USB Caribbean Emergency",  true, // ke9ns mod
                             
                                7.456501, 7.849999, "41M Night Short Wave",     false, // ke9ns mod
                               
                                // chu 7.85
                                7.850001, 7.853000, "41M USB Caribbean Emergency",  true, // ke9ns mod

                                7.853001, 8.999999, "41M Night Short Wave",     false, // ke9ns mod

                                9.000000, 9.900000, "31M Evening Short Wave",   false, // ke9ns mod
                                9.900001, 9.999999, "31M Evening Short Wave",   false, // ke9ns add
                                // wwv
                                10.000001, 10.099999, "31M Evening Short Wave", false, // ke9ns add
                                // 30m 10.1 - 10.15 mhz
	                            10.150001, 11.599999, "25M Evening Short Wave", false, // ke9ns add
                                11.600000, 12.100000, "25M Evening Short Wave", false,
                                12.100001, 13.569999, "25M Evening Short Wave", false, // ke9ns add

                                13.570000, 13.870000, "22M Daytime Short Wave",  false,
                                13.870001, 13.997999, "22M Daytime Short Wave", false, // ke9ns add

                                13.998000, 13.999999, "22M Caribbean Red Cross",  true, // ke9ns mod

                                // 20m 14.000-14.350

								14.350001, 14.414999, "19M Daytime Short Wave",  false,

                                14.415000, 14.418000, "19M Caribbean Emergency",  true, // ke9ns mod

                                14.418001, 14.669999, "19M Daytime Short Wave", false,
                               // chu 14.67
                                14.670001, 14.999999, "19M Daytime Short Wave", false,
                       
                                // wwv
                                15.000001, 15.800000, "19M Daytime Short Wave", false,
                                15.800001, 17.479999, "19M Daytime Short Wave", false, // ke9ns add
                                17.480000, 18.067999, "16M Daytime Short Wave", false, // ke9ns mod
                                // 17m 18.068-18.168
                                18.168001, 18.899999, "16M Daytime Short Wave", false, // ke9ns add
                                18.900000, 19.999999, "14M Daytime Short Wave", false, // ke9ns mod
                                // wwv
                                20.000001, 20.999999, "13M Daytime Short Wave", false, // ke9ns mod
                              
                                // 15m 21.000 - 21.450
                                21.450001, 24.889999, "13M Daytime Short Wave",  false,

                                // 12m 24.89 - 24.99 mhz
                                // wwv
                                25.000001, 26.960000, "11M Day Short Wave",     false,

                                26.960001, 26.969999, "11M CB ch 1",            false,
                                26.970001, 26.979999, "11M CB ch 2",            false,
                                26.980001, 26.989999, "11M CB ch 3",            false,
                                26.990001, 26.999999, "11M CB RC",              false,
                                27.000001, 27.009999, "11M CB ch 4",            false,
                                27.010001, 27.019999, "11M CB ch 5",            false,
                                27.020001, 27.029999, "11M CB ch 6",            false,
                                27.030001, 27.039999, "11M CB ch 7",            false,
                                27.040001, 27.049999, "11M CB RC",              false,
                                27.050001, 27.059999, "11M CB ch 8",            false,
                                27.060001, 27.069999, "11M CB ch 9",            false,
                                27.070001, 27.079999, "11M CB ch 10",           false,
                                27.080001, 27.089999, "11M CB ch 11",           false,
                                27.090001, 27.099999, "11M CB RC",              false,
                                27.100001, 27.109999, "11M CB ch 12",           false,
                                27.110001, 27.119999, "11M CB ch 13",           false,
                                27.120001, 27.129999, "11M CB ch 14",           false,
                                27.130001, 27.139999, "11M CB ch 15",           false,
                                27.140001, 27.149999, "11M CB RC",              false,
                                27.150001, 27.159999, "11M CB ch 16",           false,
                                27.160001, 27.169999, "11M CB ch 17",           false,
                                27.170001, 27.179999, "11M CB ch 18",           false,
                                27.180001, 27.189999, "11M CB ch 19",           false,
                                27.190001, 27.199999, "11M CB RC",              false,
                                27.200001, 27.209999, "11M CB ch 20",           false,
                                27.210001, 27.219999, "11M CB ch 21",           false,
                                27.220001, 27.229999, "11M CB ch 22",           false,
                                27.250001, 27.259999, "11M CB ch 23",           false,
                                27.230001, 27.239999, "11M CB ch 24",           false,
                                27.240001, 27.249999, "11M CB ch 25",           false,
                                27.260001, 27.269999, "11M CB ch 26",           false,
                                27.270001, 27.279999, "11M CB ch 27",           false,
                                27.280001, 27.289999, "11M CB ch 28",           false,
                                27.290001, 27.299999, "11M CB ch 29",             false,
                                27.300001, 27.309999, "11M CB ch 30",             false,
                                27.310001, 27.319999, "11M CB ch 31",             false,
                                27.320001, 27.329999, "11M CB ch 32",             false,
                                27.330001, 27.339999, "11M CB ch 33",             false,
                                27.340001, 27.349999, "11M CB ch 34",             false,
                                27.350001, 27.359999, "11M CB ch 35",             false,
                                27.360001, 27.369999, "11M CB ch 36",             false,
                                27.370001, 27.379999, "11M CB ch 37",             false,
                                27.380001, 27.389999, "11M CB ch 38",             false,
                                27.390001, 27.399999, "11M CB ch 39",             false,
                                27.400001, 27.409999, "11M CB ch 40",             false,

                                27.410001, 27.419999, "11M ch 41",             false,
                                27.420001, 27.429999, "11M ch 42",             false,
                                27.430001, 27.439999, "11M ch 43",             false,
                                27.440001, 27.449999, "11M ch 44",             false,
                                27.450001, 27.459999, "11M ch 45",             false,
                                27.460001, 27.469999, "11M ch 46",             false,
                                27.470001, 27.479999, "11M ch 47",             false,
                                27.480001, 27.489999, "11M ch 48",             false,
                                27.490001, 27.499999, "11M ch 49",             false,
                                27.500001, 27.509999, "11M ch 50",             false,

                                27.510001, 27.519999, "11M ch 51",             false,
                                27.520001, 27.529999, "11M ch 52",             false,
                                27.530001, 27.539999, "11M ch 53",             false,
                                27.540001, 27.549999, "11M ch 54",             false,
                                27.550001, 27.559999, "11M ch 55",             false,
                                27.560001, 27.569999, "11M ch 56",             false,
                                27.570001, 27.579999, "11M ch 57",             false,
                                27.580001, 27.589999, "11M ch 58",             false,
                                27.590001, 27.599999, "11M ch 59",             false,
                                27.600001, 27.609999, "11M ch 60",             false,

                                27.610001, 27.619999, "11M ch 61",             false,
                                27.620001, 27.629999, "11M ch 62",             false,
                                27.630001, 27.639999, "11M ch 63",             false,
                                27.640001, 27.649999, "11M ch 64",             false,
                                27.650001, 27.659999, "11M ch 65",             false,
                                27.660001, 27.669999, "11M ch 66",             false,
                                27.670001, 27.679999, "11M ch 67",             false,
                                27.680001, 27.689999, "11M ch 68",             false,
                                27.690001, 27.699999, "11M ch 69",             false,
                                27.700001, 27.709999, "11M ch 70",             false,

                                27.710001, 27.719999, "11M ch 71",             false,
                                27.720001, 27.729999, "11M ch 72",             false,
                                27.730001, 27.739999, "11M ch 73",             false,
                                27.740001, 27.749999, "11M ch 74",             false,
                                27.750001, 27.759999, "11M ch 75",             false,
                                27.760001, 27.769999, "11M ch 76",             false,
                                27.770001, 27.779999, "11M ch 77",             false,
                                27.780001, 27.789999, "11M ch 78",             false,
                                27.790001, 27.799999, "11M ch 79",             false,
                                27.800001, 27.809999, "11M ch 80",             false,

                                27.810000, 27.999999, "11M Short Wave",         false,

                                // 10m 28-29.7 mhz
                                // 6m 50 - 54 mhz

                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }

        } //  AddBandTextSWB()

        private static void AddBandRussiaTextSWB()
        {
            // SW Broadcast & Misc. Band Plan
            DataTable t = ds.Tables["BandText"];

            object[] data = {
                                0.060000, 0.060000, "WWVB Time",                false,  // ke9ns add
                                2.500000, 2.500000, "WWV Night Time",           false,
                                5.000000, 5.000000, "WWV Night Time",           false,
                                10.000000, 10.000000, "WWV Day-Evening Time",       false,
                                15.000000, 15.000000, "WWV Day Time",       false,
                                20.000000, 20.000000, "WWV Day Time",           false,
                                25.000000, 25.000000, "WWV Day Time",           false,  // ke9ns add
                                3.330000, 3.330000, "CHU Night Time",           false,
                                7.850000, 7.850000, "CHU Night Time",           false,
                                14.670000, 14.670000, "CHU Day Time",           false,
                                4.996000, 4.996000, "RWM Night Time",                      false,
                                9.996000, 9.996000, "RWM Day-Evening Time",                      false,
                                14.996000, 14.996000, "RWM Day Time",                    false,
                                4.998000, 4.998000, "EBC Night Time",                      false,
                                15.006000, 15.006000, "EBC Day Time",                    false,

                                0.135700, 0.137799, "2.2kM CW & Narrow Band",   true, // 2200m ham ke9ns change

                                0.137800, 0.148499, "Long Wave",                false, // ke9ns add
                                0.148500, 0.283500, "International AM LW",      false, // ke9ns mod
                                0.283501, 0.414999, "Long Wave - Beacons",      false, // ke9ns add
                                0.415000, 0.471999, "Maritime Band",            false,

                                0.472000, 0.478999, "630M CW & Narrow Band",    true, // 630m ham ke9ns change

                                0.479000, 0.526400, "Maritime Band",            false,

                                0.526401, 0.529999, "Long Wave - Beacons",      false,   // ke9ns add
                                0.530000, 0.530000, "TIS Travelers info stat",  false, // ke9ns add
								0.530001, 1.609999, "Broadcast AM Med Wave",    false,
                                1.610000, 1.610000, "TIS Travelers info stat",  false,
                                1.610001, 1.710000, "Extd Bcast AM Med Wave",   false,
                                1.710001, 1.799999, "Medium Wave",              false, // ke9ns add
                                // 160m ham
								2.000000, 2.499999, "120M Tropical Short Wave", false,
                                // wwv 2.5000 mhz
                                2.500001, 2.999999, "120M Tropical Short Wave", false, // ke9ns add
                                3.000000, 3.199999, "90M Tropical Short Wave",  false, // ke9ns add
                                3.200000, 3.329999, "90M Tropical Short Wave",  false,
                                // chu 3.3
                                3.330001, 3.499999, "90M Tropical Short Wave",  false,
                                // 3.5 - 4.0 80-75m ham
                                4.000001, 4.745999, "61M Night Short Wave",     false, // ke9ns add
                                4.750000, 4.995999, "61M NIght Short Wave",     false,
                                4.996001, 4.997999, "61M Night Short Wave",     false,
                                4.998001, 4.999999, "61M Night Short Wave",     false,
                                // wwv
                                5.000001, 5.060000, "61M Night Short Wave",     false,
                                5.060001, 5.249999, "61M Night Short Wave",     false, // ke9ns add
                                // 60m ham
                                5.450000, 6.999999, "49M Night Short Wave",     false,
                                // 40m ham
								7.300000, 7.849999, "41M Night Short Wave",     false, // ke9ns mod
                                // chu 7.85
                                7.850001, 8.999999, "41M Night Short Wave",     false, // ke9ns mod

                                9.000000, 9.900000, "31M Evening Short Wave",   false, // ke9ns mod
                                9.900001, 9.999999, "31M Evening Short Wave",   false, // ke9ns add
                                // wwv
                                10.000001, 10.099999, "31M Evening Short Wave", false, // ke9ns add
                                // 30m 10.1 - 10.15 mhz
	                            10.150001, 11.599999, "25M Evening Short Wave", false, // ke9ns add
                                11.600000, 12.100000, "25M Evening Short Wave", false,
                                12.100001, 13.569999, "25M Evening Short Wave", false, // ke9ns add

                                13.570000, 13.870000, "22M Daytime Short Wave", false,
                                13.870001, 13.999999, "22M Daytime Short Wave", false, // ke9ns add

                                // 20m 14.000-14.350
								14.350001, 14.669999, "19M Daytime Short Wave", false,
                                // chu 14.67
                                14.670001, 14.999999, "19M Daytime Short Wave", false,
                             
                                
                                // wwv
                                15.000001, 15.800000, "19M Daytime Short Wave", false,
                                15.800001, 17.479999, "19M Daytime Short Wave", false, // ke9ns add
                                17.480000, 18.067999, "16M Daytime Short Wave", false, // ke9ns mod
                                // 17m 18.068-18.168
                                18.168001, 18.899999, "16M Daytime Short Wave", false, // ke9ns add
                                18.900000, 19.999999, "14M Daytime Short Wave", false, // ke9ns mod
                                // wwv
                                20.000001, 20.999999, "13M Daytime Short Wave", false, // ke9ns mod
                              
                                // 15m 21.000 - 21.450
                                21.450001, 24.889999, "13M Daytime Short Wave", false,

                                // 12m 24.89 - 24.99 mhz
                                // wwv
                                25.000001, 26.960000, "11M Day Short Wave",     false,
                                26.960001, 26.969999, "11M CB ch 1",              true,
                                26.970001, 26.979999, "11M CB ch 2",              true,
                                26.980001, 26.989999, "11M CB ch 3",              true,
                                26.990001, 26.999999, "11M CB RC",                true,
                                27.000001, 27.009999, "11M CB ch 4",              true,
                                27.010001, 27.019999, "11M CB ch 5",              true,
                                27.020001, 27.029999, "11M CB ch 6",              true,
                                27.030001, 27.039999, "11M CB ch 7",              true,
                                27.040001, 27.049999, "11M CB RC",                true,
                                27.050001, 27.059999, "11M CB ch 8",              true,
                                27.060001, 27.069999, "11M CB ch 9",              true,
                                27.070001, 27.079999, "11M CB ch 10",             true,
                                27.080001, 27.089999, "11M CB ch 11",             true,
                                27.090001, 27.099999, "11M CB RC",                true,
                                27.100001, 27.109999, "11M CB ch 12",             true,
                                27.110001, 27.119999, "11M CB ch 13",             true,
                                27.120001, 27.129999, "11M CB ch 14",             true,
                                27.130001, 27.139999, "11M CB ch 15",             true,
                                27.140001, 27.149999, "11M CB RC",                true,
                                27.150001, 27.159999, "11M CB ch 16",             true,
                                27.160001, 27.169999, "11M CB ch 17",             true,
                                27.170001, 27.179999, "11M CB ch 18",             true,
                                27.180001, 27.189999, "11M CB ch 19",             true,
                                27.190001, 27.199999, "11M CB RC",                true,
                                27.200001, 27.209999, "11M CB ch 20",             true,
                                27.210001, 27.219999, "11M CB ch 21",             true,
                                27.220001, 27.229999, "11M CB ch 22",             true,
                                27.250001, 27.259999, "11M CB ch 23",             true,
                                27.230001, 27.239999, "11M CB ch 24",             true,
                                27.240001, 27.249999, "11M CB ch 25",             true,
                                27.260001, 27.269999, "11M CB ch 26",             true,
                                27.270001, 27.279999, "11M CB ch 27",             true,
                                27.280001, 27.289999, "11M CB ch 28",             true,
                                27.290001, 27.299999, "11M CB ch 29",             true,
                                27.300001, 27.309999, "11M CB ch 30",             true,
                                27.310001, 27.319999, "11M CB ch 31",             true,
                                27.320001, 27.329999, "11M CB ch 32",             true,
                                27.330001, 27.339999, "11M CB ch 33",             true,
                                27.340001, 27.349999, "11M CB ch 34",             true,
                                27.350001, 27.359999, "11M CB ch 35",             true,
                                27.360001, 27.369999, "11M CB ch 36",             true,
                                27.370001, 27.379999, "11M CB ch 37",             true,
                                27.380001, 27.389999, "11M CB ch 38",             true,
                                27.390001, 27.399999, "11M CB ch 39",             true,
                                27.400001, 27.409999, "11M CB ch 40",             true,

                                27.410001, 27.419999, "11M ch 41",             true,
                                27.420001, 27.429999, "11M ch 42",             true,
                                27.430001, 27.439999, "11M ch 43",             true,
                                27.440001, 27.449999, "11M ch 44",             true,
                                27.450001, 27.459999, "11M ch 45",             true,
                                27.460001, 27.469999, "11M ch 46",             true,
                                27.470001, 27.479999, "11M ch 47",             true,
                                27.480001, 27.489999, "11M ch 48",             true,
                                27.490001, 27.499999, "11M ch 49",             true,
                                27.500001, 27.509999, "11M ch 50",             true,

                                27.510001, 27.519999, "11M ch 51",             true,
                                27.520001, 27.529999, "11M ch 52",             true,
                                27.530001, 27.539999, "11M ch 53",             true,
                                27.540001, 27.549999, "11M ch 54",             true,
                                27.550001, 27.559999, "11M ch 55",             true,
                                27.560001, 27.569999, "11M ch 56",             true,
                                27.570001, 27.579999, "11M ch 57",             true,
                                27.580001, 27.589999, "11M ch 58",             true,
                                27.590001, 27.599999, "11M ch 59",             true,
                                27.600001, 27.609999, "11M ch 60",             true,

                                27.610001, 27.619999, "11M ch 61",             true,
                                27.620001, 27.629999, "11M ch 62",             true,
                                27.630001, 27.639999, "11M ch 63",             true,
                                27.640001, 27.649999, "11M ch 64",             true,
                                27.650001, 27.659999, "11M ch 65",             true,
                                27.660001, 27.669999, "11M ch 66",             true,
                                27.670001, 27.679999, "11M ch 67",             true,
                                27.680001, 27.689999, "11M ch 68",             true,
                                27.690001, 27.699999, "11M ch 69",             true,
                                27.700001, 27.709999, "11M ch 70",             true,

                                27.710001, 27.719999, "11M ch 71",             true,
                                27.720001, 27.729999, "11M ch 72",             true,
                                27.730001, 27.739999, "11M ch 73",             true,
                                27.740001, 27.749999, "11M ch 74",             true,
                                27.750001, 27.759999, "11M ch 75",             true,
                                27.760001, 27.769999, "11M ch 76",             true,
                                27.770001, 27.779999, "11M ch 77",             true,
                                27.780001, 27.789999, "11M ch 78",             true,
                                27.790001, 27.799999, "11M ch 79",             true,
                                27.800001, 27.809999, "11M ch 80",             true,

                                27.810000, 27.860000, "11M Short Wave",        true,

                                27.860001, 27.999999, "11M Short Wave",        false,



                            };

            Debug.WriteLine("russia2 " + (data.Length / 4));

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }

        } //  AddBandRussiaTextSWB()

        private static void ClearBandText()
        {
            ds.Tables["BandText"].Clear();


        }

        //===============================================================================================
        // ke9ns add needed to add to RevQ database
        private static void AddBandStackSWL()
        {

            //  DataRow dr = ds.Tables["BandStack"].NewRow();
            //   dr["BandName"] = band;
            //   dr["Mode"] = mode;
            //  dr["Filter"] = filter;
            //  dr["Freq"] = freq;
            //  ds.Tables["BandStack"].Rows.Add(dr);


            // ds.Tables.Add("BandStack");
            //  DataTable t = ds.Tables["BandStack"];

            //    t.Columns.Add("BandName", typeof(string));
            //   t.Columns.Add("Mode", typeof(string));
            //  t.Columns.Add("Filter", typeof(string));
            //   t.Columns.Add("Freq", typeof(double));

            object[] data = {

                                "LMF", "SAM", "F4", 0.560000,
                                "LMF", "SAM", "F4", 0.720000,
                                "LMF", "SAM", "F4", 0.780000,
                                "LMF", "SAM", "F4", 1.000000,
                                "LMF", "SAM", "F4", 1.700000,

                                "120M", "SAM", "F4", 2.400000,
                                "120M", "SAM", "F4", 2.410000,
                                "120M", "SAM", "F4", 2.420000,

                                "90M", "SAM", "F4", 3.300000,
                                "90M", "SAM", "F4", 3.310000,
                                "90M", "SAM", "F4", 3.320000,

                                "61M", "SAM", "F4", 4.700000,
                                "61M", "SAM", "F4", 4.800000,
                                "61M", "SAM", "F4", 4.820000,

                                "49M", "SAM", "F4", 5.600000,
                                "49M", "SAM", "F4", 5.700000,
                                "49M", "SAM", "F4", 5.800000,
                                "49M", "SAM", "F4", 5.900000,
                                "49M", "SAM", "F4", 6.000000,
                                "49M", "SAM", "F4", 6.200000,


                                "41M", "SAM", "F4", 7.310000,
                                "41M", "SAM", "F4", 7.400000,
                                "41M", "SAM", "F4", 7.500000,


                                "31M", "SAM", "F4", 9.100000,
                                "31M", "SAM", "F4", 9.200000,
                                "31M", "SAM", "F4", 9.300000,
                                "31M", "SAM", "F4", 9.400000,
                                "31M", "SAM", "F4", 9.500000,
                                "31M", "SAM", "F4", 9.600000,


                                "25M", "SAM", "F4", 11.700000,
                                "25M", "SAM", "F4", 11.800000,
                                "25M", "SAM", "F4", 11.900000,

                                "22M", "SAM", "F4", 13.600000,
                                "22M", "SAM", "F4", 13.700000,
                                "22M", "SAM", "F4", 13.800000,

                                "19M", "SAM", "F4", 15.200000,
                                "19M", "SAM", "F4", 15.300000,
                                "19M", "SAM", "F4", 15.400000,

                                "16M", "SAM", "F4", 17.500000,
                                "16M", "SAM", "F4", 17.600000,
                                "16M", "SAM", "F4", 17.700000,

                                "14M", "SAM", "F4", 18.900000,
                                "14M", "SAM", "F4", 19.000000,
                                "14M", "SAM", "F4", 19.100000,

                                "13M", "SAM", "F4", 21.500000,
                                "13M", "SAM", "F4", 21.600000,
                                "13M", "SAM", "F4", 21.700000,

                                "11M", "SAM", "F4", 25.700000,
                                "11M", "SAM", "F4", 26.000000,
                                "11M", "SAM", "F4", 26.500000,
                                "11M", "SAM", "F4", 27.000000,
                                "11M", "SAM", "F4", 27.500000,
                                "11M", "SAM", "F4", 27.800000

            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }

        } //ke9ns addbandstackSWL




        #region IARU Region 1 BandText

        private static void AddRegion1BandText160m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                1.810000, 1.835999, "160M CW",                  true,
                                1.836000, 1.836000, "160M CW QRP",              true,
                                1.836001, 1.837999, "160M CW",                  true,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.909999, "160M SSB/SSTV/Wide Band",  true,
                                1.910000, 1.910000, "160M SSB QRP",             true,
                                1.910001, 1.994999, "160M SSB/SSTV/Wide Band",  true,
                                1.995000, 1.999999, "160M Experimental",        true,

                               // 1.838000, 1.839999, "160M Narrow Band Modes",	true,
								//1.840000, 1.842999, "160M All Modes & Digital",	true,
                              //  1.843000, 1.999999, "160M All Modes",	true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion1BandText80m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                3.500000, 3.559999, "80M CW",                   true,
                                3.560000, 3.560000, "80M CW QRP",               true,
                                3.560001, 3.567999, "80M CW",                   true,

                                3.568000, 3.568000, "80M FT4/JT65 DIGU",             true, // ke9ns add  3.573
                                3.568001, 3.572999, "80M FT4/JT65 DIGU",             true, // ke9ns add

                                3.573000, 3.573000, "80M FT8 DIGU",             true, // ke9ns add  3.573
                                3.573001, 3.574999, "80M FT8 DIGU",             true, // ke9ns add

                                3.575000, 3.575000, "80M FT4 DIGU",             true, // ke9ns add  3.573
                                3.575001, 3.578000, "80M FT4 DIGU",             true, // ke9ns add
                              
								3.578001, 3.589999, "80M PSK",                  true,
                                3.590000, 3.590000, "80M RTTY DX",              true,
                                3.590001, 3.599999, "80M RTTY",                 true,

                               // 3.580000, 3.599999, "80M Narrow Band Modes",	true,
								3.600000, 3.689999, "80M All Modes",            true,

                                3.690000, 3.690000, "80M SSB QRP",              true,
                                3.690001, 3.759999, "80M All Modes",            true,
                                3.760000, 3.760000, "80M SSB Emergency",        true,
                                3.760001, 3.799999, "80M All Modes",            true,

                //  3.600000, 3.699999, "75M Extra SSB",            true,
                //  3.700000, 3.789999, "75M Ext/Adv SSB",          true,
                //  3.790000, 3.799999, "75M Ext/Adv DX Window",    true,

                              //  3.800000, 3.844999, "75M US SSB",                  false,
                              //  3.845000, 3.845000, "75M US SSTV",                 false,
                              //  3.845001, 3.884999, "75M US SSB",                  false,
                              ///  3.885000, 3.885000, "75M US AM Calling Frequency", false,
                              //  3.885001, 3.999999, "75M US SSB",


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // region 1 80m bandtext


        //  channels_60m.Add(new Channel(5.3320, 2800));
        //             channels_60m.Add(new Channel(5.3480, 2800));
        //             channels_60m.Add(new Channel(5.3585, 2800));
        //             channels_60m.Add(new Channel(5.3730, 2800));
        //             channels_60m.Add(new Channel(5.4050, 2800));


        // 5.250000, 5.351499, "60M RX Only"
        // 5.351500, 5.335999, "60M 200hz Narrow Band Modes
        // 5.354000, 5.356999, "60M USB Voice
        // 5.357000, 5.359999, "60M USB Voice (US CH 3)"
        // 5.360000, 5.362999, "60M USB Voice"            
        // 5.363000, 5.365999, "60M USB Voice"            
        // 5.366000, 5.366500, "60M 20hz Narrow Band Modes"    
        // 5.366501, 5.450000, "60M RX Only"          

        // ke9ns modified
        private static void AddRegion1ABandText60m() // TX Germany, Luxembourg, Belgium, spain, switzerland, Finland
        {
            DataTable t = ds.Tables["BandText"];
            Debug.WriteLine("EUROPE==============");


            object[] data = {
                                5.250000, 5.351499, "60M RX Only",                  false,

                                5.351500, 5.353999, "60M 200hz Narrow Band Modes",  true,
                                5.354000, 5.356999, "60M USB Voice (UK CH 7)",      true,
                                5.357000, 5.359999, "60M USB Voice (US CH 3)",      true,
                                5.360000, 5.362999, "60M USB Voice",                true,
                                5.363000, 5.365999, "60M USB Voice (UK CH 8)",      true,
                                5.366000, 5.366500, "60M 20hz Narrow Band Modes",   true,

                                5.366501, 5.450000, "60M RX Only",                  false,
                            };



            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        //====================================================================================================================
        // ke9ns add CB 11m (not used)
        private static void AddBandText11m() // ke9ns add CB
        {
            DataTable t = ds.Tables["BandText"];
            ///  Debug.WriteLine("11m==============");

            object[] data = {
                                26.960001, 26.969999, "CB ch 1",              false,
                                26.970001, 26.979999, "CB ch 2",              false,
                                26.980001, 26.989999, "CB ch 3",              false,
                                26.990001, 26.999999, "CB RC",              false,
                                27.000001, 27.009999, "CB ch 4",              false,
                                27.010001, 27.019999, "CB ch 5",              false,
                                27.020001, 27.029999, "CB ch 6",              false,
                                27.030001, 27.039999, "CB ch 7",              false,
                                27.040001, 27.049999, "CB RC",              false,
                                27.050001, 27.059999, "CB ch 8",              false,
                                27.060001, 27.069999, "CB ch 9",              false,
                                27.070001, 27.079999, "CB ch 10",             false,
                                27.080001, 27.089999, "CB ch 11",             false,
                                27.090001, 27.099999, "CB RC",             false,
                                27.100001, 27.109999, "CB ch 12",             false,
                                27.110001, 27.119999, "CB ch 13",             false,
                                27.120001, 27.129999, "CB ch 14",             false,
                                27.130001, 27.139999, "CB ch 15",             false,
                                27.140001, 27.149999, "CB RC",             false,
                                27.150001, 27.159999, "CB ch 16",             false,
                                27.160001, 27.169999, "CB ch 17",             false,
                                27.170001, 27.179999, "CB ch 18",             false,
                                27.180001, 27.189999, "CB ch 19",             false,
                                27.190001, 27.199999, "CB RC",             false,
                                27.200001, 27.209999, "CB ch 20",             false,
                                27.210001, 27.219999, "CB ch 21",             false,
                                27.220001, 27.229999, "CB ch 22",             false,
                                27.250001, 27.259999, "CB ch 23",             false,
                                27.230001, 27.239999, "CB ch 24",             false,
                                27.240001, 27.249999, "CB ch 25",             false,
                                27.260001, 27.269999, "CB ch 26",             false,
                                27.270001, 27.279999, "CB ch 27",             false,
                                27.280001, 27.289999, "CB ch 28",             false,
                                27.290001, 27.299999, "CB ch 29",             false,
                                27.300001, 27.309999, "CB ch 30",             false,
                                27.310001, 27.319999, "CB ch 31",             false,
                                27.320001, 27.329999, "CB ch 32",             false,
                                27.330001, 27.339999, "CB ch 33",             false,
                                27.340001, 27.349999, "CB ch 34",             false,
                                27.350001, 27.359999, "CB ch 35",             false,
                                27.360001, 27.369999, "CB ch 36",             false,
                                27.370001, 27.379999, "CB ch 37",             false,
                                27.380001, 27.389999, "CB ch 38",             false,
                                27.390001, 27.399999, "CB ch 39",             false,
                                27.400001, 27.409999, "CB ch 40",             false,

                                27.410001, 27.419999, "ch 41",             false,
                                27.420001, 27.429999, "ch 42",             false,
                                27.430001, 27.439999, "ch 43",             false,
                                27.440001, 27.449999, "ch 44",             false,
                                27.450001, 27.459999, "ch 45",             false,
                                27.460001, 27.469999, "ch 46",             false,
                                27.470001, 27.479999, "ch 47",             false,
                                27.480001, 27.489999, "ch 48",             false,
                                27.490001, 27.499999, "ch 49",             false,
                                27.500001, 27.509999, "ch 50",             false,

                                27.510001, 27.519999, "ch 51",             false,
                                27.520001, 27.529999, "ch 52",             false,
                                27.530001, 27.539999, "ch 53",             false,
                                27.540001, 27.549999, "ch 54",             false,
                                27.550001, 27.559999, "ch 55",             false,
                                27.560001, 27.569999, "ch 56",             false,
                                27.570001, 27.579999, "ch 57",             false,
                                27.580001, 27.589999, "ch 58",             false,
                                27.590001, 27.599999, "ch 59",             false,
                                27.600001, 27.609999, "ch 60",             false,

                                27.610001, 27.619999, "ch 61",             false,
                                27.620001, 27.629999, "ch 62",             false,
                                27.630001, 27.639999, "ch 63",             false,
                                27.640001, 27.649999, "ch 64",             false,
                                27.650001, 27.659999, "ch 65",             false,
                                27.660001, 27.669999, "ch 66",             false,
                                27.670001, 27.679999, "ch 67",             false,
                                27.680001, 27.689999, "ch 68",             false,
                                27.690001, 27.699999, "ch 69",             false,
                                27.700001, 27.709999, "ch 70",             false,

                                27.710001, 27.719999, "ch 71",             false,
                                27.720001, 27.729999, "ch 72",             false,
                                27.730001, 27.739999, "ch 73",             false,
                                27.740001, 27.749999, "ch 74",             false,
                                27.750001, 27.759999, "ch 75",             false,
                                27.760001, 27.769999, "ch 76",             false,
                                27.770001, 27.779999, "ch 77",             false,
                                27.780001, 27.789999, "ch 78",             false,
                                27.790001, 27.799999, "ch 79",             false,
                                27.800001, 27.809999, "ch 80",             false,


                            };





            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }

        } // AddRegion1ABandText11m() // ke9ns add CB


        // ke9ns parts of region 1 that doesnt get the new 60m plan
        private static void AddRegion1BBandText60m() // Netherlands (no longer as of 5/25/18 ke9ns)
        {
            DataTable t = ds.Tables["BandText"];

            object[] data = {
                                5.250000, 5.349999, "60M RX Only",              false,
                                5.350000, 5.450000, "60M Amateur Service",      true,


                           };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }



        // ke9ns parts of region 1 that dont get the new 60m plan
        private static void AddRegion1BandText60m() // RX only
        {
            DataTable t = ds.Tables["BandText"];
            Debug.WriteLine("EUROPE1==============");

            object[] data = {

                                5.250000, 5.351499, "60M RX Only",              false,

                                5.351500, 5.335999, "60M 200hz RX Narrow Band Modes",    false,
                                5.354000, 5.356999, "60M RX USB (UK CH 7)",            false,
                                5.357000, 5.359999, "60M RX USB (US CH 3)",  false,
                                5.360000, 5.362999, "60M RX USB",            false,
                                5.363000, 5.365999, "60M RX USB (UK CH 8)",            false,
                                5.366000, 5.366500, "60M 20hz RX Narrow Band Modes",    false,

                                5.366501, 5.450000, "60M RX Only",              false,

                           };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }



        private static void AddRegion1BandText40m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                7.000000, 7.029999, "40M CW",                   true,
                                7.030000, 7.030000, "40M CW QRP",               true,
                                7.030001, 7.039999, "40M CW",                   true,

                                7.040000, 7.042999, "40M PSK",                  true,
                                7.043000, 7.046999, "40M RTTY",                 true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.059999, "40M RTTY",                 true,

                                7.060000, 7.060000, "40M SSB Emergency",        true,
                                7.060001, 7.069999, "40M All Modes",            true,

                                7.070000, 7.070000, "40m PSK",                  true,            // ke9ns add
                                7.070001, 7.073999, "40m PSK",                  true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add
                               
                                7.079000, 7.089999, "40M RTTY",                 true,
							
                                //	7.035000, 7.039999, "40M Narrow Band Modes",	true,
							   //	7.040000, 7.059999, "40M All Modes",			true,
                              
                               7.090000, 7.090000, "40M SSB QRP",              true,
                               7.090001, 7.199999, "40M All Modes",            true,

                               7.200000, 7.299999, "40M RX ONLY",              false,

                              //  7.200000, 7.289999, "40M US SSB",               false,
                              //  7.290000, 7.290000, "40M AM Calling Frequency", false,
                              //  7.290001, 7.299999, "40M SSB",


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // AddRegion1BandText40m()

        private static void AddRegion1BandText30m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                10.100000, 10.115999, "30M CW",                 true,
                                10.116000, 10.116000, "30M CW QRP",             true,
                                10.116001, 10.129999, "30M CW",                 true,

                                10.130000, 10.135999, "30M RTTY",               true,

                                10.136000, 10.136000, "30M FT8 DIGU",           true, // ke9ns add
                                10.136001, 10.137999, "30M FT8 DIGU",       true, // ke9ns add

                                10.138000, 10.138000, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.138001, 10.138999, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.139000, 10.139999, "30M RTTY",               true,

                                10.140000, 10.140000, "30M FT4 DIGU",           true, // ke9ns add
                                10.140001, 10.142999, "30M FT4 DIGU",       true, // ke9ns add

                                10.143000, 10.149999, "30M Narrow Band Modes",   true,


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // AddRegion1BandText30m()

        private static void AddRegion1BandText20m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                14.000000, 14.059999, "20M CW",                 true,
                                14.060000, 14.060000, "20M CW QRP",             true,
                                14.060001, 14.069999, "20M CW",                 true,

                                14.070000, 14.073999, "20M PSK",                true,

                                14.074000, 14.074000, "20M FT8 DIGU",      true, // ke9ns add
                                14.074001, 14.075999, "20M FT8 DIGU",           true, // ke9ns add

                                14.076000, 14.076000, "20M JT65 DIGU",     true, // ke9ns add
                                14.076001, 14.078999, "20M JT65 DIGU",          true, // ke9ns add

                                14.079000, 14.079999, "20M RTTY",               true,

                                14.080000, 14.080000, "20M FT4 DIGU",          true, // ke9ns add
                                14.080001, 14.084999, "20M FT4 DIGU",           true, // ke9ns add

                                14.085000, 14.094999, "20M RTTY",               true,

                                14.095000, 14.098999, "20M Packet",             true,
                                14.099000, 14.099999, "20M Beacons",            true,

                                14.100000, 14.100000, "20M NCDXF Beacons",      true,
                                14.100001, 14.100999, "20M Beacons",            true,
                                14.101000, 14.111999, "20M All Mode Digital",   true,

                                14.112000, 14.129999, "20M All Mode",           true,
                                14.130000, 14.130000, "20M Digital Voice",      true,
                                14.130001, 14.229999, "20M All Modes",          true,

                                14.230000, 14.230000, "20M SSTV",               true,
                                14.230001, 14.232999, "20M SSTV",               true,

                                14.233000, 14.233000, "20M EasyPal",            true,
                                14.233001, 14.235999, "20M EasyPal",            true,

                                14.236000, 14.236001, "20M FreeDV (Digital Voice)", true,                // ke9ns add   
                                14.236002, 14.284999, "20M All Modes",          true,
                                14.285000, 14.285000, "20M SSB QRP",            true,
                                14.285001, 14.285999, "20M All Modes",          true,

                                14.286000, 14.286000, "20M AM Calling Freq",    true,

                                14.286001, 14.299999, "20M All Modes",          true,
                                14.300000, 14.300000, "20M SSB Emergency",      true,

                                14.300001, 14.339999, "20M All Modes",          true,
                                14.340000, 14.340001, "20M DV (Digital Voice)", true,                // ke9ns add   
                                14.340002, 14.349999, "20M ALL Modes",          true,


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // AddRegion1BandText20m()

        private static void AddRegion1BandText17m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                18.068000, 18.085999, "17M CW",                 true,
                                18.086000, 18.086000, "17M CW QRP",             true,
                                18.086001, 18.094999, "17M CW",                 true,

                                18.095000, 18.099999, "17M Narrow Band Modes",    true,

                                18.100000, 18.100000, "17M FT8 DIGU",           true, // ke9ns add
                                18.100001, 18.101999, "17M FT8 DIGU",           true, // ke9ns add

                                18.102000, 18.102000, "17M JT65 DIGU",          true, // ke9ns add
                                18.102001, 18.103999, "17M JT65 DIGU",          true, // ke9ns add

                                18.104000, 18.104000, "17M FT4 DIGU",           true, // ke9ns add
                                18.104001, 18.106999, "17M FT4 DIGU",           true, // ke9ns add

                                18.107000, 18.107999, "17M RTTY",               true,

                                18.108000, 18.108999, "17M PSK / Packet",        true,

                                18.109000, 18.109999, "17M Beacons",         true,
                                18.110000, 18.110000, "17M NCDXF Beacons",      true,
                                18.110001, 18.110499, "17M Beacons",            true,

                                18.110500, 18.147999, "17M All Modes",           true,
                                18.148000, 18.148001, "17M DV (Digital Voice)", true,                // ke9ns add  
                                18.148002, 18.159999, "17M All Modes",          true,

                                18.160000, 18.160000, "17M SSB Emergency",       true,

                                18.160001, 18.167999, "17M All Modes",          true,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } //  AddRegion1BandText17m()

        private static void AddRegion1BandText15m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                21.000000, 21.059999, "15M CW",                 true,
                                21.060000, 21.060000, "15M CW QRP",             true,
                                21.060001, 21.069999, "15M CW",                 true,

                                21.070000, 21.073999, "15M RTTY",               true,

                                21.074000, 21.074000, "15M FT8 DIGU",           true, // ke9ns add 
                                21.074001, 21.075999, "15M FT8 DIGU",           true, // ke9ns add

                                21.076000, 21.076000, "15M JT65 DIGU",          true, // ke9ns add
                                21.076001, 21.078999, "15M JT65 DIGU",          true, // ke9ns add
                                21.079000, 21.099999, "15M RTTY",               true,

                                21.100000, 21.109999, "15M Packet",             true,

                                21.110000, 21.119999, "15M Wide Band Digital",   true,

                                21.120000, 21.139999, "15M Narrow Band Modes",   true,

                                21.140000, 21.140000, "15M FT4 DIGU",           true, // ke9ns add 
                                21.140001, 21.144999, "15M FT4 DIGU",           true, // ke9ns add

                                21.145000, 21.148999, "15M Narrow Band Modes",    true,

                                21.149000, 21.149999, "15M Beacons",         true,
                                21.150000, 21.150000, "15M NCDXF Beacons",      true,
                                21.150001, 21.150999, "15M Beacons",            true,

                                21.151000, 21.179999, "15M All Modes",           true,
                                21.180000, 21.180000, "15M Digital Voice",      true,
                                21.180001, 21.284999, "15M All Modes",          true,
                                21.285000, 21.285000, "15M SSB QRP",            true,
                                21.285001, 21.359999, "15M All Modes",          true,
                                21.360000, 21.360000, "15M SSB Emergency",      true,
                                21.360001, 21.450000, "15M All Modes",          true,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // AddRegion1BandText15m()

        private static void AddRegion1BandText12m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                24.890000, 24.905999, "12M CW",                 true,
                                24.906000, 24.906000, "12M CW QRP",             true,
                                24.906001, 24.914999, "12M CW",                 true,

                               // 24.915000, 24.928999, "12M Narrow Band Modes",	true,
                                24.915000, 24.915000, "12M FT8 DIGU",           true, // ke9ns add 
                                24.915001, 24.916999, "12M FT8 DIGU",           true, // ke9ns add

                                24.917000, 24.917000, "12M JT65 DIGU",          true, // ke9ns add
                                24.917001, 24.918999, "12M JT65 DIGU",          true, // ke9ns add

                                24.919000, 24.919000, "12M FT4 DIGU",           true, // ke9ns add 
                                24.919001, 24.921999, "12M FT4 DIGU",           true, // ke9ns add

                                24.922000, 24.924999, "12M RTTY",               true,
                                24.925000, 24.928999, "12M Packet",             true,

                                24.929000, 24.929999, "12M Beacons",         true,
                                24.930000, 24.930000, "12M NCDXF Beacons",      true,
                                24.930001, 24.930999, "12M Beacons",            true,

                                24.931000, 24.937999, "12M All Modes Digital",   true,
                                24.938000, 24.938001, "12M DV (Digital Voice)", true,                // ke9ns add    
                                24.938002, 24.939999, "12M All Modes Digital",  true,

                                24.940000, 24.989999, "12M All Modes",           true,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } //AddRegion1BandText12m()

        private static void AddRegion1BandText10m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                28.000000, 28.059999, "10M CW",                 true,
                                28.060000, 28.060000, "10M CW QRP",             true,
                                28.060001, 28.069999, "10M CW",                 true,

                                28.070000, 28.073999, "10M Narrow Band Modes",   true,

                                28.074000, 28.074000, "10M FT8 DIGU",           true, // ke9ns add 
                                28.074001, 28.075999, "10M FT8 DIGU",           true, // ke9ns add

                                28.076000, 28.076000, "10M JT65 DIGU",          true, // ke9ns add 
                                28.076001, 28.078999, "10M JT65 DIGU",          true, // ke9ns add

								28.079000, 28.149999, "10M RTTY",               true,

                                28.150000, 28.179999, "10M Narrow Band Modes",  true,

                                28.180000, 28.180000, "10M FT4 DIGU",           true, // ke9ns add 
                                28.180001, 28.184999, "10M FT4 DIGU",           true, // ke9ns add
                            
                                28.185000, 28.189999, "10M Narrow Band Modes",  true,

                                28.190000, 28.199999, "10M Beacons",         true,
                                28.200000, 28.200000, "10m NCDXF Beacons",       true,
                                28.200001, 28.224999, "10M Beacons",            true,

                                28.225000, 28.299999, "10M All Mode Beacons",    true,
                                28.300000, 28.319999, "10M All Mode Digital",   true,
                                28.320001, 28.329999, "10M All Modes",          true,
                                28.330000, 28.330000, "10M Digital Voice",      true,
                                28.330001, 28.359999, "10M All Modes",          true,
                                28.360000, 28.360000, "10M SSB QRP",            true,
                                28.360001, 28.679999, "10M All Modes",          true,
                                28.680000, 28.680000, "10M SSTV",               true,
                                28.680001, 29.199999, "10M All Modes",          true,
                                29.200000, 29.299999, "10M FM Digital",         true,
                                29.300000, 29.509999, "10M FM Sat. Downlinks",  true,
                                29.510000, 29.519999, "10M Guard Channel",      true,
                                29.520000, 29.549999, "10M FM Simplex",         true,
                                29.550000, 29.559999, "10M Deadband",           true,
                                29.560000, 29.589999, "10M Repeater Inputs",    true,
                                29.590000, 29.599999, "10M Deadband",           true,
                                29.600000, 29.600000, "10M FM Calling",         true,
                                29.600001, 29.609999, "10M Deadband",           true,
                                29.610000, 29.649999, "10M FM Simplex",         true,
                                29.650000, 29.659999, "10M Deadband",           true,
                                29.660000, 29.699999, "10M Repeater Outputs",   true,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }

        } //  AddRegion1BandText10m()

        private static void AddRegion1BandText6m()
        {
            // 50.0 - 52.0 MHz
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 50.079999, "6M Beacon Sub-Band",     true,
                                50.080000, 50.089999, "6M CW",                  true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",            true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.124999, "6M CW & SSB",            true,
                                50.125000, 50.125000, "6M US Calling Frequency",   true, // calling freq
                                50.125001, 50.129999, "6M CW & SSB",           true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",   true,
                                50.150000, 50.150000, "6M SSB Calling",         true,

                                50.150001, 50.209999, "6M CW, SSB & Digital",   true,
                                50.210000, 50.210001, "6M DV (Digital Voice)", true,                // ke9ns add
                                50.210002, 50.249999, "6M Meteor Scatter",      true,

                                50.250000, 50.250000, "6M PSK Calling",         true,
                                50.250001, 50.275999, "6M CW, SSB & Digital",   true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.284999, "6M All Modes",           true,

                                50.285000, 50.309999, "6M All Modes",           true,

                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.399999, "6M CW, SSB & Digital", true,

                                50.400000, 50.400000, "6M WSPR Beacons",        true,
                                50.400001, 50.499999, "6M CW, SSB & Digital",   true,
                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 51.209999, "6M All Modes",           true,
                                51.210000, 51.389999, "6M FM Repeater Inputs",  true,
                                51.390000, 51.409999, "6M All Modes",           true,
                                51.410000, 51.509999, "6M FM Simplex",          true,
                                51.510000, 51.510000, "6M FM Calling",          true,
                                51.510001, 51.589999, "6M FM Simplex",          true,
                                51.590000, 51.809999, "6M All Modes",           true,
                                51.810000, 51.989999, "6M FM Repeater Ouputs",  true,
                                51.990000, 51.999999, "6M All Modes",           true,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // AddRegion1BandText6m()

        private static void AddRegion1BandText4m()
        {
            // 70.0 - 70.5 MHz
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                70.000000, 70.089999, "4M CW & Digital",        true,
                                70.090000, 70.099999, "4M Beacons",             true,
                                70.100000, 70.249999, "4M CW, SSB & Digital",   true,
                                70.250000, 70.250000, "4M CW & SSB Calling",    true,
                                70.250001, 70.259999, "4M AM & FM",             true,
                                70.260000, 70.260000, "4M AM & FM Calling",     true,
                                70.260001, 70.299999, "4M AM & FM",             true,
                                70.300000, 70.300000, "4M RTTY & FAX",          true,
                                70.300001, 70.449999, "4M FM Channels",         true,
                                70.450000, 70.450000, "4M FM Calling",          true,
                                70.450001, 70.462499, "4M FM Channels",         true,
                                70.462500, 70.462500, "4M FM Calling",          true,
                                70.462501, 70.474999, "4M FM Channels",         true,
                                70.475000, 70.475000, "4M FM Calling",          true,
                                70.475001, 70.487499, "4M FM Channels",         true,
                                70.487500, 70.499999, "4M FM Digital",          true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion1BandTextVHFplus() // UK_PLUS
        {
            // IARU Region 1: 2M and above Band Plan
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								// 144 - 146 MHz
                                144.000000, 144.034999, "2M CW & SSB EME",                true,
                                144.035000, 144.049999, "2M CW",                        true,
                                144.050000, 144.005000, "2M CW Calling",                true,
                                144.050001, 144.149999, "2M CW",                        true,

                                144.150000, 144.169999, "2M SSB",                       true,
                                144.170000, 144.173999, "2M FT4",                       true, // ke9ns add
                                144.174000, 144.176999, "2M FT8",                       true,
                                144.177000, 144.299999, "2M SSB",                       true,

                                144.300000, 144.300000, "2M SSB Calling",               true,
                                144.300001, 144.399999, "2M SSB",                       true,
                                144.400000, 144.489999, "2M Beacons",                   true,
                                144.490000, 144.499999, "2M Guard Band",                true,
                                144.500000, 144.799999, "2M All Mode",                  true,
                                144.800000, 144.989999, "2M Digital",                   true,
                                144.990000, 144.993999, "2M Deadband",                  true,
                                144.994000, 145.193499, "2M Repeater Inputs",           true,
                                145.193500, 145.193999, "2M Deadband",                  true,
                                145.194000, 145.499999, "2M FM Simplex",                true,
                                145.500000, 145.500000, "2M FM Calling",                true,
                                145.500001, 145.593499, "2M FM Simplex",                true,
                                145.593500, 145.593999, "2M Deadband",                  true,
                                145.594000, 145.793499, "2M Repeater Outputs",          true,
                                145.793500, 145.799999, "2M Deadband",                  true,
                                145.800000, 146.000000, "2M All Mode Sat.",             true,
                                146.000001, 146.899999, "2M WB Digital Ex NoV",         true, // ke9ns add
                                146.900000, 147.000000, "2M NB Digital Ex NoV",         true, // ke9ns add
                               
                                
                                // 430 - 440 MHz
								430.000000, 430.024999, "70cm Sub-Regional",          true,
                                430.025000, 430.374999, "70cm Repeater Outputs",        true,
                                430.375000, 430.399999, "70cm Sub-Regional",            true,
                                430.400000, 430.574999, "70cm FM Digital Link",         true,
                                430.575000, 430.599999, "70cm Sub-Regional",            true,
                                430.600000, 430.924999, "70cm FM Digital Repeater",     true,
                                430.925000, 431.024999, "70cm Multimode Channels",      true,
                                431.025000, 431.049999, "70cm Sub-Regional",            true,
                                431.050000, 431.974999, "70cm Repeater Inputs",         true,
                                431.975000, 431.999999, "70cm Sub-Regional",            true,
                                432.000000, 432.024999, "70cm CW EME",                  true,
                                432.025000, 432.049999, "70cm CW",                      true,
                                432.050000, 432.050000, "70cm CW Calling",              true,

                                432.050001, 432.064999, "70cm CW",                      true,
                                432.065000, 432.067999, "70cm FT8",                     true, // ke9ns add
                                432.068000, 432.149999, "70cm CW",                      true,

                                432.150000, 432.199999, "70cm CW & SSB",                true,
                                432.200000, 432.200000, "70cm SSB Calling",             true,
                                432.200001, 432.499999, "70cm CW & SSB",                true,
                                432.500000, 432.500000, "70cm SSTV",                    true,
                                432.500001, 432.599999, "70cm Transponder Input",       true,
                                432.600000, 432.600000, "70cm Digital",                 true,
                                432.600001, 432.609999, "70cm Transponder Output",      true,
                                432.610000, 432.610000, "70cm PSK",                     true,
                                432.610001, 432.699999, "70cm Transponder Output",      true,
                                432.700000, 432.700000, "70cm PSK",                     true,
                                432.700001, 432.799999, "70cm Transponder Output",      true,
                                432.800000, 432.989999, "70cm Beacons",                 true,
                                432.990000, 432.993999, "70cm Deadband",                true,
                                432.994000, 433.380999, "70cm Repeater Input",          true,
                                433.381000, 433.393999, "70cm Deadband",                true,
                                433.394000, 433.399999, "70cm FM Simplex",              true,
                                433.400000, 433.400000, "70cm FM SSTV",                 true,
                                433.400001, 433.499999, "70cm FM Simplex",              true,
                                433.500000, 433.500000, "70cm FM Calling",              true,
                                433.500001, 433.580999, "70cm FM Simplex",              true,
                                433.581000, 433.599999, "70cm Deadband",                true,
                                433.600000, 433.624999, "70cm All Mode",                true,
                                433.625000, 433.774999, "70cm Digital Modes",           true,
                                433.775000, 433.999999, "70cm All Mode",                true,
                                434.000000, 434.449999, "70cm ATV",                     true,
                                434.450000, 434.474999, "70cm Digital Comms",           true,
                                434.475000, 434.593999, "70cm ATV",                     true,
                                434.594000, 434.980999, "70cm ATV & Repeater Output",   true,
                                434.981000, 437.999999, "70cm ATV & Satellite",         true,
                                438.000000, 438.024999, "70cm ATV & Sub-Regional",      true,
                                438.025000, 438.174999, "70cm Digital Comms",           true,
                                438.175000, 438.199999, "70cm ATV & Sub-Regional",      true,
                                438.200000, 438.524999, "70cm Digital Repeater",        true,
                                438.525000, 438.549999, "70cm ATV & Sub-Regional",      true,
                                438.550000, 438.624999, "70cm multi-mode Channels",     true,
                                438.625000, 438.649999, "70cm ATV & Sub-Regional",      true,
                                438.650000, 439.424999, "70cm Repeater Output",         true,
                                439.425000, 439.799999, "70cm ATV & Sub-Regional",      true,
                                439.800000, 439.974999, "70cm Digital Comm. Link",      true,
                                439.975000, 440.000000, "70cm ATV & Sub-Regional",      true,
                                // 1240 - 1300 MHz
								1240.000000, 1240.999999, "23cm All Modes, Digital",   true,
                                1241.000000, 1242.024999, "23cm All Modes",             true,
                                1242.025000, 1242.699999, "23cm Repeater Output",       true,
                                1242.700000, 1242.724999, "23cm All Modes",             true,
                                1242.725000, 1243.249999, "23cm Packet",                true,
                                1243.250000, 1258.149999, "23cm ATV",                   true,
                                1258.150000, 1259.349999, "23cm Repeater Output",       true,
                                1259.350000, 1259.999999, "23cm ATV",                   true,
                                1260.000000, 1269.999999, "23cm Satellite",             true,
                                1270.000000, 1270.024999, "23cm All Modes",             true,
                                1270.025000, 1270.699999, "23cm Repeater Input",        true,
                                1270.700000, 1270.724999, "23cm All Modes",             true,
                                1270.725000, 1271.249000, "23cm Packet",                true,
                                1271.250000, 1271.999999, "23cm All Modes",             true,
                                1272.000000, 1290.993999, "23cm ATV",                   true,
                                1290.994000, 1291.480999, "23cm NBFM Repeater Input",   true,
                                1291.481000, 1291.493999, "23cm Deadband",              true,
                                1291.494000, 1293.149999, "23cm All Modes",             true,
                                1293.150000, 1293.349999, "23cm Repeater Input",        true,
                                1293.350000, 1295.999999, "23cm All Modes",             true,
                                1296.000000, 1296.024999, "23cm CW EME",                true,
                                1296.025000, 1296.149999, "23cm CW",                    true,
                                1296.150000, 1296.199999, "23cm CW & SSB",              true,
                                1296.200000, 1296.200000, "23cm CW Calling",            true,
                                1296.200001, 1296.399999, "23cm CW & SSB",              true,
                                1296.400000, 1296.499999, "23cm Transponder Input",     true,
                                1296.500000, 1296.500000, "23cm SSTV",                  true,
                                1296.500001, 1296.599999, "23cm Transponder Input",     true,
                                1296.600000, 1296.600000, "23cm RTTY",                  true,
                                1296.600001, 1296.699999, "23cm Transponder Output",    true,
                                1296.700000, 1296.700000, "23cm Digital",               true,
                                1296.700001, 1296.799999, "23cm Transponder Output",    true,
                                1296.800000, 1296.993999, "23cm Beacons",               true,
                                1296.994000, 1297.480999, "23cm NBFM Repeater Output",  true,
                                1297.481000, 1297.493999, "23cm Deadband",              true,
                                1297.494000, 1297.980999, "23cm NBFM Simplex",          true,
                                1297.981000, 1297.999999, "23cm Deadband",              true,
                                1298.000000, 1298.024999, "23cm All Modes",             true,
                                1298.025000, 1298.499999, "23cm Repeater Output",       true,
                                1298.500000, 1298.724999, "23cm All Modes Digital",     true,
                                1298.725000, 1298.999999, "23cm All Modes Packet",      true,
                                1299.000000, 1300.000000, "23cm All Modes Digital",     true,                                
                                // 2300 -2450 MHz
								2300.000000, 2303.999999, "13cm Sub-Regional",         true,
                                2304.000000, 2305.999999, "13cm Narrow Band ",          true,
                                2306.000000, 2307.999999, "13cm Sub-Regional",          true,
                                2308.000000, 2309.999999, "13cm Narrow Band ",          true,
                                2310.000000, 2319.999999, "13cm Sub-Regional",          true,
                                2320.000000, 2320.024999, "13cm CW EME",                true,
                                2320.025000, 2320.149999, "13cm CW",                    true,
                                2320.150000, 2320.199999, "13cm CW & SSB",              true,
                                2320.200000, 2320.200000, "13cm SSB Calling",           true,
                                2320.200001, 2320.799999, "13cm CW & SSB",              true,
                                2320.800000, 2320.999999, "13cm Beacons",               true,
                                2321.000000, 2321.999999, "13cm NBFM Simplex",          true,
                                2322.000000, 2354.999999, "13cm ATV",                   true,
                                2355.000000, 2364.999999, "13cm Digital Comms",         true,
                                2365.000000, 2369.999999, "13cm Repeaters",             true,
                                2370.000000, 2391.999999, "13cm ATV",                   true,
                                2392.000000, 2399.999999, "13cm Digital Comms",         true,
                                2400.000000, 2450.000000, "13cm Satellite",             true,
                                // 3400 -3475 MHz
								3400.000000, 3400.099999, "9cm Narrow Band Modes",      true,
                                3400.100000, 3400.100000, "9cm Narrow Band Calling",    true,
                                3400.100001, 3401.999999, "9cm Narrow Band Modes",      true,
                                3402.000000, 3419.999999, "9cm All Modes",              true,
                                3420.000000, 3429.999999, "9cm All Modes Digital",      true,
                                3430.000000, 3449.999999, "9cm All Modes",              true,
                                3450.000000, 3454.999999, "9cm All Modes Digital",      true,
                                3455.000000, 3475.000000, "9cm All Modes",              true,
                                // 5650 - 5850 MHz
                                5650.000000, 5667.999999, "5cm Satellite Uplink",       true,
                                5668.000000, 5668.199999, "5cm Sat Uplink/Narrow Band", true,
                                5668.200000, 5668.200000, "5cm Narrow Band calling",    true,
                                5668.200001, 5669.999999, "5cm Sat Uplink/Narrow Band", true,
                                5670.000000, 5699.999999, "5cm Digital",                true,
                                5700.000000, 5719.999999, "5cm ATV",                    true,
                                5720.000000, 5759.999999, "5cm All Modes",              true,
                                5760.000000, 5760.199999, "5cm Narrow Band Modes",      true,
                                5760.200000, 5760.200000, "5cm Narrow Band Calling",    true,
                                5760.200001, 5761.999999, "5cm Narrow Band Modes",      true,
                                5762.000000, 5789.999999, "5cm All Modes",              true,
                                5790.000000, 5850.000000, "5cm Satellite Downlink",     true,
                                // 10.000 - 10.500 GHz
								10000.000000, 10149.999999, "3cm Digital",              true,
                                10150.000000, 10249.999999, "3cm All Modes",            true,
                                10250.000000, 10349.999999, "3cm Digital",              true,
                                10350.000000, 10367.999999, "3cm All Modes",            true,
                                10368.000000, 10368.199999, "3cm Narrow Band Modes",    true,
                                10368.200000, 10368.200000, "3cm Narrow Band Calling",  true,
                                10368.200001, 10369.999999, "3cm Narrow Band Modes",    true,
                                10370.000000, 10449.999999, "3cm All Modes",            true,
                                10450.000000, 10500.000000, "3cm Satellite/All Modes",  true,
                                // 24.000 - 24.250 GHz
								24000.000000, 24047.999999, "1.2cm Satellite",          true,
                                24048.000000, 24048.199999, "1.2cm Narrow Band Modes",  true,
                                24048.200000, 24048.200000, "1.2cm Narrow Band Calling",true,
                                24048.200001, 24049.999999, "1.2cm Narrow Band",        true,
                                24050.000000, 24191.999999, "1.2cm All Modes",          true,
                                24192.000000, 24191.199999, "1.2cm All Modes",          true,
                                24192.200000, 24192.200000, "1.2cm Narrow Band Calling",true,
                                24192.200001, 24193.999999, "1.2cm Narrow Band",        true,
                                24194.000000, 24250.000000, "1.2cm All Modes",          true,
                                // 47.000 - 47.200 GHz
                                47000.000000, 47087.999999, "6mm All Mode",             true,
                                47088.000000, 47088.000000, "6mm Narrow Band Calling",  true,
                                47088.000001, 47200.000000, "6mm All Mode",             true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        } // AddRegion1BandTextVHFplus()

        // Region 1 specific Band Text below

        private static void AddBulgariaBandText160m()
        {
            // 1.810 - 1.850 
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                1.810000, 1.835999, "160M CW",                  true,
                                1.836000, 1.836000, "160M CW QRP",              true,
                                1.836001, 1.837999, "160M CW",                  true,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.830000, 1.839999, "160M Narrow Band Modes",   true,
                                1.840000, 1.849999, "160M All Modes & Digital", true,
                                1.850000, 1.999999, "160M General RX",          false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddItalyBandText160m()
        {
            // 1.83 - 1.850 
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                1.810000, 1.829999, "160M General RX",          true,
                                1.830000, 1.836000, "160M CW QRP",              true,
                                1.836001, 1.837999, "160M CW",                  true,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.849999, "160M All Modes & Digital", true,

                                1.850000, 1.999999, "160M General RX",          false,


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddNetherlandsBandText160m()
        {
            // 1.810 - 1.850 
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                1.810000, 1.835999, "160M CW",                  true,
                                1.836000, 1.836000, "160M CW QRP",              true,
                                1.836001, 1.837999, "160M CW",                  true,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.879999, "160M All Modes & Digital", true,

                                1.880000, 1.999999, "160M General RX",          false,




                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddUK_PlusBandText60m() // ke9ns same
        {

            DataTable t = ds.Tables["BandText"];

            object[] data = {


                                5.250000, 5.258499, "60M Band",                 false,
                                5.258500, 5.264000, "60M Band Segment 1",       true,

                                5.264001, 5.275999, "60M Band",                 false,
                                5.276000, 5.284000, "60M Band Segment 2",       true,

                                5.284001, 5.288499, "60M Band",                 false,
                                5.288500, 5.292000, "60M Band Segment 3",       true,

                                5.292001, 5.297999, "60M Band",                 false,
                                5.298000, 5.307000, "60M Band Segment 4",       true,

                                5.307001, 5.312999, "60M Band",                 false,
                                5.313000, 5.323000, "60M Band Segment 5",       true,

                                5.323001, 5.332999, "60M Band",                 false,
                                5.333000, 5.338000, "60M Band Segment 6",       true,

                                5.338001, 5.353999, "60M Band",                 false,
                                5.354000, 5.358000, "60M Band Seg 7 (IARU1)",   true,

                                5.358001, 5.361999, "60M Band",                 false,
                                5.362000, 5.362999, "60M Band Segment 8",       true,
                                5.363000, 5.365999, "60M Band Seg 8 (IARU1)",       true,
                                5.366000, 5.374500, "60M Band Segment 8",       true,

                                5.374501, 5.377999, "60M Band",                 false,
                                5.378000, 5.382000, "60M Band Segment 9",       true,

                                5.382001, 5.394999, "60M Band",                 false,
                                5.395000, 5.401500, "60M Band Segment 10",      true,

                                5.401501, 5.403499, "60M Band",                 false,
                                5.403500, 5.406500, "60M Band Segment 11",      true,

                                5.406501, 5.450000, "60M Band",                 false,

            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }


        // 	{ FLOAT_TO_VITA_MHZ(5.250), FLOAT_TO_VITA_MHZ(5.450) },

        private static void AddNorwayBandText60m() // ke9ns same  (and Denmark)
        {
            DataTable t = ds.Tables["BandText"];

            object[] data = {
                                5.250000, 5.450000, "60M Amateur Service",      true,


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }



        private static void AddSwedenBandText60m() // ke9ns same
        {
            DataTable t = ds.Tables["BandText"];

            object[] data = {
                                5.250000, 5.309999, "60M Band",                 false,
                                5.310000, 5.313000, "60M Band Segment 1",       true,

                                5.313001, 5.319999, "60M Band",                 false,
                                5.320000, 5.323000, "60M Band Segment 2",       true,

                                5.323001, 5.379999, "60M Band",                 false,
                                5.380000, 5.383000, "60M Band Segment 3",       true,

                                5.383001, 5.389999, "60M Band",                 false,
                                5.390000, 5.393000, "60M Band Segment 4",       true,

                                5.393001, 5.450000, "60M Band",                 false,
            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddHungaryBandText40m() // ke9ns same
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                7.000000, 7.029999, "40M CW",                   true,
                                7.030000, 7.030000, "40M CW QRP",               true,
                                7.030001, 7.034999, "40M CW",                   true,
                                7.035000, 7.039999, "40M Narrow Band Modes",    true,
                                7.040000, 7.046999, "40M All Modes",            true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.059999, "40M All Modes",            true,

                                7.060000, 7.060000, "40M SSB Emergency",        true,
                                7.060001, 7.069999, "40M All Modes",          true,

                                7.070000, 7.070000, "40m PSK",                  true,            // ke9ns add
                                7.070001, 7.073999, "40m PSK",                  true,


                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add
                                7.079000, 7.089999, "40M RTTY",                 true,

                                7.090000, 7.090000, "40M SSB QRP",              true,
                                7.090001, 7.199999, "40M All Modes",            true, // mod
                              //  7.100000, 7.299999, "40M General",           false,

                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddItalyPlusBandText40m() // ke9ns same
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                6.975000, 7.029999, "40M CW",                   true,
                                7.030000, 7.030000, "40M CW QRP",               true,
                                7.030001, 7.034999, "40M CW",                   true,
                                7.035000, 7.039999, "40M Narrow Band Modes",    true,
                                7.040000, 7.046999, "40M All Modes",            true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.059999, "40M All Modes",            true,

                                7.060000, 7.060000, "40M SSB Emergency",        true,
                                7.060001, 7.069999, "40M All Modes",            true,

                                7.070000, 7.070000, "40m PSK",                  true,            // ke9ns add
                                7.070001, 7.073999, "40m PSK",                  true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add
                              
                                7.079000, 7.089999, "40M All Modes",            true,

                                7.090000, 7.090000, "40M SSB QRP",              true,
                                7.090001, 7.199999, "40M All Modes",            true,
                                7.200000, 7.299999, "40M RX ONLY",              false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRussiaBandText12m()
        {
            // 24.890 - 25.15
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                24.890000, 24.905999, "12M CW",                 true,
                                24.906000, 24.906000, "12M CW QRP",             true,
                                24.906001, 24.914999, "12M CW",                 true,
                                24.915000, 24.928999, "12M Narrow Band Modes",  true,
                                24.929000, 24.930999, "12M Beacons",            true,

                                24.931000, 24.937999, "12M All Modes Digital",   true,
                                24.938000, 24.938001, "12M DV (Digital Voice)", true,                // ke9ns add    
                                24.938002, 24.939999, "12M All Modes Digital",  true,

                                24.940000, 25.139999, "12M All Modes",           true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRussiaBandText11m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                26.970000, 27.860000, "11M Citizens Band",      true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddEUBandText6m()
        {
            // 50.08 - 51.00
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 50.079999, "6M Beacon Sub-Band",     false,
                                50.080000, 50.089999, "6M CW",                  true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",            true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW, SSB & Digital",   true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",   true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.249999, "6M CW, SSB & Digital",   true,
                                50.250000, 50.250000, "6M PSK Calling",         true,

                                50.250001, 50.275999, "6M CW, SSB & Digital",  true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.284999, "6M All Modes",           true,
                                50.285000, 50.309999, "6M All Modes",           true,

                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.399999, "6M CW, SSB & Digital", true,


                                50.400000, 50.400000, "6M WSPR Beacons",        true,
                                50.400001, 50.499999, "6M CW, SSB & Digital",   true,

                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 50.999999, "6M All Modes",           true,
                                51.000000, 51.999999, "6M General RX",          false,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddLatviaBandText6m()
        {
            // 50.0 - 51.0
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 50.079999, "6M Beacon Sub-Band",     true,
                                50.080000, 50.089999, "6M CW",                  true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",            true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW, SSB & Digital",   true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",   true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.249999, "6M CW, SSB & Digital",   true,
                                50.250000, 50.250000, "6M PSK Calling",         true,

                                50.250001, 50.275999, "6M CW, SSB & Digital",    true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.399999, "6M CW, SSB & Digital", true,

                                50.400000, 50.400000, "6M WSPR Beacons",        true,

                                50.400001, 50.499999, "6M CW, SSB & Digital",    true,

                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 50.999999, "6M All Modes",           true,
                                51.000000, 51.999999, "6M General RX",          false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddBulgariaBandText6m()
        {
            // 50.50 - 52.00
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 50.049999, "6M Beacon Sub-Band RX",  false,
                                50.050000, 50.079999, "6M Beacon Sub-Band",     true,
                                50.080000, 50.089999, "6M CW",                  true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",            true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW, SSB & Digital",   true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",   true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.249999, "6M CW, SSB & Digital",   true,
                                50.250000, 50.250000, "6M PSK Calling",         true,
                                50.250001, 50.275999, "6M CW, SSB & Digital",   true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.399999, "6M CW, SSB & Digital", true,

                                50.400000, 50.400000, "6M WSPR Beacons",        true,
                                50.400001, 50.499999, "6M CW, SSB & Digital", true,

                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 51.999999, "6M All Modes",           true,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddGreeceBandText6m()
        {
            // No transmit
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 51.999999, "6M General RX",  false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        #endregion

        #region IARU2 Region 2 BandText

        // ke9ns console.cs bandtextnumber needs to be incremented if you update database.cs file


        private static void AddBandTextTable()  // Default bandtext -  // ke9ns for FRSRegion.US  AddBandTextSWB() is added in this function
        {
            if (bandtextrefresh == false) ds.Tables.Add("BandText");

            DataTable t = ds.Tables["BandText"];

            if (bandtextrefresh == false)
            {
                t.Columns.Add("Low", typeof(double));
                t.Columns.Add("High", typeof(double));
                t.Columns.Add("Name", typeof(string));
                t.Columns.Add("TX", typeof(bool));
            }

            // FT8 1.84, 3.573, 5.357, 7.074, 10.136, 14.074, 18.1, 21.074, 24.915, 28.074, 50.274? or 50.313

            object[] data = {
                //------------------------------------------------------------------

                             //   0.4720000, 0.472000, "630M JT9/WSPR/CW/Narrow DIGU",            true, // ke9ns add  1.84
                             //   0.4720001, 0.478999, "630M JT9/WSPR/CW/Narrow DIGU",            true, // ke9ns add

                                1.800000, 1.809999, "160M CW/Digital Modes",    true,
                                1.810000, 1.810000, "160M CW QRP",              true,
                                1.810001, 1.837999, "160M CW",                  true, // was 1.842999,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.889999, "160M Phone / Wide Band",   true,
                                1.890000, 1.890000, "160M Phone SSTV",            true,
                                1.890001, 1.907499, "160M Phone / Wide Band",     true,

                                1.907500, 1.907999, "160M Phone/Data (Japan Data)", true, // ke9ns add
                                1.908000, 1.909999, "160M Phone/Data (Japan FT8)",  true, // ke9ns add
                                1.910000, 1.910000, "160M SSB QRP (Japan Data/FT8)",   true,
                                1.910001, 1.912500, "160M SSB QRP (Japan Data/FT8)",  true,

                                1.912501, 1.994999, "160M Phone / Wide Band",  true,

                                1.995000, 1.999998, "160M Experimental",        true,
                                1.999999, 1.999999, "160M Beacon",              true, // ke9ns add

                            //------------------------------------------------------------------
                                3.500000, 3.524999, "80M Extra CW",             true,
                                3.525000, 3.567999, "80M CW",                   true,

                                3.568000, 3.568000, "80M FT4/JT65 DIGU",             true, // ke9ns add  3.573
                                3.568001, 3.572999, "80M FT4/JT65 DIGU",             true, // ke9ns add

                                3.573000, 3.573000, "80M FT8 DIGU",             true, // ke9ns add  3.573
                                3.573001, 3.574999, "80M FT8 DIGU",             true, // ke9ns add

                                3.575000, 3.575000, "80M FT4 DIGU",             true, // ke9ns add  3.573
                                3.575001, 3.578000, "80M FT4 DIGU",             true, // ke9ns add

								3.578001, 3.589999, "80M PSK",                  true,
                                3.590000, 3.590000, "80M RTTY DX",              true,
                                3.590001, 3.599999, "80M RTTY",                 true,

                                3.600000, 3.623999, "80M Extra Phone",            true,
                                3.624000, 3.629999, "80M Extra eSSB",            true, // ke9ns  
                                3.630000, 3.630000, "80M Extra eSSB",            true, // ke9ns 
                                3.630001, 3.699999, "80M Extra Phone",            true,

                                3.700000, 3.789999, "80M Ext/Adv Phone",            true,
                                3.790000, 3.799999, "80M Ext/Adv DX Window",    true,

                                3.800000, 3.824999, "75M Phone",                    true,

                                3.825000, 3.825000, "75M AM Frequency", true,
                                3.825001, 3.844999, "75M Phone",                    true,

                                3.845000, 3.845000, "75M SSTV",                 true,
                                3.845001, 3.869999, "75M Phone",                    true,

                                3.870000, 3.870000, "75M AM Frequency", true,
                                3.870001, 3.879999, "75M Phone",                    true,
                                3.880000, 3.880000, "75M AM Frequency", true,
                                3.880001, 3.884999, "75M Phone",                    true,
                                3.885000, 3.885000, "75M AM Calling Frequency", true,
                                3.885001, 4.000000, "75M Phone",                    true,

                             //------------------------------------------------------------------ US
                           
                              //  5.167500, 5.167500, "60M Emergency Channel",    true, // ke9ns add

                                5.250000, 5.331999, "60M General",              false,
                                5.332000, 5.332000, "60M Channel 1",            true,
                                5.332001, 5.347999, "60M General",              false,
                                5.348000, 5.348000, "60M Channel 2",            true,

                                5.348001, 5.351499, "60M General",              false,

                                5.351500, 5.353999, "60M 200hz NBM IARU1/2 only",  false,
                                5.354000, 5.358499, "60M USB Voice IARU1/2 only",  false,

                                5.358500, 5.358500, "60M Channel 3 (IARU1/2)",  true,

                                5.358501, 5.365999, "60M USB Voice IARU1/2 only",  false,
                                5.366000, 5.366500, "60M 20hz NBM IARU1/2 only",  false,

                                5.366501, 5.372999, "60M General",              false,

                                5.373000, 5.373000, "60M Channel 4",            true,
                                5.373001, 5.404999, "60M General",              false,
                                5.405000, 5.405000, "60M Channel 5",            true,
                                5.405001, 5.450000, "60M General",              false,

    
                                //------------------------------------------------------------------
 								
								7.000000, 7.024999, "40M Extra CW",             true, // ke9ns mod
								7.025000, 7.039999, "40M CW",                   true,

                                7.040000, 7.042999, "40M PSK",                  true,
                                7.043000, 7.046999, "40M RTTY",                 true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.069999, "40M RTTY",                 true,

                                7.070000, 7.070000, "40m PSK",                  true, // ke9ns add
                                7.070001, 7.073999, "40m PSK",                  true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add
                              
								7.079000, 7.099999, "40M RTTY",                 true,
                                7.100000, 7.124999, "40M CW",                   true,
                                7.125000, 7.170999, "40M Ext/Adv Phone",            true,
                                7.171000, 7.171000, "40M SSTV",                 true,

                                7.171001, 7.174999, "40M Ext/Adv Phone",            true,
                                7.175000, 7.176999, "40M Phone",                  true,

                                7.177000, 7.177000, "40m FreeDV(usb)",          true,
                                7.177001, 7.178999, "40m FreeDV(usb)",          true, // ke9ns add 1.25khz, but reserve 2k

                                7.179000, 7.189999, "40M Phone",                    true,

                                7.190000, 7.190000, "40m FreeDV(usb)",          true,
                                7.190001, 7.191999, "40m FreeDV(usb)",          true,

                                7.192000, 7.289999, "40M Phone",                 true,

                                7.290000, 7.290000, "40M AM Calling Frequency", true,

                                7.290001, 7.294999, "40M Phone",                true,
                                7.295000, 7.295000, "40M AM Frequency",        true,

                                7.295001, 7.299999, "40M Phone",                    true,
                            //------------------------------------------------------------------
 
							
								10.100000, 10.129999, "30M CW",                  true,
                                10.130000, 10.135999, "30M RTTY",               true,

                                10.136000, 10.136000, "30M FT8 DIGU",           true, // ke9ns add
                                10.136001, 10.137999, "30M FT8 DIGU",       true, // ke9ns add

                                10.138000, 10.138000, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.138001, 10.138999, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.139000, 10.139999, "30M RTTY",               true,

                                10.140000, 10.140000, "30M FT4 DIGU",           true, // ke9ns add
                                10.140001, 10.142999, "30M FT4 DIGU",       true, // ke9ns add

                                10.143000, 10.149999, "30M Narrow Band Modes", true,

                            //------------------------------------------------------------------

                                14.000000, 14.024999, "20M Extra CW",            true,
                                14.025000, 14.069999, "20M CW",                 true,
                                14.070000, 14.073999, "20M PSK",                true,

                                14.074000, 14.074000, "20M FT8 DIGU",           true, // ke9ns add
                                14.074001, 14.075999, "20M FT8 DIGU",           true, // ke9ns add

                                14.076000, 14.076000, "20M JT65 DIGU",          true, // ke9ns add
                                14.076001, 14.078999, "20M JT65 DIGU",          true, // ke9ns add
                                
                                14.079000, 14.079999, "20M RTTY",               true,

                                14.080000, 14.080000, "20M FT4 DIGU",      true, // ke9ns add
                                14.080001, 14.084999, "20M FT4 DIGU",           true, // ke9ns add

                                14.085000, 14.094999, "20M RTTY",               true,

                                14.095000, 14.099499, "20M Packet",              true,
                                14.099500, 14.099999, "20M CW",                 true,
                                14.100000, 14.100000, "20M NCDXF Beacons",      true,
                                14.100001, 14.100499, "20M CW",                 true,
                                14.100500, 14.111999, "20M Packet",             true,
                                14.112000, 14.149999, "20M CW",                 true,
                                14.150000, 14.174999, "20M Extra Phone",            true,
                                14.175000, 14.224999, "20M Ext/Adv Phone",      true,
                                14.225000, 14.229999, "20M Phone",              true,

                                14.230000, 14.230000, "20M SSTV",               true,
                                14.230001, 14.232999, "20M SSTV",               true,

                                14.233000, 14.233000, "20M EasyPal",            true, // ke9ns add
                                14.233001, 14.235999, "20M EasyPal",            true,

                                14.236000, 14.236000, "20m FreeDV",             true,
                                14.236001, 14.238000, "20m FreeDV",             true,

                                14.238001, 14.239999, "20M Phone",                true,

                                14.240000, 14.240000, "20m FreeDV",             true,
                                14.240001, 14.242000, "20m FreeDV",             true,

                                14.242001, 14.282999, "20M Phone",               true,

                                14.283000, 14.285999, "20M AM ",                true,
                                14.286000, 14.286000, "20M AM Calling Freq",    true,
                                14.286001, 14.288999, "20M AM ",                true,

                                14.289000, 14.339999, "20M Phone",               true,
                                14.340000, 14.340001, "20M DV (Digital Voice)", true,                // ke9ns add   
                                14.340002, 14.349999, "20M Phone",                true,
 
                            //------------------------------------------------------------------
								
	
								18.068000, 18.099999, "17M CW",                  true,

                                18.100000, 18.100000, "17M FT8 DIGU",           true, // ke9ns add
                                18.100001, 18.101999, "17M FT8 DIGU",           true, // ke9ns add

                                18.102000, 18.102000, "17M JT65 DIGU",          true, // ke9ns add
                                18.102001, 18.103999, "17M JT65 DIGU",          true, // ke9ns add

                                18.104000, 18.104000, "17M FT4 DIGU",           true, // ke9ns add
                                18.104001, 18.106999, "17M FT4 DIGU",           true, // ke9ns add

                                18.107000, 18.107999, "17M RTTY",                true,

                                18.108000, 18.109999, "17M PSK / Packet",        true,
                                18.110000, 18.110000, "17M NCDXF Beacons",      true,

                                18.110001, 18.147999, "17M Phone",               true,
                                18.148000, 18.148001, "17M DV (Digital Voice)", true,                // ke9ns add   
								18.148002, 18.167999, "17M Phone",                true,




                            //------------------------------------------------------------------

								21.000000, 21.024999, "15M Extra CW",            true,
                                21.025000, 21.069999, "15M CW",                 true,
                                21.070000, 21.073999, "15M RTTY",               true,

                                21.074000, 21.074000, "15M FT8 DIGU",           true, // ke9ns add 
                                21.074001, 21.075999, "15M FT8 DIGU",           true, // ke9ns add

                                21.076000, 21.076000, "15M JT65 DIGU",          true, // ke9ns add
                                21.076001, 21.078999, "15M JT65 DIGU",          true, // ke9ns add
                                21.079000, 21.099999, "15M RTTY",               true,

                                21.100000, 21.109999, "15M Packet",              true,
                                21.110000, 21.139999, "15M CW",                 true,

                                21.140000, 21.140000, "15M FT4 DIGU",           true, // ke9ns add 
                                21.140001, 21.144999, "15M FT4 DIGU",           true, // ke9ns add

                                21.145000, 21.149999, "15M CW",                   true,

                                21.150000, 21.150000, "15M NCDXF Beacons",       true,
                                21.150001, 21.199999, "15M CW",                 true,
                                21.200000, 21.224999, "15M Extra Phone",            true,
                                21.225000, 21.274999, "15M Ext/Adv Phone",      true,
                                21.275000, 21.339999, "15M Phone",              true,
                                21.340000, 21.340000, "15M SSTV",               true,

                                21.340001, 21.379999, "15M Phone",               true,
                                21.380000, 21.380001, "15M DV (Digital Voice)", true,                // ke9ns add    
                                21.380002, 21.450000, "15M Phone",                true,

                            //------------------------------------------------------------------
							   
								24.890000, 24.914999, "12M CW",                  true,
                                24.915000, 24.915000, "12M FT8 DIGU",           true, // ke9ns add 
                                24.915001, 24.916999, "12M FT8 DIGU",           true, // ke9ns add

                                24.917000, 24.917000, "12M JT65 DIGU",          true, // ke9ns add 
                                24.917001, 24.918999, "12M JT65 DIGU",          true, // ke9ns add

                                24.919000, 24.919000, "12M FT4 DIGU",           true, // ke9ns add 
                                24.919001, 24.921999, "12M FT4 DIGU",           true, // ke9ns add

                                24.922000, 24.924999, "12M RTTY",                true,
                                24.925000, 24.929999, "12M Packet",             true,
                                24.930000, 24.930000, "12M NCDXF Beacons",      true,


                                24.930001, 24.937999, "12M Phone",   true,
                                24.938000, 24.938001, "12M DV (Digital Voice)", true,                // ke9ns add    
                                24.938002, 24.989999, "12M Phone",  true,
                         
                                
                                
                                //------------------------------------------------------------------
							
								28.000000, 28.073999, "10M CW",                  true,

                                28.074000, 28.074000, "10M FT8 DIGU",           true, // ke9ns add 
                                28.074001, 28.075999, "10M FT8 DIGU",           true, // ke9ns add

                                28.076000, 28.076000, "10M JT65 DIGU",          true, // ke9ns add 
                                28.076001, 28.078999, "10M JT65 DIGU",          true, // ke9ns add


								28.079000, 28.149999, "10M RTTY",                true,
                                28.150000, 28.179999, "10M CW",                 true,

                                28.180000, 28.180000, "10M FT4 DIGU",           true, // ke9ns add 
                                28.180001, 28.184999, "10M FT4 DIGU",           true, // ke9ns add

                                28.185000, 28.199999, "10M CW",                 true,

                                28.200000, 28.200000, "10M NCDXF Beacons",       true,
                                28.200001, 28.299999, "10M Beacons",            true,
                                28.300000, 28.499999, "10M Phone-SSB(Nvc/Tech)",                true, // ke9ns Novice Tech
                                28.500000, 28.679999, "10M Phone",              true,
                                28.680000, 28.680000, "10M SSTV",               true,
                                28.680001, 28.999999, "10M Phone",              true,
                                29.000000, 29.199999, "10M AM Phone",               true,
                                29.200000, 29.299999, "10M Phone",              true,
                                29.300000, 29.509999, "10M Satellite Downlinks", true,
                                29.510000, 29.519999, "10M Deadband",           true,
                                29.520000, 29.589999, "10M Repeater Inputs",    true,
                                29.590000, 29.599999, "10M Deadband",           true,
                                29.600000, 29.600000, "10M FM Simplex",         true,
                                29.600001, 29.609999, "10M Deadband",           true,
                                29.610000, 29.699999, "10M Repeater Outputs",   true,
                            //------------------------------------------------------------------
								
								50.000000, 50.059999, "6M CW",                   true,
                                50.060000, 50.079999, "6M Beacon Sub-Band",     true,
                                50.080000, 50.099999, "6M CW",                  true,
                                50.100000, 50.124999, "6M DX Window",           true,

                                50.125000, 50.125000, "6M Calling Frequency",    true, // calling freq

								50.125001, 50.273999, "6M SSB",                  true,
                                50.274000, 50.275999, "6M All Modes",          true,
                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.399999, "6M CW, SSB & Digital", true,

                                50.400000, 50.400000, "6M WSPR Beacons",        true, // ke9ns add
                                50.400001, 50.599999, "6M All Modes",          true,
                                50.600000, 50.619999, "6M Non Voice",           true,
                                50.620000, 50.620000, "6M Digital Packet",      true,
                                50.620001, 50.799999, "6M Non Voice",           true,
                                50.800000, 50.999999, "6M RC",                  true,
                                51.000000, 51.099999, "6M Pacific DX Window",   true,
                                51.100000, 51.119999, "6M Deadband",            true,
                                51.120000, 51.179999, "6M Digital Repeater Inputs", true,
                                51.180000, 51.479999, "6M Repeater Inputs",     true,
                                51.480000, 51.619999, "6M Deadband",            true,
                                51.620000, 51.679999, "6M Digital Repeater Outputs", true,
                                51.680000, 51.979999, "6M Repeater Outputs",    true,
                                51.980000, 51.999999, "6M Deadband",            true,
                                52.000000, 52.019999, "6M Repeater Inputs",     true,
                                52.020000, 52.020000, "6M FM Simplex",          true,
                                52.020001, 52.039999, "6M Repeater Inputs",     true,
                                52.040000, 52.040000, "6M FM Simplex",          true,
                                52.040001, 52.479999, "6M Repeater Inputs",     true,
                                52.480000, 52.499999, "6M Deadband",            true,
                                52.500000, 52.524999, "6M Repeater Outputs",    true,
                                52.525000, 52.525000, "6M Primary FM Simplex",  true,
                                52.525001, 52.539999, "6M Deadband",            true,
                                52.540000, 52.540000, "6M Secondary FM Simplex", true,
                                52.540001, 52.979999, "6M Repeater Outputs",    true,
                                52.980000, 52.999999, "6M Deadbands",           true,
                                53.000000, 53.000000, "6M Remote Base FM Spx",  true,
                                53.000001, 53.019999, "6M Repeater Inputs",     true,
                                53.020000, 53.020000, "6M FM Simplex",          true,
                                53.020001, 53.479999, "6M Repeater Inputs",     true,
                                53.480000, 53.499999, "6M Deadband",            true,
                                53.500000, 53.519999, "6M Repeater Outputs",    true,
                                53.520000, 53.520000, "6M FM Simplex",          true,
                                53.520001, 53.899999, "6M Repeater Outputs",    true,
                                53.900000, 53.900000, "6M FM Simplex",          true,
                                53.900010, 53.979999, "6M Repeater Outputs",    true,
                                53.980000, 53.999999, "6M Deadband",            true,


                                137.100000, 137.100000, "NOAA19 Weather Sat",  false, // ke9ns add .217
                                137.620000, 137.620000, "NOAA15 Weather Sat",  false,
                                137.912500, 137.912500, "NOAA18 Weather Sat",  false,


                                144.000000, 144.099999, "2M CW",              true,

                                144.100000, 144.169999, "2M CW/SSB",          true,
                                144.170000, 144.173999, "2M FT4",               true, // ke9ns add
                                144.174000, 144.176999, "2M FT8",               true,
                                144.177000, 144.199999, "2M CW/SSB",            true,

                                144.200000, 144.200000, "2M Calling",         true,
                                144.200001, 144.274999, "2M CW/SSB",            true,
                                144.275000, 144.299999, "2M Beacon Sub-Band",   true,
                                144.300000, 144.499999, "2M Satellite",         true,
                                144.500000, 144.599999, "2M Linear Translator Inputs", true,
                                144.600000, 144.899999, "2M FM Repeater",       true,
                                144.900000, 145.199999, "2M FM Simplex",        true,
                                145.200000, 145.499999, "2M FM Repeater",       true,
                                145.500000, 145.799999, "2M FM Simplex",        true,
                                145.800000, 145.999999, "2M Satellite",         true,
                                146.000000, 146.399999, "2M FM Repeater",       true,
                                146.400000, 146.609999, "2M FM Simplex",        true,
                                146.610000, 147.389999, "2M FM Repeater",       true,
                                147.390000, 147.599999, "2M FM Simplex",        true,
                                147.600000, 147.999999, "2M FM Repeater",       true,


                                156.050000, 156.050000, "Marine 01 Port Op",  false, // ke9ns add .217
                                156.250000, 156.250000, "Marine 05 Port Op",  false, // ke9ns add .217
                                156.300000, 156.300000, "Marine 06 Safety",  false, // ke9ns add .217
                                156.350000, 156.350000, "Marine 07 commercial",  false, // ke9ns add .217
                                156.400000, 156.400000, "Marine 08 commercial",  false, // ke9ns add .217
                                156.450000, 156.450000, "Marine 09 calling",  false, // ke9ns add .217
                                156.500000, 156.500000, "Marine 10 commercial",  false, // ke9ns add .217
                                156.550000, 156.550000, "Marine 11 commerical",  false, // ke9ns add .217
                              
                                156.600000, 156.600000, "Marine 12 Port Op",  false, // ke9ns add .217
                                156.650000, 156.650000, "Marine 13 Nav Safety",  false, // ke9ns add .217
                                156.700000, 156.700000, "Marine 14 Port Op",  false, // ke9ns add .217
                                156.750000, 156.750000, "Marine 15 EPIRB",  false, // ke9ns add .217
                                156.800000, 156.800000, "Marine 16 int Distres",  false, // ke9ns add .217
                                156.850000, 156.850000, "Marine 17 State Ctrl",  false, // ke9ns add .217

                                156.900000, 156.900000, "Marine 18 commercial",  false, // ke9ns add .217
                                156.950000, 156.950000, "Marine 19 commercial",  false, // ke9ns add .217
                                157.000000, 157.000000, "Marine 20 Pt Op TX",  false, // ke9ns add .217
                                157.050000, 157.050000, "Marine 21 US Gov",  false, // ke9ns add .217
                                157.100000, 157.100000, "Marine 22 Cst Grd Saf",  false, // ke9ns add .217

                                157.150000, 157.150000, "Marine 23 US Gov",  false, // ke9ns add .217
                                157.200000, 157.200000, "Marine 24 public cor",  false, // ke9ns add .217
                                157.250000, 157.250000, "Marine 25 public cor",  false, // ke9ns add .217
                                157.300000, 157.300000, "Marine 26 public cor",  false, // ke9ns add .217
                                157.350000, 157.350000, "Marine 27 public cor",  false, // ke9ns add .217
                                157.400000, 157.400000, "Marine 28 public cor",  false, // ke9ns add .217
                                



                                161.600000, 161.600000, "Marine 20 Pt Op RX",  false, // ke9ns add .217

                                162.400000, 162.400000, "WX channel 2",  false, // ke9ns add .217
                                162.425000, 162.425000, "WX channel 4",  false, // ke9ns add .217
                                162.450000, 162.450000, "WX channel 5",  false, // ke9ns add .217
                                162.475000, 162.475000, "WX channel 3",  false, // ke9ns add .217
                                162.500000, 162.500000, "WX channel 6",  false, // ke9ns add .217
                                162.525000, 162.525000, "WX channel 7",  false, // ke9ns add .217
                                162.550000, 162.550000, "WX channel 1",  false, // ke9ns add .217
                                

                                222.000000, 222.024999, "1.25M EME/Weak Signal",        true,
                                222.025000, 222.049999, "1.25M Weak Signal",            true,
                                222.050000, 222.059999, "1.25M Propagation Beacons",    true,
                                222.060000, 222.099999, "1.25M Weak Signal",            true,
                                222.100000, 222.100000, "1.25M SSB/CW Calling",         true,
                                222.100001, 222.149999, "1.25M Weak Signal CW/SSB",     true,
                                222.150000, 222.249999, "1.25M Local Option",           true,
                                222.250000, 223.380000, "1.25M FM Repeater Inputs",     true,
                                223.380001, 223.399999, "1.25M General",                true,
                                223.400000, 223.519999, "1.25M FM Simplex",             true,
                                223.520000, 223.639999, "1.25M Digital/Packet",         true,
                                223.640000, 223.700000, "1.25M Links/Control",          true,
                                223.700001, 223.709999, "1.25M General",                true,
                                223.710000, 223.849999, "1.25M Local Option",           true,
                                223.850000, 224.980000, "1.25M Repeater Outputs",       true,

                                420.000000, 425.999999, "70cm ATV Repeater",  true,
                                426.000000, 431.999999, "70cm ATV Simplex",     true,

                                432.000000, 432.064999, "70cm EME",                      true,
                                432.065000, 432.067999, "70cm FT8",                     true, // ke9ns add
                                432.068000, 432.069999, "70cm EME",                      true,

                                432.070000, 432.099999, "70cm Weak Signal CW",    true,
                                432.100000, 432.100000, "70cm Calling Frequency", true,
                                432.100001, 432.299999, "70cm Mixed Mode Weak Signal", true,
                                432.300000, 432.399999, "70cm Propagation Beacons", true,
                                432.400000, 432.999999, "70cm Mixed Mode Weak Signal", true,
                                433.000000, 434.999999, "70cm Auxillary/Repeater Links", true,
                                435.000000, 437.999999, "70cm Satellite Only",  true,
                                438.000000, 441.999999, "70cm ATV Repeater",    true,
                                442.000000, 444.999999, "70cm Local Repeaters", true,
                                445.000000, 445.999999, "70cm Local Option",    true,
                                446.000000, 446.000000, "70cm Simplex",         true,
                                446.000001, 446.999999, "70cm Local Option",    true,
                                447.000000, 450.000000, "70cm Local Repeaters", true,

                                462.543751, 462.556250, "Ch15 FRS/GMRS RPTout", true,   //   462550;   .275
                                462.556251, 462.568750, "Ch1 FRS/GMRS low", true,       //   462562.5;
                                462.568751, 462.581250, "Ch16 FRS/GMRS RPTout", true,   //   462575; 
                                462.581251, 462.593750, "Ch2 FRS/GMRS low", true,       //   462587.5; 
                                462.593751, 462.606250, "Ch17 FRS/GMRS RPTout", true,  //   462600; 
                                462.606251, 462.618750, "Ch3 FRS/GMRS low", true,     //   462612.5; 
                                462.618751, 462.631250, "Ch18 FRS/GMRS RPTout", true,   //   462625; 
                                462.631251, 462.643750, "Ch4 FRS/GMRS low", true,       //   462637.5; 
                                462.643751, 462.656250, "Ch19 FRS/GMRS RPTout", true,  //   462650; 
                                462.656251, 462.668750, "Ch5 FRS/GMRS low", true,     //   462662.5; 
                                462.668751, 462.681250, "Ch20 FRS/GMRS RPTout", true,   //   462675; 
                                462.681251, 462.693750, "Ch6 FRS/GMRS low", true,     //   462687.5; 
                                462.693751, 462.706250, "Ch21 FRS/GMRS RPTout", true,   //   462700; 
                                462.706251, 462.718750, "Ch7 FRS/GMRS low", true,     //   462712.5; 
                                462.718751, 462.731250, "Ch22 FRS/GMRS RPTout", true,   //   462725; 
                                
                                467.543751, 467.556250, "Ch15R FRS/GMRS RPTin", true,    //   467550; 
                                467.556251, 467.568750, "Ch8 FRS LP", true,        //   467565.5; 
                                467.568751, 467.581250, "Ch16R FRS/GMRS RPTin", true,      //   462575; 
                                467.581251, 467.593750, "Ch9 FRS LP", true,            //   467587.5;
                                467.593751, 467.606250, "Ch17R FRS/GMRS RPTin", true,      //   467600; 
                                467.606251, 467.618750, "Ch10 FRS LP", true,            //   467612.5; 
                                467.631251, 467.643750, "Ch18R FRS/GMRS RPTin", true,      //   467625; 
                                467.631251, 467.643750, "Ch11 FRS LP", true,            //   467637.5; 
                                467.643751, 467.656250, "Ch19R FRS/GMRS RPTin", true,      //   467650;
                                467.656251, 467.668750, "Ch12 FRS LP", true,            //   467662.5; 
                                467.668751, 467.681250, "Ch20R FRS/GMRS RPTin", true,      //   467675; 
                                467.681251, 467.693750, "Ch13 FRS LP", true,            //   467687.5; 
                                467.693751, 467.706250, "Ch21R FRS/GMRS RPTin", true,      //   467700; 
                                467.706251, 467.718750, "Ch14 FRS LP", true,            //   467712.5; 
                                467.718751, 467.731250, "Ch22R FRS/GMRS RPTin", true,      //   467725; 


                                902.000000, 902.099999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
                                902.100000, 902.100000, "33cm Weak Signal Calling", true,
                                902.100001, 902.799999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
                                902.800000, 902.999999, "33cm Weak Signal EME/CW", true,
                                903.000000, 903.099999, "33cm Digital Modes",   true,
                                903.100000, 903.100000, "33cm Alternate Calling", true,
                                903.100001, 905.999999, "33cm Digital Modes",   true,
                                906.000000, 908.999999, "33cm FM Repeater Inputs", true,
                                909.000000, 914.999999, "33cm ATV",             true,
                                915.000000, 917.999999, "33cm Digital Modes",   true,
                                918.000000, 920.999999, "33cm FM Repeater Outputs", true,
                                921.000000, 926.999999, "33cm ATV",             true,
                                927.000000, 928.000000, "33cm FM Simplex/Links", true,

                                1240.000000, 1245.999999, "23cm ATV #1",       true,
                                1246.000000, 1251.999999, "23cm FM Point/Links", true,
                                1252.000000, 1257.999999, "23cm ATV #2, Digital Modes", true,
                                1258.000000, 1259.999999, "23cm FM Point/Links", true,
                                1260.000000, 1269.999999, "23cm Sat Uplinks/Wideband Exp.", true,
                                1270.000000, 1275.999999, "23cm Repeater Inputs", true,
                                1276.000000, 1281.999999, "23cm ATV #3",        true,
                                1282.000000, 1287.999999, "23cm Repeater Outputs",  true,
                                1288.000000, 1293.999999, "23cm Simplex ATV/Wideband Exp.", true,
                                1294.000000, 1294.499999, "23cm Simplex FM",        true,
                                1294.500000, 1294.500000, "23cm FM Simplex Calling", true,
                                1294.500001, 1294.999999, "23cm Simplex FM",        true,
                                1295.000000, 1295.799999, "23cm SSTV/FAX/ACSSB/Exp.", true,
                                1295.800000, 1295.999999, "23cm EME/CW Expansion",  true,
                                1296.000000, 1296.049999, "23cm EME Exclusive",     true,
                                1296.050000, 1296.069999, "23cm Weak Signal",       true,
                                1296.070000, 1296.079999, "23cm CW Beacons",        true,
                                1296.080000, 1296.099999, "23cm Weak Signal",       true,
                                1296.100000, 1296.100000, "23cm CW/SSB Calling",    true,
                                1296.100001, 1296.399999, "23cm Weak Signal",       true,
                                1296.400000, 1296.599999, "23cm X-Band Translator Input", true,
                                1296.600000, 1296.799999, "23cm X-Band Translator Output", true,
                                1296.800000, 1296.999999, "23cm Experimental Beacons", true,
                                1297.000000, 1300.000000, "23cm Digital Modes",     true,

                                2300.000000, 2302.999999, "13cm High Data Rate", true,
                                2303.000000, 2303.499999, "13cm Packet",      true,
                                2303.500000, 2303.800000, "13cm TTY Packet",  true,
                                2303.800001, 2303.899999, "13cm General", true,
                                2303.900000, 2303.900000, "13cm Packet/TTY/CW/EME", true,
                                2303.900001, 2304.099999, "13cm CW/EME",      true,
                                2304.100000, 2304.100000, "13cm Calling Frequency", true,
                                2304.100001, 2304.199999, "13cm CW/EME/SSB",  true,
                                2304.200000, 2304.299999, "13cm SSB/SSTV/FAX/Packet AM/Amtor", true,
                                2304.300000, 2304.319999, "13cm Propagation Beacon Network", true,
                                2304.320000, 2304.399999, "13cm General Propagation Beacons", true,
                                2304.400000, 2304.499999, "13cm SSB/SSTV/ACSSB/FAX/Packet AM", true,
                                2304.500000, 2304.699999, "13cm X-Band Translator Input", true,
                                2304.700000, 2304.899999, "13cm X-Band Translator Output", true,
                                2304.900000, 2304.999999, "13cm Experimental Beacons", true,
                                2305.000000, 2305.199999, "13cm FM Simplex", true,
                                2305.200000, 2305.200000, "13cm FM Simplex Calling", true,
                                2305.200001, 2305.999999, "13cm FM Simplex", true,
                                2306.000000, 2308.999999, "13cm FM Repeaters", true,
                                2309.000000, 2310.000000, "13cm Control/Aux Links", true,

                                2390.000000, 2395.999999, "13cm Fast-Scan TV", true,
                                2396.000000, 2398.999999, "13cm High Rate Data", true,
                                2399.000000, 2399.499999, "13cm Packet", true,
                                2399.500000, 2399.999999, "13cm Control/Aux Links", true,
                                2400.000000, 2402.999999, "13cm Satellite", true,
                                2403.000000, 2407.999999, "13cm Satellite High-Rate Data", true,
                                2408.000000, 2409.999999, "13cm Satellite", true,
                                2410.000000, 2412.999999, "13cm FM Repeaters", true,
                                2413.000000, 2417.999999, "13cm High-Rate Data", true,
                                2418.000000, 2429.999999, "13cm Fast-Scan TV", true,
                                2430.000000, 2432.999999, "13cm Satellite", true,
                                2433.000000, 2437.999999, "13cm Sat. High-Rate Data", true,
                                2438.000000, 2450.000000, "13cm Wideband FM/FSTV/FMTV", true,

                                3300.000000, 3455.999999, "9cm General", true,
                                3456.000000, 3456.099999, "9cm General", true,
                                3456.100000, 3456.100000, "9cm Calling Frequency", true,
                                3456.100001, 3456.299999, "9cm General", true,
                                3456.300000, 3456.400000, "9cm Propagation Beacons", true,
                                3456.400001, 3500.000000, "9cm General", true,

                                5650.000000, 5759.999999, "5cm General", true,
                                5760.000000, 5760.099999, "5cm General", true,
                                5760.100000, 5760.100000, "5cm Calling Frequency", true,
                                5760.100001, 5760.299999, "5cm General", true,
                                5760.300000, 5760.400000, "5cm Propagation Beacons", true,
                                5760.400001, 5925.000000, "5cm General", true,

                                10000.000000, 10367.999999, "3cm General", true,
                                10368.000000, 10368.099999, "3cm General", true,
                                10368.100000, 10368.100000, "3cm Calling Frequency", true,
                                10368.100001, 10368.400000, "3cm General", true,
                                10368.400001, 10449.999999, "3cm General", true,
                                10450.400000, 10500.000000, "3cm Amateur Satellite", true,

                                24000.000000, 24049.999999, "1.2cm Amateur Satellite", true,
                                24050.000000, 24191.999999, "1.2cm General", true,
                                24192.000000, 24192.099999, "1.2cm General", true,
                                24192.100000, 24192.100000, "1.2cm Calling Frequency", true,
                                24192.100001, 24192.400000, "1.2cm General", true,
                                24192.400001, 24250.000000, "1.2cm General", true,

                                47000.000000, 47087.999999, "47GHz General", true,
                                47088.000000, 47088.099999, "47GHz General", true,
                                47088.100000, 47088.100000, "47GHz Calling Frequency", true,
                                47088.100001, 47088.400000, "47GHz General", true,
                                47088.400001, 47200.000000, "47GHz General", true,

                                76000.000000, 81000.000000, "76GHz General", true,

            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }

            AddBandTextSWB(); // ke9ns add CB

        } // AddBandTextTable() US REgion

        private static void AddBand2TextTable()  // IARU2 ham bandtext (because 60m is like IARU1)
        {
            if (bandtextrefresh == false) ds.Tables.Add("BandText");

            DataTable t = ds.Tables["BandText"];

            if (bandtextrefresh == false)
            {
                t.Columns.Add("Low", typeof(double));
                t.Columns.Add("High", typeof(double));
                t.Columns.Add("Name", typeof(string));
                t.Columns.Add("TX", typeof(bool));
            }


            object[] data = {
                                1.800000, 1.809999, "160M CW/Digital Modes",    true,
                                1.810000, 1.810000, "160M CW QRP",              true,
                                1.810001, 1.837999, "160M CW",                  true, // was 1.842999,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.909999, "160M SSB/SSTV/Wide Band",  true,
                                1.910000, 1.910000, "160M SSB QRP",             true,
                                1.910001, 1.994999, "160M SSB/SSTV/Wide Band",  true,
                                1.995000, 1.999999, "160M Experimental",        true,


                                3.500000, 3.524999, "80M Extra CW",             true,
                                3.525000, 3.567999, "80M CW",                   true,

                                3.568000, 3.568000, "80M FT4/JT65 DIGU",             true, // ke9ns add  3.573
                                3.568001, 3.572999, "80M FT4/JT65 DIGU",             true, // ke9ns add

                                3.573000, 3.573000, "80M FT8 DIGU",             true, // ke9ns add  3.573
                                3.573001, 3.574999, "80M FT8 DIGU",             true, // ke9ns add

                                3.575000, 3.575000, "80M FT4 DIGU",             true, // ke9ns add  3.573
                                3.575001, 3.578000, "80M FT4 DIGU",             true, // ke9ns add
                              
								3.578001, 3.589999, "80M PSK",                  true,
                                3.590000, 3.590000, "80M RTTY DX",              true,
                                3.590001, 3.599999, "80M RTTY",                 true,

                                3.600000, 3.629999, "80M Extra SSB",            true,
                                3.630000, 3.630000, "80M Extra eSSB",            true, // ke9ns
                                3.630001, 3.699999, "80M Extra SSB",            true,


                                3.700000, 3.789999, "80M Ext/Adv SSB",          true,
                                3.790000, 3.799999, "80M Ext/Adv DX Window",    true,
                                3.800000, 3.844999, "75M SSB",                  true,
                                3.845000, 3.845000, "75M SSTV",                 true,
                                3.845001, 3.884999, "75M SSB",                  true,
                                3.885000, 3.885000, "75M AM Calling Frequency", true,
                                3.885001, 4.000000, "75M SSB",                  true,

                                //===================================================================== IARU 2
                              
                                5.250000, 5.331999, "60M General",              false,
                                5.332000, 5.332000, "60M Channel 1",            true,
                                5.332001, 5.347999, "60M General",              false,
                                5.348000, 5.348000, "60M Channel 2",            true,

                                5.348001, 5.351499, "60M General",              false,

                                5.351500, 5.353999, "60M 200hz NBM (IARU1/2)",  true,
                                5.354000, 5.358499, "60M USB Voice (IARU1/2)",  true,

                                5.358500, 5.358500, "60M Channel 3 (IARU1/2)",  true,

                                5.358501, 5.365999, "60M USB Voice (IARU1/2)",  true,
                                5.366000, 5.366500, "60M 20hz  NBM (IARU1/2)",  true,

                                5.366501, 5.372999, "60M General",              false,

                                5.373000, 5.373000, "60M Channel 4",            true,
                                5.373001, 5.404999, "60M General",              false,
                                5.405000, 5.405000, "60M Channel 5",            true,
                                5.405001, 5.450000, "60M General",              false,


                                //==========================================================================
                                7.000000, 7.024999, "40M Extra CW",             true, // ke9ns mod
								7.025000, 7.039999, "40M CW",                   true,

                                7.040000, 7.042999, "40M PSK",                  true,
                                7.043000, 7.046999, "40M RTTY",                 true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.069999, "40M RTTY",                 true,

                                7.070000, 7.070000, "40m PSK",                  true,            // ke9ns add
                                7.070001, 7.073999, "40m PSK",                  true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add
                              
								7.079000, 7.099999, "40M RTTY",                 true,
                                7.100000, 7.124999, "40M CW",                   true,
                                7.125000, 7.170999, "40M Ext/Adv SSB",          true,
                                7.171000, 7.171000, "40M SSTV",                 true,
                                7.171001, 7.174999, "40M Ext/Adv SSB",          true,
                                7.175000, 7.289999, "40M SSB",                  true,
                                7.290000, 7.290000, "40M AM Calling Frequency", true,
                                7.290001, 7.299999, "40M SSB",                  true,


                                10.100000, 10.129999, "30M CW",                 true,
                                10.130000, 10.135999, "30M RTTY",               true,

                                10.136000, 10.136000, "30M FT8 DIGU",           true, // ke9ns add
                                10.136001, 10.137999, "30M FT8 DIGU",       true, // ke9ns add

                                10.138000, 10.138000, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.138001, 10.138999, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.139000, 10.139999, "30M RTTY",               true,

                                10.140000, 10.140000, "30M FT4 DIGU",           true, // ke9ns add
                                10.140001, 10.142999, "30M FT4 DIGU",       true, // ke9ns add

                                10.143000, 10.149999, "30M Narrow Band Modes", true,

                                14.000000, 14.024999, "20M Extra CW",           true,
                                14.025000, 14.069999, "20M CW",                 true,
                                14.070000, 14.073999, "20M PSK",                true,

                                14.074000, 14.074000, "20M FT8 DIGU",           true, // ke9ns add
                                14.074001, 14.075999, "20M FT8 DIGU",           true, // ke9ns add

                                14.076000, 14.076000, "20M JT65 DIGU",          true, // ke9ns add
                                14.076001, 14.078999, "20M JT65 DIGU",          true, // ke9ns add

                                14.079000, 14.079999, "20M RTTY",               true,

                                14.080000, 14.080000, "20M FT4 DIGU",      true, // ke9ns add
                                14.080001, 14.084999, "20M FT4 DIGU",           true, // ke9ns add

                                14.085000, 14.094999, "20M RTTY",               true,


                                14.095000, 14.099499, "20M Packet",             true,
                                14.099500, 14.099999, "20M CW",                 true,
                                14.100000, 14.100000, "20M NCDXF Beacons",      true,
                                14.100001, 14.100499, "20M CW",                 true,
                                14.100500, 14.111999, "20M Packet",             true,
                                14.112000, 14.149999, "20M CW",                 true,
                                14.150000, 14.174999, "20M Extra SSB",          true,
                                14.175000, 14.224999, "20M Ext/Adv SSB",        true,
                                14.225000, 14.229999, "20M SSB",                true,

                                14.230000, 14.230000, "20M SSTV",          true,
                                14.230001, 14.232999, "20M SSTV",               true,

                                14.233000, 14.233000, "20M EasyPal",        true,
                                14.233001, 14.235999, "20M EasyPal",        true,

                                14.236000, 14.282999, "20M SSB",                true,
                                14.283000, 14.285999, "20M AM ", true,
                                14.286000, 14.286000, "20M AM Calling Freq", true,
                                14.286001, 14.288999, "20M AM ", true,

                                14.289000, 14.339999, "20M SSB",                true,
                                14.340000, 14.340001, "20M DV (Digital Voice)", true,                // ke9ns add   
                                14.340002, 14.349999, "20M SSB",          true,




                                18.068000, 18.099999, "17M CW",                 true,
                                18.100000, 18.100000, "17M FT8 DIGU",           true, // ke9ns add
                                18.100001, 18.101999, "17M FT8 DIGU",           true, // ke9ns add

                                18.102000, 18.102000, "17M JT65 DIGU",          true, // ke9ns add
                                18.102001, 18.103999, "17M JT65 DIGU",          true, // ke9ns add

                                18.104000, 18.104000, "17M FT4 DIGU",           true, // ke9ns add
                                18.104001, 18.106999, "17M FT4 DIGU",           true, // ke9ns add

                                18.107000, 18.107999, "17M RTTY",               true,
                                18.108000, 18.109999, "17M PSK / Packet",               true,

                                18.110000, 18.110000, "17M NCDXF Beacons",      true,

                                18.110001, 18.147999, "17M SSB",                true,
                                18.148000, 18.148001, "17M DV (Digital Voice)", true,                // ke9ns add   
								18.148002, 18.167999, "17M SSB",                true,

                                21.000000, 21.024999, "15M Extra CW",           true,
                                21.025000, 21.069999, "15M CW",                 true,
                                21.070000, 21.073999, "15M RTTY",               true,

                                21.074000, 21.074000, "15M FT8 DIGU",           true, // ke9ns add 
                                21.074001, 21.075999, "15M FT8 DIGU",           true, // ke9ns add

                                21.076000, 21.076000, "15M JT65 DIGU",          true, // ke9ns add
                                21.076001, 21.078999, "15M JT65 DIGU",          true, // ke9ns add
                                21.079000, 21.099999, "15M RTTY",               true,

                                21.100000, 21.109999, "15M Packet",             true,
                                21.110000, 21.139999, "15M CW",                 true,

                                21.140000, 21.140000, "15M FT4 DIGU",           true, // ke9ns add 
                                21.140001, 21.144999, "15M FT4 DIGU",           true, // ke9ns add

                                21.145000, 21.149999, "15M CW",                   true,

                                21.150000, 21.150000, "15M NCDXF Beacons",      true,
                                21.150001, 21.199999, "15M CW",                 true,
                                21.200000, 21.224999, "15M Extra SSB",          true,
                                21.225000, 21.274999, "15M Ext/Adv SSB",        true,
                                21.275000, 21.339999, "15M SSB",                true,
                                21.340000, 21.340000, "15M SSTV",               true,

                                21.340001, 21.379999, "15M SSB",                true,
                                21.380000, 21.380001, "15M DV (Digital Voice)", true,                // ke9ns add    
                                21.380002, 21.450000, "15M SSB",                true,


                                24.890000, 24.914999, "12M CW",                 true,
                                24.915000, 24.915000, "12M FT8 DIGU",           true, // ke9ns add 
                                24.915001, 24.916999, "12M FT8 DIGU",           true, // ke9ns add

                                24.917000, 24.917000, "12M JT65 DIGU",          true, // ke9ns add 
                                24.917001, 24.918999, "12M JT65 DIGU",          true, // ke9ns add

                                24.919000, 24.919000, "12M FT4 DIGU",           true, // ke9ns add 
                                24.919001, 24.921999, "12M FT4 DIGU",           true, // ke9ns add

                                24.922000, 24.924999, "12M RTTY",               true,
                                24.925000, 24.929999, "12M Packet",             true,
                                24.930000, 24.930000, "12M NCDXF Beacons",      true,

                                24.930001, 24.987999, "12M SSB",                true,
                                24.938000, 24.938001, "12M DV (Digital Voice)", true,                // ke9ns add 
                                24.938002, 24.989999, "12M SSB",                true,


                                28.000000, 28.073999, "10M CW",                 true,

                                28.074000, 28.074000, "10M FT8 DIGU",           true, // ke9ns add 
                                28.074001, 28.075999, "10M FT8 DIGU",           true, // ke9ns add
                                28.076000, 28.076000, "10M JT65 DIGU",          true, // ke9ns add 
                                28.076001, 28.078999, "10M JT65 DIGU",          true, // ke9ns add


								28.079000, 28.149999, "10M RTTY",               true,
                                28.150000, 28.179999, "10M CW",                  true,

                                28.180000, 28.180000, "10M FT4 DIGU",           true, // ke9ns add 
                                28.180001, 28.184999, "10M FT4 DIGU",           true, // ke9ns add

                                28.185000, 28.199999, "10M CW",                 true,

                                28.200000, 28.200000, "10M NCDXF Beacons",      true,
                                28.200001, 28.299999, "10M Beacons",            true,

                                28.300000, 28.399999, "10M SSB",                true,
                                28.400000, 28.400001, "10M DV (Digital Voice)", true,                // ke9ns add
                                28.400002, 28.679999, "10M SSB",                true,

                                28.680000, 28.680000, "10M SSTV",               true,
                                28.680001, 28.999999, "10M SSB",                true,
                                29.000000, 29.199999, "10M AM",                 true,
                                29.200000, 29.299999, "10M SSB",                true,
                                29.300000, 29.509999, "10M Satellite Downlinks", true,
                                29.510000, 29.519999, "10M Deadband",           true,
                                29.520000, 29.589999, "10M Repeater Inputs",    true,
                                29.590000, 29.599999, "10M Deadband",           true,
                                29.600000, 29.600000, "10M FM Simplex",         true,
                                29.600001, 29.609999, "10M Deadband",           true,
                                29.610000, 29.699999, "10M Repeater Outputs",   true,

                                50.000000, 50.059999, "6M CW",                  true,
                                50.060000, 50.079999, "6M Beacon Sub-Band",     true,
                                50.080000, 50.099999, "6M CW",                  true,
                                50.100000, 50.124999, "6M DX Window",           true,

                                50.125000, 50.125000, "6M Calling Frequency",   true, // calling freq

								50.125001, 50.209999, "6M SSB",                 true,
                                50.210000, 50.210001, "6M DV (Digital Voice)",  true,                // ke9ns add
                                50.210002, 50.273999, "6M SSB",                 true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.599999, "6M All Modes", true,

                                50.600000, 50.619999, "6M Non Voice",           true,
                                50.620000, 50.620000, "6M Digital Packet",      true,
                                50.620001, 50.799999, "6M Non Voice",           true,
                                50.800000, 50.999999, "6M RC",                  true,
                                51.000000, 51.099999, "6M Pacific DX Window",   true,
                                51.100000, 51.119999, "6M Deadband",            true,
                                51.120000, 51.179999, "6M Digital Repeater Inputs", true,
                                51.180000, 51.479999, "6M Repeater Inputs",     true,
                                51.480000, 51.619999, "6M Deadband",            true,
                                51.620000, 51.679999, "6M Digital Repeater Outputs", true,
                                51.680000, 51.979999, "6M Repeater Outputs",    true,
                                51.980000, 51.999999, "6M Deadband",            true,
                                52.000000, 52.019999, "6M Repeater Inputs",     true,
                                52.020000, 52.020000, "6M FM Simplex",          true,
                                52.020001, 52.039999, "6M Repeater Inputs",     true,
                                52.040000, 52.040000, "6M FM Simplex",          true,
                                52.040001, 52.479999, "6M Repeater Inputs",     true,
                                52.480000, 52.499999, "6M Deadband",            true,
                                52.500000, 52.524999, "6M Repeater Outputs",    true,
                                52.525000, 52.525000, "6M Primary FM Simplex",  true,
                                52.525001, 52.539999, "6M Deadband",            true,
                                52.540000, 52.540000, "6M Secondary FM Simplex", true,
                                52.540001, 52.979999, "6M Repeater Outputs",    true,
                                52.980000, 52.999999, "6M Deadbands",           true,
                                53.000000, 53.000000, "6M Remote Base FM Spx",  true,
                                53.000001, 53.019999, "6M Repeater Inputs",     true,
                                53.020000, 53.020000, "6M FM Simplex",          true,
                                53.020001, 53.479999, "6M Repeater Inputs",     true,
                                53.480000, 53.499999, "6M Deadband",            true,
                                53.500000, 53.519999, "6M Repeater Outputs",    true,
                                53.520000, 53.520000, "6M FM Simplex",          true,
                                53.520001, 53.899999, "6M Repeater Outputs",    true,
                                53.900000, 53.900000, "6M FM Simplex",          true,
                                53.900010, 53.979999, "6M Repeater Outputs",    true,
                                53.980000, 53.999999, "6M Deadband",            true,

                                144.000000, 144.099999, "2M CW",                true,

                                144.100000, 144.169999, "2M CW/SSB",            true,
                                144.170000, 144.173999, "2M FT4",               true, // ke9ns add
                                144.174000, 144.176999, "2M FT8",               true,
                                144.177000, 144.199999, "2M CW/SSB",            true,

                                144.200000, 144.200000, "2M Calling",           true,
                                144.200001, 144.274999, "2M CW/SSB",            true,
                                144.275000, 144.299999, "2M Beacon Sub-Band",   true,
                                144.300000, 144.499999, "2M Satellite",         true,
                                144.500000, 144.599999, "2M Linear Translator Inputs", true,
                                144.600000, 144.899999, "2M FM Repeater",       true,
                                144.900000, 145.199999, "2M FM Simplex",        true,
                                145.200000, 145.499999, "2M FM Repeater",       true,
                                145.500000, 145.799999, "2M FM Simplex",        true,
                                145.800000, 145.999999, "2M Satellite",         true,
                                146.000000, 146.399999, "2M FM Repeater",       true,
                                146.400000, 146.609999, "2M FM Simplex",        true,
                                146.610000, 147.389999, "2M FM Repeater",       true,
                                147.390000, 147.599999, "2M FM Simplex",        true,
                                147.600000, 147.999999, "2M FM Repeater",       true,

                                222.000000, 222.024999, "1.25M EME/Weak Signal",        true,
                                222.025000, 222.049999, "1.25M Weak Signal",            true,
                                222.050000, 222.059999, "1.25M Propagation Beacons",    true,
                                222.060000, 222.099999, "1.25M Weak Signal",            true,
                                222.100000, 222.100000, "1.25M SSB/CW Calling",         true,
                                222.100001, 222.149999, "1.25M Weak Signal CW/SSB",     true,
                                222.150000, 222.249999, "1.25M Local Option",           true,
                                222.250000, 223.380000, "1.25M FM Repeater Inputs",     true,
                                223.380001, 223.399999, "1.25M General",                true,
                                223.400000, 223.519999, "1.25M FM Simplex",             true,
                                223.520000, 223.639999, "1.25M Digital/Packet",         true,
                                223.640000, 223.700000, "1.25M Links/Control",          true,
                                223.700001, 223.709999, "1.25M General",                true,
                                223.710000, 223.849999, "1.25M Local Option",           true,
                                223.850000, 224.980000, "1.25M Repeater Outputs",       true,

                                420.000000, 425.999999, "70cm ATV Repeater",    true,
                                426.000000, 431.999999, "70cm ATV Simplex",     true,

                                432.000000, 432.064999, "70cm EME",                    true,
                                432.065000, 432.067999, "70cm FT8",                     true, // ke9ns add
                                432.068000, 432.069999, "70cm EME",                      true,

                                432.070000, 432.099999, "70cm Weak Signal CW",  true,
                                432.100000, 432.100000, "70cm Calling Frequency", true,
                                432.100001, 432.299999, "70cm Mixed Mode Weak Signal", true,
                                432.300000, 432.399999, "70cm Propagation Beacons", true,
                                432.400000, 432.999999, "70cm Mixed Mode Weak Signal", true,
                                433.000000, 434.999999, "70cm Auxillary/Repeater Links", true,
                                435.000000, 437.999999, "70cm Satellite Only",  true,
                                438.000000, 441.999999, "70cm ATV Repeater",    true,
                                442.000000, 444.999999, "70cm Local Repeaters", true,
                                445.000000, 445.999999, "70cm Local Option",    true,
                                446.000000, 446.000000, "70cm Simplex",         true,
                                446.000001, 446.999999, "70cm Local Option",    true,
                                447.000000, 450.000000, "70cm Local Repeaters", true,

                                902.000000, 902.099999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
                                902.100000, 902.100000, "33cm Weak Signal Calling", true,
                                902.100001, 902.799999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
                                902.800000, 902.999999, "33cm Weak Signal EME/CW", true,
                                903.000000, 903.099999, "33cm Digital Modes",   true,
                                903.100000, 903.100000, "33cm Alternate Calling", true,
                                903.100001, 905.999999, "33cm Digital Modes",   true,
                                906.000000, 908.999999, "33cm FM Repeater Inputs", true,
                                909.000000, 914.999999, "33cm ATV",             true,
                                915.000000, 917.999999, "33cm Digital Modes",   true,
                                918.000000, 920.999999, "33cm FM Repeater Outputs", true,
                                921.000000, 926.999999, "33cm ATV",             true,
                                927.000000, 928.000000, "33cm FM Simplex/Links", true,

                                1240.000000, 1245.999999, "23cm ATV #1",        true,
                                1246.000000, 1251.999999, "23cm FM Point/Links", true,
                                1252.000000, 1257.999999, "23cm ATV #2, Digital Modes", true,
                                1258.000000, 1259.999999, "23cm FM Point/Links", true,
                                1260.000000, 1269.999999, "23cm Sat Uplinks/Wideband Exp.", true,
                                1270.000000, 1275.999999, "23cm Repeater Inputs", true,
                                1276.000000, 1281.999999, "23cm ATV #3",        true,
                                1282.000000, 1287.999999, "23cm Repeater Outputs",  true,
                                1288.000000, 1293.999999, "23cm Simplex ATV/Wideband Exp.", true,
                                1294.000000, 1294.499999, "23cm Simplex FM",        true,
                                1294.500000, 1294.500000, "23cm FM Simplex Calling", true,
                                1294.500001, 1294.999999, "23cm Simplex FM",        true,
                                1295.000000, 1295.799999, "23cm SSTV/FAX/ACSSB/Exp.", true,
                                1295.800000, 1295.999999, "23cm EME/CW Expansion",  true,
                                1296.000000, 1296.049999, "23cm EME Exclusive",     true,
                                1296.050000, 1296.069999, "23cm Weak Signal",       true,
                                1296.070000, 1296.079999, "23cm CW Beacons",        true,
                                1296.080000, 1296.099999, "23cm Weak Signal",       true,
                                1296.100000, 1296.100000, "23cm CW/SSB Calling",    true,
                                1296.100001, 1296.399999, "23cm Weak Signal",       true,
                                1296.400000, 1296.599999, "23cm X-Band Translator Input", true,
                                1296.600000, 1296.799999, "23cm X-Band Translator Output", true,
                                1296.800000, 1296.999999, "23cm Experimental Beacons", true,
                                1297.000000, 1300.000000, "23cm Digital Modes",     true,

                                2300.000000, 2302.999999, "13cm High Data Rate", true,
                                2303.000000, 2303.499999, "13cm Packet",      true,
                                2303.500000, 2303.800000, "13cm TTY Packet",  true,
                                2303.800001, 2303.899999, "13cm General", true,
                                2303.900000, 2303.900000, "13cm Packet/TTY/CW/EME", true,
                                2303.900001, 2304.099999, "13cm CW/EME",      true,
                                2304.100000, 2304.100000, "13cm Calling Frequency", true,
                                2304.100001, 2304.199999, "13cm CW/EME/SSB",  true,
                                2304.200000, 2304.299999, "13cm SSB/SSTV/FAX/Packet AM/Amtor", true,
                                2304.300000, 2304.319999, "13cm Propagation Beacon Network", true,
                                2304.320000, 2304.399999, "13cm General Propagation Beacons", true,
                                2304.400000, 2304.499999, "13cm SSB/SSTV/ACSSB/FAX/Packet AM", true,
                                2304.500000, 2304.699999, "13cm X-Band Translator Input", true,
                                2304.700000, 2304.899999, "13cm X-Band Translator Output", true,
                                2304.900000, 2304.999999, "13cm Experimental Beacons", true,
                                2305.000000, 2305.199999, "13cm FM Simplex", true,
                                2305.200000, 2305.200000, "13cm FM Simplex Calling", true,
                                2305.200001, 2305.999999, "13cm FM Simplex", true,
                                2306.000000, 2308.999999, "13cm FM Repeaters", true,
                                2309.000000, 2310.000000, "13cm Control/Aux Links", true,

                                2390.000000, 2395.999999, "13cm Fast-Scan TV", true,
                                2396.000000, 2398.999999, "13cm High Rate Data", true,
                                2399.000000, 2399.499999, "13cm Packet", true,
                                2399.500000, 2399.999999, "13cm Control/Aux Links", true,
                                2400.000000, 2402.999999, "13cm Satellite", true,
                                2403.000000, 2407.999999, "13cm Satellite High-Rate Data", true,
                                2408.000000, 2409.999999, "13cm Satellite", true,
                                2410.000000, 2412.999999, "13cm FM Repeaters", true,
                                2413.000000, 2417.999999, "13cm High-Rate Data", true,
                                2418.000000, 2429.999999, "13cm Fast-Scan TV", true,
                                2430.000000, 2432.999999, "13cm Satellite", true,
                                2433.000000, 2437.999999, "13cm Sat. High-Rate Data", true,
                                2438.000000, 2450.000000, "13cm Wideband FM/FSTV/FMTV", true,

                                3456.000000, 3456.099999, "9cm General", true,
                                3456.100000, 3456.100000, "9cm Calling Frequency", true,
                                3456.100001, 3456.299999, "9cm General", true,
                                3456.300000, 3456.400000, "9cm Propagation Beacons", true,

                                5760.000000, 5760.099999, "5cm General", true,
                                5760.100000, 5760.100000, "5cm Calling Frequency", true,
                                5760.100001, 5760.299999, "5cm General", true,
                                5760.300000, 5760.400000, "5cm Propagation Beacons", true,

                                10368.000000, 10368.099999, "3cm General", true,
                                10368.100000, 10368.100000, "3cm Calling Frequency", true,
                                10368.100001, 10368.400000, "3cm General", true,

                                24192.000000, 24192.099999, "1.2cm General", true,
                                24192.100000, 24192.100000, "1.2cm Calling Frequency", true,
                                24192.100001, 24192.400000, "1.2cm General", true,

                                47088.000000, 47088.099999, "47GHz General", true,
                                47088.100000, 47088.100000, "47GHz Calling Frequency", true,
                                47088.100001, 47088.400000, "47GHz General", true,
            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }


        } // AddBand2TextTable() Region2

        #endregion

        private static void AddBandAusTextTable()  // ke9ns add Australia
        {
            if (bandtextrefresh == false) ds.Tables.Add("BandText");

            DataTable t = ds.Tables["BandText"];

            if (bandtextrefresh == false)
            {
                t.Columns.Add("Low", typeof(double));
                t.Columns.Add("High", typeof(double));
                t.Columns.Add("Name", typeof(string));
                t.Columns.Add("TX", typeof(bool));
            }


            object[] data = {
                                1.800000, 1.809999, "160M CW/Digital Modes",    true,
                                1.810000, 1.810000, "160M CW QRP",              true,
                                1.810001, 1.837999, "160M CW",                  true,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.909999, "160M SSB/SSTV/Wide Band",  true,

                                1.910000, 1.910000, "160M SSB QRP",             true,
                                1.910001, 1.994999, "160M SSB/SSTV/Wide Band",  true,
                                1.995000, 1.999999, "160M Experimental",        true,

                                3.500000, 3.524999, "80M Extra CW",             true,
                                3.525000, 3.567999, "80M CW",                   true,

                                3.568000, 3.568000, "80M FT4/JT65 DIGU",             true, // ke9ns add  3.573
                                3.568001, 3.572999, "80M FT4/JT65 DIGU",             true, // ke9ns add

                                3.573000, 3.573000, "80M FT8 DIGU",             true, // ke9ns add  3.573
                                3.573001, 3.574999, "80M FT8 DIGU",             true, // ke9ns add

                                3.575000, 3.575000, "80M FT4 DIGU",             true, // ke9ns add  3.573
                                3.575001, 3.578000, "80M FT4 DIGU",             true, // ke9ns add
                     
                                3.578001, 3.579999, "80M PSK",                  true,

                                3.580000, 3.589999, "80M RTTY",                 true,
                                3.590000, 3.590000, "80M RTTY DX",              true,
                                3.590001, 3.599999, "80M RTTY",                 true,

                                3.600000, 3.629999, "80M Extra SSB",            true,
                                3.630000, 3.630000, "80M Extra eSSB",            true, // ke9ns
                                3.630001, 3.699999, "80M Extra SSB",            true,


                                3.700000, 3.789999, "80M Ext/Adv SSB",          true,
                                3.790000, 3.799999, "80M Ext/Adv DX Window",    true,
                                3.800000, 3.844999, "75M SSB",                  true,
                                3.845000, 3.845000, "75M SSTV",                 true,
                                3.845001, 3.884999, "75M SSB",                  true,
                                3.885000, 3.885000, "75M AM Calling Frequency", true,
                                3.885001, 4.000000, "75M SSB",                  true,


                                7.000000, 7.024999, "40M Extra CW",             true, // ke9ns mod
								7.025000, 7.039999, "40M CW",                   true,

                                7.040000, 7.040000, "40M RTTY DX",              true,
                                7.040001, 7.046999, "40M RTTY",                 true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.069999, "40M RTTY",                 true,


                                7.070000, 7.070000, "40m PSK",                  true,            // ke9ns add
                                7.070001, 7.073999, "40m PSK",                  true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add

	                            7.079000, 7.099999, "40M RTTY",                 true,
                                7.100000, 7.124999, "40M CW",                   true,
                                7.125000, 7.170999, "40M Ext/Adv SSB",          true,
                                7.171000, 7.171000, "40M SSTV",                 true,
                                7.171001, 7.174999, "40M Ext/Adv SSB",          true,
                                7.175000, 7.289999, "40M SSB",                  true,
                                7.290000, 7.290000, "40M AM Calling Frequency", true,
                                7.290001, 7.299999, "40M SSB",                  true,

                                10.100000, 10.129999, "30M CW",                 true,
                                10.130000, 10.135999, "30M RTTY",               true,

                                10.136000, 10.136000, "30M FT8 DIGU",           true, // ke9ns add
                                10.136001, 10.137999, "30M FT8 DIGU",       true, // ke9ns add

                                10.138000, 10.138000, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.138001, 10.138999, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.139000, 10.139999, "30M RTTY",               true,

                                10.140000, 10.140000, "30M FT4 DIGU",           true, // ke9ns add
                                10.140001, 10.142999, "30M FT4 DIGU",       true, // ke9ns add

                                10.143000, 10.149999, "30M Narrow Band Modes", true,

                                14.000000, 14.024999, "20M Extra CW",           true,
                                14.025000, 14.069999, "20M CW",                 true,
                                14.070000, 14.073999, "20M RTTY",               true,

                                14.074000, 14.074000, "20M FT8 DIGU",           true, // ke9ns add
                                14.074001, 14.075999, "20M FT8 DIGU",           true, // ke9ns add

                                14.076000, 14.076000, "20M JT65 DIGU",          true, // ke9ns add
                                14.076001, 14.078999, "20M JT65 DIGU",          true, // ke9ns add

                                14.079000, 14.079999, "20M RTTY",               true,

                                14.080000, 14.080000, "20M FT4 DIGU",      true, // ke9ns add
                                14.080001, 14.084999, "20M FT4 DIGU",           true, // ke9ns add

                                14.085000, 14.094999, "20M RTTY",               true,

                                14.095000, 14.099499, "20M Packet",             true,
                                14.099500, 14.099999, "20M CW",                 true,
                                14.100000, 14.100000, "20M NCDXF Beacons",      true,
                                14.100001, 14.100499, "20M CW",                 true,
                                14.100500, 14.111999, "20M Packet",             true,
                                14.112000, 14.149999, "20M CW",                 true,
                                14.150000, 14.174999, "20M Extra SSB",          true,
                                14.175000, 14.224999, "20M Ext/Adv SSB",        true,
                                14.225000, 14.229999, "20M SSB",                true,


                                14.230000, 14.230000, "20M SSTV",          true,
                                14.230001, 14.232999, "20M SSTV",               true,

                                14.233000, 14.233000, "20M EasyPal",        true,
                                14.233001, 14.235999, "20M EasyPal",        true,

                                14.236000, 14.282999, "20M SSB",                true,
                                14.283000, 14.285999, "20M AM ", true,
                                14.286000, 14.286000, "20M AM Calling Freq", true,
                                14.286001, 14.288999, "20M AM ", true,

                                14.289000, 14.339999, "20M SSB",                true,
                                14.340000, 14.340001, "20M DV (Digital Voice)", true,                // ke9ns add   
                                14.340002, 14.349999, "20M SSB",          true,


                                18.068000, 18.099999, "17M CW",                 true,
                                18.100000, 18.100000, "17M FT8 DIGU",           true, // ke9ns add
                                18.100001, 18.101999, "17M FT8 DIGU",           true, // ke9ns add

                                18.102000, 18.102000, "17M JT65 DIGU",          true, // ke9ns add
                                18.102001, 18.103999, "17M JT65 DIGU",          true, // ke9ns add

                                18.104000, 18.104000, "17M FT4 DIGU",           true, // ke9ns add
                                18.104001, 18.106999, "17M FT4 DIGU",           true, // ke9ns add

                                18.107000, 18.107999, "17M RTTY",               true,

                                18.108000, 18.108999, "17M PSK",                true,
                                18.109000, 18.109999, "17M Packet",             true,
                                18.110000, 18.110000, "17M NCDXF Beacons",      true,

                                18.110001, 18.147999, "17M SSB",                true,
                                18.148000, 18.148001, "17M DV (Digital Voice)", true,                // ke9ns add   
								18.148002, 18.167999, "17M SSB",                true,



                                21.000000, 21.024999, "15M Extra CW",           true,
                                21.025000, 21.069999, "15M CW",                 true,
                                21.070000, 21.073999, "15M RTTY",               true,

                                21.074000, 21.074000, "15M FT8 DIGU",           true, // ke9ns add 
                                21.074001, 21.075999, "15M FT8 DIGU",           true, // ke9ns add

                                21.076000, 21.076000, "15M JT65 DIGU",          true, // ke9ns add
                                21.076001, 21.078999, "15M JT65 DIGU",          true, // ke9ns add
                                21.079000, 21.099999, "15M RTTY",               true,

                                21.100000, 21.109999, "15M Packet",             true,
                                21.110000, 21.139999, "15M CW",                   true,

                                21.140000, 21.140000, "15M FT4 DIGU",           true, // ke9ns add 
                                21.140001, 21.144999, "15M FT4 DIGU",           true, // ke9ns add

                                21.145000, 21.149999, "15M CW",                   true,
                                21.150000, 21.150000, "15M NCDXF Beacons",      true,
                                21.150001, 21.199999, "15M CW",                 true,
                                21.200000, 21.224999, "15M Extra SSB",          true,
                                21.225000, 21.274999, "15M Ext/Adv SSB",        true,
                                21.275000, 21.339999, "15M SSB",                true,
                                21.340000, 21.340000, "15M SSTV",               true,

                                21.340001, 21.379999, "15M SSB",                true,
                                21.380000, 21.380001, "15M DV (Digital Voice)", true,                // ke9ns add    
                                21.380002, 21.450000, "15M SSB",                true,



                                24.890000, 24.914999, "12M CW",                 true,

                                24.915000, 24.915000, "12M FT8 DIGU",           true, // ke9ns add 
                                24.915001, 24.916999, "12M FT8 DIGU",           true, // ke9ns add

                                24.917000, 24.917000, "12M JT65 DIGU",          true, // ke9ns add
                                24.917001, 24.918999, "12M JT65 DIGU",          true, // ke9ns add

                                24.919000, 24.919000, "12M FT4 DIGU",           true, // ke9ns add 
                                24.919001, 24.921999, "12M FT4 DIGU",           true, // ke9ns add

                                24.922000, 24.924999, "12M RTTY",               true,

                                24.925000, 24.929999, "12M Packet",             true,
                                24.930000, 24.930000, "12M NCDXF Beacons",      true,

                                24.930001, 24.987999, "12M SSB",                true,
                                24.938000, 24.938001, "12M DV (Digital Voice)", true,                // ke9ns add 
                                24.938002, 24.989999, "12M SSB",                true,





                                28.000000, 28.069999, "10M CW",                 true,
                                28.070000, 28.073999, "10M RTTY",               true,

                                28.074000, 28.074000, "10M FT8 DIGU",           true, // ke9ns add 
                                28.074001, 28.075999, "10M FT8 DIGU",           true, // ke9ns add
                                28.076000, 28.076000, "10M JT65 DIGU",          true, // ke9ns add 
                                28.076001, 28.078999, "10M JT65 DIGU",          true, // ke9ns add

                                28.079000, 28.149999, "10M RTTY",               true,


                                28.150000, 28.179999, "10M CW",                  true,

                                28.180000, 28.180000, "10M FT4 DIGU",           true, // ke9ns add 
                                28.180001, 28.184999, "10M FT4 DIGU",           true, // ke9ns add

                                28.185000, 28.199999, "10M CW",                 true,

                                28.200000, 28.200000, "10M NCDXF Beacons",      true,
                                28.200001, 28.299999, "10M Beacons",            true,

                                28.300000, 28.399999, "10M SSB",                true,
                                28.400000, 28.400001, "10M DV (Digital Voice)", true,                // ke9ns add
                                28.400002, 28.679999, "10M SSB",                true,

                                28.680000, 28.680000, "10M SSTV",               true,
                                28.680001, 28.999999, "10M SSB",                true,
                                29.000000, 29.199999, "10M AM",                 true,
                                29.200000, 29.299999, "10M SSB",                true,
                                29.300000, 29.509999, "10M Satellite Downlinks", true,
                                29.510000, 29.519999, "10M Deadband",           true,
                                29.520000, 29.589999, "10M Repeater Inputs",    true,
                                29.590000, 29.599999, "10M Deadband",           true,
                                29.600000, 29.600000, "10M FM Simplex",         true,
                                29.600001, 29.609999, "10M Deadband",           true,
                                29.610000, 29.699999, "10M Repeater Outputs",   true,

                                50.000000, 50.059999, "6M CW",                  true,
                                50.060000, 50.079999, "6M Beacon Sub-Band",     true,
                                50.080000, 50.099999, "6M CW",                  true,
                                50.100000, 50.124999, "6M DX Window",           true,
                                50.125000, 50.125000, "6M Calling Frequency",   true,


                                50.125001, 50.209999, "6M SSB",                 true,
                                50.210000, 50.210001, "6M DV (Digital Voice)",  true,                // ke9ns add
                                50.210002, 50.275999, "6M SSB",                 true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.599999, "6M All Modes", true,

                                50.600000, 50.619999, "6M Non Voice",           true,
                                50.620000, 50.620000, "6M Digital Packet",      true,
                                50.620001, 50.799999, "6M Non Voice",           true,
                                50.800000, 50.999999, "6M RC",                  true,
                                51.000000, 51.099999, "6M Pacific DX Window",   true,
                                51.100000, 51.119999, "6M Deadband",            true,
                                51.120000, 51.179999, "6M Digital Repeater Inputs", true,
                                51.180000, 51.479999, "6M Repeater Inputs",     true,
                                51.480000, 51.619999, "6M Deadband",            true,
                                51.620000, 51.679999, "6M Digital Repeater Outputs", true,
                                51.680000, 51.979999, "6M Repeater Outputs",    true,
                                51.980000, 51.999999, "6M Deadband",            true,
                                52.000000, 52.019999, "6M Repeater Inputs",     true,
                                52.020000, 52.020000, "6M FM Simplex",          true,
                                52.020001, 52.039999, "6M Repeater Inputs",     true,
                                52.040000, 52.040000, "6M FM Simplex",          true,
                                52.040001, 52.479999, "6M Repeater Inputs",     true,
                                52.480000, 52.499999, "6M Deadband",            true,
                                52.500000, 52.524999, "6M Repeater Outputs",    true,
                                52.525000, 52.525000, "6M Primary FM Simplex",  true,
                                52.525001, 52.539999, "6M Deadband",            true,
                                52.540000, 52.540000, "6M Secondary FM Simplex", true,
                                52.540001, 52.979999, "6M Repeater Outputs",    true,
                                52.980000, 52.999999, "6M Deadbands",           true,
                                53.000000, 53.000000, "6M Remote Base FM Spx",  true,
                                53.000001, 53.019999, "6M Repeater Inputs",     true,
                                53.020000, 53.020000, "6M FM Simplex",          true,
                                53.020001, 53.479999, "6M Repeater Inputs",     true,
                                53.480000, 53.499999, "6M Deadband",            true,
                                53.500000, 53.519999, "6M Repeater Outputs",    true,
                                53.520000, 53.520000, "6M FM Simplex",          true,
                                53.520001, 53.899999, "6M Repeater Outputs",    true,
                                53.900000, 53.900000, "6M FM Simplex",          true,
                                53.900010, 53.979999, "6M Repeater Outputs",    true,
                                53.980000, 53.999999, "6M Deadband",            true,

                                144.000000, 144.099999, "2M CW",                true,

                                144.100000, 144.169999, "2M CW/SSB",            true,
                                144.170000, 144.173999, "2M FT4",               true, // ke9ns add
                                144.174000, 144.176999, "2M FT8",               true,
                                144.177000, 144.199999, "2M CW/SSB",            true,

                                144.200000, 144.200000, "2M Calling",           true,
                                144.200001, 144.274999, "2M CW/SSB",            true,
                                144.275000, 144.299999, "2M Beacon Sub-Band",   true,
                                144.300000, 144.499999, "2M Satellite",         true,
                                144.500000, 144.599999, "2M Linear Translator Inputs", true,
                                144.600000, 144.899999, "2M FM Repeater",       true,
                                144.900000, 145.199999, "2M FM Simplex",        true,
                                145.200000, 145.499999, "2M FM Repeater",       true,
                                145.500000, 145.799999, "2M FM Simplex",        true,
                                145.800000, 145.999999, "2M Satellite",         true,
                                146.000000, 146.399999, "2M FM Repeater",       true,
                                146.400000, 146.609999, "2M FM Simplex",        true,
                                146.610000, 147.389999, "2M FM Repeater",       true,
                                147.390000, 147.599999, "2M FM Simplex",        true,
                                147.600000, 147.999999, "2M FM Repeater",       true,

                                222.000000, 222.024999, "1.25M EME/Weak Signal",        true,
                                222.025000, 222.049999, "1.25M Weak Signal",            true,
                                222.050000, 222.059999, "1.25M Propagation Beacons",    true,
                                222.060000, 222.099999, "1.25M Weak Signal",            true,
                                222.100000, 222.100000, "1.25M SSB/CW Calling",         true,
                                222.100001, 222.149999, "1.25M Weak Signal CW/SSB",     true,
                                222.150000, 222.249999, "1.25M Local Option",           true,
                                222.250000, 223.380000, "1.25M FM Repeater Inputs",     true,
                                223.380001, 223.399999, "1.25M General",                true,
                                223.400000, 223.519999, "1.25M FM Simplex",             true,
                                223.520000, 223.639999, "1.25M Digital/Packet",         true,
                                223.640000, 223.700000, "1.25M Links/Control",          true,
                                223.700001, 223.709999, "1.25M General",                true,
                                223.710000, 223.849999, "1.25M Local Option",           true,
                                223.850000, 224.980000, "1.25M Repeater Outputs",       true,

                                420.000000, 425.999999, "70cm ATV Repeater",    true,
                                426.000000, 431.999999, "70cm ATV Simplex",     true,

                                432.000000, 432.064999, "70cm EME",                    true,
                                432.065000, 432.067999, "70cm FT8",                     true, // ke9ns add
                                432.068000, 432.069999, "70cm EME",                      true,

                                432.070000, 432.099999, "70cm Weak Signal CW",  true,
                                432.100000, 432.100000, "70cm Calling Frequency", true,
                                432.100001, 432.299999, "70cm Mixed Mode Weak Signal", true,
                                432.300000, 432.399999, "70cm Propagation Beacons", true,
                                432.400000, 432.999999, "70cm Mixed Mode Weak Signal", true,
                                433.000000, 434.999999, "70cm Auxillary/Repeater Links", true,
                                435.000000, 437.999999, "70cm Satellite Only",  true,
                                438.000000, 441.999999, "70cm ATV Repeater",    true,
                                442.000000, 444.999999, "70cm Local Repeaters", true,
                                445.000000, 445.999999, "70cm Local Option",    true,
                                446.000000, 446.000000, "70cm Simplex",         true,
                                446.000001, 446.999999, "70cm Local Option",    true,
                                447.000000, 450.000000, "70cm Local Repeaters", true,

                                902.000000, 902.099999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
                                902.100000, 902.100000, "33cm Weak Signal Calling", true,
                                902.100001, 902.799999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
                                902.800000, 902.999999, "33cm Weak Signal EME/CW", true,
                                903.000000, 903.099999, "33cm Digital Modes",   true,
                                903.100000, 903.100000, "33cm Alternate Calling", true,
                                903.100001, 905.999999, "33cm Digital Modes",   true,
                                906.000000, 908.999999, "33cm FM Repeater Inputs", true,
                                909.000000, 914.999999, "33cm ATV",             true,
                                915.000000, 917.999999, "33cm Digital Modes",   true,
                                918.000000, 920.999999, "33cm FM Repeater Outputs", true,
                                921.000000, 926.999999, "33cm ATV",             true,
                                927.000000, 928.000000, "33cm FM Simplex/Links", true,

                                1240.000000, 1245.999999, "23cm ATV #1",        true,
                                1246.000000, 1251.999999, "23cm FM Point/Links", true,
                                1252.000000, 1257.999999, "23cm ATV #2, Digital Modes", true,
                                1258.000000, 1259.999999, "23cm FM Point/Links", true,
                                1260.000000, 1269.999999, "23cm Sat Uplinks/Wideband Exp.", true,
                                1270.000000, 1275.999999, "23cm Repeater Inputs", true,
                                1276.000000, 1281.999999, "23cm ATV #3",        true,
                                1282.000000, 1287.999999, "23cm Repeater Outputs",  true,
                                1288.000000, 1293.999999, "23cm Simplex ATV/Wideband Exp.", true,
                                1294.000000, 1294.499999, "23cm Simplex FM",        true,
                                1294.500000, 1294.500000, "23cm FM Simplex Calling", true,
                                1294.500001, 1294.999999, "23cm Simplex FM",        true,
                                1295.000000, 1295.799999, "23cm SSTV/FAX/ACSSB/Exp.", true,
                                1295.800000, 1295.999999, "23cm EME/CW Expansion",  true,
                                1296.000000, 1296.049999, "23cm EME Exclusive",     true,
                                1296.050000, 1296.069999, "23cm Weak Signal",       true,
                                1296.070000, 1296.079999, "23cm CW Beacons",        true,
                                1296.080000, 1296.099999, "23cm Weak Signal",       true,
                                1296.100000, 1296.100000, "23cm CW/SSB Calling",    true,
                                1296.100001, 1296.399999, "23cm Weak Signal",       true,
                                1296.400000, 1296.599999, "23cm X-Band Translator Input", true,
                                1296.600000, 1296.799999, "23cm X-Band Translator Output", true,
                                1296.800000, 1296.999999, "23cm Experimental Beacons", true,
                                1297.000000, 1300.000000, "23cm Digital Modes",     true,

                                2300.000000, 2302.999999, "13cm High Data Rate", true,
                                2303.000000, 2303.499999, "13cm Packet",      true,
                                2303.500000, 2303.800000, "13cm TTY Packet",  true,
                                2303.800001, 2303.899999, "13cm General", true,
                                2303.900000, 2303.900000, "13cm Packet/TTY/CW/EME", true,
                                2303.900001, 2304.099999, "13cm CW/EME",      true,
                                2304.100000, 2304.100000, "13cm Calling Frequency", true,
                                2304.100001, 2304.199999, "13cm CW/EME/SSB",  true,
                                2304.200000, 2304.299999, "13cm SSB/SSTV/FAX/Packet AM/Amtor", true,
                                2304.300000, 2304.319999, "13cm Propagation Beacon Network", true,
                                2304.320000, 2304.399999, "13cm General Propagation Beacons", true,
                                2304.400000, 2304.499999, "13cm SSB/SSTV/ACSSB/FAX/Packet AM", true,
                                2304.500000, 2304.699999, "13cm X-Band Translator Input", true,
                                2304.700000, 2304.899999, "13cm X-Band Translator Output", true,
                                2304.900000, 2304.999999, "13cm Experimental Beacons", true,
                                2305.000000, 2305.199999, "13cm FM Simplex", true,
                                2305.200000, 2305.200000, "13cm FM Simplex Calling", true,
                                2305.200001, 2305.999999, "13cm FM Simplex", true,
                                2306.000000, 2308.999999, "13cm FM Repeaters", true,
                                2309.000000, 2310.000000, "13cm Control/Aux Links", true,

                                2390.000000, 2395.999999, "13cm Fast-Scan TV", true,
                                2396.000000, 2398.999999, "13cm High Rate Data", true,
                                2399.000000, 2399.499999, "13cm Packet", true,
                                2399.500000, 2399.999999, "13cm Control/Aux Links", true,
                                2400.000000, 2402.999999, "13cm Satellite", true,
                                2403.000000, 2407.999999, "13cm Satellite High-Rate Data", true,
                                2408.000000, 2409.999999, "13cm Satellite", true,
                                2410.000000, 2412.999999, "13cm FM Repeaters", true,
                                2413.000000, 2417.999999, "13cm High-Rate Data", true,
                                2418.000000, 2429.999999, "13cm Fast-Scan TV", true,
                                2430.000000, 2432.999999, "13cm Satellite", true,
                                2433.000000, 2437.999999, "13cm Sat. High-Rate Data", true,
                                2438.000000, 2450.000000, "13cm Wideband FM/FSTV/FMTV", true,

                                3456.000000, 3456.099999, "9cm General", true,
                                3456.100000, 3456.100000, "9cm Calling Frequency", true,
                                3456.100001, 3456.299999, "9cm General", true,
                                3456.300000, 3456.400000, "9cm Propagation Beacons", true,

                                5760.000000, 5760.099999, "5cm General", true,
                                5760.100000, 5760.100000, "5cm Calling Frequency", true,
                                5760.100001, 5760.299999, "5cm General", true,
                                5760.300000, 5760.400000, "5cm Propagation Beacons", true,

                                10368.000000, 10368.099999, "3cm General", true,
                                10368.100000, 10368.100000, "3cm Calling Frequency", true,
                                10368.100001, 10368.400000, "3cm General", true,

                                24192.000000, 24192.099999, "1.2cm General", true,
                                24192.100000, 24192.100000, "1.2cm Calling Frequency", true,
                                24192.100001, 24192.400000, "1.2cm General", true,

                                47088.000000, 47088.099999, "47GHz General", true,
                                47088.100000, 47088.100000, "47GHz Calling Frequency", true,
                                47088.100001, 47088.400000, "47GHz General", true,
            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }


        } // AddBandAusTextTable() 

        #region IARU3 Region 3 BandText

        private static void AddRegion3BandText160m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                1.800000, 1.829999, "160M CW",                  true,
                                1.830000, 1.833999, "160M CW & NB Digital",     true,
                                1.834000, 1.837999, "160M CW",                  true,

                                1.838000, 1.838000, "160M PSK/JT65 DIGU",       true, // ke9ns add
                                1.838001, 1.839999, "160M PSK/JT65 DIGU",       true, // ke9ns add

                                1.840000, 1.840000, "160M FT8 DIGU",            true, // ke9ns add  1.84
                                1.840001, 1.842500, "160M FT8 DIGU",            true, // ke9ns add
                                1.842501, 1.842999, "160M FT8 DIGU",            true,

                                1.843000, 1.999999, "160M CW & Phone",          true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText80m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                3.500000, 3.509999, "80M CW DX",                true,
                                3.510000, 3.534999, "80M CW",                   true,
                                3.535000, 3.567999, "80M Phone & CW",           true,

                                3.568000, 3.568000, "80M FT4/JT65 DIGU",             true, // ke9ns add  3.573
                                3.568001, 3.572999, "80M FT4/JT65 DIGU",             true, // ke9ns add

                                3.573000, 3.573000, "80M FT8 DIGU",             true, // ke9ns add  3.573
                                3.573001, 3.574999, "80M FT8 DIGU",             true, // ke9ns add

                                3.575000, 3.575000, "80M FT4 DIGU",             true, // ke9ns add  3.573
                                3.575001, 3.578000, "80M FT4 DIGU",             true, // ke9ns add
                        
                                3.578001, 3.599999, "80M Phone & CW",          true,

                                3.600000, 3.600000, "80M IARU Emergency",       true,

                                3.600001, 3.629999, "80M Phone & CW",            true,
                                3.630000, 3.630000, "80M eSSB",                  true, //ke9ns add
                                3.630001, 3.699999, "80M Phone & CW",            true,

                                3.700000, 3.774999, "80M Phone & CW",           true,
                                3.775000, 3.799999, "80M DX Phone & CW",        true,
                                3.800000, 3.899999, "80M Phone & CW",           true,

                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText60m()
        {
            DataTable t = ds.Tables["BandText"];

            object[] data = {


                                5.250000, 5.351499, "60M RX Only",                     false,

                                5.351500, 5.335999, "60M 200hz RX Narrow Band Modes",  false,
                                5.354000, 5.356999, "60M RX USB (UK CH 7)",            false,
                                5.357000, 5.359999, "60M RX USB (US CH 3)",            false,
                                5.360000, 5.362999, "60M RX USB",                      false,
                                5.363000, 5.365999, "60M RX USB (UK CH 8)",            false,
                                5.366000, 5.366500, "60M 20hz RX Narrow Band Modes",   false,

                                5.366501, 5.450000, "60M RX Only",                    false,



                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddChinaBandText60m()
        {
            DataTable t = ds.Tables["BandText"];

            object[] data = {


                                5.250000, 5.351499, "60M RX Only",                     false,

                                5.351500, 5.335999, "60M 200hz RX Narrow Band Modes",  true,
                                5.354000, 5.356999, "60M RX USB (UK CH 7)",            true,
                                5.357000, 5.359999, "60M RX USB (US CH 3)",            true,
                                5.360000, 5.362999, "60M RX USB",                      true,
                                5.363000, 5.365999, "60M RX USB (UK CH 8)",            true,
                                5.366000, 5.366500, "60M 20hz RX Narrow Band Modes",   true,

                                5.366501, 5.450000, "60M RX Only",                    false,

                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }


        private static void AddRegion3BandText40m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                7.000000, 7.024999, "40M CW",                   true,
                                7.025000, 7.029999, "40M CW & NB Digital",      true,
                                7.030000, 7.039999, "40M All Modes",            true,

                                7.035000, 7.035000, "40m PSK",                  true,            // ke9ns add
                                7.035001, 7.039999, "40m PSK",                  true,

                                7.040000, 7.042999, "40M Phone & CW",           true,

                                7.043000, 7.046999, "40M RTTY",                 true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.073999, "40M RTTY",                 true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add

                                7.079000, 7.109999, "40M Phone & CW",           true,

                                7.110000, 7.110000, "40M IARU Emergency",       true,
                                7.110001, 7.299999, "40M Phone & CW",           true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText30m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                10.100000, 10.135999, "30M CW",                 true,

                                10.136000, 10.136000, "30M FT8 DIGU",           true, // ke9ns add
                                10.136001, 10.137999, "30M FT8 DIGU",       true, // ke9ns add

                                10.138000, 10.138000, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.138001, 10.138999, "30M PSK/JT65 DIGU",          true, // ke9ns add
                                10.139000, 10.139999, "30M RTTY",               true,

                                10.140000, 10.140000, "30M FT4 DIGU",           true, // ke9ns add
                                10.140001, 10.142999, "30M FT4 DIGU",       true, // ke9ns add
                               
                                10.143000, 10.149999, "30M CW & NB Digital", true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText20m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                14.000000, 14.069999, "20M CW",                 true,
                                14.070000, 14.073999, "20M CW & NB Digital",    true,

                                14.074000, 14.074000, "20M FT8 DIGU",           true, // ke9ns add
                                14.074001, 14.075999, "20M FT8 DIGU",           true, // ke9ns add

                                14.076000, 14.076000, "20M JT65 DIGU",          true, // ke9ns add
                                14.076001, 14.078999, "20M JT65 DIGU",          true, // ke9ns add

                                14.079000, 14.079999, "20M RTTY",               true,

                                14.080000, 14.080000, "20M FT4 DIGU",      true, // ke9ns add
                                14.080001, 14.084999, "20M FT4 DIGU",           true, // ke9ns add

                                14.085000, 14.094999, "20M RTTY",               true,

                                14.095000, 14.099499, "20M Data & Packet",      true,
                                14.099500, 14.099999, "20M Beacons",            true,
                                14.100000, 14.100000, "20M NCDXF Beacons",      true,
                                14.100001, 14.100499, "20M Beacons",            true,
                                14.100500, 14.111999, "20M Data & Packet",      true,
                                14.112000, 14.229999, "20M Phone & CW",         true,

                                14.230000, 14.230000, "20M SSTV",          true,
                                14.230001, 14.232999, "20M SSTV",               true,

                                14.233000, 14.233000, "20M EasyPal",        true,
                                14.233001, 14.235999, "20M EasyPal",        true,

                                14.236000, 14.282999, "20M SSB",                true,
                                14.283000, 14.285999, "20M AM ", true,
                                14.286000, 14.286000, "20M AM Calling Freq", true,
                                14.286001, 14.288999, "20M AM ", true,

                                14.289000, 14.299999, "20M Phone & CW",         true,
                                14.300000, 14.300000, "20M IARU Emergency",     true,

                                14.300001, 14.339999, "20M All Modes",          true,
                                14.340000, 14.340001, "20M DV (Digital Voice)", true,                // ke9ns add   
                                14.340002, 14.349999, "20M All Modes",          true,


                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText17m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                18.068000, 18.099999, "17M CW",                 true,
                                18.100000, 18.100000, "17M FT8 DIGU",           true, // ke9ns add
                                18.100001, 18.101999, "17M FT8 DIGU",           true, // ke9ns add

                                18.102000, 18.102000, "17M JT65 DIGU",          true, // ke9ns add
                                18.102001, 18.103999, "17M JT65 DIGU",          true, // ke9ns add

                                18.104000, 18.104000, "17M FT4 DIGU",           true, // ke9ns add
                                18.104001, 18.106999, "17M FT4 DIGU",           true, // ke9ns add

                                18.107000, 18.107999, "17M RTTY",               true,

                                18.108000, 18.109499, "17M PSK",                true,
                                18.109500, 18.109999, "17M Beacons",            true,
                                18.110000, 18.110000, "17M NCDXF Beacons",      true,
                                18.110001, 18.110499, "17M Beacons",            true,
                                18.110500, 18.159999, "17M Phone & CW",         true,
                                18.160000, 18.160000, "17M IARU Emergency",     true,

                                18.160001, 18.147999, "17M All Modes",                true,
                                18.148000, 18.148001, "17M DV (Digital Voice)", true,                // ke9ns add   
								18.148002, 18.167999, "17M All Modes",                true,

            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText15m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                21.000000, 21.069999, "15M CW",                 true,
                                21.070000, 21.073999, "15M CW & NB Digital",    true,

                                21.074000, 21.074000, "15M FT8 DIGU",           true, // ke9ns add 
                                21.074001, 21.075999, "15M FT8 DIGU",           true, // ke9ns add

                                21.076000, 21.076000, "15M JT65 DIGU",          true, // ke9ns add
                                21.076001, 21.078999, "15M JT65 DIGU",          true, // ke9ns add
                                21.079000, 21.099999, "15M RTTY",               true,

                                21.100000, 21.109999, "15M PSK / Packet",                true,
                                21.110000, 21.124999, "15M CW",                 true,
                                21.125000, 21.139999, "15M Phone & CW",         true,

                                21.140000, 21.140000, "15M FT4 DIGU",           true, // ke9ns add 
                                21.140001, 21.144999, "15M FT4 DIGU",           true, // ke9ns add

                                21.145000, 21.149499, "15M Phone & CW",           true,

                                21.149500, 21.149999, "15M Beacons",         true,
                                21.150000, 21.150000, "15M NCDXF Beacons",      true,
                                21.150001, 21.150499, "15M Beacons",            true,
                                21.150500, 21.339999, "15M Phone & CW",         true,
                                21.340000, 21.340000, "15M SSTV",               true,
                                21.340001, 21.359999, "15M Phone & CW",         true,
                                21.360000, 21.360000, "15M Emergency",          true,
                                21.360001, 21.450000, "15M All Modes",          true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText12m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                24.890000, 24.919999, "12M CW",                 true,
                                24.920000, 24.914999, "12M CW & NB Digital",    true,

                                24.915000, 24.915000, "12M FT8 DIGU",           true, // ke9ns add
                                24.915001, 24.916999, "12M FT8 DIGU",           true, // ke9ns add

                                24.917000, 24.917000, "12M JT65 DIGU",          true, // ke9ns add
                                24.917001, 24.918999, "12M JT65 DIGU",          true, // ke9ns add

                                24.919000, 24.919000, "12M FT4 DIGU",           true, // ke9ns add 
                                24.919001, 24.921999, "12M FT4 DIGU",           true, // ke9ns add

                                24.922000, 24.929499, "12M CW & NB Digital",   true,

                                24.929500, 24.929999, "12M Beacons",         true,
                                24.930000, 24.930000, "12M NCDXF Beacons",      true,
                                24.930001, 24.930499, "12M Beacons",            true,

                                24.930500, 24.987999, "12M All Modes",                true,
                                24.938000, 24.938001, "12M DV (Digital Voice)", true,                // ke9ns add 
                                24.938002, 24.989999, "12M All Modes",                true,


            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText10m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                28.000000, 28.049999, "10M CW",                 true,
                                28.050000, 28.073999, "10M CW & NB Digital",    true,

                                28.074000, 28.074000, "10M FT8 DIGU",           true, // ke9ns add
                                28.074001, 28.075999, "10M FT8 DIGU",           true, // ke9ns add
                                28.076000, 28.076000, "10M JT65 DIGU",          true, // ke9ns add 
                                28.076001, 28.078999, "10M JT65 DIGU",          true, // ke9ns add

                                28.079000, 28.149999, "10M CW & NB Digital",   true,

                                28.150000, 28.179999, "10M CW",                    true,

                                28.180000, 28.180000, "10M FT4 DIGU",           true, // ke9ns add 
                                28.180001, 28.184999, "10M FT4 DIGU",           true, // ke9ns add

                                28.185000, 28.189999, "10M CW",                 true,

                                28.190000, 28.199999, "10M Beacons",         true,
                                28.200000, 28.200000, "10M NCDXF Beacons",      true,
                                28.200001, 28.200499, "10M Beacons",            true,
                                28.200500, 28.679999, "10M Phone & CW",         true,
                                28.680000, 28.680000, "10M SSTV",               true,
                                28.680001, 29.299999, "10M Phone & CW",         true,
                                29.300000, 29.509999, "10M Satellite & CW",     true,
                                29.510000, 29.699999, "10M Wide Band  & CW",    true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandText6m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 50.099999, "6M CW & Beacons",        true,
                                50.100000, 50.124999, "6M Phone/NB Digital/CW", true,
                                50.125000, 50.125000, "6M Calling Frequency",   true, // calling freq
                                50.125001, 50.275999, "6M Phone/NB Digital/CW", true,
                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add
                              
                                50.326000, 53.999999, "6M Wide Band Modes & CW", true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddRegion3BandTextVHFplus()
        {
            // IARU Region 3: 2M and above Band Plan
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								// 144 - 146 MHz
                                144.000000, 144.019999, "2M EME",                     true,
                                144.020000, 144.099999, "2M CW & EME",                  true,

                                144.100000, 144.169999, "2M CW/Phone & Image",          true,
                                144.170000, 144.173999, "2M FT4",                       true, // ke9ns add
                                144.174000, 144.176999, "2M FT8",                       true,
                                144.177000, 144.399999, "2M CW/Phone & Image",          true,

                                144.400000, 144.499999, "2M CW/Phone/NB Digital",       true,
                                144.500000, 144.699999, "2M Wide Digital Modes",        true,
                                144.700000, 145.499999, "2M FM, CW & Image",            true,
                                145.500000, 145.500000, "2M Emergency",                 true,
                                145.500001, 145.649999, "2M FM, CW & Image",            true,
                                145.650000, 145.799999, "2M All Modes",                 true,
                                145.800000, 145.999999, "2M Satellite.",                true,
                                146.000000, 147.999999, "2M All Modes",                 true,
                                // 430 - 440 MHz
								430.000000, 430.099999, "70cm CW",                        true,
                                430.100000, 430.699999, "70cm CW/Phone & Image",        true,
                                430.700000, 430.799999, "70cm CW/Phone/NB Digital",     true,
                                430.800000, 431.399999, "70cm Wide Digital Modes",      true,
                                431.400000, 431.899999, "70cm FM, CW & Image",          true,
                                431.900000, 432.099999, "70cm EME",                     true,

                                432.000000, 432.064999, "70cm EME",                    true,
                                432.065000, 432.067999, "70cm FT8",                     true, // ke9ns add
                                432.068000, 432.999999, "70cm EME",                      true,


                                432.100000, 432.999999, "70cm FM, CW & Image",          true,
                                433.000000, 433.000000, "70cm Emergency",               true,
                                433.000001, 433.999999, "70cm FM, CW & Image",          true,
                                434.000000, 434.999999, "70cm Repeaters",               true,
                                435.000000, 437.999999, "70cm Satellite",               true,
                                438.000000, 438.999999, "70cm All Modes",               true,
                                439.000000, 439.999999, "70cm Repeaters",               true,
                                440.000000, 449.999999, "70cm All Modes",               true,
                                // 1240 - 1300 MHz
								1240.000000, 1259.999999, "23cm All Modes",                true,
                                1260.000000, 1269.999999, "23cm Satellite",             true,
                                1270.000000, 1295.999999, "23cm All Modes",             true,
                                1296.700000, 1296.999999, "23cm EME - all modes",       true,
                                1297.725000, 1299.999999, "23cm All Modes",             true,                                
                                // 2300 -2450 MHz
								2300.000000, 2303.999999, "13cm Sub-Regional",         true,
                                2304.000000, 2305.999999, "13cm Narrow Band ",          true,
                                2306.000000, 2307.999999, "13cm Sub-Regional",          true,
                                2308.000000, 2309.999999, "13cm Narrow Band ",          true,
                                2310.000000, 2319.999999, "13cm Sub-Regional",          true,
                                2320.000000, 2320.024999, "13cm CW EME",                true,
                                2320.025000, 2320.149999, "13cm CW",                    true,
                                2320.150000, 2320.199999, "13cm CW & SSB",              true,
                                2320.200000, 2320.200000, "13cm SSB Calling",           true,
                                2320.200001, 2320.799999, "13cm CW & SSB",              true,
                                2320.800000, 2320.999999, "13cm Beacons",               true,
                                2321.000000, 2321.999999, "13cm NBFM Simplex",          true,
                                2322.000000, 2354.999999, "13cm ATV",                   true,
                                2355.000000, 2364.999999, "13cm Digital Comms",         true,
                                2365.000000, 2369.999999, "13cm Repeaters",             true,
                                2370.000000, 2391.999999, "13cm ATV",                   true,
                                2392.000000, 2399.999999, "13cm Digital Comms",         true,
                                2400.000000, 2450.000000, "13cm Satellite",             true,
                                // 3400 -3475 MHz
								3400.000000, 3400.099999, "9cm Narrow Band Modes",      true,
                                3400.100000, 3400.100000, "9cm Narrow Band Calling",    true,
                                3400.100001, 3401.999999, "9cm Narrow Band Modes",      true,
                                3402.000000, 3419.999999, "9cm All Modes",              true,
                                3420.000000, 3429.999999, "9cm All Modes Digital",      true,
                                3430.000000, 3449.999999, "9cm All Modes",              true,
                                3450.000000, 3454.999999, "9cm All Modes Digital",      true,
                                3455.000000, 3475.000000, "9cm All Modes",              true,
                                // 5650 - 5850 MHz
                                5650.000000, 5667.999999, "5cm Satellite Uplink",       true,
                                5668.000000, 5668.199999, "5cm Sat Uplink/Narrow Band", true,
                                5668.200000, 5668.200000, "5cm Narrow Band calling",    true,
                                5668.200001, 5669.999999, "5cm Sat Uplink/Narrow Band", true,
                                5670.000000, 5699.999999, "5cm Digital",                true,
                                5700.000000, 5719.999999, "5cm ATV",                    true,
                                5720.000000, 5759.999999, "5cm All Modes",              true,
                                5760.000000, 5760.199999, "5cm Narrow Band Modes",      true,
                                5760.200000, 5760.200000, "5cm Narrow Band Calling",    true,
                                5760.200001, 5761.999999, "5cm Narrow Band Modes",      true,
                                5762.000000, 5789.999999, "5cm All Modes",              true,
                                5790.000000, 5850.000000, "5cm Satellite Downlink",     true,
                                // 10.000 - 10.500 GHz
								10000.000000, 10149.999999, "3cm Digital",              true,
                                10150.000000, 10249.999999, "3cm All Modes",            true,
                                10250.000000, 10349.999999, "3cm Digital",              true,
                                10350.000000, 10367.999999, "3cm All Modes",            true,
                                10368.000000, 10368.199999, "3cm Narrow Band Modes",    true,
                                10368.200000, 10368.200000, "3cm Narrow Band Calling",  true,
                                10368.200001, 10369.999999, "3cm Narrow Band Modes",    true,
                                10370.000000, 10449.999999, "3cm All Modes",            true,
                                10450.000000, 10500.000000, "3cm Satellite/All Modes",  true,
                                // 24.000 - 24.250 GHz
								24000.000000, 24047.999999, "1.2cm Satellite",          true,
                                24048.000000, 24048.199999, "1.2cm Narrow Band Modes",  true,
                                24048.200000, 24048.200000, "1.2cm Narrow Band Calling",true,
                                24048.200001, 24049.999999, "1.2cm Narrow Band",        true,
                                24050.000000, 24191.999999, "1.2cm All Modes",          true,
                                24192.000000, 24191.199999, "1.2cm All Modes",          true,
                                24192.200000, 24192.200000, "1.2cm Narrow Band Calling",true,
                                24192.200001, 24193.999999, "1.2cm Narrow Band",        true,
                                24194.000000, 24250.000000, "1.2cm All Modes",          true,
                                // 47.000 - 47.200 GHz
                                47000.000000, 47087.999999, "6mm All Mode",             true,
                                47088.000000, 47088.000000, "6mm Narrow Band Calling",  true,
                                47088.000001, 47200.000000, "6mm All Mode",             true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        // Region 3 specific Band Text below

        private static void AddJapanBandText160m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                1.800000, 1.809999, "160M Band RX",             false,
                                1.810000, 1.824999, "160M CW",                  true,



                                1.825000, 1.907499, "160M Band RX",             false,
                                1.907500, 1.912499, "160M CW & NB Digital (FT8)",     true,
                                1.912500, 1.999999, "160M Band RX",             false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddJapanBandText80m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                3.500000, 3.519999, "80M CW",                   true,
                                3.520000, 3.524999, "80M CW & NB Digital",      true,
                                3.525000, 3.525000, "80M Emergency",            true,
                                3.525001, 3.529999, "80M Phone/CW/NB Digital",  true,
                                3.530000, 3.567999, "80M Phone/CW/Digital",     true,

                                3.568000, 3.568000, "80M FT4/JT65 DIGU",             true, // ke9ns add  3.573
                                3.568001, 3.572999, "80M FT4/JT65 DIGU",             true, // ke9ns add

                                3.573000, 3.573000, "80M FT8 DIGU",             true, // ke9ns add  3.573
                                3.573001, 3.574999, "80M FT8 DIGU",             true, // ke9ns add

                                3.575000, 3.575000, "80M FT4 DIGU",             true, // ke9ns add  3.573
                                3.575001, 3.578000, "80M FT4 DIGU",             true, // ke9ns add
                           
                             
                                3.578001, 3.598999, "80M Band RX",              false,
                                3.599000, 3.611999, "80M Phone/CW/Digital",     true,
                                3.612000, 3.679999, "80M Band RX",              false,
                                3.680000, 3.686999, "80M Phone/CW/Image",       true,
                                3.687000, 3.701999, "80M Band RX",              false,
                                3.702000, 3.715999, "80M Phone/CW/Image",       true,
                                3.716000, 3.744999, "80M Band RX",              false,
                                3.745000, 3.769999, "80M Phone/CW/Image",       true,
                                3.770000, 3.790999, "80M Band RX",              false,
                                3.791000, 3.804999, "80M Phone/CW/NB Digital",  true,
                                3.805000, 3.899999, "80M Band RX",              false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddJapanBandText40m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                7.000000, 7.024999, "40M CW",                   true,
                                7.025000, 7.029999, "40M CW & NB Digital",      true,
                                7.030000, 7.030000, "40M Emergency",            true,
                                7.030001, 7.039999, "40M CW & NB Digital",      true,
                                7.040000, 7.044999, "40M DX NB Digital/CW",     true,
                                7.045000, 7.046999, "40M CW/Phone/Image",       true,

                                7.047000, 7.047000, "40M FT4 DIGU",             true, // ke9ns add  7.047
                                7.047001, 7.050999, "40M FT4 DIGU",             true, // ke9ns add

                                7.051000, 7.073999, "40M CW/Phone/Image",       true,

                                7.074000, 7.074000, "40M FT8 DIGU",             true, // ke9ns add  7.074
                                7.074001, 7.075999, "40M FT8 DIGU",             true, // ke9ns add

                                7.076000, 7.076000, "40M JT65 DIGU",            true, // ke9ns add
                                7.076001, 7.078999, "40M JT65 DIGU",            true, // ke9ns add
                              
                                7.079000, 7.099999, "40M CW/Phone/Image",       true,

                                7.100000, 7.199999, "40M All Modes",            true,
                                7.200000, 7.299999, "40M RX Only",              false,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddJapanBandText10m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                28.000000, 28.069999, "10M CW",                 true,
                                28.070000, 28.073999, "10M CW & NB Digital",    true,

                                28.074000, 28.074000, "10M FT8 DIGU",           true, // ke9ns add
                                28.074001, 28.075999, "10M FT8 DIGU",           true, // ke9ns add
                                28.076000, 28.076000, "10M JT65 DIGU",          true, // ke9ns add 
                                28.076001, 28.078999, "10M JT65 DIGU",          true, // ke9ns add

                                28.079000, 28.149999, "10M CW & NB Digital",    true,

                                28.150000, 28.179999, "10M CW",                  true,

                                28.180000, 28.180000, "10M FT4 DIGU",           true, // ke9ns add 
                                28.180001, 28.184999, "10M FT4 DIGU",           true, // ke9ns add

                                28.185000, 28.199499, "10M CW",                 true,

                                28.199500, 28.199999, "10M Beacons",         true,
                                28.200000, 28.200000, "10M NCDXF Beacons",      true,
                                28.200001, 28.200500, "10M Beacons",            true,
                                28.200501, 28.999999, "10M Phone/CW/NB Digital",true,
                                29.000000, 29.299999, "10M DX Phone/CW/Digital",true,
                                29.300000, 29.509999, "10M Satellite",          true,
                                29.510000, 29.589999, "10M Repeater",           true,
                                29.590000, 29.609999, "10M Wide Phone & CW",    true,
                                29.610000, 29.699999, "10M Repeater",           true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddJapanBandText6m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                50.000000, 50.099999, "6M DX CW/EME/Beacons",   true,
                                50.100000, 50.275999, "6M Phone/CW/Image",      true,

                                50.276000, 50.276000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.276001, 50.278999, "6M JT65 DIGU",           true, // ke9ns add
                                50.279000, 50.309999, "6M All Modes",           true,
                                50.310000, 50.310000, "6M JT65 DIGU",           true, // ke9ns add 
                                50.310001, 50.312999, "6M JT65 DIGU",           true, // ke9ns add

                                50.313000, 50.899999, "6M Phone/CW/Image",     true,

                                50.313000, 50.313000, "6M FT8 DIGU",            true, // ke9ns add   ?
                                50.313001, 50.315999, "6M FT8 DIGU",            true, // ke9ns add

                                50.316000, 50.317999, "6M CW, SSB & Digital",    true,

                                50.318000, 50.318000, "6M FT4 DIGU",            true, // ke9ns add   ?
                                50.318001, 50.322999, "6M FT4 DIGU",            true, // ke9ns add

                                50.323000, 50.323000, "6M DX FT8 DIGU",            true, // ke9ns add   ?
                                50.323001, 50.325999, "6M DX FT8 DIGU",            true, // ke9ns add

                                50.326000, 50.899999, "6M Phone/CW/NB Digital",           true,

                                50.900000, 50.999999, "6M Phone/CW/NB Digital", true,
                                51.100000, 51.999999, "6M Wide Phone/Image/CW", true,
                                52.000000, 52.499999, "6M Phone/CW/NB Digital", true,
                                52.500000, 52.899999, "6M Wide Digital Modes",  true,
                                52.900000, 53.999999, "6M All Modes",           true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        private static void AddJapanBandTextEmergency()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                4.629995, 4.630005, "Japan Int. Emergency", true,
                            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = t.NewRow();
                dr["Low"] = (double)data[i * 4 + 0];
                dr["High"] = (double)data[i * 4 + 1];
                dr["Name"] = (string)data[i * 4 + 2];
                dr["TX"] = (bool)data[i * 4 + 3];
                t.Rows.Add(dr);
            }
        }

        #endregion

        #region IARU Regions 1-3 Bandstack

        private static void AddBandStackTable() // default // ke9ns for FRSRegion.US
        {
            if (bandtextrefresh == false) ds.Tables.Add("BandStack");

            DataTable t = ds.Tables["BandStack"];

            if (bandtextrefresh == false)
            {
                t.Columns.Add("BandName", typeof(string));
                t.Columns.Add("Mode", typeof(string));
                t.Columns.Add("Filter", typeof(string));
                t.Columns.Add("Freq", typeof(double));

            }

            // FT8 1.84, 3.573, 5.357, 7.074, 10.136, 14.074, 18.1, 21.074, 24.915, 28.074, 50.274? or 50.313

            object[] data = {
                                "160M", "CWL", "F5", 1.810000,
                                "160M", "CWU", "F1", 1.835000,
                                "160M", "DIGU","F1", 1.840000, // FT8 3k
								"160M", "LSB", "F6", 1.845000,

                                "80M", "CWL", "F1", 3.501000,
                                "80M", "DIGU","F1", 3.573000, // FT8 3k
								"80M", "LSB", "F6", 3.751000,
                                "80M", "LSB", "F6", 3.850000,

                                "60M", "USB", "F6", 5.330500,
                                "60M", "USB", "F6", 5.346500,
                                "60M", "USB", "F6", 5.357000, // FT8/JT65
								"60M", "USB", "F6", 5.371500,
                                "60M", "USB", "F6", 5.403500,

                                "40M", "CWL", "F1", 7.001000,
                                "40M", "DIGU", "F1",7.074000, // FT8 3k
                                "40M", "LSB", "F6", 7.152000,
                                "40M", "LSB", "F6", 7.255000,

                                "30M", "CWU", "F1", 10.120000,
                                "30M", "CWU", "F1", 10.130000,
                                "30M", "DIGU", "F1",10.136000, // FT8 3k
								"30M", "CWU", "F5", 10.140000,

                                "20M", "CWU", "F1", 14.010000,
                                "20M", "DIGU", "F1",14.074000, // FT8 3k
								"20M", "USB", "F6", 14.230000,
                                "20M", "USB", "F6", 14.336000,

                                "17M", "CWU", "F1", 18.090000,
                                "17M", "DIGU", "F1",18.100000, // FT8 3k
								"17M", "USB", "F6", 18.125000,
                                "17M", "USB", "F6", 18.140000,

                                "15M", "CWU", "F1", 21.001000,
                                "15M", "DIGU", "F1",21.074000, // FT8 3k
								"15M", "USB", "F6", 21.255000,
                                "15M", "USB", "F6", 21.300000,

                                "12M", "CWU", "F1", 24.895000,
                                "12M", "USB", "F6", 24.900000,
                                "12M", "DIGU", "F1",24.915000, // FT8 3k ?
                                "12M", "USB", "F6", 24.970000,

                                "10M", "CWU", "F1", 28.010000,
                                "10M", "DIGU", "F1",28.074000, // FT8 3k
								"10M", "USB", "F6", 28.300000,
                                "10M", "USB", "F6", 28.400000,

                                "6M", "CWU", "F1", 50.010000,
                                "6M", "USB", "F6", 50.125000, // calling freq
                                "6M", "USB", "F6", 50.200000,
                                "6M", "DIGU", "F1",50.274000, // FT8 3k ? JT65?
								"6M", "DIGU", "F1",50.313000, // FT8 3k ?

								"2M", "CWU", "F1", 144.010000,
                                "2M", "USB", "F6", 144.200000,
                                "2M", "USB", "F6", 144.210000,

                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,

                                "GEN", "SAM", "F5", 13.845000,
                                "GEN", "SAM", "F5", 9.550000,
                                "GEN", "SAM", "F5", 5.975000,
                                "GEN", "SAM", "F5", 3.250000,
                                "GEN", "SAM", "F4", 0.590000,


            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }
        } //addbandstacktable


        private static void AddBand2StackTable() // ke9ns add IARU2 same as US plan except 60m is same as IARU1
        {
            ds.Tables.Add("BandStack");
            DataTable t = ds.Tables["BandStack"];

            t.Columns.Add("BandName", typeof(string));
            t.Columns.Add("Mode", typeof(string));
            t.Columns.Add("Filter", typeof(string));
            t.Columns.Add("Freq", typeof(double));

            object[] data = {
                                "160M", "CWL", "F5", 1.810000,
                                "160M", "CWU", "F1", 1.835000,
                                "160M", "LSB", "F6", 1.845000,
                                "80M", "CWL", "F1", 3.501000,
                                "80M", "LSB", "F6", 3.751000,
                                "80M", "LSB", "F6", 3.850000,

                                "60M", "DIGU", "F1", 5.351500, // DIGI
                                "60M", "USB", "F6", 5.35400, // uk CHANNEL 7
                                "60M", "USB", "F6", 5.35700, // us channel 3
                                "60M", "USB", "F6", 5.35000, // 
                                "60M", "USB", "F6", 5.36300, // uK channel 8
                                "60M", "CWU", "F6", 5.366525, // CW

                                "40M", "CWL", "F1", 7.001000,
                                "40M", "LSB", "F6", 7.152000,
                                "40M", "LSB", "F6", 7.255000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "CWU", "F1", 10.130000,
                                "30M", "CWU", "F5", 10.140000,
                                "20M", "CWU", "F1", 14.010000,
                                "20M", "USB", "F6", 14.230000,
                                "20M", "USB", "F6", 14.336000,
                                "17M", "CWU", "F1", 18.090000,
                                "17M", "USB", "F6", 18.125000,
                                "17M", "USB", "F6", 18.140000,
                                "15M", "CWU", "F1", 21.001000,
                                "15M", "USB", "F6", 21.255000,
                                "15M", "USB", "F6", 21.300000,
                                "12M", "CWU", "F1", 24.895000,
                                "12M", "USB", "F6", 24.900000,
                                "12M", "USB", "F6", 24.910000,
                                "10M", "CWU", "F1", 28.010000,
                                "10M", "USB", "F6", 28.300000,
                                "10M", "USB", "F6", 28.400000,
                                "6M", "CWU", "F1", 50.010000,
                                "6M", "USB", "F6", 50.125000,
                                "6M", "USB", "F6", 50.200000,
                                "2M", "CWU", "F1", 144.010000,
                                "2M", "USB", "F6", 144.200000,
                                "2M", "USB", "F6", 144.210000,
                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,

                                "GEN", "SAM", "F5", 13.845000,
                                "GEN", "SAM", "F5", 9.550000,
                                "GEN", "SAM", "F5", 5.975000,
                                "GEN", "SAM", "F5", 3.250000,
                                "GEN", "SAM", "F4", 0.590000,


            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }
        } //addband2stacktable

        private static void AddBandAusStackTable() // ke9ns add Australia (bandstack is like IARU1)
        {
            ds.Tables.Add("BandStack");
            DataTable t = ds.Tables["BandStack"];

            t.Columns.Add("BandName", typeof(string));
            t.Columns.Add("Mode", typeof(string));
            t.Columns.Add("Filter", typeof(string));
            t.Columns.Add("Freq", typeof(double));

            object[] data = {
                                "160M", "CWL", "F5", 1.810000,
                                "160M", "CWU", "F1", 1.835000,
                                "160M", "LSB", "F6", 1.845000,

                                "80M", "CWL", "F1", 3.501000,
                                "80M", "LSB", "F6", 3.751000,
                                "80M", "LSB", "F6", 3.790000,

                                "60M", "DIGU", "F1", 5.351500, // DIGI
                                "60M", "USB", "F6", 5.35400, // uk CHANNEL 7
                                "60M", "USB", "F6", 5.35700, // us channel 3
                                "60M", "USB", "F6", 5.35000, // 
                                "60M", "USB", "F6", 5.36300, // uK channel 8
                                "60M", "CWU", "F6", 5.366525, // CW

                                "40M", "CWL", "F1", 7.001000,
                                "40M", "LSB", "F6", 7.152000,
                                "40M", "LSB", "F6", 7.255000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "CWU", "F1", 10.130000,
                                "30M", "CWU", "F5", 10.140000,
                                "20M", "CWU", "F1", 14.010000,
                                "20M", "USB", "F6", 14.230000,
                                "20M", "USB", "F6", 14.336000,
                                "17M", "CWU", "F1", 18.090000,
                                "17M", "USB", "F6", 18.125000,
                                "17M", "USB", "F6", 18.140000,
                                "15M", "CWU", "F1", 21.001000,
                                "15M", "USB", "F6", 21.255000,
                                "15M", "USB", "F6", 21.300000,
                                "12M", "CWU", "F1", 24.895000,
                                "12M", "USB", "F6", 24.900000,
                                "12M", "USB", "F6", 24.910000,
                                "10M", "CWU", "F1", 28.010000,
                                "10M", "USB", "F6", 28.300000,
                                "10M", "USB", "F6", 28.400000,
                                "6M", "CWU", "F1", 50.010000,
                                "6M", "USB", "F6", 50.125000,
                                "6M", "USB", "F6", 50.200000,
                                "2M", "CWU", "F1", 144.010000,
                                "2M", "USB", "F6", 144.200000,
                                "2M", "USB", "F6", 144.210000,
                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,
                                "GEN", "SAM", "F5", 13.845000,
                                "GEN", "SAM", "F5", 9.550000,
                                "GEN", "SAM", "F5", 5.975000,
                                "GEN", "SAM", "F5", 3.250000,
                                "GEN", "SAM", "F4", 0.590000,


            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }
        } //addbandAusstacktable

        private static void AddRegion1BandStack() // Europe
        {
            ds.Tables["BandStack"].Clear();
            DataTable t = ds.Tables["BandStack"];

            object[] data = {
                                "160M", "CWL", "F1", 1.820000,
                                "160M", "DIGU", "F1", 1.838000,
                                "160M", "LSB", "F6", 1.843000,
                                "80M", "CWL", "F1", 3.510000,
                                "80M", "DIGU", "F1", 3.590000,
                                "80M", "LSB", "F6", 3.750000,

                                "60M", "DIGU", "F1", 5.351500, // DIGI
                                "60M", "USB", "F6", 5.35400, // uk CHANNEL 7
                                "60M", "USB", "F6", 5.35700, // us channel 3
                                "60M", "USB", "F6", 5.35000, // 
                                "60M", "USB", "F6", 5.36300, // uK channel 8
                                "60M", "CWU", "F6", 5.366525, // CW

                                "40M", "CWL", "F1", 7.010000,
                                "40M", "DIGU", "F1", 7.045000,
                                "40M", "LSB", "F6", 7.10000,
                                "30M", "CWU", "F1", 10.110000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "DIGU", "F1", 10.140000,
                                "20M", "CWU", "F1", 14.010000,
                                "20M", "DIGU", "F1", 14.085000,
                                "20M", "USB", "F6", 14.225000,
                                "17M", "CWU", "F1", 18.078000,
                                "17M", "DIGU", "F1", 18.100000,
                                "17M", "USB", "F6", 18.140000,
                                "15M", "CWU", "F1", 21.010000,
                                "15M", "DIGU", "F1", 21.090000,
                                "15M", "USB", "F6", 21.300000,
                                "12M", "CWU", "F1", 24.900000,
                                "12M", "DIGU", "F1", 24.920000,
                                "12M", "USB", "F6", 24.940000,
                                "10M", "CWU", "F1", 28.010000,
                                "10M", "DIGU", "F1", 28.120000,
                                "10M", "USB", "F6", 28.400000,
                                "6M", "CWU", "F1", 50.090000,
                                "6M", "USB", "F6", 50.150000,
                                "6M", "DIGU", "F1", 50.250000,
                                "2M", "CWU", "F1", 144.050000,
                                "2M", "DIGU", "F1", 144.138000,
                                "2M", "USB", "F6", 144.300000,
                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,
                                "GEN", "SAM", "F6", 13.845000,
                                "GEN", "SAM", "F7", 5.975000,
                                "GEN", "SAM", "F7", 9.550000,
                                "GEN", "SAM", "F7", 3.850000,
                                "GEN", "SAM", "F8", 0.590000,



            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }


        } // AddRegion1BandStack()

        //   5.250000, 5.351499, "60M RX Only",              false,
        //    5.351500, 5.335999, "60M 200hz Narrow Band Modes",    true,
        //    5.354000, 5.358499, "60M USB Voice",            true,
        //   5.358500, 5.361300, "60M USB Voice (US CH 3)",  true,
        //   5.361301, 5.362999, "60M USB Voice",            true,
        //  5.363000, 5.365999, "60M USB Voice",            true,
        //  5.366000, 5.366500, "60M 20hz Narrow Band Modes",    true,
        //  5.366501, 5.450000, "60M RX Only",              false,

        private static void AddRegion1ABandStack() // germany, spain, swits, fin, lux, belg
        {
            ds.Tables["BandStack"].Clear();
            DataTable t = ds.Tables["BandStack"];

            object[] data = {
                                "160M", "CWL", "F1", 1.820000,
                                "160M", "DIGU", "F1", 1.838000,
                                "160M", "LSB", "F6", 1.843000,
                                "80M", "CWL", "F1", 3.510000,
                                "80M", "DIGU", "F1", 3.590000,
                                "80M", "LSB", "F6", 3.750000,

                                "60M", "DIGU", "F1", 5.351500, // DIGI
                                "60M", "USB", "F6", 5.35400, // uk CHANNEL 7
                                "60M", "USB", "F6", 5.35700, // us channel 3
                                "60M", "USB", "F6", 5.35000, // 
                                "60M", "USB", "F6", 5.36300, // uK channel 8
                                "60M", "CWU", "F6", 5.366525, // CW

                                "40M", "CWL", "F1", 7.010000,
                                "40M", "DIGU", "F1", 7.045000,
                                "40M", "LSB", "F6", 7.10000,

                                "30M", "CWU", "F1", 10.110000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "DIGU", "F1", 10.140000,

                                "20M", "CWU", "F1", 14.010000,
                                "20M", "DIGU", "F1", 14.085000,
                                "20M", "USB", "F6", 14.225000,

                                "17M", "CWU", "F1", 18.078000,
                                "17M", "DIGU", "F1", 18.100000,
                                "17M", "USB", "F6", 18.140000,

                                "15M", "CWU", "F1", 21.010000,
                                "15M", "DIGU", "F1", 21.090000,
                                "15M", "USB", "F6", 21.300000,

                                "12M", "CWU", "F1", 24.900000,
                                "12M", "DIGU", "F1", 24.920000,
                                "12M", "USB", "F6", 24.940000,

                                "10M", "CWU", "F1", 28.010000,
                                "10M", "DIGU", "F1", 28.120000,
                                "10M", "USB", "F6", 28.400000,

                                "6M", "CWU", "F1", 50.090000,
                                "6M", "USB", "F6", 50.150000,
                                "6M", "DIGU", "F1", 50.250000,

                                "2M", "CWU", "F1", 144.050000,
                                "2M", "DIGU", "F1", 144.138000,
                                "2M", "USB", "F6", 144.300000,

                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,
                                "GEN", "SAM", "F6", 13.845000,
                                "GEN", "SAM", "F7", 5.975000,
                                "GEN", "SAM", "F7", 9.550000,
                                "GEN", "SAM", "F7", 3.850000,
                                "GEN", "SAM", "F8", 0.590000,



            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }


        } // AddRegion1ABandStack()




        private static void AddRegion3BandStack()  // Asia
        {
            ds.Tables["BandStack"].Clear();
            DataTable t = ds.Tables["BandStack"];

            object[] data = {
                                "160M", "CWL", "F1", 1.820000,
                                "160M", "DIGU", "F1", 1.832000,
                                "160M", "LSB", "F6", 1.843000,
                                "80M", "CWL", "F1", 3.510000,
                                "80M", "DIGU", "F1", 3.580000,
                                "80M", "LSB", "F6", 3.750000,

                                "61M", "USB", "F6",  4.629995, // "Japan Int. Emergency", true,

                                "60M", "DIGU", "F1", 5.351500, // DIGI
                                "60M", "USB", "F6", 5.35400, // uk CHANNEL 7
                                "60M", "USB", "F6", 5.35700, // us channel 3
                                "60M", "USB", "F6", 5.35000, // 
                                "60M", "USB", "F6", 5.36300, // uK channel 8
                                "60M", "CWU", "F6", 5.366525, // CW

                                "40M", "CWL", "F1", 7.010000,
                                "40M", "DIGU", "F1", 7.035000,
                                "40M", "LSB", "F6", 7.12000,
                                "30M", "CWU", "F1", 10.110000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "DIGU", "F1", 10.140000,
                                "20M", "CWU", "F1", 14.010000,
                                "20M", "DIGU", "F1", 14.085000,
                                "20M", "USB", "F6", 14.225000,
                                "17M", "CWU", "F1", 18.078000,
                                "17M", "DIGU", "F1", 18.100000,
                                "17M", "USB", "F6", 18.140000,
                                "15M", "CWU", "F1", 21.010000,
                                "15M", "DIGU", "F1", 21.090000,
                                "15M", "USB", "F6", 21.300000,
                                "12M", "CWU", "F1", 24.900000,
                                "12M", "DIGU", "F1", 24.920000,
                                "12M", "USB", "F6", 24.940000,
                                "10M", "CWU", "F1", 28.010000,
                                "10M", "DIGU", "F1", 28.120000,
                                "10M", "USB", "F6", 28.400000,
                                "6M", "CWU", "F1", 50.090000,
                                "6M", "USB", "F6", 50.150000,
                                "6M", "DIGU", "F1", 50.250000,
                                "2M", "CWU", "F1", 144.050000,
                                "2M", "DIGU", "F1", 144.138000,
                                "2M", "USB", "F6", 144.200000,
                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,
                                "GEN", "SAM", "F6", 13.845000,
                                "GEN", "SAM", "F7", 5.975000,
                                "GEN", "SAM", "F7", 9.550000,
                                "GEN", "SAM", "F7", 3.850000,
                                "GEN", "SAM", "F8", 0.590000,



            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }
        }

        private static void AddUK_PlusBandStack()
        {
            ds.Tables["BandStack"].Clear();
            DataTable t = ds.Tables["BandStack"];

            object[] data = {
                                "160M", "CWL", "F1", 1.820000,
                                "160M", "DIGU", "F1", 1.838000,
                                "160M", "LSB", "F6", 1.843000,
                                "80M", "CWL", "F1", 3.510000,
                                "80M", "DIGU", "F1", 3.590000,
                                "80M", "LSB", "F6", 3.750000,
                                "60M", "USB", "F6", 5.258500,
                                "60M", "USB", "F6", 5.276000,
                                "60M", "USB", "F6", 5.288500,
                                "60M", "USB", "F6", 5.298000,
                                "60M", "USB", "F6", 5.313000,
                                "60M", "USB", "F6", 5.333000,
                                "60M", "USB", "F6", 5.354000,
                                "60M", "USB", "F6", 5.362000,
                                "60M", "USB", "F6", 5.378000,
                                "60M", "USB", "F6", 5.395000,
                                "60M", "USB", "F6", 5.403500,
                                "40M", "CWL", "F1", 7.010000,
                                "40M", "DIGU", "F1", 7.045000,
                                "40M", "LSB", "F6", 7.10000,
                                "30M", "CWU", "F1", 10.110000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "DIGU", "F1", 10.140000,
                                "20M", "CWU", "F1", 14.010000,
                                "20M", "DIGU", "F1", 14.085000,
                                "20M", "USB", "F6", 14.225000,
                                "17M", "CWU", "F1", 18.078000,
                                "17M", "DIGU", "F1", 18.100000,
                                "17M", "USB", "F6", 18.140000,
                                "15M", "CWU", "F1", 21.010000,
                                "15M", "DIGU", "F1", 21.090000,
                                "15M", "USB", "F6", 21.300000,
                                "12M", "CWU", "F1", 24.900000,
                                "12M", "DIGU", "F1", 24.920000,
                                "12M", "USB", "F6", 24.940000,
                                "10M", "CWU", "F1", 28.010000,
                                "10M", "DIGU", "F1", 28.120000,
                                "10M", "USB", "F6", 28.400000,
                                "6M", "CWU", "F1", 50.090000,
                                "6M", "USB", "F6", 50.150000,
                                "6M", "DIGU", "F1", 50.250000,
                                "2M", "CWU", "F1", 144.050000,
                                "2M", "DIGU", "F1", 144.138000,
                                "2M", "USB", "F6", 144.300000,
                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,
                                "GEN", "SAM", "F6", 13.845000,
                                "GEN", "SAM", "F7", 5.975000,
                                "GEN", "SAM", "F7", 9.550000,
                                "GEN", "SAM", "F7", 3.850000,
                                "GEN", "SAM", "F8", 0.590000,




            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }
        }

        private static void AddSwedenBandStack()
        {
            ds.Tables["BandStack"].Clear();
            DataTable t = ds.Tables["BandStack"];

            object[] data = {
                                "160M", "CWL", "F1", 1.820000,
                                "160M", "DIGU", "F1", 1.838000,
                                "160M", "LSB", "F6", 1.843000,
                                "80M", "CWL", "F1", 3.510000,
                                "80M", "DIGU", "F1", 3.590000,
                                "80M", "LSB", "F6", 3.750000,

                              //  "60M", "USB", "F6", 5.310000,
						//		"60M", "USB", "F6", 5.320000,
							//	"60M", "USB", "F6", 5.380000,
								//"60M", "USB", "F6", 5.390000,

                                "60M", "DIGU", "F1", 5.351500, // DIGI
                                "60M", "USB", "F6", 5.35400, // uk CHANNEL 7
                                "60M", "USB", "F6", 5.35700, // us channel 3
                                "60M", "USB", "F6", 5.35000, // 
                                "60M", "USB", "F6", 5.36300, // uK channel 8
                                "60M", "CWU", "F6", 5.366525, // CW


                                "40M", "CWL", "F1", 7.010000,
                                "40M", "DIGU", "F1", 7.045000,
                                "40M", "LSB", "F6", 7.10000,
                                "30M", "CWU", "F1", 10.110000,
                                "30M", "CWU", "F1", 10.120000,
                                "30M", "DIGU", "F1", 10.140000,
                                "20M", "CWU", "F1", 14.010000,
                                "20M", "DIGU", "F1", 14.085000,
                                "20M", "USB", "F6", 14.225000,
                                "17M", "CWU", "F1", 18.078000,
                                "17M", "DIGU", "F1", 18.100000,
                                "17M", "USB", "F6", 18.140000,
                                "15M", "CWU", "F1", 21.010000,
                                "15M", "DIGU", "F1", 21.090000,
                                "15M", "USB", "F6", 21.300000,
                                "12M", "CWU", "F1", 24.900000,
                                "12M", "DIGU", "F1", 24.920000,
                                "12M", "USB", "F6", 24.940000,
                                "10M", "CWU", "F1", 28.010000,
                                "10M", "DIGU", "F1", 28.120000,
                                "10M", "USB", "F6", 28.400000,
                                "6M", "CWU", "F1", 50.090000,
                                "6M", "USB", "F6", 50.150000,
                                "6M", "DIGU", "F1", 50.250000,
                                "2M", "CWU", "F1", 144.050000,
                                "2M", "DIGU", "F1", 144.138000,
                                "2M", "USB", "F6", 144.300000,
                                "WWV", "SAM", "F5", 2.500000,
                                "WWV", "SAM", "F5", 5.000000,
                                "WWV", "SAM", "F5", 10.000000,
                                "WWV", "SAM", "F5", 15.000000,
                                "WWV", "SAM", "F5", 20.000000,
                                "WWV", "SAM", "F5", 25.000000, // ke9ns add
                                "WWV", "USB", "F6", 3.330000,
                                "WWV", "USB", "F6", 7.850000,
                                "WWV", "USB", "F6", 14.670000,
                                "GEN", "SAM", "F6", 13.845000,
                                "GEN", "SAM", "F7", 5.975000,
                                "GEN", "SAM", "F7", 9.550000,
                                "GEN", "SAM", "F7", 3.850000,
                                "GEN", "SAM", "F8", 0.590000,


            };

            for (int i = 0; i < data.Length / 4; i++)
            {
                DataRow dr = ds.Tables["BandStack"].NewRow();
                dr["BandName"] = (string)data[i * 4 + 0];
                dr["Mode"] = (string)data[i * 4 + 1];
                dr["Filter"] = (string)data[i * 4 + 2];
                dr["Freq"] = ((double)data[i * 4 + 3]).ToString("f6");
                ds.Tables["BandStack"].Rows.Add(dr);
            }
        }

        #endregion


        private static void AddMemoryTable()
        {
            ds.Tables.Add("Memory");
            DataTable t = ds.Tables["Memory"];

            t.Columns.Add("GroupID", typeof(int));
            t.Columns.Add("Freq", typeof(double));
            t.Columns.Add("ModeID", typeof(int));
            t.Columns.Add("FilterID", typeof(int));
            t.Columns.Add("Callsign", typeof(string));
            t.Columns.Add("Comments", typeof(string));
            t.Columns.Add("Scan", typeof(bool));
            t.Columns.Add("Squelch", typeof(int));
            t.Columns.Add("StepSizeID", typeof(int));
            t.Columns.Add("AGCID", typeof(int));
            t.Columns.Add("Gain", typeof(string));
            t.Columns.Add("FilterLow", typeof(int));
            t.Columns.Add("FilterHigh", typeof(int));
            t.Columns.Add("CreateDate", typeof(string));
        }

        private static void AddGroupListTable()
        {
            ds.Tables.Add("GroupList");
            DataTable t = ds.Tables["GroupList"];

            t.Columns.Add("GroupID", typeof(int));
            t.Columns.Add("GroupName", typeof(string));

            string[] vals = { "AM", "FM", "SSB", "SSTV", "CW", "PSK", "RTTY" };

            for (int i = 0; i < vals.Length; i++)
            {
                DataRow dr = t.NewRow();
                dr[0] = i;
                dr[1] = vals[i];
                t.Rows.Add(dr);
            }
        }

        //=================================================================================
        // ke9ns: update older database here
        public static void ModTXProfileTable()
        {
            DataTable t = ds.Tables["TXProfile"];


            try
            {
                t.Columns.Add("TXEQPreamp", typeof(int));

            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TXEQ28Preamp", typeof(int)); // ke9ns add
            }
            catch (Exception e1)
            {
                //   Debug.WriteLine("ALEADY a TXEQ28Preamp " + e1);
            }


            try
            {

                t.Columns.Add("TXEQ9Preamp", typeof(int)); // ke9ns add

            }
            catch (Exception)
            {

            }


            try
            {
                t.Columns.Add("TXEQ28Band", typeof(bool));  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TXEQ10Band", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TXEQBand", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TXEQ9Band", typeof(bool));   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            }
            catch (Exception)
            {

            }
            try
            {
                t.Columns.Add("TXEQ37Band", typeof(bool));  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TX28EQ1", typeof(int));
                t.Columns.Add("TX28EQ2", typeof(int));
                t.Columns.Add("TX28EQ3", typeof(int));
                t.Columns.Add("TX28EQ4", typeof(int));
                t.Columns.Add("TX28EQ5", typeof(int));
                t.Columns.Add("TX28EQ6", typeof(int));
                t.Columns.Add("TX28EQ7", typeof(int));
                t.Columns.Add("TX28EQ8", typeof(int));
                t.Columns.Add("TX28EQ9", typeof(int));
                t.Columns.Add("TX28EQ10", typeof(int));
                t.Columns.Add("TX28EQ11", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ12", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ13", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ14", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ15", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ16", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ17", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ18", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ19", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ20", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ21", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ22", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ23", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ24", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ25", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ26", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ27", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ28", typeof(int)); // ke9ns add
            }
            catch (Exception)
            {

            }

            try
            {

                t.Columns.Add("PEQ1", typeof(int));
                t.Columns.Add("PEQ2", typeof(int));
                t.Columns.Add("PEQ3", typeof(int));
                t.Columns.Add("PEQ4", typeof(int));
                t.Columns.Add("PEQ5", typeof(int));
                t.Columns.Add("PEQ6", typeof(int));
                t.Columns.Add("PEQ7", typeof(int));
                t.Columns.Add("PEQ8", typeof(int));
                t.Columns.Add("PEQ9", typeof(int));
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("PEQfreq1", typeof(int));
                t.Columns.Add("PEQfreq2", typeof(int));
                t.Columns.Add("PEQfreq3", typeof(int));
                t.Columns.Add("PEQfreq4", typeof(int));
                t.Columns.Add("PEQfreq5", typeof(int));
                t.Columns.Add("PEQfreq6", typeof(int));
                t.Columns.Add("PEQfreq7", typeof(int));
                t.Columns.Add("PEQfreq8", typeof(int));
                t.Columns.Add("PEQfreq9", typeof(int));
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("PEQoctave1", typeof(int));
                t.Columns.Add("PEQoctave2", typeof(int));
                t.Columns.Add("PEQoctave3", typeof(int));
                t.Columns.Add("PEQoctave4", typeof(int));
                t.Columns.Add("PEQoctave5", typeof(int));
                t.Columns.Add("PEQoctave6", typeof(int));
                t.Columns.Add("PEQoctave7", typeof(int));
                t.Columns.Add("PEQoctave8", typeof(int));
                t.Columns.Add("PEQoctave9", typeof(int));
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("RX1DSPMODE", typeof(DSPMode)); // .196
                t.Columns.Add("RX2DSPMODE", typeof(DSPMode)); // .196

            }
            catch (Exception)
            {

            }


        } // ModTXProfileTable()

        //=================================================================================
        // ke9ns: update older database here
        public static void ModTXProfileDefTable()
        {

            DataTable t = ds.Tables["TXProfileDef"];


            try
            {
                t.Columns.Add("TXEQPreamp", typeof(int));

            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TXEQ28Preamp", typeof(int)); // ke9ns add
            }
            catch (Exception e1)
            {
                //  Debug.WriteLine("ALEADY a TXEQ28Preamp " + e1);
            }


            try
            {

                t.Columns.Add("TXEQ9Preamp", typeof(int)); // ke9ns add

            }
            catch (Exception)
            {

            }


            try
            {
                t.Columns.Add("TXEQ28Band", typeof(bool));  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            }
            catch (Exception)
            {

            }


            try
            {
                t.Columns.Add("TXEQ10Band", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TXEQBand", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            }
            catch (Exception)
            {

            }


            try
            {
                t.Columns.Add("TXEQ9Band", typeof(bool));   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            }
            catch (Exception)
            {

            }
            try
            {
                t.Columns.Add("TXEQ37Band", typeof(bool));  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("TX28EQ1", typeof(int));
                t.Columns.Add("TX28EQ2", typeof(int));
                t.Columns.Add("TX28EQ3", typeof(int));
                t.Columns.Add("TX28EQ4", typeof(int));
                t.Columns.Add("TX28EQ5", typeof(int));
                t.Columns.Add("TX28EQ6", typeof(int));
                t.Columns.Add("TX28EQ7", typeof(int));
                t.Columns.Add("TX28EQ8", typeof(int));
                t.Columns.Add("TX28EQ9", typeof(int));
                t.Columns.Add("TX28EQ10", typeof(int));
                t.Columns.Add("TX28EQ11", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ12", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ13", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ14", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ15", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ16", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ17", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ18", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ19", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ20", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ21", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ22", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ23", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ24", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ25", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ26", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ27", typeof(int)); // ke9ns add
                t.Columns.Add("TX28EQ28", typeof(int)); // ke9ns add
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("PEQ1", typeof(int));
                t.Columns.Add("PEQ2", typeof(int));
                t.Columns.Add("PEQ3", typeof(int));
                t.Columns.Add("PEQ4", typeof(int));
                t.Columns.Add("PEQ5", typeof(int));
                t.Columns.Add("PEQ6", typeof(int));
                t.Columns.Add("PEQ7", typeof(int));
                t.Columns.Add("PEQ8", typeof(int));
                t.Columns.Add("PEQ9", typeof(int));
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("PEQfreq1", typeof(int));
                t.Columns.Add("PEQfreq2", typeof(int));
                t.Columns.Add("PEQfreq3", typeof(int));
                t.Columns.Add("PEQfreq4", typeof(int));
                t.Columns.Add("PEQfreq5", typeof(int));
                t.Columns.Add("PEQfreq6", typeof(int));
                t.Columns.Add("PEQfreq7", typeof(int));
                t.Columns.Add("PEQfreq8", typeof(int));
                t.Columns.Add("PEQfreq9", typeof(int));
            }
            catch (Exception)
            {

            }

            try
            {
                t.Columns.Add("PEQoctave1", typeof(int));
                t.Columns.Add("PEQoctave2", typeof(int));
                t.Columns.Add("PEQoctave3", typeof(int));
                t.Columns.Add("PEQoctave4", typeof(int));
                t.Columns.Add("PEQoctave5", typeof(int));
                t.Columns.Add("PEQoctave6", typeof(int));
                t.Columns.Add("PEQoctave7", typeof(int));
                t.Columns.Add("PEQoctave8", typeof(int));
                t.Columns.Add("PEQoctave9", typeof(int));
            }
            catch (Exception)
            {

            }


            try
            {
                t.Columns.Add("RX1DSPMODE", typeof(DSPMode));  // ke9ns add .196
                t.Columns.Add("RX2DSPMODE", typeof(DSPMode));  // ke9ns add .196
            }
            catch (Exception)
            {

            }

        } // ModTXProfileDefTable()


        //=================================================================================
        // ke9ns: initial only create a txprofile setup if the database has none
        public static void AddTXProfileTable(Model model)
        {
            //  Debug.WriteLine("ADDTXPROFILETABLE");

            ds.Tables.Add("TXProfile");
            DataTable t = ds.Tables["TXProfile"];

            t.Columns.Add("Name", typeof(string));
            t.Columns.Add("FilterLow", typeof(int));
            t.Columns.Add("FilterHigh", typeof(int));

            t.Columns.Add("TXEQNumBands", typeof(int));
            t.Columns.Add("TXEQEnabled", typeof(bool));

            t.Columns.Add("TXEQPreamp", typeof(int));
            t.Columns.Add("TXEQ28Preamp", typeof(int)); // ke9ns add
            t.Columns.Add("TXEQ9Preamp", typeof(int)); // ke9ns add

            t.Columns.Add("TXEQ28Band", typeof(bool));  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            t.Columns.Add("TXEQ10Band", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            t.Columns.Add("TXEQBand", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            t.Columns.Add("TXEQ9Band", typeof(bool));   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            t.Columns.Add("TXEQ37Band", typeof(bool));  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            t.Columns.Add("TXEQ1", typeof(int));
            t.Columns.Add("TXEQ2", typeof(int));
            t.Columns.Add("TXEQ3", typeof(int));
            t.Columns.Add("TXEQ4", typeof(int));
            t.Columns.Add("TXEQ5", typeof(int));
            t.Columns.Add("TXEQ6", typeof(int));
            t.Columns.Add("TXEQ7", typeof(int));
            t.Columns.Add("TXEQ8", typeof(int));
            t.Columns.Add("TXEQ9", typeof(int));
            t.Columns.Add("TXEQ10", typeof(int));

            t.Columns.Add("TX28EQ1", typeof(int));
            t.Columns.Add("TX28EQ2", typeof(int));
            t.Columns.Add("TX28EQ3", typeof(int));
            t.Columns.Add("TX28EQ4", typeof(int));
            t.Columns.Add("TX28EQ5", typeof(int));
            t.Columns.Add("TX28EQ6", typeof(int));
            t.Columns.Add("TX28EQ7", typeof(int));
            t.Columns.Add("TX28EQ8", typeof(int));
            t.Columns.Add("TX28EQ9", typeof(int));
            t.Columns.Add("TX28EQ10", typeof(int));
            t.Columns.Add("TX28EQ11", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ12", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ13", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ14", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ15", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ16", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ17", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ18", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ19", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ20", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ21", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ22", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ23", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ24", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ25", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ26", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ27", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ28", typeof(int)); // ke9ns add

            t.Columns.Add("PEQ1", typeof(int));
            t.Columns.Add("PEQ2", typeof(int));
            t.Columns.Add("PEQ3", typeof(int));
            t.Columns.Add("PEQ4", typeof(int));
            t.Columns.Add("PEQ5", typeof(int));
            t.Columns.Add("PEQ6", typeof(int));
            t.Columns.Add("PEQ7", typeof(int));
            t.Columns.Add("PEQ8", typeof(int));
            t.Columns.Add("PEQ9", typeof(int));

            t.Columns.Add("PEQfreq1", typeof(int));
            t.Columns.Add("PEQfreq2", typeof(int));
            t.Columns.Add("PEQfreq3", typeof(int));
            t.Columns.Add("PEQfreq4", typeof(int));
            t.Columns.Add("PEQfreq5", typeof(int));
            t.Columns.Add("PEQfreq6", typeof(int));
            t.Columns.Add("PEQfreq7", typeof(int));
            t.Columns.Add("PEQfreq8", typeof(int));
            t.Columns.Add("PEQfreq9", typeof(int));

            t.Columns.Add("PEQoctave1", typeof(int));
            t.Columns.Add("PEQoctave2", typeof(int));
            t.Columns.Add("PEQoctave3", typeof(int));
            t.Columns.Add("PEQoctave4", typeof(int));
            t.Columns.Add("PEQoctave5", typeof(int));
            t.Columns.Add("PEQoctave6", typeof(int));
            t.Columns.Add("PEQoctave7", typeof(int));
            t.Columns.Add("PEQoctave8", typeof(int));
            t.Columns.Add("PEQoctave9", typeof(int));

            t.Columns.Add("RX1DSPMODE", typeof(DSPMode)); // .196
            t.Columns.Add("RX2DSPMODE", typeof(DSPMode)); // .196

            //==================================

            t.Columns.Add("VAC1_SelectA", typeof(bool)); //.279
            t.Columns.Add("VAC1_SelectB", typeof(bool)); //.279
            t.Columns.Add("VAC1_MixAudio", typeof(bool)); //.279
            t.Columns.Add("VAC1_Reset", typeof(bool)); //.279

            t.Columns.Add("Drive_Max", typeof(int)); //.279a

            //==================================

            t.Columns.Add("DXOn", typeof(bool));
            t.Columns.Add("DXLevel", typeof(int));
            t.Columns.Add("CompanderOn", typeof(bool));
            t.Columns.Add("CompanderLevel", typeof(int));
            t.Columns.Add("MicGain", typeof(int));
            t.Columns.Add("FMMicGain", typeof(int));
            t.Columns.Add("Lev_On", typeof(bool));
            t.Columns.Add("Lev_Slope", typeof(int));
            t.Columns.Add("Lev_MaxGain", typeof(int));
            t.Columns.Add("Lev_Attack", typeof(int));
            t.Columns.Add("Lev_Decay", typeof(int));
            t.Columns.Add("Lev_Hang", typeof(int));
            t.Columns.Add("Lev_HangThreshold", typeof(int));
            t.Columns.Add("ALC_Slope", typeof(int));
            t.Columns.Add("ALC_MaxGain", typeof(int));
            t.Columns.Add("ALC_Attack", typeof(int));
            t.Columns.Add("ALC_Decay", typeof(int));
            t.Columns.Add("ALC_Hang", typeof(int));
            t.Columns.Add("ALC_HangThreshold", typeof(int));
            t.Columns.Add("Power", typeof(int));
            t.Columns.Add("Dexp_On", typeof(bool));
            t.Columns.Add("Dexp_Threshold", typeof(int));
            t.Columns.Add("Dexp_Attenuate", typeof(int));
            t.Columns.Add("VOX_On", typeof(bool));
            t.Columns.Add("VOX_Threshold", typeof(int));
            t.Columns.Add("VOX_HangTime", typeof(int));
            t.Columns.Add("Tune_Power", typeof(int));
            t.Columns.Add("Tune_Meter_Type", typeof(string));
            t.Columns.Add("TX_Limit_Slew", typeof(bool));
            t.Columns.Add("TXBlankingTime", typeof(int));
            t.Columns.Add("MicBoost", typeof(bool));
            t.Columns.Add("TX_AF_Level", typeof(int));
            t.Columns.Add("AM_Carrier_Level", typeof(int));
            t.Columns.Add("Show_TX_Filter", typeof(bool));
            t.Columns.Add("VAC1_On", typeof(bool));
            t.Columns.Add("VAC1_Auto_On", typeof(bool));
            t.Columns.Add("VAC1_RX_Gain", typeof(int));
            t.Columns.Add("VAC1_TX_Gain", typeof(int));
            t.Columns.Add("VAC1_Stereo_On", typeof(bool));
            t.Columns.Add("VAC1_Sample_Rate", typeof(string));
            t.Columns.Add("VAC1_Buffer_Size", typeof(string));
            t.Columns.Add("VAC1_IQ_Output", typeof(bool));
            t.Columns.Add("VAC1_IQ_Correct", typeof(bool));
            t.Columns.Add("VAC1_PTT_OverRide", typeof(bool));
            t.Columns.Add("VAC1_Combine_Input_Channels", typeof(bool));
            t.Columns.Add("VAC1_Latency_On", typeof(bool));
            t.Columns.Add("VAC1_Latency_Duration", typeof(int));
            t.Columns.Add("VAC2_On", typeof(bool));
            t.Columns.Add("VAC2_Auto_On", typeof(bool));
            t.Columns.Add("VAC2_RX_Gain", typeof(int));
            t.Columns.Add("VAC2_TX_Gain", typeof(int));
            t.Columns.Add("VAC2_Stereo_On", typeof(bool));
            t.Columns.Add("VAC2_Sample_Rate", typeof(string));
            t.Columns.Add("VAC2_Buffer_Size", typeof(string));
            t.Columns.Add("VAC2_IQ_Output", typeof(bool));
            t.Columns.Add("VAC2_IQ_Correct", typeof(bool));
            t.Columns.Add("VAC2_Combine_Input_Channels", typeof(bool));
            t.Columns.Add("VAC2_Latency_On", typeof(bool));
            t.Columns.Add("VAC2_Latency_Duration", typeof(int));
            t.Columns.Add("Phone_RX_DSP_Buffer", typeof(string));
            t.Columns.Add("Phone_TX_DSP_Buffer", typeof(string));
            t.Columns.Add("Digi_RX_DSP_Buffer", typeof(string));
            t.Columns.Add("Digi_TX_DSP_Buffer", typeof(string));
            t.Columns.Add("CW_RX_DSP_Buffer", typeof(string));
            t.Columns.Add("Mic_Input_On", typeof(string));
            t.Columns.Add("Mic_Input_Level", typeof(int));
            t.Columns.Add("Line_Input_On", typeof(string));
            t.Columns.Add("Line_Input_Level", typeof(int));
            t.Columns.Add("Balanced_Line_Input_On", typeof(string));
            t.Columns.Add("Balanced_Line_Input_Level", typeof(int));
            t.Columns.Add("FlexWire_Input_On", typeof(string));
            t.Columns.Add("FlexWire_Input_Level", typeof(int));


            #region Default

            DataRow dr = t.NewRow();

            dr["Name"] = "Default";
            dr["FilterLow"] = 70;  // .261
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;


            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;


            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;


            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            //=================================================

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a

            //=================================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            } // switch

            t.Rows.Add(dr);

            #endregion

            #region Default DX

            dr = t.NewRow();
            dr["Name"] = "Default DX";
            dr["FilterLow"] = 200;
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;


            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;


            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            //=================================================

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a


            //============================================
            dr["DXOn"] = true;
            dr["DXLevel"] = 5;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 5;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region FHM-1

            dr = t.NewRow();
            dr["Name"] = "FHM-1";
            dr["FilterLow"] = 70; // .261 was 150
            dr["FilterHigh"] = 3050;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;

            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;


            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = -3;
            dr["TXEQ3"] = -7;
            dr["TXEQ4"] = -5;
            dr["TXEQ5"] = -3;
            dr["TXEQ6"] = 2;
            dr["TXEQ7"] = 6;
            dr["TXEQ8"] = 3;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = -6;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            //=================================================

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a

            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 35;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 100;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = true;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "1024";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = -28;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 15;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region FHM-1 DX

            dr = t.NewRow();
            dr["Name"] = "FHM-1 DX";
            dr["FilterLow"] = 200; // ke9ns .261 was 350
            dr["FilterHigh"] = 2400;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;


            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = -3;
            dr["TXEQ3"] = -6;
            dr["TXEQ4"] = -10;
            dr["TXEQ5"] = -6;
            dr["TXEQ6"] = 3;
            dr["TXEQ7"] = 6;
            dr["TXEQ8"] = 3;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = -6;


            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196


            //=================================================

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a

            //============================================


            dr["DXOn"] = true;
            dr["DXLevel"] = 5;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 35;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = true;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = -28;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 15;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

        } //  AddTXProfileTable(Model model)

        #region TX Profile Management


        //=================================================================================
        // ke9ns: initial only create a txprofile setup if the database hos none
        public static void AddTXProfileDefTable(Model model)
        {
            ds.Tables.Add("TXProfileDef");
            DataTable t = ds.Tables["TXProfileDef"];

            t.Columns.Add("Name", typeof(string));
            t.Columns.Add("FilterLow", typeof(int));
            t.Columns.Add("FilterHigh", typeof(int));
            t.Columns.Add("TXEQNumBands", typeof(int));
            t.Columns.Add("TXEQEnabled", typeof(bool));
            t.Columns.Add("TXEQPreamp", typeof(int));

            t.Columns.Add("TXEQ28Preamp", typeof(int)); // ke9ns add
            t.Columns.Add("TXEQ9Preamp", typeof(int)); // ke9ns add

            t.Columns.Add("TXEQ28Band", typeof(bool));  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            t.Columns.Add("TXEQ10Band", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            t.Columns.Add("TXEQBand", typeof(bool));   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            t.Columns.Add("TXEQ9Band", typeof(bool));   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            t.Columns.Add("TXEQ37Band", typeof(bool));  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;


            t.Columns.Add("TXEQ1", typeof(int));
            t.Columns.Add("TXEQ2", typeof(int));
            t.Columns.Add("TXEQ3", typeof(int));
            t.Columns.Add("TXEQ4", typeof(int));
            t.Columns.Add("TXEQ5", typeof(int));
            t.Columns.Add("TXEQ6", typeof(int));
            t.Columns.Add("TXEQ7", typeof(int));
            t.Columns.Add("TXEQ8", typeof(int));
            t.Columns.Add("TXEQ9", typeof(int));
            t.Columns.Add("TXEQ10", typeof(int));



            t.Columns.Add("TX28EQ1", typeof(int));
            t.Columns.Add("TX28EQ2", typeof(int));
            t.Columns.Add("TX28EQ3", typeof(int));
            t.Columns.Add("TX28EQ4", typeof(int));
            t.Columns.Add("TX28EQ5", typeof(int));
            t.Columns.Add("TX28EQ6", typeof(int));
            t.Columns.Add("TX28EQ7", typeof(int));
            t.Columns.Add("TX28EQ8", typeof(int));
            t.Columns.Add("TX28EQ9", typeof(int));
            t.Columns.Add("TX28EQ10", typeof(int));
            t.Columns.Add("TX28EQ11", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ12", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ13", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ14", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ15", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ16", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ17", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ18", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ19", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ20", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ21", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ22", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ23", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ24", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ25", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ26", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ27", typeof(int)); // ke9ns add
            t.Columns.Add("TX28EQ28", typeof(int)); // ke9ns add

            t.Columns.Add("PEQ1", typeof(int));
            t.Columns.Add("PEQ2", typeof(int));
            t.Columns.Add("PEQ3", typeof(int));
            t.Columns.Add("PEQ4", typeof(int));
            t.Columns.Add("PEQ5", typeof(int));
            t.Columns.Add("PEQ6", typeof(int));
            t.Columns.Add("PEQ7", typeof(int));
            t.Columns.Add("PEQ8", typeof(int));
            t.Columns.Add("PEQ9", typeof(int));

            t.Columns.Add("PEQfreq1", typeof(int));
            t.Columns.Add("PEQfreq2", typeof(int));
            t.Columns.Add("PEQfreq3", typeof(int));
            t.Columns.Add("PEQfreq4", typeof(int));
            t.Columns.Add("PEQfreq5", typeof(int));
            t.Columns.Add("PEQfreq6", typeof(int));
            t.Columns.Add("PEQfreq7", typeof(int));
            t.Columns.Add("PEQfreq8", typeof(int));
            t.Columns.Add("PEQfreq9", typeof(int));

            t.Columns.Add("PEQoctave1", typeof(int));
            t.Columns.Add("PEQoctave2", typeof(int));
            t.Columns.Add("PEQoctave3", typeof(int));
            t.Columns.Add("PEQoctave4", typeof(int));
            t.Columns.Add("PEQoctave5", typeof(int));
            t.Columns.Add("PEQoctave6", typeof(int));
            t.Columns.Add("PEQoctave7", typeof(int));
            t.Columns.Add("PEQoctave8", typeof(int));
            t.Columns.Add("PEQoctave9", typeof(int));

            t.Columns.Add("RX1DSPMODE", typeof(DSPMode)); // .196
            t.Columns.Add("RX2DSPMODE", typeof(DSPMode)); // .196


            //==================================

            t.Columns.Add("VAC1_SelectA", typeof(bool)); //.279
            t.Columns.Add("VAC1_SelectB", typeof(bool)); //.279
            t.Columns.Add("VAC1_MixAudio", typeof(bool)); //.279
            t.Columns.Add("VAC1_Reset", typeof(bool)); //.279
           
            t.Columns.Add("Drive_Max", typeof(int)); //.279a


            //===============================================
            t.Columns.Add("DXOn", typeof(bool));
            t.Columns.Add("DXLevel", typeof(int));
            t.Columns.Add("CompanderOn", typeof(bool));
            t.Columns.Add("CompanderLevel", typeof(int));
            t.Columns.Add("MicGain", typeof(int));
            t.Columns.Add("FMMicGain", typeof(int));
            t.Columns.Add("Lev_On", typeof(bool));
            t.Columns.Add("Lev_Slope", typeof(int));
            t.Columns.Add("Lev_MaxGain", typeof(int));
            t.Columns.Add("Lev_Attack", typeof(int));
            t.Columns.Add("Lev_Decay", typeof(int));
            t.Columns.Add("Lev_Hang", typeof(int));
            t.Columns.Add("Lev_HangThreshold", typeof(int));
            t.Columns.Add("ALC_Slope", typeof(int));
            t.Columns.Add("ALC_MaxGain", typeof(int));
            t.Columns.Add("ALC_Attack", typeof(int));
            t.Columns.Add("ALC_Decay", typeof(int));
            t.Columns.Add("ALC_Hang", typeof(int));
            t.Columns.Add("ALC_HangThreshold", typeof(int));
            t.Columns.Add("Power", typeof(int));
            t.Columns.Add("Dexp_On", typeof(bool));
            t.Columns.Add("Dexp_Threshold", typeof(int));
            t.Columns.Add("Dexp_Attenuate", typeof(int));
            t.Columns.Add("VOX_On", typeof(bool));
            t.Columns.Add("VOX_Threshold", typeof(int));
            t.Columns.Add("VOX_HangTime", typeof(int));
            t.Columns.Add("Tune_Power", typeof(int));
            t.Columns.Add("Tune_Meter_Type", typeof(string));
            t.Columns.Add("TX_Limit_Slew", typeof(bool));
            t.Columns.Add("TXBlankingTime", typeof(int));
            t.Columns.Add("MicBoost", typeof(bool));
            t.Columns.Add("TX_AF_Level", typeof(int));
            t.Columns.Add("AM_Carrier_Level", typeof(int));
            t.Columns.Add("Show_TX_Filter", typeof(bool));
            t.Columns.Add("VAC1_On", typeof(bool));
            t.Columns.Add("VAC1_Auto_On", typeof(bool));
            t.Columns.Add("VAC1_RX_Gain", typeof(int));
            t.Columns.Add("VAC1_TX_Gain", typeof(int));
            t.Columns.Add("VAC1_Stereo_On", typeof(bool));
            t.Columns.Add("VAC1_Sample_Rate", typeof(string));
            t.Columns.Add("VAC1_Buffer_Size", typeof(string));
            t.Columns.Add("VAC1_IQ_Output", typeof(bool));
            t.Columns.Add("VAC1_IQ_Correct", typeof(bool));
            t.Columns.Add("VAC1_PTT_OverRide", typeof(bool));
            t.Columns.Add("VAC1_Combine_Input_Channels", typeof(bool));
            t.Columns.Add("VAC1_Latency_On", typeof(bool));
            t.Columns.Add("VAC1_Latency_Duration", typeof(int));
            t.Columns.Add("VAC2_On", typeof(bool));
            t.Columns.Add("VAC2_Auto_On", typeof(bool));
            t.Columns.Add("VAC2_RX_Gain", typeof(int));
            t.Columns.Add("VAC2_TX_Gain", typeof(int));
            t.Columns.Add("VAC2_Stereo_On", typeof(bool));
            t.Columns.Add("VAC2_Sample_Rate", typeof(string));
            t.Columns.Add("VAC2_Buffer_Size", typeof(string));
            t.Columns.Add("VAC2_IQ_Output", typeof(bool));
            t.Columns.Add("VAC2_IQ_Correct", typeof(bool));
            t.Columns.Add("VAC2_Combine_Input_Channels", typeof(bool));
            t.Columns.Add("VAC2_Latency_On", typeof(bool));
            t.Columns.Add("VAC2_Latency_Duration", typeof(int));
            t.Columns.Add("Phone_RX_DSP_Buffer", typeof(string));
            t.Columns.Add("Phone_TX_DSP_Buffer", typeof(string));
            t.Columns.Add("Digi_RX_DSP_Buffer", typeof(string));
            t.Columns.Add("Digi_TX_DSP_Buffer", typeof(string));
            t.Columns.Add("CW_RX_DSP_Buffer", typeof(string));
            t.Columns.Add("Mic_Input_On", typeof(string));
            t.Columns.Add("Mic_Input_Level", typeof(int));
            t.Columns.Add("Line_Input_On", typeof(string));
            t.Columns.Add("Line_Input_Level", typeof(int));
            t.Columns.Add("Balanced_Line_Input_On", typeof(string));
            t.Columns.Add("Balanced_Line_Input_Level", typeof(int));
            t.Columns.Add("FlexWire_Input_On", typeof(string));
            t.Columns.Add("FlexWire_Input_Level", typeof(int));

            #region Default

            DataRow dr = t.NewRow();
            dr["Name"] = "Default";
            dr["FilterLow"] = 70; // .261  200
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

         
            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a

            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region Default DX

            dr = t.NewRow();
            dr["Name"] = "Default DX";
            dr["FilterLow"] = 200;
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;


            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196
          
            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a

            //============================================

            dr["DXOn"] = true;
            dr["DXLevel"] = 5;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 5;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region FHM-1

            dr = t.NewRow();
            dr["Name"] = "FHM-1";
            dr["FilterLow"] = 70; //150
            dr["FilterHigh"] = 3050;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = -3;
            dr["TXEQ3"] = -7;
            dr["TXEQ4"] = -5;
            dr["TXEQ5"] = -3;
            dr["TXEQ6"] = 2;
            dr["TXEQ7"] = 6;
            dr["TXEQ8"] = 3;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = -6;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196


         
            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279
            dr["Drive_Max"] = 100; // .279a

            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 35;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 100;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = true;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "1024";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = -28;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 15;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region FHM-1 DX

            dr = t.NewRow();
            dr["Name"] = "FHM-1 DX";
            dr["FilterLow"] = 200; //300
            dr["FilterHigh"] = 2400;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = -3;
            dr["TXEQ3"] = -6;
            dr["TXEQ4"] = -10;
            dr["TXEQ5"] = -6;
            dr["TXEQ6"] = 3;
            dr["TXEQ7"] = 6;
            dr["TXEQ8"] = 3;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = -6;





            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

        
            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a

            //============================================
            dr["DXOn"] = true;
            dr["DXLevel"] = 5;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 35;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = true;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = -28;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 15;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region DIGI 1K@1500

            dr = t.NewRow();
            dr["Name"] = "Digi 1K@1500";
            dr["FilterLow"] = 1000;
            dr["FilterHigh"] = 2000;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

         
            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            
            dr["VAC1_Reset"] = false; //.279
            dr["Drive_Max"] = 100; // .279a

            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 0;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 0;
            dr["MicGain"] = 5;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = false;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = true;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region DIGI 1K@2210

            dr = t.NewRow();
            dr["Name"] = "Digi 1K@2210";
            dr["FilterLow"] = 1710;
            dr["FilterHigh"] = 2710;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

         
            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 0;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 0;
            dr["MicGain"] = 5;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = false;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = true;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region AM

            dr = t.NewRow();
            dr["Name"] = "AM";
            dr["FilterLow"] = 0;
            dr["FilterHigh"] = 4000;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;





            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region Conventional

            dr = t.NewRow();
            dr["Name"] = "Conventional";
            dr["FilterLow"] = 70; //100
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region D-104

            dr = t.NewRow();
            dr["Name"] = "D-104";
            dr["FilterLow"] = 70; // 100
            dr["FilterHigh"] = 3500;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = -6;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 7;
            dr["TXEQ2"] = 3;
            dr["TXEQ3"] = 4;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 5;
            dr["MicGain"] = 25;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region D-104+CPDR

            dr = t.NewRow();
            dr["Name"] = "D-104+CPDR";
            dr["FilterLow"] = 70; // 100
            dr["FilterHigh"] = 3500;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = -6;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 7;
            dr["TXEQ2"] = 3;
            dr["TXEQ3"] = 4;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 5;
            dr["MicGain"] = 20;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region D-104+EQ

            dr = t.NewRow();
            dr["Name"] = "D-104+EQ";
            dr["FilterLow"] = 70; //100
            dr["FilterHigh"] = 3500;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -6;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 7;
            dr["TXEQ2"] = 3;
            dr["TXEQ3"] = 4;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 5;
            dr["MicGain"] = 20;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region DX / Constest

            dr = t.NewRow();
            dr["Name"] = "DX / Contest";
            dr["FilterLow"] = 250;
            dr["FilterHigh"] = 3250;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;

            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = true;
            dr["DXLevel"] = 5;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region ESSB

            dr = t.NewRow();
            dr["Name"] = "ESSB";
            dr["FilterLow"] = 70;
            dr["FilterHigh"] = 4000;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = false;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region HC4-5

            dr = t.NewRow();
            dr["Name"] = "HC4-5";
            dr["FilterLow"] = 70; //100
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 5;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region HC4-5+CPDR

            dr = t.NewRow();
            dr["Name"] = "HC4-5+CPDR";
            dr["FilterLow"] = 70; //100
            dr["FilterHigh"] = 3100;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 5;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region PR40+W2IHY

            dr = t.NewRow();
            dr["Name"] = "PR40+W2IHY";
            dr["FilterLow"] = 50;
            dr["FilterHigh"] = 3650;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region PR40+W2IHY+CPDR

            dr = t.NewRow();
            dr["Name"] = "PR40+W2IHY+CPDR";
            dr["FilterLow"] = 50;
            dr["FilterHigh"] = 3650;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = false;
            dr["TXEQPreamp"] = 0;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = 0;
            dr["TXEQ2"] = 0;
            dr["TXEQ3"] = 0;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region PR781+EQ

            dr = t.NewRow();
            dr["Name"] = "PR781+EQ";
            dr["FilterLow"] = 100;
            dr["FilterHigh"] = 3200;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -11;
            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = -6;
            dr["TXEQ2"] = 2;
            dr["TXEQ3"] = 8;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;




            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================
            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = false;
            dr["CompanderLevel"] = 3;
            dr["MicGain"] = 12;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

            #region PR781+EQ+CPDR

            dr = t.NewRow();
            dr["Name"] = "PR781+EQ+CPDR";
            dr["FilterLow"] = 100;
            dr["FilterHigh"] = 3200;
            dr["TXEQNumBands"] = 3;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -9;

            dr["TXEQ28Preamp"] = 0; // ke9ns add
            dr["TXEQ9Preamp"] = 0; // ke9ns add

            dr["TXEQ28Band"] = false;  //   dr["TXEQ28Band"] = console.eqForm.rad28Band.Checked;
            dr["TXEQ10Band"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;
            dr["TXEQBand"] = false;   //  dr["TXEQBand"] = console.eqForm.rad10Band.Checked;

            dr["TXEQ9Band"] = false;   //  dr["TXEQ9Band"] = console.eqForm.radPEQ.Checked;
            dr["TXEQ37Band"] = false;  //  dr["TXEQ37Band"] = console.eqForm.chkBothEQ.Checked;

            dr["TXEQ1"] = -8;
            dr["TXEQ2"] = 3;
            dr["TXEQ3"] = 7;
            dr["TXEQ4"] = 0;
            dr["TXEQ5"] = 0;
            dr["TXEQ6"] = 0;
            dr["TXEQ7"] = 0;
            dr["TXEQ8"] = 0;
            dr["TXEQ9"] = 0;
            dr["TXEQ10"] = 0;



            dr["TX28EQ1"] = 0;
            dr["TX28EQ2"] = 0;
            dr["TX28EQ3"] = 0;
            dr["TX28EQ4"] = 0;
            dr["TX28EQ5"] = 0;
            dr["TX28EQ6"] = 0;
            dr["TX28EQ7"] = 0;
            dr["TX28EQ8"] = 0;
            dr["TX28EQ9"] = 0;
            dr["TX28EQ10"] = 0;
            dr["TX28EQ11"] = 0; // ke9ns add
            dr["TX28EQ12"] = 0; // ke9ns add
            dr["TX28EQ13"] = 0; // ke9ns add
            dr["TX28EQ14"] = 0; // ke9ns add
            dr["TX28EQ15"] = 0; // ke9ns add
            dr["TX28EQ16"] = 0; // ke9ns add
            dr["TX28EQ17"] = 0; // ke9ns add
            dr["TX28EQ18"] = 0; // ke9ns add
            dr["TX28EQ19"] = 0; // ke9ns add
            dr["TX28EQ20"] = 0; // ke9ns add
            dr["TX28EQ21"] = 0; // ke9ns add
            dr["TX28EQ22"] = 0; // ke9ns add
            dr["TX28EQ23"] = 0; // ke9ns add
            dr["TX28EQ24"] = 0; // ke9ns add
            dr["TX28EQ25"] = 0; // ke9ns add
            dr["TX28EQ26"] = 0; // ke9ns add
            dr["TX28EQ27"] = 0; // ke9ns add
            dr["TX28EQ28"] = 0; // ke9ns add

            dr["PEQ1"] = 0;
            dr["PEQ2"] = 0;
            dr["PEQ3"] = 0;
            dr["PEQ4"] = 0;
            dr["PEQ5"] = 0;
            dr["PEQ6"] = 0;
            dr["PEQ7"] = 0;
            dr["PEQ8"] = 0;
            dr["PEQ9"] = 0;

            dr["PEQfreq1"] = 30;
            dr["PEQfreq2"] = 63;
            dr["PEQfreq3"] = 125;
            dr["PEQfreq4"] = 250;
            dr["PEQfreq5"] = 500;
            dr["PEQfreq6"] = 1000;
            dr["PEQfreq7"] = 2000;
            dr["PEQfreq8"] = 4000;
            dr["PEQfreq9"] = 8000;

            dr["PEQoctave1"] = 10;
            dr["PEQoctave2"] = 10;
            dr["PEQoctave3"] = 10;
            dr["PEQoctave4"] = 10;
            dr["PEQoctave5"] = 10;
            dr["PEQoctave6"] = 10;
            dr["PEQoctave7"] = 10;
            dr["PEQoctave8"] = 10;
            dr["PEQoctave9"] = 10;

            dr["RX1DSPMODE"] = DSPMode.LSB; // .196
            dr["RX2DSPMODE"] = DSPMode.LSB; // .196

            dr["VAC1_SelectA"] = true; //.279
            dr["VAC1_SelectB"] = false; //.279
            dr["VAC1_MixAudio"] = false; //.279
            dr["VAC1_Reset"] = false; //.279

            dr["Drive_Max"] = 100; // .279a
            //============================================

            dr["DXOn"] = false;
            dr["DXLevel"] = 3;
            dr["CompanderOn"] = true;
            dr["CompanderLevel"] = 2;
            dr["MicGain"] = 10;
            dr["FMMicGain"] = 10;
            dr["Lev_On"] = true;
            dr["Lev_Slope"] = 0;
            dr["Lev_MaxGain"] = 5;
            dr["Lev_Attack"] = 2;
            dr["Lev_Decay"] = 500;
            dr["Lev_Hang"] = 500;
            dr["Lev_HangThreshold"] = 0;
            dr["ALC_Slope"] = 0;
            dr["ALC_MaxGain"] = -20;
            dr["ALC_Attack"] = 2;
            dr["ALC_Decay"] = 10;
            dr["ALC_Hang"] = 500;
            dr["ALC_HangThreshold"] = 0;
            dr["Power"] = 50;
            dr["Dexp_On"] = false;
            dr["Dexp_Threshold"] = -40;
            dr["Dexp_Attenuate"] = 80;
            dr["VOX_On"] = false;
            dr["VOX_Threshold"] = 100;
            dr["VOX_HangTime"] = 250;
            dr["Tune_Power"] = 10;
            dr["Tune_Meter_Type"] = "Fwd Pwr";
            dr["TX_Limit_Slew"] = false;
            dr["TXBlankingTime"] = 200;
            dr["MicBoost"] = false;
            dr["TX_AF_Level"] = 50;
            dr["AM_Carrier_Level"] = 25;
            dr["Show_TX_Filter"] = false;
            dr["VAC1_On"] = false;
            dr["VAC1_Auto_On"] = false;
            dr["VAC1_RX_GAIN"] = 0;
            dr["VAC1_TX_GAIN"] = 0;
            dr["VAC1_Stereo_On"] = false;
            dr["VAC1_Sample_Rate"] = "48000";
            dr["VAC1_Buffer_Size"] = "2048";
            dr["VAC1_IQ_Output"] = false;
            dr["VAC1_IQ_Correct"] = true;
            dr["VAC1_PTT_OverRide"] = true;
            dr["VAC1_Combine_Input_Channels"] = false;
            dr["VAC1_Latency_On"] = true;
            dr["VAC1_Latency_Duration"] = 120;
            dr["VAC2_On"] = false;
            dr["VAC2_Auto_On"] = false;
            dr["VAC2_RX_GAIN"] = 0;
            dr["VAC2_TX_GAIN"] = 0;
            dr["VAC2_Stereo_On"] = false;
            dr["VAC2_Sample_Rate"] = "48000";
            dr["VAC2_Buffer_Size"] = "2048";
            dr["VAC2_IQ_Output"] = false;
            dr["VAC2_IQ_Correct"] = true;
            dr["VAC2_Combine_Input_Channels"] = false;
            dr["VAC2_Latency_On"] = true;
            dr["VAC2_Latency_Duration"] = 120;
            dr["Phone_RX_DSP_Buffer"] = "2048";
            dr["Phone_TX_DSP_Buffer"] = "2048";
            dr["Digi_RX_DSP_Buffer"] = "2048";
            dr["Digi_TX_DSP_Buffer"] = "2048";
            dr["CW_RX_DSP_Buffer"] = "2048";
            switch (model)
            {
                case Model.FLEX5000:
                case Model.FLEX3000:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
                case Model.FLEX1500:
                    dr["Mic_Input_On"] = "1";
                    dr["Mic_Input_Level"] = 60;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 60;
                    break;
                default:
                    dr["Mic_Input_On"] = "0";
                    dr["Mic_Input_Level"] = 0;
                    dr["Line_Input_On"] = "0";
                    dr["Line_Input_Level"] = 0;
                    dr["Balanced_Line_Input_On"] = "0";
                    dr["Balanced_Line_Input_Level"] = 0;
                    dr["FlexWire_Input_On"] = "0";
                    dr["FlexWire_Input_Level"] = 0;
                    break;
            }

            t.Rows.Add(dr);

            #endregion

        } // AddTXProfileDefTable(Model model)

        private static void CheckBandTextValid()
        {
            ArrayList bad_rows = new ArrayList();

            if (ds == null) return;
            foreach (DataRow dr in ds.Tables["BandText"].Rows)
            {
                // check low freq
                string f = ((double)dr["Low"]).ToString("f6");
                f = f.Replace(",", ".");
                if (f.Contains(".") == false) f = f + ".0"; // ke9ns add

                DataRow[] rows = ds.Tables["BandText"].Select(f + ">=Low AND " + f + "<=High");
                if (rows.Length > 1)
                {
                    // handle multiple entries
                    if (!bad_rows.Contains(dr))
                        bad_rows.Add(dr);
                }

                // check high freq
                f = ((double)dr["High"]).ToString("f6");
                f = f.Replace(",", ".");
                if (f.Contains(".") == false) f = f + ".0"; // ke9ns add

                rows = ds.Tables["BandText"].Select(f + ">=Low AND " + f + "<=High");
                if (rows.Length > 1)
                {
                    // handle multiple entries
                    if (!bad_rows.Contains(dr))
                        bad_rows.Add(dr);
                }
            }

            foreach (DataRow dr in bad_rows)
                ds.Tables["BandText"].Rows.Remove(dr);
        }

        #endregion

        #endregion

        #region Public Member Functions
        // ======================================================
        // Public Member Functions 
        // ======================================================

        public static bool Init(Model model) // ke9ns first sets up a default FRSRegion.US, then in console.cs changes it
        {



            if (file_name.Contains("database_F") || file_name.Contains("database_D")) // ke9ns add  make sure your now looking at RevQ database only
            {
                file_name1 = file_name; // ke9ns use your original to copy into new RevQ database as starting point
                file_name = file_name.Replace("database_", "database-RevQ_");
            }

            string backup_filename1 = file_name.Remove(file_name.Length - 4) + "_sbu.xml";     // current session backup (sbu)
            string backup_filename2 = file_name.Remove(file_name.Length - 4) + "_bak1.xml";    // 1st gen BU, copy of SBU
            string backup_filename3 = file_name.Remove(file_name.Length - 4) + "_bak2.xml";    // 2nd gen BU, copy of bak
            string backup_filename4 = file_name.Remove(file_name.Length - 4) + "_bak3.xml";    // 3rd gen BU, copy of bak2

            bool database_exists = false;

            ds = new DataSet("Data");



            if (File.Exists(file_name))  // ke9ns mod  file_name is now looking for RevQ database first
            {
                //   Trace.WriteLine("reading RevQ database " + file_name);


                try
                {

                    ds.ReadXml(file_name);
                    database_exists = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The database schema is corrupted and unusable.  " +
                        "The database exception error was:\n\n" + ex.Message + "\n\n" +
                        "Auto database recovery using the most recent valid database backup will be attempted.",
                        "ERROR: Database is Unusable",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    string recovery_db = "";

                    if (File.Exists(backup_filename4)) recovery_db = backup_filename4;
                    if (File.Exists(backup_filename3)) recovery_db = backup_filename3;
                    if (File.Exists(backup_filename2)) recovery_db = backup_filename2;
                    if (File.Exists(backup_filename1)) recovery_db = backup_filename1;

                    try
                    {
                        ds.ReadXml(recovery_db);
                        database_exists = true;
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("A database backup does not exist or the backup database schema is corrupted.  " +
                            "The database exception error was:\n\n " + ex2.Message + "\n\n" +
                            "A new default database will be created.",
                            "ERROR: Database Backup is Unusable",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } // catch

            } // if file exists

            else // ke9ns add copy over your good original Flex database into RevQdatabase.xml so we dont touch the original just in case
            {

                //   Trace.WriteLine("Must copy original Database " + file_name1); // file_name1 is old database.xml file
                //  Trace.WriteLine("To new REVQ database " + file_name); // file_name1 is old database.xml file



                if (File.Exists(file_name1)) File.Copy(file_name1, file_name, true);  // ke9ns add   File.Copy(old, new)


                if (File.Exists(file_name))  // ke9ns mod  file_name is now looking for RevQ database first
                {
                    //   Trace.WriteLine("Now we have RevQ database " + file_name);

                    try
                    {

                        ds.ReadXml(file_name);
                        database_exists = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The database schema is corrupted and unusable.  " +
                            "The database exception error was:\n\n" + ex.Message + "\n\n" +
                            "Auto database recovery using the most recent valid database backup will be attempted.",
                            "ERROR: Database is Unusable",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        string recovery_db = "";

                        if (File.Exists(backup_filename4)) recovery_db = backup_filename4;
                        if (File.Exists(backup_filename3)) recovery_db = backup_filename3;
                        if (File.Exists(backup_filename2)) recovery_db = backup_filename2;
                        if (File.Exists(backup_filename1)) recovery_db = backup_filename1;

                        try
                        {
                            ds.ReadXml(recovery_db);
                            database_exists = true;
                        }
                        catch (Exception ex2)
                        {
                            MessageBox.Show("A database backup does not exist or the backup database schema is corrupted.  " +
                                "The database exception error was:\n\n " + ex2.Message + "\n\n" +
                                "A new default database will be created.",
                                "ERROR: Database Backup is Unusable",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } // catch

                    if (database_exists == true)
                    {
                        AddBandStackSWL(); // ke9ns add put in database just copied over from original
                    }

                } // if file exists
            } // file did not exist

            VerifyTables(model); // setup database

            CheckBandTextValid();

            if (database_exists == false) return database_exists;
            else
            {
                try
                {
                    // copy 2nd gen BU to create 3rd gen BU
                    if (File.Exists(backup_filename3)) File.Copy(backup_filename3, backup_filename4, true);

                    // copy 1st gen BU to create 2nd gen BU
                    if (File.Exists(backup_filename2)) File.Copy(backup_filename2, backup_filename3, true);

                    // copy SBU to create 1st gen BU
                    if (File.Exists(backup_filename1)) File.Copy(backup_filename1, backup_filename2, true);

                    // create SBU from the current validated database
                    ds.WriteXml(backup_filename1, XmlWriteMode.WriteSchema);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A database backup or copy operation failed.  " +
                        "The exception error was:\n\n" + ex.Message + "\n\n" +
                        "This will not adversly effect the operation of your radio.",
                        "ERROR: Database Backup Creation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return database_exists;

        } // init()

        public static void Update()  // ke9ns write database file
        {
            //  StreamWriter writer = new StreamWriter(@"C: \Users\RADIO\AppData\Roaming\FlexRadio Systems\PowerSDR v2.8.0\PowerDown_4of4.log"); //   // look for %userprofile%\AppData\Roaming\FlexRadio Systems\PowerSDR v2.8.0\
            //   writer.AutoFlush = true;

            //   writer.WriteLine("DB WRITE FILE: (1 of 1 jobs)");

            //   writer.WriteLine("1) Attempt to write DB file");

            try
            {

                ds.WriteXml(file_name, XmlWriteMode.WriteSchema); // ds.WriteXml(file_name, XmlWriteMode.WriteSchema);
                                                                  //  writer.WriteLine("1) Done");

            }
            catch (Exception ex)
            {
                //   writer.WriteLine("1) FAILURE to write file");

                MessageBox.Show("A database write to file operation failed.  " +
                    "The exception error was:\n\n" + ex.Message,
                    "ERROR: Database Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //  writer.Close();


        } // Update()

        public static void Exit()
        {
            Update();
            ds = null;
        }

        public static bool BandText(double freq, out string outStr)
        {
            try
            {
                outStr = "";
                string f = freq.ToString("f6");

                f = f.Replace(",", ".");
                if (f.Contains(".") == false) f = f + ".0"; // ke9ns add


                DataRow[] rows = ds.Tables["BandText"].Select(f + ">=Low AND " + f + "<=High");

                if (rows.Length == 0)       // band not found
                {
                    outStr = "Out of Band";
                    return false;
                }
                else if (rows.Length == 1)  // found band
                {
                    outStr = ((string)rows[0]["Name"]);
                    return (bool)rows[0]["TX"];
                }
                else //if(rows.Length > 1)	// this should never happen
                {
                    MessageBox.Show("Error reading BandInfo table.", "Database Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    outStr = "Error";
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n\n" + e.StackTrace, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                outStr = "Error";
                return false;
            }
        }





        public static int[] GetBandStackNum() // ke9ns mod for GEN SWL bands
        {
            string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
                                     "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
                                      "LMF","120M","90M","61M","49M","41M","31M","25M",
                                     "22M","19M","16M","14M","13M","11M",
                                     "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
                                     "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
                                     "VHF12", "VHF13" };

            int[] retvals = new int[band_list.Length];

            for (int i = 0; i < band_list.Length; i++)
            {
                string s = band_list[i];
                DataRow[] rows = ds.Tables["BandStack"].Select("'" + s + "' = BandName");
                retvals[i] = rows.Length;
            }

            return retvals;
        } //  GetBandStackNum




        //==================================================================================================
        public static bool GetBandStack(string band, int index, out string mode, out string filter, out double freq)
        {
            DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

            if (rows.Length == 0)
            {
                //MessageBox.Show("No Entries found for Band: "+ band, "No Entry Found",
                //	MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show("No Entries found for Band: " + band + " Adding this Freq to new list", "No Entry Found",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                mode = console.RX1DSPMode.ToString();
                filter = console.RX1Filter.ToString();
                freq = Math.Round(console.VFOAFreq, 6);

                AddBandStack(band, mode, filter, freq); // take current band, DSP mode, filter, and freq

                return true;

                //  mode = "";
                //	filter = "";
                //freq = 0.0f;
                //return false;

            }

            index = index % rows.Length;

            mode = (string)((DataRow)rows[index])["Mode"];
            filter = (string)((DataRow)rows[index])["Filter"];
            freq = (double)((DataRow)rows[index])["Freq"];


            return true;


        } //GetBandStack

        public static bool GetBandStack2(string band, int index, out string mode, out string filter, out double freq) //.209
        {
            DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

            if (rows.Length == 0)
            {

                MessageBox.Show("No Entries found for Band: " + band + " Adding this Freq to new list", "No Entry Found",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                mode = console.RX2DSPMode.ToString();
                filter = console.RX2Filter.ToString();
                freq = Math.Round(console.VFOBFreq, 6);

                AddBandStack(band, mode, filter, freq); // take current band, DSP mode, filter, and freq

                return true;

                //  mode = "";
                //	filter = "";
                //freq = 0.0f;
                //return false;

            }

            index = index % rows.Length;

            mode = (string)((DataRow)rows[index])["Mode"];
            filter = (string)((DataRow)rows[index])["Filter"];
            freq = (double)((DataRow)rows[index])["Freq"];


            return true;


        } //GetBandStack2


        /*
<BandStack>
    <BandName>160M</BandName>
    <Mode>CWL</Mode>
    <Filter>F5</Filter>
    <Freq>1.81</Freq>
</BandStack>

            //  console.MemoryList.List.Remove(console.MemoryList.List[dataGridView1.CurrentCell.RowIndex]);

*/
        // not used at this time
        public static void GetBandStack1(string band)
        {
            DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

            if (rows.Length == 0)
            {
                //   MessageBox.Show("NO Entries found for Band: " + band, "No Entry Found",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show("NO Entries found for Band: " + band + " Adding this Freq to new list", "No Entry Found",
                                       MessageBoxButtons.OK, MessageBoxIcon.Warning);

                AddBandStack(band, console.RX1DSPMode.ToString(), console.RX1Filter.ToString(), Math.Round(console.VFOAFreq, 6)); // take current band, DSP mode, filter, and freq

            }

        }


        //==================================================================================================
        // ke9ns add to delete the current bandstack entry (passed from stack.cs to console.cs)
        public static void PurgeBandStack(int index, string band, string mode, string filter, string freq2)
        {

            if (!ds.Tables.Contains("BandStack")) return;  // dont run in no bandstack data

            string temp = "Freq = '" + freq2 + "'";

            try
            {
                DataRow[] rows = ds.Tables["BandStack"].Select(temp);   // find the identical freq in the bandstack

                foreach (var row in rows)
                {
                    row.Delete();
                    break;               // if there is a dup thenjust delete the first occurance
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No Entries found to Delete for Band: " + band, "No Entry Found",
                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        } // PurgeBandStack()


        //====================================================================================================
        // ke9ns add  allows bubble sort routine in stack.cs to update bandstack without checking for dups
        public static void SortBandStack(string band, int index, string mode, string filter, double freq)
        {
            try
            {

                DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

                filter3 = Console.BandStackLock;

                index = index % rows.Length;

                DataRow d = (DataRow)rows[index];
                d["Mode"] = mode;
                d["Filter"] = filter;
                d["Freq"] = freq;

                //  Debug.WriteLine("=====BANDSTACK SORT====");
            }
            catch (Exception)
            {
                MessageBox.Show("problem found sorting entry for Band: " + band, "No Entry Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        } //Sortbandstack

        public static void SortBandStack2(string band, int index, string mode, string filter, double freq) // .209
        {
            try
            {

                DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

                filter3 = Console.BandStackLock2;

                index = index % rows.Length;

                DataRow d = (DataRow)rows[index];
                d["Mode"] = mode;
                d["Filter"] = filter;
                d["Freq"] = freq;

                //  Debug.WriteLine("=====BANDSTACK SORT====");
            }
            catch (Exception)
            {
                MessageBox.Show("problem found sorting entry for Band: " + band, "No Entry Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        } //Sortbandstack2


        //==================================================================================================
        public static void AddBandStack(string band, string mode, string filter, double freq)
        {
            DataRow dr = ds.Tables["BandStack"].NewRow();


            dr["BandName"] = band;
            dr["Mode"] = mode;
            dr["Filter"] = filter;
            dr["Freq"] = freq;
            ds.Tables["BandStack"].Rows.Add(dr);

        } // AddBandStack


        //==================================================================================================
        // ke9ns add
        public static void AddBandText(double freq, double freq1, string name, bool tx)
        {
            DataRow dr = ds.Tables["BandText"].NewRow();
            dr["Low"] = freq;
            dr["High"] = freq1;
            dr["Name"] = name;
            dr["TX"] = tx;
            ds.Tables["BandText"].Rows.Add(dr);
        }


        //===========================================================
        // ke9ns add
        public static void WWV25()
        {

            DataTable t = ds.Tables["BandText"];
            object[] data = {
                                2500000, 2500000, "WWV Time",                 false,
            };

            int i = 0;

            DataRow dr = t.NewRow();
            dr["Low"] = (double)data[i * 4 + 0];
            dr["High"] = (double)data[i * 4 + 1];
            dr["Name"] = (string)data[i * 4 + 2];
            dr["TX"] = (bool)data[i * 4 + 3];
            t.Rows.Add(dr);

        } // WWV25;

        public static int filter3 = 0;


        //====================================================================================================
        public static void SaveBandStack(string band, int index, string mode, string filter, double freq)
        {
            DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

            if (rows.Length == 0) return;

            foreach (DataRow datarow in rows)           // prevent duplicates
            {
                if ((string)datarow["BandName"] == band && (double)datarow["Freq"] == freq)
                {
                    datarow["Filter"] = filter;
                    datarow["Mode"] = mode;

                    Debug.WriteLine("savebandstack ==update filter === " + filter);
                    return;
                }
            }

            filter3 = Console.BandStackLock;

            Debug.WriteLine("database check for lock status= " + filter3);

            if (filter3 == 1) // ke9ns add (for bandstack locking)
            {
                Debug.WriteLine("====LOCKED DONT UPDATE THIS BANDSTACK FREQ===");
                return;
            }


            index = index % rows.Length;

            DataRow d = (DataRow)rows[index];
            d["Mode"] = mode;
            d["Filter"] = filter;
            d["Freq"] = freq;

            Debug.WriteLine("=====SAVEBANDSTACK====");

        } //savebandstack

        public static void SaveBandStack2(string band, int index, string mode, string filter, double freq) // .209
        {
            DataRow[] rows = ds.Tables["BandStack"].Select("'" + band + "' = BandName");

            if (rows.Length == 0) return;

            foreach (DataRow datarow in rows)           // prevent duplicates
            {
                if ((string)datarow["BandName"] == band && (double)datarow["Freq"] == freq)
                {
                    datarow["Filter"] = filter;
                    datarow["Mode"] = mode;

                    Debug.WriteLine("savebandstack2 ====upadte filter === " + filter);
                    return;
                }
            }

            filter3 = Console.BandStackLock2;

            Debug.WriteLine("database check for lock status= " + filter3);

            if (filter3 == 1) // ke9ns add (for bandstack locking)
            {
                Debug.WriteLine("====LOCKED DONT UPDATE THIS BANDSTACK FREQ===");
                return;
            }


            index = index % rows.Length;

            DataRow d = (DataRow)rows[index];
            d["Mode"] = mode;
            d["Filter"] = filter;
            d["Freq"] = freq;

            Debug.WriteLine("=====SAVEBANDSTACK====");

        } //savebandstack2


        //===============================================================================================================
        // This removes the notches from the state database so we can rewrite all of them without
        // having one that was previously deleted staying in the database
        public static void PurgeNotches()
        {
            // make sure there is a State table
            if (!ds.Tables.Contains("State")) return;

            // find all the notches and remove them
            var rows = ds.Tables["State"].Select("Key like 'notchlist*'");
            if (rows != null)
            {
                foreach (var row in rows)
                    row.Delete();
            }
        }


        //=================================================================================
        // ke9ns: SaveState() console_closing("State", ref a) to update state variables
        public static void SaveVars(string tableName, ref ArrayList list)
        {
            if (!ds.Tables.Contains(tableName)) AddFormTable(tableName);

            foreach (string s in list)
            {
                string[] vals = s.Split('/');

                if (vals.Length > 2)
                {
                    for (int i = 2; i < vals.Length; i++)
                    {
                        vals[1] += "/" + vals[i];
                    }
                }

                if (vals.Length <= 1) continue;  // skip it as no data was provided


                DataRow[] rows = ds.Tables[tableName].Select("Key = '" + vals[0] + "'");

                if (rows.Length == 0)   // name is not in list
                {
                    DataRow newRow = ds.Tables[tableName].NewRow();
                    newRow[0] = vals[0];
                    newRow[1] = vals[1];
                    ds.Tables[tableName].Rows.Add(newRow);
                }
                else if (rows.Length == 1)
                {
                    rows[0][1] = vals[1];
                }

            } //foreach


        } // SaveVars()

        public static ArrayList GetVars(string tableName)
        {
            ArrayList list = new ArrayList();
            if (!ds.Tables.Contains(tableName))
                return list;

            DataTable t = ds.Tables[tableName];

            for (int i = 0; i < t.Rows.Count; i++)
            {
                list.Add(t.Rows[i][0].ToString() + "/" + t.Rows[i][1].ToString());
            }

            return list;
        }

        public static bool ImportDatabase(string filename)
        {
            // if(!File.Exists(filename)) return false;

            DataSet file = new DataSet();

            try
            {
                file.ReadXml(filename);
            }
            catch (Exception)
            {
                return false;
            }

            ds = file;

            // Handle change of mode from FMN to just FM
            DataRow[] rows = ds.Tables["BandStack"].Select("Mode = 'FMN'");
            foreach (DataRow dr in rows)
                dr["Mode"] = "FM";

            return true;
        }


        //===============================================================================================
        //===============================================================================================
        //===============================================================================================

        public static void UpdateRegion(FRSRegion current_region)
        {
            //harmonize BandText and BandStack with radio region
            switch (current_region)
            {

                // ke9ns is this EU01 (UK,CH,Slov,France,Malta)?

                case FRSRegion.UK: // EU01
                case FRSRegion.Slovakia:
                case FRSRegion.France:


                    // if (current_region == FRSRegion.Spain_UK)
                    //    AddRegion1ABandStack(); // ke9ns mod
                    //  else 
                    if (bandstackrefresh == true) AddRegion1BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();

                    //  if (current_region == FRSRegion.Spain_UK)
                    //    AddRegion1ABandText60m(); // transmit region1
                    // else

                    //  AddRegion1BandText60m(); // non transmit region1
                    AddRegion1ABandText60m(); // TX

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();



                    break;

                case FRSRegion.Europe: // EU00
                    if (bandstackrefresh == true) AddRegion1ABandStack(); // ke9ns mod 
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();
                    AddRegion1ABandText60m(); // KE9NS add transmit

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddEUBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();

                    break;

                //  case FRSRegion.Switerland: // EU12
                case FRSRegion.ES_CH_FIN: // EU12
                                          //  case FRSRegion.Finland: // EU12


                    if (bandstackrefresh == true) AddRegion1ABandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddNetherlandsBandText160m();
                    AddRegion1BandText80m();

                    AddRegion1ABandText60m(); // ke9ns add transmit

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();

                    break;

                case FRSRegion.Italy: // EU10


                    //  AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1ABandStack(); // ke9ns mod
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddItalyBandText160m();
                    AddRegion1BandText80m();

                    AddRegion1ABandText60m(); // tx
                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddLatviaBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();

                    break;

                case FRSRegion.UK_Plus: // EU02
                    if (bandstackrefresh == true) AddUK_PlusBandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();
                    AddUK_PlusBandText60m();

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();


                    break;

                case FRSRegion.Norway: // EU03
                case FRSRegion.Denmark:
                    if (bandstackrefresh == true) AddRegion1BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();
                    AddNorwayBandText60m(); // 5.25 to 5.45 tx

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();

                    break;

                case FRSRegion.Latvia:// EU08

                    //  AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1ABandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();
                    AddRegion1BandText60m();  // no transmit
                                              //  AddRegion1ABandText60m();

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddLatviaBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();

                    break;

                case FRSRegion.Bulgaria: // EU07
                                         // AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1ABandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddBulgariaBandText160m();
                    AddRegion1BandText80m();
                    AddRegion1BandText60m(); // no TX
                                             //  AddRegion1ABandText60m();

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddBulgariaBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();

                    break;

                case FRSRegion.Greece: // EU09
                                       //  AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddBulgariaBandText160m();
                    AddRegion1BandText80m();
                    //  AddRegion1BandText60m(); // no TX
                    AddRegion1ABandText60m(); // tx

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddGreeceBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.Hungary: // EU05
                                        // AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1ABandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();

                    AddRegion1ABandText60m(); // tx

                    AddHungaryBandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.Netherlands: // EU13

                    if (bandstackrefresh == true) AddRegion1BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddNetherlandsBandText160m();
                    AddRegion1BandText80m();

                    //   AddRegion1BBandText60m(); // ke9ns add 5.35 to 5.45 TX
                    AddRegion1ABandText60m(); // KE9NS add transmit

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.Russia: // RUSS
                                       //  Debug.WriteLine("RUSSIA===============");

                    //  AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1ABandStack(); // 60m bandstack
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();

                    AddRegion1BandText60m(); // no tx
                                             //AddRegion1ABandText60m();

                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRussiaBandText12m();

                    AddRegion1BandText10m();
                    AddGreeceBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    //  AddRussiaBandText11m(); // ke9ns this was the original
                    AddBandRussiaTextSWB();

                    break;

                case FRSRegion.Sweden: // EU06
                    if (bandstackrefresh == true) AddSwedenBandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();
                    //  AddSwedenBandText60m(); // 5.


                    AddRegion1ABandText60m(); // ke9ns sweden now IARU 1 60m


                    AddRegion1BandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddRegion1BandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.Australia: // ke9ns add new


                    if (bandstackrefresh == true) AddBandAusStackTable(); // Ham bandstack (60m is same as IARU1)
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();

                    AddBandAusTextTable(); // Ham band text

                    AddBandTextSWB(); // short wave text

                    AddRegion1BandText60m(); // KE9NS add no transmit

                    break;

                case FRSRegion.IARU2: // ke9ns add was region_2


                    if (bandstackrefresh == true) AddBand2StackTable(); // Ham bandstack (60m is same as IARU1)
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();

                    AddBand2TextTable(); // Ham band text

                    AddBandTextSWB(); // short wave text

                    //   AddRegion1ABandText60m(); // KE9NS add transmit


                    break;


                case FRSRegion.IARU3: // ke9ns mod was region_3

                    if (bandstackrefresh == true) AddRegion3BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();

                    AddRegion3BandText160m();
                    AddRegion3BandText80m();
                    AddRegion3BandText60m();

                    AddRegion3BandText40m();
                    AddRegion3BandText30m();
                    AddRegion3BandText20m();
                    AddRegion3BandText17m();
                    AddRegion3BandText15m();
                    AddRegion3BandText12m();
                    AddRegion3BandText10m();
                    AddRegion3BandText6m();
                    AddRegion3BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.China: // similar to IARU3 but with 60m

                    if (bandstackrefresh == true) AddRegion3BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();

                    AddRegion3BandText160m();
                    AddRegion3BandText80m();

                    AddChinaBandText60m(); //

                    AddRegion3BandText40m();
                    AddRegion3BandText30m();
                    AddRegion3BandText20m();
                    AddRegion3BandText17m();
                    AddRegion3BandText15m();
                    AddRegion3BandText12m();
                    AddRegion3BandText10m();
                    AddRegion3BandText6m();
                    AddRegion3BandTextVHFplus();
                    AddBandTextSWB();
                    break;


                case FRSRegion.Japan:
                    if (bandstackrefresh == true) AddRegion3BandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddJapanBandText160m();
                    AddJapanBandText80m();
                    AddRegion3BandText60m(); // no transmit

                    //  AddRegion1BandText60m(); // no transmit

                    AddJapanBandTextEmergency();

                    AddJapanBandText40m();
                    AddRegion3BandText30m();
                    AddRegion3BandText20m();
                    AddRegion3BandText17m();
                    AddRegion3BandText15m();
                    AddRegion3BandText12m();
                    AddJapanBandText10m();
                    AddJapanBandText6m();
                    AddRegion3BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.Italy_Plus: // EU11
                                           //   AddRegion1BandStack();
                    if (bandstackrefresh == true) AddRegion1ABandStack();
                    if (bandstackrefresh == true) AddBandStackSWL(); // ke9ns add

                    ClearBandText();
                    AddItalyBandText160m();
                    AddRegion1BandText80m();

                    AddRegion1ABandText60m(); // tx

                    AddItalyPlusBandText40m();
                    AddRegion1BandText30m();
                    AddRegion1BandText20m();
                    AddRegion1BandText17m();
                    AddRegion1BandText15m();
                    AddRegion1BandText12m();
                    AddRegion1BandText10m();
                    AddLatviaBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();
                    break;
            } // switch

            CheckBandTextValid();
            Update();
        } // updateregion

        #endregion
    }
}
