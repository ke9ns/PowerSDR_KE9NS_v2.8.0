//=================================================================
// Http.cs
//=================================================================
// Http Server
//
// Николай  RN3KK
// Darrin ke9ns
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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.IO.Ports;
using TDxInput;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using Microsoft.JScript;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;



//using System.Runtime.Remoting.Contexts;

namespace PowerSDR
{
    sealed public class Http
    {
        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public static Setup setupForm;          // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        public static TcpListener m_listener;

        private const String IMAGE_REQUEST = "/image";

        enum RequestType
        {
            GET_IMAGE,
            GET_HTML_INDEX_PAGE,
            UNKNOWN,
            ERROR
        }
        public Http(Console c)
        {
            console = c;

        }




        //=========================================================
        public string Weather()
        {
            var wthr = WeatherAAsync().Result;
            return wthr.ToString();
        }

        public string WeatherOpen()
        {
            var wthr = WeatherAAsync().Result;
            return wthr.ToString();
        }

        public string WeatherOpen1()
        {
            var wthr = WeatherAAsync1().Result;
            return wthr.ToString();
        }

        bool OpenWeather = false; // ke9ns: true = outside USA, use open weather as source

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns  ASYNC 
        public async Task<string> WeatherAAsync()
        {

            Debug.WriteLine("GET Real weather data=========");


            OpenWeather = false;
            string content1 = " ";

            if (console.SpotForm != null)
            {

                if ( (console.OpenWeather == false) && ((int)console.SpotForm.udDisplayLat.Value > 25) && ((int)console.SpotForm.udDisplayLat.Value < 51))
                {
                    if (((int)console.SpotForm.udDisplayLong.Value > -140) && ((int)console.SpotForm.udDisplayLong.Value < -69))
                    {
                        Debug.WriteLine("GOOD LAT AND LONG weather data=========");

                        //    string latitude = console.SpotForm.udDisplayLat.Value.ToString("##0.00").PadLeft(5);   // -90.00
                        //    string longitude = console.SpotForm.udDisplayLong.Value.ToString("###0.00").PadLeft(6);  // -180.00 

                        string latitude = console.SpotForm.udDisplayLat.Value.ToString("0.00");    // -90.00
                        string longitude = console.SpotForm.udDisplayLong.Value.ToString("0.00");  // -180.00 

                        var url = new Uri("https://forecast.weather.gov/MapClick.php?lat=" + latitude + "&lon=" + longitude + "&FcstType=dwml");

                        //  var url = new Uri("https://f1.weather.gov/MapClick.php?lat=" + latitude + "&lon=" + longitude + "&FcstType=dwml");
                        //   var url = new Uri("https://www.nws.noaa.gov/wtf/MapClick.php?lat=" + latitude + "&lon=" + longitude + "&FcstType=dwml");


                        HttpClient client = new HttpClient();

                        try
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                        }
                        catch (Exception g)
                        {
                            Debug.WriteLine("http client user agent fail " + g);
                        }

                        Debug.WriteLine("GOOD LAT AND LONG weather data1=========" + url);

                        try
                        {
                            var xml = await client.GetStringAsync(url).ConfigureAwait(false);
                            content1 = xml.ToString();
                            client.Dispose();
                            return content1;

                        }
                        catch (Exception g)
                        {
                            content1 = "Error " + g.ToString();
                            client.Dispose();
                            return content1;
                        }

                    } // if (((int)console.SpotForm.udDisplayLong.Value > -140) && ((int)console.SpotForm.udDisplayLong.Value < -69))
                    else
                    {
                        Debug.WriteLine("LAT not good==use openweathermap=======");
                        OpenWeather = true;
                    }


                } //   if (((int)console.SpotForm.udDisplayLat.Value > 25) && ((int)console.SpotForm.udDisplayLat.Value < 51))
                else
                {
                    Debug.WriteLine("LONG not good=use openweathermap========");
                    OpenWeather = true;
                }


                //------------------------------------------------------
                // ke9ns: used outside of USA for weather

                if (OpenWeather == true)
                {

                    // https://api.openweathermap.org/data/2.5/onecall?lat=42.0&lon=-88.1&units=imperial&exclude=minutely,hourly,daily&appid=5665fe4f274249599b33ad319e2fdad1
                    // https://api.openweathermap.org/data/2.5/weather?lat=42.01&lon=78.0&units=metric&appid=5665fe4f274249599b33ad319e2fdad1

                    string latitude = console.SpotForm.udDisplayLat.Value.ToString("0.00");    // -90.00
                    string longitude = console.SpotForm.udDisplayLong.Value.ToString("0.00");  // -180.00 

                    //   var url = new Uri("https://api.openweathermap.org/data/2.5/onecall?lat=" + latitude + "&lon=" + longitude + "&units=metric" + "&exclude=minutely,hourly,daily" + "&appid=5665fe4f274249599b33ad319e2fdad1");
                    var url = new Uri("https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&units=metric" + "&appid=5665fe4f274249599b33ad319e2fdad1");

                    HttpClient client = new HttpClient();
                    try
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    }
                    catch (Exception g)
                    {
                        Debug.WriteLine("http client user agent fail " + g);
                    }

                    Debug.WriteLine("GOOD LAT AND LONG openweather data========= " + url);

                    try
                    {

                        var test = await client.GetStringAsync(url).ConfigureAwait(false);


                        content1 = test.ToString();

                        client.Dispose();

                        Debug.WriteLine("OPENWEATHER4: " + content1);

                        return content1;

                    }
                    catch (Exception g)
                    {
                        content1 = "Error " + g.ToString();
                        client.Dispose();
                        return content1;
                    }



                } // if (OpenWeather == true)


            } //
            else Debug.WriteLine("Spotform not open=========");

            console.LOCALWEATHER = false;
            Console.noaaON = 1;

