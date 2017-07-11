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
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

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

        private static void VerifyTables(Model model)
		{
			if(!ds.Tables.Contains("BandText"))
				AddBandTextTable();

            if (!ds.Tables.Contains("BandStack"))
            {
                AddBandStackTable();
                AddBandStackSWL(); // ke9ns add
            }

			if(!ds.Tables.Contains("Memory"))
				AddMemoryTable();

			if(!ds.Tables.Contains("GroupList"))
				AddGroupListTable();

			if(!ds.Tables.Contains("TXProfile"))
				AddTXProfileTable(model);

			if(!ds.Tables.Contains("TXProfileDef"))
				AddTXProfileDefTable(model);

			Update();
		}

		private static void AddFormTable(string name)
		{
			ds.Tables.Add(name);
			ds.Tables[name].Columns.Add("Key", typeof(string));
			ds.Tables[name].Columns.Add("Value", typeof(string));
		}

        private static void AddBandTextSWB()
        {
            // SW Broadcast & Misc. Band Plan
            DataTable t = ds.Tables["BandText"];

            object[] data = {
                                2.500000, 2.500000, "WWV Time",					false,
								5.000000, 5.000000, "WWV Time",					false,
								10.000000, 10.000000, "WWV Time",				false,
								15.000000, 15.000000, "WWV Time",			    false,
								20.000000, 20.000000, "WWV Time",			    false,
                                25.000000, 25.000000, "WWV Time",               false,  // ke9ns add
                                3.330000, 3.330000, "CHU Time",                 false,
                                7.850000, 7.850000, "CHU Time",                 false,
                                14.670000, 14.670000, "CHU Time",               false,
                                4.996000, 4.996000, "RWM",                      false,
                                9.996000, 9.996000, "RWM",                     	false,
                                14.996000, 14.996000, "RWM",                   	false,
                                4.998000, 4.998000, "EBC",                     	false,
                                15.006000, 15.006000, "EBC",                    false,

								0.135700, 0.137799, "2.2kM CW & Narrow Band",   true, // ke9ns change
                                0.153000, 0.279000, "AM - Long Wave",			false,
                                0.415000, 0.471999, "Maritime Band",            false,
                                0.472000, 0.478999, "630M CW & Narrow Band",    true, // ke9ns change
                                0.479000, 0.526400, "Maritime Band",            false,
								0.530000, 1.710000, "Broadcast AM Med Wave",	false,
								2.300000, 2.495000, "120M Short Wave",			false,
								3.200000, 3.329999, "90M Short Wave",			false,
                                3.330001, 3.400000, "90M Short Wave",			false,
								4.750000, 4.995999, "61M Short Wave",			false,
                                4.996001, 4.997999, "61M Short Wave",			false,
                                4.998001, 4.999999, "61M Short Wave",			false,
                                5.000001, 5.060000, "61M Short Wave",			false,
								5.900000, 6.999999, "49M Short Wave",			false,
								7.300000, 7.350000, "41M Short Wave",			false,
								7.360000, 9.900000, "31M Short Wave",			false,
								11.600000, 12.100000, "25M Short Wave",			false,
								13.570000, 13.870000, "22M Short Wave",			false,
								15.100000, 15.800000, "19M Short Wave",			false,
								17.480000, 17.900000, "16M Short Wave",			false,
								18.900000, 19.020000, "15M Short Wave",			false,
								21.450000, 21.850000, "13M Short Wave",			false,

                                25.600000, 26.960000, "11M Short Wave",         false,

                                26.960001, 26.969999, "11M CB ch 1",              false,
                                26.970001, 26.979999, "11M CB ch 2",              false,
                                26.980001, 26.989999, "11M CB ch 3",              false,
                                26.990001, 26.999999, "11M CB RC",              false,
                                27.000001, 27.009999, "11M CB ch 4",              false,
                                27.010001, 27.019999, "11M CB ch 5",              false,
                                27.020001, 27.029999, "11M CB ch 6",              false,
                                27.030001, 27.039999, "11M CB ch 7",              false,
                                27.040001, 27.049999, "11M CB RC",              false,
                                27.050001, 27.059999, "11M CB ch 8",              false,
                                27.060001, 27.069999, "11M CB ch 9",              false,
                                27.070001, 27.079999, "11M CB ch 10",             false,
                                27.080001, 27.089999, "11M CB ch 11",             false,
                                27.090001, 27.099999, "11M CB RC",             false,
                                27.100001, 27.109999, "11M CB ch 12",             false,
                                27.110001, 27.119999, "11M CB ch 13",             false,
                                27.120001, 27.129999, "11M CB ch 14",             false,
                                27.130001, 27.139999, "11M CB ch 15",             false,
                                27.140001, 27.149999, "11M CB RC",             false,
                                27.150001, 27.159999, "11M CB ch 16",             false,
                                27.160001, 27.169999, "11M CB ch 17",             false,
                                27.170001, 27.179999, "11M CB ch 18",             false,
                                27.180001, 27.189999, "11M CB ch 19",             false,
                                27.190001, 27.199999, "11M CB RC",             false,
                                27.200001, 27.209999, "11M CB ch 20",             false,
                                27.210001, 27.219999, "11M CB ch 21",             false,
                                27.220001, 27.229999, "11M CB ch 22",             false,
                                27.250001, 27.259999, "11M CB ch 23",             false,
                                27.230001, 27.239999, "11M CB ch 24",             false,
                                27.240001, 27.249999, "11M CB ch 25",             false,
                                27.260001, 27.269999, "11M CB ch 26",             false,
                                27.270001, 27.279999, "11M CB ch 27",             false,
                                27.280001, 27.289999, "11M CB ch 28",             false,
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
                                2.500000, 2.500000, "WWV Time",                 false,
                                5.000000, 5.000000, "WWV Time",                 false,
                                10.000000, 10.000000, "WWV Time",               false,
                                15.000000, 15.000000, "WWV Time",               false,
                                20.000000, 20.000000, "WWV Time",               false,
                                25.000000, 25.000000, "WWV Time",               false,  // ke9ns add
                                3.330000, 3.330000, "CHU Time",                 false,
                                7.850000, 7.850000, "CHU Time",                 false,
                                14.670000, 14.670000, "CHU Time",               false,
                                4.996000, 4.996000, "RWM",                      false,
                                9.996000, 9.996000, "RWM",                      false,
                                14.996000, 14.996000, "RWM",                    false,
                                4.998000, 4.998000, "EBC",                      false,
                                15.006000, 15.006000, "EBC",                    false,

                                0.135700, 0.137799, "2.2kM CW & Narrow Band",   true, // ke9ns change
                                0.153000, 0.279000, "AM - Long Wave",           false,
                                0.415000, 0.471999, "Maritime Band",            false,
                                0.472000, 0.478999, "630M CW & Narrow Band",    true, // ke9ns change
                                0.479000, 0.526400, "Maritime Band",            false,
                                0.530000, 1.710000, "Broadcast AM Med Wave",    false,
                                2.300000, 2.495000, "120M Short Wave",          false,
                                3.200000, 3.329999, "90M Short Wave",           false,
                                3.330001, 3.400000, "90M Short Wave",           false,
                                4.750000, 4.995999, "61M Short Wave",           false,
                                4.996001, 4.997999, "61M Short Wave",           false,
                                4.998001, 4.999999, "61M Short Wave",           false,
                                5.000001, 5.060000, "61M Short Wave",           false,
                                5.900000, 6.999999, "49M Short Wave",           false,
                                7.300000, 7.350000, "41M Short Wave",           false,
                                7.360000, 9.900000, "31M Short Wave",           false,
                                11.600000, 12.100000, "25M Short Wave",         false,
                                13.570000, 13.870000, "22M Short Wave",         false,
                                15.100000, 15.800000, "19M Short Wave",         false,
                                17.480000, 17.900000, "16M Short Wave",         false,
                                18.900000, 19.020000, "15M Short Wave",         false,
                                21.450000, 21.850000, "13M Short Wave",         false,

                                25.600000, 26.960000, "11M Short Wave",         true,

                                26.960001, 26.969999, "11M CB ch 1",              true,
                                26.970001, 26.979999, "11M CB ch 2",              true,
                                26.980001, 26.989999, "11M CB ch 3",              true,
                                26.990001, 26.999999, "11M CB RC",              true,
                                27.000001, 27.009999, "11M CB ch 4",              true,
                                27.010001, 27.019999, "11M CB ch 5",              true,
                                27.020001, 27.029999, "11M CB ch 6",              true,
                                27.030001, 27.039999, "11M CB ch 7",              true,
                                27.040001, 27.049999, "11M CB RC",              true,
                                27.050001, 27.059999, "11M CB ch 8",              true,
                                27.060001, 27.069999, "11M CB ch 9",              true,
                                27.070001, 27.079999, "11M CB ch 10",             true,
                                27.080001, 27.089999, "11M CB ch 11",             true,
                                27.090001, 27.099999, "11M CB RC",             true,
                                27.100001, 27.109999, "11M CB ch 12",             true,
                                27.110001, 27.119999, "11M CB ch 13",             true,
                                27.120001, 27.129999, "11M CB ch 14",             true,
                                27.130001, 27.139999, "11M CB ch 15",             true,
                                27.140001, 27.149999, "11M CB RC",             true,
                                27.150001, 27.159999, "11M CB ch 16",             true,
                                27.160001, 27.169999, "11M CB ch 17",             true,
                                27.170001, 27.179999, "11M CB ch 18",             true,
                                27.180001, 27.189999, "11M CB ch 19",             true,
                                27.190001, 27.199999, "11M CB RC",             true,
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
								1.810000, 1.835999, "160M CW",	                true,
                                1.836000, 1.836000, "160M CW QRP",	            true,
                                1.836001, 1.837999, "160M CW",	                true,
								1.838000, 1.839999, "160M Narrow Band Modes",	true,
								1.840000, 1.842999, "160M All Modes & Digital",	true,
                                1.843000, 1.999999, "160M All Modes",	true,
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
								3.500000, 3.559999, "80M CW",	    			true,
                                3.560000, 3.560000, "80M CW QRP",	    		true,
                                3.560001, 3.579999, "80M CW",	    			true,
								3.580000, 3.599999, "80M Narrow Band Modes",	true,
								3.600000, 3.689999, "80M All Modes",			true,
                                3.690000, 3.690000, "80M SSB QRP",			    true,
                                3.690001, 3.759999, "80M All Modes",			true,
                                3.760000, 3.760000, "80M SSB Emergency",        true,
                                3.760001, 3.799999, "80M All Modes",			true,
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
        private static void AddRegion1ABandText60m() // Germany, Luxembourg, Belgium, spain, switzerland, Finland
        {
            DataTable t = ds.Tables["BandText"];
            Debug.WriteLine("EUROPE==============");

            object[] data = {
                                5.250000, 5.351499, "60M RX Only",              false,

                                5.351500, 5.353999, "60M 200hz Narrow Band Modes",    true,
                                5.354000, 5.356999, "60M USB Voice (UK CH 7)",  true,
                                5.357000, 5.359999, "60M USB Voice (US CH 3)",  true,
                                5.360000, 5.362999, "60M USB Voice",            true,
                                5.363000, 5.365999, "60M USB Voice (UK CH 8)",  true,
                                5.366000, 5.366500, "60M 20hz Narrow Band Modes",    true,

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

        //====================================================================================================================
        // ke9ns add CB 11m
        private static void AddBandText11m() // ke9ns add CB
        {
            DataTable t = ds.Tables["BandText"];
            Debug.WriteLine("11m==============");

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



        // ke9ns parts of region 1 that dont get the new 60m plan
        private static void AddRegion1BBandText60m() // Netherlands
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
        private static void AddRegion1BandText60m()
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
								7.000000, 7.029999, "40M CW",			    	true,
                                7.030000, 7.030000, "40M CW QRP",			    true,
                                7.030001, 7.034999, "40M CW",			    	true,
								7.035000, 7.039999, "40M Narrow Band Modes",	true,
								7.040000, 7.059999, "40M All Modes",			true,
                                7.060000, 7.060000, "40M SSB Emergency",        true,
                                7.060001, 7.089999, "40M All Modes",			true,
                                7.090000, 7.090000, "40M SSB QRP",              true,
                                7.090001, 7.199999, "40M All Modes",            true,
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

        private static void AddRegion1BandText30m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								10.100000, 10.115999, "30M CW",					true,
                                10.116000, 10.116000, "30M CW QRP",				true,
                                10.116001, 10.139999, "30M CW",					true,
                                10.140000, 10.149999, "30M Narrow Band Modes",	true,
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

        private static void AddRegion1BandText20m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								14.000000, 14.059999, "20M CW",		        	true,
                                14.060000, 14.060000, "20M CW QRP",		        true,
                                14.060001, 14.069999, "20M CW",		        	true,
								14.070000, 14.098999, "20M Narrow Band Modes",	true,
								14.099000, 14.099999, "20M Beacons",			true,
                                14.100000, 14.100000, "20M NCDXF Beacons",	    true,
                                14.100001, 14.100999, "20M Beacons",			true,
                                14.101000, 14.111999, "20M All Mode Digital",   true,
                                14.112000, 14.129999, "20M All Mode",           true,
                                14.130000, 14.130000, "20M Digital Voice",      true,
                                14.130001, 14.229999, "20M All Modes",          true,
                                14.230000, 14.230000, "20M SSTV",				true,
                                14.230001, 14.284999, "20M All Modes",          true,
                                14.285000, 14.285000, "20M SSB QRP",			true,
                                14.285001, 14.299999, "20M All Modes",          true,
                                14.300000, 14.300000, "20M SSB Emergency",      true,
                                14.300001, 14.349999, "20M All Modes",          true,
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

        private static void AddRegion1BandText17m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								18.068000, 18.085999, "17M CW",					true,
                                18.086000, 18.086000, "17M CW QRP",				true,
                                18.086001, 18.094999, "17M CW",					true,
								18.095000, 18.108999, "17M Narrow Band Modes",	true,
								18.109000, 18.109999, "17M Beacons",		    true,
                                18.110000, 18.110000, "17M NCDXF Beacons",	    true,
                                18.110001, 18.110499, "17M Beacons",		    true,
								18.110500, 18.159999, "17M All Modes",			true,
                                18.160000, 18.160000, "17M SSB Emergency",		true,
                                18.160001, 18.167999, "17M All Modes",			true,
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

        private static void AddRegion1BandText15m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								21.000000, 21.059999, "15M CW",			        true,
                                21.060000, 21.060000, "15M CW QRP",			    true,
                                21.060001, 21.069999, "15M CW",			        true,
								21.070000, 21.109999, "15M Narrow Band Modes",	true,
								21.110000, 21.119999, "15M Wide Band Digital",	true,
                                21.120000, 21.148999, "15M Narrow Band Modes",	true,
                                21.149000, 21.149999, "15M Beacons",		    true,
                                21.150000, 21.150000, "15M NCDXF Beacons",	    true,
                                21.150001, 21.150999, "15M Beacons",		    true,
								21.151000, 21.179999, "15M All Modes",			true,
                                21.180000, 21.180000, "15M Digital Voice",		true,
                                21.180001, 21.284999, "15M All Modes",			true,
                                21.285000, 21.285000, "15M SSB QRP",	    	true,
                                21.285001, 21.359999, "15M All Modes",			true,
                                21.360000, 21.360000, "15M SSB Emergency",      true,
                                21.360001, 21.449999, "15M All Modes",			true,
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

        private static void AddRegion1BandText12m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								24.890000, 24.905999, "12M CW",					true,
                                24.906000, 24.906000, "12M CW QRP",		        true,
                                24.906001, 24.914999, "12M CW",					true,
                                24.915000, 24.928999, "12M Narrow Band Modes",	true,
								24.929000, 24.929999, "12M Beacons",		    true,
                                24.930000, 24.930000, "12M NCDXF Beacons",	    true,
                                24.930001, 24.930999, "12M Beacons",		    true,
                                24.931000, 24.939999, "12M All Modes Digital",	true,
                                24.940000, 24.989999, "12M All Modes",			true,
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

        private static void AddRegion1BandText10m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								28.000000, 28.059999, "10M CW",					true,
                                28.060000, 28.060000, "10M CW QRP",	        	true,
                                28.060001, 28.069999, "10M CW",					true,
								28.070000, 28.189999, "10M Narrow Band Modes",	true,
								28.190000, 28.199999, "10M Beacons",			true,
                                28.200000, 28.200000, "10m NCDXF Beacons",       true,
                                28.200001, 28.224999, "10M Beacons",			true,
                                28.225000, 28.299999, "10M All Mode Beacons",	true,
                                28.300000, 28.319999, "10M All Mode Digital",	true,
                                28.320001, 28.329999, "10M All Modes",	    	true,
                                28.330000, 28.330000, "10M Digital Voice",	    true,
                                28.330001, 28.359999, "10M All Modes",	    	true,
                                28.360000, 28.360000, "10M SSB QRP",	        true,
                                28.360001, 28.679999, "10M All Modes",	    	true,
                                28.680000, 28.680000, "10M SSTV",	            true,
                                28.680001, 29.199999, "10M All Modes",	    	true,
                                29.200000, 29.299999, "10M FM Digital",         true,
                                29.300000, 29.509999, "10M FM Sat. Downlinks",  true,
                                29.510000, 29.519999, "10M Guard Channel",		true,
                                29.520000, 29.549999, "10M FM Simplex",         true,
                                29.550000, 29.559999, "10M Deadband",		    true,
                                29.560000, 29.589999, "10M Repeater Inputs",	true,
                                29.590000, 29.599999, "10M Deadband",			true,
                                29.600000, 29.600000, "10M FM Calling",         true,
                                29.600001, 29.609999, "10M Deadband",			true,
                                29.610000, 29.649999, "10M FM Simplex",         true,
                                29.650000, 29.659999, "10M Deadband",			true,
                                29.660000, 29.699999, "10M Repeater Outputs",	true,
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

        private static void AddRegion1BandText6m()
        {
            // 50.0 - 52.0 MHz
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								50.000000, 50.079999, "6M Beacon Sub-Band",		true,
								50.080000, 50.089999, "6M CW",					true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",			true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW & SSB",			true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",	true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.209999, "6M CW, SSB & Digital",	true,
                                50.210000, 50.249999, "6M Meteor Scatter",      true,
                                50.250000, 50.284999, "6M CW, SSB & Digital",	true,
                                50.285000, 50.285000, "6M PSK Calling",         true,
                                50.285001, 50.399999, "6M CW, SSB & Digital",	true,
                                50.400000, 50.400000, "6M WSPR Beacons",        true,
                                50.400001, 50.499999, "6M CW, SSB & Digital",	true,
                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 51.209999, "6M All Modes",           true,
                                51.210000, 51.389999, "6M FM Repeater Inputs",	true,
                                51.390000, 51.409999, "6M All Modes",           true,
                                51.410000, 51.509999, "6M FM Simplex",          true,
                                51.510000, 51.510000, "6M FM Calling",          true,
                                51.510001, 51.589999, "6M FM Simplex",          true,
                                51.590000, 51.809999, "6M All Modes",           true,
                                51.810000, 51.989999, "6M FM Repeater Ouputs",	true,
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
        }

        private static void AddRegion1BandText4m()
        {
            // 70.0 - 70.5 MHz
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								70.000000, 70.089999, "4M CW & Digital", 		true,
								70.090000, 70.099999, "4M Beacons",				true,
                                70.100000, 70.249999, "4M CW, SSB & Digital",   true,
                                70.250000, 70.250000, "4M CW & SSB Calling",    true,
                                70.250001, 70.259999, "4M AM & FM",		    	true,
                                70.260000, 70.260000, "4M AM & FM Calling",     true,
                                70.260001, 70.299999, "4M AM & FM",	            true,
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

        private static void AddRegion1BandTextVHFplus()
        {
            // IARU Region 1: 2M and above Band Plan
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								// 144 - 146 MHz
                                144.000000, 144.034999, "2M CW & SSB EME",		        true,
                                144.035000, 144.049999, "2M CW",                        true,
                                144.050000, 144.005000, "2M CW Calling",                true,
                                144.050001, 144.149999, "2M CW",                        true,
                                144.150000, 144.299999, "2M SSB",                       true,
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
                                // 430 - 440 MHz
								430.000000, 430.024999, "70cm Sub-Regional",	        true,
                                430.025000, 430.374999, "70cm Repeater Outputs",	    true,
                                430.375000, 430.399999, "70cm Sub-Regional",	        true,
                                430.400000, 430.574999, "70cm FM Digital Link",	        true,
                                430.575000, 430.599999, "70cm Sub-Regional",	        true,
                                430.600000, 430.924999, "70cm FM Digital Repeater",	    true,
                                430.925000, 431.024999, "70cm Multimode Channels",	    true,
                                431.025000, 431.049999, "70cm Sub-Regional",	        true,
                                431.050000, 431.974999, "70cm Repeater Inputs",	        true,
                                431.975000, 431.999999, "70cm Sub-Regional",	        true,
                                432.000000, 432.024999, "70cm CW EME",                  true,
                                432.025000, 432.049999, "70cm CW",                      true,
                                432.050000, 432.050000, "70cm CW Calling",              true,
                                432.050001, 432.149999, "70cm CW",                      true,
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
								1240.000000, 1240.999999, "23cm All Modes, Digital",	true,
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
								2300.000000, 2303.999999, "13cm Sub-Regional",	        true,
                                2304.000000, 2305.999999, "13cm Narrow Band ",	        true,
                                2306.000000, 2307.999999, "13cm Sub-Regional",	        true,
                                2308.000000, 2309.999999, "13cm Narrow Band ",	        true,
                                2310.000000, 2319.999999, "13cm Sub-Regional",	        true,
                                2320.000000, 2320.024999, "13cm CW EME",	            true,
                                2320.025000, 2320.149999, "13cm CW",	                true,
                                2320.150000, 2320.199999, "13cm CW & SSB",	            true,
                                2320.200000, 2320.200000, "13cm SSB Calling",	        true,
                                2320.200001, 2320.799999, "13cm CW & SSB",	            true,
                                2320.800000, 2320.999999, "13cm Beacons",	            true,
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
								1.810000, 1.835999, "160M CW",	                true,
                                1.836000, 1.836000, "160M CW QRP",	            true,
                                1.836001, 1.837999, "160M CW",	                true,
								1.838000, 1.839999, "160M Narrow Band Modes",	true,
								1.840000, 1.849999, "160M All Modes & Digital",	true,
                                1.850000, 1.999999, "160M General RX",	        false,
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
								1.810000, 1.829999, "160M General RX",	        true,
                                1.830000, 1.836000, "160M CW QRP",	            true,
                                1.836001, 1.837999, "160M CW",	                true,
								1.838000, 1.839999, "160M Narrow Band Modes",	true,
								1.840000, 1.849999, "160M All Modes & Digital",	true,
                                1.850000, 1.999999, "160M General RX",	        false,
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
								1.810000, 1.835999, "160M CW",	                true,
                                1.836000, 1.836000, "160M CW QRP",	            true,
                                1.836001, 1.837999, "160M CW",	                true,
								1.838000, 1.839999, "160M Narrow Band Modes",	true,
								1.840000, 1.879999, "160M All Modes & Digital",	true,
                                1.880000, 1.999999, "160M General RX",	        false,
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
								5.258500, 5.264000, "60M Band Segment 1",		true,

                                5.264001, 5.275999, "60M Band",                 false,
								5.276000, 5.284000, "60M Band Segment 2",		true,

                                5.284001, 5.288499, "60M Band",                 false,
								5.288500, 5.292000, "60M Band Segment 3",		true,

                                5.292001, 5.297999, "60M Band",                 false,
                                5.298000, 5.307000, "60M Band Segment 4",		true,

                                5.307001, 5.312999, "60M Band",                 false,
								5.313000, 5.323000, "60M Band Segment 5",		true,

                                5.323001, 5.332999, "60M Band",                 false,
								5.333000, 5.338000, "60M Band Segment 6",		true,

                                5.338001, 5.353999, "60M Band",                 false,
								5.354000, 5.358000, "60M Band Segment 7 (IARU 1)",	true,

                                5.358001, 5.361999, "60M Band",                 false,
                                5.362000, 5.362999, "60M Band Segment 8",		true,
                                5.363000, 5.365999, "60M Band Segment 8 (IARU 1)",		true,
                                5.366000, 5.374500, "60M Band Segment 8",		true,

                                5.374501, 5.377999, "60M Band",                 false,                                
                                5.378000, 5.382000, "60M Band Segment 9",		true,

                                5.382001, 5.394999, "60M Band",                 false,
                                5.395000, 5.401500, "60M Band Segment 10",		true,

                                5.401501, 5.403499, "60M Band",                 false,
                                5.403500, 5.406500, "60M Band Segment 11",		true,

                                5.406501, 5.449999, "60M Band",                 false,

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
								5.310000, 5.313000, "60M Band Segment 1",		true,

                                5.313001, 5.319999, "60M Band",                 false,
								5.320000, 5.323000, "60M Band Segment 2",		true,

                                5.323001, 5.379999, "60M Band",                 false,
								5.380000, 5.383000, "60M Band Segment 3",		true,

                                5.383001, 5.389999, "60M Band",                 false,
                                5.390000, 5.393000, "60M Band Segment 4",		true, 
                                                               
                                5.393001, 5.449999, "60M Band",                 false,
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
								7.000000, 7.029999, "40M CW",			    	true,
                                7.030000, 7.030000, "40M CW QRP",			    true,
                                7.030001, 7.034999, "40M CW",			    	true,
								7.035000, 7.039999, "40M Narrow Band Modes",	true,
								7.040000, 7.059999, "40M All Modes",			true,
                                7.060000, 7.060000, "40M SSB Emergency",        true,
                                7.060001, 7.089999, "40M All Modes",			true,
                                7.090000, 7.090000, "40M SSB QRP",              true,
                                7.090001, 7.099999, "40M All Modes",            true,
                                7.100000, 7.199999, "40M General RX",           false,
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
								6.975000, 7.029999, "40M CW",			    	true,
                                7.030000, 7.030000, "40M CW QRP",			    true,
                                7.030001, 7.034999, "40M CW",			    	true,
								7.035000, 7.039999, "40M Narrow Band Modes",	true,
								7.040000, 7.059999, "40M All Modes",			true,
                                7.060000, 7.060000, "40M SSB Emergency",        true,
                                7.060001, 7.089999, "40M All Modes",			true,
                                7.090000, 7.090000, "40M SSB QRP",              true,
                                7.090001, 7.199999, "40M All Modes",            true,
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
								24.890000, 24.905999, "12M CW",					true,
                                24.906000, 24.906000, "12M CW QRP",		        true,
                                24.906001, 24.914999, "12M CW",					true,
                                24.915000, 24.928999, "12M Narrow Band Modes",	true,
								24.929000, 24.930999, "12M Beacons",		    true,
                                24.931000, 24.939999, "12M All Modes Digital",	true,
                                24.940000, 25.139999, "12M All Modes",			true,
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
								26.970000, 27.860000, "11M Citizens Band",		true,
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
								50.000000, 50.079999, "6M Beacon Sub-Band",		false,
								50.080000, 50.089999, "6M CW",					true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",			true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW, SSB & Digital",	true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",	true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.249999, "6M CW, SSB & Digital",	true,
                                50.250000, 50.250000, "6M PSK Calling",         true,
                                50.250001, 50.499999, "6M CW, SSB & Digital",	true,
                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 50.999999, "6M All Modes",           true,
                                51.000000, 51.999999, "6M General RX",	        false,
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
								50.000000, 50.079999, "6M Beacon Sub-Band",		true,
								50.080000, 50.089999, "6M CW",					true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",			true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW, SSB & Digital",	true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",	true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.249999, "6M CW, SSB & Digital",	true,
                                50.250000, 50.250000, "6M PSK Calling",         true,
                                50.250001, 50.499999, "6M CW, SSB & Digital",	true,
                                50.500000, 50.619999, "6M All Modes",           true,
                                50.620000, 50.749999, "6M Digital Comms.",      true,
                                50.750000, 50.999999, "6M All Modes",           true,
                                51.000000, 51.999999, "6M General RX",	        false,
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
								50.000000, 50.049999, "6M Beacon Sub-Band RX",	false,
                                50.050000, 50.079999, "6M Beacon Sub-Band",		true,
								50.080000, 50.089999, "6M CW",					true,
                                50.090000, 50.090000, "6M CW Calling",          true,
                                50.090001, 50.099999, "6M CW",                  true,
                                50.100000, 50.109999, "6M CW & SSB",			true,
                                50.110000, 50.110000, "6M SSB DX Calling",      true,
                                50.110001, 50.129999, "6M CW, SSB & Digital",	true,
                                50.130000, 50.149999, "6M CW, SSB & Digital",	true,
                                50.150000, 50.150000, "6M SSB Calling",         true,
                                50.150001, 50.249999, "6M CW, SSB & Digital",	true,
                                50.250000, 50.250000, "6M PSK Calling",         true,
                                50.250001, 50.499999, "6M CW, SSB & Digital",	true,
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
								50.000000, 51.999999, "6M General RX",	false,
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

        #region IARU Region 2 BandText

        private static void AddBandTextTable()  // Default bandtext - US Region 2
        {            
            ds.Tables.Add("BandText");
            DataTable t = ds.Tables["BandText"];

            Debug.WriteLine("USA==============");

            t.Columns.Add("Low", typeof(double));
            t.Columns.Add("High", typeof(double));
            t.Columns.Add("Name", typeof(string));
            t.Columns.Add("TX", typeof(bool));

            object[] data = {
								1.800000, 1.809999, "160M CW/Digital Modes",	true,
								1.810000, 1.810000, "160M CW QRP",				true,
								1.810001, 1.842999, "160M CW",					true,
								1.843000, 1.909999, "160M SSB/SSTV/Wide Band",	true,
								1.910000, 1.910000, "160M SSB QRP",				true,
								1.910001, 1.994999, "160M SSB/SSTV/Wide Band",	true,
								1.995000, 1.999999, "160M Experimental",		true,

								3.500000, 3.524999, "80M Extra CW",				true,
								3.525000, 3.579999, "80M CW",					true,
								3.580000, 3.589999, "80M RTTY",					true,
								3.590000, 3.590000, "80M RTTY DX",				true,
								3.590001, 3.599999, "80M RTTY",					true,
								3.600000, 3.699999, "75M Extra SSB",			true,
								3.700000, 3.789999, "75M Ext/Adv SSB",			true,
								3.790000, 3.799999, "75M Ext/Adv DX Window",	true,
								3.800000, 3.844999, "75M SSB",					true,
								3.845000, 3.845000, "75M SSTV",					true,
								3.845001, 3.884999, "75M SSB",					true,
								3.885000, 3.885000, "75M AM Calling Frequency", true,
								3.885001, 3.999999, "75M SSB",					true,

                                5.167500, 5.167500, "60M Emergency Channel",            true, // ke9ns add

                                5.250000, 5.331999, "60M General",              false,
								5.332000, 5.332000, "60M Channel 1",			true,
                                5.332001, 5.347999, "60M General",              false,
								5.348000, 5.348000, "60M Channel 2",			true,
                                5.348001, 5.358499, "60M General",              false,
								5.358500, 5.358500, "60M Channel 3 (IARU 1)",			true,
                                5.358501, 5.372999, "60M General",              false,
								5.373000, 5.373000, "60M Channel 4",			true,
                                5.373001, 5.404999, "60M General",              false,
								5.405000, 5.405000, "60M Channel 5",			true,
                                5.405001, 5.449999, "60M General",              false,
								
								7.000000, 7.024999, "40M Extra CW",				true, // ke9ns mod
								7.025000, 7.039999, "40M CW",					true,
								7.040000, 7.040000, "40M RTTY DX",				true,
								7.040001, 7.099999, "40M RTTY",					true,
								7.100000, 7.124999, "40M CW",					true,
								7.125000, 7.170999, "40M Ext/Adv SSB",			true,
								7.171000, 7.171000, "40M SSTV",					true,
								7.171001, 7.174999, "40M Ext/Adv SSB",			true,
								7.175000, 7.289999, "40M SSB",					true,
								7.290000, 7.290000, "40M AM Calling Frequency", true,
								7.290001, 7.299999, "40M SSB",					true,
								
								10.100000, 10.129999, "30M CW",					true,
								10.130000, 10.139999, "30M RTTY",				true,
								10.140000, 10.149999, "30M Packet",				true,

								14.000000, 14.024999, "20M Extra CW",			true,
								14.025000, 14.069999, "20M CW",					true,
								14.070000, 14.094999, "20M RTTY",				true,
								14.095000, 14.099499, "20M Packet",				true,
								14.099500, 14.099999, "20M CW",					true,
								14.100000, 14.100000, "20M NCDXF Beacons",	    true,
								14.100001, 14.100499, "20M CW",					true,
								14.100500, 14.111999, "20M Packet",				true,
								14.112000, 14.149999, "20M CW",					true,
								14.150000, 14.174999, "20M Extra SSB",			true,
								14.175000, 14.224999, "20M Ext/Adv SSB",		true,
								14.225000, 14.229999, "20M SSB",				true,
								14.230000, 14.230000, "20M SSTV",				true,
								14.230001, 14.285999, "20M SSB",				true,
								14.286000, 14.286000, "20M AM Calling Frequency", true,
								14.286001, 14.349999, "20M SSB",				true,
								
								18.068000, 18.099999, "17M CW",					true,
								18.100000, 18.104999, "17M RTTY",				true,
								18.105000, 18.109999, "17M Packet",				true,
								18.110000, 18.110000, "17M NCDXF Beacons",	    true,
								18.110001, 18.167999, "17M SSB",				true,
								
								21.000000, 21.024999, "15M Extra CW",			true,
								21.025000, 21.069999, "15M CW",					true,
								21.070000, 21.099999, "15M RTTY",				true,
								21.100000, 21.109999, "15M Packet",				true,
								21.110000, 21.149999, "15M CW",					true,
								21.150000, 21.150000, "15M NCDXF Beacons",	    true,
								21.150001, 21.199999, "15M CW",					true,
								21.200000, 21.224999, "15M Extra SSB",			true,
								21.225000, 21.274999, "15M Ext/Adv SSB",		true,
								21.275000, 21.339999, "15M SSB",				true,
								21.340000, 21.340000, "15M SSTV",				true,
								21.340001, 21.449999, "15M SSB",				true,
								
								24.890000, 24.919999, "12M CW",					true,
								24.920000, 24.924999, "12M RTTY",				true,
								24.925000, 24.929999, "12M Packet",				true,
								24.930000, 24.930000, "12M NCDXF Beacons",	    true,
								24.930001, 24.989999, "12M SSB",				true,
								
								28.000000, 28.069999, "10M CW",					true,
								28.070000, 28.149999, "10M RTTY",				true,
								28.150000, 28.199999, "10M CW",					true,
								28.200000, 28.200000, "10M NCDXF Beacons",	    true,
								28.200001, 28.299999, "10M Beacons",			true,
								28.300000, 28.679999, "10M SSB",				true,
								28.680000, 28.680000, "10M SSTV",				true,
								28.680001, 28.999999, "10M SSB",				true,
								29.000000, 29.199999, "10M AM",					true,
								29.200000, 29.299999, "10M SSB",				true,
								29.300000, 29.509999, "10M Satellite Downlinks", true,
								29.510000, 29.519999, "10M Deadband",			true,
								29.520000, 29.589999, "10M Repeater Inputs",	true,
								29.590000, 29.599999, "10M Deadband",			true,
								29.600000, 29.600000, "10M FM Simplex",			true,
								29.600001, 29.609999, "10M Deadband",			true,
								29.610000, 29.699999, "10M Repeater Outputs",	true,
								
								50.000000, 50.059999, "6M CW",					true,
								50.060000, 50.079999, "6M Beacon Sub-Band",		true,
								50.080000, 50.099999, "6M CW",					true,
								50.100000, 50.124999, "6M DX Window",			true,
								50.125000, 50.125000, "6M Calling Frequency",	true,
								50.125001, 50.299999, "6M SSB",					true,
								50.300000, 50.599999, "6M All Modes",			true,
								50.600000, 50.619999, "6M Non Voice",			true,
								50.620000, 50.620000, "6M Digital Packet",      true,
								50.620001, 50.799999, "6M Non Voice",			true,
								50.800000, 50.999999, "6M RC",					true,
								51.000000, 51.099999, "6M Pacific DX Window",	true,
								51.100000, 51.119999, "6M Deadband",			true,
								51.120000, 51.179999, "6M Digital Repeater Inputs", true,
								51.180000, 51.479999, "6M Repeater Inputs",		true,
								51.480000, 51.619999, "6M Deadband",			true,
								51.620000, 51.679999, "6M Digital Repeater Outputs", true,
								51.680000, 51.979999, "6M Repeater Outputs",	true,
								51.980000, 51.999999, "6M Deadband",			true,
								52.000000, 52.019999, "6M Repeater Inputs",		true,
								52.020000, 52.020000, "6M FM Simplex",			true,
								52.020001, 52.039999, "6M Repeater Inputs",		true,
								52.040000, 52.040000, "6M FM Simplex",			true,
								52.040001, 52.479999, "6M Repeater Inputs",		true,
								52.480000, 52.499999, "6M Deadband",			true,
								52.500000, 52.524999, "6M Repeater Outputs",	true,
								52.525000, 52.525000, "6M Primary FM Simplex",	true,
								52.525001, 52.539999, "6M Deadband",			true,
								52.540000, 52.540000, "6M Secondary FM Simplex", true,
								52.540001, 52.979999, "6M Repeater Outputs",	true,
								52.980000, 52.999999, "6M Deadbands",			true,
								53.000000, 53.000000, "6M Remote Base FM Spx",	true,
								53.000001, 53.019999, "6M Repeater Inputs",		true,
								53.020000, 53.020000, "6M FM Simplex",			true,
								53.020001, 53.479999, "6M Repeater Inputs",		true,
								53.480000, 53.499999, "6M Deadband",			true,
								53.500000, 53.519999, "6M Repeater Outputs",	true,
								53.520000, 53.520000, "6M FM Simplex",			true,
								53.520001, 53.899999, "6M Repeater Outputs",	true,
								53.900000, 53.900000, "6M FM Simplex",			true,
								53.900010, 53.979999, "6M Repeater Outputs",	true,
								53.980000, 53.999999, "6M Deadband",			true,
								
								144.000000, 144.099999, "2M CW",				true,
								144.100000, 144.199999, "2M CW/SSB",			true,
								144.200000, 144.200000, "2M Calling",			true,
								144.200001, 144.274999, "2M CW/SSB",			true,
								144.275000, 144.299999, "2M Beacon Sub-Band",	true,
								144.300000, 144.499999, "2M Satellite",			true,
								144.500000, 144.599999, "2M Linear Translator Inputs", true,
								144.600000, 144.899999, "2M FM Repeater",		true,
								144.900000, 145.199999, "2M FM Simplex",		true,
								145.200000, 145.499999, "2M FM Repeater",		true,
								145.500000, 145.799999, "2M FM Simplex",		true,
								145.800000, 145.999999, "2M Satellite",			true,
								146.000000, 146.399999, "2M FM Repeater",		true,
								146.400000, 146.609999, "2M FM Simplex",		true,
								146.610000, 147.389999, "2M FM Repeater",		true,
								147.390000, 147.599999, "2M FM Simplex",		true,
								147.600000, 147.999999, "2M FM Repeater",		true,

								222.000000, 222.024999, "1.25M EME/Weak Signal",        true,
								222.025000, 222.049999, "1.25M Weak Signal",	        true,
								222.050000, 222.059999, "1.25M Propagation Beacons",    true,
								222.060000, 222.099999, "1.25M Weak Signal",	        true,
								222.100000, 222.100000, "1.25M SSB/CW Calling",	        true,
								222.100001, 222.149999, "1.25M Weak Signal CW/SSB",     true,
								222.150000, 222.249999, "1.25M Local Option",	        true,
								222.250000, 223.380000, "1.25M FM Repeater Inputs",     true,
								223.380001, 223.399999, "1.25M General",                true,
								223.400000, 223.519999, "1.25M FM Simplex",		        true,
								223.520000, 223.639999, "1.25M Digital/Packet",	        true,
								223.640000, 223.700000, "1.25M Links/Control",	        true,
								223.700001, 223.709999, "1.25M General",	            true,
								223.710000, 223.849999, "1.25M Local Option",	        true,
								223.850000, 224.980000, "1.25M Repeater Outputs",       true,

								420.000000, 425.999999, "70cm ATV Repeater",	true,
								426.000000, 431.999999, "70cm ATV Simplex",		true,
								432.000000, 432.069999, "70cm EME",				true,
								432.070000, 432.099999, "70cm Weak Signal CW",	true,
								432.100000, 432.100000, "70cm Calling Frequency", true,
								432.100001, 432.299999, "70cm Mixed Mode Weak Signal", true,
								432.300000, 432.399999, "70cm Propagation Beacons", true,
								432.400000, 432.999999, "70cm Mixed Mode Weak Signal", true,
								433.000000, 434.999999, "70cm Auxillary/Repeater Links", true,
								435.000000, 437.999999, "70cm Satellite Only",	true,
								438.000000, 441.999999, "70cm ATV Repeater",	true,
								442.000000, 444.999999, "70cm Local Repeaters",	true,
								445.000000, 445.999999, "70cm Local Option",	true,
								446.000000, 446.000000, "70cm Simplex",			true,
								446.000001, 446.999999, "70cm Local Option",	true,
								447.000000, 450.000000, "70cm Local Repeaters", true,

								902.000000, 902.099999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
								902.100000, 902.100000, "33cm Weak Signal Calling", true,
								902.100001, 902.799999, "33cm Weak Signal SSTV/FAX/ACSSB", true,
								902.800000, 902.999999, "33cm Weak Signal EME/CW", true,
								903.000000, 903.099999, "33cm Digital Modes",	true,
								903.100000, 903.100000, "33cm Alternate Calling", true,
								903.100001, 905.999999, "33cm Digital Modes",	true,
								906.000000, 908.999999, "33cm FM Repeater Inputs", true,
								909.000000, 914.999999, "33cm ATV",				true,
								915.000000, 917.999999, "33cm Digital Modes",	true,
								918.000000, 920.999999, "33cm FM Repeater Outputs", true,
								921.000000, 926.999999, "33cm ATV",				true,
								927.000000, 928.000000, "33cm FM Simplex/Links", true,
								
								1240.000000, 1245.999999, "23cm ATV #1",		true,
								1246.000000, 1251.999999, "23cm FM Point/Links", true,
								1252.000000, 1257.999999, "23cm ATV #2, Digital Modes", true,
								1258.000000, 1259.999999, "23cm FM Point/Links", true,
								1260.000000, 1269.999999, "23cm Sat Uplinks/Wideband Exp.", true,
								1270.000000, 1275.999999, "23cm Repeater Inputs", true,
								1276.000000, 1281.999999, "23cm ATV #3",		true,
								1282.000000, 1287.999999, "23cm Repeater Outputs",	true,
								1288.000000, 1293.999999, "23cm Simplex ATV/Wideband Exp.", true,
								1294.000000, 1294.499999, "23cm Simplex FM",		true,
								1294.500000, 1294.500000, "23cm FM Simplex Calling", true,
								1294.500001, 1294.999999, "23cm Simplex FM",		true,
								1295.000000, 1295.799999, "23cm SSTV/FAX/ACSSB/Exp.", true,
								1295.800000, 1295.999999, "23cm EME/CW Expansion",	true,
								1296.000000, 1296.049999, "23cm EME Exclusive",		true,
								1296.050000, 1296.069999, "23cm Weak Signal",		true,
								1296.070000, 1296.079999, "23cm CW Beacons",		true,
								1296.080000, 1296.099999, "23cm Weak Signal",		true,
								1296.100000, 1296.100000, "23cm CW/SSB Calling",	true,
								1296.100001, 1296.399999, "23cm Weak Signal",		true,
								1296.400000, 1296.599999, "23cm X-Band Translator Input", true,
								1296.600000, 1296.799999, "23cm X-Band Translator Output", true,
								1296.800000, 1296.999999, "23cm Experimental Beacons", true,
								1297.000000, 1300.000000, "23cm Digital Modes",		true,

								2300.000000, 2302.999999, "2.3GHz High Data Rate", true,
								2303.000000, 2303.499999, "2.3GHz Packet",		true,
								2303.500000, 2303.800000, "2.3GHz TTY Packet",	true,
								2303.800001, 2303.899999, "2.3GHz General",	true,
								2303.900000, 2303.900000, "2.3GHz Packet/TTY/CW/EME", true,
								2303.900001, 2304.099999, "2.3GHz CW/EME",		true,
								2304.100000, 2304.100000, "2.3GHz Calling Frequency", true,
								2304.100001, 2304.199999, "2.3GHz CW/EME/SSB",	true,
								2304.200000, 2304.299999, "2.3GHz SSB/SSTV/FAX/Packet AM/Amtor", true,
								2304.300000, 2304.319999, "2.3GHz Propagation Beacon Network", true,
								2304.320000, 2304.399999, "2.3GHz General Propagation Beacons", true,
								2304.400000, 2304.499999, "2.3GHz SSB/SSTV/ACSSB/FAX/Packet AM", true,
								2304.500000, 2304.699999, "2.3GHz X-Band Translator Input", true,
								2304.700000, 2304.899999, "2.3GHz X-Band Translator Output", true,
								2304.900000, 2304.999999, "2.3GHz Experimental Beacons", true,
								2305.000000, 2305.199999, "2.3GHz FM Simplex", true,
								2305.200000, 2305.200000, "2.3GHz FM Simplex Calling", true,
								2305.200001, 2305.999999, "2.3GHz FM Simplex", true,
								2306.000000, 2308.999999, "2.3GHz FM Repeaters", true,
								2309.000000, 2310.000000, "2.3GHz Control/Aux Links", true,
								2390.000000, 2395.999999, "2.3GHz Fast-Scan TV", true,
								2396.000000, 2398.999999, "2.3GHz High Rate Data", true,
								2399.000000, 2399.499999, "2.3GHz Packet", true,
								2399.500000, 2399.999999, "2.3GHz Control/Aux Links", true,
								2400.000000, 2402.999999, "2.4GHz Satellite", true,
								2403.000000, 2407.999999, "2.4GHz Satellite High-Rate Data", true,
								2408.000000, 2409.999999, "2.4GHz Satellite", true,
								2410.000000, 2412.999999, "2.4GHz FM Repeaters", true,
								2413.000000, 2417.999999, "2.4GHz High-Rate Data", true,
								2418.000000, 2429.999999, "2.4GHz Fast-Scan TV", true,
								2430.000000, 2432.999999, "2.4GHz Satellite", true,
								2433.000000, 2437.999999, "2.4GHz Sat. High-Rate Data", true,
								2438.000000, 2450.000000, "2.4GHz Wideband FM/FSTV/FMTV", true,

								3456.000000, 3456.099999, "3.4GHz General", true,
								3456.100000, 3456.100000, "3.4GHz Calling Frequency", true,
								3456.100001, 3456.299999, "3.4GHz General", true,
								3456.300000, 3456.400000, "3.4GHz Propagation Beacons", true,

								5760.000000, 5760.099999, "5.7GHz General", true,
								5760.100000, 5760.100000, "5.7GHz Calling Frequency", true,
								5760.100001, 5760.299999, "5.7GHz General", true,
								5760.300000, 5760.400000, "5.7GHz Propagation Beacons", true,

								10368.000000, 10368.099999, "10GHz General", true,
								10368.100000, 10368.100000, "10GHz Calling Frequency", true,
								10368.100001, 10368.400000, "10GHz General", true,

								24192.000000, 24192.099999, "24GHz General", true,
								24192.100000, 24192.100000, "24GHz Calling Frequency", true,
								24192.100001, 24192.400000, "24GHz General", true,

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

            AddBandTextSWB(); // ke9ns add CB
          
        } // AddBandTextTable() Region2

        #endregion

        #region IARU Region 3 BandText

        private static void AddRegion3BandText160m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								1.800000, 1.829999, "160M CW",	                true,
                                1.830000, 1.833999, "160M CW & NB Digital",     true,
                                1.834000, 1.839999, "160M CW",	                true,
								1.840000, 1.999999, "160M CW & Phone",	        true,
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
								3.500000, 3.509999, "80M CW DX",	  			true,
                                3.510000, 3.534999, "80M CW",	    	    	true,
                                3.535000, 3.599999, "80M Phone & CW",	    	true,
                                3.600000, 3.600000, "80M IARU Emergency",       true,
                                3.600001, 3.774999, "80M Phone & CW",	    	true,
								3.775000, 3.799999, "80M DX Phone & CW",	    true,
								3.800000, 3.899999, "80M Phone & CW",			true,
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

        private static void AddRegion3BandText40m()
        {
            DataTable t = ds.Tables["BandText"];
            object[] data = {
								7.000000, 7.024999, "40M CW",			    	true,
                                7.025000, 7.029999, "40M CW & NB Digital",		true,
                                7.030000, 7.039999, "40M All Modes",	        true,
								7.040000, 7.109999, "40M Phone & CW",       	true,
                                7.110000, 7.110000, "40M IARU Emergency",       true,
                                7.110001, 7.299999, "40M Phone & CW",			true,
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
								10.100000, 10.139999, "30M CW",					true,
                                10.140000, 10.149999, "30M CW & NB Digital",	true,
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
								14.000000, 14.069999, "20M CW",		        	true,
								14.070000, 14.094999, "20M CW & NB Digital",	true,
                                14.095000, 14.099499, "20M Data & Packet",      true,
								14.099500, 14.099999, "20M Beacons",		    true,
                                14.100000, 14.100000, "20M NCDXF Beacons",  	true,
                                14.100001, 14.100499, "20M Beacons",		    true,
                                14.100500, 14.111999, "20M Data & Packet",      true,
                                14.112000, 14.229999, "20M Phone & CW",         true,
                                14.230000, 14.230000, "20M SSTV",				true,
                                14.230001, 14.299999, "20M Phone & CW",         true,
                                14.300000, 14.300000, "20M IARU Emergency",     true,
                                14.300001, 14.349999, "20M All Modes",          true,
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
								18.068000, 18.099999, "17M CW",					true,
								18.100000, 18.109499, "17M CW & NB Digital",	true,
								18.109500, 18.109999, "17M Beacons",		    true,
                                18.110000, 18.110000, "17M NCDXF Beacons",	    true,
                                18.110001, 18.110499, "17M Beacons",		    true,
								18.110500, 18.159999, "17M Phone & CW",			true,
                                18.160000, 18.160000, "17M IARU Emergency",		true,
                                18.160001, 18.167999, "17M All Modes",	        true,
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
								21.000000, 21.069999, "15M CW",			        true,
								21.070000, 21.124999, "15M CW & NB Digital",	true,
								21.125000, 21.149499, "15M Phone & CW",	        true,
                                21.149500, 21.149999, "15M Beacons",		    true,
                                21.150000, 21.150000, "15M NCDXF Beacons",	    true,
                                21.150001, 21.150499, "15M Beacons",		    true,
								21.150500, 21.339999, "15M Phone & CW",			true,
                                21.340000, 21.340000, "15M SSTV",		        true,
                                21.340001, 21.359999, "15M Phone & CW",			true,
                                21.360000, 21.360000, "15M Emergency",	    	true,
                                21.360001, 21.449999, "15M All Modes",			true,
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
								24.890000, 24.919999, "12M CW",					true,
                                24.920000, 24.929499, "12M CW & NB Digital",	true,
								24.929500, 24.929999, "12M Beacons",		    true,
                                24.930000, 24.930000, "12M NCDXF Beacons",	    true,
                                24.930001, 24.930499, "12M Beacons",		    true,
                                24.930500, 24.989999, "12M All Modes",			true,
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
								28.000000, 28.049999, "10M CW",					true,
                                28.050000, 28.149999, "10M CW & NB Digital",	true,
                                28.150000, 28.189999, "10M CW",					true,
								28.190000, 28.199999, "10M Beacons",	        true,
                                28.200000, 28.200000, "10M NCDXF Beacons",      true,
                                28.200001, 28.200499, "10M Beacons",	        true,
                                28.200500, 28.679999, "10M Phone & CW",     	true,
                                28.680000, 28.680000, "10M SSTV",	            true,
                                28.680001, 29.299999, "10M Phone & CW",	    	true,
                                29.300000, 29.509999, "10M Satellite & CW",     true,
                                29.510000, 29.699999, "10M Wide Band  & CW",	true,
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
								50.000000, 50.099999, "6M CW & Beacons",		    true,
                                50.100000, 50.499999, "6M Phone/NB Digital/CW",	    true,
                                50.500000, 53.999999, "6M Wide Band Modes & CW",    true,
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
                                144.000000, 144.019999, "2M EME",		                true,
                                144.020000, 144.099999, "2M CW & EME",                  true,
                                144.100000, 144.399999, "2M CW/Phone & Image",          true,
                                144.400000, 144.499999, "2M CW/Phone/NB Digital",       true,
                                144.500000, 144.699999, "2M Wide Digital Modes",        true,
                                144.700000, 145.499999, "2M FM, CW & Image",            true,
                                145.500000, 145.500000, "2M Emergency",                 true,
                                145.500001, 145.649999, "2M FM, CW & Image",            true,
                                145.650000, 145.799999, "2M All Modes",                 true,
                                145.800000, 145.999999, "2M Satellite.",                true,
                                146.000000, 147.999999, "2M All Modes",                 true,
                                // 430 - 440 MHz
								430.000000, 430.099999, "70cm CW",	                    true,
                                430.100000, 430.699999, "70cm CW/Phone & Image",        true,
                                430.700000, 430.799999, "70cm CW/Phone/NB Digital",     true,
                                430.800000, 431.399999, "70cm Wide Digital Modes",      true,
                                431.400000, 431.899999, "70cm FM, CW & Image",          true,
                                431.900000, 432.099999, "70cm EME",                     true,
                                432.100000, 432.999999, "70cm FM, CW & Image",          true,
                                433.000000, 433.000000, "70cm Emergency",               true,
                                433.000001, 433.999999, "70cm FM, CW & Image",          true,
                                434.000000, 434.999999, "70cm Repeaters",               true,
                                435.000000, 437.999999, "70cm Satellite",               true,
                                438.000000, 438.999999, "70cm All Modes",               true,
                                439.000000, 439.999999, "70cm Repeaters",               true,
                                440.000000, 449.999999, "70cm All Modes",               true,
                                // 1240 - 1300 MHz
								1240.000000, 1259.999999, "23cm All Modes",	            true,
                                1260.000000, 1269.999999, "23cm Satellite",             true,
                                1270.000000, 1295.999999, "23cm All Modes",             true,
                                1296.700000, 1296.999999, "23cm EME - all modes",       true,
                                1297.725000, 1299.999999, "23cm All Modes",             true,                                
                                // 2300 -2450 MHz
								2300.000000, 2303.999999, "13cm Sub-Regional",	        true,
                                2304.000000, 2305.999999, "13cm Narrow Band ",	        true,
                                2306.000000, 2307.999999, "13cm Sub-Regional",	        true,
                                2308.000000, 2309.999999, "13cm Narrow Band ",	        true,
                                2310.000000, 2319.999999, "13cm Sub-Regional",	        true,
                                2320.000000, 2320.024999, "13cm CW EME",	            true,
                                2320.025000, 2320.149999, "13cm CW",	                true,
                                2320.150000, 2320.199999, "13cm CW & SSB",	            true,
                                2320.200000, 2320.200000, "13cm SSB Calling",	        true,
                                2320.200001, 2320.799999, "13cm CW & SSB",	            true,
                                2320.800000, 2320.999999, "13cm Beacons",	            true,
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
                                1.810000, 1.824999, "160M CW",	                true,
                                1.825000, 1.907499, "160M Band RX",             false,
                                1.907500, 1.912499, "160M CW & NB Digital",     true,
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
								3.500000, 3.519999, "80M CW",	  			    true,
                                3.520000, 3.524999, "80M CW & NB Digital",	    true,
                                3.525000, 3.525000, "80M Emergency",            true,                                
                                3.525001, 3.529999, "80M Phone/CW/NB Digital",	true,
                                3.530000, 3.574999, "80M Phone/CW/Digital",     true,
                                3.575000, 3.598999, "80M Band RX",              false,
                                3.599000, 3.611999, "80M Phone/CW/Digital",     true,
                                3.612000, 3.679999, "80M Band RX",              false,
                                3.680000, 3.686999, "80M Phone/CW/Image",       true,
                                3.687000, 3.701999, "80M Band RX",              false,
                                3.702000, 3.715999, "80M Phone/CW/Image",       true,
                                3.716000, 3.744999, "80M Band RX",              false,
                                3.745000, 3.769999, "80M Phone/CW/Image",       true,
                                3.770000, 3.790999, "80M Band RX",              false,
                                3.791000, 3.804999, "80M Phone/CW/NB Digital",	true,
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
								7.000000, 7.024999, "40M CW",			    	true,
                                7.025000, 7.029999, "40M CW & NB Digital",		true,
                                7.030000, 7.030000, "40M Emergency",            true,
                                7.030001, 7.039999, "40M CW & NB Digital",      true,
                                7.040000, 7.044999, "40M DX NB Digital/CW",	    true,
                                7.045000, 7.099999, "40M CW/Phone/Image",       true,
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
								28.000000, 28.069999, "10M CW",					true,
                                28.070000, 28.149999, "10M CW & NB Digital",	true,
                                28.150000, 28.199499, "10M CW",					true,
								28.199500, 28.199999, "10M Beacons",		    true,
                                28.200000, 28.200000, "10M NCDXF Beacons",	    true,
                                28.200001, 28.200500, "10M Beacons",		    true,
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
								50.000000, 50.099999, "6M DX CW/EME/Beacons",	true,
                                50.100000, 50.899999, "6M Phone/CW/Image",	    true,
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
								4.629995, 4.630005, "Japan Int. Emergency",	true,
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
        
        private static void AddBandStackTable()
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
								"60M", "USB", "F6", 5.330500,
								"60M", "USB", "F6", 5.346500,
								"60M", "USB", "F6", 5.357000,
								"60M", "USB", "F6", 5.371500,
								"60M", "USB", "F6", 5.403500,
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

			for(int i= 0; i<data.Length/4; i++)
			{
				DataRow dr = ds.Tables["BandStack"].NewRow();
				dr["BandName"] = (string)data[i*4+0];
				dr["Mode"] = (string)data[i*4+1];
				dr["Filter"] = (string)data[i*4+2];
				dr["Freq"] = ((double)data[i*4+3]).ToString("f6");
				ds.Tables["BandStack"].Rows.Add(dr);
			}
		} //addbandstacktable

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
			
			for(int i= 0; i<vals.Length; i++)
			{
				DataRow dr = t.NewRow();
				dr[0] = i;
				dr[1] = vals[i];
				t.Rows.Add(dr);
			}
		}

        private static void AddTXProfileTable(Model model)
		{
			ds.Tables.Add("TXProfile");
			DataTable t = ds.Tables["TXProfile"];

			t.Columns.Add("Name", typeof(string));
			t.Columns.Add("FilterLow", typeof(int));
			t.Columns.Add("FilterHigh", typeof(int));
			t.Columns.Add("TXEQNumBands", typeof(int));
			t.Columns.Add("TXEQEnabled", typeof(bool));
			t.Columns.Add("TXEQPreamp", typeof(int));
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
            t.Columns.Add("Dexp_Threshold",typeof(int));
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
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
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
            dr["FilterLow"] = 150;
            dr["FilterHigh"] = 3050;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
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
            dr["FilterLow"] = 300;
            dr["FilterHigh"] = 2400;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
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
        }

        #region TX Profile Management

        private static void AddTXProfileDefTable(Model model)
		{
			ds.Tables.Add("TXProfileDef");
			DataTable t = ds.Tables["TXProfileDef"];

			t.Columns.Add("Name", typeof(string));
			t.Columns.Add("FilterLow", typeof(int));
			t.Columns.Add("FilterHigh", typeof(int));
			t.Columns.Add("TXEQNumBands", typeof(int));
			t.Columns.Add("TXEQEnabled", typeof(bool));
			t.Columns.Add("TXEQPreamp", typeof(int));
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
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
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
            dr["FilterLow"] = 150;
            dr["FilterHigh"] = 3050;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
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
            dr["FilterLow"] = 300;
            dr["FilterHigh"] = 2400;
            dr["TXEQNumBands"] = 10;
            dr["TXEQEnabled"] = true;
            dr["TXEQPreamp"] = -3;
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
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
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
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = -6;
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
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = -6;
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
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = true;
			dr["TXEQPreamp"] = -6;
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
			dr["FilterLow"] = 50;
			dr["FilterHigh"] = 3650;
			dr["TXEQNumBands"] = 10;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
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
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
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
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
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
		}

		private static void CheckBandTextValid()
		{
			ArrayList bad_rows = new ArrayList();

			if(ds == null) return;
			foreach(DataRow dr in ds.Tables["BandText"].Rows)
			{
				// check low freq
				string f = ((double)dr["Low"]).ToString("f6");
				f = f.Replace(",", ".");
                if (f.Contains(".") == false) f = f + ".0"; // ke9ns add

                DataRow[] rows = ds.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length > 1)
				{
					// handle multiple entries
					if(!bad_rows.Contains(dr))
						bad_rows.Add(dr);
				}

				// check high freq
				f = ((double)dr["High"]).ToString("f6");
				f = f.Replace(",", ".");
                if (f.Contains(".") == false) f = f + ".0"; // ke9ns add

                rows = ds.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length > 1)
				{
					// handle multiple entries
					if(!bad_rows.Contains(dr))
						bad_rows.Add(dr);
				}
			}

			foreach(DataRow dr in bad_rows)
				ds.Tables["BandText"].Rows.Remove(dr);
		}

		#endregion

        #endregion

        #region Public Member Functions
// ======================================================
// Public Member Functions 
// ======================================================

		public static bool Init(Model model)
		{

            

           if (file_name.Contains("database_F")|| file_name.Contains("database_D")) // ke9ns add  make sure your now looking at RevQ database only
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
		}

		public static void Update()
		{
            try
            {
                ds.WriteXml(file_name, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {                
                MessageBox.Show("A database write to file operation failed.  " +
                    "The exception error was:\n\n" + ex.Message,
                    "ERROR: Database Write Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

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
                if( f.Contains(".") == false)   f = f + ".0"; // ke9ns add


				DataRow[] rows = ds.Tables["BandText"].Select(f + ">=Low AND " + f + "<=High");

				if(rows.Length == 0)		// band not found
				{
					outStr = "Out of Band";
					return false;
				}
				else if(rows.Length == 1)	// found band
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
			catch(Exception e)
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

			for(int i= 0; i < band_list.Length; i++)
			{
				string s = band_list[i];
				DataRow[] rows = ds.Tables["BandStack"].Select("'"+s+"' = BandName");
				retvals[i] = rows.Length;
			}

			return retvals;
        } //  GetBandStackNum




        //==================================================================================================
        public static bool GetBandStack(string band, int index, out string mode, out string filter, out double freq)
		{
			DataRow[] rows = ds.Tables["BandStack"].Select("'"+band+"' = BandName");

			if(rows.Length == 0)
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
            catch(Exception)
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
            catch(Exception)
            {
                MessageBox.Show("problem found sorting entry for Band: " + band, "No Entry Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        } //Sortbandstack




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
        public static void AddBandText( double freq, double freq1, string name, bool tx)
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
			DataRow[] rows = ds.Tables["BandStack"].Select("'"+band+"' = BandName");

			if(rows.Length == 0) return;

			foreach(DataRow datarow in rows)			// prevent duplicates
			{
				if((string)datarow["BandName"] == band && (double)datarow["Freq"] == freq)
				{
					datarow["Filter"] = filter;
					datarow["Mode"] = mode;

                    Debug.WriteLine("====DUPLICATE===");
					return;
				}
			}
          
            filter3 = Console.BandStackLock;
          
            Debug.WriteLine("database check for lock status= "+ filter3);

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

		public static void SaveVars(string tableName, ref ArrayList list)
		{
			if(!ds.Tables.Contains(tableName))
				AddFormTable(tableName);

			foreach(string s in list)
			{
				string[] vals = s.Split('/');
				if(vals.Length > 2)
				{
					for(int i=2; i<vals.Length; i++)
						vals[1] += "/"+vals[i];
				}

                if(vals.Length <= 1) // skip it as no data was provided
                    continue;

				DataRow[] rows = ds.Tables[tableName].Select("Key = '"+vals[0]+"'");
				if(rows.Length == 0)	// name is not in list
				{
					DataRow newRow = ds.Tables[tableName].NewRow();
					newRow[0] = vals[0];
					newRow[1] = vals[1];
					ds.Tables[tableName].Rows.Add(newRow);
				}
				else if(rows.Length == 1)
				{
					rows[0][1] = vals[1];
				}
			}
		}

		public static ArrayList GetVars(string tableName)
		{
			ArrayList list = new ArrayList();
			if(!ds.Tables.Contains(tableName))
				return list;

			DataTable t = ds.Tables[tableName];

			for(int i= 0; i < t.Rows.Count; i++)
			{
				list.Add(t.Rows[i][0].ToString()+"/"+t.Rows[i][1].ToString());
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
			catch(Exception)
			{
				return false;
			}

			ds = file;

            // Handle change of mode from FMN to just FM
            DataRow[] rows = ds.Tables["BandStack"].Select("Mode = 'FMN'");
            foreach(DataRow dr in rows)
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
                    AddRegion1BandStack();

                    AddBandStackSWL(); // ke9ns add
                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();

                    //  if (current_region == FRSRegion.Spain_UK)
                    //    AddRegion1ABandText60m(); // transmit region1
                    // else

                    AddRegion1BandText60m(); // non transmit region1

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
                    AddRegion1ABandStack(); // ke9ns mod 

                    AddBandStackSWL(); // ke9ns add
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


                    AddRegion1ABandStack();

                    AddBandStackSWL(); // ke9ns add
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
                    AddRegion1ABandStack(); // ke9ns mod

                    AddBandStackSWL(); // ke9ns add
                    ClearBandText();
                    AddItalyBandText160m();
                    AddRegion1BandText80m();
                    AddRegion1BandText60m(); // ke9ns no transmit
                 //   AddRegion1ABandText60m();

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
                    AddUK_PlusBandStack();

                    AddBandStackSWL(); // ke9ns add
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
                    AddRegion1BandStack();

                    AddBandStackSWL(); // ke9ns add
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
                    AddRegion1ABandStack();

                    AddBandStackSWL(); // ke9ns add
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
                    AddRegion1ABandStack();

                    AddBandStackSWL(); // ke9ns add
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
                    AddRegion1BandStack();

                    AddBandStackSWL(); // ke9ns add
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
                    AddGreeceBandText6m();
                    AddRegion1BandText4m();
                    AddRegion1BandTextVHFplus();
                    AddBandTextSWB();
                    break;

                case FRSRegion.Hungary: // EU05
                   // AddRegion1BandStack();
                    AddRegion1ABandStack();

                    AddBandStackSWL(); // ke9ns add
                    ClearBandText();
                    AddRegion1BandText160m();
                    AddRegion1BandText80m();
                    AddRegion1BandText60m(); // no tx
                  //AddRegion1ABandText60m();

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

                    AddRegion1BandStack();

                    AddBandStackSWL(); // ke9ns add
                    ClearBandText();
                    AddNetherlandsBandText160m();
                    AddRegion1BandText80m();

                    AddRegion1BBandText60m(); // ke9ns add 5.35 to 5.45 TX

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
                    Debug.WriteLine("RUSSIA===============");

                  //  AddRegion1BandStack();
                    AddRegion1ABandStack(); // 60m bandstack
                    AddBandStackSWL(); // ke9ns add

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
                    AddSwedenBandStack();

                    AddBandStackSWL(); // ke9ns add
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

                case FRSRegion.Region_3:
                    AddRegion3BandStack();
                    AddBandStackSWL(); // ke9ns add
                    ClearBandText();
                    AddRegion3BandText160m();
                    AddRegion3BandText80m();
                   // AddRegion3BandText60m();
                    AddRegion1BandText60m(); // no transmit

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
                    AddRegion3BandStack();
                    AddBandStackSWL(); // ke9ns add
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
                    AddRegion1ABandStack();
                  
                    AddBandStackSWL(); // ke9ns add
                    ClearBandText();
                    AddItalyBandText160m();
                    AddRegion1BandText80m();
                    AddRegion1BandText60m();
                  //  AddRegion1ABandText60m();

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