            return content1;
        } // aync weather data

        public string latitude1 = "-";
        public string longitude1 = "-";
        string content2 = " ";

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns  ASYNC 
        public async Task<string> WeatherAAsync1()
        {

            Debug.WriteLine("GET Real weather data=========");


            if (console.SpotForm != null)
            {

                //------------------------------------------------------
                // ke9ns: used outside of USA for weather

                if (OpenWeather == true)
                {

                    // https://open.mapquestapi.com/elevation/v1/profile?key=djHUpLQ0tP4p2bLbGZAfIIhLiBWLu916&shapeFormat=raw&latLngCollection=42.0,-88.19


                    if (latitude1 == console.SpotForm.udDisplayLat.Value.ToString("0.00") && longitude1 == console.SpotForm.udDisplayLong.Value.ToString("0.00")) return content2;

                    latitude1 = console.SpotForm.udDisplayLat.Value.ToString("0.00");    // -90.00
                    longitude1 = console.SpotForm.udDisplayLong.Value.ToString("0.00");  // -180.00 

                    var url = new Uri("https://open.mapquestapi.com/elevation/v1/profile?key=djHUpLQ0tP4p2bLbGZAfIIhLiBWLu916&shapeFormat=raw&latLngCollection=" + latitude1 + "," + longitude1);
                    HttpClient client = new HttpClient();

                    try
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    }
                    catch (Exception g)
                    {
                        Debug.WriteLine("http client user agent fail " + g);
                    }

                    Debug.WriteLine("1GOOD LAT AND LONG openweather data========= " + url);

                    try
                    {

                        var test = await client.GetStringAsync(url).ConfigureAwait(false);

                        content2 = test.ToString();

                        client.Dispose();

                        Debug.WriteLine("OPENWEATHER5: " + content2);

                        return content2;

                    }
                    catch (Exception g)
                    {
                        content2 = "Error " + g.ToString();
                        client.Dispose();
                        return content2;
                    }



                } // if (OpenWeather == true)


            } //
            else Debug.WriteLine("Spotform not open=========");

            console.LOCALWEATHER = false;
            Console.noaaON = 1;

            return content2;
        } // aync1 weather data



        //=========================================================================================
        // ke9ns  ASYNC Communicate with ARRL LoTW server to get result if you have made contact with this station before
        //=========================================================

        int file_LoTW = 0;

        // this gets ALL your LoTW XML contact Data EVER made
        public string Lotw1() // called by SPOT.CS
        {
            if (console.SpotForm != null)
            {
                file_LoTW = 0;

                var lotw = LotwAAsync(2).Result;         // download Master FULL LoTW Log (only if there is no Master Full LOG file)
                Debug.WriteLine("lotw2: " + file_LoTW );
              
                var lotw1 = LotwAAsync(3).Result;        // download partial update of QSO and QSL info (no need to download the full Log file ever again)
                Debug.WriteLine("lotw3: " + file_LoTW );

                return lotw.ToString(); // return XML data
            }
            else
            {
                Debug.WriteLine("LoTW NOT READY");
                return "NOT READY";
            }
        } //Lotw1()



        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns:  ASYNC Communicate with ARRL LoTW server to download your entire log, then download from the prior date of last download
        // 1 Download entire log file from 1901 to today then download QSL update immediately 
        // 2 Download from last time time you downloaded your log 
        // 3 merge back to 1 file
        // keep QSO and QSL files to make it easier for updating the Spotter screen



        bool OLOG = false; // .199



        public async Task<string> LotwAAsync(int f_LoTW) //    public  LotwAAsync(int f_LoTW)
        {
            string content1 = "NOT READY";

            List<string> LoTW_master = new List<string>();  // master LOG file
          
            List<string> LoTW_MasterCall = new List<string>(); // .199 to help speed up large LoTW QSO logs
            List<string> LoTW_MasterMode = new List<string>(); // .199
            List<string> LoTW_MasterBand = new List<string>(); // .199
            List<string> LoTW_MasterQSL = new List<string>(); // .199
         
            List<string> LoTW_QSO = new List<string>();
            List<string> LoTW_QSOCall = new List<string>(); // .199
            List<string> LoTW_QSOBand = new List<string>(); // .199
            List<string> LoTW_QSOMode = new List<string>(); // .199

            List<string> LoTW_QSL = new List<string>();
            List<string> LoTW_QSLCall = new List<string>(); // .199
            List<string> LoTW_QSLBand = new List<string>(); // .199
            List<string> LoTW_QSLMode = new List<string>(); // .199
        
         
            string file_name1 = console.AppDataPath + "LoTW_LOG.adi";

            string file_name2 = console.AppDataPath + "LoTW_LOG.adi";                     // Master Combined QSO & QSL detail file ( your LoTW_LOG file )
            string file_name3 = console.AppDataPath + "LoTW_LOG_QSO_Update.adi";          // Only QSO that occured ON or after the specified date (date of the last modification to the Master file).
            string file_name4 = console.AppDataPath + "LoTW_LOG_QSL_Update.adi";          // Only QSO's that were turned into QSL's ON or After the specified date (date of the last modification to the Master file).
            string file_name5 = console.AppDataPath + "LoTW_OLOG.adi";                                                                               // 3 and 4 will be merged with the 1  Master LOG file at the end of the partial update
            Uri url;  //

            string mod1 = DateTime.Now.ToString("yyyy-MM-dd");

            // http://www.arrl.org/adif  this explains the format to get QSO/QSL data

       
            if ((!File.Exists(file_name1)) && (File.Exists(file_name5)) && (f_LoTW == 2) ) //.199  if the LoTW_OLOG.adi file is present (full log), and LoTW_QSO_Update.adi file is not present
            {
             
                console.SpotForm.textBox1.Text += "You have the LoTW_OLOG file, but no LoTW_QSO file. Will update and create new master LOG.\r\n";
                Debug.WriteLine("LoTW you have a LoTW_OLOG file");
                file_LoTW = 2;
                 File.Copy(file_name5, file_name1); // copy the OLOG file to the master LOG file
                OLOG = true;
                return content1;
            }
            else if ((!File.Exists(file_name1)) && (f_LoTW == 2))                                  // if you dont have a Master LOG file, then get your ENTIRE LoTW QSO/QSL file
            {
                OLOG = false;
                file_LoTW = 2; // get a FULL Master LoTW log file to (NEW)
                console.SpotForm.textBox1.Text += "Downloading your complete LoTW LOG file (including QSL station details).... (Approx: 5 seconds / 400 QSO's)\r\n";
            }
            else // get an update to your file because the master file already exists (and the 2nd time around after Master log downloaded, file_LoTW will still be ==2
            {
                DateTime modDate;
                Debug.WriteLine("LoTW OLOG " + file_LoTW +" , " + OLOG);

                if (OLOG == true)
                {
                    //   modDate = File.GetCreationTime(file_name5); // if you have a OLOG file (a full LoTW QSO/QSL log untouched) use its date as a starting point
                    modDate = File.GetLastWriteTime(file_name5);
                }
                else
                {
                    modDate = File.GetLastWriteTime(file_name2);
                }

               

                Debug.WriteLine("-LoTW OLOG " + file_LoTW);
               
                mod1 = modDate.ToString("yyyy-MM-dd");
                Debug.WriteLine("LoTW you have a LoTW_OLOG file with a MOD date of " + mod1);

                if (f_LoTW == 2) file_LoTW = 3; // get QSO partial data
                else file_LoTW = 4;             // get QSL partial data

                // console.SpotForm.textBox1.Text += "Downloading an QSO & QSL updates to your Master LoTW LOG file back to date: " + modDate + "\r\n";
                console.SpotForm.textBox1.Text += "Last PowerSDR LoTW Log file update was: " + modDate + "\r\n";
                console.SpotForm.textBox1.Text += "Download only QSO and QSL LOG data after the Last update.\r\n";


            } // create a partial LoTW update file to the Master

            // this gets all your QSO's & QSL's, but they dont always have DXCC entity numbers, etc., etc.

            Uri url2 = new Uri("https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + console.SpotForm.callBox.Text + "&password=" + console.SpotForm.LoTWPASS +
                            "&qso_qsldetail=yes" + "&qso_qsl=no" + "&qso_query=1" + "&qso_qsorxsince=1901-01-01");


            // https://lotw.arrl.org/lotwuser/lotwreport.adi?login=ke9ns&password=xxxxxx&qso_qsl=no&qso_query=1&qso_qsorxsince=1901-01-01&qso_qsldetail=yes

            // Uri url2 = new Uri("https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + console.SpotForm.callBox.Text + "&password=" + console.SpotForm.LoTWPASS +
            //             "&qso_qsldetail=yes" + "&qso_qsl=no" + "&qso_query=1");


            //  Debug.WriteLine("==== " + url2);

            // Only QSO that occured ON or after the specified date.
            Uri url3 = new Uri("https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + console.SpotForm.callBox.Text + "&password=" + console.SpotForm.LoTWPASS +
               "&qso_query=1" + "&qso_qsl=no" + "&qso_qsorxsince=" + mod1 + "&qso_qsldetail=yes");

            // only QSL that occured ON or after the specified date.
            Uri url4 = new Uri("https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + console.SpotForm.callBox.Text + "&password=" + console.SpotForm.LoTWPASS +
                                             "&qso_query=1" + "&qso_qsl=yes" + "&qso_qslsince=" + mod1);

         
            if (file_LoTW == 2)
            {
                url = url2;        // download for Master LOG file
              
            }
            else if (f_LoTW == 4) url = url4;     // download for QSL partial update only
            else if (file_LoTW == 3) url = url3;  // download for QSO partial update only
            else url = url4;                      // download for QSL partial update only (do this directly after a full log download

            //  console.SpotForm.textBox1.Text +=  url + "\r\n";


            WebClient wc = new WebClient(); // vehicle to make a connection to ARRL LoTW LOG server
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3  | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
          

            try
            {
                console.SpotForm.button4.BackColor = Color.Yellow; // let the user know your downloading
               
                Debug.WriteLine("LoTW START download: " + file_LoTW);
             
                if (file_LoTW == 3) // if you dont have a Master LOG file == 2, then get your ENTIRE LoTW QSO/QSL file, otherwise get only new data
                {
                    file_name2 = file_name3; // QSO
                }
                else if (file_LoTW == 4) // if you dont have a Master LOG file, then get your ENTIRE LoTW QSO/QSL file, otherwise get only new data
                {
                    file_name2 = file_name4; // QSL (directly after a full log download)
                }

                Debug.WriteLine("LoTW FILE TO DOWNLOAD " + file_name2);


                wc.DownloadFile(url, file_name2); // GET LOG file from ARRL LoTW site and save file directly

                
                if (file_LoTW == 2 && OLOG == false)
                {
                    File.Copy(file_name2, file_name5); // make a backup to the OLOG file (mostly for people with very large LOG files)
                }
                
                if (OLOG == true)
                {
                    wc.DownloadFile(url4, file_name3); // GET QSO LOG file from ARRL LoTW site and save file directly

                    wc.DownloadFile(url3, file_name4); // GET QSL LOG file from ARRL LoTW site and save file directly

                    OLOG = false;

                }

                wc.Dispose();

                Debug.WriteLine("LoTW DONE SAVE FILE: " + file_name2);

            } // try
            catch (Exception g)
            {
                wc.Dispose();

                console.SpotForm.button4.BackColor = Color.Red;
                console.SpotForm.textBox1.Text += "Could not download LoTW LOG file. File will not be saved, Download Exception ERROR: " + g + "\r\n";

                content1 = "Error " + g.ToString();
              
                console.SpotForm.LoTWResult = 3;
                console.SpotForm.LoTWDone = true;
                return content1;

            }

            if (file_LoTW == 2) console.SpotForm.textBox1.Text += "\r\nDone. Saving Full LoTW Log file to location:\r\n" + file_name2 + "\r\n\r\n";
            else if (file_LoTW == 3) console.SpotForm.textBox1.Text += "\r\nDone. Saving updated QSO to LoTW Log File \r\n";
            else console.SpotForm.textBox1.Text += "\r\nDone. Saving updated QSL LoTW Log to location:\r\n" + file_name2 + "\r\n\r\n";


                //-----------------------------------------------------------------------------------------------
             

                if (file_LoTW == 4)                                                           // if you just did an update, then merge the QSO and QSL into the Master file
                {

                    console.SpotForm.button4.BackColor = Color.Yellow;
                 

                    int lotw_records = 0; // master LoTW
                    int lotw_records1 = 0; // QSO
                    int lotw_records2 = 0; // QSL

                    //...............................................................................................
                    // extract Master LoTW Log file and place each QSO/QSL into a LoTW_master. List<string> array
                    try
                    {
                      
                        Debug.WriteLine("LoTW reader GET LOG master file " + file_name1);

                    if (File.Exists(file_name1))
                    {
                        Debug.WriteLine("LoTW LOG file exists");

                    }

                        string lotw_log = File.ReadAllText(file_name1);   // OPEN FILE AND READ INTO A STRING HOLDER (all a single string that will need to be parsed out)
                    

                        int x1 = lotw_log.IndexOf("<APP_LoTW_NUMREC:"); // find header at top of string
                        Debug.WriteLine("LoTW reader GOT x1 " + x1);
                       
                        int x = lotw_log.IndexOf("<eoh>"); // find header at top of string
                        Debug.WriteLine("LoTW reader GOT x " + x);
                        var ss = lotw_log.Substring(x1 + 19, x - (x1 + 19)); // start and length   var ss = lotw_log.Substring(x1 + 19, x - (x1 + 19)); /
                     
                        Debug.WriteLine("LoTW reader GOT x1 " + x1 + " , " + x + " , " + ss);

                        try
                        {
                            lotw_records = System.Convert.ToInt32(ss); // get total number of LoTW QSO records to parse

                            Debug.WriteLine("LoTW master database records count: " + lotw_records);

                            console.SpotForm.textBox1.Text += "Master LoTW log file total contacts: " + lotw_records + "\r\n";
                          
                            x = x + 2; // position in giant lotw_log string

                            for (x1 = 0; x1 < lotw_records; x1++)  // get each QSO/QSL record (all the ASCII text for each contact)
                            {
                                int y = lotw_log.IndexOf("<eor>", x + 5);
                                LoTW_master.Add(lotw_log.Substring(x + 5, y - (x))); // this is 1 QSO record
                                x = y; // move pointer to start of next

                              
                                int z = LoTW_master[x1].IndexOf("<CALL:");
                                if (z < 1)
                                {
                                    LoTW_MasterCall.Add("-");
                                    Debug.WriteLine("NO CALL FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z1 = System.Convert.ToInt32(LoTW_master[x1].Substring(z + 6, 1));
                                    LoTW_MasterCall.Add(LoTW_master[x1].Substring(z + 8, z1)); // call callsign
                                }

                                                           
                                int z2 = LoTW_master[x1].IndexOf("<APP_LoTW_MODEGROUP:");
                                if (z2 < 1)
                                {
                                    LoTW_MasterMode.Add("-");
                                    Debug.WriteLine("NO MODE FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z3 = System.Convert.ToInt32(LoTW_master[x1].Substring(z2 + 20, 1));
                                    LoTW_MasterMode.Add(LoTW_master[x1].Substring(z2 + 22, z3)); // call mode group (data, phone)
                                }

                                int z4 = LoTW_master[x1].IndexOf("<BAND:");
                                if (z4 < 1)
                                {
                                    LoTW_MasterBand.Add("-");
                                    Debug.WriteLine("NO BAND FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z5 = System.Convert.ToInt32(LoTW_master[x1].Substring(z4 + 6, 1));
                                    LoTW_MasterBand.Add(LoTW_master[x1].Substring(z4 + 8, z5)); // call band

                                }
                               
                                int z8 = LoTW_master[x1].IndexOf("2xQSL:");
                                if (z8 < 1)
                                {
                                   LoTW_MasterQSL.Add("N");
                                }
                                else
                                {
                                    int z9 = System.Convert.ToInt32(LoTW_master[x1].Substring(z8 + 6, 1));
                                    LoTW_MasterQSL.Add(LoTW_master[x1].Substring(z8 + 8, z9)); // call 2 way QSL confirmed Y / N
                                }


                            } // for x1 loop for Master LOG file

                        lotw_log = null;

                        }
                        catch
                        {
                            console.SpotForm.button4.BackColor = Color.Red;
                            Debug.WriteLine("LoTW Failed reading master file, on record: " + x1 );
                        }
                    }
                    catch (Exception e)
                    {
                        console.SpotForm.button4.BackColor = Color.Red;
                        Debug.WriteLine("LoTW Failed opening lotw1 log from file to read " + file_name1 + " ,    e:" + e);
                        //  goto LoTW1; // cant open file so end it now.

                    } // Master LOG Catch

              


                    //................................................................................................
                    //................................................................................................
                    // extract QSO Update LoTW LOG file
                    try
                    {
                        Debug.WriteLine("LoTW reader GET QSO file3 " + file_name3); // LoTW_LOG_QSO_Update.adi

                        string lotw_log_QSO = File.ReadAllText(file_name3);

                      
                        int x1 = lotw_log_QSO.IndexOf("<APP_LoTW_NUMREC:"); // find header at top of string
                        int x = lotw_log_QSO.IndexOf("<eoh>"); // find header at top of string int x = lotw_log_QSO.IndexOf("<eoh>"); 
                        var ss = lotw_log_QSO.Substring(x1 + 19, x - (x1 + 19)); // start and length

                        try
                        {
                            lotw_records1 = System.Convert.ToInt32(ss); // get total number of LoTW QSO records to parse
                            Debug.WriteLine("LoTW QSO database records count: " + lotw_records1);
                            console.SpotForm.textBox1.Text += "QSO Update LoTW log file total contacts: " + lotw_records1 + "\r\n";

                            x = x + 2;
                            for (x1 = 0; x1 < lotw_records1; x1++)  // get each QSO record
                            {
                                int y = lotw_log_QSO.IndexOf("<eor>", x + 5);

                                LoTW_QSO.Add(lotw_log_QSO.Substring(x + 5, y - (x))); // this is 1 QSO record
                                                          
                                x = y; // move pointer to start of next



                                int z = LoTW_QSO[x1].IndexOf("<CALL:");
                                if (z == -1)
                                {
                                    Debug.WriteLine("NO QSO CALL FOUND....BAD " + x1);
                                    LoTW_QSOCall.Add("--");
                                   
                                }
                                else
                                {
                                    int z1 = System.Convert.ToInt32(LoTW_QSO[x1].Substring(z + 6, 1));
                                    LoTW_QSOCall.Add(LoTW_QSO[x1].Substring(z + 8, z1)); // call callsign
                                }


                                int z2 = LoTW_QSO[x1].IndexOf("<APP_LoTW_MODEGROUP:");
                                if (z2 == -1)
                                {
                                    LoTW_QSOMode.Add("--");
                                    Debug.WriteLine("NO QSO MODE FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z3 = System.Convert.ToInt32(LoTW_QSO[x1].Substring(z2 + 20, 1));
                                    LoTW_QSOMode.Add(LoTW_QSO[x1].Substring(z2 + 22, z3)); // call mode group (data, phone)
                                }

                                int z4 = LoTW_QSO[x1].IndexOf("<BAND:");
                                if (z4 == -1)
                                {
                                    LoTW_QSOBand.Add("--");
                                    Debug.WriteLine("NO QSO BAND FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z5 = System.Convert.ToInt32(LoTW_QSO[x1].Substring(z4 + 6, 1));
                                    LoTW_QSOBand.Add(LoTW_QSO[x1].Substring(z4 + 8, z5)); // call band

                                }


                            } // for x1 loop for QSO LOG file

                        lotw_log_QSO = null;
                        }
                        catch
                        {
                            lotw_records1 = 0;
                            console.SpotForm.button4.BackColor = Color.Red;
                            Debug.WriteLine("LoTW Failed reading QSO file " + x1);
                        }

                    }
                    catch (Exception)
                    {
                        lotw_records1 = 0;
                         console.SpotForm.button4.BackColor = Color.Red;
                        Debug.WriteLine("LoTW Failed opening lotw3 log from file to read");
                        //  goto LoTW1; // cant open file so end it now.

                    } // QSO Log Catch



                    //.................................................................................................
                    //................................................................................................
                    // extract QSL Update LotW LOG File
                    try
                    {
                        Debug.WriteLine("LoTW reader GET QSL file4 " + file_name4);

                        string lotw_log_QSL = File.ReadAllText(file_name4);


                        int x1 = lotw_log_QSL.IndexOf("<APP_LoTW_NUMREC:"); // find header at top of string
                        int x = lotw_log_QSL.IndexOf("<eoh>"); // find header at top of string
                        var ss = lotw_log_QSL.Substring(x1 + 19, x - (x1 + 19)); // start and length

                        x = x + 2;

                        try
                        {
                            lotw_records2 = System.Convert.ToInt32(ss); // get total number of LoTW QSL records to parse

                            Debug.WriteLine("LoTW QSL UPDATE records count: " + lotw_records2);
                            console.SpotForm.textBox1.Text += "QSL Update LoTW log file total contacts: " + lotw_records2 + "\r\n";

                            for (x1 = 0; x1 < lotw_records2; x1++)  // get each QSL record
                            {

                                int y = lotw_log_QSL.IndexOf("<eor>", x + 5);
                                LoTW_QSL.Add(lotw_log_QSL.Substring(x + 5, y - (x))); // this is 1 QSL record   LoTW_QSL.Add(lotw_log_QSL.Substring(x + 5, y - (x + 5))); 

                                x = y; // move pointer to start of next


                                int z = LoTW_QSL[x1].IndexOf("<CALL:");
                                if (z < 1)
                                {
                                    LoTW_QSLCall.Add("---");
                                    Debug.WriteLine("NO QSL CALL FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z1 = System.Convert.ToInt32(LoTW_QSL[x1].Substring(z + 6, 1));
                                    LoTW_QSLCall.Add(LoTW_QSL[x1].Substring(z + 8, z1)); // call callsign
                                }


                                int z2 = LoTW_QSL[x1].IndexOf("<APP_LoTW_MODEGROUP:");
                                if (z2 < 1)
                                {
                                    LoTW_QSLMode.Add("---");
                                    Debug.WriteLine("NO QSL MODE FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z3 = System.Convert.ToInt32(LoTW_QSL[x1].Substring(z2 + 20, 1));
                                    LoTW_QSLMode.Add(LoTW_QSL[x1].Substring(z2 + 22, z3)); // call mode group (data, phone)
                                }

                                int z4 = LoTW_QSL[x1].IndexOf("<BAND:");
                                if (z4 < 1)
                                {
                                    LoTW_QSLBand.Add("---");
                                    Debug.WriteLine("NO QSL BAND FOUND....BAD " + x1);
                                }
                                else
                                {
                                    int z5 = System.Convert.ToInt32(LoTW_QSL[x1].Substring(z4 + 6, 1));
                                    LoTW_QSLBand.Add(LoTW_QSL[x1].Substring(z4 + 8, z5)); // call band

                                }

                            } // for x1 loop QSL Log 

                        lotw_log_QSL = null;

                        }
                        catch
                        {
                            lotw_records2 = 0;
                            console.SpotForm.button4.BackColor = Color.Red;
                            Debug.WriteLine("LoTW Failed reading QSO file");
                        }


                    }
                    catch (Exception)
                    {
                        lotw_records2 = 0;
                        console.SpotForm.button4.BackColor = Color.Red;
                        Debug.WriteLine("LoTW Failed opening lotw4 log from file to read");
                        //  goto LoTW1; // cant open file so end it now.

                    } // QSL LOG catch

                    //.......................................................................................
                    //.......................................................................................
                    //.......................................................................................

                    // have 3 files at this point.
                    // Need to parse each file then merge identical call signs (modes and bands) from QSL and QSO files into Master file and resave Master LOG file

                 

                    bool bypass = false; // if a QSO or QSL fails to match only because the QSL in the master already had a Y, then  dont update master (set to true)

                    Debug.WriteLine("LoTW Parse QSO records1 " + lotw_records + " , "  + lotw_records1);
                    Debug.WriteLine("items " + LoTW_master.Count);

                    //  console.SpotForm.textBox1.Text += "Parse Records 1 " + lotw_records + "\r\n";

                 
                    for (int q = 0; q < lotw_records1; q++) // QSO parse
                    {
                        // get callsign from QSO Update and check to see if an update to the Master

                        int q1 = 0;

                        //  console.SpotForm.textBox1.Text += "Parse Master Records 1a\r\n";
                      //  Debug.Write("QSO=" + q + " " + lotw_records1 ); // lotw_records1


                        for ( q1 = 0; q1 < lotw_records; q1++) // master check
                        {
                            if (LoTW_QSOCall[q] == LoTW_MasterCall[q1])    //(foundCallQSO == foundCall) // if QSL matches something in your Master log, then replace it with this new QSL
                            {
                                if (LoTW_QSOBand[q] == LoTW_MasterBand[q1])
                                {
                                    if (LoTW_QSOMode[q] == LoTW_MasterMode[q1])
                                    {
                                      
                                        bypass = true;  // since this call,mode, and band are already in your master, dont add a new entry
                                     
                                        if (LoTW_MasterQSL[q1] != "Y") // found master had prior entry, but was not a confirmed QSL, so update it
                                        {
                                            LoTW_master[q1] = "\r\n" + LoTW_QSO[q];
                                            LoTW_MasterCall[q1] = LoTW_QSOCall[q];
                                            LoTW_MasterBand[q1] = LoTW_QSOBand[q];
                                            LoTW_MasterMode[q1] = LoTW_QSOMode[q];
                                            LoTW_MasterQSL[q1] ="N";

                                            continue;
                                        }
                                        else // found master had prior entry, but it was already a confirmed QSL, no need to update
                                        {
                                            continue;
                                        }

                                    } // mode match
                                   
                                } // band match
                               
                            } // call sign match
                          
                        } // for loop Master LoTW log scan



                        if ((bypass == false) && (q1 == lotw_records)) // FOR loop ended, this indicates no match was found in the master, so add it .
                        {

                            LoTW_master.Add("\r\n" + LoTW_QSO[q]);
                            LoTW_MasterCall.Add(LoTW_QSOCall[q]);
                            LoTW_MasterBand.Add(LoTW_QSOBand[q]);
                            LoTW_MasterMode.Add(LoTW_QSOMode[q]);
                            LoTW_MasterQSL.Add("N");

                            lotw_records++;
                          
                        }

                        bypass = false; // reset flag

                    } // for loop QSO to Master LOG updater




                    //......................................................................................................
                    //......................................................................................................
                    //......................................................................................................
                    //......................................................................................................

                    Debug.WriteLine("LoTW Parse QSL records2: " + lotw_records + " , " + lotw_records2);

                    //   console.SpotForm.textBox1.Text += "Parse QSL Records 2" + lotw_records2 + "\r\n";


                    for (int q = 0; q < lotw_records2; q++) // QSL parse
                    {
                        // get callsign from QSL Update and check to see if an update to the Master

                         //   Debug.Write("QSL=" + q); // lotw_records2

                         //-------------------------------------------------------------------------------


                        int q1 = 0;
                        for (q1 = 0; q1 < lotw_records; q1++) // master check
                        {

                            if (LoTW_QSLCall[q] == LoTW_MasterCall[q1]) // if QSL matches something in your Master log, then replace it with this new QSL
                            {
                                if (LoTW_QSLBand[q] == LoTW_MasterBand[q1])
                                {
                                    if (LoTW_QSLMode[q] == LoTW_MasterMode[q1])
                                    {
                                        bypass = true;  // since this call,mode, and band are already in your master, dont add a new entry

                                        if (LoTW_MasterQSL[q1] != "Y") // if record was here before but not a confirmed QSL, then update the record
                                        {

                                            LoTW_master[q1] = "\r\n" + LoTW_QSL[q];
                                            LoTW_MasterCall[q1] = LoTW_QSLCall[q];
                                            LoTW_MasterBand[q1] = LoTW_QSLBand[q];
                                            LoTW_MasterMode[q1] = LoTW_QSLMode[q];
                                            LoTW_MasterQSL[q1] = "N";
                                             continue;
                                        }
                                        else // if its already a conformed QSL, then leave it alone
                                        {
                                            continue;
                                        }

                                    } // mode match

                                } // band match

                            } // call sign match

                        } // for loop Master LoTW log scan

                        if ((bypass == false) && (q1 == lotw_records))// FOR loop reached end, this indicates no match was found in the master, so add it.
                        {
                         
                            LoTW_master.Add("\r\n" + LoTW_QSL[q]);
                            LoTW_MasterCall.Add(LoTW_QSLCall[q]);
                            LoTW_MasterBand.Add(LoTW_QSLBand[q]);
                            LoTW_MasterMode.Add(LoTW_QSLMode[q]);
                            LoTW_MasterQSL.Add("Y");
                            lotw_records++;

                        }

                        bypass = false;

                    } // for loop QSL UPdate log




                //.......................................................................................
                //.......................................................................................
                //.......................................................................................
                // create new master file.


                Debug.WriteLine("LoTW CLEAR UN-NEEDED DATA");

                LoTW_QSL.Clear();
                LoTW_QSLBand.Clear();
                LoTW_QSLMode.Clear();
                LoTW_QSLCall.Clear();

                LoTW_QSO.Clear();
                LoTW_QSOBand.Clear();
                LoTW_QSOMode.Clear();
                LoTW_QSOCall.Clear();


                LoTW_MasterBand.Clear();
                LoTW_MasterMode.Clear();
                LoTW_MasterCall.Clear();
                LoTW_MasterQSL.Clear();


                Debug.WriteLine("LoTW update MASTER LOG file now " + lotw_records + " , " + LoTW_master.Count + " , In FILE1 " + file_name1);


                using (FileStream stream2 = new FileStream(file_name1, FileMode.Create))              // create  file
                {
                    using (BinaryWriter writer2 = new BinaryWriter(stream2))
                    {
                        writer2.Write("<APP_LoTW_NUMREC:4>" + lotw_records.ToString() + "\r\n\r\n<eoh>\r\n");


                        for (int x7 = 0; x7 < LoTW_master.Count; x7++)
                        {
                            writer2.Write(LoTW_master[x7]);

                        }

                        writer2.Dispose();
                        stream2.Dispose();
                        writer2.Close();    // close  file
                        stream2.Close();   // close stream
                    }
                }
             
                LoTW_master.Clear();

                Debug.WriteLine("DONE DONE DONE");

                } //  if (file_LoTW == 4) // if you just did an update, then merge the QSO and QSL into the Master file


                console.SpotForm.textBox1.Text += "Finished Saving\r\n";

                console.SpotForm.LoTWResult = 2; // good
                console.SpotForm.LoTWDone = true;

                return content1;



        } //   public async Task<string> LotwAAsync()




        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        public void HttpServer1()
        {
            Debug.WriteLine("httpserver1 has been called");
            try
            {
                m_listener = new TcpListener(IPAddress.Any, console.HTTP_PORT);

            }
            catch (Exception e)
            {

                Debug.WriteLine("7exception" + e);
                return;

            }


            Console.m_terminated = false;

            Thread t = new Thread(new ThreadStart(TCPSERVER));
            t.Name = "TCP SERVER THREAD";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

        } // httpserver()


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //========================================================================================
        // ke9ns add  THREAD
        private void TCPSERVER()
        {
            try
            {
                m_listener.Start();


            }
            catch (Exception e)
            {
                Debug.WriteLine("Cannot start thread " + e);

                terminate();
            }

            Debug.WriteLine("LISTENER STARTED");


            while (!Console.m_terminated)
            {

                try
                {
                    Thread.Sleep(50);


                    TcpClient tempClient = getHandler(m_listener.AcceptTcpClient());

                    //   TcpClient client = m_listener.AcceptTcpClient();
                    //   string ip = ((IPEndPoint)m_listener.Server.LocalEndPoint).Address.ToString();
                    //    TcpClient tempClient = getHandler(client);

                    if (TcpType != 0)
                    {
                        if (TcpType == 1)
                        {

                            ImageRequest(tempClient);
                            //  AudioRequest(tempClient);
                        }
                        else if (TcpType == 2)
                        {
                            WebPageRequest(tempClient);
                        }
                        else
                        {
                            UnknownRequest(tempClient);
                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine("get TCP RECEIVE fault " + e);

                    try
                    {
                        m_listener.Stop(); // try and close the getcontext thread
                        m_listener.Start();
                    }
                    catch (Exception e1)
                    {
                        Debug.WriteLine("close THREAD " + e1);
                        break;
                    }


                }

                Thread.Sleep(50);

            } //while (!m_terminated)


            console.URLPRESENT = false;

        } // TCPSERVER() THREAD


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        public static void terminate()
        {
            Console.m_terminated = true;
            console.URLPRESENT = false;
            try
            {
                m_listener.Stop(); // try and close the getcontext thread

            }
            catch (Exception e)
            {
                Debug.WriteLine("close THREAD " + e);
            }
        }

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        public static int TcpType = 0;

        public static TcpClient getHandler(TcpClient tcpClient)
        {
            switch (getType(tcpClient))
            {
                case RequestType.GET_IMAGE:   //  ImageRequest(tempClient);
                    TcpType = 1;
                    return tcpClient;
                case RequestType.GET_HTML_INDEX_PAGE: //  WebPageRequest(tempClient);
                    TcpType = 2;
                    return tcpClient;
                case RequestType.UNKNOWN: //  UnknownRequest(tempClient);
                    TcpType = 3;
                    return tcpClient;
            }

            TcpType = 0;
            return tcpClient;

        } // TcpClient getHandler


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        private static RequestType getType(TcpClient tcpClient)
        {
            string Request = "";
            byte[] Buffer = new byte[1024];
            int Count;

            while ((Count = tcpClient.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
            {
                Request += Encoding.ASCII.GetString(Buffer, 0, Count);
                if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                {
                    break;
                }
            }

            Match ReqMatch = Regex.Match(Request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (ReqMatch == Match.Empty)
            {
                SendError(tcpClient, 400);
                return RequestType.ERROR;
            }

            string RequestUri = ReqMatch.Groups[1].Value;

            Debug.WriteLine("URI " + RequestUri);

            RequestUri = Uri.UnescapeDataString(RequestUri);

            if (RequestUri.IndexOf("..") >= 0)
            {
                SendError(tcpClient, 400);
                return RequestType.ERROR;
            }

            else if (RequestUri.CompareTo(IMAGE_REQUEST) == 0)  // /image
            {
                return RequestType.GET_IMAGE;
            }

            else if (RequestUri.CompareTo("/") == 0)
            {
                // return RequestType.GET_IMAGE;

                return RequestType.GET_HTML_INDEX_PAGE;
            }

            return RequestType.UNKNOWN;

        } // private static RequestType getType(TcpClient tcpClient)


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================


        public void ImageRequest(TcpClient m_tcpClient)
        {

            if (m_tcpClient == null) return;

            Debug.WriteLine("IMAGEREQUEST1");

            if (console.URLPRESENT == false) console.URLPRESENT = true; // ke9ns let the setup HTTP server know that someone is requesting an image


            byte[] imageArray = console.getImage(); // ke9ns this gets either the Spectral Display or the entire Console widow and puts it into a jpeg byte array



            if (imageArray == null) // if we dont have an image, let the requestor know we dont have an image to send.
            {
                string CodeStr = "500 " + ((System.Net.HttpStatusCode)500).ToString();

                string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";

                string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

                byte[] Buffer = Encoding.ASCII.GetBytes(Str);

                m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);
                m_tcpClient.Close();
                return;
            }

            //  "<meta http-equiv= \"refresh\" content= \"500\" > \r\n" +

            string responseHeaders = "HTTP/1.1 200 The file is coming right up!\r\n" +
                                       "Server: MyOwnServer\r\n" +
                                       "Content-Length: " + imageArray.Length + "\r\n" +
                                       "Content-Type: image/jpeg\r\n" +
                                       "Content-Disposition: inline;filename=\"picDisplay.jpg;\"\r\n" +
                                       "\r\n";



            byte[] headerArray = Encoding.ASCII.GetBytes(responseHeaders); // convert responseheader into byte array


            NetworkStream stream1 = m_tcpClient.GetStream(); // create a stream to send/receive data over the TCP/IP connection

            stream1.Write(headerArray, 0, headerArray.Length); // send header
            stream1.Write(imageArray, 0, imageArray.Length);   // send image


            stream1.Close();

            m_tcpClient.Close();


        } // ImageRequest()


        public void AudioRequest(TcpClient m_tcpClient)
        {

            if (m_tcpClient == null) return;

            Debug.WriteLine("AudioREQUEST1");

            if (console.URLPRESENT == false) console.URLPRESENT = true; // ke9ns let the setup HTTP server know that someone is requesting an image

            byte[] audioArray = console.getAudio(); // ke9ns gets audio stream


            if (audioArray == null) // if we dont have an image, let the requestor know we dont have an image to send.
            {
                string CodeStr = "500 " + ((System.Net.HttpStatusCode)500).ToString();

                string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";

                string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

                byte[] Buffer = Encoding.ASCII.GetBytes(Str);

                m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);
                m_tcpClient.Close();
                return;
            }

            //  "<meta http-equiv= \"refresh\" content= \"500\" > \r\n" +

            string responseHeaders = "HTTP/1.1 200 The file is coming right up!\r\n" +
                                       "Server: MyOwnServer\r\n" +
                                       "Content-Length: " + audioArray.Length + "\r\n" +
                                       "Content-Type: audio/mpeg\r\n" +
                                       //  "Content-Disposition: inline;filename=\"picDisplay.jpg;\"\r\n" +
                                       "\r\n";



            byte[] headerArray = Encoding.ASCII.GetBytes(responseHeaders); // convert responseheader into byte array


            NetworkStream stream1 = m_tcpClient.GetStream(); // create a stream to send/receive data over the TCP/IP connection

            stream1.Write(headerArray, 0, headerArray.Length); // send header
            stream1.Write(audioArray, 0, audioArray.Length);   // send audio

            stream1.Close();

            m_tcpClient.Close();


        } // AudioRequest()

        //===============================================================================


        public void PlayAudio() //  public void PlayAudio(int id)
        {
            byte[] bytes = new byte[0];

            // using (The_FactoryDBContext db = new The_FactoryDBContext())
            //  {
            //    if (db.Words.FirstOrDefault(word => word.wordID == id).engAudio != null)
            //  {
            //        bytes = db.Words.FirstOrDefault(word => word.wordID == id).engAudio;

            //  }
            // }

            //  Context.Response.Clear();
            //  Context.Response.ClearHeaders();
            //  Context.Response.ContentType = "audio/wav"; //  "audio/mpeg";
            // Context.Response.AddHeader("Content-Length", bytes.Length.ToString());
            // Context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            // Context.Response.End();
        }

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        public void UnknownRequest(TcpClient m_tcpClient)
        {
            Debug.WriteLine("Unknown_REQUEST");


            if (m_tcpClient == null) return;

            string CodeStr = "404 " + ((HttpStatusCode)404).ToString();
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);

            m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);

            m_tcpClient.Close();

        } // UnknownRequest()

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        public void WebPageRequest(TcpClient m_tcpClient)
        {
            Debug.WriteLine("Web_REQUEST");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


            if (m_tcpClient == null) return;

            Debug.WriteLine("Web_REQUEST2");


            string timeRefresh_in_ms = getTimeRefresh();

            string CodeStr = "200 " + ((HttpStatusCode)200).ToString();

            string Html = "<!DOCTYPE html>\n" +
                          "<html>\n" +
                          "<head>\n" +
                          "<title></title>\n" +
                          "</head>\n" +
                          "<body>\n" +
                          "<div><img id = 'img' src = \"\"></div>\n" +
                          "<script type = \"text/javascript\" src = \"https://code.jquery.com/jquery-3.1.1.min.js\"></script>\n" +
                          "<script type = \"text/javascript\">\n" +
                          "var link = \"http://\"+window.location.host;\n" +
                          "console.log(link);\n" +
                          "setInterval(function(){\n" +
                          "var now = new Date();\n" +
                          "$('#img').prop(\"src\",link+\"/image\" + '?_=' + now.getTime());\n" +
                          "}, " + timeRefresh_in_ms + ");\n" +
                          "</script>\n" +
                          "</body>\n" +
                          "</html>\n";


            string Str = "HTTP/1.1 " + CodeStr + "\nContent-Type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

            Debug.WriteLine("STRING TO SEND: " + Str);


            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);
            m_tcpClient.Close();

        } // webrequest

        private string getTimeRefresh()
        {
            //  return "200"; // ************** Darrin,need add property "Refreh time in ms" and get data from his
            return console.HTTP_REFRESH.ToString();

        }



        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        Bitmap bitmap;
        byte[] picDisplayOutput;
        MemoryStream memstream;


        private byte[] getImage()
        {

            bitmap = new Bitmap(console.picDisplay.Width, console.picDisplay.Height); // ke9ns set bitmap size to size of picDisplay since it gets resized with your screen
            console.picDisplay.DrawToBitmap(bitmap, console.picDisplay.ClientRectangle); // ke9ns grab picDisplay and convert to bitmap

            using (memstream = new MemoryStream())
            {
                bitmap.Save(memstream, ImageFormat.Jpeg);
                picDisplayOutput = memstream.ToArray();
            }


            return picDisplayOutput;

        } // getImage()





        /*     // ke9ns if you want to save image as a file and then read file
                private byte[] getImage()
                {

                    bitmap = new Bitmap(console.picDisplay.Width, console.picDisplay.Height); // ke9ns set bitmap size to size of picDisplay since it gets resized with your screen
                    console.picDisplay.DrawToBitmap(bitmap, console.picDisplay.ClientRectangle); // ke9ns grab picDisplay and convert to bitmap
                    bitmap.Save(console.AppDataPath + "picDisplay.jpg", ImageFormat.Jpeg); // ke9ns save image into database folder

                    FileInfo picDisplayFile = new FileInfo(console.AppDataPath + "picDisplay.jpg");
                    FileStream picDisplayStream = new FileStream(console.AppDataPath + "picDisplay.jpg", FileMode.Open, FileAccess.Read); // open file  stream 
                    BinaryReader picDisplayReader = new BinaryReader(picDisplayStream); // open stream for binary reading

                    picDisplayOutput = picDisplayReader.ReadBytes((int)picDisplayFile.Length); // create array of bytes to transmit

                    picDisplayReader.Close();
                    picDisplayStream.Close();

                    return picDisplayOutput;


                } // getImage()

            */
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        private static void SendError(TcpClient Client, int Code)
        {
            string CodeStr = Code.ToString() + " " + ((HttpStatusCode)Code).ToString();
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            Client.Close();
        }




    } // class http

    sealed public class Server
    {


        private Socket sock;

        private int port = 8080;

        private IPAddress addr = IPAddress.Any;

        private int backlog;


        //------------------------------------------------
        public void Start() //  // This is the method that starts the server listening.
        {
            this.sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sock.Bind(new IPEndPoint(this.addr, this.port));

            this.sock.Listen(this.backlog); // places socket in listen state

            this.sock.BeginAccept(this.OnConnectRequest, sock); // accepts incoming connection attemp


        } // Start()


        //------------------------------------------------
        public void OnConnectRequest(IAsyncResult result)  // This is the method that is called when the socket receives a request for a new connection.
        {

            Socket sock = (Socket)result.AsyncState;  // get socket 

            Connection newConn = new Connection(sock.EndAccept(result)); // create new connection

            sock.BeginAccept(this.OnConnectRequest, sock); // tell listener socket to start listening again


        } //  public void OnConnectRequest(IAsyncResult result)





    } //  public class Server




    public class Connection
    {

        private Socket sock;
        byte[] dataRcvBuf;

        private Encoding encoding = Encoding.UTF8;

        //--------------------------------------
        public Connection(Socket s)
        {
            this.sock = s;
            this.BeginReceive(); // start listening for incoming data (this could be in a thread

        } //AsyncResult result)


        //--------------------------------------
        private void BeginReceive()  //  // Call this method to set this connection's socket up to receive data.
        {
            this.sock.BeginReceive(this.dataRcvBuf, 0, this.dataRcvBuf.Length, SocketFlags.None, new AsyncCallback(this.OnBytesReceived), this);


        } // private void BeginReceive()


        //--------------------------------------
        protected void OnBytesReceived(IAsyncResult result)   // This is the method that is called whenever the socket receives incoming bytes.
        {

            int nBytesRec = this.sock.EndReceive(result);   // End the data receiving that the socket has done and get the number of bytes read.

            if (nBytesRec <= 0)                            // If no bytes were received, the connection is closed (at least as far as we're concerned).
            {
                this.sock.Close();
                return;
            }

            string strReceived = this.encoding.GetString(this.dataRcvBuf, 0, nBytesRec);   // Convert the data we have to a string.

            Debug.WriteLine("!!!!!!GOT BYTES" + strReceived);


            // ...Now, do whatever works best with the string data.
            // You could, for example, look at each character in the string
            // one-at-a-time and check for characters like the "end of text"
            // character ('\u0003') from a client indicating that they've finished
            // sending the current message.  It's totally up to you how you want
            // the protocol to work.

            // Whenever you decide the connection should be closed, call 
            // sock.Close() and don't call sock.BeginReceive() again.  But as long 
            // as you want to keep processing incoming data...


            this.sock.BeginReceive(this.dataRcvBuf, 0, this.dataRcvBuf.Length, SocketFlags.None, new AsyncCallback(this.OnBytesReceived), this);  // Set up again to get the next chunk of data.

        } //  protected void OnBytesReceived(IAsyncResult result)


    } //  public class Connection

} // namespace powersdr